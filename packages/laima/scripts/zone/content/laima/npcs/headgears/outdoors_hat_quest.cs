using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class OutdoorsHatQuestScript : QuestScript
{
	private const int QuestNumber = 10024;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Forest Fashion";
	private const string QuestDescription = "Help the wandering tailor create a sturdy hat perfect for outdoor adventures.";

	private const int TimberPieceId = 645585;
	private const int CystNeedleId = 645586;
	private const int OutdoorsHatId = 628111;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_timber", "Collect 40 Short Timber", new CollectItemObjective(TimberPieceId, 40));
		AddObjective("collect_needles", "Collect 25 Cyst Needles", new CollectItemObjective(CystNeedleId, 25));
		AddReward(new ItemReward(OutdoorsHatId, 1));

		AddNpc(155035, L("[Tailor] Pascal"), "f_farm_49_2", -401, 1089, 0, TailorDialog);
	}

	private async Task TailorDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Pascal"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("This place used to be so beautiful, the architecture, the constructions!"));
			await dialog.Msg(L("*adjusts measuring tape* Welcome, welcome! I'm crafting the perfect tool for outdoor adventurers. Something light yet durable, stylish yet practical!"));
			await dialog.Msg(L("The local timber is perfect for the frame, and those nasty Cyst needles make excellent pins for stitching. Care to help me gather the materials?"));
		}
		else
		{
			var selection = await dialog.Select(L("Found everything we need for our fashionable creation?"),
				Option(L("Here are the materials"), "complete", () => player.Inventory.HasItem(TimberPieceId, 40) && player.Inventory.HasItem(CystNeedleId, 25)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(TimberPieceId, 40) &&
					player.Inventory.HasItem(CystNeedleId, 25))
				{
					player.Inventory.RemoveItem(TimberPieceId, 40);
					player.Inventory.RemoveItem(CystNeedleId, 25);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*skillfully crafts the hat* Marvelous! The frame is light but strong, and those needles work perfectly for the detailed stitching. Here you are - perfect for any outdoor adventure!"));

					await dialog.Msg(L("*starts sketching new designs* You know, I have more ideas for outdoor wear. Interested in another hat?"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*measures some fabric* Take your time! Quality materials make quality hats, after all."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Shall we create a practical yet stylish outdoor hat?"),
			Option(L("Let's do it"), "accept"),
			Option(L("Another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*excitedly pulls out sketches* Excellent! Bring me 40 pieces of Short Timber for the frame and 25 Cyst Needles for stitching. Together we'll create the perfect outdoor hat!"));
		}
		else
		{
			await dialog.Msg(L("*returns to sketching* Fashion waits for no one, but I'll be here when you're ready!"));
		}
	}
}
