//--- Melia Script ----------------------------------------------------------
// Zeraha Quest NPCs
//--- Description -----------------------------------------------------------
// Cheerful Ferret-tribe quests for the Zeraha orchard map.
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

public class FOrchard342QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Pebble Barrage
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Harvest Foreman] Tamas"), "f_orchard_34_2", 1480, 1270, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1001);

			dialog.SetTitle(L("Tamas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Welcome to Zeraha! Well - welcome to the part of Zeraha that isn't currently being pelted with acorns."));
				await dialog.Msg(L("The Ferret Slingers have decided my harvest crew is a fantastic target. Every five minutes - thwack, thwack, right on the forehead."));
				await dialog.Msg(L("Nobody's been seriously hurt, but half my crew won't come back to work until the Slingers are taught a lesson."));

				var response = await dialog.Select(L("How many need teaching?"),
					Option(L("I'll scare off the Slingers"), "help"),
					Option(L("Can't you just wear helmets?"), "info"),
					Option(L("Dodge better"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Chase off twenty-two Ferret Slingers from the harvest fields?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twenty-two should do it. They travel in little cliques - clear a clique, the rest scatter."));
							await dialog.Msg(L("Watch your forehead. Those acorns hurt."));
						}
						break;

					case "info":
						await dialog.Msg(L("We've tried. They just aim lower. A Ferret Slinger will absolutely hit you in the knee on purpose."));
						await dialog.Msg(L("Smart little devils. Just... unproductive to work around."));
						break;

					case "leave":
						await dialog.Msg(L("Easy for you to say - you haven't had an acorn to the ear at ten paces."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("chaseSlingers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Quiet at last! My crew is already back at the trees. You've saved this season's harvest."));
					await dialog.Msg(L("Take this with the company's thanks."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still hearing thwacks out there. Keep after them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Not a single acorn to the forehead in a week. I'd forgotten how peaceful that was."));
			}
		});

		// Quest 2: The Acorn Bounty
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Collector] Wren"), "f_orchard_34_2", 0, 0, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1002);

			dialog.SetTitle(L("Wren"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Have you seen the old oaks in Zeraha? They're ancient - and their acorns are the size of turnips."));
				await dialog.Msg(L("The Alemeth bakers pay handsomely for pristine giants. One acorn makes three loaves of acorn bread, and the flour keeps all winter."));
				await dialog.Msg(L("I'd climb for them myself but my back won't forgive me. A sharp-eyed scavenger like you though..."));

				var response = await dialog.Select(L("How many do you need?"),
					Option(L("I'll gather five giant acorns"), "help"),
					Option(L("Acorn bread? Really?"), "info"),
					Option(L("Climb your own trees"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Gather five pristine giant acorns from the old oaks?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Bless you! Look for oaks with the low-hanging pods - those are the ones with giants ready to drop."));
							await dialog.Msg(L("Mind the Searchers. Ferret Searchers think acorns are their personal property."));
						}
						break;

					case "info":
						await dialog.Msg(L("Slightly nutty, slightly sweet, holds butter like you wouldn't believe."));
						await dialog.Msg(L("It's the entire reason Alemeth's festival bakers do so well. Their secret ingredient comes from my trees."));
						break;

					case "leave":
						await dialog.Msg(L("My knees would disagree, but fair enough."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var acornCount = character.Inventory.CountItem(661094);

				if (acornCount >= 5)
				{
					await dialog.Msg(L("Five beauties! Look at the shine on these shells - the bakers will weep."));
					await dialog.Msg(L("Take this with my thanks. You've earned it twice over."));

					character.Inventory.Remove(661094, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Still a few more to go. You've got {0} of five.", acornCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Alemeth bakers sent a case of acorn bread by way of thanks. I saved a loaf for you!"));
			}
		});

		// Giant Acorn Tree Points
		//-------------------------------------------------------------------------
		void AddOakTree(int nodeNum, int x, int z, int direction)
		{
			AddNpc(46218, L("Low-Pod Oak"), "f_orchard_34_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_2", 1002);
				var variableKey = $"Laima.Quests.f_orchard_34_2.Quest1002.Acorn{nodeNum}";
				var spawnedKey = $"Laima.Quests.f_orchard_34_2.Quest1002.Acorn{nodeNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*An old oak whose pods sag with unusually heavy acorns*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already plucked this tree's giant acorn*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 30)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Ferret_Searcher, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FFCC66}A pair of Ferret Searchers scramble out, chittering angrily!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Plucking giant acorn..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(661094, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Plucked: Giant Acorn"));

					var currentCount = character.Inventory.CountItem(661094);
					character.ServerMessage(LF("Giant acorns plucked: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All giants gathered! Return to Wren.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You lowered the acorn back. Try again."));
				}
			});
		}

		AddOakTree(1, 200, 620, 0);
		AddOakTree(2, -300, 500, 0);
		AddOakTree(3, 400, 900, 0);
		AddOakTree(4, -100, 300, 0);
		AddOakTree(5, 620, 720, 0);

		// Quest 3: The Stolen Crates
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Caravan Master] Rudolf"), "f_orchard_34_2", -1000, 1500, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1003);

			dialog.SetTitle(L("Rudolf"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Last night a pack of Ferret Loaders ran off with four of my supply crates. Four!"));
				await dialog.Msg(L("The crates are too heavy for them to get far - they've just stashed them in their scrub-camp up the northwest ridge."));
				await dialog.Msg(L("I need the crates back, and the Loaders... well, let's just say a strong word from a capable traveler would improve their manners."));

				var response = await dialog.Select(L("How bad is the Loader camp?"),
					Option(L("I'll retrieve the crates and thin the Loaders"), "help"),
					Option(L("Why Ferret Loaders specifically?"), "info"),
					Option(L("File a claim"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Ferret Loaders and recover four stolen supply crates?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! The crates are crimson - you can't miss them. Rudolf Logistics stamps everything red."));
							await dialog.Msg(L("Loaders drop their haul the moment you make it clear you mean business. Fifteen should make it very clear."));
						}
						break;

					case "info":
						await dialog.Msg(L("Loaders are the pack-carriers of the tribe. Built like little brick walls, faster than they look."));
						await dialog.Msg(L("They don't hurt anyone - they just run off with things. Constantly. With alarming dedication."));
						break;

					case "leave":
						await dialog.Msg(L("File a claim? With who? Zeraha hasn't had a magistrate since before the war."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLoaders", out var killObj)) return;
				if (!quest.TryGetProgress("recoverCrates", out var crateObj)) return;

				if (killObj.Done && crateObj.Done)
				{
					await dialog.Msg(L("All four crates! And the Loaders properly spooked - they'll think twice before the next raid."));
					await dialog.Msg(L("Good work. Here's your share, straight from the crates you just saved."));

					character.Inventory.Remove(650097, character.Inventory.CountItem(650097), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Ferret Loaders from the camp. ");
					if (!crateObj.Done)
						status += L("Recover more stolen supply crates. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three solid days without a theft. New record."));
			}
		});

		// Stolen Crate Points
		//-------------------------------------------------------------------------
		void AddStolenCrate(int crateNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Stolen Supply Crate"), "f_orchard_34_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_2", 1003);
				var variableKey = $"Laima.Quests.f_orchard_34_2.Quest1003.Crate{crateNum}";
				var spawnedKey = $"Laima.Quests.f_orchard_34_2.Quest1003.Crate{crateNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A red-stamped supply crate, half-buried in scrub*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already recovered this crate*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 40)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Ferret_Loader, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF9966}Ferret Loaders scramble to defend their haul!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Recovering crate..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650097, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Stolen Supply Crate"));

					var currentCount = character.Inventory.CountItem(650097);
					character.ServerMessage(LF("Crates recovered: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All crates recovered! Return to Rudolf.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You set the crate down again."));
				}
			});
		}

		AddStolenCrate(1, -800, 1700, 0);
		AddStolenCrate(2, -500, 1500, 0);
		AddStolenCrate(3, -700, 1800, 0);
		AddStolenCrate(4, -400, 1600, 0);

		// Quest 4: The Chief's Apology
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Chief] Burrows"), "f_orchard_34_2", 720, 820, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1004);

			dialog.SetTitle(L("Burrows"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Greetings, tall-friend. I am Burrows, chief of the peaceable Ferret Folk of Zeraha."));
				await dialog.Msg(L("A splinter of my tribe - the Searchers - have forgotten their manners. They snatch, they bite, they shame our name."));
				await dialog.Msg(L("I cannot raise paws against my own. But if a stranger thins the rogues, and gathers peace-offerings along the safe paths, I will honour both with the tribe's trust."));

				var response = await dialog.Select(L("What are the peace-offerings?"),
					Option(L("I'll handle the rogues and gather the offerings"), "help"),
					Option(L("Why can't you discipline your own?"), "info"),
					Option(L("Tribe drama isn't my problem"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve rogue Ferret Searchers and gather five peace-offering pouches from the safe paths?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The offerings are woven pouches, left at marker-stones by the tribe. Each one contains our apology in miniature - a pebble, a feather, a sweet."));
							await dialog.Msg(L("Collect five, and the rogue Searchers will see their elders do not approve of them."));
						}
						break;

					case "info":
						await dialog.Msg(L("If I strike my own, I splinter the tribe further. If a stranger strikes them, I mourn privately and the tribe holds together."));
						await dialog.Msg(L("It is not the tall-folk way. But it is ours."));
						break;

					case "leave":
						await dialog.Msg(L("Then go your way, tall-friend. The offer will remain if you return."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSearchers", out var killObj)) return;
				var pouchCount = character.Inventory.CountItem(650096);

				if (killObj.Done && pouchCount >= 5)
				{
					await dialog.Msg(L("Five offerings, and the rogues brought low. The tribe will mourn, and then it will mend."));
					await dialog.Msg(L("You carry our trust now, tall-friend. Take this - a gift from my own stores."));

					character.Inventory.Remove(650096, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more rogue Searchers. ");
					if (pouchCount < 5)
						status += L("Gather more peace-offering pouches. ");

					await dialog.Msg(LF("Take your time. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The tribe speaks your name at the fireside now, tall-friend. Always welcome at our hearths."));
			}
		});

		// Peace Offering Points
		//-------------------------------------------------------------------------
		void AddOfferingStone(int offeringNum, int x, int z, int direction)
		{
			AddNpc(47190, L("Marker-Stone Offering"), "f_orchard_34_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_2", 1004);
				var variableKey = $"Laima.Quests.f_orchard_34_2.Quest1004.Offering{offeringNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A woven pouch rests atop a moss-covered marker-stone*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already collected this offering*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Lifting pouch..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650096, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Collected: Peace-Offering Pouch"));

					var currentCount = character.Inventory.CountItem(650096);
					character.ServerMessage(LF("Offerings collected: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All offerings collected! Return to Chief Burrows.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You left the pouch on the stone."));
				}
			});
		}

		AddOfferingStone(1, 500, 600, 0);
		AddOfferingStone(2, 800, 700, 0);
		AddOfferingStone(3, 300, 800, 0);
		AddOfferingStone(4, 600, 900, 0);
		AddOfferingStone(5, 900, 600, 0);

		// Quest 5: The Hoard-King
		//-------------------------------------------------------------------------
		AddNpc(20109, L("[Bounty Hunter] Dag"), "f_orchard_34_2", 1100, 0, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1005);
			var kingSpawnedKey = "Laima.Quests.f_orchard_34_2.Quest1005.KingSpawned";

			dialog.SetTitle(L("Dag"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("There's a ferret in these woods who styles himself 'Hoard-King'. Sits on a nest of stolen coins and chatters orders at the rest."));
				await dialog.Msg(L("He won't show his face while his retinue's intact. But thin the surrounding Ferret Folk enough - about ten of them - and his pride brings him out."));
				await dialog.Msg(L("Bounty's posted: the Hoard-King's coin-pile, plus my personal contribution for whoever brings his crown in."));

				var response = await dialog.Select(L("Sounds like a fun fight."),
					Option(L("I'll take the bounty"), "help"),
					Option(L("What crown?"), "info"),
					Option(L("Leave him to his coins"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Ferret Folk from the retinue, then bring down the Hoard-King when he emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten Folk first. Come back when the count's done - I'll know when he's close to emerging."));
							await dialog.Msg(L("Don't underestimate him. He didn't get the crown by sitting still."));
						}
						break;

					case "info":
						await dialog.Msg(L("A circle of woven twigs, set with pilfered buttons and a polished bottle cap."));
						await dialog.Msg(L("Ridiculous to look at. But every ferret in Zeraha salutes when he wears it."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe next week. The bounty keeps climbing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRetinue", out var retObj)) return;
				if (!quest.TryGetProgress("killKing", out var kingObj)) return;

				if (retObj.Done && kingObj.Done)
				{
					await dialog.Msg(L("Crown in hand? That's him. That's really him."));
					await dialog.Msg(L("Bounty paid in full, plus the contribution I promised. And the tribe will be quieter for it - Burrows will thank you too, I wager."));

					character.Variables.Perm.Remove(kingSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (retObj.Done && !kingObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(kingSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(kingSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Ferret_Loader, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("His retinue's thin enough. He's emerging now - I can hear the rattle of his coin-pile from here."));
							await dialog.Msg(L("{#FF9966}Go, quickly! He doesn't stay in the open long!{/}"));
							character.ServerMessage(L("{#FF9966}The Hoard-King emerges to defend his pile!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out there. Don't let him scurry back into his burrow."));
					}
				}
				else
				{
					await dialog.Msg(L("His retinue's still thick. Thin them first, then he'll come out."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The crown's nailed to my post. The bounty board finally has something worth bragging about."));
			}
		});

		// Quest 6: The Zeraha Road
		//-------------------------------------------------------------------------
		AddNpc(155145, L("[Pathfinder] Jenna"), "f_orchard_34_2", 900, -400, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1006);

			dialog.SetTitle(L("Jenna"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Zeraha Road runs southeast into Alemeth - shortest route to the festival for half the province."));
				await dialog.Msg(L("Right now it's a gauntlet. Ferret Patters block the carts, Slingers pelt the drivers. Caravans are turning back, and festival supplies aren't getting through."));

				var response = await dialog.Select(L("Want both species thinned?"),
					Option(L("I'll clear the road"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Use another road"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Ferret Patters and twelve Ferret Slingers along the Zeraha Road?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twelve of each. Patters work the ground-level, Slingers take the treetops - both are all along the road."));
							await dialog.Msg(L("Clear them, and the festival carts roll again."));
						}
						break;

					case "info":
						await dialog.Msg(L("Patters are worse underfoot. Slingers are worse overhead. Neither is strictly more irritating than the other."));
						await dialog.Msg(L("Together they're unbearable. Separately, merely annoying."));
						break;

					case "leave":
						await dialog.Msg(L("The other roads add a week to the trip. Not an option if the festival's to have food."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPatters", out var pattObj)) return;
				if (!quest.TryGetProgress("killSlingers", out var slingObj)) return;

				if (pattObj.Done && slingObj.Done)
				{
					await dialog.Msg(L("Road's clear! The first caravan rolled through an hour ago - the drivers are beaming."));
					await dialog.Msg(L("Take your pay, and know that every festival pie owes you a slice."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!pattObj.Done)
						status += L("Kill more Ferret Patters. ");
					if (!slingObj.Done)
						status += L("Kill more Ferret Slingers. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The caravans are running on schedule. The festival might actually have jam this year."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Pebble Barrage
//-----------------------------------------------------------------------------

public class PebbleBarrageQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1001);
		SetName(L("Pebble Barrage"));
		SetType(QuestType.Sub);
		SetDescription(L("Chase off the Ferret Slingers pelting the harvest crew with acorns."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Harvest Foreman] Tamas"), "f_orchard_34_2");

		AddObjective("chaseSlingers", L("Chase off Ferret Slingers"),
			new KillObjective(22, new[] { MonsterId.Ferret_Slinger }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 15)); // Normal HP Potion
		AddReward(new ItemReward(640007, 15)); // Normal SP Potion
	}
}

// Quest 1002 CLASS: The Acorn Bounty
//-----------------------------------------------------------------------------

public class TheAcornBountyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1002);
		SetName(L("The Acorn Bounty"));
		SetType(QuestType.Sub);
		SetDescription(L("Gather giant acorns from Zeraha's ancient oaks for the Alemeth festival bakers."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Collector] Wren"), "f_orchard_34_2");

		AddObjective("gatherAcorns", L("Gather giant acorns"),
			new CollectItemObjective(661094, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(640013, 13)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661094, character.Inventory.CountItem(661094), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1002.Acorn{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1002.Acorn{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661094, character.Inventory.CountItem(661094), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1002.Acorn{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1002.Acorn{i}.Spawned");
		}
	}
}

// Quest 1003 CLASS: The Stolen Crates
//-----------------------------------------------------------------------------

public class TheStolenCratesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1003);
		SetName(L("The Stolen Crates"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ferret Loaders raiding Rudolf's caravan and recover the stolen supply crates from their northwestern camp."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Rudolf"), "f_orchard_34_2");

		AddObjective("killLoaders", L("Kill Ferret Loaders"),
			new KillObjective(15, new[] { MonsterId.Ferret_Loader }));

		AddObjective("recoverCrates", L("Recover stolen supply crates"),
			new CollectItemObjective(650097, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 15)); // Normal HP Potion
		AddReward(new ItemReward(640007, 15)); // Normal SP Potion
		AddReward(new ItemReward(640013, 14)); // Recovery Potion
		AddReward(new ItemReward(926012, 1)); // Recipe - Shield Breaker
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650097, character.Inventory.CountItem(650097), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1003.Crate{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1003.Crate{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650097, character.Inventory.CountItem(650097), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1003.Crate{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1003.Crate{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Chief's Apology
//-----------------------------------------------------------------------------

public class TheChiefsApologyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1004);
		SetName(L("The Chief's Apology"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill the rogue Ferret Searchers and gather Chief Burrows' peace-offering pouches from the marker-stones."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Chief] Burrows"), "f_orchard_34_2");

		AddObjective("killSearchers", L("Kill rogue Ferret Searchers"),
			new KillObjective(12, new[] { MonsterId.Ferret_Searcher }));

		AddObjective("collectOfferings", L("Collect peace-offering pouches"),
			new CollectItemObjective(650096, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(640013, 13)); // Recovery Potion
		AddReward(new ItemReward(941035, 1)); // Recipe - Ferret Marauder Shield
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650096, character.Inventory.CountItem(650096), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1004.Offering{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650096, character.Inventory.CountItem(650096), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1004.Offering{i}");
		}
	}
}

// Quest 1005 CLASS: The Hoard-King
//-----------------------------------------------------------------------------

public class TheHoardKingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1005);
		SetName(L("The Hoard-King"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Hoard-King's retinue of Ferret Folk to draw him out, then bring him down for the bounty."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Dag"), "f_orchard_34_2");

		AddObjective("killRetinue", L("Kill the Hoard-King's retinue"),
			new KillObjective(10, new[] { MonsterId.Ferret_Folk }));

		AddObjective("killKing", L("Defeat the Hoard-King"),
			new KillObjective(1, new[] { MonsterId.Ferret_Loader }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 15)); // Normal HP Potion
		AddReward(new ItemReward(640007, 15)); // Normal SP Potion
		AddReward(new ItemReward(640013, 14)); // Recovery Potion
		AddReward(new ItemReward(531122, 1)); // Plate Armor
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_orchard_34_2.Quest1005.KingSpawned");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_orchard_34_2.Quest1005.KingSpawned");
	}
}

// Quest 1006 CLASS: The Zeraha Road
//-----------------------------------------------------------------------------

public class TheZerahaRoadQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1006);
		SetName(L("The Zeraha Road"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear the Ferret Patters and Slingers harassing festival caravans along the Zeraha Road."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pathfinder] Jenna"), "f_orchard_34_2");

		AddObjective("killPatters", L("Kill Ferret Patters from the road"),
			new KillObjective(12, new[] { MonsterId.Ferret_Patter }));

		AddObjective("killSlingers", L("Kill Ferret Slingers from the road"),
			new KillObjective(12, new[] { MonsterId.Ferret_Slinger }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(502207, 1)); // Hasta Plate Gauntlets
	}
}
