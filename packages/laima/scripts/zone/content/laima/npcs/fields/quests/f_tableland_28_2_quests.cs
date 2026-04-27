//--- Melia Script ----------------------------------------------------------
// Stogas Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Stogas Plateau.
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

public class FTableland282QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Blue Siaulav Flocks
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Sky-Ward] Algirdas"), "f_tableland_28_2", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1001);

			dialog.SetTitle(L("Algirdas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Siaulav flocks choke the Stogas sky. They dive on travelers and shepherds alike."));
				await dialog.Msg(L("Kill twenty-five and the sky clears."));

				var response = await dialog.Select(L("Clear the sky?"),
					Option(L("I'll kill"), "help"),
					Option(L("Why dive?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-five."));
						break;

					case "info":
						await dialog.Msg(L("Territorial and hungry."));
						break;

					case "leave":
						await dialog.Msg(L("Sheep can't fly, can they?"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSiaulavs", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Sky's clear."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Shepherds grateful."));
			}
		});

		// Quest 2: Siaulav Mage Feathers
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Fletcher] Grazina"), "f_tableland_28_2", -800, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1002);

			dialog.SetTitle(L("Grazina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Siaulav Mages drop mana-laced feathers. Six of them fletch a magic arrow."));

				var response = await dialog.Select(L("Six feathers?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Magic arrows?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Only the tail-shafts have mana. Pluck cleanly."));
						break;

					case "info":
						await dialog.Msg(L("Orsha archers pay triple."));
						break;

					case "leave":
						await dialog.Msg(L("Fine. Next fletcher then."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFeathers", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Six mana feathers. Magic flight guaranteed."));
					character.Inventory.Remove(650069, character.Inventory.CountItem(650069), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Orsha order paid out double."));
			}
		});

		// Quest 3: Siaulav Bow Sinew
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Bowyer] Ramunas"), "f_tableland_28_2", 400, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1003);

			dialog.SetTitle(L("Ramunas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Siaulav Bows have sinew stronger than any beast on the plateau. Seven sinews make a masterwork bow."));

				var response = await dialog.Select(L("Sinews?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Why so strong?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Seven. Don't shred them."));
						break;

					case "info":
						await dialog.Msg(L("Airborne tension. They draw against wind all their lives."));
						break;

					case "leave":
						await dialog.Msg(L("Then the bow stays unmade."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBows", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSinews", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Seven sinews. Bow gets strung this week."));
					character.Inventory.Remove(650070, character.Inventory.CountItem(650070), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Bow's with a Fedimian ranger. Draws twenty paces further."));
			}
		});

		// Quest 4: Lapasape Swarm
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Shepherd] Ausra"), "f_tableland_28_2", -500, -1400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1004);

			dialog.SetTitle(L("Ausra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A shepherd in a long oilcloth coat counts heads in a small flock as the sheep file past her staff*{/}"));
				await dialog.Msg(L("Three generations my family has run this pasture. Stogas grass is the best on the tableland - sheep grow fat on it, wool grows long, the wool-merchants pay above-market every spring."));
				await dialog.Msg(L("Then the Blue Lapasapes came down out of the salt-flats. They eat grass in rolls, the way you'd unroll a carpet - one minute the pasture is green, the next minute it's bare earth and the sheep are wandering in confused circles."));

				var response = await dialog.Select(L("Kill 35 Blue Lapasapes to push them off the pasture, then reset the four sheep-bell posts they've knocked flat. The bells help me find strays at dusk. Will you help?"),
					Option(L("I'll thin the Lapasapes and reset the posts"), "help"),
					Option(L("Why not just move the pasture?"), "info"),
					Option(L("That's not work for a swordhand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She unhooks a length of leather strap from her belt and hands it across*{/}"));
						await dialog.Msg(L("Thirty-five Lapasapes, no shortcuts. They flock aggressive when threatened - five at a time, all sides. Don't let them get behind you; the rear-flank dive is what gets shepherds killed."));
						await dialog.Msg(L("Bell-posts are wooden stakes set in the four pasture-corners. Tip them upright, lash the bell to the head with the strap I gave you. The bells should ring loose - if they ring tight, the flock can't hear them."));
						break;

					case "info":
						await dialog.Msg(L("Move the pasture? My grandfather sank a foundation here. The shepherd's hut, the lambing-shed, the wool-press - all of it costs more to rebuild than three years of mutton-money. The land is part of the family."));
						await dialog.Msg(L("And the Stogas grass doesn't grow elsewhere. There is no second pasture. There is only this one, defended."));
						break;

					case "leave":
						await dialog.Msg(L("Then the flock starves slow and the family pasture goes back to whatever the Lapasapes leave behind. I will count my sheep tonight as I always have. There may be fewer of them tomorrow."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLapasapes", out var killObj)) return;
				if (!quest.TryGetProgress("resetPosts", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("{#666666}*She pauses to listen as the dusk-wind catches each of the four bells in turn, then nods, satisfied*{/}"));
					await dialog.Msg(L("Hear that? Four bells, four corners, the flock answering each one in turn. Pasture's resting, the strays are finding their way home before dark. Sheep'll be fat again by month-end."));
					await dialog.Msg(L("Take this. Shepherd's purse, plus a wool-skein from last spring's clipping - softest one I had. Tell the wool-merchant I sent it; he'll pay you double for it on principle."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Thirty-five Lapasapes first. Bell-posts won't help if the flock keeps getting trampled at the corners by a fresh roll-line."));
				}
				else
				{
					await dialog.Msg(L("Lapasapes are pushed back. Now the four bell-posts - upright, leather-strap, bell loose enough to ring. The dusk-wind's coming."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Flock's fat, wool prices up, four bells ring at dusk like a small chord. The wool-merchant came by last spring and asked who I owed - I told him. He said you bought him three years of premium clip with one afternoon's work. He'd like to meet you. So would I, again."));
			}
		});

		// Sheep-bell post reset for Quest 1004
		//-------------------------------------------------------------------------
		void AddBellPost(int postNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Sheep-Bell Post"), "f_tableland_28_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_28_2", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A toppled wooden post, sheep-bell still tied to its head*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_28_2.Quest1004.Post{postNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already upright; the bell sways in the wind*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Resetting post..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_28_2.Quest1004.PostsReset", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_28_2.Quest1004.PostsReset", count);
					character.ServerMessage(LF("Bell-posts reset: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All bell-posts reset! Return to Shepherd Ausra.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reset interrupted."));
				}
			});
		}

		AddBellPost(1, -400, -1300, 0);
		AddBellPost(2, -700, -1500, 90);
		AddBellPost(3, -300, -1600, 180);
		AddBellPost(4, -600, -1200, 270);

		// Quest 5: Siaulav King
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Falconer] Ignas"), "f_tableland_28_2", 1200, 1200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1005);
			var kingSpawnedKey = "Laima.Quests.f_tableland_28_2.Quest1005.KingSpawned";

			dialog.SetTitle(L("Ignas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A falconer slips a hood off a small kestrel, the bird shaking out its wings before resettling on her glove*{/}"));
				await dialog.Msg(L("Siaulav King rules the upper winds above the tableland. Wingspan twenty feet, claws the size of kitchen knives, soars without a wingbeat for hours on the cliff-edge thermals."));
				await dialog.Msg(L("My falcons can't share the sky with him. Three thermal-banners I've cut sit at the cliff edges - drive them in and the canvas breaks the updraft he rides. Then we thin his flock and he dives to challenge whatever's grounded him."));

				var response = await dialog.Select(L("Plant the three thermal-banners at the cliff edges to break his updrafts, then kill 10 Blue Siaulavs to draw him into a dive. He fights when grounded. Will you take him?"),
					Option(L("I'll provoke him down"), "help"),
					Option(L("How big?"), "info"),
					Option(L("That's too high a fight"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She kisses the kestrel on the brow, hoods it again, then turns full attention to you*{/}"));
						await dialog.Msg(L("Three banners, three cliff-edges. Drive the banner-pole deep, lash the canvas open. The wind catches the canvas and the thermal collapses inside an hour."));
						await dialog.Msg(L("Then ten birds. He dives around the eighth - it'll feel like a portcullis falling. Step sideways at the last second; his momentum carries him past you. Strike his throat as he banks."));
						break;

					case "info":
						await dialog.Msg(L("Wingspan twenty feet, claws the size of kitchen knives, beak that can shear through bronze. He's killed three of my falcons in the past six months - all caught in his thermal-shadow before they could climb."));
						await dialog.Msg(L("He doesn't hunt for hunger; he hunts for territory. The upper winds are his and he asserts the claim every dawn. We can't share the sky with that. Either he goes or my falcons stay grounded forever."));
						break;

					case "leave":
						await dialog.Msg(L("His flock's splitting already - some have moved south. He hasn't followed; he won't leave the cliff-thermals. The kestrel and I will keep ground-hunting until you change your mind. We'll eat thinner for it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("plantBanners", out var bObj)) return;
				if (!quest.TryGetProgress("killFlock", out var fObj)) return;
				if (!quest.TryGetProgress("killKing", out var kObj)) return;

				if (bObj.Done && fObj.Done && kObj.Done)
				{
					await dialog.Msg(L("{#666666}*She unhoods the kestrel, lifts the glove, and the bird launches into a long climbing arc into the sudden empty sky*{/}"));
					await dialog.Msg(L("Banners standing, King down, kestrel climbing. Watch her - first time in six months she's been able to take that thermal without checking for a shadow above her."));
					await dialog.Msg(L("Falconer's purse, plus a King-claw. Mounted, it makes a fine cloak-clasp. Wear it where another falconer can see; it tells the trade you're someone who walks among us now."));
					character.Variables.Perm.Remove(kingSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (fObj.Done && !kObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(kingSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(kingSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Siaulav_Blue, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Dive incoming!"));
							character.ServerMessage(L("{#FF9966}The Siaulav King dives from the upper winds!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him before he climbs again."));
					}
				}
				else if (!bObj.Done)
				{
					await dialog.Msg(L("Three banners first. Cliff edges, pole deep, canvas open. The thermal collapses inside the hour - he can't dive without being grounded first."));
				}
				else
				{
					await dialog.Msg(L("Banners are up, his thermals are gone. Now ten birds. He dives at the eighth - portcullis-fall, sideways step, throat-strike on the bank."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Sky's quiet. My falcons hunt the upper winds in pairs now - they couldn't before; the King would split them. Three new chicks fledged this season; one of them I named for you, since you wouldn't give me your name to use otherwise."));
			}
		});

		// Thermal-banner plant points for Quest 1005
		//-------------------------------------------------------------------------
		void AddThermalBanner(int bannerNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Thermal-Banner"), "f_tableland_28_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_28_2", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A folded canvas banner waiting at the cliff edge*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_28_2.Quest1005.Banner{bannerNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already planted; the canvas snaps in the wind*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Planting banner..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_28_2.Quest1005.BannersPlanted", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_tableland_28_2.Quest1005.BannersPlanted", count);
					character.ServerMessage(LF("Thermal-banners planted: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All banners planted! Now bait out the King.{/}"));
				}
				else
				{
					character.ServerMessage(L("Planting interrupted."));
				}
			});
		}

		AddThermalBanner(1, 1100, 1100, 0);
		AddThermalBanner(2, 1300, 1300, 90);
		AddThermalBanner(3, 1000, 1400, 180);

		// Quest 6: Stogas Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Master] Leokadija"), "f_tableland_28_2", -1200, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1006);

			dialog.SetTitle(L("Leokadija"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A caravan-master folds a route-map against her hip and ties it shut with a length of waxed twine*{/}"));
				await dialog.Msg(L("Stogas plateau is the southern leg of every wool-caravan running between the tableland and Klaipeda. The route's profitable, but only if the staging-post stays viable - and the staging-post stays viable only if the surrounding pasture stays cleared."));
				await dialog.Msg(L("Standard sweep clears the route. The Caravan-Stone at the staging-post records each sweep in chalk; the next caravan-master reads the stone before the wagons roll. Without the chalk-line, they pad guard-pay by a third."));

				var response = await dialog.Select(L("Kill 12 Siaulavs and 12 Lapasapes to clear the route, then chalk the tally on the Caravan-Stone at the staging-post. I countersign warrants from the chalk. Take the contract?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why a stone instead of a ledger-book?"), "info"),
					Option(L("Find a closer hand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She presses a stub of red caravan-chalk into your palm, the colour brighter than the standard white*{/}"));
						await dialog.Msg(L("Twenty-four kills, no padding. Caravan-master cycles through here every week and counts the corpse-piles against my expected ledger."));
						await dialog.Msg(L("Caravan-Stone sits at the staging-post bend. Chalk a single horizontal line - red chalk this time, that's the Stogas convention. The next caravan-master reads it as 'sweep done by a contractor I trust'. Lying about a sweep gets you blacklisted."));
						break;

					case "info":
						await dialog.Msg(L("A ledger travels with the master. The Caravan-Stone stays where the next master needs it. Half my staging-posts have stones older than my mother - the chalk-lines layer down the stone like rings on a tree-stump."));
						await dialog.Msg(L("It's not a system anyone designed. It's the system that worked. Caravan-traders are practical people; we keep what works."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan still rolls. We pay double-guard, the wool's more expensive in Klaipeda, the wool-merchants raise prices, and the whole route loses customers. All for a sweep. I'll find a closer hand if I have to walk to find them."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSiaulavs", out var sObj)) return;
				if (!quest.TryGetProgress("killLapasapes", out var lObj)) return;
				if (!quest.TryGetProgress("chalkStone", out var cObj)) return;

				if (sObj.Done && lObj.Done && cObj.Done)
				{
					await dialog.Msg(L("{#666666}*She countersigns a warrant in matching red ink, then folds it and seals it with a thumb-print*{/}"));
					await dialog.Msg(L("Stone chalked clean, sweep math holds. Caravan-master rate, paid honest. The next wool-train rolls at single-guard rate; that's coin in your pocket and coin in mine."));
					await dialog.Msg(L("Stop by next fortnight if you want the contract again. The route's always hiring; the caravan-trade always thanks a swordhand who chalks the stone like they mean it."));
					character.Quests.Complete(questId);
				}
				else if (sObj.Done && lObj.Done)
				{
					await dialog.Msg(L("Sweep complete. Now the Caravan-Stone - red chalk, single horizontal line, no flourishes. The next master reads it tomorrow at first light."));
				}
				else
				{
					await dialog.Msg(L("Twelve of each. Take the Lapasapes early - they flock at midday when the sun's high; later in the day they scatter and the count gets harder."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravan's through, single-guard rate, wool-merchants delighted. Your red chalk-line on the Caravan-Stone is the third-most-recent on the eastern face. Some traders ask who chalked it; I tell them a swordhand who knows the value of a stone-line."));
			}
		});

		// Caravan-Stone for Quest 1006
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Caravan-Stone"), "f_tableland_28_2", -1150, 1050, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A flat staging-post stone, chalked with old caravan-tallies*{/}"));
				return;
			}

			var chalkedKey = "Laima.Quests.f_tableland_28_2.Quest1006.StoneChalked";
			if (character.Variables.Perm.GetBool(chalkedKey, false))
			{
				await dialog.Msg(L("{#666666}*Already chalked*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killSiaulavs", out var sObj)) return;
			if (!quest.TryGetProgress("killLapasapes", out var lObj)) return;

			if (!(sObj.Done && lObj.Done))
			{
				await dialog.Msg(L("{#666666}*Stone's ready, but the sweep isn't done*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Chalking stone..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(chalkedKey, true);
				character.ServerMessage(L("{#FFD700}Caravan-Stone chalked. Return to Caravan Master Leokadija.{/}"));
			}
			else
			{
				character.ServerMessage(L("Chalking interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland282Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1001);
		SetName(L("Blue Siaulav Flocks"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Siaulavs dominating the Stogas sky."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Sky-Ward] Algirdas"), "f_tableland_28_2");

		AddObjective("killSiaulavs", L("Kill Blue Siaulavs"),
			new KillObjective(25, new[] { MonsterId.Siaulav_Blue }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FTableland282Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1002);
		SetName(L("Mana Feathers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Siaulav Mages and bring mana feathers for magic arrows."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Grazina"), "f_tableland_28_2");

		AddObjective("killMages", L("Kill Blue Siaulav Mages"),
			new KillObjective(15, new[] { MonsterId.Siaulav_Mage_Blue }));

		AddObjective("gatherFeathers", L("Gather mana feathers"),
			new CollectItemObjective(650069, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650069, character.Inventory.CountItem(650069), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650069, character.Inventory.CountItem(650069), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland282Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1003);
		SetName(L("Masterwork Sinews"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Siaulav Bows and bring sinews for a masterwork bow."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bowyer] Ramunas"), "f_tableland_28_2");

		AddObjective("killBows", L("Kill Blue Siaulav Bows"),
			new KillObjective(15, new[] { MonsterId.Siaulav_Bow_Blue }));

		AddObjective("gatherSinews", L("Gather sinews"),
			new CollectItemObjective(650070, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650070, character.Inventory.CountItem(650070), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650070, character.Inventory.CountItem(650070), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland282Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1004);
		SetName(L("Lapasape Swarm"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Lapasapes stripping the Stogas pasture."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shepherd] Ausra"), "f_tableland_28_2");

		AddObjective("killLapasapes", L("Kill Blue Lapasapes"),
			new KillObjective(35, new[] { MonsterId.Lapasape_Blue }));

		AddObjective("resetPosts", L("Reset the four sheep-bell posts"),
			new VariableCheckObjective("Laima.Quests.f_tableland_28_2.Quest1004.PostsReset", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_2.Quest1004.PostsReset");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_2.Quest1004.Post{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_2.Quest1004.PostsReset");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_2.Quest1004.Post{i}");
	}
}

public class FTableland282Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1005);
		SetName(L("The Siaulav King"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Siaulav flock to draw down the King from the upper winds."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Falconer] Ignas"), "f_tableland_28_2");

		AddObjective("plantBanners", L("Plant the three thermal-banners"),
			new VariableCheckObjective("Laima.Quests.f_tableland_28_2.Quest1005.BannersPlanted", 3, true));

		AddObjective("killFlock", L("Thin Blue Siaulavs"),
			new KillObjective(10, new[] { MonsterId.Siaulav_Blue }));

		AddObjective("killKing", L("Defeat the Siaulav King"),
			new KillObjective(1, new[] { MonsterId.Siaulav_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_2.Quest1005.BannersPlanted");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_2.Quest1005.Banner{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_2.Quest1005.BannersPlanted");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_2.Quest1005.Banner{i}");
	}
}

public class FTableland282Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1006);
		SetName(L("Stogas Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Blue Siaulavs and Blue Lapasapes."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Leokadija"), "f_tableland_28_2");

		AddObjective("killSiaulavs", L("Kill Blue Siaulavs"),
			new KillObjective(12, new[] { MonsterId.Siaulav_Blue }));

		AddObjective("killLapasapes", L("Kill Blue Lapasapes"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Blue }));

		AddObjective("chalkStone", L("Chalk the tally on the Caravan-Stone"),
			new VariableCheckObjective("Laima.Quests.f_tableland_28_2.Quest1006.StoneChalked", 1, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_2.Quest1006.StoneChalked");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_2.Quest1006.StoneChalked");
	}
}
