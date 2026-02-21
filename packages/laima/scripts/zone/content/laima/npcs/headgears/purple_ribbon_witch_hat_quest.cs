using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class PurpleRibbonWitchHatQuestScript : QuestScript
{
	private const int QuestNumber = 10051;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Owl Witch's Bargain";
	private const string QuestDescription = "Help the mysterious witch Morvenna gather materials for her enchanted hat in the Owl Burial Grounds.";

	private const int HighVubbeTokenId = 645479;
	private const int HighVubbeTokenAmount = 50;
	private const int BushspiderLeafId = 645363;
	private const int BushspiderLeafAmount = 75;
	private const int PurpleRibbonWitchHatId = 628344;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_tokens", "Collect 50 High Vubbe Tokens", new CollectItemObjective(HighVubbeTokenId, HighVubbeTokenAmount));
		AddObjective("collect_leaves", "Collect 75 Bushspider Leaves", new CollectItemObjective(BushspiderLeafId, BushspiderLeafAmount));
		AddReward(new ItemReward(PurpleRibbonWitchHatId, 1));

		AddNpc(20136, L("[Witch] Morvenna"), "f_katyn_13", -1093, -883, 45, MorvennaDialog);
	}

	private async Task MorvennaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Morvenna"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*A hunched figure tends to a cauldron among the weathered graves, owl statues watching silently from their perches*{/}"));
			await dialog.Msg(L("Ah, a visitor who dares walk among the departed... The owls have been watching you, child. They whisper secrets of your arrival."));
			await dialog.Msg(L("I am Morvenna, keeper of this burial ground. The spirits here have grown restless, and my craft requires... specific ingredients to keep them at peace."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials I require?"),
				Option(L("Yes, I have them"), "complete",
					() => player.Inventory.HasItem(HighVubbeTokenId, HighVubbeTokenAmount) &&
						  player.Inventory.HasItem(BushspiderLeafId, BushspiderLeafAmount)),
				Option(L("Still searching"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(HighVubbeTokenId, HighVubbeTokenAmount) &&
					player.Inventory.HasItem(BushspiderLeafId, BushspiderLeafAmount))
				{
					player.Inventory.RemoveItem(HighVubbeTokenId, HighVubbeTokenAmount);
					player.Inventory.RemoveItem(BushspiderLeafId, BushspiderLeafAmount);
					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*Morvenna's eyes gleam as she examines the materials, her gnarled fingers weaving through the air*{/}"));
					await dialog.Msg(L("Excellent... The Vubbe tokens carry traces of their tribal magic, and the spider leaves still pulse with forest essence. These will serve well."));
					await dialog.Msg(L("As promised, take this hat I crafted during the last purple moon. It belonged to my predecessor, who now rests beneath these very stones. Wear it wisely, and the owls shall watch over you."));
				}
			}
			else
			{
				await dialog.Msg(L("{#666666}*The owl statues seem to turn their stone heads ever so slightly*{/}"));
				await dialog.Msg(L("The forest creatures guard their treasures fiercely. Seek the High Vubbes in the eastern glades and the Bushspiders that lurk in the shadowed groves. Return when your task is complete."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you aid an old witch in her sacred duty? I shall reward you handsomely."),
			Option(L("What do you need?"), "accept"),
			Option(L("I must decline"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("The spirits grow impatient, but you bring hope. I need 50 High Vubbe Tokens from the tribal creatures that infest these woods, and 75 Bushspider Leaves from the eight-legged horrors."));
				await dialog.Msg(L("Bring these to me, and I shall craft you something special... a witch's hat imbued with the essence of this sacred place. The owls shall guide your path."));
				break;
			case "decline":
				await dialog.Msg(L("{#666666}*Morvenna's expression darkens momentarily*{/}"));
				await dialog.Msg(L("So be it. But remember, child... the owls never forget those who turned away from the graves. Walk carefully in these woods."));
				break;
		}
	}
}
