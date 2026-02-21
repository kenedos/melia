//--- Melia Script ----------------------------------------------------------
// Srautas Gorge Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Srautas Gorge for post-demon war storyline.
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
using Yggdrasil.Util;

public class FGele571QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Helper Method: Wildflower Collection Points
		//-------------------------------------------------------------------------
		void AddWildflowerSpot(int flowerNumber, int x, int z, int direction)
		{
			AddNpc(160049, "Gorge Wildflower", "f_gele_57_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_gele_57_1", 1002);
				var variableKey = $"Laima.Quests.f_gele_57_1.Quest1002.Flower{flowerNumber}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg("{#666666}*A beautiful wildflower with vibrant petals swaying in the breeze*{/}");
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg("{#666666}*You've already collected stamens from this flower*{/}");
					return;
				}

				var spawnedKey = $"{variableKey}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);

				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Grummer, 2, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage("{#FF6666}Territorial Grummer emerge from the flowers!{/}");
					}
				}

				// Lure nearby Grummers when disturbing the flowers
				var luredCount = LureNearbyEnemies(character, new[] { MonsterId.Grummer }, 150, 150);
				if (luredCount > 0)
				{
					character.ServerMessage("{#FF6666}Nearby Grummer notice you disturbing the flowers!{/}");
				}

				var result = await character.TimeActions.StartAsync(
					"Collecting flower stamens...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650696, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage("Found: Wildflower Stamen");

					var currentCount = character.Inventory.CountItem(650696);
					character.ServerMessage($"Wildflower Stamens collected: {currentCount}/6");

					if (currentCount >= 6)
					{
						character.ServerMessage("{#FFD700}All stamens collected! Return to the botanist.{/}");
					}
				}
				else
				{
					character.ServerMessage("Collection cancelled.");
				}
			});
		}

		// Quest NPC 1: Local Settler (Panto Population Control)
		//-------------------------------------------------------------------------
		AddNpc(20117, "[Local Settler] Torin", "f_gele_57_1", -1394, 603, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_1", 1001);

			dialog.SetTitle("Torin");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Welcome to Mieguista slope, traveler. Beautiful view, isn't it?");
				await dialog.Msg("{#666666}*He gestures toward the valley below, then his expression darkens*{/}");
				await dialog.Msg("It would be perfect... if not for the Panto problem. They're breeding out of control!");

				var response = await dialog.Select("The slope used to be peaceful, but now they're everywhere!",
					Option("I'll thin their numbers", "help"),
					Option("What's the problem with Panto?", "info"),
					Option("Good luck with that", "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo("Would you really help? We need someone to clear out the Panto before they overrun the entire gorge!"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! Hunt down 25 of them here on the slope. That should give us breathing room.");
							await dialog.Msg("Be careful - there are a LOT of them around here. They're everywhere you look.");
						}
						break;

					case "info":
						await dialog.Msg("Panto are normally harmless in small numbers, but they breed incredibly fast.");
						await dialog.Msg("A few months ago there were maybe a dozen on this slope. Now there are hundreds!");
						await dialog.Msg("If we don't control their population now, they'll spread to the entire valley.");
						break;

					case "leave":
						await dialog.Msg("Can't blame you. It's not your problem... yet.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPanto", out var pantoObj)) return;

				if (pantoObj.Done)
				{
					await dialog.Msg("You've cleared out a good number of them! I can already see the difference!");
					await dialog.Msg("The slope feels safer now. Here - take this as thanks for your help.");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"Keep hunting! The Panto are all around the slope - you won't have to look far.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Thanks to you, we can actually enjoy the view again without Panto underfoot.");
				await dialog.Msg("They'll breed back up eventually, but at least we have some peace for now.");
			}
		});

		// Quest NPC 2: Traveling Botanist (Honey for the Hives)
		//-------------------------------------------------------------------------
		AddNpc(20168, "[Traveling Botanist] Elara", "f_gele_57_1", -449, -1265, 89, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_1", 1002);

			dialog.SetTitle("Elara");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A woman in travel-worn robes examines the wildflowers with keen interest*{/}");
				await dialog.Msg("These flowers... they're exactly what I need for my medicinal research!");

				var response = await dialog.Select("The stamens contain compounds that can treat fever and inflammation.",
					Option("Can I help with your research?", "help"),
					Option("What kind of medicine?", "info"),
					Option("Good luck with that", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("You would help? That's wonderful!");

						if (await dialog.YesNo("I need wildflower stamens from 6 different flowers in this area. But be careful - Grummer and Zignuts patrol these fields!"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("The flowers are scattered throughout the southern meadow. Look for bright petals!");
							await dialog.Msg("The creatures here are territorial. They might attack if you disturb the flowers. Stay alert!");
						}
						break;

					case "info":
						await dialog.Msg("I'm researching natural remedies for post-war ailments. Many survivors suffer from lingering fevers.");
						await dialog.Msg("These Srautas wildflowers contain compounds that reduce inflammation and fight infection.");
						await dialog.Msg("If I can perfect the formula, we could help countless people recover faster.");
						break;

					case "leave":
						await dialog.Msg("I understand. It is dangerous work gathering ingredients here.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var stamenCount = character.Inventory.CountItem(650696);

				if (stamenCount >= 6)
				{
					await dialog.Msg("{#666666}*Her eyes light up as she sees the stamens*{/}");
					await dialog.Msg("Perfect! These are exactly what I need! The pollen is still fresh!");
					await dialog.Msg("With these, I can create enough medicine to treat an entire village.");
					await dialog.Msg("Here - take this glove. Drake leather is perfect for handling delicate flowers while staying protected.");

					character.Inventory.Remove(650696, 6, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"You have {stamenCount} stamens so far. I need 6 total from different flowers in the meadow.");
					await dialog.Msg("Watch for the Grummer and Zignuts - they don't like visitors disturbing their territory.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The medicine is working beautifully! Fever cases are dropping in the villages I visit.");
				await dialog.Msg("Thank you again for braving the flower fields. Your help saved lives.");
			}
		});

		// Wildflower Collection Points
		//-------------------------------------------------------------------------
		AddWildflowerSpot(1, -420, -1643, 45);
		AddWildflowerSpot(2, -226, -1706, 45);
		AddWildflowerSpot(3, -35, -1630, 45);
		AddWildflowerSpot(4, -83, -1458, 45);
		AddWildflowerSpot(5, -249, -1547, 45);
		AddWildflowerSpot(6, -277, -1352, 45);
		AddWildflowerSpot(7, -399, -1400, 45);

		// Quest NPC 3: Traveling Merchant (Merchant's Panto Problem)
		//-------------------------------------------------------------------------
		AddNpc(20165, "[Traveling Merchant] Gareth", "f_gele_57_1", 578, 465, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_1", 1003);

			dialog.SetTitle("Gareth");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Ah, a fellow traveler! Tell me - have you noticed the Panto in this meadow?");
				await dialog.Msg("Their claws are prized by craftsmen! Sharp, durable, and perfect for making tools and weapons.");

				var response = await dialog.Select("I'm buying Panto Horns at good prices. Interested?",
					Option("I'll hunt Panto for you", "help"),
					Option("What makes their claws valuable?", "info"),
					Option("Not interested", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("Excellent! A hunter with initiative!");

						if (await dialog.YesNo("Bring me 15 Panto Horns and I'll pay you handsomely. I'll even throw in a special crafting recipe!"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Hunt the Panto right here in the meadow. They're everywhere!");
							await dialog.Msg("Fair warning - the claws don't come off easily. You'll need to hunt quite a few to get 15 good ones.");
						}
						break;

					case "info":
						await dialog.Msg("Panto Horns are naturally sharp and retain their edge far longer than steel blades.");
						await dialog.Msg("Craftsmen use them for precision tools - surgical instruments, engraving tools, even weapon edges.");
						await dialog.Msg("A single quality claw can be worth its weight in silver to the right buyer.");
						break;

					case "leave":
						await dialog.Msg("Your loss! These claws practically sell themselves in the cities.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var clawCount = character.Inventory.CountItem(645134);

				if (clawCount >= 15)
				{
					await dialog.Msg("{#666666}*He examines the claws carefully, nodding with approval*{/}");
					await dialog.Msg("Perfect quality! These will fetch a fine price in Klaipeda!");
					await dialog.Msg("You're a skilled hunter. Here's your payment - plus this recipe for a Panto Sword.");
					await dialog.Msg("It's an old design using Panto parts. Fitting reward for a Panto hunter, wouldn't you say?");

					character.Inventory.Remove(645134, 15, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"You have {clawCount} claws so far. Keep hunting - I need 15 quality claws.");
					await dialog.Msg("The meadow is full of Panto. You won't have to search far.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Those claws you brought sold out in two days! Craftsmen were fighting over them!");
				await dialog.Msg("If you find more quality claws, I'll buy them. A hunter like you is always welcome.");
			}
		});

		// Quest NPC 4: Supply Caravan Organizer (Gorge Delivery Service)
		//-------------------------------------------------------------------------
		AddNpc(20103, "[Caravan Organizer] Roland", "f_gele_57_1", 504, 1099, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_1", 1004);

			dialog.SetTitle("Roland");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Our supply caravans can't reach Gele Plateau anymore. The path is too dangerous for wagons.");
				await dialog.Msg("{#666666}*He gestures toward the plateau rising above the gorge*{/}");
				await dialog.Msg("The outpost up there is running low on provisions. They need help urgently.");

				if (await dialog.YesNo("Would you carry emergency food supplies to the outpost? They're desperate up there."))
				{
					character.Quests.Start(questId);
					await dialog.Msg("Thank you! Head through the northern passage to Gele Plateau.");
					await dialog.Msg("Look for Scout Miriam at the outpost entrance. She's coordinating the supply distribution.");

					character.Inventory.Add(667122, 1, InventoryAddType.PickUp);
				}
				else
				{
					await dialog.Msg("I understand. It's a difficult trek. But those people up there are counting on us...");
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				await dialog.Msg("The supplies need to reach Scout Miriam at the Gele Plateau outpost entrance.");
				await dialog.Msg("Head north through the passage. The path is steep but well-marked.");
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Word came back - the supplies arrived safely! The outpost is grateful.");
				await dialog.Msg("You've helped keep them going until proper caravans can get through again.");
			}
		});

		// Quest NPC 5: Ranger (The Gorge Patrol)
		//-------------------------------------------------------------------------
		AddNpc(147415, "[Ranger] Lynn", "f_gele_57_1", -404, -644, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_1", 1005);

			dialog.SetTitle("Lynn");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A weathered ranger scans the gorge with a practiced eye*{/}");
				await dialog.Msg("The eastern gorge needs patrolling. Grummer have been multiplying, and there's something worse...");
				await dialog.Msg("{#FF6666}Cafrisun.{/} An elite Panto warrior. Extremely dangerous. It's been terrorizing travelers on the road.");

				var response = await dialog.Select("I need someone skilled to clear the area and hunt that beast.",
					Option("I'll patrol the gorge", "help"),
					Option("Tell me about Cafrisun", "info"),
					Option("That sounds too dangerous", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("You've got courage, I'll give you that.");

						if (await dialog.YesNo("Clear out 15 Grummer, then track down Cafrisun. Can you handle it?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Head east into the gorge. The Grummer patrol the southern areas.");
							await dialog.Msg("{#FF6666}Cafrisun spawns in the eastern section. Be ready - it's fast, strong, and doesn't go down easy.{/}");
							await dialog.Msg("That beast has elite training and weapons. Don't underestimate it.");
						}
						break;

					case "info":
						await dialog.Msg("Cafrisun is a Panto Javelin warrior - elite rank. Far more dangerous than common Panto.");
						await dialog.Msg("It's smart, aggressive, and carries a wickedly sharp javelin. I've seen it take down three soldiers alone.");
						await dialog.Msg("Only hunters with real skill should attempt to face it. Most travelers just avoid that section entirely.");
						break;

					case "leave":
						await dialog.Msg("I don't blame you. Cafrisun isn't something to face unprepared.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;

				bool grummerDone = quest.TryGetProgress("killGrummer", out var grummerObj) && grummerObj.Done;
				bool cafrisunDone = quest.TryGetProgress("killCafrisun", out var cafrisunObj) && cafrisunObj.Done;

				if (grummerDone && cafrisunDone)
				{
					await dialog.Msg("{#666666}*She looks at you with newfound respect*{/}");
					await dialog.Msg("You actually took down Cafrisun? That beast has been plaguing us for weeks!");
					await dialog.Msg("The gorge is safe again. Travelers can use the eastern road without fear.");
					await dialog.Msg("Here - this boot is a ranger's secret. Drake leather boots will serve you well on patrol.");

					character.Quests.Complete(questId);
				}
				else if (!grummerDone)
				{
					await dialog.Msg("Start with the Grummer in the southern sections. They're dangerous but manageable.");
					await dialog.Msg("Save your strength for Cafrisun - you'll need it.");
				}
				else
				{
					await dialog.Msg("{#FF6666}Now for the real challenge - Cafrisun patrols the eastern gorge.{/}");
					await dialog.Msg("It's fast and hits hard. Keep moving, watch for its javelin throws, and don't let it corner you.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The gorge has been quiet since you dealt with Cafrisun. No more attacks on travelers.");
				await dialog.Msg("You've got the skills of a true ranger. If you ever need work, look me up.");
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Panto Population Control
//-----------------------------------------------------------------------------
public class PantoPopulationControlQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_1", 1001);
		SetName("Panto Population Control");
		SetDescription("Help Torin thin the Panto population on Mieguista slope before they overrun the entire gorge.");
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Local Settler] Torin", "f_gele_57_1");
		// Objectives
		AddObjective("killPanto", "Hunt Panto",
			new KillObjective(25, new[] { MonsterId.Npanto_Baby }));

		// Rewards
		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
	}
}

// Quest 1002 CLASS: Honey for the Hives
//-----------------------------------------------------------------------------
public class HoneyForTheHivesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_1", 1002);
		SetName("Honey for the Hives");
		SetDescription("Help the traveling botanist collect wildflower stamens from the dangerous flower fields patrolled by territorial Grummer and Zignuts.");
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Traveling Botanist] Elara", "f_gele_57_1");
		// Objectives
		AddObjective("collectStamens", "Collect Wildflower Stamens",
			new CollectItemObjective(650696, 6));

		// Rewards
		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 4)); // Recovery Potion
		AddReward(new ItemReward(502113, 1)); // Drake Leather Gloves
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650696, character.Inventory.CountItem(650696),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 7; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_1.Quest1002.Flower{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_1.Quest1002.Flower{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650696, character.Inventory.CountItem(650696),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 7; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_1.Quest1002.Flower{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_1.Quest1002.Flower{i}.Spawned");
		}
	}
}

// Quest 1003 CLASS: Merchant's Panto Problem
//-----------------------------------------------------------------------------
public class MerchantsPantoProblemQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_1", 1003);
		SetName("Merchant's Panto Problem");
		SetDescription("Hunt Panto in the meadow and collect 15 valuable Panto Horns for the traveling merchant Gareth.");
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Traveling Merchant] Gareth", "f_gele_57_1");
		
		// Objectives
		AddObjective("collectClaws", "Collect Panto Horns",
			new CollectItemObjective(645134, 15));

		// Rewards
		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(24000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
		AddReward(new ItemReward(920010, 1)); // Recipe - Panto Sword
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(645134, character.Inventory.CountItem(645134),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(645134, character.Inventory.CountItem(645134),
			InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Gorge Delivery Service
//-----------------------------------------------------------------------------
public class GorgeDeliveryServiceQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_1", 1004);
		SetName("Gorge Delivery Service");
		SetDescription("Deliver emergency food supplies from Srautas Gorge to the isolated outpost on Gele Plateau.");
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Caravan Organizer] Roland", "f_gele_57_1");
		// Objectives
		AddObjective("deliverSupplies", "Deliver supplies to Scout Miriam on Gele Plateau",
			new ManualObjective());

		// Rewards
		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667122, character.Inventory.CountItem(667122),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667122, character.Inventory.CountItem(667122),
			InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Gorge Patrol
//-----------------------------------------------------------------------------
public class GorgePatrolQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_1", 1005);
		SetName("The Gorge Patrol");
		SetDescription("Patrol the eastern gorge, clear Grummer threats, and hunt down the dangerous elite warrior Cafrisun.");
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Ranger] Lynn", "f_gele_57_1");
		// Objectives
		AddObjective("killGrummer", "Hunt Grummer",
			new KillObjective(15, new[] { MonsterId.Grummer }));

		AddObjective("killCafrisun", "Hunt Cafrisun",
			new KillObjective(1, new[] { MonsterId.Panto_Javelin_Gele }));

		// Rewards
		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(24000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
		AddReward(new ItemReward(512113, 1)); // Drake Leather Boots
	}
}
