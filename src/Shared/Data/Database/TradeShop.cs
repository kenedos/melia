using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data;
using Melia.Shared.Data.Database;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using Yggdrasil.Logging;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class TradeShopItemData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public string Category { get; set; }
		public string Name { get; set; }
		public string ItemCraftedClassName { get; set; }
		public int ItemCraftedCount { get; set; }
		public string ShopType { get; set; }
		public Dictionary<int, int> RequiredItems { get; set; } = new Dictionary<int, int>();
	}

	/// <summary>
	/// Item Trade Shop database.
	/// </summary>
	public class TradeShopDb : DatabaseJsonIndexed<int, TradeShopItemData>
	{
		private readonly ItemDb _itemDb;

		public TradeShopDb(ItemDb itemDb)
		{
			_itemDb = itemDb;
		}

		public TradeShopItemData FindByClass(string name)
		{
			name = name.ToLower();
			return this.Entries.FirstOrDefault(a => a.Value.ClassName.ToLower() == name).Value;
		}

		public TradeShopItemData FindByItemClass(string name)
		{
			name = name.ToLower();
			return this.Entries.FirstOrDefault(a => a.Value.ItemCraftedClassName.ToLower() == name).Value;
		}

		protected override void ReadEntry(JObject entry)
		{
			// Values: id, className, [shopType], itemCrafted, itemCraftedCount, [requiredItems[]]
			entry.AssertNotMissing("id", "className", "itemCrafted", "itemCraftedCount", "requiredItems");

			var data = new TradeShopItemData();

			data.Id = entry.ReadInt("id");
			data.ClassName = entry.ReadString("className");
			data.Name = entry.ReadString("name");
			data.ItemCraftedClassName = entry.ReadString("itemCrafted");
			data.ItemCraftedCount = entry.ReadInt("itemCraftedCount");
			data.ShopType = entry.ReadString("shopType");

			if (entry.ContainsKey("requiredItems"))
			{
				foreach (JObject item in entry["requiredItems"].Cast<JObject>())
				{
					item.AssertNotMissing("item", "amount");
					var itemClassName = item.ReadString("item");
					var itemAmount = item.ReadInt("amount");
					var itemData = _itemDb.FindByClass(itemClassName);

					if (itemData == null)
					{
						Log.Warning("TradeShopDb: Required item {0} not found to trade for {1}", itemClassName, data.ItemCraftedClassName);
						return;
					}
					data.RequiredItems.Add(itemData.Id, itemAmount);
				}
			}

			this.AddOrReplace(data.Id, data);
		}
	}
}
