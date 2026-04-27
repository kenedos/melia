//--- Melia Script ----------------------------------------------------------
// Northern Parias Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Northern Parias Forest (f_maple_24_3).
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

public class FMaple243QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Fragolin Thinning
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Forest-Ward] Rasa"), "f_maple_24_3", -1500, -1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1001);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Fragolins swarm the north trail in Parias. Forty-five killed clears the pilgrim road."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Pilgrims?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty-five. Watch the berry-patches."));
						break;

					case "info":
						await dialog.Msg(L("Road runs to the Parias shrine. Swarm keeps pilgrims out."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays closed."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFragolins", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Pilgrims already passing."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Road's open. Shrine receives again."));
			}
		});

		// Quest 2: Cloverin Pollen
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Herbalist] Vaiva"), "f_maple_24_3", -600, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1002);

			dialog.SetTitle(L("Vaiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Cloverin pollen binds the forest wards. Kill thirty, bring eight pollen sacs."));

				var response = await dialog.Select(L("Pollen?"),
					Option(L("I'll bring"), "help"),
					Option(L("Wards?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Sacs whole. They spoil if torn."));
						break;

					case "info":
						await dialog.Msg(L("Wards keep rootcrystal corruption off the glade. Pollen feeds them."));
						break;

					case "leave":
						await dialog.Msg(L("Wards thin."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCloverins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPollen", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Eight sacs. Wards refed."));
					character.Inventory.Remove(650248, character.Inventory.CountItem(650248), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Wards hum steady again."));
			}
		});

		// Quest 3: Blueberrin Essence
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Alchemist] Eimis"), "f_maple_24_3", 400, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1003);

			dialog.SetTitle(L("Eimis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blueberrin essence cures the pilgrim fever. Kill fifteen, bring five essence-vials."));

				var response = await dialog.Select(L("Essence?"),
					Option(L("I'll bring"), "help"),
					Option(L("Fever?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't shake them."));
						break;

					case "info":
						await dialog.Msg(L("Fever catches pilgrims off the road. Essence breaks it in a night."));
						break;

					case "leave":
						await dialog.Msg(L("Fever spreads."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBlueberrins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherEssence", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Five vials. Fever ward ready."));
					character.Inventory.Remove(650249, character.Inventory.CountItem(650249), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Fever broken in six pilgrims already."));
			}
		});

		// Quest 4: Rootcrystal Killing
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Crystal-Warder] Audra"), "f_maple_24_3", -900, -400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1004);

			dialog.SetTitle(L("Audra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A crystal-warder strikes a small chime-stone in her palm, listens to its note, then frowns and strikes it again*{/}"));
				await dialog.Msg(L("Parias has a true note. The grove hums it, the path-trees carry it, the pilgrims pray to it without knowing. When the note is true, this whole forest sings on key."));
				await dialog.Msg(L("Rootcrystals pull the bedrock and the bedrock pulls the note flat. Pilgrims hear the off-tone before they see anything wrong - they turn back at the path-stone, no idea why their feet wouldn't carry them further."));

				var response = await dialog.Select(L("Break 45 Parias Rootcrystals to quiet the pull, then re-tune the four chime-stones I've hung from the path-trees. The chimes re-anchor the true note. Will you walk the path?"),
					Option(L("I'll break the crystals and tune the chimes"), "help"),
					Option(L("How does a forest hum a note?"), "info"),
					Option(L("That sounds like cantor-work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She presses a small bone-mallet into your hand, the head smoothed by years of striking*{/}"));
						await dialog.Msg(L("Forty-five crystals first. Strike low, step back - they shatter sharp and the shards carry the off-note. Don't pocket any."));
						await dialog.Msg(L("Then the four chime-stones. They hang from the path-trees at shoulder-height. Tap each one once with the bone-mallet, listen for the true note. If it rings flat, tap a second time - never a third."));
						break;

					case "info":
						await dialog.Msg(L("Every place has a note. A meadow, a mountain, a city. Most places the note is too quiet to hear, drowned by the noise of living. Parias's note is louder - the grove was consecrated for it, centuries back."));
						await dialog.Msg(L("Crystals interrupt the note the way a closed fist interrupts a song. Chime-stones are how we re-teach the grove its own pitch after the interruption ends."));
						break;

					case "leave":
						await dialog.Msg(L("It is cantor-work, in part. The cantors are at Salvia's seminary, three days' walk. The grove will be flat by then. I'll find someone closer, if I can."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("tuneChimes", out var tObj)) return;

				if (killObj.Done && tObj.Done)
				{
					await dialog.Msg(L("{#666666}*She closes her eyes a long moment, head tilted, then breaks into a slow, real smile*{/}"));
					await dialog.Msg(L("Glade reads clean. Chimes ringing true from here all the way to the inner path. Parias sings on key for the first time in two months."));
					await dialog.Msg(L("Crystal-warder's purse, plus a wax-sealed phial of grove-water from the heart-spring. Drink it slow - the note carries in it."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("The crystals are still pulling. Tap a chime-stone now and you'd just teach the grove the wrong note. Break the crystals first."));
				}
				else
				{
					await dialog.Msg(L("Crystals are silent. Now the four chime-stones. Bone-mallet, single tap, listen for the true note. Patience - the grove is listening too."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Parias hums clean from the path-stone all the way through. A pilgrim walked through last week and stopped halfway down the inner path - said she heard her grandmother's lullaby in the leaves. That's the note doing its work."));
			}
		});

		// Chime-stone tuning points for Quest 1004
		//-------------------------------------------------------------------------
		void AddChimeStone(int chimeNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Chime-Stone"), "f_maple_24_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_maple_24_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pale chime-stone hung from a path-tree branch, swaying in the glade air*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_maple_24_3.Quest1004.Chime{chimeNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already tuned; the stone rings clean*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Tuning chime..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_maple_24_3.Quest1004.ChimesTuned", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_maple_24_3.Quest1004.ChimesTuned", count);
					character.ServerMessage(LF("Chime-stones tuned: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All chime-stones tuned! Return to Crystal-Warder Audra.{/}"));
				}
				else
				{
					character.ServerMessage(L("Tuning interrupted."));
				}
			});
		}

		AddChimeStone(1, -800, -300, 0);
		AddChimeStone(2, -1100, -500, 90);
		AddChimeStone(3, -700, -100, 180);
		AddChimeStone(4, -1000, -600, 270);

		// Quest 5: The Fragolin Mother
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Tadas"), "f_maple_24_3", -1700, -1100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_maple_24_3.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Tadas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty hunter cleans dried Fragolin musk from his vambrace with a wet rag, the rag turning brown*{/}"));
				await dialog.Msg(L("Fragolin Mother nests under the north ridge. Old, scarred, smarter than three of her daughters put together. Most hunters who go after her come back with one less arm than they left with."));
				await dialog.Msg(L("Three brood-burrows along the ridge feed her hatch cycle. Smoke the burrows and her instinct flips - instead of retreating into the deep nest, she fights surface to defend the cycle. That's when we have her."));

				var response = await dialog.Select(L("Smoke the three brood-burrows along the north ridge, then kill 10 Fragolins to draw her out. She fights when her hatch is threatened. Will you take her?"),
					Option(L("I'll take the Mother"), "help"),
					Option(L("Why smoke and not destroy the burrows?"), "info"),
					Option(L("Find a Fragolin specialist"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a leather pouch of dry tinder and a striker*{/}"));
						await dialog.Msg(L("Three burrows first - I left smudge-torches at each entrance. Light the torch, jam it deep into the burrow-mouth, walk away. The smoke does the work."));
						await dialog.Msg(L("Then ten brood. She emerges around the eighth - she's heavy, slow off the mark, but her swing has the reach of a polearm. Stand inside the swing-arc, not outside it."));
						break;

					case "info":
						await dialog.Msg(L("Destroyed burrows leave a vacuum - the next Fragolin Mother claims the territory inside a season. Smoked burrows stay haunted to Fragolins; they avoid the ridge for years."));
						await dialog.Msg(L("Old hunter trick. Cheaper than digging, lasts longer than blocking. Smoke also flushes the Mother because her instinct reads the smoke as a wildfire threatening her hatch - she comes out fighting, not bolting."));
						break;

					case "leave":
						await dialog.Msg(L("Swarm grows, ridge becomes uninhabitable, the path to the Parias inner grove closes by autumn. I'll be cleaning my vambrace when you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("smokeBurrows", out var sObj)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var aObj)) return;

				if (sObj.Done && pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("{#666666}*He counts coin into a leather pouch, slow, like he's making sure each piece is real*{/}"));
					await dialog.Msg(L("Burrows smoked clean, Mother's down, swarm broken. The ridge will stay haunted to Fragolins for years - that's the smoke doing its work."));
					await dialog.Msg(L("Bounty plus a hide-stipend - the Mother's hide makes good vambrace-leather, and you earned your share. The next inner-grove pilgrim caravan will roll through unbothered. Drink to that."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Fragolin, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She comes!"));
							character.ServerMessage(L("{#FF9966}The Fragolin Mother emerges from the ridge!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find her."));
					}
				}
				else if (!sObj.Done)
				{
					await dialog.Msg(L("Three smudge-torches, three burrow-mouths. Light, jam deep, walk away. The smoke does what blade can't."));
				}
				else
				{
					await dialog.Msg(L("Burrows are smoking. Now ten brood - she emerges around the eighth, swing-arc the reach of a polearm."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ridge stays quiet, pilgrims sleep easy. Three new caravans through this season - none with a bounty contract attached. That's the highest form of thanks: nobody had to ask for one."));
			}
		});

		// Brood-burrow smoke points for Quest 1005
		//-------------------------------------------------------------------------
		void AddBroodBurrow(int burrowNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Fragolin Brood-Burrow"), "f_maple_24_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_maple_24_3", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A musk-thick burrow under the ridge, smoke-torch propped at the entrance*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_maple_24_3.Quest1005.Burrow{burrowNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already smoked; the air still hangs sharp*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Smoking burrow..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_maple_24_3.Quest1005.BurrowsSmoked", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_maple_24_3.Quest1005.BurrowsSmoked", count);
					character.ServerMessage(LF("Brood-burrows smoked: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All burrows smoked! Now bait out the Mother.{/}"));
				}
				else
				{
					character.ServerMessage(L("Smoking interrupted."));
				}
			});
		}

		AddBroodBurrow(1, -1500, -900, 0);
		AddBroodBurrow(2, -1900, -1300, 90);
		AddBroodBurrow(3, -1600, -1200, 180);

		// Quest 6: Parias Forest Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Ranger-Captain] Mindaugas"), "f_maple_24_3", 200, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1006);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A ranger-captain unrolls a trail-map and weighs the corners with smooth river-pebbles*{/}"));
				await dialog.Msg(L("Parias forest stretches three days' walk in any direction. We can't patrol all of it. So we sweep the trail-corridors - the routes pilgrims actually walk - and log the work on the Ranger Cairn at the trail-head."));
				await dialog.Msg(L("Three species contest the trail-corridors: Fragolins, Cloverins, Blueberrins. Each ranger does the round in their cycle, then logs the kill-count. The next ranger reads the cairn before they leave the lodge."));

				var response = await dialog.Select(L("Kill 12 Fragolins, 12 Cloverins, and 12 Blueberrins, then chalk the tally on the Ranger Cairn at the trail-head. Standard ranger-rate. Take it?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why not just patrol everywhere?"), "info"),
					Option(L("Find a forest-hand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She passes you a stub of charcoal and a folded forest-pass*{/}"));
						await dialog.Msg(L("Thirty-six kills, no padding. The next ranger counts the bone-piles on Sunday's perimeter walk."));
						await dialog.Msg(L("Cairn's at the trail-head, east face. Mark a chevron - one stroke down, one stroke up. Don't write your name; the cairn doesn't know names, only sweeps."));
						break;

					case "info":
						await dialog.Msg(L("Forty rangers can't patrol three days' walk in any direction. Twelve rangers can patrol the trail-corridors and respond to off-trail sightings. Mathematics. We picked twelve because that's what the ranger-budget allows."));
						await dialog.Msg(L("So we sweep the corridors hard and the off-trail areas occasional. Pilgrims walk the corridors. Pilgrims are who we protect."));
						break;

					case "leave":
						await dialog.Msg(L("Then the next pilgrim caravan walks an unswept corridor, and the corridor decides who comes back. I'd rather not roll those dice."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFragolins", out var fObj)) return;
				if (!quest.TryGetProgress("killCloverins", out var cObj)) return;
				if (!quest.TryGetProgress("killBlueberrins", out var bObj)) return;
				if (!quest.TryGetProgress("logCairn", out var lObj)) return;

				if (fObj.Done && cObj.Done && bObj.Done && lObj.Done)
				{
					await dialog.Msg(L("{#666666}*She countersigns your forest-pass with a tar-stub and counts coin into a cloth*{/}"));
					await dialog.Msg(L("Cairn chevron is clean. Sweep done, math holds, the next ranger walks the corridor at first light."));
					await dialog.Msg(L("Ranger-rate plus a half-purse for the off-trail sightings you reported. The forest-corridor system pays its way - you're a small reason why."));
					character.Quests.Complete(questId);
				}
				else if (fObj.Done && cObj.Done && bObj.Done)
				{
					await dialog.Msg(L("Sweep done. Now the cairn - east face, chevron, no flourishes. Chalk hard - rain takes a soft mark."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each. The triad cycle through the corridor in sequence - Fragolins lead, Cloverins follow, Blueberrins clean up the kills. Sweep them in order if you can."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Rangers walk the trail-corridor every dawn now, paid by your sweep's chevron-count. Three pilgrim caravans through this fortnight, no losses, no off-trail sightings. The cairn keeps its accounting; we keep the corridor."));
			}
		});

		// Ranger Cairn for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Ranger Cairn"), "f_maple_24_3", 250, 1050, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A trailhead cairn, chalked with old ranger-tallies*{/}"));
				return;
			}

			var loggedKey = "Laima.Quests.f_maple_24_3.Quest1006.CairnLogged";
			if (character.Variables.Perm.GetBool(loggedKey, false))
			{
				await dialog.Msg(L("{#666666}*Your tally chalked already*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killFragolins", out var fObj)) return;
			if (!quest.TryGetProgress("killCloverins", out var cObj)) return;
			if (!quest.TryGetProgress("killBlueberrins", out var bObj)) return;

			if (!(fObj.Done && cObj.Done && bObj.Done))
			{
				await dialog.Msg(L("{#666666}*The cairn is ready, but you haven't finished the sweep*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Logging cairn..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(loggedKey, true);
				character.ServerMessage(L("{#FFD700}Cairn logged. Return to Ranger-Captain Mindaugas.{/}"));
			}
			else
			{
				character.ServerMessage(L("Logging interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FMaple243Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1001);
		SetName(L("Fragolin Thinning"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Fragolins swarming the Parias pilgrim road."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forest-Ward] Rasa"), "f_maple_24_3");

		AddObjective("killFragolins", L("Kill Fragolins"),
			new KillObjective(45, new[] { MonsterId.Fragolin }));

		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(1200));
		AddReward(new ItemReward(640081, 1));
		AddReward(new ItemReward(640002, 2));
		AddReward(new ItemReward(640005, 2));
	}
}

public class FMaple243Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1002);
		SetName(L("Cloverin Pollen"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Cloverins and bring pollen sacs for the forest wards."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Vaiva"), "f_maple_24_3");

		AddObjective("killCloverins", L("Kill Cloverins"),
			new KillObjective(30, new[] { MonsterId.Cloverin }));

		AddObjective("gatherPollen", L("Gather pollen sacs"),
			new CollectItemObjective(650248, 8));

		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(1600));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640002, 2));
		AddReward(new ItemReward(640005, 2));
		AddReward(new ItemReward(640008, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650248, character.Inventory.CountItem(650248), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650248, character.Inventory.CountItem(650248), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple243Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1003);
		SetName(L("Blueberrin Essence"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blueberrins and bring essence-vials for the pilgrim fever cure."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Eimis"), "f_maple_24_3");

		AddObjective("killBlueberrins", L("Kill Blueberrins"),
			new KillObjective(15, new[] { MonsterId.Blueberrin }));

		AddObjective("gatherEssence", L("Gather essence-vials"),
			new CollectItemObjective(650249, 5));

		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(1600));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640002, 2));
		AddReward(new ItemReward(640005, 2));
		AddReward(new ItemReward(640008, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650249, character.Inventory.CountItem(650249), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650249, character.Inventory.CountItem(650249), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple243Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1004);
		SetName(L("Rootcrystal Killing"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Parias Rootcrystals pulling the forest off-tone."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Warder] Audra"), "f_maple_24_3");

		AddObjective("breakCrystals", L("Break Parias Rootcrystals"),
			new KillObjective(45, new[] { MonsterId.Rootcrystal_01 }));

		AddObjective("tuneChimes", L("Tune the four chime-stones"),
			new VariableCheckObjective("Laima.Quests.f_maple_24_3.Quest1004.ChimesTuned", 4, true));

		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(1200));
		AddReward(new ItemReward(640081, 1));
		AddReward(new ItemReward(640002, 2));
		AddReward(new ItemReward(640005, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_3.Quest1004.ChimesTuned");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_3.Quest1004.Chime{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_3.Quest1004.ChimesTuned");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_3.Quest1004.Chime{i}");
	}
}

public class FMaple243Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1005);
		SetName(L("The Fragolin Mother"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Fragolins to draw out the Fragolin Mother nesting under the ridge."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Tadas"), "f_maple_24_3");

		AddObjective("smokeBurrows", L("Smoke the three brood-burrows"),
			new VariableCheckObjective("Laima.Quests.f_maple_24_3.Quest1005.BurrowsSmoked", 3, true));

		AddObjective("killPack", L("Kill Fragolins"),
			new KillObjective(10, new[] { MonsterId.Fragolin }));

		AddObjective("killAlpha", L("Defeat the Fragolin Mother"),
			new KillObjective(1, new[] { MonsterId.Fragolin }));

		AddReward(new ExpReward(1600, 1100));
		AddReward(new SilverReward(2000));
		AddReward(new ItemReward(640081, 3));
		AddReward(new ItemReward(640002, 3));
		AddReward(new ItemReward(640005, 3));
		AddReward(new ItemReward(640008, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_3.Quest1005.BurrowsSmoked");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_3.Quest1005.Burrow{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_3.Quest1005.BurrowsSmoked");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_3.Quest1005.Burrow{i}");
	}
}

public class FMaple243Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1006);
		SetName(L("Parias Forest Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Fragolins, Cloverins, and Blueberrins."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger-Captain] Mindaugas"), "f_maple_24_3");

		AddObjective("killFragolins", L("Kill Fragolins"),
			new KillObjective(12, new[] { MonsterId.Fragolin }));

		AddObjective("killCloverins", L("Kill Cloverins"),
			new KillObjective(12, new[] { MonsterId.Cloverin }));

		AddObjective("killBlueberrins", L("Kill Blueberrins"),
			new KillObjective(12, new[] { MonsterId.Blueberrin }));

		AddObjective("logCairn", L("Log the sweep on the Ranger Cairn"),
			new VariableCheckObjective("Laima.Quests.f_maple_24_3.Quest1006.CairnLogged", 1, true));

		AddReward(new ExpReward(1600, 1100));
		AddReward(new SilverReward(2000));
		AddReward(new ItemReward(640081, 3));
		AddReward(new ItemReward(640002, 3));
		AddReward(new ItemReward(640005, 3));
		AddReward(new ItemReward(640008, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_3.Quest1006.CairnLogged");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_3.Quest1006.CairnLogged");
	}
}
