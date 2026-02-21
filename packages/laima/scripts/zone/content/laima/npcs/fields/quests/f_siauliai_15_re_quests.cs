//--- Melia Script ----------------------------------------------------------
// Woods of the Linked Bridges - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_siauliai_15_re map.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai15ReQuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Cleansing the Corrupted Air
		// =====================================================================
		// Scout Adventurer - Near Orsha Entrance
		//-------------------------------------------------------------------------
		AddNpc(57264, "[Scout] Milda", "f_siauliai_15_re", 2621, 99, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_15_re", 1001);

			dialog.SetTitle("Milda");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A scout crouches near a patch of withered vegetation, examining toxic residue*{/}");
				await dialog.Msg("The contamination is spreading faster than I anticipated. I've been tracking the poison's source throughout this region.");

				var response = await dialog.Select("These woods are becoming increasingly hazardous. The corrupted creatures are carriers - spreading toxins with every movement.",
					Option("I can help deal with the corrupted creatures", "help"),
					Option("What's causing the contamination?", "cause"),
					Option("I should go", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*She stands, her hand moving to her weapon*{/}");
						await dialog.Msg("Another capable fighter? Good. We need more people taking action.");
						await dialog.Msg("The Jukopus, Kepas, and Pokubu - they've all become vectors for the corruption. Every one of them spreads poison through this region.");

						if (await dialog.YesNo("We need to cull the infected population before the contamination overwhelms the entire forest. I can offer you a proper weapon - a soldier's iron club - if you help thin their numbers. Are you in?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Excellent. Hunt down the corrupted Jukopus, poisoned Kepas, and infected Pokubu throughout these woods.");
							await dialog.Msg("Come back when you've made a significant impact on their numbers. The club will be waiting for you.");
						}
						break;

					case "cause":
						await dialog.Msg("{#666666}*She gestures toward the forest*{/}");
						await dialog.Msg("After the demon war, residual dark energy saturated these lands. When the guardian owl's protection weakened, that corruption took root.");
						await dialog.Msg("Now it's infecting the wildlife, turning ordinary creatures into toxic threats. If we don't contain it, this entire region could become uninhabitable.");
						await dialog.Msg("I've been scouting the affected zones, mapping contamination levels. But studying it isn't enough - we need to actively cull the infected creatures.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*She returns to examining the ground*{/}");
						await dialog.Msg("Watch yourself in these woods. The poison doesn't discriminate - it affects everyone.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killJukopus", out var jukopusObj)) return;
				if (!quest.TryGetProgress("killKepas", out var kepasObj)) return;
				if (!quest.TryGetProgress("killPokubu", out var pokubuObj)) return;

				var jukopusDone = jukopusObj.Done;
				var kepasDone = kepasObj.Done;
				var pokubuDone = pokubuObj.Done;

				if (!jukopusDone || !kepasDone || !pokubuDone)
				{
					await dialog.Msg("Still culling the infected creatures?");
					await dialog.Msg("Keep at it. Every infected creature eliminated slows the contamination's spread.");
				}
				else
				{
					await dialog.Msg("{#666666}*She looks up from her field notes, impressed*{/}");
					await dialog.Msg("Excellent work. I've been monitoring the contamination levels, and they're already starting to stabilize in some areas.");
					await dialog.Msg("You've proven yourself capable. Here - take this soldier's short bow. It's a solid weapon, and you've earned it.");
					await dialog.Msg("{#666666}*She hands you a well-crafted short bow*{/}");
					await dialog.Msg("With fighters like you helping to contain this threat, we might actually reclaim these woods.");

					character.Quests.Complete(questId);
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The culling operation you conducted made a real difference. Contamination readings have improved in several zones.");
				await dialog.Msg("With continued effort, we might be able to reclaim these woods from the poison's grip.");
			}
		});

		// =====================================================================
		// QUEST 1002: The Lost Caravan
		// =====================================================================
		// Distressed Merchant
		//-------------------------------------------------------------------------
		AddNpc(20153, "[Merchant] Alfred", "f_siauliai_15_re", 2404, -1088, 44, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_15_re", 1002);

			dialog.SetTitle("Alfred");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A disheveled merchant sits on a crate, head in his hands*{/}");
				await dialog.Msg("Ruined... I'm completely ruined...");

				var response = await dialog.Select("My caravan was attacked by those blasted poisoned Kepas! They scattered my goods everywhere!",
					Option("I can help recover your goods", "help"),
					Option("What were you transporting?", "goods"),
					Option("Maybe you should hire guards", "leave")
				);

				switch (response)
				{
					case "help":
						if (character.Level < 3)
						{
							await dialog.Msg("{#666666}*He looks you over skeptically*{/}");
							await dialog.Msg("No offense, friend, but those Kepas are tougher than they look. You'd need to be at least level 3 to handle them safely.");
							await dialog.Msg("Come back when you're stronger, and I'll still need the help.");
						}
						else
						{
							await dialog.Msg("{#666666}*He jumps to his feet*{/}");
							await dialog.Msg("You'd do that? Oh, thank the goddess!");

							if (await dialog.YesNo("My goods are scattered all over this area. Those poisoned Kepas probably dragged some of it around. Can you recover what you can find?"))
							{
								character.Quests.Start(questId);
								await dialog.Msg("Thank you! Look for scattered crates and packages. The Kepas might have taken some of it too.");
								await dialog.Msg("Bring me back 8 packages and I can salvage at least part of my business!");
							}
						}
						break;

					case "goods":
						await dialog.Msg("Medicinal herbs, cloth, tools... supplies for the villages around Orsha.");
						await dialog.Msg("Nothing fancy, but it's my livelihood. Without those goods, I can't pay my debts.");
						await dialog.Msg("My family will starve if I don't recover something from this disaster.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*He laughs bitterly*{/}");
						await dialog.Msg("Hire guards? With what money? I spent everything I had on this shipment!");
						await dialog.Msg("I gambled it all on one big delivery and lost. That's the merchant's life for you.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var goods = character.Inventory.CountItem(650399);

				if (goods >= 8)
				{
					await dialog.Msg("{#666666}*He eagerly examines the packages*{/}");
					await dialog.Msg("You found them! Let me see... yes, yes! These are still in good condition!");
					await dialog.Msg("{#666666}*He starts counting quickly*{/}");
					await dialog.Msg("This is enough to save my business! I can still make my deliveries, pay my debts... oh, thank you!");
					await dialog.Msg("Here, please accept this reward. You've saved my family from ruin.");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("Still looking for my goods? Please keep searching.");
					await dialog.Msg("Please hurry - the longer they're out there, the more damaged they'll become.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("Thanks to you, my business survived. I've been making deliveries again!");
				await dialog.Msg("Next time I'll definitely hire guards before traveling through these woods.");
			}
		});

		// =====================================================================
		// QUEST 1003: The Hermit's Discovery
		// =====================================================================
		// Old Hermit - Secluded Cabin
		//-------------------------------------------------------------------------
		AddNpc(20152, "[Hermit] Torsten", "f_siauliai_15_re", 462, 2100, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_15_re", 1003);

			dialog.SetTitle("Torsten");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*An old hermit sits outside his cabin, watching the forest*{/}");
				await dialog.Msg("Hm. Another traveler. Don't see many people out here in this remote spot.");

				var response = await dialog.Select("Though I suppose the boars have been acting strange lately. More aggressive. Almost... corrupted.",
					Option("I could investigate for you", "help"),
					Option("What do you mean by corrupted?", "corrupted"),
					Option("Why do you live so far from town?", "hermit")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*He nods slowly*{/}");
						await dialog.Msg("So you're a fighter, eh? Good. The forest needs people willing to face the darkness.");

						if (await dialog.YesNo("Hunt the corrupted Pokubu and the Pokuborn. If enough of them are culled, maybe the corruption will slow its spread. Will you do this?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Good. Hunt the Pokubu throughout these woods. The Pokuborn especially - they're the most dangerous.");
							await dialog.Msg("When you're done, return here. I have something that might help protect you in the future.");
						}
						break;

					case "corrupted":
						await dialog.Msg("The Pokubu - the boars that roam these woods. They've always been territorial, but lately they've become violent.");
						await dialog.Msg("Their eyes glow with dark energy. Some - the Pokuborn - have darker hides and are far more aggressive.");
						await dialog.Msg("I've lived in these woods for thirty years. I know when something's unnatural.");
						break;

					case "hermit":
						await dialog.Msg("I came out here to escape people and their politics. Cities, armies, wars... all that noise.");
						await dialog.Msg("Out here, it's just me, the trees, and the beasts. Or at least it used to be peaceful.");
						await dialog.Msg("But even in isolation, the world's problems find you eventually.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPokubu", out var pokubuObj)) return;
				if (!quest.TryGetProgress("killArburn", out var arburnObj)) return;

				var pokubuDone = pokubuObj.Done;
				var arburnDone = arburnObj.Done;

				if (!pokubuDone || !arburnDone)
				{
					await dialog.Msg("Still hunting the corrupted boars?");
					await dialog.Msg("Be careful out there. The Pokuborn are particularly vicious.");
				}
				else
				{
					await dialog.Msg("{#666666}*He stands and inspects you carefully*{/}");
					await dialog.Msg("You did it. I can already sense the forest feels... calmer. Less corrupted energy in the air.");
					await dialog.Msg("{#666666}*He retrieves a piece of leather armor from his cabin*{/}");
					await dialog.Msg("This is an armor I crafted with the leather from these monsters.");
					await dialog.Msg("You've earned it. Thank you for helping maintain the balance in these woods.");

					character.Quests.Complete(questId);
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("The boars have been calmer lately. The corruption's influence is weakening, thanks to you.");
				await dialog.Msg("Keep that armor close. It'll serve you well against dark forces.");
			}
		});

		// =====================================================================
		// QUEST 1004: Reconstructing the Bridges
		// =====================================================================
		// Orsha Soldier - Near Linked Bridges
		//-------------------------------------------------------------------------
		AddNpc(20060, "[Soldier] Henrik", "f_siauliai_15_re", -759, -176, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_15_re", 1004);

			dialog.SetTitle("Henrik");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A sturdy soldier stands near the bridge, examining rubble and debris*{/}");
				await dialog.Msg("Hmm... these bridges are shedding rubble. The war took its toll on the infrastructure.");

				var response = await dialog.Select("I'm Henrik, sent from Orsha to assess the damage and plan reconstruction efforts. These linked bridges are vital for supply routes and trade.",
					Option("How can I help?", "help"),
					Option("What's your assessment?", "assessment"),
					Option("Good luck with that", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*He looks up from his plans*{/}");
						await dialog.Msg("You'd help with the survey? That would speed things up considerably.");

						if (await dialog.YesNo("I need someone to inspect the rubble piles coming off the bridges - this will help us assess which areas need the most urgent repairs. Can you handle that?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Excellent. Follow the main road through the Woods of the Linked Bridges and inspect each rubble pile you find.");
							await dialog.Msg("There's another bridge further south - follow the road and you'll find more rubble piles along the way.");
							await dialog.Msg("Once you've inspected all the rubble piles, report back to me. We'll use your findings to prioritize reconstruction efforts.");
						}
						break;

					case "assessment":
						await dialog.Msg("{#666666}*He rolls out a worn map of the forest*{/}");
						await dialog.Msg("Multiple bridges connect this entire region - built generations ago by Master Siauliai.");
						await dialog.Msg("After the demon war, we lost contact with this area. The bridges are deteriorating - rubble and debris falling from the supports.");
						await dialog.Msg("Without proper bridges, Orsha's supply routes are cut off. Villages remain isolated. Commerce has ground to a halt.");
						await dialog.Msg("We need to survey the damage before we can begin reconstruction work.");
						break;

					case "leave":
						await dialog.Msg("{#666666}*He returns to his construction plans*{/}");
						await dialog.Msg("Right. This is Orsha military business anyway. Safe travels.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var bridgesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_15_re.Quest1004.BridgesChecked", 0);

				if (bridgesChecked >= 6)
				{
					await dialog.Msg("{#666666}*He straightens up, eager for your report*{/}");
					await dialog.Msg("You've inspected all the rubble piles? Give me your assessment!");
					await dialog.Msg("{#666666}*You provide detailed reports on the damage at each location*{/}");
					await dialog.Msg("{#666666}*He makes notes on his plans*{/}");
					await dialog.Msg("This is excellent intelligence. Now I know exactly which bridges need the most urgent repairs.");
					await dialog.Msg("I can take this back to Orsha and get reconstruction approved immediately. We'll have work crews out here within the week.");
					await dialog.Msg("Here - take this soldier's iron club as payment for your survey work. Orsha thanks you for helping reconnect the region!");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("How's the survey going? Keep looking for more rubble piles to inspect.");
					await dialog.Msg("Follow the main road through the forest. Remember, there's another bridge further south - keep following the road.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*He's reviewing construction schedules*{/}");
				await dialog.Msg("Thanks to your survey, reconstruction has been approved. Work crews are already repairing the most damaged bridges.");
				await dialog.Msg("Within a few months, all the linked bridges will be fully restored. Commerce and travel will return to normal.");
			}
		});

		// =====================================================================
		// QUEST 1005: The Beekeeper's Hope
		// =====================================================================
		// Former Beekeeper - Abandoned Bee Farm
		//-------------------------------------------------------------------------
		AddNpc(20116, "[Beekeeper] Greta", "f_siauliai_15_re", 52, -1382, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_15_re", 1005);

			dialog.SetTitle("Greta");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*An elderly woman stands among empty beehives*{/}");
				await dialog.Msg("My bees... they're all gone...");

				var response = await dialog.Select("This was once the most productive bee farm in all of Orsha. Now look at it. Empty. Silent.",
					Option("What happened to your bees?", "bees"),
					Option("Maybe I can help", "help"),
					Option("That's unfortunate", "leave")
				);

				switch (response)
				{
					case "bees":
						await dialog.Msg("{#666666}*She gazes toward the forest*{/}");
						await dialog.Msg("After the war, corruption spread throughout these woods. The air became toxic.");
						await dialog.Msg("The bees... they couldn't survive here. Some died. Most fled to cleaner lands.");
						await dialog.Msg("But I've heard reports of wild flowers blooming in certain spots - flowers that bees love.");
						await dialog.Msg("I tried to stay, hoping they'd return. But hope fades with each empty sunrise.");
						break;

					case "help":
						await dialog.Msg("{#666666}*A spark of hope appears in her eyes*{/}");
						await dialog.Msg("You... you would help an old beekeeper?");
						await dialog.Msg("I've heard there are special flowers growing wild in the forest - Moonbells, Sunpetals, Honeydew blooms...");
						await dialog.Msg("The kind of flowers that bees absolutely love. If I could get some of those flowers, I could use them to lure wild bees back to my farm!");

						if (await dialog.YesNo("Would you gather those bee-attracting flowers for me? I need at least 5 bunches to have a chance of luring the bees back."))
						{
							character.Quests.Start(questId);
							await dialog.Msg("Oh, thank you! Look for colorful flower patches throughout the forest.");
							await dialog.Msg("Gather 5 bunches of those special flowers and bring them back to me.");
							await dialog.Msg("With those flowers near my hives, maybe... just maybe... the bees will return to these woods.");
						}
						break;

					case "leave":
						await dialog.Msg("{#666666}*She nods sadly*{/}");
						await dialog.Msg("Yes... unfortunate. But such is life after war. We lose the things we love.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var flowersCollected = character.Inventory.CountItem(650428);

				if (flowersCollected >= 5)
				{
					await dialog.Msg("{#666666}*She clasps your hands gratefully*{/}");
					await dialog.Msg("You gathered them! All five bunches of bee-attracting flowers!");
					await dialog.Msg("{#666666}*She carefully arranges the flowers around her beehives*{/}");
					await dialog.Msg("{#666666}*Almost immediately, you hear a faint buzzing in the distance*{/}");
					await dialog.Msg("Listen! Do you hear that? Bees! Wild bees are already coming to investigate!");
					await dialog.Msg("You've given me something I'd lost - hope. Hope that someday, these hives will be full of life again.");
					await dialog.Msg("Thank you, dear traveler. Please, take this. It's not much, but it's offered with a grateful heart.");

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg($"Have you found the bee-attracting flowers? You have {flowersCollected} out of 5 bunches.");
					await dialog.Msg("Look for colorful flower patches throughout the forest - places where the corruption hasn't taken hold.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("I can hear bees buzzing around the hives again! A few have even started building new colonies!");
				await dialog.Msg("It'll take time to rebuild what was lost, but you've given me that chance. Thank you.");
			}
		});

		// =====================================================================
		// QUEST 1006: Seeds of Hope
		// =====================================================================
		// Desperate Farmer - Morku Farm
		//-------------------------------------------------------------------------
		AddNpc(20117, "[Farmer] Morku", "f_siauliai_15_re", -2330, -172, 44, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_15_re", 1006);

			dialog.SetTitle("Morku");

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg("{#666666}*A weathered farmer stares at an empty plot of land*{/}");
				await dialog.Msg("Nothing grows anymore. Nothing at all.");

				var response = await dialog.Select("I've tried everything - new seeds, fertilizer, prayers. But the soil is dead. Corrupted.",
					Option("Perhaps I can help", "help"),
					Option("What corrupted your farm?", "corruption"),
					Option("Maybe you should move", "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg("{#666666}*He looks at you with tired eyes*{/}");
						await dialog.Msg("Help? I don't know if anyone can help. But...");
						await dialog.Msg("There's a legend about an ancient stone golem. A guardian that watches over a sacred monument north of here.");

						if (await dialog.YesNo("They say the golem can grant blessings to purify corrupted land. It's probably just a story, but... I'm desperate. Would you seek out the Stone Guardian and ask for its blessing?"))
						{
							character.Quests.Start(questId);
							await dialog.Msg("The Stone Guardian is north of here, deep in the forest. Look for a giant carved stone head - the golem stands watch over it.");
							await dialog.Msg("If the legends are true... maybe it will help us. Please, try.");
						}
						break;

					case "corruption":
						await dialog.Msg("After the war, dark energy seeped into the ground. Poisoned everything.");
						await dialog.Msg("My crops withered. My animals died. Even the weeds won't grow here anymore.");
						await dialog.Msg("This farm has been in my family for four generations. Now it's just... dead earth.");
						break;


					case "leave":
						await dialog.Msg("{#666666}*He clenches his fists*{/}");
						await dialog.Msg("Move? And abandon four generations of family history? Never.");
						await dialog.Msg("I'll die on this land before I give up on it.");
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (character.Inventory.HasItem(650713))
				{
					await dialog.Msg("{#666666}*His eyes widen*{/}");
					await dialog.Msg("That glow... is that... the Stone Guardian's purified essence?!");
					await dialog.Msg("{#666666}*He kneels reverently*{/}");
					await dialog.Msg("You actually received it! The ancient guardian acknowledged you!");
					await dialog.Msg("Please, may I... may I apply this to my farm?");
					await dialog.Msg("{#666666}*He carefully spreads the purified essence across his empty field*{/}");
					await dialog.Msg("{#666666}*The soil begins to glow faintly, the corruption visibly receding*{/}");
					await dialog.Msg("It's working! The land... it's healing! After all this time, there's finally hope!");
					await dialog.Msg("{#666666}*He looks at you with tears in his eyes*{/}");
					await dialog.Msg("Thank you. Thank you for giving me back my future.");

					character.Inventory.Remove(650713, 1, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg("Have you found the Stone Guardian? It should be west of here, deeper in the forest.");
					await dialog.Msg("Look for a giant carved stone head monument - the golem stands watch over it.");
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*He's working in his field, where small green shoots are beginning to emerge*{/}");
				await dialog.Msg("Look! Real crops! Growing in my soil again!");
				await dialog.Msg("The Stone Guardian's blessing worked. The corruption is gone, the land is healing!");
				await dialog.Msg("I'll never forget what you've done for me and my family.");
			}
		});

		// =====================================================================
		// QUEST 1006: Stone Golem Guardian
		// =====================================================================
		// Ancient Stone Golem Guardian
		//-------------------------------------------------------------------------
		AddNpc(47125, "[Ancient Guardian] Stone Golem", "f_siauliai_15_re", -2889, 900, 135, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_15_re", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg("{#666666}*A towering stone golem stands motionless, facing an enormous carved stone head*{/}");
				await dialog.Msg("{#666666}*The ancient monument is covered in glowing runes. The golem seems to be guarding it reverently*{/}");
				return;
			}

			if (character.Inventory.HasItem(650713))
			{
				await dialog.Msg("{#666666}*The golem's eyes glow faintly as it gazes at the stone head*{/}");
				await dialog.Msg("The blessing... has been granted. Take it... to the farmer.");
				return;
			}

			var riddleSolved = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_15_re.Quest1006.RiddleSolved", false);

			if (riddleSolved)
			{
				await dialog.Msg("{#666666}*The golem gestures toward the monument*{/}");
				await dialog.Msg("You have proven yourself... worthy. Approach the monument... and pray.");
				return;
			}

			await dialog.Msg("{#666666}*The golem's eyes ignite with blue light as you approach*{/}");
			await dialog.Msg("Mortal... you disturb... the sacred monument.");
			await dialog.Msg("{#666666}*It turns its massive stone head toward you*{/}");
			await dialog.Msg("I have stood... for centuries. Guarding this place... since before the demon war ended.");

			var response1 = await dialog.Select("Tell me... did you witness... the great war between goddesses and demons?",
				Option("Yes, I fought in the demon war", "fought"),
				Option("No, I came after the war ended", "after"),
				Option("I don't remember much about it", "forget")
			);

			switch (response1)
			{
				case "fought":
					await dialog.Msg("{#666666}*The golem's eyes dim slightly*{/}");
					await dialog.Msg("Deception... I sense no war-weariness in your soul. You lie to impress me.");
					await dialog.Msg("The monument's blessing... is not for deceivers. Leave.");
					return;

				case "after":
					await dialog.Msg("{#666666}*The golem nods slowly*{/}");
					await dialog.Msg("Truthful... good. The young should not carry... the burdens of old wars.");
					await dialog.Msg("{#666666}*It gestures toward the massive stone head monument behind it*{/}");
					await dialog.Msg("This monument... honors the Earth Goddess. Many seek its power... but few deserve it.");
					break;

				case "forget":
					await dialog.Msg("{#666666}*The golem studies you carefully*{/}");
					await dialog.Msg("Uncertain... evasive. You seek to hide something... or simply speak carelessly.");
					await dialog.Msg("I cannot grant blessings... to those who do not speak clearly. Return when you know your truth.");
					return;
			}

			var response2 = await dialog.Select("Why do you seek... the monument's blessing?",
				Option("To prove I am worthy of such power", "worthy"),
				Option("Because I need its power for myself", "power"),
				Option("To help a farmer whose land is corrupted", "help")
			);

			switch (response2)
			{
				case "worthy":
					await dialog.Msg("{#666666}*The golem's eyes flicker with disappointment*{/}");
					await dialog.Msg("Pride... ambition. You seek validation... not healing.");
					await dialog.Msg("The blessing is not... a trophy for your collection. Leave.");
					return;

				case "power":
					await dialog.Msg("{#666666}*The golem's entire form seems to harden*{/}");
					await dialog.Msg("Selfishness... greed. Your soul harbors darkness.");
					await dialog.Msg("The monument's blessing... would be wasted on you. Begone.");
					return;

				case "help":
					await dialog.Msg("{#666666}*The golem's eyes glow brighter*{/}");
					await dialog.Msg("Compassion... selflessness. You seek to heal... not to take.");
					await dialog.Msg("Morku's suffering... yes... I sense it from here. His land cries out... poisoned by war's remnants.");
					break;
			}

			var response3 = await dialog.Select("One final question... Where do you come from, stranger?",
				Option("I am just a wandering traveler", "wanderer"),
				Option("From Orsha, the military stronghold", "orsha"),
				Option("From Goddess Laima's Maple Leaf Sanctuary", "laima")
			);

			switch (response3)
			{
				case "wanderer":
					await dialog.Msg("{#666666}*The golem pauses, as if sensing something*{/}");
					await dialog.Msg("Wanderer... perhaps. But I sense... something more. A divine touch upon your soul.");
					await dialog.Msg("No matter. Your intentions are pure... that is what matters.");
					break;

				case "orsha":
					await dialog.Msg("{#666666}*The golem considers your words*{/}");
					await dialog.Msg("Orsha... strong warriors. Good hearts. The land remembers... their sacrifices.");
					await dialog.Msg("You carry their spirit... that is enough.");
					break;

				case "laima":
					await dialog.Msg("{#666666}*The golem's entire form seems to brighten*{/}");
					await dialog.Msg("Laima... the merciful goddess. She who creates sanctuary... for the lost and weary.");
					await dialog.Msg("You are one of her Otherworlders... summoned to help heal this broken world.");
					await dialog.Msg("Then you are... exactly who should receive... this blessing.");
					break;
			}

			await dialog.Msg("{#666666}*The golem places a massive hand on the stone head monument*{/}");
			await dialog.Msg("You have proven yourself... worthy. Your heart is pure... your intentions noble.");
			await dialog.Msg("The Earth Goddess... hears the pleas of those who suffer. And she answers... through those like you.");
			await dialog.Msg("{#666666}*The golem steps aside, revealing the glowing monument*{/}");
			await dialog.Msg("Approach the monument... and offer your prayer. The goddess... will answer.");

			character.Variables.Perm.Set("Laima.Quests.f_siauliai_15_re.Quest1006.RiddleSolved", true);
			character.ServerMessage("{#FFD700}The golem has granted you permission. Approach the monument and pray.{/}");
		});

		// =====================================================================
		// QUEST 1006: Stone Head Monument
		// =====================================================================
		// Sacred Monument - Only accessible after solving the golem's riddle
		//-------------------------------------------------------------------------
		AddNpc(12080, "Sacred Monument", "f_siauliai_15_re", -2828, 917, 135, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_15_re", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg("{#666666}*An enormous stone head carved with ancient runes. Its eyes seem to watch you*{/}");
				return;
			}

			if (character.Inventory.HasItem(650713))
			{
				await dialog.Msg("{#666666}*The monument's blessing has already been granted*{/}");
				return;
			}

			var riddleSolved = character.Variables.Perm.GetBool("Laima.Quests.f_siauliai_15_re.Quest1006.RiddleSolved", false);

			if (!riddleSolved)
			{
				await dialog.Msg("{#666666}*The monument is protected by the stone golem. You must prove yourself worthy first*{/}");
				return;
			}

			var result = await character.TimeActions.StartAsync("Praying before the monument...", "Cancel", "PRAY", TimeSpan.FromSeconds(5));

			if (result == TimeActionResult.Completed)
			{
				await dialog.Msg("{#666666}*The runes on the monument begin to glow brilliantly*{/}");
				await dialog.Msg("{#666666}*Divine energy flows from the monument, forming a crystalline essence*{/}");
				await dialog.Msg("{#666666}*The purified essence floats gently into your hands*{/}");
				await dialog.Msg("{#666666}*You feel the Earth Goddess's blessing upon you*{/}");

				character.Inventory.Add(650713, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(questId, "seekBlessing");
				character.ServerMessage("{#FFD700}Received Purified Essence! Return to Morku.{/}");
			}
			else
			{
				await dialog.Msg("{#666666}*Your prayer was interrupted. The monument waits patiently*{/}");
			}
		});

		// =====================================================================
		// PURIFYING STATUES - FOREST ROAD PROTECTION
		// =====================================================================
		// These were from the original Orsha Quest 2002
		// =====================================================================

		var forestQuestId = new QuestId("Orsha", 2002);

		// Purifying Statue Helper
		//-------------------------------------------------------------------------
		void AddPurifyingStatue(int statueNumber, int x, int y, int direction, string inactiveMsg1, string inactiveMsg2, string activationMsg1, string activationMsg2)
		{
			AddNpc(150226, "Purifying Statue", "f_siauliai_15_re", x, y, direction, async dialog =>
			{
				var character = dialog.Player;

				var statuesPrayed = character.Variables.Perm.GetInt("Laima.Quests.Orsha.Quest2002.PurifyingStatuesAwakened", 0);
				var variableKey = $"Laima.Quests.Orsha.Quest2002.PurifyingStatue{statueNumber}";
				var prayed = character.Variables.Perm.GetBool(variableKey, false) || character.Quests.HasCompleted(forestQuestId);

				if (prayed)
				{
					await dialog.Msg("{#666666}*The statue glows with holy light, warding off evil creatures*{/}");
					await dialog.Msg("Your prayer has already awakened this guardian. The road here is safe.");
					return;
				}

				if (!character.Quests.IsActive(forestQuestId))
				{
					await dialog.Msg(inactiveMsg1);
					if (!string.IsNullOrEmpty(inactiveMsg2))
						await dialog.Msg(inactiveMsg2);
					return;
				}

				var result = await character.TimeActions.StartAsync("Praying...", "Cancel", "PRAY", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					await dialog.Msg(activationMsg1);
					if (!string.IsNullOrEmpty(activationMsg2))
						await dialog.Msg(activationMsg2);

					character.Variables.Perm.Set("Laima.Quests.Orsha.Quest2002.PurifyingStatuesAwakened", statuesPrayed + 1);
					character.Variables.Perm.Set(variableKey, true);

					// Display visual effect
					Send.ZC_NORMAL.PlayEffect(character.Connection, dialog.Npc, animationName: "F_light076_spread_in_blue_loop");

					var newTotal = statuesPrayed + 1;
					character.ServerMessage($"Purifying statues awakened: {newTotal}/5");

					if (newTotal >= 5)
					{
						character.ServerMessage("{#FFD700}All statues awakened! The forest roads are protected. Return to Miriam in Orsha.{/}");
					}
				}
				else
				{
					character.ServerMessage("Your prayer was interrupted. The statue remains dormant.");
				}
			});
		}

		// Purifying Statue 1 - Northern Path
		//-------------------------------------------------------------------------
		AddPurifyingStatue(1, -1374, 174, 45,
			"{#666666}*An ancient wooden statue carved in the shape of an Owl. Its holy power lies dormant*{/}",
			"The statue once protected travelers on this road, but its blessing has faded.",
			"{#666666}*Holy light bursts from the statue! The Guardian's blessing awakens!*{/}",
			"The statue comes alive with divine energy. Monsters nearby flee from its radiance.");

		// Purifying Statue 2 - Eastern Path
		//-------------------------------------------------------------------------
		AddPurifyingStatue(2, 1347, 900, 45,
			"{#666666}*A weathered wooden guardian stands watch over the eastern road*{/}",
			"The statue's divine protection has long since faded. Monsters roam freely here.",
			"{#666666}*The Guardian's power flows through the statue! Light banishes the shadows!*{/}",
			"The guardian awakens. This path is under divine protection once more.");

		// Purifying Statue 3 - Southern Path
		//-------------------------------------------------------------------------
		AddPurifyingStatue(3, 2743, 24, 45,
			"{#666666}*The southern guardian stands silent and dark, its blessing dormant*{/}",
			"Without the statue's protection, this road has become treacherous.",
			"{#666666}*Sacred light erupts from the statue! The guardian's blessing returns!*{/}",
			"The southern road is safe once more. The goddess hears your prayers.");

		// Purifying Statue 4 - Western Path
		//-------------------------------------------------------------------------
		AddPurifyingStatue(4, -187, -1427, 45,
			"{#666666}*An ancient guardian carved from blessed wood stands watch over the western approach*{/}",
			"Its holy power has faded. Travelers on this road are no longer safe.",
			"{#666666}*Divine power surges through the statue! The guardian awakens with radiant light!*{/}",
			"The western road is blessed once more. Your prayers have been answered.");

		// Purifying Statue 5 - Central Crossroads
		//-------------------------------------------------------------------------
		AddPurifyingStatue(5, 360, -37, 45,
			"{#666666}*The greatest of the five guardians stands at the forest crossroads. Once, this statue's blessing protected the entire forest network of roads*{/}",
			"But now it stands dark and silent. Without its power, no road is truly safe.",
			"{#666666}*Brilliant holy light explodes from the statue! The Guardian's full blessing returns!*{/}",
			"The forest roads are completely safe now. Your devotion has restored what was lost.");

		// =====================================================================
		// RUBBLE INSPECTION POINTS
		// =====================================================================
		// For Quest 1004 - Reconstructing the Bridges
		// =====================================================================

		void AddRubbleInspectionPoint(int rubbleNumber, int x, int y, int direction)
		{
			AddNpc(150231, " ", "f_siauliai_15_re", x, y, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_15_re", 1004);

				if (!character.Quests.IsActive(questId))
					return;

				var variableKey = $"Laima.Quests.f_siauliai_15_re.Quest1004.Bridge{rubbleNumber}";
				var isChecked = character.Variables.Perm.GetBool(variableKey, false);

				if (isChecked)
				{
					await dialog.Msg("{#666666}*You've already inspected this rubble pile*{/}");
					return;
				}

				var result = await character.TimeActions.StartAsync("Inspecting rubble...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var bridgesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_15_re.Quest1004.BridgesChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_siauliai_15_re.Quest1004.BridgesChecked", bridgesChecked + 1);

					character.ServerMessage($"Rubble piles inspected: {bridgesChecked + 1}/6");

					if (bridgesChecked + 1 >= 6)
					{
						character.ServerMessage("{#FFD700}All rubble piles inspected! Return to Soldier Henrik.{/}");
					}
				}
				else
				{
					character.ServerMessage("Inspection cancelled.");
				}
			});
		}

		// Rubble Pile 1
		AddRubbleInspectionPoint(1, 135, -18, 44);

		// Rubble Pile 2
		AddRubbleInspectionPoint(2, -33, -47, 45);

		// Rubble Pile 3
		AddRubbleInspectionPoint(3, 19, -486, 45);

		// Rubble Pile 4
		AddRubbleInspectionPoint(4, -726, -54, 45);

		// Rubble Pile 5
		AddRubbleInspectionPoint(5, 52, -1703, 45);

		// Rubble Pile 6
		AddRubbleInspectionPoint(6, 47, -2133, 44);

		// =====================================================================
		// FLOWER COLLECTION POINTS
		// =====================================================================
		// For Quest 1005 - The Beekeeper's Hope
		// =====================================================================

		void AddFlowerCollectionSpot(int spotNumber, int x, int y)
		{
			AddNpc(153067, "Bee-Attracting Flowers", "f_siauliai_15_re", x, y, 0, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_15_re", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg("A beautiful patch of colorful wildflowers. Bees would love these.");
					return;
				}

				var variableKey = $"Laima.Quests.f_siauliai_15_re.Quest1005.FlowerSpot{spotNumber}";
				var collected = character.Variables.Perm.GetBool(variableKey, false);

				if (collected)
				{
					await dialog.Msg("You've already gathered flowers from this patch.");
					return;
				}

				var result = await character.TimeActions.StartAsync("Gathering flowers...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650428, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650428);
					character.ServerMessage($"Bee-attracting flowers collected: {currentCount}/5");

					if (currentCount >= 5)
					{
						character.ServerMessage("All flowers collected! Return to Beekeeper Greta.");
					}
				}
				else
				{
					character.ServerMessage("Collection cancelled.");
				}
			});
		}

		// Flower Spot 1
		AddFlowerCollectionSpot(1, -47, -1298);

		// Flower Spot 2
		AddFlowerCollectionSpot(2, -233, -1356);

		// Flower Spot 3
		AddFlowerCollectionSpot(3, -342, -1509);

		// Flower Spot 4
		AddFlowerCollectionSpot(4, -174, -1648);

		// Flower Spot 5
		AddFlowerCollectionSpot(5, -104, -1565);

		// Flower Spot 6
		AddFlowerCollectionSpot(6, 58, -1556);

		// Flower Spot 7
		AddFlowerCollectionSpot(7, 176, -1496);

		// Flower Spot 8
		AddFlowerCollectionSpot(8, -435, -1666);

		// Flower Spot 9
		AddFlowerCollectionSpot(9, -419, -1800);

		// Flower Spot 10
		AddFlowerCollectionSpot(10, -243, -1143);

		// Flower Spot 11
		AddFlowerCollectionSpot(11, -64, -1234);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001: Cleansing the Corrupted Air
//-----------------------------------------------------------------------------
public class CleansingCorruptedAirQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_15_re", 1001);
		SetName("Cleansing the Corrupted Air");
		SetDescription("Cull the infected creatures spreading poison throughout the woods to help Scout Milda contain the contamination.");
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Scout] Milda", "f_siauliai_15_re");

		AddObjective("killJukopus", "Hunt corrupted Jukopus",
			new KillObjective(15, new[] { MonsterId.Sec_Jukopus }));
		AddObjective("killKepas", "Hunt poisoned Kepas",
			new KillObjective(5, new[] { MonsterId.Onion_Green }));
		AddObjective("killPokubu", "Hunt infected Pokubu",
			new KillObjective(5, new[] { MonsterId.Sec_Pokubu }));

		AddReward(new ExpReward(500, 350));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(162112, 1)); // Soldier's Short Bow
		AddReward(new ItemReward(640002, 10)); // HP potions
		AddReward(new ItemReward(640005, 6)); // SP potions
		AddReward(new ItemReward(640080, 3)); // Lv1 EXP Cards
	}
}

// Quest 1002: The Lost Caravan
//-----------------------------------------------------------------------------
public class LostCaravanQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_15_re", 1002);
		SetName("The Lost Caravan");
		SetDescription("Recover loose packages from the poisoned Kepas.");
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Merchant] Alfred", "f_siauliai_15_re");

		// Drop loose packages from Onion_Green (poisoned kepas)
		AddDrop(650399, 0.30f, MonsterId.Onion_Green);

		AddObjective("collectGoods", "Recover Loose Packages",
			new CollectItemObjective(650399, 8));

		AddReward(new ExpReward(400, 300));
		AddReward(new SilverReward(2500));
		AddReward(new ItemReward(640002, 12)); // HP potions
		AddReward(new ItemReward(640005, 10)); // SP potions
		AddReward(new ItemReward(640080, 2)); // Lv1 EXP Cards
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650399, character.Inventory.CountItem(650399),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650399, character.Inventory.CountItem(650399),
			InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003: The Hermit's Discovery
//-----------------------------------------------------------------------------
public class HermitDiscoveryQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_15_re", 1003);
		SetName("The Hermit's Discovery");
		SetDescription("Hunt corrupted boars to slow the spread of corruption in the forest.");
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Hermit] Torsten", "f_siauliai_15_re");

		AddObjective("killPokubu", "Hunt corrupted Pokubu",
			new KillObjective(15, new[] { MonsterId.Sec_Pokubu }));
		AddObjective("killArburn", "Hunt Pokuborn",
			new KillObjective(8, new[] { MonsterId.Sec_Arburn_Pokubu }));

		AddReward(new ExpReward(600, 400));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(640002, 15)); // HP potions
		AddReward(new ItemReward(640005, 12)); // SP potions
		AddReward(new ItemReward(532112, 1)); // Pokubu Leather Armor
		AddReward(new ItemReward(640081, 3)); // Lv2 EXP Cards
	}
}

// Quest 1004: Reconstructing the Bridges
//-----------------------------------------------------------------------------
public class TalesOfBridgesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_15_re", 1004);
		SetName("Reconstructing the Bridges");
		SetDescription("Inspect rubble piles coming off the deteriorating bridges to help assess the damage for reconstruction efforts.");
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Soldier] Henrik", "f_siauliai_15_re");

		AddObjective("checkBridges", "Inspect the rubble piles",
			new VariableCheckObjective("Laima.Quests.f_siauliai_15_re.Quest1004.BridgesChecked", 6, true));

		AddReward(new ExpReward(600, 400));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(202111, 1)); // Soldier's Iron Club
		AddReward(new ItemReward(640002, 10)); // HP potions
		AddReward(new ItemReward(640005, 8)); // SP potions
		AddReward(new ItemReward(640080, 3)); // Lv1 EXP Cards
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Clear rubble check variables
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_15_re.Quest1004.BridgesChecked");
		for (int i = 1; i <= 6; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_15_re.Quest1004.Bridge{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Clear rubble check variables
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_15_re.Quest1004.BridgesChecked");
		for (int i = 1; i <= 6; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_15_re.Quest1004.Bridge{i}");
		}
	}
}

// Quest 1005: The Beekeeper's Hope
//-----------------------------------------------------------------------------
public class BeekeeperHopeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_15_re", 1005);
		SetName("The Beekeeper's Hope");
		SetDescription("Gather bee-attracting flowers from around the forest to help Beekeeper Greta lure wild bees back to her farm.");
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Beekeeper] Greta", "f_siauliai_15_re");

		AddObjective("collectFlowers", "Collect bee-attracting flowers",
			new CollectItemObjective(650428, 5));

		// Rewards - Peaceful quest with good rewards
		AddReward(new ExpReward(500, 300));
		AddReward(new SilverReward(3500));
		AddReward(new ItemReward(581101, 1)); // HP Necklace
		AddReward(new ItemReward(640002, 12)); // HP potions
		AddReward(new ItemReward(640005, 10)); // SP potions
		AddReward(new ItemReward(640097, 5)); // Stamina potions
		AddReward(new ItemReward(640080, 3)); // Lv1 EXP Cards
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(650428, character.Inventory.CountItem(650428),
			InventoryItemRemoveMsg.Destroyed);

		// Clear flower collection variables
		for (int i = 1; i <= 11; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_15_re.Quest1005.FlowerSpot{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(650428, character.Inventory.CountItem(650428),
			InventoryItemRemoveMsg.Destroyed);

		// Clear flower collection variables
		for (int i = 1; i <= 11; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_15_re.Quest1005.FlowerSpot{i}");
		}
	}
}

// Quest 1006: Seeds of Hope (Farmer's Request)
//-----------------------------------------------------------------------------
public class SeedsOfHopeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_15_re", 1006);
		SetName("Seeds of Hope");
		SetDescription("Seek out the ancient stone golem guardian and request its blessing to purify Morku's corrupted farmland.");
		SetLocation("f_siauliai_15_re");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Morku", "f_siauliai_15_re");

		AddObjective("seekBlessing", "Seek the Stone Golem's blessing",
			new ManualObjective());

		AddReward(new ExpReward(800, 600));
		AddReward(new SilverReward(6000));
		AddReward(new ItemReward(640002, 15)); // HP potions
		AddReward(new ItemReward(640005, 12)); // SP potions
		AddReward(new ItemReward(222101, 1)); // Black Wooden Shield
		AddReward(new ItemReward(640080, 4)); // Lv2 EXP Cards
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove purified essence if any remains
		character.Inventory.Remove(650713, character.Inventory.CountItem(650713),
			InventoryItemRemoveMsg.Destroyed);

		// Clear riddle solved variable
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_15_re.Quest1006.RiddleSolved");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove purified essence
		character.Inventory.Remove(650713, character.Inventory.CountItem(650713),
			InventoryItemRemoveMsg.Destroyed);

		// Clear riddle solved variable
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_15_re.Quest1006.RiddleSolved");
	}
}
