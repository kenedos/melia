using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class SnowyReindeerAntlersQuestScript : QuestScript
{
	private const int QuestNumber = 10048;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Spirit of the Frost";
	private const string QuestDescription = "Help an old hunter craft ceremonial Snowy Reindeer Antlers using materials from the frozen creatures of Grand Yard Mesa.";

	private const int BlueTiniHornId = 645676;
	private const int BlueTiniHornAmount = 100;
	private const int BlueHohenManeId = 645723;
	private const int BlueHohenManeAmount = 25;
	private const int SnowyReindeerAntlersId = 628361;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_horns", $"Collect {BlueTiniHornAmount} Blue Tini Horns", new CollectItemObjective(BlueTiniHornId, BlueTiniHornAmount));
		AddObjective("collect_manes", $"Collect {BlueHohenManeAmount} Blue Hohen Manes", new CollectItemObjective(BlueHohenManeId, BlueHohenManeAmount));
		AddReward(new ItemReward(SnowyReindeerAntlersId, 1));

		AddNpc(20117, L("[Hunter] Valdis"), "f_tableland_71", -1141, 679, 90, ValdisDialog);
	}

	private async Task ValdisDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Valdis"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*sits by a small campfire, carefully carving a piece of bone*{/}"));
			await dialog.Msg(L("Ah, a traveler braving the cold! Come, warm yourself by the fire. These frozen plains are unforgiving to those unprepared."));
			await dialog.Msg(L("I am Valdis, a hunter of these lands for many winters now. I've been working on something special - a headdress in honor of the frost spirits that guide us through the harshest blizzards."));
			await dialog.Msg(L("The old tales speak of great reindeer that once roamed these mesas, their antlers crowned with frost and their manes flowing like winter clouds. I seek to capture that spirit in a ceremonial piece."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the materials I need?"),
				Option(L("Yes, I have everything"), "complete", () => player.Inventory.HasItem(BlueTiniHornId, BlueTiniHornAmount) && player.Inventory.HasItem(BlueHohenManeId, BlueHohenManeAmount)),
				Option(L("Still hunting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(BlueTiniHornId, BlueTiniHornAmount) &&
					player.Inventory.HasItem(BlueHohenManeId, BlueHohenManeAmount))
				{
					player.Inventory.RemoveItem(BlueTiniHornId, BlueTiniHornAmount);
					player.Inventory.RemoveItem(BlueHohenManeId, BlueHohenManeAmount);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*examines the materials with practiced hands*{/}"));
					await dialog.Msg(L("Magnificent! These Blue Tini horns have exactly the shape and curvature I envisioned. And the Hohen manes... feel how soft yet resilient they are, like snow that refuses to melt."));
					await dialog.Msg(L("{#666666}*works with surprising speed, binding and shaping the materials*{/}"));
					await dialog.Msg(L("There. The antlers curve just as they did in the old paintings, and the mane provides warmth against even the fiercest winds. Wear these with pride, for you carry the blessing of the frost spirits."));

					await dialog.Msg(L("If the spirits call to you again and you wish for another, I will be here by my fire."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("{#666666}*stokes the fire thoughtfully*{/}"));
				await dialog.Msg(L("The Blue Tinis swarm across these frozen plains in great numbers - small creatures with curved horns that catch the light like ice. And the Blue Hohens, those armored knights of the frost, their manes are prized for their warmth."));
				await dialog.Msg(L("Take care out there. The cold itself is your greatest enemy, not just the creatures."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you help me gather materials for this ceremonial headdress?"),
			Option(L("I will help you"), "accept"),
			Option(L("Perhaps when I'm better prepared"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("{#666666}*nods with quiet gratitude*{/}"));
			await dialog.Msg(L("The frost spirits smile upon you. I need 100 Blue Tini Horns - the small creatures are everywhere in this frozen expanse. Their horns, when properly arranged, will form the antlers."));
			await dialog.Msg(L("I also need 25 Blue Hohen Manes from the armored warriors that patrol these lands. Their flowing manes will provide the finishing touch and keep your ears warm against the bitter cold."));
			await dialog.Msg(L("May the winds guide your hunt, traveler."));
		}
		else
		{
			await dialog.Msg(L("{#666666}*returns to his carving*{/}"));
			await dialog.Msg(L("A wise choice if you feel unprepared. The frozen plains are not kind to those who underestimate them. Return when you are ready, and the fire will still be burning."));
		}
	}
}
