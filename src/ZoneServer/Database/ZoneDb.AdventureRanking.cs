using System.Collections.Generic;
using System.Linq;
using MySqlConnector;

namespace Melia.Zone.Database
{
	public class AdventureRankingEntry
	{
		public long AccountId { get; set; }
		public string TeamName { get; set; } = "";
		public long MonsterKills { get; set; }
		public int UniqueMonsters { get; set; }
		public int ClassesDiscovered { get; set; }
		public int UniqueItems { get; set; }
		public long ItemsObtained { get; set; }
		public long ItemsCrafted { get; set; }
		public long ItemsUsed { get; set; }
		public int UniqueMonsterDropPairs { get; set; }
		public int MonstersWithDrops { get; set; }
		public int UniqueDropItems { get; set; }
		public long TotalDrops { get; set; }
		public long Points { get; set; }
	}

	public partial class ZoneDb
	{
		public List<AdventureRankingEntry> GetAdventureRanking()
		{
			var result = new List<AdventureRankingEntry>();

			using var conn = this.GetConnection();
			using var cmd = new MySqlCommand(@"
SELECT
	a.accountId,
	COALESCE(MAX(c.teamName), '') AS teamName,
	COALESCE(MAX(ab.monsterKills), 0) AS monsterKills,
	COALESCE(MAX(ab.uniqueMonsters), 0) AS uniqueMonsters,
	COALESCE(MAX(ab.classesDiscovered), 0) AS classesDiscovered,
	COALESCE(MAX(ai.uniqueItems), 0) AS uniqueItems,
	COALESCE(MAX(ai.itemsObtained), 0) AS itemsObtained,
	COALESCE(MAX(ai.itemsCrafted), 0) AS itemsCrafted,
	COALESCE(MAX(ai.itemsUsed), 0) AS itemsUsed,
	COALESCE(MAX(ad.uniqueMonsterDropPairs), 0) AS uniqueMonsterDropPairs,
	COALESCE(MAX(ad.monstersWithDrops), 0) AS monstersWithDrops,
	COALESCE(MAX(ad.uniqueDropItems), 0) AS uniqueDropItems,
	COALESCE(MAX(ad.totalDrops), 0) AS totalDrops
FROM
(
	SELECT accountId FROM adventure_book
	UNION
	SELECT accountId FROM adventure_book_items
	UNION
	SELECT accountId FROM adventure_book_monster_drops
) a
LEFT JOIN characters c ON c.accountId = a.accountId
LEFT JOIN
(
	SELECT
		accountId,
		COALESCE(SUM(CASE WHEN type = 0 THEN count ELSE 0 END), 0) AS monsterKills,
		COUNT(DISTINCT CASE WHEN type = 0 THEN classId END) AS uniqueMonsters,
		COUNT(DISTINCT CASE WHEN type = 9 THEN classId END) AS classesDiscovered
	FROM adventure_book
	GROUP BY accountId
) ab ON ab.accountId = a.accountId
LEFT JOIN
(
	SELECT
		accountId,
		COUNT(DISTINCT CASE WHEN obtainCount > 0 THEN itemId END) AS uniqueItems,
		COALESCE(SUM(obtainCount), 0) AS itemsObtained,
		COALESCE(SUM(craftCount), 0) AS itemsCrafted,
		COALESCE(SUM(useCount), 0) AS itemsUsed
	FROM adventure_book_items
	GROUP BY accountId
) ai ON ai.accountId = a.accountId
LEFT JOIN
(
	SELECT
		accountId,
		COUNT(DISTINCT CONCAT(monsterId, ':', itemId)) AS uniqueMonsterDropPairs,
		COUNT(DISTINCT monsterId) AS monstersWithDrops,
		COUNT(DISTINCT itemId) AS uniqueDropItems,
		COALESCE(SUM(count), 0) AS totalDrops
	FROM adventure_book_monster_drops
	GROUP BY accountId
) ad ON ad.accountId = a.accountId
GROUP BY a.accountId
ORDER BY uniqueMonsters DESC, uniqueItems DESC, classesDiscovered DESC, a.accountId ASC;", conn);

			using var reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				result.Add(new AdventureRankingEntry
				{
					AccountId = reader.GetInt64("accountId"),
					TeamName = reader.GetString("teamName"),
					Points =
						(long)reader.GetInt32("uniqueMonsters") * 10L +
						(long)reader.GetInt32("classesDiscovered") * 50L +
						(long)reader.GetInt32("uniqueItems") * 5L +
						(long)reader.GetInt32("monstersWithDrops") * 5L +
						(long)reader.GetInt32("uniqueDropItems") * 5L +
						(long)reader.GetInt32("uniqueMonsterDropPairs") * 2L,
					MonsterKills = reader.GetInt64("monsterKills"),
					UniqueMonsters = reader.GetInt32("uniqueMonsters"),
					ClassesDiscovered = reader.GetInt32("classesDiscovered"),
					UniqueItems = reader.GetInt32("uniqueItems"),
					ItemsObtained = reader.GetInt64("itemsObtained"),
					ItemsCrafted = reader.GetInt64("itemsCrafted"),
					ItemsUsed = reader.GetInt64("itemsUsed"),
					UniqueMonsterDropPairs = reader.GetInt32("uniqueMonsterDropPairs"),
					MonstersWithDrops = reader.GetInt32("monstersWithDrops"),
					UniqueDropItems = reader.GetInt32("uniqueDropItems"),
					TotalDrops = reader.GetInt64("totalDrops")
				});
			}

			return result
	.OrderByDescending(x => x.Points)
	.ThenByDescending(x => x.UniqueMonsters)
	.ThenByDescending(x => x.UniqueItems)
	.ThenByDescending(x => x.ClassesDiscovered)
	.ThenBy(x => x.AccountId)
	.ToList();
		}
	}
}
