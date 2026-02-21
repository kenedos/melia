using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class BuriedCarrotQuestScript : QuestScript
{
	private const int QuestNumber = 10040;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Underground Harvest";
	private const string QuestDescription = "Help a forager searching for underground vegetables along the Gate Route.";

	private const int TruffleCrystalId = 645162;
	private const int GrollHornId = 645218;
	private const int BuriedCarrotId = 628230;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_crystals", "Collect 60 Truffle Crystals", new CollectItemObjective(TruffleCrystalId, 60));
		AddObjective("collect_horns", "Collect 30 Groll Horns", new CollectItemObjective(GrollHornId, 30));
		AddReward(new ItemReward(BuriedCarrotId, 1));

		AddNpc(20117, L("[Forager] Terron"), "d_thorn_19", 1657, 2689, 0, TerronDialog);
	}

	private async Task TerronDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Terron"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*digs through the thorny undergrowth with a small trowel*{/}"));
			await dialog.Msg(L("Cauliflowers... I was told there were cauliflowers growing nearby, but all I find are these strange crystallized mushrooms and those aggressive thorny creatures!"));
			await dialog.Msg(L("Wait... perhaps I can use what I find here instead. Those crystal formations from the Truffles and the sturdy horns from the Grolls... I could craft something quite unique!"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the underground materials I need?"),
				Option(L("Yes, I have them all"), "complete", () => player.Inventory.HasItem(TruffleCrystalId, 60) && player.Inventory.HasItem(GrollHornId, 30)),
				Option(L("Still foraging"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(TruffleCrystalId, 60) && player.Inventory.HasItem(GrollHornId, 30))
				{
					player.Inventory.RemoveItem(TruffleCrystalId, 60);
					player.Inventory.RemoveItem(GrollHornId, 30);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*carefully arranges the materials, grinding and mixing them together*{/}"));
					await dialog.Msg(L("Remarkable! The crystal essence combined with the ground horn creates this earth-toned pigment... and look! It has formed into the shape of a carrot! A buried carrot, if you will."));
					await dialog.Msg(L("Please, take this as thanks for helping me discover something far more interesting than mere cauliflowers!"));

					await dialog.Msg(L("{#666666}*excitedly examines the remaining materials*{/} I wonder what else I could create with these components..."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("The Truffles scatter their crystals when defeated, and the Grolls... well, be careful with those. Their horns are sharp!"));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials from this thorny underground passage?"),
			Option(L("I'll help you forage"), "accept"),
			Option(L("Not right now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Wonderful! I need 60 Truffle Crystals from the fungal creatures wandering these tunnels, and 30 Groll Horns from those green beasts that lurk about. The combination should yield something quite special!"));
		}
		else
		{
			await dialog.Msg(L("{#666666}*returns to digging*{/} Very well. I shall continue my search for those elusive cauliflowers..."));
		}
	}
}
