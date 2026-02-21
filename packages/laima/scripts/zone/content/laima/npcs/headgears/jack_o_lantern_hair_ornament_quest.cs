//--- Melia Script -----------------------------------------------------------
// Jack-O-Lantern Hair Ornament Quest
//--- Description -----------------------------------------------------------
// A lumberjack in the haunted Escanciu Village offers a pumpkin ornament
// in exchange for bones from the ghostly Hallowventers that haunt the area.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class JackOLanternHairOrnamentQuestScript : QuestScript
{
	private const int QuestNumber = 10037;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Carver's Last Harvest";
	private const string QuestDescription = "Help the lumberjack Garvyn craft a Jack-O-Lantern ornament using bones from the Hallowventers haunting Escanciu Village.";

	private const int HallowventerHandBonesId = 645481;
	private const int HallowventerHandBonesAmount = 75;
	private const int JackOLanternHairOrnamentId = 628268;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_bones", "Collect 75 Hallowventer Hand Bones",
			new CollectItemObjective(HallowventerHandBonesId, HallowventerHandBonesAmount));

		AddReward(new ItemReward(JackOLanternHairOrnamentId, 1));

		AddNpc(20160, L("[Lumberjack] Garvyn"), "f_remains_39", -1313, 482, 0, GarvynDialog);
	}

	private async Task GarvynDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Garvyn"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*The lumberjack sits on a stump, nervously carving a small pumpkin*{/}"));
			await dialog.Msg(L("Ah, a traveler! Don't mind the axe - I'm no threat. Haven't swung it at a tree in weeks. Not since... well, not since the ghosts came."));
			await dialog.Msg(L("See, I used to carve pumpkins for the village children every harvest season. But now the village is empty, and these cursed Hallowventers float about with their bony fingers, mocking what we once had."));
			await dialog.Msg(L("I've been thinking... if I had some of their hand bones, I could fashion something special. A little ornament to remember the old days - shaped like the pumpkins I used to carve."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the Hallowventer Hand Bones?"),
				Option(L("Yes, I have them"), "complete",
					() => player.Inventory.HasItem(HallowventerHandBonesId, HallowventerHandBonesAmount)),
				Option(L("Still hunting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(HallowventerHandBonesId, HallowventerHandBonesAmount))
				{
					player.Inventory.RemoveItem(HallowventerHandBonesId, HallowventerHandBonesAmount);
					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*Garvyn's weathered hands work with surprising delicacy, shaping the bones*{/}"));
					await dialog.Msg(L("There we are... a little Jack-O-Lantern, just like I used to make for the children. The bones give it an eerie glow - fitting for this haunted place."));
					await dialog.Msg(L("Take it, friend. Wear it proudly. And if you ever see any of those village folk... tell them old Garvyn still remembers the harvest days."));
				}
			}
			else
			{
				await dialog.Msg(L("Those Hallowventers drift about the village and the surrounding area. They're not hard to find - just follow the cold spots in the air."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help an old woodsman reclaim a bit of the past?"),
			Option(L("I'll gather the bones"), "accept"),
			Option(L("Not right now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Bless you, traveler. Bring me 75 Hallowventer Hand Bones - you'll find plenty of those wretched spirits floating about the village. They won't give up their bones easily, mind you."));
				break;
			case "decline":
				await dialog.Msg(L("I understand. It's dangerous work, hunting ghosts. I'll be here if you change your mind - not like I have anywhere else to go."));
				break;
		}
	}
}
