using System;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.World.Quests.Weekly
{
	public class WeeklyDungeonObjective : QuestObjective
	{
		public WeeklyDungeonObjective(int targetCount)
		{
			if (targetCount <= 0)
				throw new ArgumentOutOfRangeException(nameof(targetCount));
			this.TargetCount = targetCount;
		}

		public override void Load() { }
		public override void Unload() { }

		public static void ReportCompletion(Character character)
		{
			if (character == null)
				return;

			var manager = new WeeklyQuestManager();
			character.Quests.UpdateObjectives<WeeklyDungeonObjective>((quest, objective, progress) =>
			{
				if (!progress.Unlocked || progress.Done || !manager.IncreaseProgress(character, WeeklyQuestKeys.Dungeon))
					return;

				var saved = manager.GetQuest(character, WeeklyQuestKeys.Dungeon);
				if (saved == null)
					return;

				progress.Count = Math.Min(saved.Progress, objective.TargetCount);
				if (progress.Count >= objective.TargetCount)
					progress.SetDone();
			});
		}
	}
}
