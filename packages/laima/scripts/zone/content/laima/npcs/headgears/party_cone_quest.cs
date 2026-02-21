using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class PartyConeQuestScript : QuestScript
{
	private const int QuestNumber = 10011;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Celebration Creation";
	private const string QuestDescription = "Help the festive hat maker create party cones using colorful materials from the forest creatures.";

	private const int PonponBranch = 645740;
	private const int LoktanunFeather = 645739;
	private const int PartyConeId = 628075;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_branches", "Collect 5 Ponpon Branches", new CollectItemObjective(PonponBranch, 5));
		AddObjective("collect_feathers", "Collect 45 Loktanun Feathers", new CollectItemObjective(LoktanunFeather, 45));
		AddReward(new ItemReward(PartyConeId, 1));

		AddNpc(20188, L("[Party Enthusiast] Fiesta"), "f_bracken_63_2", 227, 851, 90, FiestaDialog);
	}

	private async Task FiestaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Fiesta"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*dancing while crafting* Life is a party, and everyone needs the perfect hat! *twirls* These forest creatures have the most wonderful materials for making celebration cones!"));
			await dialog.Msg(L("*stops dancing briefly* Oh! Would you like to help make some party cones? The Ponpon branches make the perfect cone shape, and Loktanun feathers add such festive flair!"));
		}
		else
		{
			var selection = await dialog.Select(L("*bouncing excitedly* Did you bring the party materials?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(PonponBranch, 5) && player.Inventory.HasItem(LoktanunFeather, 45)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(PonponBranch, 5) &&
					player.Inventory.HasItem(LoktanunFeather, 45))
				{
					player.Inventory.RemoveItem(PonponBranch, 5);
					player.Inventory.RemoveItem(LoktanunFeather, 45);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*dances with joy* Wonderful! *crafts while humming* A twist here, a feather there... Ta-da! Your very own Party Cone! Now you're ready to celebrate anything and everything!"));

					await dialog.Msg(L("*starts dancing again* You know what's better than one party hat? TWO party hats! Want to make another one? *spins gleefully*"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*continues dancing* Keep looking! The most festive materials are worth the hunt!"));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Want to join the party-making fun?"),
			Option(L("Let's make a party hat!"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*jumps with excitement* Yay! Bring me 5 Ponpon Branches for the base - they're naturally cone-shaped! And 45 Loktanun Feathers for decoration. They're so colorful, they'll make the perfect party hat!"));
		}
		else
		{
			await dialog.Msg(L("*spins around* That's okay! The party never ends here - come back when you're ready to join the fun!"));
		}
	}
}
