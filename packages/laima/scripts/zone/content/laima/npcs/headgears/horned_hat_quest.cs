using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class HornedHatQuestScript : QuestScript
{
	private const int QuestNumber = 10050;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Petrified Guardian";
	private const string QuestDescription = "Help the historian Valdric recreate a ceremonial horned hat worn by the guardian who fell to the curse of Roxona Market.";

	private const int MoyaFragmentId = 645559;
	private const int MoyaFragmentAmount = 80;
	private const int BavonFeatherId = 645409;
	private const int BavonFeatherAmount = 40;
	private const int HornedHatId = 628255;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_fragments", "Collect 80 Moya Fragments", new CollectItemObjective(MoyaFragmentId, MoyaFragmentAmount));
		AddObjective("collect_feathers", "Collect 40 Bavon Feathers", new CollectItemObjective(BavonFeatherId, BavonFeatherAmount));
		AddReward(new ItemReward(HornedHatId, 1));

		AddNpc(20121, L("[Historian] Valdric"), "f_flash_60", -1147, -285, 0, ValdricDialog);
	}

	private async Task ValdricDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Valdric"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*runs a hand along a petrified stone formation*{/}"));
			await dialog.Msg(L("Do you see these spikes? They were not always stone. Long ago, this was a thriving market town, protected by a guardian who wore a distinctive horned hat."));
			await dialog.Msg(L("When the curse swept through Roxona, he stood against it... and was petrified along with everything else. Now these Moyas and Bavons infest his domain, their bodies carrying fragments of the cursed energy that turned him to stone."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials I need?"),
				Option(L("Yes, I have them"), "complete", () => player.Inventory.HasItem(MoyaFragmentId, MoyaFragmentAmount) && player.Inventory.HasItem(BavonFeatherId, BavonFeatherAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(MoyaFragmentId, MoyaFragmentAmount) &&
					player.Inventory.HasItem(BavonFeatherId, BavonFeatherAmount))
				{
					player.Inventory.RemoveItem(MoyaFragmentId, MoyaFragmentAmount);
					player.Inventory.RemoveItem(BavonFeatherId, BavonFeatherAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*carefully arranges the materials on a worn workbench*{/}"));
					await dialog.Msg(L("Remarkable... these fragments still pulse with residual energy. And the feathers from those flying creatures carry traces of the storm that accompanied the curse."));
					await dialog.Msg(L("Here, take this recreation of the Guardian's Horned Hat. Perhaps by wearing it, his memory will live on through you. May you never share his fate."));
				}
			}
			else
			{
				await dialog.Msg(L("The Moyas gather near the petrified buildings to the west. The Bavons circle overhead, drawn to the cursed energy. Be careful - this place claims many who linger too long."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Will you help me gather what I need to recreate the guardian's hat?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Thank you. I need 80 Moya Fragments - the creatures seem to absorb the curse's energy into their very bodies. I also need 40 Bavon Feathers from the flying beasts that circle this cursed sky."));
				await dialog.Msg(L("With these materials, I can craft a replica of the Guardian's Horned Hat. It won't break the curse, but... perhaps it will honor his sacrifice."));
				break;
			case "decline":
				await dialog.Msg(L("{#666666}*returns to examining a petrified merchant stall*{/} The stones here have many secrets to tell. Return if you change your mind."));
				break;
		}
	}
}
