//--- Melia Script ----------------------------------------------------------
// Coastal Fortress Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed quests for the Coastal Fortress ruins.
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

public class FFlash291QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Minos Charge
		//-------------------------------------------------------------------------
		AddNpc(20120, L("[Fortress Watch] Havel"), "f_flash_29_1", -1000, -700, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1001);

			dialog.SetTitle(L("Havel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Coastal Fortress was built to watch the sea. The curse didn't come from the sea - it came up through the foundations, stone-warping the garrison where they stood."));
				await dialog.Msg(L("Minos herds graze on the curse-ground now. They charge anything upright. The seawall patrol can't walk their rounds with this many."));
				await dialog.Msg(L("Thin twenty-two. The seawall matters - tide-wards depend on the patrol holding the line."));

				var response = await dialog.Select(L("Twenty-two?"),
					Option(L("I'll thin the Minos"), "help"),
					Option(L("Tide-wards?"), "info"),
					Option(L("Abandon the fortress"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-two Orange Minos from the fortress grounds?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twenty-two. Stand ground when they charge - dodging invites a swarm."));
							await dialog.Msg(L("Keep your ward-charm close. They leave stone-dust when they fall."));
						}
						break;

					case "info":
						await dialog.Msg(L("The curse touches seawater too. Tide-wards keep the curse from spreading to the coastal towns at high tide."));
						await dialog.Msg(L("Patrol breaks, wards break. Wards break, the coast greys by next moon."));
						break;

					case "leave":
						await dialog.Msg(L("Fortress holds the tide-wards. Fortress falls, the coast follows. I'm not moving."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Patrol walked their rounds clean this morning. Tide-wards hold another week."));
					await dialog.Msg(L("Take your pay. And a salt-charm - coastal curse cuts differently than inland."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Seawall still hearing hooves. Keep cutting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Tide-wards are steady. Coastal towns stay flesh another moon."));
			}
		});

		// Quest 2: Seacore Cracking
		//-------------------------------------------------------------------------
		AddNpc(20121, L("[Tide-Warden] Iness"), "f_flash_29_1", -500, -200, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1002);

			dialog.SetTitle(L("Iness"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Rootcrystals here are the deep variant - they pull curse down through the foundations and lock it in a salt-rimed core. Seacores, we call them."));
				await dialog.Msg(L("Seacores are the only thing that holds under tide-exposure. Five of them charges the main tide-ward for the season."));
				await dialog.Msg(L("My hands are brine-grey - sea-curse is slower but harder to reverse. Can you crack?"));

				var response = await dialog.Select(L("Five seacores?"),
					Option(L("I'll crack the seacores"), "help"),
					Option(L("Brine-grey?"), "info"),
					Option(L("Find another warden"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Crack five deep Rootcrystals and bring back their seacores?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Seacores are rime-white with salt. Dull ones are duds - listen for the ring."));
							await dialog.Msg(L("Five ringing ones. Thank you."));
						}
						break;

					case "info":
						await dialog.Msg(L("Salt-water curse variant. Slow. Soft. Permanent. My fingers are brine-stone to the second knuckle."));
						await dialog.Msg(L("I warm them in fresh water each night. Keeps the spread to a finger-width a year. I'll have hands for a decade yet."));
						break;

					case "leave":
						await dialog.Msg(L("No other tide-warden this side of the fortress. If I don't work, the coast greys."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var coreCount = character.Inventory.CountItem(650250);

				if (coreCount >= 5)
				{
					await dialog.Msg(L("Five rime-white ones. Good ear. The tide-ward will charge tonight."));
					await dialog.Msg(L("Take your pay. Coastal towns owe you a meal, even if they don't know your name yet."));

					character.Inventory.Remove(650250, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep cracking. {0} of five. Listen for the ring.", coreCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Tide-ward charged clean. Three coastal towns stay flesh another season."));
			}
		});

		// Quest 3: The Fortress Armament
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Quartermaster] Osk"), "f_flash_29_1", 400, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1003);

			dialog.SetTitle(L("Osk"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The fortress garrison was mid-change-of-watch when the curse took them. They left four weapon caches sealed in the armament niches, untouched."));
				await dialog.Msg(L("Infroholder Bowmen roost on the caches - the niches are high and sheltered. Thin fifteen, recover four, and the current watch finally gets proper equipment."));

				var response = await dialog.Select(L("Infroholders and armament?"),
					Option(L("I'll recover the caches"), "help"),
					Option(L("Proper equipment?"), "info"),
					Option(L("Use current gear"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Infroholder Bowmen and recover four fortress armament caches?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Fifteen. The bowmen shoot first - close fast. Caches are in the high niches, sealed with iron clasp."));
							await dialog.Msg(L("Pry the clasps with a knife. Don't force - the wood is curse-dried."));
						}
						break;

					case "info":
						await dialog.Msg(L("We've been patrolling with mismatched spears and old swords. The caches have standard-issue gear - uniform, heavy, purpose-built."));
						await dialog.Msg(L("Watch rounds go faster with proper gear. Faster rounds mean tighter wards."));
						break;

					case "leave":
						await dialog.Msg(L("Current gear is half-broken and mismatched. That's why three patrol-hands greyed last month - gear failures."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBowmen", out var killObj)) return;
				if (!quest.TryGetProgress("recoverCaches", out var cacheObj)) return;

				if (killObj.Done && cacheObj.Done)
				{
					await dialog.Msg(L("Four caches. Every patrol-hand gets fresh gear tomorrow. First full re-equip in a decade."));
					await dialog.Msg(L("Take your pay. The watch will drink to you tonight."));

					character.Inventory.Remove(650388, character.Inventory.CountItem(650388), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Infroholder Bowmen. ");
					if (!cacheObj.Done)
						status += L("Recover more armament caches. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Full re-equip done. Patrol walks faster, holds tighter. Three-month mortality down to zero."));
			}
		});

		// Armament Niche Points
		//-------------------------------------------------------------------------
		void AddArmamentNiche(int cacheNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Armament Niche"), "f_flash_29_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_flash_29_1", 1003);
				var variableKey = $"Laima.Quests.f_flash_29_1.Quest1003.Cache{cacheNum}";
				var spawnedKey = $"Laima.Quests.f_flash_29_1.Quest1003.Cache{cacheNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A sealed iron-clasp niche in the fortress wall, high and sheltered.*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already recovered this niche's cache*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 35)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Infroholder_Bow_Red, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FFCC66}Infroholder Bowmen drop from the upper niches, bows drawn!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Prying the iron clasp..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650388, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Fortress Armament Cache"));

					var currentCount = character.Inventory.CountItem(650388);
					character.ServerMessage(LF("Caches recovered: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All four armament caches recovered! Return to Osk.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You set the clasp back. Try again."));
				}
			});
		}

		AddArmamentNiche(1, -800, 200, 0);
		AddArmamentNiche(2, 200, 500, 0);
		AddArmamentNiche(3, 600, -400, 0);
		AddArmamentNiche(4, -200, -600, 0);

		// Quest 4: The Green-Mage Cabal
		//-------------------------------------------------------------------------
		AddNpc(20122, L("[Curse-Scholar] Riza"), "f_flash_29_1", 100, 400, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1004);

			dialog.SetTitle(L("Riza"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Minos Mages are channelling the coastal curse-variant. Their staves carry sigil-plates - salt-brands, they're called."));
				await dialog.Msg(L("Every salt-brand we recover breaks a node in their channel-network. Twelve mages, five brands - enough to collapse the coastal channel for a season."));

				var response = await dialog.Select(L("Twelve and five?"),
					Option(L("I'll bring the brands"), "help"),
					Option(L("Channel-network?"), "info"),
					Option(L("Let them channel"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Green Minos Mages and recover five salt-brands?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Salt-brands are socketed into the staves. Pry them out clean - we need the sigils intact."));
							await dialog.Msg(L("Glove up. Raw salt-brand cold-burns on contact."));
						}
						break;

					case "info":
						await dialog.Msg(L("Imagine a grid of glowing lines under the coast. Each mage is a node. Enough nodes, enough brands, the grid stays lit and the curse spreads along it."));
						await dialog.Msg(L("Collapse nodes, grid dims, spread stalls. Simple enough in theory."));
						break;

					case "leave":
						await dialog.Msg(L("Every day we don't, the grid extends another foot inland. Not an option."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherBrands", out var brandObj)) return;

				if (killObj.Done && brandObj.Done)
				{
					await dialog.Msg(L("Five salt-brands. Twelve mages silenced. The coastal channel just went dark for the season."));
					await dialog.Msg(L("Take your pay. The brands go in the cold-furnace tonight - smoke rises blue when they cancel."));

					character.Inventory.Remove(650575, character.Inventory.CountItem(650575), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Green Minos Mages. ");
					if (!brandObj.Done)
						status += L("Recover more salt-brands. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Brands went blue in the cold-furnace. Coastal channel's dim. The curse edges back a foot this month."));
			}
		});

		// Quest 5: The Herd-Master
		//-------------------------------------------------------------------------
		AddNpc(20123, L("[Bounty Hunter] Drus"), "f_flash_29_1", 700, -300, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1005);
			var herdSpawnedKey = "Laima.Quests.f_flash_29_1.Quest1005.HerdMasterSpawned";

			dialog.SetTitle(L("Drus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Orange Minos herd answers to a Herd-Master - a curse-warped bull twice the size of his pack, stone-plated across the brow."));
				await dialog.Msg(L("Thin ten Minos and his pride drags him out to reestablish order. Bounty's big. His brow-plate is worth more than a year's patrol pay."));

				var response = await dialog.Select(L("Sounds like a fight."),
					Option(L("I'll take the Herd-Master"), "help"),
					Option(L("Brow-plate?"), "info"),
					Option(L("Leave him to his herd"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Thin ten Orange Minos and bring down the Herd-Master when he emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten. Stay mobile when he charges - his brow-plate is a battering ram."));
							await dialog.Msg(L("Good hunting."));
						}
						break;

					case "info":
						await dialog.Msg(L("Cursed bone-plate. The ward-smiths carve it into charge-wards that stop a curse-wave at the coast."));
						await dialog.Msg(L("One plate, a dozen wards. Pays the whole patrol for a year."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe next month. Bounty keeps climbing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHerd", out var herdObj)) return;
				if (!quest.TryGetProgress("killHerdMaster", out var masterObj)) return;

				if (herdObj.Done && masterObj.Done)
				{
					await dialog.Msg(L("Brow-plate's intact. A dozen charge-wards off that one piece."));
					await dialog.Msg(L("Bounty paid, plus my share. The coastal patrol drinks tonight."));

					character.Variables.Perm.Remove(herdSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (herdObj.Done && !masterObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(herdSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(herdSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Minos_Orange, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Herd's thinned. Listen - that's his bellow, deeper than the rest."));
							await dialog.Msg(L("{#FF9966}Move! Don't let him retreat to the herd.{/}"));
							character.ServerMessage(L("{#FF9966}The Herd-Master charges out, brow-plate gleaming!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out there. Hunt him down before he slips back."));
					}
				}
				else
				{
					await dialog.Msg(L("Herd still thick. Thin them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Brow-plate carved into charge-wards. Coastal line held through a curse-surge last tide."));
			}
		});

		// Quest 6: Seawall Perimeter
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Seawall Captain] Mara"), "f_flash_29_1", -200, -800, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_29_1", 1006);

			dialog.SetTitle(L("Mara"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The seawall patrol runs the whole length of the coastal perimeter. Right now it's being bled on both flanks - Minos on the landward side, Infroholders on the seaward."));
				await dialog.Msg(L("Patrol-hands won't commit until both flanks thin. I need the patrol walking, and I need them walking this week."));

				var response = await dialog.Select(L("Both flanks?"),
					Option(L("I'll clear both"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Pull the patrol back"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Orange Minos and twelve Infroholder Bowmen along the seawall?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twelve and twelve. Minos charge straight, Infroholders harry from range. Mind both."));
							await dialog.Msg(L("Clear them, and the patrol walks the full perimeter Monday."));
						}
						break;

					case "info":
						await dialog.Msg(L("Minos charge. Infroholders pin. Whichever one just hit you is worse."));
						await dialog.Msg(L("Together they're impassable. Apart, patrol-walkable."));
						break;

					case "leave":
						await dialog.Msg(L("Pull back and the tide-wards break. Wards break, coastal towns grey. Not happening on my watch."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var minObj)) return;
				if (!quest.TryGetProgress("killInfros", out var infObj)) return;

				if (minObj.Done && infObj.Done)
				{
					await dialog.Msg(L("Both flanks thinned. Patrol walks the full perimeter Monday."));
					await dialog.Msg(L("Take your pay. Tide-wards stay lit."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!minObj.Done)
						status += L("Kill more Orange Minos. ");
					if (!infObj.Done)
						status += L("Kill more Infroholder Bowmen. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Patrol's walking clean. Tide-wards are holding through the equinox."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class MinosChargeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1001);
		SetName(L("Minos Charge"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Orange Minos grazing on curse-ground so the Coastal Fortress seawall patrol can walk its rounds."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fortress Watch] Havel"), "f_flash_29_1");

		AddObjective("killMinos", L("Kill curse-grazing Orange Minos"),
			new KillObjective(22, new[] { MonsterId.Minos_Orange }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
	}
}

public class SeacoreCrackingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1002);
		SetName(L("Seacore Cracking"));
		SetType(QuestType.Sub);
		SetDescription(L("Crack deep Rootcrystals and bring salt-rimed seacores to the tide-warden for the main tide-ward."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tide-Warden] Iness"), "f_flash_29_1");

		AddObjective("gatherSeacores", L("Gather rime-white seacores"),
			new CollectItemObjective(650250, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 5));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650250, character.Inventory.CountItem(650250), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650250, character.Inventory.CountItem(650250), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheFortressArmamentQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1003);
		SetName(L("The Fortress Armament"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Infroholder Bowmen roosting in the armament niches and recover four sealed fortress caches for the current watch."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Quartermaster] Osk"), "f_flash_29_1");

		AddObjective("killBowmen", L("Kill Infroholder Bowmen"),
			new KillObjective(15, new[] { MonsterId.Infroholder_Bow_Red }));

		AddObjective("recoverCaches", L("Recover fortress armament caches"),
			new CollectItemObjective(650388, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650388, character.Inventory.CountItem(650388), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1003.Cache{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1003.Cache{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650388, character.Inventory.CountItem(650388), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1003.Cache{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_29_1.Quest1003.Cache{i}.Spawned");
		}
	}
}

public class TheGreenMageCabalQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1004);
		SetName(L("The Green-Mage Cabal"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Minos Mages channelling coastal curse-spread and recover their salt-brands to collapse the channel grid."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Scholar] Riza"), "f_flash_29_1");

		AddObjective("killMages", L("Kill Green Minos Mages"),
			new KillObjective(12, new[] { MonsterId.Minos_Mage_Green }));

		AddObjective("gatherBrands", L("Recover salt-brands"),
			new CollectItemObjective(650575, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650575, character.Inventory.CountItem(650575), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650575, character.Inventory.CountItem(650575), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheHerdMasterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1005);
		SetName(L("The Herd-Master"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Minos herd to draw out the stone-plated Herd-Master, then bring him down for the brow-plate bounty."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Drus"), "f_flash_29_1");

		AddObjective("killHerd", L("Thin the Minos herd"),
			new KillObjective(10, new[] { MonsterId.Minos_Orange }));

		AddObjective("killHerdMaster", L("Defeat the Herd-Master"),
			new KillObjective(1, new[] { MonsterId.Minos_Orange }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}
}

public class SeawallPerimeterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_29_1", 1006);
		SetName(L("Seawall Perimeter"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Orange Minos on the landward flank and Infroholder Bowmen on the seaward flank to reopen the seawall patrol."));
		SetLocation("f_flash_29_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Seawall Captain] Mara"), "f_flash_29_1");

		AddObjective("killMinos", L("Kill Orange Minos"),
			new KillObjective(12, new[] { MonsterId.Minos_Orange }));

		AddObjective("killInfros", L("Kill Infroholder Bowmen"),
			new KillObjective(12, new[] { MonsterId.Infroholder_Bow_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}
}
