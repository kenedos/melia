// --- Melia Script ----------------------------------------------------------
// Peltasta Advancement
// --- Description -----------------------------------------------------------
// Provides job advancement quest and npc.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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

public class PeltastaMasterNpc : GeneralScript
{
	private const int PeltastaMasterId = 20028;
	private const string PeltastaMasterName = "Quintus";
	private const string PeltastaMasterLocation = "c_Klaipe";
	private const int RequiredLevel = 15;
	private const int HiddenQuestItemId = ItemId.EP16_RAID_SHIELD;
	private const int QuestItemCount = 1;

	private const int CombatTrialMapId = 523;
	private const int CombatTrialDuration = 180; // Seconds
	private const int CombatTrialMobId = MonsterId.Goblin_Miners_Blue;

	protected override void Load()
	{
		// Register the "Peltasta" keyword
		RegisterKeyword("PELTASTA", L("Peltasta"),
			L("A shield-wielding warrior specializing in defense and crowd control."));

		// Add the Peltasta Master NPC to Klaipeda
		AddNpc(PeltastaMasterId, L($"[Peltasta Master] {PeltastaMasterName}"), PeltastaMasterLocation, -230, -397,
			90.0, PeltastaMasterDialog);

		// Prepare the combat trial map (if it's a dynamic map)
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var _) &&
			ZoneServer.Instance.Data.MapDb.TryFind(CombatTrialMapId, out var mapData))
		{
			var combatTrialMap = new DynamicMap(mapData.Id);
			ZoneServer.Instance.World.AddMap(combatTrialMap);
		}
	}

	private async Task PeltastaMasterDialog(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L($"Peltasta Master {PeltastaMasterName}"));
		dialog.SetPortrait("Dlg_port_PELTASTA_MASTER");

		// Debug Relationship Status
		// await dialog.DebugRelation(); 

		dialog.UpdateRelation();

		var continueDialog = true;
		while (continueDialog)
		{
			if (character.JobId == JobId.Swordsman)
			{
				if (dialog.GetMemory() == 0)
				{
					await dialog.Intro(L(
						"Welcome, warrior. I am Quintus, master of the Peltasta arts. We are the vanguard, the living shield that protects our allies. As Peltastas, we specialize in defense and crowd control."));
				}

				if (character.Job.Level >= RequiredLevel && character.Quests.IsActive(GetQuestId()))
				{
					if (!character.Variables.Perm.GetBool("PeltastaQuestMasterMet", false))
					{
						if (character.Variables.Perm.GetBool("FoundHiddenPeltastaNote", false) &&
							!character.Variables.Perm.GetBool("HiddenPeltastaQuestActive", false))
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
					else if (!character.Variables.Perm.GetBool("PeltastaKnowledgeTestPassed", false))
					{
						continueDialog = await KnowledgeTest(dialog);

						if (continueDialog) // Only prompt if the loop is not breaking
						{
							var continueDialogue = await dialog.Select(
								L("Do you wish to continue your training?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(L("Very well. Return when you are ready to proceed."));
								continueDialog = false;
							}
						}
					}
					else if (!character.Variables.Perm.GetBool("PeltastaCombatTrialPassed", false))
					{
						continueDialog = await CombatTrial(dialog);
						if (!continueDialog)
							break;
					}
					else if (!character.Variables.Perm.GetBool("PeltastaEthicalTestPassed", false))
					{
						continueDialog = await EthicalTest(dialog);

						if (continueDialog) // Only prompt if the loop is not breaking
						{
							var continueDialogue = await dialog.Select(L("Are you prepared for the final step?"),
								Option(L("Yes"), "continue"),
								Option(L("No"), "exit")
							);

							if (continueDialogue == "exit")
							{
								await dialog.Msg(
									L("I understand. Return when you are ready to embrace the Peltasta path."));
								continueDialog = false;
							}
						}
					}
					else
					{
						await CompleteJobChange(dialog);
						continueDialog = false; // Exit loop after job change
					}
				}
				else
				{
					await dialog.Msg(
						LF(
							"You show promise, young warrior, but you need more experience. Return when you've reached level {0}.",
							RequiredLevel));
					continueDialog = false; // Exit loop if level requirement not met
				}
			}
			else if (character.JobId == JobId.Peltasta)
			{
				await dialog.Msg(
					L(
						"Ah, one of my proud Peltastas! How goes your training? Remember, our shield is our strength and our pride."));
				continueDialog = false; // Exit loop if already a Peltasta
			}
			else
			{
				await dialog.Msg(
					L(
						"Greetings, traveler. While I admire your path, I'm afraid I can only train those who begin as warriors."));
				continueDialog = false; // Exit loop if another class
			}

			//await dialog.Msg(dialog.GetMoodMessage());
		} // End of while loop
	}

	private async Task<bool> OfferQuest(Dialog dialog)
	{
		var character = dialog.Player;

		var response = await dialog.Select(
			L(
				"Are you interested in the way of the Peltasta?  They are masters of the shield and guardians of their allies."),
			Option(L("Tell me more about Peltastas."), "moreInfo", () => !character.Variables.Temp.GetBool("PeltastaInfoSeen", false)),
			Option(L("Yes, I want to become a Peltasta!"), "acceptQuest"),
			Option(L("Not now."), "declineQuest")
		);

		switch (response)
		{
			case "moreInfo":
				await dialog.Msg(
					L(
						"Peltastas are renowned for their unwavering defense. They stand firm against any onslaught, shielding their allies from harm. With their sturdy shields, they can control the flow of battle, disrupting enemies and protecting the weak."));
				dialog.ModifyMemory(1);
				character.Variables.Temp.Set("PeltastaInfoSeen", true);
				break;
			case "acceptQuest":
				await dialog.Msg(
					L(
						"Excellent! The path of the Peltasta begins with understanding. Prove you are ready by passing my trials."));
				character.Variables.Perm.Set("PeltastaQuestMasterMet", true);
				character.Quests.CompleteObjective(GetQuestId(), "talkToQuintus");
				dialog.ModifyFavor(1);
				break;
			case "declineQuest":
				await dialog.Msg(
					L(
						"I understand. The path of a Peltasta is not for everyone. Return when you feel ready to embrace our ways."));
				return false;
		}

		return true;
	}

	private async Task OfferHiddenQuest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"You found that old note, eh? Interesting... It speaks of a shield even mightier than the one I usually ask for."));
		var response = await dialog.Select(
			L("Are you up for a greater challenge? Finding this shield would truly prove your dedication."),
			Option(L("Yes, I will find this shield."), "acceptHidden"),
			Option(L("Perhaps another time."), "declineHidden")
		);

		switch (response)
		{
			case "acceptHidden":
				await dialog.Msg(
					L(
						"Excellent! I can sense a powerful Peltasta in the making. Return to me with this shield, and I will gladly welcome you into our ranks."));
				character.Variables.Perm.Set("PeltastaQuestMasterMet", true);
				character.Variables.Perm.Set("HiddenPeltastaQuestActive", true);
				character.Quests.CompleteObjective(GetQuestId(), "talkToQuintus");
				dialog.ModifyFavor(5);
				break;
			case "declineHidden":
				await dialog.Msg(
					L(
						"That's alright. The standard test is still a worthy challenge. Come back when you're ready."));
				break;
		}
	}

	private async Task<bool> KnowledgeTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(L("Before we proceed, I must test your knowledge of the Peltasta's role and strategies."));

		var question1 = await dialog.Select(L("What is the primary duty of a Peltasta on the battlefield?"),
			Option(L("To deal massive damage to enemies."), "wrong"),
			Option(L("To protect allies from harm."), "correct"),
			Option(L("To scout ahead and gather information."), "wrong")
		);

		var question2 = await dialog.Select(L("Which of these shield techniques is essential for a Peltasta?"),
			Option(L("Crosscut."), "wrong"),
			Option(L("Shield Lob."), "correct"),
			Option(L("Dual Wielding."), "wrong")
		);

		if (question1 == "correct" && question2 == "correct")
		{
			await dialog.Msg(L("Impressive! You have a good grasp of Peltasta fundamentals."));
			character.Variables.Perm.Set("PeltastaKnowledgeTestPassed", true);
			character.Quests.CompleteObjective(GetQuestId(), "knowledgeTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(
				L(
					"Hmm, you need to study the Peltasta's ways more carefully. Come back when you're better prepared."));
			dialog.ModifyStress(1);
			return false;
		}
	}

	private async Task<bool> CombatTrial(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"A Peltasta must be able to withstand any onslaught! Enter the training ground and prove your resilience."));
		var choice = await dialog.Select(L("Are you ready to face the challenge?"),
			Option(L("Yes, I'm ready!"), "enterTrial"),
			Option(L("I need to prepare more."), "declineTrial")
		);

		if (choice == "declineTrial")
		{
			await dialog.Msg(L("Very well. Return when you feel confident in your abilities."));
			return false;
		}

		// Get the combat trial map (either existing or newly created)
		if (!ZoneServer.Instance.World.TryGetMap(CombatTrialMapId, out var combatTrialMap))
		{
			await dialog.Msg(L("The training grounds seem to be unavailable right now. Please try again later."));
			return false;
		}

		// Warp the player to the combat trial map
		character.Warp(combatTrialMap.Id, combatTrialMap.Data.DefaultPosition);

		// Spawn quest enemies.
		var dummies = new List<Mob>();
		for (var i = 0; i < 5; i++)
		{
			var enemy = new Mob(CombatTrialMobId, RelationType.Enemy);
			enemy.Components.Add(new MovementComponent(enemy));
			enemy.Components.Add(new AiComponent(enemy, "BasicMonster"));
			enemy.Components.Add(new LifeTimeComponent(enemy, TimeSpan.FromSeconds(CombatTrialDuration)));
			enemy.Visibility = ActorVisibility.Individual;
			enemy.VisibilityId = character.ObjectId;
			enemy.HasExp = false;
			enemy.HasDrops = false;
			combatTrialMap.AddMonster(enemy);
			dummies.Add(enemy);

			enemy.Died += (mob, killer) => dummies.Remove(mob);
		}

		return true;
	}

	private async Task<bool> EthicalTest(Dialog dialog)
	{
		var character = dialog.Player;

		await dialog.Msg(
			L(
				"A true Peltasta is not just a skilled warrior but also a protector of the innocent. Let's see if you possess the heart of a guardian."));
		await dialog.Msg(
			L(
				"Imagine you are escorting a group of villagers through a dangerous forest. Suddenly, you are ambushed by bandits!"));
		var choice = await dialog.Select(
			L(
				"The bandits are too strong for you to defeat alone. You can either fight bravely and protect the villagers, knowing you might fall, or flee and save yourself. What do you choose?"),
			Option(L("Stand and fight, even if it means my life."), "correct"),
			Option(L("Flee and save myself."), "wrong")
		);

		if (choice == "correct")
		{
			await dialog.Msg(
				L(
					"Your courage is admirable! A Peltasta is willing to sacrifice everything to protect the helpless. You have passed the final test."));
			character.Variables.Perm.Set("PeltastaEthicalTestPassed", true);
			character.Quests.CompleteObjective(GetQuestId(), "ethicalTest");
			dialog.ModifyFavor(1);
			return true;
		}
		else
		{
			await dialog.Msg(
				L(
					"A Peltasta's duty is to shield the innocent, even at their own peril. Think carefully about your choice and return when you understand the true meaning of our path."));
			dialog.ModifyFavor(-1);
			return false;
		}
	}

	private async Task CompleteJobChange(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Variables.Perm.GetBool("HiddenPeltastaQuestActive", false) &&
			character.Inventory.CountItem(HiddenQuestItemId) >= QuestItemCount)
		{
			await dialog.Msg(
				L("Incredible! You found the legendary shield! You possess the heart of a true Peltasta."));
			character.Inventory.Remove(HiddenQuestItemId, QuestItemCount);
		}
		else
		{
			await dialog.Msg(L("You have proven yourself worthy!  Arise, Peltasta!"));
		}
		character.Quests.Complete(this.GetQuestId());

		character.ChangeJob(JobId.Peltasta);
		character.AddKeyword("PELTASTA");

		await dialog.Msg(
			L("I officially declare you a Peltasta! May your shield always protect you and your allies."));
		dialog.ModifyFavor(1);
		dialog.ModifyMemory(1);
	}

	private int GetQuestId()
	{
		return 10_000_000 + (int)JobId.Peltasta;
	}
}

public class PeltastaJobChangeQuestScript : QuestScript
{
	protected override void Load()
	{
		SetId(10_000_000 + (int)JobId.Peltasta);
		SetName("Path of the Peltasta");
		SetDescription("Prove your worth to become a Peltasta by passing Quintus's trials.");
		SetAutoTracked(true);
		SetCancelable(true);

		AddObjective("talkToQuintus", "Seek out the Peltasta Master", new ManualObjective());
		AddObjective("knowledgeTest", "Pass Quintus's knowledge test", new ManualObjective());
		var objective = AddObjective("combatTrial", "Complete Quintus's combat trial",
			new KillObjective(5, MonsterId.Goblin_Miners_Blue));
		objective.Completed += (character, objective) =>
		{
			character.WorldMessage(L("You have successfully completed the Peltasta combat trial!"));
			character.Variables.Perm.Set("PeltastaCombatTrialPassed", true);

			Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(_ =>
			{
				// Warp the player back to the Peltasta Master
				character.Warp(1001, new Position(-122, 0, 784));
			});
		};
		AddObjective("ethicalTest", "Pass Quintus's ethical test", new ManualObjective());

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
