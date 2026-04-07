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
		AddNpc(20060, L("[Guard] Borin"), "f_siauliai_11_re", 2167, -281, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1001);

			dialog.SetTitle(L("Borin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A rain-soaked Orshan guard stands watch at the crossing, constantly swatting at blue Popolions*{/}"));
				await dialog.Msg(L("Another day, another downpour. And these cursed Popolions won't leave me alone!"));
				await dialog.Msg(L("I'm supposed to be watching for bandits, but I can barely keep these pests off my post."));

				var response = await dialog.Select(L("I haven't had a moment's rest since dawn. Every time I sit down, more of them swarm in."),
					Option(L("I can help clear them out"), "help"),
					Option(L("Why are there so many?"), "why"),
					Option(L("Stay strong"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He looks at you with hopeful eyes*{/}"));
						await dialog.Msg(L("You'd do that? Thank the goddess!"));
						await dialog.Msg(L("Just clear enough of them so I can actually do my job. Maybe then I can finally dry off this armor."));

						if (await dialog.YesNo(L("Will you help clear the Popolions from the crossing?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("I appreciate this more than you know. The Popolions are everywhere - shouldn't take you long to find them."));
							await dialog.Msg(L("Come back when you've thinned their numbers."));
						}
						break;

					case "why":
						await dialog.Msg(L("{#666666}*He gestures toward the forest*{/}"));
						await dialog.Msg(L("The rain brings them out, or maybe it's corruption from Koru Jungle seeping this way."));
						await dialog.Msg(L("Whatever the reason, they've made this crossing miserable to guard."));
						await dialog.Msg(L("I'd clear them myself, but I can't abandon my post. Orders are orders."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He returns to watching the road, flinching as another Popolion hops past*{/}"));
						await dialog.Msg(L("Right. I'll... manage somehow."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPopolion", out var obj)) return;

				if (obj.Done)
				{
					await dialog.Msg(L("{#666666}*He stands a bit straighter, no longer constantly swatting at blue creatures*{/}"));
					await dialog.Msg(L("I noticed the difference immediately. Fewer Popolions hopping around, more time to actually guard."));
					await dialog.Msg(L("You've done me - and Orsha - a real service today. Here, take this as thanks."));
					await dialog.Msg(L("{#666666}*He hands you payment from his guard stipend*{/}"));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still fighting off those blue pests?"));
					await dialog.Msg(L("They're everywhere around here. Keep at it - every one you clear helps."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*He's finally managing to dry his armor in the brief moments between rain showers*{/}"));
				await dialog.Msg(L("The crossing is much more manageable now. Thank you again!"));
			}
		});

		// =====================================================================
		// QUEST 1002: The Timber Thief Mystery
		// =====================================================================
		// Angry Lumberjack - Naudingas Felled Area
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Lumberjack] Kaspar"), "f_siauliai_11_re", 2291, 707, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1002);

			dialog.SetTitle(L("Kaspar"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An angry lumberjack gestures at scattered wood logs, several stacks conspicuously empty*{/}"));
				await dialog.Msg(L("This is the third time this week! Every morning, more timber is missing!"));
				await dialog.Msg(L("I cut it, stack it, and by dawn - gone! Someone's stealing my livelihood!"));

				var response = await dialog.Select(L("I need someone to investigate. The guard won't help - they say I'm probably just miscounting. Miscounting!"),
					Option(L("I'll investigate for you"), "help"),
					Option(L("What makes you think it's theft?"), "evidence"),
					Option(L("Maybe you should guard it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He grabs your shoulders eagerly*{/}"));
						await dialog.Msg(L("Finally! Someone who takes this seriously!"));
						await dialog.Msg(L("Look around the area - check the three log piles where timber keeps vanishing."));
						await dialog.Msg(L("Examine them carefully. Maybe you'll find something I missed!"));

						if (await dialog.YesNo(L("Will you investigate the missing timber?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! Start by examining the three log piles. Look for any clues about what's taking my timber."));
						}
						break;

					case "evidence":
						await dialog.Msg(L("The drag marks! Look at the ground - something heavy was dragged away."));
						await dialog.Msg(L("And the timing is too consistent. Every night, same amount missing."));
						await dialog.Msg(L("This isn't animals. This is deliberate!"));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*His face reddens*{/}"));
						await dialog.Msg(L("Guard it myself? I work from dawn to dusk cutting and processing!"));
						await dialog.Msg(L("I can't stay up all night watching logs too!"));
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
					await dialog.Msg(L("{#666666}*He looks relieved as you approach*{/}"));
					await dialog.Msg(L("I heard the commotion! So it was Woodin all along?"));
					await dialog.Msg(L("{#666666}*He slaps his forehead*{/}"));
					await dialog.Msg(L("Of course! The drag marks, the nighttime activity - they're building nests!"));
					await dialog.Msg(L("I've been blaming bandits this whole time when it was just those cursed plants!"));
					await dialog.Msg(L("{#666666}*He laughs in relief and embarrassment*{/}"));
					await dialog.Msg(L("And you've cleared out enough of them that they shouldn't be a problem anymore. Here - you've earned this!"));

					character.Quests.Complete(questId);
				}
				else if (encounterTriggered)
				{
					await dialog.Msg(L("Woodin! So that's what's been stealing my timber!"));
					await dialog.Msg(L("Can you clear them out? I need to make sure they don't come back for more logs."));
				}
				else
				{
					await dialog.Msg(L("Have you examined the log piles yet?"));
					await dialog.Msg(L("Look for any clues about what's taking my timber!"));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*He's hammering wooden stakes into the ground around his lumber stacks*{/}"));
				await dialog.Msg(L("These barriers should keep those Woodin away from my timber. Much appreciated, friend!"));
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
		AddNpc(155019, L("[Herbalist] Ruta"), "f_siauliai_11_re", 1418, 504, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1003);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A herbalist crouches between trees and bushes, carefully examining rain-soaked plants*{/}"));
				await dialog.Msg(L("Perfect conditions... absolutely perfect."));
				await dialog.Msg(L("The endless rain in Paupys Crossing creates ideal conditions for Moonlight Flowers - rare medicinal herbs."));

				var response = await dialog.Select(L("They only bloom when the rain falls like this. But I can't gather them all myself - too much ground to cover."),
					Option(L("I can help gather them"), "help"),
					Option(L("What makes these herbs special?"), "special"),
					Option(L("Good luck with your search"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*Her eyes light up*{/}"));
						await dialog.Msg(L("You'd help? Wonderful!"));
						await dialog.Msg(L("I need five in total. They're precious, but rare. You should find them looking around the forest. Take only what we need!"));

						if (await dialog.YesNo(L("Will you help gather the Moonlight Flowers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Take your time. The rain ensures they'll stay fresh until you return."));
						}
						break;

					case "special":
						await dialog.Msg(L("{#666666}*She holds up a delicate silver-white flower*{/}"));
						await dialog.Msg(L("Moonlight Flowers have incredible healing properties. They can treat wounds that normal medicine can't touch."));
						await dialog.Msg(L("But they only bloom during sustained rainfall, and only in specific locations. That's what makes them so valuable."));
						await dialog.Msg(L("Orsha's wounded soldiers could benefit greatly from these. That's why I'm gathering them."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*She returns to examining the plants around her hideout*{/}"));
						await dialog.Msg(L("May the rain bless your path."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var flowerCount = character.Inventory.CountItem(666062);

				if (flowerCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She carefully takes the Moonlight Flowers from you*{/}"));
					await dialog.Msg(L("Perfect specimens! You have a gentle touch - not a single petal damaged."));
					await dialog.Msg(L("These will help many wounded soldiers recover. You've done Orsha a great service."));
					await dialog.Msg(L("{#666666}*She packages the flowers carefully in oiled cloth*{/}"));
					await dialog.Msg(L("Please, accept this payment. Your help was invaluable."));

					character.Inventory.Remove(666062, 5, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("You've gathered {0} Moonlight Flowers so far. Look for the blooms in peaceful spots across the crossing.", flowerCount));
					await dialog.Msg(L("They prefer areas untouched by heavy combat - serene places where the rain falls gently."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*She's preparing the Moonlight Flowers for transport*{/}"));
				await dialog.Msg(L("I'll be taking these to Orsha's medical tents tomorrow. Your help made all the difference."));
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
		AddNpc(20059, L("[Scout] Daina"), "f_siauliai_11_re", 781, -694, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1004);

			dialog.SetTitle(L("Daina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An Orshan scout crouches at the cliff edge, she looks furious*{/}"));
				await dialog.Msg(L("Ugh! Not again!"));
				await dialog.Msg(L("{#666666}*She points down cliff at a blue Popolion bouncing away with something shiny in its mouth*{/}"));
				await dialog.Msg(L("I knew it! That little pest stole one of my healing potions!"));

				var response = await dialog.Select(L("The purification crystals are down at Deer Hooves Lot, below the cliff. I need someone to check the three crystals for traces of corruption."),
					Option(L("I'll check the crystals for you"), "help"),
					Option(L("You're a soldier of Orsha, deal with it!"), "tough"),
					Option(L("That does sound annoying"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*Her face lights up with relief*{/}"));
						await dialog.Msg(L("Really? You're a lifesaver!"));
						await dialog.Msg(L("Head down to Deer Hooves Lot and inspect the three purification crystals. They're enchanted to detect corruption from Koru Jungle."));
						await dialog.Msg(L("{#666666}*She glances at the Popolions and Hanaming nearby*{/}"));
						await dialog.Msg(L("And while you're at it... if you happen to get rid of some of those pests down there, I'd be grateful. They've been making my work impossible."));

						if (await dialog.YesNo(L("Will you inspect the purification crystals at Deer Hooves Lot?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! The crystals should show their corruption readings when you examine them."));
							await dialog.Msg(L("And if you clear out some Popolions and Hanaming while you're there... well, that would be a nice bonus."));
						}
						break;

					case "tough":
						await dialog.Msg(L("{#666666}*Her eyes narrow*{/}"));
						await dialog.Msg(L("Deal with it? DEAL WITH IT?!"));
						await dialog.Msg(L("I've been on this cliff for three days straight monitoring corruption spread from Koru Jungle!"));
						await dialog.Msg(L("I can handle bandits. I can handle monsters. I can even handle demon corruption."));
						await dialog.Msg(L("{#666666}*She gestures emphatically*{/}"));
						await dialog.Msg(L("But these little THIEVES keep stealing my supplies, interrupting my sleep, and making nests in my gear!"));
						await dialog.Msg(L("You try doing surveillance work when something keeps chewing your hair in the middle of the night!"));
						await dialog.Msg(L("{#666666}*She takes a deep breath, calming herself*{/}"));
						await dialog.Msg(L("...I'm a scout, not a pest exterminator. That's not what Orsha trained me for."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*She sighs and swats at another Popolion hopping toward her pack*{/}"));
						await dialog.Msg(L("Right. I suppose I'll just... keep guard duty with one eye open from now on."));
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
					await dialog.Msg(L("{#666666}*She's finally able to work without creatures stealing her supplies*{/}"));
					await dialog.Msg(L("Peace at last! I can actually focus on my observations now."));
					await dialog.Msg(L("And the crystal readings... all showing mild traces only. Minor contamination, but not spreading rapidly."));
					await dialog.Msg(L("{#666666}*She retrieves her stolen potion from a nearby bush where you must have dropped it*{/}"));
					await dialog.Msg(L("Ha! Found it! Those little thieves."));
					await dialog.Msg(L("This is valuable information for Orsha's defense planning. And I can finally sleep without guarding my hair. You've helped more than you know!"));

					character.Quests.Complete(questId);
				}
				else
				{
					var needKills = !popObj.Done || !hanObj.Done;
					var needMarkers = !marker1 || !marker2 || !marker3;

					if (needKills)
					{
						await dialog.Msg(L("The pests are still causing trouble. A few less would make all the difference!"));
					}
					else if (needMarkers)
					{
						await dialog.Msg(L("Good, they're not bothering me as much. Now check those purification crystals for me."));
					}
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*She's making detailed observations with her supplies safely secured*{/}"));
				await dialog.Msg(L("The contamination is stable for now. And no more potion thieves! Thanks again for your help!"));
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
		AddNpc(20113, L("[Commander] Uska"), "f_siauliai_11_re", 389, 1669, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1005);

			dialog.SetTitle(L("Uska"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A stern Orshan commander stands before the entrance to Ashaq Underground Prison, inspecting a glowing magical device*{/}"));
				await dialog.Msg(L("Otherworlder. Your timing is fortuitous."));
				await dialog.Msg(L("This prison holds some of the most dangerous creatures that survived the demon war. Dumaros, Wendigos - things that should never see daylight again."));

				var response = await dialog.Select(L("The magical detection device that monitors threats from the underground cells is critical for Orsha's defense. But it hasn't been tested in months. I need someone to verify it's working properly."),
					Option(L("I'll help test the device"), "help"),
					Option(L("What does the device detect?"), "device"),
					Option(L("Sounds dangerous"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He nods approvingly*{/}"));
						await dialog.Msg(L("Good. I expected nothing less from someone who assists Orsha."));
						await dialog.Msg(L("I've prepared monster bait designed to lure prison creatures to the surface. Deploy it at three designated points near the detection device."));
						await dialog.Msg(L("{#666666}*He hands you a sealed package*{/}"));
						await dialog.Msg(L("When the bait attracts Dumaros and Wendigos, the device should alert us. That's how we'll know it's functioning."));

						if (await dialog.YesNo(L("Will you test the detection device for Orsha's security?")))
						{
							character.Quests.Start(questId);
							character.Inventory.Add(668008, 1, InventoryAddType.PickUp);
							await dialog.Msg(L("Deploy the bait at the detection device three times. The creatures that emerge will be dangerous - Blue Dumaros and Blue Wendigos."));
							await dialog.Msg(L("Eliminate any threats that surface, then report back to me with the results."));
						}
						break;

					case "device":
						await dialog.Msg(L("{#666666}*He gestures to the glowing apparatus*{/}"));
						await dialog.Msg(L("This magical device was created by Orsha's war mages specifically to monitor the underground prison."));
						await dialog.Msg(L("It detects movement, magical signatures, and escape attempts from the cells below. When something threatens to break free, it alerts the garrison."));
						await dialog.Msg(L("Without it functioning properly, we're blind to what's happening in those depths. And that is unacceptable."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He returns to inspecting the device*{/}"));
						await dialog.Msg(L("Then step aside. Orsha's defense cannot wait for the hesitant."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var testCount = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_11_re.Quest1005.DeviceTests", 0);

				if (testCount >= 3)
				{
					await dialog.Msg(L("{#666666}*He reviews notes on a tactical parchment as you approach*{/}"));
					await dialog.Msg(L("The device registered all three incursions. Blue Dumaros, Blue Wendigos - exactly as predicted."));
					await dialog.Msg(L("More importantly, the detection alerts triggered immediately when the bait drew them near. The system is fully operational."));
					await dialog.Msg(L("{#666666}*He nods with satisfaction*{/}"));
					await dialog.Msg(L("Excellent work. You've confirmed that Orsha's early warning system for the prison remains intact."));
					await dialog.Msg(L("This is critical intelligence for our city's defense. Here - you've earned proper compensation."));
					await dialog.Msg(L("{#666666}*He hands you a military-issue sword*{/}"));
					await dialog.Msg(L("A soldier's weapon, for someone who serves Orsha's interests well."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("The device has registered {0} tests so far. You need to complete all three deployment points.", testCount));
					await dialog.Msg(L("Use the bait at the detection device. Be prepared to fight whatever emerges from below."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*He monitors the detection device's readouts with professional vigilance*{/}"));
				await dialog.Msg(L("The device continues to function perfectly. Orsha's prison remains secure. Your assistance was invaluable."));
			}
		});

		// Magical Detection Device
		AddNpc(153137, L("Magical Detection Device"), "f_siauliai_11_re", 223, 1545, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1005);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A complex magical apparatus hums with arcane energy, monitoring the prison depths below*{/}"));
				return;
			}

			if (!character.Inventory.HasItem(668008))
			{
				await dialog.Msg(L("{#666666}*The device awaits deployment of the monster bait*{/}"));
				await dialog.Msg(L("{#666666}*You need the bait from Commander Uska to proceed*{/}"));
				return;
			}

			var testCount = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_11_re.Quest1005.DeviceTests", 0);
			var spawnedKey = $"Laima.Quests.f_siauliai_11_re.Quest1005.Test{testCount + 1}.Spawned";

			if (testCount >= 3)
			{
				await dialog.Msg(L("{#666666}*The device has completed all three detection tests. Return to Commander Uska.*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(
				L("Deploying monster bait..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
			);

			if (result == TimeActionResult.Completed)
			{
				testCount++;
				character.Variables.Perm.Set("Laima.Quests.f_siauliai_11_re.Quest1005.DeviceTests", testCount);

				await dialog.Msg(L("{#666666}*The bait releases its scent. The detection device's runes flare brightly!*{/}"));
				character.ServerMessage(LF("Detection tests completed: {0}/3", testCount));

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (
						SpawnTempMonsters(character, MonsterId.Dumaro_Blue, 3, 200, TimeSpan.FromMinutes(2)) &&
						SpawnTempMonsters(character, MonsterId.Wendigo_Blue, 3, 200, TimeSpan.FromMinutes(2))
						)
					{
						await dialog.Msg(L("{#666666}*Creatures emerge from the prison depths!*{/}"));

						// Play alerting effect
						for (var i = 0; i < 3; i++)
						{
							if (character.Connection != null)
								Send.ZC_NORMAL.PlayEffect(character.Connection, dialog.Npc, animationName: "F_lineup017_red3", scale: 5);
							if (i < 2) Task.Delay(1500);
						}
					}

					character.ServerMessage(L("{#FF6666}The device glows! Prison creatures detected!{/}"));
				}

				if (testCount >= 3)
				{
					character.Quests.CompleteObjective(questId, "testDevice");
					character.ServerMessage(L("{#FFD700}All detection tests complete! Return to Commander Uska.{/}"));
				}
			}
			else
			{
				character.ServerMessage(L("Bait deployment cancelled."));
			}
		});

		// =====================================================================
		// QUEST 1006: A Moment of Peace
		// =====================================================================
		// Hermit Settler - Secluded Grove
		//---------------------------------------------------------------------
		AddNpc(20165, L("[Hermit] Lukas"), "f_siauliai_11_re", -1616, 1403, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1006);

			dialog.SetTitle(L("Lukas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A peaceful hermit sits cross-legged in a secluded grove, completely relaxed*{/}"));
				await dialog.Msg(L("{#666666}*He opens his eyes slowly as you approach, a serene smile on his face*{/}"));
				await dialog.Msg(L("Hey there, friend. Beautiful day, isn't it?"));
				await dialog.Msg(L("The rain, the quiet... it's perfect. Just perfect."));

				var response = await dialog.Select(L("You know, people are always rushing around. Fighting, questing, chasing rewards. But sometimes... sometimes you just need to sit. Breathe. Be."),
					Option(L("That sounds nice"), "help"),
					Option(L("What brought you here?"), "about"),
					Option(L("How do you find inner peace?"), "peace"),
					Option(L("Any advice for staying calm?"), "advice"),
					Option(L("I should go"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*His smile widens*{/}"));
						await dialog.Msg(L("Right? Just... nice."));
						await dialog.Msg(L("You want to join me? No agenda, no tasks. Just sit here with me for a bit."));
						await dialog.Msg(L("Listen to the rain. Feel the positive energy of this grove. Let yourself relax."));

						if (await dialog.YesNo(L("Want to sit with me for a moment?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Awesome. Just come back and talk to me when you're ready."));
							await dialog.Msg(L("No rush. Take your time."));
						}
						break;

					case "about":
						await dialog.Msg(L("{#666666}*He gazes peacefully at the rain*{/}"));
						await dialog.Msg(L("After the war... I just couldn't do it anymore. The fighting, the stress."));
						await dialog.Msg(L("I needed to find balance. Inner peace. Positive energy."));
						await dialog.Msg(L("So I came here. This grove accepted me. Taught me to slow down."));
						await dialog.Msg(L("Best decision I ever made."));
						break;

					case "peace":
						await dialog.Msg(L("{#666666}*He closes his eyes for a moment, breathing deeply*{/}"));
						await dialog.Msg(L("Inner peace isn't something you find. It's something you allow."));
						await dialog.Msg(L("We spend so much time fighting - fighting monsters, fighting fate, fighting ourselves."));
						await dialog.Msg(L("But peace? Peace comes when you stop fighting. When you accept the moment as it is."));
						await dialog.Msg(L("The rain falls. I don't fight it. I don't wish for sunshine. I just... let it be rain."));
						await dialog.Msg(L("That's the secret, friend. Stop resisting what is. Just be present."));
						break;

					case "advice":
						await dialog.Msg(L("{#666666}*He gestures to the grove around him*{/}"));
						await dialog.Msg(L("Nature is the best teacher. Watch the trees - they don't stress about tomorrow's weather."));
						await dialog.Msg(L("Listen to the rain - it doesn't hurry, doesn't force. It just flows."));
						await dialog.Msg(L("When you feel overwhelmed, find a quiet spot. Sit down. Close your eyes."));
						await dialog.Msg(L("Breathe in for four counts. Hold for four. Breathe out for four. Hold for four."));
						await dialog.Msg(L("Do that a few times. Feel your heartbeat slow. Your mind clear."));
						await dialog.Msg(L("{#666666}*He smiles warmly*{/}"));
						await dialog.Msg(L("It's simple, but simple works. The world makes things complicated. Peace is simple."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He nods peacefully*{/}"));
						await dialog.Msg(L("All good. May your path be peaceful, friend."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*He opens his eyes as you approach, smiling warmly*{/}"));
				await dialog.Msg(L("Hey there. How did it feel?"));
				await dialog.Msg(L("Did you find your inner peace moment?"));
				await dialog.Msg(L("{#666666}*He doesn't wait for an answer, just nods knowingly*{/}"));
				await dialog.Msg(L("I can see it in your eyes. You get it now."));
				await dialog.Msg(L("That's all it takes - just a moment of being present. Of letting go."));
				await dialog.Msg(L("{#666666}*He hands you something with a gentle smile*{/}"));
				await dialog.Msg(L("Thanks for sharing this time with me, friend. You're always welcome back here."));

				character.Quests.Complete(questId);
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*He's returned to his meditation, completely at peace*{/}"));
				await dialog.Msg(L("Welcome back. The grove is always open to you."));
			}
		});

	}

	// Helper method for log pile investigation points
	void AddLogPile(int pileNum, int x, int z, int direction)
	{
		AddNpc(151031, L("Log Pile"), "f_siauliai_11_re", x, z, direction, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1002);
			var variableKey = $"Laima.Quests.f_siauliai_11_re.Quest1002.LogPile{pileNum}";

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A stack of freshly cut timber logs*{/}"));
				return;
			}

			if (character.Variables.Perm.GetBool(variableKey, false))
			{
				await dialog.Msg(L("{#666666}*You've already examined this log pile*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(
				L("Examining log pile..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
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
						await dialog.Msg(L("{#666666}*You notice deep drag marks in the muddy ground leading away from the pile*{/}"));
						await dialog.Msg(L("{#666666}*The marks suggest something heavy was pulled, not carried*{/}"));
						break;
					case 2:
						await dialog.Msg(L("{#666666}*Fresh scratches mark the sides of several logs*{/}"));
						await dialog.Msg(L("{#666666}*The scratches look almost like... claw marks? But oddly plant-like*{/}"));
						break;
					case 3:
						await dialog.Msg(L("{#666666}*Scattered wood chips and sawdust form a trail leading into the forest*{/}"));
						await dialog.Msg(L("{#666666}*You notice some broken twigs and disturbed vegetation nearby*{/}"));
						break;
				}

				character.ServerMessage(LF("Log piles examined: {0}/3", inspectedCount));

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
							await dialog.Msg(L("{#666666}*Suddenly, three Woodin emerge from the forest, clutching pieces of timber!*{/}"));
							character.ServerMessage(L("{#FF6666}Woodin caught stealing timber!{/}"));
							character.Quests.CompleteObjective(questId, "investigateLogPiles");
						}
					}
				}
			}
			else
			{
				character.ServerMessage(L("Examination cancelled."));
			}
		});
	}

	// Helper method for Moonlight Flower collection spots
	void AddMoonlightFlower(int flowerNum, int x, int z, int direction)
	{
		AddNpc(156040, L("Blooming Moonlight Flower"), "f_siauliai_11_re", x, z, direction, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1003);
			var variableKey = $"Laima.Quests.f_siauliai_11_re.Quest1003.Flower{flowerNum}";

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A delicate silver-white flower blooms in the rain*{/}"));
				return;
			}

			if (character.Variables.Perm.GetBool(variableKey, false))
			{
				await dialog.Msg(L("{#666666}*You've already collected a flower from this spot*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(
				L("Carefully harvesting Moonlight Flower..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
			);

			if (result == TimeActionResult.Completed)
			{
				character.Inventory.Add(666062, 1, InventoryAddType.PickUp);
				character.Variables.Perm.Set(variableKey, true);
				character.ServerMessage(L("Harvested: Moonlight Flower"));

				var currentCount = character.Inventory.CountItem(666062);
				character.ServerMessage(LF("Moonlight Flowers collected: {0}/5", currentCount));

				if (currentCount >= 5)
				{
					character.ServerMessage(L("{#FFD700}All Moonlight Flowers collected! Return to Herbalist Ruta.{/}"));
				}
			}
			else
			{
				character.ServerMessage(L("Harvest cancelled."));
			}
		});
	}

	// Helper method for purification crystals
	void AddPurificationCrystal(int crystalNum, int x, int z, int direction)
	{
		AddNpc(46218, L("Purification Crystal"), "f_siauliai_11_re", x, z, direction, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_11_re", 1004);
			var variableKey = $"Laima.Quests.f_siauliai_11_re.Quest1004.Marker{crystalNum}";

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A purification crystal hums with faint magical energy*{/}"));
				return;
			}

			if (character.Variables.Perm.GetBool(variableKey, false))
			{
				await dialog.Msg(L("{#666666}*You've already inspected this purification crystal*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(
				L("Inspecting purification crystal..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
			);

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(variableKey, true);

				await dialog.Msg(L("{#666666}*The crystal pulses softly - showing only mild traces of corruption*{/}"));
				character.ServerMessage(LF("Purification Crystal {0} inspected: mild corruption traces", crystalNum));

				var crystalCount = 0;
				for (int i = 1; i <= 3; i++)
				{
					if (character.Variables.Perm.GetBool($"Laima.Quests.f_siauliai_11_re.Quest1004.Marker{i}", false))
						crystalCount++;
				}

				character.ServerMessage(LF("Purification crystals inspected: {0}/3", crystalCount));

				if (crystalCount >= 3)
				{
					character.Quests.CompleteObjective(questId, "inspectCrystals");
					character.ServerMessage(L("All crystals inspected. Return to Scout Daina."));
				}
			}
			else
			{
				character.ServerMessage(L("Inspection cancelled."));
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
		SetName(L("Crossing Guard Duty"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Guard Borin clear aggressive Popolions from the Uninhabited Crossing so he can properly watch for threats."));
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Guard] Borin"), "f_siauliai_11_re");
		// Objectives
		AddObjective("killPopolion", L("Clear Popolion from the crossing"),
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
		SetName(L("The Timber Thief Mystery"));
		SetType(QuestType.Sub);
		SetDescription(L("Investigate who or what is stealing timber from Lumberjack Kaspar's work area by examining log piles, then deal with the culprits."));
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential); // Sequential
		AddQuestGiver(L("[Lumberjack] Kaspar"), "f_siauliai_11_re");
		// Objectives
		AddObjective("investigateLogPiles", L("Examine the three log piles for clues"),
			new ManualObjective());
		AddObjective("killWoodin", L("Hunt Woodin"),
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
		SetName(L("Herbal Rain Harvest"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Herbalist Ruta gather rare Moonlight Flowers that bloom during Paupys Crossing's endless rain."));
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Ruta"), "f_siauliai_11_re");
		// Objectives
		AddObjective("collectFlowers", L("Gather Moonlight Flowers from five locations"),
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
		SetName(L("Corruption Watch"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Scout Daina inspect the three purification crystals at Deer Hooves Lot to check for traces of corruption from Koru Jungle."));
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout] Daina"), "f_siauliai_11_re");
		// Objectives
		AddObjective("inspectCrystals", L("Inspect three purification crystals at Deer Hooves Lot"),
			new ManualObjective());
		AddObjective("killPopolion", L("Clear Popolion"),
			new KillObjective(10, new[] { MonsterId.Sec_Popolion_Blue }));
		AddObjective("killHanaming", L("Clear Hanaming"),
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
		SetName(L("Detection Device Test"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Commander Uska test the magical detection device that monitors threats from Ashaq Underground Prison by deploying monster bait to attract prison creatures."));
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Commander] Uska"), "f_siauliai_11_re");
		// Objectives
		AddObjective("testDevice", L("Deploy monster bait at the detection device three times"),
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
		SetName(L("A Moment of Peace"));
		SetType(QuestType.Sub);
		SetDescription(L("Join Hermit Lukas for a peaceful moment of meditation and relaxation in his secluded grove."));
		SetLocation("f_siauliai_11_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hermit] Lukas"), "f_siauliai_11_re");
		// Objectives
		AddObjective("sitWithLukas", L("Sit peacefully with Lukas"),
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
