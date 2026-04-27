//--- Melia Script ----------------------------------------------------------
// Pilgrim Road Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the Salvia pilgrim road, overrun by Minos and Lapasape.
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

public class FPilgrimroad413QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Clear the Road
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Pilgrim-Warden] Brone"), "f_pilgrimroad_41_3", -900, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1001);

			dialog.SetTitle(L("Brone"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Minos packs choke the Salvia road. Pilgrims can't pass. Kill forty Green Minos and the caravan moves tonight."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Caravan?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. The road opens step by step."));
						break;

					case "info":
						await dialog.Msg(L("Grain wagons. Relic boxes. Folk on foot. All stopped a week."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims keep waiting."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Road's walkable. Caravan rolls at dawn."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First wagon through. Pilgrims blessed your name."));
			}
		});

		// Quest 2: Bow-Minos Quivers
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Road-Marshal] Ysanne"), "f_pilgrimroad_41_3", 200, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1002);

			dialog.SetTitle(L("Ysanne"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bow-Minos pick pilgrims from the ridges. Kill twenty-five, bring eight quivers for the militia armoury."));

				var response = await dialog.Select(L("Quivers?"),
					Option(L("I'll bring"), "help"),
					Option(L("Ridges?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Eight quivers. Intact if you can."));
						break;

					case "info":
						await dialog.Msg(L("North ridge, east ridge. They shoot from cover. Close the distance fast."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims keep falling."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBows", out var killObj)) return;
				if (!quest.TryGetProgress("gatherQuivers", out var qObj)) return;

				if (killObj.Done && qObj.Done)
				{
					await dialog.Msg(L("Eight quivers. Militia scouts arm up tonight."));
					character.Inventory.Remove(650251, character.Inventory.CountItem(650251), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Scouts arrow the ridges clean each morning now."));
			}
		});

		// Quest 3: Lapasape Grimoires
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Cantor] Ilse-II"), "f_pilgrimroad_41_3", -1000, -300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1003);

			dialog.SetTitle(L("Ilse"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Brown Lapasape Mages hex the pilgrim wells. Kill thirty, bring six hex-grimoires for the shrine."));

				var response = await dialog.Select(L("Grimoires?"),
					Option(L("I'll bring"), "help"),
					Option(L("Wells?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't read the margins."));
						break;

					case "info":
						await dialog.Msg(L("Pilgrims drink, pilgrims sicken. Grimoires let the shrine trace the hex and break it."));
						break;

					case "leave":
						await dialog.Msg(L("Wells stay poisoned."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherGrimoires", out var gObj)) return;

				if (killObj.Done && gObj.Done)
				{
					await dialog.Msg(L("Six grimoires. Shrine breaks the hex by matins."));
					character.Inventory.Remove(650252, character.Inventory.CountItem(650252), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two wells cleansed. Four to go."));
			}
		});

		// Quest 4: Crystal Breaking
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Road-Mason] Caelum"), "f_pilgrimroad_41_3", 800, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1004);

			dialog.SetTitle(L("Caelum"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A road-mason kneels beside a fractured flagstone, running his thumb along a crack that's grown a fingerwidth since yesterday*{/}"));
				await dialog.Msg(L("Twenty years I've laid stones for the Salvia pilgrimage. Twenty years of frost and cart-wheels, and the road held. Then last spring the rootcrystals started pushing up through the bedrock."));
				await dialog.Msg(L("Now every dawn I find another flagstone split. My boys can re-lay one a day; the crystals crack three. The arithmetic is not in our favour."));

				var response = await dialog.Select(L("Break twenty Rootcrystals along the road, and walk the four chalk-marked splits so I know where to start re-laying. Will you help?"),
					Option(L("I'll break the crystals and walk the splits"), "help"),
					Option(L("Why not just patch the cracks?"), "info"),
					Option(L("That's mason work, not mine"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He hands you a chalk-stub and a wedge-iron, weight of a small loaf*{/}"));
						await dialog.Msg(L("The crystals shatter sharp - they'll cut through boot-leather if you let them. Strike low, step back."));
						await dialog.Msg(L("Four chalk-crosses mark the worst splits: east bend, south crook, north verge, west arc. Kneel at each one and chalk a wedge-mark over the cross. That tells my crew where the new stone goes first."));
						break;

					case "info":
						await dialog.Msg(L("Patching cracks while the crystals still grow is like bandaging a fountain. The crystal shoulders the stone up from below; the patch pops loose by the next thaw."));
						await dialog.Msg(L("Break the crystal at the source, then re-lay clean. That's the only way the road outlives me."));
						break;

					case "leave":
						await dialog.Msg(L("Then the road splits, and the pilgrims take the old shepherd-path through Lapasape country. Last year, six of them didn't come back."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("surveySplits", out var surveyObj)) return;

				if (killObj.Done && surveyObj.Done)
				{
					await dialog.Msg(L("{#666666}*He reads your chalk-marks against his crew-roster, nodding slow*{/}"));
					await dialog.Msg(L("Twenty crystals broken, four splits chalked. My boys start at the east bend at first light - that's the worst of them, the one I lose sleep over."));
					await dialog.Msg(L("Take this. Mason's purse, with a bit on top. You bought us a season."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("The crystals first. No use chalking splits while fresh ones still push up underneath the road."));
				}
				else
				{
					await dialog.Msg(L("Crystals are down - I felt the bedrock settle from here. Now walk the four chalk-crosses. Each one needs a wedge-mark."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Flagstones relaid clean across all four splits. Heard a pilgrim humming a hymn on the east bend last evening - first time in months."));
			}
		});

		// Cracked flagstone survey points for Quest 1004
		//-------------------------------------------------------------------------
		void AddCrackedFlagstone(int splitNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Cracked Flagstone"), "f_pilgrimroad_41_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A cracked flagstone, fractured by a rootcrystal pushing up from below*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_pilgrimroad_41_3.Quest1004.Split{splitNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already chalked and noted for the masons*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Surveying split..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_41_3.Quest1004.SplitsSurveyed", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_pilgrimroad_41_3.Quest1004.SplitsSurveyed", count);
					character.ServerMessage(LF("Splits surveyed: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All splits surveyed! Return to Road-Mason Caelum.{/}"));
				}
				else
				{
					character.ServerMessage(L("Survey interrupted."));
				}
			});
		}

		AddCrackedFlagstone(1, -1020, 443, 0);
		AddCrackedFlagstone(2, -984, -393, 90);
		AddCrackedFlagstone(3, 248, -160, 180);
		AddCrackedFlagstone(4, 795, 316, 270);

		// Quest 5: The Minos Warchief
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Doran-III"), "f_pilgrimroad_41_3", -800, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1005);
			var warchiefSpawnedKey = "Laima.Quests.f_pilgrimroad_41_3.Quest1005.WarchiefSpawned";

			dialog.SetTitle(L("Doran"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty hunter cleans his crossbow nocks with a strip of oiled rag, eyes never leaving the treeline*{/}"));
				await dialog.Msg(L("There's a warchief running the Salvia Minos. Big one, scarred down the snout, drilled discipline into his pack until they hunt in fives instead of mobs."));
				await dialog.Msg(L("Salvia put a bounty on him after he ambushed a relic-cart and dragged the cantor into the reeds. The cantor's bones came back. The relic did not."));

				var response = await dialog.Select(L("Scout the three pack-banners his sub-clans stake along the road, then kill 12 Green Minos to bait him out and end him. Coin's good. Will you take it?"),
					Option(L("I'll take the bounty"), "help"),
					Option(L("Why scout the banners first?"), "info"),
					Option(L("That's not my fight"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*He claps your shoulder once, hard, and points down the road*{/}"));
						await dialog.Msg(L("Three banners, staked at the north verge, the south crook, and the east bend. Read the dye-marks on each - tells me which sub-clans still answer to him."));
						await dialog.Msg(L("Then 12 Green Minos. Pride drags him out by the tenth or so. When he comes, fight on open ground - he's stronger in the reeds."));
						break;

					case "info":
						await dialog.Msg(L("Banners tell us how deep his hold runs. If only one clan answers, killing him fractures the lot. If all three answer, the next chief is already chosen and we've bought ourselves a month, no more."));
						await dialog.Msg(L("Salvia wants the banners catalogued before the kill. The scribes draw paychecks too."));
						break;

					case "leave":
						await dialog.Msg(L("Then the warchief eats another cantor and Salvia raises the bounty. I'll be sitting here when you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("scoutBanners", out var sObj)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killWarchief", out var wObj)) return;

				if (sObj.Done && pObj.Done && wObj.Done)
				{
					await dialog.Msg(L("{#666666}*He weighs a coin pouch in his palm, the leather creaking*{/}"));
					await dialog.Msg(L("Warchief's down, banners are catalogued. Salvia's scribes will sleep easy and the packs scatter by morning."));
					await dialog.Msg(L("Bounty paid in full, and a stipend on top for the banner-work. Drink the first cup for the cantor."));
					character.Variables.Perm.Remove(warchiefSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !wObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(warchiefSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(warchiefSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Minos_Green, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Minos Warchief storms the pilgrim road!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else if (!sObj.Done)
				{
					await dialog.Msg(L("Banners first. Three of them - north verge, south crook, east bend. Read the dye, mark the colours, then we kill."));
				}
				else
				{
					await dialog.Msg(L("Banners are read. Now thin the pack to twelve - he won't show his face for less."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Packs without a chief, road's holding. Cantor's family came by last week to thank Salvia for the kill - I sent them to thank you instead."));
			}
		});

		// Warchief banner scout points for Quest 1005
		//-------------------------------------------------------------------------
		void AddWarchiefBanner(int bannerNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Warchief Pack-Banner"), "f_pilgrimroad_41_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_3", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A staked Minos pack-banner, dyed in pack colours*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_pilgrimroad_41_3.Quest1005.Banner{bannerNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already noted in your scouting tally*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Scouting banner..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_pilgrimroad_41_3.Quest1005.BannersScouted", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_pilgrimroad_41_3.Quest1005.BannersScouted", count);
					character.ServerMessage(LF("Banners scouted: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All banners scouted! Now bait out the Warchief.{/}"));
				}
				else
				{
					character.ServerMessage(L("Scouting interrupted."));
				}
			});
		}

		AddWarchiefBanner(1, -330, 1012, 0);
		AddWarchiefBanner(2, -1112, -1029, 90);
		AddWarchiefBanner(3, 248, -160, 180);

		// Quest 6: Pilgrim Road Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Marek"), "f_pilgrimroad_41_3", 1000, 900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1006);

			dialog.SetTitle(L("Marek"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A militia-captain reads a sweep-roster aloud to a young scribe, both of them squinting in the road-glare*{/}"));
				await dialog.Msg(L("The Salvia pilgrim road needs a working sweep every fortnight or the packs reclaim it inside a week. We don't have the men. The militia is forty strong; the road is fourteen leagues."));
				await dialog.Msg(L("So we hire it out. Per-head bounty, posted at every waystone. The catch is the cantor's ledger - Pelke's pilgrimage tax depends on the count, so the count gets logged or it never happened."));

				var response = await dialog.Select(L("Kill 12 Green Minos, 12 Bow-Minos, and 12 Brown Lapasape Mages, then chalk your tally on the slate at the wayside Pilgrim Shrine. Standard rate. Take it?"),
					Option(L("I'll take the sweep"), "help"),
					Option(L("Why does the count need logging?"), "info"),
					Option(L("Find someone else"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She unfolds a tally-marker and presses it into your palm*{/}"));
						await dialog.Msg(L("Twelve of each, no shortcuts. The shrine slate is on the south bend - lintel-high, can't miss it. Chalk three rows of four, then your initials underneath."));
						await dialog.Msg(L("If a cantor questions the tally, point them to me. I countersign at sundown."));
						break;

					case "info":
						await dialog.Msg(L("Pelke charges a per-pilgrim safety levy on the Salvia route. The levy scales with how many monsters we cleared that fortnight - more kills, more levy, more pilgrims arriving alive."));
						await dialog.Msg(L("Without the shrine-log, the cantors assume zero kills and the levy collapses. Then we don't get paid, then the militia goes home, then the road belongs to the Minos again."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays contested, levy lapses, the militia goes hungry. I'll be here when you reconsider."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var mObj)) return;
				if (!quest.TryGetProgress("killBows", out var bObj)) return;
				if (!quest.TryGetProgress("killMages", out var gObj)) return;
				if (!quest.TryGetProgress("logTally", out var lObj)) return;

				if (mObj.Done && bObj.Done && gObj.Done && lObj.Done)
				{
					await dialog.Msg(L("{#666666}*She countersigns the slate-rubbing in tar-ink and hands you a heavy purse*{/}"));
					await dialog.Msg(L("Tally logged, levy holds. Salvia coin in full, with the militia's regards."));
					await dialog.Msg(L("The cantors will read your initials at the next pilgrimage gathering. Drink to that."));
					character.Quests.Complete(questId);
				}
				else if (mObj.Done && bObj.Done && gObj.Done)
				{
					await dialog.Msg(L("Sweep complete. Now the shrine-slate on the south bend - three rows of four, your initials underneath."));
				}
				else
				{
					await dialog.Msg(L("Twelve Green Minos, twelve Bow-Minos, twelve Lapasape Mages. Keep at it - the shrine waits."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Militia patrols the road hourly now, paid by your sweep's levy. The cantors mention you in the dawn invocation - they don't say your name, just 'the swordhand who held the road.'"));
			}
		});

		// Pilgrim Shrine for Quest 1006 tally log
		//-------------------------------------------------------------------------
		AddNpc(47190, L("Wayside Pilgrim Shrine"), "f_pilgrimroad_41_3", -800, -900, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1006);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A wayside shrine to Pelke, slate lintel chalked with old pilgrim tallies*{/}"));
				return;
			}

			var loggedKey = "Laima.Quests.f_pilgrimroad_41_3.Quest1006.TallyLogged";
			if (character.Variables.Perm.GetBool(loggedKey, false))
			{
				await dialog.Msg(L("{#666666}*Your tally is already chalked on the slate*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("killMinos", out var mObj)) return;
			if (!quest.TryGetProgress("killBows", out var bObj)) return;
			if (!quest.TryGetProgress("killMages", out var gObj)) return;

			if (!(mObj.Done && bObj.Done && gObj.Done))
			{
				await dialog.Msg(L("{#666666}*The slate is ready, but you haven't finished the sweep yet*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Chalking tally..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(loggedKey, true);
				character.ServerMessage(L("{#FFD700}Tally logged on the shrine lintel. Return to Militia-Captain Marek.{/}"));
			}
			else
			{
				character.ServerMessage(L("Logging interrupted."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FPilgrimroad413Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1001);
		SetName(L("Clear the Road"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Minos choking the Salvia pilgrim road."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim-Warden] Brone"), "f_pilgrimroad_41_3");

		AddObjective("killMinos", L("Kill Green Minos"),
			new KillObjective(40, new[] { MonsterId.Minos_Green }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FPilgrimroad413Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1002);
		SetName(L("Bow-Minos Quivers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bow-Minos and bring quivers for the militia armoury."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Marshal] Ysanne"), "f_pilgrimroad_41_3");

		AddObjective("killBows", L("Kill Bow-Minos"),
			new KillObjective(25, new[] { MonsterId.Minos_Bow_Green }));

		AddObjective("gatherQuivers", L("Gather Bow-Minos quivers"),
			new CollectItemObjective(650251, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650251, character.Inventory.CountItem(650251), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650251, character.Inventory.CountItem(650251), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad413Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1003);
		SetName(L("Hex-Grimoires"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Lapasape Mages and bring hex-grimoires for the shrine."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Cantor] Ilse"), "f_pilgrimroad_41_3");

		AddObjective("killMages", L("Kill Brown Lapasape Mages"),
			new KillObjective(30, new[] { MonsterId.Lapasape_Mage_Brown }));

		AddObjective("gatherGrimoires", L("Gather hex-grimoires"),
			new CollectItemObjective(650252, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650252, character.Inventory.CountItem(650252), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650252, character.Inventory.CountItem(650252), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad413Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1004);
		SetName(L("Crystal Breaking"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Rootcrystals splitting the pilgrim road."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Mason] Caelum"), "f_pilgrimroad_41_3");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(20, new[] { MonsterId.Rootcrystal_01 }));

		AddObjective("surveySplits", L("Survey the four chalked flagstone splits"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_3.Quest1004.SplitsSurveyed", 4, true));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_3.Quest1004.SplitsSurveyed");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_3.Quest1004.Split{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_3.Quest1004.SplitsSurveyed");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_3.Quest1004.Split{i}");
	}
}

public class FPilgrimroad413Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1005);
		SetName(L("The Minos Warchief"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Minos to bait the warchief, then end him."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Doran"), "f_pilgrimroad_41_3");

		AddObjective("scoutBanners", L("Scout the three Warchief pack-banners"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_3.Quest1005.BannersScouted", 3, true));

		AddObjective("killPack", L("Kill Green Minos"),
			new KillObjective(12, new[] { MonsterId.Minos_Green }));

		AddObjective("killWarchief", L("Defeat the Minos Warchief"),
			new KillObjective(1, new[] { MonsterId.Minos_Green }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_3.Quest1005.BannersScouted");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_3.Quest1005.Banner{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_3.Quest1005.BannersScouted");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_3.Quest1005.Banner{i}");
	}
}

public class FPilgrimroad413Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1006);
		SetName(L("Pilgrim Road Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Green Minos, Bow-Minos, and Brown Lapasape Mages."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Marek"), "f_pilgrimroad_41_3");

		AddObjective("killMinos", L("Kill Green Minos"),
			new KillObjective(12, new[] { MonsterId.Minos_Green }));

		AddObjective("killBows", L("Kill Bow-Minos"),
			new KillObjective(12, new[] { MonsterId.Minos_Bow_Green }));

		AddObjective("killMages", L("Kill Brown Lapasape Mages"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Mage_Brown }));

		AddObjective("logTally", L("Log the tally at the wayside Pilgrim Shrine"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_3.Quest1006.TallyLogged", 1, true));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_3.Quest1006.TallyLogged");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_3.Quest1006.TallyLogged");
	}
}
