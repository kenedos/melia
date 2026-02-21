//--- Melia Script ----------------------------------------------------------
// Srautas Gorge (Gele Plateau) Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Srautas Gorge for Klaipeda storyline.
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

public class FGele572QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Plateau's Menace - Klaipeda Scout
		//---------------------------------------------------------------------
		AddNpc(147410, "[Klaipeda Scout] Lania", "f_gele_57_2", 16, -426, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_2", 1001);

			dialog.SetTitle("Lania");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Adventurer! Thank the Goddess you're here.");
				await dialog.Msg("{#666666}*The scout looks visibly concerned as she surveys the plateau*{/}");
				await dialog.Msg("I've been stationed here to monitor the trade routes through Srautas Gorge, but something troubling is happening. Three different monster types are converging on this plateau from all directions.");
				await dialog.Msg("The plant-like Leafly creatures from the forests, and two types of aggressive Panto warriors - some wielding swords, others fighting with their hands - are spreading across the entire area.");

				var response = await dialog.Select("Will you help me thin their numbers?",
					Option("I'll hunt them down", "accept"),
					Option("What's causing this?", "info"),
					Option("Not right now", "decline")
				);

				switch (response)
				{
					case "accept":
						if (await dialog.YesNo("These monsters are scattered all over the plateau. You'll need to hunt 15 of each type. Can you handle such an extensive patrol?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Excellent! The Leafly tend to gather in the eastern and western regions. The Panto warriors with swords can be found in the western and eastern areas, while those fighting with their hands spawn mostly in the southeast and scattered central locations.");
							await dialog.Msg("It's a vast plateau, so prepare for a lot of ground to cover. Return to me once you've completed the hunt.");
						}
						break;

					case "info":
						await dialog.Msg("I'm not entirely sure what's drawing them here. This gorge has always been cheerful and peaceful - full of flowers and sunshine. But ever since the demon war ended, monster behavior has been unpredictable.");
						await dialog.Msg("The Pantos seem to be expanding their territory, while the Leafly are migrating in unusual patterns. Whatever the cause, they're making the trade routes dangerous for merchants and travelers.");
						break;

					case "decline":
						await dialog.Msg("I understand. The patrol would cover a massive area. If you change your mind, I'll be here.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;

				var leaflyDone = quest.TryGetProgress("killLeafly", out var leaflyObj) && leaflyObj.Done;
				var swordDone = quest.TryGetProgress("killPantoSword", out var swordObj) && swordObj.Done;
				var handDone = quest.TryGetProgress("killPantoHand", out var handObj) && handObj.Done;

				if (leaflyDone && swordDone && handDone)
				{
					await dialog.Msg("{#666666}*The scout breathes a sigh of relief*{/}");
					await dialog.Msg("Remarkable work! I've been observing from my post - you covered nearly the entire plateau. The monster presence has noticeably decreased already.");
					await dialog.Msg("The trade routes should be safer now, and merchants can resume their journeys through the gorge. Klaipeda owes you a debt of gratitude.");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("The hunt continues? Remember, you need to clear out all three types:");

					if (!leaflyDone)
						await dialog.Msg("- Leafly in the northern and western regions");
					if (!swordDone)
						await dialog.Msg("- Panto warriors with swords in the western and eastern areas");
					if (!handDone)
						await dialog.Msg("- Panto warriors fighting with their hands in the southeast");

					await dialog.Msg("It's a long patrol, but I know you can do it.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The plateau is much safer thanks to your efforts. Klaipeda's merchants can travel freely again.");
			}
		});

		// Quest 1002: Totem Desecration - Klaipeda Researcher
		//---------------------------------------------------------------------
		AddNpc(152058, "[Klaipeda Researcher] Veric", "f_gele_57_2", 1027, 999, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_2", 1002);

			dialog.SetTitle("Veric");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Good day! I'm studying the ecology of this beautiful gorge.");
				await dialog.Msg("{#666666}*He gestures toward the flowers and butterflies around him*{/}");
				await dialog.Msg("Srautas Gorge is known for its cheerful atmosphere - the abundance of flowers, the bright sunshine, the harmonious wildlife. But I've discovered something disturbing.");
				await dialog.Msg("The Panto tribes have erected territorial totems throughout the southern plateau. These markers are attracting more aggressive Pantos to the area and disrupting the natural balance.");

				var response = await dialog.Select("Would you be willing to help?",
					Option("I'll destroy the totems", "accept"),
					Option("Why are totems harmful?", "info"),
					Option("Perhaps later", "decline")
				);

				switch (response)
				{
					case "accept":
						if (await dialog.YesNo("I need you to head south from here and destroy 6 of these Panto totems. Will you take on this task?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! The totems are concentrated in the southern region of the plateau. You'll find them scattered across the Panto encampments.");
							await dialog.Msg("They're not dangerous themselves - just wooden structures - but they symbolize Panto territorial claims. Destroying them will discourage the Pantos from gathering in such large numbers.");
						}
						break;

					case "info":
						await dialog.Msg("The totems themselves aren't magical, but they serve as territorial markers. When Pantos erect these totems, it signals to other Panto tribes that the area is claimed territory.");
						await dialog.Msg("This attracts more Pantos seeking to join the settlement, which leads to overpopulation and aggressive behavior toward travelers. By removing the totems, we're essentially erasing their territorial claims.");
						break;

					case "decline":
						await dialog.Msg("I understand. It's quite a trek to the southern plateau. If you change your mind, I'll be here continuing my research.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;

				if (quest.TryGetProgress("destroyTotems", out var totemObj) && totemObj.Done)
				{
					await dialog.Msg("{#666666}*Veric looks pleased*{/}");
					await dialog.Msg("Excellent work! I've already noticed a change in the Panto behavior. With their territorial markers destroyed, they're dispersing from the southern plateau.");
					await dialog.Msg("The gorge's natural harmony is beginning to restore itself. The flowers seem brighter already!");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("Still working on destroying those totems? Head south from here - you'll find them concentrated throughout the Panto territory.");
					await dialog.Msg("They're easy to spot once you reach the encampments.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The gorge's ecosystem is recovering nicely thanks to your help. Nature finds a way when we give it a chance.");
			}
		});

		// Quest 1003: Message to the Hillside - Klaipeda Messenger (Start)
		//---------------------------------------------------------------------
		AddNpc(20127, "[Klaipeda Messenger] Gareth", "f_gele_57_2", 191, 1341, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_2", 1003);
			var letterId = 666037;

			dialog.SetTitle("Gareth");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Adventurer! Perfect timing.");
				await dialog.Msg("{#666666}*The messenger clutches a sealed letter urgently*{/}");
				await dialog.Msg("I'm stationed here at the border crossing, but I have an urgent message that needs to reach our forward scout in Nefritas Cliff immediately. Unfortunately, I cannot leave my post.");
				await dialog.Msg("The recipient is stationed at Uzbaigi Hillside, deep within Nefritas Cliff. The warp is just ahead - it's quite a long journey to reach him.");

				var response = await dialog.Select("Can you deliver this message?",
					Option("I'll deliver it", "accept"),
					Option("What's in the letter?", "info"),
					Option("That sounds far", "decline")
				);

				switch (response)
				{
					case "accept":
						if (await dialog.YesNo("The letter is sealed and urgent. It's a long walk through Nefritas Cliff. Can you handle this?"))
						{
							character.Quests.Start(questId);
							character.Inventory.Add(letterId, 1, InventoryAddType.PickUp);

							await dialog.Msg("Thank you! The warp to Nefritas Cliff is just north of here. Once you arrive, head northeast toward the Uzbaigi Hillside.");
							await dialog.Msg("Look for a scout wearing Klaipeda colors - his name is Roderic. He'll be watching for threats near the hillside's edge.");
							await dialog.Msg("Be wary of the Banshees that have been lurking along the path. Those ghosts can be quite troublesome.");
						}
						break;

					case "info":
						await dialog.Msg("I'm afraid the contents are confidential - sealed orders from Klaipeda command. All I can tell you is that it's time-sensitive intelligence about monster movements in the region.");
						await dialog.Msg("Roderic will know what to do with it when you arrive.");
						break;

					case "decline":
						await dialog.Msg("I understand. It's quite a long journey. If you reconsider, I'll be here.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (character.Inventory.HasItem(letterId))
				{
					await dialog.Msg("You still have the letter? The warp to Nefritas Cliff is just north of here. Find Roderic at Uzbaigi Hillside - head northeast once you arrive.");
					await dialog.Msg("Time is of the essence!");
				}
				else
				{
					await dialog.Msg("{#666666}*The messenger looks confused*{/}");
					await dialog.Msg("You've lost the sealed letter? That's... concerning. Let me give you another copy, but please be more careful this time!");

					character.Inventory.Add(letterId, 1, InventoryAddType.PickUp);
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Thank you for delivering that message. Roderic's intelligence reports are vital for Klaipeda's security efforts.");
			}
		});

		// Delivery Recipient NPC for f_gele_57_1 Quest 1004
		//---------------------------------------------------------------------
		AddNpc(147416, "[Scout] Miriam", "f_gele_57_2", 1320, -272, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_1", 1004);

			dialog.SetTitle("Miriam");

			if (character.Quests.IsActive(questId) && character.Inventory.HasItem(667122))
			{
				await dialog.Msg("{#666666}*Her tired face lights up with relief*{/}");
				await dialog.Msg("Emergency supplies! Thank the goddesses you made it through!");
				await dialog.Msg("We've been rationing for days. This food will keep the outpost going until proper caravans arrive.");
				await dialog.Msg("You've saved lives today, traveler. Here - take this reward. It's all we can spare, but you've earned it.");

				character.Inventory.Remove(667122, 1, InventoryItemRemoveMsg.Given);
				character.Quests.Complete(questId);
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The supplies you brought lasted us three full days. We're doing better now.");
				await dialog.Msg("If you're heading back to the gorge, let Roland know we're grateful for his coordination.");
			}
			else
			{
				await dialog.Msg("Are you headed to Grynas Hills? Be careful, that forest is haunted by spirits and dark creatures.");
				await dialog.Msg("I'm on duty to prevent threats coming from the forest and help on the logistics. We have a guard camp deep in that place.");
				await dialog.Msg("We're managing, but supply deliveries from the gorge would help tremendously.");
				await dialog.Msg("The path is treacherous for wagons. We rely on brave couriers like yourself.");
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Plateau's Menace
//-----------------------------------------------------------------------------

public class PlateausMenaceQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_2", 1001);
		SetName("Plateau's Menace");
		SetDescription("Help Klaipeda Scout Lania thin the monster population across Srautas Gorge by hunting Leafly, Panto warriors with swords, and Panto warriors with their hands.");
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Klaipeda Scout] Lania", "f_gele_57_2");
		// Objectives
		AddObjective("killLeafly", "Hunt Leafly",
			new KillObjective(15, new[] { MonsterId.Leafly }));

		AddObjective("killPantoSword", "Hunt Panto Warriors (Sword)",
			new KillObjective(15, new[] { MonsterId.Npanto_Sword }));

		AddObjective("killPantoHand", "Hunt Panto Warriors (Hand)",
			new KillObjective(15, new[] { MonsterId.Npanto_Hand }));

		// Rewards
		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(24000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Cards
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Normal Recovery Potion
		AddReward(new ItemReward(112006, 1)); // Sketis Dagger
	}
}

// Quest 1002 CLASS: Totem Desecration
//-----------------------------------------------------------------------------

public class TotemDesecrationQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_2", 1002);
		SetName("Totem Desecration");
		SetDescription("Help Klaipeda Researcher Veric restore the gorge's natural balance by destroying Panto territorial totems in the southern plateau.");
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Klaipeda Researcher] Veric", "f_gele_57_2");
		// Objectives
		AddObjective("destroyTotems", "Destroy Panto Totems",
			new KillObjective(6, new[] { MonsterId.Mon_Goat_Totem_Q }));

		// Rewards
		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
	}
}

// Quest 1003 CLASS: Message to the Hillside
//-----------------------------------------------------------------------------

public class MessageToTheHillsideQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_2", 1003);
		SetName("Message to the Hillside");
		SetDescription("Deliver an urgent sealed letter from Klaipeda Messenger Gareth to Hillside Watcher Roderic in Nefritas Cliff's Uzbaigi Hillside.");
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Klaipeda Messenger] Gareth", "f_gele_57_2");
		// Objectives
		AddObjective("deliverLetter", "Deliver letter to Roderic in Nefritas Cliff",
			new ManualObjective());

		// Rewards
		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(24000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 12)); // Normal HP Potion
		AddReward(new ItemReward(640006, 12)); // Normal SP Potion
		AddReward(new ItemReward(640011, 5)); // Recovery Potion
		AddReward(new ItemReward(924014, 1)); // Recipe - Hunting Bow
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clean up letter item
		character.Inventory.Remove(666037, character.Inventory.CountItem(666037),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Clean up letter item
		character.Inventory.Remove(666037, character.Inventory.CountItem(666037),
			InventoryItemRemoveMsg.Destroyed);
	}
}
