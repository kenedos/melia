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
		AddNpc(20059, L("[Scout] Kaina"), "f_bracken_63_1", 1094, 1132, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1001);

			dialog.SetTitle(L("Kaina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Traveler! You've arrived at Koru Jungle - but I'm afraid the entrance isn't safe."));
				await dialog.Msg(L("Poison-tainted Polibu creatures have been swarming the pathway. They're making it dangerous for anyone trying to enter or leave."));

				var response = await dialog.Select(L("Can you help me clear them out?"),
					Option(L("I'll clear the area"), "help"),
					Option(L("What caused this?"), "info"),
					Option(L("Not right now"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Will you help me drive back the Polibu so travelers can pass safely?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! The Polibu spawn just ahead on the path."));
							await dialog.Msg(L("Clear out about twenty of them and the entrance should be safe again."));
						}
						break;

					case "info":
						await dialog.Msg(L("The poison corruption has been spreading through Koru Jungle for weeks now."));
						await dialog.Msg(L("Wildlife that once lived peacefully here have become aggressive and tainted."));
						await dialog.Msg(L("We need help pushing them back before the corruption spreads further."));
						break;

					case "leave":
						await dialog.Msg(L("Be careful if you venture into the jungle. The poison creatures are everywhere."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPolibu", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("You did it! The entrance is much safer now."));
					await dialog.Msg(L("Travelers can pass through without fear of ambush. Thank you!"));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The Polibu are still swarming the entrance. Keep clearing them out."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Thanks to you, the jungle entrance is safe again."));
				await dialog.Msg(L("Travelers can come and go without being attacked by those poison creatures."));
			}
		});

		// Quest 2: Herb Gatherer's Plea
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Herbalist] Ruta"), "f_bracken_63_1", 63, -18, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An elderly herbalist tends to damaged plants near a small cabin*{/}"));
				await dialog.Msg(L("My medicinal garden... it's being destroyed!"));
				await dialog.Msg(L("Those four-legged Parrot creatures - they're trampling everything, spreading poison wherever they go."));

				var response = await dialog.Select(L("Will you help me protect my garden?"),
					Option(L("I'll drive them away"), "help"),
					Option(L("What do you grow here?"), "info"),
					Option(L("Good luck with that"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Will you clear the Parrot creatures from around my garden?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Bless you! The Parrot spawn not far from here."));
							await dialog.Msg(L("Drive away enough of them and they should stop trampling my herbs."));
						}
						break;

					case "info":
						await dialog.Msg(L("I cultivate rare medicinal herbs that can counteract poison and corruption."));
						await dialog.Msg(L("But if these creatures keep trampling them, I'll lose everything I've worked for."));
						await dialog.Msg(L("These herbs could help so many people suffering from the poison corruption..."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*Returns to tending the damaged plants sadly*{/}"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killParrot", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The garden is safe! I can already see the Parrot staying away."));
					await dialog.Msg(L("Now I can focus on cultivating these herbs to help people. Thank you so much!"));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Please, drive away those Parrot creatures before they destroy everything."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("My garden is recovering nicely. The herbs will be ready to harvest soon."));
				await dialog.Msg(L("Thank you again for protecting my life's work."));
			}
		});

		// Quest 3: Elevator Maintenance
		//-------------------------------------------------------------------------
		AddNpc(20109, L("[Engineer] Tomas"), "f_bracken_63_1", -122, 861, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1003);

			dialog.SetTitle(L("Tomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The jungle elevator is working, but it's showing serious wear and tear."));
				await dialog.Msg(L("I need to inspect the critical components for damage, but there's a problem..."));
				await dialog.Msg(L("Poison-tainted Polibu have nested all around the machinery. I can't get close enough to work!"));

				var response = await dialog.Select(L("Can you help me with this maintenance work?"),
					Option(L("I'll help clear the area"), "help"),
					Option(L("What needs inspecting?"), "info"),
					Option(L("Find someone else"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Will you clear the Polibu and inspect the elevator components for me?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Excellent! First, clear out the Polibu swarming the elevator platform."));
							await dialog.Msg(L("Then inspect these four critical components: the gear assembly, cable anchor, support beam, and pulley system."));
							await dialog.Msg(L("They're located around the elevator platform. I've marked them for you."));
						}
						break;

					case "info":
						await dialog.Msg(L("The elevator has four critical stress points that need regular inspection:"));
						await dialog.Msg(L("The gear assembly, cable anchor, support beam, and pulley system."));
						await dialog.Msg(L("If any of them fail, the whole elevator could collapse with people inside."));
						break;

					case "leave":
						await dialog.Msg(L("I understand. It's dangerous work with those creatures around."));
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
					await dialog.Msg(L("Wonderful! The area is clear and you've inspected all the components!"));
					await dialog.Msg(L("Let me review your reports... Yes, I can see exactly what needs repair now."));
					await dialog.Msg(L("This will keep the elevator running safely. Thank you!"));

					character.Inventory.Remove(650585, character.Inventory.CountItem(650585), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Clear the Polibu from the elevator area. ");
					if (!itemObj.Done)
						status += L("Inspect the elevator components around the platform. ");

					await dialog.Msg(LF("Keep working on the maintenance. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The elevator is running smoothly now. All the components are in good shape."));
				await dialog.Msg(L("Thanks for your help with the inspection!"));
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
					await dialog.Msg(LF("{{#666666}}*{0} for the elevator mechanism*{{/}}", componentName));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already inspected this component*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 30)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Folibu, 2, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}Poison creatures emerge from the machinery!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Inspecting component..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650585, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(LF("Found: {0}", componentName));

					var currentCount = character.Inventory.CountItem(650585);
					character.ServerMessage(LF("Components inspected: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All components inspected! Return to Tomas.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Inspection cancelled."));
				}
			});
		}

		AddElevatorComponent(1, "Damaged Gear Assembly", -339, 1337, 180);
		AddElevatorComponent(2, "Corroded Cable Anchor", -402, 1319, 270);
		AddElevatorComponent(3, "Cracked Support Beam", -182, 946, 225);
		AddElevatorComponent(4, "Worn Pulley System", -279, 921, 270);

		// Quest 4: Waterfall Sanctuary
		//-------------------------------------------------------------------------
		AddNpc(58290, L("[Monk] Darius"), "f_bracken_63_1", -619, 1948, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1004);

			dialog.SetTitle(L("Darius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A monk meditates peacefully beside the waterfall*{/}"));
				await dialog.Msg(L("Ah, a traveler. You've reached the waterfall sanctuary."));
				await dialog.Msg(L("Long ago, this waterfall held purifying properties that countered poison and corruption."));
				await dialog.Msg(L("But the demon war left its mark. The purification has weakened."));

				var response = await dialog.Select(L("Will you help me restore the waterfall's power?"),
					Option(L("I'll help gather samples"), "help"),
					Option(L("How do we restore it?"), "info"),
					Option(L("Not interested"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Will you collect water samples from the sacred crystal formations?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you. The crystal formations are scattered near this waterfall."));
							await dialog.Msg(L("Collect samples from four of them. I'll use them to study how to restore the purification."));
						}
						break;

					case "info":
						await dialog.Msg(L("The waterfall once channeled divine energy through crystal formations."));
						await dialog.Msg(L("Those crystals still retain traces of purifying power."));
						await dialog.Msg(L("If I can study water samples from them, I may learn how to reawaken the waterfall's blessing."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*Returns to meditation*{/}"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sampleCount = character.Inventory.CountItem(666246);

				if (sampleCount >= 4)
				{
					await dialog.Msg(L("You've collected all four water samples! Excellent work."));
					await dialog.Msg(L("Let me examine these... Yes, I can feel the residual purifying energy."));
					await dialog.Msg(L("With these samples, I can begin the work of restoring the waterfall's blessing."));

					character.Inventory.Remove(666246, 4, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Collect water samples from the crystal formations near the waterfall."));
					await dialog.Msg(L("I need four samples to complete my study."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The waterfall's purification grows stronger each day."));
				await dialog.Msg(L("Soon it will cleanse this entire jungle of poison corruption."));
			}
		});

		// Waterfall Crystal Collection Points
		//-------------------------------------------------------------------------
		void AddWaterfallCrystal(int crystalNum, int x, int z, int direction)
		{
			AddNpc(46218, L("Purifying Crystal"), "f_bracken_63_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_1", 1004);
				var variableKey = $"Laima.Quests.f_bracken_63_1.Quest1004.Crystal{crystalNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A crystal formation glows faintly with purifying energy*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already collected water from this crystal*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Collecting water sample..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(666246, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Found: Purifying Holy Water"));

					var currentCount = character.Inventory.CountItem(666246);
					character.ServerMessage(LF("Water samples collected: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All samples collected! Return to Darius.{/}"));

						var spawnedKey = "Laima.Quests.f_bracken_63_1.Quest1004.TanuSpawned";
						var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);

						if (!hasSpawned)
						{
							character.Variables.Perm.Set(spawnedKey, true);

							if (SpawnTempMonsters(character, MonsterId.Tanu, 3, 100, TimeSpan.FromMinutes(2)))
							{
								character.ServerMessage(L("{#FF6666}Tanu emerge from the bushes!{/}"));
							}
						}
					}
				}
				else
				{
					character.ServerMessage(L("Collection cancelled."));
				}
			});
		}

		AddWaterfallCrystal(1, -547, 2075, 0);
		AddWaterfallCrystal(2, -389, 1781, 0);
		AddWaterfallCrystal(3, -620, 1344, 0);
		AddWaterfallCrystal(4, -580, 1664, 0);

		// Quest 5: Lost in the Mist
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Lost Traveler] Milda"), "f_bracken_63_1", -1000, 278, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1005);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An elderly woman sits exhausted by the waterfall*{/}"));
				await dialog.Msg(L("O-oh! A person! Thank the goddesses!"));
				await dialog.Msg(L("I'm a merchant, but I got separated from my caravan in the mist."));
				await dialog.Msg(L("Those Leafnut creatures chased me here, and I dropped my supply crates everywhere!"));

				var response = await dialog.Select(L("Would you be able to help?"),
					Option(L("I'll help you recover your supplies"), "help"),
					Option(L("How did you get lost?"), "info"),
					Option(L("You're on your own"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Will you clear the Leafnut and recover my lost supply crates?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! The Leafnut shouldn't be far from here."));
							await dialog.Msg(L("And I dropped five supply crates nearby when I was running. Please find them!"));
						}
						break;

					case "info":
						await dialog.Msg(L("The jungle mist is so thick here. I couldn't see where I was going."));
						await dialog.Msg(L("Then those creatures appeared from nowhere and I panicked."));
						await dialog.Msg(L("I ran until I couldn't run anymore. Now I'm lost and my goods are scattered."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*Looks at you with pleading eyes*{/}"));
						await dialog.Msg(L("P-please... don't leave me here alone..."));
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
					await dialog.Msg(L("You found my supplies! And cleared away those creatures!"));
					await dialog.Msg(L("Now I can find my way back to the caravan route. Thank you so much!"));

					character.Inventory.Remove(650442, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Clear the Leafnut from the area. ");
					if (supplyCount < 5)
						status += L("Recover my lost supply crates. ");

					await dialog.Msg(LF("Please help me. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I found my way back to the caravan. We're safely on the trade route now."));
				await dialog.Msg(L("Thank you for saving me and my goods!"));
			}
		});

		// Supply Crate Collection Points
		//-------------------------------------------------------------------------
		void AddSupplyCrate(int crateNum, int x, int z, int direction)
		{
			AddNpc(46212, L("Lost Supply Crate"), "f_bracken_63_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_1", 1005);
				var variableKey = $"Laima.Quests.f_bracken_63_1.Quest1005.Crate{crateNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A merchant's supply crate lies abandoned*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already recovered this supply crate*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Recovering supplies..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650442, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Found: Supply Bag"));

					var currentCount = character.Inventory.CountItem(650442);
					character.ServerMessage(LF("Supply crates recovered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All supplies recovered! Return to Milda.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Recovery cancelled."));
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
		AddNpc(155145, L("[Pathfinder] Jonas"), "f_bracken_63_1", -145, -1302, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_1", 1006);

			dialog.SetTitle(L("Jonas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("This pathway is a critical junction for jungle travelers."));
				await dialog.Msg(L("But it's become overrun with both Parrot and Polibu - poison creatures everywhere!"));
				await dialog.Msg(L("I can't safely guide people through here until the pathway is cleared."));

				var response = await dialog.Select(L("Will you help me clear this route?"),
					Option(L("I'll clear the pathway"), "help"),
					Option(L("How bad is it?"), "info"),
					Option(L("Find another route"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Will you clear both the Parrot and Polibu from this pathway?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! Both types of creatures have nested along this route."));
							await dialog.Msg(L("Drive back the Parrot and Polibu, and this pathway will be safe for travelers again."));
						}
						break;

					case "info":
						await dialog.Msg(L("The Parrot trample everything in their path with those four legs."));
						await dialog.Msg(L("And the Polibu spread poison wherever they go."));
						await dialog.Msg(L("Together they've made this pathway completely impassable."));
						break;

					case "leave":
						await dialog.Msg(L("There are other routes, but they're much longer and more dangerous."));
						await dialog.Msg(L("This pathway needs to be cleared."));
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
					await dialog.Msg(L("The pathway is clear! Both types of poison creatures are gone!"));
					await dialog.Msg(L("I can resume guiding travelers safely through this route. Excellent work!"));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!parrotObj.Done)
						status += L("Clear the Parrot from the pathway. ");
					if (!polibuObj.Done)
						status += L("Clear the Polibu from the pathway. ");

					await dialog.Msg(LF("Keep clearing the route. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The pathway is safe and clear. Travelers can pass through without danger."));
				await dialog.Msg(L("Thank you for helping me restore this critical route!"));
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
		SetName(L("Jungle Entry Patrol"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear poison-tainted Polibu from the jungle entrance to make it safe for travelers."));
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout] Kaina"), "f_bracken_63_1");

		AddObjective("killPolibu", L("Clear poison-tainted Polibu"),
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
		SetName(L("Herb Gatherer's Plea"));
		SetType(QuestType.Sub);
		SetDescription(L("Protect the herbalist's garden by driving away poison-corrupted Parrot creatures."));
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Ruta"), "f_bracken_63_1");

		AddObjective("killParrot", L("Drive away Parrot creatures"),
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
		SetName(L("Elevator Maintenance"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear Polibu from the elevator and inspect critical components for the engineer."));
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Engineer] Tomas"), "f_bracken_63_1");

		AddObjective("clearPolibu", L("Clear poison creatures from the elevator area"),
			new KillObjective(15, new[] { MonsterId.Folibu }));

		AddObjective("inspectComponents", L("Inspect elevator components"),
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
		SetName(L("Waterfall Sanctuary"));
		SetType(QuestType.Sub);
		SetDescription(L("Collect purifying water samples from sacred crystal formations near the waterfall."));
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Monk] Darius"), "f_bracken_63_1");

		AddObjective("collectWater", L("Collect water samples from purifying crystals"),
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
		SetName(L("Lost in the Mist"));
		SetType(QuestType.Sub);
		SetDescription(L("Help a lost merchant by clearing Leafnut and recovering her scattered supply crates."));
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Lost Traveler] Milda"), "f_bracken_63_1");

		AddObjective("clearLeafnut", L("Clear Leafnut threats"),
			new KillObjective(12, new[] { MonsterId.Leafnut }));

		AddObjective("recoverSupplies", L("Recover lost supply crates"),
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
		SetName(L("Pathway Purge"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear the jungle pathway of both Parrot and Polibu to restore the safe travel route."));
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pathfinder] Jonas"), "f_bracken_63_1");

		AddObjective("killParrot", L("Clear Parrot from the pathway"),
			new KillObjective(15, new[] { MonsterId.Ferrot }));

		AddObjective("killPolibu", L("Clear Polibu from the pathway"),
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

						character.ServerMessage(L("{#66FF66}You feel a toxic miasma seeping into your body!{/}"));
					}
					else
					{
						character.Variables.Perm.Set(spawnKey, true);

						var spawnCount = RandomProvider.Get().Next(2, 4);
						if (SpawnTempMonsters(character, MonsterId.Tanu, spawnCount, 70, TimeSpan.FromMinutes(1)))
						{
							character.ServerMessage(L("{#FF6666}Tanu emerge from the poisonous undergrowth!{/}"));
						}
					}
				}
			});
		}
	}
}
