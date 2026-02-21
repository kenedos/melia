using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class SailorCapQuestScript : QuestScript
{
	private const int QuestNumber = 10016;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Sailor's Pride";
	private const string QuestDescription = "Help retired sailor Jacques create a traditional Sailor Cap using high-quality materials.";

	private const int FerretFurId = 645746;
	private const int SailorCapId = 628113;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_fur", "Collect 250 Ferret Fur", new CollectItemObjective(FerretFurId, 250));
		AddReward(new ItemReward(SailorCapId, 1));

		AddNpc(20189, L("[Retired Sailor] Jacques"), "f_orchard_32_4", 965, 1519, 225, JacquesDialog);
	}

	private async Task JacquesDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Jacques"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*adjusts his old cap* This view is amazing! Don't you think? Ah I miss so much my old mates... we used to sail all day!"));
			await dialog.Msg(L("But you know why I'm here, right? this ferret fur... It reminds me of the materials we used to use. Strong, weather-resistant, perfect for every condition..."));
			await dialog.Msg(L("Perfect for making sailor caps! Yes, exactly! So... Are you interested in helping me?"));
		}
		else
		{
			var selection = await dialog.Select(L("Do you have the ferret fur?"),
				Option(L("Yes, here it is"), "complete", () => player.Inventory.HasItem(FerretFurId, 250)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(FerretFurId, 250))
				{
					player.Inventory.RemoveItem(FerretFurId, 250);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*expertly crafts the cap* Ah, perfect! *holds it up proudly* Now THIS is a proper sailor's cap! The ferret fur gives it that sturdy yet comfortable feel we always valued out at sea."));

					// Offer repeatable quest
					await dialog.Msg(L("*straightens his posture* You know, I could always make another one if you're interested. Nothing beats a well-made sailor's cap!"));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*checks his compass* Take your time, sailor. Quality can't be rushed, and neither can collecting the right materials."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Want to help create a genuine sailor's cap?"),
			Option(L("I'll help gather"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*beams with excitement* Excellent! We'll need plenty of ferret fur for this - it's the only material that truly holds up to maritime standards. And believe me, I would know!"));
		}
		else
		{
			await dialog.Msg(L("*returns to adjusting his cap* No rush, the sea teaches us patience after all... Come back when you're ready to learn about real sailing gear!"));
		}
	}
}
