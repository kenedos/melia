using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
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
using static Melia.Zone.Scripting.Shortcuts;
using static Melia.Zone.Scripting.Extensions.Keywords.Shortcuts;

public class HighlanderMasterNpc : GeneralScript
{
	private const int HighlanderMasterId = 20029;
	private const string HighlanderMasterName = "Aulus";
	private const string HighlanderMasterMap = "c_highlander";
	private const int RequiredLevel = 15;
	private const int HiddenQuestItemId = ItemId.TSW04_107;
	private const int QuestItemCount = 1;

	private const int CombatTrialMapId = 523;
	private const int CombatTrialDuration = 180; // Seconds
	private const int CombatTrialMobId = MonsterId.Goblin_Archer;

	protected override void Load()
	{
		// Register the "Highlander" keyword
		RegisterKeyword("HIGHLANDER", L("Highlander"),
			L("A skilled swordsman known for their powerful techniques that control the battlefield."));

		// Add the Highlander Master NPC
		AddNpc(HighlanderMasterId, L($"[Highlander Master] {HighlanderMasterName}"),
			HighlanderMasterMap, -15, 62,
			90.0, HighlanderMasterDialog);

		// Prepare the combat trial map 
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var _) &&
			ZoneServer.Instance.Data.MapDb.TryFind(CombatTrialMapId, out var mapData))
		{
			var combatTrialMap = new DynamicMap(mapData.Id);
			ZoneServer.Instance.World.AddMap(combatTrialMap);
		}
	}

	private async Task HighlanderMasterDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L($"Highlander Master {HighlanderMasterName}"));
		dialog.SetPortrait("Dlg_port_HIGHLANDER");

		dialog.UpdateRelation();

		var continueDialog = true;
		while (continueDialog)
		{
			if (character.JobId == JobId.Swordsman)
			{
				if (dialog.GetMemory() == 0)
				{
					await dialog.Intro(L(
						"Greetings, warrior. I am Aulus, a master of the Highlander style.  Highlanders are renowned for their mastery of the sword, using powerful techniques to control the battlefield."));
				}

				if (character.Job.Level >= RequiredLevel && character.Quests.IsActive(GetQuestId()))
				{
					if (!character.Variables.Perm.GetBool("HighlanderQuestMasterMet", false))
					{
						if (character.Variables.Perm.GetBool("FoundHiddenHighlanderScroll", false) &&
							!character.Variables.Perm.GetBool("HiddenHighlanderQuestActive", false))
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
					else if (!character.Variables.Perm.GetBool("HighlanderKnowledgeTestPassed", false))
					{
						continueDialog = await KnowledgeTest(dialog);

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Are you ready to continue your Highlander training?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L("Take your time. The path of the Highlander requires dedication."));
								continueDialog = false;
							}
						}
					}
					else if (!character.Variables.Perm.GetBool("HighlanderCombatTrialPassed", false))
					{
						continueDialog = await CombatTrial(dialog);
						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("HighlanderTechniqueTestPassed", false))
					{
						continueDialog = await TechniqueTest(dialog);

						if (continueDialog)
						{
							var continueDialogue =
								await dialog.Select(L("Have you mastered the heart of the Highlander?"),
									Option(L("Yes"), "continue"),
									Option(L("Not yet"), "exit")
								);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"The essence of the Highlander takes time and practice. Return when you are ready."));
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
							"You have the spirit of a warrior, but you need more experience. Come back when you reach level {0}.",
							RequiredLevel));
					continueDialog = false;
				}
			}
			else if (character.JobId == JobId.Highlander)
			{
				await dialog.Msg(
					L(
						"Ah, a fellow Highlander! Keep honing your skills. Remember, the battlefield is our canvas, and our sword is the brush."));
				continueDialog = false;
			}
			else
			{
				await dialog.Msg(
					L(
						"Greetings, traveler. While your path is admirable, I can only train those who walk the warrior's road."));
				continueDialog = false;
			}

			//await dialog.Msg(dialog.GetMoodMessage());
		}
	}

	private async Task<bool> OfferQuest(Dialog dialog)
	{
		var character = dialog.Player;

		var response = await dialog.Select(
			L(
				"The Highlander path is not for the faint of heart. It requires strength, skill, and unwavering focus. Are you interested in learning our ways?"),
			Option(L("Tell me more about Highlanders."), "moreInfo",
				() => !character.Variables.Temp.GetBool("HighlanderInfoSeen", false)),
			Option(L("Yes, I want to be a Highlander!"), "acceptQuest"),
			Option(L("I need more time to consider."), "declineQuest")
		);

		switch (response)
		{
			case "moreInfo":
				await dialog.Msg(
					L("Highlanders are elite swordsmen, wielding powerful techniques passed down through generations. We specialize in maneuvers that disrupt the enemy, pushing them into each other or against obstacles."));
				dialog.ModifyMemory(1);
				character.Variables.Temp.Set("HighlanderInfoSeen", true);
				break;
			case "acceptQuest":
				await dialog.Msg(
					L("Excellent! Your Highlander journey begins now. Prepare to be tested - strength of body, mind, and spirit."));
				character.Variables.Perm.Set("HighlanderQuestMasterMet", true);
				character.Quests.CompleteObjective(GetQuestId(), "talkToAulus");
				dialog.ModifyFavor(1);
				break;
			case "declineQuest":
				await dialog.Msg(
					L("The Highlander path is a demanding one. Consider your choice carefully. You are welcome to return when you are ready."));
				return false;
		}

		return true;
	}

	private async Task OfferHiddenQuest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L("You possess an ancient scroll detailing a legendary Highlander technique!"));
		var response = await dialog.Select(
			L("Are you willing to take on this extraordinary challenge?"),
			Option(L("Yes, I seek to master this technique!"), "acceptHidden"),
			Option(L("Perhaps another time."), "declineHidden")
		);

		switch (response)
		{
			case "acceptHidden":
				await dialog.Msg(L(
					"Very well! Your determination is impressive. Return to me when you have mastered the technique described in the scroll."));
				character.Variables.Perm.Set("HighlanderQuestMasterMet", true);
				character.Variables.Perm.Set("HiddenHighlanderQuestActive", true);
				character.Quests.CompleteObjective(GetQuestId(), "talkToAulus");
				dialog.ModifyFavor(5);
				break;
			case "declineHidden":
				await dialog.Msg(L("The standard path is still a worthy challenge."));
				break;
		}
	}

	private async Task<bool> KnowledgeTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"A Highlander must have a keen understanding of the battlefield. Let's test your tactical knowledge."));

		var question1 = await dialog.Select(L("What is the Highlander's greatest strength?"),
			Option(L("Overwhelming power."), "wrong"),
			Option(L("Strategic positioning and disruption."), "correct"),
			Option(L("Unmatched speed and agility."), "wrong")
		);

		var question2 = await dialog.Select(L("Which of these is a signature Highlander technique?"),
			Option(L("Frenzy."), "wrong"),
			Option(L("Wagon Wheel."), "correct"),
			Option(L("Impaler."), "wrong")
		);

		if (question1 == "correct" && question2 == "correct")
		{
			await dialog.Msg(L("Well done! You have a sharp mind for tactics."));
			character.Variables.Perm.Set("HighlanderKnowledgeTestPassed", true);
			character.Quests.CompleteObjective(GetQuestId(), "knowledgeTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(
				L(
					"Your tactical understanding needs improvement. Study the art of war and return when you are ready."));
			dialog.ModifyStress(1);
			return false;
		}
	}

	private async Task<bool> CombatTrial(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"A Highlander's strength is forged in the heat of battle. Prove your worth in the training grounds."));
		var choice = await dialog.Select(L("Are you prepared for the trial?"),
			Option(L("I am ready!"), "enterTrial"),
			Option(L("I need more training."), "declineTrial")
		);

		if (choice == "declineTrial")
		{
			await dialog.Msg(L("Return when you are confident in your abilities."));
			return false;
		}

		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var combatTrialMap))
		{
			await dialog.Msg(L("The training grounds are currently unavailable."));
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

	private async Task<bool> TechniqueTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L("To truly master the Highlander's art, you must demonstrate the core principles of our techniques. Show me you understand the essence of 'Gathering'."));

		await dialog.Msg(L("For this trial, you will be transported to a special arena. Your objective is to gather all the training dummies into a designated area within the time limit. Use your skills wisely!"));

		var choice = await dialog.Select(L("Are you ready to begin?"),
			Option(L("Yes, let's do this!"), "startTechniqueTest"),
			Option(L("I need a moment to prepare."), "notReady")
		);

		if (choice == "notReady")
		{
			await dialog.Msg(L("Take your time. Precision and focus are key to a Highlander's success."));
			return false; // Stay in the dialog loop
		}

		// --- Technique Trial Implementation ---

		// 1. Teleport the Player to the Trial Arena
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var trialMap))
		{
			await dialog.Msg(L("The training arena seems to be unavailable. Come back later."));
			return false; // Stay in the dialog loop
		}

		character.Warp(trialMap.Id, trialMap.Data.DefaultPosition);

		// 2. Spawn Training Dummies in the Arena
		const int DummyCount = 5;
		var dummies = new List<Mob>();

		for (var i = 0; i < DummyCount; i++)
		{
			var dummy = new Mob(MonsterId.Wood_Carving_Fire, RelationType.Enemy);
			dummy.Components.Add(new MovementComponent(dummy));

			trialMap.AddMonster(dummy);
			dummies.Add(dummy);
		}

		// 3. (Optional) Display a Visual Indicator of the Target Area (e.g., a circle on the ground)
		// You can use particle effects or other visual cues to guide the player

		// 4. Start the Timer and Monitor Dummy Positions
		const int TimeLimit = 60; // Seconds
		var startTime = DateTime.Now;

		while ((DateTime.Now - startTime).TotalSeconds < TimeLimit)
		{
			await Task.Delay(1000); // Check every second

			// Check if all dummies are within the designated area
			var allDummiesGathered = dummies.All(dummy => /* Condition to check if dummy is in the target area */ true); // Replace with your logic

			if (allDummiesGathered)
			{
				character.WorldMessage(L("Excellent! You have grasped the essence of Gathering."));
				character.Variables.Perm.Set("HighlanderTechniqueTestPassed", true);
				character.Quests.CompleteObjective(GetQuestId(), "techniqueTest");
				dialog.ModifyFavor(1);

				// 5. Teleport Player back to the Highlander Master (add a delay if needed)
				await Task.Delay(TimeSpan.FromSeconds(3)); // Optional delay
				character.Warp(1001, new Position(-100, 0, 784));
				return true;
			}
		}

		// 6. Trial Failed (Timer Ran Out)
		character.WorldMessage(L("You ran out of time! The Highlander's path requires quick thinking and decisive action."));
		dialog.ModifyStress(1);

		// 7. Teleport Player back to Highlander Master
		character.Warp(1001, new Position(-100, 0, 784));
		return false;
	}

	private async Task CompleteJobChange(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Variables.Perm.GetBool("HiddenHighlanderQuestActive", false) &&
			character.Inventory.CountItem(HiddenQuestItemId) >= QuestItemCount)
		{
			await dialog.Msg(L("Magnificent! You've mastered the secret technique."));
			character.Inventory.Remove(HiddenQuestItemId, QuestItemCount);
			character.Quests.Complete(GetQuestId());
		}
		else
		{
			await dialog.Msg(L("You have proven yourself worthy. Arise, Highlander!"));
			character.Quests.Complete(GetQuestId());
		}

		character.ChangeJob(JobId.Highlander);
		character.AddKeyword("HIGHLANDER");

		await dialog.Msg(L("I officially declare you a Highlander! May your blade sing on the battlefield!"));
		dialog.ModifyFavor(1);
		dialog.ModifyMemory(1);
	}

	private int GetQuestId()
	{
		return 10_000_000 + (int)JobId.Highlander;
	}
}

public class HighlanderJobChangeQuestScript : QuestScript
{
	protected override void Load()
	{
		SetId("Laima.JobQuest", (int)JobId.Highlander);
		SetName("Path of the Highlander");
		SetDescription("Embrace the way of the Highlander and prove your worth to Aulus.");
		SetAutoTracked(true);
		SetCancelable(true);

		AddObjective("talkToAulus", "Seek out the Highlander Master", new ManualObjective());
		AddObjective("knowledgeTest", "Pass Aulus's knowledge test", new ManualObjective());
		var objective = AddObjective("combatTrial", "Complete Aulus's combat trial",
			new KillObjective(5, MonsterId.Goblin_Archer));
		objective.Completed += (character, objective) =>
		{
			character.WorldMessage(L("You have passed the Highlander combat trial!"));
			character.Variables.Perm.Set("HighlanderCombatTrialPassed", true);
		};
		AddObjective("techniqueTest", "Demonstrate the essence of Highlander techniques", new ManualObjective());

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
