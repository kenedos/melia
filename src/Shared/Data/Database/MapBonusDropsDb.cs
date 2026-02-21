using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using Yggdrasil.Logging;

namespace Melia.Shared.Data.Database
{
	/// <summary>
	/// Represents a single bonus drop item for a map.
	/// </summary>
	[Serializable]
	public class MapBonusDropItemData
	{
		public int ItemId { get; set; }
		public float Chance { get; set; }
		public int MinAmount { get; set; } = 1;
		public int MaxAmount { get; set; } = 1;
	}

	/// <summary>
	/// Represents bonus drops configuration for a specific map.
	/// </summary>
	[Serializable]
	public class MapBonusDropData
	{
		public string MapClassName { get; set; }
		public List<MapBonusDropItemData> Items { get; set; } = new List<MapBonusDropItemData>();
	}

	/// <summary>
	/// Database for map-specific bonus drops.
	/// Allows configuring additional item drops for mobs killed in specific maps.
	/// </summary>
	public class MapBonusDropsDb : DatabaseJson<MapBonusDropData>
	{
		private readonly Dictionary<string, MapBonusDropData> _byMapClassName = new Dictionary<string, MapBonusDropData>(StringComparer.OrdinalIgnoreCase);
		private readonly ItemDb _itemDb;

		public MapBonusDropsDb(ItemDb itemDb)
		{
			this._itemDb = itemDb;
		}

		/// <summary>
		/// Tries to find bonus drop data for the given map class name.
		/// </summary>
		/// <param name="mapClassName"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool TryFind(string mapClassName, out MapBonusDropData data)
		{
			return _byMapClassName.TryGetValue(mapClassName, out data);
		}

		/// <summary>
		/// Returns bonus drop data for the given map class name, or null if not found.
		/// </summary>
		/// <param name="mapClassName"></param>
		/// <returns></returns>
		public MapBonusDropData Find(string mapClassName)
		{
			_byMapClassName.TryGetValue(mapClassName, out var data);
			return data;
		}

		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("mapClassName", "items");

			var data = new MapBonusDropData();
			data.MapClassName = entry.ReadString("mapClassName");

			var items = entry["items"] as JArray;
			if (items != null)
			{
				foreach (var itemEntry in items)
				{
					var itemObj = itemEntry as JObject;
					if (itemObj == null)
						continue;

					var itemId = itemObj.ReadInt("itemId");
					var chance = itemObj.ReadFloat("chance");
					var minAmount = itemObj.ReadInt("minAmount", 1);
					var maxAmount = itemObj.ReadInt("maxAmount", minAmount);

					// Validate item exists
					if (!_itemDb.TryFind(itemId, out var itemData))
					{
						Log.Warning($"MapBonusDropsDb: Item '{itemId}' not found for map '{data.MapClassName}', skipping.");
						continue;
					}

					data.Items.Add(new MapBonusDropItemData
					{
						ItemId = itemId,
						Chance = chance,
						MinAmount = minAmount,
						MaxAmount = maxAmount
					});
				}
			}

			if (data.Items.Count > 0)
			{
				this.Entries.Add(data);
				_byMapClassName[data.MapClassName] = data;
			}
			else
			{
				Log.Warning($"MapBonusDropsDb: No valid items for map '{data.MapClassName}', entry skipped.");
			}
		}
	}
}
