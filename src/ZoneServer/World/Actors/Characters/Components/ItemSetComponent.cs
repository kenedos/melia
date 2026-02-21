using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
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
		/// Tracks the equipped item class names per set.
		/// </summary>
		private readonly Dictionary<string, HashSet<string>> _equippedSetItems = new();

		/// <summary>
		/// Tracks the current piece count per set (for script notification).
		/// </summary>
		private readonly Dictionary<string, int> _currentPieceCounts = new();

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

			if (!ZoneServer.Instance.Data.ItemSetDb.TryGetSetForItem(itemClassName, out var setData))
				return;

			lock (_syncLock)
			{
				// Initialize tracking for this set if needed
				if (!_equippedSetItems.TryGetValue(setData.ClassName, out var equippedItems))
				{
					equippedItems = new HashSet<string>();
					_equippedSetItems[setData.ClassName] = equippedItems;
					_currentPieceCounts[setData.ClassName] = 0;
				}

				var oldPieceCount = equippedItems.Count;

				// Add this item to the set tracking
				equippedItems.Add(itemClassName);

				var newPieceCount = equippedItems.Count;
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

				var oldPieceCount = equippedItems.Count;

				// Remove this item from set tracking
				equippedItems.Remove(itemClassName);

				var newPieceCount = equippedItems.Count;
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
					var item = kvp.Value;
					if (item == null || item.Id == 0) // Skip dummy items
						continue;

					var itemClassName = item.Data?.ClassName;
					if (string.IsNullOrEmpty(itemClassName))
						continue;

					if (!ZoneServer.Instance.Data.ItemSetDb.TryGetSetForItem(itemClassName, out var setData))
						continue;

					// Initialize tracking for this set if needed
					if (!_equippedSetItems.TryGetValue(setData.ClassName, out var setItems))
					{
						setItems = new HashSet<string>();
						_equippedSetItems[setData.ClassName] = setItems;
					}

					setItems.Add(itemClassName);
				}

				// Notify scripts for each tracked set (from 0 to current count)
				foreach (var setClassName in _equippedSetItems.Keys.ToList())
				{
					if (!ZoneServer.Instance.Data.ItemSetDb.TryGetByClassName(setClassName, out var setData))
						continue;

					var pieceCount = _equippedSetItems[setClassName].Count;
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
						EquippedPieceCount = equippedItems.Count
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
