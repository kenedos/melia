using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class PolkaDotRibbonQuestScript : QuestScript
{
	private const int QuestNumber = 10025;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Enchanted Ribbon";
	private const string QuestDescription = "Help the eccentric enchantress create magical polka dot ribbons using materials from magical creatures.";

	private const int MageCoreId = 645447;
	private const int MageStaffId = 645945;
	private const int PolkaDotRibbonId = 628094;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_cotton", "Collect 10 Shaman Doll Cotton", new CollectItemObjective(MageCoreId, 10));
		AddObjective("collect_staff", "Collect 3 Tala Mage Staffs", new CollectItemObjective(MageStaffId, 3));
		AddReward(new ItemReward(PolkaDotRibbonId, 1));

		AddNpc(153215, L("[Enchantress] Dotty"), "d_thorn_39_1", 888, -165, 90, EnchantressDialog);
	}

	private async Task EnchantressDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Dotty"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*twirls excitedly* Oh! Oh! You simply must help me! I'm creating the most wonderful enchanted ribbons with the most delightful polka dots that sparkle and shimmer!"));
			await dialog.Msg(L("*bounces in place* The magic from the mages here is perfect for it - their cotton has such lovely residual energy, and their staffs can be ground down for the most spectacular magical dust!"));
		}
		else
		{
			var selection = await dialog.Select(L("*spins around* Did you bring the magical materials?"),
				Option(L("Here they are"), "complete", () => player.Inventory.HasItem(MageCoreId, 10) && player.Inventory.HasItem(MageStaffId, 3)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(MageCoreId, 10) &&
					player.Inventory.HasItem(MageStaffId, 3))
				{
					player.Inventory.RemoveItem(MageCoreId, 10);
					player.Inventory.RemoveItem(MageStaffId, 3);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*dances while crafting* Yes, yes, YES! Watch the magic swirl... see the dots appear... *giggles* Here you are! Isn't it just the most delightful thing you've ever seen?"));

					await dialog.Msg(L("*claps hands* Oh, we simply must make another! The dots are never quite the same twice - that's what makes them so magical!"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*hums while spinning in place* Take your time! The magic must be just right... just right!"));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("*bounces eagerly* Shall we create a magical polka dot ribbon?"),
			Option(L("Let's make magic!"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*squeals with delight* Wonderful! Bring me 10 bits of Shaman Doll Cotton and 3 Tala Mage Staffs. Oh, this will be so much fun!"));
		}
		else
		{
			await dialog.Msg(L("*pouts briefly before twirling* Well, the dots will be here waiting when you're ready for their sparkly magic!"));
		}
	}
}
