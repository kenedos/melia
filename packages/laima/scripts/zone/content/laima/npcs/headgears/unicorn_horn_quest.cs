using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class UnicornHornQuestScript : QuestScript
{
	private const int QuestNumber = 10005;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Horns of Science";
	private const string QuestDescription = "Help the researcher study Panto horns for his... scientific research.";

	private const int PantoHornId = 645134;
	private const int UnicornHornId = 628059;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_horns", "Collect 250 Panto Horns", new CollectItemObjective(PantoHornId, 250));
		AddReward(new ItemReward(UnicornHornId, 1));

		AddNpc(20190, L("[Researcher] Dr. Hornsby"), "f_gele_57_1", 632, -1221, 0, ResearcherDialog);

		AddNpc(153131, L("Alchemy Table"), "f_gele_57_1", 600, -1221, 0);
	}

	private async Task ResearcherDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Dr. Hornsby"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*scribbling notes frantically* The social dynamics of Pantos are fascinating! But their horns... oh, their horns are the real treasure!"));
			await dialog.Msg(L("*adjusts glasses nervously* I mean... they're scientifically significant. Yes, that's it. For research purposes only, of course."));
		}
		else
		{
			var selection = await dialog.Select(L("*eagerly* Do you have those horns- I mean, research specimens?"),
				Option(L("Here are the horns"), "complete", () => player.Inventory.HasItem(PantoHornId, 250)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(PantoHornId, 250))
				{
					player.Inventory.RemoveItem(PantoHornId, 250);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*eyes gleaming* Beautiful specimens! Here's your Unicorn Horn. A purely decorative piece, of course... not that I would know anything about real unicorns... *laughs nervously*"));

					// Offer repeatable quest
					await dialog.Msg(L("Haha lovely horns... So lovely... Please come back if you find more of them... Ah I love my job~"));
					await StartNewQuest(dialog, player);
				}
				else
				{
					await dialog.Msg(L("*adjusts glasses* My research requires exactly 250 Panto Horns. The sample size must be precise!"));
				}
				return;
			}

			await dialog.Msg(L("*muttering to self* Take your time... good specimens are worth the wait..."));
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help gather Panto Horns for my completely legitimate scientific research?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Excellent! *scribbling intensely* Remember, I need 250 horns for a statistically significant sample size. And do try not to question why I need so many... it's for science!"));
		}
		else
		{
			await dialog.Msg(L("*sighs disappointedly* Very well... I suppose I'll have to continue my research alone. Do come back if you change your mind!"));
		}
	}
}
