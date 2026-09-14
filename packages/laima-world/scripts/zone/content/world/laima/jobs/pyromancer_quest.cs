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

public class PyromancerMasterNpc : GeneralScript
{
	private const int QuestId = 10_000_000 + (int)JobId.Pyromancer;
	private const int PyromancerMasterId = 20027;
	private const string PyromancerMasterName = "Ignis";
	private const string PyromancerMasterLocation = "c_firemage";
	private const int RequiredLevel = 15;
	private const int HiddenQuestItemId = ItemId.TSW03_313_1710_Steam_Friend_Event;
	private const int QuestItemCount = 1;

	private const int CombatTrialMapId = 523; // Replace with a suitable map ID
	private const int CombatTrialDuration = 180; // Seconds
	private const int CombatTrialMobId = MonsterId.Fire_Golem; // Replace with a challenging mob ID

	protected override void Load()
	{
		// Register the "Pyromancer" keyword
		RegisterKeyword("PYROMANCER", L("Pyromancer"),
			L("A mage who wields the destructive power of fire, incinerating enemies with scorching flames."));

		// Add the Pyromancer Master NPC
		AddNpc(PyromancerMasterId, L($"[Pyromancer Master] {PyromancerMasterName}"), PyromancerMasterLocation, -56, -18,
			90.0, PyromancerMasterDialog);

		// Prepare the combat trial map 
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var _) &&
			ZoneServer.Instance.Data.MapDb.TryFind(CombatTrialMapId, out var mapData))
		{
			var combatTrialMap = new DynamicMap(mapData.Id);
			ZoneServer.Instance.World.AddMap(combatTrialMap);
		}
	}

	private async Task PyromancerMasterDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L($"Pyromancer Master {PyromancerMasterName}"));
		dialog.SetPortrait("Dlg_port_PYROMANCER"); // Replace with a suitable portrait

		dialog.UpdateRelation();

		var continueDialog = true;
		while (continueDialog)
		{
			if (character.JobId == JobId.Wizard)
			{
				if (dialog.GetMemory() == 0)
				{
					await dialog.Intro(L(
						"Greetings, aspiring mage. I am Ignis, a master of Pyromancy. We command the flames, wielding their destructive power to purge our enemies."));
				}

				if (character.Job.Level >= RequiredLevel && character.Quests.IsActive(QuestId))
				{
					if (!character.Variables.Perm.GetBool("PyromancerQuestMasterMet", false))
					{
						if (character.Variables.Perm.GetBool("FoundBurningPyromancerScroll", false) &&
							!character.Variables.Perm.GetBool("HiddenPyromancerQuestActive", false))
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
					else if (!character.Variables.Perm.GetBool("PyromancerKnowledgeTestPassed", false))
					{
						continueDialog = await KnowledgeTest(dialog);

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Are you ready to continue your path to becoming a Pyromancer?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L("The flames of knowledge require time to be kindled. Return when you are ready."));
								continueDialog = false;
							}
						}
					}
					else if (!character.Variables.Perm.GetBool("PyromancerCombatTrialPassed", false))
					{
						continueDialog = await CombatTrial(dialog);
						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("PyromancerFireControlTestPassed", false))
					{
						continueDialog = await FireControlTest(dialog); // New trial for Pyromancer

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Have you learned to tame the fiery inferno within?"),
								Option(L("Yes"), "continue"),
								Option(L("Not yet"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"Mastery over fire demands unwavering discipline. Return when your control is absolute."));
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
						LF("Your affinity for fire is evident, but you need more training. Return when you reach level {0}.", RequiredLevel));
					continueDialog = false;
				}
			}
			else if (character.JobId == JobId.Pyromancer)
			{
				await dialog.Msg(
					L("Ah, a Pyromancer! May your flames burn bright and consume your foes. Remember, a true Pyromancer bends the fire to their will."));
				continueDialog = false;
			}
			else
			{
				await dialog.Msg(
					L("Greetings, traveler. My teachings are for those who have begun their magical journey as Wizards."));
				continueDialog = false;
			}
		}
	}

	private async Task<bool> OfferQuest(Dialog dialog)
	{
		var character = dialog.Player;

		var response = await dialog.Select(
			L("The path of the Pyromancer is one of destruction and rebirth. Are you willing to embrace the fiery trials that await?"),
			Option(L("Tell me more about Pyromancers."), "moreInfo",
				() => !character.Variables.Temp.GetBool("PyromancerInfoSeen", false)),
			Option(L("Yes, I wish to learn Pyromancy!"), "acceptQuest"),
			Option(L("I need to consider my options."), "declineQuest")
		);

		switch (response)
		{
			case "moreInfo":
				await dialog.Msg(
					L("Pyromancers are masters of fire magic, capable of unleashing devastating flames upon our enemies. Our spells consume and purify, leaving nothing but ash in their wake."));
				dialog.ModifyMemory(1);
				character.Variables.Temp.Set("PyromancerInfoSeen", true);
				return true;
			case "acceptQuest":
				await dialog.Msg(
					L("Excellent! You have chosen a path of power.  Prove yourself worthy through trials, and you shall become a Pyromancer."));
				character.Variables.Perm.Set("PyromancerQuestMasterMet", true);
				character.Quests.CompleteObjective(QuestId, "talkToIgnis");
				dialog.ModifyFavor(1);
				return true;
			case "declineQuest":
				await dialog.Msg(
					L("Fire is a dangerous element. Choose your path wisely. Return when you are certain."));
				return false;
			default:
				return true;
		}
	}

	private async Task OfferHiddenQuest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L("The scroll you possess speaks of a forbidden Pyromancer technique."));
		var response = await dialog.Select(
			L("Do you dare to wield such power?"),
			Option(L("Yes, I will master this technique."), "acceptHidden"),
			Option(L("No, it's too dangerous."), "declineHidden")
		);

		switch (response)
		{
			case "acceptHidden":
				await dialog.Msg(L("Intriguing. A true Pyromancer fears no flame. Return when you have unlocked the scroll's secrets."));
				character.Variables.Perm.Set("PyromancerQuestMasterMet", true);
				character.Variables.Perm.Set("HiddenPyromancerQuestActive", true);
				character.Quests.CompleteObjective(QuestId, "talkToIgnis");
				dialog.ModifyFavor(5);
				break;
			case "declineHidden":
				await dialog.Msg(L("A wise choice. The standard trials will still push you to your limits."));
				break;
		}
	}

	private async Task<bool> KnowledgeTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"A Pyromancer's power comes not just from raw strength but also from knowledge. Let's test your understanding of fire."));

		var question1 = await dialog.Select(L("What is the element that counters fire magic?"),
			Option(L("Earth"), "wrong"),
			Option(L("Water"), "correct"),
			Option(L("Air"), "wrong")
		);

		var question2 = await dialog.Select(L("What is a key strategy for Pyromancers in combat?"),
			Option(L("Utilizing a wide range of area-of-effect spells."), "correct"),
			Option(L("Focusing on single-target spells for maximum damage."), "wrong"),
			Option(L("Summoning fire elementals to assist in battle."), "wrong")
		);

		if (question1 == "correct" && question2 == "correct")
		{
			await dialog.Msg(L("Impressive! Your knowledge burns bright."));
			character.Variables.Perm.Set("PyromancerKnowledgeTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "knowledgeTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(
				L(
					"Your knowledge of fire needs more refinement. Study the flames more closely and return."));
			dialog.ModifyStress(1);
			return false;
		}
	}

	private async Task<bool> CombatTrial(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L("A Pyromancer must be able to dance amidst the flames without fear. Prove your courage in this trial."));
		var choice = await dialog.Select(L("Are you prepared to face the inferno?"),
			Option(L("I am ready to be tested!"), "enterTrial"),
			Option(L("I require more preparation."), "declineTrial")
		);

		if (choice == "declineTrial")
		{
			await dialog.Msg(L("A wise decision. Return when you feel the fire within you burn strong."));
			return false;
		}

		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var combatTrialMap))
		{
			await dialog.Msg(L("The trial grounds are not currently available."));
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

	private async Task<bool> FireControlTest(Dialog dialog)
	{
		var character = dialog.Player;

		// Implement the Fire Control Test here. This could involve:
		// - Lighting a series of braziers in a specific order.
		// - Extinguishing a raging fire using control techniques.
		// - Protecting an object from fire damage.
		// ...

		if (/* condition for passing the Fire Control Test */ true)
		{
			await dialog.Msg(L("Impressive! You have shown true mastery over fire."));
			character.Variables.Perm.Set("PyromancerFireControlTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "fireControlTest");
			dialog.ModifyFavor(15);
			return true;
		}
		else
		{
			await dialog.Msg(L("Your control is faltering. Practice diligently and return."));
			dialog.ModifyStress(5);
			return false;
		}
	}

	private async Task CompleteJobChange(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Variables.Perm.GetBool("HiddenPyromancerQuestActive", false) &&
			character.Inventory.CountItem(HiddenQuestItemId) >= QuestItemCount)
		{
			await dialog.Msg(L(
				"You have claimed the forbidden knowledge! It takes a true master to wield such power."));
			character.Inventory.Remove(HiddenQuestItemId, QuestItemCount);
		}
		else
		{
			await dialog.Msg(L("You have proven your worth. Rise as a Pyromancer!"));
		}

		character.Quests.Complete(QuestId);

		character.ChangeJob(JobId.Pyromancer);
		character.AddKeyword("PYROMANCER");

		await dialog.Msg(L("May your flames illuminate your path and incinerate your foes."));
		dialog.ModifyFavor(1);
		dialog.ModifyMemory(1);
	}
}

public class PyromancerJobChangeQuestScript : QuestScript
{
	protected override void Load()
	{
		SetId(10_000_000 + (int)JobId.Pyromancer); // Unique ID for Pyromancer quest
		SetName("Path of the Pyromancer");
		SetDescription("Embrace the fiery path of the Pyromancer and prove your worth to Ignis.");
		SetAutoTracked(true);
		SetCancelable(true);

		AddObjective("talkToIgnis", "Seek out the Pyromancer Master", new ManualObjective());
		AddObjective("knowledgeTest", "Pass Ignis's knowledge test", new ManualObjective());
		var objective = AddObjective("combatTrial", "Complete Ignis's combat trial",
			new KillObjective(1, MonsterId.Fire_Golem)); // Adjust enemy count and type as needed
		objective.Completed += (character, objective) =>
		{
			character.WorldMessage(L("You have passed the Pyromancer combat trial!"));
			character.Variables.Perm.Set("PyromancerCombatTrialPassed", true);
		};
		AddObjective("fireControlTest", "Demonstrate mastery of fire control", new ManualObjective());

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
