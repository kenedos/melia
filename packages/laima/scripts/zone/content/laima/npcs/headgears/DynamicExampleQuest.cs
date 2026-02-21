using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using Melia.Zone.World.Actors.Characters;

// Import the relationship extensions
using Melia.Zone.Scripting.Extensions.LivelyDialog;

using static Melia.Zone.Scripting.Shortcuts;
using Yggdrasil.Logging;
using Melia.Zone;
using Yggdrasil.Extensions;
using System.Linq; // For Log.Info/Warning

public class DynamicExampleQuestScript : QuestScript
{
	// Define the core quest details
	private const int QuestNumber = 20001;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quest"; // Using namespace from user's code
	private const string QuestName = "Hemlock's Herbs";
	private const string QuestDescription = "Gather some specific herbs for Old Man Hemlock.";

	// Items
	private const int MoonpetalId = ItemId.EXORCIST_JOB_QUEST_ITEM1; // Example Item ID from user's code
	private const int BasicHerbRewardId = ItemId.Event_Adventure_Reward_Seed_1; // Example Reward Item ID from user's code
	private const int GoodHerbRewardId = ItemId.Event_Adventure_Reward_Seed_2; // Example Bonus Reward from user's code

	// NPC ID (consistent with AddNpc call)
	private const int HemlockNpcId = 19999;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.Sequential);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		// --- Base Objectives ---
		// Objective 1: Gather Petals (Hint embedded in description)
		AddObjective("gather_petals", "Gather Moonpetals (Found near Whispering Falls)", new CollectItemObjective(MoonpetalId, 10)); // Base requirement: 10

		// Objective 2: Return to NPC (Using Manual Objective)
		AddObjective("return_hemlock", "Return to Old Man Hemlock", new ManualObjective());

		// --- Base Rewards ---
		AddReward(new ExpReward(500)); // Base XP (Using ExpReward from user's code)
		AddReward(new ItemReward(BasicHerbRewardId, 1)); // Base item reward

		// --- NPC Definition ---
		// Ensure NPC ID matches the one used in objective if needed, and matches HemlockNpcId const
		var mapData = ZoneServer.Instance.Data.MapDb.Entries.Values.Where(m => m.Type == Melia.Shared.Game.Const.MapType.Field).Random();
		if (!ZoneServer.Instance.World.TryGetMap(mapData.ClassName, out var map))
			mapData = ZoneServer.Instance.Data.MapDb.Entries.Values.Where(m => m.Type == Melia.Shared.Game.Const.MapType.Field).Random();
		if (!map.Ground.TryGetRandomPosition(out var position))
			map.Ground.TryGetRandomPosition(out position);
		//Log.Debug($"Spawned Old Man Hemlock at {map.ClassName} {position.X} {position.Z}");
		//AddNpc(HemlockNpcId, L("[Herbalist] Old Man Hemlock"), mapData.ClassName, position.X, position.Z, 0, HemlockDialog);

		// Optionally, register keywords if the quest teaches one
		// RegisterKeyword("herb_knowledge", "Herb Knowledge", "Learned about common medicinal herbs.");
	}

	// --- NPC Dialog Logic ---
	private async Task HemlockDialog(Dialog dialog)
	{
		var player = dialog.Player;
		// Check if the interaction is with the correct NPC, might be redundant if script only attached to one NPC ID
		if (dialog.Npc.Id != HemlockNpcId) return;

		var npc = dialog.Npc;

		dialog.SetTitle(L(npc.Name));
		// dialog.SetPortrait("HEMLOCK_DLG");

		var status = dialog.GetRelationshipStatus();
		await dialog.Intro(L($"You approach an old man tending to a small garden. He looks up as you get closer, his eyes assessing you."));
		await dialog.Msg(dialog.GetMoodMessage());

		dialog.UpdateRelation();
		// await dialog.DebugRelation(); // Optional Debug

		var hasActiveQuest = player.Quests.IsActive(QuestId);

		if (hasActiveQuest)
		{
			await HandleActiveQuest(dialog, player, status);
		}
		else
		{
			// Also check if player already completed it, maybe offer repeatable logic?
			if (player.Quests.HasCompleted(QuestId))
			{
				// Example repeatable logic - maybe offer again after cooldown or with different requirements?
				await dialog.Msg(L("Good to see you again. Need more herbs, perhaps? My supplies are always dwindling..."));
				// Add logic here to potentially re-offer the quest if it's designed to be repeatable
				// For now, just end the conversation politely.
				await dialog.Msg(L("Always appreciate your help, though."));
			}
			else
			{
				await HandleQuestOffer(dialog, player, status);
			}
		}
	}

	// --- Helper: Handle Offering the Quest ---
	private async Task HandleQuestOffer(Dialog dialog, Character player, RelationshipStatus status)
	{
		// **Greeting based on Recall Level** (Same as before)
		switch (status.RecallLevel)
		{
			case RecallLevel.FirstMeeting:
				await dialog.Msg(L("Hmmph. Haven't seen you around here before. What brings you to my little corner of the woods?"));
				dialog.ModifyRelation(memory: 1, favor: 1, stress: 0);
				break;
			case RecallLevel.Recognition:
			case RecallLevel.Familiarity:
				await dialog.Msg(L($"Ah, {player.Name}, isn't it? Back again?"));
				dialog.ModifyRelation(memory: 1, favor: 0, stress: 0);
				break;
			case RecallLevel.Acquaintance:
			case RecallLevel.Remembrance:
				await dialog.Msg(L($"Well met, {player.Name}. Good to see a familiar face."));
				dialog.ModifyRelation(memory: 1, favor: 1, stress: 0);
				break;
		}

		// **Prerequisite Checks** (Same as before)
		if (player.Level < 5)
		{
			await dialog.Msg(L("This task requires a bit more experience, youngster. Come back when you're level 5 or higher."));
			dialog.ModifyRelation(memory: 0, favor: -1, stress: 1);
			return;
		}
		if (status.Favor < 5 && status.RecallLevel < RecallLevel.Familiarity)
		{
			await dialog.Msg(L("I don't know you well enough to ask for favors. Perhaps chat with me some more another time."));
			dialog.ModifyRelation(memory: 0, favor: 0, stress: 1);
			return;
		}

		// --- Offer the Quest --- (Same as before, storing dynamic requirement)
		var requiredAmount = 10 + (player.Level / 5) + (status.Favor / 10);
		requiredAmount = Math.Max(5, requiredAmount);
		player.Variables.Temp.SetInt($"Quest_{QuestNumber}_ReqAmount", requiredAmount);

		var offerText = status.Favor >= 20
			? L($"My friend, I could use your sharp eyes. I need about {requiredAmount} Moonpetals for a poultice. They grow near the Whispering Falls. Think you could help an old man out?")
			: L($"You look capable. I'm running low on Moonpetals. Need around {requiredAmount} of 'em. They grow near the Whispering Falls. Interested in gathering some?");

		var selection = await dialog.Select(offerText,
			Option(L("I'll gather them for you."), "accept"),
			Option(L("What are Moonpetals used for?"), "ask_usage"),
			Option(L("Sorry, I can't right now."), "decline")
		);

		switch (selection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Excellent! Be careful out there. Those petals bruise easily. Come back when you have them all."));
				dialog.ModifyRelation(memory: 2, favor: 5, stress: -1);
				break;
			case "ask_usage":
				await dialog.Msg(L("Ah, curious are we? Moonpetals have soothing properties. Excellent for teas and calming salves. Now, about gathering them...?"));
				dialog.ModifyRelation(memory: 1, favor: 1, stress: 0);
				await HandleQuestOffer(dialog, player, status); // Re-offer
				break;
			case "decline":
				await dialog.Msg(L("Hmph. Suit yourself. Finding reliable help these days... *grumbles*"));
				dialog.ModifyRelation(memory: 0, favor: -2, stress: 1);
				break;
		}
	}

	// --- Helper: Handle Active Quest State ---
	private async Task HandleActiveQuest(Dialog dialog, Character player, RelationshipStatus status)
	{
		// Try to get the quest instance
		if (!player.Quests.TryGet(QuestId.Value, out var quest))
		{
			// Should not happen if IsActive is true, but good practice to check
			Log.Warning($"Quest {QuestId}: Could not retrieve active quest instance for {player.Name}.");
			await dialog.Msg(L("Hmm, my memory must be failing me. I thought you were helping me with something... Never mind."));
			return;
		}

		// Check progress on the first objective (gathering)
		if (quest.TryGetProgress("gather_petals", out var gatherProgress) && !gatherProgress.Done)
		{
			await dialog.Msg(L("How goes the herb gathering? Have you found the Moonpetals yet?"));
			await dialog.Msg(L("Remember, they grow near the Whispering Falls. Keep searching!"));
			dialog.ModifyRelation(memory: 0, favor: -1, stress: 1); // Slight impatience if they talk before gathering
			return; // Player needs to complete gathering first
		}

		// If gathering is done, check the return objective
		if (quest.TryGetProgress("return_hemlock", out var returnProgress) && returnProgress.Unlocked && !returnProgress.Done)
		{
			// This is the stage where the player should turn in items
			await dialog.Msg(L("Ah, you're back! Did you manage to gather the Moonpetals?"));

			var requiredAmount = player.Variables.Temp.GetInt($"Quest_{QuestNumber}_ReqAmount", 10);
			var hasItems = player.Inventory.HasItem(MoonpetalId, requiredAmount);

			var options = new List<DialogOption> { Option(L("I haven't found enough yet."), "progress") };
			if (hasItems)
			{
				options.Insert(0, Option(L($"Yes, I have {requiredAmount} Moonpetals right here."), "complete"));
			}

			var selection = await dialog.Select(L("Let's see what you've brought."), options.ToArray());

			if (selection == "complete" && hasItems)
			{
				// **Complete the Manual Objective**
				player.Inventory.RemoveItem(MoonpetalId, requiredAmount); // Remove items first
				returnProgress.SetDone(); // Mark this manual step as done

				// **Explicitly Complete the Quest**
				// Even if all objectives are done, this ensures OnComplete etc. trigger correctly.
				player.Quests.Complete(QuestId);

				// Give rewards and feedback (Same as before)
				var completionText = status.Favor >= 30
					? L("Wonderful! Just what I needed. You've been a great help, truly. Take this for your trouble.")
					: L("Took you long enough, but these look right. Here's your payment.");
				await dialog.Msg(completionText);

				if (status.Favor >= 40)
				{
					player.AddItem(GoodHerbRewardId, 1); // Use AddItem as Inventory.AddItem might not exist
					await dialog.Msg(L("(Hemlock slips you an extra bundle of potent herbs with a wink.)"));
					dialog.ModifyRelation(memory: 1, favor: 3, stress: -1);
				}

				dialog.ModifyRelation(memory: 3, favor: 10, stress: -2);
				player.Variables.Temp.Remove($"Quest_{QuestNumber}_ReqAmount");
			}
			else if (selection == "progress")
			{
				await dialog.Msg(L("Keep at it then. I'll be here when you have the right amount."));
				dialog.ModifyRelation(memory: 0, favor: -1, stress: 1);
			}
			else // Error case: selected complete but didn't have items
			{
				await dialog.Msg(L("Hmm, doesn't look like you have the right amount. Are you sure you counted correctly?"));
				dialog.ModifyRelation(memory: 0, favor: -2, stress: 2);
			}
		}
		else
		{
			// Quest is active, but objectives might be completed or in an unexpected state
			Log.Warning($"Quest {QuestId}: Player {player.Name} interacted while quest active, but objective state unexpected. GatherDone: {gatherProgress?.Done}, ReturnUnlocked: {returnProgress?.Unlocked}, ReturnDone: {returnProgress?.Done}");
			await dialog.Msg(L("Good to see you, but I don't think there's anything more for us to discuss about those herbs right now."));
		}
	}

	// --- Quest Lifecycle Callbacks (Same as before) ---

	public override void OnStart(Character character, Quest quest)
	{
		Log.Info($"Character {character.Name} started quest {QuestId.Value} ({QuestName}).");
		base.OnStart(character, quest);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		Log.Info($"Character {character.Name} completed quest {QuestId.Value} ({QuestName}).");
		// Clean up temp variable just in case it wasn't removed during completion dialog
		character.Variables.Temp.Remove($"Quest_{QuestNumber}_ReqAmount");
		base.OnComplete(character, quest);
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		Log.Info($"Character {character.Name} succeeded objectives for quest {QuestId.Value} ({QuestName}).");
		base.OnSuccess(character, quest);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		Log.Info($"Character {character.Name} cancelled quest {QuestId.Value} ({QuestName}).");
		// Clean up temp variable on cancel too
		character.Variables.Temp.Remove($"Quest_{QuestNumber}_ReqAmount");
		base.OnCancel(character, quest);
	}
}
