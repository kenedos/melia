using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Quests.Daily;
using Yggdrasil.Util;

namespace Melia.Zone.World.Quests.Weekly
{
	public class WeeklyQuestGenerator
	{
		private const int MapCount = 8;

		public List<WeeklyQuestDefinition> Generate()
		{
			var quests = new List<WeeklyQuestDefinition>
			{
				CreateQuest(WeeklyQuestKeys.Monster, WeeklyQuestType.Monster, "Monster Hunter", "Defeat normal monsters.", 8000),
				CreateQuest(WeeklyQuestKeys.Elite, WeeklyQuestType.Elite, "Elite Hunter", "Defeat elite monsters.", 640),
				CreateQuest(WeeklyQuestKeys.Boss, WeeklyQuestType.Boss, "Boss Hunter", "Defeat field bosses.", 64),
				CreateQuest(WeeklyQuestKeys.Mythic, WeeklyQuestType.Mythic, "Mythic Hunter", "Defeat mythic monsters.", 160),
				CreateQuest(WeeklyQuestKeys.Dungeon, WeeklyQuestType.Dungeon, "Dungeon Explorer", "Complete dungeons.", 40),
			};

			foreach (var race in WeeklyQuestPools.RaceTargets)
				quests.Add(CreateRaceQuest(race));

			foreach (var attribute in WeeklyQuestPools.AttributeTargets)
				quests.Add(CreateAttributeQuest(attribute));

			foreach (var size in WeeklyQuestPools.SizeTargets)
				quests.Add(CreateSizeQuest(size));

			quests.AddRange(this.CreateMapQuests());

			if (quests.Count != 29)
				throw new InvalidOperationException($"Weekly quest generator created {quests.Count} quests instead of 29.");

			return quests;
		}

		private static WeeklyQuestDefinition CreateRaceQuest(RaceType race)
		{
			var name = DailyQuestDisplayNames.GetRaceDisplayName(race);
			var quest = CreateQuest(WeeklyQuestKeys.Race(race), WeeklyQuestType.Race, name + " Hunter", "Defeat " + name.ToLowerInvariant() + " monsters.", 6400);
			quest.TargetRace = (int)race;
			return quest;
		}

		private static WeeklyQuestDefinition CreateAttributeQuest(AttributeType attribute)
		{
			var name = DailyQuestDisplayNames.GetAttributeDisplayName(attribute);
			var quest = CreateQuest(WeeklyQuestKeys.Attribute(attribute), WeeklyQuestType.Attribute, name + " Hunter", "Defeat monsters with the " + name.ToLowerInvariant() + " attribute.", 4800);
			quest.TargetAttribute = (int)attribute;
			return quest;
		}

		private static WeeklyQuestDefinition CreateSizeQuest(SizeType size)
		{
			var name = DailyQuestDisplayNames.GetSizeDisplayName(size);
			var quest = CreateQuest(WeeklyQuestKeys.Size(size), WeeklyQuestType.Size, name + " Monster Hunter", "Defeat " + name.ToLowerInvariant() + " monsters.", 4800);
			quest.TargetSize = (int)size;
			return quest;
		}

		private IEnumerable<WeeklyQuestDefinition> CreateMapQuests()
		{
			var maps = WeeklyQuestPools.MapTargets;
			if (maps == null || maps.Count < MapCount)
				throw new InvalidOperationException($"At least {MapCount} Weekly Quest maps are required, but only {maps?.Count ?? 0} are available.");

			var indexes = new int[maps.Count];
			for (var i = 0; i < indexes.Length; i++)
				indexes[i] = i;

			var random = RandomProvider.Get();
			for (var i = indexes.Length - 1; i > 0; i--)
			{
				var swapIndex = random.Next(i + 1);
				(indexes[i], indexes[swapIndex]) = (indexes[swapIndex], indexes[i]);
			}

			for (var i = 0; i < MapCount; i++)
			{
				var map = maps[indexes[i]];
				var quest = CreateQuest(WeeklyQuestKeys.Map(i), WeeklyQuestType.Map, "Regional Hunter", "Defeat monsters in " + map.DisplayName + ".", 1500);
				quest.TargetMapClassName = map.ClassName;
				yield return quest;
			}
		}

		private static WeeklyQuestDefinition CreateQuest(string key, WeeklyQuestType type, string name, string description, int target)
		{
			if (target <= 0)
				throw new ArgumentOutOfRangeException(nameof(target));

			return new WeeklyQuestDefinition
			{
				Key = key,
				Type = type,
				Name = name,
				Description = description,
				Target = target,
			};
		}
	}
}
