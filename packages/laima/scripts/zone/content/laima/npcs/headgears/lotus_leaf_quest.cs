using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Scripting.Extensions.LivelyDialog;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class LotusLeafQuestScript : QuestScript
{
	private const int QuestNumber = 10002;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Nature's Beauty";
	private const string QuestDescription = "Help Miri craft a special Lotus Leaf headpiece by gathering materials from the forest.";

	private const int LeafBugShellId = 645118;
	private const int HanamingPetalId = 645024;
	private const int LotusLeafId = 628257;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_shells", "Collect 50 Leaf Bug Shells", new CollectItemObjective(LeafBugShellId, 50));
		AddObjective("collect_petals", "Collect 30 Hanaming Petals", new CollectItemObjective(HanamingPetalId, 30));
		AddReward(new ItemReward(LotusLeafId, 1));

		AddNpc(20117, L("[Plant Enthusiast] Miri"), "f_siauliai_west", -596, -540, 90, MiriDialog);
	}

	private async Task MiriDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);
		var hasCompletedQuest = player.Quests.HasCompleted(QuestId);

		dialog.SetTitle(L("Miri"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("Ah, welcome! I don't get many visitors out here. *sniffs a nearby flower* But the plants keep me company."));
			await dialog.Msg(L("I've been working on crafting a special headpiece made from lotus leaves, but I need some materials that are hard to find..."));
		}
		else
		{
			var selection = await dialog.Select(L("How is the material gathering going?"),
				Option(L("I have everything"), "complete", () => player.Inventory.HasItem(LeafBugShellId, 50) && player.Inventory.HasItem(HanamingPetalId, 30)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(LeafBugShellId, 50) &&
					player.Inventory.HasItem(HanamingPetalId, 30))
				{
					// Remove materials
					player.Inventory.RemoveItem(LeafBugShellId, 50);
					player.Inventory.RemoveItem(HanamingPetalId, 30);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("Perfect! These materials are exactly what I needed. Here's your Lotus Leaf headpiece - may it bring you closer to nature's beauty!"));
				}
				else
				{
					await dialog.Msg("Umm sorry you seem to be missing a few items. I need 50 Leaf Bug Shells and 30 Hanaming Petals.");
				}
			}
			else
			{
				await dialog.Msg(L("Take your time. Nature's beauty can't be rushed."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather materials for a special Lotus Leaf headpiece?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Thank you! The shells will give structure to the headpiece while the petals will add a wonderful fragrance. Please bring them when you can."));
				break;
			case "decline":
				await dialog.Msg(L("Perhaps another time then. The flowers will still be here when you return."));
				break;
			default:
				break;
		}
	}
}
