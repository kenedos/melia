using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class CrocusQuestScript : QuestScript
{
	private const int QuestNumber = 10031;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Shrine's Bloom";
	private const string QuestDescription = "Help the shrine keeper Velana craft a sacred Crocus ornament using materials from Septyni Glen.";

	private const int MantiwoodSkinId = 645403;
	private const int PineMushroomId = 640044;
	private const int CrocusId = 628261;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_skins", "Collect 50 Mantiwood Skins", new CollectItemObjective(MantiwoodSkinId, 50));
		AddObjective("collect_mushrooms", "Collect 75 Pine Mushrooms", new CollectItemObjective(PineMushroomId, 75));
		AddReward(new ItemReward(CrocusId, 1));

		AddNpc(20168, L("[Shrine Keeper] Velana"), "f_huevillage_58_4", 5, -70, 0, VelanaDialog);
	}

	private async Task VelanaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Velana"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*kneels before the weathered shrine pillar, arranging wilted flowers*{/}"));
			await dialog.Msg(L("Oh... a visitor to these forgotten ruins. The old shrine once bloomed with sacred crocuses, but the glen's corruption has made it difficult to tend them."));
			await dialog.Msg(L("I've been trying to create a lasting tribute to the goddesses - a crocus ornament that won't wither. But I need materials from the creatures that now roam these waters..."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials I need?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(MantiwoodSkinId, 50) && player.Inventory.HasItem(PineMushroomId, 75)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(MantiwoodSkinId, 50) &&
					player.Inventory.HasItem(PineMushroomId, 75))
				{
					player.Inventory.RemoveItem(MantiwoodSkinId, 50);
					player.Inventory.RemoveItem(PineMushroomId, 75);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*carefully weaves the materials together with practiced hands*{/}"));
					await dialog.Msg(L("The Mantiwood's resilient skin forms the base, and the pine mushrooms provide the essence that gives it life... There. A crocus that will never fade."));
					await dialog.Msg(L("Take it as thanks for your devotion. May it remind you that even in corrupted waters, beauty can still bloom."));

					await dialog.Msg(L("If you wish to create another, I would welcome your help again."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("The Mantiwoods lurk in the deeper parts of the glen, and the Carcashu tend to the mushroom patches near the pools. Be cautious around the waters."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials for a sacred crocus ornament?"),
			Option(L("I'll help"), "accept"),
			Option(L("Perhaps another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Thank you, kind traveler. I need 50 Mantiwood Skins from the Mantiwoods that roam the forest, and 75 Pine Mushrooms that grow near the Carcashu. May the goddesses watch over you."));
		}
		else
		{
			await dialog.Msg(L("{#666666}*returns to tending the shrine*{/} The glen's beauty endures... for now. Return if you change your mind."));
		}
	}
}
