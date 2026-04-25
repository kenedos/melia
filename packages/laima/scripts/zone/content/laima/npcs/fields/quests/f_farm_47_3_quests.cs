//--- Melia Script ----------------------------------------------------------
// Myrkiti Farm - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_farm_47_3 map.
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

public class FFarm473QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Needlers in the Barley
		// =====================================================================
		// Farmer Stanislovas - Cronewts ambushing farmhands
		//---------------------------------------------------------------------
		AddNpc(20138, L("[Farmer] Stanislovas"), "f_farm_47_3", -559, -372, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1001);

			dialog.SetTitle(L("Stanislovas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A thick-set farmer presses a bandaged arm, staring grimly down a barley row*{/}"));
				await dialog.Msg(L("The Cronewts are in my barley. Needlers, every one - poison-tipped arrows they fire from the stalks before you even see 'em."));

				var response = await dialog.Select(L("Two farmhands down already this week. Tinker Dovas from Baron Allerno sent me his stormfeather traps, but traps don't stop arrows. I need them cleared out before harvest, or there won't be a harvest."),
					Option(L("I'll kill the Needlers"), "help"),
					Option(L("Why are they here suddenly?"), "info"),
					Option(L("Find a hunter"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He grunts, approving*{/}"));

						if (await dialog.YesNo(L("Twenty should break their hunting pattern. They favor the western rows and the aqueduct-side hedgerows. Can you handle it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Watch the stalks - they crouch low. Rustle means arrow incoming."));
							await dialog.Msg(L("And wear anything heavy. Leather, if you've got it. The poison sinks through cloth."));
						}
						break;

					case "info":
						await dialog.Msg(L("Ask the farmhands and they'll point at the cracks."));
						await dialog.Msg(L("Demon-prison pollen seeped out a few months back. The monsters followed it. Cronewts that lived peaceful up by the thorns came south hunting anything that moves."));
						break;

					case "leave":
						await dialog.Msg(L("Klaipeda hunters want five times what I can pay. Won't matter if the harvest fails anyway."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killNeedler", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He exhales heavily, shoulders dropping with relief*{/}"));
					await dialog.Msg(L("Twenty clean kills. The farmhands'll sleep tonight for the first time in a month."));
					await dialog.Msg(L("Take this - hunting leathers, passed down from my grandfather. He'd want them on someone who uses them."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. Western barley, aqueduct hedgerows. Listen for the rustle."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Harvest came in three-quarters this year. Better than nothing, thanks to you."));
			}
		});

		// =====================================================================
		// QUEST 1002: Pollen Warning
		// =====================================================================
		// Farmer Morta - Warning for Shaton Farm
		//---------------------------------------------------------------------
		AddNpc(20116, L("[Farmer] Morta"), "f_farm_47_3", -491, 38, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1002);

			dialog.SetTitle(L("Morta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A worried farmer waves a folded letter and sneezes twice*{/}"));
				await dialog.Msg(L("The pollen's drifting south now. You can taste it - metal, sweet-rotten, makes your eyes water. The demon-prison crack on the north ridge has been leaking for weeks."));

				var response = await dialog.Select(L("Jonas-the-Elder runs Shaton Farm through the southern warp. He's got three greenhouses of tomatoes - if the pollen reaches his panes, his whole season dies in a week. I wrote him a warning to shut the vents. Carry it for me?"),
					Option(L("I'll deliver it"), "help"),
					Option(L("What's the demon prison?"), "info"),
					Option(L("Ask someone else"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She presses the letter into your hand*{/}"));

						if (await dialog.YesNo(L("Through the south warp. Jonas is an old man, white beard down to his chest - can't miss him. Bring his reply back. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Tell him I'll send Keposeed husks next week - Eglė's stitching masks for our hands, and I'll spare him a dozen."));
						}
						break;

					case "info":
						await dialog.Msg(L("North of the thorn-maps there's a prison for demons - old, deep, sealed since before the war. Something broke the seal."));
						await dialog.Msg(L("Pollen leaks out where the cracks are. Monsters that were quiet before are hungry now. The Klaipeda magistrates say they're 'investigating.' Meanwhile, we farm."));
						break;

					case "leave":
						await dialog.Msg(L("Then breathe through a cloth if you go south. Don't say I didn't warn you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverWarning", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She reads Jonas's reply and presses her hands together in relief*{/}"));
					await dialog.Msg(L("Greenhouses sealed by sunset. Good. His tomatoes survive another season, ours might too."));
					await dialog.Msg(L("Here - my thanks. Small coin, but honest. Tell me if you pass Eglė; she'll want the husks before supper."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("South warp. White beard, runs the greenhouses. Go quickly - the wind's turning."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Jonas saved his tomato crop. Sent us a crate of them in thanks. The farmhands ate like nobility."));
			}
		});

		// =====================================================================
		// Farmer Jonas-the-Elder - Recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Farmer] Jonas-the-Elder"), "f_farm_47_3", -1628, -924, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1002);

			dialog.SetTitle(L("Jonas-the-Elder"));

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*An old farmer with a beard to his chest squints toward the distant greenhouses*{/}"));
				await dialog.Msg(L("Every summer a new blight. Every winter a new frost. Farming is just bad weather with hope attached."));
				return;
			}

			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_3.Quest1002.Delivered", 0) >= 1;

			if (delivered)
			{
				await dialog.Msg(L("Tell Morta the vents are shut. And tell her I'll trade husks for tomatoes, come autumn."));
				return;
			}

			await dialog.Msg(L("{#666666}*He reads the warning, eyebrows climbing*{/}"));
			await dialog.Msg(L("Demon-pollen, is it? Well. I've seen rot-mold and leaf-curl and the grey blight of '58. Add pollen to the list."));
			await dialog.Msg(L("Tell Morta I'll shut every vent and every pane by sunset. The tomatoes will hold."));
			await dialog.Msg(L("{#666666}*He squints north*{/}"));
			await dialog.Msg(L("And tell her I'm grateful. Not every farmer would spare a letter for an old man's greenhouses. Send her my thanks - and tomatoes, when they ripen."));

			character.Variables.Perm.Set("Laima.Quests.f_farm_47_3.Quest1002.Delivered", 1);
			character.ServerMessage(L("{#FFD700}Warning delivered. Return to Farmer Morta.{/}"));
		});

		// =====================================================================
		// QUEST 1003: Anti-Pollen Masks
		// =====================================================================
		// Farmer Eglė - Stitching breathing-masks from seed husks
		//---------------------------------------------------------------------
		AddNpc(147420, L("[Farmer] Eglė"), "f_farm_47_3", -546, -292, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1003);

			dialog.SetTitle(L("Eglė"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer stitches a seed-husk and a strip of linen into a crude mask, coughing twice into her elbow*{/}"));
				await dialog.Msg(L("The Kepa-pods have a hollow inner shell - tough, porous. Stitch one between linen and you've got a mask that filters most of the demon-pollen. Not pretty, but it works."));

				var response = await dialog.Select(L("I can stitch a mask an hour, but I can't gather husks and stitch both. The Keposeeds drift through the middle fields - shake a pod and the inner shell falls out. I need five. Will you gather them?"),
					Option(L("I'll gather five husks"), "help"),
					Option(L("Do they even help?"), "info"),
					Option(L("I'm no pod-shaker"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She pauses her stitching and smiles*{/}"));

						if (await dialog.YesNo(L("Five pods across the fields. Approach gently - the Keposeeds are skittish, they'll startle and fly. If one flies, just wait for the husk to settle. Ready?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The inner shell is the pale one - white-violet, about the size of your palm. Leave the outer pod; that part's alive."));
							await dialog.Msg(L("And cover your mouth while you gather. The husks are clean, but the air around them isn't."));
						}
						break;

					case "info":
						await dialog.Msg(L("Better than breathing raw pollen, I'll tell you that much. Two farmhands keeled over last week - now they wear masks and they're back swinging scythes."));
						await dialog.Msg(L("Maybe not pretty. But my hands itch less. My eyes water less. That's something."));
						break;

					case "leave":
						await dialog.Msg(L("Then breathe shallow if you pass the fields. Small mercies."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var husksCount = character.Inventory.CountItem(662163);

				if (husksCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She weighs the husks in her palm, nodding at each one*{/}"));
					await dialog.Msg(L("Clean separations. You didn't crush a single one. I'll have five more masks by supper."));
					await dialog.Msg(L("Take these - saved from last season's honest earnings. Morta and the rest owe you as much as I do."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Five pods. Pale inner shell. Shake gently, don't crush."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Every farmhand on Myrkiti has a mask now. The demon-pollen still drifts, but we breathe."));
			}
		});

		// =====================================================================
		// KEPOSEED PODS
		// =====================================================================
		// For Quest 1003 - Anti-Pollen Masks
		// =====================================================================

		void AddKeposeedPod(int podNumber, int x, int z, int direction)
		{
			AddNpc(152017, L("Drifting Kepa-Pod"), "f_farm_47_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_3", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pale-violet seed pod hovers low over the barley, its inner shell faintly visible*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_3.Quest1003.Pod{podNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The pod is hollow now. Only the outer shell drifts on, seeking new ground*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_farm_47_3.Quest1003.Pod{podNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Kepo_Seed_Violet, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A startled Keposeed bursts from the pod!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Shaking the pod..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(662163, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(662163);
					character.ServerMessage(LF("Inner shells gathered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All husks gathered! Return to Farmer Eglė.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Shaking interrupted."));
				}
			});
		}

		AddKeposeedPod(1, -605, -420, 0);
		AddKeposeedPod(2, -1114, -420, 90);
		AddKeposeedPod(3, -150, -730, 180);
		AddKeposeedPod(4, -77, 30, 270);
		AddKeposeedPod(5, -480, 50, 0);

		// =====================================================================
		// QUEST 1004: Portal Crack Survey
		// =====================================================================
		// Farmer Dominykas - Logging the demon-prison seepage
		//---------------------------------------------------------------------
		AddNpc(20118, L("[Farmer] Dominykas"), "f_farm_47_3", -2181, 430, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1004);

			dialog.SetTitle(L("Dominykas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An anxious farmer clutches a rolled map, glancing at the sky every few breaths*{/}"));
				await dialog.Msg(L("Four cracks, that I've counted. Four holes in the earth across Myrkiti, all leaking orange pollen. The magistrates in Klaipeda won't listen to 'farmer's worries' without evidence."));

				var response = await dialog.Select(L("I need someone to walk each crack and mark it on my map. Proper notes - shape, size, smell, what's growing wrong nearby. If I bring the magistrates a numbered survey, they'll have to send a mage. Maybe seal them. Maybe not. But at least send one."),
					Option(L("I'll survey the cracks"), "help"),
					Option(L("Why not a mage already?"), "info"),
					Option(L("I'm not a surveyor"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He unfolds the map with trembling hands*{/}"));

						if (await dialog.YesNo(L("Four cracks. Don't step in them - the pollen is thickest right at the edge. Note what you see and keep walking. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("One near the aqueduct, one in the old cabbage rows, one at the eastern fence line, one by the abandoned well."));
							await dialog.Msg(L("If any have grown wider since last week... come back faster than you think."));
						}
						break;

					case "info":
						await dialog.Msg(L("Mages cost silver. Farmers don't have silver. Klaipeda sends mages where the gold comes from - and we've been late on taxes two seasons running."));
						await dialog.Msg(L("The demon prison leaked for months before anyone in a city noticed. If the cracks reach Klaipeda itself, maybe then. Until then, we survey our own."));
						break;

					case "leave":
						await dialog.Msg(L("Fine. But when the cracks widen, I'll remember who wouldn't walk."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var cracksVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited", 0);

				if (cracksVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*He reads your notes, face tightening*{/}"));
					await dialog.Msg(L("All four widening. The cabbage-row one doubled since last month. The well-crack is deeper than a man."));
					await dialog.Msg(L("I'll ride to Klaipeda tomorrow. This survey will get a mage sent, or I'll camp outside the magistrate's door until one comes. Thank you - take this for the walking."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four cracks. Aqueduct, cabbage rows, east fence, abandoned well."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The magistrates sent a Pyromancer. She sealed two of the four cracks. Not enough, but a start."));
			}
		});

		// =====================================================================
		// PORTAL CRACKS
		// =====================================================================
		// For Quest 1004 - Portal Crack Survey
		// =====================================================================

		void AddPortalCrack(int crackNumber, string crackName, string observation, int x, int z, int direction)
		{
			AddNpc(153019, L(crackName), "f_farm_47_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A jagged crack in the earth breathes out faint orange pollen, smelling of scorched metal*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_3.Quest1004.Crack{crackNumber}";
				var surveyed = character.Variables.Perm.GetBool(variableKey, false);

				if (surveyed)
				{
					await dialog.Msg(L("{#666666}*You've already surveyed this crack. The pollen here has not settled*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Surveying the crack..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var cracksVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited", cracksVisited + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Cracks surveyed: {0}/4", cracksVisited + 1));

					if (cracksVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}Survey complete! Return to Farmer Dominykas.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Survey interrupted."));
				}
			});
		}

		AddPortalCrack(1, "Aqueduct Crack", "Hairline fissure along the aqueduct footing. Pollen thick; stonework blackened.", -2170, 415, 0);
		AddPortalCrack(2, "Cabbage-Row Crack", "Widest of the four. Cabbages within ten paces are pollen-yellow and slumped.", -1170, -240, 90);
		AddPortalCrack(3, "East Fence Crack", "Hidden under weeds. Fence posts warped where the pollen touched wood.", 1000, 50, 180);
		AddPortalCrack(4, "Abandoned Well Crack", "Deepest of the four. Pollen rises in visible threads from the well's edge.", -1630, -900, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Needlers in the Barley
//-----------------------------------------------------------------------------

public class NeedlersInTheBarleyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_3", 1001);
		SetName("Needlers in the Barley");
		SetType(QuestType.Sub);
		SetDescription("Farmer Stanislovas needs the Cronewt Poisoned Needlers cleared from Myrkiti's barley fields before the harvest fails.");
		SetLocation("f_farm_47_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Stanislovas", "f_farm_47_3");

		AddObjective("killNeedler", "Defeat Cronewt Poisoned Needlers",
			new KillObjective(20, new[] { MonsterId.Cronewt_Bow }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 4));  // Recovery Potion
		AddReward(new ItemReward(531142, 1));  // Hunting Armor
	}
}

// Quest 1002 CLASS: Pollen Warning
//-----------------------------------------------------------------------------

public class PollenWarningQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_3", 1002);
		SetName("Pollen Warning");
		SetType(QuestType.Sub);
		SetDescription("Farmer Morta needs a warning letter carried south to Jonas-the-Elder at Shaton Farm before the demon-pollen reaches his greenhouses.");
		SetLocation("f_farm_47_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Morta", "f_farm_47_3");

		AddObjective("deliverWarning", "Deliver Morta's warning to Jonas-the-Elder",
			new VariableCheckObjective("Laima.Quests.f_farm_47_3.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_3.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_3.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Anti-Pollen Masks
//-----------------------------------------------------------------------------

public class AntiPollenMasksQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_3", 1003);
		SetName("Anti-Pollen Masks");
		SetType(QuestType.Sub);
		SetDescription("Farmer Eglė needs five Kepari inner shells gathered from drifting seed-pods so she can stitch breathing-masks for Myrkiti's farmhands.");
		SetLocation("f_farm_47_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Eglė", "f_farm_47_3");

		AddObjective("collectHusks", "Gather inner shells from drifting Kepa-pods",
			new CollectItemObjective(662163, 5));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 3));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662163,
			character.Inventory.CountItem(662163),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_3.Quest1003.Pod{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_3.Quest1003.Pod{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662163,
			character.Inventory.CountItem(662163),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_3.Quest1003.Pod{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_3.Quest1003.Pod{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Portal Crack Survey
//-----------------------------------------------------------------------------

public class PortalCrackSurveyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_3", 1004);
		SetName("Portal Crack Survey");
		SetType(QuestType.Sub);
		SetDescription("Farmer Dominykas has asked you to survey the four demon-prison cracks across Myrkiti Farm so he can petition Klaipeda's magistrates for a proper sealing mage.");
		SetLocation("f_farm_47_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Dominykas", "f_farm_47_3");

		AddObjective("surveyCracks", "Survey the demon-prison cracks",
			new VariableCheckObjective("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited", 4, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_3.Quest1004.Crack{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_3.Quest1004.Crack{i}");
		}
	}
}
