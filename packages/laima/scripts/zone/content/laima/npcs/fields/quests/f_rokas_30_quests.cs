//--- Melia Script ----------------------------------------------------------
// King's Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs for the Fedimian Grand Archive expedition studying the
// magical barrier around the Mage's Tower.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;
using Yggdrasil.Util;

public class KingsPlateauQuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Shadows of the Plateau - Expedition Leader Brennan
		//---------------------------------------------------------------------
		AddNpc(152058, "[Expedition Leader] Brennan", "f_rokas_30", 893, -871, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_30", 1001);

			dialog.SetTitle("Brennan");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A scholarly man in worn traveling robes looks up from a collection of scattered notes*{/}");
				await dialog.Msg("Ah, a traveler! Welcome to King's Plateau. I am Brennan, leader of this research expedition from the Fedimian Grand Archive.");
				await dialog.Msg("You may have noticed the perpetual darkness here. It's quite unsettling at first, but I assure you it's perfectly... well, mostly safe.");

				var response = await dialog.Select("We've camped here to study the magical barrier surrounding the Mage's Tower. Do you see it? That faint shimmer in the distance?",
					Option("Tell me about this barrier", "barrier"),
					Option("Why does Fedimian care about this?", "fedimian"),
					Option("I'm just passing through", "leave")
				);

				switch (response)
				{
					case "barrier":
						await dialog.Msg("The barrier was erected centuries ago by the tower's original inhabitants - powerful mages who sealed themselves inside.");
						await dialog.Msg("It warps light itself, creating this eternal night. Our instruments detect massive magical fluctuations emanating from within.");
						await dialog.Msg("{#666666}*He gestures toward the dark horizon*{/}");
						await dialog.Msg("We've established research camps across the plateau, but our lead investigator, Magister Corvinus, has made the most progress.");
						await dialog.Msg("He's stationed at Sviesa Altar - an ancient site with a direct view of the tower entrance.");

						if (await dialog.YesNo("I need someone to deliver this research document to him. The data is time-sensitive and the Hogma have made travel dangerous. Would you help?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Excellent! Magister Corvinus will be expecting these. He's brilliant but... intense. Don't be put off by his manner.");
							await dialog.Msg("You'll find him at Sviesa Altar, on the far side of the plateau. The altar predates even the Mage's Tower - Corvinus believes they're connected.");
							await dialog.Msg("Be careful of the Hogma tribes. They've grown aggressive since we arrived.");

							character.Inventory.Add(650531, 1, InventoryAddType.PickUp);
							character.ServerMessage("Received: Research Document");
						}
						break;

					case "fedimian":
						await dialog.Msg("The Grand Archive seeks to understand all magical phenomena. This barrier represents unprecedented magical engineering.");
						await dialog.Msg("If we can decipher how it works, we might learn to create protective wards for our cities - or find ways to dispel demonic corruption.");
						await dialog.Msg("The war left scars that normal magic cannot heal. Ancient knowledge like this could be the key to true restoration.");
						await dialog.Msg("Magister Corvinus believes the tower holds secrets that could benefit all three cities. That's why we're here.");
						break;

					case "leave":
						await dialog.Msg("Safe travels, then. But if you change your mind, our expedition could use capable hands.");
						await dialog.Msg("And do speak with Magister Corvinus if you venture toward Sviesa Altar. He's the most knowledgeable person here about the tower's mysteries.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				await dialog.Msg("Have you delivered the notes to Magister Corvinus yet? He's waiting at Sviesa Altar.");
				await dialog.Msg("The altar is on the far side of the plateau. Be wary of Hogma patrols.");
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Thank you for helping us. Magister Corvinus speaks highly of your capabilities.");
				await dialog.Msg("The expedition's work continues. If you wish to assist further, speak with my colleagues across the plateau.");
			}
		});

		// Quest 1002: Plateau Patrol - Expedition Guard Senna
		//---------------------------------------------------------------------
		AddNpc(147415, "[Expedition Guard] Senna", "f_rokas_30", 1422, 564, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_30", 1002);

			dialog.SetTitle("Senna");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A Fedimian guard stands alert, her hand resting on her weapon*{/}");
				await dialog.Msg("Hold. You don't look like Hogma, so I won't attack first. I'm Senna, assigned to protect this expedition.");
				await dialog.Msg("{#666666}*She scans the darkness around the camp*{/}");
				await dialog.Msg("These Hogma tribes have been a constant threat since we arrived. They seem drawn to the barrier's magic.");

				var response = await dialog.Select("The Hogma Shamans are the worst. They channel some kind of dark fire magic that our scholars can barely defend against.",
					Option("I can help thin their numbers", "help"),
					Option("Why are the Hogma here?", "info"),
					Option("Not my problem", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("You'd fight them? Good. We're stretched thin protecting multiple research sites.");

						if (await dialog.YesNo("The Hogma Scouts coordinate their raids, and the Shamans provide magical support. Eliminate both to break their offensive. Will you do this?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you. The Hogma are spread across the central plateau. You'll find plenty of both Scouts and Shamans.");
							await dialog.Msg("The Shamans are more dangerous - they wield fire magic and seem to draw power from the ancient sites here.");
							await dialog.Msg("Return when you've culled their numbers. The expedition will be in your debt.");
						}
						break;

					case "info":
						await dialog.Msg("The Hogma are widlings - tribal warriors who've inhabited these rocky regions for generations.");
						await dialog.Msg("But something's changed. They've become more aggressive, more organized. Our scholars think the barrier's magic is affecting them.");
						await dialog.Msg("The Shamans especially seem drawn to the ancient monuments. They perform rituals there that make our researchers nervous.");
						await dialog.Msg("Whatever the reason, they attack our camps almost nightly. We need to push them back.");
						break;

					case "leave":
						await dialog.Msg("Suit yourself. But if you're staying on this plateau, you'll have to deal with them eventually.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killScouts", out var scoutObj)) return;
				if (!quest.TryGetProgress("killShamans", out var shamanObj)) return;

				if (scoutObj.Done && shamanObj.Done)
				{
					await dialog.Msg("You've done it! I can already see the difference - fewer Hogma scouts on the horizon.");
					await dialog.Msg("{#666666}*She relaxes slightly for the first time*{/}");
					await dialog.Msg("The expedition can work more safely now. Our scholars can focus on research instead of watching for attacks.");
					await dialog.Msg("You've proven yourself a capable warrior. Take this reward - you've earned it.");

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!scoutObj.Done)
						status += "Hogma Scouts still patrol the plateau. ";
					if (!shamanObj.Done)
						status += "Hogma Shamans continue their rituals at the ancient sites.";

					await dialog.Msg(status);
					await dialog.Msg("Keep hunting. The expedition is counting on you.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The Hogma have pulled back since your attack. Our researchers can work without constant fear.");
				await dialog.Msg("You've done the expedition a great service.");
			}
		});

		// Quest 1003: Echoes in the Jars - Archaeologist Marius
		//---------------------------------------------------------------------
		var jarQuestId = new QuestId("f_rokas_30", 1003);

		AddNpc(152059, "[Archaeologist] Marius", "f_rokas_30", 242, 524, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle("Marius");

			if (!character.Quests.Has(jarQuestId))
			{
				await dialog.Msg("{#666666}*A scholar kneels among a collection of ancient ceramic jars, tapping them with a small mallet*{/}");
				await dialog.Msg("Shh! Listen... do you hear that resonance? These jars are extraordinary!");
				await dialog.Msg("{#666666}*He stands, brushing dust from his robes*{/}");
				await dialog.Msg("I am Marius, archaeological specialist for the expedition. These ceremonial vessels were used by the tower's builders for magical communication.");

				var response = await dialog.Select("Each jar produces a different harmonic frequency when activated. I believe the correct sequence will reveal hidden inscriptions!",
					Option("How can I help?", "help"),
					Option("What kind of inscriptions?", "info"),
					Option("Sounds complicated", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("Wonderful! I've identified three distinct jar types, but I need someone to help activate them in sequence.");

						if (await dialog.YesNo("The jars are scattered around this area. Activate them in the correct order: Small, Large, Tall, Small, Large, Tall. Will you try?"))
						{
							character.Quests.Start(jarQuestId);
							await dialog.Msg("Excellent! The jars have distinct shapes - small narrow ones, large wide ones, and tall ceremonial ones.");
							await dialog.Msg("Activate them in this sequence: Small, Large, Tall, then repeat - Small, Large, Tall.");
							await dialog.Msg("If you make a mistake, the resonance will fade and you'll need to start over. Return to me when you've completed the sequence!");
							await dialog.Msg("Magister Corvinus will be thrilled if we can decipher these inscriptions. He believes they hold clues to entering the tower.");

							character.Variables.Perm.Set("Laima.Quests.f_rokas_30.Quest1003.JarSequence", 0);
						}
						break;

					case "info":
						await dialog.Msg("The tower's original builders left messages encoded in magical harmonics. When the correct sequence plays, the magic reveals hidden text.");
						await dialog.Msg("I've found fragments suggesting these inscriptions describe the barrier's construction - perhaps even how to bypass it.");
						await dialog.Msg("Magister Corvinus at Sviesa Altar is particularly interested in anything related to entering the tower. This could be the breakthrough we need.");
						break;

					case "leave":
						await dialog.Msg("It IS complicated. But that's what makes it fascinating! The ancient mages were true geniuses.");
						break;
				}
			}
			else if (character.Quests.IsActive(jarQuestId))
			{
				var sequenceComplete = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_30.Quest1003.SequenceComplete", 0) >= 1;

				if (sequenceComplete)
				{
					await dialog.Msg("You've done it! The harmonic sequence revealed the ancient inscriptions!");
					await dialog.Msg("{#666666}*He excitedly examines the glowing text that appeared on the jars*{/}");
					await dialog.Msg("Remarkable! These describe a 'Key of Passage' - magical fragments that can unlock the tower's entrance!");
					await dialog.Msg("This information is invaluable! Magister Corvinus must see this immediately. I'll prepare a full report.");
					await dialog.Msg("You have a scholar's patience and a musician's ear. Please, accept this token of the Archive's gratitude.");

					character.Quests.Complete(jarQuestId);
				}
				else
				{
					var currentStep = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_30.Quest1003.JarSequence", 0);
					await dialog.Msg($"Remember the sequence: Small, Large, Tall, Small, Large, Tall.");
					if (currentStep > 0)
					{
						await dialog.Msg($"You've activated {currentStep} jar(s) correctly so far. Keep going!");
					}
					else
					{
						await dialog.Msg("Find the jars scattered around this area and activate them in order.");
					}
				}
			}
			else if (character.Quests.HasCompleted(jarQuestId))
			{
				await dialog.Msg("The inscriptions you revealed have been transcribed and sent to the Grand Archive.");
				await dialog.Msg("Magister Corvinus was particularly interested in the 'Key of Passage' references. Speak with him if you haven't already.");
			}
		});

		// Jar sequence data
		// jarType: 1 = Small (47257), 2 = Large (47258), 3 = Tall (47256)
		// Correct sequence: Small(1), Large(2), Tall(3), Small(1), Large(2), Tall(3)
		int[] correctSequence = { 1, 2, 3, 1, 2, 3 };

		// Track activated jars per character (characterId -> list of activated jar NPCs)
		var activatedJarsPerPlayer = new Dictionary<long, List<Npc>>();

		// Helper to play looping effects on activated jars
		async Task PlayJarEffectsLoop(Character character, List<Npc> activatedJars, int expectedCount)
		{
			while (true)
			{
				var currentCount = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_30.Quest1003.JarSequence", 0);

				// Stop if sequence was reset or quest completed/cancelled
				if (currentCount < expectedCount || !character.Quests.IsActive(jarQuestId))
					break;

				// Play effect on all activated jars
				foreach (var jar in activatedJars)
				{
					Send.ZC_NORMAL.PlayEffect(character.Connection, jar, animationName: "F_spread_in012_blue", scale: 0.5f);
				}

				await Task.Delay(TimeSpan.FromSeconds(3));
			}
		}

		// Jar Helper Method
		void AddJar(int jarType, int modelId, string jarName, int x, int z, int direction)
		{
			AddNpc(modelId, jarName, "f_rokas_30", x, z, direction, async dialog =>
			{
				var character = dialog.Player;

				if (!character.Quests.IsActive(jarQuestId))
				{
					await dialog.Msg("{#666666}*An ancient ceremonial jar. It seems to hum faintly with residual magic.*{/}");
					return;
				}

				var sequenceComplete = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_30.Quest1003.SequenceComplete", 0) >= 1;
				if (sequenceComplete)
				{
					await dialog.Msg("{#666666}*The jar glows softly, its magic already activated.*{/}");
					return;
				}

				var currentStep = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_30.Quest1003.JarSequence", 0);
				var expectedType = correctSequence[currentStep];

				var result = await character.TimeActions.StartAsync(
					"Activating jar...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(2)
				);

				if (result == TimeActionResult.Completed)
				{
					if (jarType == expectedType)
					{
						currentStep++;
						character.Variables.Perm.Set("Laima.Quests.f_rokas_30.Quest1003.JarSequence", currentStep);

						// Get or create activated jars list for this player
						if (!activatedJarsPerPlayer.TryGetValue(character.ObjectId, out var activatedJars))
						{
							activatedJars = new List<Npc>();
							activatedJarsPerPlayer[character.ObjectId] = activatedJars;
						}

						// Add this jar to activated list
						activatedJars.Add(dialog.Npc);

						// Play effect on this jar immediately
						Send.ZC_NORMAL.PlayEffect(character.Connection, dialog.Npc, animationName: "F_spread_in012_blue");

						// Start looping effect task for all activated jars
						_ = PlayJarEffectsLoop(character, activatedJars, currentStep);

						if (currentStep >= 6)
						{
							character.Variables.Perm.Set("Laima.Quests.f_rokas_30.Quest1003.SequenceComplete", 1);
							character.ServerMessage("{#FFD700}The jars resonate in perfect harmony! Ancient inscriptions glow with magical light!{/}");
							character.ServerMessage("{#FFD700}Return to Archaeologist Marius!{/}");
						}
						else
						{
							string[] stepNames = { "Small", "Large", "Tall", "Small", "Large", "Tall" };
							character.ServerMessage($"Correct! Step {currentStep}/6 complete. Next: {stepNames[currentStep]} jar.");
						}
					}
					else
					{
						// Reset sequence - effects will stop on next loop iteration
						character.Variables.Perm.Set("Laima.Quests.f_rokas_30.Quest1003.JarSequence", 0);

						// Clear activated jars for this player
						if (activatedJarsPerPlayer.ContainsKey(character.ObjectId))
							activatedJarsPerPlayer[character.ObjectId].Clear();

						character.ServerMessage("{#FF6666}Wrong jar! The resonance fades... Sequence reset.{/}");
						character.ServerMessage("Remember: Small, Large, Tall, Small, Large, Tall.");
					}
				}
			});
		}

		// Add jars around Marius (jarType, modelId, name, x, z, direction)
		// Small=47257, Large=47258, Tall=47256
		AddJar(1, 47257, "Small Jar", 181, 551, 90);     // Small
		AddJar(2, 47258, "Large Jar", 188, 509, 90);     // Large
		AddJar(3, 47256, "Tall Jar", 148, 540, 90);      // Tall
		AddJar(1, 47257, "Small Jar", 238, 565, 90);     // Small
		AddJar(2, 47258, "Large Jar", 277, 534, 89);     // Large
		AddJar(3, 47256, "Tall Jar", 298, 612, 90);      // Tall

		// Quest 1004: Unlocking the Tower - Magister Corvinus (SPECIAL NPC)
		//---------------------------------------------------------------------
		var towerQuestId = new QuestId("f_rokas_30", 1004);

		AddNpc(152060, "[Magister] Corvinus", "f_rokas_30", -1435, -866, 89, async dialog =>
		{
			var character = dialog.Player;
			var deliveryQuestId = new QuestId("f_rokas_30", 1001);

			dialog.SetTitle("Corvinus");

			// Handle delivery from Quest 1001
			if (character.Quests.IsActive(deliveryQuestId) && character.Inventory.HasItem(650531))
			{
				await dialog.Msg("{#666666}*A stern-faced scholar in elaborate robes looks up from ancient texts*{/}");
				await dialog.Msg("You have something for me? Ah, Brennan's research document. Finally.");
				await dialog.Msg("{#666666}*He takes the notes and scans them quickly*{/}");
				await dialog.Msg("Hmm. Brennan's measurements confirm my theories. The barrier fluctuates in predictable patterns.");
				await dialog.Msg("You've done well bringing these. I am Magister Corvinus, lead investigator for this expedition.");

				character.Inventory.Remove(650531, 1, InventoryItemRemoveMsg.Given);
				character.Quests.CompleteObjective(deliveryQuestId, "deliverNotes");
				character.Quests.Complete(deliveryQuestId);

				await dialog.Msg("If you're capable of navigating this plateau, perhaps you can assist me with something far more significant...");
				return;
			}

			if (!character.Quests.Has(towerQuestId))
			{
				await dialog.Msg("{#666666}*A stern-faced scholar stands before an ancient altar, studying the tower visible in the distance*{/}");
				await dialog.Msg("I am Magister Corvinus. You stand at Sviesa Altar - a sacred site that predates even the Mage's Tower.");
				await dialog.Msg("{#666666}*He gestures toward the shimmering barrier in the distance*{/}");
				await dialog.Msg("For months I've studied that barrier. It's a masterwork of ancient magic - and I've finally discovered how to breach it.");

				var response = await dialog.Select("The tower's entrance requires a 'Key of Passage' - magical fragments scattered when the barrier activated centuries ago.",
					Option("How can I help unlock it?", "help"),
					Option("Tell me about the tower", "tower"),
					Option("I have other priorities", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("Direct and practical. I appreciate that.");
						await dialog.Msg("The Key of Passage requires two components: organic material infused with the plateau's magic, and barrier energy from the ancient monuments.");

						if (await dialog.YesNo("The Hogma have lived here for generations - their tusks have absorbed trace amounts of magical residue. And the monuments scattered across the plateau still hold barrier energy. Will you gather both?"))
						{
							character.Quests.Start(towerQuestId);
							await dialog.Msg("Excellent. Collect Hogma Tusks from the warriors across the plateau - I'll need thirty to extract enough magical essence.");
							await dialog.Msg("The monuments scattered across the plateau still resonate with barrier energy. Examine five of them to extract Destroyed Barrier Pieces.");
							await dialog.Msg("Return when you have both components. I'll combine them into a functional Key of Passage.");
							await dialog.Msg("This will be a significant achievement - the first entry into the Mage's Tower in centuries.");
						}
						break;

					case "tower":
						await dialog.Msg("The Mage's Tower was built by an order of archmages who sought to push the boundaries of magical knowledge.");
						await dialog.Msg("When the demon war began, they sealed themselves inside rather than fight. The barrier has held ever since.");
						await dialog.Msg("What knowledge lies within? What power? The Grand Archive has theorized for generations.");
						await dialog.Msg("Some say the mages still live, preserved by their magic. Others say only their research remains - and their guardians.");
						await dialog.Msg("Either way, what's inside could change everything we know about magic.");
						break;

					case "leave":
						await dialog.Msg("A pity. The secrets of the tower will remain hidden a while longer.");
						await dialog.Msg("But if you reconsider, I'll be here. This research is my life's work.");
						break;
				}
			}
			else if (character.Quests.IsActive(towerQuestId))
			{
				if (!character.Quests.TryGetById(towerQuestId, out var quest)) return;
				if (!quest.TryGetProgress("collectCrystals", out var crystalObj)) return;
				if (!quest.TryGetProgress("collectBarrier", out var barrierObj)) return;

				if (crystalObj.Done && barrierObj.Done)
				{
					await dialog.Msg("You have everything? Let me see...");
					await dialog.Msg("{#666666}*He examines the tusks and barrier pieces with intense focus*{/}");
					await dialog.Msg("Yes... YES! The magical essence in these tusks is perfect. Combined with the barrier energy...");
					await dialog.Msg("{#666666}*Magical energy swirls as he works, fusing the components together*{/}");
					await dialog.Msg("It's done. The Mage's Tower is now accessible. You've achieved what scholars dreamed of for centuries.");
					await dialog.Msg("The tower entrance lies just beyond this altar. Use caution - the guardians within will not welcome intruders.");
					await dialog.Msg("You've earned this reward, and more importantly, you've earned the right to explore what lies within.");

					character.Inventory.Remove(645177, 30, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(towerQuestId);
					character.ServerMessage("{#FFD700}Mage's Tower 1F is now accessible!{/}");
				}
				else
				{
					var tuskCount = character.Inventory.CountItem(645177);
					var barrierCount = character.Inventory.CountItem(650704);

					await dialog.Msg($"Hogma Tusks: {tuskCount}/30. Barrier Pieces: {barrierCount}/5.");

					if (!crystalObj.Done)
						await dialog.Msg("The Hogma across the plateau carry the tusks I need. Hunt them.");
					if (!barrierObj.Done)
						await dialog.Msg("Ancient monuments still hold barrier energy. Find them and extract the residue.");
				}
			}
			else if (character.Quests.HasCompleted(towerQuestId))
			{
				await dialog.Msg("The tower awaits you. What you find inside... that's for you to discover.");
				await dialog.Msg("I'll remain here, documenting everything. The Grand Archive will want a complete record of this achievement.");
			}
		});

		// Monument Helper Method for Quest 1004
		void AddMonument(int monumentNumber, int x, int z, int direction)
		{
			AddNpc(147414, "Ancient Monument", "f_rokas_30", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var variableKey = $"Laima.Quests.f_rokas_30.Quest1004.Monument{monumentNumber}";
				var spawnedKey = $"Laima.Quests.f_rokas_30.Quest1004.Monument{monumentNumber}.Spawned";

				if (!character.Quests.IsActive(towerQuestId))
				{
					await dialog.Msg("{#666666}*An ancient stone obelisk covered in worn inscriptions. It hums faintly with residual magic.*{/}");
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg("{#666666}*You've already extracted the barrier energy from this monument.*{/}");
					return;
				}

				// Chance to spawn Hogma when interacting
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 30)
				{
					character.Variables.Perm.Set(spawnedKey, true);
					SpawnTempMonsters(character, MonsterId.Hogma_Guard, 2, 70, TimeSpan.FromMinutes(1));
					SpawnTempMonsters(character, MonsterId.Hogma_Sorcerer, 1, 70, TimeSpan.FromMinutes(1));
					character.ServerMessage("{#FF6666}Hogma guardians emerge to protect the monument!{/}");
				}

				var result = await character.TimeActions.StartAsync(
					"Extracting barrier energy...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650704, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage("Obtained: Destroyed Barrier Piece");

					var currentCount = character.Inventory.CountItem(650704);
					character.ServerMessage($"Barrier Pieces: {currentCount}/5");

					if (currentCount >= 5)
					{
						character.ServerMessage("{#FFD700}All barrier pieces collected!{/}");
					}
				}
			});
		}

		// Add monuments across the plateau (7 monuments, need 5)
		AddMonument(1, 1553, 379, 0);
		AddMonument(2, 1239, -130, 90);
		AddMonument(3, 323, 676, 90);
		AddMonument(4, 462, 1267, 0);
		AddMonument(5, -401, 490, 90);
		AddMonument(6, -584, -466, 90);
		AddMonument(7, -1498, -413, 90);

		// Quest 1005: The Precarious Crossing - Cartographer Varus
		//---------------------------------------------------------------------
		var bridgeQuestId = new QuestId("f_rokas_30", 1005);

		AddNpc(152061, "[Cartographer] Varus", "f_rokas_30", -1074, -152, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle("Varus");

			if (!character.Quests.Has(bridgeQuestId))
			{
				await dialog.Msg("{#666666}*A nervous scholar clutches a bundle of maps, glancing repeatedly toward a rickety bridge*{/}");
				await dialog.Msg("Oh! A traveler! Please, you must help me!");
				await dialog.Msg("I'm Varus, cartographer for the expedition. I crossed that bridge to map this side of the plateau, but now...");
				await dialog.Msg("{#666666}*He shudders*{/}");
				await dialog.Msg("Now I've noticed the ropes are frayed. The planks are loose. I'm CERTAIN it wasn't like that before!");

				var response = await dialog.Select("I can't cross back! But I can't stay here forever either! The expedition needs these maps!",
					Option("I'll investigate the bridge", "help"),
					Option("Maybe you're imagining things", "doubt"),
					Option("Good luck with that", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("You will? Oh, thank the goddesses!");

						if (await dialog.YesNo("Please examine the bridge carefully. If it's been sabotaged, I need to know. Will you investigate?"))
						{
							character.Quests.Start(bridgeQuestId);
							await dialog.Msg("Thank you! The bridge is nearby, hanging over the chasm.");
							await dialog.Msg("Look for signs of deliberate damage - examine the support posts carefully. There are four of them holding the bridge.");
							await dialog.Msg("And be careful! The Hogma have been lurking near the bridge lately...");
						}
						break;

					case "doubt":
						await dialog.Msg("Imagining?! I've crossed hundreds of bridges in my career! I know when something's wrong!");
						await dialog.Msg("{#666666}*He takes a deep breath*{/}");
						await dialog.Msg("Look, I admit I'm not brave. But I'm not stupid either. That bridge has been tampered with.");
						await dialog.Msg("Please, just take a look. If I'm wrong, I'll admit it. But if I'm right...");
						break;

					case "leave":
						await dialog.Msg("Wait! Please! I can't...");
						await dialog.Msg("{#666666}*He trails off, looking miserable*{/}");
						await dialog.Msg("Fine. I'll just... stay here. With the Hogma. In the dark. Forever.");
						break;
				}
			}
			else if (character.Quests.IsActive(bridgeQuestId))
			{
				if (!character.Quests.TryGetById(bridgeQuestId, out var quest)) return;
				if (!quest.TryGetProgress("investigatePosts", out var postsObj)) return;

				if (postsObj.Done)
				{
					await dialog.Msg("You've inspected all the support posts? What did you find?");
					await dialog.Msg("{#666666}*He listens intently as you describe the sabotage*{/}");
					await dialog.Msg("Hogma sabotage! I knew it! Those creatures have been watching the expedition...");
					await dialog.Msg("{#666666}*He practically collapses with relief*{/}");
					await dialog.Msg("But at least now we know. Thank you! I'll inform Brennan immediately.");
					await dialog.Msg("Please, take this reward. You've saved my life - and probably others too!");

					character.Quests.Complete(bridgeQuestId);
				}
				else
				{
					var inspectedCount = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_30.Quest1005.PostsInspected", 0);
					await dialog.Msg($"Have you examined the bridge support posts yet? You've inspected {inspectedCount}/4 so far.");
					await dialog.Msg("Check all four posts - two on each side of the bridge.");
				}
			}
			else if (character.Quests.HasCompleted(bridgeQuestId))
			{
				await dialog.Msg("I made it back safely, thanks to you. The expedition was worried when I didn't return on schedule.");
				await dialog.Msg("Brennan has posted guards at both ends of the bridge now. No more Hogma sabotage.");
				await dialog.Msg("My maps are complete - this plateau is now fully documented for the Grand Archive!");
			}
		});

		// Bridge Post Investigation Points
		void AddBridgePost(int postNum, int x, int z, int direction)
		{
			AddNpc(12080, "Bridge Post", "f_rokas_30", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var variableKey = $"Laima.Quests.f_rokas_30.Quest1005.Post{postNum}";

				if (!character.Quests.IsActive(bridgeQuestId))
				{
					await dialog.Msg("{#666666}*A wooden support post holding up the bridge. It looks weathered but sturdy.*{/}");
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg("{#666666}*You've already inspected this support post.*{/}");
					return;
				}

				var result = await character.TimeActions.StartAsync(
					"Inspecting bridge post...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_30.Quest1005.PostsInspected", 0);
					currentCount++;
					character.Variables.Perm.Set("Laima.Quests.f_rokas_30.Quest1005.PostsInspected", currentCount);

					await dialog.Msg("{#666666}*You examine the support post closely...*{/}");

					if (postNum == 1)
						await dialog.Msg("The ropes have been partially cut with crude blades. This was deliberate sabotage.");
					else if (postNum == 2)
						await dialog.Msg("Several bolts have been loosened. The wood shows fresh scratch marks from tools.");
					else if (postNum == 3)
						await dialog.Msg("Hogma footprints are visible in the dust around the base. They've been here recently.");
					else
						await dialog.Msg("A piece of Hogma cloth is caught on a splinter. Definitive evidence of their tampering.");

					character.ServerMessage($"Bridge posts inspected: {currentCount}/4");

					if (currentCount >= 4)
					{
						character.ServerMessage("{#FFD700}All posts inspected! Return to Varus.{/}");
					}
				}
			});
		}

		AddBridgePost(1, -979, -216, 135);
		AddBridgePost(2, -990, -269, 270);
		AddBridgePost(3, -823, -288, 270);
		AddBridgePost(4, -818, -232, 225);

		// Bridge Ambush Trigger Zone
		AddAreaTrigger("f_rokas_30", -900, -252, 20, async (args) =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.IsDead)
				return;

			// Only trigger ambush if player has the quest active
			if (!character.Quests.IsActive(bridgeQuestId))
				return;

			var ambushCountKey = "Laima.Quests.f_rokas_30.Quest1005.BridgeAmbushCount";
			var ambushCount = character.Variables.Perm.GetInt(ambushCountKey, 0);

			// Only trigger up to 2 times per player
			if (ambushCount >= 2)
				return;

			character.Variables.Perm.Set(ambushCountKey, ambushCount + 1);

			if (SpawnTempMonsters(character, MonsterId.Hogma_Guard, 2, 70, TimeSpan.FromMinutes(1)))
			{
				character.ServerMessage("{#FF6666}Hogma Scouts ambush you on the bridge!{/}");
			}
		});

		// Mage's Tower Portal - Quest Gated
		//---------------------------------------------------------------------
		AddNpc(147384, "Mage's Tower Entrance", "f_rokas_30", -1356, -866, 0, async dialog =>
		{
			var character = dialog.Player;
			var unlockQuestId = new QuestId("f_rokas_30", 1004);

			if (character.Quests.HasCompleted(unlockQuestId))
			{
				var targetPos = new Position(-2410, 0, -1512);
				if (ZoneServer.Instance.World.TryGetMap("d_firetower_41", out var targetMap))
				{
					targetPos.Y = targetMap.Ground.GetHeightAt(targetPos);
				}
				character.Warp("d_firetower_41", targetPos.X, targetPos.Y, targetPos.Z);
			}
			else
			{
				await dialog.Msg("{#666666}*An invisible barrier blocks your path. Powerful magic prevents entry to the tower.*{/}");
				await dialog.Msg("The ancient seal holds firm. Perhaps Magister Corvinus at Sviesa Altar knows how to bypass this barrier...");
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Shadows of the Plateau
//-----------------------------------------------------------------------------

public class ShadowsOfThePlateauQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_30", 1001);
		SetName("Shadows of the Plateau");
		SetDescription("Deliver Expedition Leader Brennan's research document to Magister Corvinus at Sviesa Altar.");
		SetLocation("f_rokas_30");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Expedition Leader] Brennan", "f_rokas_30");

		AddObjective("deliverNotes", "Deliver research document to Magister Corvinus at Sviesa Altar",
			new ManualObjective());

		// Rewards - Simple (1/10) Level 30
		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 8));  // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650531, character.Inventory.CountItem(650531),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650531, character.Inventory.CountItem(650531),
			InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Plateau Patrol
//-----------------------------------------------------------------------------

public class PlateauPatrolQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_30", 1002);
		SetName("Plateau Patrol");
		SetDescription("Eliminate Hogma Scouts and Shamans threatening the expedition camps on King's Plateau.");
		SetLocation("f_rokas_30");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Expedition Guard] Senna", "f_rokas_30");

		AddObjective("killScouts", "Hunt Hogma Scout",
			new KillObjective(18, new[] { MonsterId.Hogma_Guard }));
		AddObjective("killShamans", "Hunt Hogma Shaman",
			new KillObjective(12, new[] { MonsterId.Hogma_Sorcerer }));

		// Rewards - Moderate (5/10) Level 30
		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
		AddReward(new ItemReward(922064, 1));  // Recipe - Krag Rod
	}
}

// Quest 1003 CLASS: Echoes in the Jars
//-----------------------------------------------------------------------------

public class EchoesInTheJarsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_30", 1003);
		SetName("Echoes in the Jars");
		SetDescription("Activate ancient ceremonial jars in the correct sequence to reveal hidden inscriptions about the Mage's Tower.");
		SetLocation("f_rokas_30");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Archaeologist] Marius", "f_rokas_30");

		AddObjective("completeSequence", "Activate jars in sequence: Small, Large, Tall, Small, Large, Tall",
			new VariableCheckObjective("Laima.Quests.f_rokas_30.Quest1003.SequenceComplete", 1, true));

		// Rewards - Moderate (5/10) Level 30
		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5));  // Recovery Potion
		AddReward(new ItemReward(581120, 1));  // Intel Fedimian Chain
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_30.Quest1003.JarSequence");
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_30.Quest1003.SequenceComplete");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_30.Quest1003.JarSequence");
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_30.Quest1003.SequenceComplete");
	}
}

// Quest 1004 CLASS: Unlocking the Tower
//-----------------------------------------------------------------------------

public class UnlockingTheTowerQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_30", 1004);
		SetName("Unlocking the Tower");
		SetDescription("Gather Hogma Tusks and Destroyed Barrier Pieces from ancient monuments to create a Key of Passage for the Mage's Tower.");
		SetLocation("f_rokas_30");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Magister] Corvinus", "f_rokas_30");

		AddObjective("collectCrystals", "Collect Hogma Tusks",
			new CollectItemObjective(645177, 30)); // Hogma Tusk
		AddObjective("collectBarrier", "Collect Destroyed Barrier Pieces from ancient monuments",
			new CollectItemObjective(650704, 5));

		// Rewards - Complex (7/10) Level 30
		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(24000));
		AddReward(new ItemReward(640082, 2));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 11)); // Normal HP Potion
		AddReward(new ItemReward(640006, 13)); // Normal SP Potion
		AddReward(new ItemReward(640011, 6));  // Recovery Potion
		AddReward(new ItemReward(923016, 1));  // Recipe - Karsto Staff
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650704, character.Inventory.CountItem(650704),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 7; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_30.Quest1004.Monument{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_30.Quest1004.Monument{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650704, character.Inventory.CountItem(650704),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 7; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_30.Quest1004.Monument{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_30.Quest1004.Monument{i}.Spawned");
		}
	}
}

// Quest 1005 CLASS: The Precarious Crossing
//-----------------------------------------------------------------------------

public class ThePrecariousCrossingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_30", 1005);
		SetName("The Precarious Crossing");
		SetDescription("Investigate the sabotaged bridge for Cartographer Varus by examining all four support posts.");
		SetLocation("f_rokas_30");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Cartographer] Varus", "f_rokas_30");

		AddObjective("investigatePosts", "Inspect the bridge support posts",
			new VariableCheckObjective("Laima.Quests.f_rokas_30.Quest1005.PostsInspected", 4, true));

		// Rewards - Simple (3/10) Level 30
		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 8));  // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_30.Quest1005.Post{i}");
		}
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_30.Quest1005.PostsInspected");
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_30.Quest1005.BridgeAmbushCount");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_30.Quest1005.Post{i}");
		}
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_30.Quest1005.PostsInspected");
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_30.Quest1005.BridgeAmbushCount");
	}
}
