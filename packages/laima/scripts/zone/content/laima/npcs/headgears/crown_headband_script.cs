using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class CrownHeadbandQuestScript : QuestScript
{
	private const int QuestNumber = 10012;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Noble Aspirations";
	private const string QuestDescription = "Help the noble artisan craft a crown headband using precious materials from the forest.";

	private const int FerretFur = 645746;
	private const int CrownHeadbandId = 628090;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_fur", "Collect 300 Ferret Fur", new CollectItemObjective(FerretFur, 300));
		AddReward(new ItemReward(CrownHeadbandId, 1));

		AddNpc(155022, L("[Noble Artisan] Coronet"), "f_orchard_34_2", 513, 1066, 0, CoronetDialog);
	}

	private async Task CoronetDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Coronet"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("Every piece must be perfect... *adjusts monocle* The finest fur for comfort... Nothing less will do for my creations."));
			await dialog.Msg(L("Ah, you seem interested in the finer things. Would you care to assist in creating a crown headband worthy of nobility? I require only the most select materials, of course."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you procured the materials I requested?"),
				Option(L("Yes, Your Excellence"), "complete", () => player.Inventory.HasItem(FerretFur, 300)),
				Option(L("Not yet"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(FerretFur, 300))
				{
					player.Inventory.RemoveItem(FerretFur, 300);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*examines materials with approval* Yes... yes, these will do nicely. *crafts with precise movements* Behold, a crown headband that speaks of refinement and taste. Do wear it with the dignity it deserves."));

					await dialog.Msg(L("*adjusts gloves* Should you wish to commission another piece, I would be... amenable to the arrangement."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*slight frown* Remember, only the finest materials will suffice. Do take care in your gathering."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Shall we begin work on this distinguished piece?"),
			Option(L("I would be honored"), "accept"),
			Option(L("Perhaps another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*nods approvingly* Excellent choice. I require 300 pieces of the finest Ferret Fur - do be gentle in collecting it."));
		}
		else
		{
			await dialog.Msg(L("*returns to arranging materials* Very well. Quality craftsmanship cannot be rushed, after all."));
		}
	}
}
