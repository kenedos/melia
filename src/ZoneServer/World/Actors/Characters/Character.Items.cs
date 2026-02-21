// ===================================================================
// CharacterItems.cs - Item and inventory management
// ===================================================================
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		/// <summary>
		/// Returns ids of equipped items.
		/// </summary>
		public int[] GetEquipIds()
		{
			return this.Inventory.GetEquipIds();
		}

		/// <summary>
		/// Returns true if the character is wearing a full armor set of the given armor material type.
		/// </summary>
		public bool IsWearingFullArmorSetOfType(ArmorMaterialType armorType)
		{
			var equipIds = this.GetEquipIds();
			var armorSlots = new[] { EquipSlot.Shoes, EquipSlot.Top, EquipSlot.Pants, EquipSlot.Gloves };
			var armorCount = armorSlots.Count(slot =>
			{
				var itemId = equipIds[(int)slot];
				return ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out var item) && item.Material == armorType;
			});
			return armorCount == 4;
		}

		/// <summary>
		/// Returns true if the character is wearing any armor piece of the given armor material type.
		/// </summary>
		public bool IsWearingArmorOfType(ArmorMaterialType armorType)
		{
			var equipIds = this.GetEquipIds();
			var armorSlots = new[] { EquipSlot.Shoes, EquipSlot.Top, EquipSlot.Pants, EquipSlot.Gloves };
			var armorCount = armorSlots.Count(slot =>
			{
				var itemId = equipIds[(int)slot];
				return ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out var item) && item.Material == armorType;
			});
			return armorCount > 0;
		}

		/// <summary>
		/// Turns item monster into an item and adds it to the character's inventory.
		/// </summary>
		public void PickUp(ItemMonster itemMonster)
		{
			itemMonster.PickedUp = true;
			itemMonster.Item.ClearProtections();

			Send.ZC_ITEM_GET(this, itemMonster, itemMonster.Item.Amount);

			this.Inventory.Add(itemMonster.Item, InventoryAddType.PickUp);

			if (itemMonster.Item.Data.Journal)
			{
				if (itemMonster.MonsterId != 0)
					this.AdventureBook.AddMonsterDrop(itemMonster.MonsterId, itemMonster.Item.Id, itemMonster.Item.Amount);
				if (this.AdventureBook.IsNewEntry(AdventureBookType.ItemObtained, itemMonster.Item.Id))
					this.AddonMessage("ADVENTURE_BOOK_NEW", itemMonster.Item.Data.Name);

				this.AdventureBook.AddItemObtained(itemMonster.Item.Id, itemMonster.Item.Amount);
			}

			this.Map.RemoveMonster(itemMonster);
		}

		/// <summary>
		/// Returns true if the player has silver and has at least the requested amount.
		/// </summary>
		public bool HasSilver(int amount)
		{
			var hasEnoughSilver = this.HasItem(ItemId.Silver, amount);
			if (!hasEnoughSilver)
				this.SystemMessage("NotEnoughMoney", false);
			return hasEnoughSilver;
		}

		/// <summary>
		/// Returns true if the player has the item and has at least the requested amount.
		/// </summary>
		public bool HasItem(string itemClassName, int amount = 1)
		{
			if (ZoneServer.Instance.Data.ItemDb.TryFind(itemClassName, out var item))
				return this.HasItem(item.Id, amount);
			return false;
		}

		/// <summary>
		/// Returns true if the player has the item and has at least the requested amount.
		/// </summary>
		public bool HasItem(int itemId, int requiredAmount = 1)
		{
			return this.Inventory.CountItem(itemId) >= requiredAmount;
		}

		/// <summary>
		/// Removes an item from the inventory and returns amount removed.
		/// </summary>
		public int RemoveItem(string itemClassName, int amount = 1)
		{
			if (ZoneServer.Instance.Data.ItemDb.TryFind(itemClassName, out var item))
				return this.RemoveItem(item.Id, amount);
			return -1;
		}

		/// <summary>
		/// Removes an item from the inventory and returns amount removed.
		/// </summary>
		public int RemoveItem(int itemId, int amount = 1)
		{
			return this.Inventory.Remove(itemId, amount, InventoryItemRemoveMsg.Given);
		}

		/// <summary>
		/// Adds an item to the inventory.
		/// </summary>
		public void AddItem(string itemClassName, int amount = 1, string source = "")
		{
			var itemData = ZoneServer.Instance.Data.ItemDb.FindByClass(itemClassName);
			if (itemData != null)
			{
				this.AddItem(itemData.Id, amount, source);
			}
			else
			{
				Log.Warning("Failed to add item {ItemName} x {Amount} for character {CharacterName} ({CharacterId}). Reason: {Reason}", itemClassName, amount, this.Name, this.DbId, source);
			}
		}

		/// <summary>
		/// Adds an item to the inventory.
		/// </summary>
		public void AddItem(int itemId, int amount = 1, string source = null)
		{
			this.Inventory.Add(itemId, amount, InventoryAddType.New, reason: source);
		}

		/// <summary>
		/// Safely consumes a specific quantity of an item from the character's inventory.
		/// </summary>
		public bool ConsumeItem(long worldId, int quantity = 1)
		{
			var itemToConsume = this.Inventory.GetItem(worldId);

			if (itemToConsume == null)
			{
				Log.Warning($"ConsumeItem: Character '{this.Name}' attempted to consume item WorldId '{worldId}', but it was not found in their inventory.");
				return false;
			}

			if (itemToConsume.IsLocked)
			{
				Log.Info($"ConsumeItem: Character '{this.Name}' could not consume item '{itemToConsume.Name}' because it is now locked.");
				this.ServerMessage("Cannot use the item because it is locked.");
				return false;
			}

			if (itemToConsume.Amount < quantity)
			{
				Log.Warning($"ConsumeItem: Character '{this.Name}' attempted to consume {quantity} of '{itemToConsume.Name}', but only had {itemToConsume.Amount}.");
				return false;
			}

			this.Inventory.Remove(itemToConsume, quantity, InventoryItemRemoveMsg.Used);
			return true;
		}

		/// <summary>
		/// Equips a new item. Note: This does not remove it from inventory. For testing purposes.
		/// </summary>
		public void EquipItem(EquipSlot slot, int itemId)
		{
			if (this.Inventory.HasItem(itemId))
				return;

			var item = new Item(itemId, 1);
			this.Inventory.SetEquipSilent(slot, item);
		}
	}
}
