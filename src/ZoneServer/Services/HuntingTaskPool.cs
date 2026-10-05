using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Yggdrasil.Logging;
using Melia.Zone.World.Quests.Daily;

namespace Melia.Zone.Services
{
	public class HuntingTaskMonster
	{
		public int MonsterId { get; set; }
		public string ClassName { get; set; }
		public string Name { get; set; }
		public int Level { get; set; }
		public string MapClassName { get; set; }
		public string MapName { get; set; }
		public string[] MapClassNames { get; set; }
		public string[] MapNames { get; set; }
	}

	public static class HuntingTaskPool
	{
		private static HuntingTaskMonster[] _monsters;

		public static IReadOnlyList<HuntingTaskMonster> Monsters
		{
			get
			{
				EnsureBuilt();
				return _monsters;
			}
		}

		public static HuntingTaskMonster[] GetOptions(int characterLevel, int count = 3)
		{
			EnsureBuilt();

			if (_monsters == null || _monsters.Length == 0)
				return Array.Empty<HuntingTaskMonster>();

			var minLevel = Math.Max(1, characterLevel - 10);
			var maxLevel = characterLevel + 10;

			var candidates = _monsters
				.Where(monster => monster.Level >= minLevel && monster.Level <= maxLevel)
				.ToArray();

			if (candidates.Length < count)
			{
				candidates = _monsters
					.OrderBy(monster => Math.Abs(monster.Level - characterLevel))
					.Take(Math.Max(count, 20))
					.ToArray();
			}

			if (candidates.Length == 0)
				return Array.Empty<HuntingTaskMonster>();

			return candidates
				.OrderBy(_ => Random.Shared.Next())
				.Take(count)
				.ToArray();
		}

		public static HuntingTaskMonster GetMonster(int monsterId)
		{
			EnsureBuilt();
			return _monsters?.FirstOrDefault(monster => monster.MonsterId == monsterId);
		}

		private static void EnsureBuilt()
		{
			if (_monsters != null)
				return;

			BuildPool();
		}

		private static void BuildPool()
		{
			var monsterDb = ZoneServer.Instance.Data.MonsterDb;
			var result = new List<HuntingTaskMonster>();

			var generatedSpawns = HuntingTaskMonsters.Spawns;

			var notFound = 0;
			var invalid = 0;
			var rootCrystals = 0;
			var invalidMaps = 0;

			foreach (var spawn in generatedSpawns)
			{
				if (!DailyQuestPools.TryGetHuntingTaskMapName(
					spawn.MapClassName,
					out var mapName))
				{
					invalidMaps++;

					Log.Warning(
						"[Hunting Task Pool] MAP REMOVED: MonsterId={0}, Map={1}",
						spawn.MonsterId,
						spawn.MapClassName
					);

					continue;
				}

				var monster = monsterDb.Find(spawn.MonsterId);

				if (monster == null)
				{
					notFound++;

					Log.Warning(
						"[Hunting Task Pool] NOT FOUND: MonsterId={0}, Map={1}",
						spawn.MonsterId,
						spawn.MapClassName
					);

					continue;
				}

				if (IsRootCrystal(monster))
				{
					rootCrystals++;

					continue;
				}

				if (!IsValidMonster(monster))
				{
					invalid++;

					Log.Warning(
						"[Hunting Task Pool] INVALID REMOVED: MonsterId={0}, Name={1}, ClassName={2}, Level={3}, Faction={4}, Rank={5}, Map={6}",
						monster.Id,
						monster.Name,
						monster.ClassName,
						monster.Level,
						monster.Faction,
						monster.Rank,
						spawn.MapClassName
					);

					continue;
				}

				result.Add(new HuntingTaskMonster
				{
					MonsterId = monster.Id,
					ClassName = monster.ClassName,
					Name = monster.Name,
					Level = monster.Level,
					MapClassName = spawn.MapClassName,
					MapName = mapName,
				});
			}

			_monsters = result
	.GroupBy(monster => monster.MonsterId)
	.Select(group =>
	{
		var first = group.First();

		first.MapClassNames = group
			.Select(monster => monster.MapClassName)
			.Where(map => !string.IsNullOrWhiteSpace(map))
			.Distinct(StringComparer.OrdinalIgnoreCase)
			.ToArray();

		first.MapNames = group
			.Select(monster => monster.MapName)
			.Where(map => !string.IsNullOrWhiteSpace(map))
			.Distinct(StringComparer.OrdinalIgnoreCase)
			.ToArray();

		return first;
	})
	.OrderBy(monster => monster.Level)
	.ThenBy(monster => monster.Name)
	.ToArray();

			Log.Info(
				"[Hunting Task Pool] GeneratedSpawns={0}, ValidMonsters={1}, Invalid={2}, NotFound={3}, RootCrystals={4}, InvalidMaps={5}",
				generatedSpawns.Length,
				_monsters.Length,
				invalid,
				notFound,
				rootCrystals,
				invalidMaps
			);
		}

		private static bool IsRootCrystal(MonsterData monster)
		{
			if (monster == null)
				return false;

			if (!string.IsNullOrWhiteSpace(monster.ClassName) &&
				monster.ClassName.IndexOf("rootcrystal", StringComparison.OrdinalIgnoreCase) >= 0)
				return true;

			if (!string.IsNullOrWhiteSpace(monster.Name) &&
				monster.Name.IndexOf("root crystal", StringComparison.OrdinalIgnoreCase) >= 0)
				return true;

			return false;
		}

		private static bool IsValidMonster(MonsterData monster)
		{
			if (monster == null)
				return false;

			if (monster.Id <= 0)
				return false;

			if (string.IsNullOrWhiteSpace(monster.ClassName))
				return false;

			if (string.IsNullOrWhiteSpace(monster.Name))
				return false;

			if (monster.Level <= 0)
				return false;

			if (monster.Faction != FactionType.Monster)
				return false;

			if (monster.Rank == MonsterRank.Boss)
				return false;

			return true;
		}

		public static HuntingTaskMonster Find(int monsterId)
		{
			EnsureBuilt();

			return _monsters.FirstOrDefault(
				monster => monster.MonsterId == monsterId
			);
		}

		public static bool Contains(int monsterId)
		{
			return Find(monsterId) != null;
		}

		public static void LogPool()
		{
			EnsureBuilt();

			Log.Info(
				"[Hunting Task Pool] Total valid monsters: {0}",
				_monsters.Length
			);
		}
	}
}
