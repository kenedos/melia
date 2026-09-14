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

public class CryomancerMasterNpc : GeneralScript
{
	private const int QuestId = 10_000_000 + (int)JobId.Cryomancer;
	private const int CryomancerMasterId = 20137;
	private const string CryomancerMasterName = "Glacia";
	private const string CryomancerMasterLocation = "c_Klaipe";
	private const int RequiredLevel = 15;
	private const int HiddenQuestItemId = ItemId.STF01_104;
	private const int QuestItemCount = 1;

	private const int CombatTrialMapId = 501; // Replace with a suitable map ID
	private const int CombatTrialDuration = 180; // Seconds
	private const int CombatTrialMobId = MonsterId.Goblin_Elite; // Replace with a challenging mob ID

	protected override void Load()
	{
		// Register the "Cryomancer" keyword
		RegisterKeyword("CRYOMANCER", L("Cryomancer"),
			L("A mage who commands the power of ice, freezing enemies and controlling the battlefield with chilling magic."));

		// Add the Cryomancer Master NPC
		AddNpc(CryomancerMasterId, L($"[Cryomancer Master] {CryomancerMasterName}"), CryomancerMasterLocation, -95, -309,
			0, CryomancerMasterDialog); // Adjust position as needed

		// Prepare the combat trial map (if it's a dynamic map)
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var _) &&
			ZoneServer.Instance.Data.MapDb.TryFind(CombatTrialMapId, out var mapData))
		{
			var combatTrialMap = new DynamicMap(mapData.Id);
			ZoneServer.Instance.World.AddMap(combatTrialMap);
		}
	}

	private async Task CryomancerMasterDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L($"Cryomancer Master {CryomancerMasterName}"));
		dialog.SetPortrait("Dlg_port_CRYOMANCER_MASTER"); // Replace with a suitable portrait

		dialog.UpdateRelation();

		var continueDialog = true;
		while (continueDialog)
		{
			if (character.JobId == JobId.Wizard)
			{
				if (dialog.GetMemory() == 0)
				{
					await dialog.Intro(L(
						"Greetings, young mage. I am Glacia, a master of Cryomancy.  We wield the chilling power of ice, freezing our enemies and shaping the battlefield to our will."));
				}

				if (character.Job.Level >= RequiredLevel && character.Quests.IsActive(QuestId))
				{
					if (!character.Variables.Perm.GetBool("CryomancerQuestMasterMet", false))
					{
						if (character.Variables.Perm.GetBool("FoundFrozenCryomancerTome", false) &&
							!character.Variables.Perm.GetBool("HiddenCryomancerQuestActive", false))
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
					else if (!character.Variables.Perm.GetBool("CryomancerKnowledgeTestPassed", false))
					{
						continueDialog = await KnowledgeTest(dialog);

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Do you wish to continue on the path of the Cryomancer?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"Ice magic requires patience and focus. Return when you are ready to delve deeper."));
								continueDialog = false;
							}
						}
					}
					else if (!character.Variables.Perm.GetBool("CryomancerCombatTrialPassed", false))
					{
						continueDialog = await CombatTrial(dialog);
						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("CryomancerFreezingTestPassed", false))
					{
						continueDialog = await FreezingTest(dialog); // New trial for Cryomancer

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Have you learned to command the true essence of ice?"),
								Option(L("Yes"), "continue"),
								Option(L("Not yet"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"Mastery of Cryomancy takes time and dedication. Return when your control over ice is absolute."));
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
							"Your magical potential is clear, but you require further training. Come back when you reach level {0}.",
							RequiredLevel));
					continueDialog = false;
				}
			}
			else if (character.JobId == JobId.Cryomancer)
			{
				await dialog.Msg(
					L(
						"Ah, a Cryomancer! Embrace the cold. Let it flow through you. And remember, a true Cryomancer controls not only ice but the very flow of battle."));
				continueDialog = false;
			}
			else
			{
				await dialog.Msg(
					L(
						"Greetings, traveler. I sense magic within you, but my teachings are for those who have begun their journey as Wizards."));
				continueDialog = false;
			}
		}
	}

	private async Task<bool> OfferQuest(Dialog dialog)
	{
		var character = dialog.Player;

		var response = await dialog.Select(
			L(
				"The path of the Cryomancer is a challenging one, but the rewards are great.  Are you ready to embrace the power of ice?"),
			Option(L("Tell me more about Cryomancers."), "moreInfo",
				() => !character.Variables.Temp.GetBool("CryomancerInfoSeen", false)),
			Option(L("Yes, teach me the ways of the Cryomancer!"), "acceptQuest"),
			Option(L("I am not ready."), "declineQuest")
		);

		switch (response)
		{
			case "moreInfo":
				await dialog.Msg(
					L(
						"Cryomancers are masters of ice magic, wielding its destructive and controlling power. We can freeze our enemies solid, create icy barriers, and manipulate the battlefield to our advantage."));
				dialog.ModifyMemory(1);
				character.Variables.Temp.Set("CryomancerInfoSeen", true);
				return true;
			case "acceptQuest":
				await dialog.Msg(
					L(
						"Very well! I will guide you, but you must prove your worth through trials. Only then will you earn the title of Cryomancer."));
				character.Variables.Perm.Set("CryomancerQuestMasterMet", true);
				character.Quests.CompleteObjective(QuestId, "talkToGlacia");
				dialog.ModifyFavor(1);
				return true;
			case "declineQuest":
				await dialog.Msg(
					L(
						"Cryomancy is not a path to be taken lightly.  Return when you are certain of your choice."));
				return false;
			default:
				return true;
		}
	}

	private async Task OfferHiddenQuest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L("You found a frozen tome! It contains whispers of a forgotten Cryomancer technique."));
		var response = await dialog.Select(
			L("Are you brave enough to unlock its secrets?"),
			Option(L("Yes, I will master this lost art!"), "acceptHidden"),
			Option(L("I am not prepared for such power."), "declineHidden")
		);

		switch (response)
		{
			case "acceptHidden":
				await dialog.Msg(
					L(
						"Impressive! A true Cryomancer seeks knowledge wherever it may be frozen. Return to me when you have mastered the technique within the tome."));
				character.Variables.Perm.Set("CryomancerQuestMasterMet", true);
				character.Variables.Perm.Set("HiddenCryomancerQuestActive", true);
				character.Quests.CompleteObjective(QuestId, "talkToGlacia");
				dialog.ModifyFavor(5);
				break;
			case "declineHidden":
				await dialog.Msg(
					L("Wisdom is knowing your limits. The standard path will still test your abilities."));
				break;
		}
	}

	private async Task<bool> KnowledgeTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L(
			"Cryomancy is more than just brute force. A true Cryomancer understands the properties of ice and its tactical applications."));

		var question1 = await dialog.Select(L("What is the weakness of ice magic?"),
			Option(L("Fire."), "correct"),
			Option(L("Water."), "wrong"),
			Option(L("Earth."), "wrong")
		);

		var question2 = await dialog.Select(L("Which of these is a core principle of Cryomancy?"),
			Option(L("Controlling the battlefield through freezing and slowing effects."), "correct"),
			Option(L("Summoning powerful ice elementals."), "wrong"),
			Option(L("Enhancing weapons with ice magic for increased damage."), "wrong")
		);

		if (question1 == "correct" && question2 == "correct")
		{
			await dialog.Msg(L("Your understanding of Cryomancy is promising!"));
			character.Variables.Perm.Set("CryomancerKnowledgeTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "knowledgeTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(
				L("You need to study the principles of Cryomancy more thoroughly. Return when your knowledge is solidified."));
			dialog.ModifyStress(1);
			return false;
		}
	}

	private async Task<bool> CombatTrial(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"Cryomancers must be able to withstand the heat of battle while maintaining their icy focus. Prove your resilience in this trial."));
		var choice = await dialog.Select(L("Are you prepared to face the challenge?"),
			Option(L("I am ready."), "enterTrial"),
			Option(L("I need to prepare."), "declineTrial")
		);

		if (choice == "declineTrial")
		{
			await dialog.Msg(L("Return when your spirit is as unyielding as ice."));
			return false;
		}

		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var combatTrialMap))
		{
			await dialog.Msg(L("The trial grounds are currently inaccessible."));
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

	private async Task<bool> FreezingTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L(
			"The true power of a Cryomancer lies in their control over ice. In this trial, you must demonstrate your mastery of freezing."));

		// You can implement the Freezing Test in various ways:
		// - Require the player to freeze a specific number of enemies within a time limit.
		// - Challenge them to create a path of ice to reach a certain point.
		// - Ask them to solve a puzzle using ice magic.

		if (/* condition for passing the Freezing Test */ true)
		{
			await dialog.Msg(L("Excellent! You have proven your command over the icy arts."));
			character.Variables.Perm.Set("CryomancerFreezingTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "freezingTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(
				L(
					"Your control over ice is not yet absolute. Practice your skills and return when you have mastered them."));
			dialog.ModifyStress(5);
			return false;
		}
	}

	private async Task CompleteJobChange(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Variables.Perm.GetBool("HiddenCryomancerQuestActive", false) &&
			character.Inventory.CountItem(HiddenQuestItemId) >= QuestItemCount)
		{
			await dialog.Msg(L("The ancient technique is yours! You truly embody the Cryomancer spirit."));
			character.Inventory.Remove(HiddenQuestItemId, QuestItemCount);
			character.Quests.Complete(QuestId);
		}
		else
		{
			await dialog.Msg(L(
				"You have faced the trials and emerged victorious. Your dedication is commendable. Arise, Cryomancer!"));
			character.Quests.Complete(QuestId);
		}

		character.ChangeJob(JobId.Cryomancer);
		character.AddKeyword("CRYOMANCER");

		await dialog.Msg(L("May your magic freeze your enemies and protect your allies!"));
		dialog.ModifyFavor(1);
		dialog.ModifyMemory(1);
	}
}

public class CryomancerJobChangeQuestScript : QuestScript
{
	protected override void Load()
	{
		SetId(10_000_000 + (int)JobId.Cryomancer);
		SetName("Path of the Cryomancer");
		SetDescription("Follow the icy path of the Cryomancer and prove your worth to Glacia.");
		SetAutoTracked(true);
		SetCancelable(true);

		AddObjective("talkToGlacia", "Seek out the Cryomancer Master", new ManualObjective());
		AddObjective("knowledgeTest", "Pass Glacia's knowledge test", new ManualObjective());
		var objective = AddObjective("combatTrial", "Complete Glacia's combat trial",
			new KillObjective(5, MonsterId.Goblin_Elite)); // Adjust enemy count and type as needed
		objective.Completed += (character, objective) =>
		{
			character.WorldMessage(L("You have passed the Cryomancer combat trial!"));
			character.Variables.Perm.Set("CryomancerCombatTrialPassed", true);
		};
		AddObjective("freezingTest", "Demonstrate mastery of freezing magic", new ManualObjective());

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
