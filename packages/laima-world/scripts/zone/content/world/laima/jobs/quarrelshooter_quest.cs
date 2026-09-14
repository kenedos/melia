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

public class QuarrelShooterMasterNpc : GeneralScript
{
	private const int QuestId = 10_000_000 + (int)JobId.QuarrelShooter;
	private const int QuarrelShooterMasterId = 147445;
	private const string QuarrelShooterMasterName = "Caeso"; // Replace with the actual name
	private const string QuarrelShooterMasterLocation = "c_orsha";
	private const int RequiredLevel = 15;
	private const int HiddenQuestItemId = ItemId.BOW05_104;
	private const int QuestItemCount = 1;

	private const int CombatTrialMapId = 523;
	private const int CombatTrialDuration = 180; // Seconds
	private const int CombatTrialMobId = MonsterId.Shredded; // Replace with a challenging mob ID

	protected override void Load()
	{
		// Register the "Quarrel Shooter" keyword
		RegisterKeyword("QUARREL_SHOOTER", L("Quarrel Shooter"),
			L("A disciplined archer who uses a crossbow and shield for strategic combat, specializing in both offense and defense."));

		// Add the Quarrel Shooter Master NPC
		AddNpc(QuarrelShooterMasterId, L($"[Quarrel Shooter Master] {QuarrelShooterMasterName}"),
			QuarrelShooterMasterLocation, -67, 238, 90.0, QuarrelShooterMasterDialog);

		// Prepare the combat trial map (if it's a dynamic map)
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var _) &&
			ZoneServer.Instance.Data.MapDb.TryFind(CombatTrialMapId, out var mapData))
		{
			var combatTrialMap = new DynamicMap(mapData.Id);
			ZoneServer.Instance.World.AddMap(combatTrialMap);
		}
	}

	private async Task QuarrelShooterMasterDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L($"Quarrel Shooter Master {QuarrelShooterMasterName}"));
		dialog.SetPortrait("Dlg_port_QUARREL_SHOOTER_MASTER"); // Replace with a suitable portrait

		dialog.UpdateRelation();

		var continueDialog = true;
		while (continueDialog)
		{
			if (character.JobId == JobId.Archer)
			{
				if (dialog.GetMemory() == 0)
				{
					await dialog.Intro(L(
						"Greetings, archer. I am Caeso, a master of the Quarrel Shooter discipline. We are archers who value precision, discipline, and unwavering defense."));
				}

				if (character.Job.Level >= RequiredLevel && character.Quests.IsActive(QuestId))
				{
					if (!character.Variables.Perm.GetBool("QuarrelShooterQuestMasterMet", false))
					{
						if (character.Variables.Perm.GetBool("FoundQuarrelShooterManual", false) &&
							!character.Variables.Perm.GetBool("HiddenQuarrelShooterQuestActive", false))
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
					else if (!character.Variables.Perm.GetBool("QuarrelShooterKnowledgeTestPassed", false))
					{
						continueDialog = await KnowledgeTest(dialog);

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Are you ready to continue your Quarrel Shooter training?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(
									L("A Quarrel Shooter never rushes into battle. Return when your focus is sharp."));
								continueDialog = false;
							}
						}
					}
					else if (!character.Variables.Perm.GetBool("QuarrelShooterCombatTrialPassed", false))
					{
						continueDialog = await CombatTrial(dialog);
						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("QuarrelShooterDefenseTestPassed", false))
					{
						continueDialog = await DefenseTest(dialog); // New trial for Quarrel Shooter

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Have you mastered the art of unwavering defense?"),
								Option(L("Yes"), "continue"),
								Option(L("Not yet"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"A strong defense is as vital as a sharp aim. Return when you are ready to prove your resilience."));
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
						LF("You show promise, archer, but you need more experience. Return when you reach level {0}.",
							RequiredLevel));
					continueDialog = false;
				}
			}
			else if (character.JobId == JobId.QuarrelShooter)
			{
				await dialog.Msg(
					L("Ah, a fellow Quarrel Shooter! Remember, every bolt counts, and a well-placed pavise can win the day."));
				continueDialog = false;
			}
			else
			{
				await dialog.Msg(
					L("Greetings, traveler. My training is for those who have embraced the Archer's path."));
				continueDialog = false;
			}
		}
	}

	private async Task<bool> OfferQuest(Dialog dialog)
	{
		var character = dialog.Player;

		var response = await dialog.Select(
			L("The Quarrel Shooter combines ranged power with strategic defense. Are you interested in learning this demanding art?"),
			Option(L("Tell me more about Quarrel Shooters."), "moreInfo",
				() => !character.Variables.Temp.GetBool("QuarrelShooterInfoSeen", false)),
			Option(L("Yes, I wish to become a Quarrel Shooter!"), "acceptQuest"),
			Option(L("I need more time to think."), "declineQuest")
		);

		switch (response)
		{
			case "moreInfo":
				await dialog.Msg(
					L("Quarrel Shooters are masters of the crossbow, wielding its power with precision. We also utilize pavises, large shields that can be deployed for superior defense against ranged attacks."));
				dialog.ModifyMemory(1);
				character.Variables.Temp.Set("QuarrelShooterInfoSeen", true);
				return true;
			case "acceptQuest":
				await dialog.Msg(
					L(
						"Very well! Your journey as a Quarrel Shooter begins now. Prove your worth through these trials, and you shall earn the title."));
				character.Variables.Perm.Set("QuarrelShooterQuestMasterMet", true);
				character.Quests.CompleteObjective(QuestId, "talkToCaeso");
				dialog.ModifyFavor(1);
				return true;
			case "declineQuest":
				await dialog.Msg(
					L(
						"The path of the Quarrel Shooter is not an easy one. Return when you are ready to dedicate yourself to the craft."));
				return false;
			default:
				return true;
		}
	}

	private async Task OfferHiddenQuest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L("You found a manual detailing advanced Quarrel Shooter techniques!"));
		var response = await dialog.Select(
			L("Do you want to pursue this challenging path?"),
			Option(L("Yes, I will master these techniques."), "acceptHidden"),
			Option(L("No, I will stick to the traditional training."), "declineHidden")
		);

		switch (response)
		{
			case "acceptHidden":
				await dialog.Msg(L(
					"Your dedication is admirable. Return when you have mastered the techniques described in the manual."));
				character.Variables.Perm.Set("QuarrelShooterQuestMasterMet", true);
				character.Variables.Perm.Set("HiddenQuarrelShooterQuestActive", true);
				character.Quests.CompleteObjective(QuestId, "talkToCaeso");
				dialog.ModifyFavor(5);
				break;
			case "declineHidden":
				await dialog.Msg(L("The standard path is challenging enough for most."));
				break;
		}
	}

	private async Task<bool> KnowledgeTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L("A Quarrel Shooter must have a keen understanding of their tools and tactics."));

		var question1 = await dialog.Select(L("What is the primary advantage of a pavise over a regular shield?"),
		Option(L("Increased melee defense."), "wrong"),
		Option(L("Enhanced mobility and speed."), "wrong"),
		Option(L("Superior protection against ranged attacks."), "correct")
		);

		var question2 = await dialog.Select(L("What is a key characteristic of a crossbow compared to a regular bow?"),
			Option(L("Faster firing rate."), "wrong"),
			Option(L("Greater accuracy and power."), "correct"),
			Option(L("Longer range."), "wrong")
		);

		if (question1 == "correct" && question2 == "correct")
		{
			await dialog.Msg(L("Excellent! Your knowledge of Quarrel Shooter tactics is impressive!"));
			character.Variables.Perm.Set("QuarrelShooterKnowledgeTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "knowledgeTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(
				L("You must study more diligently. Return when you have a better grasp of Quarrel Shooter fundamentals."));
			dialog.ModifyStress(1);
			return false;
		}
	}

	private async Task<bool> CombatTrial(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"A Quarrel Shooter's strength lies in their ability to combine offense and defense. Prove your skills in this trial."));
		var choice = await dialog.Select(L("Are you ready for the challenge?"),
			Option(L("I am ready."), "enterTrial"),
			Option(L("I need more practice."), "declineTrial")
		);

		if (choice == "declineTrial")
		{
			await dialog.Msg(
				L(
					"A wise choice. Return when you are confident in your ability to fight and defend simultaneously."));
			return false;
		}

		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var combatTrialMap))
		{
			await dialog.Msg(L("The trial grounds are currently unavailable."));
			return false;
		}

		character.Warp(combatTrialMap.Id, combatTrialMap.Data.DefaultPosition);

		var enemies = new List<Mob>();
		for (var i = 0; i < 5; i++) // Adjust enemy count as needed
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

	private async Task<bool> DefenseTest(Dialog dialog)
	{
		var character = dialog.Player;

		// Implement the Defense Test here. This could involve:
		// - Deploying a pavise to block a series of projectile attacks.
		// - Surviving waves of enemies for a specific duration while using a pavise.
		// - Demonstrating knowledge of proper pavise placement and usage.
		// ...

		if (/* condition for passing the Defense Test */ true)
		{
			await dialog.Msg(L("Your defense is as strong as iron! You have mastered the shield."));
			character.Variables.Perm.Set("QuarrelShooterDefenseTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "defenseTest");
			dialog.ModifyFavor(15);
			return true;
		}
		else
		{
			await dialog.Msg(L("Your defenses are still weak. Train harder and return when you are ready."));
			dialog.ModifyStress(5);
			return false;
		}
	}

	private async Task CompleteJobChange(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Variables.Perm.GetBool("HiddenQuarrelShooterQuestActive", false) &&
			character.Inventory.CountItem(HiddenQuestItemId) >= QuestItemCount)
		{
			await dialog.Msg(
				L(
					"The advanced techniques are yours to command!  You have proven yourself to be a true Quarrel Shooter."));
			character.Inventory.Remove(HiddenQuestItemId, QuestItemCount);
		}
		else
		{
			await dialog.Msg(
				L(
					"You have passed the trials and shown your dedication. I welcome you into the ranks of the Quarrel Shooters!"));
		}

		character.Quests.Complete(QuestId);
		character.ChangeJob(JobId.QuarrelShooter);
		character.AddKeyword("QUARREL_SHOOTER");

		await dialog.Msg(
			L(
				"May your aim be true and your pavise unwavering. Go forth, Quarrel Shooter, and protect the realm."));
		dialog.ModifyFavor(1);
		dialog.ModifyMemory(1);
	}
}

public class QuarrelShooterJobChangeQuestScript : QuestScript
{
	protected override void Load()
	{
		SetId(10_000_000 + (int)JobId.QuarrelShooter); // Unique ID for Quarrel Shooter quest
		SetName("Path of the Quarrel Shooter");
		SetDescription(
			"Embrace the discipline of the Quarrel Shooter and prove your worth to Caeso.");
		SetAutoTracked(true);
		SetCancelable(true);

		AddObjective("talkToCaeso", "Seek out the Quarrel Shooter Master", new ManualObjective());
		AddObjective("knowledgeTest", "Pass Caeso's knowledge test", new ManualObjective());
		var objective = AddObjective("combatTrial", "Complete Caeso's combat trial",
			new KillObjective(5, MonsterId.Shredded)); // Adjust enemy count and type as needed
		objective.Completed += (character, objective) =>
		{
			character.WorldMessage(L("You have passed the Quarrel Shooter combat trial!"));
			character.Variables.Perm.Set("QuarrelShooterCombatTrialPassed", true);
		};
		AddObjective("defenseTest", "Demonstrate mastery of defensive techniques",
			new ManualObjective());

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
