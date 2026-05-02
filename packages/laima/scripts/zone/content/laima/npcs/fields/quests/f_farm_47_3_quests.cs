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
		// Farmer Stanislovas - Cronewts ambushing farmhands; father of Goda
		//---------------------------------------------------------------------
		AddNpc(20138, L("[Farmer] Stanislovas"), "f_farm_47_3", -526, -273, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1001);
			var warningQuestId = new QuestId("f_farm_47_3", 1002);

			dialog.SetTitle(L("Stanislovas"));

			var warningDelivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_3.Quest1002.Delivered", 0) >= 1;

			if (character.Quests.IsActive(warningQuestId) && !warningDelivered)
			{
				await dialog.Msg(L("{#666666}*He unfolds Morta's letter and reads it through twice*{/}"));
				await dialog.Msg(L("Demon-pollen drifting south. I've smelled it on the wind already, but a written warning's different. Means Morta's serious enough to put it on paper."));
				await dialog.Msg(L("Tell her I'll have the farmhands wrap their faces and shut the barley sheds by nightfall. And Goda's been turning out masks faster than I thought — I'll send some over for Morta's lot too."));
				await dialog.Msg(L("{#666666}*He folds the letter and tucks it into his vest*{/}"));

				character.Variables.Perm.Set("Laima.Quests.f_farm_47_3.Quest1002.Delivered", 1);
				character.ServerMessage(L("{#FFD700}Warning delivered. Return to Farmer Morta.{/}"));
				return;
			}

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A thick-set farmer is pressing a bandaged arm, glancing toward his daughter Goda*{/}"));
				await dialog.Msg(L("The Cronewts are in my barley. Needlers, every one of them — poison arrows fired from the stalks before you even see 'em. Goda's stitching breathing-masks faster than I can clear the rows."));

				var response = await dialog.Select(L("Two farmhands down already. Tinker Dovas sent me his stormfeather traps, but traps don't stop arrows. I need the Needlers cleared out before harvest, or there won't be one."),
					Option(L("I'll kill the Needlers"), "help"),
					Option(L("Why are they here suddenly?"), "info"),
					Option(L("Find a hunter"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He grunts*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Watch the stalks — they hide low. If you hear a rustle, there's an arrow coming."));
						await dialog.Msg(L("And wear something heavy. Leather, if you've got it. The poison goes right through cloth."));
						break;

					case "info":
						await dialog.Msg(L("Ask the farmhands, they'll point you at the cracks."));
						await dialog.Msg(L("Demon-prison pollen leaked out a few months back, and the monsters followed it down. The Cronewts used to be peaceful up by the thorns — now they come south hunting anything that moves."));
						break;

					case "leave":
						await dialog.Msg(L("Klaipeda hunters want five times what I can pay. Won't matter much if the harvest fails anyway."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killNeedler", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lets out a long breath*{/}"));
					await dialog.Msg(L("Twenty clean kills. The farmhands'll actually sleep tonight, first time in a month."));
					await dialog.Msg(L("Here, take these — hunting leathers, passed down from my grandfather. He'd want them on someone who'll use them."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. Western barley, the aqueduct hedgerows. Listen for the rustle."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Harvest came in at three-quarters this year. Better than nothing, thanks to you."));
			}
		});

		// =====================================================================
		// QUEST 1002: Pollen Warning
		// =====================================================================
		// Farmer Morta - Warning for Shaton Farm
		//---------------------------------------------------------------------
		AddNpc(20116, L("[Farmer] Morta"), "f_farm_47_3", -1220, 579, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1002);

			dialog.SetTitle(L("Morta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A worried farmer is waving a folded letter, and sneezes twice*{/}"));
				await dialog.Msg(L("Pollen's drifting south now. You can taste it — metal, sort of sweet-rotten, makes your eyes water. The demon-prison crack on the north ridge has been leaking for weeks."));

				var response = await dialog.Select(L("Stanislovas works the barley fields east of here, near his daughter Goda's stitching shed. The pollen'll hit his sheds before anyone else's, and his farmhands haven't even started masking up. I wrote him a warning to wrap their faces and shut the sheds — can you carry it for me?"),
					Option(L("I'll deliver it"), "help"),
					Option(L("What's the demon prison?"), "info"),
					Option(L("Ask someone else"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She hands you the letter*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Tell him I'll send Keposeed husks next week — he can pass them to Goda for extra masks. Sooner everyone's wrapped up, fewer farmhands end up coughing blood."));
						break;

					case "info":
						await dialog.Msg(L("North of here there's an old prison for demons, sealed since before the war. Something broke the seal."));
						await dialog.Msg(L("Pollen leaks out wherever there's a crack. Monsters that used to be quiet are hungry now. The Klaipeda magistrates say they're 'investigating.' Meanwhile, we farm."));
						break;

					case "leave":
						await dialog.Msg(L("Then breathe through a cloth if you head south. Don't say I didn't warn you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverWarning", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She reads Stanislovas's reply and visibly relaxes*{/}"));
					await dialog.Msg(L("Sheds shut by nightfall, masks on every farmhand. Good. His barley survives another season, and ours might too."));
					await dialog.Msg(L("Here, take this. Small coin, but honest. If you pass Goda, let her know — she'll want those husks before supper."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("East from here, in the barley fields. Thick-set man, bandaged arm. Go quickly — the wind's picking up."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Stanislovas's farmhands are all masked up by now. Goda doubled her stitching pace. Keposeed husks for everyone."));
			}
		});

		// =====================================================================
		// Farmer Jonas - flavor NPC (no longer a quest recipient)
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Farmer] Jonas"), "f_farm_47_3", -1906, -96, 90, async dialog =>
		{
			dialog.SetTitle(L("Jonas"));
			await dialog.Msg(L("{#666666}*An old farmer with a long beard pulls his hood up against the rain*{/}"));
			await dialog.Msg(L("Three weeks of rain now. Three weeks. And the aqueduct up there's still running full — go figure."));
			await dialog.Msg(L("My boots haven't been dry in a month. The barley loves it. I don't."));
			await dialog.Msg(L("Every summer it's a new blight, every winter a new frost. Farming's mostly just bad weather and hope. Lately the weather's been winning."));
		});

		// =====================================================================
		// QUEST 1003: Anti-Pollen Masks
		// =====================================================================
		// Farmer Goda - Stitching breathing-masks from seed husks
		//---------------------------------------------------------------------
		AddNpc(147420, L("[Farmer] Goda"), "f_farm_47_3", -761, -261, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1003);

			dialog.SetTitle(L("Goda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer is stitching a seed-husk and a strip of linen into a crude mask, coughing twice into her elbow*{/}"));
				await dialog.Msg(L("The Kepa-pods have a hollow inner shell — tough but porous. Stitch one between two strips of linen and you've got a mask that filters out most of the demon-pollen. Not pretty, but it works."));

				var response = await dialog.Select(L("I can stitch one mask an hour, but I can't gather husks and stitch at the same time. The Keposeeds drift through the middle fields — shake a pod and the inner shell falls right out. I need five. Will you gather them?"),
					Option(L("I'll gather five husks"), "help"),
					Option(L("Do they even help?"), "info"),
					Option(L("I'm no pod-shaker"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She smiles and sets her stitching aside*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("The inner shell is the pale one — white-violet, about palm-sized. Leave the outer pod alone, that part's still alive."));
						await dialog.Msg(L("And cover your mouth while you gather. The husks are clean, but the air around them isn't."));
						break;

					case "info":
						await dialog.Msg(L("Better than breathing raw pollen, I can tell you that. Two farmhands keeled over last week. Now they wear masks and they're back out swinging scythes."));
						await dialog.Msg(L("Not pretty, sure, but my hands itch less and my eyes water less. That's something."));
						break;

					case "leave":
						await dialog.Msg(L("Then breathe shallow if you pass the fields. Take care."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var husksCount = character.Inventory.CountItem(662163);

				if (husksCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She turns each husk over in her hand*{/}"));
					await dialog.Msg(L("All clean — you didn't crush a single one. I'll have five more masks by supper."));
					await dialog.Msg(L("Take this — it's from last season's earnings. Morta and the others owe you as much as I do."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Five pods, pale inner shell. Shake gently, don't crush them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Every farmhand on Myrkiti has a mask now. The pollen still drifts, but at least we can breathe."));
			}
		});

		// =====================================================================
		// KEPOSEED PODS
		// =====================================================================
		// For Quest 1003 - Anti-Pollen Masks
		// =====================================================================

		void AddKeposeedPod(int podNumber, int x, int z, int direction)
		{
			AddNpc(152050, L("Drifting Kepa-Pod"), "f_farm_47_3", x, z, direction, async dialog =>
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
						character.ServerMessage(L("{#FFD700}All husks gathered! Return to Farmer Goda.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Shaking interrupted."));
				}
			});
		}

		AddKeposeedPod(1, -1039, -307, 44);
		AddKeposeedPod(2, -1364, -476, 44);
		AddKeposeedPod(3, -716, -414, 45);
		AddKeposeedPod(4, -1568, -115, 44);
		AddKeposeedPod(5, -1841, -9, 44);
		AddKeposeedPod(6, -1642, 534, 44);
		AddKeposeedPod(7, -1808, 271, 44);
		AddKeposeedPod(8, -1230, 438, 44);

		// =====================================================================
		// QUEST 1004: Portal Crack Survey
		// =====================================================================
		// Farmer Dominykas - Logging the demon-prison seepage
		//---------------------------------------------------------------------
		AddNpc(20118, L("[Farmer] Dominykas"), "f_farm_47_3", -2056, 836, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_3", 1004);

			dialog.SetTitle(L("Dominykas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An anxious farmer holds a rolled map, glancing up at the sky*{/}"));
				await dialog.Msg(L("Burrows. Tunnels dug into the earth — half a dozen across Myrkiti so far, mouths wide enough to fit a man's arm, and orange pollen drifting out of every one. Whatever's down there dug up, not down. And the magistrates in Klaipeda won't listen to 'farmer's worries' without evidence."));

				var response = await dialog.Select(L("I need someone to walk each burrow and mark it on my map. Proper notes — how wide the mouth is, what the pollen smells like, what's wilting around it. If I bring the magistrates a numbered survey, they'll have to send a mage out. Maybe seal the tunnels, maybe not — but at least send one."),
					Option(L("I'll survey the burrows"), "help"),
					Option(L("Why not a mage already?"), "info"),
					Option(L("I'm not a surveyor"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He unfolds the map with shaky hands*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("They're spread across the western fields. Look for ringed mounds of soil — that's the dirt from whatever dug the tunnel. The burrow itself is dark and smells like scorched metal."));
						await dialog.Msg(L("Don't reach into one. Pollen's bad enough at the rim. If any of them have widened since last week, come back as fast as you can."));
						break;

					case "info":
						await dialog.Msg(L("Mages cost silver, and farmers don't have silver. Klaipeda sends mages where the gold is, and we've been late on taxes two seasons running."));
						await dialog.Msg(L("The demon prison leaked for months before anyone in a city noticed. If the tunnels reach Klaipeda itself, maybe then. Until that happens, we survey our own."));
						break;

					case "leave":
						await dialog.Msg(L("Fine. But when the tunnels widen, I'll remember who wouldn't walk."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var cracksVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited", 0);

				if (cracksVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*He reads your notes, his expression hardening*{/}"));
					await dialog.Msg(L("Every one of them is widening. Two doubled in mouth-width since last month. The forked one's breathing pollen out of both arms now."));
					await dialog.Msg(L("I'll ride to Klaipeda tomorrow. This survey will get a mage sent — or I'll camp outside the magistrate's door until one comes. Thank you. Take this for the walking."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The burrows are out across the western fields. Look for ringed soil-mounds and the smell of scorched metal."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The magistrates sent a Pyromancer. She sealed two of the tunnels and packed the rest with salt. Not enough, but a start."));
			}
		});

		// =====================================================================
		// PORTAL CRACKS
		// =====================================================================
		// For Quest 1004 - Portal Crack Survey
		// =====================================================================

		void AddPortalCrack(int crackNumber, string crackName, string observation, int x, int z, int direction)
		{
			AddNpc(155010, L(crackName), "f_farm_47_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A burrow tunnel sunk into the field, its mouth ringed by mounded spoil. Faint orange pollen rises from the dark, smelling of scorched metal.*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_3.Quest1004.Crack{crackNumber}";
				var surveyed = character.Variables.Perm.GetBool(variableKey, false);

				if (surveyed)
				{
					await dialog.Msg(L("{#666666}*You've already surveyed this crack. The pollen here has not settled*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Surveying the burrow..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var cracksVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited", cracksVisited + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Burrows surveyed: {0}/4", cracksVisited + 1));

					if (SpawnTempMonsters(character, MonsterId.Hohen_Ritter_Green, 1, 50, TimeSpan.FromMinutes(2)))
					{
						character.ServerMessage(L("{#FF6666}A demon emerges from the burrow!{/}"));
					}

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

		AddPortalCrack(1, "Demon Burrow", "Narrow mouth, fresh spoil ringed around it. Pollen seeps out in slow orange threads. Soil around the rim scorched black.", -1644, 859, 44);
		AddPortalCrack(2, "Demon Burrow", "Wider mouth than the rest. Grass within five paces is yellow and slumped, leaves curled.", -1785, 522, 44);
		AddPortalCrack(3, "Demon Burrow", "Long, slot-shaped opening parallel to the cart-track. Rim crusted with a fine yellow film.", -1771, 225, 44);
		AddPortalCrack(4, "Demon Burrow", "Deepest of the lot - dark all the way down. Pollen rises in visible columns when the wind drops.", -1880, -219, 44);
		AddPortalCrack(5, "Demon Burrow", "Half-hidden under flattened barley. The stalks above it are root-rotted and brittle.", -1101, 558, 44);
		AddPortalCrack(6, "Demon Burrow", "Forked - two tunnel mouths diverging from a single spoil-ring. Both breathe pollen evenly.", -1337, 842, 44);
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
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1));  // Recovery Potion
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
		SetDescription("Farmer Morta needs a warning letter carried east to Farmer Stanislovas in the Myrkiti barley fields, so his farmhands can mask up and shut the sheds before the demon-pollen reaches them.");
		SetLocation("f_farm_47_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Morta", "f_farm_47_3");

		AddObjective("deliverWarning", "Deliver Morta's warning to Stanislovas",
			new VariableCheckObjective("Laima.Quests.f_farm_47_3.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
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
		SetDescription("Farmer Goda needs five Kepari inner shells gathered from drifting seed-pods so she can stitch breathing-masks for Myrkiti's farmhands.");
		SetLocation("f_farm_47_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Goda", "f_farm_47_3");

		AddObjective("collectHusks", "Gather inner shells from drifting Kepa-pods",
			new CollectItemObjective(662163, 5));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662163,
			character.Inventory.CountItem(662163),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
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

		for (int i = 1; i <= 8; i++)
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
		SetName("Demon Burrow Survey");
		SetType(QuestType.Sub);
		SetDescription("Farmer Dominykas has asked you to survey the demon-burrows tunneled across Myrkiti Farm so he can petition Klaipeda's magistrates for a proper sealing mage.");
		SetLocation("f_farm_47_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Dominykas", "f_farm_47_3");

		AddObjective("surveyCracks", "Survey the demon-prison burrows",
			new VariableCheckObjective("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited", 4, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(521142, 1)); // Hunting Pants
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited");
		for (int i = 1; i <= 6; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_3.Quest1004.Crack{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_3.Quest1004.CracksVisited");
		for (int i = 1; i <= 6; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_3.Quest1004.Crack{i}");
		}
	}
}
