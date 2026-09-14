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
		AddNpc(20138, L("[Farmer] Vilius"), "f_farm_47_1", 1123, -990, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1001);

			dialog.SetTitle(L("Vilius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer is checking a splintered pallet*{/}"));
				await dialog.Msg(L("Third pallet this week. The Geppettos come around at dusk and chew the corners up. If a kid had been sleeping on one of these... yeah. Doesn't bear thinking about."));

				var response = await dialog.Select(L("Mykolas needs seven barns ready in five days, and the Geppettos can wreck one in a night. If you can kill eighteen of them, we'll have a chance. They're Lightning-element, so steel hurts them — use it heavy."),
					Option(L("I'll kill the Geppettos"), "help"),
					Option(L("Why did they suddenly appear?"), "info"),
					Option(L("Find a proper hunter"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He sets the pallet down*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Here, take these — leather pants. My brother wore them in the farmhand militia. Better on you than in a chest."));
						await dialog.Msg(L("Oh, and watch out for the puppet-strings on their arms. Cut those first, that takes the lightning out of them."));
						break;

					case "info":
						await dialog.Msg(L("Mykolas reckons the demon-pollen woke them up. Lina says lightning-things always drift toward broken wards."));
						await dialog.Msg(L("Either way, they keep coming and we keep killing them. That's the job right now."));
						break;

					case "leave":
						await dialog.Msg(L("The real hunters are off in Klaipeda chasing bounties. Meanwhile we're out here pulling splinters out of pallets."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGeppetto", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He glances over the intact pallets*{/}"));
					await dialog.Msg(L("Eighteen. Alright. The barns'll hold tonight, and maybe long enough for us to finish the last few."));
					await dialog.Msg(L("Take this — farmer's purse, plus the pants. You earned them both twice over."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Try the central barn and the northern hedgerow. And remember — cut the puppet-strings first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Seven barns ready by week's end. If the bell rings, at least the kids have somewhere to run."));
			}
		});

		// =====================================================================
		// QUEST 1002: Supply Line to Cobalt
		// =====================================================================
		// Tenant-Farmer Mykolas - Refugee supply requisition
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Tenant-Farmer] Mykolas"), "f_farm_47_1", -619, 989, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1002);
			var protocolQuestId = new QuestId("f_farm_47_2", 1002);

			dialog.SetTitle(L("Mykolas"));

			var protocolDelivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_2.Quest1002.Delivered", 0) >= 1;

			if (character.Quests.IsActive(protocolQuestId) && !protocolDelivered)
			{
				if (character.Inventory.CountItem(650580) <= 0)
				{
					await dialog.Msg(L("{#666666}*He glances at your empty hands*{/}"));
					await dialog.Msg(L("Audrone said she'd send a pin-marked map. Come back when you have it — I'm not laying out barns from memory."));
					return;
				}

				await dialog.Msg(L("{#666666}*He unfolds the map and reads the pin assignments*{/}"));
				await dialog.Msg(L("Red pins first. So Myrkiti's kids come here first. Alright."));
				await dialog.Msg(L("{#666666}*He folds the map and tucks it into his coat*{/}"));
				await dialog.Msg(L("Tell Audrone I've actually got seven barns at thirty pallets each — that's more than she figured. The big hay-barn has a coal stove, so I'll put the babies in there."));
				await dialog.Msg(L("And tell her five days, not seven. No point cutting it close when the portal keeps widening."));

				character.Inventory.Remove(650580, 1, InventoryItemRemoveMsg.Given);
				character.Variables.Perm.Set("Laima.Quests.f_farm_47_2.Quest1002.Delivered", 1);
				character.ServerMessage(L("{#FFD700}Protocol delivered. Return to Audrone.{/}"));
				return;
			}

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*Mykolas is hunched over a supply ledger, crossing items off*{/}"));
				await dialog.Msg(L("Audrone's protocol is posted. Seven barns, thirty pallets — but pallets need blankets, and I haven't got any. The Cobalt Forest merchants sell wool cheaper than the Klaipeda guilds."));

				var response = await dialog.Select(L("Izolde runs the Fedimian supply post through the Cobalt Forest warp. I've drafted a requisition — thirty blankets, fifty pallets, dried oats for three hundred. Mind carrying it over? She'll deliver by sundown if we pay up front."),
					Option(L("I'll carry the requisition"), "help"),
					Option(L("Why Fedimian merchants?"), "info"),
					Option(L("I'm busy"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He folds the requisition and seals it with a thumb-print*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("If she haggles, remind her we've paid on time for six years running. Our coin's as good as Klaipeda's."));
						break;

					case "info":
						await dialog.Msg(L("Because Klaipeda's guilds triple the price the moment they smell a crisis. Izolde doesn't. She charges cost plus the cart-fee, and she delivers when she says she will."));
						await dialog.Msg(L("Most farmers around here buy Fedimian wool. Klaipeda doesn't like that, but Klaipeda's not running our barns either."));
						break;

					case "leave":
						await dialog.Msg(L("Then I'll walk it myself at dawn. Three hours each way — three hours I'd rather spend counting pallets."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverRequisition", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*He reads Izolde's confirmation and nods*{/}"));
					await dialog.Msg(L("Cart arrives tomorrow at noon. Blankets, pallets, oats — enough to shelter every Myrkiti kid for a week. Good. That's what I needed to know."));
					await dialog.Msg(L("Here, take this. A tenant's wage, paid honest. You saved me three hours of walking and a whole day of worrying."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Izolde's up at the Cobalt Forest warp, north end of the farm. Don't lose the requisition — I haven't got a copy."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Supplies came. Barns are ready. Audrone sent word — the Myrkiti kids know which wagon is theirs."));
			}
		});

		// =====================================================================
		// Fedimian Merchant Izolde - Recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Fedimian Merchant] Izolde"), "f_farm_47_1", -1155, -81, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1002);

			dialog.SetTitle(L("Izolde"));

			var questActive = character.Quests.IsActive(questId);
			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_1.Quest1002.Delivered", 0) >= 1;

			if (questActive && !delivered)
			{
				await dialog.Msg(L("{#666666}*She reads the requisition, tallying with a pencil*{/}"));
				await dialog.Msg(L("Thirty blankets — I've got forty in stock. Fifty pallets — I'll split that across two carts. Oats for three hundred is doable, but I'll have to pull from the Fedimian granary."));
				await dialog.Msg(L("{#666666}*She signs a confirmation slip and hands it to you*{/}"));
				await dialog.Msg(L("Tell Mykolas the cart's there tomorrow at noon. And tell him I'm throwing in lamp-oil for the hay-barn stove at no extra charge. Kids sleeping on straw in autumn need it more than I do."));

				character.Variables.Perm.Set("Laima.Quests.f_farm_47_1.Quest1002.Delivered", 1);
				character.ServerMessage(L("{#FFD700}Requisition delivered. Return to Mykolas.{/}"));
				return;
			}

			await dialog.Msg(L("{#666666}*A Fedimian merchant is counting coin by the Cobalt warp-stone, a wicker basket of farm goods beside her*{/}"));
			await dialog.Msg(L("Fresh off the Fedimian cart — berries, bread, mushrooms, milk. The kind Klaipeda charges triple for. What'll it be?"));

			await dialog.OpenShop("IzoldeFruits");
		});

		// =====================================================================
		// Tailor Agne - Armband recipes and crafting lore
		//---------------------------------------------------------------------
		AddNpc(152004, L("[Tailor] Agne"), "f_farm_47_1", 8, -221, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Agne"));

			await dialog.Msg(L("{#666666}*A seamstress is working a length of dyed cord through a half-finished armband*{/}"));

			var response = await dialog.Select(L("What can I do for you, traveler?"),
				Option(L("What do you do here?"), "info"),
				Option(L("Show me your recipes"), "shop"),
				Option(L("Can you lend me a compass?"), "compass"),
				Option(L("Just passing through"), "leave")
			);

			switch (response)
			{
				case "info":
					await dialog.Msg(L("I'm a tailor — folks around here just call me the sewing lady. Mostly I stitch armbands."));
					await dialog.Msg(L("Cord, a leather strap, a charm or two. Looks simple, but each one has its own knot-pattern, and that pattern means something. Other adventurers can read it like a badge."));
					await dialog.Msg(L("There are old armbands buried all up and down the farm road — Tenants', the Aqueduct, Myrkiti, Shaton. If you go hunting for them, ask me for a compass. The needle points at the nearest burrow."));
					await dialog.Msg(L("Bring me whatever you dig up and I'll tell you which pattern it carries."));
					break;

				case "shop":
					await dialog.Msg(L("Recipes are eighty thousand silver. I sell them at cost — the value's in actually making them, not in the paper."));
					await dialog.OpenShop("AgneArmbandRecipes");
					break;

				case "compass":
					if (character.Inventory.CountItem(11200079) > 0)
					{
						await dialog.Msg(L("You've already got one. Open your palm and the needle should point toward the nearest burrow - if there's nothing buried in range, it just spins."));
						break;
					}
					await dialog.Msg(L("{#666666}*She fishes a small brass compass from her sewing kit*{/}"));
					await dialog.Msg(L("Old tailor's trick. Needle's tuned to the burrows — hold it flat and it'll point. Not subtle, but it works."));
					character.Inventory.Add(11200079, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Take it. Lose it and come back — I've got a few spare."));
					break;

				case "leave":
					await dialog.Msg(L("Safe travels."));
					break;
			}
		});

		// =====================================================================
		// QUEST 1004: Round Up the Carts
		// =====================================================================
		// Soldier Rytis - Posted near the farm, helping recover supply carts
		//---------------------------------------------------------------------
		AddNpc(20125, L("[Soldier] Rytis"), "f_farm_47_1", 407, 517, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_47_1", 1004);

			dialog.SetTitle(L("Rytis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A soldier in field colors leans on a halberd, holding a wooden tally-board*{/}"));
				await dialog.Msg(L("Garrison posted me to Tenants' for the season — patrol the road, help where I can. Right now Mykolas is short bodies, and Izolde's cart-train rolled in at sunup with nobody to sort it out."));

				var response = await dialog.Select(L("The field hands dropped the supply carts where they stood and went straight back to the rows. Now they're scattered all over the farm. Rain's an hour off. I need four of them marked on this tally so my squad can fetch them. I'd do it myself, but I can't leave the road. Can you find four?"),
					Option(L("I'll round up the carts"), "help"),
					Option(L("Why is a soldier doing farmer's work?"), "info"),
					Option(L("Not my problem"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He hands you the tally-board and a stub of chalk*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("They're all over the place — north fields, south hedge, by the well, out past the orchard. Look for the canvas tarps."));
						await dialog.Msg(L("Tick four of them off and that's enough for me to send the squad out."));
						break;

					case "info":
						await dialog.Msg(L("Bread comes from this farm. Garrison eats this farm's grain. The captain says a soldier who won't lift a sack when the harvest's at risk isn't worth his pay, and he's right."));
						await dialog.Msg(L("Besides, Mykolas paid Fedimian rates for those oats. Letting them rot would be a worse waste than any fight I've been in."));
						break;

					case "leave":
						await dialog.Msg(L("Suit yourself. The road still needs walking either way."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var cartsFound = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_1.Quest1004.CartsFound", 0);

				if (cartsFound >= 4)
				{
					await dialog.Msg(L("{#666666}*He reads the tally and signals two soldiers down the road with a sharp whistle*{/}"));
					await dialog.Msg(L("Four logged. Squad's moving on them now. Oats stay dry, Mykolas keeps his harvest, garrison keeps its bread. That's a good morning."));
					await dialog.Msg(L("Here. Soldier's purse, plus what I scrounged from the supply tent. You earned both."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Look for the canvas tarps. Hedges, ditches, behind the well — they're out there."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Carts are in, harvest's safe. Road still needs walking, though."));
			}
		});

		// =====================================================================
		// SCATTERED SUPPLY CARTS
		// =====================================================================
		// For Quest 1004 - Round Up the Carts
		// =====================================================================

		void AddCartFinder(int cartNumber, string observation, int x, int z, int direction)
		{
			AddNpc(45316, L("Supply Cart"), "f_farm_47_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_47_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A canvas-covered supply cart, parked off the path*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_47_1.Quest1004.Cart{cartNumber}";
				var marked = character.Variables.Perm.GetBool(variableKey, false);

				if (marked)
				{
					await dialog.Msg(L("{#666666}*This cart is already ticked on the tally*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Marking cart..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(2));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var cartsFound = character.Variables.Perm.GetInt("Laima.Quests.f_farm_47_1.Quest1004.CartsFound", 0);
					character.Variables.Perm.Set("Laima.Quests.f_farm_47_1.Quest1004.CartsFound", cartsFound + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Carts marked: {0}/4", cartsFound + 1));

					if (cartsFound + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}Enough carts marked! Return to Soldier Rytis.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Marking interrupted."));
				}
			});
		}

		AddCartFinder(1, "Sacks of oats under the tarp. Still dry.", 188, 341, 90);
		AddCartFinder(2, "Bundled wool blankets. These would be heavy soaked.", -602, 773, 90);
		AddCartFinder(3, "Pallets stacked four-deep. Rain would warp these.", 157, 1124, 0);
		AddCartFinder(4, "Lamp-oil jugs packed in straw. Shouldn't be left in the open.", -934, 323, 0);
		AddCartFinder(5, "Dried beans in burlap. Mice would get to these by morning.", -779, -1148, 90);
		AddCartFinder(6, "More oats. Mykolas wasn't kidding about the order size.", 492, -309, 0);
		AddCartFinder(7, "Salt and dried fish. Far from the kitchen, but it'll keep.", 963, 383, 0);
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
		AddReward(new ItemReward(928003, 1));  // Recipe - Trident
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

// Quest 1004 CLASS: Round Up the Carts
//-----------------------------------------------------------------------------

public class BarnReadinessQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_47_1", 1004);
		SetName("Round Up the Carts");
		SetType(QuestType.Sub);
		SetDescription("Farmer Rytis needs four of the supply carts scattered around Tenants' Farm marked on his tally before the rain spoils the oats and pallets sitting in the open.");
		SetLocation("f_farm_47_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Soldier] Rytis", "f_farm_47_1");

		AddObjective("findCarts", "Find and mark scattered supply carts",
			new VariableCheckObjective("Laima.Quests.f_farm_47_1.Quest1004.CartsFound", 4, true));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(3800));
		AddReward(new ItemReward(640082, 2));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_1.Quest1004.CartsFound");
		for (int i = 1; i <= 7; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_1.Quest1004.Cart{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_47_1.Quest1004.CartsFound");
		for (int i = 1; i <= 7; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_47_1.Quest1004.Cart{i}");
		}
	}
}
