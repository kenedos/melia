//--- Melia Script ----------------------------------------------------------
// Mesafasla Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the Mesafasla tableland, paired with Stogas.
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

public class FTableland281QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Bunny Overrun
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Plateau-Warden] Stogas"), "f_tableland_28_1", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1001);

			dialog.SetTitle(L("Stogas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Repusbunnies strip the plateau grass to bare rock. Kill forty before the tableland starves."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Grass?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. The warren's east of the crag."));
						break;

					case "info":
						await dialog.Msg(L("Thin soil up here. Once the grass goes, the wind takes the dirt."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau bleeds."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Herd's trimmed. Grass gets a season."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Seedlings sprouting on the east ridge already."));
			}
		});

		// Quest 2: Bow-Bunny Fletchings
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Fletcher] Vaidile"), "f_tableland_28_1", 800, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1002);

			dialog.SetTitle(L("Vaidile"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bow-Repusbunnies carry reed-fletchings the wind-hunters envy. Kill twenty, bring eight fletchings."));

				var response = await dialog.Select(L("Fletchings?"),
					Option(L("I'll bring"), "help"),
					Option(L("Reed?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't crease them."));
						break;

					case "info":
						await dialog.Msg(L("Plateau-reed fletches straighter than anything lowland. Wind shapes it."));
						break;

					case "leave":
						await dialog.Msg(L("Arrows stay crooked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFletchings", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Eight good ones. Quiver's worth a week."));
					character.Inventory.Remove(650266, character.Inventory.CountItem(650266), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First quiver flies true. Crag-hunters owe me."));
			}
		});

		// Quest 3: Mage Ember-Dust
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Alchemist] Rutenis"), "f_tableland_28_1", -500, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1003);

			dialog.SetTitle(L("Rutenis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Saltisdaughter Mages burn ember-dust in their palms. Kill sixteen, bring six dust pinches."));

				var response = await dialog.Select(L("Dust?"),
					Option(L("I'll bring"), "help"),
					Option(L("Ember?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Wax-paper packets. Don't breathe it."));
						break;

					case "info":
						await dialog.Msg(L("Ember-dust holds heat weeks after the mage dies. Lamp-oil of the plateau."));
						break;

					case "leave":
						await dialog.Msg(L("Lamps stay cold."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherDust", out var dObj)) return;

				if (killObj.Done && dObj.Done)
				{
					await dialog.Msg(L("Six pinches. Lamps for a month."));
					character.Inventory.Remove(650267, character.Inventory.CountItem(650267), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Whole plateau lit tonight. First time in years."));
			}
		});

		// Quest 4: Rootcrystal Field
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Crystal-Cutter] Silute"), "f_tableland_28_1", -1100, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1004);

			dialog.SetTitle(L("Silute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A crystal-cutter measures a fresh fissure with a length of brass-wire, marking the depth on a scribe-stick*{/}"));
				await dialog.Msg(L("Tableland bluff is two hundred feet at the south crag. Bedrock holds it up; bedrock is what the rootcrystals are pushing through. Each crystal that surfaces splits the shelf a fingerwidth wider."));
				await dialog.Msg(L("My grandfather told me the bluff would last a thousand years. He was wrong - the rootcrystals weren't here in his time. If I can't gauge the worst cracks before next thaw, the south crag drops and takes the militia camp with it."));

				var response = await dialog.Select(L("Break 14 Rootcrystals to halt the push, then press a wedge into each of the four paint-marked shelf-cracks. The wedges tell me which cracks are still spreading. Will you do both?"),
					Option(L("I'll break the crystals and gauge the cracks"), "help"),
					Option(L("Why not just evacuate the camp?"), "info"),
					Option(L("Bluff work isn't my work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She presses four iron wedges and a small mallet into your pack*{/}"));
						await dialog.Msg(L("Fourteen crystals first. Strike low, mind the spray - shards carry the same crack-energy that splits the bedrock. They'll cut clean through boot-leather."));
						await dialog.Msg(L("The four shelf-cracks are paint-marked yellow. Press the wedge into each crack until it stops, then tap once with the mallet. If it sinks further on the tap, the crack is still spreading. If it holds, the crack has settled."));
						break;

					case "info":
						await dialog.Msg(L("The militia camp would evacuate in a day. The waystone-shrine on the south crag would not - it's stone-anchored, three centuries old, the only consecrated shrine on the high tableland."));
						await dialog.Msg(L("Lose the shrine and pilgrims stop coming through. Lose the pilgrims and the camp loses its reason. The whole settlement runs on the bluff being there in the morning. So we save the bluff."));
						break;

					case "leave":
						await dialog.Msg(L("Then the south crag drops at the next bad thaw and the shrine goes with it. I will measure the cracks alone and write down their numbers and watch the line widen on my own gauge. That has to be enough."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("gaugeCracks", out var gObj)) return;

				if (killObj.Done && gObj.Done)
				{
					await dialog.Msg(L("{#666666}*She reads each wedge in turn against her brass-wire gauge, then exhales slow*{/}"));
					await dialog.Msg(L("Crystals broken, all four wedges holding. Shelf has settled - the bluff stands another year, maybe two if the next thaw is gentle."));
					await dialog.Msg(L("Crystal-cutter's purse, plus a stipend from the shrine-cantors. They asked me to thank the swordhand who saved their stones - I'm doing it for them, since they're up at the shrine praying you'll come back through some day."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Crystals first. Wedging cracks while the bedrock still moves under my feet would just teach the wedges the wrong reading."));
				}
				else
				{
					await dialog.Msg(L("Crystals down, bedrock settling. Now the four wedges - paint-marked cracks, mallet-tap, watch for sink. Take your time on the readings."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Surveyors walked the bluff at first thaw. All four wedges held, the south crag is stable, the waystone-shrine still stands. The shrine-cantors say they'll inscribe your description on the lintel - I told them you wouldn't want it. They're inscribing it anyway."));
			}
		});

		// Shelf-crack gauge points for Quest 1004
		//-------------------------------------------------------------------------
		void AddShelfCrack(int crackNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Shelf-Crack"), "f_tableland_28_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_28_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A paint-marked crack splitting the bedrock shelf*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_28_1.Quest1004.Crack{crackNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already wedged; the crack reads steady*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Wedging crack..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_28_1.Quest1004.CracksGauged", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_28_1.Quest1004.CracksGauged", count);
					character.ServerMessage(LF("Shelf-cracks gauged: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All cracks gauged! Return to Crystal-Cutter Silute.{/}"));
				}
				else
				{
					character.ServerMessage(L("Wedging interrupted."));
				}
			});
		}

		AddShelfCrack(1, -1000, 200, 0);
		AddShelfCrack(2, -1300, -100, 90);
		AddShelfCrack(3, -900, -200, 180);
		AddShelfCrack(4, -1200, 300, 270);

		// Quest 5: The Warren-Matriarch
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Gediminas"), "f_tableland_28_1", 1800, -300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1005);
			var matriarchSpawnedKey = "Laima.Quests.f_tableland_28_1.Quest1005.MatriarchSpawned";

			dialog.SetTitle(L("Gediminas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty hunter strings a horn-bow with a single fluid motion, the bow-arms creaking pleasantly*{/}"));
				await dialog.Msg(L("Bow-Repusbunny Matriarch runs the deep warren below the west crag. Old, accurate enough at fifty paces to put an arrow through your eye-socket, smart enough to never come up except to scout."));
				await dialog.Msg(L("Three scouting-stones at the burrow-mouths let her see the surface without surfacing. Foul the stones - bear-grease blinds them - and she has to come up personally to look. That's when we have her."));

				var response = await dialog.Select(L("Foul the three deep-burrow scouting-stones with bear-grease, then kill 10 Bow-Repusbunny archers to flush her up. She's a horn-bow shot at fifty paces. Will you take her?"),
					Option(L("I'll take the Matriarch"), "help"),
					Option(L("Why not just dig her out?"), "info"),
					Option(L("Find a horn-bow specialist"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She tosses you a small clay tub of bear-grease wrapped in oilcloth*{/}"));
						await dialog.Msg(L("Three stones, three smears. Use the back of your knife to spread the grease - skin-touch lasts a day, knife-spread lasts a week. The matriarch reads scent off the stone, not sight."));
						await dialog.Msg(L("Then ten archers. She surfaces around the eighth, takes one shot from cover, then closes for melee. Stay below the burrow-mound's lip; she shoots flat trajectory and a high stance is a free target."));
						break;

					case "info":
						await dialog.Msg(L("Digging out a Repusbunny warren takes a sapper-team of ten and three days. Killing the matriarch in a fight takes one swordhand and an afternoon. Mathematics."));
						await dialog.Msg(L("And the warren collapses in on itself once she dies - the surviving Repusbunnies disperse, the burrow-network falls in on the deep chambers, the whole problem ends. With a sapper-dig, the Repusbunnies just relocate to the next set of crags."));
						break;

					case "leave":
						await dialog.Msg(L("Warren grows. She breeds three new archer-cohorts a season. Inside a year the militia camp is ringed with bow-fire and we abandon the bluff. I'll be here, sharpening, waiting."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("foulStones", out var sObj)) return;
				if (!quest.TryGetProgress("killScouts", out var pObj)) return;
				if (!quest.TryGetProgress("killMatriarch", out var aObj)) return;

				if (sObj.Done && pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("{#666666}*She unstrings the horn-bow with the same fluid motion as before, satisfied as a hunter who has finished the day's work*{/}"));
					await dialog.Msg(L("Stones fouled, matriarch down, warren collapsed in on itself by sundown. The militia camp can sleep with both eyes shut tonight - first time in months."));
					await dialog.Msg(L("Bounty plus a horn-bow stipend; you used your own bow on her last shot, the militia council voted it counted as horn-work. Drink to the warren that won't ring our crags anymore."));
					character.Variables.Perm.Remove(matriarchSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(matriarchSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(matriarchSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Repusbunny_Bow_Green, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She comes!"));
							character.ServerMessage(L("{#FF9966}The Warren-Matriarch bursts from the deep burrow!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find her."));
					}
				}
				else if (!sObj.Done)
				{
					await dialog.Msg(L("Three scouting-stones first. Bear-grease, knife-spread, scent-blind. She won't surface while she can still see the surface from below."));
				}
				else
				{
					await dialog.Msg(L("Stones are fouled, she's blind. Now ten archers - she surfaces around the eighth, takes one shot, then closes."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Burrows collapsed in on themselves, plateau's quiet, bow-fire gone. The militia camp posted a thank-you on the canteen door - I read it three times before I understood it was meant for you. They aren't a poetic crowd."));
			}
		});

		// Scouting-stone foul points for Quest 1005
		//-------------------------------------------------------------------------
		void AddScoutingStone(int stoneNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Scouting-Stone"), "f_tableland_28_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_28_1", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A polished scouting-stone, scent-marked by something deep underground*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_28_1.Quest1005.Stone{stoneNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already greased; the scent runs sour*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Fouling stone..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_28_1.Quest1005.StonesFouled", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_28_1.Quest1005.StonesFouled", count);
					character.ServerMessage(LF("Scouting-stones fouled: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All scouting-stones fouled! Now bait out the Matriarch.{/}"));
				}
				else
				{
					character.ServerMessage(L("Fouling interrupted."));
				}
			});
		}

		AddScoutingStone(1, 1700, -200, 0);
		AddScoutingStone(2, 1900, -500, 90);
		AddScoutingStone(3, 1600, -400, 180);

		// Quest 6: Tableland Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Tautvydas"), "f_tableland_28_1", 1700, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1006);

			dialog.SetTitle(L("Tautvydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A militia-captain pins a fresh sweep-roster to a board, weighting it with a small bronze bell-pull*{/}"));
				await dialog.Msg(L("Tableland plateau is bigger than the militia can patrol on foot. So we contract the sweeps out and signal the camp by bell when each one's done. Old mountain custom - the bell carries up the plateau when nothing else does."));
				await dialog.Msg(L("Three species contest the plateau: Repusbunnies on the open ground, Bow-Repusbunnies on the crag-rim, Saltisdaughter Mages in the salt-flats. We need each species' count down before the camp can sleep through a full night."));

				var response = await dialog.Select(L("Kill 12 Repusbunnies, 12 Bow-Repusbunnies, and 12 Saltisdaughter Mages, then ring the Crag-Bell at the camp edge - one peal, carries the whole plateau. Take the contract?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why not just post more militia?"), "info"),
					Option(L("Find another swordhand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She unhooks the bell-pull and drops it onto your palm with a small clink*{/}"));
						await dialog.Msg(L("Thirty-six kills, no padding. The dawn-patrol counts the corpse-piles before sun-up."));
						await dialog.Msg(L("Crag-Bell sits on a stone arch at the camp edge. Hook the pull onto the rope, draw down once, count to three, release. Single long peal. The camp knows the difference between sweep-bell, danger-bell, and meal-bell - don't muddle the count."));
						break;

					case "info":
						await dialog.Msg(L("More militia means more wages, more rations, more wagons up a road that washes out twice a year. The camp is forty strong because forty is what the budget allows. We supplement with contractors who don't need permanent rations."));
						await dialog.Msg(L("It's not the system I'd build. It's the system the council funded. The bell-tradition keeps the contract-work honest at least."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau stays wild and the dawn-patrol takes more losses. I'll be at the bell-board when you reconsider."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var bObj)) return;
				if (!quest.TryGetProgress("killArchers", out var aObj)) return;
				if (!quest.TryGetProgress("killMages", out var mObj)) return;
				if (!quest.TryGetProgress("ringBell", out var lObj)) return;

				if (bObj.Done && aObj.Done && mObj.Done && lObj.Done)
				{
					await dialog.Msg(L("{#666666}*She listens for a moment to the wind, half-smiling at the bell-echo still rolling across the plateau*{/}"));
					await dialog.Msg(L("Bell-echo's still carrying. Sweep done, count holds, dawn-patrol can rest the morning. The plateau hasn't been this quiet at sundown all season."));
					await dialog.Msg(L("Coin in full plus a half-purse for the clean bell-form - some contractors muddle the peals and we have to send a runner. You didn't. That's worth the extra."));
					character.Quests.Complete(questId);
				}
				else if (bObj.Done && aObj.Done && mObj.Done)
				{
					await dialog.Msg(L("Sweep done. Now the Crag-Bell at the camp edge - bell-pull, single peal, count to three. Don't sound it twice; that's the danger-call and the camp will turn out armed."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each. The salt-flats are the worst stretch - Saltisdaughter Mages cluster around the brine-pools. Do the open-ground first if you're flagging."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Militia camps the crag in shifts now, paid by your sweep-levy. The dawn-patrol logged a clean perimeter for ten days running - I can't remember the last time that happened. The camp toasts you at supper. Some of the newer hands think the swordhand's a folk-tale."));
			}
		});

		// Crag-Bell for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Crag-Bell"), "f_tableland_28_1", 1750, 1050, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A bronze bell hung from a stone arch on the crag's edge*{/}"));
				return;
			}

			var rungKey = "Laima.Quests.f_tableland_28_1.Quest1006.BellRung";
			if (character.Variables.Perm.GetBool(rungKey, false))
			{
				await dialog.Msg(L("{#666666}*Already rung*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killBunnies", out var bObj)) return;
			if (!quest.TryGetProgress("killArchers", out var aObj)) return;
			if (!quest.TryGetProgress("killMages", out var mObj)) return;

			if (!(bObj.Done && aObj.Done && mObj.Done))
			{
				await dialog.Msg(L("{#666666}*The rope is in your hand, but the sweep isn't done*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Ringing bell..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(rungKey, true);
				character.ServerMessage(L("{#FFD700}Crag-Bell rung. Return to Militia-Captain Tautvydas.{/}"));
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

public class FTableland281Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1001);
		SetName(L("Bunny Overrun"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Repusbunnies stripping the plateau grass."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Plateau-Warden] Stogas"), "f_tableland_28_1");

		AddObjective("killBunnies", L("Kill Green Repusbunnies"),
			new KillObjective(40, new[] { MonsterId.Repusbunny_Green }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FTableland281Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1002);
		SetName(L("Reed-Fletchings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Bow-Repusbunnies and bring reed-fletchings for the wind-hunters."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Vaidile"), "f_tableland_28_1");

		AddObjective("killArchers", L("Kill Green Bow-Repusbunnies"),
			new KillObjective(20, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddObjective("gatherFletchings", L("Gather reed-fletchings"),
			new CollectItemObjective(650266, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650266, character.Inventory.CountItem(650266), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650266, character.Inventory.CountItem(650266), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland281Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1003);
		SetName(L("Ember-Dust Pinches"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Saltisdaughter Mages and bring ember-dust for the plateau lamps."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Rutenis"), "f_tableland_28_1");

		AddObjective("killMages", L("Kill Red Saltisdaughter Mages"),
			new KillObjective(16, new[] { MonsterId.Saltisdaughter_Mage_Red }));

		AddObjective("gatherDust", L("Gather ember-dust pinches"),
			new CollectItemObjective(650267, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650267, character.Inventory.CountItem(650267), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650267, character.Inventory.CountItem(650267), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland281Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1004);
		SetName(L("Rootcrystal Field"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Rootcrystals splitting the tableland bedrock."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Cutter] Silute"), "f_tableland_28_1");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(14, new[] { MonsterId.Rootcrystal_03 }));

		AddObjective("gaugeCracks", L("Gauge the four shelf-cracks"),
			new VariableCheckObjective("Laima.Quests.f_tableland_28_1.Quest1004.CracksGauged", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_1.Quest1004.CracksGauged");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_1.Quest1004.Crack{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_1.Quest1004.CracksGauged");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_1.Quest1004.Crack{i}");
	}
}

public class FTableland281Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1005);
		SetName(L("The Warren-Matriarch"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bow-Repusbunnies to flush the Warren-Matriarch from the deep burrow."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Gediminas"), "f_tableland_28_1");

		AddObjective("foulStones", L("Foul the three deep-burrow scouting-stones"),
			new VariableCheckObjective("Laima.Quests.f_tableland_28_1.Quest1005.StonesFouled", 3, true));

		AddObjective("killScouts", L("Kill Green Bow-Repusbunnies"),
			new KillObjective(10, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddObjective("killMatriarch", L("Defeat the Warren-Matriarch"),
			new KillObjective(1, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_1.Quest1005.StonesFouled");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_1.Quest1005.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_1.Quest1005.StonesFouled");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_1.Quest1005.Stone{i}");
	}
}

public class FTableland281Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1006);
		SetName(L("Tableland Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Repusbunnies, Bow-Repusbunnies, and Saltisdaughter Mages."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Tautvydas"), "f_tableland_28_1");

		AddObjective("killBunnies", L("Kill Green Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny_Green }));

		AddObjective("killArchers", L("Kill Green Bow-Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddObjective("killMages", L("Kill Red Saltisdaughter Mages"),
			new KillObjective(12, new[] { MonsterId.Saltisdaughter_Mage_Red }));

		AddObjective("ringBell", L("Ring the Crag-Bell"),
			new VariableCheckObjective("Laima.Quests.f_tableland_28_1.Quest1006.BellRung", 1, true));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(18800));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_1.Quest1006.BellRung");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_1.Quest1006.BellRung");
	}
}
