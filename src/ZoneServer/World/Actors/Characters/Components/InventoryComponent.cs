using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Versioning;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Items.Effects;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Represents a character's inventory.
	/// </summary>
	public class InventoryComponent : CharacterComponent
	{
		private readonly object _syncLock = new();

		private Dictionary<InventoryCategory, List<Item>> _items = new();
		private readonly Dictionary<long, Item> _itemsWorldIndex = new();
		private readonly Dictionary<EquipSlot, Item> _equip = new(InventoryDefaults.EquipSlotCount);
		private readonly List<Item> _warehouse = new();
		private readonly Dictionary<int, Item> _cards = new();

		/// <summary>
		/// Clears all internal collections to release item references
		/// for GC after the character has been saved and removed from
		/// the world.
		/// </summary>
		public void ReleaseAll()
		{
			lock (_syncLock)
			{
				foreach (var list in _items.Values)
					list.Clear();
				_items.Clear();
				_itemsWorldIndex.Clear();
				_equip.Clear();
				_warehouse.Clear();
				_cards.Clear();
			}
		}

		/// <summary>
		/// Raised when the character equipped an item.
		/// </summary>
		public event Action<Character, Item> Equipped;

		/// <summary>
		/// Raised when the character unequipped an item.
		/// </summary>
		public event Action<Character, Item> Unequipped;

		/// <summary>
		/// Creates new inventory for character.
		/// </summary>
		public InventoryComponent(Character character) : base(character)
		{
			foreach (InventoryCategory category in Enum.GetValues(typeof(InventoryCategory)))
				_items.Add(category, new List<Item>());

			for (EquipSlot slot = 0; slot < (EquipSlot)InventoryDefaults.EquipSlotCount; ++slot)
				_equip.Add(slot, new DummyEquipItem(slot));
		}

		/// <summary>
		/// Returns list of ids of equipped items, including dummy items,
		/// in the order of EquipSlot.
		/// </summary>
		/// <returns></returns>
		public int[] GetEquipIds()
		{
			// TODO: Cache.

			lock (_syncLock)
				return _equip.Where(a => (int)a.Key <= InventoryDefaults.EquipSlotCount).OrderBy(a => a.Key).Select(a => a.Value.Id).ToArray();
		}

		/// <summary>
		/// Returns list of ids of equipped items with briquetting
		/// appearance overrides applied, in the order of EquipSlot.
		/// If an equipped item has a BriquettingIndex property set,
		/// that value is used instead of the actual item id.
		/// </summary>
		/// <returns></returns>
		public int[] GetVisualEquipIds()
		{
			lock (_syncLock)
			{
				return _equip
					.Where(a => (int)a.Key <= InventoryDefaults.EquipSlotCount)
					.OrderBy(a => a.Key)
					.Select(a =>
					{
						var briquettingIndex = (int)a.Value.Properties.GetFloat(PropertyName.BriquettingIndex);
						return briquettingIndex > 0 ? briquettingIndex : a.Value.Id;
					})
					.ToArray();
			}
		}

		/// <summary>
		/// Returns unordered list of ids of equipped items, excluding
		/// dummy items.
		/// </summary>
		/// <returns></returns>
		public int[] GetActualEquipIds()
		{
			// TODO: Cache.

			lock (_syncLock)
				return _equip.Where(a => (int)a.Key <= InventoryDefaults.EquipSlotCount && a.Value is not DummyEquipItem).Select(a => a.Value.Id).ToArray();
		}

		/// <summary>
		/// Returns dictionary of equipped items.
		/// </summary>
		/// <remarks>
		/// If no actual item is equipped in a slot, it will be filled with
		/// a dummy item, which this method returns as well.
		/// </remarks>
		/// <returns></returns>
		public Dictionary<EquipSlot, Item> GetEquip()
		{
			lock (_syncLock)
				return _equip.ToDictionary(a => a.Key, a => a.Value);
		}

		/// <summary>
		/// Returns item equipped in the given slot.
		/// </summary>
		/// <remarks>
		/// If no actual item is equipped in a slot, it will be filled with
		/// a dummy item, which this method returns as well.
		/// </remarks>
		/// <param name="slot"></param>
		/// <returns></returns>
		public Item GetEquip(EquipSlot slot)
		{
			lock (_syncLock)
			{
				_equip.TryGetValue(slot, out var item);
				return item;
			}
		}

		/// <summary>
		/// Returns item equipped in the given slot.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public Item GetEquip(int itemId)
		{
			lock (_syncLock)
			{
				return _equip.Values.FirstOrDefault(a => a.Id == itemId);
			}
		}

		/// <summary>
		/// Returns if an item is equipped.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public bool IsEquipped(int itemId)
		{
			lock (_syncLock)
			{
				return _equip.Values.Any(a => a.Id == itemId);
			}
		}

		/// <summary>
		/// Returns the sum of the property on all equipped items.
		/// Gems are calculated in a special way because the client doesn't
		/// allow them to have properties directly (i.e. 'STR') so we use
		/// the RandomOption properties as a hacky way to do it.
		/// </summary>
		/// <returns></returns>
		public float GetEquipProperties(string propertyName)
		{
			lock (_syncLock)
			{
				return _equip.Values
					.Where(a => a.MaxDurability == 0 || a.Durability != 0)
					.Sum(item =>
					{
						var total = item.Properties.GetFloat(propertyName, 0);

						// Add gem socket stats from random options
						if (item.HasSockets)
						{
							foreach (var gem in item.GetGemSockets().Where(g => g != null))
							{
								var option = gem.Properties.GetString("RandomOption_1");
								if (option == propertyName)
								{
									total += gem.Properties.GetFloat("RandomOptionValue_1", 0);
								}
							}
						}

						return total;
					});
			}
		}

		/// <summary>
		/// Returns the sum of the properties on all equipped items.
		/// </summary>
		/// <returns></returns>
		public float GetSumOfEquipProperties(params string[] propertyNames)
		{
			var total = 0f;

			lock (_syncLock)
			{
				foreach (var propertyName in propertyNames)
					total += _equip.Values.Sum(a => a.Properties.GetFloat(propertyName, 0) + (a.GetGemSockets()?.Sum(a => a?.Properties.GetFloat(propertyName, 0) ?? 0) ?? 0));
			}

			return total;
		}

		/// <summary>
		/// Returns the total card bonus for a specific property.
		/// Calculates on-demand by summing all equipped card contributions.
		/// </summary>
		/// <param name="propertyName">The property name to get card bonuses for.</param>
		/// <returns>The total bonus from all equipped cards for this property.</returns>
		public float GetCardBonuses(string propertyName)
		{
			return CardPropertyModifier.Instance.GetCardBonus(this.Character, propertyName);
		}

		/// <summary>
		/// Returns the number of equipped items with the given armor
		/// material type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public int CountEquipMaterial(ArmorMaterialType type)
		{
			lock (_syncLock)
				return _equip.Values.Where(a => a.Data.Material == type).Sum(a => a.Amount);
		}

		/// <summary>
		/// Returns a dictionary with all items, Key being their inventory
		/// index.
		/// </summary>
		/// <returns></returns>
		public IDictionary<int, Item> GetItems()
			=> this.GetItems(null);

		/// <summary>
		/// Returns a dictionary with all items that match the predicate,
		/// Key being their index.
		/// </summary>
		/// <returns></returns>
		public IDictionary<int, Item> GetItems(Func<Item, bool> predicate)
		{
			var result = new Dictionary<int, Item>();

			lock (_syncLock)
			{
				foreach (var category in _items)
				{
					for (var i = 0; i < category.Value.Count; ++i)
					{
						var index = category.Value[i].GetInventoryIndex(i);
						var item = category.Value[i];

						if (predicate == null || predicate(item))
							result.Add(index, item);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Returns a list with all items in the warehouse.
		/// </summary>
		/// <returns></returns>
		public IList<Item> GetWarehouseItems()
		{
			lock (_syncLock)
				return this._warehouse;
		}

		/// <summary>
		/// Returns a dictionary with all items' inventory indices and
		/// object ids.
		/// </summary>
		/// <returns></returns>
		public IDictionary<int, long> GetIndices()
		{
			var result = new Dictionary<int, long>();

			lock (_syncLock)
			{
				foreach (var category in _items)
				{
					for (var i = 0; i < category.Value.Count; ++i)
					{
						var index = category.Value[i].GetInventoryIndex(i);
						var itemObjectId = category.Value[i].ObjectId;

						result.Add(index, itemObjectId);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Returns a dictionary with the inventory indices and object
		/// ids of all items in the given category.
		/// </summary>
		/// <returns></returns>
		public IDictionary<int, long> GetIndices(InventoryCategory category)
		{
			var result = new Dictionary<int, long>();

			lock (_syncLock)
			{
				if (!_items.TryGetValue(category, out var items))
					throw new ArgumentException("Unknown item category.");

				for (var i = 0; i < items.Count; ++i)
				{
					var index = items[i].GetInventoryIndex(i);
					var itemObjectId = items[i].ObjectId;

					result.Add(index, itemObjectId);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns true if any item in the inventory is set to expire
		/// </summary>
		public bool HasExpiringItems => this.HasItems(a => a.IsExpiring);

		/// <summary>
		/// Returns true if an item with the given predicate exist in the inventory.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public bool HasItems(Func<Item, bool> predicate)
		{
			lock (_syncLock)
				return _items.SelectMany(a => a.Value).Any(predicate);
		}

		/// <summary>
		/// Returns true if an item with the given class name exist in the inventory.
		/// </summary>
		/// <param name="itemClassName">Item's ClassName</param>
		/// <param name="amount">Item Amount</param>
		/// <returns></returns>
		public bool HasItem(string itemClassName, int amount = 1)
		{
			return ZoneServer.Instance.Data.ItemDb.TryFind(itemClassName, out var itemClass)
				&& this.HasItem(itemClass.Id, amount);
		}

		/// <summary>
		/// Returns true if an item with the given id exist in the inventory.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public bool HasItem(int itemId, int amount = 1)
		{
			lock (_syncLock)
				return _items.SelectMany(a => a.Value).Any(a => a.Id == itemId && a.Amount >= amount);
		}

		/// <summary>
		/// Returns amount of items with the given id in the inventory.
		/// </summary>
		/// <remarks>
		/// Stacks count as the amount of items in the stack.
		/// E.g.: One stack with 10 HP potions results in a count of 10.
		/// </remarks>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public int CountItem(int itemId)
		{
			lock (_syncLock)
				return _items.SelectMany(a => a.Value).Where(a => a.Id == itemId).Sum(a => a.Amount);
		}

		/// <summary>
		/// Returns item by world id, or null if it doesn't exist.
		/// </summary>
		/// <param name="worldId"></param>
		/// <returns></returns>
		public Item GetItem(long worldId, InventoryType inventoryType = InventoryType.Inventory)
		{
			Item item;
			lock (_syncLock)
			{
				switch (inventoryType)
				{
					default:
						_itemsWorldIndex.TryGetValue(worldId, out item);
						break;
				}
			}

			return item;
		}

		/// <summary>
		/// Returns item by world id, or null if it doesn't exist.
		/// Checks both inventory and equipment.
		/// </summary>
		/// <param name="worldId"></param>
		/// <returns></returns>
		public Item GetInventoryItem(long worldId, InventoryType inventoryType = InventoryType.Inventory)
		{
			Item item;
			lock (_syncLock)
			{
				switch (inventoryType)
				{
					default:
						if (!_itemsWorldIndex.TryGetValue(worldId, out item))
							item = _equip.Values.FirstOrDefault(item => item.ObjectId == worldId);
						break;
				}
			}

			return item;
		}

		/// <summary>
		/// Returns an item by its inventory index (category+index).
		/// Returns null if no item with the given index was found.
		/// </summary>
		/// <param name="inventoryIndex"></param>
		/// <returns></returns>
		public Item GetItemByIndex(int inventoryIndex)
		{
			ZoneServer.Instance.Data.InvBaseIdDb.SplitCategoryIndex(inventoryIndex, out var category, out var index);

			Item item;
			lock (_syncLock)
			{
				var list = _items[category];
				if (list.Count < index)
					return null;

				item = list[index];
			}

			return item;
		}

		/// <summary>
		/// Returns item in given equip slot, or null if there is none.
		/// </summary>
		/// <param name="slot"></param>
		/// <returns></returns>
		public Item GetItem(EquipSlot slot)
		{
			Item item;
			lock (_syncLock)
				_equip.TryGetValue(slot, out item);

			return item;
		}

		/// <summary>
		/// Returns the first item stack in the inventory that matches the
		/// given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public Item FindItem(Predicate<Item> predicate)
		{
			lock (_syncLock)
			{
				foreach (var category in _items)
				{
					for (var i = 0; i < category.Value.Count; ++i)
					{
						var item = category.Value[i];
						if (predicate(item))
							return item;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the first item stack in the inventory that matches the
		/// given class name via out. Returns false if no match was found.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool TryFindItem(string className, out Item item)
		{
			item = this.FindItem(a => a.Data.ClassName == className);
			return item != null;
		}

		/// <summary>
		/// Adds item to inventory without updating the character's client.
		/// </summary>
		/// <param name="item"></param>
		public void AddSilent(Item item, InventoryType inventoryType = InventoryType.Inventory)
		{
			var left = this.FillStacks(item, InventoryAddType.NotNew, true, inventoryType);
			if (left > 0)
			{
				item.Amount = left;
				this.AddStack(item, InventoryAddType.NotNew, true, inventoryType);
			}
		}

		/// <summary>
		/// Adds item to inventory and updates client.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="addType"></param>
		public bool Add(Item item, InventoryAddType addType = InventoryAddType.New,
			InventoryType inventoryType = InventoryType.Inventory, float notificationDelay = 0f,
			string reason = null)
		{
			if (_items.Count > 2000 && inventoryType == InventoryType.PersonalStorage)
				return false;

			var amountToAdd = item.Amount;

			var left = this.FillStacks(item, addType, false, inventoryType, notificationDelay, reason);
			if (left > 0)
			{
				item.Amount = left;
				this.AddStack(item, addType, false, inventoryType, notificationDelay);
			}

			if (inventoryType == InventoryType.Inventory)
				this.UpdateWeight();

			// The server's item list uses position-based indices that shift
			// when items are removed. The client doesn't re-index on its
			// own after ZC_ITEM_REMOVE, so we send a lightweight index
			// list for the affected category to keep client indices in
			// sync. This replaces a previous workaround that re-sent the
			// entire inventory (ZC_ITEM_INVENTORY_DIVISION_LIST) which
			// caused massive client lag with large inventories.
			if (Versions.Client > KnownVersions.ClosedBeta1)
			{
				Send.ZC_ITEM_INVENTORY_INDEX_LIST(this.Character, item.Data.Category);
				Send.ZC_EQUIP_GEM_INFO(this.Character);
			}

			ZoneServer.Instance.ServerEvents.PlayerAddedItem.Raise(new PlayerItemEventArgs(this.Character, item.Id, amountToAdd));

			return true;
		}

		/// <summary>
		/// Adds new items with given item id to inventory.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <param name="addType"></param>
		public void Add(int itemId, int amount, InventoryAddType addType = InventoryAddType.PickUp, string reason = null)
		{
			while (amount > 0)
			{
				// Item caps Amount automatically. By using amount directly
				// either the entire amount is used for the new item,
				// or the max amount for it. This way we add new stacks until
				// amount is 0.

				var item = new Item(itemId, amount);
				this.Add(item, addType, reason: reason);

				amount -= item.Amount;
			}
		}

		/// <summary>
		/// Filles stacks with item, returns the amount of items left.
		/// The item is not modified.
		/// </summary>
		/// <remarks>
		/// If no stacks are found, nothing happens, and the amount returned
		/// will be equal to item.Amount.
		/// </remarks>
		/// <param name="item">Item to fill existing stacks with.</param>
		/// <param name="silent">If true, client isn't updated.</param>
		private int FillStacks(Item item, InventoryAddType addType, bool silent,
			InventoryType inventoryType = InventoryType.Inventory, float notificationDelay = 0f,
			string reason = null)
		{
			// If item isn't stackable, we've got nothing to do here.
			if (!item.IsStackable)
				return item.Amount;

			var itemId = item.Id;
			var amount = item.Amount;
			var cat = item.Data.Category;
			var stacks = this.GetStacks(cat, itemId, inventoryType);

			// Fill stacks
			foreach (var index in stacks)
			{
				lock (_syncLock)
				{
					var categoryItem = inventoryType switch
					{
						InventoryType.PersonalStorage => _warehouse.ElementAtOrDefault(index),
						_ => _items[cat][index],
					};
					if (categoryItem == null)
						return item.Amount;
					var space = (categoryItem.Data.MaxStack - categoryItem.Amount);
					var add = Math.Min(amount, space);

					categoryItem.Amount += add;
					amount -= add;
					var previousAmount = categoryItem.Amount - add;

					if (!silent)
					{
						var categoryIndex = categoryItem.GetInventoryIndex(index);

						// Use given add type if this was the last of it,
						// or NotNew if only some was just added to a stack.
						if (previousAmount > 0)
							Send.ZC_ITEM_REMOVE(this.Character, categoryItem.ObjectId, previousAmount, InventoryItemRemoveMsg.Given, inventoryType);

						Send.ZC_ITEM_ADD(this.Character, categoryItem, categoryIndex, categoryItem.Amount, InventoryAddType.NotNew, inventoryType, notificationDelay);
						if (ZoneServer.Instance.Conf.Log.LogItems)
						{
							var logReason = reason ?? InventoryAddType.NotNew.ToString();
							ZoneServer.Instance.Database.LogItemTransaction(this.Character.DbId, this.Character.Name, categoryItem.ObjectId, categoryItem.DbId, categoryItem.Id, categoryItem.Name, add, "Add", logReason);
						}
					}
				}

				if (amount == 0)
					break;
			}

			return amount;
		}

		/// <summary>
		/// Adds given stack to inventory.
		/// </summary>
		/// <param name="item">Item to add to inventory.</param>
		/// <param name="silent">If true, client isn't updated.</param>
		private void AddStack(Item item, InventoryAddType addType, bool silent,
			InventoryType inventoryType = InventoryType.Inventory, float notificationDelay = 0f,
			string reason = null)
		{
			var cat = item.Data.Category;

			lock (_syncLock)
			{
				switch (inventoryType)
				{
					case InventoryType.PersonalStorage:
						_warehouse.Add(item);
						_itemsWorldIndex[item.ObjectId] = item;
						break;
					default:
						if (!_items.TryGetValue(cat, out var categoryList))
							return;
						categoryList.Add(item);
						_itemsWorldIndex[item.ObjectId] = item;
						break;
				}

				if (!silent)
				{
					var itemCount = inventoryType == InventoryType.PersonalStorage
						? _warehouse.Count
						: _items.TryGetValue(cat, out var catItems) ? catItems.Count : 0;
					var categoryIndex = item.GetInventoryIndex(itemCount - 1);
					Send.ZC_ITEM_ADD(this.Character, item, categoryIndex, item.Amount, addType, inventoryType, notificationDelay);

					if (ZoneServer.Instance.Conf.Log.LogItems)
					{
						var logReason = reason ?? addType.ToString();
						ZoneServer.Instance.Database.LogItemTransaction(this.Character.DbId, this.Character.Name, item.ObjectId, item.DbId, item.Id, item.Name, item.Amount, "Add", logReason);
					}
				}
			}
		}

		/// <summary>
		/// Returns indices of stacks in cat with the given item id that
		/// aren't full yet.
		/// </summary>
		/// <param name="cat"></param>
		/// <param name="itemId"></param>
		/// <returns></returns>
		private List<int> GetStacks(InventoryCategory cat, int itemId, InventoryType inventoryType = InventoryType.Inventory)
		{
			var result = new List<int>();

			lock (_syncLock)
			{
				switch (inventoryType)
				{
					case InventoryType.PersonalStorage:
						var index = _warehouse.FindIndex(item => item.Id == itemId && item.Amount < item.Data.MaxStack);
						result.Add(index);
						break;
					default:
						if (!_items.TryGetValue(cat, out var categoryItems))
							break;
						for (var i = 0; i < categoryItems.Count; ++i)
						{
							var item = categoryItems[i];
							if (item.Id == itemId && item.Amount < item.Data.MaxStack)
								result.Add(i);
						}
						break;
				}
			}

			return result;
		}

		/// <summary>
		/// Puts item into equip slot without updating the client.
		/// </summary>
		/// <param name="item"></param>
		public void SetEquipSilent(EquipSlot slot, Item item)
		{
			lock (_syncLock)
				_equip[slot] = item;

			this.Character.UpdateStance();
		}

		/// <summary>
		/// Checks if a card with the given item ID is currently equipped.
		/// </summary>
		/// <param name="itemId">The ID of the card to check.</param>
		/// <returns>True if the card is equipped, false otherwise.</returns>
		public bool IsCardEquipped(int itemId)
		{
			lock (_syncLock)
			{
				return _cards.Values.Any(a => a.Id == itemId);
			}
		}

		/// <summary>
		/// Returns the equipped card with the given item ID, or null if not found.
		/// </summary>
		/// <param name="itemId">The ID of the card to retrieve.</param>
		/// <returns>The equipped card if found, null otherwise.</returns>
		public Item GetEquippedCard(int itemId)
		{
			lock (_syncLock)
			{
				return _cards.Values.FirstOrDefault(a => a.Id == itemId);
			}
		}

		/// <summary>
		/// Retrieves the card item at the specified slot, or null if not found.
		/// </summary>
		/// <param name="slot">The slot to retrieve the card from.</param>
		/// <returns>The card item at the specified slot, or null if not found.</returns>
		public Item GetCard(int slot)
		{
			lock (_syncLock)
				return _cards.TryGetValue(slot, out var card) ? card : null;
		}

		/// <summary>
		/// Attempts to retrieve the card item at the specified slot.
		/// </summary>
		/// <param name="slot">The slot to retrieve the card from.</param>
		/// <param name="card">The card item if found, otherwise null.</param>
		/// <returns>True if the card was found, false otherwise.</returns>
		public bool TryGetCard(int slot, out Item card)
		{
			lock (_syncLock)
				return _cards.TryGetValue(slot, out card);
		}

		/// <summary>
		/// Returns a copy of the dictionary containing all equipped cards.
		/// </summary>
		/// <returns>A dictionary containing all equipped cards.</returns>
		public Dictionary<int, Item> GetCards()
		{
			lock (_syncLock)
				return _cards.ToDictionary(a => a.Key, a => a.Value);
		}

		/// <summary>
		/// Equips the item with the specified world ID to the given slot.
		/// </summary>
		/// <param name="slot">The slot to equip the card to.</param>
		/// <param name="worldId">The world ID of the item to equip.</param>
		/// <returns>An InventoryResult indicating the success or failure of the operation.</returns>
		public InventoryResult EquipCard(int slot, long worldId)
		{
			var item = this.GetItem(worldId);
			if (item == null)
				return InventoryResult.ItemNotFound;

			if (slot < 1 || slot > 15)
			{
				Log.Warning("EquipCard: Character '{0}' attempted to equip card to invalid slot {1}.", this.Character.Name, slot);
				return InventoryResult.InvalidSlot;
			}

			// Unequip existing card first if slot is occupied
			if (_cards.TryGetValue(slot, out var value) && value != null)
			{
				this.UnequipCard(slot);
			}

			// Remove from inventory and equip to card slot (consistent with regular Equip method)
			lock (_syncLock)
			{
				_cards[slot] = item;
				_items[item.Data.Category].Remove(item);
				_itemsWorldIndex.Remove(item.ObjectId);
			}

			// Update client (consistent with regular equipment)
			Send.ZC_ITEM_REMOVE(this.Character, item.ObjectId, 1, InventoryItemRemoveMsg.Equipped, InventoryType.Inventory);
			Send.ZC_ITEM_INVENTORY_INDEX_LIST(this.Character, item.Data.Category);
			Send.ZC_EQUIP_CARD_INFO(this.Character);
			Send.ZC_UPDATED_PCAPPEARANCE(this.Character);

			this.ProcessCardScript(item);

			this.Equipped?.Invoke(this.Character, item);

			return InventoryResult.Success;
		}

		/// <summary>
		/// Unequips the card from the specified slot.
		/// </summary>
		/// <param name="slot">The slot to unequip the card from.</param>
		/// <returns>An InventoryResult indicating the success or failure of the operation.</returns>
		public InventoryResult UnequipCard(int slot)
		{
			if (slot < 1 || slot > 15)
				return InventoryResult.InvalidSlot;

			Item item;
			lock (_syncLock)
			{
				if (!_cards.TryGetValue(slot, out item) || item == null)
					return InventoryResult.ItemNotFound;

				// Process unequip scripts BEFORE removing from inventory
				// so CardPropertyModifier can find the card's slot
				this.ProcessCardUnequipScript(item);

				_cards.Remove(slot);
			}

			Zone.Items.Effects.ItemHookRegistry.Instance.UnregisterItem(this.Character, item.ObjectId);
			Zone.Items.Effects.CardMetadataRegistry.Instance.Remove(item.ObjectId);

			Send.ZC_EQUIP_CARD_INFO(this.Character);
			Send.ZC_UPDATED_PCAPPEARANCE(this.Character);

			this.Add(item, InventoryAddType.NotNew);

			this.Unequipped?.Invoke(this.Character, item);

			return InventoryResult.Success;
		}

		/// <summary>
		/// Processes the equip scripts for all equipped items.
		/// </summary>
		public void ProcessEquipScripts()
		{
			foreach (var equip in _equip)
			{
				var slot = equip.Key;
				var item = equip.Value;

				// Try to execute script
				var script = item.Data.Script;

				if (ScriptableFunctions.Equip.TryGet("SCP_ON_EQUIP_ITEM", out var scriptFunc))
					scriptFunc(this.Character, item, slot);

				if (!string.IsNullOrEmpty(item.Data.EquipSkill) && ScriptableFunctions.Equip.TryGet("SCP_ON_EQUIP_ITEM_SKILL", out scriptFunc))
				{
					if (script == null)
					{
						script = new ItemScriptData()
						{
							StrArg2 = item.Data.EquipSkill,
						};
						item.Data.Script = script;
					}
					scriptFunc(this.Character, item, slot);
				}

				if (item.HasSockets)
				{
					foreach (var gem in item.GetUsedGemSockets())
					{
						if (ScriptableFunctions.Equip.TryGet("SCR_GEM_EQUIP", out scriptFunc))
							scriptFunc(this.Character, gem, slot);
					}
				}
			}
		}

		/// <summary>
		/// Processes the script for a single card when equipped.
		/// </summary>
		/// <param name="card">The card item to process.</param>
		public void ProcessCardScript(Item card)
		{
			if (!ZoneServer.Instance.Data.EquipCardDb.TryFind(card.Data.ClassName, out var cardData))
				return;

			var script = cardData.Script;
			if (script == null || string.IsNullOrEmpty(script.Function))
				return;

			if (!ScriptableFunctions.Card.TryGet(script.Function, out var scriptFunc))
				return;

			var conditionScript = cardData.ConditionScript;
			var metadata = new Melia.Zone.Items.Effects.CardMetadata
			{
				ConditionFunction = conditionScript?.Function ?? "",
				ConditionArg = conditionScript?.StrArg ?? "",
				ConditionArg2 = conditionScript?.StrArg2 ?? "",
				ConditionNumArg1 = conditionScript?.NumArg1 ?? 0,
				ConditionNumArg2 = conditionScript?.NumArg2 ?? 0,
				CardUseType = (int)cardData.UseType
			};
			Zone.Items.Effects.CardMetadataRegistry.Instance.Set(card.ObjectId, metadata);

			var cardLevel = card.Properties[PropertyName.CardLevel];

			scriptFunc(this.Character, null, card, cardLevel,
					  script.StrArg, script.StrArg2, script.StrArg3);
		}

		/// <summary>
		/// Processes the unequip script for a card to remove its effects.
		/// </summary>
		/// <param name="card">The card item to process.</param>
		public void ProcessCardUnequipScript(Item card)
		{
			if (!ZoneServer.Instance.Data.EquipCardDb.TryFind(card.Data.ClassName, out var cardData))
				return;

			var script = cardData.Script;
			if (script == null || string.IsNullOrEmpty(script.Function))
				return;

			if (!ScriptableFunctions.Card.TryGet(script.Function, out var scriptFunc))
				return;

			scriptFunc(this.Character, null, card, -card.Properties[PropertyName.CardLevel],
					  script.StrArg, script.StrArg2, script.StrArg3);
		}

		/// <summary>
		/// Processes the scripts for all equipped cards.
		/// </summary>
		public void ProcessCardScripts()
		{
			var cards = this.GetCards();
			foreach (var cardEntry in cards)
			{
				this.ProcessCardScript(cardEntry.Value);
			}
		}

		/// <summary>
		/// Moves item with the given id into the given slot.
		/// </summary>
		/// <param name="slot"></param>
		/// <param name="worldId"></param>
		/// <returns></returns>
		public InventoryResult Equip(EquipSlot slot, long worldId)
		{
			if (!Enum.IsDefined(typeof(EquipSlot), slot))
				return InventoryResult.InvalidSlot;

			var item = this.GetItem(worldId);
			if (item == null)
				return InventoryResult.ItemNotFound;

			// Check job-based weapon restrictions
			if (!item.Data.CanJobEquip(this.Character.JobClass))
				return InventoryResult.InvalidSlot;

			// Handle weapon slot conflicts between RH and LH.
			// Read current equip state under lock, then unequip outside it
			// since Unequip also acquires the lock.
			var unequipLH = false;
			var unequipRH = false;
			var redirectToRH = false;

			lock (_syncLock)
			{
				var rhItem = _equip[EquipSlot.RightHand];
				var lhItem = _equip[EquipSlot.LeftHand];
				var rhOccupied = rhItem is not DummyEquipItem;
				var lhOccupied = lhItem is not DummyEquipItem;

				if (slot == EquipSlot.RightHand)
				{
					if (lhOccupied)
					{
						var newIsTwoHanded = item.Data.IsTwoHanded;
						var lhIsTrinket = lhItem.Data.EquipType1 == EquipType.Trinket;

						// Equipping a 2H weapon: unequip LH if it's not a trinket
						// Equipping a 1H weapon: unequip LH if it's a trinket (trinkets pair with 2H only)
						if ((newIsTwoHanded && !lhIsTrinket) || (!newIsTwoHanded && lhIsTrinket))
							unequipLH = true;
					}
				}
				else if (slot == EquipSlot.LeftHand)
				{
					if (rhOccupied)
					{
						var rhIsTwoHanded = rhItem.Data.IsTwoHanded;
						var newIsTrinket = item.Data.EquipType1 == EquipType.Trinket;

						// Equipping a trinket to LH: unequip RH if it's a 1H weapon (trinkets pair with 2H only)
						// Equipping a non-trinket to LH: unequip RH if it's a 2H weapon (daggers/shields pair with 1H only)
						if ((newIsTrinket && !rhIsTwoHanded) || (!newIsTrinket && rhIsTwoHanded))
							unequipRH = true;
					}

					// If equipping a main weapon (Weapon group) to LH and RH has a 1H weapon,
					// move the new weapon to RH instead
					if (rhOccupied && !unequipRH && item.Data.Group == ItemGroup.Weapon && rhItem.Data.Group == ItemGroup.Weapon)
						redirectToRH = true;
				}
			}

			if (unequipLH)
				this.Unequip(EquipSlot.LeftHand);

			if (unequipRH)
				this.Unequip(EquipSlot.RightHand);

			if (redirectToRH)
			{
				this.Unequip(EquipSlot.RightHand);
				slot = EquipSlot.RightHand;
			}

			// Unequip existing item in the target slot
			if (this.GetItem(slot) is not DummyEquipItem)
				this.Unequip(slot);

			// Equip new item
			lock (_syncLock)
			{
				_equip[slot] = item;
				_items[item.Data.Category].Remove(item);
				_itemsWorldIndex.Remove(item.ObjectId);
			}

			// Update character
			this.HandleAppearanceChanges(slot);

			// Update client
			Send.ZC_ITEM_REMOVE(this.Character, item.ObjectId, 1, InventoryItemRemoveMsg.Equipped, InventoryType.Inventory);
			Send.ZC_ITEM_INVENTORY_INDEX_LIST(this.Character, item.Data.Category);
			Send.ZC_ITEM_EQUIP_LIST(this.Character);
			Send.ZC_UPDATED_PCAPPEARANCE(this.Character);
			Send.ZC_EQUIP_GEM_INFO(this.Character);

			// If the equipped item has a briquetting (appearance) override,
			// broadcast a look update so the swapped 3D model is rendered for
			// the player's own avatar and for nearby players. APPEARANCE_PC's
			// visualEquipIds covers other players' initial render but does not
			// retrigger a model swap on the wearer, so this packet is needed.
			var briquettingIndex = (int)item.Properties.GetFloat(PropertyName.BriquettingIndex);
			if (briquettingIndex > 0)
				Send.ZC_NORMAL.UpdateCharacterLook(this.Character, briquettingIndex, slot);

			if (this.Character.IsOutOfBody())
				Send.ZC_NORMAL.SetActorColor(this.Character, 255, 200, 100, 150, 0.01f);

			this.Equipped?.Invoke(this.Character, item);

			// Try to execute script
			var script = item.Data.Script;

			if (ScriptableFunctions.Equip.TryGet("SCP_ON_EQUIP_ITEM", out var scriptFunc))
				scriptFunc(this.Character, item, slot);

			if (!string.IsNullOrEmpty(item.Data.EquipSkill) && ScriptableFunctions.Equip.TryGet("SCP_ON_EQUIP_ITEM_SKILL", out scriptFunc))
			{
				if (script == null)
				{
					script = new ItemScriptData()
					{
						StrArg2 = item.Data.EquipSkill,
					};
					item.Data.Script = script;
				}
				scriptFunc(this.Character, item, slot);
			}

			if (item.HasSockets)
			{
				foreach (var gem in item.GetUsedGemSockets())
				{
					if (gem != null && ScriptableFunctions.Equip.TryGet("SCR_GEM_EQUIP", out scriptFunc))
						scriptFunc(this.Character, gem, slot);
				}
			}

			return InventoryResult.Success;
		}

		/// <summary>
		/// Moves item from given slot into inventory.
		/// </summary>
		/// <param name="slot"></param>
		/// <returns></returns>
		public InventoryResult Unequip(EquipSlot slot)
		{
			if (!Enum.IsDefined(typeof(EquipSlot), slot))
				return InventoryResult.InvalidSlot;

			var item = this.GetItem(slot);
			if (item == null || item is DummyEquipItem)
				return InventoryResult.ItemNotFound;

			lock (_syncLock)
				_equip[slot] = new DummyEquipItem(slot);

			// Update character
			this.HandleAppearanceChanges(slot);

			// Update client
			Send.ZC_ITEM_EQUIP_LIST(this.Character);
			Send.ZC_UPDATED_PCAPPEARANCE(this.Character);

			// If the unequipped item had a briquetting (appearance) override,
			// clear the previously broadcast look so the swapped 3D model is
			// detached on the wearer and on nearby players.
			var briquettingIndex = (int)item.Properties.GetFloat(PropertyName.BriquettingIndex);
			if (briquettingIndex > 0)
				Send.ZC_NORMAL.UpdateCharacterLook(this.Character, 0, slot);

			if (this.Character.IsOutOfBody())
				Send.ZC_NORMAL.SetActorColor(this.Character, 255, 200, 100, 150, 0.01f);

			this.Add(item, InventoryAddType.NotNew);

			this.Unequipped?.Invoke(this.Character, item);

			// Try to execute script
			var script = item.Data.Script;

			if (ScriptableFunctions.Unequip.TryGet("SCP_ON_UNEQUIP_ITEM", out var scriptFunc))
				scriptFunc(this.Character, item, slot);

			if (!string.IsNullOrEmpty(item.Data.EquipSkill) && ScriptableFunctions.Unequip.TryGet("SCP_ON_UNEQUIP_ITEM_SKILL", out scriptFunc))
			{
				if (script == null)
				{
					script = new ItemScriptData()
					{
						StrArg2 = item.Data.EquipSkill,
					};
					item.Data.Script = script;
				}
				scriptFunc(this.Character, item, slot);
			}

			if (item.HasSockets)
			{
				foreach (var gem in item.GetUsedGemSockets())
				{
					if (ScriptableFunctions.Unequip.TryGet("SCR_GEM_UNEQUIP", out scriptFunc))
						scriptFunc(this.Character, gem, slot);
				}
			}

			return InventoryResult.Success;
		}

		/// <summary>
		/// Updates appearance related properties when (un)equipping
		/// relevant item.
		/// </summary>
		/// <param name="slot"></param>
		private void HandleAppearanceChanges(EquipSlot slot)
		{
			if (slot == EquipSlot.Hair)
			{
				var hair = this.Character.Hair;

				var hairEquip = this.GetEquip(EquipSlot.Hair);
				if (hairEquip is not DummyEquipItem)
				{
					var hairClassName = hairEquip.Data.Script.StrArg;

					if (ZoneServer.Instance.Data.HairTypeDb.TryFindByClassName(hairClassName, out var partData))
						hair = partData.Index;
				}

				if (hair != this.Character.Hair)
					this.Character.Variables.Perm.SetInt("Melia.DisplayHair", hair);
				else
					this.Character.Variables.Perm.Remove("Melia.DisplayHair");
			}

			this.Character.UpdateStance();
		}

		/// <summary>
		/// Unequips all currently equipped items.
		/// </summary>
		public void UnequipAll()
		{
			foreach (var slot in Enum.GetValues(typeof(EquipSlot)))
				this.Unequip((EquipSlot)slot);
		}

		/// <summary>
		/// Remove an item and a given amount
		/// </summary>
		/// <param name="itemClassName"></param>
		/// <param name="amount"></param>
		/// <returns>Amount of pieces removed.</returns>
		public int RemoveItem(string itemClassName, int amount = 1)
		{
			if (ZoneServer.Instance.Data.ItemDb.TryFind(itemClassName, out var itemData))
				return this.Remove(itemData.Id, amount, InventoryItemRemoveMsg.Given);
			return 0;
		}

		/// <summary>
		/// Remove an item and a given amount
		/// </summary>
		/// <param name="itemClassName"></param>
		/// <param name="amount"></param>
		/// <returns>Amount of pieces removed.</returns>
		public int RemoveItem(int itemId, int amount = 1)
		{
			if (ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out var itemData))
				return this.Remove(itemData.Id, amount, InventoryItemRemoveMsg.Given);
			return 0;
		}

		/// <summary>
		/// Removes item with given id from inventory.
		/// </summary>
		/// <param name="worldId"></param>
		/// <param name="amount"></param>
		/// <param name="msg"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public InventoryResult Remove(long worldId, int amount = 1, InventoryItemRemoveMsg msg = InventoryItemRemoveMsg.Destroyed, InventoryType type = InventoryType.Inventory, string reason = "", bool silently = false)
		{
			var item = this.GetItem(worldId, type);
			if (item == null || item is DummyEquipItem)
				return InventoryResult.ItemNotFound;

			return this.Remove(item, amount, msg, type, silently);
		}

		/// <summary>
		/// Removes item from inventory.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="msg"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private InventoryResult Remove(Item item, InventoryItemRemoveMsg msg = InventoryItemRemoveMsg.Destroyed, InventoryType type = InventoryType.Inventory, bool silently = false)
		{
			lock (_syncLock)
			{
				switch (type)
				{
					case InventoryType.PersonalStorage:
						if (!_warehouse.Remove(item))
							return InventoryResult.ItemNotFound;
						break;
					default:
						if (!_items[item.Data.Category].Remove(item))
							return InventoryResult.ItemNotFound;
						break;
				}

				_itemsWorldIndex.Remove(item.ObjectId);
			}

			// TODO: Add localizable strings or dictionary keys to item data,
			//   so that we can send those for the system message.
			if (msg == InventoryItemRemoveMsg.Destroyed)
				this.Character.SystemMessage("Delete{ITEM}{COUNT}", new MsgParameter("ITEM", item.Data.Name), new MsgParameter("COUNT", item.Amount));

			if (!silently)
				Send.ZC_ITEM_REMOVE(this.Character, item.ObjectId, item.Amount, msg, type);

			if (ZoneServer.Instance.Conf.Log.LogItems)
				ZoneServer.Instance.Database.LogItemTransaction(this.Character.DbId, this.Character.Name, item.ObjectId, item.DbId, item.Id, item.Name, item.Amount, "Remove", msg.ToString());

			// We need to update the indices after removing an item,
			// because we'll run into issues with the client potentially
			// misidentifying items otherwise, caused by duplicate indices.
			if (!silently)
			{
				Send.ZC_ITEM_INVENTORY_INDEX_LIST(this.Character, item.Data.Category);
				Send.ZC_EQUIP_GEM_INFO(this.Character);

				this.UpdateWeight();
			}

			return InventoryResult.Success;
		}

		/// <summary>
		/// Reduces item's amount by the given value. Item is removed
		/// if amount becomes 0.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="amount"></param>
		/// <param name="msg"></param>
		/// <param name="inventoryType"></param>
		/// <returns></returns>
		public InventoryResult Remove(Item item, int amount, InventoryItemRemoveMsg msg, InventoryType inventoryType = InventoryType.Inventory, bool silently = false)
		{
			// Check if item exists in inventory
			lock (_syncLock)
			{
				switch (inventoryType)
				{
					case InventoryType.PersonalStorage:
						if (!_warehouse.Contains(item))
							return InventoryResult.ItemNotFound;
						break;
					default:
						if (!_items[item.Data.Category].Contains(item))
							return InventoryResult.ItemNotFound;
						break;
				}
			}

			int amountRemoved;

			// Remove or reduce
			if (item.Amount <= amount)
			{
				this.Remove(item, msg, inventoryType, silently);
				amountRemoved = item.Amount;
			}
			else
			{
				item.Amount -= amount;
				amountRemoved = amount;

				if (!silently)
				{
					Send.ZC_ITEM_REMOVE(this.Character, item.ObjectId, amount, msg, inventoryType);

					if (inventoryType == InventoryType.Inventory && Versions.Client > KnownVersions.ClosedBeta1)
						Send.ZC_ITEM_INVENTORY_DIVISION_LIST(this.Character);
				}

				if (ZoneServer.Instance.Conf.Log.LogItems)
					ZoneServer.Instance.Database.LogItemTransaction(this.Character.DbId, this.Character.Name, item.ObjectId, item.DbId, item.Id, item.Name, amount, "Remove", msg.ToString());

				if (!silently && inventoryType == InventoryType.Inventory)
					this.UpdateWeight();
			}

			ZoneServer.Instance.ServerEvents.PlayerRemovedItem.Raise(new PlayerItemEventArgs(this.Character, item.Id, amountRemoved));

			return InventoryResult.Success;
		}

		/// <summary>
		/// Removes items with given id from inventory.
		/// </summary>
		/// <param name="itemId">Id of the item to remove.</param>
		/// <param name="amount">Amount of pieces to remove.</param>
		/// <returns>Amount of pieces removed.</returns>
		public int Remove(int itemId, int amount, InventoryItemRemoveMsg msg)
		{
			if (amount == 0)
				return 0;

			var amountRemoved = 0;

			// Potential optimization: don't search every category. However,
			// technically any item can be in any category.
			foreach (var itemkv in this.GetItems(a => a.Id == itemId))
			{
				var index = itemkv.Key;
				var item = itemkv.Value;
				var itemAmount = item.Amount;
				var category = item.Data.Category;
				var reduce = Math.Min(amount, item.Amount);

				item.Amount -= reduce;
				amount -= reduce;
				amountRemoved += reduce;

				if (reduce == itemAmount)
				{
					lock (_syncLock)
					{
						_items[category].Remove(item);
						_itemsWorldIndex.Remove(item.ObjectId);

					}
				}

				Send.ZC_ITEM_REMOVE(this.Character, item.ObjectId, reduce, msg, InventoryType.Inventory);
				if (reduce == itemAmount)
					Send.ZC_ITEM_INVENTORY_INDEX_LIST(this.Character, item.Data.Category);
				Send.ZC_EQUIP_GEM_INFO(this.Character);
			}

			if (amountRemoved != 0)
			{
				this.UpdateWeight();
				ZoneServer.Instance.ServerEvents.PlayerRemovedItem.Raise(new PlayerItemEventArgs(this.Character, itemId, amountRemoved));
			}

			return amountRemoved;
		}

		/// <summary>
		/// Swaps the two items with each other.
		/// </summary>
		/// <param name="worldId1"></param>
		/// <param name="worldId2"></param>
		/// <returns></returns>
		public InventoryResult Swap(long worldId1, long worldId2, InventoryType inventoryType = InventoryType.Inventory)
		{
			var item1 = this.GetItem(worldId1, inventoryType);
			var item2 = this.GetItem(worldId2, inventoryType);

			if (item1 == null || item2 == null)
				return InventoryResult.ItemNotFound;

			if (item1.Data.Category != item2.Data.Category)
				return InventoryResult.InvalidOperation;

			var category = item1.Data.Category;

			lock (_syncLock)
			{
				IList<Item> list;
				switch (inventoryType)
				{
					case InventoryType.PersonalStorage:
						list = _warehouse;
						break;
					default:
						list = _items[category];
						break;
				}
				var index1 = list.IndexOf(item1);
				var index2 = list.IndexOf(item2);

				list[index1] = item2;
				list[index2] = item1;
			}

			//Send.ZC_ITEM_INVENTORY_INDEX_LIST(this.Character, category);

			return InventoryResult.Success;
		}

		/// <summary>
		/// Sorts inventory by given order.
		/// </summary>
		/// <param name="order"></param>
		public void Sort(InventoryOrder order)
		{
			lock (_syncLock)
			{
				var items = new Dictionary<InventoryCategory, List<Item>>();

				// Go through the categories one by one and sort them by
				// the given order.

				foreach (var category in _items.Keys)
				{
					switch (order)
					{
						case InventoryOrder.Price:
							items[category] = _items[category].OrderBy(a => a.Data.Price).ToList();
							break;

						case InventoryOrder.Weight:
							items[category] = _items[category].OrderBy(a => a.Data.Weight).ToList();
							break;

						//case InventoryOrder.Grade:
						//	items[category] = _items[category].OrderBy(a => a.[...]).ToList();
						//	break;

						case InventoryOrder.Name:
							items[category] = _items[category].OrderBy(a => a.Data.Name).ToList();
							break;

						default:
							items[category] = _items[category].OrderBy(a => a.Id).ToList();
							break;
					}
				}

				_items = items;
			}

			// Sending ZC_ITEM_INVENTORY_INDEX_LIST stopped working at some
			// point after the iCBT2, officials send a full list now.
			//Send.ZC_ITEM_INVENTORY_INDEX_LIST(this.Character);

			// Sending ZC_ITEM_INVENTORY_LIST stopped working at some point.
			// Third time's the charm, I bet ZC_ITEM_INVENTORY_DIVISION_LIST
			// is way better than the previous options!
			//Send.ZC_ITEM_INVENTORY_LIST(this.Character);

			Send.ZC_ITEM_INVENTORY_DIVISION_LIST(this.Character);
			Send.ZC_EQUIP_GEM_INFO(this.Character);
		}

		/// <summary>
		/// Calculate the current weight and updates the client.
		/// </summary>
		private void UpdateWeight()
		{
			var prevMSPD = this.Character.Properties.GetFloat(PropertyName.MSPD);
			this.Character.Properties.Invalidate(PropertyName.NowWeight, PropertyName.MSPD);
			Send.ZC_OBJECT_PROPERTY(this.Character, PropertyName.NowWeight, PropertyName.MSPD);

			var currentMSPD = this.Character.Properties.GetFloat(PropertyName.MSPD);
			if (prevMSPD != currentMSPD)
				Send.ZC_MOVE_SPEED(this.Character);
		}

		/// <summary>
		/// Returns combined weight of all items in the inventory.
		/// </summary>
		/// <returns></returns>
		public float GetNowWeight()
		{
			var result = 0f;

			// TODO: Cache.
			lock (_syncLock)
			{
				result += _items.SelectMany(a => a.Value).Sum(a => a.Amount * a.Data.Weight);
				result += _equip.Values.Where(a => a is not DummyEquipItem).Sum(a => a.Amount * a.Data.Weight);
			}

			return result;
		}

		/// <summary>
		/// Logs the entire inventory and the equipment.
		/// </summary>
		public void Debug()
		{
			Log.Debug("<Debug> -----------------------------");

			Log.Debug("Inventory");
			foreach (var category in _items)
			{
				Log.Debug("  {0}", category.Key);
				for (var i = 0; i < category.Value.Count; ++i)
				{
					var index = category.Value[i].GetInventoryIndex(i);
					var item = category.Value[i];

					Log.Debug("    {0} : {1}", index, item.Data.ClassName);
				}
			}

			Log.Debug("Equip");
			foreach (var item in _equip.ToDictionary(a => a.Key, a => a.Value))
				Log.Debug("  {0} : {1}", item.Key, item.Value.Data.ClassName);

			Log.Debug("</Debug> ----------------------------");
		}

		/// <summary>
		/// Removes all items from inventory.
		/// </summary>
		public InventoryResult Clear()
		{
			var modifiedCategories = new HashSet<InventoryCategory>();
			var items = this.GetItems();

			// Remove items
			foreach (var item in items.Values)
			{
				lock (_syncLock)
				{
					_items[item.Data.Category].Remove(item);
					_itemsWorldIndex.Remove(item.ObjectId);
				}

				modifiedCategories.Add(item.Data.Category);
				Send.ZC_ITEM_REMOVE(this.Character, item.ObjectId, item.Amount, InventoryItemRemoveMsg.Destroyed, InventoryType.Inventory);
			}

			// Update categories
			//foreach (var category in modifiedCategories)
			//	Send.ZC_ITEM_INVENTORY_INDEX_LIST(this.Character, category);
			Send.ZC_ITEM_INVENTORY_DIVISION_LIST(this.Character);
			Send.ZC_EQUIP_GEM_INFO(this.Character);

			// Update weight
			this.UpdateWeight();

			return InventoryResult.Success;
		}

		/// <summary>
		/// Try to get an item with a given item world id.
		/// </summary>
		/// <param name="itemWorldId"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool TryGetItem(long itemWorldId, out Item item)
		{
			item = this.GetItem(itemWorldId);
			return item != null;
		}

		/// <summary>
		/// Try to get an item or equipment with a given item world id.
		/// </summary>
		/// <param name="itemWorldId"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool TryGetItemOrEquip(long itemWorldId, out Item item)
		{
			item = this.GetInventoryItem(itemWorldId);
			return item != null;
		}

		/// <summary>
		/// Returns an item by its inventory index (category+index) via out.
		/// Returns whether an item with the given index was found.
		/// </summary>
		/// <param name="inventoryIndex"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool TryGetItemByIndex(int inventoryIndex, out Item item)
		{
			item = this.GetItemByIndex(inventoryIndex);
			return item != null;
		}

		public InventoryResult AddSilentCard(int slot, Item card)
		{
			if (_cards.ContainsKey(slot))
				return InventoryResult.InvalidOperation;

			lock (_syncLock)
				_cards[slot] = card;

			return InventoryResult.Success;
		}

		public bool TryFindItem(int itemId, out Item item)
		{
			item = _itemsWorldIndex.Values.FirstOrDefault(a => a.Id == itemId);

			return item != null;
		}
	}

	public enum InventoryResult
	{
		Success,
		ItemNotFound,
		InvalidSlot,
		InvalidOperation,
	}
}
