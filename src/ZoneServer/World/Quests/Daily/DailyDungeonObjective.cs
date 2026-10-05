using System;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.World.Quests.Daily
{
	public class DailyDungeonObjective : QuestObjective
	{
		public DailyDungeonObjective(int targetCount)
		{
			if (targetCount <= 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(targetCount),
					"Daily dungeon target count must be greater than zero.");
			}

			this.TargetCount = targetCount;
		}

		public override void Load()
		{
		}

		public override void Unload()
		{
		}

		/// <summary>
		/// Registers one completed dungeon for the character and synchronizes
		/// the persistent daily progress with the Quest List and Quest Tracker.
		/// </summary>
		public static void ReportCompletion(Character character)
		{
			if (character == null)
				return;

			var manager = new DailyQuestManager();

			character.Quests.UpdateObjectives<DailyDungeonObjective>(
				(quest, objective, progress) =>
				{
					if (!progress.Unlocked)
						return;

					if (progress.Done)
						return;

					var changed = manager.IncreaseProgress(
						character,
						DailyQuestType.Dungeon,
						1);

					if (!changed)
						return;

					var savedQuest = manager.GetQuest(
						character,
						DailyQuestType.Dungeon);

					if (savedQuest == null)
						return;

					progress.Count = Math.Min(
						savedQuest.Progress,
						objective.TargetCount);

					if (progress.Count >= objective.TargetCount)
						progress.SetDone();
				});
		}
	}
}
