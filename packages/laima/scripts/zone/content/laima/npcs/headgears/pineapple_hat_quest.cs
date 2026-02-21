using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class PineappleHatQuestScript : QuestScript
{
	private const int QuestNumber = 10019;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Tropical Dreams";
	private const string QuestDescription = "Help Pierre create a unique pineapple-shaped hat using materials from fallen birds and warrior armor.";

	private const int CockatriceFeatherId = 645455;
	private const int HogmaWarriorBoneId = 645364;
	private const int PineappleHatId = 628086;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_feathers", "Collect 75 Cockatrice Feathers", new CollectItemObjective(CockatriceFeatherId, 75));
		AddObjective("collect_bones", "Collect 2 Gigantic Shinbones", new CollectItemObjective(HogmaWarriorBoneId, 2));
		AddReward(new ItemReward(PineappleHatId, 1));

		AddNpc(161003, L("[Eccentric Designer] Pierre"), "f_rokas_24", 879, -595, 90, PierreDialog);
	}

	private async Task PierreDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Pierre"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*sketching frantically* FA-BU-LOUS! *looks up suddenly* Oh! You there! Have you see how absolutely stunning these cockatrices are~~!? Ohh~ The colors, the texture~! Mmm~! Simply D-E-L-I-C-I-O-U-S."));
			await dialog.Msg(L("*licking lips* Yes! I'm working on something revolutionary - a hat shaped like a pineapple! It will be my ultimate creation~! But I need special materials... Aaah the pain... The texture must be just right, you see? Say, will you help make fashion history?"));
		}
		else
		{
			var selection = await dialog.Select(L("*bouncing with anticipation* Do you have the materials for my masterpiece?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(CockatriceFeatherId, 75) && player.Inventory.HasItem(HogmaWarriorBoneId, 2)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(CockatriceFeatherId, 75) &&
					player.Inventory.HasItem(HogmaWarriorBoneId, 2))
				{
					player.Inventory.RemoveItem(CockatriceFeatherId, 75);
					player.Inventory.RemoveItem(HogmaWarriorBoneId, 2);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*works with manic energy* YES! The feathers create the perfect spiky texture! And the bones... *crafts furiously* ...give it the proper structure! BEHOLD, FASHION INNOVATION!"));

					await dialog.Msg(L("*adjusts glasses rapidly* You know... I have MORE ideas! So many ideas! Would you help me create another masterpiece? Each one is unique - a testament to the artistic spirit!"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*returns to sketching* Take your time~!, take your t-i-m-e! But not too much time - fashion waits for no one! *giggles manically*"));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Shall we create a magnificent piece of wearable art~?"),
			Option(L("Yes, let's make fashion history!"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*jumps excitedly* MAGNIFICENT! The feathers from those Cockatrices - perfect for the spiky texture! And the Hogma warrior bones - they'll give it just the right structure! Now go, GO! Art awaits!"));
		}
		else
		{
			await dialog.Msg(L("*sighs dramatically* Alas, another visionary masterpiece must wait... But fashion never sleeps! Neither do I! *returns to furious sketching*"));
		}
	}
}
