using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class RadishuQuestScript : QuestScript
{
	private const int QuestNumber = 10043;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Perfect Radish";
	private const string QuestDescription = "Help the old farmer Theodric create a special compost to grow the perfect radish.";

	private const int AshrongStemId = 645573;
	private const int AshrongStemAmount = 80;
	private const int CronewtBonesId = 645547;
	private const int CronewtBonesAmount = 50;
	private const int RadishuId = 628201;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_stems", "Collect 80 Ashrong Stems", new CollectItemObjective(AshrongStemId, AshrongStemAmount));
		AddObjective("collect_bones", "Collect 50 Cronewt Bones", new CollectItemObjective(CronewtBonesId, CronewtBonesAmount));
		AddReward(new ItemReward(RadishuId, 1));

		AddNpc(20117, L("[Farmer] Theodric"), "f_farm_47_2", 740, -1516, 90, TheodricDialog);
	}

	private async Task TheodricDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Theodric"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*wipes sweat from his brow while examining a wilted plant*{/}"));
			await dialog.Msg(L("Ah, another traveler passing through! Say, you wouldn't happen to know anything about farming, would you? These fields used to produce the finest radishes in all the kingdom..."));
			await dialog.Msg(L("But ever since those Ashronge and Cronewts moved in, the soil's gone all wrong. Funny thing is, I think those very creatures might be the key to fixing it!"));
		}
		else
		{
			var selection = await dialog.Select(L("Did you gather the materials for my special compost?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(AshrongStemId, AshrongStemAmount) && player.Inventory.HasItem(CronewtBonesId, CronewtBonesAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(AshrongStemId, AshrongStemAmount) &&
					player.Inventory.HasItem(CronewtBonesId, CronewtBonesAmount))
				{
					player.Inventory.RemoveItem(AshrongStemId, AshrongStemAmount);
					player.Inventory.RemoveItem(CronewtBonesId, CronewtBonesAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*excitedly examines the materials*{/} Perfect! These stems will break down beautifully, and the bones... they'll add just the right minerals to the soil!"));
					await dialog.Msg(L("{#666666}*mixes the compost and tends to a special plot*{/} Would you look at that! The radish is already sprouting! Here, take this one - it grew so fast it popped right out of the ground wearing its own little hat!"));

					await dialog.Msg(L("You know, the soil around here could always use more enrichment. If you bring me more materials, I could grow another prize radish for you..."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("The Ashronge wander all around the aqueduct area - you can't miss their strange stems. As for the Cronewts, those lizard-folk leave bones scattered everywhere near their camps to the south."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials for a special compost?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Wonderful! I need Ashrong Stems - about 80 of them - for the organic matter. They're fibrous and break down slowly, perfect for long-term soil health. And 50 Cronewt Bones will add the minerals my radishes crave!"));
		}
		else
		{
			await dialog.Msg(L("{#666666}*sighs and returns to tending the wilted crops*{/} Can't blame you. These old bones aren't what they used to be either... Maybe next time."));
		}
	}
}
