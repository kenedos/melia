using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using Yggdrasil.Util;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class ItemSummonBossData
	{
		public int ClassId { get; set; }
		public int SummonLevel { get; set; }
		public int Count { get; set; }
		public int Ratio { get; set; }
		public int LifeTime { get; set; }
		public string Group { get; set; }
		public string MonsterClassName { get; set; }
	}

	/// <summary>
	/// Item summon boss database for monster card album summons.
	/// </summary>
	public class ItemSummonBossDb : DatabaseJsonIndexed<int, ItemSummonBossData>
	{
		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("classId", "summonLevel", "count", "ratio", "lifeTime", "group", "monsterClassName");

			var data = new ItemSummonBossData();

			data.ClassId = entry.ReadInt("classId");
			data.SummonLevel = entry.ReadInt("summonLevel");
			data.Count = entry.ReadInt("count");
			data.Ratio = entry.ReadInt("ratio");
			data.LifeTime = entry.ReadInt("lifeTime");
			data.Group = entry.ReadString("group");
			data.MonsterClassName = entry.ReadString("monsterClassName");

			this.AddOrReplace(data.ClassId, data);
		}

		/// <summary>
		/// Returns all entries matching the given groups.
		/// </summary>
		/// <param name="groups">Array of group names to match (e.g., "Red", "Blue")</param>
		/// <returns></returns>
		public IEnumerable<ItemSummonBossData> GetByGroups(params string[] groups)
		{
			var isAll = groups.Any(g => g == "All");
			return this.Entries.Values.Where(e => isAll || groups.Contains(e.Group));
		}

		/// <summary>
		/// Selects a random boss from the given groups using weighted selection.
		/// </summary>
		/// <param name="groups">Array of group names to match (e.g., "Red", "Blue")</param>
		/// <returns>Selected boss data, or null if no matching entries found.</returns>
		public ItemSummonBossData GetRandomFromGroups(params string[] groups)
		{
			var matchingEntries = this.GetByGroups(groups).ToList();
			if (matchingEntries.Count == 0)
				return null;

			var totalRatio = matchingEntries.Sum(e => e.Ratio);
			var roll = RandomProvider.Get().Next(1, totalRatio + 1);

			var cumulativeRatio = 0;
			foreach (var entry in matchingEntries)
			{
				cumulativeRatio += entry.Ratio;
				if (roll <= cumulativeRatio)
					return entry;
			}

			// Fallback (shouldn't happen)
			return matchingEntries.Last();
		}
	}
}
