//--- Melia Script ----------------------------------------------------------
// Tenants' Farm - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_farm_47_1 map. Planned refugee shelter for
// the farm cluster's evacuation protocol.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm471QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Geppetto Attacks
		// =====================================================================
		// Farmer Vilius - Geppettos wrecking barn conversions
		//---------------------------------------------------------------------
		AddNpc(20138, L("[Farmer] Vilius"), "f_farm_47_1", 1527, -1336, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1001);

			dialog.SetTitle(L("Vilius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer holds a splintered pallet up to the light, shaking his head*{/}"));
				await dialog.Msg(L("Third pallet this week. Geppettos come at dusk, tangle in the ropes, chew the pallet corners. If a child was sleeping on one of these, we'd be burying her."));

				var response = await dialog.Select(L("Mykolas needs seven barns ready in five days. Geppettos wreck one in a night. Kill eighteen and we'll have a chance - they're Lightning-element, so they hate steel. Use it heavy."),
					Option(L("I'll kill the Geppettos"), "help"),
					Option(L("Why did they suddenly appear?"), "info"),
					Option(L("Find a proper hunter"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He sets the splintered pallet down carefully, as if it were a child's shoe*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Take these - leather pants. My brother wore them when he was farmhand-militia. He'd want them on someone defending a barn."));
						await dialog.Msg(L("And mind the puppet-strings on their arms. Cut those first - disarms the Lightning magic."));
						break;

					case "info":
						await dialog.Msg(L("Mykolas thinks the demon-pollen woke them up. Saule across the way says Lightning-creatures always drift toward disturbed wards."));
						await dialog.Msg(L("Either way. They come. We kill. We keep the barns ready."));
						break;

					case "leave":
						await dialog.Msg(L("The hunters are in Klaipeda collecting bounties. We're here with splinters."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGeppetto", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He looks at the intact pallets, letting out a slow breath*{/}"));
					await dialog.Msg(L("Eighteen. The barns will hold tonight. Maybe long enough to finish the last conversions."));
					await dialog.Msg(L("Take this - farmer's purse, plus the pants. You earned them both twice over."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Central barn, northern hedgerow. Cut the puppet-strings first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Seven barns ready by week's end. If the bell rings, the children have somewhere to run."));
			}
		});

		// =====================================================================
		// QUEST 1002: Supply Line to Cobalt
		// =====================================================================
		// Tenant-Farmer Mykolas - Refugee supply requisition
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Tenant-Farmer] Mykolas"), "f_farm_47_1", -524, 933, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1002);

			dialog.SetTitle(L("Mykolas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*Mykolas hunches over a supply ledger, crossing items off as he writes*{/}"));
				await dialog.Msg(L("Audrone's protocol is posted. Seven barns, thirty pallets - but pallets need blankets, and blankets I don't have. The Cobalt Forest merchants trade wool cheaper than Klaipeda's guilds."));

				var response = await dialog.Select(L("Izolde runs the Fedimian-side supply post through the Cobalt Forest warp. I've drafted the requisition - thirty blankets, fifty pallets, dried oats for three hundred. Carry it to her? She'll deliver by sundown if we pay up front."),
					Option(L("I'll carry the requisition"), "help"),
					Option(L("Why Fedimian merchants?"), "info"),
					Option(L("I'm busy"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He folds the requisition and seals it with a thumb-print in blackberry ink*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("If she haggles, remind her we've paid on time for six years running. Our coin is as good as Klaipeda's."));
						break;

					case "info":
						await dialog.Msg(L("Because Klaipeda's guilds triple the price when they smell crisis. Izolde doesn't. She sells at cost plus cart-fee, and she delivers when she says she will."));
						await dialog.Msg(L("Most farmers this side of the aqueduct buy Fedimian wool. Klaipeda doesn't like it, but Klaipeda doesn't run our barns either."));
						break;

					case "leave":
						await dialog.Msg(L("Then I'll walk it myself at dawn. Three hours each way, leaves me three hours less to count pallets."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverRequisition", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*He reads Izolde's confirmation and nods once*{/}"));
					await dialog.Msg(L("Cart arrives tomorrow at noon. Blankets, pallets, oats - enough to shelter every Myrkiti child for a week. That's what I needed to know."));
					await dialog.Msg(L("Here - a tenant's wage, paid honestly. You saved me three hours of walking and a day of worry."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Izolde is at the Cobalt Forest warp. North end of the farm. Don't lose the requisition - I've no copy."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The supplies came. Barns are ready. Audrone sent word - Myrkiti's children know which wagon is theirs."));
			}
		});

		// =====================================================================
		// Fedimian Merchant Izolde - Recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Fedimian Merchant] Izolde"), "f_farm_47_1", 235, 1471, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1002);

			dialog.SetTitle(L("Izolde"));

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A Fedimian merchant counts coin by the Cobalt warp-stone, her cart-driver waiting with a patient horse*{/}"));
				await dialog.Msg(L("Supplies, wool, lamp oil, dried oats. Cart runs at dawn and sundown. Need anything?"));
				return;
			}

			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_1.Quest1002.Delivered", 0) >= 1;

			if (delivered)
			{
				await dialog.Msg(L("Tell Mykolas the cart arrives tomorrow at noon. Blankets, pallets, oats - full requisition."));
				return;
			}

			await dialog.Msg(L("{#666666}*She reads the requisition, tallying with a pencil*{/}"));
			await dialog.Msg(L("Thirty blankets - I have forty in stock. Fifty pallets - I'll split two carts. Oats for three hundred - doable, but I'll need to pull from the Fedimian granary reserves."));
			await dialog.Msg(L("{#666666}*She signs a confirmation slip and hands it to you*{/}"));
			await dialog.Msg(L("Tell Mykolas cart arrives tomorrow at noon. Tell him I'm adding lamp-oil for the hay-barn stove at no extra charge. Children sleeping on straw in autumn need warmth more than cheaper books."));

			character.Variables.Perm.Set("Laima.Quests.f_farm_47_1.Quest1002.Delivered", 1);
			character.ServerMessage(L("{#FFD700}Requisition delivered. Return to Mykolas.{/}"));
		});

		// =====================================================================
		// QUEST 1003: Pino Resin
		// =====================================================================
		// Barn-Mother Agne - Sealing barn doorframes against pollen
		//---------------------------------------------------------------------
		AddNpc(157100, L("[Barn-Mother] Agne"), "f_farm_47_1", -96, -340, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1003);

			dialog.SetTitle(L("Agne"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer presses amber resin into the cracks of a barn doorframe with a bone spatula*{/}"));
				await dialog.Msg(L("Pine resin dries to amber in a day. Amber blocks pollen like stone blocks arrows. Every barn in my care gets a double-seal around the door and the window frames."));

				var response = await dialog.Select(L("I've sealed three barns. Four still to do, and I'm out of resin. The Pino-pools drip on their own if you find the right trees - scratched bark, low branches, faint amber smell. Can you gather five clumps?"),
					Option(L("I'll gather five clumps"), "help"),
					Option(L("How did you learn this?"), "info"),
					Option(L("That's not farming"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She hands you a bone spatula and a wax-lined pouch*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Don't touch the resin with bare fingers - it sticks for three days and the Pinos smell it on you."));
						await dialog.Msg(L("If a Pino startles and swarms you, back off. They calm down if you stand still for a count of ten."));
						break;

					case "info":
						await dialog.Msg(L("My grandmother. She sealed barns during the demon war. Said 'amber outlasts armies, child.' She was right twice."));
						await dialog.Msg(L("I don't know why the recipe works. I only know it does. Every sealed frame is one less way pollen gets to a child's lungs."));
						break;

					case "leave":
						await dialog.Msg(L("Fine. Then don't complain when you breathe a lungful of pollen in three months. Doors don't seal themselves."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var resinCount = character.Inventory.CountItem(650609);

				if (resinCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She tests each clump with the bone spatula; clean, soft, amber-warm*{/}"));
					await dialog.Msg(L("Five clumps, all pollen-clean. Enough for two more barn-frames. I'll scrape what's left into the west doorway tonight."));
					await dialog.Msg(L("Take these. A farmer's wage from the barn-mother's purse. The barns sleep safer because of you."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Five clumps. Scratched bark, low branches, amber smell."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Every barn on Tenants' has amber-sealed frames now. The pollen still drifts, but it doesn't cross the threshold."));
			}
		});

		// =====================================================================
		// PINO SAP POOLS
		// =====================================================================
		// For Quest 1003 - Pino Resin
		// =====================================================================

		void AddPinoSapPool(int poolNumber, int x, int z, int direction)
		{
			AddNpc(152012, L("Pino Sap-Pool"), "f_farm_47_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_1", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*Amber resin drips from scratched bark into a small pool at the tree's root*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_1.Quest1003.Pool{poolNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The pool is scraped clean. A fresh drip is already forming at the bark*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_farm_47_1.Quest1003.Pool{poolNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Pino_White, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A White Pino darts out, hissing at the disturbed sap!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Scraping resin..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650609, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650609);
					character.ServerMessage(LF("Resin clumps gathered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All clumps gathered! Return to Barn-Mother Agne.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Scraping interrupted."));
				}
			});
		}

		AddPinoSapPool(1, -1279, 390, 0);
		AddPinoSapPool(2, -243, -1261, 90);
		AddPinoSapPool(3, -1070, 159, 180);
		AddPinoSapPool(4, 120, -504, 270);
		AddPinoSapPool(5, 548, -1245, 0);

		// =====================================================================
		// QUEST 1004: Barn Readiness
		// =====================================================================
		// Evacuation-Warden Rytis - Inspecting converted barns
		//---------------------------------------------------------------------
		AddNpc(20125, L("[Evacuation-Warden] Rytis"), "f_farm_47_1", 100, 1400, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1004);

			dialog.SetTitle(L("Rytis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer in a heavy coat checks a wooden tally-board, ticking items off with a nub of chalk*{/}"));
				await dialog.Msg(L("Mykolas assigned me evacuation-warden. I ring the bell, I count the barns, I send the families. Four barns are converted. Four still need inspection before the bell can ring in earnest."));

				var response = await dialog.Select(L("Each barn needs stove, pallets, door-seal, bell-rope. Check each one, mark it ready or not-ready. If any barn fails inspection, families get diverted. Can you walk all four?"),
					Option(L("I'll inspect the barns"), "help"),
					Option(L("What happens if the bell rings tonight?"), "info"),
					Option(L("I've got my own chores"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He hands you the tally-board and a spare nub of chalk*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("The barns are spread - north, south, east, west of the central yard."));
						await dialog.Msg(L("If a barn isn't ready, leave the tally un-ticked. I'll send Agne to fix it."));
						break;

					case "info":
						await dialog.Msg(L("If the bell rings tonight, I send Myrkiti's families through the Aqueduct warp. They bed down in whatever barns are finished. If none are finished, they sleep in the yard under a wagon tarp."));
						await dialog.Msg(L("That's why the inspection matters. 'Bed under a tarp' means pneumonia by week's end."));
						break;

					case "leave":
						await dialog.Msg(L("Then chores first, children second. I'll note who said what when the bell rings."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var barnsVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_1.Quest1004.BarnsVisited", 0);

				if (barnsVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*He reads the tally, ticks each box, and nods once*{/}"));
					await dialog.Msg(L("All four ready. Stoves lit, pallets stacked, frames sealed, ropes hung. If the bell rings tonight, every Myrkiti family has a roof."));
					await dialog.Msg(L("Take this. Farmer's purse, warden's seal. You can come back anytime - Tenants' remembers who prepared its barns."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("North, south, east, west. Stove, pallets, door-seal, bell-rope."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The barns are ready. The bell hangs. We wait. And farm. And pray."));
			}
		});

		// =====================================================================
		// CONVERTED BARNS
		// =====================================================================
		// For Quest 1004 - Barn Readiness
		// =====================================================================

		void AddBarnInspection(int barnNumber, string barnName, string observation, int x, int z, int direction)
		{
			AddNpc(160060, L(barnName), "f_farm_47_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A converted hay-barn, pallets stacked inside the doorway*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_1.Quest1004.Barn{barnNumber}";
				var inspected = character.Variables.Perm.GetBool(variableKey, false);

				if (inspected)
				{
					await dialog.Msg(L("{#666666}*This barn is already inspected and ticked on the tally*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Inspecting barn..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var barnsVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_1.Quest1004.BarnsVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_farm_47_1.Quest1004.BarnsVisited", barnsVisited + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Barns inspected: {0}/4", barnsVisited + 1));

					if (barnsVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}Inspection complete! Return to Warden Rytis.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Inspection interrupted."));
				}
			});
		}

		AddBarnInspection(1, "North Barn", "Stove lit, pallets stacked, frames amber-sealed. Bell-rope hung and tested.", -1250, 400, 0);
		AddBarnInspection(2, "South Barn", "Stove cold but laid; pallets stacked; frames sealed; bell-rope hung.", -220, -1240, 90);
		AddBarnInspection(3, "East Barn", "The hay-barn with the coal stove. Warmest of the four. Ready for infants.", 200, -320, 180);
		AddBarnInspection(4, "West Barn", "Smallest but cleanest. Reserved for the elders and the sick.", -1040, 180, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Geppetto Attacks
//-----------------------------------------------------------------------------

public class GeppettoAttacksQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_1", 1001);
		SetName("Geppetto Attacks");
		SetType(QuestType.Sub);
		SetDescription("Farmer Vilius needs the White Geppettos killed before they wreck the barns being converted for Myrkiti's evacuation shelter.");
		SetLocation("f_farm_47_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Vilius", "f_farm_47_1");

		AddObjective("killGeppetto", "Defeat White Geppettos",
			new KillObjective(18, new[] { MonsterId.Geppetto_White }));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1));  // Stamina Potion
		AddReward(new ItemReward(522112, 1));  // Pokubon Leather Pants
	}
}

// Quest 1002 CLASS: Supply Line to Cobalt
//-----------------------------------------------------------------------------

public class SupplyLineToCobaltQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_1", 1002);
		SetName("Supply Line to Cobalt");
		SetType(QuestType.Sub);
		SetDescription("Mykolas needs a supply requisition carried to Fedimian Merchant Izolde at the Cobalt Forest warp so the refugee barns are stocked before the evacuation bell rings.");
		SetLocation("f_farm_47_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Tenant-Farmer] Mykolas", "f_farm_47_1");

		AddObjective("deliverRequisition", "Deliver the requisition to Izolde",
			new VariableCheckObjective("Laima.Quests.f_farm_47_1.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_1.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_1.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Pino Resin
//-----------------------------------------------------------------------------

public class PinoResinQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_1", 1003);
		SetName("Pino Resin");
		SetType(QuestType.Sub);
		SetDescription("Barn-Mother Agne needs five Varb Resin clumps from Pino sap-pools to seal the barn doorframes against demon-pollen.");
		SetLocation("f_farm_47_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Barn-Mother] Agne", "f_farm_47_1");

		AddObjective("collectResin", "Scrape resin from Pino sap-pools",
			new CollectItemObjective(650609, 5));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1));  // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650609,
			character.Inventory.CountItem(650609),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_1.Quest1003.Pool{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_1.Quest1003.Pool{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650609,
			character.Inventory.CountItem(650609),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_1.Quest1003.Pool{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_1.Quest1003.Pool{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Barn Readiness
//-----------------------------------------------------------------------------

public class BarnReadinessQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_1", 1004);
		SetName("Barn Readiness");
		SetType(QuestType.Sub);
		SetDescription("Evacuation-Warden Rytis has asked you to inspect the four converted barns on Tenants' Farm before the evacuation bell can be rung.");
		SetLocation("f_farm_47_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Evacuation-Warden] Rytis", "f_farm_47_1");

		AddObjective("inspectBarns", "Inspect the converted barns",
			new VariableCheckObjective("Laima.Quests.f_farm_47_1.Quest1004.BarnsVisited", 4, true));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(3800));
		AddReward(new ItemReward(640082, 2));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_1.Quest1004.BarnsVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_1.Quest1004.Barn{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_1.Quest1004.BarnsVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_1.Quest1004.Barn{i}");
		}
	}
}
