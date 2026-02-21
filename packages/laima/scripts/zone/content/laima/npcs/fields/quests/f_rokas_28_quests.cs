//--- Melia Script ----------------------------------------------------------
// Tiltas Valley Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Tiltas Valley for post-demon war storyline.
// Quests: f_rokas_28/1001 "Scorpion Suppression" (kill quest),
//         f_rokas_28/1002 "Valley Patrol" (hybrid kill quest),
//         f_rokas_28/1003 "Monuments of the Valley" (exploration quest),
//         f_rokas_28/1004 "Desert Herbalist" (collection quest)
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

public class TiltasValleyQuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Scorpion Suppression - Supply Officer
		//-------------------------------------------------------------------------
		AddNpc(20013, "[Supply Officer] Darius", "f_rokas_28", -563, -1537, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_28", 1001);

			dialog.SetTitle("Darius");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Welcome to our supply camp, traveler. Though I must warn you - we've been having some... difficulties.");
				await dialog.Msg("{#666666}*He glances nervously at the ground around the tents*{/}");
				await dialog.Msg("The Lauzinute scorpions aren't naturally aggressive, but there are so many of them around here that my workers keep stepping on them by accident.");
				await dialog.Msg("Every time someone provokes one, we have incidents. Just yesterday, three of my men were stung while unloading supplies.");

				var response = await dialog.Select("We need to thin out their population before someone gets seriously hurt.",
					Option("I'll help clear the area", "help"),
					Option("How dangerous are they?", "info"),
					Option("Not my problem", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("You'd do that? Excellent! I can't spare any of my men for this - we're already short-staffed.");

						if (await dialog.YesNo("Could you reduce the Lauzinute population around our camp? We need the immediate area safer for the supply teams."))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! You'll find plenty of them right around here - they've made the entire area their nesting ground.");
							await dialog.Msg("Be careful though. While they're not aggressive by nature, they will defend themselves if provoked.");
						}
						break;

					case "info":
						await dialog.Msg("The Lauzinute themselves aren't particularly deadly - their venom causes pain and swelling, but it's rarely fatal.");
						await dialog.Msg("The real problem is their sheer numbers. When you're carrying heavy supplies and focused on your work, it's too easy to disturb one accidentally.");
						await dialog.Msg("We need fewer of them around the camp so my workers have room to operate safely.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*He sighs*{/}");
						await dialog.Msg("I understand. I'll keep warning my men to watch their step.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLauzinute", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg("You've returned! I can already tell the difference - my workers reported far fewer sightings today.");
					await dialog.Msg("{#666666}*He looks visibly relieved*{/}");
					await dialog.Msg("The camp is much safer now. We can actually move supplies without constantly checking the ground for scorpions.");
					await dialog.Msg("You've made a real difference for the supply route between Fedimian and Klaipeda. Here, you've earned this.");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("The Lauzinute are still thick around here. You'll find them all over the area near the camp.");
					await dialog.Msg("Thank you for your help with this.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Thanks to you, supply operations have returned to normal. My workers can focus on their jobs instead of watching for scorpions.");
				await dialog.Msg("Safe travels, friend.");
			}
		});

		// Quest 1002: Valley Patrol - Scout
		//-------------------------------------------------------------------------
		AddNpc(147415, "[Scout] Valeria", "f_rokas_28", -361, 394, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_28", 1002);

			dialog.SetTitle("Valeria");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A Fedimian scout stands at the cliff edge, scanning the valley with a spyglass*{/}");
				await dialog.Msg("This overlook provides the best view of Tiltas Valley. I can monitor threats from here and report to Fedimian command.");
				await dialog.Msg("{#666666}*She lowers the spyglass and turns to you*{/}");
				await dialog.Msg("And right now, I'm seeing two distinct threats. Hogma Archer raiders are establishing camps in the area, and Lauzinute infestations are spreading below the cliffs.");

				var response = await dialog.Select("This observation post is crucial for Fedimian's security, but I can't leave to deal with these threats myself.",
					Option("I'll handle both threats", "help"),
					Option("What are the Hogma doing here?", "hogma"),
					Option("I have other priorities", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("A capable warrior offering assistance? This is exactly what I needed.");

						if (await dialog.YesNo("Will you conduct a patrol sweep? Eliminate the Hogma Archers establishing camps, and clear the Lauzinute below the cliffs?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Excellent! The Lauzinute are clustered right below this cliff - you'll find them easily.");
							await dialog.Msg("The Hogma Archers are more spread out, establishing camps at various points around the valley. You'll need to patrol the area to find them all.");
							await dialog.Msg("Once both threats are eliminated, return here. I'll report your success to Fedimian command.");
						}
						break;

					case "hogma":
						await dialog.Msg("The Hogma are widling warriors - brutal and territorial. They've been pushing into this area from the west.");
						await dialog.Msg("I've counted at least a dozen archers setting up defensive positions. If we don't drive them out now, they'll fortify and become a permanent threat.");
						await dialog.Msg("This valley is strategically important - it connects to the Royal Mausoleum and sits between Fedimian and the outer regions. We can't let raiders control it.");
						break;

					case "leave":
						await dialog.Msg("I understand. I'll continue monitoring from here and hope reinforcements arrive soon.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLauzinute", out var lauzinuteObj)) return;
				if (!quest.TryGetProgress("killHogma", out var hogmaObj)) return;

				if (lauzinuteObj.Done && hogmaObj.Done)
				{
					await dialog.Msg("You've completed the patrol sweep! I've been tracking your progress from this vantage point.");
					await dialog.Msg("{#666666}*She surveys the valley with satisfaction*{/}");
					await dialog.Msg("The Hogma camps are abandoned, and the Lauzinute population is thinned significantly. The valley is secure - for now.");
					await dialog.Msg("I'll send word to Fedimian command that the threat has been neutralized. They'll want to know we have a capable ally in this region.");
					await dialog.Msg("You've done Fedimian a great service today. Accept this with our gratitude.");

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!lauzinuteObj.Done)
						status += "The Lauzinute below the cliffs still need clearing. ";
					if (!hogmaObj.Done)
						status += "Hogma Archer camps are still active in the valley. ";

					await dialog.Msg($"{status}");
					await dialog.Msg("I'll be watching from here. Stay safe out there.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The valley remains secure thanks to your patrol. I continue to monitor from this position.");
				await dialog.Msg("Fedimian command was impressed by your efficiency. You're building quite a reputation.");
			}
		});

		// Quest 1003: Monuments of the Valley - Archaeologist
		//-------------------------------------------------------------------------
		var monumentQuestId = new QuestId("f_rokas_28", 1003);

		AddNpc(152058, "[Archaeologist] Tobias", "f_rokas_28", 121, -391, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle("Tobias");

			if (!character.Quests.Has(monumentQuestId))
			{
				await dialog.Msg("{#666666}*An elderly scholar examines ancient ruins near the settlement, making sketches in a worn journal*{/}");
				await dialog.Msg("Fascinating! These structures predate even the Royal Mausoleum. The architectural style is completely different from anything in Fedimian's archives.");
				await dialog.Msg("{#666666}*He notices you and straightens up*{/}");
				await dialog.Msg("Ah, forgive me. I get absorbed in my work. I'm cataloging the ancient monuments of Tiltas Valley for the Grand Archive.");

				var response = await dialog.Select("There are two particularly significant structures I've identified - one at Zibintu Hill, another at Stone Pillar Hill.",
					Option("How can I help?", "help"),
					Option("What makes them significant?", "info"),
					Option("Good luck with your research", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("You're offering to assist? Wonderful! My old legs aren't what they used to be, and these monuments are quite far apart.");

						if (await dialog.YesNo("Would you travel to both monuments and document their inscriptions? I need rubbings or detailed observations of the carvings."))
						{
							character.Quests.Start(monumentQuestId);
							await dialog.Msg("Excellent! The first monument stands at Zibintu Hill - you'll find it to the northwest of here.");
							await dialog.Msg("The second is at Stone Pillar Hill, northeast near the Royal Mausoleum approach. Both are ancient stone structures with carved inscriptions.");
							await dialog.Msg("Simply examine each monument closely and document what you find. The inscriptions should still be partially legible despite weathering.");
							await dialog.Msg("Return when you've investigated both sites. This discovery could rewrite our understanding of the region's history!");
						}
						break;

					case "info":
						await dialog.Msg("The carving style and runic patterns don't match any known Fedimian historical period.");
						await dialog.Msg("I believe these monuments were erected by an earlier civilization - perhaps the original inhabitants before even the royal family established the mausoleum.");
						await dialog.Msg("If we can decipher the inscriptions, we might learn about the valley's true origins and why the Royal Mausoleum was built in this specific location.");
						break;

					case "leave":
						await dialog.Msg("Thank you. The work continues, slowly but steadily.");
						break;
				}
			}
			else if (character.Quests.IsActive(monumentQuestId))
			{
				if (!character.Quests.TryGetById(monumentQuestId, out var quest)) return;
				if (!quest.TryGetProgress("investigateMonuments", out var obj)) return;

				if (obj.Done)
				{
					await dialog.Msg("You've returned! Did you find both monuments?");
					await dialog.Msg("{#666666}*He listens eagerly to your descriptions*{/}");
					await dialog.Msg("Remarkable! The patterns you describe match pre-war historical records I found in damaged archives.");
					await dialog.Msg("These monuments appear to mark territorial boundaries of an ancient kingdom that existed here millennia ago.");
					await dialog.Msg("{#666666}*He makes rapid notes in his journal*{/}");
					await dialog.Msg("This suggests the Royal Mausoleum wasn't just built randomly - it was placed here because this land had sacred significance long before Fedimian's royal line.");
					await dialog.Msg("You've contributed to a major historical discovery! The Grand Archive will want to hear about this immediately.");
					await dialog.Msg("Please, accept this reward. Your assistance has been invaluable to scholarship.");

					character.Quests.Complete(monumentQuestId);
				}
				else
				{
					await dialog.Msg("Remember - one monument at Zibintu Hill to the northwest, and one at Stone Pillar Hill to the northeast.");
					await dialog.Msg("Take your time examining them. The inscriptions are ancient and require careful observation.");
				}
			}
			else if (character.Quests.HasCompleted(monumentQuestId))
			{
				await dialog.Msg("Thanks to your fieldwork, I'm preparing a comprehensive report for the Grand Archive about the valley's ancient history.");
				await dialog.Msg("Scholars will be studying these findings for years to come!");
			}
		});

		// Monument Collection Points
		//-------------------------------------------------------------------------
		void AddMonument(int monumentNumber, int x, int z, int direction, string hillName)
		{
			AddNpc(12080, "Ancient Monument", "f_rokas_28", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var variableKey = $"Laima.Quests.f_rokas_28.Quest1003.Monument{monumentNumber}";

				if (!character.Quests.IsActive(monumentQuestId))
				{
					await dialog.Msg("{#666666}*An ancient stone monument rises from the ground, weathered by centuries but still standing*{/}");
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg("{#666666}*You've already documented this monument*{/}");
					return;
				}

				var result = await character.TimeActions.StartAsync(
					"Examining ancient inscriptions...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage($"Documented: {hillName} Monument");

					var monumentsInvestigated = character.Variables.Perm.GetInt("Laima.Quests.f_rokas_28.Quest1003.MonumentsInvestigated", 0);
					character.Variables.Perm.Set("Laima.Quests.f_rokas_28.Quest1003.MonumentsInvestigated", monumentsInvestigated + 1);

					character.ServerMessage($"Monuments documented: {monumentsInvestigated + 1}/2");

					if (monumentsInvestigated + 1 >= 2)
					{
						character.Quests.CompleteObjective(monumentQuestId, "investigateMonuments");
						character.ServerMessage("{#FFD700}All monuments documented! Return to Archaeologist Tobias.{/}");
					}
				}
				else
				{
					character.ServerMessage("Investigation cancelled.");
				}
			});
		}

		// Add both monuments
		AddMonument(1, -631, 47, 135, "Zibintu Hill");
		AddMonument(2, 1282, 1822, 45, "Stone Pillar Hill");

		// Quest 1004: Desert Herbalist - Herbalist
		//-------------------------------------------------------------------------
		var herbalistQuestId = new QuestId("f_rokas_28", 1004);

		AddNpc(155135, "[Herbalist] Linnea", "f_rokas_28", 1600, 2018, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle("Linnea");

			if (!character.Quests.Has(herbalistQuestId))
			{
				await dialog.Msg("{#666666}*A Fedimian herbalist kneels beside her equipment near the mausoleum slope, examining dried plant samples*{/}");
				await dialog.Msg("The desert climate here is harsh, but it produces some remarkably potent medicinal herbs.");
				await dialog.Msg("{#666666}*She looks up from her work and glances nervously at the mausoleum entrance*{/}");
				await dialog.Msg("I'm researching antidotes to demon corruption - the lingering effects from the war still plague many people.");
				await dialog.Msg("I've set up my research camp here in the valley, though I refuse to venture near the Royal Mausoleum itself.");

				var response = await dialog.Select("The herbs that grow in this valley have unique hemostatic properties that could be crucial for my work.",
					Option("I'll help gather herbs", "help"),
					Option("What kind of antidote?", "info"),
					Option("Why avoid the mausoleum?", "mausoleum"),
					Option("Best of luck", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("You're willing to help with botanical research? Perfect!");

						if (await dialog.YesNo("I need fresh samples of Hemostasis Herb - it grows in scattered locations across the valley. Could you collect eight samples for me?"))
						{
							character.Quests.Start(herbalistQuestId);
							await dialog.Msg("Wonderful! The herbs grow on small desert plants - you'll recognize them by their distinctive silvery leaves.");
							await dialog.Msg("I've spotted several growing sites across the valley, but be careful - some are near Lauzinute nesting areas.");
							await dialog.Msg("{#666666}*She lowers her voice*{/}");
							await dialog.Msg("And whatever you do, don't enter the Royal Mausoleum unprepared. The herbs in the valley are valuable enough - there's no need to risk your life in that cursed place.");
						}
						break;

					case "info":
						await dialog.Msg("Demon corruption causes internal bleeding and tissue breakdown that normal healing magic can't fully address.");
						await dialog.Msg("The Hemostasis Herbs that grow here have natural properties that stop bleeding and promote cellular regeneration.");
						await dialog.Msg("Combined with purification magic, they could create an antidote that reverses corruption damage permanently.");
						await dialog.Msg("This research could save countless lives - people are still suffering from war wounds years later.");
						break;

					case "mausoleum":
						await dialog.Msg("{#666666}*Her expression darkens*{/}");
						await dialog.Msg("There's a hidden entrance to the Royal Mausoleum deeper in this valley. Most people don't know it exists - and Fedimian authorities refuse to acknowledge it.");
						await dialog.Msg("I discovered it two weeks ago with a research expedition - seven herbalists and guards, all experienced adventurers. We thought we could collect rare fungi from the upper floors.");
						await dialog.Msg("We were wrong.");
						await dialog.Msg("{#666666}*She touches a scar on her arm*{/}");
						await dialog.Msg("Golems patrol the corridors - massive stone constructs that crush anything in their path. Demons lurk in the shadows. The very air is thick with corruption.");
						await dialog.Msg("I watched three of my colleagues fall on just the first floor. The guards managed to drag me out, but the others...");
						await dialog.Msg("{#666666}*She shakes her head*{/}");
						await dialog.Msg("I reported the entrance to the Grand Archive immediately. They... dismissed my testimony. Said the mausoleum's official entrance is sealed and that's all that matters.");
						await dialog.Msg("They don't want to admit there's an unguarded breach into one of the kingdom's most dangerous sites. Political embarrassment, I suppose.");
						await dialog.Msg("So the entrance remains open. Unguarded. Waiting for foolish adventurers to stumble into a death trap.");
						await dialog.Msg("If you value your life, stay in the valley. The monsters here are far beyond anything you'll encounter in these fields, but at least you can run from Lauzinute and Hogmas.");
						await dialog.Msg("In the mausoleum's corridors? There's nowhere to run.");

						// Give recovery potions once
						if (!character.Variables.Perm.GetBool("Laima.Quests.f_rokas_28.Quest1004.LinneaPotionsGiven", false))
						{
							await dialog.Msg("{#666666}*She reaches into her pack*{/}");
							await dialog.Msg("Here, take these recovery potions. They're all I have left from the expedition. If you insist on venturing near that cursed place, at least be prepared.");
							character.Inventory.Add(640011, 5, InventoryAddType.PickUp);
							character.Variables.Perm.Set("Laima.Quests.f_rokas_28.Quest1004.LinneaPotionsGiven", true);
						}
						break;

					case "leave":
						await dialog.Msg("Thank you. The work continues, one plant sample at a time.");
						await dialog.Msg("{#666666}*She glances toward the mausoleum again*{/}");
						await dialog.Msg("And stay away from that cursed place unless you absolutely must enter.");
						break;
				}
			}
			else if (character.Quests.IsActive(herbalistQuestId))
			{
				var herbCount = character.Inventory.CountItem(666061);

				if (herbCount >= 8)
				{
					await dialog.Msg("You've collected the herbs! Let me examine them...");
					await dialog.Msg("{#666666}*She carefully inspects each sample*{/}");
					await dialog.Msg("Perfect! These are excellent specimens - fresh, undamaged, and at peak potency.");
					await dialog.Msg("With these, I can begin synthesizing the antidote formula. Each sample will be processed and tested carefully.");
					await dialog.Msg("{#666666}*She stores the herbs in protective containers*{/}");
					await dialog.Msg("You've contributed to potentially life-saving research. When this antidote is perfected, it will help corruption victims across all three cities.");
					await dialog.Msg("Please, accept this reward. You've done more than gather plants - you've given hope to those still suffering from the war.");

					character.Inventory.Remove(666061, 8, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(herbalistQuestId);
				}
				else
				{
					await dialog.Msg($"You've collected {herbCount} out of 8 Hemostasis Herbs so far.");
					await dialog.Msg("Look for desert plants with silvery leaves. They're scattered across the valley, though some are near dangerous areas.");
				}
			}
			else if (character.Quests.HasCompleted(herbalistQuestId))
			{
				await dialog.Msg("The antidote research is progressing well thanks to your herb samples.");
				await dialog.Msg("Soon, we'll be able to help those still suffering from demonic corruption. You should be proud of your contribution.");
			}
		});

		// Herb Collection Points
		//-------------------------------------------------------------------------
		void AddHerbNode(int herbNumber, int x, int z, int direction)
		{
			AddNpc(47200, "Hemostasis Herb Plant", "f_rokas_28", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var variableKey = $"Laima.Quests.f_rokas_28.Quest1004.Herb{herbNumber}";

				if (!character.Quests.IsActive(herbalistQuestId))
				{
					await dialog.Msg("{#666666}*A desert plant with distinctive silvery leaves grows here*{/}");
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg("{#666666}*You've already gathered from this plant*{/}");
					return;
				}

				var result = await character.TimeActions.StartAsync(
					"Carefully harvesting Hemostasis Herb...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(666061, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage("Found: Hemostasis Herb");

					var currentCount = character.Inventory.CountItem(666061);
					character.ServerMessage($"Hemostasis Herbs collected: {currentCount}/8");

					if (currentCount >= 8)
					{
						character.ServerMessage("{#FFD700}All herbs collected! Return to Herbalist Linnea.{/}");
					}
				}
				else
				{
					character.ServerMessage("Harvest cancelled.");
				}
			});
		}

		// Add 16 herb nodes (player needs 8, extras for flexibility)
		AddHerbNode(1, 1569, 1822, 45);
		AddHerbNode(2, 1443, 1824, 45);
		AddHerbNode(3, 1116, 2070, 45);
		AddHerbNode(4, 1221, 1120, 45);
		AddHerbNode(5, 1314, 654, 44);
		AddHerbNode(6, 679, 818, 45);
		AddHerbNode(7, 507, 682, 45);
		AddHerbNode(8, 420, -437, 45);
		AddHerbNode(9, -500, -208, 45);
		AddHerbNode(10, -241, 238, 45);
		AddHerbNode(11, -551, 201, 45);
		AddHerbNode(12, -792, -107, 45);
		AddHerbNode(13, -210, -1759, 45);
		AddHerbNode(14, -671, -1635, 45);
		AddHerbNode(15, -1387, -291, 44);
		AddHerbNode(16, -1768, -791, 45);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Scorpion Suppression
//-----------------------------------------------------------------------------

public class ScorpionSuppressionQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_28", 1001);
		SetName("Scorpion Suppression");
		SetDescription("Help the supply officer thin out the Lauzinute population around the camp to prevent accidents.");
		SetLocation("f_rokas_28");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Supply Officer] Darius", "f_rokas_28");
		// Objectives
		AddObjective("killLauzinute", "Hunt Lauzinute",
			new KillObjective(25, new[] { MonsterId.Lauzinute }));

		// Rewards
		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 1));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 5));  // Stamina Potion
	}
}

// Quest 1002 CLASS: Valley Patrol
//-----------------------------------------------------------------------------

public class ValleyPatrolQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_28", 1002);
		SetName("Valley Patrol");
		SetDescription("Help the Fedimian scout eliminate threats in Tiltas Valley by clearing Hogma Archers and Lauzinute.");
		SetLocation("f_rokas_28");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Scout] Valeria", "f_rokas_28");
		// Objectives
		AddObjective("killLauzinute", "Hunt Lauzinute",
			new KillObjective(18, new[] { MonsterId.Lauzinute }));
		AddObjective("killHogma", "Hunt Hogma Archer",
			new KillObjective(12, new[] { MonsterId.Hogma_Archer }));

		// Rewards
		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 5));  // Stamina Potion
		AddReward(new ItemReward(602117, 1));  // Argint Bracelet
	}
}

// Quest 1003 CLASS: Monuments of the Valley
//-----------------------------------------------------------------------------

public class MonumentsOfTheValleyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_28", 1003);
		SetName("Monuments of the Valley");
		SetDescription("Document ancient monuments at Zibintu Hill and Stone Pillar Hill for the Fedimian archaeologist's research.");
		SetLocation("f_rokas_28");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Archaeologist] Tobias", "f_rokas_28");
		// Objectives
		AddObjective("investigateMonuments", "Investigate ancient monuments",
			new VariableCheckObjective("Laima.Quests.f_rokas_28.Quest1003.MonumentsInvestigated", 2, true));

		// Rewards
		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 5));  // Stamina Potion
		AddReward(new ItemReward(141108, 1));  // Owl Rod
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clear monument tracking
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_28.Quest1003.MonumentsInvestigated");
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_28.Quest1003.Monument1");
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_28.Quest1003.Monument2");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_28.Quest1003.MonumentsInvestigated");
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_28.Quest1003.Monument1");
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_28.Quest1003.Monument2");
	}
}

// Quest 1004 CLASS: Desert Herbalist
//-----------------------------------------------------------------------------

public class DesertHerbalistQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_28", 1004);
		SetName("Desert Herbalist");
		SetDescription("Collect Hemostasis Herbs across the valley for the Fedimian herbalist's antidote research.");
		SetLocation("f_rokas_28");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Herbalist] Linnea", "f_rokas_28");
		// Objectives
		AddObjective("collectHerbs", "Collect Hemostasis Herbs",
			new CollectItemObjective(666061, 8));

		// Rewards
		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 8));  // Normal HP Potions
		AddReward(new ItemReward(640006, 12)); // Normal SP Potions
		AddReward(new ItemReward(640009, 3));  // Stamina Potions
		AddReward(new ItemReward(581127, 1));  // Magic Pendant
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(666061, character.Inventory.CountItem(666061),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags for all 16 herb nodes
		for (int i = 1; i <= 16; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_28.Quest1004.Herb{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(666061, character.Inventory.CountItem(666061),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags for all 16 herb nodes
		for (int i = 1; i <= 16; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_28.Quest1004.Herb{i}");
		}
	}
}
