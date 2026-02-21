using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class SquareBlackHatQuestScript : QuestScript
{
	private const int QuestNumber = 10014;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Scholar's Pride";
	private const string QuestDescription = "Help the scholarly Tiny collect materials for a proper academic's hat.";

	private const int TinyBrownBoneId = 645683;
	private const int TinyBrownHornId = 645684;
	private const int SquareBlackHatId = 628082;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_bones", "Collect 120 Brown Tini Bones", new CollectItemObjective(TinyBrownBoneId, 120));
		AddObjective("collect_horns", "Collect 2 Brown Tini Horns", new CollectItemObjective(TinyBrownHornId, 2));
		AddReward(new ItemReward(SquareBlackHatId, 1));

		AddNpc(147421, L("[Scholar] Tini"), "f_pilgrimroad_49", 347, -822, 45, TiniDialog);
	}

	private async Task TiniDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Professor Tini"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*adjusts spectacles while reading* Hmm... according to my research, the traditional academic's hat requires very specific materials..."));
			await dialog.Msg(L("*looks up from book* Oh! Pardon me. Would you be interested in helping me craft a proper scholar's hat? It's quite fascinating really..."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you collected the specimens for our academic endeavor?"),
				Option(L("Yes, Professor"), "complete", () => player.Inventory.HasItem(TinyBrownBoneId, 120) && player.Inventory.HasItem(TinyBrownHornId, 2)),
				Option(L("Still researching"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(TinyBrownBoneId, 120) &&
					player.Inventory.HasItem(TinyBrownHornId, 2))
				{
					player.Inventory.RemoveItem(TinyBrownBoneId, 120);
					player.Inventory.RemoveItem(TinyBrownHornId, 2);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*examines materials thoroughly* Yes, yes! These specimens are perfect! Here's your Square Black Hat - quite distinguished, if I do say so myself."));

					await dialog.Msg(L("*returns to books* You know, I have several theories about alternative material compositions... Interested in testing another hypothesis?"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*peers over glasses* Take all the time you need. Proper research cannot be rushed."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Shall we proceed with this scholarly pursuit?"),
			Option(L("Yes, Professor"), "accept"),
			Option(L("Another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*straightens papers* Excellent! I require 120 Brown Tini Bones for the structure and 2 Brown Tini Horns for the embellishments. The proportions must be exact!"));
		}
		else
		{
			await dialog.Msg(L("*returns to reading* Very well. The pursuit of knowledge waits for those who are ready."));
		}
	}
}
