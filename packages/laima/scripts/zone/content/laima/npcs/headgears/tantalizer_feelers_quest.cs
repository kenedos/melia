using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class TantalizerFeelersQuestScript : QuestScript
{
	private const int QuestNumber = 10039;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Whispers in the Dark";
	private const string QuestDescription = "Help the naturalist Vaelen study the sensory appendages of the Gate Route's creatures.";

	private const int VelwrigglerFeatherId = 645413;
	private const int VelwrigglerFeatherAmount = 80;
	private const int ThornballThornId = 645159;
	private const int ThornballThornAmount = 60;
	private const int TantalizerFeelersId = 628355;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_feathers", "Collect 80 Velwriggler Feathers", new CollectItemObjective(VelwrigglerFeatherId, VelwrigglerFeatherAmount));
		AddObjective("collect_thorns", "Collect 60 Thornball Thorns", new CollectItemObjective(ThornballThornId, ThornballThornAmount));
		AddReward(new ItemReward(TantalizerFeelersId, 1));

		AddNpc(20109, L("[Naturalist] Vaelen"), "d_thorn_19", -273, -3167, 90, VaelenDialog);
	}

	private async Task VaelenDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Vaelen"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*crouches in the shadows, examining something with a dim lantern*{/}"));
			await dialog.Msg(L("Shh... careful where you step. The creatures here navigate these lightless tunnels using specialized sensory organs. Fascinating, truly fascinating."));
			await dialog.Msg(L("The Velwrigglers possess feather-like appendages that detect the slightest air currents. And those Thornballs... their thorns aren't just defensive. They sense vibrations in complete darkness. I could craft something remarkable with enough samples."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the specimens I requested?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(VelwrigglerFeatherId, VelwrigglerFeatherAmount) && player.Inventory.HasItem(ThornballThornId, ThornballThornAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(VelwrigglerFeatherId, VelwrigglerFeatherAmount) &&
					player.Inventory.HasItem(ThornballThornId, ThornballThornAmount))
				{
					player.Inventory.RemoveItem(VelwrigglerFeatherId, VelwrigglerFeatherAmount);
					player.Inventory.RemoveItem(ThornballThornId, ThornballThornAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*carefully arranges the specimens on a worn cloth, hands moving with practiced precision*{/}"));
					await dialog.Msg(L("Extraordinary quality! With these feathers and thorns, I've crafted something special. Wear these feelers and you'll sense the world as these creatures do... or at least look like you might. A memento of the Gate Route's dark wonders."));
				}
			}
			else
			{
				await dialog.Msg(L("The Velwrigglers drift through the upper passages, while the Thornballs prefer the lower reaches. Both species are abundant, but be wary in the dark."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather specimens for my research?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Excellent! Bring me 80 Velwriggler Feathers and 60 Thornball Thorns. The feathers are delicate, so handle them carefully. The thorns... well, they'll handle you if you're not careful."));
				break;
			case "decline":
				await dialog.Msg(L("The darkness isn't for everyone. I'll continue my observations here, where the light doesn't reach."));
				break;
			default:
				break;
		}
	}
}
