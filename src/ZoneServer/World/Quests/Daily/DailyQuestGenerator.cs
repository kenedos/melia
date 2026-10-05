using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;

namespace Melia.Zone.World.Quests.Daily
{
	public class DailyQuestGenerator
	{
		public List<DailyQuestDefinition> Generate(
			DailyQuestDifficulty difficulty)
		{
			return new List<DailyQuestDefinition>
			{
				CreateQuest(
					DailyQuestType.Monster,
					difficulty,
					"Monster Hunter",
					"Defeat normal monsters.",
					GetTarget(
						DailyQuestType.Monster,
						difficulty)),

				CreateQuest(
					DailyQuestType.Elite,
					difficulty,
					"Elite Hunter",
					"Defeat elite monsters.",
					GetTarget(
						DailyQuestType.Elite,
						difficulty)),

				CreateQuest(
					DailyQuestType.Boss,
					difficulty,
					"Boss Hunter",
					"Defeat field bosses.",
					GetTarget(
						DailyQuestType.Boss,
						difficulty)),

				CreateQuest(
					DailyQuestType.Mythic,
					difficulty,
					"Mythic Hunter",
					"Defeat mythic monsters.",
					GetTarget(
						DailyQuestType.Mythic,
						difficulty)),

				CreateQuest(
					DailyQuestType.Dungeon,
					difficulty,
					"Dungeon Explorer",
					"Complete dungeons.",
					GetTarget(
						DailyQuestType.Dungeon,
						difficulty)),

				CreateMapQuest(
					difficulty),

				CreateRaceQuest(
					difficulty),

				CreateAttributeQuest(
					difficulty),

				CreateSizeQuest(
					difficulty),
			};
		}

		private DailyQuestDefinition CreateMapQuest(DailyQuestDifficulty difficulty)
		{
			var maps =
				DailyQuestPools.MapTargets;

			if (maps == null ||
				maps.Count == 0)
			{
				throw new InvalidOperationException(
					"No Daily Quest map targets are available.");
			}

			var map =
				Choose(
					maps);

			var quest =
				CreateQuest(
					DailyQuestType.Map,
					difficulty,
					"Regional Hunter",
					"Defeat monsters in " +
					map.DisplayName +
					".",
					GetTarget(
						DailyQuestType.Map,
						difficulty));

			quest.TargetMapClassName =
				map.ClassName;

			return quest;
		}

		private DailyQuestDefinition CreateRaceQuest(
			DailyQuestDifficulty difficulty)
		{
			var race =
				Choose(
					DailyQuestPools.RaceTargets);

			var raceName =
				DailyQuestDisplayNames
					.GetRaceDisplayName(
						race);

			var quest =
				CreateQuest(
					DailyQuestType.Race,
					difficulty,
					raceName +
					" Hunter",
					"Defeat " +
					raceName.ToLowerInvariant() +
					" monsters.",
					GetTarget(
						DailyQuestType.Race,
						difficulty));

			quest.TargetRace =
				(int)race;

			return quest;
		}

		private DailyQuestDefinition CreateAttributeQuest(
			DailyQuestDifficulty difficulty)
		{
			var attribute =
				Choose(
					DailyQuestPools.AttributeTargets);

			var attributeName =
				DailyQuestDisplayNames
					.GetAttributeDisplayName(
						attribute);

			var quest =
				CreateQuest(
					DailyQuestType.Attribute,
					difficulty,
					attributeName +
					" Hunter",
					"Defeat monsters with the " +
					attributeName.ToLowerInvariant() +
					" attribute.",
					GetTarget(
						DailyQuestType.Attribute,
						difficulty));

			quest.TargetAttribute =
				(int)attribute;

			return quest;
		}

		private DailyQuestDefinition CreateSizeQuest(
			DailyQuestDifficulty difficulty)
		{
			var size =
				Choose(
					DailyQuestPools.SizeTargets);

			var sizeName =
				DailyQuestDisplayNames
					.GetSizeDisplayName(
						size);

			var quest =
				CreateQuest(
					DailyQuestType.Size,
					difficulty,
					sizeName +
					" Monster Hunter",
					"Defeat " +
					sizeName.ToLowerInvariant() +
					" monsters.",
					GetTarget(
						DailyQuestType.Size,
						difficulty));

			quest.TargetSize =
				(int)size;

			return quest;
		}

		private DailyQuestDefinition CreateQuest(
			DailyQuestType type,
			DailyQuestDifficulty difficulty,
			string name,
			string description,
			int target)
		{
			return new DailyQuestDefinition
			{
				Type = type,
				Difficulty = difficulty,
				Name = name,
				Description = description,
				Target = target,
				Progress = 0,
				Reward =
					DailyQuestRewards.GetReward(
						difficulty),
				Completed = false,
				RewardClaimed = false,
			};
		}

		private int GetTarget(
			DailyQuestType type,
			DailyQuestDifficulty difficulty)
		{
			return difficulty switch
			{
				DailyQuestDifficulty.Easy =>
					type switch
					{
						DailyQuestType.Monster => 250,
						DailyQuestType.Elite => 20,
						DailyQuestType.Boss => 2,
						DailyQuestType.Mythic => 5,
						DailyQuestType.Dungeon => 1,
						DailyQuestType.Map => 250,
						DailyQuestType.Race => 200,
						DailyQuestType.Attribute => 150,
						DailyQuestType.Size => 150,
						_ => 0,
					},

				DailyQuestDifficulty.Medium =>
					type switch
					{
						DailyQuestType.Monster => 500,
						DailyQuestType.Elite => 40,
						DailyQuestType.Boss => 4,
						DailyQuestType.Mythic => 10,
						DailyQuestType.Dungeon => 3,
						DailyQuestType.Map => 500,
						DailyQuestType.Race => 400,
						DailyQuestType.Attribute => 300,
						DailyQuestType.Size => 300,
						_ => 0,
					},

				DailyQuestDifficulty.Hard =>
					type switch
					{
						DailyQuestType.Monster => 1000,
						DailyQuestType.Elite => 80,
						DailyQuestType.Boss => 8,
						DailyQuestType.Mythic => 20,
						DailyQuestType.Dungeon => 5,
						DailyQuestType.Map => 1000,
						DailyQuestType.Race => 800,
						DailyQuestType.Attribute => 600,
						DailyQuestType.Size => 600,
						_ => 0,
					},

				_ => 0,
			};
		}

		private static T Choose<T>(
			IReadOnlyList<T> values)
		{
			if (values == null ||
				values.Count == 0)
			{
				throw new InvalidOperationException(
					"Daily quest target pool cannot be empty.");
			}

			var random =
				RandomProvider.Get();

			return values[
				random.Next(
					values.Count)];
		}
	}
}
