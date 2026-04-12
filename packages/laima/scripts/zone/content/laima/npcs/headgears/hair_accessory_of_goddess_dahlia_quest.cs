using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors.Characters;

public class GoddessDahliaQuestScript : QuestScript
{
	private const int QuestNumber = 10057;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Silent Prayers";
	private const string QuestDescription = "Assist Sancti in order to drive out the evil energy in the Tower.";

	private const int MinivernSkinId = 645407;
	private const int MinivernSkinAmount = 30;
	private const int DimmerMarbleId = 645205;
	private const int DimmerMarbleAmount = 10;
	private const int BlackDrakeHeartId = 645206;
	private const int BlackDrakeHeartAmount = 20;
	private const int BlackShamanDollCoreId = 645204;
	private const int BlackShamanDollCoreAmount = 30;
	private const int DahliaAccessoryId = 11041002;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetType(QuestType.Repeat);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_minvernskin", "Collect 30 Minivern Skins", new CollectItemObjective(MinivernSkinId, MinivernSkinAmount));
		AddObjective("collect_dimmermarbles", "Collect 10 Dimmer Marbles", new CollectItemObjective(DimmerMarbleId, DimmerMarbleAmount));
		AddObjective("collect_drakehearts", "Collect 20 Black Drake Hearts", new CollectItemObjective(BlackDrakeHeartId, BlackDrakeHeartAmount));
		AddObjective("collect_shamancores", "Collect 30 Black Shaman Doll Cores", new CollectItemObjective(BlackShamanDollCoreId, BlackShamanDollCoreAmount));
		AddReward(new ItemReward(DahliaAccessoryId, 1));

		AddNpc(147491, L("[Believer] Sancti"), "d_firetower_45", -1801, -672, 90, SanctiDialog);
	}

	private async Task SanctiDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Sancti"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*praying with her eyes shut*{/}"));
			await dialog.Msg(L("Greetings Adventurer, what brings you here? Did you know that this area is rumored to have traces of a Goddess? With this place overrun by monsters it proves difficult to establish a spiritual connection through prayer."));
		}
		else
		{
			var selection = await dialog.Select(L("Are you done?"),
				Option(L("Yes, I have cleared the vicinity"), "complete", () => player.Inventory.HasItem(MinivernSkinId, MinivernSkinAmount) && player.Inventory.HasItem(DimmerMarbleId, DimmerMarbleAmount) && player.Inventory.HasItem(BlackDrakeHeartId, BlackDrakeHeartAmount) && player.Inventory.HasItem(BlackShamanDollCoreId, BlackShamanDollCoreAmount)),
				Option(L("There are still creatures lurking around"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(MinivernSkinId, MinivernSkinAmount) &&
					player.Inventory.HasItem(DimmerMarbleId, DimmerMarbleAmount) && player.Inventory.HasItem(BlackDrakeHeartId, BlackDrakeHeartAmount) && player.Inventory.HasItem(BlackShamanDollCoreId, BlackShamanDollCoreAmount))
				{
					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*the monster remains were magically purified into thin air*{/}"));
					await dialog.Msg(L("This must be prove that our efforts paid off."));
					await dialog.Msg(L("Take this as a token of my gratitude. Perhaps you can assist me once more to drive out the remaining monsters?"));
				}
			}
			else
			{
				await dialog.Msg(L("Please restore this place to it's former glory."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Will you help me drive out the monsters?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Thank you. Clearing out the evil energy around here should improve my prayers."));
				await dialog.Msg(L("Please collect 30 Minivern Skins 10 Dimmer Marbles 20 Black Drake Hearts and 30 Black Shaman Doll Cores as proof of your efforts."));
				break;
			case "decline":
				await dialog.Msg(L("{#666666}*continues praying*"));
				break;
		}
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.RemoveItem(MinivernSkinId, MinivernSkinAmount);
		character.Inventory.RemoveItem(DimmerMarbleId, DimmerMarbleAmount);
		character.Inventory.RemoveItem(BlackDrakeHeartId, BlackDrakeHeartAmount);
		character.Inventory.RemoveItem(BlackShamanDollCoreId, BlackShamanDollCoreAmount);
	}
}
