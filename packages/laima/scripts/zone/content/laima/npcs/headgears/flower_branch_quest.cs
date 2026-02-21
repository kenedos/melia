using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FlowerBranchQuestScript : QuestScript
{
	private const int QuestNumber = 10015;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Nature's Dance";
	private const string QuestDescription = "Help Florina create a beautiful Flower Branch hair ornament using materials from Bracken Forest.";

	private const int LeafnutHornId = 645738;
	private const int PolibuLeafId = 645737;
	private const int ParrotStems = 645736;
	private const int FlowerBranchId = 628093;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_horns", "Collect 25 Leafnut Horns", new CollectItemObjective(LeafnutHornId, 25));
		AddObjective("collect_leaves", "Collect 40 Polibu Leaves", new CollectItemObjective(PolibuLeafId, 40));
		AddObjective("collect_feathers", "Collect 15 Parrot Stems", new CollectItemObjective(ParrotStems, 15));
		AddReward(new ItemReward(FlowerBranchId, 1));

		AddNpc(151040, L("[Forest Artist] Florina"), "f_bracken_63_1", 322, -646, 0, FlorinaDialog);
	}

	private async Task FlorinaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Florina"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*arranging leaves and flowers* Nature creates the most beautiful patterns... if only we could capture their essence in our accessories!"));
			await dialog.Msg(L("These forest materials could be perfect for a lovely hair ornament. Would you help me gather some special pieces?"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the forest treasures?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(LeafnutHornId, 25) && player.Inventory.HasItem(PolibuLeafId, 40) && player.Inventory.HasItem(ParrotStems, 15)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(LeafnutHornId, 25) &&
					player.Inventory.HasItem(PolibuLeafId, 40) &&
					player.Inventory.HasItem(ParrotStems, 15))
				{
					player.Inventory.RemoveItem(LeafnutHornId, 25);
					player.Inventory.RemoveItem(PolibuLeafId, 40);
					player.Inventory.RemoveItem(ParrotStems, 15);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*carefully weaves the materials together* Beautiful! The curve of the Leafnut horns provides the perfect foundation, while the Polibu leaves add texture, and the Parrot stems give it that delicate finishing touch."));

					// Offer repeatable quest
					await dialog.Msg(L("*admires her work* There are endless possibilities with nature's treasures. Would you like to help me create another one?"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("Take your time gathering the materials. Art cannot be rushed, and nature's gifts are worth the wait."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials for a Flower Branch hair ornament?"),
			Option(L("I'll help gather"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Wonderful! The Leafnut horns will form the base structure, Polibu leaves will add a natural texture, and Parrot stems will give it that special ethereal quality. Happy gathering!"));
		}
		else
		{
			await dialog.Msg(L("*returns to arranging flowers* Nature's beauty will still be here when you return..."));
		}
	}
}
