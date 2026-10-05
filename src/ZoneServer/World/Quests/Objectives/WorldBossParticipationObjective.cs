using System;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Objectives
{
	public class WorldBossParticipationObjective : QuestObjective
	{
		public WorldBossParticipationObjective()
		{
			this.TargetCount = int.MaxValue;
		}

		public override void Load()
		{
		}

		public override void Unload()
		{
		}

		public static void ReportParticipation(Character character)
		{
			if (character == null)
				return;

			character.Quests.UpdateObjectives<WorldBossParticipationObjective>((quest, objective, progress) =>
			{
				if (!progress.Unlocked)
					return;

				if (progress.Count < int.MaxValue)
					progress.Count++;
			});

			character.Quests.RefreshClientQuest(new QuestId("world_boss", 1001));
		}

		public static void ResetParticipation(Character character)
		{
			if (character == null)
				return;

			character.Quests.UpdateObjectives<WorldBossParticipationObjective>((quest, objective, progress) =>
			{
				if (!progress.Unlocked)
					return;

				progress.Count = 0;
				progress.Done = false;
			});

			character.Quests.RefreshClientQuest(new QuestId("world_boss", 1001));

			character.Quests.UpdateClient();
		}
	}
}
