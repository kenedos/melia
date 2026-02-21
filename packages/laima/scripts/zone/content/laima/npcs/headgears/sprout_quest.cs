using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class SproutQuestScript : QuestScript
{
	private const int QuestNumber = 10013;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Nature's Gift";
	private const string QuestDescription = "Help the young botanist grow a special sprout accessory using rare seeds.";

	private const int HanamingPetalId = 645024;
	private const int OnionRedCrystalId = 645127;
	private const int SproutId = 628072;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_petals", "Collect 75 Hanaming Petals", new CollectItemObjective(HanamingPetalId, 75));
		AddObjective("collect_crystals", "Collect 1 Red Kepa Crystals", new CollectItemObjective(OnionRedCrystalId, 1));
		AddReward(new ItemReward(SproutId, 1));

		AddNpc(150238, L("[Botanist] Flora"), "f_siauliai_11_re", 692, 833, 45, FloraDialog);
	}

	private async Task FloraDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Flora"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*excitedly examines plants* The combination of Hanaming's life energy and the crystallized essence of Red Kepas... Yes, yes! It could work!"));
			await dialog.Msg(L("Oh! Hello there! Would you like to help me with an experiment? I'm trying to create a special sprout that can be worn as an accessory!"));
		}
		else
		{
			var selection = await dialog.Select(L("Did you gather the materials for our sprout experiment?"),
				Option(L("Yes, I have them"), "complete", () => player.Inventory.HasItem(HanamingPetalId, 75) && player.Inventory.HasItem(OnionRedCrystalId, 1)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(HanamingPetalId, 75) &&
					player.Inventory.HasItem(OnionRedCrystalId, 1))
				{
					player.Inventory.RemoveItem(HanamingPetalId, 75);
					player.Inventory.RemoveItem(OnionRedCrystalId, 1);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*bounces excitedly* It worked! It worked! Look at this adorable little sprout! Here, you should wear it - you helped create it after all!"));

					await dialog.Msg(L("*eyes sparkle* Oh! We could try different combinations of materials! Want to make another one?"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*tends to nearby plants* Take your time! Good science can't be rushed!"));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Want to help create a magical sprout accessory?"),
			Option(L("Sure, sounds fun!"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*claps happily* Wonderful! Bring me 75 Hanaming Petals and 1 Red Kepa Crystals. The petals provide life essence, while the crystals help stabilize the growth!"));
		}
		else
		{
			await dialog.Msg(L("*returns to her plants* That's okay! Plants teach us patience. Come back when you're ready!"));
		}
	}
}
