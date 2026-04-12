using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors.Characters;

public class WhiteFlowerQuestScript : QuestScript
{
	private const int QuestNumber = 10056;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "An Ex-Refugee's Homesickness";
	private const string QuestDescription = "Help Ohla defeat monsters around Mishekan Forest.";

	private const int FlowleviDownId = 645859;
	private const int FlowleviDownAmount = 30;
	private const int FlowlonLeafId = 645889;
	private const int FlowlonLeafAmount = 30;
	private const int BloomLeafId = 645887;
	private const int BloomLeafAmount = 30;
	private const int VirdneyLeafId = 645888;
	private const int VirdneyLeafAmount = 30;
	private const int WhiteFlowerHeadgearId = 628132;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetType(QuestType.Repeat);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_flowlevidowns", "Collect 30 Flowlevi Downs", new CollectItemObjective(FlowleviDownId, FlowleviDownAmount));
		AddObjective("collect_flowlonleaves", "Collect 30 Flowlon Leaves", new CollectItemObjective(FlowlonLeafId, FlowlonLeafAmount));
		AddObjective("collect_bloomleaves", "Collect 30 Bloom Leaves", new CollectItemObjective(BloomLeafId, BloomLeafAmount));
		AddObjective("collect_virdneyleaves", "Collect 30 Virdney Leaves", new CollectItemObjective(VirdneyLeafId, VirdneyLeafAmount));
		AddReward(new ItemReward(WhiteFlowerHeadgearId, 1));

		AddNpc(150183, L("[Ex-Refugee] Ohla"), "f_whitetrees_56_1", -75, 651, 0, OhlaDialog);
	}

	private async Task OhlaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Ohla"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*looking around*{/}"));
			await dialog.Msg(L("Me and my daughter used to be refugees living in this area. Although our situation has improved since then, I do come here occasionally. It's nostalgic, in a way."));
			await dialog.Msg(L("I see this place is still plagued with monsters. Would you help me in culling their numbers? Just bring me materials from the monsters around here."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you defeated monsters from around here?"),
				Option(L("Yes, I have."), "complete", () => player.Inventory.HasItem(FlowleviDownId, FlowleviDownAmount) && player.Inventory.HasItem(FlowlonLeafId, FlowlonLeafAmount) && player.Inventory.HasItem(BloomLeafId, BloomLeafAmount) && player.Inventory.HasItem(VirdneyLeafId, VirdneyLeafAmount)),
				Option(L("There are still more monsters to defeat."), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(FlowleviDownId, FlowleviDownAmount) &&
					player.Inventory.HasItem(FlowlonLeafId, FlowlonLeafAmount) &&
					player.Inventory.HasItem(BloomLeafId, BloomLeafAmount) &&
					player.Inventory.HasItem(VirdneyLeafId, VirdneyLeafAmount))
				{
					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*inspects the materials*{/}"));
					await dialog.Msg(L("Thank you. Although we have long moved away from here, I am happy to see this area becoming a nicer place."));
					await dialog.Msg(L("I know it isn't much, but I'd like to give you this. I appreciate you defeating these monsters."));
				}
			}
			else
			{
				await dialog.Msg(L("You will find all types of monsters at different parts of Mishekan Forest."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you do that for me?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Thank you. Bring me 30 Flowlevi Downs, Flowlon Leaves, Bloom Leaves and Virdney Leaves. That should be enough proof you have cleaned up the area."));
				await dialog.Msg(L("With this, hopefully this area becomes safer."));
				break;
			case "decline":
				await dialog.Msg(L("{#666666}*looks at the monsters from a distance*"));
				break;
		}
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.RemoveItem(FlowleviDownId, FlowleviDownAmount);
		character.Inventory.RemoveItem(FlowlonLeafId, FlowlonLeafAmount);
		character.Inventory.RemoveItem(BloomLeafId, BloomLeafAmount);
		character.Inventory.RemoveItem(VirdneyLeafId, VirdneyLeafAmount);
	}
}
