using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class ImperialHelmQuestScript : QuestScript
{
	private const int QuestNumber = 10034;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Echoes of Empire";
	private const string QuestDescription = "Help the archeologist Aldric restore an ancient imperial helm discovered among the Stele Road ruins.";

	private const int AshWoodId = 645239;
	private const int TamaSkinId = 645462;
	private const int ImperialHelmId = 628077;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_wood", "Collect 80 Ash Wood", new CollectItemObjective(AshWoodId, 80));
		AddObjective("collect_skin", "Collect 60 Tama Skin", new CollectItemObjective(TamaSkinId, 60));
		AddReward(new ItemReward(ImperialHelmId, 1));

		AddNpc(152058, L("[Archeologist] Aldric"), "f_remains_37", 434, -1592, 90, ArcheologistDialog);
	}

	private async Task ArcheologistDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Aldric"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*carefully brushes dust from a weathered stone tablet*{/}"));
			await dialog.Msg(L("Ah, a visitor to these forgotten roads! I am Aldric, and I have dedicated my life to uncovering the secrets of the empire that once ruled these lands."));
			await dialog.Msg(L("Among these ruins, I discovered fragments of an imperial helm - a relic from a long-fallen dynasty. With the right materials, I could restore it to its former glory. The stumpy trees here produce an ash wood with unique preservative properties, and the Tama creatures... their skin makes for excellent binding material."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the restoration materials?"),
				Option(L("Yes, I have everything"), "complete", () => player.Inventory.HasItem(AshWoodId, 80) && player.Inventory.HasItem(TamaSkinId, 60)),
				Option(L("Still searching"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(AshWoodId, 80) &&
					player.Inventory.HasItem(TamaSkinId, 60))
				{
					player.Inventory.RemoveItem(AshWoodId, 80);
					player.Inventory.RemoveItem(TamaSkinId, 60);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*examines the materials with scholarly precision*{/}"));
					await dialog.Msg(L("Magnificent! The ash wood's grain is perfect, and this Tama skin... exceptional quality. Watch closely..."));
					await dialog.Msg(L("{#666666}*works with practiced hands, binding and shaping*{/}"));
					await dialog.Msg(L("Behold - the Imperial Helm, restored! Emperors once wore helms like this into battle, leading armies across these very roads. Now it is yours, a piece of history reborn. May it serve you as it served them."));

					await dialog.Msg(L("Should you desire another, I can always craft more. The empire's legacy deserves to live on."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("{#666666}*returns to examining ancient inscriptions*{/}"));
				await dialog.Msg(L("The stumpy trees grow throughout this region - their ash wood is distinctive. And the Tama creatures roam freely here as well. Take your time; history has waited centuries. It can wait a little longer."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials to restore an Imperial Helm?"),
			Option(L("I would be honored"), "accept"),
			Option(L("Perhaps another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Excellent! I require 80 pieces of Ash Wood from the stumpy trees and 60 Tama Skins. The wood's natural preservatives will protect the helm's structure, while the Tama skin provides the binding needed for the interior padding. Return when you have gathered them all."));
		}
		else
		{
			await dialog.Msg(L("{#666666}*nods understandingly*{/}"));
			await dialog.Msg(L("I understand. The ruins will still be here when you return. These stones have waited for ages - they shall wait a while longer."));
		}
	}
}
