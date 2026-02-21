using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class AncientCrownQuestScript : QuestScript
{
	private const int QuestNumber = 10021;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Lost Crown of the Ancients";
	private const string QuestDescription = "Help the Lizardman Scholar recover materials to recreate an ancient crown worn by his ancestors.";

	private const int LizardmanBonesId = 645583;
	private const int TamaLeafId = 645584;
	private const int AncientCrownId = 628125;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_bones", "Collect 100 Lizardman Bones", new CollectItemObjective(LizardmanBonesId, 100));
		AddObjective("collect_leaves", "Collect 50 Tama Leaves", new CollectItemObjective(TamaLeafId, 50));
		AddReward(new ItemReward(AncientCrownId, 1));

		AddNpc(153217, L("[Scholar] Sssevren"), "f_farm_49_1", 864, 762, 0, ScholarDialog);
	}

	private async Task ScholarDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Sssevren"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("Ah, a visitor! My research into our ancient civilization's ceremonial headpieces is progressing well, but I need more materials..."));
			await dialog.Msg(L("The bones of our warriors carry the strength of our ancestors, and the sacred Tama leaves were used in all our ceremonies. With these, I can recreate a crown of great significance!"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials for the ancient crown?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(LizardmanBonesId, 100) && player.Inventory.HasItem(TamaLeafId, 50)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(LizardmanBonesId, 100) &&
					player.Inventory.HasItem(TamaLeafId, 50))
				{
					player.Inventory.RemoveItem(LizardmanBonesId, 100);
					player.Inventory.RemoveItem(TamaLeafId, 50);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*eyes gleaming with scholarly excitement* Yesss! These are perfect specimens! Watch as I craft... *carefully assembles the crown* Here, take this recreation of our ancient crown. May it serve you as well as it served my ancestors."));

					await dialog.Msg(L("*adjusts spectacles again* You know... I could always craft another if you bring more materials. There's still so much to learn about the different styles and variations..."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*scratches notes on parchment* Remember, we need 100 bones from our warrior ancestors and 50 sacred Tama leaves. The proportions must be exact for historical accuracy!"));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials to recreate another ancient crown?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*excitedly flips through research notes* Excellent! Remember, we need the bones of our warriors - they must be from true Lizardman warriors, not just any bones - and the sacred Tama leaves. Both are essential for historical accuracy!"));
		}
		else
		{
			await dialog.Msg(L("*returns to studying scrolls* Very well... The secrets of our ancient crowns will wait. They have waited for centuries, after all..."));
		}
	}
}
