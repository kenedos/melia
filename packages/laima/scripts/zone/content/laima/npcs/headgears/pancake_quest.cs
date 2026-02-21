using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class PancakeQuestScript : QuestScript
{
	private const int QuestNumber = 10032;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Honey Harvest Breakfast";
	private const string QuestDescription = "Help the traveling cook Morella create a special honey-glazed pancake hat at Dina Bee Farm.";

	private const int HoneybeanStingerId = 645593;
	private const int HoneybeanStingerAmount = 100;
	private const int RabbeeFurId = 645592;
	private const int RabbeeFurAmount = 75;
	private const int PancakeHatId = 628195;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_stingers", "Collect 100 Honeybean Stingers", new CollectItemObjective(HoneybeanStingerId, HoneybeanStingerAmount));
		AddObjective("collect_fur", "Collect 75 Rabbee Fur", new CollectItemObjective(RabbeeFurId, RabbeeFurAmount));
		AddReward(new ItemReward(PancakeHatId, 1));

		AddNpc(20114, L("[Cook] Morella"), "f_siauliai_46_4", -237, -983, 0, MorellaDialog);
	}

	private async Task MorellaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Morella"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*wipes flour from her apron while eyeing the buzzing bees nearby*{/}"));
			await dialog.Msg(L("Ah, a fellow traveler! You've caught me at quite the predicament. I came to this farm seeking the legendary honey of the Honeybeans, but these creatures are far more... aggressive than I anticipated."));
			await dialog.Msg(L("You see, I'm a traveling cook, and I've been experimenting with a most peculiar recipe - a hat that looks exactly like a golden, fluffy pancake! But to capture that perfect honey glaze, I need materials from these bees."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials for the Pancake hat?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(HoneybeanStingerId, HoneybeanStingerAmount) && player.Inventory.HasItem(RabbeeFurId, RabbeeFurAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(HoneybeanStingerId, HoneybeanStingerAmount) &&
					player.Inventory.HasItem(RabbeeFurId, RabbeeFurAmount))
				{
					player.Inventory.RemoveItem(HoneybeanStingerId, HoneybeanStingerAmount);
					player.Inventory.RemoveItem(RabbeeFurId, RabbeeFurAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*excitedly takes the materials and begins crafting*{/}"));
					await dialog.Msg(L("Magnificent! The stingers will give it that authentic honey sheen, and the fur will make it wonderfully soft - just like a real pancake fresh off the griddle! Here, take this. May it bring sweetness to your adventures!"));
				}
			}
			else
			{
				await dialog.Msg(L("The bees around here can be quite temperamental. The Honeybeans flutter about everywhere, and the Rabbees hop around the southern areas. Be careful of those stingers!"));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather materials for my Pancake creation?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Wonderful! I'll need 100 Honeybean Stingers - they contain a special essence that gives the hat its golden glow. And bring me 75 pieces of Rabbee Fur for the soft, fluffy texture. Together, we'll create the most delicious-looking headwear in all the kingdom!"));
				break;
			case "decline":
				await dialog.Msg(L("I understand. The bees can be quite intimidating. Perhaps another adventurer will help me realize my culinary vision. I'll be here, dreaming of pancakes..."));
				break;
			default:
				break;
		}
	}
}
