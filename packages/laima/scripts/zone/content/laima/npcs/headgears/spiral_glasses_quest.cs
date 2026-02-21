using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class SpiralGlassesQuestScript : QuestScript
{
	private const int QuestNumber = 10003;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Crystal Clear Vision";
	private const string QuestDescription = "Help the miner craft special glasses using minerals from Crystal Mines.";

	private const int VubbeTokenId = 645129;
	private const int StoneOrcaCoreId = 645130;
	private const int SpiralGlassesId = 628315;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_tokens", "Collect 50 Vubbe Tokens", new CollectItemObjective(VubbeTokenId, 50));
		AddObjective("collect_cores", "Collect 200 Stone Orca Cores", new CollectItemObjective(StoneOrcaCoreId, 200));
		AddReward(new ItemReward(SpiralGlassesId, 1));

		AddNpc(20157, L("[Crystal Miner] Gareth"), "f_siauliai_out", -156, -129, 45, MinerDialog);
	}

	private async Task MinerDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Gareth"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("Oh, are you an adventurer...?"));
			await dialog.Msg(L("I've been working on a design for special glasses that can help spot mineral deposits. With the right materials from the mines, I could craft them."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you collected the materials from the mines?"),
				Option(L("Here are the materials"), "complete", () => player.Inventory.HasItem(VubbeTokenId, 50) && player.Inventory.HasItem(StoneOrcaCoreId, 200)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(VubbeTokenId, 50) &&
					player.Inventory.HasItem(StoneOrcaCoreId, 200))
				{
					player.Inventory.RemoveItem(VubbeTokenId, 50);
					player.Inventory.RemoveItem(StoneOrcaCoreId, 200);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("Perfect! These will work wonderfully. Here are your Spiral Glasses - they'll help you spot treasures others might miss!"));

					// Since quest is repeatable, offer it again
					await dialog.Msg(L("Say... I could always craft another pair if you're interested in gathering more materials."));
					await StartNewQuest(dialog, player);
				}
				else
				{
					await dialog.Msg(L("Hmm, looks like you're missing some materials. I need 50 Vubbe Tokens and 200 Stone Orca Cores to craft the glasses."));
				}
				return;
			}

			await dialog.Msg(L("Take your time in the mines, but be careful down there."));
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials for the Spiral Glasses?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Excellent! The Vubbe Tokens have unique refractive properties, and the Stone Orca Cores can be ground into special lenses. Come back when you have them all."));
		}
		else
		{
			await dialog.Msg(L("Well, if you change your mind, you know where to find me. These old glasses won't last much longer..."));
		}
	}
}
