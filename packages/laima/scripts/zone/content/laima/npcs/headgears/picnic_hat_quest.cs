using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class PicnicHatQuestScript : QuestScript
{
	private const int QuestNumber = 10042;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "A Perfect Spring Day";
	private const string QuestDescription = "Help Mila prepare for a special picnic by gathering Siaulamb Masks and Dilgele flowers.";

	private const int SiaulambMaskId = 645596;
	private const int DilgeleId = 640038;
	private const int PicnicHatId = 628301;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_masks", "Collect 50 Siaulamb Masks", new CollectItemObjective(SiaulambMaskId, 50));
		AddObjective("collect_flowers", "Collect 30 Dilgele", new CollectItemObjective(DilgeleId, 30));
		AddReward(new ItemReward(PicnicHatId, 1));

		AddNpc(147473, L("[Little Girl] Mila"), "f_siauliai_46_1", 1918, 871, 0, MilaDialog);
	}

	private async Task MilaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Mila"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*A young girl sits on a blanket, looking up at the sky*{/}"));
			await dialog.Msg(L("Oh! Hello there, mister! I'm waiting for my mommy. She said we could have a picnic today when the flowers are blooming!"));
			await dialog.Msg(L("But I lost my favorite hat somewhere in the woods... Mommy made it for me. It was so pretty with little flowers on it!"));
			await dialog.Msg(L("Maybe if I had enough pretty things, I could make a new one? The Siaulavs around here have such colorful masks, and there are Dilgele flowers growing everywhere!"));
		}
		else
		{
			var selection = await dialog.Select(L("Did you find the things I need for my new picnic hat?"),
				Option(L("Yes, here they are!"), "complete", () => player.Inventory.HasItem(SiaulambMaskId, 50) && player.Inventory.HasItem(DilgeleId, 30)),
				Option(L("Still looking"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(SiaulambMaskId, 50) &&
					player.Inventory.HasItem(DilgeleId, 30))
				{
					player.Inventory.RemoveItem(SiaulambMaskId, 50);
					player.Inventory.RemoveItem(DilgeleId, 30);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*Mila's eyes light up with joy*{/}"));
					await dialog.Msg(L("Wow! These are so pretty! Look at all the colors!"));
					await dialog.Msg(L("{#666666}*She carefully arranges the materials into a charming hat*{/}"));
					await dialog.Msg(L("Here, you should have this one! I can always make another. Thank you so much, mister! Now my picnic with mommy will be perfect!"));
				}
			}
			else
			{
				await dialog.Msg(L("That's okay! The Siaulavs like to play near the big trees over there. And the Dilgele flowers grow all over these woods. Good luck!"));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Will you help me find pretty things for my new picnic hat?"),
			Option(L("Of course!"), "accept"),
			Option(L("Not right now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Really?! Thank you so much! I need 50 of those colorful Siaulamb Masks from the Siaulavs, and 30 Dilgele flowers! Then I can make the prettiest hat ever!"));
				break;
			case "decline":
				await dialog.Msg(L("Oh... okay. Maybe someone else will help. I really want my picnic hat..."));
				break;
			default:
				break;
		}
	}
}
