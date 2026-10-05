using System;
using Melia.Zone;
using Melia.Zone.Services;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;

public class HuntingTaskObjective : QuestObjective
{
	public HuntingTaskObjective()
	{
		this.TargetCount = 50;
	}

	public override void Load()
	{
	}

	public override void Unload()
	{
	}

	public static void Sync(Character character)
	{
		if (character == null)
			return;

		var task = ZoneServer.Instance.Database.GetHuntingTask(character.AccountDbId);

		if (task == null)
			return;

		var monster = task.SelectedMonsterId > 0
			? HuntingTaskPool.GetMonster(task.SelectedMonsterId)
			: null;

		character.Quests.UpdateObjectives<HuntingTaskObjective>((quest, objective, progress) =>
		{
			if (!progress.Unlocked)
				return;

			if (monster != null)
				objective.Text = $"Defeat {monster.Name}";

			var targetCount = task.RequiredKills > 0
				? task.RequiredKills
				: objective.TargetCount;

			progress.Count = Math.Min(task.CurrentKills, targetCount);

			if (task.Status == 2 || progress.Count >= targetCount)
				progress.SetDone();
			else
				progress.Done = false;
		});

		character.Quests.RefreshClientQuest(
			new QuestId("hunting_tasks", 1001));
	}

	public static void Reset(Character character)
	{
		if (character == null)
			return;

		character.Quests.UpdateObjectives<HuntingTaskObjective>((quest, objective, progress) =>
		{
			progress.Count = 0;
			progress.Done = false;
		});

		var questId = new QuestId("hunting_tasks", 1001);

		character.Quests.RefreshClientQuest(questId);
		character.Quests.UpdateClient();
	}
}
