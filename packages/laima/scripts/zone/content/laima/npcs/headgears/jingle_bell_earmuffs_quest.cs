using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class JingleBellEarmuffsQuestScript : QuestScript
{
	private const int QuestNumber = 10046;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Winter's Melody";
	private const string QuestDescription = "Help the traveling merchant Mirena craft festive Jingle Bell Earmuffs using materials from the frozen exile.";

	private const int WhiteSpionFurId = 645677;
	private const int BrownLapasapeLeavesId = 645678;
	private const int JingleBellEarmuffsId = 628363;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_fur", "Collect 100 White Spion Fur", new CollectItemObjective(WhiteSpionFurId, 100));
		AddObjective("collect_leaves", "Collect 75 Brown Lapasape Leaves", new CollectItemObjective(BrownLapasapeLeavesId, 75));
		AddReward(new ItemReward(JingleBellEarmuffsId, 1));

		AddNpc(20104, L("[Traveling Merchant] Mirena"), "f_tableland_72", -449, 2, 0, MirenaDialog);
	}

	private async Task MirenaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Mirena"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*shivers beside her small merchant cart, rubbing her hands together for warmth*{/}"));
			await dialog.Msg(L("Brr! The cold in this exile is far worse than I expected. I came here hoping to gather materials for my winter crafts, but I underestimated just how bitter this frozen wasteland would be."));
			await dialog.Msg(L("I specialize in festive accessories, you see. There's a particular design I've been working on - earmuffs adorned with tiny bells that chime softly in the winter wind. But I need materials from the local creatures to finish them..."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials I need?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(WhiteSpionFurId, 100) && player.Inventory.HasItem(BrownLapasapeLeavesId, 75)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(WhiteSpionFurId, 100) &&
					player.Inventory.HasItem(BrownLapasapeLeavesId, 75))
				{
					player.Inventory.RemoveItem(WhiteSpionFurId, 100);
					player.Inventory.RemoveItem(BrownLapasapeLeavesId, 75);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*her eyes light up as she examines the materials*{/}"));
					await dialog.Msg(L("Perfect! The White Spion fur is wonderfully soft and insulating - just what you need to keep your ears warm in this bitter cold. And these Lapasape leaves have just the right flexibility to bind the jingle bells securely."));
					await dialog.Msg(L("{#666666}*works quickly with nimble fingers, humming a festive tune*{/}"));
					await dialog.Msg(L("There we go! Listen to that gentle chime. These Jingle Bell Earmuffs will keep you warm and add a touch of winter magic to your adventures. May they bring you joy in even the coldest of places!"));

					await dialog.Msg(L("If you'd like another pair, perhaps as a gift for a friend, I'd be happy to make more."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("The White Spions wander all across this frozen landscape - they're the large white spider-like creatures. Be careful, their icy touch can be quite dangerous. The Brown Lapasapes are smaller plant creatures that blend in with the dead foliage."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials for a pair of Jingle Bell Earmuffs?"),
			Option(L("I'll help"), "accept"),
			Option(L("Perhaps another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Wonderful! I need 100 White Spion Fur for the cozy lining, and 75 Brown Lapasape Leaves to weave the frame that holds the bells. The Spions are everywhere in this exile, and the Lapasapes tend to gather near the vegetation. Stay warm out there!"));
		}
		else
		{
			await dialog.Msg(L("{#666666}*pulls her cloak tighter against the wind*{/} I understand. This cold isn't for everyone. If you change your mind, I'll be here... trying not to freeze."));
		}
	}
}
