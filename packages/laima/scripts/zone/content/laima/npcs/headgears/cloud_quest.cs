using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class CloudQuestScript : QuestScript
{
	private const int QuestNumber = 10033;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Dreams of the Waterfall";
	private const string QuestDescription = "Help Tomas the dreamer craft a Cloud headpiece using the soft fur leaves of the Siaulamb near Dina Bee Farm.";

	private const int SiaulambFurLeafId = 645590;
	private const int SiaulambFurLeafAmount = 120;
	private const int CloudHeadgearId = 628231;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_fur_leaves", "Collect 120 Siaulamb Fur Leaves", new CollectItemObjective(SiaulambFurLeafId, SiaulambFurLeafAmount));
		AddReward(new ItemReward(CloudHeadgearId, 1));

		AddNpc(20117, L("[Dreamer] Tomas"), "f_siauliai_46_4", 899, 87, 90, TomasDialog);
	}

	private async Task TomasDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Tomas"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*gazes up at the sky near the waterfall*{/}"));
			await dialog.Msg(L("Have you ever looked at the clouds and wondered what it would be like to touch one? I've spent years watching them drift over these falls..."));
			await dialog.Msg(L("The Siaulamb that roam this area have the softest fur leaves I've ever seen. When layered together, they look just like the fluffy clouds above. I want to craft something that captures that feeling."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the Siaulamb Fur Leaves?"),
				Option(L("Yes, I have them all"), "complete", () => player.Inventory.HasItem(SiaulambFurLeafId, SiaulambFurLeafAmount)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(SiaulambFurLeafId, SiaulambFurLeafAmount))
				{
					player.Inventory.RemoveItem(SiaulambFurLeafId, SiaulambFurLeafAmount);
					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*carefully arranges the soft leaves into a fluffy shape*{/}"));
					await dialog.Msg(L("Magnificent! Look at how they catch the light from the waterfall's mist. Here, take this Cloud headpiece. Whenever you wear it, may you feel as free as the clouds drifting over these waters."));
				}
				else
				{
					await dialog.Msg(L("Hmm, it seems you're still missing some. I need 120 Siaulamb Fur Leaves to create the proper cloud effect."));
				}
			}
			else
			{
				await dialog.Msg(L("The Siaulamb wander all around this farm. Their fur leaves are so soft... just like clouds. Take your time gathering them."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather Siaulamb Fur Leaves to craft a Cloud headpiece?"),
			Option(L("I'll help you reach the clouds"), "accept"),
			Option(L("Not right now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Thank you, friend! The Siaulamb here shed their fur leaves quite often. Bring me 120 of them, and I'll craft you a headpiece that will make you feel like you're walking among the clouds themselves."));
				break;
			case "decline":
				await dialog.Msg(L("I understand. Dreams can wait... they always do. But the clouds will still be here when you return."));
				break;
		}
	}
}
