using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class WoodOwlQuestScript : QuestScript
{
	private const int QuestNumber = 10020;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Wooden Watcher";
	private const string QuestDescription = "A mysterious wooden owl statue seeks materials from creatures that roam in darkness.";

	private const int RidimedStemId = 645689;
	private const int RedStrawId = 645690;
	private const int KepaSkinId = 645691;
	private const int WoodOwlHatId = 628089;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_stems", "Collect 45 Blue Ridimed Stems", new CollectItemObjective(RidimedStemId, 45));
		AddObjective("collect_straw", "Collect 30 Red Straw Sheaves", new CollectItemObjective(RedStrawId, 30));
		AddObjective("collect_skin", "Collect 8 Black Old Kepa Skins", new CollectItemObjective(KepaSkinId, 8));
		AddReward(new ItemReward(WoodOwlHatId, 1));

		AddNpc(153038, L("[Wooden Owl] Nocturnus"), "f_katyn_45_2", -142, -929, 90, NocturnusDialog);
	}

	private async Task NocturnusDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Nocturnus"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*a voice whispers from the carved wooden owl, though it remains perfectly still* The shadows speak to me... through ancient wood grain... {pcname}..."));
			await dialog.Msg(L("*the voice seems to emanate from everywhere and nowhere* For centuries I have watched... observed... From this wooden form, I see what others cannot..."));
		}
		else
		{
			var selection = await dialog.Select(L("*the whispers echo from the unmoving wooden beak* Have you brought... what the night has yielded?"),
				Option(L("Yes, here they are"), "complete", () =>
					player.Inventory.HasItem(RidimedStemId, 45) &&
					player.Inventory.HasItem(RedStrawId, 30) &&
					player.Inventory.HasItem(KepaSkinId, 8)),
				Option(L("Still searching"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(RidimedStemId, 45) &&
					player.Inventory.HasItem(RedStrawId, 30) &&
					player.Inventory.HasItem(KepaSkinId, 8))
				{
					player.Inventory.RemoveItem(RidimedStemId, 45);
					player.Inventory.RemoveItem(RedStrawId, 30);
					player.Inventory.RemoveItem(KepaSkinId, 8);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*the wooden owl's eyes seem to absorb the darkness around it* Yesss... The stems hold moonlight... the straw carries twilight... the skin contains shadow itself..."));
					await dialog.Msg(L("*whispers grow fainter* Take this gift... forged from night's essence... I will wait here... as I always have... as I always will..."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*the whispers fade to barely audible* Time means nothing to one who cannot move... I will wait..."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("*whispers drift from the wooden owl statue* Will you gather... what the darkness holds?"),
			Option(L("I will help"), "accept"),
			Option(L("Not now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*the voice grows stronger* Find the blue Ridimed where moonlight touches water... the red Puragi where shadows pool... the purple Kepa where darkness dwells... My wooden eyes see where they hide..."));
		}
		else
		{
			await dialog.Msg(L("*the whispers grow distant* I will remain... as I have for centuries... watching... waiting... in this wooden form..."));
		}
	}
}
