using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FeatherHelmetQuestScript : QuestScript
{
	private const int QuestNumber = 10022;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Warrior's Pride";
	private const string QuestDescription = "Help a former warrior craft a feather helmet worthy of battle.";

	private const int EllomBellId = 645438;
	private const int CronewtLeatherId = 645459;
	private const int KepoFluffId = 645552;
	private const int FeatherHelmetId = 628070;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_bells", "Collect 40 Ellom Bells", new CollectItemObjective(EllomBellId, 40));
		AddObjective("collect_leather", "Collect 25 Cronewt Leather", new CollectItemObjective(CronewtLeatherId, 25));
		AddObjective("collect_fluff", "Collect 60 Kepo Fluff", new CollectItemObjective(KepoFluffId, 60));
		AddReward(new ItemReward(FeatherHelmetId, 1));

		AddNpc(154032, L("[Retired Warrior] Helena"), "f_farm_47_3", -1498, 867, 0, HelenaDialog);
	}

	private async Task HelenaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Helena"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*polishing an old helmet* In my days as a warrior, we made our helmets from the finest materials. The bells of the Ellom for strength, Cronewt leather for durability, and Kepo fluff for comfort."));
			await dialog.Msg(L("These young warriors today with their plain iron helmets... *shakes head* They don't know what they're missing. Would you like to learn how to craft a proper warrior's helmet?"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered all the materials for the helmet?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(EllomBellId, 40) && player.Inventory.HasItem(CronewtLeatherId, 25) && player.Inventory.HasItem(KepoFluffId, 60)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(EllomBellId, 40) &&
					player.Inventory.HasItem(CronewtLeatherId, 25) &&
					player.Inventory.HasItem(KepoFluffId, 60))
				{
					player.Inventory.RemoveItem(EllomBellId, 40);
					player.Inventory.RemoveItem(CronewtLeatherId, 25);
					player.Inventory.RemoveItem(KepoFluffId, 60);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*expertly crafts the helmet* Ah, this brings back memories! The bells will ring with each victory, the leather will protect you, and the fluff... well, you'll be the most comfortable warrior on the battlefield!"));

					await dialog.Msg(L("*eyes gleaming with pride* You know, I could always teach you to make another. Nothing brings me more joy than passing on the old warrior traditions."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*returns to polishing helmets* Take your time, young one. A proper helmet can't be rushed. The materials must be perfect, just like in the old days."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you like to learn to craft another feather helmet?"),
			Option(L("Yes, teach me"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*stands up straight with military bearing* Excellent! Remember: 40 Ellom bells for strength - they must ring clear and true. 25 pieces of Cronewt leather - make sure they're not damaged. And 60 tufts of Kepo fluff - the softest you can find!"));
		}
		else
		{
			await dialog.Msg(L("*nods understandingly* A warrior must choose their own path. Return if you wish to learn more about the old ways."));
		}
	}
}
