using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FlowerCrownQuestScript : QuestScript
{
	private const int QuestNumber = 10018;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Nature's Crown";
	private const string QuestDescription = "Help Flora create a beautiful flower crown using petals and leaves from the forest.";

	private const int CaroLeafId = 645451;
	private const int UpentBarkId = 645156;
	private const int FlowerCrownId = 628151;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_petals", "Collect 50 Caro Leaves", new CollectItemObjective(CaroLeafId, 50));
		AddObjective("collect_bark", "Collect 25 Upent Bark", new CollectItemObjective(UpentBarkId, 25));
		AddReward(new ItemReward(FlowerCrownId, 1));

		AddNpc(152001, L("[Florist] Flora"), "f_huevillage_58_3", 1290, -863, 180, FloraDialog);
	}

	private async Task FloraDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Flora"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*arranging flowers delicately* Each petal tells a story, each leaf holds a memory... *smiles warmly* This old village provides us with such beautiful materials to work with."));
			await dialog.Msg(L("I've been working on a special kind of crown, one that captures the essence of our beautiful places. Would you help me gather some materials?"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials for our flower crown?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(CaroLeafId, 50) && player.Inventory.HasItem(UpentBarkId, 25)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(CaroLeafId, 50) &&
					player.Inventory.HasItem(UpentBarkId, 25))
				{
					player.Inventory.RemoveItem(CaroLeafId, 50);
					player.Inventory.RemoveItem(UpentBarkId, 25);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*weaves the materials with practiced hands* Such lovely colors! Here, try it on. The crown suits you perfectly! The forest has blessed us with its beauty once again."));

					await dialog.Msg(L("*eyes sparkle* You know... I could always create another one if you bring me more materials. Each crown turns out unique, just like the flowers themselves!"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*continues arranging flowers* Take your time gathering the materials. The forest's gifts are meant to be appreciated, not rushed."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you like to help me create another flower crown?"),
			Option(L("I'll help gather materials"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*claps excitedly* Wonderful! The Caro leaves have such a lovely shape, perfect for the crown's structure. And the Upent bark, when treated properly, creates beautiful accents. Happy gathering!"));
		}
		else
		{
			await dialog.Msg(L("*nods understandingly* Of course, of course. The flowers will still be here when you return. They always are."));
		}
	}
}
