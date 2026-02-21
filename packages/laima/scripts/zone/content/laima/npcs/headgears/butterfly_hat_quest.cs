using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class ButterflyHatQuestScript : QuestScript
{
	private const int QuestNumber = 10006;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Beautiful Wings";
	private const string QuestDescription = "Help Grandma Nella craft a decorative Butterfly Hat from Leafly Wings and Dilgele flowers.";

	private const int LeaflyWingId = 645300;
	private const int DilgeleId = 640038;
	private const int ButterflyHatId = 628152;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_wings", "Collect 150 Leafly Wings", new CollectItemObjective(LeaflyWingId, 150));
		AddObjective("collect_flowers", "Collect 25 Dilgele", new CollectItemObjective(DilgeleId, 25));
		AddReward(new ItemReward(ButterflyHatId, 1));

		AddNpc(147489, L("[Grandma] Nella"), "f_gele_57_2", -1182, -463, 90, NellaDialog);
	}

	private async Task NellaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Nella"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("Oh my, what a lovely day to watch the butterflies! *adjusts her shawl* They remind me of my younger days, dancing in these fields..."));
			await dialog.Msg(L("You know, I've been working on a special hat design. Something to capture their grace and beauty. Would you help this old lady gather some materials?"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials for the Butterfly Hat?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(LeaflyWingId, 150) && player.Inventory.HasItem(DilgeleId, 25)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(LeaflyWingId, 150) &&
					player.Inventory.HasItem(DilgeleId, 25))
				{
					player.Inventory.RemoveItem(LeaflyWingId, 150);
					player.Inventory.RemoveItem(DilgeleId, 25);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("Oh, these are perfect! *carefully crafts the hat* Here you are, dear. Now you can carry a piece of these beautiful butterflies with you wherever you go."));
				}
			}
			else
			{
				await dialog.Msg(L("Take your time, dear. The butterflies will always be here in the plateau."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather materials for the Butterfly Hat?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Thank you, dear! Bring me 150 Leafly Wings for the delicate fabric, and 25 Dilgele flowers for the decoration. Together we'll make something truly special!"));
				break;
			case "decline":
				await dialog.Msg(L("Oh, that's alright dear. Perhaps another time. I'll be here, watching my butterflies."));
				break;
			default:
				break;
		}
	}
}
