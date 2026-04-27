//--- Melia Script ----------------------------------------------------------
// Escanciu Village Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Escanciu Village.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors;

public class FRemains39QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Gravegolem Graveyard
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Gravekeeper] Ema"), "f_remains_39", 1000, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1001);

			dialog.SetTitle(L("Ema"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Gravegolems have risen in the east plots. Twenty-five need breaking before the ceremony."));

				var response = await dialog.Select(L("Break the golems?"),
					Option(L("I'll kill"), "help"),
					Option(L("Ceremony?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Shatter the cores. They won't rebuild."));
						break;

					case "info":
						await dialog.Msg(L("Village remembrance rite. Every year."));
						break;

					case "leave":
						await dialog.Msg(L("Villagers grieve anyway."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGolems", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Plots quiet. Ceremony tomorrow."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ceremony went well. Thank you."));
			}
		});

		// Quest 2: Zolem Cores
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Stonewright] Vilis"), "f_remains_39", 400, -500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1002);

			dialog.SetTitle(L("Vilis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Zolems carry earth-cores. Kill twenty, bring six clean cores for masonry binding."));

				var response = await dialog.Select(L("Cores?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Masonry?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Heart of the beast. Sift the rubble."));
						break;

					case "info":
						await dialog.Msg(L("Bind stone to stone. Stronger than mortar."));
						break;

					case "leave":
						await dialog.Msg(L("Wall crumbles without."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killZolems", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCores", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Six cores. Wall holds."));
					character.Inventory.Remove(650240, character.Inventory.CountItem(650240), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Wall stands a generation now."));
			}
		});

		// Quest 3: Hook Thorn
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Seamstress] Rima"), "f_remains_39", -500, 200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1003);

			dialog.SetTitle(L("Rima"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Hook thorns work as curved needles. Kill twenty Hooks, five prime thorns."));

				var response = await dialog.Select(L("Needles?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Curved?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Longest thorn from the hump."));
						break;

					case "info":
						await dialog.Msg(L("For stitching boots. Saddler can't work without."));
						break;

					case "leave":
						await dialog.Msg(L("Boots unmade."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHooks", out var killObj)) return;
				if (!quest.TryGetProgress("gatherThorns", out var tObj)) return;

				if (killObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Five thorns. Boots go out this week."));
					character.Inventory.Remove(650245, character.Inventory.CountItem(650245), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Dozen pairs of boots shipped."));
			}
		});

		// Quest 4: Flog Swarm
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Beekeeper-Monk] Ambrozijs"), "f_remains_39", -600, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1004);

			dialog.SetTitle(L("Ambrozijs"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A monk in beekeeper's veil lifts a brood-comb to the light, frowning at the bees that aren't there*{/}"));
				await dialog.Msg(L("My hives have fed Escanciu's monastery for thirty years. Honey for the kitchens, wax for the candles, mead for the festival days. Last spring my Mother Abbess used to say I was the monastery's quiet treasury."));
				await dialog.Msg(L("Then the Flogs came. They chase my bees from the flowers and tear through my hive-canopies for the brood-combs. Four canopies torn open in a fortnight; the bees roost in the rain and die in the rain."));

				var response = await dialog.Select(L("Kill 25 Flying Flogs to push them off the cloister flowers, then re-thatch the four torn hive-canopies. Will you take the work, traveler?"),
					Option(L("I'll kill the Flogs and re-thatch the canopies"), "help"),
					Option(L("Why are the Flogs after the bees?"), "info"),
					Option(L("Find another beekeeper-hand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He blesses your blade with a wisp of beeswax-incense from a clay thurible*{/}"));
						await dialog.Msg(L("Twenty-five Flogs, no shortcuts. Strike low - they roost head-down on the cloister-trees and dive when threatened. The dive's their weakest moment."));
						await dialog.Msg(L("Then the four canopies in the cloister garden. Reed-thatch is stacked beside each one - lay it across the frame in three overlapping layers, weight the corners with garden-stones. The bees re-roost the same evening."));
						break;

					case "info":
						await dialog.Msg(L("Same flowers, same hours, same nectar. Bees and Flogs evolved on the same cloister flowers - until last spring they kept to opposite times of day. Bees mornings, Flogs evenings."));
						await dialog.Msg(L("Then a wet spring shifted the Flog roosting season. Now they overlap with the bees, and the bees lose - smaller, gentler, lower-priority on a Flog's hunting list. Wet weather kept the cloister flowers blooming, and that broke the truce."));
						break;

					case "leave":
						await dialog.Msg(L("Then the monastery starves slow. Honey first, then candles, then mead, then the brothers' patience. I will pray for someone less occupied."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFlogs", out var killObj)) return;
				if (!quest.TryGetProgress("rethatchCanopies", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("{#666666}*He smiles, a slow tired smile, and listens for a long moment to the cloister*{/}"));
					await dialog.Msg(L("Listen - the cloister hums again. Bees on the flowers, no Flog-shadows on the canopies. The first time in months I've heard the morning-hum at full volume."));
					await dialog.Msg(L("Take this. Beekeeper's purse, plus a sealed jar of last spring's honey from the Mother Abbess's stores. She insisted. The monastery owes you one whole season of breakfasts."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Twenty-five Flogs first. The canopies will tear again before they're up if a Flog is still hunting the cloister."));
				}
				else
				{
					await dialog.Msg(L("Flogs are gone. Now the four canopies - reed-thatch in three overlapping layers, garden-stones on each corner. The bees know to re-roost; we just have to make a roof."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Honey flows again, three jars to the kitchens this morning. The Mother Abbess sends her blessing - she said to thank 'the swordhand who became a beekeeper for an afternoon.' She has a poet's tongue when she chooses."));
			}
		});

		// Hive-canopy thatch points for Quest 1004
		//-------------------------------------------------------------------------
		void AddHiveCanopy(int canopyNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Hive-Canopy"), "f_remains_39", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_remains_39", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A reed-thatched hive canopy, half-torn open by Flog claws*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_remains_39.Quest1004.Canopy{canopyNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already re-thatched; bees clustering underneath*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Re-thatching canopy..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_remains_39.Quest1004.CanopiesThatched", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_remains_39.Quest1004.CanopiesThatched", count);
					character.ServerMessage(LF("Hive-canopies re-thatched: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All canopies re-thatched! Return to Beekeeper-Monk Ambrozijs.{/}"));
				}
				else
				{
					character.ServerMessage(L("Thatching interrupted."));
				}
			});
		}

		AddHiveCanopy(1, -500, 600, 0);
		AddHiveCanopy(2, -700, 400, 90);
		AddHiveCanopy(3, -400, 300, 180);
		AddHiveCanopy(4, -800, 700, 270);

		// Quest 5: The Hallowventor Menace
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Exorcist] Basilijs"), "f_remains_39", 200, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1005);
			var champSpawnedKey = "Laima.Quests.f_remains_39.Quest1005.ChampSpawned";

			dialog.SetTitle(L("Basilijs"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An exorcist trims a candle-wick to length, then sets the candle alight from a smaller one already burning*{/}"));
				await dialog.Msg(L("Escanciu village has a haunting. Not the noisy kind - this one is quiet. Things rearrange themselves overnight. Children dream of someone who isn't there. The bell on the chapel rings at three in the morning with no hand on the rope."));
				await dialog.Msg(L("There's a Hallowventor champion anchoring it. He doesn't haunt the village himself - his presence near the burial-stones lets weaker shades through into the houses. Until he's gone, the haunting deepens."));

				var response = await dialog.Select(L("Bless the three roadside shrines along the village path to break his hold on the dead, then kill 10 of his flock to draw him out. The chapel needs this done. Will you?"),
					Option(L("I'll bless the shrines and face the Champion"), "help"),
					Option(L("What kind of haunting?"), "info"),
					Option(L("That's priest-work entirely"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a small clay phial of consecrated oil and a folded slip of paper with the rite written in clear hand*{/}"));
						await dialog.Msg(L("Three shrines on the village path - oil on the lintel-stone, recite the rite from the paper, mark the doorway with your thumb. The shrines remember whoever blesses them last; choose your blessings carefully in life."));
						await dialog.Msg(L("Then ten Hallowventors. The Champion manifests around the eighth - tall, paler than mist, doesn't speak. End him quickly; he doesn't fight, but his presence drains the will from a swordhand."));
						break;

					case "info":
						await dialog.Msg(L("Quiet hauntings are the dangerous ones. Loud hauntings exhaust themselves; quiet hauntings settle in. Inside a year, the village forgets what an unhaunted morning feels like and starts blaming itself for the dread."));
						await dialog.Msg(L("Two of the Escanciu families have already moved their elders to Salvia. Two more are talking about it. Without the chapel, the village hollows out by autumn."));
						break;

					case "leave":
						await dialog.Msg(L("Then the haunting spreads, the chapel closes, the village hollows. I will keep a candle for you regardless. Some prayers go in both directions."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("blessShrines", out var bObj)) return;
				if (!quest.TryGetProgress("killFlock", out var fObj)) return;
				if (!quest.TryGetProgress("killChamp", out var cObj)) return;

				if (bObj.Done && fObj.Done && cObj.Done)
				{
					await dialog.Msg(L("{#666666}*He bows his head a long moment, then makes the sign of the candle over both your hands*{/}"));
					await dialog.Msg(L("Shrines blessed, Champion dispelled, village rests. The chapel-bell rang once at dawn this morning - one hand on the rope, that of the chapel-keeper. The first natural ring in months."));
					await dialog.Msg(L("Take this. Exorcist's purse, modest as our order requires. The village families will thank you in their own way - probably with bread you'll find on your bedroll for weeks."));
					character.Variables.Perm.Remove(champSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (fObj.Done && !cObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(champSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(champSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Hallowventor, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He manifests!"));
							character.ServerMessage(L("{#FF9966}The Hallowventor Champion manifests!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him before he fades."));
					}
				}
				else if (!bObj.Done)
				{
					await dialog.Msg(L("The three shrines first. Oil on the lintel, rite from the paper, mark the doorway. The Champion can't be drawn while his hold on the dead still runs the village path."));
				}
				else
				{
					await dialog.Msg(L("Shrines bless clean. Now ten of his flock - the Champion manifests around the eighth. Brace yourself; his presence drains the will."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Village sleeps and dreams again. The two families who moved their elders to Salvia are talking about bringing them back. The third family already has - the elder said the morning-quiet finally felt like quiet, not like waiting."));
			}
		});

		// Roadside shrines for Quest 1005
		//-------------------------------------------------------------------------
		void AddRoadsideShrine(int shrineNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Roadside Shrine"), "f_remains_39", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_remains_39", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weather-worn roadside shrine, candle stubs at the base*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_remains_39.Quest1005.Shrine{shrineNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already blessed; the candles burn steady*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Blessing shrine..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_remains_39.Quest1005.ShrinesBlessed", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_remains_39.Quest1005.ShrinesBlessed", count);
					character.ServerMessage(LF("Roadside shrines blessed: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All shrines blessed! Now bait out the Champion.{/}"));
				}
				else
				{
					character.ServerMessage(L("Blessing interrupted."));
				}
			});
		}

		AddRoadsideShrine(1, 200, 200, 0);
		AddRoadsideShrine(2, 400, 400, 90);
		AddRoadsideShrine(3, 100, 500, 180);

		// Quest 6: Escanciu Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Village Elder] Augustas"), "f_remains_39", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1006);

			dialog.SetTitle(L("Augustas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A village elder leans on a hawthorn-staff, watching the road from the chapel-step*{/}"));
				await dialog.Msg(L("Escanciu has stood here for two centuries. Nine generations of my family have been laid to rest in our burial-ground. The village pays its own way, sweeps its own road, and never asks Salvia for help unless we have to."));
				await dialog.Msg(L("Three monsters contest our patrol-circuit. Gravegolems wander out of the burial-ground. Zolems push up through the cellar-floors. Hooks roost in the rafters of the abandoned mill. We've never had all three at once before this season."));

				var response = await dialog.Select(L("Kill 12 Gravegolems, 12 Zolems, and 12 Hooks on the patrol-circuit, then ring the Village Bell to signal the elders the patrol's done. Will you walk the round?"),
					Option(L("I'll walk the round"), "help"),
					Option(L("Why ring the bell?"), "info"),
					Option(L("Find a younger swordhand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a copper key on a worn lanyard*{/}"));
						await dialog.Msg(L("Thirty-six kills, no padding. The patrol-circuit runs burial-ground, cellar-row, mill-loft, in that order. Gravegolems first - they wander, so easier in the morning."));
						await dialog.Msg(L("Bell's at the village square. Use the key to unlock the rope-housing. One long peal - the elders count from their windows. Two peals means trouble; one means clear."));
						break;

					case "info":
						await dialog.Msg(L("Village too small for a watch-runner. Bell carries to every house in Escanciu. The elders sleep in their daylight chairs at this age - the bell is what tells them the patrol-circuit is done and they can take their nap properly."));
						await dialog.Msg(L("Pelke's old custom from the demon war. We never went back. A bell peal is a small thing that means a great deal to old people in winter."));
						break;

					case "leave":
						await dialog.Msg(L("Then I pay the next swordhand who walks past, and they will. There always is one. There is rarely one I trust to ring the bell properly."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGolems", out var gObj)) return;
				if (!quest.TryGetProgress("killZolems", out var zObj)) return;
				if (!quest.TryGetProgress("killHooks", out var hObj)) return;
				if (!quest.TryGetProgress("ringBell", out var bObj)) return;

				if (gObj.Done && zObj.Done && hObj.Done && bObj.Done)
				{
					await dialog.Msg(L("{#666666}*He smiles at the bell-tower as if listening to an old friend, then counts coin into a wooden box*{/}"));
					await dialog.Msg(L("Patrol done, bell rung clean - I heard it from here, two seconds long, perfect form. The elders are taking their afternoon naps as we speak."));
					await dialog.Msg(L("Village fund, paid honest. And a loaf of dawn-bread the chapel-keeper baked for whoever did the round - she said you'd come by today, somehow."));
					character.Quests.Complete(questId);
				}
				else if (gObj.Done && zObj.Done && hObj.Done)
				{
					await dialog.Msg(L("Patrol complete. Now the bell at the square - copper key, rope-housing, one long peal. The elders are listening."));
				}
				else
				{
					await dialog.Msg(L("Burial-ground, cellar-row, mill-loft. Twelve of each. Take your time on the mill-loft - the rafters give if you weight one beam too long."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Another week of peace - first uninterrupted week in months. The chapel-keeper's dawn-bread is on the church step every morning now, with no name on the basket. We all pretend not to know who it's for."));
			}
		});

		// Village Bell for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Village Bell"), "f_remains_39", 50, 50, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*The village bell, hanging from a stone arch in the square*{/}"));
				return;
			}

			var rungKey = "Laima.Quests.f_remains_39.Quest1006.BellRung";
			if (character.Variables.Perm.GetBool(rungKey, false))
			{
				await dialog.Msg(L("{#666666}*Already rung; the elders heard*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killGolems", out var gObj)) return;
			if (!quest.TryGetProgress("killZolems", out var zObj)) return;
			if (!quest.TryGetProgress("killHooks", out var hObj)) return;

			if (!(gObj.Done && zObj.Done && hObj.Done))
			{
				await dialog.Msg(L("{#666666}*The rope is in your hand, but you haven't finished the patrol*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Ringing bell..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(rungKey, true);
				character.ServerMessage(L("{#FFD700}Village Bell rung. Return to Village Elder Augustas.{/}"));
			}
			else
			{
				character.ServerMessage(L("Ringing interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FRemains39Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1001);
		SetName(L("Gravegolem Graveyard"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Gravegolems risen in the east plots before the remembrance rite."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Gravekeeper] Ema"), "f_remains_39");

		AddObjective("killGolems", L("Kill Gravegolems"),
			new KillObjective(25, new[] { MonsterId.Gravegolem }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FRemains39Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1002);
		SetName(L("Earth-Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Zolems and gather earth-cores for masonry binding."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stonewright] Vilis"), "f_remains_39");

		AddObjective("killZolems", L("Kill Zolems"),
			new KillObjective(20, new[] { MonsterId.Zolem }));

		AddObjective("gatherCores", L("Gather earth-cores"),
			new CollectItemObjective(650240, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650240, character.Inventory.CountItem(650240), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650240, character.Inventory.CountItem(650240), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains39Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1003);
		SetName(L("Curved Needles"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hooks and gather thorns for seamstress needles."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Seamstress] Rima"), "f_remains_39");

		AddObjective("killHooks", L("Kill Hooks"),
			new KillObjective(20, new[] { MonsterId.Hook }));

		AddObjective("gatherThorns", L("Gather thorns"),
			new CollectItemObjective(650245, 5));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650245, character.Inventory.CountItem(650245), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650245, character.Inventory.CountItem(650245), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains39Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1004);
		SetName(L("Flog Swarm"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Flying Flogs raiding the monastery bee hives."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Beekeeper-Monk] Ambrozijs"), "f_remains_39");

		AddObjective("killFlogs", L("Kill Flying Flogs"),
			new KillObjective(25, new[] { MonsterId.Flying_Flog }));

		AddObjective("rethatchCanopies", L("Re-thatch the four hive-canopies"),
			new VariableCheckObjective("Laima.Quests.f_remains_39.Quest1004.CanopiesThatched", 4, true));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_39.Quest1004.CanopiesThatched");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_39.Quest1004.Canopy{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_39.Quest1004.CanopiesThatched");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_39.Quest1004.Canopy{i}");
	}
}

public class FRemains39Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1005);
		SetName(L("The Hallowventor Champion"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hallowventors to draw out their champion."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Exorcist] Basilijs"), "f_remains_39");

		AddObjective("blessShrines", L("Bless the three roadside shrines"),
			new VariableCheckObjective("Laima.Quests.f_remains_39.Quest1005.ShrinesBlessed", 3, true));

		AddObjective("killFlock", L("Kill Hallowventors"),
			new KillObjective(10, new[] { MonsterId.Hallowventor }));

		AddObjective("killChamp", L("Defeat the Champion"),
			new KillObjective(1, new[] { MonsterId.Hallowventor }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_39.Quest1005.ShrinesBlessed");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_39.Quest1005.Shrine{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_39.Quest1005.ShrinesBlessed");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_39.Quest1005.Shrine{i}");
	}
}

public class FRemains39Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1006);
		SetName(L("Village Patrol"));
		SetType(QuestType.Sub);
		SetDescription(L("Patrol Escanciu Village killing Gravegolems, Zolems, and Hooks."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Village Elder] Augustas"), "f_remains_39");

		AddObjective("killGolems", L("Kill Gravegolems"),
			new KillObjective(12, new[] { MonsterId.Gravegolem }));

		AddObjective("killZolems", L("Kill Zolems"),
			new KillObjective(12, new[] { MonsterId.Zolem }));

		AddObjective("killHooks", L("Kill Hooks"),
			new KillObjective(12, new[] { MonsterId.Hook }));

		AddObjective("ringBell", L("Ring the Village Bell"),
			new VariableCheckObjective("Laima.Quests.f_remains_39.Quest1006.BellRung", 1, true));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_39.Quest1006.BellRung");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_39.Quest1006.BellRung");
	}
}
