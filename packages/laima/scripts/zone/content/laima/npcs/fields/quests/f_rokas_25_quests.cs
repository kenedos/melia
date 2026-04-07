//--- Melia Script ----------------------------------------------------------
// Rokas Ridge - Field Content
//--- Description -----------------------------------------------------------
// Mountainous region with ancient shrine and corrupted scholar encampment.
// Monsters drop Purification Scrolls for quest Fedimian/3002.
// Quests: Fedimian/3001 "Chronicles of the Demon War" (testimony),
//         Fedimian/3002 "Ancient Purification" (scroll drops),
//         Fedimian/3004 "Trials of the Forbidden Archives",
//         f_rokas_25/1001 "Desert Mysteries" (chupacabra behavior research),
//         f_rokas_25/1002 "Heights of Dzida" (cliff blossom collection),
//         f_rokas_25/1003 "Echoes in Stone" (monument restoration),
//         f_rokas_25/1004 "Sanctum's Memory" (clear Zinutes and recover relics)
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class RokasRidgeScript : GeneralScript
{
	protected override void Load()
	{

		// =====================================================================
		// PEREGRIN LYON - WAR TESTIMONY (Quest: Fedimian/3001)
		// =====================================================================
		// A contemplative soul atop the mountain, appreciating peace after war
		// =====================================================================

		var testimonyQuestId = new QuestId("Fedimian", 3001);

		// Peregrin Lyon NPC
		//-------------------------------------------------------------------------
		AddNpc(153176, L("[Wanderer] Peregrin Lyon"), "f_rokas_25", -2065, 761, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Peregrin Lyon"));

			if (!character.Quests.Has(testimonyQuestId))
			{
				await dialog.Msg(L("{#666666}*A solitary figure stands at the mountain's peak, gazing out at the horizon*{/}"));
				await dialog.Msg(L("Ah, a visitor. I come here often to appreciate the view and the peace."));
				await dialog.Msg(L("After the chaos of war, I find solace in places like this. High above the world, where the air is clear and the silence profound."));

				var response = await dialog.Select(L("What brings you to this mountain?"),
					Option(L("Tell me about yourself"), "story"),
					Option(L("What happened during the war?"), "war"),
					Option(L("I'll leave you to your thoughts"), "leave")
				);

				switch (response)
				{
					case "story":
						await dialog.Msg(L("I'm just a wanderer now. During the war, I was... involved. Let's leave it at that."));
						await dialog.Msg(L("When it ended, I couldn't go back to the cities. Too much noise, too many memories."));
						await dialog.Msg(L("So I travel to places like this - mountaintops, quiet forests, anywhere I can find peace."));
						await dialog.Msg(L("Here, atop this mountain, I can breathe again."));
						break;

					case "war":
						await dialog.Msg(L("{#666666}*His expression darkens briefly*{/}"));
						await dialog.Msg(L("The war taught me many things. Mostly, it taught me the value of silence and solitude."));
						await dialog.Msg(L("When you've seen enough chaos, peace becomes sacred. This mountain gives me that."));
						break;

					case "leave":
						await dialog.Msg(L("Safe travels, friend. May you find your own peace."));
						break;
				}
			}
			else if (character.Quests.IsActive(testimonyQuestId))
			{
				var recorded = character.Variables.Perm.GetBool("Laima.Quests.Fedimian.Quest3001.TestimonyPeregrinLyon", false);

				if (recorded)
				{
					await dialog.Msg(L("My testimony is recorded. Share it with the Grand Archive."));
					await dialog.Msg(L("Perhaps others will understand why peace is so precious."));
				}
				else
				{
					if (!character.Inventory.HasItem(650282))
					{
						await dialog.Msg(L("You need a recording crystal to preserve my testimony."));
						return;
					}

					await dialog.Msg(L("You're documenting the war? I suppose someone should remember what happened."));

					if (await dialog.YesNo(L("Would you like to hear my perspective as someone who sought peace after the chaos?")))
					{
						await dialog.Msg(L("{#666666}*Peregrin speaks quietly into the recording crystal, his voice reflective*{/}"));
						await dialog.Msg(L("'I am Peregrin Lyon, a wanderer. The war took everything familiar and turned it into chaos.'"));
						await dialog.Msg(L("'I fought, I survived, but I couldn't return to normal life. The noise, the crowds - it was all too much.'"));
						await dialog.Msg(L("'So I sought places like this. Mountaintops where the world is quiet, where I can appreciate what we still have.'"));
						await dialog.Msg(L("'Peace. Silence. The view from high above. These are what I fought for, even if I didn't know it then.'"));

						character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3001.TestimonyPeregrinLyon", true);
						character.Quests.CompleteObjective(testimonyQuestId, "testimony2");

						var testimonies = character.Variables.Perm.GetInt("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected", 0);
						character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected", testimonies + 1);

						character.ServerMessage(LF("War testimonies recorded: {0}/3", testimonies + 1));

						if (testimonies + 1 >= 3)
						{
							character.ServerMessage(L("{#FFD700}All testimonies collected! Return to Grand Archivist Elwen in Fedimian.{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("I understand. The view will be here when you're ready."));
					}
				}
			}
			else
			{
				await dialog.Msg(L("Thank you for preserving our stories. Maybe future generations will cherish peace as I do."));
				await dialog.Msg(L("{#666666}*He returns to gazing at the horizon*{/}"));
			}
		});

		// =====================================================================
		// MARCUS - DESERT MYSTERIES (Quest: f_rokas_25/1001)
		// =====================================================================
		// A Fedimian villager researching chupacabra behavior
		// =====================================================================

		var chupacabraQuestId = new QuestId("f_rokas_25", 1001);

		AddNpc(20117, L("[Researcher] Marcus"), "f_rokas_25", -2067, -742, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Marcus"));

			if (!character.Quests.Has(chupacabraQuestId))
			{
				await dialog.Msg(L("Greetings, traveler. I'm from Fedimian, here to study the local wildlife."));
				await dialog.Msg(L("{#666666}*He glances at the desert landscape*{/}"));
				await dialog.Msg(L("These chupacabras are fascinating creatures. Fox-like, docile... yet the old records claim they were once vicious monsters."));


				var response = await dialog.Select(L("These chupacabras are fascinating creatures. Fox-like, docile... yet the old records claim they were once vicious monsters."),
						Option(L("How can I help?"), "help"),
						Option(L("What changed them?"), "change"),
						Option(L("I'm not interested"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("You're willing to help? Excellent! I need empirical data on their current hostility levels."));
						await dialog.Msg(L("I understand that Felix, a colleague of mine, is... well, he's hiding behind that pillar over there. Still terrified of the old stories."));

						await dialog.Msg(L("He thinks they're dangerous, but I've observed them for weeks. They're harmless now!"));

						if (await dialog.YesNo(L("Would you help me prove this scientifically? I need you to engage with some chupacabras and document their behavior.")))

						{
							character.Quests.Start(chupacabraQuestId);
							await dialog.Msg(L("Perfect! Engage with a few desert chupacabras. Observe their reactions, their threat level."));
							await dialog.Msg(L("It's unfortunate, but the only way to truly test hostility is through direct confrontation. Return when you have sufficient data."));
						}
						break;

					case "change":
						await dialog.Msg(L("That's the mystery, isn't it? The demon war changed many things."));
						await dialog.Msg(L("Perhaps the demonic influence that once corrupted them has faded. Or perhaps they've simply adapted to a peaceful world."));

						await dialog.Msg(L("Without proper research, we're just speculating. That's why I need data."));
						break;

					case "leave":
						await dialog.Msg(L("Safe travels then. Watch your step in the desert."));
						break;
				}
			}
			else if (character.Quests.IsActive(chupacabraQuestId))
			{
				if (!character.Quests.TryGetById(chupacabraQuestId, out var quest)) return;
				if (!quest.TryGetProgress("killChupacabras", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("You've returned! Tell me - did they attack unprovoked? Display aggression?"));
					await dialog.Msg(L("{#666666}*He listens intently to your report*{/}"));
					await dialog.Msg(L("Interesting! They defend themselves when attacked, but show no predatory behavior. Just as I theorized!"));

					await dialog.Msg(L("This data is invaluable. The old stories painted them as bloodthirsty beasts, but they're simply... creatures trying to survive."));

					await dialog.Msg(L("Perhaps someone should tell Felix he can stop hiding now. Here, you've earned this for your assistance."));


					character.Quests.Complete(chupacabraQuestId);
				}
				else
				{
					await dialog.Msg(L("Still gathering data? Remember - I need observations from encounters with the desert chupacabras."));

					await dialog.Msg(L("Return when you have sufficient behavioral documentation."));
				}
			}
			else
			{
				await dialog.Msg(L("Your research was quite helpful! I'm compiling my findings for the Fedimian archives."));
				await dialog.Msg(L("Hopefully this will put old superstitions to rest. Well, once Felix stops hiding, at least."));
			}
		});

		// =====================================================================
		// ELENA - HEIGHTS OF DZIDA (Quest: f_rokas_25/1002)
		// =====================================================================
		// An adventurer impressed by acrobatic cliff-climbing skills
		// =====================================================================

		var cliffsideQuestId = new QuestId("f_rokas_25", 1002);

		// Elena NPC
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Botanist] Elena"), "f_rokas_25", 821, 1214, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Elena"));

			if (!character.Quests.Has(cliffsideQuestId))
			{
				await dialog.Msg(L("{#666666}*A woman stands carefully back from the cliff edge, studying plants in her journal*{/}"));
				await dialog.Msg(L("Well, well! Someone who can navigate those platforms!"));
				await dialog.Msg(L("Most people take one look at the jumping required and turn right back around."));

				var response = await dialog.Select(L("I'm actually looking for someone acrobatic like you."),
					Option(L("What do you need?"), "help"),
					Option(L("How did you get up here?"), "climb"),
					Option(L("I should get going"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("You see, I'm a botanist studying cliff-dwelling flora. The plants up here are unique!"));
						await dialog.Msg(L("There are specimens growing on precarious ledges near the cliff edge."));
						await dialog.Msg(L("{#666666}*She points to various rocky outcroppings in the distance, careful not to look down*{/}"));
						await dialog.Msg(L("I'm... well, I'm a bit afraid of heights, actually. Ironic, isn't it?"));

						if (await dialog.YesNo(L("Would you mind collecting some cliff blossoms for me? You clearly have the skills for it.")))
						{
							character.Quests.Start(cliffsideQuestId);
							await dialog.Msg(L("Wonderful! Look for the distinctive blue flowers growing from crevices in high places."));
							await dialog.Msg(L("They're called Dzida Blossoms. I need six specimens for my research."));
							await dialog.Msg(L("Be careful out there - one wrong step and... well, you know."));
						}
						break;

					case "climb":
						await dialog.Msg(L("{#666666}*She laughs nervously*{/}"));
						await dialog.Msg(L("Very, very carefully. And with my eyes half-closed most of the time."));
						await dialog.Msg(L("The view is magnificent, but I try not to look down too often!"));
						break;

					case "leave":
						await dialog.Msg(L("Safe climbing! Watch your footing out there."));
						break;
				}
			}
			else if (character.Quests.IsActive(cliffsideQuestId))
			{
				var blossomCount = character.Inventory.CountItem(650587);

				if (blossomCount >= 6)
				{
					await dialog.Msg(L("You found them! And you're still in one piece!"));
					await dialog.Msg(L("{#666666}*She carefully examines the blossoms*{/}"));
					await dialog.Msg(L("Perfect specimens! The altitude and mineral-rich rocks give them unique properties."));
					await dialog.Msg(L("These will be invaluable for my research. Thank you for risking life and limb!"));

					character.Inventory.Remove(650587, 6, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(cliffsideQuestId);
				}
				else
				{
					await dialog.Msg(L("Still searching for the Dzida Blossoms? They grow in rocky crevices at high elevations."));
					await dialog.Msg(L("I need six total. Be careful on those ledges!"));
				}
			}
			else
			{
				await dialog.Msg(L("Thanks again for the specimens! My research paper is coming along nicely."));
				await dialog.Msg(L("{#666666}*She's carefully pressing flowers in a journal*{/}"));
			}
		});

		// Cliff Blossom Collection Points
		//-------------------------------------------------------------------------
		void AddCliffBlossom(int blossomNumber, int x, int z, int direction)
		{
			AddNpc(153021, L("Dzida Blossom"), "f_rokas_25", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var variableKey = $"Laima.Quests.Quest1002.Blossom{blossomNumber}";

				if (!character.Quests.IsActive(cliffsideQuestId))
				{
					await dialog.Msg(L("{#666666}*Blue flowers grow near the cliff edge*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already collected this blossom*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Carefully harvesting cliff blossom..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650587, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Found: Blue Fragaras Petal"));

					var currentCount = character.Inventory.CountItem(650587);
					character.ServerMessage(LF("Blue Fragaras Petals collected: {0}/6", currentCount));

					if (currentCount >= 6)
					{
						character.ServerMessage(L("{#FFD700}All petals collected! Return to Elena.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Collection cancelled."));
				}
			});
		}

		// Add all 8 cliff blossoms
		AddCliffBlossom(1, 833, 1381, 44);
		AddCliffBlossom(2, 900, 1376, 45);
		AddCliffBlossom(3, 984, 1343, 45);
		AddCliffBlossom(4, 980, 1310, 44);
		AddCliffBlossom(5, 1104, 1189, 45);
		AddCliffBlossom(6, 1026, 1009, 44);
		AddCliffBlossom(7, 972, 965, 0);
		AddCliffBlossom(8, 850, 940, 45);

		// =====================================================================
		// ARCHIVIST TOBIAS - ECHOES IN STONE (Quest: f_rokas_25/1003)
		// =====================================================================
		// Fedimian archivist restoring war-damaged monuments
		// =====================================================================

		var monumentQuestId = new QuestId("f_rokas_25", 1003);

		// Archivist Tobias NPC
		//-------------------------------------------------------------------------
		AddNpc(20165, L("[Archivist] Tobias"), "f_rokas_25", 1222, -118, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Tobias"));

			if (!character.Quests.Has(monumentQuestId))
			{
				await dialog.Msg(L("{#666666}*An elderly scholar kneels beside shattered stone monuments, sketching in a journal*{/}"));
				await dialog.Msg(L("These magnificent monuments... reduced to rubble by the war."));
				await dialog.Msg(L("They once told the history of this region. Treaties, battles, important figures... all carved in stone."));

				var response = await dialog.Select(L("Now they're just broken memories."),
					Option(L("Can they be restored?"), "help"),
					Option(L("What did they commemorate?"), "history"),
					Option(L("I'll leave you to your work"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("Physically? No, the damage is too severe. But their knowledge can be preserved!"));
						await dialog.Msg(L("The Fedimian Grand Archive is compiling records of everything lost in the war."));
						await dialog.Msg(L("{#666666}*He shows you rubbings of partial inscriptions*{/}"));
						await dialog.Msg(L("These fragments are scattered across the area. Each piece holds part of our history."));

						if (await dialog.YesNo(L("Would you help me collect stone fragments with readable inscriptions? We can't let this knowledge die.")))
						{
							character.Quests.Start(monumentQuestId);
							await dialog.Msg(L("Bless you! Look for monument pieces - they'll have carved text or symbols."));
							await dialog.Msg(L("I need fragments from at least five different monuments to piece together the full record."));
							await dialog.Msg(L("Some pieces are small, others partially buried. Check the ground carefully."));
						}
						break;

					case "history":
						await dialog.Msg(L("This was once called the Ridge of Heroes. Monuments honored those who defended Fedimian."));
						await dialog.Msg(L("There were victory columns, peace treaties carved in archways, names of the fallen..."));
						await dialog.Msg(L("{#666666}*His voice becomes bitter*{/}"));
						await dialog.Msg(L("The demons cared nothing for history. They smashed it all."));
						break;

					case "leave":
						await dialog.Msg(L("Every day more weathering erodes what remains. Time is not on our side."));
						break;
				}
			}
			else if (character.Quests.IsActive(monumentQuestId))
			{
				var fragmentCount = character.Inventory.CountItem(650242);

				if (fragmentCount >= 5)
				{
					await dialog.Msg(L("You've found them! Let me see..."));
					await dialog.Msg(L("{#666666}*He carefully examines each stone fragment*{/}"));
					await dialog.Msg(L("'...treaty signed in the year...' Yes! And this one mentions Captain Valdris!"));
					await dialog.Msg(L("This fragment shows part of the victory column's base inscription..."));
					await dialog.Msg(L("With these, I can reconstruct significant portions of what was lost. The Archive will be so pleased!"));
					await dialog.Msg(L("You've helped preserve history itself. Thank you, truly."));

					character.Inventory.Remove(650242, 5, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(monumentQuestId);
				}
				else
				{
					await dialog.Msg(L("Still searching for monument fragments? They're scattered around this area."));
					await dialog.Msg(L("Look for pieces of carved stone - they'll have inscriptions or symbols on them."));
				}
			}
			else
			{
				await dialog.Msg(L("I'm preparing a full report for the Grand Archive based on your findings."));
				await dialog.Msg(L("These heroes won't be forgotten. Not while we preserve their stories."));
			}
		});

		// Monument Fragment Collection Points
		//-------------------------------------------------------------------------
		void AddBrokenPillar(int fragmentNumber, int x, int z, int direction)
		{
			AddNpc(12080, L("Broken Pillar"), "f_rokas_25", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var variableKey = $"Laima.Quests.Quest1003.Fragment{fragmentNumber}";
				var spawnedKey = $"Laima.Quests.Quest1003.Fragment{fragmentNumber}.Spawned";

				if (!character.Quests.IsActive(monumentQuestId))
				{
					await dialog.Msg(L("{#666666}*A piece of carved stone lies partially buried in the dirt*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already collected this fragment*{/}"));
					return;
				}

				// 30% chance to spawn 2 Lichenclops BEFORE the timed action (only once per pillar per player)
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 30)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Lichenclops, 2, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}The ground trembles as corrupted creatures emerge!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Carefully extracting monument fragment..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(5)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650242, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Found: Monument Fragment"));

					var currentCount = character.Inventory.CountItem(650242);
					character.ServerMessage(LF("Monument fragments collected: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All fragments collected! Return to Archivist Tobias.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Collection cancelled."));
				}
			});
		}

		// Add all 5 broken pillars
		AddBrokenPillar(1, 1023, -189, 315);
		AddBrokenPillar(2, 1113, -251, 225);
		AddBrokenPillar(3, 890, -224, 315);
		AddBrokenPillar(4, 1047, 30, 315);
		AddBrokenPillar(5, 1276, 31, 180);

		// =====================================================================
		// CYRIL - SANCTUM'S MEMORY (Quest: f_rokas_25/1004)
		// =====================================================================
		// Fedimian paladin protecting ruined sanctum
		// =====================================================================

		var sanctumQuestId = new QuestId("f_rokas_25", 1004);

		// Cyril NPC
		//-------------------------------------------------------------------------
		AddNpc(20113, L("[Paladin] Cyril"), "f_rokas_25", 2237, 639, 179, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Cyril"));

			if (!character.Quests.Has(sanctumQuestId))
			{
				await dialog.Msg(L("{#666666}*A Fedimian paladin stands pressed against the rocky cliff face, carefully inspecting every crack and crevice*{/}"));
				await dialog.Msg(L("This wall... I've been examining it for days, searching for any trace of the sanctum's relics."));
				await dialog.Msg(L("{#666666}*He runs his hand along the rough stone, checking for anything embedded in the rock*{/}"));
				await dialog.Msg(L("When the Sanctum of the Eternal Light collapsed, debris scattered everywhere. Some relics may have been buried here in the rockslide."));

				var response = await dialog.Select(L("I haven't found anything in this wall yet, but the ruins behind me still need to be searched."),
					Option(L("How can I help?"), "help"),
					Option(L("What made it special?"), "history"),
					Option(L("I should let you continue your watch"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("The sanctum can never be rebuilt. But perhaps its memory can be honored."));
						await dialog.Msg(L("Zinutes - stone-like scorpions, dangerous but docile - have made the ruins their home."));
						await dialog.Msg(L("{#666666}*He gestures toward the collapsed walls*{/}"));
						await dialog.Msg(L("They don't attack unprovoked, but their presence prevents pilgrims from visiting. We need to clear them out."));
						await dialog.Msg(L("And if you find any holy relics among the rubble, bring them to me. We can at least preserve what remains."));

						if (await dialog.YesNo(L("Will you help restore access to this sacred place?")))
						{
							character.Quests.Start(sanctumQuestId);
							await dialog.Msg(L("May the goddesses protect you. The Zinutes are territorial - they'll defend themselves if threatened."));
							await dialog.Msg(L("As you search the ruins, look for sacred items scattered among the debris."));
							await dialog.Msg(L("The shrines are shattered, but some relics may still be intact. Gather what you can find."));
						}
						break;

					case "history":
						await dialog.Msg(L("The Sanctum was built three hundred years ago, after a great plague."));
						await dialog.Msg(L("Fedimian citizens came here to pray for healing, for protection, for lost loved ones."));
						await dialog.Msg(L("{#666666}*His expression becomes solemn*{/}"));
						await dialog.Msg(L("I trained here as a young squire. The Order of the Eternal Light once called this place home."));
						await dialog.Msg(L("That order is gone now, scattered by the war. But I still honor their memory by standing watch."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He nods and returns to his vigil*{/}"));
						await dialog.Msg(L("May the goddesses watch over you."));
						break;
				}
			}
			else if (character.Quests.IsActive(sanctumQuestId))
			{
				if (!character.Quests.TryGetById(sanctumQuestId, out var quest)) return;
				if (!quest.TryGetProgress("clearZinutes", out var killObj)) return;
				if (!quest.TryGetProgress("collectRelics", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("You've returned! The Zinutes have retreated, and the ruins are accessible once more."));
					await dialog.Msg(L("{#666666}*He carefully receives the sacred items*{/}"));
					await dialog.Msg(L("These prayer beads belonged to a pilgrim from Orsha. This offering bowl was blessed by the high priest."));
					await dialog.Msg(L("Each piece holds memories of faith and devotion."));
					await dialog.Msg(L("{#666666}*He holds the items with reverence*{/}"));
					await dialog.Msg(L("Thank you. The Sanctum may be gone, but its spirit lives on through these remnants."));
					await dialog.Msg(L("You've honored the memory of those who served here. The Order would be grateful."));

					character.Inventory.Remove(650259, character.Inventory.CountItem(650259), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(sanctumQuestId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Clear the Zinutes from the sanctum ruins. ");
					if (!itemObj.Done)
						status += L("Search the shattered shrines for sacred relics. ");

					await dialog.Msg(LF("The work continues. {0}", status));
					await dialog.Msg(L("Every step brings us closer to honoring what was lost."));
				}
			}
			else
			{
				await dialog.Msg(L("The sanctum's relics are now safely preserved in Fedimian's Grand Archive."));
				await dialog.Msg(L("Future generations will know what stood here. Thank you, friend."));
			}
		});

		// Sacred Relic Collection Points
		//-------------------------------------------------------------------------
		void AddShatteredShrine(int shrineNumber, int x, int z, int direction)
		{
			AddNpc(147417, L("Shattered Shrine"), "f_rokas_25", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var variableKey = $"Laima.Quests.Quest1004.Shrine{shrineNumber}";

				if (!character.Quests.IsActive(sanctumQuestId))
				{
					await dialog.Msg(L("{#666666}*The remains of a small shrine lie scattered among the rubble*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already searched this shrine*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Searching through shrine rubble..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650259, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Found: Sacred Relic"));

					var currentCount = character.Inventory.CountItem(650259);
					character.ServerMessage(LF("Sacred relics found: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All relics collected! Return to Paladin Cyril.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Search cancelled."));
				}
			});
		}

		// Add all 9 shattered shrines
		AddShatteredShrine(1, 1978, 383, 44);
		AddShatteredShrine(2, 2237, 310, 45);
		AddShatteredShrine(3, 2526, -196, 45);
		AddShatteredShrine(4, 2187, -396, 45);
		AddShatteredShrine(5, 1834, -640, 45);
		AddShatteredShrine(6, 1576, -235, 44);
		AddShatteredShrine(7, 2855, -150, 45);
		AddShatteredShrine(8, 1685, -898, 45);
		AddShatteredShrine(9, 2546, -791, 45);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Desert Mysteries
//-----------------------------------------------------------------------------

public class DesertMysteriesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_25", 1001);
		SetName(L("Desert Mysteries"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Marcus gather data on chupacabra behavior to prove they are no longer dangerous."));
		SetLocation("f_rokas_25");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Researcher] Marcus"), "f_rokas_25");

		// Kill objective
		AddObjective("killChupacabras", L("Gather behavioral data from Desert Chupacabras"),
				new KillObjective(20, new[] { MonsterId.Chupacabra_Desert }));

		// Rewards
		AddReward(new ExpReward(600, 400));
		AddReward(new SilverReward(3000));
		AddReward(new ItemReward(640002, 8)); // HP potions
		AddReward(new ItemReward(640005, 10)); // SP potions
		AddReward(new ItemReward(640080, 2));  // Lv1 EXP Cards
		AddReward(new ItemReward(141118, 1));  // Long Rod
	}
}

// Quest 1002: Heights of Dzida
//-----------------------------------------------------------------------------

public class HeightsOfDzidaQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_25", 1002);
		SetName(L("Heights of Dzida"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Elena collect rare cliff blossoms from dangerous heights around the Dzida cliffs."));
		SetLocation("f_rokas_25");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Botanist] Elena"), "f_rokas_25");

		// Collect objective
		AddObjective("collectBlossoms", L("Collect Blue Fragaras Petals from cliff blossoms"),
			new CollectItemObjective(650587, 6));

		// Rewards
		AddReward(new ExpReward(800, 550));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640002, 10)); // HP potions
		AddReward(new ItemReward(640005, 12)); // SP potions
		AddReward(new ItemReward(640080, 3));  // Lv1 EXP Cards
		AddReward(new ItemReward(511103, 1));  // Leather Boots
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clean up quest items
		character.Inventory.Remove(650587, character.Inventory.CountItem(650587),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags for all 8 crevices
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.Quest1002.Blossom{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(650587, character.Inventory.CountItem(650587),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags for all 8 crevices
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.Quest1002.Blossom{i}");
		}
	}
}

// Quest 1003: Echoes in Stone
//-----------------------------------------------------------------------------
public class EchoesInStoneQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_25", 1003);
		SetName(L("Echoes in Stone"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Archivist Tobias collect monument fragments to preserve the history destroyed during the war."));
		SetLocation("f_rokas_25");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Archivist] Tobias"), "f_rokas_25");

		// Collect objective
		AddObjective("collectFragments", L("Collect monument fragments with inscriptions"),
			new CollectItemObjective(650242, 5));

		// Rewards
		AddReward(new ExpReward(700, 480));
		AddReward(new SilverReward(3500));
		AddReward(new ItemReward(640002, 8)); // HP potions
		AddReward(new ItemReward(640005, 12)); // SP potions
		AddReward(new ItemReward(640080, 3));  // Lv1 EXP Cards
		AddReward(new ItemReward(581102, 1));  // SP Necklace
		AddReward(new ItemReward(ItemId.Scroll_Warp_Fedimian, 1)); // Fedimian scroll
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clean up quest items
		character.Inventory.Remove(650242, character.Inventory.CountItem(650242),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags and spawn flags
		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.Quest1003.Fragment{i}");
			character.Variables.Perm.Remove($"Laima.Quests.Quest1003.Fragment{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(650242, character.Inventory.CountItem(650242),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.Quest1003.Fragment{i}");
			character.Variables.Perm.Remove($"Laima.Quests.Quest1003.Fragment{i}.Spawned");
		}
	}
}

// Quest 1004: Sanctum's Memory
//-----------------------------------------------------------------------------
public class SanctumsMemoryQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_25", 1004);
		SetName(L("Sanctum's Memory"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Paladin Cyril restore access to the ruined sanctum by clearing Zinutes and recovering sacred relics."));
		SetLocation("f_rokas_25");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Paladin] Cyril"), "f_rokas_25");

		// Kill objective
		AddObjective("clearZinutes", L("Defeat Zinutes near the sanctum ruins"),
			new KillObjective(15, new[] { MonsterId.Zinute }));

		// Collect objective
		AddObjective("collectRelics", L("Collect sacred relics from shattered shrines"),
			new CollectItemObjective(650259, 5));

		// Rewards
		AddReward(new ExpReward(800, 600));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(640002, 10)); // HP potions
		AddReward(new ItemReward(640005, 14)); // SP potions
		AddReward(new ItemReward(640080, 3));  // Lv1 EXP Cards
		AddReward(new ItemReward(601125, 1));  // Ring Bracelet
		AddReward(new ItemReward(ItemId.Scroll_Warp_Fedimian, 2)); // Fedimian scroll
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clean up quest items (already removed in NPC dialog, but cleanup any extras)
		character.Inventory.Remove(650259, character.Inventory.CountItem(650259),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags for all 9 shrines
		for (int i = 1; i <= 9; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.Quest1004.Shrine{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(650259, character.Inventory.CountItem(650259),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags for all 9 shrines
		for (int i = 1; i <= 9; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.Quest1004.Shrine{i}");
		}
	}
}
