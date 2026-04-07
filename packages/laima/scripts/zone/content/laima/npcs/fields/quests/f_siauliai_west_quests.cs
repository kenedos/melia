//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in West Siauliai Woods for post-demon war storyline.
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


public class FSiauliaiWestQuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Lost Farmer
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Lost Farmer] Bronius"), "f_siauliai_west", -1895, -1007, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_west", 1001);

			dialog.SetTitle(L("Bronius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I used to tend these farmlands... before the war. Now look at them - overrun by Kepas and those cursed leaf bugs."));

				var response = await dialog.Select(L("I tried harvesting this morning but those creatures drove me away. My tools are still out there somewhere."),
					Option(L("I'll help you recover your tools"), "help"),
					Option(L("Why not just buy new tools?"), "buy"),
					Option(L("Good luck with that"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("You would? Thank you! My farming tools are scattered across the farmlands - I dropped them in bags when I was fleeing."));

						if (await dialog.YesNo(L("I need my Shovel, Rake, and Sickle. They're in three different bags. Will you search for them?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The bags should be somewhere in the farmlands near the Kepas and leaf bugs. Please be careful - those creatures are more aggressive than they look."));
						}
						break;

					case "buy":
						await dialog.Msg(L("Buy new tools? {#666666}*He laughs bitterly*{/} Do I look like I have silver to spare?"));
						await dialog.Msg(L("Those tools belonged to my father, and his father before him. They're family heirlooms, even if they're falling apart."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He sighs and returns to staring at his ruined farmland*{/}"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var hasShovel = character.Inventory.HasItem(662053);
				var hasRake = character.Inventory.HasItem(662055);
				var hasSickle = character.Inventory.HasItem(662056);

				if (hasShovel && hasRake && hasSickle)
				{
					await dialog.Msg(L("{#666666}*His eyes light up when he sees the tools*{/}"));
					await dialog.Msg(L("You found them! All three! I can't believe it - I thought they were lost forever!"));
					await dialog.Msg(L("These tools have been in my family for three generations. My grandfather worked this soil with these very implements."));
					await dialog.Msg(L("Here, take this. It's not much, but it's all I can offer. And... thank you, truly."));

					character.Inventory.Remove(662053, 1, InventoryItemRemoveMsg.Given);
					character.Inventory.Remove(662055, 1, InventoryItemRemoveMsg.Given);
					character.Inventory.Remove(662056, 1, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					var foundCount = (hasShovel ? 1 : 0) + (hasRake ? 1 : 0) + (hasSickle ? 1 : 0);
					await dialog.Msg(LF("Have you found my tools? You have {0} of 3.", foundCount));
					await dialog.Msg(L("They should be in bags scattered around the farmlands. The Kepas and leaf bugs probably knocked them around."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("With my tools back, I can start working the fields again. It won't be easy, but it's a beginning."));
				await dialog.Msg(L("If you're heading deeper into the woods, watch yourself. Strange things have been happening near the old broken house."));
			}
		});

		// Bag NPCs - Talk to collect tools
		//-------------------------------------------------------------------------
		void AddBagNpc(int bagNumber, string toolName, int itemId, int x, int z)
		{
			AddNpc(155005, L("Bag"), "f_siauliai_west", x, z, 45, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_west", 1001);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered bag lies on the ground, partially covered in dirt*{/}"));
					return;
				}

				if (character.Inventory.HasItem(itemId))
				{
					await dialog.Msg(L("{#666666}*You've already searched this bag*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Searching bag..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(itemId, 1, InventoryAddType.PickUp);
					character.ServerMessage(LF("Found: {0}", toolName));

					var toolCount = (character.Inventory.HasItem(662053) ? 1 : 0) +
									(character.Inventory.HasItem(662055) ? 1 : 0) +
									(character.Inventory.HasItem(662056) ? 1 : 0);

					character.ServerMessage(LF("Tools found: {0}/3", toolCount));

					if (toolCount >= 3)
					{
						character.ServerMessage(L("{#FFD700}All farming tools found! Return to Bronius.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Search cancelled."));
				}
			});
		}

		AddBagNpc(1, "Shovel", 662053, -2044, -1274);
		AddBagNpc(2, "Rake", 662055, -2057, -986);
		AddBagNpc(3, "Sickle", 662056, -1719, -671);

		// Eccentric Herbalist
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Herbalist] Vesta"), "f_siauliai_west", 730, 381, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_west", 1002);

			dialog.SetTitle(L("Vesta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A woman crouches by the pathway, examining plants with an unusual magnifying crystal*{/}"));
				await dialog.Msg(L("Fascinating! The Chinency monsters have been consuming these herbs, but the plants show signs of magical residue..."));

				var response = await dialog.Select(L("Oh! I didn't see you there. Are you also studying the local flora?"),
					Option(L("I could help with your research"), "help"),
					Option(L("What are you researching?"), "research"),
					Option(L("Just passing through"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("A willing assistant! Excellent! I need samples from the creatures that feed on these magical herbs."));

						if (await dialog.YesNo(L("Specifically, I need samples from Chinency monsters. Their digestive systems process the herbs uniquely. Will you collect samples for me?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Perfect! I need 8 Chinency Roots. The magical residue in their bodies will tell me so much!"));
							await dialog.Msg(L("They're right over there, shouldn't be too difficult."));
						}
						break;

					case "research":
						await dialog.Msg(L("I'm studying how magical contamination from the demon war affected plant life in this region."));
						await dialog.Msg(L("The monsters that eat these plants show interesting mutations. If I can understand the process, perhaps we can develop antidotes or even beneficial potions!"));
						break;

					case "leave":
						await dialog.Msg(L("Safe travels! Mind the mutated plants - some have developed defensive thorns."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var chinencyCount = character.Inventory.CountItem(ItemId.Misc_Bokchoy2);

				if (chinencyCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She eagerly takes the samples*{/}"));
					await dialog.Msg(L("Magnificent specimens! Look at these cellular structures - the magical residue has integrated into their biology!"));
					await dialog.Msg(L("{#666666}*She examines the samples through her magnifying crystal*{/}"));
					await dialog.Msg(L("This confirms my hypothesis! The war's magical fallout has created a new ecological balance. Fascinating and terrifying in equal measure."));
					await dialog.Msg(L("Your contribution to science will not be forgotten! Here, take these - they're experimental potions I've been developing."));

					character.Inventory.Remove(ItemId.Misc_Bokchoy2, 8, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("You can get some Chinency Roots from these plant-like things over there."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I'm making excellent progress with my research thanks to your samples!"));
				await dialog.Msg(L("The magical contamination patterns are complex, but I believe I'm close to a breakthrough in understanding post-war ecology."));
			}
		});

		// Village Herbalist
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Village Herbalist] Henrik"), "f_siauliai_west", -731, -413, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_west", 1003);

			dialog.SetTitle(L("Henrik"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An elderly herbalist stands near his cottage, studying crystalline formations*{/}"));
				await dialog.Msg(L("Ah, a traveler! Welcome. I've been researching the Rootcrystals that grow in these woods."));

				var response = await dialog.Select(L("These aren't ordinary crystals - they're manifestations of the World Tree's roots, extending throughout the entire continent."),
					Option(L("Tell me more about these Rootcrystals"), "more"),
					Option(L("How do they interact with humans?"), "interact"),
					Option(L("Just passing through"), "leave")
				);

				switch (response)
				{
					case "more":
						await dialog.Msg(L("The World Tree oversees the entire continent, and its roots run deep beneath the soil. Sometimes these roots crystallize when exposed to magical energy."));
						await dialog.Msg(L("I believe studying how they respond to being destroyed could reveal much about the World Tree's connection to our world."));

						if (await dialog.YesNo(L("Would you help me with this research? I need you to destroy 2 Rootcrystals so I can observe the World Tree's response.")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Excellent! When you destroy a Rootcrystal, the World Tree subtly reacts. I'll be monitoring the mystical fluctuations from here."));
							await dialog.Msg(L("You'll find Rootcrystals scattered throughout these woods. Return when you've destroyed 2 of them."));
						}
						break;

					case "interact":
						await dialog.Msg(L("Fascinating question! The Rootcrystals appear to resonate with human life force. Those who spend time near them report feeling... connected somehow."));
						await dialog.Msg(L("I theorize that the World Tree uses these crystalline formations to monitor and perhaps even protect the life on this continent."));
						await dialog.Msg(L("By studying how the crystals react when destroyed, I hope to understand this relationship better."));
						break;

					case "leave":
						await dialog.Msg(L("Safe travels! The Rootcrystals are harmless, but the monsters around them are not."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRootcrystals", out var monObj)) return;

				var killCount = monObj.Count;

				if (killCount >= 2)
				{
					await dialog.Msg(L("{#666666}*His eyes light up with excitement*{/}"));
					await dialog.Msg(L("You've done it! I've been recording the mystical fluctuations this entire time. The data is remarkable!"));
					await dialog.Msg(L("{#666666}*He flips through pages of notes covered in diagrams and measurements*{/}"));
					await dialog.Msg(L("As I suspected, the World Tree responds to each destruction by channeling energy to nearby roots. It's actively maintaining balance across the continent!"));
					await dialog.Msg(L("This proves the World Tree is far more than legend - it's a living entity watching over us all. Your contribution to this research is invaluable!"));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The energy fluctuations are fascinating! Continue destroying them so I can gather more data."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Thanks to your help, I've made a breakthrough in understanding the World Tree's influence on our world."));
				await dialog.Msg(L("The Rootcrystals are like the World Tree's eyes and hands, spread across the entire continent. Remarkable!"));
			}
		});

		// Bridge Guard
		//-------------------------------------------------------------------------
		AddNpc(150219, L("[Bridge Guard] Tomas"), "f_siauliai_west", 334, -331, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_west", 1004);

			dialog.SetTitle(L("Tomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A guard stands watch near the bridge, looking concerned*{/}"));
				await dialog.Msg(L("Greetings, traveler. I'm responsible for maintaining the bridge, but I've run into a problem."));

				var response = await dialog.Select(L("The bridge supports need reinforcement, but I need special materials."),
					Option(L("I can help gather materials"), "help"),
					Option(L("What materials do you need?"), "materials"),
					Option(L("Good luck with that"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("Thank you! I need Infrorocktor Fragments from the Infrorocktor monsters. They're dense and perfect for reinforcing the foundation."));

						if (await dialog.YesNo(L("Can you collect 10 Infrorocktor Fragments for me? The bridge safety depends on it.")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Excellent! Infrorocktor can be found throughout the southern areas of these woods."));
							await dialog.Msg(L("Once you have 10 Infrorocktor Fragments, bring them back and I'll get to work on the repairs."));
						}
						break;

					case "materials":
						await dialog.Msg(L("The Infrorocktor monsters have these dense cores that are perfect for construction work."));
						await dialog.Msg(L("They're incredibly sturdy - much better than regular stone for bridge reinforcement."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He nods and returns to inspecting the bridge*{/}"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var coreCount = character.Inventory.CountItem(666025);

				if (coreCount >= 10)
				{
					await dialog.Msg(L("{#666666}*His face lights up when he sees the Infrorocktor Fragments*{/}"));
					await dialog.Msg(L("Perfect! These are exactly what I need! The density is ideal for the bridge foundation."));
					await dialog.Msg(L("{#666666}*He begins placing the cores strategically around the bridge supports*{/}"));
					await dialog.Msg(L("With these reinforcements, this bridge will be safe for years to come. Thank you for your help!"));

					character.Inventory.Remove(666025, 10, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Have you collected the Infrorocktor Fragments? You have {0}/10 so far.", coreCount));
					await dialog.Msg(L("Infrorocktor can be found throughout the southern areas of West Siauliai Woods."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The bridge is holding up wonderfully thanks to your help!"));
				await dialog.Msg(L("Travelers can cross safely now. I owe you one!"));
			}
		});

		// Traveling Merchant
		//-------------------------------------------------------------------------
		AddNpc(151080, L("[Traveling Merchant] Sigrid"), "f_siauliai_west", -541, 1334, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_west", 1005);

			dialog.SetTitle(L("Sigrid"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A merchant sits surrounded by broken wagon parts, looking distressed*{/}"));
				await dialog.Msg(L("Of all the places for my wagon to break down... right in the middle of monster territory!"));

				var response = await dialog.Select(L("I'm trying to reach Klaipeda with these goods, but I can't move the wagon like this."),
					Option(L("I can help repair it"), "help"),
					Option(L("What happened to your wagon?"), "wagon"),
					Option(L("Maybe abandon the wagon?"), "abandon")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("You're a lifesaver! The problem is, I need specific materials that I don't have."));

						if (await dialog.YesNo(L("I need Leaf Bug Feelers and Hanaming Petals. These materials can be used to fix my wagon! Can you gather these materials for me?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wonderful! Please lookg for these plant-based monsters in the area."));
						}
						break;

					case "wagon":
						await dialog.Msg(L("Hit a rock hidden in the road. Cracked the axle and broke several support beams."));
						await dialog.Msg(L("Without repairs, this wagon isn't going anywhere. And I can't afford to lose this cargo - it's my entire livelihood!"));
						break;

					case "abandon":
						await dialog.Msg(L("{#666666}*She looks horrified*{/} Abandon it? This cargo represents three months of trading!"));
						await dialog.Msg(L("I'd rather take my chances here than lose everything I've worked for."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var petalCount = character.Inventory.CountItem(645024);
				var feelerCount = character.Inventory.CountItem(645262);

				if (petalCount >= 8 && feelerCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She eagerly examines the materials*{/}"));
					await dialog.Msg(L("This is exactly what I need! The petals are strong enough to serve as cover, and this feeler is perfect rope material!"));
					await dialog.Msg(L("{#666666}*She works quickly, replacing broken parts and reinforcing the wagon structure*{/}"));
					await dialog.Msg(L("There! Good as new! Well, good enough to reach Klaipeda at least."));
					await dialog.Msg(L("Here, take these goods from my wagon. It's the least I can do to thank you for saving my business!"));

					// Remove those items here because they're normal materials,
					// not actual quest items.
					character.Inventory.Remove(645024, 8, InventoryItemRemoveMsg.Given);
					character.Inventory.Remove(645262, 8, InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("How's the material gathering going? This wagon won't budge at all!"));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Thanks to you, I can make it safely to Klaipeda!"));
				await dialog.Msg(L("I'm still preparing for the trip, but I'll make it there someday!"));
			}
		});

		// Wagon
		//-------------------------------------------------------------------------
		AddNpc(45316, L("Wagon"), "f_siauliai_west", -606, 1345, 135);
	}
}

//-----------------------------------------------------------------------------
// Quests
//-----------------------------------------------------------------------------

// Quest 1001: Lost Farming Tools
//-----------------------------------------------------------------------------
public class FSiauliaiWestFarmingToolsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_west", 1001);
		SetName(L("The Farmer's Legacy"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Bronius recover his family's heirloom farming tools by searching through bags scattered across the farmlands when monsters drove him away."));
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Lost Farmer] Bronius"), "f_siauliai_west");

		// Search bags to collect the three farming tools
		AddObjective("collectShovel", L("Collect Shovel from a bag"),
			new CollectItemObjective(662053, 1));
		AddObjective("collectRake", L("Collect Rake from a bag"),
			new CollectItemObjective(662055, 1));
		AddObjective("collectSickle", L("Collect Sickle from a bag"),
			new CollectItemObjective(662056, 1));

		// Rewards
		AddReward(new ExpReward(350, 230));
		AddReward(new SilverReward(2000));
		AddReward(new ItemReward(640002, 5)); // Small HP potions
		AddReward(new ItemReward(640005, 5)); // Small SP potions
		AddReward(new ItemReward(221103, 1)); // Wooden Kite Shield
		AddReward(new ItemReward(640080, 2)); // Lv1 EXP Cards
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(662053, character.Inventory.CountItem(662053), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(662055, character.Inventory.CountItem(662055), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(662056, character.Inventory.CountItem(662056), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(662053, character.Inventory.CountItem(662053), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(662055, character.Inventory.CountItem(662055), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(662056, character.Inventory.CountItem(662056), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002: Herbalist's Research
//-----------------------------------------------------------------------------
public class FSiauliaiWestHerbalistQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_west", 1002);
		SetName(L("Magical Contamination Study"));
		SetType(QuestType.Sub);
		SetDescription(L("Collect samples from Bokchoy and Chinency monsters to help Vesta's research on post-war magical contamination in the local ecosystem."));
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Vesta"), "f_siauliai_west");

		// Collect samples (items already drop normally)
		AddObjective("collectChinency", L("Collect Chinency Roots"),
			new CollectItemObjective(ItemId.Misc_Bokchoy2, 8));

		// Rewards
		AddReward(new ExpReward(420, 280));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640002, 5)); // Small HP potions
		AddReward(new ItemReward(640005, 5)); // Small SP potions
		AddReward(new ItemReward(640097, 5)); // Stamina potions
		AddReward(new ItemReward(531103, 1)); // Leather Armor
		AddReward(new ItemReward(640080, 3)); // Lv1 EXP Cards
	}
}

// Quest 1003: World Tree Research
//-----------------------------------------------------------------------------
public class FSiauliaiWestVillageRemedyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_west", 1003);
		SetName(L("Roots of the World Tree"));
		SetType(QuestType.Sub);
		SetDescription(L("Help Henrik with his research by destroying 2 Rootcrystals."));
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Village Herbalist] Henrik"), "f_siauliai_west");

		// Kill Rootcrystals
		AddObjective("killRootcrystals", L("Destroy Rootcrystals"),
			new KillObjective(2, MonsterId.Rootcrystal_01));

		// Rewards
		AddReward(new ExpReward(480, 320));
		AddReward(new SilverReward(3500));
		AddReward(new ItemReward(640002, 12)); // Small HP potions
		AddReward(new ItemReward(640005, 12)); // Small SP potions
		AddReward(new ItemReward(501103, 1)); // Leather Gloves
		AddReward(new ItemReward(640080, 3)); // Lv1 EXP Cards
	}
}

// Quest 1004: Bridge Reinforcement
//-----------------------------------------------------------------------------
public class FSiauliaiWestRunestoneQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_west", 1004);
		SetName(L("Bridge Maintenance"));
		SetType(QuestType.Sub);
		SetDescription(L("Help bridge guard Tomas reinforce the bridge by collecting Infrorocktor Fragments from Infrorocktor monsters."));
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bridge Guard] Tomas"), "f_siauliai_west");

		// Add quest item drops
		AddDrop(666025, 0.35f, MonsterId.InfroRocktor);

		// Collect Infrorocktor Fragments
		AddObjective("collectCores", L("Collect Infrorocktor Fragments from Infrorocktor"),
			new CollectItemObjective(666025, 10));

		// Rewards
		AddReward(new ExpReward(400, 300));
		AddReward(new SilverReward(3000));
		AddReward(new ItemReward(640002, 10)); // Small HP potions
		AddReward(new ItemReward(640005, 10)); // Small SP potions
		AddReward(new ItemReward(511103, 1)); // Leather Boots
		AddReward(new ItemReward(640080, 2)); // Lv1 EXP Cards
	}

	public override void OnComplete(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(666025, character.Inventory.CountItem(666025), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		// Remove quest items
		character.Inventory.Remove(666025, character.Inventory.CountItem(666025), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005: Broken Wagon Repairs
//-----------------------------------------------------------------------------
public class FSiauliaiWestMerchantQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_west", 1005);
		SetName(L("The Broken Wagon"));
		SetType(QuestType.Sub);
		SetDescription(L("Help traveling merchant Sigrid repair her broken wagon by collecting Hanaming Petals and Leaf Bug Feelers."));
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Traveling Merchant] Sigrid"), "f_siauliai_west");

		// Collect samples (Items already drop normally)
		AddObjective("collectHanaming", L("Collect Hanaming Petals"),
			new CollectItemObjective(645024, 8));
		AddObjective("collectLeafBug", L("Collect Leaf Bug Feelers"),
			new CollectItemObjective(645262, 8));

		// Rewards
		AddReward(new ExpReward(420, 280));
		AddReward(new SilverReward(4500));
		AddReward(new ItemReward(640002, 10)); // Small HP potions
		AddReward(new ItemReward(640005, 10)); // Small SP potions
		AddReward(new ItemReward(521103, 1)); // Leather Pants
		AddReward(new ItemReward(640080, 3)); // Lv1 EXP Cards
	}
}
