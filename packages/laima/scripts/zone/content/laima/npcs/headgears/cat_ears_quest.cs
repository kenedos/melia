using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class ChupacabraFearQuestScript : QuestScript
{
	private const int QuestNumber = 10008;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Desert Fears";
	private const string QuestDescription = "Help Felix overcome his fear of chupacabras by gathering proof of their docile nature.";

	private const int ChupacabraMeatId = 645173;
	private const int ChupacabraSkinId = 645420;
	private const int GreyCatEarsId = 628295;
	private const int OrangeCatEarsId = 628296;
	private const int WhiteCatEarsId = 628297;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_meat", "Collect 80 Desert Chupacabra Meat", new CollectItemObjective(ChupacabraMeatId, 80));
		AddObjective("collect_skin", "Collect 5 Desert Chupacabra Skin", new CollectItemObjective(ChupacabraSkinId, 5));

		// Random reward will be given in the dialogue
		AddReward(new ItemReward(GreyCatEarsId, 1));
		AddReward(new ItemReward(OrangeCatEarsId, 1));
		AddReward(new ItemReward(WhiteCatEarsId, 1));

		AddNpc(155143, L("[Fearful] Felix"), "f_rokas_25", -2502, -1124, 90, FelixDialog);
	}

	private async Task FelixDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Felix"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*nervously looking around* Th-they say chupacabras are everywhere in these deserts! Have you heard the stories? They drink blood and... and... *shudders*"));
			await dialog.Msg(L("I need proof they're not as terrible as people say. But I'm too scared to go near them! Could you help me?"));
		}
		else
		{
			var selection = await dialog.Select(L("D-did you get close to those creatures?"),
				Option(L("Yes, I have proof"), "complete", () => player.Inventory.HasItem(ChupacabraMeatId, 80) && player.Inventory.HasItem(ChupacabraSkinId, 5)),
				Option(L("Still gathering proof"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(ChupacabraMeatId, 80) &&
					player.Inventory.HasItem(ChupacabraSkinId, 5))
				{
					player.Inventory.RemoveItem(ChupacabraMeatId, 80);
					player.Inventory.RemoveItem(ChupacabraSkinId, 5);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*examines the materials* They... they really are just like desert foxes? I've been afraid for nothing! Here, take these cat ears - they remind me that not everything is as scary as it seems."));
				}
			}
			else
			{
				await dialog.Msg(L("*trembling* Take your time... just be careful out there!"));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Will you help me learn the truth about chupacabras?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Oh thank you! *wrings hands nervously* I need 80 pieces of their meat and 5 of their skin. That should tell me what they really are made of... right? Just... just be careful!"));
				break;
			case "decline":
				await dialog.Msg(L("*whimpers* I understand... they are scary after all..."));
				break;
			default:
				break;
		}
	}
}
