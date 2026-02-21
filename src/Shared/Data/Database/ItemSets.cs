using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	/// <summary>
	/// Represents a script reference for an item set.
	/// </summary>
	[Serializable]
	public class ItemSetScriptData
	{
		/// <summary>
		/// Gets the base script function name (e.g., "SCR_set_001").
		/// The script is called with old and new piece counts when equipment changes.
		/// </summary>
		public string Function { get; set; }
	}

	/// <summary>
	/// Represents an item set with its items and script handler.
	/// </summary>
	[Serializable]
	public class ItemSetData
	{
		/// <summary>
		/// Gets the unique ID of this set.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets the class name of this set (e.g., "set_001").
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		/// Gets the display name of this set.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the list of item class names that belong to this set.
		/// </summary>
		public List<string> Items { get; set; } = new();

		/// <summary>
		/// Gets the script reference for this set.
		/// The script handles all piece threshold bonuses internally.
		/// </summary>
		public ItemSetScriptData Script { get; set; }

		/// <summary>
		/// Returns the maximum number of pieces in this set.
		/// </summary>
		public int MaxPieces => this.Items.Count;
	}

	/// <summary>
	/// Database for item sets.
	/// </summary>
	public class ItemSetDb : DatabaseJsonIndexed<string, ItemSetData>
	{
		private readonly Dictionary<string, ItemSetData> _itemToSet = new();
		private readonly ItemDb _itemDb;

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="itemDb"></param>
		public ItemSetDb(ItemDb itemDb)
		{
			_itemDb = itemDb;
		}

		/// <summary>
		/// Returns the set that contains the given item, if any.
		/// </summary>
		/// <param name="itemClassName"></param>
		/// <param name="setData"></param>
		/// <returns></returns>
		public bool TryGetSetForItem(string itemClassName, out ItemSetData setData)
		{
			return _itemToSet.TryGetValue(itemClassName, out setData);
		}

		/// <summary>
		/// Returns the set with the given class name, if it exists.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="setData"></param>
		/// <returns></returns>
		public bool TryGetByClassName(string className, out ItemSetData setData)
		{
			return this.Entries.TryGetValue(className, out setData);
		}

		/// <summary>
		/// Returns the set with the given ID, if it exists.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="setData"></param>
		/// <returns></returns>
		public bool TryGetById(int id, out ItemSetData setData)
		{
			setData = this.Entries.Values.FirstOrDefault(s => s.Id == id);
			return setData != null;
		}

		/// <summary>
		/// Reads entry from JSON.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "className", "name", "items");

			var data = new ItemSetData();

			data.Id = entry.ReadInt("id");
			data.ClassName = entry.ReadString("className");
			data.Name = entry.ReadString("name");

			// Read items array
			var items = entry["items"] as JArray;
			if (items != null)
			{
				foreach (var item in items)
				{
					var itemClassName = item.ToString();
					if (!string.IsNullOrEmpty(itemClassName) && itemClassName != "None")
					{
						// Validate that the item exists
						if (_itemDb.FindByClass(itemClassName) == null)
							throw new DatabaseWarningException(null, $"Unknown item '{itemClassName}' in set '{data.ClassName}'.");

						data.Items.Add(itemClassName);
					}
				}
			}

			// Read script reference
			if (entry.ContainsKey("script"))
			{
				var scriptObj = entry["script"] as JObject;
				if (scriptObj != null)
				{
					data.Script = new ItemSetScriptData
					{
						Function = scriptObj.ReadString("function", "")
					};
				}
			}

			// Register the set
			this.Entries[data.ClassName] = data;

			// Build reverse lookup from items to sets
			foreach (var itemClassName in data.Items)
				_itemToSet[itemClassName] = data;
		}

		/// <summary>
		/// Clears the database.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			_itemToSet.Clear();
		}
	}
}
