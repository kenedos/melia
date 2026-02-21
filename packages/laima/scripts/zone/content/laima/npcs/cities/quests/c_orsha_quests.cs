//--- Melia Script ----------------------------------------------------------
// Orsha Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Orsha for post-demon war storyline.
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

public class OrshaQuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// War Veteran Commander Theron
		//-------------------------------------------------------------------------
		AddNpc(20113, "[Commander] Theron", "c_orsha", -1050, -538, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Orsha", 2001);
			var testimonyQuestId = new QuestId("Fedimian", 3001);
			var unitySummitQuestId = new QuestId("Fedimian", 3003);

			dialog.SetTitle("Theron");

			// Handle Unity Summit quest
			if (character.Quests.IsActive(unitySummitQuestId))
			{
				var deliveredOrsha = character.Variables.Perm.GetBool("Laima.Quests.Fedimian.Quest3003.DeliveredOrshaContract", false);

				if (deliveredOrsha)
				{
					await dialog.Msg("I've already signed the cooperation contract. Deliver it to Agatha in Fedimian.");
					await dialog.Msg("Orsha stands ready to coordinate with the other cities.");
					return;
				}
				else if (character.Inventory.HasItem(650633))
				{
					await dialog.Msg("A cooperation contract from Fedimian? Let me review this...");
					await dialog.Msg("{#666666}*He reads the contract with a stern expression*{/}");
					await dialog.Msg("A Unity Summit between the three cities... Hmph. Politicians and diplomats talking instead of acting.");
					await dialog.Msg("But I suppose talking is better than fighting. Orsha needs allies, not more enemies.");
					await dialog.Msg("Tell Agatha that Commander Theron accepts on behalf of Orsha. We'll send our military advisors.");
					await dialog.Msg("If nothing else, it's an opportunity to coordinate our defenses against the remaining monster threats.");

					character.Inventory.Remove(650633, 1, InventoryItemRemoveMsg.Given);
					character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3003.DeliveredOrshaContract", true);
					character.Quests.CompleteObjective(unitySummitQuestId, "inviteOrsha");

					var contractsDelivered = character.Variables.Perm.GetInt("Laima.Quests.Fedimian.Quest3003.UnityContractsDelivered", 0);
					character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3003.UnityContractsDelivered", contractsDelivered + 1);

					character.ServerMessage($"Unity contracts delivered: {contractsDelivered + 1}/2");

					if (contractsDelivered + 1 >= 2)
					{
						character.ServerMessage("{#FFD700}All contracts delivered! Return to Agatha in Fedimian.{/}");
					}

					return;
				}
			}

			// Handle Fedimian testimony quest
			if (character.Quests.IsActive(testimonyQuestId))
			{
				var recorded = character.Variables.Perm.GetBool("Laima.Quests.Fedimian.Quest3001.TestimonyCommanderTheron", false);

				if (!recorded && character.Inventory.HasItem(650282))
				{
					await dialog.Msg("{#666666}*The grizzled commander regards you with interest*{/}");
					await dialog.Msg("Recording testimonies from the war? The Grand Archive is doing important work.");

					if (await dialog.YesNo("Would you like to hear my account as a military commander who fought on the front lines?"))
					{
						await dialog.Msg("{#666666}*Commander Theron speaks with military precision into the recording crystal*{/}");
						await dialog.Msg("'I am Commander Theron of Orsha. I led the defense of our northern borders during the demon war.'");
						await dialog.Msg("'We lost many good soldiers - friends, comrades, people I trained myself. But we held the line.'");
						await dialog.Msg("'Orsha stands strong because we never gave up. That's what makes a true soldier.'");
						await dialog.Msg("'The war cost us dearly, but it taught us that unity and determination can overcome even the darkest threats.'");

						character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3001.TestimonyCommanderTheron", true);
						character.Quests.CompleteObjective(testimonyQuestId, "testimony1");

						var testimonies = character.Variables.Perm.GetInt("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected", 0);
						character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected", testimonies + 1);

						character.ServerMessage($"War testimonies recorded: {testimonies + 1}/3");

						if (testimonies + 1 >= 3)
						{
							character.ServerMessage("{#FFD700}All testimonies collected! Return to Grand Archivist Elwen in Fedimian.{/}");
						}

						await dialog.Msg("Make sure the Grand Archive preserves these accounts. Future generations must learn from our experiences.");
						return;
					}
				}
				else if (recorded)
				{
					await dialog.Msg("I've already given my testimony. Share it with the Grand Archive.");
					return;
				}
			}

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Are you headed to Izoliacjia Plateau? These used to be the Orsha Military's old training grounds...");
				await dialog.Msg("{#666666}*The grizzled commander gazes toward the plateau with a distant look*{/}");
				await dialog.Msg("We held those lands for generations. Every soldier in Orsha trained there - myself included.");

				var response = await dialog.Select("But after the demon war, we lost control. The area became overrun with monsters. Now they roam freely where our soldiers once marched.",
					Option("I could help reclaim those lands", "help"),
					Option("Tell me about the war", "war"),
					Option("I'll be going now", "leave")
				);

				switch (response)
				{
					case "help":
						if (character.Level < 30)
						{
							await dialog.Msg("{#666666}*He shakes his head*{/}");
							await dialog.Msg("Sorry, but I would need you to be stronger to handle this. You know, these monsters are dangerous.");
							await dialog.Msg("Come back when you're at least level 30, and we'll talk about reclaiming our lands.");
						}
						else
						{
							await dialog.Msg("{#666666}*He clears his throat*{/}");
							await dialog.Msg("Would you help me reclaim these lands? They have been overrun by monsters.");
							await dialog.Msg("If we could clear out the beasts, we could restore the training grounds and rebuild our military strength.");

							if (await dialog.YesNo("Drive back the monsters and I'll personally ensure you're rewarded for your service. Will you help?"))
							{
								character.Quests.Start(questId);
								await dialog.Msg("Excellent! Head to Izoliacjia Plateau and drive back the Yakmap and Yakmambo infesting our old training grounds.");
								await dialog.Msg("When you return victorious, Orsha will be one step closer to reclaiming our heritage. Move out!");
							}
						}
						break;

					case "war":
						await dialog.Msg("{#666666}*His expression darkens*{/}");
						await dialog.Msg("The demon war... we lost too many good soldiers. Friends, comrades, people I trained myself.");
						await dialog.Msg("But we survived. Orsha stands strong because we never gave up. That's what makes a true soldier.");
						break;

					case "leave":
						await dialog.Msg("Dismissed! Come back when you're ready to serve.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killYakmap", out var yakmapObj)) return;
				if (!quest.TryGetProgress("killYakmambo", out var yakmamboObj)) return;

				var yakmapProgress = yakmapObj.Count;
				var yakmamboProgress = yakmamboObj.Count;
				var yakmapDone = yakmapObj.Done;
				var yakmamboDone = yakmamboObj.Done;

				if (!yakmapDone || !yakmamboDone)
				{
					await dialog.Msg($"The training grounds aren't clear yet!");
					await dialog.Msg($"Yakmap eliminated: {yakmapProgress}/40");
					await dialog.Msg($"Yakmambo eliminated: {yakmamboProgress}/5");
					await dialog.Msg("Our recruits depend on having safe training facilities. Get back out there!");
				}
				else
				{
					await dialog.Msg("Outstanding work! The training grounds are clear and recruits are already using them again.");
					await dialog.Msg("You've proven yourself a capable warrior. Here - these combat techniques served me well in the war.");
					await dialog.Msg("Use them wisely, and maybe you'll live as long as I have!");

					character.Quests.Complete(questId);
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				// Check if Unity Summit was completed
				if (character.Quests.HasCompleted(unitySummitQuestId))
				{
					await dialog.Msg("The Unity Summit proved useful. Our cities are coordinating defenses and sharing resources.");
					await dialog.Msg("Orsha remains strong, and now we have reliable allies. That's good tactics.");
				}
				else
				{
					await dialog.Msg("The recruits are training hard thanks to you. Orsha's military grows stronger every day.");
					await dialog.Msg("You're welcome in our ranks anytime, soldier!");
				}
			}
		});

		// Forest Warden Miriam
		//-------------------------------------------------------------------------
		AddNpc(147473, "[Forest Warden] Miriam", "c_orsha", -994, 409, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Orsha", 2002);

			dialog.SetTitle("Miriam");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("The roads through the forest have become dangerous. Travelers and adventurers are being attacked by monsters.");
				await dialog.Msg("{#666666}*She gestures toward the forest path outside the city*{/}");
				await dialog.Msg("The Woods of the Linked Bridges used to be a safe passage. Now? Dark creatures roam freely, and the ancient purifying statues have lost their power.");

				var response = await dialog.Select("Can we make the roads safe again?",
					Option("How can I help?", "help"),
					Option("What are these purifying statues?", "cause"),
					Option("That sounds difficult", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("There is a way! The ancient purifying statues blessed by the Guardian Owl still stand in the forest.");

						if (await dialog.YesNo("If you could pray at each of the five purifying statues, their holy power will awaken and drive back the monsters. Will you help us?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank the goddesses! The five purifying statues are scattered throughout the Woods of the Linked Bridges.");
							await dialog.Msg("You'll recognize them - they're wooden statues carved in the shape of an owl. Pray at each one to energize them.");
							await dialog.Msg("Be careful out there. The monsters have made the forest their territory.");
						}
						break;

					case "cause":
						await dialog.Msg("Long ago, our ancestors erected purifying statues throughout the forest. They were blessed by priests of the goddess Laima.");
						await dialog.Msg("These statues protected travelers and kept the forest safe. But during the demon war, their holy power was drained.");
						await dialog.Msg("Now they stand dormant, and without their protection, monsters have overrun the roads.");
						break;

					case "leave":
						await dialog.Msg("It is difficult. But someone must protect our travelers. Every day, more people are hurt on those roads.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("prayStatues", out var statuesObj)) return;

				var statuesPrayed = statuesObj.Count;
				var statuesDone = statuesObj.Done;

				if (!statuesDone)
				{
					await dialog.Msg($"Have you prayed at the purifying statues? You've energized {statuesPrayed} out of 5 so far.");
					await dialog.Msg("Look for the wooden statues carved in the shape of an owl. Offer your prayers to awaken their holy power.");
				}
				else
				{
					await dialog.Msg("{#666666}*Her eyes light up with hope*{/}");
					await dialog.Msg("I can already feel the difference! The forest feels safer, and travelers are reporting fewer monster attacks!");
					await dialog.Msg("The purifying statues are glowing with holy light again. The roads will be safe once more!");
					await dialog.Msg("Here - take this as thanks. This magical talisman will protect you from darkness. You've protected countless lives today, traveller.");

					character.Quests.Complete(questId);
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The roads through the forest are safe again! Travelers can pass without fear.");
				await dialog.Msg("The purifying statues protect us once more, thanks to you.");
			}
		});

		// Raymond (Historian)
		//-------------------------------------------------------------------------
		AddNpc(155142, "[Relic Hunter] Raymond", "c_orsha", 956, 136, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Orsha", 2003);

			dialog.SetTitle("Raymond");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("I'm a historian working with the Orsha military. We're searching for fragments of an ancient tombstone.");
				await dialog.Msg("This tombstone holds inscriptions about the old war with the demons - tactical knowledge that could be invaluable.");

				var response = await dialog.Select("What do you seek?",
					Option("Can I assist your search?", "help"),
					Option("Why is this tombstone important?", "importance"),
					Option("I'll be on my way", "leave")
				);

				switch (response)
				{
					case "help":
						if (character.Level < 15)
						{
							await dialog.Msg("{#666666}*He shakes his head apologetically*{/}");
							await dialog.Msg("I appreciate your willingness to help, but the battlefield is extremely dangerous. The corrupted monsters there would overwhelm someone of your current experience.");
							await dialog.Msg("Come back when you're at least level 15, and we can discuss this further. The Orsha military needs capable fighters for this mission.");
						}
						else
						{
							await dialog.Msg("Someone willing to help preserve our military history! The Orsha command will be grateful.");

							if (await dialog.YesNo("The tombstone was destroyed during the demon war, shattered into five pieces. They're scattered across the old battlefield in Nobreer Forest. Can you help me recover them?"))
							{
								character.Quests.Start(questId);
								await dialog.Msg("Excellent! The fragments are scattered across the old battlefield south of Orsha.");
								await dialog.Msg("The area is still dangerous - corrupted monsters guard the ruins. But the stone fragments should be recognizable among the rubble.");
							}
						}
						break;

					case "importance":
						await dialog.Msg("The tombstone belonged to a legendary commander who fought in the first demon war, centuries ago.");
						await dialog.Msg("The inscriptions contain battle formations, demon weaknesses, and tactical knowledge that was lost over time.");
						await dialog.Msg("With demons potentially returning, the Orsha military desperately needs this knowledge. It could save countless lives in future conflicts.");
						break;

					case "leave":
						await dialog.Msg("Safe travels. If you change your mind, the military needs all the help it can get.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var fragments = character.Inventory.CountItem(650732);

				if (fragments >= 5)
				{
					await dialog.Msg("You found them! All five fragments of the ancient tombstone!");
					await dialog.Msg("{#666666}*He carefully examines each piece, his hands trembling with excitement*{/}");
					await dialog.Msg("Remarkable! Even damaged, I can make out parts of the inscriptions. Battle formations... demon weak points... this is exactly what the military needed!");
					await dialog.Msg("The tombstone belonged to Commander Valen, who led Orsha's forces in the first demon war. His tactical genius saved the kingdom.");
					await dialog.Msg("I'll work with military scholars to translate and preserve these inscriptions. This knowledge could be crucial if the demons ever return.");
					await dialog.Msg("You've done Orsha a great service. The military council asked me to give you this reward. Thank you.");

					character.Inventory.Remove(650732, 5, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"You've found {fragments} fragments so far. We need all 5 pieces to restore the tombstone's inscriptions.");
					await dialog.Msg("Keep searching the old battlefield. The stone fragments should stand out among the debris.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The military scholars have finished translating the tombstone inscriptions. The tactical knowledge is already being incorporated into training.");
				await dialog.Msg("Commander Valen's wisdom will help protect Orsha for generations to come, thanks to your help.");
			}
		});

		// Infirmary Healer Tamara
		//-------------------------------------------------------------------------
		AddNpc(147493, "[Healer] Tamara", "c_orsha", -877, -20, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Orsha", 2004);

			dialog.SetTitle("Tamara");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Welcome to the Orsha Infirmary. This building has been converted to treat our soldiers.");
				await dialog.Msg("{#666666}*You hear coughing and groans of pain from inside the building*{/}");

				var response = await dialog.Select("So many wounded from the war... We're doing our best, but we're running low on medicinal herbs.",
					Option("How can I help?", "help"),
					Option("What happened to these soldiers?", "soldiers"),
					Option("I should go", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*Her exhausted face lights up with relief*{/}");
						await dialog.Msg("Bless you! Our soldiers are suffering from battle wounds, infections, and fevers.");

						if (await dialog.YesNo("I need medicinal herbs to create poultices and remedies. Without them, many won't survive. Will you help gather herbs for the infirmary?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! I need 5 Languid Herbs for pain relief, 5 Sweet Herbs for fever, and 5 Fragrant Herbs for infection.");
							await dialog.Msg("These herbs grow in the forests surrounding Orsha. Some monsters carry them as well.");
							await dialog.Msg("Please hurry - every hour without medicine, we lose more soldiers.");
						}
						break;

					case "soldiers":
						await dialog.Msg("These are our brave warriors who fought in the demon war. Battle wounds, demon corruption, festering injuries...");
						await dialog.Msg("The military healers are overwhelmed. This building was hastily converted into an infirmary to handle the overflow.");
						await dialog.Msg("We're doing everything we can, but without proper herbs and medicines, I fear many won't make it.");
						break;

					case "leave":
						await dialog.Msg("May the goddess watch over you. If you change your mind, these soldiers need all the help they can get.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var languidHerbs = character.Inventory.CountItem(650662);
				var sweetHerbs = character.Inventory.CountItem(662020);
				var fragrantHerbs = character.Inventory.CountItem(663261);

				if (languidHerbs >= 30 && sweetHerbs >= 15 && fragrantHerbs >= 30)
				{
					await dialog.Msg("You've brought all the herbs! Thank the goddess!");
					await dialog.Msg("{#666666}*She immediately begins preparing remedies*{/}");
					await dialog.Msg("The Languid Herbs will ease their pain. The Sweet Herbs will break their fevers. The Fragrant Herbs will fight infection.");
					await dialog.Msg("You've saved lives today. These soldiers will survive because of you.");
					await dialog.Msg("Here - take these supplies from the military stores. The Commander approved this reward for helping our wounded.");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"Did you find the herbs? We desperately need them - soldiers are dying in there. Please hurry!");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Thanks to those herbs, many soldiers have recovered. Some are even back on duty already.");
				await dialog.Msg("The infirmary is still crowded, but at least now I have the medicine to treat them properly.");
			}
		});

		// Merchant Guild Representative (receiving letter from Klaipeda quest)
		//-------------------------------------------------------------------------
		AddNpc(20061, "[Merchant Guild] Rose", "c_orsha", 251, 633, 0, async dialog =>
		{
			var character = dialog.Player;
			var klaipeQuestId = new QuestId("Klaipeda", 1004);

			dialog.SetTitle("Rose");

			if (character.Quests.IsActive(klaipeQuestId) && character.Inventory.HasItem(663220))
			{
				await dialog.Msg("Ah, a letter from the Klaipeda Guild! Let me see...");
				await dialog.Msg("{#666666}*She reads the letter carefully*{/}");
				await dialog.Msg("Trade proposals, resource sharing, joint caravans... this is exactly what we need!");
				await dialog.Msg("Tell your guild master we accept. Orsha will resume grain shipments to Klaipeda starting next month.");
				await dialog.Msg("Here's our formal response. Thank you for making this journey.");

				character.Inventory.Remove(663220, 1, InventoryItemRemoveMsg.Given);
				character.Variables.Perm.Set("Laima.Quests.Klaipeda.Quest1004.DeliveredOrshaLetter", true);
				character.Quests.CompleteObjective(klaipeQuestId, "deliverOrsha");
			}
			else if (character.Variables.Perm.GetBool("Laima.Quests.Klaipeda.Quest1004.DeliveredOrshaLetter", false))
			{
				await dialog.Msg("Trade with Klaipeda is resuming nicely. It's good to see cooperation between our cities again.");
			}
			else
			{
				await dialog.Msg("The Merchant Guild is working to restore trade networks across all three cities.");
				await dialog.Msg("War may have divided us, but commerce will bring us back together.");
			}
		});
	}
}

// Quest 2001: Reclaim Training Grounds
//-----------------------------------------------------------------------------
public class OrshaTrainingGroundsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("Orsha", 2001);
		SetName("Reclaim the Training Grounds");
		SetDescription("Clear monsters from Orsha's military training grounds so recruits can train safely.");
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Commander] Theron", "c_orsha");

		AddObjective("killYakmap", "Kill Yakmap", new KillObjective(40, new[] { MonsterId.Yakmab }));
		AddObjective("killYakmambo", "Kill Yakmambo", new KillObjective(5, new[] { MonsterId.Yakmambo }));

		// Rewards
		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(24000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Cards
		AddReward(new ItemReward(640004, 10)); // Large HP Potion
		AddReward(new ItemReward(640007, 10)); // Large SP Potion
		AddReward(new ItemReward(640012, 4));  // Recovery Potion
	}
}

// Quest 2002: Energize the Purifying Statues
//-----------------------------------------------------------------------------
public class GuardiansOfTheForestRoadsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("Orsha", 2002);
		SetName("Guardians of the Forest Roads");
		SetDescription("Pray at the five ancient purifying statues in the Woods of the Linked Bridges to awaken their holy power and protect travelers from monster attacks.");
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Forest Warden] Miriam", "c_orsha");

		// Pray at 5 purifying statues (tracked by VariableCheckObjective)
		AddObjective("prayStatues", "Pray at Purifying Statues",
			new VariableCheckObjective("Laima.Quests.Orsha.Quest2002.PurifyingStatuesAwakened", 5, true));

		// Rewards
		AddReward(new ExpReward(800, 600));
		AddReward(new SilverReward(10000));
		AddReward(new ItemReward(640002, 15)); // HP potions
		AddReward(new ItemReward(640005, 10)); // SP potions
		AddReward(new ItemReward(581126, 1)); // Talisman
		AddReward(new ItemReward(640080, 4)); // Lv1 EXP Cards
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clear prayer flags
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatuesAwakened");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue1");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue2");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue3");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue4");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue5");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Clear prayer flags
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatuesAwakened");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue1");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue2");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue3");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue4");
		character.Variables.Perm.Remove("Laima.Quests.Orsha.Quest2002.PurifyingStatue5");
	}
}

// Quest 2003: Tombstone Fragments
//-----------------------------------------------------------------------------
public class EchoesOfAncientWarQuest : QuestScript
{
	protected override void Load()
	{
		SetId("Orsha", 2003);
		SetName("Echoes of the Ancient War");
		SetDescription("Recover the five fragments of an ancient tombstone containing tactical knowledge from the first demon war.");
		SetLocation("f_whitetrees_21_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("Raymond", "c_orsha");

		// Add quest item drops from specific monsters when quest is active
		AddDrop(650732, 0.1f, MonsterId.Kucarry_Symbani);
		AddDrop(650732, 0.1f, MonsterId.Kucarry_Zeffi);
		AddDrop(650732, 0.1f, MonsterId.Kucarry_Balzer);

		// Collect 5 fragments
		AddObjective("collectFragments", "Collect Tombstone fragments",
			new CollectItemObjective(650732, 5));

		// Rewards
		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Cards
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(640009, 4));  // Stamina Potion
		AddReward(new ItemReward(162111, 1));  // Strong Bow
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove fragments
		character.Inventory.Remove(650732, character.Inventory.CountItem(650732),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove collected fragments
		character.Inventory.Remove(650732, character.Inventory.CountItem(650732),
			InventoryItemRemoveMsg.Destroyed);
	}
}

//-----------------------------------------------------------------------------
// Quests
//-----------------------------------------------------------------------------

// Quest 2004: Healing the Wounded
//-----------------------------------------------------------------------------
public class OrshaInfirmaryQuest : QuestScript
{
	protected override void Load()
	{
		SetId("Orsha", 2004);
		SetName("Healing the Wounded");
		SetDescription("Gather medicinal herbs for the Orsha Infirmary to treat wounded soldiers - Languid Herbs, Sweet Herbs, and Fragrant Herbs.");
		SetLocation("c_orsha");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Healer] Tamara", "c_orsha");

		// Add quest item drops from monsters when quest is active
		AddDrop(650662, 0.3f, MonsterId.Sec_Jukopus);
		AddDrop(650662, 0.3f, MonsterId.Onion_Green);
		AddDrop(662020, 0.3f, MonsterId.Sec_Pokubu);
		AddDrop(662020, 0.3f, MonsterId.Sec_Arburn_Pokubu);
		AddDrop(663261, 0.3f, MonsterId.Sec_Pokubu);
		AddDrop(663261, 0.3f, MonsterId.Sec_Jukopus);

		// Collect herbs
		AddObjective("collectLanguidHerbs", "Collect Languid Herbs",
			new CollectItemObjective(650662, 30));
		AddObjective("collectSweetHerbs", "Collect Sweet Herbs",
			new CollectItemObjective(662020, 15));
		AddObjective("collectFragrantHerbs", "Collect Fragrant Herbs",
			new CollectItemObjective(663261, 30));

		// Rewards
		AddReward(new ExpReward(475, 320));
		AddReward(new SilverReward(4500));
		AddReward(new ItemReward(640080, 4)); // Lv1 EXP Cards
		AddReward(new ItemReward(640002, 10)); // Small HP Potion
		AddReward(new ItemReward(640005, 10)); // Small SP Potion
		AddReward(new ItemReward(640008, 4));  // Small Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(650662, character.Inventory.CountItem(650662),
			InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(662020, character.Inventory.CountItem(662020),
			InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(663261, character.Inventory.CountItem(663261),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(650662, character.Inventory.CountItem(650662),
			InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(662020, character.Inventory.CountItem(662020),
			InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(663261, character.Inventory.CountItem(663261),
			InventoryItemRemoveMsg.Destroyed);
	}
}
