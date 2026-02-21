using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class SelectItemData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public int SelectItemTranscend { get; set; }
		public int SelectItemReinforce { get; set; }
		public string Script { get; set; }
		public List<int> RequiredItemCount { get; set; }
		public Dictionary<int, List<SimpleItemData>> Items { get; set; } = new Dictionary<int, List<SimpleItemData>>();
	}

	[Serializable]
	public class SimpleItemData
	{
		public string ItemId { get; set; }
		public int Amount { get; set; }
	}

	/// <summary>
	/// Select Item database.
	/// </summary>
	public class SelectItemDb : DatabaseJsonIndexed<string, SelectItemData>
	{
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "className", "requiredItemCount", "items");

			var data = new SelectItemData();

			data.Id = entry.ReadInt("id");
			data.ClassName = entry.ReadString("className");
			data.RequiredItemCount = entry.ReadList<int>("requiredItemCount");

			if (entry.ContainsKey("items"))
			{
				foreach (JObject item in entry["items"])
				{
					item.AssertNotMissing("group", "item", "amount");
					var group = item.ReadInt("group");
					if (!data.Items.ContainsKey(group))
						data.Items.Add(group, new List<SimpleItemData>());

					var itemData = new SimpleItemData();

					itemData.ItemId = item.ReadString("item");
					itemData.Amount = item.ReadInt("amount", 1);

					data.Items[group].Add(itemData);
				}
			}

			this.Entries[data.ClassName] = data;
		}
	}
}
