using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class PuddingHatQuestScript : QuestScript
{
	private const int QuestNumber = 10009;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Sweet Creation";
	private const string QuestDescription = "Help Chef Purin create a special dessert-inspired hat using mushrooms and flowers from the orchard.";

	private const int TruffleMushroom = 645652;
	private const int CorpseFlower = 645653;
	private const int PuddingHatId = 628091;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_mushrooms", "Collect 100 Red Truffle Mushrooms", new CollectItemObjective(TruffleMushroom, 100));
		AddObjective("collect_flowers", "Collect 50 Pungent Stems", new CollectItemObjective(CorpseFlower, 50));
		AddReward(new ItemReward(PuddingHatId, 1));

		AddNpc(152002, L("[Chef] Purin"), "f_orchard_34_1", -1240, 66, 90, PurinDialog);

		AddNpc(155127, L("Cauldron"), "f_orchard_34_1", -1219, 71, 0);
	}

	private async Task PurinDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Purin"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*stirring a large pot* Ah, this place is just perfect... *sniffs the air excitedly* The mushrooms and flowers around here have such... interesting aromas."));
			await dialog.Msg(L("I'm working on a special hat design inspired by my favorite dessert - pudding! Would you help me gather some ingredients? I promise the end result will be worth it!"));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the ingredients for my special creation?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(TruffleMushroom, 100) && player.Inventory.HasItem(CorpseFlower, 50)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(TruffleMushroom, 100) &&
					player.Inventory.HasItem(CorpseFlower, 50))
				{
					player.Inventory.RemoveItem(TruffleMushroom, 100);
					player.Inventory.RemoveItem(CorpseFlower, 50);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*claps excitedly* Perfect! The mushrooms give it that bouncy texture, and the flower stems add just the right aroma! Here's your Pudding Hat - doesn't it look delicious enough to eat?"));

					await dialog.Msg(L("*stirs pot thoughtfully* You know... I'm always experimenting with new recipes. Come back if you'd like another one!"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*tastes the mixture* Hmm, not quite right yet. Keep gathering those ingredients!"));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you like to help me create a Pudding Hat?"),
			Option(L("I'll help gather ingredients"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Wonderful! Bring me 100 Red Truffle Mushrooms for the base and 50 Pungent Stems for that special aroma. Together we'll make something sweetly spectacular!"));
		}
		else
		{
			await dialog.Msg(L("*returns to stirring* That's okay! The pot's always simmering if you change your mind~"));
		}
	}
}
