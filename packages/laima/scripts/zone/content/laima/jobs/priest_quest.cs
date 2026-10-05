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
using static Melia.Zone.Scripting.Shortcuts;
using static Melia.Zone.Scripting.Extensions.Keywords.Shortcuts;

public class PriestMasterNpc : GeneralScript
{
	private const int QuestId = 10_000_000 + (int)JobId.Priest;
	private const int PriestMasterId = 57229; // Replace with the actual Priest Master ID
	private const string PriestMasterName = "Seraphina"; // Replace with the actual name
	private const string PriestMasterLocation = "c_Klaipe";
	private const int RequiredLevel = 15;
	private const int HiddenQuestItemId = ItemId.TOSHero_THMACE; // Replace with a suitable powerful mace ID
	private const int QuestItemCount = 1;

	private const int CombatTrialMapId = 511; // Replace with a suitable map ID
	private const int CombatTrialDuration = 180; // Seconds
	private const int CombatTrialMobId = MonsterId.Ghoul; // Replace with a challenging mob ID

	protected override void Load()
	{
		// Register the "Priest" keyword
		RegisterKeyword("PRIEST", L("Priest"),
			L("A devout healer who channels divine power to mend wounds and protect allies."));

		// Add the Priest Master NPC
		AddNpc(PriestMasterId, L($"[Priest Master] {PriestMasterName}"), PriestMasterLocation, -187, 361,
			0, PriestMasterDialog);

		// Prepare the combat trial map 
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var _) &&
			ZoneServer.Instance.Data.MapDb.TryFind(CombatTrialMapId, out var mapData))
		{
			var combatTrialMap = new DynamicMap(mapData.Id);
			ZoneServer.Instance.World.AddMap(combatTrialMap);
		}
	}

	private async Task PriestMasterDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L($"Priest Master {PriestMasterName}"));
		dialog.SetPortrait("Dlg_port_PRIEST_MASTER"); // Replace with a suitable portrait

		dialog.UpdateRelation();

		var continueDialog = true;
		while (continueDialog)
		{
			if (character.JobId == JobId.Cleric)
			{
				if (dialog.GetMemory() == 0)
				{
					await dialog.Intro(L(
						"Welcome, child of the divine. I am Seraphina, a master of the Priest's path. We are conduits of healing, channeling the divine to mend and protect."));
				}

				if (character.Job.Level >= RequiredLevel && character.Quests.IsActive(QuestId))
				{
					if (!character.Variables.Perm.GetBool("PriestQuestMasterMet", false))
					{
						if (character.Variables.Perm.GetBool("FoundSacredPriestRelic", false) &&
							!character.Variables.Perm.GetBool("HiddenPriestQuestActive", false))
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
					else if (!character.Variables.Perm.GetBool("PriestKnowledgeTestPassed", false))
					{
						continueDialog = await KnowledgeTest(dialog);

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Are you ready to continue your journey as a Priest?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"Divine knowledge takes time to absorb. Seek guidance when you are ready."));
								continueDialog = false;
							}
						}
					}
					else if (!character.Variables.Perm.GetBool("PriestCombatTrialPassed", false))
					{
						continueDialog = await CombatTrial(dialog);
						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("PriestHealingTestPassed", false))
					{
						continueDialog = await HealingTest(dialog); // New trial for Priest

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Have you mastered the art of healing, child?"),
								Option(L("Yes"), "continue"),
								Option(L("Not yet"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"True healing requires compassion and unwavering faith. Return when you are ready."));
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
							"Your spirit is strong, but you need more experience. Return when you reach level {0}, child.",
							RequiredLevel));
					continueDialog = false;
				}
			}
			else if (character.JobId == JobId.Priest)
			{
				await dialog.Msg(
					L(
						"Ah, a fellow Priest!  May your faith be your shield, and your hands bring solace. Remember, a Priest is a beacon of hope in times of darkness."));
				continueDialog = false;
			}
			else
			{
				await dialog.Msg(
					L(
						"Greetings, traveler. My guidance is for those who have begun their path as Clerics."));
				continueDialog = false;
			}
		}
	}

	private async Task<bool> OfferQuest(Dialog dialog)
	{
		var character = dialog.Player;

		var response = await dialog.Select(
			L(
				"The Priest's path is one of devotion and selflessness. Do you seek to become a conduit of healing and protection?"),
			Option(L("Tell me more about Priests."), "moreInfo",
				() => !character.Variables.Temp.GetBool("PriestInfoSeen", false)),
			Option(L("Yes, I wish to walk the Priest's path!"), "acceptQuest"),
			Option(L("I need time to reflect."), "declineQuest")
		);

		switch (response)
		{
			case "moreInfo":
				await dialog.Msg(
					L(
						"Priests are healers and protectors, drawing upon divine power to mend wounds and shield allies from harm. Our faith is our strength, and our compassion guides our actions."));
				dialog.ModifyMemory(1);
				character.Variables.Temp.Set("PriestInfoSeen", true);
				return true;
			case "acceptQuest":
				await dialog.Msg(
					L(
						"Your heart is pure. I will test your resolve and guide you towards your destiny. Complete these trials, and you shall become a Priest."));
				character.Variables.Perm.Set("PriestQuestMasterMet", true);
				character.Quests.CompleteObjective(QuestId, "talkToSeraphina");
				dialog.ModifyFavor(1);
				return true;
			case "declineQuest":
				await dialog.Msg(
					L(
						"The path of a Priest is not to be taken lightly. Pray for guidance and return when you are ready."));
				return false;
			default:
				return true;
		}
	}

	private async Task OfferHiddenQuest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L("You possess a sacred relic! It radiates with ancient healing energy."));
		var response = await dialog.Select(
			L("Are you willing to embrace its power and take on the challenges it presents?"),
			Option(L("Yes, I will prove myself worthy."), "acceptHidden"),
			Option(L("No, I am not yet ready."), "declineHidden")
		);

		switch (response)
		{
			case "acceptHidden":
				await dialog.Msg(
					L(
						"Your faith is strong.  Return to me when you have overcome the challenges associated with this relic."));
				character.Variables.Perm.Set("PriestQuestMasterMet", true);
				character.Variables.Perm.Set("HiddenPriestQuestActive", true);
				character.Quests.CompleteObjective(QuestId, "talkToSeraphina");
				dialog.ModifyFavor(5);
				break;
			case "declineHidden":
				await dialog.Msg(
					L(
						"Humility is a virtue. The standard trials will still test your worthiness."));
				break;
		}
	}

	private async Task<bool> KnowledgeTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L(
			"A Priest's knowledge of healing arts and divine scriptures is essential.  Let me assess your understanding."));

		var question1 = await dialog.Select(L("What is the primary role of a Priest in a party?"),
			Option(L("To deal damage to enemies."), "wrong"),
			Option(L("To support and heal allies."), "correct"),
			Option(L("To explore and scout ahead."), "wrong")
		);

		var question2 = await dialog.Select(
			L("Which of these virtues is crucial for a Priest?"),
			Option(L("Courage"), "wrong"),
			Option(L("Compassion"), "correct"),
			Option(L("Strength"), "wrong")
		);

		if (question1 == "correct" && question2 == "correct")
		{
			await dialog.Msg(L("Your knowledge shines with divine light!"));
			character.Variables.Perm.Set("PriestKnowledgeTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "knowledgeTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(
				L(
					"You must delve deeper into the scriptures and practice your healing arts. Return when you are more prepared."));
			dialog.ModifyStress(1);
			return false;
		}
	}

	private async Task<bool> CombatTrial(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L(
			"Even a healer must be able to defend themselves and their allies. Your next trial will test your strength and resilience."));
		var choice = await dialog.Select(L("Are you ready to face this challenge?"),
			Option(L("Yes, I am prepared."), "enterTrial"),
			Option(L("I need more time to train."), "declineTrial")
		);

		if (choice == "declineTrial")
		{
			await dialog.Msg(
				L("Strength comes from both body and spirit. Train diligently and return when you are ready."));
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

	private async Task<bool> HealingTest(Dialog dialog)
	{
		var character = dialog.Player;

		// Implement a Healing Test here. This could involve:
		// - Healing a set number of injured NPCs within a time limit.
		// - Maintaining an ally's health above a certain threshold during a simulated battle.
		// - Demonstrating knowledge of different healing spells and their applications.
		// ...

		if (/* condition for passing the Healing Test */ true)
		{
			await dialog.Msg(L("Your healing touch is a gift from the divine!"));
			character.Variables.Perm.Set("PriestHealingTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "healingTest");
			dialog.ModifyFavor(15);
			return true;
		}
		else
		{
			await dialog.Msg(L(
				"Your healing skills need further refinement. Practice your craft and return when your touch brings true solace."));
			dialog.ModifyStress(5);
			return false;
		}
	}

	private async Task CompleteJobChange(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Variables.Perm.GetBool("HiddenPriestQuestActive", false) &&
			character.Inventory.CountItem(HiddenQuestItemId) >= QuestItemCount)
		{

			await dialog.Msg(L(
				"You have harnessed the power of the sacred relic. Your devotion inspires me."));
			character.Inventory.Remove(HiddenQuestItemId, QuestItemCount);
		}
		else
		{
			await dialog.Msg(
				L(
					"You have walked the path of the Priest with grace and humility. Rise now and embrace your calling!"));
		}

		character.Quests.Complete(QuestId);
		character.ChangeJob(JobId.Priest);
		character.AddKeyword("PRIEST");

		await dialog.Msg(
			L(
				"Go forth, Priest, and be a light in the darkness. May your healing touch bring peace and solace to all."));
		dialog.ModifyFavor(1);
		dialog.ModifyMemory(1);
	}
}

public class PriestJobChangeQuestScript : QuestScript
{
	protected override void Load()
	{
		SetId(10_000_000 + (int)JobId.Priest); // Unique ID for Priest quest
		SetName("Path of the Priest");
		SetDescription("Prove your devotion and become a Priest under the guidance of Seraphina.");
		SetAutoTracked(true);
		SetCancelable(true);

		AddObjective("talkToSeraphina", "Seek out the Priest Master", new ManualObjective());
		AddObjective("knowledgeTest", "Pass Seraphina's knowledge test", new ManualObjective());
		var objective = AddObjective("combatTrial", "Complete Seraphina's combat trial",
			new KillObjective(5, MonsterId.Ghoul)); // Adjust enemy count and type as needed
		objective.Completed += (character, objective) =>
		{
			character.WorldMessage(L("You have passed the Priest combat trial!"));
			character.Variables.Perm.Set("PriestCombatTrialPassed", true);
		};
		AddObjective("healingTest", "Demonstrate your healing abilities", new ManualObjective());

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
