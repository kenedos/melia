using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Manages item set bonuses for a character.
	/// </summary>
	public class ItemSetComponent : CharacterComponent
	{
		private readonly object _syncLock = new();

		/// <summary>
		/// Tracks the equipped item class names and their counts per set.
		/// Uses a dictionary of counts instead of a HashSet to support
		/// duplicate items (e.g., two identical bracelets in a set).
		/// </summary>
		private readonly Dictionary<string, Dictionary<string, int>> _equippedSetItems = new();

		/// <summary>
		/// Tracks the current piece count per set (for script notification).
		/// </summary>
		private readonly Dictionary<string, int> _currentPieceCounts = new();

		/// <summary>
		/// Returns the total piece count from item count tracking.
		/// </summary>
		private static int GetPieceCount(Dictionary<string, int> itemCounts)
			=> itemCounts.Values.Sum();

		/// <summary>
		/// Returns the maximum allowed count for an item in a set,
		/// based on how many times it appears in the set definition.
		/// </summary>
		private static int GetMaxItemCount(ItemSetData setData, string itemClassName)
			=> setData.Items.Count(i => i == itemClassName);

		/// <summary>
		/// Returns true if the slot is a gear slot that can contribute
		/// to set bonuses (armor, weapons, accessories), excluding
		/// cosmetic and outer add-on slots.
		/// </summary>
		private static bool IsGearSlot(EquipSlot slot)
		{
			return slot switch
			{
				EquipSlot.Top => true,
				EquipSlot.Pants => true,
				EquipSlot.Gloves => true,
				EquipSlot.Shoes => true,
				EquipSlot.RightHand => true,
				EquipSlot.LeftHand => true,
				EquipSlot.Bracelet1 => true,
				EquipSlot.Bracelet2 => true,
				EquipSlot.Necklace => true,
				EquipSlot.Earring => true,
				_ => false,
			};
		}

		/// <summary>
		/// Creates new instance for character.
		/// </summary>
		/// <param name="character"></param>
		public ItemSetComponent(Character character)
			: base(character)
		{
			// Subscribe to equipment events
			character.Inventory.Equipped += this.OnItemEquipped;
			character.Inventory.Unequipped += this.OnItemUnequipped;
		}

		/// <summary>
		/// Called when an item is equipped.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		private void OnItemEquipped(Character character, Item item)
		{
			var itemClassName = item.Data.ClassName;

			// Log.Debug("ItemSetComponent.OnItemEquipped: '{0}' (Id={1}, IsDummy={2}) on '{3}'", itemClassName, item.Id, item is DummyEquipItem, character.Name);

			if (!ZoneServer.Instance.Data.ItemSetDb.TryGetSetForItem(itemClassName, out var setData))
				return;

			lock (_syncLock)
			{
				// Initialize tracking for this set if needed
				if (!_equippedSetItems.TryGetValue(setData.ClassName, out var equippedItems))
				{
					equippedItems = new Dictionary<string, int>();
					_equippedSetItems[setData.ClassName] = equippedItems;
					_currentPieceCounts[setData.ClassName] = 0;
				}

				var oldPieceCount = GetPieceCount(equippedItems);

				// Increment count for this item class name, capped at the
				// number of times it appears in the set definition
				equippedItems.TryGetValue(itemClassName, out var count);
				var maxCount = GetMaxItemCount(setData, itemClassName);
				if (count >= maxCount)
					return;

				equippedItems[itemClassName] = count + 1;

				var newPieceCount = GetPieceCount(equippedItems);
				_currentPieceCounts[setData.ClassName] = newPieceCount;

				// Notify script of the change
				if (oldPieceCount != newPieceCount)
					this.NotifyScriptOfChange(setData, oldPieceCount, newPieceCount);
			}
		}

		/// <summary>
		/// Called when an item is unequipped.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		private void OnItemUnequipped(Character character, Item item)
		{
			var itemClassName = item.Data.ClassName;

			if (!ZoneServer.Instance.Data.ItemSetDb.TryGetSetForItem(itemClassName, out var setData))
				return;

			lock (_syncLock)
			{
				if (!_equippedSetItems.TryGetValue(setData.ClassName, out var equippedItems))
					return;

				// Check if this item is actually tracked
				if (!equippedItems.TryGetValue(itemClassName, out var count))
					return;

				var oldPieceCount = GetPieceCount(equippedItems);

				// Decrement count, remove entry if it reaches 0
				if (count <= 1)
					equippedItems.Remove(itemClassName);
				else
					equippedItems[itemClassName] = count - 1;

				var newPieceCount = GetPieceCount(equippedItems);
				_currentPieceCounts[setData.ClassName] = newPieceCount;

				// Notify script of the change
				if (oldPieceCount != newPieceCount)
					this.NotifyScriptOfChange(setData, oldPieceCount, newPieceCount);

				// Clean up empty sets
				if (equippedItems.Count == 0)
				{
					_equippedSetItems.Remove(setData.ClassName);
					_currentPieceCounts.Remove(setData.ClassName);
				}
			}
		}

		/// <summary>
		/// Notifies the script of a piece count change.
		/// </summary>
		/// <param name="setData"></param>
		/// <param name="oldPieceCount"></param>
		/// <param name="newPieceCount"></param>
		private void NotifyScriptOfChange(ItemSetData setData, int oldPieceCount, int newPieceCount)
		{
			Log.Debug("ItemSetComponent: Set '{0}' ({1}) piece count changing from {2} to {3} on character '{4}'",
				setData.Name, setData.ClassName, oldPieceCount, newPieceCount, this.Character.Name);

			if (setData.Script == null || string.IsNullOrEmpty(setData.Script.Function))
			{
				Log.Debug("ItemSetComponent: Set '{0}' has no script function defined.", setData.ClassName);
				return;
			}

			if (ScriptableFunctions.ItemSet.TryGet(setData.Script.Function, out var func))
			{
				try
				{
					func(this.Character, setData, oldPieceCount, newPieceCount);
					Log.Debug("ItemSetComponent: Successfully executed script '{0}' for set '{1}'",
						setData.Script.Function, setData.ClassName);
				}
				catch (Exception ex)
				{
					Log.Warning("ItemSetComponent: Error executing script '{0}' for set '{1}': {2}",
						setData.Script.Function, setData.ClassName, ex.Message);
				}
			}
			else
			{
				Log.Warning("ItemSetComponent: Script function '{0}' not found for set '{1}'. Implement it in item_sets.cs.",
					setData.Script.Function, setData.ClassName);
			}
		}

		/// <summary>
		/// Recalculates all set bonuses based on currently equipped items.
		/// Call this on login to restore set bonus state.
		/// </summary>
		public void RecalculateAllSetBonuses()
		{
			lock (_syncLock)
			{
				// Clear existing tracking
				_equippedSetItems.Clear();
				_currentPieceCounts.Clear();

				// Get all equipped items
				var equippedItems = this.Character.Inventory.GetEquip();

				// Build set tracking from equipped items
				foreach (var kvp in equippedItems)
				{
					var slot = kvp.Key;
					var item = kvp.Value;
					if (item == null || item is DummyEquipItem)
						continue;

					// Only count items in gear slots, skip cosmetic/outer
					// add-on slots (Ring1-Ring4 are OUTERADD slots that can
					// contain stale data)
					if (!IsGearSlot(slot))
						continue;

					var itemClassName = item.Data?.ClassName;
					if (string.IsNullOrEmpty(itemClassName))
						continue;

					if (!ZoneServer.Instance.Data.ItemSetDb.TryGetSetForItem(itemClassName, out var setData))
						continue;

					Log.Debug("ItemSetComponent.Recalculate: Slot={0}, Item='{1}' (Id={2}) matches set '{3}'",
						slot, itemClassName, item.Id, setData.ClassName);

					// Initialize tracking for this set if needed
					if (!_equippedSetItems.TryGetValue(setData.ClassName, out var setItems))
					{
						setItems = new Dictionary<string, int>();
						_equippedSetItems[setData.ClassName] = setItems;
					}

					setItems.TryGetValue(itemClassName, out var count);
					var maxCount = GetMaxItemCount(setData, itemClassName);
					if (count < maxCount)
						setItems[itemClassName] = count + 1;
				}

				// Notify scripts for each tracked set (from 0 to current count)
				foreach (var setClassName in _equippedSetItems.Keys.ToList())
				{
					if (!ZoneServer.Instance.Data.ItemSetDb.TryGetByClassName(setClassName, out var setData))
						continue;

					var pieceCount = GetPieceCount(_equippedSetItems[setClassName]);
					_currentPieceCounts[setClassName] = pieceCount;

					// Notify script of the change (0 -> current count)
					this.NotifyScriptOfChange(setData, 0, pieceCount);
				}
			}
		}

		/// <summary>
		/// Returns information about currently active set bonuses.
		/// </summary>
		/// <returns></returns>
		public IReadOnlyList<ActiveSetBonusInfo> GetActiveSetBonuses()
		{
			var result = new List<ActiveSetBonusInfo>();

			lock (_syncLock)
			{
				foreach (var setKvp in _equippedSetItems)
				{
					var setClassName = setKvp.Key;
					var equippedItems = setKvp.Value;

					if (!ZoneServer.Instance.Data.ItemSetDb.TryGetByClassName(setClassName, out var setData))
						continue;

					result.Add(new ActiveSetBonusInfo
					{
						SetData = setData,
						EquippedPieceCount = GetPieceCount(equippedItems)
					});
				}
			}

			return result;
		}
	}

	/// <summary>
	/// Information about an active set bonus.
	/// </summary>
	public class ActiveSetBonusInfo
	{
		/// <summary>
		/// Gets the set data.
		/// </summary>
		public ItemSetData SetData { get; set; }

		/// <summary>
		/// Gets the number of pieces currently equipped.
		/// </summary>
		public int EquippedPieceCount { get; set; }
	}
}
