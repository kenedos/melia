//--- Melia Script ----------------------------------------------------------
// Koru Jungle Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Koru Jungle for poison corruption storyline.
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

public class FBracken631QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Jungle Entry Patrol
		//-------------------------------------------------------------------------
		AddNpc(20059, "[Scout] Kaina", "f_bracken_63_1", 1094, 1132, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1001);

			dialog.SetTitle("Kaina");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Traveler! You've arrived at Koru Jungle - but I'm afraid the entrance isn't safe.");
				await dialog.Msg("Poison-tainted Polibu creatures have been swarming the pathway. They're making it dangerous for anyone trying to enter or leave.");

				var response = await dialog.Select("Can you help me clear them out?",
					Option("I'll clear the area", "help"),
					Option("What caused this?", "info"),
					Option("Not right now", "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo("Will you help me drive back the Polibu so travelers can pass safely?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! The Polibu spawn just ahead on the path.");
							await dialog.Msg("Clear out about twenty of them and the entrance should be safe again.");
						}
						break;

					case "info":
						await dialog.Msg("The poison corruption has been spreading through Koru Jungle for weeks now.");
						await dialog.Msg("Wildlife that once lived peacefully here have become aggressive and tainted.");
						await dialog.Msg("We need help pushing them back before the corruption spreads further.");
						break;

					case "leave":
						await dialog.Msg("Be careful if you venture into the jungle. The poison creatures are everywhere.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPolibu", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg("You did it! The entrance is much safer now.");
					await dialog.Msg("Travelers can pass through without fear of ambush. Thank you!");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"The Polibu are still swarming the entrance. Keep clearing them out.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Thanks to you, the jungle entrance is safe again.");
				await dialog.Msg("Travelers can come and go without being attacked by those poison creatures.");
			}
		});

		// Quest 2: Herb Gatherer's Plea
		//-------------------------------------------------------------------------
		AddNpc(155018, "[Herbalist] Ruta", "f_bracken_63_1", 63, -18, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1002);

			dialog.SetTitle("Ruta");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*An elderly herbalist tends to damaged plants near a small cabin*{/}");
				await dialog.Msg("My medicinal garden... it's being destroyed!");
				await dialog.Msg("Those four-legged Parrot creatures - they're trampling everything, spreading poison wherever they go.");

				var response = await dialog.Select("Will you help me protect my garden?",
					Option("I'll drive them away", "help"),
					Option("What do you grow here?", "info"),
					Option("Good luck with that", "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo("Will you clear the Parrot creatures from around my garden?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Bless you! The Parrot spawn not far from here.");
							await dialog.Msg("Drive away enough of them and they should stop trampling my herbs.");
						}
						break;

					case "info":
						await dialog.Msg("I cultivate rare medicinal herbs that can counteract poison and corruption.");
						await dialog.Msg("But if these creatures keep trampling them, I'll lose everything I've worked for.");
						await dialog.Msg("These herbs could help so many people suffering from the poison corruption...");
						break;

					case "leave":
						await dialog.Msg("{#666666}*Returns to tending the damaged plants sadly*{/}");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killParrot", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg("The garden is safe! I can already see the Parrot staying away.");
					await dialog.Msg("Now I can focus on cultivating these herbs to help people. Thank you so much!");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("Please, drive away those Parrot creatures before they destroy everything.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("My garden is recovering nicely. The herbs will be ready to harvest soon.");
				await dialog.Msg("Thank you again for protecting my life's work.");
			}
		});

		// Quest 3: Elevator Maintenance
		//-------------------------------------------------------------------------
		AddNpc(20109, "[Engineer] Tomas", "f_bracken_63_1", -122, 861, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1003);

			dialog.SetTitle("Tomas");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("The jungle elevator is working, but it's showing serious wear and tear.");
				await dialog.Msg("I need to inspect the critical components for damage, but there's a problem...");
				await dialog.Msg("Poison-tainted Polibu have nested all around the machinery. I can't get close enough to work!");

				var response = await dialog.Select("Can you help me with this maintenance work?",
					Option("I'll help clear the area", "help"),
					Option("What needs inspecting?", "info"),
					Option("Find someone else", "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo("Will you clear the Polibu and inspect the elevator components for me?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Excellent! First, clear out the Polibu swarming the elevator platform.");
							await dialog.Msg("Then inspect these four critical components: the gear assembly, cable anchor, support beam, and pulley system.");
							await dialog.Msg("They're located around the elevator platform. I've marked them for you.");
						}
						break;

					case "info":
						await dialog.Msg("The elevator has four critical stress points that need regular inspection:");
						await dialog.Msg("The gear assembly, cable anchor, support beam, and pulley system.");
						await dialog.Msg("If any of them fail, the whole elevator could collapse with people inside.");
						break;

					case "leave":
						await dialog.Msg("I understand. It's dangerous work with those creatures around.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearPolibu", out var killObj)) return;
				if (!quest.TryGetProgress("inspectComponents", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg("Wonderful! The area is clear and you've inspected all the components!");
					await dialog.Msg("Let me review your reports... Yes, I can see exactly what needs repair now.");
					await dialog.Msg("This will keep the elevator running safely. Thank you!");

					character.Inventory.Remove(650585, character.Inventory.CountItem(650585), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += "Clear the Polibu from the elevator area. ";
					if (!itemObj.Done)
						status += "Inspect the elevator components around the platform. ";

					await dialog.Msg($"Keep working on the maintenance. {status}");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The elevator is running smoothly now. All the components are in good shape.");
				await dialog.Msg("Thanks for your help with the inspection!");
			}
		});

		// Elevator Component Collection Points
		//-------------------------------------------------------------------------
		void AddElevatorComponent(int componentNum, string componentName, int x, int z, int direction)
		{
			AddNpc(12080, componentName, "f_bracken_63_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_1", 1003);
				var variableKey = $"Laima.Quests.f_bracken_63_1.Quest1003.Component{componentNum}";
				var spawnedKey = $"Laima.Quests.f_bracken_63_1.Quest1003.Component{componentNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg($"{{#666666}}*{componentName} for the elevator mechanism*{{/}}");
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg("{#666666}*You've already inspected this component*{/}");
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 30)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Folibu, 2, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage("{#FF6666}Poison creatures emerge from the machinery!{/}");
					}
				}

				var result = await character.TimeActions.StartAsync(
					"Inspecting component...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650585, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage($"Found: {componentName}");

					var currentCount = character.Inventory.CountItem(650585);
					character.ServerMessage($"Components inspected: {currentCount}/4");

					if (currentCount >= 4)
					{
						character.ServerMessage("{#FFD700}All components inspected! Return to Tomas.{/}");
					}
				}
				else
				{
					character.ServerMessage("Inspection cancelled.");
				}
			});
		}

		AddElevatorComponent(1, "Damaged Gear Assembly", -339, 1337, 180);
		AddElevatorComponent(2, "Corroded Cable Anchor", -402, 1319, 270);
		AddElevatorComponent(3, "Cracked Support Beam", -182, 946, 225);
		AddElevatorComponent(4, "Worn Pulley System", -279, 921, 270);

		// Quest 4: Waterfall Sanctuary
		//-------------------------------------------------------------------------
		AddNpc(58290, "[Monk] Darius", "f_bracken_63_1", -619, 1948, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1004);

			dialog.SetTitle("Darius");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A monk meditates peacefully beside the waterfall*{/}");
				await dialog.Msg("Ah, a traveler. You've reached the waterfall sanctuary.");
				await dialog.Msg("Long ago, this waterfall held purifying properties that countered poison and corruption.");
				await dialog.Msg("But the demon war left its mark. The purification has weakened.");

				var response = await dialog.Select("Will you help me restore the waterfall's power?",
					Option("I'll help gather samples", "help"),
					Option("How do we restore it?", "info"),
					Option("Not interested", "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo("Will you collect water samples from the sacred crystal formations?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you. The crystal formations are scattered near this waterfall.");
							await dialog.Msg("Collect samples from four of them. I'll use them to study how to restore the purification.");
						}
						break;

					case "info":
						await dialog.Msg("The waterfall once channeled divine energy through crystal formations.");
						await dialog.Msg("Those crystals still retain traces of purifying power.");
						await dialog.Msg("If I can study water samples from them, I may learn how to reawaken the waterfall's blessing.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*Returns to meditation*{/}");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sampleCount = character.Inventory.CountItem(666246);

				if (sampleCount >= 4)
				{
					await dialog.Msg("You've collected all four water samples! Excellent work.");
					await dialog.Msg("Let me examine these... Yes, I can feel the residual purifying energy.");
					await dialog.Msg("With these samples, I can begin the work of restoring the waterfall's blessing.");

					character.Inventory.Remove(666246, 4, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("Collect water samples from the crystal formations near the waterfall.");
					await dialog.Msg("I need four samples to complete my study.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The waterfall's purification grows stronger each day.");
				await dialog.Msg("Soon it will cleanse this entire jungle of poison corruption.");
			}
		});

		// Waterfall Crystal Collection Points
		//-------------------------------------------------------------------------
		void AddWaterfallCrystal(int crystalNum, int x, int z, int direction)
		{
			AddNpc(46218, "Purifying Crystal", "f_bracken_63_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_1", 1004);
				var variableKey = $"Laima.Quests.f_bracken_63_1.Quest1004.Crystal{crystalNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg("{#666666}*A crystal formation glows faintly with purifying energy*{/}");
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg("{#666666}*You've already collected water from this crystal*{/}");
					return;
				}

				var result = await character.TimeActions.StartAsync(
					"Collecting water sample...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(666246, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage("Found: Purifying Holy Water");

					var currentCount = character.Inventory.CountItem(666246);
					character.ServerMessage($"Water samples collected: {currentCount}/4");

					if (currentCount >= 4)
					{
						character.ServerMessage("{#FFD700}All samples collected! Return to Darius.{/}");

						var spawnedKey = "Laima.Quests.f_bracken_63_1.Quest1004.TanuSpawned";
						var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);

						if (!hasSpawned)
						{
							character.Variables.Perm.Set(spawnedKey, true);

							if (SpawnTempMonsters(character, MonsterId.Tanu, 3, 100, TimeSpan.FromMinutes(2)))
							{
								character.ServerMessage("{#FF6666}Tanu emerge from the bushes!{/}");
							}
						}
					}
				}
				else
				{
					character.ServerMessage("Collection cancelled.");
				}
			});
		}

		AddWaterfallCrystal(1, -547, 2075, 0);
		AddWaterfallCrystal(2, -389, 1781, 0);
		AddWaterfallCrystal(3, -620, 1344, 0);
		AddWaterfallCrystal(4, -580, 1664, 0);

		// Quest 5: Lost in the Mist
		//-------------------------------------------------------------------------
		AddNpc(20114, "[Lost Traveler] Milda", "f_bracken_63_1", -1000, 278, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1005);

			dialog.SetTitle("Milda");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*An elderly woman sits exhausted by the waterfall*{/}");
				await dialog.Msg("O-oh! A person! Thank the goddesses!");
				await dialog.Msg("I'm a merchant, but I got separated from my caravan in the mist.");
				await dialog.Msg("Those Leafnut creatures chased me here, and I dropped my supply crates everywhere!");

				var response = await dialog.Select("Would you be able to help?",
					Option("I'll help you recover your supplies", "help"),
					Option("How did you get lost?", "info"),
					Option("You're on your own", "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo("Will you clear the Leafnut and recover my lost supply crates?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! The Leafnut shouldn't be far from here.");
							await dialog.Msg("And I dropped five supply crates nearby when I was running. Please find them!");
						}
						break;

					case "info":
						await dialog.Msg("The jungle mist is so thick here. I couldn't see where I was going.");
						await dialog.Msg("Then those creatures appeared from nowhere and I panicked.");
						await dialog.Msg("I ran until I couldn't run anymore. Now I'm lost and my goods are scattered.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*Looks at you with pleading eyes*{/}");
						await dialog.Msg("P-please... don't leave me here alone...");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearLeafnut", out var killObj)) return;
				var supplyCount = character.Inventory.CountItem(650442);

				if (killObj.Done && supplyCount >= 5)
				{
					await dialog.Msg("You found my supplies! And cleared away those creatures!");
					await dialog.Msg("Now I can find my way back to the caravan route. Thank you so much!");

					character.Inventory.Remove(650442, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += "Clear the Leafnut from the area. ";
					if (supplyCount < 5)
						status += "Recover my lost supply crates. ";

					await dialog.Msg($"Please help me. {status}");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("I found my way back to the caravan. We're safely on the trade route now.");
				await dialog.Msg("Thank you for saving me and my goods!");
			}
		});

		// Supply Crate Collection Points
		//-------------------------------------------------------------------------
		void AddSupplyCrate(int crateNum, int x, int z, int direction)
		{
			AddNpc(46212, "Lost Supply Crate", "f_bracken_63_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_1", 1005);
				var variableKey = $"Laima.Quests.f_bracken_63_1.Quest1005.Crate{crateNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg("{#666666}*A merchant's supply crate lies abandoned*{/}");
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg("{#666666}*You've already recovered this supply crate*{/}");
					return;
				}

				var result = await character.TimeActions.StartAsync(
					"Recovering supplies...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650442, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage("Found: Supply Bag");

					var currentCount = character.Inventory.CountItem(650442);
					character.ServerMessage($"Supply crates recovered: {currentCount}/5");

					if (currentCount >= 5)
					{
						character.ServerMessage("{#FFD700}All supplies recovered! Return to Milda.{/}");
					}
				}
				else
				{
					character.ServerMessage("Recovery cancelled.");
				}
			});
		}

		AddSupplyCrate(1, -743, 348, 0);
		AddSupplyCrate(2, -739, 72, 90);
		AddSupplyCrate(3, -627, -102, 90);
		AddSupplyCrate(4, -933, -316, 90);
		AddSupplyCrate(5, -1076, -76, 90);

		// Quest 6: Pathway Purge
		//-------------------------------------------------------------------------
		AddNpc(155145, "[Pathfinder] Jonas", "f_bracken_63_1", -145, -1302, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1006);

			dialog.SetTitle("Jonas");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("This pathway is a critical junction for jungle travelers.");
				await dialog.Msg("But it's become overrun with both Parrot and Polibu - poison creatures everywhere!");
				await dialog.Msg("I can't safely guide people through here until the pathway is cleared.");

				var response = await dialog.Select("Will you help me clear this route?",
					Option("I'll clear the pathway", "help"),
					Option("How bad is it?", "info"),
					Option("Find another route", "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo("Will you clear both the Parrot and Polibu from this pathway?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! Both types of creatures have nested along this route.");
							await dialog.Msg("Drive back the Parrot and Polibu, and this pathway will be safe for travelers again.");
						}
						break;

					case "info":
						await dialog.Msg("The Parrot trample everything in their path with those four legs.");
						await dialog.Msg("And the Polibu spread poison wherever they go.");
						await dialog.Msg("Together they've made this pathway completely impassable.");
						break;

					case "leave":
						await dialog.Msg("There are other routes, but they're much longer and more dangerous.");
						await dialog.Msg("This pathway needs to be cleared.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killParrot", out var parrotObj)) return;
				if (!quest.TryGetProgress("killPolibu", out var polibuObj)) return;

				if (parrotObj.Done && polibuObj.Done)
				{
					await dialog.Msg("The pathway is clear! Both types of poison creatures are gone!");
					await dialog.Msg("I can resume guiding travelers safely through this route. Excellent work!");

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!parrotObj.Done)
						status += "Clear the Parrot from the pathway. ";
					if (!polibuObj.Done)
						status += "Clear the Polibu from the pathway. ";

					await dialog.Msg($"Keep clearing the route. {status}");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The pathway is safe and clear. Travelers can pass through without danger.");
				await dialog.Msg("Thank you for helping me restore this critical route!");
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Jungle Entry Patrol
//-----------------------------------------------------------------------------

public class JungleEntryPatrolQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_1", 1001);
		SetName("Jungle Entry Patrol");
		SetDescription("Clear poison-tainted Polibu from the jungle entrance to make it safe for travelers.");
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Scout] Kaina", "f_bracken_63_1");

		AddObjective("killPolibu", "Clear poison-tainted Polibu",
			new KillObjective(20, new[] { MonsterId.Folibu }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8)); // Normal SP Potion
	}
}

// Quest 1002 CLASS: Herb Gatherer's Plea
//-----------------------------------------------------------------------------

public class HerbGatherersPleaQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_1", 1002);
		SetName("Herb Gatherer's Plea");
		SetDescription("Protect the herbalist's garden by driving away poison-corrupted Parrot creatures.");
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Herbalist] Ruta", "f_bracken_63_1");

		AddObjective("killParrot", "Drive away Parrot creatures",
			new KillObjective(18, new[] { MonsterId.Ferrot }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8)); // Normal SP Potion
	}
}

// Quest 1003 CLASS: Elevator Maintenance
//-----------------------------------------------------------------------------

public class ElevatorMaintenanceQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_1", 1003);
		SetName("Elevator Maintenance");
		SetDescription("Clear Polibu from the elevator and inspect critical components for the engineer.");
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Engineer] Tomas", "f_bracken_63_1");

		AddObjective("clearPolibu", "Clear poison creatures from the elevator area",
			new KillObjective(15, new[] { MonsterId.Folibu }));

		AddObjective("inspectComponents", "Inspect elevator components",
			new CollectItemObjective(650585, 4));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
		AddReward(new ItemReward(926012, 1)); // Recipe - Shield Breaker
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650585, character.Inventory.CountItem(650585), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_1.Quest1003.Component{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_1.Quest1003.Component{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650585, character.Inventory.CountItem(650585), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_1.Quest1003.Component{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_1.Quest1003.Component{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Waterfall Sanctuary
//-----------------------------------------------------------------------------

public class WaterfallSanctuaryQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_1", 1004);
		SetName("Waterfall Sanctuary");
		SetDescription("Collect purifying water samples from sacred crystal formations near the waterfall.");
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Monk] Darius", "f_bracken_63_1");

		AddObjective("collectWater", "Collect water samples from purifying crystals",
			new CollectItemObjective(666246, 4));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
		AddReward(new ItemReward(941035, 1)); // Recipe - Ferret Marauder Shield
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666246, character.Inventory.CountItem(666246), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_1.Quest1004.Crystal{i}");
		}

		character.Variables.Perm.Remove("Laima.Quests.f_bracken_63_1.Quest1004.TanuSpawned");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666246, character.Inventory.CountItem(666246), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_1.Quest1004.Crystal{i}");
		}

		character.Variables.Perm.Remove("Laima.Quests.f_bracken_63_1.Quest1004.TanuSpawned");
	}
}

// Quest 1005 CLASS: Lost in the Mist
//-----------------------------------------------------------------------------

public class LostInTheMistQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_1", 1005);
		SetName("Lost in the Mist");
		SetDescription("Help a lost merchant by clearing Leafnut and recovering her scattered supply crates.");
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Lost Traveler] Milda", "f_bracken_63_1");

		AddObjective("clearLeafnut", "Clear Leafnut threats",
			new KillObjective(12, new[] { MonsterId.Leafnut }));

		AddObjective("recoverSupplies", "Recover lost supply crates",
			new CollectItemObjective(650442, 5));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
		AddReward(new ItemReward(531122, 1)); // Plate Armor
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650442, character.Inventory.CountItem(650442), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_1.Quest1005.Crate{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650442, character.Inventory.CountItem(650442), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_1.Quest1005.Crate{i}");
		}
	}
}

// Quest 1006 CLASS: Pathway Purge
//-----------------------------------------------------------------------------

public class PathwayPurgeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_1", 1006);
		SetName("Pathway Purge");
		SetDescription("Clear the jungle pathway of both Parrot and Polibu to restore the safe travel route.");
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Pathfinder] Jonas", "f_bracken_63_1");

		AddObjective("killParrot", "Clear Parrot from the pathway",
			new KillObjective(15, new[] { MonsterId.Ferrot }));

		AddObjective("killPolibu", "Clear Polibu from the pathway",
			new KillObjective(15, new[] { MonsterId.Folibu }));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8)); // Normal SP Potion
		AddReward(new ItemReward(927011, 1)); // Recipe - Grand Spontoon
	}
}

//-----------------------------------------------------------------------------
// ENVIRONMENTAL DANGER ZONES
//-----------------------------------------------------------------------------
// These zones trigger either poison or monster spawns when players pass through
//-----------------------------------------------------------------------------

public class FBracken631DangerZonesScript : GeneralScript
{
	private const int PoisonDamage = 500;
	private const int PoisonDuration = 10;
	private const int TriggerChance = 30;

	protected override void Load()
	{
		var zones = new[]
		{
			new { x = 407.19998, z = 422.4217 },
			new { x = 359.4645, z = 165.72365 },
			new { x = -447.8865, z = -62.906685 },
			new { x = -32.98659, z = 427.0288 },
			new { x = 146.6903, z = 687.331 },
			new { x = 380.9853, z = -263.2964 },
			new { x = 825.70184, z = -868.8547 },
			new { x = 6.3541436, z = -725.57263 },
			new { x = -191.0922, z = -684.8806 },
			new { x = 312.29556, z = -1548.8113 },
			new { x = -853.1676, z = -849.5134 },
			new { x = -594.1013, z = 1167.1467 },
			new { x = -1131.1354, z = 1277.1892 },
			new { x = -904.2366, z = 1529.6384 },
			new { x = -252.95355, z = 1887.9817 }
		};

		for (int i = 0; i < zones.Length; i++)
		{
			var zone = zones[i];
			var zoneIndex = i;

			AddAreaTrigger("f_bracken_63_1", zone.x, zone.z, 50, async (args) =>
			{
				if (args.Initiator is not Character character)
					return;

				if (character.IsDead)
					return;

				var poisonKey = $"Laima.Environment.f_bracken_63_1.Zone{zoneIndex}.Poisoned";
				var spawnKey = $"Laima.Environment.f_bracken_63_1.Zone{zoneIndex}.Spawned";

				var alreadyPoisoned = character.Variables.Perm.GetBool(poisonKey, false);
				var alreadySpawned = character.Variables.Perm.GetBool(spawnKey, false);

				if (alreadyPoisoned || alreadySpawned)
					return;

				if (RandomProvider.Get().Next(100) < TriggerChance)
				{
					var eventType = RandomProvider.Get().Next(2);

					if (eventType == 0)
					{
						character.Variables.Perm.Set(poisonKey, true);

						character.StartBuff(
							BuffId.UC_poison,
							1,
							PoisonDamage,
							TimeSpan.FromSeconds(PoisonDuration),
							character
						);

						character.ServerMessage("{#66FF66}You feel a toxic miasma seeping into your body!{/}");
					}
					else
					{
						character.Variables.Perm.Set(spawnKey, true);

						var spawnCount = RandomProvider.Get().Next(2, 4);
						if (SpawnTempMonsters(character, MonsterId.Tanu, spawnCount, 70, TimeSpan.FromMinutes(1)))
						{
							character.ServerMessage("{#FF6666}Tanu emerge from the poisonous undergrowth!{/}");
						}
					}
				}
			});
		}
	}
}
