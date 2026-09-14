using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Scripting.Extensions.Keywords;
using Melia.Zone.Scripting.Extensions.LivelyDialog;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Yggdrasil.Extensions;
using static Melia.Zone.Scripting.Shortcuts;
using static Melia.Zone.Scripting.Extensions.Keywords.Shortcuts;
using Melia.Shared.World;

public class RangerMasterNpc : GeneralScript
{
	private const int QuestId = 10_000_000 + (int)JobId.Ranger;
	private const int RangerMasterId = MonsterId.Npc_RAG_Master;
	private const string RangerMasterName = "Faustus"; // Replace with the actual name
	private const string RangerMasterLocation = "c_orsha";
	private const int RequiredLevel = 15;
	private const int HiddenQuestItemId = ItemId.TOSHero_BOW; // Replace with a suitable powerful bow ID
	private const int QuestItemCount = 1;

	private const int CombatTrialMapId = 523; // Replace with a suitable map ID
	private const int CombatTrialDuration = 180; // Seconds
	private const int CombatTrialMobId = MonsterId.Goblin_Archer; // Replace with a challenging mob ID
	private const int TrackingTestTargetMobId = MonsterId.Goblin_Archer; // Replace with a trackable mob ID

	protected override void Load()
	{
		// Register the "Ranger" keyword
		RegisterKeyword("RANGER", L("Ranger"),
			L("A skilled archer who excels in tracking and eliminating single targets, wielding both bows and crossbows with deadly precision."));

		// Add the Ranger Master NPC
		AddNpc(RangerMasterId, L($"[Ranger Master] {RangerMasterName}"), RangerMasterLocation, -8, 613, 0,
			RangerMasterDialog);

		// Prepare the combat trial map
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var _) &&
			ZoneServer.Instance.Data.MapDb.TryFind(CombatTrialMapId, out var mapData))
		{
			var combatTrialMap = new DynamicMap(mapData.Id);
			ZoneServer.Instance.World.AddMap(combatTrialMap);
		}
	}

	private async Task RangerMasterDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L($"Ranger Master {RangerMasterName}"));
		// No ranger master portrait
		//dialog.SetPortrait("Dlg_port_RANGER_MASTER");

		dialog.UpdateRelation();

		var continueDialog = true;
		while (continueDialog)
		{
			if (character.JobId == JobId.Archer)
			{
				if (dialog.GetMemory() == 0)
				{
					await dialog.Intro(L(
						"Welcome, archer. I am Faustus, a master of the Rangers. We are independent hunters, skilled in tracking and eliminating our targets."));
				}

				if (character.Job.Level >= RequiredLevel && character.Quests.IsActive(QuestId))
				{
					if (!character.Variables.Perm.GetBool("RangerQuestMasterMet", false))
					{
						if (character.Variables.Perm.GetBool("FoundRangerFieldNotes", false) &&
							!character.Variables.Perm.GetBool("HiddenRangerQuestActive", false))
						{
							await OfferHiddenQuest(dialog);
						}
						else
						{
							continueDialog = await OfferQuest(dialog);
						}

						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("RangerKnowledgeTestPassed", false))
					{
						continueDialog = await KnowledgeTest(dialog);

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Are you ready to continue on the path of the Ranger?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L("A Ranger knows patience is key. Return when your mind is clear."));
								continueDialog = false;
							}
						}
					}
					else if (!character.Variables.Perm.GetBool("RangerCombatTrialPassed", false))
					{
						continueDialog = await CombatTrial(dialog);
						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("RangerTrackingTestPassed", false))
					{
						continueDialog = await TrackingTest(dialog); // New trial for Ranger

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Have you honed your senses to become a true hunter?"),
								Option(L("Yes"), "continue"),
								Option(L("Not yet"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"A Ranger's senses are their greatest weapon. Return when you have mastered the art of tracking."));
								continueDialog = false;
							}
						}
					}
					else
					{
						await CompleteJobChange(dialog);
						continueDialog = false;
					}
				}
				else
				{
					await dialog.Msg(
						LF(
							"You have potential, but you need to hone your skills. Come back when you reach level {0}.",
							RequiredLevel));
					continueDialog = false;
				}
			}
			else if (character.JobId == JobId.Ranger)
			{
				await dialog.Msg(
					L("Ah, a fellow Ranger! May your aim be true and your steps silent. Remember, a Ranger is always one with the wilderness."));
				continueDialog = false;
			}
			else
			{
				await dialog.Msg(
					L("Greetings, traveler. My teachings are for those who have already embraced the way of the Archer."));
				continueDialog = false;
			}
		}
	}

	private async Task<bool> OfferQuest(Dialog dialog)
	{
		var character = dialog.Player;

		var response = await dialog.Select(
			L(
				"The Ranger's path is one of solitude and mastery over the wild. Are you prepared for the challenges that await?"),
			Option(L("Tell me more about Rangers."), "moreInfo",
				() => !character.Variables.Temp.GetBool("RangerInfoSeen", false)),
			Option(L("Yes, I wish to become a Ranger!"), "acceptQuest"),
			Option(L("I need time to consider."), "declineQuest")
		);

		switch (response)
		{
			case "moreInfo":
				await dialog.Msg(
					L(
						"Rangers are experts in tracking, stealth, and precise ranged attacks. We use both bows and crossbows to eliminate our targets with swiftness and efficiency."));
				dialog.ModifyMemory(1);
				character.Variables.Temp.Set("RangerInfoSeen", true);
				return true;
			case "acceptQuest":
				await dialog.Msg(
					L(
						"Very well. The Ranger's path requires dedication and a deep connection to nature. Prove your worth through these trials."));
				character.Variables.Perm.Set("RangerQuestMasterMet", true);
				character.Quests.CompleteObjective(QuestId, "talkToFaustus");
				dialog.ModifyFavor(1);
				return true;
			case "declineQuest":
				await dialog.Msg(
					L(
						"The wilderness is not for the faint of heart. Choose your path carefully. Return when you are ready."));
				return false;
			default:
				return true;
		}
	}

	private async Task OfferHiddenQuest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"You found a Ranger's field notes detailing a unique bow crafting technique."));
		var response = await dialog.Select(
			L(
				"Are you interested in learning this secret and undertaking a special challenge?"),
			Option(L("Yes, I seek this knowledge."), "acceptHidden"),
			Option(L("No, I will follow the traditional path."), "declineHidden")
		);

		switch (response)
		{
			case "acceptHidden":
				await dialog.Msg(L(
					"A true Ranger never stops learning. Return to me when you have crafted the bow described in the notes."));
				character.Variables.Perm.Set("RangerQuestMasterMet", true);
				character.Variables.Perm.Set("HiddenRangerQuestActive", true);
				character.Quests.CompleteObjective(QuestId, "talkToFaustus");
				dialog.ModifyFavor(5);
				break;
			case "declineHidden":
				await dialog.Msg(
					L(
						"The traditional path is challenging enough. You can always seek this knowledge later."));
				break;
		}
	}

	private async Task<bool> KnowledgeTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L(
			"A Ranger's knowledge of nature and tracking is crucial. Let's test your understanding."));

		var question1 = await dialog.Select(
			L("What is the best way to track a wounded animal?"),
			Option(L("Follow the scent of blood."), "correct"),
			Option(L("Listen for its cries of pain."), "wrong"),
			Option(L("Look for broken branches and disturbed foliage."), "wrong")
		);

		var question2 = await dialog.Select(
			L("Which of these is NOT a tool used by Rangers for tracking?"),
			Option(L("Binoculars."), "wrong"),
			Option(L("A magnifying glass."), "correct"),
			Option(L("Footprints and other signs."), "wrong")
		);

		if (question1 == "correct" && question2 == "correct")
		{
			await dialog.Msg(L("Your knowledge of the wild is impressive!"));
			character.Variables.Perm.Set("RangerKnowledgeTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "knowledgeTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(L(
				"Your tracking skills require more practice. Spend more time in the wilderness and hone your senses."));
			dialog.ModifyStress(1);
			return false;
		}
	}

	private async Task<bool> CombatTrial(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"A Ranger's agility and precision in combat are unmatched. Show me your skills in this trial."));
		var choice = await dialog.Select(L("Are you prepared to face the challenge?"),
			Option(L("Yes, I am ready."), "enterTrial"),
			Option(L("I require further training."), "declineTrial")
		);

		if (choice == "declineTrial")
		{
			await dialog.Msg(L(
				"Practice your archery and footwork. Return when you are confident in your abilities."));
			return false;
		}

		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var combatTrialMap))
		{
			await dialog.Msg(L("The trial grounds are currently unavailable."));
			return false;
		}

		character.Warp(combatTrialMap.Id, combatTrialMap.Data.DefaultPosition);

		var enemies = new List<Mob>();
		for (var i = 0; i < 5; i++)
		{
			var enemy = new Mob(CombatTrialMobId, RelationType.Enemy);
			enemy.Components.Add(new MovementComponent(enemy));
			enemy.Components.Add(new AiComponent(enemy, "BasicMonster"));
			enemy.Components.Add(new LifeTimeComponent(enemy, TimeSpan.FromSeconds(CombatTrialDuration)));
			enemy.Visibility = ActorVisibility.Individual;
			enemy.VisibilityId = character.ObjectId;
			enemy.HasDrops = false;
			enemy.HasExp = false;
			combatTrialMap.AddMonster(enemy);
			enemies.Add(enemy);

			enemy.Died += (mob, killer) => enemies.Remove(mob);
		}

		return true;
	}

	private async Task<bool> TrackingTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L(
			"A Ranger's true skill lies in their ability to track and hunt. Prove your worth in this trial."));

		await dialog.Msg(
			LF("Track down and eliminate {0} {1}s. Use your senses and skills to locate your prey.", 5,
				L(ZoneServer.Instance.Data.MonsterDb.Find(TrackingTestTargetMobId).Name)));

		var choice = await dialog.Select(L("Are you ready to begin the hunt?"),
			Option(L("Yes, I will track them down."), "startTracking"),
			Option(L("I need to sharpen my senses first."), "declineTracking")
		);

		if (choice == "declineTracking")
		{
			await dialog.Msg(L("Return when your senses are honed and your spirit is focused."));
			return false;
		}

		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var trackingMap))
		{
			await dialog.Msg(L("The hunting grounds are currently unavailable."));
			return false;
		}

		// Start the Tracking Test
		character.Variables.Temp.SetInt("RangerTrackingTestCount", 0);
		character.Warp(trackingMap.Id, trackingMap.Data.DefaultPosition);

		// Set up a listener for when the player kills the target mob
		ZoneServer.Instance.ServerEvents.EntityKilled.Subscribe((sender, args) =>
		{
			if (args.Attacker == character && args.Target is Mob mob &&
				mob.Data.Id == TrackingTestTargetMobId)
			{
				var currentCount = character.Variables.Temp.GetInt("RangerTrackingTestCount");
				character.Variables.Temp.SetInt("RangerTrackingTestCount", currentCount + 1);
				character.WorldMessage(
					LF("Tracking Test Progress: {0}/5 {1}s eliminated.", currentCount + 1,
						L(mob.Data.Name)));

				if (currentCount + 1 >= 5)
				{
					character.WorldMessage(L("You have successfully completed the Ranger Tracking Test!"));
					character.Variables.Perm.Set("RangerTrackingTestPassed", true);
					character.Quests.CompleteObjective(QuestId, "trackingTest");

					// Warp the player back to the Ranger Master after a delay
					Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(_ =>
					{
						character.Warp(1001, new Position(150, 0, 70)); // Adjust position as needed
					});
				}
			}
		});

		return true;
	}

	private async Task CompleteJobChange(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Variables.Perm.GetBool("HiddenRangerQuestActive", false) &&
			character.Inventory.CountItem(HiddenQuestItemId) >= QuestItemCount)
		{
			await dialog.Msg(
				L(
					"You have crafted a remarkable bow! Your dedication to the Ranger's craft is evident."));
			character.Inventory.Remove(HiddenQuestItemId, QuestItemCount);
			character.Quests.Complete(QuestId);
		}
		else
		{
			await dialog.Msg(L("You have proven yourself worthy.  Rise as a Ranger!"));
			character.Quests.Complete(QuestId);
		}

		character.ChangeJob(JobId.Ranger);
		character.AddKeyword("RANGER");

		await dialog.Msg(L(
			"May your arrows fly true and your footsteps be silent. The wilderness awaits, Ranger."));
		dialog.ModifyFavor(1);
		dialog.ModifyMemory(1);
	}
}

public class RangerJobChangeQuestScript : QuestScript
{
	protected override void Load()
	{
		SetId(10_000_000 + (int)JobId.Ranger); // Unique ID for Ranger quest
		SetName("Path of the Ranger");
		SetDescription("Embrace the way of the Ranger and prove your worth to Faustus.");
		SetAutoTracked(true);
		SetCancelable(true);

		AddObjective("talkToFaustus", "Seek out the Ranger Master", new ManualObjective());
		AddObjective("knowledgeTest", "Pass Faustus's knowledge test", new ManualObjective());
		var objective = AddObjective("combatTrial", "Complete Faustus's combat trial",
			new KillObjective(5, MonsterId.Goblin_Archer)); // Adjust enemy count and type as needed
		objective.Completed += (character, objective) =>
		{
			character.WorldMessage(L("You have passed the Ranger combat trial!"));
			character.Variables.Perm.Set("RangerCombatTrialPassed", true);
		};
		AddObjective("trackingTest", "Demonstrate your tracking skills", new KillObjective(5, MonsterId.Goblin_Archer));

		SetReceive(QuestReceiveType.Manual);
	}

	public override void OnStart(Character character, Quest quest)
	{
		character.Variables.Perm.Set("JobAdvancement", true);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Set("JobAdvancement", false);
	}
}
