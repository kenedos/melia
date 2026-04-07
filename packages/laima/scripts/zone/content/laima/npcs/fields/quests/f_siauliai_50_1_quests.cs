//--- Melia Script ----------------------------------------------------------
// Gytis Settlement Area Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Gytis Settlement Area for post-demon war storyline.
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

public class FSiauliai501QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Crop Bundle Collection Helper
		//-------------------------------------------------------------------------
		void AddCropBundle(int bundleNumber, int x, int z, int direction)
		{
			AddNpc(47201, L("Crop Bundle"), "f_siauliai_50_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_50_1", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A bundle of fresh crops tied together with twine*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_siauliai_50_1.Quest1005.CropBundle{bundleNumber}";
				var collected = character.Variables.Perm.GetBool(variableKey, false);

				if (collected)
				{
					await dialog.Msg(L("{#666666}*You've already collected this bundle*{/}"));
					return;
				}

				// 20% chance to spawn 2 Orange Sakmoli when collecting crops
				var spawnedKey = $"Laima.Quests.f_siauliai_50_1.Quest1005.SpawnedBundle{bundleNumber}";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 20)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Sakmoli_Orange, 2, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}Angry pumpkin monsters emerge from the crops!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Collecting crops..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650072, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					// Increment the counter
					var currentCount = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_50_1.Quest1005.BundlesCollected", 0);
					character.Variables.Perm.Set("Laima.Quests.f_siauliai_50_1.Quest1005.BundlesCollected", currentCount + 1);

					character.ServerMessage(LF("Crop bundles collected: {0}/11", currentCount + 1));

					if (currentCount + 1 >= 11)
					{
						character.ServerMessage(L("{#FFD700}All bundles collected! Return to Jonas.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Collection cancelled."));
				}
			});
		}

		// Concerned Farmer Marta - Quest 1001: Warning the Farmers
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Concerned Farmer] Marta"), "f_siauliai_50_1", 1125, -1325, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_50_1", 1001);

			dialog.SetTitle(L("Marta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Those orange pumpkin things... they've gotten so aggressive! I've never seen them act like this before."));
				await dialog.Msg(L("I'm worried about the other farmers scattered across the settlement. They need to be warned about these monsters before someone gets hurt!"));

				var response = await dialog.Select(L("Will you help me warn the farmers?"),
					Option(L("I'll deliver the warnings"), "help"),
					Option(L("What happened to the pumpkins?"), "info"),
					Option(L("Stay safe"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Thank you! I've written warning letters for the other farmers. Can you deliver them to Gregor at Ziuluti Vacant Lot, Henrik by the bridge of Joint Farmland, and Aldric near the Bandrass Forkroad?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Here are the letters. Please hurry - the pumpkins are getting more violent every day!"));

							// Give quest items
							character.Inventory.Add(666037, 3, InventoryAddType.PickUp); // Warning Letters
						}
						break;

					case "info":
						await dialog.Msg(L("They used to be peaceful, just regular farm pests. But something's changed... there's a darkness in them now."));
						await dialog.Msg(L("Some say it's leftover corruption from the demon war. Whatever it is, they're dangerous now."));
						break;

					case "leave":
						await dialog.Msg(L("I understand. I'll wait here near the Klaipeda entrance where it's safer."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var deliveredGregor = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredGregor", false);
				var deliveredHenrik = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredHenrik", false);
				var deliveredAldric = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredAldric", false);

				if (deliveredGregor && deliveredHenrik && deliveredAldric)
				{
					await dialog.Msg(L("You've warned everyone! Thank you so much. Now at least they can prepare defenses."));
					await dialog.Msg(L("Here, take this. It's not much, but you've done us all a great service today."));

					character.Quests.Complete(questId);
				}
				else
				{
					var remaining = "";
					if (!deliveredGregor) remaining += "Gregor at Ziuluti Vacant Lot, ";
					if (!deliveredHenrik) remaining += "Henrik at Joint Farmland, ";
					if (!deliveredAldric) remaining += "Aldric by the Bandrass Forkroad";

					await dialog.Msg(LF("Please deliver the warnings to: {0}.", remaining.TrimEnd(',', ' ')));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The other farmers are taking precautions now. You may have saved lives today."));
			}
		});

		// Farmer Gregor - Delivery NPC for Quest 1001
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Farmer] Gregor"), "f_siauliai_50_1", -349, -1575, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_50_1", 1001);
			var quest1002Id = new QuestId("f_siauliai_50_1", 1002);

			dialog.SetTitle(L("Gregor"));

			// Quest 1001 - Delivery
			if (character.Quests.IsActive(questId) && character.Inventory.HasItem(666037))
			{
				var alreadyDelivered = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredGregor", false);
				if (!alreadyDelivered)
				{
					await dialog.Msg(L("A letter from Marta? Let me see..."));
					await dialog.Msg(L("{#666666}*He reads the warning letter with growing concern*{/}"));
					await dialog.Msg(L("By the goddesses... the pumpkins are that aggressive now? I need to warn my workers immediately!"));

					character.Inventory.Remove(666037, 1, InventoryItemRemoveMsg.Given);
					character.Variables.Perm.Set("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredGregor", true);
					character.Quests.CompleteObjective(questId, "deliverGregor");

					await dialog.Msg(L("Thank you for bringing this warning. I'll make sure everyone here stays alert."));
					// Don't return - allow quest 1002 to be offered
				}
			}

			// Quest 1002 - Quest Giver
			if (!character.Quests.Has(quest1002Id))
			{
				await dialog.Msg(L("The fields are overrun! Those cursed Orange Sakmoli and Black Ridimed are destroying everything we've planted!"));
				await dialog.Msg(L("We can't harvest anything with them swarming the area. Someone needs to clear them out!"));

				if (await dialog.YesNo(L("Will you help us clear the fields? We need both the pumpkins and the Ridimed dealt with!")))
				{
					character.Quests.Start(quest1002Id);
					await dialog.Msg(L("Bless you! Clear out at least 15 of each - that should give us breathing room to work."));
					await dialog.Msg(L("Watch yourself out there. The Sakmoli are violent, and there are swarms of Ridimed everywhere."));
				}
				else
				{
					await dialog.Msg(L("I understand. It's dangerous work. We'll manage somehow..."));
				}
			}
			else if (character.Quests.IsActive(quest1002Id))
			{
				if (!character.Quests.TryGetById(quest1002Id, out var quest)) return;

				if (quest.ObjectivesCompleted)
				{
					await dialog.Msg(L("You've cleared them out! I can already see my workers heading back to the fields!"));
					await dialog.Msg(L("This is exactly what we needed. Here - you've more than earned this reward."));

					character.Quests.Complete(quest1002Id);
				}
				else
				{
					await dialog.Msg(L("Keep at it! We need both the Orange Sakmoli and the Black Ridimed cleared out."));
				}
			}
			else if (character.Quests.HasCompleted(quest1002Id))
			{
				await dialog.Msg(L("The fields are finally safe enough to work again. You have our gratitude."));
			}
			else
			{
				await dialog.Msg(L("Farming isn't easy in times like these. But we manage."));
			}
		});

		// Farmer Henrik - Delivery NPC for Quest 1001 & Quest Giver for 1003
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Farmer] Henrik"), "f_siauliai_50_1", 470, 410, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_50_1", 1001);
			var quest1003Id = new QuestId("f_siauliai_50_1", 1003);

			dialog.SetTitle(L("Henrik"));

			// Quest 1001 - Delivery
			if (character.Quests.IsActive(questId) && character.Inventory.HasItem(666037))
			{
				var alreadyDelivered = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredHenrik", false);
				if (!alreadyDelivered)
				{
					await dialog.Msg(L("Marta sent you? What's wrong?"));
					await dialog.Msg(L("{#666666}*He reads the letter, his face growing pale*{/}"));
					await dialog.Msg(L("This is worse than I thought. I've been watching those pumpkins... there's something unnatural about them."));

					character.Inventory.Remove(666037, 1, InventoryItemRemoveMsg.Given);
					character.Variables.Perm.Set("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredHenrik", true);
					character.Quests.CompleteObjective(questId, "deliverHenrik");

					await dialog.Msg(L("Thank you for the warning. I'll keep my distance from those monsters."));
					// Don't return - allow quest 1003 to be offered
				}
			}

			// Quest 1003 - Quest Giver
			if (!character.Quests.Has(quest1003Id))
			{
				await dialog.Msg(L("I've been studying those Orange Sakmoli pumpkins from a safe distance. There's definitely something wrong with them."));
				await dialog.Msg(L("They drop these strange stems - dark, corrupted-looking things. If I could examine them, I might understand what's causing this aggression."));

				if (await dialog.YesNo(L("Would you collect some Tough Orange Sakmoli Stems for me? I need about 8 to properly study them.")))
				{
					character.Quests.Start(quest1003Id);
					await dialog.Msg(L("Excellent! Be careful when fighting them - they're unpredictable and violent."));
					await dialog.Msg(L("Bring me the stems when you have enough. I'll see if I can identify the source of the corruption."));
				}
				else
				{
					await dialog.Msg(L("I understand. Fighting those monsters is dangerous work."));
				}
			}
			else if (character.Quests.IsActive(quest1003Id))
			{
				var stemCount = character.Inventory.CountItem(663023);

				if (stemCount >= 8)
				{
					await dialog.Msg(L("{#666666}*He examines the corrupted stems carefully*{/}"));
					await dialog.Msg(L("Just as I suspected... these stems are saturated with dark magic. This is definitely lingering corruption from the demon war."));
					await dialog.Msg(L("At least now we know what we're dealing with. Thank you for gathering these samples."));

					character.Inventory.Remove(663023, character.Inventory.CountItem(663023), InventoryItemRemoveMsg.Destroyed);
					character.Quests.Complete(quest1003Id);
				}
				else
				{
					await dialog.Msg(LF("Keep gathering those stems. I need 8 total to properly study the corruption (you have {0}).", stemCount));
				}
			}
			else if (character.Quests.HasCompleted(quest1003Id))
			{
				await dialog.Msg(L("The corruption in those stems confirms my fears. We're dealing with demon war remnants."));
			}
			else
			{
				await dialog.Msg(L("These pumpkins weren't always so aggressive. Something changed them."));
			}
		});

		// Farmer Aldric - Delivery NPC for Quest 1001 & Quest Giver for 1004
		//-------------------------------------------------------------------------
		AddNpc(20138, L("[Farmer] Aldric"), "f_siauliai_50_1", -399, 1277, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_50_1", 1001);
			var quest1004Id = new QuestId("f_siauliai_50_1", 1004);

			dialog.SetTitle(L("Aldric"));

			// Quest 1001 - Delivery
			if (character.Quests.IsActive(questId) && character.Inventory.HasItem(666037))
			{
				var alreadyDelivered = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredAldric", false);
				if (!alreadyDelivered)
				{
					await dialog.Msg(L("A letter? What's this about?"));
					await dialog.Msg(L("{#666666}*He unfolds the letter and reads quickly*{/}"));
					await dialog.Msg(L("The pumpkins are getting that bad? I've had my own problems with those damn rabbits stealing vegetables!"));

					character.Inventory.Remove(666037, 1, InventoryItemRemoveMsg.Given);
					character.Variables.Perm.Set("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredAldric", true);
					character.Quests.CompleteObjective(questId, "deliverAldric");

					await dialog.Msg(L("Thanks for the warning. At least the rabbits aren't violent like those pumpkins."));
					// Don't return - allow quest 1004 to be offered
				}
			}

			// Quest 1004 - Quest Giver
			if (!character.Quests.Has(quest1004Id))
			{
				await dialog.Msg(L("Every night, those sneaky Lepusbunny Assassins raid my vegetable garden. They're not aggressive, but they're relentless!"));
				await dialog.Msg(L("I've tried everything - fences, scarecrows, even staying up to chase them off. Nothing works!"));

				if (await dialog.YesNo(L("Could you help me reduce their numbers? If we thin them out a bit, maybe my crops will have a chance!")))
				{
					character.Quests.Start(quest1004Id);
					await dialog.Msg(L("Thank you! Hunt down at least 6 of those rabbits. That should discourage the rest from raiding my garden."));
					await dialog.Msg(L("They're not dangerous - mostly they just run away. But there are so many of them!"));
				}
				else
				{
					await dialog.Msg(L("I understand. I'll keep trying to protect my crops somehow..."));
				}
			}
			else if (character.Quests.IsActive(quest1004Id))
			{
				if (!character.Quests.TryGetById(quest1004Id, out var quest)) return;

				if (quest.ObjectivesCompleted)
				{
					await dialog.Msg(L("Finally! I can already see fewer rabbits around my garden!"));
					await dialog.Msg(L("My vegetables might actually survive the season now. Here, this is for your help."));

					character.Quests.Complete(quest1004Id);
				}
				else
				{
					await dialog.Msg(L("Keep hunting those rabbits! My vegetables are counting on you!"));
				}
			}
			else if (character.Quests.HasCompleted(quest1004Id))
			{
				await dialog.Msg(L("My garden is doing so much better now. Thank you again!"));
			}
			else
			{
				await dialog.Msg(L("Farming is hard enough without pests stealing everything you grow."));
			}
		});

		// Farmer Jonas - Quest 1005: Hope Among the Fields
		//-------------------------------------------------------------------------
		AddNpc(20151, L("[Farmer] Jonas"), "f_siauliai_50_1", 2016, 699, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_50_1", 1005);

			dialog.SetTitle(L("Jonas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Look around - this is one of the last areas where crops are still growing healthy!"));
				await dialog.Msg(L("The monsters are getting closer every day. I need to harvest everything quickly, but there's too much for one person!"));

				if (await dialog.YesNo(L("Could you help me gather crop bundles from the fields? There are 11 bundles scattered around - we need to work fast before the monsters arrive!")))
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Thank you! The bundles are tied with twine and ready to collect. Gather all 11 and bring them back here."));
					await dialog.Msg(L("This might be the last good harvest we get this season. Every bundle counts!"));
				}
				else
				{
					await dialog.Msg(L("I understand. I'll do what I can, though it'll take much longer alone..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var bundleCount = character.Inventory.CountItem(650072);

				if (bundleCount >= 11)
				{
					await dialog.Msg(L("You got all of them! These crops will feed families through the winter!"));
					await dialog.Msg(L("{#666666}*He carefully stores the bundles in his cart*{/}"));
					await dialog.Msg(L("You've given us hope. As long as we can harvest, we can survive. Thank you."));

					character.Inventory.Remove(650072, character.Inventory.CountItem(650072), InventoryItemRemoveMsg.Destroyed);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep gathering those bundles! I need all 11 to make the harvest worthwhile (you have {0}).", bundleCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Those crops you helped me gather will sustain multiple families. You did good work."));
			}
		});

		// Crop Bundle Collection Points
		//-------------------------------------------------------------------------
		AddCropBundle(1, 1951, 748, 45);
		AddCropBundle(2, 1884, 711, 44);
		AddCropBundle(3, 1872, 674, 45);
		AddCropBundle(4, 1921, 666, 45);
		AddCropBundle(5, 1957, 692, 44);
		AddCropBundle(6, 1919, 525, 44);
		AddCropBundle(7, 1888, 497, 44);
		AddCropBundle(8, 1890, 429, 44);
		AddCropBundle(9, 1933, 456, 45);
		AddCropBundle(10, 1970, 491, 45);
		AddCropBundle(11, 1947, 546, 45);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Warning the Farmers
//-----------------------------------------------------------------------------

public class WarningTheFarmersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_50_1", 1001);
		SetName(L("Warning the Farmers"));
		SetType(QuestType.Sub);
		SetDescription(L("Deliver warning letters to farmers across the Gytis Settlement Area about aggressive Orange Sakmoli monsters."));
		SetLocation("f_siauliai_50_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Concerned Farmer] Marta"), "f_siauliai_50_1");
		// Objectives
		AddObjective("deliverGregor", L("Deliver warning to Gregor at Ziuluti Vacant Lot"),
			new ManualObjective());
		AddObjective("deliverHenrik", L("Deliver warning to Henrik at Joint Farmland"),
			new ManualObjective());
		AddObjective("deliverAldric", L("Deliver warning to Aldric by the Bandrass Forkroad"),
			new ManualObjective());

		// Rewards
		AddReward(new ExpReward(750, 500));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640081, 3));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 5));  // Stamina Potion
		AddReward(new ItemReward(181103, 1));  // Dokyu Bow
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(666037, character.Inventory.CountItem(666037), InventoryItemRemoveMsg.Destroyed);

		// Clear variables
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredGregor");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredHenrik");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredAldric");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(666037, character.Inventory.CountItem(666037), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredGregor");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredHenrik");
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_50_1.Quest1001.DeliveredAldric");
	}
}

// Quest 1002 CLASS: Clearing the Fields
//-----------------------------------------------------------------------------

public class ClearingTheFieldsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_50_1", 1002);
		SetName(L("Clearing the Fields"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear Orange Sakmoli and Black Ridimed from the farmland to protect the crops."));
		SetLocation("f_siauliai_50_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Farmer] Gregor"), "f_siauliai_50_1");
		// Objectives
		AddObjective("killSakmoli", L("Hunt Orange Sakmoli"),
			new KillObjective(15, new[] { MonsterId.Sakmoli_Orange }));
		AddObjective("killRidimed", L("Hunt Black Ridimed"),
			new KillObjective(15, new[] { MonsterId.Ridimed_Purple }));

		// Rewards
		AddReward(new ExpReward(850, 570));
		AddReward(new SilverReward(10500));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 6));  // Stamina Potion
		AddReward(new ItemReward(531104, 1));  // Hard Leather Armor
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// No items to clean up for kill quest
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// No items to clean up for kill quest
	}
}

// Quest 1003 CLASS: The Seed of Corruption
//-----------------------------------------------------------------------------

public class TheSeedOfCorruptionQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_50_1", 1003);
		SetName(L("The Seed of Corruption"));
		SetType(QuestType.Sub);
		SetDescription(L("Collect Tough Orange Sakmoli Stems to help Henrik study the corruption affecting the pumpkin monsters."));
		SetLocation("f_siauliai_50_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Farmer] Henrik"), "f_siauliai_50_1");
		// Quest item drops
		AddDrop(663023, 0.40f, MonsterId.Sakmoli_Orange);

		// Objectives
		AddObjective("collectStems", L("Collect Tough Orange Sakmoli Stems from Orange Sakmoli"),
			new CollectItemObjective(663023, 8));

		// Rewards
		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Cards
		AddReward(new ItemReward(640003, 9)); // Normal HP Potion
		AddReward(new ItemReward(640006, 9)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(663023, character.Inventory.CountItem(663023), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(663023, character.Inventory.CountItem(663023), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Rabbit Problem
//-----------------------------------------------------------------------------

public class RabbitProblemQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_50_1", 1004);
		SetName(L("Rabbit Problem"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Aldric by reducing the population of Lepusbunny Assassins raiding his vegetable garden."));
		SetLocation("f_siauliai_50_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Farmer] Aldric"), "f_siauliai_50_1");
		// Objectives
		AddObjective("killRabbits", L("Hunt Lepusbunny Assassins"),
			new KillObjective(6, new[] { MonsterId.Repusbunny_Bow }));

		// Rewards
		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 1));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// No items to clean up for kill quest
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// No items to clean up for kill quest
	}
}

// Quest 1005 CLASS: Hope Among the Fields
//-----------------------------------------------------------------------------

public class HopeAmongTheFieldsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_50_1", 1005);
		SetName(L("Hope Among the Fields"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Jonas gather crop bundles from the eastern fields before monsters destroy the harvest."));
		SetLocation("f_siauliai_50_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Farmer] Jonas"), "f_siauliai_50_1");
		// Objectives
		AddObjective("collectBundles", L("Collect Crop Bundles from the eastern fields"),
			new VariableCheckObjective("Laima.Quests.f_siauliai_50_1.Quest1005.BundlesCollected", 11, true));

		// Rewards
		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 1));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 5));  // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(650072, character.Inventory.CountItem(650072), InventoryItemRemoveMsg.Destroyed);

		// Clear collection flags and counter
		for (int i = 1; i <= 11; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_50_1.Quest1005.CropBundle{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_50_1.Quest1005.SpawnedBundle{i}");
		}
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_50_1.Quest1005.BundlesCollected");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Same cleanup as OnComplete
		character.Inventory.Remove(650072, character.Inventory.CountItem(650072), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 11; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_50_1.Quest1005.CropBundle{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_50_1.Quest1005.SpawnedBundle{i}");
		}
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_50_1.Quest1005.BundlesCollected");
	}
}
