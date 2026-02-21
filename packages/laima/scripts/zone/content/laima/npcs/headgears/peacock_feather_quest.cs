using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class PeacockFeatherQuestScript : QuestScript
{
	private const int QuestNumber = 10017;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Elegant Plume";
	private string QuestDescription = "Help Lady Isabella create an exquisite Peacock Feather hair ornament using rare and beautiful materials.";

	private const int CockatriesCrestId = 645715;
	private const int CockatriesFeatherId = 645714;
	private const int PeacockFeatherId = 628084;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_crests", "Collect 1 Red Cockat Crests", new CollectItemObjective(CockatriesCrestId, 1));
		AddObjective("collect_feathers", "Collect 75 Cockatries Feathers", new CollectItemObjective(CockatriesFeatherId, 75));
		AddReward(new ItemReward(PeacockFeatherId, 1));

		AddNpc(155021, L("[Noblewoman] Isabella"), "f_rokas_24", 1023, 693, 90, IsabellaDialog);
	}

	private async Task IsabellaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Isabella"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*delicately fans herself* The local Cockatries have such magnificent plumage... Almost as grand as the peacocks in my family's estate."));
			await dialog.Msg(L("*eyes sparkling with inspiration* With the right combination of their feathers, one could create something truly... magnificent. Would you be interested in helping me create such an elegant piece?"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you acquired the feathers I requested?"),
				Option(L("Yes, my lady"), "complete", () => player.Inventory.HasItem(CockatriesCrestId, 1) && player.Inventory.HasItem(CockatriesFeatherId, 75)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(CockatriesCrestId, 1) &&
					player.Inventory.HasItem(CockatriesFeatherId, 75))
				{
					player.Inventory.RemoveItem(CockatriesCrestId, 1);
					player.Inventory.RemoveItem(CockatriesFeatherId, 75);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*arranges the feathers with practiced grace* Simply divine! The crests provide that perfect crown-like shape, while the feathers add such elegant flourish. It's absolutely perfect for any formal occasion."));

					// Offer repeatable quest
					await dialog.Msg(L("*adjusts her posture* One can never have too many elegant accessories. Should you wish to create another, I would be delighted to guide you."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*gracefully nods* Take all the time you need. True beauty, after all, cannot be rushed."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Shall we create a magnificent Peacock Feather ornament?"),
			Option(L("I would be honored"), "accept"),
			Option(L("Another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*claps delicately* Splendid! We'll need both the crests and feathers of the Cockatries - only the finest specimens will do. The combination will create something truly worthy of nobility."));
		}
		else
		{
			await dialog.Msg(L("*returns to her fan* Very well, we shall meet again!"));
		}
	}
}
