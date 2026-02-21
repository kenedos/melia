using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class SeaThemedHeadbandQuestScript : QuestScript
{
	private const int QuestNumber = 10053;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Whispers of the Tides";
	private const string QuestDescription = "Help the shrine keeper restore an ancient ceremonial headband blessed by the water spirits.";

	private const int MerogHeartId = 645164;
	private const int MerogHeartAmount = 60;
	private const int CinnabarId = 645234;
	private const int CinnabarAmount = 40;
	private const int SeaThemedHeadbandId = 628242;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_hearts", "Collect 60 Merog Hearts", new CollectItemObjective(MerogHeartId, MerogHeartAmount));
		AddObjective("collect_cinnabar", "Collect 40 Cinnabar", new CollectItemObjective(CinnabarId, CinnabarAmount));
		AddReward(new ItemReward(SeaThemedHeadbandId, 1));

		AddNpc(20120, L("[Shrine Keeper] Thalassa"), "f_3cmlake_83", -118, 259, 0, ShrineKeeperDialog);
	}

	private async Task ShrineKeeperDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Thalassa"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*gazes out over the still waters*{/}"));
			await dialog.Msg(L("Traveler... these ruins once housed a shrine dedicated to the spirits of the deep. Long ago, pilgrims would come seeking the blessing of the tides."));
			await dialog.Msg(L("I have been trying to restore an ancient ceremonial headband, blessed by the water spirits themselves. But I need materials that only the Merogs carry..."));
		}
		else
		{
			var selection = await dialog.Select(L("{#666666}*turns from the water*{/} Have you gathered what I need to complete the restoration?"),
				Option(L("Yes, I have everything"), "complete", () => player.Inventory.HasItem(MerogHeartId, MerogHeartAmount) && player.Inventory.HasItem(CinnabarId, CinnabarAmount)),
				Option(L("Still searching"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(MerogHeartId, MerogHeartAmount) &&
					player.Inventory.HasItem(CinnabarId, CinnabarAmount))
				{
					player.Inventory.RemoveItem(MerogHeartId, MerogHeartAmount);
					player.Inventory.RemoveItem(CinnabarId, CinnabarAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*carefully weaves the materials together, whispering ancient words*{/}"));
					await dialog.Msg(L("The spirits of the deep have blessed this work. Take this headband - may the waters always guide you safely home."));

					await dialog.Msg(L("{#666666}*returns to watching the water*{/} The shrine's power grows stronger each day. Should you wish another blessing, return with more materials..."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("The Merogs have grown bold since the shrine fell into disrepair. Their hearts carry the essence of the waters they once protected. The cinnabar they hoard preserves the old magic."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Will you help me restore this sacred artifact?"),
			Option(L("I will gather what you need"), "accept"),
			Option(L("Perhaps another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("The Merog creatures nearby have absorbed the shrine's power over the years. Their hearts carry the essence I need, and the shamans among them hoard cinnabar - a mineral sacred to water rituals. Bring me these, and I shall craft for you a headband blessed by the tides themselves."));
		}
		else
		{
			await dialog.Msg(L("{#666666}*nods slowly*{/} The waters are patient. Return when you are ready."));
		}
	}
}
