using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class RedemptionSetData
	{
		public string SetName { get; set; }
		public int MaterialItemId { get; set; }
		public Dictionary<int, int> RequiredAmounts { get; set; } = new();
		public List<RewardItemEntry> RewardItems { get; set; } = new();
	}

	// Helper class to represent an entry in the rewardItems array
	[Serializable]
	public class RewardItemEntry
	{
		public Gender Gender { get; set; } // Male, Female, or All (both)
		public Dictionary<int, int> Items { get; set; } = new();
	}

	/// <summary>
	/// Redemption set database, indexed by set name.
	/// </summary>
	public class RedemptionSetDb : DatabaseJsonIndexed<string, RedemptionSetData>
	{
		private readonly ItemDb _itemDb;

		public RedemptionSetDb(ItemDb itemDb)
		{
			_itemDb = itemDb;
		}

		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("setName", "materialItem", "requiredAmounts", "rewardItems"); // Validation

			var data = new RedemptionSetData();

			data.SetName = entry.ReadString("setName");

			var materialItemClassName = entry.ReadString("materialItem");
			var materialItemData = _itemDb.FindByClass(materialItemClassName);
			if (materialItemData == null)
				throw new DatabaseWarningException(null, $"Unknown material item '{materialItemClassName}' in redemption set '{data.SetName}'.");
			data.MaterialItemId = materialItemData.Id;
			data.RequiredAmounts = this.LoadRequiredAmounts(entry["requiredAmounts"], data.SetName);
			var rewardItemEntries = entry["rewardItems"].ToObject<List<JObject>>();
			foreach (var rewardItemEntry in rewardItemEntries)
			{
				var gender = rewardItemEntry.ReadEnum("gender", Gender.All);
				var rewardEntry = new RewardItemEntry { Gender = gender };
				rewardEntry.Items = LoadRewardItems(rewardItemEntry, data.SetName);
				data.RewardItems.Add(rewardEntry);
			}

			this.Entries[data.SetName] = data;
		}

		// Helper function to load required amounts
		private Dictionary<int, int> LoadRequiredAmounts(JToken token, string setName)
		{
			var amounts = new Dictionary<int, int>();
			var reqAmounts = token.ToObject<Dictionary<string, int>>();
			foreach (var (rewardClassName, requiredAmount) in reqAmounts)
			{
				var rewardItemData = _itemDb.FindByClass(rewardClassName);
				if (rewardItemData == null)
					throw new DatabaseWarningException(null, $"Unknown reward item '{rewardClassName}' in redemption set '{setName}'.");
				amounts[rewardItemData.Id] = requiredAmount;
			}
			return amounts;
		}

		// Helper to load reward items (from a JObject now)
		private Dictionary<int, int> LoadRewardItems(JObject rewardItemsObject, string setName)
		{
			var items = new Dictionary<int, int>();
			foreach (var property in rewardItemsObject.Properties()
				.Where(p => p.Name != "gender")) // Exclude the "gender" property
			{
				var rewardClassName = property.Name;
				var amount = property.Value.ToObject<int>();

				var rewardItemData = _itemDb.FindByClass(rewardClassName);
				if (rewardItemData == null)
					throw new DatabaseWarningException(null, $"Unknown reward item '{rewardClassName}' in redemption set '{setName}'.");
				items[rewardItemData.Id] = amount;
			}
			return items;
		}
	}
}
