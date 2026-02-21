using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class ChristmasTreeSnowglobeQuestScript : QuestScript
{
	private const int QuestNumber = 10049;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Frozen Memories";
	private const string QuestDescription = "Help the tinkerer Holvir create a magical Snowglobe that captures the essence of winter.";

	private const int GrollHornId = 645218;
	private const int GrollHornAmount = 60;
	private const int WhiteChainId = 645649;
	private const int WhiteChainAmount = 80;
	private const int ChristmasTreeSnowglobeId = 628287;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_horns", "Collect 60 Groll Horns", new CollectItemObjective(GrollHornId, GrollHornAmount));
		AddObjective("collect_chains", "Collect 80 White Chains", new CollectItemObjective(WhiteChainId, WhiteChainAmount));
		AddReward(new ItemReward(ChristmasTreeSnowglobeId, 1));

		AddNpc(20105, L("[Tinkerer] Holvir"), "f_tableland_11_1", 387, 2147, 0, HolvirDialog);
	}

	private async Task HolvirDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Holvir"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*tinkers with a small glass sphere near his workshop*{/}"));
			await dialog.Msg(L("Ah, a traveler! Come, come... see this? It's supposed to be a snowglobe, but it's missing something magical. The railroad brought supplies before it broke down, but now I must make do with what the plateau offers."));
			await dialog.Msg(L("The White Grolls roaming these melting snows have horns of pure crystalline ice. And those Saltisdaughters carry white chains infused with frost... With these, I could create something truly wonderful!"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials for my snowglobe?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(GrollHornId, GrollHornAmount) && player.Inventory.HasItem(WhiteChainId, WhiteChainAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(GrollHornId, GrollHornAmount) &&
					player.Inventory.HasItem(WhiteChainId, WhiteChainAmount))
				{
					player.Inventory.RemoveItem(GrollHornId, GrollHornAmount);
					player.Inventory.RemoveItem(WhiteChainId, WhiteChainAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*carefully grinds the horns into a fine powder and weaves the chains into an intricate frame*{/}"));
					await dialog.Msg(L("Magnificent! Watch closely... {#666666}*shakes the completed snowglobe*{/} See how the snow swirls around the tiny tree? A piece of eternal winter, just for you. May it remind you of the magic found in these cold lands."));
				}
			}
			else
			{
				await dialog.Msg(L("The Grolls wander the plateau in large numbers, and the Saltisdaughters are never far behind. Stay warm out there, friend."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather materials for this magical creation?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Wonderful! Bring me 60 Groll Horns from the White Grolls and 80 White Chains from the Saltisdaughters. Their frozen essence will make the perfect snow for this globe!"));
				break;
			case "decline":
				await dialog.Msg(L("Understandable. The cold can be unforgiving. Should you change your mind, I'll be here, tinkering away..."));
				break;
			default:
				break;
		}
	}
}
