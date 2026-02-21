//--- Melia Script ----------------------------------------------------------
// Paupys Crossing Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Paupys Crossing for post-demon war storyline.
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
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Shared.World;
using System.Threading.Tasks;
using Melia.Zone.Network;

public class FSiauliai11ReQuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Crossing Guard Duty
		// =====================================================================
		// Weary Orshan Guard - Uninhabited Crossing
		//---------------------------------------------------------------------
		AddNpc(20060, "[Guard] Borin", "f_siauliai_11_re", 2167, -281, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1001);

			dialog.SetTitle("Borin");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A rain-soaked Orshan guard stands watch at the crossing, constantly swatting at blue Popolions*{/}");
				await dialog.Msg("Another day, another downpour. And these cursed Popolions won't leave me alone!");
				await dialog.Msg("I'm supposed to be watching for bandits, but I can barely keep these pests off my post.");

				var response = await dialog.Select("I haven't had a moment's rest since dawn. Every time I sit down, more of them swarm in.",
					Option("I can help clear them out", "help"),
					Option("Why are there so many?", "why"),
					Option("Stay strong", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*He looks at you with hopeful eyes*{/}");
						await dialog.Msg("You'd do that? Thank the goddess!");
						await dialog.Msg("Just clear enough of them so I can actually do my job. Maybe then I can finally dry off this armor.");

						if (await dialog.YesNo("Will you help clear the Popolions from the crossing?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("I appreciate this more than you know. The Popolions are everywhere - shouldn't take you long to find them.");
							await dialog.Msg("Come back when you've thinned their numbers.");
						}
						break;

					case "why":
						await dialog.Msg("{#666666}*He gestures toward the forest*{/}");
						await dialog.Msg("The rain brings them out, or maybe it's corruption from Koru Jungle seeping this way.");
						await dialog.Msg("Whatever the reason, they've made this crossing miserable to guard.");
						await dialog.Msg("I'd clear them myself, but I can't abandon my post. Orders are orders.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*He returns to watching the road, flinching as another Popolion hops past*{/}");
						await dialog.Msg("Right. I'll... manage somehow.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPopolion", out var obj)) return;

				if (obj.Done)
				{
					await dialog.Msg("{#666666}*He stands a bit straighter, no longer constantly swatting at blue creatures*{/}");
					await dialog.Msg("I noticed the difference immediately. Fewer Popolions hopping around, more time to actually guard.");
					await dialog.Msg("You've done me - and Orsha - a real service today. Here, take this as thanks.");
					await dialog.Msg("{#666666}*He hands you payment from his guard stipend*{/}");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("Still fighting off those blue pests?");
					await dialog.Msg("They're everywhere around here. Keep at it - every one you clear helps.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*He's finally managing to dry his armor in the brief moments between rain showers*{/}");
				await dialog.Msg("The crossing is much more manageable now. Thank you again!");
			}
		});

		// =====================================================================
		// QUEST 1002: The Timber Thief Mystery
		// =====================================================================
		// Angry Lumberjack - Naudingas Felled Area
		//---------------------------------------------------------------------
		AddNpc(20117, "[Lumberjack] Kaspar", "f_siauliai_11_re", 2291, 707, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1002);

			dialog.SetTitle("Kaspar");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*An angry lumberjack gestures at scattered wood logs, several stacks conspicuously empty*{/}");
				await dialog.Msg("This is the third time this week! Every morning, more timber is missing!");
				await dialog.Msg("I cut it, stack it, and by dawn - gone! Someone's stealing my livelihood!");

				var response = await dialog.Select("I need someone to investigate. The guard won't help - they say I'm probably just miscounting. Miscounting!",
					Option("I'll investigate for you", "help"),
					Option("What makes you think it's theft?", "evidence"),
					Option("Maybe you should guard it yourself", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*He grabs your shoulders eagerly*{/}");
						await dialog.Msg("Finally! Someone who takes this seriously!");
						await dialog.Msg("Look around the area - check the three log piles where timber keeps vanishing.");
						await dialog.Msg("Examine them carefully. Maybe you'll find something I missed!");

						if (await dialog.YesNo("Will you investigate the missing timber?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! Start by examining the three log piles. Look for any clues about what's taking my timber.");
						}
						break;

					case "evidence":
						await dialog.Msg("The drag marks! Look at the ground - something heavy was dragged away.");
						await dialog.Msg("And the timing is too consistent. Every night, same amount missing.");
						await dialog.Msg("This isn't animals. This is deliberate!");
						break;

					case "leave":
						await dialog.Msg("{#666666}*His face reddens*{/}");
						await dialog.Msg("Guard it myself? I work from dawn to dusk cutting and processing!");
						await dialog.Msg("I can't stay up all night watching logs too!");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killWoodin", out var woodinObj)) return;

				var inspectedPiles = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_11_re.Quest1002.LogPilesInspected", 0);
				var encounterTriggered = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_11_re.Quest1002.WoodinEncounter", false);

				if (woodinObj.Done)
				{
					await dialog.Msg("{#666666}*He looks relieved as you approach*{/}");
					await dialog.Msg("I heard the commotion! So it was Woodin all along?");
					await dialog.Msg("{#666666}*He slaps his forehead*{/}");
					await dialog.Msg("Of course! The drag marks, the nighttime activity - they're building nests!");
					await dialog.Msg("I've been blaming bandits this whole time when it was just those cursed plants!");
					await dialog.Msg("{#666666}*He laughs in relief and embarrassment*{/}");
					await dialog.Msg("And you've cleared out enough of them that they shouldn't be a problem anymore. Here - you've earned this!");

					character.Quests.Complete(questId);
				}
				else if (encounterTriggered)
				{
					await dialog.Msg("Woodin! So that's what's been stealing my timber!");
					await dialog.Msg("Can you clear them out? I need to make sure they don't come back for more logs.");
				}
				else
				{
					await dialog.Msg("Have you examined the log piles yet?");
					await dialog.Msg("Look for any clues about what's taking my timber!");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*He's hammering wooden stakes into the ground around his lumber stacks*{/}");
				await dialog.Msg("These barriers should keep those Woodin away from my timber. Much appreciated, friend!");
			}
		});

		// Investigation Points - Log Piles
		AddLogPile(1, 2222, 764, 90);
		AddLogPile(2, 2172, 715, 90);
		AddLogPile(3, 2227, 674, 90);

		// =====================================================================
		// QUEST 1003: Herbal Rain Harvest
		// =====================================================================
		// Herbalist Scout - Paslaptis Hideout
		//---------------------------------------------------------------------
		AddNpc(155019, "[Herbalist] Ruta", "f_siauliai_11_re", 1418, 504, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1003);

			dialog.SetTitle("Ruta");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A herbalist crouches between trees and bushes, carefully examining rain-soaked plants*{/}");
				await dialog.Msg("Perfect conditions... absolutely perfect.");
				await dialog.Msg("The endless rain in Paupys Crossing creates ideal conditions for Moonlight Flowers - rare medicinal herbs.");

				var response = await dialog.Select("They only bloom when the rain falls like this. But I can't gather them all myself - too much ground to cover.",
					Option("I can help gather them", "help"),
					Option("What makes these herbs special?", "special"),
					Option("Good luck with your search", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*Her eyes light up*{/}");
						await dialog.Msg("You'd help? Wonderful!");
						await dialog.Msg("I need five in total. They're precious, but rare. You should find them looking around the forest. Take only what we need!");

						if (await dialog.YesNo("Will you help gather the Moonlight Flowers?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Take your time. The rain ensures they'll stay fresh until you return.");
						}
						break;

					case "special":
						await dialog.Msg("{#666666}*She holds up a delicate silver-white flower*{/}");
						await dialog.Msg("Moonlight Flowers have incredible healing properties. They can treat wounds that normal medicine can't touch.");
						await dialog.Msg("But they only bloom during sustained rainfall, and only in specific locations. That's what makes them so valuable.");
						await dialog.Msg("Orsha's wounded soldiers could benefit greatly from these. That's why I'm gathering them.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*She returns to examining the plants around her hideout*{/}");
						await dialog.Msg("May the rain bless your path.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var flowerCount = character.Inventory.CountItem(666062);

				if (flowerCount >= 5)
				{
					await dialog.Msg("{#666666}*She carefully takes the Moonlight Flowers from you*{/}");
					await dialog.Msg("Perfect specimens! You have a gentle touch - not a single petal damaged.");
					await dialog.Msg("These will help many wounded soldiers recover. You've done Orsha a great service.");
					await dialog.Msg("{#666666}*She packages the flowers carefully in oiled cloth*{/}");
					await dialog.Msg("Please, accept this payment. Your help was invaluable.");

					character.Inventory.Remove(666062, 5, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"You've gathered {flowerCount} Moonlight Flowers so far. Look for the blooms in peaceful spots across the crossing.");
					await dialog.Msg("They prefer areas untouched by heavy combat - serene places where the rain falls gently.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*She's preparing the Moonlight Flowers for transport*{/}");
				await dialog.Msg("I'll be taking these to Orsha's medical tents tomorrow. Your help made all the difference.");
			}
		});

		// Moonlight Flower Collection Points
		AddMoonlightFlower(1, 1573, 358, 45);
		AddMoonlightFlower(2, 1537, -265, 45);
		AddMoonlightFlower(3, 2164, 76, 45);
		AddMoonlightFlower(4, 2600, 930, 45);
		AddMoonlightFlower(5, 1399, -520, 45);
		AddMoonlightFlower(6, 1042, -53, 45);
		AddMoonlightFlower(7, 537, 34, 45);
		AddMoonlightFlower(8, 897, 988, 44);
		AddMoonlightFlower(9, 887, 1605, 45);
		AddMoonlightFlower(10, 507, -368, 45);
		AddMoonlightFlower(11, 306, -953, 45);
		AddMoonlightFlower(12, 1426, -1259, 45);
		AddMoonlightFlower(13, -1329, 1000, 45);
		AddMoonlightFlower(14, -1283, 1838, 45);

		// =====================================================================
		// QUEST 1004: Corruption Watch
		// =====================================================================
		// Cliff Scout - Rohonsa Cliff
		//---------------------------------------------------------------------
		AddNpc(20059, "[Scout] Daina", "f_siauliai_11_re", 781, -694, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1004);

			dialog.SetTitle("Daina");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*An Orshan scout crouches at the cliff edge, she looks furious*{/}");
				await dialog.Msg("Ugh! Not again!");
				await dialog.Msg("{#666666}*She points down cliff at a blue Popolion bouncing away with something shiny in its mouth*{/}");
				await dialog.Msg("I knew it! That little pest stole one of my healing potions!");

				var response = await dialog.Select("The purification crystals are down at Deer Hooves Lot, below the cliff. I need someone to check the three crystals for traces of corruption.",
					Option("I'll check the crystals for you", "help"),
					Option("You're a soldier of Orsha, deal with it!", "tough"),
					Option("That does sound annoying", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*Her face lights up with relief*{/}");
						await dialog.Msg("Really? You're a lifesaver!");
						await dialog.Msg("Head down to Deer Hooves Lot and inspect the three purification crystals. They're enchanted to detect corruption from Koru Jungle.");
						await dialog.Msg("{#666666}*She glances at the Popolions and Hanaming nearby*{/}");
						await dialog.Msg("And while you're at it... if you happen to get rid of some of those pests down there, I'd be grateful. They've been making my work impossible.");

						if (await dialog.YesNo("Will you inspect the purification crystals at Deer Hooves Lot?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! The crystals should show their corruption readings when you examine them.");
							await dialog.Msg("And if you clear out some Popolions and Hanaming while you're there... well, that would be a nice bonus.");
						}
						break;

					case "tough":
						await dialog.Msg("{#666666}*Her eyes narrow*{/}");
						await dialog.Msg("Deal with it? DEAL WITH IT?!");
						await dialog.Msg("I've been on this cliff for three days straight monitoring corruption spread from Koru Jungle!");
						await dialog.Msg("I can handle bandits. I can handle monsters. I can even handle demon corruption.");
						await dialog.Msg("{#666666}*She gestures emphatically*{/}");
						await dialog.Msg("But these little THIEVES keep stealing my supplies, interrupting my sleep, and making nests in my gear!");
						await dialog.Msg("You try doing surveillance work when something keeps chewing your hair in the middle of the night!");
						await dialog.Msg("{#666666}*She takes a deep breath, calming herself*{/}");
						await dialog.Msg("...I'm a scout, not a pest exterminator. That's not what Orsha trained me for.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*She sighs and swats at another Popolion hopping toward her pack*{/}");
						await dialog.Msg("Right. I suppose I'll just... keep guard duty with one eye open from now on.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPopolion", out var popObj)) return;
				if (!quest.TryGetProgress("killHanaming", out var hanObj)) return;

				var marker1 = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_11_re.Quest1004.Marker1", false);
				var marker2 = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_11_re.Quest1004.Marker2", false);
				var marker3 = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_11_re.Quest1004.Marker3", false);

				if (popObj.Done && hanObj.Done && marker1 && marker2 && marker3)
				{
					await dialog.Msg("{#666666}*She's finally able to work without creatures stealing her supplies*{/}");
					await dialog.Msg("Peace at last! I can actually focus on my observations now.");
					await dialog.Msg("And the crystal readings... all showing mild traces only. Minor contamination, but not spreading rapidly.");
					await dialog.Msg("{#666666}*She retrieves her stolen potion from a nearby bush where you must have dropped it*{/}");
					await dialog.Msg("Ha! Found it! Those little thieves.");
					await dialog.Msg("This is valuable information for Orsha's defense planning. And I can finally sleep without guarding my hair. You've helped more than you know!");

					character.Quests.Complete(questId);
				}
				else
				{
					var needKills = !popObj.Done || !hanObj.Done;
					var needMarkers = !marker1 || !marker2 || !marker3;

					if (needKills)
					{
						await dialog.Msg("The pests are still causing trouble. A few less would make all the difference!");
					}
					else if (needMarkers)
					{
						await dialog.Msg("Good, they're not bothering me as much. Now check those purification crystals for me.");
					}
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*She's making detailed observations with her supplies safely secured*{/}");
				await dialog.Msg("The contamination is stable for now. And no more potion thieves! Thanks again for your help!");
			}
		});

		// Purification Crystals - Deer Hooves Lot
		AddPurificationCrystal(1, 462, -1031, 45);
		AddPurificationCrystal(2, 781, -1179, 44);
		AddPurificationCrystal(3, 349, -1404, 45);

		// =====================================================================
		// QUEST 1005: Detection Device Test
		// =====================================================================
		// Commander Uska - Ashaq Underground Prison Entrance
		//---------------------------------------------------------------------
		AddNpc(20113, "[Commander] Uska", "f_siauliai_11_re", 389, 1669, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1005);

			dialog.SetTitle("Uska");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A stern Orshan commander stands before the entrance to Ashaq Underground Prison, inspecting a glowing magical device*{/}");
				await dialog.Msg("Otherworlder. Your timing is fortuitous.");
				await dialog.Msg("This prison holds some of the most dangerous creatures that survived the demon war. Dumaros, Wendigos - things that should never see daylight again.");

				var response = await dialog.Select("The magical detection device that monitors threats from the underground cells is critical for Orsha's defense. But it hasn't been tested in months. I need someone to verify it's working properly.",
					Option("I'll help test the device", "help"),
					Option("What does the device detect?", "device"),
					Option("Sounds dangerous", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*He nods approvingly*{/}");
						await dialog.Msg("Good. I expected nothing less from someone who assists Orsha.");
						await dialog.Msg("I've prepared monster bait designed to lure prison creatures to the surface. Deploy it at three designated points near the detection device.");
						await dialog.Msg("{#666666}*He hands you a sealed package*{/}");
						await dialog.Msg("When the bait attracts Dumaros and Wendigos, the device should alert us. That's how we'll know it's functioning.");

						if (await dialog.YesNo("Will you test the detection device for Orsha's security?"))
						{
							character.Quests.Start(questId);
							character.Inventory.Add(668008, 1, InventoryAddType.PickUp);
							await dialog.Msg("Deploy the bait at the detection device three times. The creatures that emerge will be dangerous - Blue Dumaros and Blue Wendigos.");
							await dialog.Msg("Eliminate any threats that surface, then report back to me with the results.");
						}
						break;

					case "device":
						await dialog.Msg("{#666666}*He gestures to the glowing apparatus*{/}");
						await dialog.Msg("This magical device was created by Orsha's war mages specifically to monitor the underground prison.");
						await dialog.Msg("It detects movement, magical signatures, and escape attempts from the cells below. When something threatens to break free, it alerts the garrison.");
						await dialog.Msg("Without it functioning properly, we're blind to what's happening in those depths. And that is unacceptable.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*He returns to inspecting the device*{/}");
						await dialog.Msg("Then step aside. Orsha's defense cannot wait for the hesitant.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var testCount = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_11_re.Quest1005.DeviceTests", 0);

				if (testCount >= 3)
				{
					await dialog.Msg("{#666666}*He reviews notes on a tactical parchment as you approach*{/}");
					await dialog.Msg("The device registered all three incursions. Blue Dumaros, Blue Wendigos - exactly as predicted.");
					await dialog.Msg("More importantly, the detection alerts triggered immediately when the bait drew them near. The system is fully operational.");
					await dialog.Msg("{#666666}*He nods with satisfaction*{/}");
					await dialog.Msg("Excellent work. You've confirmed that Orsha's early warning system for the prison remains intact.");
					await dialog.Msg("This is critical intelligence for our city's defense. Here - you've earned proper compensation.");
					await dialog.Msg("{#666666}*He hands you a military-issue sword*{/}");
					await dialog.Msg("A soldier's weapon, for someone who serves Orsha's interests well.");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"The device has registered {testCount} tests so far. You need to complete all three deployment points.");
					await dialog.Msg("Use the bait at the detection device. Be prepared to fight whatever emerges from below.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*He monitors the detection device's readouts with professional vigilance*{/}");
				await dialog.Msg("The device continues to function perfectly. Orsha's prison remains secure. Your assistance was invaluable.");
			}
		});

		// Magical Detection Device
		AddNpc(153137, "Magical Detection Device", "f_siauliai_11_re", 223, 1545, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1005);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg("{#666666}*A complex magical apparatus hums with arcane energy, monitoring the prison depths below*{/}");
				return;
			}

			if (!character.Inventory.HasItem(668008))
			{
				await dialog.Msg("{#666666}*The device awaits deployment of the monster bait*{/}");
				await dialog.Msg("{#666666}*You need the bait from Commander Uska to proceed*{/}");
				return;
			}

			var testCount = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_11_re.Quest1005.DeviceTests", 0);
			var spawnedKey = $"Laima.Quests.f_siauliai_11_re.Quest1005.Test{testCount + 1}.Spawned";

			if (testCount >= 3)
			{
				await dialog.Msg("{#666666}*The device has completed all three detection tests. Return to Commander Uska.*{/}");
				return;
			}

			var result = await character.TimeActions.StartAsync(
				"Deploying monster bait...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
			);

			if (result == TimeActionResult.Completed)
			{
				testCount++;
				character.Variables.Perm.Set("Laima.Quests.f_siauliai_11_re.Quest1005.DeviceTests", testCount);

				await dialog.Msg("{#666666}*The bait releases its scent. The detection device's runes flare brightly!*{/}");
				character.ServerMessage($"Detection tests completed: {testCount}/3");

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (
						SpawnTempMonsters(character, MonsterId.Dumaro_Blue, 3, 200, TimeSpan.FromMinutes(2)) &&
						SpawnTempMonsters(character, MonsterId.Wendigo_Blue, 3, 200, TimeSpan.FromMinutes(2))
						)
					{
						await dialog.Msg("{#666666}*Creatures emerge from the prison depths!*{/}");

						// Play alerting effect
						for (var i = 0; i < 3; i++)
						{
							if (character.Connection != null)
								Send.ZC_NORMAL.PlayEffect(character.Connection, dialog.Npc, animationName: "F_lineup017_red3", scale: 5);
							if (i < 2) Task.Delay(1500);
						}
					}

					character.ServerMessage("{#FF6666}The device glows! Prison creatures detected!{/}");
				}

				if (testCount >= 3)
				{
					character.Quests.CompleteObjective(questId, "testDevice");
					character.ServerMessage("{#FFD700}All detection tests complete! Return to Commander Uska.{/}");
				}
			}
			else
			{
				character.ServerMessage("Bait deployment cancelled.");
			}
		});

		// =====================================================================
		// QUEST 1006: A Moment of Peace
		// =====================================================================
		// Hermit Settler - Secluded Grove
		//---------------------------------------------------------------------
		AddNpc(20165, "[Hermit] Lukas", "f_siauliai_11_re", -1616, 1403, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1006);

			dialog.SetTitle("Lukas");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A peaceful hermit sits cross-legged in a secluded grove, completely relaxed*{/}");
				await dialog.Msg("{#666666}*He opens his eyes slowly as you approach, a serene smile on his face*{/}");
				await dialog.Msg("Hey there, friend. Beautiful day, isn't it?");
				await dialog.Msg("The rain, the quiet... it's perfect. Just perfect.");

				var response = await dialog.Select("You know, people are always rushing around. Fighting, questing, chasing rewards. But sometimes... sometimes you just need to sit. Breathe. Be.",
					Option("That sounds nice", "help"),
					Option("What brought you here?", "about"),
					Option("How do you find inner peace?", "peace"),
					Option("Any advice for staying calm?", "advice"),
					Option("I should go", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*His smile widens*{/}");
						await dialog.Msg("Right? Just... nice.");
						await dialog.Msg("You want to join me? No agenda, no tasks. Just sit here with me for a bit.");
						await dialog.Msg("Listen to the rain. Feel the positive energy of this grove. Let yourself relax.");

						if (await dialog.YesNo("Want to sit with me for a moment?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Awesome. Just come back and talk to me when you're ready.");
							await dialog.Msg("No rush. Take your time.");
						}
						break;

					case "about":
						await dialog.Msg("{#666666}*He gazes peacefully at the rain*{/}");
						await dialog.Msg("After the war... I just couldn't do it anymore. The fighting, the stress.");
						await dialog.Msg("I needed to find balance. Inner peace. Positive energy.");
						await dialog.Msg("So I came here. This grove accepted me. Taught me to slow down.");
						await dialog.Msg("Best decision I ever made.");
						break;

					case "peace":
						await dialog.Msg("{#666666}*He closes his eyes for a moment, breathing deeply*{/}");
						await dialog.Msg("Inner peace isn't something you find. It's something you allow.");
						await dialog.Msg("We spend so much time fighting - fighting monsters, fighting fate, fighting ourselves.");
						await dialog.Msg("But peace? Peace comes when you stop fighting. When you accept the moment as it is.");
						await dialog.Msg("The rain falls. I don't fight it. I don't wish for sunshine. I just... let it be rain.");
						await dialog.Msg("That's the secret, friend. Stop resisting what is. Just be present.");
						break;

					case "advice":
						await dialog.Msg("{#666666}*He gestures to the grove around him*{/}");
						await dialog.Msg("Nature is the best teacher. Watch the trees - they don't stress about tomorrow's weather.");
						await dialog.Msg("Listen to the rain - it doesn't hurry, doesn't force. It just flows.");
						await dialog.Msg("When you feel overwhelmed, find a quiet spot. Sit down. Close your eyes.");
						await dialog.Msg("Breathe in for four counts. Hold for four. Breathe out for four. Hold for four.");
						await dialog.Msg("Do that a few times. Feel your heartbeat slow. Your mind clear.");
						await dialog.Msg("{#666666}*He smiles warmly*{/}");
						await dialog.Msg("It's simple, but simple works. The world makes things complicated. Peace is simple.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*He nods peacefully*{/}");
						await dialog.Msg("All good. May your path be peaceful, friend.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				await dialog.Msg("{#666666}*He opens his eyes as you approach, smiling warmly*{/}");
				await dialog.Msg("Hey there. How did it feel?");
				await dialog.Msg("Did you find your inner peace moment?");
				await dialog.Msg("{#666666}*He doesn't wait for an answer, just nods knowingly*{/}");
				await dialog.Msg("I can see it in your eyes. You get it now.");
				await dialog.Msg("That's all it takes - just a moment of being present. Of letting go.");
				await dialog.Msg("{#666666}*He hands you something with a gentle smile*{/}");
				await dialog.Msg("Thanks for sharing this time with me, friend. You're always welcome back here.");

				character.Quests.Complete(questId);
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*He's returned to his meditation, completely at peace*{/}");
				await dialog.Msg("Welcome back. The grove is always open to you.");
			}
		});

	}

	// Helper method for log pile investigation points
	void AddLogPile(int pileNum, int x, int z, int direction)
	{
		AddNpc(151031, "Log Pile", "f_siauliai_11_re", x, z, direction, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1002);
			var variableKey = $"Laima.Quests.f_siauliai_11_re.Quest1002.LogPile{pileNum}";

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg("{#666666}*A stack of freshly cut timber logs*{/}");
				return;
			}

			if (character.Variables.Perm.GetBool(variableKey, false))
			{
				await dialog.Msg("{#666666}*You've already examined this log pile*{/}");
				return;
			}

			var result = await character.TimeActions.StartAsync(
				"Examining log pile...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
			);

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(variableKey, true);

				var inspectedCount = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_11_re.Quest1002.LogPilesInspected", 0);
				inspectedCount++;
				character.Variables.Perm.Set("Laima.Quests.f_siauliai_11_re.Quest1002.LogPilesInspected", inspectedCount);

				// Different clues for each pile
				switch (pileNum)
				{
					case 1:
						await dialog.Msg("{#666666}*You notice deep drag marks in the muddy ground leading away from the pile*{/}");
						await dialog.Msg("{#666666}*The marks suggest something heavy was pulled, not carried*{/}");
						break;
					case 2:
						await dialog.Msg("{#666666}*Fresh scratches mark the sides of several logs*{/}");
						await dialog.Msg("{#666666}*The scratches look almost like... claw marks? But oddly plant-like*{/}");
						break;
					case 3:
						await dialog.Msg("{#666666}*Scattered wood chips and sawdust form a trail leading into the forest*{/}");
						await dialog.Msg("{#666666}*You notice some broken twigs and disturbed vegetation nearby*{/}");
						break;
				}

				character.ServerMessage($"Log piles examined: {inspectedCount}/3");

				// Spawn Woodin at the last pile
				if (inspectedCount >= 3)
				{
					var spawnedKey = "Laima.Quests.f_siauliai_11_re.Quest1002.WoodinEncounter";
					var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);

					if (!hasSpawned)
					{
						character.Variables.Perm.Set(spawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Woodin, 3, 70, TimeSpan.FromMinutes(2)))
						{
							await dialog.Msg("{#666666}*Suddenly, three Woodin emerge from the forest, clutching pieces of timber!*{/}");
							character.ServerMessage("{#FF6666}Woodin caught stealing timber!{/}");
							character.Quests.CompleteObjective(questId, "investigateLogPiles");
						}
					}
				}
			}
			else
			{
				character.ServerMessage("Examination cancelled.");
			}
		});
	}

	// Helper method for Moonlight Flower collection spots
	void AddMoonlightFlower(int flowerNum, int x, int z, int direction)
	{
		AddNpc(156040, "Blooming Moonlight Flower", "f_siauliai_11_re", x, z, direction, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1003);
			var variableKey = $"Laima.Quests.f_siauliai_11_re.Quest1003.Flower{flowerNum}";

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg("{#666666}*A delicate silver-white flower blooms in the rain*{/}");
				return;
			}

			if (character.Variables.Perm.GetBool(variableKey, false))
			{
				await dialog.Msg("{#666666}*You've already collected a flower from this spot*{/}");
				return;
			}

			var result = await character.TimeActions.StartAsync(
				"Carefully harvesting Moonlight Flower...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
			);

			if (result == TimeActionResult.Completed)
			{
				character.Inventory.Add(666062, 1, InventoryAddType.PickUp);
				character.Variables.Perm.Set(variableKey, true);
				character.ServerMessage("Harvested: Moonlight Flower");

				var currentCount = character.Inventory.CountItem(666062);
				character.ServerMessage($"Moonlight Flowers collected: {currentCount}/5");

				if (currentCount >= 5)
				{
					character.ServerMessage("{#FFD700}All Moonlight Flowers collected! Return to Herbalist Ruta.{/}");
				}
			}
			else
			{
				character.ServerMessage("Harvest cancelled.");
			}
		});
	}

	// Helper method for purification crystals
	void AddPurificationCrystal(int crystalNum, int x, int z, int direction)
	{
		AddNpc(46218, "Purification Crystal", "f_siauliai_11_re", x, z, direction, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1004);
			var variableKey = $"Laima.Quests.f_siauliai_11_re.Quest1004.Marker{crystalNum}";

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg("{#666666}*A purification crystal hums with faint magical energy*{/}");
				return;
			}

			if (character.Variables.Perm.GetBool(variableKey, false))
			{
				await dialog.Msg("{#666666}*You've already inspected this purification crystal*{/}");
				return;
			}

			var result = await character.TimeActions.StartAsync(
				"Inspecting purification crystal...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
			);

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(variableKey, true);

				await dialog.Msg("{#666666}*The crystal pulses softly - showing only mild traces of corruption*{/}");
				character.ServerMessage($"Purification Crystal {crystalNum} inspected: mild corruption traces");

				var crystalCount = 0;
				for (int i = 1; i <= 3; i++)
				{
					if (character.Variables.Perm.GetBool($"Laima.Quests.f_siauliai_11_re.Quest1004.Marker{i}", false))
						crystalCount++;
				}

				character.ServerMessage($"Purification crystals inspected: {crystalCount}/3");

				if (crystalCount >= 3)
				{
					character.Quests.CompleteObjective(questId, "inspectCrystals");
					character.ServerMessage("All crystals inspected. Return to Scout Daina.");
				}
			}
			else
			{
				character.ServerMessage("Inspection cancelled.");
			}
		});
	}

}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Crossing Guard Duty
//-----------------------------------------------------------------------------
public class CrossingGuardDutyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_11_re", 1001);
		SetName("Crossing Guard Duty");
		SetDescription("Help Guard Borin clear aggressive Popolions from the Uninhabited Crossing so he can properly watch for threats.");
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Guard] Borin", "f_siauliai_11_re");
		// Objectives
		AddObjective("killPopolion", "Clear Popolion from the crossing",
			new KillObjective(18, new[] { MonsterId.Sec_Popolion_Blue }));

		// Rewards
		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 1));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8));  // Normal SP Potion
	}
}

// Quest 1002 CLASS: The Timber Thief Mystery
//-----------------------------------------------------------------------------
public class TimberThiefMysteryQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_11_re", 1002);
		SetName("The Timber Thief Mystery");
		SetDescription("Investigate who or what is stealing timber from Lumberjack Kaspar's work area by examining log piles, then deal with the culprits.");
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential); // Sequential
		AddQuestGiver("[Lumberjack] Kaspar", "f_siauliai_11_re");
		// Objectives
		AddObjective("investigateLogPiles", "Examine the three log piles for clues",
			new ManualObjective());
		AddObjective("killWoodin", "Hunt Woodin",
			new KillObjective(6, new[] { MonsterId.Woodin }));

		// Rewards
		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8));  // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clear investigation variables
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.LogPile1");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.LogPile2");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.LogPile3");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.LogPilesInspected");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.WoodinEncounter");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.LogPile1");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.LogPile2");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.LogPile3");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.LogPilesInspected");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1002.WoodinEncounter");
	}
}

// Quest 1003 CLASS: Herbal Rain Harvest
//-----------------------------------------------------------------------------
public class HerbalRainHarvestQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_11_re", 1003);
		SetName("Herbal Rain Harvest");
		SetDescription("Help Herbalist Ruta gather rare Moonlight Flowers that bloom during Paupys Crossing's endless rain.");
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Herbalist] Ruta", "f_siauliai_11_re");
		// Objectives
		AddObjective("collectFlowers", "Gather Moonlight Flowers from five locations",
			new CollectItemObjective(666062, 5));

		// Rewards
		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 1));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8));  // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(666062, character.Inventory.CountItem(666062),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags
		for (int i = 1; i <= 14; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_11_re.Quest1003.Flower{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(666062, character.Inventory.CountItem(666062),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 14; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_11_re.Quest1003.Flower{i}");
		}
	}
}

// Quest 1004 CLASS: Corruption Watch
//-----------------------------------------------------------------------------
public class CorruptionWatchQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_11_re", 1004);
		SetName("Corruption Watch");
		SetDescription("Help Scout Daina inspect the three purification crystals at Deer Hooves Lot to check for traces of corruption from Koru Jungle.");
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Scout] Daina", "f_siauliai_11_re");
		// Objectives
		AddObjective("inspectCrystals", "Inspect three purification crystals at Deer Hooves Lot",
			new ManualObjective());
		AddObjective("killPopolion", "Clear Popolion",
			new KillObjective(10, new[] { MonsterId.Sec_Popolion_Blue }));
		AddObjective("killHanaming", "Clear Hanaming",
			new KillObjective(10, new[] { MonsterId.Sec_Hanaming }));

		// Rewards
		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8));  // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
		AddReward(new ItemReward(181114, 1));  // Crossbow
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clear marker flags
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1004.Marker1");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1004.Marker2");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1004.Marker3");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1004.Marker1");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1004.Marker2");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1004.Marker3");
	}
}

// Quest 1005 CLASS: Detection Device Test
//-----------------------------------------------------------------------------
public class DetectionDeviceTestQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_11_re", 1005);
		SetName("Detection Device Test");
		SetDescription("Help Commander Uska test the magical detection device that monitors threats from Ashaq Underground Prison by deploying monster bait to attract prison creatures.");
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Commander] Uska", "f_siauliai_11_re");
		// Objectives
		AddObjective("testDevice", "Deploy monster bait at the detection device three times",
			new ManualObjective());

		// Rewards
		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8));  // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
		AddReward(new ItemReward(101120, 1));  // Kaskara
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest item
		character.Inventory.Remove(668008, character.Inventory.CountItem(668008),
			InventoryItemRemoveMsg.Destroyed);

		// Clear test counter and spawn flags
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1005.DeviceTests");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1005.Test1.Spawned");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1005.Test2.Spawned");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1005.Test3.Spawned");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(668008, character.Inventory.CountItem(668008),
			InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1005.DeviceTests");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1005.Test1.Spawned");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1005.Test2.Spawned");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_11_re.Quest1005.Test3.Spawned");
	}
}

// Quest 1006 CLASS: A Moment of Peace
//-----------------------------------------------------------------------------
public class AMomentOfPeaceQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_11_re", 1006);
		SetName("A Moment of Peace");
		SetDescription("Join Hermit Lukas for a peaceful moment of meditation and relaxation in his secluded grove.");
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Hermit] Lukas", "f_siauliai_11_re");
		// Objectives
		AddObjective("sitWithLukas", "Sit peacefully with Lukas",
			new ManualObjective());

		// Rewards
		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 1));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8));  // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
		AddReward(new ItemReward(648513, 1));  // Emoticon: Little Burk(Peaceful)
	}
}
