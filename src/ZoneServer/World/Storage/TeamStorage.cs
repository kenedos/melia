using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Tos.Const;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.Network;
using Melia.Zone.Database;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.World.Storage
{
	/// <summary>
	/// Team storage of an account.
	/// </summary>
	public class TeamStorage : Storage
	{
		private int _silver;
		private int _silverMax;
		private Queue<StorageSilverTransaction> _silverTransactions;
		private int _maxSilverTransactions = 5; // Client limit

		private int _defaultMaxStorageSize = 5;
		private int _maxExtendSilverPrice = 2000000;

		// This is necessary because the client identifies the
		// silver in storage by its objectId
		private Item _silverDummyItem;

		/// <summary>
		/// Account that owns this team storage.
		/// </summary>
		public Account Owner { get; private set; }

		/// <summary>
		/// Whether the owner is currently browsing this storage.
		/// </summary>
		public bool IsBrowsing { get; private set; }

		/// <summary>
		/// Adds an amount of silver to storage.
		/// Updates client.
		/// </summary>
		/// <param name="character">Character that is performing interaction</param>
		/// <param name="amount">Amount of silver to store</param>
		/// <param name="invType">Storage inventory type</param>
		/// <returns></returns>
		private StorageResult StoreSilver(Character character, int amount, InventoryType invType)
		{
			if (amount <= 0)
				return StorageResult.InvalidOperation;

			var inventory = character.Inventory;

			// Transaction limit
			amount = Math.Min(inventory.CountItem(ItemId.Silver), amount);
			amount = Math.Min(_silverMax - _silver, amount);

			// Storing
			inventory.Remove(ItemId.Silver, amount, InventoryItemRemoveMsg.Given);
			_silver += amount;
			// This packet updates how much silver client knows there is in storage,
			// even if it is not visible in UI
			Send.ZC_ITEM_ADD(character, this.GetSilver(), 0, amount, InventoryAddType.New, invType);

			// Updates transaction list
			this.AddSilverTransaction(StorageInteraction.Store, amount);
			Send.ZC_NORMAL.AccountProperties(character);
			Send.ZC_NORMAL.StorageSilverTransaction(character, this.GetSilverTransactions(), false);

			return StorageResult.Success;
		}

		/// <summary>
		/// Removes an amount of silver to storage.
		/// Updates client.
		/// </summary>
		/// <param name="character">Character that is performing interaction</param>
		/// <param name="amount">Amount of silver to retrieve</param>
		/// <param name="invType">Storage inventory type</param>
		/// <returns></returns>
		private StorageResult RetrieveSilver(Character character, int amount, InventoryType invType)
		{
			if (amount <= 0)
				return StorageResult.InvalidOperation;

			if (_silver <= 0)
				return StorageResult.InvalidOperation;

			var inventory = character.Inventory;

			// Transaction limit
			amount = Math.Min(_silver, amount);
			amount = Math.Min(_silverMax, amount);

			// Retrieving
			inventory.Add(ItemId.Silver, amount, InventoryAddType.New);
			_silver -= amount;
			// This packet updates how much silver client knows there is in storage,
			// even if it is not visible in UI
			Send.ZC_ITEM_REMOVE(character, _silverDummyItem.ObjectId, amount, InventoryItemRemoveMsg.Given, invType);

			// Updates transaction list
			this.AddSilverTransaction(StorageInteraction.Retrieve, amount);
			Send.ZC_NORMAL.AccountProperties(character);
			Send.ZC_NORMAL.StorageSilverTransaction(character, this.GetSilverTransactions(), false);

			return StorageResult.Success;
		}

		/// <summary>
		/// Checks if character has the necessary silver to increase
		/// given amount of storage size. Returns true if character has
		/// enough silver and returns the price via out.
		/// </summary>
		/// <returns></returns>
		private bool CheckExtendStorageCost(Character character, int size, out int price)
		{
			var dynamicPrice = (int)Math.Pow(2,  this.GetStorageSize() - _defaultMaxStorageSize + size) * 100000;
			price = Math.Min(dynamicPrice, _maxExtendSilverPrice);

			if (character.Inventory.CountItem(ItemId.Silver) < price)
				return false;
			else
				return true;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="owner"></param>
		public TeamStorage(Account owner) : base()
		{
			this.Owner = owner;
			this.IsBrowsing = false;
			this.AddSize(_defaultMaxStorageSize);
			_silverDummyItem = new Item(ItemId.Silver);
			_silverMax = _silverDummyItem.Data.MaxStack;
			_silverTransactions = new Queue<StorageSilverTransaction>();
		}

		/// <summary>
		/// Adds a silver transaction to this storage.
		/// If number of transactions exceed the max, deletes older transactions.
		/// Does not update client.
		/// </summary>
		/// <param name="interaction">Retrieve or Store interaction</param>
		/// <param name="silverTransacted">Amount of silver</param>
		/// <param name="silverTotal">If not provided will automatically update total</param>
		/// <param name="fileTime">If not provided will be set to current time</param>
		/// <returns></returns>
		public StorageResult AddSilverTransaction(StorageInteraction interaction, int silverTransacted, int silverTotal = -1, long fileTime = -1)
		{
			if ((silverTotal < -1) || (fileTime < -1))
				return StorageResult.InvalidOperation;

			if ((interaction != StorageInteraction.Store) && (interaction != StorageInteraction.Retrieve))
				return StorageResult.InvalidOperation;

			// Adds transaction
			var transaction = new StorageSilverTransaction();
			transaction.Interaction = interaction;
			transaction.SilverTransacted = silverTransacted;
			transaction.SilverTotal = silverTotal != -1 ? silverTotal : _silver;
			_silverTransactions.Enqueue(transaction);

			if (_silverTransactions.Count > _maxSilverTransactions)
			{
				_silverTransactions.Dequeue();
			}

			return StorageResult.Success;
		}

		/// <summary>
		/// Returns the silver transactions in this storage.
		/// Returns null if no transactions were made.
		/// </summary>
		/// <returns></returns>
		public StorageSilverTransaction[] GetSilverTransactions()
		{
			return _silverTransactions.ToArray();
		}

		/// <summary>
		/// Returns the silver item or null
		/// if no silver exists
		/// </summary>
		/// <returns></returns>
		public Item GetSilver()
		{
			if (_silver <= 0)
				return null;

			_silverDummyItem.Amount = _silver;
			return _silverDummyItem;
		}

		/// <summary>
		/// Sets the silver in storage
		/// </summary>
		public void SetSilver(int amount)
		{
			_silver = Math.Min(_silverMax, amount);
		}

		/// <summary>
		/// Opens team storage.
		/// Updates client for owner.
		/// </summary>
		/// <returns></returns>
		public override StorageResult Open()
		{
			this.IsBrowsing = true;
			Send.ZC_CUSTOM_DIALOG(this.Owner.Connection.SelectedCharacter, "accountwarehouse", "");
			return StorageResult.Success;
		}

		/// <summary>
		/// Closes storage.
		/// Updates client for owner.
		/// </summary>
		/// <returns></returns>
		public override StorageResult Close()
		{
			this.IsBrowsing = false;
			Send.ZC_DIALOG_CLOSE(this.Owner.Connection.SelectedCharacter.Connection);
			return StorageResult.Success;
		}

		/// <summary>
		/// Adds an item to storage.
		/// Updates client for owner.
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public override StorageResult StoreItem(long objectId, int amount = 1)
		{
			return this.StoreItem(this.Owner.Connection.SelectedCharacter, objectId, amount, InventoryType.AccountWarehouse);
		}

		/// <summary>
		/// Retrieves an item from storage.
		/// Updates client for owner.
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public override StorageResult RetrieveItem(long objectId, int amount = 1)
		{
			return this.RetrieveItem(this.Owner.Connection.SelectedCharacter, objectId, amount, InventoryType.AccountWarehouse);
		}

		/// <summary>
		/// Checks if owner can extend storage by given size
		/// and removes silver from owner.
		/// Updates client for owner.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public StorageResult TryExtendStorage(int size)
		{
			var character = this.Owner.Connection.SelectedCharacter;
			if (CheckExtendStorageCost(character, size, out var price))
			{
				character.Inventory.Remove(ItemId.Silver, price, InventoryItemRemoveMsg.Given);
				this.AddSize(size);
				this.Owner.Properties.Modify(PropertyName.AccountWareHouseExtend, size);
				Send.ZC_NORMAL.AccountProperties(character);
				Send.ZC_ADDON_MSG(character, "ACCOUNT_WAREHOUSE_ITEM_LIST", 0, null);
				Send.ZC_ADDON_MSG(character, "ACCOUNT_UPDATE", 0, null);
				return StorageResult.Success;
			}

			return StorageResult.InvalidOperation;
		}

		/// <summary>
		/// Retrieves multiple items from storage.
		/// Updates client for owner.
		/// </summary>
		/// <param name="objectIds"></param>
		/// <param name="amounts"></param>
		/// <returns></returns>
		public StorageResult RetrieveItems(List<long> objectIds, List<int> amounts)
		{
			return this.RetrieveItems(this.Owner.Connection.SelectedCharacter, objectIds, amounts, InventoryType.AccountWarehouse);
		}

		/// <summary>
		/// Stores silver to storage.
		/// Updates client for owner.
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		public StorageResult StoreSilver(int amount)
		{
			return this.StoreSilver(this.Owner.Connection.SelectedCharacter, amount, InventoryType.AccountWarehouse);
		}

		/// <summary>
		/// Retrieves silver from storage.
		/// Updates client for owner.
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		public StorageResult RetrieveSilver(int amount)
		{
			return this.RetrieveSilver(this.Owner.Connection.SelectedCharacter, amount, InventoryType.AccountWarehouse);
		}
	}
}
