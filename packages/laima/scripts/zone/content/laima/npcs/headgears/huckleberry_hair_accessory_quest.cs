//--- Melia Script -----------------------------------------------------------
// Huckleberry Hair Accessory Quest
//--- Description -----------------------------------------------------------
// A quest to gather materials from White Pino and White Geppetto monsters
// at Tenants' Farm to craft a Huckleberry Hair Accessory.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class HuckleberryHairAccessoryQuestScript : QuestScript
{
	private const int QuestNumber = 10044;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Pollen Season";
	private const string QuestDescription = "Help the farmer's daughter craft a decorative hair accessory using materials from the local plant creatures.";

	private const int PinoStemId = 645476;
	private const int PinoStemAmount = 50;
	private const int GeppettoLeafId = 645445;
	private const int GeppettoLeafAmount = 25;
	private const int HuckleberryHairAccessoryId = 628145;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_pino_stems", "Collect 50 Pino Stems",
			new CollectItemObjective(PinoStemId, PinoStemAmount));
		AddObjective("collect_geppetto_leaves", "Collect 25 Geppetto Leaves",
			new CollectItemObjective(GeppettoLeafId, GeppettoLeafAmount));

		AddReward(new ItemReward(HuckleberryHairAccessoryId, 1));

		AddNpc(20114, L("[Farmer's Daughter] Lina"), "f_farm_47_1", -1190, -395, 90, LinaDialog);
	}

	private async Task LinaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Lina"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*brushes pollen from her hair*{/}"));
			await dialog.Msg(L("Oh, hello there! You've caught me at the worst time of year. The pollen from these creatures is just everywhere during this season."));
			await dialog.Msg(L("But you know what? My grandmother always said, 'If you can't beat them, make something beautiful from them.' I've been wanting to craft a hair accessory using parts from these little plant monsters."));
			await dialog.Msg(L("The stems from the Pinos are flexible and hold their shape nicely, and the leaves from the Geppettos have this lovely purple hue that reminds me of wild huckleberries."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials I need?"),
				Option(L("Yes, here they are"), "complete",
					() => player.Inventory.HasItem(PinoStemId, PinoStemAmount) &&
						  player.Inventory.HasItem(GeppettoLeafId, GeppettoLeafAmount)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(PinoStemId, PinoStemAmount) &&
					player.Inventory.HasItem(GeppettoLeafId, GeppettoLeafAmount))
				{
					player.Inventory.RemoveItem(PinoStemId, PinoStemAmount);
					player.Inventory.RemoveItem(GeppettoLeafId, GeppettoLeafAmount);
					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*carefully weaves the materials together*{/}"));
					await dialog.Msg(L("Perfect! The stems are just the right texture, and look how the leaves catch the light."));
					await dialog.Msg(L("Here, take this as thanks for your help. I made two, so we can both remember this pollen season fondly!"));
				}
			}
			else
			{
				await dialog.Msg(L("The white Pinos are all around the vineyards here, and the Geppettos tend to wander near the pollen clouds. Watch out though, they can be feisty when disturbed!"));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather some materials?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not right now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Thank you so much! I need 50 Pino Stems from the white Pinos, and 25 Geppetto Leaves from the white Geppettos."));
				await dialog.Msg(L("They're scattered all around the farm. The Pinos especially love hanging around the vineyard areas where the pollen is thickest."));
				break;
			case "decline":
				await dialog.Msg(L("That's alright. The pollen will be here for a while yet if you change your mind."));
				break;
		}
	}
}
