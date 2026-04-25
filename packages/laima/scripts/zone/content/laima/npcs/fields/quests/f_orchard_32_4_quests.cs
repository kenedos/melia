//--- Melia Script ----------------------------------------------------------
// Seir Rainforest Quest NPCs
//--- Description -----------------------------------------------------------
// Ferret-cartel quests for the deeper rainforest map.
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

public class FOrchard324QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Canopy Volley
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Ranger] Vittorin"), "f_orchard_32_4", 1400, 40, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1001);

			dialog.SetTitle(L("Vittorin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Seir Rainforest looks peaceful until you look up. Ferret Archers nest in every canopy, and they shoot first."));
				await dialog.Msg(L("The trade road through here has been abandoned for a month. Merchants say the arrows aren't aimed to kill - they're aimed to scare. That's almost worse."));
				await dialog.Msg(L("Thin out the canopy archers and the merchants will come back. Without them, Alemeth's festival loses its spice trade."));

				var response = await dialog.Select(L("How many Archers?"),
					Option(L("I'll kill the canopy"), "help"),
					Option(L("Aimed to scare?"), "info"),
					Option(L("Reroute the road"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two. Look up often - they love to drop shots right past your ear."));
						await dialog.Msg(L("Good hunting."));
						break;

					case "info":
						await dialog.Msg(L("Every arrow's been a warning shot. They're marking territory, not hunting."));
						await dialog.Msg(L("Doesn't matter. A warning shot still turns merchants around."));
						break;

					case "leave":
						await dialog.Msg(L("The reroute crosses the abbey pass. Two weeks longer, with twice the bandits. It's Seir or nothing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Canopy's quiet. I can hear the river again - that's how I know it's safe."));
					await dialog.Msg(L("Take your pay. The merchants will owe you a round when they get back."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still arrows coming down. Keep climbing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First caravan through yesterday. Not one arrow. Like old times."));
			}
		});

		// Quest 2: Rainforest Crystal Bloom
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Crystal Scholar] Immre"), "f_orchard_32_4", 900, 550, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1002);

			dialog.SetTitle(L("Immre"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Rootcrystals in Seir bloom differently than any I've seen. Petal-like growths, pale violet, swollen with mana."));
				await dialog.Msg(L("I can't get close - they lash out like something alive. But you can crack them open."));
				await dialog.Msg(L("Five blooms is all I need. One paper's worth of research, and then I go home to Fedimian with something the Academy has never catalogued."));

				var response = await dialog.Select(L("Five blooms?"),
					Option(L("I'll break five Rootcrystals"), "help"),
					Option(L("Why are they alive?"), "info"),
					Option(L("Do your own fieldwork"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Look for the ones with the violet swellings - those are the bloomed ones. Dull grey clusters are dormant."));
						await dialog.Msg(L("Bring me five. I'll cite you in the footnotes."));
						break;

					case "info":
						await dialog.Msg(L("Theory is they're absorbing residual mana from something deeper in the forest. A ley-line, perhaps - or worse."));
						await dialog.Msg(L("Either way, the blooms are the proof. If I can test one, I'll know which."));
						break;

					case "leave":
						await dialog.Msg(L("The last scholar who tried lost three fingers. I'd rather keep mine, thank you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var bloomCount = character.Inventory.CountItem(650310);

				if (bloomCount >= 5)
				{
					await dialog.Msg(L("Five! And the color - exquisite. Still warm with mana, look at that."));
					await dialog.Msg(L("Take this with my thanks. I'll dedicate the paper to you if you'd like - or keep your name out of it, no offense taken."));

					character.Inventory.Remove(650310, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep cracking them. You've got {0} of five.", bloomCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The paper's drafted. The Academy is going to lose its mind over the mana readings."));
			}
		});

		// Quest 3: The Contraband Stash
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Customs Officer] Brenna"), "f_orchard_32_4", -1000, -600, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1003);

			dialog.SetTitle(L("Brenna"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ferret Searchers have been combing Seir for months. They're not foraging - they're working for somebody."));
				await dialog.Msg(L("Every so often a Searcher buries a little seed pouch. Contraband. Narcotic spores, by the smell of it."));
				await dialog.Msg(L("I need four pouches for evidence, and the Searcher count down a good bit so the network stalls while Orsha moves on the leadership."));

				var response = await dialog.Select(L("Searchers and pouches?"),
					Option(L("I'll do both"), "help"),
					Option(L("Narcotic spores?"), "info"),
					Option(L("Call in the garrison"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The pouches are small, wrapped in oiled leaves. Look for disturbed soil under the Searcher routes."));
						await dialog.Msg(L("Don't open them. Don't even smell them. Just bring them to me sealed."));
						break;

					case "info":
						await dialog.Msg(L("Dried spores from some kind of fungal tree. Mixed with tobacco, they're stronger than anything we've seen."));
						await dialog.Msg(L("Two caravans from Fedimian lost half their crew to the stuff last month. I'm putting a stop to it."));
						break;

					case "leave":
						await dialog.Msg(L("The garrison is stretched thin. That's why I'm out here alone. That's why I'm asking you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSearchers", out var killObj)) return;
				if (!quest.TryGetProgress("recoverPouches", out var pouchObj)) return;

				if (killObj.Done && pouchObj.Done)
				{
					await dialog.Msg(L("Four pouches, all sealed. That's the evidence I needed."));
					await dialog.Msg(L("Orsha will move on the cartel leadership tonight. Your name goes in the report, if you want credit."));

					character.Inventory.Remove(650700, character.Inventory.CountItem(650700), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Ferret Searchers. ");
					if (!pouchObj.Done)
						status += L("Recover more contraband seed pouches. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The cartel's top three are in Orsha's cells. Paperwork's a nightmare but the forest is cleaner."));
			}
		});

		// Contraband Seed Pouch Points
		//-------------------------------------------------------------------------
		void AddPouchStash(int pouchNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Disturbed Soil"), "f_orchard_32_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_32_4", 1003);
				var variableKey = $"Laima.Quests.f_orchard_32_4.Quest1003.Pouch{pouchNum}";
				var spawnedKey = $"Laima.Quests.f_orchard_32_4.Quest1003.Pouch{pouchNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*Loose soil, recently turned*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already dug up this stash*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 35)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Ferret_Searcher, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FFCC66}Ferret Searchers scramble out from the brush to defend the stash!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Digging up stash..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650700, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Contraband Seed Pouch"));

					var currentCount = character.Inventory.CountItem(650700);
					character.ServerMessage(LF("Pouches recovered: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All four pouches recovered! Return to Brenna.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You stopped digging."));
				}
			});
		}

		AddPouchStash(1, -500, -300, 0);
		AddPouchStash(2, -800, -550, 0);
		AddPouchStash(3, -1200, -600, 0);
		AddPouchStash(4, -700, -750, 0);

		// Quest 4: The Cartel Ledgers
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Investigator] Marek"), "f_orchard_32_4", -100, 500, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1004);

			dialog.SetTitle(L("Marek"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ferret Vendors aren't vendors. They're bookkeepers. Every one of them carries a little ledger stitched from bark."));
				await dialog.Msg(L("Those ledgers track routes, suppliers, buyers - the whole cartel, written in a chittering shorthand I'm finally starting to crack."));
				await dialog.Msg(L("Bring me five ledgers. Kill twelve Vendors while you're at it so the network runs thin. I'll do the translation."));

				var response = await dialog.Select(L("Five ledgers?"),
					Option(L("I'll bring the ledgers"), "help"),
					Option(L("How do they chitter a ledger?"), "info"),
					Option(L("Read them yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Vendors always drop their ledgers when struck - they're light sleepers and lighter pocketed."));
						await dialog.Msg(L("Twelve Vendors, five ledgers. Simple math, complicated fieldwork."));
						break;

					case "info":
						await dialog.Msg(L("Dots, scratches, little tooth-marks. Each mark is a cargo type, each row is a week."));
						await dialog.Msg(L("If you squint it's almost elegant. Which is maddening, because these are ferrets."));
						break;

					case "leave":
						await dialog.Msg(L("I'm trying to. Hard to read a ledger still clutched in a defiant paw."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killVendors", out var killObj)) return;
				if (!quest.TryGetProgress("gatherLedgers", out var ledgerObj)) return;

				if (killObj.Done && ledgerObj.Done)
				{
					await dialog.Msg(L("Five ledgers! And twelve Vendors cooler than they were this morning. The network's gutted."));
					await dialog.Msg(L("Take your pay. I'll be up all night translating."));

					character.Inventory.Remove(650420, character.Inventory.CountItem(650420), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Ferret Vendors. ");
					if (!ledgerObj.Done)
						status += L("Recover more cartel ledgers. ");

					await dialog.Msg(LF("Keep going. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Translated every ledger. Orsha has the supplier names, the buyer names, everything."));
			}
		});

		// Quest 5: The Cartel Kingpin
		//-------------------------------------------------------------------------
		AddNpc(20011, L("[Bounty Captain] Levko"), "f_orchard_32_4", 1350, 110, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1005);
			var kingpinSpawnedKey = "Laima.Quests.f_orchard_32_4.Quest1005.KingpinSpawned";

			dialog.SetTitle(L("Levko"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Bearers are the cartel's muscle. Big, armored, mean when roused. They answer to one ferret - the Kingpin."));
				await dialog.Msg(L("The Kingpin doesn't show himself while his Bearers are intact. Kill ten Bearers, and his pride - and a lot of shouting in chittering ferret-cant - will bring him out."));
				await dialog.Msg(L("Bounty's set. His seal-ring alone is worth more than a house in Orsha."));

				var response = await dialog.Select(L("Sounds fair."),
					Option(L("I'll take the Kingpin"), "help"),
					Option(L("Seal-ring?"), "info"),
					Option(L("Leave the crown"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten Bearers. Don't rush the count - he won't come out till they're thinned for real."));
						await dialog.Msg(L("When he shows, he shows hard. Good luck."));
						break;

					case "info":
						await dialog.Msg(L("A huge ring carved from amber, stamped with the cartel mark. Every Vendor bows to it."));
						await dialog.Msg(L("Crack the ring, and every Vendor in Seir loses their permission slip."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe. The bounty doesn't expire."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBearers", out var bearerObj)) return;
				if (!quest.TryGetProgress("killKingpin", out var kingObj)) return;

				if (bearerObj.Done && kingObj.Done)
				{
					await dialog.Msg(L("The ring! Amber's cracked but the stamp's clean - that's him."));
					await dialog.Msg(L("Bounty paid, plus my share. The cartel's done in Seir."));

					character.Variables.Perm.Remove(kingpinSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (bearerObj.Done && !kingObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(kingpinSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(kingpinSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Ferret_Vendor, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Bearers are thin enough. Listen - the chittering's picked up. He's coming."));
							await dialog.Msg(L("{#FF9966}Move, now! He won't give you twice!{/}"));
							character.ServerMessage(L("{#FF9966}The Cartel Kingpin emerges, screaming orders!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out there. Hunt him down before he slips back in."));
					}
				}
				else
				{
					await dialog.Msg(L("Bearers still thick on the ground. Thin them, then he'll come."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ring's in the case on my wall. The cartel's splinter-groups are fighting each other now. Progress."));
			}
		});

		// Quest 6: Seir Trail Cleanup
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Master] Denys"), "f_orchard_32_4", -1700, 870, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1006);

			dialog.SetTitle(L("Denys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Seir Trail is the only way out of the rainforest west. It's thick with Archers up top and Searchers at the roots."));
				await dialog.Msg(L("The caravans want assurance both species are thinned before they commit. Festival season's at stake."));

				var response = await dialog.Select(L("Both species?"),
					Option(L("I'll clear both"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Try the river route"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve of each. Archers up, Searchers down. Mind both eye-lines."));
						await dialog.Msg(L("Festival carts are staged already. Clear the trail and they roll by morning."));
						break;

					case "info":
						await dialog.Msg(L("Archers shoot from the canopy. Searchers pin your ankles in the roots. I hate the ankles more, personally."));
						await dialog.Msg(L("Neither is strictly deadlier. Together, impassable."));
						break;

					case "leave":
						await dialog.Msg(L("The river route washed out last month. Not passable till spring."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var archerObj)) return;
				if (!quest.TryGetProgress("killSearchers", out var searcherObj)) return;

				if (archerObj.Done && searcherObj.Done)
				{
					await dialog.Msg(L("Both species thinned to safe numbers. Caravans roll out at dawn."));
					await dialog.Msg(L("Take your cut. Every festival stall owes you a toast."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!archerObj.Done)
						status += L("Kill more Ferret Archers. ");
					if (!searcherObj.Done)
						status += L("Kill more Ferret Searchers. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three caravans through yesterday. The festival committee sent a thank-you cask."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Canopy Volley
//-----------------------------------------------------------------------------

public class CanopyVolleyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1001);
		SetName(L("Canopy Volley"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ferret Archers nesting in the Seir Rainforest canopy so merchant caravans can return."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger] Vittorin"), "f_orchard_32_4");

		AddObjective("killArchers", L("Kill Ferret Archers"),
			new KillObjective(22, new[] { MonsterId.Ferret_Archer }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1002 CLASS: Rainforest Crystal Bloom
//-----------------------------------------------------------------------------

public class RainforestCrystalBloomQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1002);
		SetName(L("Rainforest Crystal Bloom"));
		SetType(QuestType.Sub);
		SetDescription(L("Break bloomed Rootcrystals in Seir Rainforest and bring their violet blooms to the crystal scholar."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal Scholar] Immre"), "f_orchard_32_4");

		AddObjective("gatherBlooms", L("Gather rainforest crystal blooms"),
			new CollectItemObjective(650310, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 3)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650310, character.Inventory.CountItem(650310), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650310, character.Inventory.CountItem(650310), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Contraband Stash
//-----------------------------------------------------------------------------

public class TheContrabandStashQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1003);
		SetName(L("The Contraband Stash"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ferret Searchers combing the rainforest and dig up four contraband seed pouches for customs evidence."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Customs Officer] Brenna"), "f_orchard_32_4");

		AddObjective("killSearchers", L("Kill Ferret Searchers"),
			new KillObjective(15, new[] { MonsterId.Ferret_Searcher }));

		AddObjective("recoverPouches", L("Recover contraband seed pouches"),
			new CollectItemObjective(650700, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 3)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650700, character.Inventory.CountItem(650700), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_32_4.Quest1003.Pouch{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_32_4.Quest1003.Pouch{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650700, character.Inventory.CountItem(650700), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_32_4.Quest1003.Pouch{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_32_4.Quest1003.Pouch{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Cartel Ledgers
//-----------------------------------------------------------------------------

public class TheCartelLedgersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1004);
		SetName(L("The Cartel Ledgers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ferret Vendors and recover their bark-stitched ledgers to expose the cartel's trade network."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Investigator] Marek"), "f_orchard_32_4");

		AddObjective("killVendors", L("Kill Ferret Vendors"),
			new KillObjective(12, new[] { MonsterId.Ferret_Vendor }));

		AddObjective("gatherLedgers", L("Recover cartel ledgers"),
			new CollectItemObjective(650420, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 3)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650420, character.Inventory.CountItem(650420), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650420, character.Inventory.CountItem(650420), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Cartel Kingpin
//-----------------------------------------------------------------------------

public class TheCartelKingpinQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1005);
		SetName(L("The Cartel Kingpin"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill ten Ferret Bearers from the Kingpin's guard, then bring him down when his pride draws him out."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Captain] Levko"), "f_orchard_32_4");

		AddObjective("killBearers", L("Kill the Kingpin's Bearer guard"),
			new KillObjective(10, new[] { MonsterId.Ferret_Bearer_Elite }));

		AddObjective("killKingpin", L("Defeat the Cartel Kingpin"),
			new KillObjective(1, new[] { MonsterId.Ferret_Vendor }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 3)); // Recovery Potion
	}
}

// Quest 1006 CLASS: Seir Trail Cleanup
//-----------------------------------------------------------------------------

public class SeirTrailCleanupQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1006);
		SetName(L("Seir Trail Cleanup"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ferret Archers in the canopy and Ferret Searchers in the roots to reopen the Seir Trail."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Denys"), "f_orchard_32_4");

		AddObjective("killArchers", L("Kill Ferret Archers"),
			new KillObjective(12, new[] { MonsterId.Ferret_Archer }));

		AddObjective("killSearchers", L("Kill Ferret Searchers"),
			new KillObjective(12, new[] { MonsterId.Ferret_Searcher }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 3)); // Recovery Potion
	}
}
