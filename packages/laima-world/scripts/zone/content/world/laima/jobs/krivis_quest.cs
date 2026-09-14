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

public class KrivisMasterNpc : GeneralScript
{
	private const int QuestId = 10_000_000 + (int)JobId.Krivis;
	private const int KrivisMasterId = 57228; // Replace with the actual Krivis Master ID
	private const string KrivisMasterName = "Valdis"; // Replace with the actual name
	private const string KrivisMasterLocation = "c_Klaipe";
	private const int RequiredLevel = 15;
	private const int HiddenQuestItemId = ItemId.TOSHero_MACE; // Replace with a suitable powerful mace ID
	private const int QuestItemCount = 1;

	private const int CombatTrialMapId = 512; // Replace with a suitable map ID
	private const int CombatTrialDuration = 180; // Seconds
	private const int CombatTrialMobId = MonsterId.Ghost_Soldier; // Replace with a challenging mob ID

	protected override void Load()
	{
		// Register the "Krivis" keyword
		RegisterKeyword("KRIVIS", L("Krivis"),
			L("A priest who upholds ancient traditions, wielding divine fire to heal and protect or unleash devastating attacks."));

		// Add the Krivis Master NPC
		AddNpc(KrivisMasterId, L($"[Krivis Master] {KrivisMasterName}"), KrivisMasterLocation, -505, -2,
			45.0, KrivisMasterDialog);

		// Prepare the combat trial map (if it's a dynamic map)
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var _) &&
			ZoneServer.Instance.Data.MapDb.TryFind(CombatTrialMapId, out var mapData))
		{
			var combatTrialMap = new DynamicMap(mapData.Id);
			ZoneServer.Instance.World.AddMap(combatTrialMap);
		}
	}

	private async Task KrivisMasterDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L($"Krivis Master {KrivisMasterName}"));
		// No portrait for this master.
		// dialog.SetPortrait("Dlg_port_KRIVIS_MASTER");

		dialog.UpdateRelation();

		var continueDialog = true;
		while (continueDialog)
		{
			if (character.JobId == JobId.Cleric)
			{
				if (dialog.GetMemory() == 0)
				{
					await dialog.Intro(L(
						"Welcome, seeker of ancient wisdom. I am Valdis, a master of the Krivis tradition. We are keepers of the old ways, wielding divine fire to serve the goddesses."));
				}

				if (character.Job.Level >= RequiredLevel && character.Quests.IsActive(QuestId))
				{
					if (!character.Variables.Perm.GetBool("KrivisQuestMasterMet", false))
					{
						if (character.Variables.Perm.GetBool("FoundAncientKrivisArtifact", false) &&
							!character.Variables.Perm.GetBool("HiddenKrivisQuestActive", false))
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
					else if (!character.Variables.Perm.GetBool("KrivisKnowledgeTestPassed", false))
					{
						continueDialog = await KnowledgeTest(dialog);

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Do you wish to continue your Krivis training?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"The ancient knowledge takes time to comprehend. Return when your spirit is ready."));
								continueDialog = false;
							}
						}
					}
					else if (!character.Variables.Perm.GetBool("KrivisCombatTrialPassed", false))
					{
						continueDialog = await CombatTrial(dialog);
						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("KrivisDivineFlameTestPassed", false))
					{
						continueDialog = await DivineFlameTest(dialog); // New trial for Krivis

						if (continueDialog)
						{
							var continueDialogue = await dialog.Select(
								L("Have you embraced the divine flame within?"),
								Option(L("Yes"), "continue"),
								Option(L("Not yet"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L(
									"The divine flame requires careful tending. Return when you have mastered its power."));
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
							"Your faith is strong, but you need more experience. Return when you reach level {0}.",
							RequiredLevel));
					continueDialog = false;
				}
			}
			else if (character.JobId == JobId.Krivis)
			{
				await dialog.Msg(
					L(
						"Greetings, fellow Krivis. May the goddesses guide your path. Remember to always honor the ancient traditions."));
				continueDialog = false;
			}
			else
			{
				await dialog.Msg(
					L(
						"Welcome, traveler. My teachings are for those who follow the path of the Cleric."));
				continueDialog = false;
			}
		}
	}

	private async Task<bool> OfferQuest(Dialog dialog)
	{
		var character = dialog.Player;

		var response = await dialog.Select(
			L(
				"The Krivis path is ancient and demanding, requiring unwavering devotion to tradition. Are you willing to embrace the old ways?"),
			Option(L("Tell me more about the Krivis."), "moreInfo",
				() => !character.Variables.Temp.GetBool("KrivisInfoSeen", false)),
			Option(L("Yes, I wish to become a Krivis."), "acceptQuest"),
			Option(L("I'm not sure yet."), "declineQuest")
		);

		switch (response)
		{
			case "moreInfo":
				await dialog.Msg(
					L(
						"Krivis are priests who channel divine fire, using it to both heal and inflict damage. Our methods are steeped in tradition, passed down from generations of devoted worshipers."));
				dialog.ModifyMemory(1);
				character.Variables.Temp.Set("KrivisInfoSeen", true);
				return true;
			case "acceptQuest":
				await dialog.Msg(
					L(
						"You have chosen wisely. Your journey begins now. Prove your devotion through trials, and you shall become a true Krivis."));
				character.Variables.Perm.Set("KrivisQuestMasterMet", true);
				character.Quests.CompleteObjective(QuestId, "talkToValdis");
				dialog.ModifyFavor(1);
				return true;
			case "declineQuest":
				await dialog.Msg(
					L("The Krivis path is not for everyone.  Reflect on your choice and return if you feel the calling."));
				return false;
			default:
				return true;
		}
	}

	private async Task OfferHiddenQuest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L(
			"You possess an artifact imbued with the power of ancient Krivis! It emanates with a strong divine aura."));
		var response = await dialog.Select(
			L("Do you wish to unlock its secrets and embark on a more challenging path?"),
			Option(L("Yes, I accept this challenge."), "acceptHidden"),
			Option(L("I am not ready for such a task."), "declineHidden")
		);

		switch (response)
		{
			case "acceptHidden":
				await dialog.Msg(
					L(
						"Your spirit is bold.  Return to me when you have mastered the artifact's power."));
				character.Variables.Perm.Set("KrivisQuestMasterMet", true);
				character.Variables.Perm.Set("HiddenKrivisQuestActive", true);
				character.Quests.CompleteObjective(QuestId, "talkToValdis");
				dialog.ModifyFavor(5);
				break;
			case "declineHidden":
				await dialog.Msg(L("The traditional path is still a worthy one."));
				break;
		}
	}

	private async Task<bool> KnowledgeTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"To wield divine fire, you must understand its nature and the ancient rituals that invoke it. Let us test your knowledge."));

		var question1 = await dialog.Select(L("What is the primary source of a Krivis's power?"),
			Option(L("Their own inner strength."), "wrong"),
			Option(L("The ancient goddesses."), "correct"),
			Option(L("The spirits of nature."), "wrong")
		);

		var question2 = await dialog.Select(L("What is the significance of Daino, the sacred fire?"),
			Option(L("It represents purification and protection."), "correct"),
			Option(L("It symbolizes chaos and destruction."), "wrong"),
			Option(L("It embodies the balance between light and darkness."), "wrong")
		);

		if (question1 == "correct" && question2 == "correct")
		{
			await dialog.Msg(L("You have shown a deep understanding of Krivis teachings."));
			character.Variables.Perm.Set("KrivisKnowledgeTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "knowledgeTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(L(
				"Your knowledge of Krivis traditions needs more depth. Study the ancient texts and return."));
			dialog.ModifyStress(1);
			return false;
		}
	}

	private async Task<bool> CombatTrial(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"A Krivis must be able to defend the faith against any threat.  Prove your strength and courage in this trial."));
		var choice = await dialog.Select(L("Are you ready to face the challenge?"),
			Option(L("I am ready."), "enterTrial"),
			Option(L("I need to prepare."), "declineTrial")
		);

		if (choice == "declineTrial")
		{
			await dialog.Msg(
				L("Return when your body and spirit are prepared for battle."));
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

	private async Task<bool> DivineFlameTest(Dialog dialog)
	{
		var character = dialog.Player;

		// Implement the Divine Flame Test here. This could involve:
		// - Using fire magic to cleanse a corrupted area.
		// - Demonstrating the ability to heal and damage using divine fire.
		// - Completing a ritual that requires precise control of fire magic.
		// ...

		if (/* condition for passing the Divine Flame Test */ true)
		{
			await dialog.Msg(L("You have truly embraced the divine flame!"));
			character.Variables.Perm.Set("KrivisDivineFlameTestPassed", true);
			character.Quests.CompleteObjective(QuestId, "divineFlameTest");
			dialog.ModifyFavor(15);
			return true;
		}
		else
		{
			await dialog.Msg(L(
				"Your control over the divine flame is not yet complete. Continue your practice and return."));
			dialog.ModifyStress(5);
			return false;
		}
	}

	private async Task CompleteJobChange(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Variables.Perm.GetBool("HiddenKrivisQuestActive", false) &&
			character.Inventory.CountItem(HiddenQuestItemId) >= QuestItemCount)
		{
			await dialog.Msg(
				L(
					"The ancient artifact has accepted you as its wielder.  You have proven your devotion to the old ways."));
			character.Inventory.Remove(HiddenQuestItemId, QuestItemCount);
			character.Quests.Complete(QuestId);
		}
		else
		{
			await dialog.Msg(L(
				"You have walked the path of the Krivis with honor and respect for tradition. Arise, Krivis!"));
			character.Quests.Complete(QuestId);
		}

		character.ChangeJob(JobId.Krivis);
		character.AddKeyword("KRIVIS");

		await dialog.Msg(
			L(
				"May the divine fire guide and protect you, Krivis. Go forth and spread the wisdom of the goddesses."));
		dialog.ModifyFavor(1);
		dialog.ModifyMemory(1);
	}
}

public class KrivisJobChangeQuestScript : QuestScript
{
	protected override void Load()
	{
		SetId("Laima.JobQuest", (int)JobId.Krivis); // Unique ID for Krivis quest
		SetName("Path of the Krivis");
		SetDescription("Prove your devotion to the ancient ways and become a Krivis under Valdis's guidance.");
		SetAutoTracked(true);
		SetCancelable(true);

		AddObjective("talkToValdis", "Seek out the Krivis Master", new ManualObjective());
		AddObjective("knowledgeTest", "Pass Valdis's knowledge test", new ManualObjective());
		var objective = AddObjective("combatTrial", "Complete Valdis's combat trial",
			new KillObjective(5, MonsterId.Ghoul)); // Adjust enemy count and type as needed
		objective.Completed += (character, objective) =>
		{
			character.WorldMessage(L("You have passed the Krivis combat trial!"));
			character.Variables.Perm.Set("KrivisCombatTrialPassed", true);
		};
		AddObjective("divineFlameTest", "Demonstrate mastery over the divine flame", new ManualObjective());

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
