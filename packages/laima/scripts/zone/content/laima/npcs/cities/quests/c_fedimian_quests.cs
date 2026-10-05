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
		AddNpc(150183, L("[Grand Archivist] Elwen"), "c_fedimian", -764, 135, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Fedimian", 3001);
			var mausoleumQuestId = new QuestId("Fedimian", 3004);

			dialog.SetTitle(L("Elwen"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Welcome to Fedimian's Grand Archive. Here we preserve the knowledge and history of our world."));
				await dialog.Msg(L("{#666666}*She gestures to countless scrolls and books lining the walls*{/}"));
				await dialog.Msg(L("I'm compiling a comprehensive history of the demon war. Future generations must understand what happened - both the horrors and the heroism."));

				var response = await dialog.Select(L("How can I assist?"),
					Option(L("I'd like to contribute"), "help"),
					Option(L("What kind of history are you compiling?"), "history"),
					Option(L("Just browsing"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("Truly? I need someone to seek out war survivors and collect their testimonies."));

						if (await dialog.YesNo(L("There are three key witnesses I've identified. Would you be willing to interview them and record their stories?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wonderful! Here's a recording crystal. It will preserve their words exactly as spoken."));
							await dialog.Msg(L("The witnesses are: Commander Theron in Orsha, Peregrin Lyon at Ramstis Ridge, and Wizard Silas here in Fedimian."));
							await dialog.Msg(L("Ask them about their experiences during the war. The crystal will do the rest."));

							// Give recording crystal
							character.Inventory.Add(650282, 1, InventoryAddType.PickUp);
						}
						break;

					case "history":
						await dialog.Msg(L("I'm gathering firsthand accounts from those who lived through the war - soldiers, civilians, survivors."));
						await dialog.Msg(L("Each person's story is unique. Together, they form a complete picture of what our world endured."));
						await dialog.Msg(L("But many survivors won't come forward. They're scattered across the land, and their memories are painful."));
						break;

					case "leave":
						await dialog.Msg(L("Feel free to browse our collection. Knowledge should be shared."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var testimonies = character.Variables.Perm.GetInt("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected", 0);

				if (testimonies >= 3)
				{
					await dialog.Msg(L("You've collected all three testimonies! Let me review them..."));
					await dialog.Msg(L("{#666666}*She holds the crystal, tears streaming down her face as she listens*{/}"));
					await dialog.Msg(L("These stories... such pain, such courage. This is exactly what we need to preserve."));
					await dialog.Msg(L("Future generations will learn from these accounts. They'll understand the true cost of war, and hopefully prevent another one."));
					await dialog.Msg(L("You've made an invaluable contribution to our history. Thank you."));

					character.Inventory.Remove(650282, 1, InventoryItemRemoveMsg.Given);
					character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3001.WarTestimoniesCollected");
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("You've collected {0} out of 3 testimonies so far.", testimonies));
					await dialog.Msg(L("Remember: Commander Theron in Orsha, Peregrin Lyon at Ramstis Ridge, and Wizard Silas here in Fedimian."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				// Offer quest 3004 if player hasn't started it yet
				if (!character.Quests.Has(mausoleumQuestId) && character.Level >= 20)
				{
					await dialog.Msg(L("Your work on the War Chronicles has been invaluable. The testimonies you gathered will educate generations."));
					await dialog.Msg(L("{#666666}*She pauses, studying you with keen eyes*{/}"));

					var response = await dialog.Select(L("Your dedication and trustworthiness have not gone unnoticed. I have another matter - one of great importance and sensitivity."),
						Option(L("I'm willing to help"), "help"),
						Option(L("Tell me more"), "more"),
						Option(L("I'm not interested"), "leave")
					);

					switch (response)
					{
						case "help":
						case "more":
							await dialog.Msg(L("The Royal Mausoleum in the Ramstis region contains ancient relics from Fedimian's past."));
							await dialog.Msg(L("Our sages have been investigating reports of a powerful relic documented by the great scholar Kepeck."));
							await dialog.Msg(L("However, the mausoleum is sealed and filled with dangerous creatures. Only those with official permission may enter."));

							if (await dialog.YesNo(L("I would entrust you with the Great King's Key - proof of your authority to enter the Royal Mausoleum. Will you venture there and recover Kepeck's Relic Memorandum?")))
							{
								character.Quests.Start(mausoleumQuestId);
								await dialog.Msg(L("Excellent! This key proves you act with the Archive's full authority."));
								await dialog.Msg(L("Travel to the Royal Mausoleum entrance in Ramstis Ridge. Show the key to the guard, and they will grant you passage."));
								await dialog.Msg(L("Inside, search for Kepeck's Relic Memorandum - scrolls documenting a powerful relic. Be careful - the mausoleum is dangerous."));

								// Give Great King's Key
								character.Inventory.Add(650244, 1, InventoryAddType.PickUp);
							}
							else
							{
								await dialog.Msg(L("I understand. The mausoleum is perilous. Come back if you change your mind."));
							}
							break;

						case "leave":
							await dialog.Msg(L("I understand. Should you change your mind, I'll be here."));
							break;
					}
				}
				else if (character.Quests.IsActive(mausoleumQuestId))
				{
					var hasMemoranda = character.Inventory.CountItem(650657);

					if (hasMemoranda >= 3)
					{
						await dialog.Msg(L("You've recovered Kepeck's Relic Memoranda! Let me examine them..."));
						await dialog.Msg(L("{#666666}*She carefully unrolls the ancient scrolls*{/}"));
						await dialog.Msg(L("Remarkable! These contain detailed descriptions of a powerful relic that could benefit all of Fedimian."));
						await dialog.Msg(L("Your courage in entering the Royal Mausoleum has uncovered knowledge we thought lost forever."));
						await dialog.Msg(L("You have my deepest gratitude. Here - you've more than earned this reward."));

						character.Quests.Complete(mausoleumQuestId);
					}
					else
					{
						await dialog.Msg(LF("You've recovered {0} out of 3 Relic Memoranda.", hasMemoranda));
						await dialog.Msg(L("Search the Royal Mausoleum thoroughly. Kepeck's scrolls should be hidden within."));
					}
				}
				else if (character.Quests.HasCompleted(mausoleumQuestId))
				{
					await dialog.Msg(L("Thanks to your efforts, we've recovered Kepeck's research on the ancient relic."));
					await dialog.Msg(L("The Royal Mausoleum remains open to you should you wish to explore it further."));
				}
				else
				{
					await dialog.Msg(L("The War Chronicles are nearly complete, thanks to your help. Scholars from across the world come to study them."));
					await dialog.Msg(L("History lives through those who remember and record it."));
				}
			}
		});

		// Divine Restoration - Scholar Lysander
		//-------------------------------------------------------------------------
		AddNpc(20113, L("[Divine Scholar] Lysander"), "c_fedimian", -442, 164, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Fedimian", 3002);

			dialog.SetTitle(L("Lysander"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Welcome. I'm Lysander, devoted to restoring divine protection to our three great cities."));
				await dialog.Msg(L("{#666666}*His eyes burn with intense dedication*{/}"));

				var response = await dialog.Select(L("The demon war left lingering corruption in Klaipeda, Orsha, and even here in Fedimian. We need purification rituals to cleanse these traces."),
					Option(L("How can I help?"), "help"),
					Option(L("Tell me about the purification"), "purification"),
					Option(L("I see. Good luck with that."), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("Excellent! I need your help recovering ancient knowledge."));
						await dialog.Msg(L("Long ago, an old druid master lived here in Fedimian. He created powerful purification scrolls to cleanse demonic corruption."));

						if (await dialog.YesNo(L("These scrolls were scattered and lost during the war. I believe monsters in Ramstis Ridge have found them. Will you venture there and recover 5 purification scrolls?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! The monsters in Ramstis Ridge - particularly near the old druid's hermitage - should have the scrolls."));
							await dialog.Msg(L("The druid master studied corruption extensively. His scrolls are our best hope for purifying the lingering demonic traces."));
							await dialog.Msg(L("Once you collect 5 Purification Scrolls, return to me. We'll use them to help all three cities!"));
						}
						break;

					case "purification":
						await dialog.Msg(L("The old druid master was a brilliant scholar who lived in Fedimian centuries ago."));
						await dialog.Msg(L("He studied demonic corruption and created purification rituals recorded on sacred scrolls."));
						await dialog.Msg(L("When the demon war erupted, his research became invaluable - but the scrolls were scattered and lost."));
						await dialog.Msg(L("Now we need them more than ever to cleanse what the war left behind."));
						break;

					case "leave":
						await dialog.Msg(L("The corruption won't cleanse itself. But I understand if you have other priorities."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var scrollCount = character.Inventory.CountItem(667024);

				if (scrollCount >= 15)
				{
					await dialog.Msg(L("You've recovered the purification scrolls! This is remarkable!"));
					await dialog.Msg(L("{#666666}*He carefully examines the ancient parchments*{/}"));
					await dialog.Msg(L("These are genuine! The druid master's own handwriting... his knowledge of purification magic was unmatched."));
					await dialog.Msg(L("With these scrolls, we can perform cleansing rituals in Klaipeda, Orsha, and Fedimian."));
					await dialog.Msg(L("The demon war's corruption will finally be purged from our cities!"));
					await dialog.Msg(L("You've helped all three cities today. Accept this reward - you've more than earned it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("You've collected {0} out of 5 Purification Scrolls so far.", scrollCount));
					await dialog.Msg(L("Search Ramstis Ridge thoroughly. The monsters there have found the old druid's lost scrolls."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Thanks to the purification scrolls you recovered, we've cleansed the corruption in all three cities!"));
				await dialog.Msg(L("Klaipeda, Orsha, and Fedimian are safer now. The old druid master's legacy lives on."));
			}
		});

		// Peace Envoy Agatha
		//-------------------------------------------------------------------------
		AddNpc(151080, L("[Peace Envoy] Agatha"), "c_fedimian", 418, 131, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("Fedimian", 3003);

			dialog.SetTitle(L("Agatha"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Klaipeda, Orsha, and Fedimian - three great cities that once worked together. Now we barely communicate."));
				await dialog.Msg(L("The war left wounds beyond the physical. Old rivalries resurfaced, trust was broken."));
				await dialog.Msg(L("But we're stronger together. If we can't unite now, during reconstruction, how will we face the next crisis?"));

				var response = await dialog.Select(L("Can the cities be unified?"),
					Option(L("I want to help unite them"), "help"),
					Option(L("What divides them?"), "divisions"),
					Option(L("Some things can't be fixed"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("A champion of unity! This is exactly what we need!"));

						if (await dialog.YesNo(L("I need someone to organize a Unity Summit - bring representatives from all three cities together. Will you help arrange it?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Excellent! First, deliver these cooperation contracts to the other city leaders."));
							await dialog.Msg(L("Visit Mayor Oswald in Klaipeda and Commander Theron in Orsha. Convince them to send delegations here to Fedimian."));
							await dialog.Msg(L("It won't be easy - old grudges run deep. But if anyone can bring them together, it's someone like you - an outsider they all respect."));

							// Give contracts
							character.Inventory.Add(650633, 2, InventoryAddType.PickUp);
						}
						break;

					case "divisions":
						await dialog.Msg(L("Klaipeda focuses on commerce and rebuilding. Orsha maintains military strength and agricultural production. Fedimian pursues knowledge and diplomacy."));
						await dialog.Msg(L("These different priorities create friction. Each city thinks their approach is best."));
						await dialog.Msg(L("But we need all three - trade, defense, and knowledge. United, we could thrive. Divided, we merely survive."));
						break;

					case "leave":
						await dialog.Msg(L("Perhaps you're right. But I have to try. The future of our world depends on cooperation."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var contractsDelivered = character.Variables.Perm.GetInt("Laima.Quests.Fedimian.Quest3003.UnityContractsDelivered", 0);

				if (contractsDelivered >= 2)
				{
					await dialog.Msg(L("Both Klaipeda and Orsha have signed the contracts! This is historic!"));
					await dialog.Msg(L("{#666666}*The summit hall fills with representatives from all three cities*{/}"));
					await dialog.Msg(L("It's tense at first, but as they discuss shared challenges - monsters, refugees, trade - common ground emerges."));
					await dialog.Msg(L("By the end, they've formalized the Mutual Aid Pact. Each city will support the others in times of need."));
					await dialog.Msg(L("This is just the beginning, but you've helped take the first step toward true unity."));
					await dialog.Msg(L("The world is safer because of what you've done today."));

					character.Variables.Perm.Remove("Laima.Quests.Fedimian.Quest3003.UnityContractsDelivered");
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("You've delivered {0} out of 2 contracts.", contractsDelivered));
					await dialog.Msg(L("Find Mayor Oswald in Klaipeda and Commander Theron in Orsha. They need to sign the cooperation contracts."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Mutual Aid Pact is working beautifully. We've already coordinated three joint operations against monster threats."));
				await dialog.Msg(L("You proved that unity is possible. Thank you."));
			}
		});


		// Merchant Guild Representative (receiving letter from Klaipeda quest)
		//-------------------------------------------------------------------------
		AddNpc(147345, L("[Merchant Guild] Anne"), "c_fedimian", 542, -46, 0, async dialog =>
		{
			var character = dialog.Player;
			var klaipeQuestId = new QuestId("Klaipeda", 1004);
			var crossroadsQuestId = new QuestId("f_rokas_31", 1001);

			dialog.SetTitle(L("Anne"));

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
					await dialog.Msg(L("You have urgent news? Please, tell me."));
					await dialog.Msg(L("{#666666}*She listens intently*{/}"));
					await dialog.Msg(L("The Zachariel Crossroads route is being overrun by Warleader Hogmas? This is serious!"));
					await dialog.Msg(L("That's one of our key trade routes to Ramstis Ridge and the Royal Mausoleum. If it's compromised, we'll need to reroute all caravans."));
					await dialog.Msg(L("Thank you for this warning. I'll inform the Guildmaster immediately and arrange for additional guards on future expeditions."));

					character.Quests.CompleteObjective(crossroadsQuestId, "deliverWarning");
					character.ServerMessage(L("{#FFD700}Warning delivered! Return to Darius at Zachariel Crossroads.{/}"));
					return;
				}
				else if (!hogmasKilled)
				{
					await dialog.Msg(L("You look like you have something to say, but it seems you're still handling a situation. Come back when you're ready."));
					return;
				}
			}

			// Handle Klaipeda merchant quest
			if (character.Quests.IsActive(klaipeQuestId) && character.Inventory.HasItem(663221))
			{
				await dialog.Msg(L("A letter from Klaipeda's Merchant Guild? How delightful!"));
				await dialog.Msg(L("{#666666}*She reads eagerly*{/}"));
				await dialog.Msg(L("Resource sharing agreements, academic exchange programs, joint ventures... this is precisely what Fedimian needs!"));
				await dialog.Msg(L("We accept their proposal wholeheartedly. Fedimian will provide academic resources and magical research in exchange for goods."));
				await dialog.Msg(L("Here's our formal acceptance. Thank you for facilitating this connection!"));

				character.Inventory.Remove(663221, 1, InventoryItemRemoveMsg.Given);
				character.Variables.Perm.Set("Laima.Quests.Klaipeda.Quest1004.DeliveredFedimianLetter", true);
				character.Quests.CompleteObjective(klaipeQuestId, "deliverFedimian");
			}
			else if (character.Variables.Perm.GetBool("Laima.Quests.Klaipeda.Quest1004.DeliveredFedimianLetter", false))
			{
				await dialog.Msg(L("Trade with Klaipeda has opened wonderful opportunities. We've exchanged dozens of scholars and merchants already!"));
			}
			else
			{
				await dialog.Msg(L("Fedimian's Merchant Guild specializes in knowledge trade - books, research, magical training."));
				await dialog.Msg(L("We may not have Orsha's food or Klaipeda's goods, but our intellectual resources are unmatched."));
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
		AddNpc(153213, L("[Wizard] Silas"), "c_fedimian", 784, 239, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Silas"));

			if (!character.Quests.Has(testimonyQuestId))
			{
				await dialog.Msg(L("{#666666}*The air is thick with incense smoke rising from a large cauldron*{/}"));
				await dialog.Msg(L("I am Silas, a wizard who survived the great war. These protective wards and incense help keep the dark spirits at bay."));
				await dialog.Msg(L("The demons may be gone, but their corruption lingers. I've learned to ward against it."));

				var response = await dialog.Select(L("What knowledge do you possess?"),
					Option(L("Tell me about the war"), "story"),
					Option(L("What is the incense for?"), "incense"),
					Option(L("I'll leave you to your work"), "leave")
				);

				switch (response)
				{
					case "story":
						await dialog.Msg(L("I fought in the great war as a battle wizard. I witnessed things that still haunt my dreams."));
						await dialog.Msg(L("Demons, corruption, cities burning... but I survived through knowledge and preparation."));
						await dialog.Msg(L("Now I use what I learned to protect this place. The incense, the wards - they keep the darkness away."));
						await dialog.Msg(L("I may have survived, but I'll never forget what was lost."));
						break;

					case "incense":
						await dialog.Msg(L("This incense is made from sacred herbs blessed with protective magic."));
						await dialog.Msg(L("It purifies the air and repels malevolent spirits. After the war, such precautions became necessary."));
						await dialog.Msg(L("The cauldron never stops burning. As long as it does, this place remains safe from corruption."));
						break;

					case "leave":
						await dialog.Msg(L("Stay vigilant. The darkness never truly rests."));
						break;
				}
			}
			else if (character.Quests.IsActive(testimonyQuestId))
			{
				var recorded = character.Variables.Perm.GetBool("Laima.Quests.Fedimian.Quest3001.TestimonyWizardSilas", false);

				if (recorded)
				{
					await dialog.Msg(L("My testimony is recorded. Share it with the Grand Archive."));
					await dialog.Msg(L("The world must know what we endured, and how we survived."));
				}
				else
				{
					if (!character.Inventory.HasItem(650282))
					{
						await dialog.Msg(L("You need a recording crystal to preserve my testimony."));
						return;
					}

					await dialog.Msg(L("You're documenting the war for posterity? Important work."));

					if (await dialog.YesNo(L("Would you like to hear my account as a battle wizard who survived the great war?")))
					{
						await dialog.Msg(L("{#666666}*Silas speaks solemnly into the recording crystal*{/}"));
						await dialog.Msg(L("'I am Silas, battle wizard. I fought in the great war and witnessed horrors beyond imagination.'"));
						await dialog.Msg(L("'Cities burned, people died, and demons seemed unstoppable. But we fought on with magic and steel.'"));
						await dialog.Msg(L("'I survived through preparation, knowledge, and protective wards. Now I use what I learned to keep the darkness at bay.'"));
						await dialog.Msg(L("'The war may be over, but its corruption lingers. We must remain vigilant, always.'"));

						character.Variables.Perm.Set("Laima.Quests.Fedimian.Quest3001.TestimonyWizardSilas", true);
						character.Quests.CompleteObjective(testimonyQuestId, "testimony3");

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
						await dialog.Msg(L("I understand. Return when you're ready to hear my account."));
					}
				}
			}
			else
			{
				await dialog.Msg(L("Thank you for preserving the truth in Fedimian's Grand Archive."));
				await dialog.Msg(L("Future generations will know the goddesses stood with us."));
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
		SetName(L("Chronicles of the Demon War"));
		SetType(QuestType.Sub);
		SetDescription(L("Collect testimonies from war survivors to complete the Grand Archive's historical record."));
		SetLocation("c_fedimian");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Grand Archivist] Elwen"), "c_fedimian");

		// Collect 3 testimonies (tracked manually)
		AddObjective("testimony1", L("Record testimony from Commander Theron in Orsha"),
			new ManualObjective());
		AddObjective("testimony2", L("Record testimony from Peregrin Lyon in Ramstis Ridge"),
			new ManualObjective());
		AddObjective("testimony3", L("Record testimony from Wizard Silas in Fedimian"),
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
		SetName(L("Ancient Purification"));
		SetType(QuestType.Sub);
		SetDescription(L("Recover lost purification scrolls from monsters in Ramstis Ridge to help cleanse demonic corruption."));
		SetLocation("c_fedimian");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Divine Scholar] Lysander"), "c_fedimian");

		// Add quest item drops from monsters in f_rokas_25
		// Purification Scroll (667024) drops from various monsters
		AddDrop(667024, 0.3f, MonsterId.Zinute);
		AddDrop(667024, 0.3f, MonsterId.Chupacabra_Desert);
		AddDrop(667024, 0.3f, MonsterId.Lichenclops);

		// Collect 5 Purification Scrolls
		AddObjective("collectScrolls", L("Collect Purification Scrolls from monsters in Ramstis Ridge"),
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
		SetName(L("Path to Unity"));
		SetType(QuestType.Sub);
		SetDescription(L("Organize a summit between the three great cities to establish cooperation and mutual aid."));
		SetLocation("c_fedimian");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Peace Envoy] Agatha"), "c_fedimian");

		// Deliver invitations to city leaders (manual objectives)
		AddObjective("inviteKlaipeda", L("Deliver invitation to Mayor Oswald in Klaipeda"),
			new ManualObjective());
		AddObjective("inviteOrsha", L("Deliver invitation to Commander Theron in Orsha"),
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
		SetName(L("Royal Mausoleum Expedition"));
		SetType(QuestType.Sub);
		SetDescription(L("Recover Kepeck's Relic Memoranda from the dangerous Royal Mausoleum to aid the Grand Archive's research."));
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Grand Archivist] Elwen"), "c_fedimian");

		// Prerequisite: Must complete quest 3001
		// AddPrerequisite(new QuestPrerequisite("Fedimian", 3001, ReqType.Complete));

		// Collect 3 Relic Memoranda from the Royal Mausoleum
		AddObjective("collectMemoranda", L("Collect Relic Memoranda from the Ancient Pedestals"),
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
