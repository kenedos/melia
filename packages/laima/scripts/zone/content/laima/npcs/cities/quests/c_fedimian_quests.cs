//--- Melia Script ----------------------------------------------------------
// Fedimian Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Fedimian for post-demon war storyline.
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

public class FedimianQuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Grand Archivist Elwen
		//-------------------------------------------------------------------------
		AddNpc(150183, "[Grand Archivist] Elwen", "c_fedimian", -764, 135, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Fedimian", 3001);
			var mausoleumQuestId = new QuestId("Fedimian", 3004);

			dialog.SetTitle("Elwen");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Welcome to Fedimian's Grand Archive. Here we preserve the knowledge and history of our world.");
				await dialog.Msg("{#666666}*She gestures to countless scrolls and books lining the walls*{/}");
				await dialog.Msg("I'm compiling a comprehensive history of the demon war. Future generations must understand what happened - both the horrors and the heroism.");

				var response = await dialog.Select("How can I assist?",
					Option("I'd like to contribute", "help"),
					Option("What kind of history are you compiling?", "history"),
					Option("Just browsing", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("Truly? I need someone to seek out war survivors and collect their testimonies.");

						if (await dialog.YesNo("There are three key witnesses I've identified. Would you be willing to interview them and record their stories?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Wonderful! Here's a recording crystal. It will preserve their words exactly as spoken.");
							await dialog.Msg("The witnesses are: Commander Theron in Orsha, Peregrin Lyon at Ramstis Ridge, and Wizard Silas here in Fedimian.");
							await dialog.Msg("Ask them about their experiences during the war. The crystal will do the rest.");

							// Give recording crystal
							character.Inventory.Add(650282, 1, InventoryAddType.PickUp);
						}
						break;

					case "history":
						await dialog.Msg("I'm gathering firsthand accounts from those who lived through the war - soldiers, civilians, survivors.");
						await dialog.Msg("Each person's story is unique. Together, they form a complete picture of what our world endured.");
						await dialog.Msg("But many survivors won't come forward. They're scattered across the land, and their memories are painful.");
						break;

					case "leave":
						await dialog.Msg("Feel free to browse our collection. Knowledge should be shared.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var testimonies = character.Variables.Perm.GetInt("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected", 0);

				if (testimonies >= 3)
				{
					await dialog.Msg("You've collected all three testimonies! Let me review them...");
					await dialog.Msg("{#666666}*She holds the crystal, tears streaming down her face as she listens*{/}");
					await dialog.Msg("These stories... such pain, such courage. This is exactly what we need to preserve.");
					await dialog.Msg("Future generations will learn from these accounts. They'll understand the true cost of war, and hopefully prevent another one.");
					await dialog.Msg("You've made an invaluable contribution to our history. Thank you.");

					character.Inventory.Remove(650282, 1, InventoryItemRemoveMsg.Given);
					character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected");
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"You've collected {testimonies} out of 3 testimonies so far.");
					await dialog.Msg("Remember: Commander Theron in Orsha, Peregrin Lyon at Ramstis Ridge, and Wizard Silas here in Fedimian.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				// Offer quest 3004 if player hasn't started it yet
				if (!character.Quests.Has(mausoleumQuestId) && character.Level >= 20)
				{
					await dialog.Msg("Your work on the War Chronicles has been invaluable. The testimonies you gathered will educate generations.");
					await dialog.Msg("{#666666}*She pauses, studying you with keen eyes*{/}");

					var response = await dialog.Select("Your dedication and trustworthiness have not gone unnoticed. I have another matter - one of great importance and sensitivity.",
						Option("I'm willing to help", "help"),
						Option("Tell me more", "more"),
						Option("I'm not interested", "leave")
					);

					switch (response)
					{
						case "help":
						case "more":
							await dialog.Msg("The Royal Mausoleum in the Ramstis region contains ancient relics from Fedimian's past.");
							await dialog.Msg("Our sages have been investigating reports of a powerful relic documented by the great scholar Kepeck.");
							await dialog.Msg("However, the mausoleum is sealed and filled with dangerous creatures. Only those with official permission may enter.");

							if (await dialog.YesNo("I would entrust you with the Great King's Key - proof of your authority to enter the Royal Mausoleum. Will you venture there and recover Kepeck's Relic Memorandum?"))
							{
								character.Quests.Start(mausoleumQuestId);
								await dialog.Msg("Excellent! This key proves you act with the Archive's full authority.");
								await dialog.Msg("Travel to the Royal Mausoleum entrance in Ramstis Ridge. Show the key to the guard, and they will grant you passage.");
								await dialog.Msg("Inside, search for Kepeck's Relic Memorandum - scrolls documenting a powerful relic. Be careful - the mausoleum is dangerous.");

								// Give Great King's Key
								character.Inventory.Add(650244, 1, InventoryAddType.PickUp);
							}
							else
							{
								await dialog.Msg("I understand. The mausoleum is perilous. Come back if you change your mind.");
							}
							break;

						case "leave":
							await dialog.Msg("I understand. Should you change your mind, I'll be here.");
							break;
					}
				}
				else if (character.Quests.IsActive(mausoleumQuestId))
				{
					var hasMemoranda = character.Inventory.CountItem(650657);

					if (hasMemoranda >= 3)
					{
						await dialog.Msg("You've recovered Kepeck's Relic Memoranda! Let me examine them...");
						await dialog.Msg("{#666666}*She carefully unrolls the ancient scrolls*{/}");
						await dialog.Msg("Remarkable! These contain detailed descriptions of a powerful relic that could benefit all of Fedimian.");
						await dialog.Msg("Your courage in entering the Royal Mausoleum has uncovered knowledge we thought lost forever.");
						await dialog.Msg("You have my deepest gratitude. Here - you've more than earned this reward.");

						character.Quests.Complete(mausoleumQuestId);
					}
					else
					{
						await dialog.Msg($"You've recovered {hasMemoranda} out of 3 Relic Memoranda.");
						await dialog.Msg("Search the Royal Mausoleum thoroughly. Kepeck's scrolls should be hidden within.");
					}
				}
				else if (character.Quests.HasCompleted(mausoleumQuestId))
				{
					await dialog.Msg("Thanks to your efforts, we've recovered Kepeck's research on the ancient relic.");
					await dialog.Msg("The Royal Mausoleum remains open to you should you wish to explore it further.");
				}
				else
				{
					await dialog.Msg("The War Chronicles are nearly complete, thanks to your help. Scholars from across the world come to study them.");
					await dialog.Msg("History lives through those who remember and record it.");
				}
			}
		});

		// Divine Restoration - Scholar Lysander
		//-------------------------------------------------------------------------
		AddNpc(20113, "[Divine Scholar] Lysander", "c_fedimian", -442, 164, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Fedimian", 3002);

			dialog.SetTitle("Lysander");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Welcome. I'm Lysander, devoted to restoring divine protection to our three great cities.");
				await dialog.Msg("{#666666}*His eyes burn with intense dedication*{/}");

				var response = await dialog.Select("The demon war left lingering corruption in Klaipeda, Orsha, and even here in Fedimian. We need purification rituals to cleanse these traces.",
					Option("How can I help?", "help"),
					Option("Tell me about the purification", "purification"),
					Option("I see. Good luck with that.", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("Excellent! I need your help recovering ancient knowledge.");
						await dialog.Msg("Long ago, an old druid master lived here in Fedimian. He created powerful purification scrolls to cleanse demonic corruption.");

						if (await dialog.YesNo("These scrolls were scattered and lost during the war. I believe monsters in Ramstis Ridge have found them. Will you venture there and recover 5 purification scrolls?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Thank you! The monsters in Ramstis Ridge - particularly near the old druid's hermitage - should have the scrolls.");
							await dialog.Msg("The druid master studied corruption extensively. His scrolls are our best hope for purifying the lingering demonic traces.");
							await dialog.Msg("Once you collect 5 Purification Scrolls, return to me. We'll use them to help all three cities!");
						}
						break;

					case "purification":
						await dialog.Msg("The old druid master was a brilliant scholar who lived in Fedimian centuries ago.");
						await dialog.Msg("He studied demonic corruption and created purification rituals recorded on sacred scrolls.");
						await dialog.Msg("When the demon war erupted, his research became invaluable - but the scrolls were scattered and lost.");
						await dialog.Msg("Now we need them more than ever to cleanse what the war left behind.");
						break;

					case "leave":
						await dialog.Msg("The corruption won't cleanse itself. But I understand if you have other priorities.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var scrollCount = character.Inventory.CountItem(667024);

				if (scrollCount >= 15)
				{
					await dialog.Msg("You've recovered the purification scrolls! This is remarkable!");
					await dialog.Msg("{#666666}*He carefully examines the ancient parchments*{/}");
					await dialog.Msg("These are genuine! The druid master's own handwriting... his knowledge of purification magic was unmatched.");
					await dialog.Msg("With these scrolls, we can perform cleansing rituals in Klaipeda, Orsha, and Fedimian.");
					await dialog.Msg("The demon war's corruption will finally be purged from our cities!");
					await dialog.Msg("You've helped all three cities today. Accept this reward - you've more than earned it.");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"You've collected {scrollCount} out of 5 Purification Scrolls so far.");
					await dialog.Msg("Search Ramstis Ridge thoroughly. The monsters there have found the old druid's lost scrolls.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Thanks to the purification scrolls you recovered, we've cleansed the corruption in all three cities!");
				await dialog.Msg("Klaipeda, Orsha, and Fedimian are safer now. The old druid master's legacy lives on.");
			}
		});

		// Peace Envoy Agatha
		//-------------------------------------------------------------------------
		AddNpc(151080, "[Peace Envoy] Agatha", "c_fedimian", 418, 131, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Fedimian", 3003);

			dialog.SetTitle("Agatha");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("Klaipeda, Orsha, and Fedimian - three great cities that once worked together. Now we barely communicate.");
				await dialog.Msg("The war left wounds beyond the physical. Old rivalries resurfaced, trust was broken.");
				await dialog.Msg("But we're stronger together. If we can't unite now, during reconstruction, how will we face the next crisis?");

				var response = await dialog.Select("Can the cities be unified?",
					Option("I want to help unite them", "help"),
					Option("What divides them?", "divisions"),
					Option("Some things can't be fixed", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("A champion of unity! This is exactly what we need!");

						if (await dialog.YesNo("I need someone to organize a Unity Summit - bring representatives from all three cities together. Will you help arrange it?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Excellent! First, deliver these cooperation contracts to the other city leaders.");
							await dialog.Msg("Visit Mayor Oswald in Klaipeda and Commander Theron in Orsha. Convince them to send delegations here to Fedimian.");
							await dialog.Msg("It won't be easy - old grudges run deep. But if anyone can bring them together, it's someone like you - an outsider they all respect.");

							// Give contracts
							character.Inventory.Add(650633, 2, InventoryAddType.PickUp);
						}
						break;

					case "divisions":
						await dialog.Msg("Klaipeda focuses on commerce and rebuilding. Orsha maintains military strength and agricultural production. Fedimian pursues knowledge and diplomacy.");
						await dialog.Msg("These different priorities create friction. Each city thinks their approach is best.");
						await dialog.Msg("But we need all three - trade, defense, and knowledge. United, we could thrive. Divided, we merely survive.");
						break;

					case "leave":
						await dialog.Msg("Perhaps you're right. But I have to try. The future of our world depends on cooperation.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var contractsDelivered = character.Variables.Perm.GetInt("Laima.Quests.Fedimian.Quest3003.UnityContractsDelivered", 0);

				if (contractsDelivered >= 2)
				{
					await dialog.Msg("Both Klaipeda and Orsha have signed the contracts! This is historic!");
					await dialog.Msg("{#666666}*The summit hall fills with representatives from all three cities*{/}");
					await dialog.Msg("It's tense at first, but as they discuss shared challenges - monsters, refugees, trade - common ground emerges.");
					await dialog.Msg("By the end, they've formalized the Mutual Aid Pact. Each city will support the others in times of need.");
					await dialog.Msg("This is just the beginning, but you've helped take the first step toward true unity.");
					await dialog.Msg("The world is safer because of what you've done today.");

					character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3003.UnityContractsDelivered");
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"You've delivered {contractsDelivered} out of 2 contracts.");
					await dialog.Msg("Find Mayor Oswald in Klaipeda and Commander Theron in Orsha. They need to sign the cooperation contracts.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The Mutual Aid Pact is working beautifully. We've already coordinated three joint operations against monster threats.");
				await dialog.Msg("You proved that unity is possible. Thank you.");
			}
		});


		// Merchant Guild Representative (receiving letter from Klaipeda quest)
		//-------------------------------------------------------------------------
		AddNpc(147345, "[Merchant Guild] Anne", "c_fedimian", 542, -46, 0, async dialog =>
		{
			var character = dialog.Player;
			var klaipeQuestId = new QuestId("Klaipeda", 1004);
			var crossroadsQuestId = new QuestId("f_rokas_31", 1001);

			dialog.SetTitle("Anne");

			// Handle Zachariel Crossroads warning quest
			if (character.Quests.IsActive(crossroadsQuestId))
			{
				if (!character.Quests.TryGetById(crossroadsQuestId, out var quest)) return;

				// Check if kill objective is done
				var hogmasKilled = false;
				if (quest.TryGetProgress("killHogmas", out var killObj))
				{
					hogmasKilled = killObj.Done;
				}

				// Check if already delivered
				var messageDelivered = false;
				if (quest.TryGetProgress("deliverWarning", out var deliveryObj))
				{
					messageDelivered = deliveryObj.Done;
				}

				if (hogmasKilled && !messageDelivered)
				{
					await dialog.Msg("You have urgent news? Please, tell me.");
					await dialog.Msg("{#666666}*She listens intently*{/}");
					await dialog.Msg("The Zachariel Crossroads route is being overrun by Warleader Hogmas? This is serious!");
					await dialog.Msg("That's one of our key trade routes to Ramstis Ridge and the Royal Mausoleum. If it's compromised, we'll need to reroute all caravans.");
					await dialog.Msg("Thank you for this warning. I'll inform the Guildmaster immediately and arrange for additional guards on future expeditions.");

					character.Quests.CompleteObjective(crossroadsQuestId, "deliverWarning");
					character.ServerMessage("{#FFD700}Warning delivered! Return to Darius at Zachariel Crossroads.{/}");
					return;
				}
				else if (!hogmasKilled)
				{
					await dialog.Msg("You look like you have something to say, but it seems you're still handling a situation. Come back when you're ready.");
					return;
				}
			}

			// Handle Klaipeda merchant quest
			if (character.Quests.IsActive(klaipeQuestId) && character.Inventory.HasItem(663221))
			{
				await dialog.Msg("A letter from Klaipeda's Merchant Guild? How delightful!");
				await dialog.Msg("{#666666}*She reads eagerly*{/}");
				await dialog.Msg("Resource sharing agreements, academic exchange programs, joint ventures... this is precisely what Fedimian needs!");
				await dialog.Msg("We accept their proposal wholeheartedly. Fedimian will provide academic resources and magical research in exchange for goods.");
				await dialog.Msg("Here's our formal acceptance. Thank you for facilitating this connection!");

				character.Inventory.Remove(663221, 1, InventoryItemRemoveMsg.Given);
				character.Variables.Perm.Set("Laima.Quests.Klaipeda.Quest1004.DeliveredFedimianLetter", true);
				character.Quests.CompleteObjective(klaipeQuestId, "deliverFedimian");
			}
			else if (character.Variables.Perm.GetBool("Laima.Quests.Klaipeda.Quest1004.DeliveredFedimianLetter", false))
			{
				await dialog.Msg("Trade with Klaipeda has opened wonderful opportunities. We've exchanged dozens of scholars and merchants already!");
			}
			else
			{
				await dialog.Msg("Fedimian's Merchant Guild specializes in knowledge trade - books, research, magical training.");
				await dialog.Msg("We may not have Orsha's food or Klaipeda's goods, but our intellectual resources are unmatched.");
			}
		});

		// =====================================================================
		// EASTERN HERMIT - WAR TESTIMONY (Quest: Fedimian/3001)
		// =====================================================================
		// Survived temple siege, witnessed divine intervention
		// =====================================================================

		var testimonyQuestId = new QuestId("Fedimian", 3001);

		// Wizard Silas NPC
		//-------------------------------------------------------------------------
		AddNpc(153213, "[Wizard] Silas", "c_fedimian", 784, 239, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle("Silas");

			if (!character.Quests.Has(testimonyQuestId))
			{
				await dialog.Msg("{#666666}*The air is thick with incense smoke rising from a large cauldron*{/}");
				await dialog.Msg("I am Silas, a wizard who survived the great war. These protective wards and incense help keep the dark spirits at bay.");
				await dialog.Msg("The demons may be gone, but their corruption lingers. I've learned to ward against it.");

				var response = await dialog.Select("What knowledge do you possess?",
					Option("Tell me about the war", "story"),
					Option("What is the incense for?", "incense"),
					Option("I'll leave you to your work", "leave")
				);

				switch (response)
				{
					case "story":
						await dialog.Msg("I fought in the great war as a battle wizard. I witnessed things that still haunt my dreams.");
						await dialog.Msg("Demons, corruption, cities burning... but I survived through knowledge and preparation.");
						await dialog.Msg("Now I use what I learned to protect this place. The incense, the wards - they keep the darkness away.");
						await dialog.Msg("I may have survived, but I'll never forget what was lost.");
						break;

					case "incense":
						await dialog.Msg("This incense is made from sacred herbs blessed with protective magic.");
						await dialog.Msg("It purifies the air and repels malevolent spirits. After the war, such precautions became necessary.");
						await dialog.Msg("The cauldron never stops burning. As long as it does, this place remains safe from corruption.");
						break;

					case "leave":
						await dialog.Msg("Stay vigilant. The darkness never truly rests.");
						break;
				}
			}
			else if (character.Quests.IsActive(testimonyQuestId))
			{
				var recorded = character.Variables.Perm.GetBool("Laima.Quests.Fedimian.Quest3001.TestimonyWizardSilas", false);

				if (recorded)
				{
					await dialog.Msg("My testimony is recorded. Share it with the Grand Archive.");
					await dialog.Msg("The world must know what we endured, and how we survived.");
				}
				else
				{
					if (!character.Inventory.HasItem(650282))
					{
						await dialog.Msg("You need a recording crystal to preserve my testimony.");
						return;
					}

					await dialog.Msg("You're documenting the war for posterity? Important work.");

					if (await dialog.YesNo("Would you like to hear my account as a battle wizard who survived the great war?"))
					{
						await dialog.Msg("{#666666}*Silas speaks solemnly into the recording crystal*{/}");
						await dialog.Msg("'I am Silas, battle wizard. I fought in the great war and witnessed horrors beyond imagination.'");
						await dialog.Msg("'Cities burned, people died, and demons seemed unstoppable. But we fought on with magic and steel.'");
						await dialog.Msg("'I survived through preparation, knowledge, and protective wards. Now I use what I learned to keep the darkness at bay.'");
						await dialog.Msg("'The war may be over, but its corruption lingers. We must remain vigilant, always.'");

						character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3001.TestimonyWizardSilas", true);
						character.Quests.CompleteObjective(testimonyQuestId, "testimony3");

						var testimonies = character.Variables.Perm.GetInt("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected", 0);
						character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected", testimonies + 1);

						character.ServerMessage($"War testimonies recorded: {testimonies + 1}/3");

						if (testimonies + 1 >= 3)
						{
							character.ServerMessage("{#FFD700}All testimonies collected! Return to Grand Archivist Elwen in Fedimian.{/}");
						}
					}
					else
					{
						await dialog.Msg("I understand. Return when you're ready to hear my account.");
					}
				}
			}
			else
			{
				await dialog.Msg("Thank you for preserving the truth in Fedimian's Grand Archive.");
				await dialog.Msg("Future generations will know the goddesses stood with us.");
			}
		});
	}

}

//-----------------------------------------------------------------------------
// Quests
//-----------------------------------------------------------------------------

// Quest 3001: War Chronicles
//-----------------------------------------------------------------------------
public class FedimianWarChroniclesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("Fedimian", 3001);
		SetName("Chronicles of the Demon War");
		SetDescription("Collect testimonies from war survivors to complete the Grand Archive's historical record.");
		SetLocation("c_fedimian");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Grand Archivist] Elwen", "c_fedimian");

		// Collect 3 testimonies (tracked manually)
		AddObjective("testimony1", "Record testimony from Commander Theron in Orsha",
			new ManualObjective());
		AddObjective("testimony2", "Record testimony from Peregrin Lyon in Ramstis Ridge",
			new ManualObjective());
		AddObjective("testimony3", "Record testimony from Wizard Silas in Fedimian",
			new ManualObjective());

		// Rewards
		AddReward(new ExpReward(500, 330));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(640002, 10)); // Small HP potions
		AddReward(new ItemReward(640005, 15)); // Small SP potions
		AddReward(new ItemReward(640080, 4)); // Lv1 EXP Cards
		AddReward(new ItemReward(640182, 5)); // Fedimian Warp Scrolls
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove recording crystal
		character.Inventory.Remove(650282, character.Inventory.CountItem(650282),
			InventoryItemRemoveMsg.Destroyed);

		// Clear testimony tracking
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.TestimonyCommanderTheron");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.TestimonyPeregrinLyon");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.TestimonyWizardSilas");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove recording crystal
		character.Inventory.Remove(650282, character.Inventory.CountItem(650282),
			InventoryItemRemoveMsg.Destroyed);

		// Clear testimony tracking
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.TestimonyCommanderTheron");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.TestimonyPeregrinLyon");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.TestimonyWizardSilas");
	}
}

// Quest 3002: Purification Scrolls
//-----------------------------------------------------------------------------
public class FedimianPurificationScrollsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("Fedimian", 3002);
		SetName("Ancient Purification");
		SetDescription("Recover lost purification scrolls from monsters in Ramstis Ridge to help cleanse demonic corruption.");
		SetLocation("c_fedimian");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Divine Scholar] Lysander", "c_fedimian");

		// Add quest item drops from monsters in f_rokas_25
		// Purification Scroll (667024) drops from various monsters
		AddDrop(667024, 0.3f, MonsterId.Zinute);
		AddDrop(667024, 0.3f, MonsterId.Chupacabra_Desert);
		AddDrop(667024, 0.3f, MonsterId.Lichenclops);

		// Collect 5 Purification Scrolls
		AddObjective("collectScrolls", "Collect Purification Scrolls from monsters in Ramstis Ridge",
			new CollectItemObjective(667024, 15));

		// Rewards
		AddReward(new ExpReward(600, 400));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(640080, 3)); // Lv1 EXP Cards
		AddReward(new ItemReward(640002, 10)); // HP potions
		AddReward(new ItemReward(640005, 15)); // SP potions
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove purification scrolls
		character.Inventory.Remove(667024, character.Inventory.CountItem(667024),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove purification scrolls
		character.Inventory.Remove(667024, character.Inventory.CountItem(667024),
			InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 3003: Unity Summit
//-----------------------------------------------------------------------------
public class FedimianUnitySummitQuest : QuestScript
{
	protected override void Load()
	{
		SetId("Fedimian", 3003);
		SetName("Path to Unity");
		SetDescription("Organize a summit between the three great cities to establish cooperation and mutual aid.");
		SetLocation("c_fedimian");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Peace Envoy] Agatha", "c_fedimian");

		// Deliver invitations to city leaders (manual objectives)
		AddObjective("inviteKlaipeda", "Deliver invitation to Mayor Oswald in Klaipeda",
			new ManualObjective());
		AddObjective("inviteOrsha", "Deliver invitation to Commander Theron in Orsha",
			new ManualObjective());

		// Rewards
		AddReward(new ExpReward(600, 400));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(640002, 10)); // HP potions
		AddReward(new ItemReward(640005, 15)); // SP potions
		AddReward(new ItemReward(640080, 3)); // Lv1 EXP Cards
		AddReward(new ItemReward(ItemId.Scroll_Warp_Klaipe, 5));
		AddReward(new ItemReward(ItemId.Scroll_Warp_Orsha, 5));
		AddReward(new ItemReward(ItemId.Scroll_Warp_Fedimian, 5));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove contracts
		character.Inventory.Remove(650633, character.Inventory.CountItem(650633),
			InventoryItemRemoveMsg.Destroyed);

		// Clear delivery tracking
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3003.UnityContractsDelivered");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3003.DeliveredKlaipedaContract");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3003.DeliveredOrshaContract");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove contracts
		character.Inventory.Remove(650633, character.Inventory.CountItem(650633),
			InventoryItemRemoveMsg.Destroyed);

		// Clear delivery tracking
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3003.UnityContractsDelivered");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3003.DeliveredKlaipedaContract");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3003.DeliveredOrshaContract");
	}
}

// Quest 3004: Royal Mausoleum Expedition - Royal Masoleum (Front Door) Dungeon Access
//-----------------------------------------------------------------------------
public class FedimianRoyalMausoleumQuest : QuestScript
{
	protected override void Load()
	{
		SetId("Fedimian", 3004);
		SetName("Royal Mausoleum Expedition");
		SetDescription("Recover Kepeck's Relic Memoranda from the dangerous Royal Mausoleum to aid the Grand Archive's research.");
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Grand Archivist] Elwen", "c_fedimian");

		// Prerequisite: Must complete quest 3001
		// AddPrerequisite(new QuestPrerequisite("Fedimian", 3001, ReqType.Complete));

		// Collect 3 Relic Memoranda from the Royal Mausoleum
		AddObjective("collectMemoranda", "Collect Relic Memoranda from the Ancient Pedestals",
			new CollectItemObjective(650657, 3));

		// Rewards - Royal Masoleum (Front Door) Dungeon Access
		AddReward(new ExpReward(2000, 1500));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(142103, 1)); // Fire Rod
		AddReward(new ItemReward(640003, 10)); // HP potions
		AddReward(new ItemReward(640006, 15)); // SP potions
		AddReward(new ItemReward(640081, 5)); // Lv2 EXP Cards
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove Relic Memoranda
		character.Inventory.Remove(650657, character.Inventory.CountItem(650657),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection tracking (5 possible pedestals)
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum1");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum2");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum3");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum4");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum5");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove Relic Memoranda
		character.Inventory.Remove(650657, character.Inventory.CountItem(650657),
			InventoryItemRemoveMsg.Destroyed);

		// Clear collection tracking (5 possible pedestals)
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum1");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum2");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum3");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum4");
		character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3004.RelicMemorandum5");

		// Remove Great King's Key if player has it
		character.Inventory.Remove(650244, character.Inventory.CountItem(650244),
			InventoryItemRemoveMsg.Destroyed);
	}
}
