//--- Melia Script ----------------------------------------------------------
// Daily Quest NPC
//--- Description -----------------------------------------------------------
// Handles daily quest difficulty selection, progress display, and rewards.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests.Daily;
using static Melia.Zone.Scripting.Shortcuts;

public class DailyQuestNpcScript : GeneralScript
{
	private readonly DailyQuestManager manager = new();

	protected override void Load()
	{
		AddNpc(57223, L("[Daily Quest Manager] Vitor"), "Vitor", "c_Klaipe", -590, 890, 0, async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(
					L("Daily Quest Manager"));

				this.manager.ResetIfNewDay(character);

				if (!this.manager.HasChosenDifficulty(character))
				{
					await this.ShowDifficultySelection(dialog);

					return;
				}

				await this.ShowQuestMenu(dialog);
			});
	}

	private async Task DailyQuestDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Daily Quests"));

		if (!this.manager.HasChosenDifficulty(character))
		{
			await dialog.Msg(L("Welcome to the Daily Quest System.{nl}" + "Choose the difficulty for today's quests."));
			return;
		}

		var progress = this.manager.Load(character);

		var completedQuestCount = GetCompletedQuestCount(progress);

		var message = "Your daily quests are active.{nl}" + "Difficulty: " + progress.Difficulty + "{nl}" + "Completed quests: " + completedQuestCount + "/" + progress.Quests.Count;

		await dialog.Msg(L(message));
	}

	private async Task ShowDifficultySelection(
		Dialog dialog)
	{
		var response = await dialog.Select(
			L(
				"Welcome to the Daily Quest System.{nl}{nl}" +
				"Choose today's difficulty."
			),
			Option(
				L("Easy"),
				"easy"),
			Option(
				L("Medium"),
				"medium"),
			Option(
				L("Hard"),
				"hard"),
			Option(
				L("Cancel"),
				"cancel"));

		if (response == "cancel")
			return;

		DailyQuestDifficulty difficulty;

		switch (response)
		{
			case "easy":
				difficulty =
					DailyQuestDifficulty.Easy;
				break;

			case "medium":
				difficulty =
					DailyQuestDifficulty.Medium;
				break;

			case "hard":
				difficulty =
					DailyQuestDifficulty.Hard;
				break;

			default:
				return;
		}

		this.manager.ChooseDifficulty(dialog.Player, difficulty);

		await dialog.Msg(
			L(
				"Your daily quests have been generated."
			));

		await this.ShowQuestMenu(
			dialog);
	}

	private async Task ShowQuestMenu(
		Dialog dialog)
	{
		var progress = this.manager.Load(dialog.Player);

		var completedQuestCount = GetCompletedQuestCount(progress);

		if (progress.Quests.Count < 9)
		{
			await dialog.Msg(
				L(
					"Daily Quest data is incomplete.{nl}" +
					"Please contact the server administration."
				));

			return;
		}

		var message =
			"Difficulty: " +
			progress.Difficulty +
			"{nl}" +
			"Completed quests: " +
			completedQuestCount +
			"/" +
			progress.Quests.Count +
			"{nl}{nl}" +
			"Select a daily quest.";

		var response = await dialog.Select(
			L(message),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[0])),
				"quest_0"),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[1])),
				"quest_1"),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[2])),
				"quest_2"),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[3])),
				"quest_3"),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[4])),
				"quest_4"),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[5])),
				"quest_5"),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[6])),
				"quest_6"),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[7])),
				"quest_7"),

			Option(
				L(
					GetQuestOptionText(
						progress.Quests[8])),
				"quest_8"),

			Option(
				L(GetCompletionBonusOptionText(dialog.Player, progress)),
				"completion_bonus"),

			Option(
				L("Reset Information"),
				"reset_information"),

			Option(
				L("Exit"),
				"exit")
		);

		switch (response)
		{
			case "quest_0":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[0]);
				break;

			case "quest_1":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[1]);
				break;

			case "quest_2":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[2]);
				break;

			case "quest_3":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[3]);
				break;

			case "quest_4":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[4]);
				break;

			case "quest_5":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[5]);
				break;

			case "quest_6":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[6]);
				break;

			case "quest_7":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[7]);
				break;

			case "quest_8":
				await this.ShowQuestDetails(
					dialog,
					progress.Quests[8]);
				break;

			case "completion_bonus":
				await this.ClaimCompletionBonus(dialog);
				break;

			case "reset_information":
				await this.ShowResetInformation(
					dialog);
				break;

			default:
				return;
		}
	}

	private async Task ShowQuestDetails(Dialog dialog, DailyQuestDefinition quest)
	{
		var message = GetQuestDisplayName(quest) + "{nl}{nl}" + GetQuestDescription(quest) + "{nl}{nl}" + "Progress: " + quest.Progress + "/" + quest.Target + "{nl}" + "Status: " + GetQuestStatus(quest) + "{nl}" + "Reward: " + quest.Reward + " Vaivora Coin(s)";

		if (quest.Completed && !quest.RewardClaimed)
		{
			var response = await dialog.Select(L(message),
				Option(L("Claim Reward"), "claim"),
				Option(L("Back"), "back"),
				Option(L("Exit"), "exit"));

			switch (response)
			{
				case "claim":
					await this.ClaimQuestReward(dialog, quest.Type);
					break;

				case "back":
					await this.ShowQuestMenu(dialog);
					break;
			}

			return;
		}

		var normalResponse = await dialog.Select(L(message),
			Option(L("Back"), "back"),
			Option(L("Exit"), "exit"));

		if (normalResponse == "back")
			await this.ShowQuestMenu(dialog);
	}

	private async Task ClaimQuestReward(Dialog dialog, DailyQuestType type)
	{
		var character = dialog.Player;
		var quest = this.manager.GetQuest(character, type);

		if (quest == null)
		{
			await dialog.Msg(L("Daily Quest data could not be found."));
			await this.ShowQuestMenu(dialog);
			return;
		}

		if (!quest.Completed)
		{
			await dialog.Msg(L("This Daily Quest has not been completed yet."));
			await this.ShowQuestMenu(dialog);
			return;
		}

		if (quest.RewardClaimed)
		{
			await dialog.Msg(L("You have already claimed the reward for this Daily Quest."));
			await this.ShowQuestMenu(dialog);
			return;
		}

		var reward = quest.Reward;
		var claimed = this.manager.ClaimReward(character, type);

		if (!claimed)
		{
			await dialog.Msg(L("The Daily Quest reward could not be claimed."));
			await this.ShowQuestMenu(dialog);
			return;
		}

		await dialog.Msg(L("Daily Quest completed!{nl}{nl}You received " + reward + " Vaivora Coin(s)."));
		await this.ShowQuestMenu(dialog);
	}

	private string GetCompletionBonusOptionText(Melia.Zone.World.Actors.Characters.Character character, DailyQuestProgress progress)
	{
		var reward = DailyQuestRewards.GetCompletionBonus(progress.Difficulty);

		if (this.manager.HasClaimedCompletionBonus(character))
			return "Daily Completion Bonus [Claimed]";

		if (this.manager.HasClaimedAllQuestRewards(character))
			return "Daily Completion Bonus [Claim " + reward + " Vaivora Coins]";

		return "Daily Completion Bonus [Locked]";
	}

	private async Task ClaimCompletionBonus(Dialog dialog)
	{
		var character = dialog.Player;

		if (this.manager.HasClaimedCompletionBonus(character))
		{
			await dialog.Msg(L("You have already claimed today's Daily Completion Bonus."));
			await this.ShowQuestMenu(dialog);
			return;
		}

		if (!this.manager.HasClaimedAllQuestRewards(character))
		{
			await dialog.Msg(L("You must claim the rewards from all 9 Daily Quests before receiving the Daily Completion Bonus."));
			await this.ShowQuestMenu(dialog);
			return;
		}

		var difficulty = this.manager.GetDifficulty(character);
		var reward = DailyQuestRewards.GetCompletionBonus(difficulty);

		if (reward <= 0)
		{
			await dialog.Msg(L("The Daily Completion Bonus could not be determined."));
			await this.ShowQuestMenu(dialog);
			return;
		}

		character.AddItem(DailyQuestRewards.RewardItemId, reward);
		this.manager.MarkCompletionBonusClaimed(character);

		await dialog.Msg(L("All 9 Daily Quest rewards have been claimed!{nl}{nl}You received the Daily Completion Bonus: " + reward + " Vaivora Coin(s)."));
		await this.ShowQuestMenu(dialog);
	}

	private async Task ShowResetInformation(
	Dialog dialog)
	{
		if (!this.manager.CanManuallyReset(
			dialog.Player))
		{
			await dialog.Msg(L("Today's Daily Quests can no longer be reset.{nl}{nl}One or more Daily Quest rewards have already been claimed.{nl}{nl}New Daily Quests will become available after 3:00 AM Brasília time."));

			await this.ShowQuestMenu(dialog);

			return;
		}

		var response = await dialog.Select(
			L(
				"Daily Quest Reset{nl}{nl}" +
				"Daily quests reset every day at 3:00 AM Brasília time.{nl}{nl}" +
				"You can also reset your quests manually{nl}" +
				"to choose another difficulty.{nl}{nl}" +
				"WARNING:{nl}" +
				"• Current progress will be lost.{nl}" +
				"• Unclaimed rewards will be lost."
			),
			Option(
				L("Reset Daily Quests"),
				"reset"),
			Option(
				L("Back"),
				"back"),
			Option(
				L("Exit"),
				"exit")
		);

		switch (response)
		{
			case "reset":
				await this.ShowResetConfirmation(
					dialog);
				break;

			case "back":
				await this.ShowQuestMenu(
					dialog);
				break;
		}
	}

	private async Task ShowResetConfirmation(
	Dialog dialog)
	{
		if (!this.manager.CanManuallyReset(
			dialog.Player))
		{
			await dialog.Msg(L("Today's Daily Quests can no longer be reset.{nl}{nl}One or more Daily Quest rewards have already been claimed.{nl}{nl}New Daily Quests will become available after 3:00 AM Brasília time."));

			await this.ShowQuestMenu(dialog);

			return;
		}

		var response = await dialog.Select(
			L(
				"Confirm Daily Quest Reset{nl}{nl}" +
				"All current daily quest progress will be permanently erased.{nl}" +
				"Unclaimed rewards will also be lost.{nl}{nl}" +
				"Do you want to continue?"
			),
			Option(
				L("Yes, Reset My Quests"),
				"confirm"),
			Option(
				L("No, Go Back"),
				"back"),
			Option(
				L("Exit"),
				"exit")
		);

		switch (response)
		{
			case "confirm":
			{
				var resetSucceeded =
					await this.manager.ResetDifficultySelection(
						dialog.Player);

				if (!resetSucceeded)
				{
					await dialog.Msg(L("Today's Daily Quests can no longer be reset.{nl}{nl}One or more Daily Quest rewards have already been claimed."));

					await this.ShowQuestMenu(dialog);

					return;
				}

				await dialog.Msg(
					L(
						"Your daily quests have been reset.{nl}" +
						"You may now choose a new difficulty."
					));

				await this.ShowDifficultySelection(
					dialog);

				break;
			}

			case "back":
				await this.ShowResetInformation(
					dialog);
				break;
		}
	}

	private static string GetQuestDisplayName(
		DailyQuestDefinition quest)
	{
		switch (quest.Type)
		{
			case DailyQuestType.Monster:
				return "Defeat Monsters";

			case DailyQuestType.Elite:
				return "Defeat Elite Monsters";

			case DailyQuestType.Boss:
				return "Defeat Boss Monsters";

			case DailyQuestType.Dungeon:
				return "Complete Dungeons";

			case DailyQuestType.Mythic:
				return "Defeat Mythic Monsters";

			case DailyQuestType.Map:
				return
					"Defeat monsters in " +
					GetMapDisplayName(
						quest.TargetMapClassName);

			case DailyQuestType.Race:
				return
					"Defeat " +
					GetMonsterRaceDisplayName(
						quest.TargetRace) +
					" monsters";

			case DailyQuestType.Attribute:
				return
					"Defeat " +
					GetMonsterAttributeDisplayName(
						quest.TargetAttribute) +
					" monsters";

			case DailyQuestType.Size:
				return
					"Defeat " +
					GetMonsterSizeDisplayName(
						quest.TargetSize) +
					" monsters";

			default:
				return quest.Type.ToString();
		}
	}

	private static string GetQuestDescription(
		DailyQuestDefinition quest)
	{
		switch (quest.Type)
		{
			case DailyQuestType.Monster:
				return
					"Defeat regular monsters anywhere in the world.";

			case DailyQuestType.Elite:
				return
					"Defeat elite monsters during your adventures.";

			case DailyQuestType.Boss:
				return
					"Defeat boss monsters.";

			case DailyQuestType.Dungeon:
				return
					"Complete dungeon activities.";

			case DailyQuestType.Mythic:
				return
					"Defeat powerful Mythic monsters during your adventures.";

			case DailyQuestType.Map:
				return
					"Defeat monsters in " +
					GetMapDisplayName(
						quest.TargetMapClassName) +
					".";

			case DailyQuestType.Race:
				return
					"Defeat monsters of the " +
					GetMonsterRaceDisplayName(
						quest.TargetRace) +
					" race.";

			case DailyQuestType.Attribute:
				return
					"Defeat monsters with the " +
					GetMonsterAttributeDisplayName(
						quest.TargetAttribute) +
					" attribute.";

			case DailyQuestType.Size:
				return
					"Defeat monsters classified as " +
					GetMonsterSizeDisplayName(
						quest.TargetSize) +
					".";

			default:
				return
					"Complete the requested daily objective.";
		}
	}

	private static string GetQuestOptionText(
		DailyQuestDefinition quest)
	{
		string status;

		if (quest.RewardClaimed)
		{
			status = "[Claimed]";
		}
		else if (quest.Completed)
		{
			status = "[Claim Reward]";
		}
		else
		{
			status =
				"[" +
				quest.Progress +
				"/" +
				quest.Target +
				"]";
		}

		return
			GetQuestDisplayName(
				quest) +
			" " +
			status;
	}

	private static string GetQuestStatus(
		DailyQuestDefinition quest)
	{
		if (quest.RewardClaimed)
			return "Reward claimed";

		if (quest.Completed)
			return "Completed - Reward available";

		return "In progress";
	}

	private static int GetCompletedQuestCount(
		DailyQuestProgress progress)
	{
		var completed = 0;

		foreach (var quest in progress.Quests)
		{
			if (quest.Completed)
				completed++;
		}

		return completed;
	}

	private static string GetMonsterRaceDisplayName(
	int race)
	{
		return DailyQuestDisplayNames
			.GetRaceDisplayName(
				race);
	}

	private static string GetMonsterAttributeDisplayName(
	int attribute)
	{
		return DailyQuestDisplayNames
			.GetAttributeDisplayName(
				attribute);
	}

	private static string GetMonsterSizeDisplayName(
	int size)
	{
		return DailyQuestDisplayNames
			.GetSizeDisplayName(
				size);
	}

	private static string GetMapDisplayName(
	string mapClassName)
	{
		return DailyQuestDisplayNames
			.GetMapDisplayName(
				mapClassName);
	}

	private static string FormatInternalName(
		string value)
	{
		if (string.IsNullOrWhiteSpace(
			value))
		{
			return "Unknown";
		}

		var normalized =
			value
				.Trim()
				.Replace("_", " ")
				.Replace("-", " ");

		var words =
			normalized.Split(
				' ',
				System.StringSplitOptions.RemoveEmptyEntries);

		for (var i = 0; i < words.Length; i++)
		{
			if (words[i].Length == 1)
			{
				words[i] =
					words[i].ToUpperInvariant();

				continue;
			}

			words[i] =
				char.ToUpperInvariant(
					words[i][0]) +
				words[i]
					.Substring(1)
					.ToLowerInvariant();
		}

		return string.Join(
			" ",
			words);
	}
}
