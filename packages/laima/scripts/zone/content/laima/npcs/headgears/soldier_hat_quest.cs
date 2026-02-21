using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class SoldierHatQuestScript : QuestScript
{
	private const int QuestNumber = 10004;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Guild Gatherer";
	private const string QuestDescription = "Help an adventurer collect items for the Adventurer's Guild research.";

	private const int ShreddedClothId = 645287;
	private const int YekubiteAntennaeId = 645132;
	private const int SoldierHatId = 628279;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_cloth", "Collect 1 Shredded Pieces of Cloth", new CollectItemObjective(ShreddedClothId, 1));
		AddObjective("collect_antennae", "Collect 150 Yekubite Antennae", new CollectItemObjective(YekubiteAntennaeId, 150));
		AddReward(new ItemReward(SoldierHatId, 1));

		AddNpc(154033, L("[Adventurer] Marcus"), "d_cmine_02", 83, -110, 45, AdventurerDialog);
	}

	private async Task AdventurerDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Marcus"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*checking list* The guild's research department is always in need of specimens from the field."));
			await dialog.Msg(L("I'm on collection duty, but there's so much to gather... Say, would you be interested in helping out? The guild rewards its helpers well."));
		}
		else
		{
			var selection = await dialog.Select(L("Found everything on the list?"),
				Option(L("Turn in items"), "complete", () => player.Inventory.HasItem(ShreddedClothId, 1) && player.Inventory.HasItem(YekubiteAntennaeId, 150)),
				Option(L("Still searching"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(ShreddedClothId, 1) &&
					player.Inventory.HasItem(YekubiteAntennaeId, 150))
				{
					player.Inventory.RemoveItem(ShreddedClothId, 1);
					player.Inventory.RemoveItem(YekubiteAntennaeId, 150);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("Excellent specimens! The research department will be pleased. Here's your Soldier Hat - straight from the guild's special collection."));

					// Offer repeatable quest
					await dialog.Msg(L("You know... The guild always needs more samples. Interested in another collection run?"));
					await StartNewQuest(dialog, player);
				}
				else
				{
					await dialog.Msg(L("Let me check the list again... We need 1 Shredded Pieces of Cloth and 150 Yekubite Antennae."));
				}
				return;
			}

			await dialog.Msg(L("Keep searching! The specimens aren't going to collect themselves."));
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Want to help with the guild's collection efforts?"),
			Option(L("I'll help collect"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Great! The cloth samples will help us study material degradation in the mines, and the antennae have unique properties the researchers are studying. Happy hunting!"));
		}
		else
		{
			await dialog.Msg(L("No problem, I'll keep collecting. Stop by if you change your mind - the guild always needs help!"));
		}
	}
}
