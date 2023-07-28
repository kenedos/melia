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
		/// <summary>
		/// Account that owns this team storage.
		/// </summary>
		public Account Owner { get; private set; }

		/// <summary>
		/// Whether the owner is currently browsing this storage.
		/// </summary>
		public bool IsBrowsing { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="owner"></param>
		public TeamStorage(Account owner) : base()
		{
			this.Owner = owner;
			this.IsBrowsing = false;
			this.AddStorageSize(5);
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
