using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;

namespace Melia.Zone.World.Quests.Weekly
{
	public static class WeeklyQuestKeys
	{
		public const string Monster = "Monster";
		public const string Elite = "Elite";
		public const string Boss = "Boss";
		public const string Mythic = "Mythic";
		public const string Dungeon = "Dungeon";

		public static string Map(int index) => "Map." + index;
		public static string Race(RaceType race) => "Race." + race;
		public static string Attribute(AttributeType attribute) => "Attribute." + attribute;
		public static string Size(SizeType size) => "Size." + size;

		public static IEnumerable<string> GetAll()
		{
			yield return Monster;
			yield return Elite;
			yield return Boss;
			yield return Mythic;
			yield return Dungeon;

			foreach (var race in WeeklyQuestPools.RaceTargets)
				yield return Race(race);

			foreach (var attribute in WeeklyQuestPools.AttributeTargets)
				yield return Attribute(attribute);

			foreach (var size in WeeklyQuestPools.SizeTargets)
				yield return Size(size);

			for (var i = 0; i < 8; i++)
				yield return Map(i);
		}

		public static WeeklyQuestType GetQuestType(string key)
		{
			if (key == Monster) return WeeklyQuestType.Monster;
			if (key == Elite) return WeeklyQuestType.Elite;
			if (key == Boss) return WeeklyQuestType.Boss;
			if (key == Mythic) return WeeklyQuestType.Mythic;
			if (key == Dungeon) return WeeklyQuestType.Dungeon;
			if (key.StartsWith("Map.", StringComparison.Ordinal)) return WeeklyQuestType.Map;
			if (key.StartsWith("Race.", StringComparison.Ordinal)) return WeeklyQuestType.Race;
			if (key.StartsWith("Attribute.", StringComparison.Ordinal)) return WeeklyQuestType.Attribute;
			if (key.StartsWith("Size.", StringComparison.Ordinal)) return WeeklyQuestType.Size;
			throw new ArgumentOutOfRangeException(nameof(key), key, "Unknown Weekly Quest key.");
		}
	}
}
