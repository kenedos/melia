using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class AngelWingsQuestScript : QuestScript
{
	private const int QuestNumber = 10035;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Guardian's Rest";
	private const string QuestDescription = "Help the tomb keeper Seraphiel craft Angel Wings using sacred materials from the Goddess' Ancient Garden.";

	private const int ResinId = 645427;
	private const int ResinAmount = 75;
	private const int InfroburkShellId = 645190;
	private const int InfroburkShellAmount = 50;
	private const int AngelWingsId = 628064;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_resin", "Collect 75 Resin", new CollectItemObjective(ResinId, ResinAmount));
		AddObjective("collect_shells", "Collect 50 Infroburk Shells", new CollectItemObjective(InfroburkShellId, InfroburkShellAmount));
		AddReward(new ItemReward(AngelWingsId, 1));

		AddNpc(20168, L("[Tomb Keeper] Seraphiel"), "f_remains_38", -1733, -1711, 90, SeraphielDialog);
	}

	private async Task SeraphielDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Seraphiel"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*stands before the ancient tomb, head bowed in silent prayer*{/}"));
			await dialog.Msg(L("You walk upon hallowed ground, traveler. This tomb holds the remains of one who once served the goddesses directly - a guardian whose wings carried divine light across the land."));
			await dialog.Msg(L("I have long wished to honor their memory with a fitting tribute. With the right materials, I could craft wings that echo their former glory..."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials for the wings?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(ResinId, ResinAmount) && player.Inventory.HasItem(InfroburkShellId, InfroburkShellAmount)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(ResinId, ResinAmount) &&
					player.Inventory.HasItem(InfroburkShellId, InfroburkShellAmount))
				{
					player.Inventory.RemoveItem(ResinId, ResinAmount);
					player.Inventory.RemoveItem(InfroburkShellId, InfroburkShellAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*takes the materials with reverent care, crafting beneath the shadow of the tomb*{/}"));
					await dialog.Msg(L("The resin of the ancient trees holds memories of this garden's sacred past. The shells provide the structure, resilient yet light. Together... they become something divine."));
					await dialog.Msg(L("Take these wings, friend. Though you are not the guardian who rests here, perhaps their blessing now extends to you."));

					await dialog.Msg(L("Should you wish to craft another pair, return to me."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("The Long-Branched Trees hold sacred resin within their bark. The Infroburks wander the northern paths near here. Both carry echoes of this garden's ancient power."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials to craft a pair of Angel Wings?"),
			Option(L("I will honor the guardian"), "accept"),
			Option(L("Perhaps another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Bless you, traveler. I need 75 Resin from the Long-Branched Trees that grow throughout the garden, and 50 Infroburk Shells from the Infroburks that patrol the northern areas. May the guardian's light guide your steps."));
		}
		else
		{
			await dialog.Msg(L("{#666666}*turns back toward the tomb*{/} The guardian waits patiently. They have waited for centuries... a little longer matters not."));
		}
	}
}
