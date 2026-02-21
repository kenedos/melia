using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class CozyPolarBearEarsQuestScript : QuestScript
{
	private const int QuestNumber = 10045;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Warmth in the Blizzard";
	private const string QuestDescription = "Help the mountain ranger Valdis craft a pair of cozy bear ears to survive the frozen plateau.";

	private const int BlueCronewtLeatherId = 645673;
	private const int BlueCronewtLeatherAmount = 100;
	private const int BlueHohenManeId = 645723;
	private const int BlueHohenManeAmount = 30;
	private const int CozyPolarBearEarsId = 628348;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_leather", "Collect 100 Blue Cronewt Leather", new CollectItemObjective(BlueCronewtLeatherId, BlueCronewtLeatherAmount));
		AddObjective("collect_mane", "Collect 30 Blue Hohen Mane", new CollectItemObjective(BlueHohenManeId, BlueHohenManeAmount));
		AddReward(new ItemReward(CozyPolarBearEarsId, 1));

		AddNpc(20109, L("[Mountain Ranger] Valdis"), "f_tableland_70", 3248, -2596, 90, ValdisDialog);
	}

	private async Task ValdisDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Valdis"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*pulls a thick fur hood tighter against the biting wind*{/}"));
			await dialog.Msg(L("Another traveler braving the plateau? Careful now - the cold up here claims the unprepared. I've spent twenty winters mapping these peaks, and I still respect what the frost can do."));
			await dialog.Msg(L("You know what keeps me going? Good gear. I've been working on something special - a pair of ear covers fashioned after the bears that once roamed these heights. Cozy as a campfire, they are."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials I need?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(BlueCronewtLeatherId, BlueCronewtLeatherAmount) && player.Inventory.HasItem(BlueHohenManeId, BlueHohenManeAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(BlueCronewtLeatherId, BlueCronewtLeatherAmount) &&
					player.Inventory.HasItem(BlueHohenManeId, BlueHohenManeAmount))
				{
					player.Inventory.RemoveItem(BlueCronewtLeatherId, BlueCronewtLeatherAmount);
					player.Inventory.RemoveItem(BlueHohenManeId, BlueHohenManeAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*examines the materials with practiced hands*{/} Excellent quality! The leather from those blue Cronewts is naturally resistant to frost - it never quite thaws, which makes it perfect for insulation."));
					await dialog.Msg(L("{#666666}*carefully stitches and shapes the materials*{/} And these Hohen manes... thick and soft, they'll keep the bitter wind from reaching your ears. There we go - a proper pair of polar bear ears!"));

					await dialog.Msg(L("The plateau never stops demanding respect, friend. If you find yourself needing another pair - perhaps for a companion - you know where to find me."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("The blue Cronewts are everywhere on this plateau - scaly beasts that have adapted to the cold. Their leather has a blue tint from the frost. The Hohens are trickier - look for the warriors with the distinctive manes. Both purple and blue variants have the thick fur I need."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you like me to craft you a pair of these cozy bear ears?"),
			Option(L("I would like that"), "accept"),
			Option(L("Not right now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Splendid! I'll need two things from the local creatures. First, about 100 pieces of Blue Cronewt Leather - their hides have this remarkable frost-resistant quality. Second, 30 Blue Hohen Manes for the soft lining. The Hohens' manes trap warmth like nothing else."));
		}
		else
		{
			await dialog.Msg(L("{#666666}*nods knowingly*{/} The plateau will still be here when you're ready. Stay warm out there, traveler."));
		}
	}
}
