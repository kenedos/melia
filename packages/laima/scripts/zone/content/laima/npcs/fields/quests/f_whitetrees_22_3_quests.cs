//--- Melia Script ----------------------------------------------------------
// Izoliacjia Plateau - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_whitetrees_22_3 map.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees223QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Black Beasts of the Plateau
		// =====================================================================
		// Sergeant Zigmas - Military reclamation follow-up
		//---------------------------------------------------------------------
		AddNpc(20128, L("[Sergeant] Zigmas"), "f_whitetrees_22_3", 1379, 897, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1001);

			dialog.SetTitle(L("Zigmas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A broad-shouldered sergeant scowls across the plateau, hand resting on his halberd*{/}"));
				await dialog.Msg(L("The commander in Orsha thinks clearing the Yakmap was enough. He's wrong."));

				var response = await dialog.Select(L("When Narvas Temple fell last spring, the demons inside didn't stay put. They've been pushing the Black Hohens out of their dens - and the Hohens are pushing toward our training grounds. The beasts are mad with fear, and mad beasts bite twice as hard."),
					Option(L("I'll kill them"), "help"),
					Option(L("What happened at the temple?"), "info"),
					Option(L("Sounds like your fight"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("Good. The Gulaks den in the western scree; the Manes hunt the central paths."));

						if (await dialog.YesNo(L("Cut twenty Manes and ten Gulaks. Break their numbers before the temple spillover gets worse. Can you do this?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Start with the Manes - they're the bolder of the two. The Gulaks you'll find west, near the temple warp."));
							await dialog.Msg(L("And stay well clear of the temple itself. Whatever Narvas is now, it isn't a place for soldiers."));
						}
						break;

					case "info":
						await dialog.Msg(L("{#666666}*His jaw tightens*{/}"));
						await dialog.Msg(L("Demons came up from somewhere beneath the foundations. Took the brothers before they could raise the alarm."));
						await dialog.Msg(L("The few who fled camp here on the plateau now. You might meet one - Brother Laimis. Don't press him on it."));
						break;

					case "leave":
						await dialog.Msg(L("Run if you must. I'll still be here holding the line."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMane", out var maneObj)) return;
				if (!quest.TryGetProgress("killGulak", out var gulakObj)) return;

				if (maneObj.Done && gulakObj.Done)
				{
					await dialog.Msg(L("{#666666}*He nods once, approving*{/}"));
					await dialog.Msg(L("That'll thin them enough to hold the line for another month. Maybe two."));
					await dialog.Msg(L("Take this. Veteran's tunic - served me well before the temple fell. You've earned it more than I need it now."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. Manes in the central grove, Gulaks west near the temple warp."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Hohens are quieter now. But the temple still bleeds demons. We'll need a real campaign before long."));
			}
		});

		// =====================================================================
		// QUEST 1002: Letter to the Abbot
		// =====================================================================
		// Quartermaster Rimvyda - Dispatch for Narvas refugees
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Quartermaster] Rimvyda"), "f_whitetrees_22_3", 1456, 1129, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1002);

			dialog.SetTitle(L("Rimvyda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A weary quartermaster tallies crates of salt pork and bandages, her ledger stained with dust*{/}"));
				await dialog.Msg(L("These supplies were bound for Narvas Temple. Now they're bound for whatever's left of Narvas."));

				var response = await dialog.Select(L("Brother Laimis leads the few monks who escaped the desecration. They camp near the western warp - they can't go home, and Orsha's walls are too full for refugees. I need someone to deliver a dispatch confirming we'll redirect the caravan to his camp."),
					Option(L("I'll carry the dispatch"), "help"),
					Option(L("What happened at Narvas?"), "info"),
					Option(L("I have my own concerns"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She presses a sealed scroll into your hand*{/}"));

						if (await dialog.YesNo(L("Follow the path west, past the central ruins, to the temple warp. You'll find Laimis in a cluster of grey tents. Hand him the dispatch, hear his reply, and come back. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The path's crossed by Hohens - mind yourself. If you have to run, run straight through."));
							await dialog.Msg(L("Laimis doesn't speak much. Whatever he tells you, bring it back word for word."));
						}
						break;

					case "info":
						await dialog.Msg(L("{#666666}*She sets down her quill and rubs her eyes*{/}"));
						await dialog.Msg(L("The Narvas brothers guarded something beneath the temple - relics from before the demon war, they said. Sealed vaults."));
						await dialog.Msg(L("The seals broke. Or were broken. The demons came up from below in a single night."));
						await dialog.Msg(L("Six brothers made it out. The rest... we don't know. We don't want to know."));
						break;

					case "leave":
						await dialog.Msg(L("Then move along. I've a hundred crates left to count."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverDispatch", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She reads Laimis's reply slowly, then folds it with care*{/}"));
					await dialog.Msg(L("Bless that man. He's still thinking of others even after what he's seen."));
					await dialog.Msg(L("Here - these are yours. The Narvas brothers asked me to thank anyone who helped them keep faith. Consider this their thanks and mine."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Laimis is at the western warp. Grey tents, quiet camp. Find him."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The caravan reached the refugee camp. For what it's worth, the brothers ate well this week."));
			}
		});

		// =====================================================================
		// Brother Laimis - Narvas refugee, recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Brother] Laimis"), "f_whitetrees_22_3", -1468, 102, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1002);

			dialog.SetTitle(L("Laimis"));

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A monk in soot-stained robes tends a small cook-fire, his rosary missing several beads*{/}"));
				await dialog.Msg(L("Peace to you, traveler. The plateau gives poor shelter, but better than where we came from."));
				return;
			}

			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_22_3.Quest1002.Delivered", 0) >= 1;

			if (delivered)
			{
				await dialog.Msg(L("Tell Rimvyda we are grateful. Tell her the brothers pray for Orsha's walls."));
				return;
			}

			await dialog.Msg(L("{#666666}*He reads the dispatch without expression, lips moving once in silent thanks*{/}"));
			await dialog.Msg(L("Tell Rimvyda the camp has twenty-three souls now. Two more found us yesterday - burned, but alive."));
			await dialog.Msg(L("Tell her we will pray for the caravan's safe passage. The demons test the western ridges every third night."));
			await dialog.Msg(L("{#666666}*He looks toward the temple warp, eyes empty*{/}"));
			await dialog.Msg(L("And tell her - if she finds an Orsha priest willing to ride out - we need someone to consecrate the graves we could not bury."));

			character.Variables.Perm.Set("Laima.Quests.f_whitetrees_22_3.Quest1002.Delivered", 1);
			character.ServerMessage(L("{#FFD700}Dispatch delivered. Return to Quartermaster Rimvyda.{/}"));
		});

		// =====================================================================
		// QUEST 1003: Tattered Standards
		// =====================================================================
		// Engineer Aldas - Restoring the training grounds
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Engineer] Aldas"), "f_whitetrees_22_3", -17, -16, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1003);

			dialog.SetTitle(L("Aldas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An engineer sketches foundation lines on a weathered map, mortar bucket at his feet*{/}"));
				await dialog.Msg(L("The commander wants the training grounds rebuilt by winter. I've got two apprentices and a plateau full of ghosts."));

				var response = await dialog.Select(L("Before the demons came, every barrack flew an Orsha standard. When the grounds fell, the Hohens dragged the banners into their dens for bedding. I need five torn flag fragments recovered - the old cloth still carries the regiment marks. They matter to the men who survived."),
					Option(L("I'll recover the fragments"), "help"),
					Option(L("Why does the cloth matter?"), "info"),
					Option(L("Sounds like busywork"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He sets down the chalk and nods seriously*{/}"));

						if (await dialog.YesNo(L("Five fragments, scattered across the plateau ruins. The Manes guard some of them - pulling a flag might bring one down on you. Still willing?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Look for the bright red - the dye held even through the demon war."));
							await dialog.Msg(L("Any fragment with a gold thread is from the old guard regiment. Those I'd particularly like back."));
						}
						break;

					case "info":
						await dialog.Msg(L("A standard is a soldier's second life. The cloth holds the names of every man who fought beneath it."));
						await dialog.Msg(L("The old sergeant at Narvas used to say the temple and the training grounds shared the same flag. Now both are ruins - but the flag, we can rebuild."));
						break;

					case "leave":
						await dialog.Msg(L("Then don't waste my mortar. I've walls to raise."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var fragmentCount = character.Inventory.CountItem(661037);

				if (fragmentCount >= 5)
				{
					await dialog.Msg(L("{#666666}*He spreads the fragments across his worktable*{/}"));
					await dialog.Msg(L("Four regiments accounted for... and one from Narvas. The brothers carried this into the temple the night it fell."));
					await dialog.Msg(L("{#666666}*He folds the Narvas fragment separately, reverently*{/}"));
					await dialog.Msg(L("Thank you. Every standard raised on the rebuilt wall is a name that didn't die forgotten."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Five fragments. Red cloth, gold thread where you can find it. Check the Mane dens."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The west wall flies three new standards this morning. The brothers stood and saluted when we raised them."));
			}
		});

		// =====================================================================
		// BANNER FRAGMENTS
		// =====================================================================
		// For Quest 1003 - Tattered Standards
		// =====================================================================

		void AddBannerFragment(int fragmentNumber, int x, int z, int direction)
		{
			AddNpc(160168, L("Tattered Orsha Standard"), "f_whitetrees_22_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_22_3", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A ragged piece of red cloth, half-buried in the ruin. The weave looks old*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_22_3.Quest1003.Fragment{fragmentNumber}";
				var collected = character.Variables.Perm.GetBool(variableKey, false);

				if (collected)
				{
					await dialog.Msg(L("{#666666}*The fragment is already recovered. Only a scrap of fallen thread remains*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_whitetrees_22_3.Quest1003.Fragment{fragmentNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 20)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Hohen_Mane_Black, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A Black Hohen Mane leaps from the rubble!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Recovering fragment..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(661037, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(661037);
					character.ServerMessage(LF("Banner fragments recovered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All fragments recovered! Return to Engineer Aldas.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Recovery interrupted."));
				}
			});
		}

		AddBannerFragment(1, 583, 873, 0);
		AddBannerFragment(2, -680, -1130, 90);
		AddBannerFragment(3, -25, 5, 180);
		AddBannerFragment(4, -1050, 290, 270);
		AddBannerFragment(5, -290, -600, 0);

		// =====================================================================
		// QUEST 1004: War Memorial Survey
		// =====================================================================
		// Historian Dainora - Archive expansion project
		//---------------------------------------------------------------------
		AddNpc(157100, L("[Historian] Dainora"), "f_whitetrees_22_3", -44, 1080, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1004);

			dialog.SetTitle(L("Dainora"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A historian in ink-stained robes unfolds a large sheet of rubbing paper*{/}"));
				await dialog.Msg(L("Four cadet memorials stand on this plateau. Four classes of recruits who died here before they ever became soldiers."));

				var response = await dialog.Select(L("Orsha's archive has no record of their names. The war took so much of our paper that we recorded only survivors. I want to change that - starting with these four stones. Each bears a roll call carved into the granite."),
					Option(L("I'll take the rubbings"), "help"),
					Option(L("Who were they?"), "info"),
					Option(L("Names don't bring people back"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She hands you a roll of paper and a chalk stick*{/}"));

						if (await dialog.YesNo(L("Press the paper against the stone, rub gently. The names will appear. Four memorials - they stand in the clearings where the cadets fell. Will you do it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Take your time. Each stone deserves a proper rubbing, not a hurried one."));
							await dialog.Msg(L("And - if you pass the westernmost stone - leave a pebble on top. It belongs to the class that tried to relieve Narvas Temple. They should not be forgotten even by me."));
						}
						break;

					case "info":
						await dialog.Msg(L("Three classes of cadets - boys and girls of fifteen, sixteen, seventeen. They held the plateau while Orsha's army was pinned south."));
						await dialog.Msg(L("The fourth memorial is for the class that marched from here to Narvas Temple the morning the demons broke the seals. None of them came back. Laimis was the only one who saw them fall."));
						break;

					case "leave":
						await dialog.Msg(L("That's a soldier's answer. The archive disagrees."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var memorialsVisited = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_22_3.Quest1004.MemorialsVisited", 0);

				if (memorialsVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*She smooths the rubbings across her lap and reads the names in silence*{/}"));
					await dialog.Msg(L("One hundred and forty-two cadets. Two volunteer cooks. One stable-boy who took a spear when the perimeter broke."));
					await dialog.Msg(L("Every name will have a line in the Orsha archive by the new year. Thank you - this is the quiet work that history forgets to do."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four memorials across the clearings. Press the paper, rub the chalk, move on."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The rubbings are bound into the archive now. Brother Laimis asked to read them when they were ready."));
			}
		});

		// =====================================================================
		// CADET MEMORIALS
		// =====================================================================
		// For Quest 1004 - War Memorial Survey
		// =====================================================================

		void AddCadetMemorial(int memorialNumber, int x, int z, int direction)
		{
			AddNpc(150231, L("Cadet Memorial"), "f_whitetrees_22_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_22_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered granite stone carved with rows of names. Some entries are fresh, some a generation old*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_22_3.Quest1004.Memorial{memorialNumber}";
				var observed = character.Variables.Perm.GetBool(variableKey, false);

				if (observed)
				{
					await dialog.Msg(L("{#666666}*You've already taken a rubbing from this stone*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Taking rubbing..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var memorialsVisited = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_22_3.Quest1004.MemorialsVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_whitetrees_22_3.Quest1004.MemorialsVisited", memorialsVisited + 1);

					character.ServerMessage(LF("Memorials surveyed: {0}/4", memorialsVisited + 1));

					if (memorialsVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All memorials surveyed! Return to Historian Dainora.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Rubbing interrupted."));
				}
			});
		}

		AddCadetMemorial(1, 770, 955, 0);
		AddCadetMemorial(2, -1455, 240, 90);
		AddCadetMemorial(3, 120, 35, 180);
		AddCadetMemorial(4, 180, -1180, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Black Beasts of the Plateau
//-----------------------------------------------------------------------------

public class BlackBeastsOfThePlateauQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1001);
		SetName("Black Beasts of the Plateau");
		SetType(QuestType.Sub);
		SetDescription("Sergeant Zigmas needs the Black Hohens killed before the demon spillover from the fallen Narvas Temple pushes them into Orsha's recovering training grounds.");
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Sergeant] Zigmas", "f_whitetrees_22_3");

		AddObjective("killMane", "Defeat Black Hohen Mane",
			new KillObjective(20, new[] { MonsterId.Hohen_Mane_Black }));
		AddObjective("killGulak", "Defeat Black Hohen Gulak",
			new KillObjective(10, new[] { MonsterId.Hohen_Gulak_Black }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11)); // Large SP Potion
		AddReward(new ItemReward(640012, 4));  // Recovery Potion
		AddReward(new ItemReward(531151, 1));  // Veteran Tunic
	}
}

// Quest 1002 CLASS: Letter to the Abbot
//-----------------------------------------------------------------------------

public class LetterToTheAbbotQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1002);
		SetName("Letter to the Abbot");
		SetType(QuestType.Sub);
		SetDescription("Quartermaster Rimvyda needs a dispatch carried to Brother Laimis, leader of the Narvas Temple refugees camped near the western warp.");
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Quartermaster] Rimvyda", "f_whitetrees_22_3");

		AddObjective("deliverDispatch", "Deliver the dispatch to Brother Laimis",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_22_3.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 10)); // Large HP Potion
		AddReward(new ItemReward(640007, 10));  // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_22_3.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_22_3.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Tattered Standards
//-----------------------------------------------------------------------------

public class TatteredStandardsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1003);
		SetName("Tattered Standards");
		SetType(QuestType.Sub);
		SetDescription("Engineer Aldas is rebuilding the training grounds and needs five torn Orsha regiment banner fragments recovered from the plateau ruins.");
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Engineer] Aldas", "f_whitetrees_22_3");

		AddObjective("collectFragments", "Recover tattered Orsha standards",
			new CollectItemObjective(661037, 5));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11));  // Large SP Potion
		AddReward(new ItemReward(640012, 3));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661037,
			character.Inventory.CountItem(661037),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_22_3.Quest1003.Fragment{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_22_3.Quest1003.Fragment{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661037,
			character.Inventory.CountItem(661037),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_22_3.Quest1003.Fragment{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_22_3.Quest1003.Fragment{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: War Memorial Survey
//-----------------------------------------------------------------------------

public class WarMemorialSurveyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1004);
		SetName("War Memorial Survey");
		SetType(QuestType.Sub);
		SetDescription("Historian Dainora has asked you to take rubbings from the four cadet memorials scattered across Izoliacjia Plateau.");
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Historian] Dainora", "f_whitetrees_22_3");

		AddObjective("visitMemorials", "Take rubbings from cadet memorials",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_22_3.Quest1004.MemorialsVisited", 4, true));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11));  // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_22_3.Quest1004.MemorialsVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_22_3.Quest1004.Memorial{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_22_3.Quest1004.MemorialsVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_22_3.Quest1004.Memorial{i}");
		}
	}
}
