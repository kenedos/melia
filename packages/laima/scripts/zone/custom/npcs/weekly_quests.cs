//--- Melia Script ----------------------------------------------------------
// Weekly Quest NPC
//---------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests.Daily;
using Melia.Zone.World.Quests.Weekly;
using static Melia.Zone.Scripting.Shortcuts;

public class WeeklyQuestNpcScript : GeneralScript
{
	private readonly WeeklyQuestManager manager = new();

	protected override void Load()
	{
		AddNpc(57223, L("[Weekly Quest Manager] Joao"), "Joao", "c_Klaipe", -700, 890, 0, async dialog =>
		{
			dialog.SetTitle(L("Weekly Quest Manager"));
			await this.manager.ResetIfNewWeek(dialog.Player);

			if (!this.manager.IsActive(dialog.Player))
				await this.ShowIntroduction(dialog);
			else
			{
				await this.manager.StartQuest(dialog.Player);
				await this.ShowMainMenu(dialog);
			}
		});
	}

	private async Task ShowIntroduction(Dialog dialog)
	{
		var response = await dialog.Select(
			L("Welcome to the Weekly Quest System.{nl}{nl}You will receive 29 objectives that remain active until the next weekly cycle.{nl}Each completed objective rewards 4 Vaivora Coins.{nl}{nl}Weekly quests cannot be manually reset."),
			Option(L("Accept Weekly Quests"), "accept"),
			Option(L("Exit"), "exit"));

		if (response != "accept")
			return;

		if (!await this.manager.Activate(dialog.Player))
		{
			await dialog.Msg(L("Weekly Quests could not be activated."));
			return;
		}

		await dialog.Msg(L("Your 29 Weekly Quests have been generated."));
		await this.ShowMainMenu(dialog);
	}

	private async Task ShowMainMenu(Dialog dialog)
	{
		var progress = this.manager.Load(dialog.Player);
		var completed = progress.Quests.Count(quest => quest.Completed);
		var claimed = progress.Quests.Count(quest => quest.RewardClaimed);
		var message = "Weekly Quest progress: " + completed + "/29{nl}Rewards claimed: " + claimed + "/29{nl}{nl}Select a category.";
		var response = await dialog.Select(
			L(message),
			Option(L("General Objectives"), "general"),
			Option(L("Monster Races"), "races"),
			Option(L("Monster Attributes"), "attributes"),
			Option(L("Monster Sizes"), "sizes"),
			Option(L("Regional Objectives"), "maps"),
			Option(L("Weekly Reset Information"), "reset_info"),
			Option(L("Exit"), "exit"));

		switch (response)
		{
			case "general": await this.ShowQuestList(dialog, WeeklyQuestType.Monster, WeeklyQuestType.Elite, WeeklyQuestType.Boss, WeeklyQuestType.Mythic, WeeklyQuestType.Dungeon); break;
			case "races": await this.ShowQuestList(dialog, WeeklyQuestType.Race); break;
			case "attributes": await this.ShowQuestList(dialog, WeeklyQuestType.Attribute); break;
			case "sizes": await this.ShowQuestList(dialog, WeeklyQuestType.Size); break;
			case "maps": await this.ShowQuestList(dialog, WeeklyQuestType.Map); break;
			case "reset_info":
				await dialog.Msg(L("Weekly Quests reset automatically every Monday at 3:00 AM Brasilia time.{nl}{nl}They cannot be manually reset. Unclaimed rewards are lost when a new weekly cycle begins."));
				await this.ShowMainMenu(dialog);
				break;
		}
	}

	private async Task ShowQuestList(Dialog dialog, params WeeklyQuestType[] types)
	{
		var quests = this.manager.Load(dialog.Player).Quests.Where(quest => types.Contains(quest.Type)).ToArray();
		var options = quests.Select(quest => Option(L(GetQuestOptionText(quest)), quest.Key))
			.Concat(new[] { Option(L("Back"), "back"), Option(L("Exit"), "exit") })
			.ToArray();
		var response = await dialog.Select(L("Select a Weekly Quest."), options);

		if (response == "back")
		{
			await this.ShowMainMenu(dialog);
			return;
		}

		if (response == "exit")
			return;

		var selected = quests.FirstOrDefault(quest => quest.Key == response);
		if (selected != null)
			await this.ShowQuestDetails(dialog, selected);
	}

	private async Task ShowQuestDetails(Dialog dialog, WeeklyQuestDefinition quest)
	{
		var message = GetQuestDisplayName(quest) + "{nl}{nl}Progress: " + quest.Progress + "/" + quest.Target + "{nl}Status: " + GetQuestStatus(quest) + "{nl}Reward: " + quest.Reward + " Vaivora Coin(s)";
		if (quest.Completed && !quest.RewardClaimed)
		{
			var response = await dialog.Select(L(message), Option(L("Claim Reward"), "claim"), Option(L("Back"), "back"), Option(L("Exit"), "exit"));
			if (response == "claim")
			{
				if (this.manager.ClaimReward(dialog.Player, quest.Key))
					await dialog.Msg(L("Weekly Quest completed!{nl}{nl}You received 4 Vaivora Coins."));
				else
					await dialog.Msg(L("The Weekly Quest reward could not be claimed."));
				await this.ShowMainMenu(dialog);
			}
			else if (response == "back")
				await this.ShowQuestList(dialog, quest.Type);
			return;
		}

		var normalResponse = await dialog.Select(L(message), Option(L("Back"), "back"), Option(L("Exit"), "exit"));
		if (normalResponse == "back")
			await this.ShowQuestList(dialog, quest.Type);
	}

	private static string GetQuestOptionText(WeeklyQuestDefinition quest)
	{
		var status = quest.RewardClaimed ? "[Claimed]" : quest.Completed ? "[Claim Reward]" : "[" + quest.Progress + "/" + quest.Target + "]";
		return GetQuestDisplayName(quest) + " " + status;
	}

	private static string GetQuestStatus(WeeklyQuestDefinition quest)
	{
		if (quest.RewardClaimed) return "Reward claimed";
		if (quest.Completed) return "Completed - Reward available";
		return "In progress";
	}

	private static string GetQuestDisplayName(WeeklyQuestDefinition quest)
	{
		return quest.Type switch
		{
			WeeklyQuestType.Monster => "Defeat Monsters",
			WeeklyQuestType.Elite => "Defeat Elite Monsters",
			WeeklyQuestType.Boss => "Defeat Boss Monsters",
			WeeklyQuestType.Mythic => "Defeat Mythic Monsters",
			WeeklyQuestType.Dungeon => "Complete Dungeons",
			WeeklyQuestType.Map => "Defeat monsters in " + DailyQuestDisplayNames.GetMapDisplayName(quest.TargetMapClassName),
			WeeklyQuestType.Race => "Defeat " + DailyQuestDisplayNames.GetRaceDisplayName(quest.TargetRace) + " monsters",
			WeeklyQuestType.Attribute => "Defeat " + DailyQuestDisplayNames.GetAttributeDisplayName(quest.TargetAttribute) + " monsters",
			WeeklyQuestType.Size => "Defeat " + DailyQuestDisplayNames.GetSizeDisplayName(quest.TargetSize) + " monsters",
			_ => quest.Type.ToString(),
		};
	}
}
