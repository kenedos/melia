//--- Melia Script ----------------------------------------------------------
// Gateway of the Great King Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the Gateway of the Great King map.
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

public class FRokas24QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Hogma Warband
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Gate Marshal] Einar"), "f_rokas_24", 850, -1800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1001);

			dialog.SetTitle(L("Einar"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Hogma warbands have taken the whole gate stretch. Warriors up front, Combat-class behind them, all drilled and mean."));
				await dialog.Msg(L("They've been chipping the Great King's statue for trophy stones. I won't abide defacement. Not here."));
				await dialog.Msg(L("Kill thirty Hogma - mixed Warriors and Combat - and their chant breaks. That's when the statue gets peace again."));

				var response = await dialog.Select(L("Thirty Hogma?"),
					Option(L("I'll break the chant"), "help"),
					Option(L("Chant?"), "info"),
					Option(L("Let them have it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Warriors up top, Combat flanking. Mind the two-on-one."));
						await dialog.Msg(L("Every trophy they drop is going back on the statue."));
						break;

					case "info":
						await dialog.Msg(L("They chant while they chip. A guttural verse, over and over. It's how they keep rhythm."));
						await dialog.Msg(L("Thin the numbers enough and the chant collapses. Then the rest scatter."));
						break;

					case "leave":
						await dialog.Msg(L("Not while I'm the marshal here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killWarriors", out var warriorObj)) return;
				if (!quest.TryGetProgress("killCombat", out var combatObj)) return;

				if (warriorObj.Done && combatObj.Done)
				{
					await dialog.Msg(L("Chant's broken. The gate stretch is quiet for the first time in weeks."));
					await dialog.Msg(L("Statue goes back together this month. Take your pay."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. Break the chant for good."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Statue's patched. Pilgrims are coming through again."));
			}
		});

		// Quest 2: Cockatrie Killings
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Falconer] Yrsa"), "f_rokas_24", 880, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1002);

			dialog.SetTitle(L("Yrsa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Cockatries nest thick along the east cliffs. Small ones in swarms, big ones bullying pilgrims off the road."));
				await dialog.Msg(L("Bring me sixteen feathers from Big Cockatries - red and normal both. The plumes are worth a fortune to Fedimian's fletchers."));

				var response = await dialog.Select(L("Sixteen plumes?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Why the big ones?"), "info"),
					Option(L("Skip the cliffs"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Red or normal, doesn't matter - I sort them later. Just sixteen clean plumes."));
						await dialog.Msg(L("Watch the dive. A big one drops straight down."));
						break;

					case "info":
						await dialog.Msg(L("The small ones shed in handfuls. Worthless fluff. The big ones have structure - shafts a fletcher can trust."));
						await dialog.Msg(L("A dozen Fedimian bowmen are waiting on this shipment."));
						break;

					case "leave":
						await dialog.Msg(L("The cliffs don't skip you. Remember that."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var feathers = character.Inventory.CountItem(650100);

				if (feathers >= 16)
				{
					await dialog.Msg(L("Sixteen clean plumes, look at these shafts. The fletchers are going to weep."));
					await dialog.Msg(L("Take your pay. I'll be back here on the next shipment."));

					character.Inventory.Remove(650100, 16, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep plucking. {0} of sixteen.", feathers));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Those arrows reached Fedimian last week. Full order paid out."));
			}
		});

		// Quest 3: Tontus Tally
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Pilgrim] Halli"), "f_rokas_24", -700, -200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1003);

			dialog.SetTitle(L("Halli"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A pilgrim sits cross-legged on a flat stone beside the road, a prayer-string slipping through her fingers bead by bead*{/}"));
				await dialog.Msg(L("Tontus and Dandels are thick on this road. I'm not a fighter - I count steps from one waystone to the next, and I pray for those of us walking behind."));
				await dialog.Msg(L("But there are four of us walking behind who haven't made it past the third waystone. Three days they've been kneeling on the verge, weeping, unable to go further. They've seen too much. They need a witness more than a guard."));

				var response = await dialog.Select(L("Kill 20 Tontus and 15 Dandels to clear the road, and sit with each of the four broken pilgrims long enough to be a witness. Will you do both?"),
					Option(L("I'll clear the road and sit with the four"), "help"),
					Option(L("How many pilgrims behind you?"), "info"),
					Option(L("Turn back, this isn't my pilgrimage"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She presses a small wooden prayer-bead into your palm and closes your fingers around it*{/}"));
						await dialog.Msg(L("Blessings on your blade and on your patience. The Tontus first, where you can find them - then sit with the four. They are kneeling along the verge, you cannot miss them."));
						await dialog.Msg(L("Don't tell them anything. Just sit. They'll know what to do with the silence."));
						break;

					case "info":
						await dialog.Msg(L("Forty, give or take. Three elders among them. One pregnant woman walking for her unborn. Two children walking for siblings the demon-war took."));
						await dialog.Msg(L("They each have their reason. None of them planned for the Tontus to be this thick on Pelke's road. We will not leave any of them behind."));
						break;

					case "leave":
						await dialog.Msg(L("No turning back. The goddess calls, and I count my steps. The four kneeling pilgrims will rise when they rise. I will wait with them."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTontus", out var tontusObj)) return;
				if (!quest.TryGetProgress("killDandels", out var dandelObj)) return;
				if (!quest.TryGetProgress("comfortPilgrims", out var pObj)) return;

				if (tontusObj.Done && dandelObj.Done && pObj.Done)
				{
					await dialog.Msg(L("{#666666}*She watches the four pilgrims walking past her stone, single file, each one nodding to her in turn*{/}"));
					await dialog.Msg(L("Blessed goddess. The road is open and the four are walking again - I see them now, threading through the waystones, eyes still red but steps steady."));
					await dialog.Msg(L("Take this offering. Pilgrim's purse, earned not tithed. The four will write your name in the shrine-book at the journey's end. They asked me to ask if you have one."));

					character.Quests.Complete(questId);
				}
				else if (tontusObj.Done && dandelObj.Done)
				{
					await dialog.Msg(L("Road's clear of monsters. Now the four on the verge - sit with each, just sit. They have been kneeling three days; another quarter-hour will heal them more than a swordswing."));
				}
				else
				{
					await dialog.Msg(L("Pray with me. Keep swinging. The Tontus do not pause for prayer; my prayer is for you, not them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("All forty reached the shrine. Three miracles recorded in the shrine-book - and a fourth, which the cantor wrote as 'a swordhand who sat with the broken'. The pregnant woman gave birth at the shrine three days later. She wants you named godparent if she ever finds you again."));
			}
		});

		// Distressed pilgrim comfort points for Quest 1003
		//-------------------------------------------------------------------------
		void AddDistressedPilgrim(int pilgrimNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Weeping Pilgrim"), "f_rokas_24", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_rokas_24", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pilgrim kneels on the verge, head in hands*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_rokas_24.Quest1003.Pilgrim{pilgrimNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already comforted; the pilgrim is gathering their pack*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Comforting pilgrim..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_24.Quest1003.PilgrimsComforted", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_rokas_24.Quest1003.PilgrimsComforted", count);
					character.ServerMessage(LF("Pilgrims comforted: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All pilgrims comforted! Return to Halli.{/}"));
				}
				else
				{
					character.ServerMessage(L("Comfort interrupted."));
				}
			});
		}

		AddDistressedPilgrim(1, -600, -100, 0);
		AddDistressedPilgrim(2, -800, -300, 90);
		AddDistressedPilgrim(3, -500, -250, 180);
		AddDistressedPilgrim(4, -700, -50, 270);

		// Quest 4: Pino-Geppetto Grove
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Toymaker] Leif"), "f_rokas_24", -200, -1950, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1004);

			dialog.SetTitle(L("Leif"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Pinos and Geppettos used to be harmless. A novelty for the shop. Now they gang up and trample my lumber."));
				await dialog.Msg(L("Kill twelve Pinos and fifteen Geppettos. Bring me six pinewood knots from the grove - those are the ones the creatures chew on."));

				var response = await dialog.Select(L("Mixed work?"),
					Option(L("I'll clear the grove"), "help"),
					Option(L("Why do they chew knots?"), "info"),
					Option(L("Buy lumber elsewhere"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The knots are gummy when they drop - don't worry, they clean up."));
						await dialog.Msg(L("Six is enough to resume production."));
						break;

					case "info":
						await dialog.Msg(L("Sap-rich. The knots are where the tree hoarded its strongest sap. Toy mouths, apparently, go for it like candy."));
						break;

					case "leave":
						await dialog.Msg(L("Lumber from the north is twice the price and half the grain. Please help."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPinos", out var pinoObj)) return;
				if (!quest.TryGetProgress("killGeppettos", out var gepObj)) return;
				if (!quest.TryGetProgress("gatherKnots", out var knotObj)) return;

				if (pinoObj.Done && gepObj.Done && knotObj.Done)
				{
					await dialog.Msg(L("Six knots, all clean. The grove's breathing again."));
					await dialog.Msg(L("Your pay, plus a little doll for luck. Don't laugh - it really works."));

					character.Inventory.Remove(650011, character.Inventory.CountItem(650011), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!pinoObj.Done) status += L("More Pinos. ");
					if (!gepObj.Done) status += L("More Geppettos. ");
					if (!knotObj.Done) status += L("More pinewood knots. ");
					await dialog.Msg(LF("{0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First toys of the new batch are on the shelves. The grove's hum is a lot softer now."));
			}
		});

		// Quest 5: The Cockatrie Matron
		//-------------------------------------------------------------------------
		AddNpc(147418, L("[Huntress] Siv"), "f_rokas_24", 1100, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1005);
			var matronSpawnedKey = "Laima.Quests.f_rokas_24.Quest1005.MatronSpawned";

			dialog.SetTitle(L("Siv"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A huntress sits on a leather travel-pack, oiling a length of chain with practiced thumbs*{/}"));
				await dialog.Msg(L("There's a matron above the cliffs. Biggest Cockatrie you'll ever meet - wingspan would shadow this whole campsite. She spawns the red line, the dangerous ones, the ones the Falconer keeps asking for plumes from."));
				await dialog.Msg(L("Three of her egg-clutches sit along the cliff path. Scatter them and her hatch cycle breaks - she has to defend the breeders directly, no more relying on the next generation. That's when we have her."));

				var response = await dialog.Select(L("Scatter the three Cockatrie egg-clutches along the cliff path, then kill 10 Big Red Cockatries to draw her down. She fights when her hatch is gone. Will you hunt her?"),
					Option(L("I'll hunt the matron"), "help"),
					Option(L("Why the red line specifically?"), "info"),
					Option(L("Not this one"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She tosses you a leather glove, the gauntlet stiff with old Cockatrie-musk*{/}"));
						await dialog.Msg(L("Three clutches first - kick the eggs over the edge, don't smash them in place. Smashed eggs draw the matron the wrong direction. Cliff-edge eggs send her thinking the brood-pull's gone south."));
						await dialog.Msg(L("Then ten breeders. She comes down around the eighth. When she dives, drop flat - dive-claws clear the rim if you're prone. Then strike up into her chest as she banks."));
						break;

					case "info":
						await dialog.Msg(L("The red ones are her direct hatch - the firstborn of each cycle, raised on her milk and her instruction. Bigger, meaner, louder. She throws them out onto the cliffs to test them."));
						await dialog.Msg(L("The red survivors become her next generation of breeders. Killing the red line collapses her bloodline - the village won't see another red Cockatrie for ten years, maybe twenty."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. She is not subtle prey - and a hunter who chooses their fights badly does not get a second cliff-fight. I'll hold the bounty for someone with the right wrist for it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("scatterEggs", out var eObj)) return;
				if (!quest.TryGetProgress("killBreeders", out var breederObj)) return;
				if (!quest.TryGetProgress("killMatron", out var matronObj)) return;

				if (eObj.Done && breederObj.Done && matronObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays a single enormous feather across her lap, runs a fingertip along its shaft, then folds it into oilcloth*{/}"));
					await dialog.Msg(L("Eggs scattered, matron's wingspan across the cliff-path. Biggest feather I've ever recovered - the Falconer's going to weep when he sees it."));
					await dialog.Msg(L("Season's safe, bloodline collapsed, red line broken for a generation. Take your bounty - and this glove. The musk's set in; it'll mean luck against any Cockatrie you meet for a year."));

					character.Variables.Perm.Remove(matronSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (breederObj.Done && !matronObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(matronSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(matronSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Big_Cockatries_Red, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She's screaming. You hear it? She's coming down."));
							character.ServerMessage(L("{#FF9966}The Matron descends from the cliffs!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("She's out there. Finish her before she retreats."));
					}
				}
				else if (!eObj.Done)
				{
					await dialog.Msg(L("Three clutches first - along the cliff path. Kick them over the edge, don't smash them in place. The matron must read the wrong direction or she won't dive."));
				}
				else
				{
					await dialog.Msg(L("Eggs are scattered, the brood-pull's broken. Now ten breeders. She comes at the eighth, dives at the ninth, screams at the tenth. Wait her out."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The cliffs sing a different song this season - higher, thinner, no red note in it. Red line's broken for a generation. The Falconer mounted the matron's feather above his shop-door and won't let anyone touch it."));
			}
		});

		// Cockatrie egg-clutch scatter points for Quest 1005
		//-------------------------------------------------------------------------
		void AddEggClutch(int clutchNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Cockatrie Egg-Clutch"), "f_rokas_24", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_rokas_24", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A clutch of red-tinged Cockatrie eggs nestled in cliff scrub*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_rokas_24.Quest1005.Clutch{clutchNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already scattered; only shells left*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Scattering eggs..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_24.Quest1005.ClutchesScattered", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_rokas_24.Quest1005.ClutchesScattered", count);
					character.ServerMessage(LF("Egg-clutches scattered: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All clutches scattered! Now bait out the Matron.{/}"));
				}
				else
				{
					character.ServerMessage(L("Scattering interrupted."));
				}
			});
		}

		AddEggClutch(1, 1100, 800, 0);
		AddEggClutch(2, 1300, 600, 90);
		AddEggClutch(3, 900, 900, 180);

		// Quest 6: Great King's Passage
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Road Warden] Gunnar"), "f_rokas_24", -500, -3400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1006);

			dialog.SetTitle(L("Gunnar"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A road warden warms gloved hands at a small charcoal brazier on the gate-step, breath fogging in the south-wind*{/}"));
				await dialog.Msg(L("South passage is the worst stretch on the Great King's road. Hogma Warriors push from the east; Cockatries dive from the west. Neither side likes the other and neither side cares which pilgrim they catch in the crossfire."));
				await dialog.Msg(L("We sweep it weekly or it's not a passage anymore. And the pilgrim caravans won't roll without a green banner on the gate-pole - that's the signal that the south passage is held for the next seven days."));

				var response = await dialog.Select(L("Kill 12 Hogma Warriors and 12 Cockatries to clear the south passage, then haul up the green banner on the gate-pole. Pilgrim caravans roll on the banner. Will you clear both?"),
					Option(L("I'll clear both sides and raise the banner"), "help"),
					Option(L("Which side is worse?"), "info"),
					Option(L("Not my fight"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He stands and unhooks a folded green pennant from a hook beside the brazier, pressing it into your pack*{/}"));
						await dialog.Msg(L("Twelve of each, no shortcuts. The Hogma spit firebrand-globs at distance; the Cockatries dive from above. Stand against a stone wall when you can - it covers both lines of attack."));
						await dialog.Msg(L("Banner's the green pennant. Haul-line is on the south side of the gate-pole. Tie it off with a double bowline knot - the south wind's brutal and a single knot won't hold past the second day."));
						break;

					case "info":
						await dialog.Msg(L("Hogma are smarter, Cockatries are faster. Hogma fight in formation; Cockatries fight in dives. Pilgrims prefer neither. I prefer neither. The wind off the south crook prefers neither either - it strips the warmth out of you within an hour."));
						await dialog.Msg(L("If I had to choose, I'd say the Cockatries. Hogma die when killed; Cockatries scream a death-call that brings two more from the cliffs. Plan the order of your sweep accordingly."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims wait for nobody. Including you. I'll be here at the brazier when you change your mind, or when the next swordhand passes through. Whichever comes first."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHogma", out var hogmaObj)) return;
				if (!quest.TryGetProgress("killCockatries", out var cockObj)) return;
				if (!quest.TryGetProgress("raiseBanner", out var bObj)) return;

				if (hogmaObj.Done && cockObj.Done && bObj.Done)
				{
					await dialog.Msg(L("{#666666}*He squints south through the gate-arch and points at a pinprick of green movement on the far horizon*{/}"));
					await dialog.Msg(L("Banner up - and look there, that's the pilgrim caravan-banner answering from the south camp. They saw your green pennant within the hour. They'll be here by tomorrow noon."));
					await dialog.Msg(L("Your pay. Buy warm cider somewhere out of this wind - the south crook's no place for celebration. Drink to the pilgrims who walked because of your sweep."));

					character.Quests.Complete(questId);
				}
				else if (hogmaObj.Done && cockObj.Done)
				{
					await dialog.Msg(L("Passage is clear of monsters. Now the green pennant - haul-line, double bowline, south side of the pole. The pilgrims won't move without it."));
				}
				else
				{
					await dialog.Msg(L("Both sides need pushing. Hogma east, Cockatries west. Don't let either think the south passage belongs to them again."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Passage held for ten days - a record since I took the wardenship. The pilgrim caravan that answered your banner had a cantor in it; he made me name the swordhand for the dawn invocation. I gave him a description; he wrote a song instead of a name. They sing it at the south camp now."));
			}
		});

		// Road-Banner for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Road-Banner Pole"), "f_rokas_24", -450, -3450, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A weathered banner-pole at the south gate, green pennant rolled at its base*{/}"));
				return;
			}

			var raisedKey = "Laima.Quests.f_rokas_24.Quest1006.BannerRaised";
			if (character.Variables.Perm.GetBool(raisedKey, false))
			{
				await dialog.Msg(L("{#666666}*The green pennant flies above the gate*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killHogma", out var hogmaObj)) return;
			if (!quest.TryGetProgress("killCockatries", out var cockObj)) return;

			if (!(hogmaObj.Done && cockObj.Done))
			{
				await dialog.Msg(L("{#666666}*The pole is ready, but you haven't cleared both sides*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Raising banner..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(raisedKey, true);
				character.ServerMessage(L("{#FFD700}Road-Banner raised. Return to Road Warden Gunnar.{/}"));
			}
			else
			{
				character.ServerMessage(L("Raising interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FRokas24Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1001);
		SetName(L("Hogma Warband"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hogma Warriors and Hogma Combat at the Gateway so the Great King's statue can be repaired."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Gate Marshal] Einar"), "f_rokas_24");

		AddObjective("killWarriors", L("Kill Hogma Warriors"),
			new KillObjective(15, new[] { MonsterId.Hogma_Warrior }));

		AddObjective("killCombat", L("Kill Hogma Combat"),
			new KillObjective(15, new[] { MonsterId.Hogma_Combat }));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}

public class FRokas24Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1002);
		SetName(L("Cockatrie Plumes"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Big Cockatries and deliver sixteen plume feathers to Fedimian's fletchers."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Falconer] Yrsa"), "f_rokas_24");

		AddObjective("gatherPlumes", L("Gather Cockatrie plumes"),
			new CollectItemObjective(650100, 16));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650100, character.Inventory.CountItem(650100), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650100, character.Inventory.CountItem(650100), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRokas24Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1003);
		SetName(L("Pilgrim Road Cleanup"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tontus and Dandels along the pilgrim road so travelers can pass safely."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim] Halli"), "f_rokas_24");

		AddObjective("killTontus", L("Kill Tontus"),
			new KillObjective(20, new[] { MonsterId.Tontus }));

		AddObjective("killDandels", L("Kill Dandels"),
			new KillObjective(15, new[] { MonsterId.Dandel }));

		AddObjective("comfortPilgrims", L("Comfort the four weeping pilgrims"),
			new VariableCheckObjective("Laima.Quests.f_rokas_24.Quest1003.PilgrimsComforted", 4, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_24.Quest1003.PilgrimsComforted");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_24.Quest1003.Pilgrim{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_24.Quest1003.PilgrimsComforted");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_24.Quest1003.Pilgrim{i}");
	}
}

public class FRokas24Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1004);
		SetName(L("Pino-Geppetto Grove"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Pinos and Geppettos and gather pinewood knots for the toymaker."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Toymaker] Leif"), "f_rokas_24");

		AddObjective("killPinos", L("Kill Pinos"),
			new KillObjective(12, new[] { MonsterId.Pino }));

		AddObjective("killGeppettos", L("Kill Geppettos"),
			new KillObjective(15, new[] { MonsterId.Geppetto }));

		AddObjective("gatherKnots", L("Gather pinewood knots"),
			new CollectItemObjective(650011, 6));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650011, character.Inventory.CountItem(650011), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650011, character.Inventory.CountItem(650011), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRokas24Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1005);
		SetName(L("The Cockatrie Matron"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill ten Big Red Cockatries to draw out and slay their matron."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Huntress] Siv"), "f_rokas_24");

		AddObjective("scatterEggs", L("Scatter the three Cockatrie egg-clutches"),
			new VariableCheckObjective("Laima.Quests.f_rokas_24.Quest1005.ClutchesScattered", 3, true));

		AddObjective("killBreeders", L("Kill Big Red Cockatries"),
			new KillObjective(10, new[] { MonsterId.Big_Cockatries_Red }));

		AddObjective("killMatron", L("Defeat the Matron"),
			new KillObjective(1, new[] { MonsterId.Big_Cockatries_Red }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_24.Quest1005.ClutchesScattered");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_24.Quest1005.Clutch{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_24.Quest1005.ClutchesScattered");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_24.Quest1005.Clutch{i}");
	}
}

public class FRokas24Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1006);
		SetName(L("Great King's Passage"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hogma Warriors and Cockatries contesting the south passage."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road Warden] Gunnar"), "f_rokas_24");

		AddObjective("killHogma", L("Kill Hogma Warriors"),
			new KillObjective(12, new[] { MonsterId.Hogma_Warrior }));

		AddObjective("killCockatries", L("Kill Cockatries"),
			new KillObjective(12, new[] { MonsterId.Cockatries }));

		AddObjective("raiseBanner", L("Raise the Road-Banner at the south gate"),
			new VariableCheckObjective("Laima.Quests.f_rokas_24.Quest1006.BannerRaised", 1, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_24.Quest1006.BannerRaised");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_24.Quest1006.BannerRaised");
	}
}
