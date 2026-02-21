using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class SnowmanNoseQuestScript : QuestScript
{
	private const int QuestNumber = 10047;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Loneliest Peak";
	private const string QuestDescription = "Help the stranded mountaineer Bjorn craft a companion for his lonely vigil atop Steel Heights.";

	private const int WhiteSpionFurId = 645677;
	private const int WhiteSpionFurAmount = 80;
	private const int BlueHarugalManeId = 645685;
	private const int BlueHarugalManeAmount = 30;
	private const int SnowmanNoseId = 628366;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_fur", "Collect 80 White Spion Fur", new CollectItemObjective(WhiteSpionFurId, WhiteSpionFurAmount));
		AddObjective("collect_mane", "Collect 30 Blue Harugal Mane", new CollectItemObjective(BlueHarugalManeId, BlueHarugalManeAmount));
		AddReward(new ItemReward(SnowmanNoseId, 1));

		AddNpc(20138, L("[Mountaineer] Bjorn"), "f_tableland_74", 278, -141, 90, BjornDialog);
	}

	private async Task BjornDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Bjorn"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*sits near a tent, staring at a pile of frozen meat with a distant look*{/}"));
			await dialog.Msg(L("Ah, another soul brave enough to climb this far! Welcome to the highest point of the tableland. It's just me, the wind, and... well, that frozen meat there. Not the best company, I'm afraid."));
			await dialog.Msg(L("I've been up here so long, I started building a snowman to keep me company. But the blasted thing needs a proper nose, and I ate my last carrot weeks ago. Now I have a rather foolish idea..."));
			await dialog.Msg(L("Those White Spions that prowl these heights have the softest fur. And the Blue Harugals, rare as they are, have manes that shimmer like ice crystals. With enough of both, I could fashion a nose that would make any snowman proud!"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials for my snowman's nose?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(WhiteSpionFurId, WhiteSpionFurAmount) && player.Inventory.HasItem(BlueHarugalManeId, BlueHarugalManeAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(WhiteSpionFurId, WhiteSpionFurAmount) &&
					player.Inventory.HasItem(BlueHarugalManeId, BlueHarugalManeAmount))
				{
					player.Inventory.RemoveItem(WhiteSpionFurId, WhiteSpionFurAmount);
					player.Inventory.RemoveItem(BlueHarugalManeId, BlueHarugalManeAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*eagerly takes the materials and begins weaving them together with practiced hands*{/}"));
					await dialog.Msg(L("Ha! Look at this beauty! The fur makes it soft to the touch, and the mane gives it that distinctive blue-white shimmer of the frozen peaks."));
					await dialog.Msg(L("You know what? I made two. One for my snowman friend here, and one for you. Consider it thanks for humoring a lonely old mountaineer. Maybe you'll find your own peak that needs some company someday."));
				}
			}
			else
			{
				await dialog.Msg(L("The White Spions tend to hunt in packs near the cliffs, and the Blue Harugals... well, they're rare beasts, but fierce. Watch yourself out there, friend."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help an old mountaineer with his peculiar project?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("You're a good soul! Bring me 80 bundles of White Spion Fur and 30 Blue Harugal Manes. The Spions are common enough around here, but the Harugals are elusive. Good hunting!"));
				break;
			case "decline":
				await dialog.Msg(L("Can't blame you. It is a bit of a foolish request. But if you change your mind, I'll be here... talking to my frozen meat, apparently."));
				break;
			default:
				break;
		}
	}
}
