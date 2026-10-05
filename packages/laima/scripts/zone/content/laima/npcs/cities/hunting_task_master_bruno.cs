using System.Threading.Tasks;
using Melia.Zone;
using Melia.Zone.Database;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Services;
using Melia.Zone.World.Quests;
using ScriptsZoneLaima.content.laima.npcs.cities;
using static Melia.Zone.Scripting.Shortcuts;

public class HuntingTaskMasterBrunoScript : GeneralScript
{
	private const int NpcId = 151049;

	protected override void Load()
	{
		AddNpc(NpcId, L("[Hunting Task Master] Bruno"), "Bruno", "c_Klaipe", -300, 910, 0, async dialog =>
			{
				await ShowMainMenu(dialog);
			}
		);
	}

	private async Task ShowMainMenu(Dialog dialog)
	{
		var character = dialog.Player;
		var state = ZoneServer.Instance.HuntingTasks.GetState(character);
		var points = state?.Points ?? 0;
		var completed = state?.TasksCompleted ?? 0;

		dialog.SetTitle(L("[Hunting Task Master] Bruno"));

		await dialog.Msg(
			L(
				"Welcome to the Hunting Task System.\n\n" +
				$"Hunting Points: {points}\n" +
				$"Completed Tasks: {completed}"
			)
		);

		var option = await dialog.Select(
			L("What would you like to do?"),
			Option(L("Hunting Task"), "task"),
			Option(L("Hunting Shop"), "shop"),
			Option(L("System Information"), "info"),
			Option(L("Exit"), "exit")
		);

		switch (option)
		{
			case "task":
				await ShowHuntingTask(dialog);
				break;

			case "shop":
				await ShowShop(dialog);
				break;

			case "info":
				await ShowInformation(dialog);
				break;
		}
	}

	private async Task ShowHuntingTask(Dialog dialog)
	{
		var character = dialog.Player;
		var state = ZoneServer.Instance.HuntingTasks.GetState(character);

		if (state == null || state.Status == 0)
		{
			await ShowAvailableTasks(dialog);
			return;
		}

		if (state.Status == 1)
		{
			await ShowActiveTask(dialog, state);
			return;
		}

		if (state.Status == 2)
		{
			await ShowCompletedTask(dialog, state);
			return;
		}

		await ShowMainMenu(dialog);
	}

	private async Task ShowAvailableTasks(Dialog dialog)
	{
		var character = dialog.Player;
		var service = ZoneServer.Instance.HuntingTasks;
		var state = service.GetOrCreateTaskOptions(character);

		if (state == null)
		{
			await dialog.Msg(
				L("I couldn't generate Hunting Tasks for you right now.")
			);

			await ShowMainMenu(dialog);
			return;
		}

		var option1 = HuntingTaskPool.Find(state.Option1MonsterId);
		var option2 = HuntingTaskPool.Find(state.Option2MonsterId);
		var option3 = HuntingTaskPool.Find(state.Option3MonsterId);

		var name1 = option1?.Name ?? $"Monster {state.Option1MonsterId}";
		var name2 = option2?.Name ?? $"Monster {state.Option2MonsterId}";
		var name3 = option3?.Name ?? $"Monster {state.Option3MonsterId}";

		var level1 = option1?.Level ?? 0;
		var level2 = option2?.Level ?? 0;
		var level3 = option3?.Level ?? 0;

		var locations1 = GetLocations(option1);
		var locations2 = GetLocations(option2);
		var locations3 = GetLocations(option3);

		dialog.SetTitle(L("[Hunting Task Master] Bruno"));

		await dialog.Msg(
			L(
				"Choose your Hunting Task.\n\n" +
				"You may choose one of the three available monsters."
			)
		);

		var option = await dialog.Select(
			L("Select a target."),
			Option(L($"{name1} - Lv. {level1} - {locations1}"), "1"),
			Option(L($"{name2} - Lv. {level2} - {locations2}"), "2"),
			Option(L($"{name3} - Lv. {level3} - {locations3}"), "3"),
			Option(L("Reroll"), "reroll"),
			Option(L("Back"), "back")
		);

		switch (option)
		{
			case "1":
				await ConfirmTask(dialog, 1);
				break;

			case "2":
				await ConfirmTask(dialog, 2);
				break;

			case "3":
				await ConfirmTask(dialog, 3);
				break;

			case "reroll":
				await RerollTasks(dialog);
				break;

			default:
				await ShowMainMenu(dialog);
				break;
		}
	}

	private async Task ConfirmTask(Dialog dialog, int option)
	{
		var character = dialog.Player;
		var service = ZoneServer.Instance.HuntingTasks;
		var state = service.GetState(character);

		if (state == null)
		{
			await ShowAvailableTasks(dialog);
			return;
		}

		var monsterId = option switch
		{
			1 => state.Option1MonsterId,
			2 => state.Option2MonsterId,
			3 => state.Option3MonsterId,
			_ => 0,
		};

		var monster = HuntingTaskPool.Find(monsterId);

		if (monster == null)
		{
			await dialog.Msg(
				L("That Hunting Task is no longer available.")
			);

			await ShowAvailableTasks(dialog);
			return;
		}

		var locations = GetLocations(monster);

		var confirmation = await dialog.Select(
			L(
				$"Target: {monster.Name}\n" +
				$"Level: {monster.Level}\n" +
				$"Location: {locations}\n\n" +
				"Do you want to accept this Hunting Task?"
			),
			Option(L("Accept Hunting Task"), "accept"),
			Option(L("Back"), "back")
		);

		if (confirmation != "accept")
		{
			await ShowAvailableTasks(dialog);
			return;
		}

		if (!service.SelectTask(character, option))
		{
			await dialog.Msg(
				L("I couldn't start that Hunting Task.")
			);

			await ShowAvailableTasks(dialog);
			return;
		}

		await service.StartTracker(character);

		state = service.GetState(character);

		await dialog.Msg(
			L(
				"Your Hunting Task has begun!\n\n" +
				$"Target: {monster.Name}\n" +
				$"Location: {locations}\n" +
				$"Required Kills: {state.RequiredKills}\n" +
				$"Reward: {state.RewardPoints} Hunting Points"
			)
		);
	}

	private async Task ShowActiveTask(Dialog dialog, HuntingTaskState state)
	{
		var monster = HuntingTaskPool.Find(state.SelectedMonsterId);
		var monsterName = monster?.Name ?? $"Monster {state.SelectedMonsterId}";
		var locations = GetLocations(monster);

		dialog.SetTitle(L("[Hunting Task Master] Bruno"));

		var option = await dialog.Select(
			L(
				"Current Hunting Task\n\n" +
				$"Target: {monsterName}\n" +
				$"Location: {locations}\n" +
				$"Progress: {state.CurrentKills}/{state.RequiredKills}\n" +
				$"Reward: {state.RewardPoints} Hunting Points"
			),
			Option(L("Back"), "back")
		);

		await ShowMainMenu(dialog);
	}

	private async Task ShowCompletedTask(Dialog dialog, HuntingTaskState state)
	{
		var character = dialog.Player;
		var monster = HuntingTaskPool.Find(state.SelectedMonsterId);
		var monsterName = monster?.Name ?? $"Monster {state.SelectedMonsterId}";
		var locations = GetLocations(monster);

		dialog.SetTitle(L("[Hunting Task Master] Bruno"));

		var option = await dialog.Select(
			L(
				"Hunting Task Completed!\n\n" +
				$"Target: {monsterName}\n" +
				$"Location: {locations}\n" +
				$"Progress: {state.CurrentKills}/{state.RequiredKills}\n" +
				$"Reward: {state.RewardPoints} Hunting Points\n\n" +
				$"Total Hunting Points: {state.Points}\n" +
				$"Completed Tasks: {state.TasksCompleted}"
			),
			Option(L("Generate Next Hunting Task"), "next"),
			Option(L("Back"), "back")
		);

		if (option == "next")
		{
			if (!ZoneServer.Instance.HuntingTasks.GenerateNextTask(character))
			{
				await dialog.Msg(
					L("I couldn't generate your next Hunting Task.")
				);

				await ShowMainMenu(dialog);
				return;
			}

			await ShowAvailableTasks(dialog);
			return;
		}

		await ShowMainMenu(dialog);
	}

	private async Task RerollTasks(Dialog dialog)
	{
		var character = dialog.Player;
		var service = ZoneServer.Instance.HuntingTasks;

		var confirmation = await dialog.Select(
			L(
				"Rerolling will replace all three available Hunting Tasks.\n\n" +
				"Do you want to continue?"
			),
			Option(L("Reroll"), "reroll"),
			Option(L("Cancel"), "cancel")
		);

		if (confirmation != "reroll")
		{
			await ShowAvailableTasks(dialog);
			return;
		}

		if (!service.Reroll(character))
		{
			await dialog.Msg(
				L("I couldn't reroll your Hunting Tasks.")
			);

			await ShowAvailableTasks(dialog);
			return;
		}

		var state = service.GetState(character);

		await dialog.Msg(
			L(
				"Your Hunting Task choices have been rerolled.\n\n" +
				$"Total Rerolls: {state.RerollCount}"
			)
		);

		await ShowAvailableTasks(dialog);
	}

	private async Task ShowShop(Dialog dialog)
	{
		var character = dialog.Player;
		var points = ZoneServer.Instance.HuntingTasks.GetPoints(character);

		dialog.SetTitle(L("[Hunting Task Master] Bruno"));

		await dialog.Msg(
			L(
				"Hunting Shop\n\n" +
				$"Your Hunting Points: {points}\n\n" +
				"Exchange your Hunting Points for rewards."
			)
		);

		dialog.OpenPropertyShop(
			HuntingTaskShop.ShopName,
			string.Empty,
			"uphill_defense_shoppoint",
			"MercenaryWingShop");
	}

	private async Task ShowInformation(Dialog dialog)
	{
		dialog.SetTitle(L("[Hunting Task Master] Bruno"));

		var option = await dialog.Select(
			L(
				"Hunting Task System\n\n" +
				"Choose one monster from a list of three targets and complete the required number of kills.\n\n" +
				"Completing a Hunting Task rewards Hunting Points and unlocks a new set of targets.\n\n" +
				"Hunting Points can be exchanged for rewards in the Hunting Shop.\n\n" +
				"If you don't like the available targets, you can reroll the list before accepting a task."
			),
			Option(L("Back"), "back")
		);

		await ShowMainMenu(dialog);
	}

	private static string GetLocations(HuntingTaskMonster monster)
	{
		if (monster == null)
			return "Unknown Location";

		if (monster.MapNames != null && monster.MapNames.Length > 0)
			return string.Join(", ", monster.MapNames);

		if (!string.IsNullOrWhiteSpace(monster.MapName))
			return monster.MapName;

		return "Unknown Location";
	}
}

public class HuntingTaskTrackerQuest : QuestScript
{
	public const string QuestNamespace = "hunting_tasks";
	public const int QuestNumber = 1001;

	public static QuestId TrackerQuestId => new(QuestNamespace, QuestNumber);

	protected override void Load()
	{
		this.SetId(QuestNamespace, QuestNumber);
		this.SetName(L("Hunting Task"));
		this.SetType(QuestType.Repeat);
		this.SetDescription(L("Defeat the monster assigned by [Hunting Task Master] Bruno."));
		this.SetLocation("c_Klaipe");
		this.SetAutoTracked(true);
		this.SetReceive(QuestReceiveType.Manual);
		this.SetCancelable(false);
		this.SetUnlock(QuestUnlockType.AllAtOnce);
		this.AddQuestGiver(L("[Hunting Task Master] Bruno"), "c_Klaipe");

		this.AddObjective(
			"huntingTaskMonster",
			L("Defeat the assigned monster"),
			new HuntingTaskObjective());
	}
}
