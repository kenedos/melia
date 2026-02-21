using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class UmbrellaQuestScript : QuestScript
{
	private const int QuestNumber = 10052;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Endless Rain";
	private const string QuestDescription = "Help a stranded traveler craft a new umbrella to shield against the eternal downpour of Letas Stream.";

	private const int LargeHookId = 645347;
	private const int LargeHookAmount = 50;
	private const int KitchenKnifeId = 645346;
	private const int KitchenKnifeAmount = 75;
	private const int UmbrellaId = 628256;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_hooks", "Collect 50 Large Hooks", new CollectItemObjective(LargeHookId, LargeHookAmount));
		AddObjective("collect_knives", "Collect 75 Kitchen Knives", new CollectItemObjective(KitchenKnifeId, KitchenKnifeAmount));
		AddReward(new ItemReward(UmbrellaId, 1));

		AddNpc(20117, L("[Traveler] Pluvis"), "f_katyn_12", 171, -954, 45, PluvisDialog);
	}

	private async Task PluvisDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Pluvis"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*huddles beneath a leaking canopy of leaves, water dripping down his collar*{/}"));
			await dialog.Msg(L("Bah! Another drop down my neck! I've been stuck in this accursed forest for three days now, and it hasn't stopped raining for a single moment. Not one!"));
			await dialog.Msg(L("My umbrella... my beautiful umbrella. I lost it crossing the stream. Swept away by the current before I could even react. Now I'm just... slowly dissolving, I think."));
			await dialog.Msg(L("You look like someone who knows their way around monsters. Those Puragis with their oversized hooks, and the Zigris always waving those kitchen knives about... If you could gather some of those, I could fashion a new umbrella. The hooks would make excellent ribs for the frame, and the knife blades can be melted down for the shaft."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials for my umbrella?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(LargeHookId, LargeHookAmount) && player.Inventory.HasItem(KitchenKnifeId, KitchenKnifeAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(LargeHookId, LargeHookAmount) &&
					player.Inventory.HasItem(KitchenKnifeId, KitchenKnifeAmount))
				{
					player.Inventory.RemoveItem(LargeHookId, LargeHookAmount);
					player.Inventory.RemoveItem(KitchenKnifeId, KitchenKnifeAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*takes the materials with trembling, waterlogged hands and begins working feverishly*{/}"));
					await dialog.Msg(L("Yes, yes! The hooks curve perfectly for the ribs... and the metal from these knives... just need to shape it properly..."));
					await dialog.Msg(L("{#666666}*after a few minutes of concentrated work, he holds up a surprisingly elegant umbrella*{/}"));
					await dialog.Msg(L("Ha! Would you look at that! It's actually better than my old one! Here, take this spare I fashioned while I was at it. Consider it payment for saving me from becoming a permanent water feature in this forsaken forest."));
				}
			}
			else
			{
				await dialog.Msg(L("The Puragis lurk deeper in the forest, usually near the murkier pools. And the Zigris... well, they're everywhere really. Just listen for the sound of clanking cutlery and impending doom."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help a soggy traveler reclaim his dignity from the rain?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("You have my eternal gratitude! Bring me 50 Large Hooks from the Puragis and 75 Kitchen Knives from the Zigris. With those, I can craft an umbrella sturdy enough to withstand even this ridiculous eternal rain!"));
				break;
			case "decline":
				await dialog.Msg(L("I understand. No one wants to wade through this miserable swamp for a stranger. I'll just... stand here. Getting wetter. Thinking about my life choices."));
				break;
			default:
				break;
		}
	}
}
