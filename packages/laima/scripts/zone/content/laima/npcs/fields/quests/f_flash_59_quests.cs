//--- Melia Script ----------------------------------------------------------
// Verkti Square Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed plateau quests for the Verkti Square map.
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

public class FFlash59QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Stone-Wet Jukopus
		//-------------------------------------------------------------------------
		AddNpc(20070, L("[Ward-Keeper] Halya"), "f_flash_59", 275, 602, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1001);

			dialog.SetTitle(L("Halya"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Welcome to Verkti Square. Don't stand still too long - the curse here takes the still before it takes the moving."));
				await dialog.Msg(L("The Grey Jukopus drink the cursed groundwater. Their flesh is half-stone now - slow, but vicious, and their grip leaves creeping grey streaks on whatever they grab."));
				await dialog.Msg(L("Thin the herd and the streaks in the soil retreat. Let them breed and the curse spreads faster than a runner can outpace."));

				var response = await dialog.Select(L("How many need killing?"),
					Option(L("I'll thin the stone-wet Jukopus"), "help"),
					Option(L("Creeping grey streaks?"), "info"),
					Option(L("Leave the cursed ground"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two. Keep moving - their touch leaves marks, and marks become stone."));
						await dialog.Msg(L("If a limb goes numb and grey, run. Don't fight through it."));
						break;

					case "info":
						await dialog.Msg(L("Every Jukopus that dies here leaves a patch of the square a little greyer. Every one you kill away from the patch lets that ground recover."));
						await dialog.Msg(L("Pest control and curse-pushback, both at once. That's the only arithmetic that matters."));
						break;

					case "leave":
						await dialog.Msg(L("I wish I could. My wards need a keeper, and the keeper has to be rooted. So I stay."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killJukopus", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The grey's pulling back. I can see the green around the wards again - first time in a month."));
					await dialog.Msg(L("Take your pay. And these - ward-dust salves. They slow the curse if a streak catches you."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still too many at the edges. The streaks are holding. Keep at them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The square's color is coming back. Slow, but it's coming."));
			}
		});

		// Quest 2: Cursebinding Cores
		//-------------------------------------------------------------------------
		AddNpc(20071, L("[Ward-Smith] Radek"), "f_flash_59", 1122, 84, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1002);

			dialog.SetTitle(L("Radek"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Rootcrystals here do something none do elsewhere - they eat the curse. Whatever petrification they absorb, they lock inside a vein-core at the heart."));
				await dialog.Msg(L("Those cores are the only reliable charm-stones we have. Five cores buys me enough wardwork for the inner square for a season."));
				await dialog.Msg(L("My hands don't swing like they used to - the curse has taken my wrists already, slowly. But yours are still flesh. Swing the pick for me?"));

				var response = await dialog.Select(L("Five cursebinding cores?"),
					Option(L("I'll crack Rootcrystals for cores"), "help"),
					Option(L("Your wrists?"), "info"),
					Option(L("Forge your own cores"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Strike clean. The cores ring if they're good - the dead ones crumble to grey dust."));
						await dialog.Msg(L("Five good cores. That's what I need."));
						break;

					case "info":
						await dialog.Msg(L("Stone from the fingertips to the elbow. Slow-set, so I can still move them, but they're heavy as lead and cold as the grave."));
						await dialog.Msg(L("If I stop working on wards, the cure-work slows for the whole square. So I keep swinging, stone wrists and all."));
						break;

					case "leave":
						await dialog.Msg(L("I'm forging what I can. The cores are the bottleneck. Swing the pick or don't - the curse doesn't wait."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var coreCount = character.Inventory.CountItem(663068);

				if (coreCount >= 5)
				{
					await dialog.Msg(L("Five, and every one of them rings. You have a good ear for clean cores."));
					await dialog.Msg(L("Take your pay. I'll start the ward-work tonight - the inner square will hold for the season."));

					character.Inventory.Remove(663068, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep cracking. You've got {0} of five. The dull ones won't count - listen for the ring.", coreCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The wards are holding. My wrists ache less than they did, though I couldn't say why."));
			}
		});

		// Quest 3: The Statued Garrison
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Statue-Cataloguer] Dania"), "f_flash_59", -75, 119, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1003);

			dialog.SetTitle(L("Dania"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The garrison here didn't fall to an enemy. The curse took them, one by one, at their posts. Every grey boulder in Verkti used to be a soldier."));
				await dialog.Msg(L("Each of them buried a cache before the stone reached their arms. Letters home, name-tokens, muster records. The Rambears sleep on the boulders now, and on the caches underneath."));
				await dialog.Msg(L("Thin the Rambears. Dig up four caches. Give these soldiers names their families haven't heard in a hundred years."));

				var response = await dialog.Select(L("Rambears and caches?"),
					Option(L("I'll recover the caches"), "help"),
					Option(L("Why do Rambears sleep on them?"), "info"),
					Option(L("Let the stone keep them"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fifteen should clear the sleeping ground. The caches are under the boundary-stones - which are soldiers too, most of them."));
						await dialog.Msg(L("Treat the stones with respect. They can't move, but something in them still watches."));
						break;

					case "info":
						await dialog.Msg(L("Curse-warmth. Stone that used to be living flesh holds heat longer than ordinary rock. The bears love it."));
						await dialog.Msg(L("Their hides have stone-streaks now too. Long exposure. The whole ecology's wrong here."));
						break;

					case "leave":
						await dialog.Msg(L("The stone hasn't taken their names yet. Don't let that be what finally does."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRambears", out var killObj)) return;
				if (!quest.TryGetProgress("recoverCaches", out var cacheObj)) return;

				if (killObj.Done && cacheObj.Done)
				{
					await dialog.Msg(L("Four caches. Four soldiers back in the record. The archive will match them to families if families still exist."));
					await dialog.Msg(L("Take your pay. I'll carve their names on a proper plaque - stone fighting stone."));

					character.Inventory.Remove(650675, character.Inventory.CountItem(650675), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Rambears. ");
					if (!cacheObj.Done)
						status += L("Recover more statued-garrison caches. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three of the four caches matched to living descendants. One great-granddaughter came to see the stone. She stayed all afternoon."));
			}
		});

		// Quest 4: Petrifier-Cant
		//-------------------------------------------------------------------------
		AddNpc(20121, L("[Curse-Scholar] Ivor"), "f_flash_59", 99, -298, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1004);

			dialog.SetTitle(L("Ivor"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Wand-Goblins aren't native to Verkti. They came for the curse - they think they can harvest it, bottle it, turn it into cant they can chant."));
				await dialog.Msg(L("And they're not wrong. The pages they carry are stolen transcripts of petrifier-cant, half-deciphered. Every page Fedimian's archives recover means the curse is a little less weaponizable by strangers."));
				await dialog.Msg(L("Twelve goblins, five pages. The pages matter more than the goblins - but you'll fell them to get the pages. Both are the work."));

				var response = await dialog.Select(L("Twelve and five?"),
					Option(L("I'll bring the pages"), "help"),
					Option(L("Why do goblins want this curse?"), "info"),
					Option(L("Let them have it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Pages are folded small and oiled against damp. Check every belt-pouch."));
						await dialog.Msg(L("Their chanting lands as a greying touch - not full stone, but lingering. Close fast, don't let them complete a verse."));
						break;

					case "info":
						await dialog.Msg(L("A curse that turns people to stone is a weapon nobody can counter until they know the cant. Goblins figure: steal the cant, sell it, rule a valley."));
						await dialog.Msg(L("Ambitious. Also catastrophic if they succeed. We don't let them succeed."));
						break;

					case "leave":
						await dialog.Msg(L("And wake up to a statued village next year? No. The pages come back or the pages burn."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killWandGoblins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPages", out var pageObj)) return;

				if (killObj.Done && pageObj.Done)
				{
					await dialog.Msg(L("Five pages. And twelve goblins silenced. The cant's contained - for this batch, anyway."));
					await dialog.Msg(L("Take your pay. The pages go in the sealed case tonight. Two of them had live sigils - I'll neutralize those myself."));

					character.Inventory.Remove(650783, character.Inventory.CountItem(650783), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Wand-Goblins. ");
					if (!pageObj.Done)
						status += L("Recover more petrifier-cant pages. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pages are sealed in the Fedimian archive. Whatever the goblins planned, it's a year set back."));
			}
		});

		// Quest 5: The Stone-Scarred Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Stryker"), "f_flash_59", 747, -476, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_flash_59.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Stryker"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("There's an Alpha Rambear east of the square. Half his hide's stone now - the curse took, but didn't finish. Made him meaner, heavier, slower to tire."));
				await dialog.Msg(L("He won't engage while his pack's intact. Thin ten Rambears first - once the pack scatters, he comes out swinging. And his stone-side hits like a landslide."));
				await dialog.Msg(L("Bounty's set. His stone-plated hide alone is worth more than a house; the ward-smiths will pay for every pound of it."));

				var response = await dialog.Select(L("Sounds like a fight."),
					Option(L("I'll take the Alpha"), "help"),
					Option(L("Why pay for cursed hide?"), "info"),
					Option(L("Leave him be"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten. Don't rush - if you face him unready, his stone-side will send you home in pieces."));
						await dialog.Msg(L("He leads with the stone shoulder. Stay off his right side."));
						break;

					case "info":
						await dialog.Msg(L("Stone-cured by the curse itself. The ward-smiths carve the hide into cursework that actually holds against the thing that made it."));
						await dialog.Msg(L("Fight fire with fire. Or stone with stone."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe next month. The bounty doesn't cool."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var packObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var alphaObj)) return;

				if (packObj.Done && alphaObj.Done)
				{
					await dialog.Msg(L("Stone hide intact. That's him. You don't get plates like that on any normal bear."));
					await dialog.Msg(L("Bounty paid, plus my share. The ward-smiths will carve cursework for weeks off that pelt."));

					character.Variables.Perm.Remove(alphaSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (packObj.Done && !alphaObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Rambear, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Pack's thin. Listen - that's his roar. He's coming."));
							await dialog.Msg(L("{#FF9966}Move! Stay off his right.{/}"));
							character.ServerMessage(L("{#FF9966}The Stone-Scarred Alpha charges out of the copse!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out there. Don't let him slip back into the copse."));
					}
				}
				else
				{
					await dialog.Msg(L("Pack's still thick. Thin them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Hide's with the ward-smiths. Three new charm-plates going into the inner wards tonight."));
			}
		});

		// Quest 6: The Cursed Perimeter
		//-------------------------------------------------------------------------
		AddNpc(155150, L("[Trail Master] Odessa"), "f_flash_59", -712, -309, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1006);

			dialog.SetTitle(L("Odessa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Verkti perimeter trail is the only way through without crossing deeper curse-ground. Right now it's choked on both flanks."));
				await dialog.Msg(L("Stone-wet Jukopus on the low ground - their slime leaves grey streaks that creep into the cart tracks. Wand-Goblins on the rises, chanting curse-cant at every passerby."));

				var response = await dialog.Select(L("Both flanks?"),
					Option(L("I'll clear both"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Use the deep route"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve and twelve. Low threats at the ankles, high threats chanting from the ridges. Stay covered."));
						await dialog.Msg(L("Clear them, and three caravans roll through by week's end."));
						break;

					case "info":
						await dialog.Msg(L("Jukopus streak you slow. Goblins grey you fast. Whichever's touching you is worse."));
						await dialog.Msg(L("Ward-paste helps for either. Keep some in a side-pocket."));
						break;

					case "leave":
						await dialog.Msg(L("The deep route crosses the inner curse. You'd come out half-stone, if you came out. It's the perimeter or nothing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killJukopus", out var jukObj)) return;
				if (!quest.TryGetProgress("killGoblins", out var gobObj)) return;

				if (jukObj.Done && gobObj.Done)
				{
					await dialog.Msg(L("Both flanks thinned to safe numbers. Caravans commit first thing tomorrow."));
					await dialog.Msg(L("Take your pay. The perimeter's yours by reputation now."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!jukObj.Done)
						status += L("Kill more Grey Jukopus. ");
					if (!gobObj.Done)
						status += L("Kill more Wand-Goblins. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four caravans through since. Drivers leave ward-charms at the waypost - a toll of thanks."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Stone-Wet Jukopus
//-----------------------------------------------------------------------------

public class StoneWetJukopusQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1001);
		SetName(L("Stone-Wet Jukopus"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Grey Jukopus whose cursed slime is creeping petrification into Verkti Square."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ward-Keeper] Halya"), "f_flash_59");

		AddObjective("killJukopus", L("Kill stone-wet Grey Jukopus"),
			new KillObjective(22, new[] { MonsterId.Jukopus_Gray }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

// Quest 1002 CLASS: Cursebinding Cores
//-----------------------------------------------------------------------------

public class CursebindingCoresQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1002);
		SetName(L("Cursebinding Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("Crack Rootcrystals in Verkti Square and bring the ward-smith the curse-locked vein cores."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ward-Smith] Radek"), "f_flash_59");

		AddObjective("gatherCores", L("Gather cursebinding vein cores"),
			new CollectItemObjective(663068, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(663068, 0.40f, MonsterId.Jukopus_Gray);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663068, character.Inventory.CountItem(663068), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663068, character.Inventory.CountItem(663068), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Statued Garrison
//-----------------------------------------------------------------------------

public class TheStatuedGarrisonQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1003);
		SetName(L("The Statued Garrison"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Rambears sleeping on the statued soldiers and dig up four caches the garrison buried before the curse took their arms."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Statue-Cataloguer] Dania"), "f_flash_59");

		AddObjective("killRambears", L("Kill stone-streaked Rambears"),
			new KillObjective(15, new[] { MonsterId.Rambear }));

		AddObjective("recoverCaches", L("Recover statued-garrison caches"),
			new CollectItemObjective(650675, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650675, 0.40f, MonsterId.Rambear);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650675, character.Inventory.CountItem(650675), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650675, character.Inventory.CountItem(650675), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Petrifier-Cant
//-----------------------------------------------------------------------------

public class PetrifierCantQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1004);
		SetName(L("Petrifier-Cant"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Wand-Goblins harvesting the curse and recover the stolen petrifier-cant pages before they can weaponize it."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Scholar] Ivor"), "f_flash_59");

		AddObjective("killWandGoblins", L("Kill Wand-Goblins"),
			new KillObjective(12, new[] { MonsterId.Goblin2_Wand1 }));

		AddObjective("gatherPages", L("Recover petrifier-cant pages from Wand-Goblins"),
			new CollectItemObjective(650783, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650783, 0.50f, MonsterId.Goblin2_Wand1);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650783, character.Inventory.CountItem(650783), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650783, character.Inventory.CountItem(650783), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Stone-Scarred Alpha
//-----------------------------------------------------------------------------

public class TheStoneScarredAlphaQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1005);
		SetName(L("The Stone-Scarred Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Rambear pack to draw out the Stone-Scarred Alpha, then bring him down for the bounty."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Stryker"), "f_flash_59");

		AddObjective("killPack", L("Thin the Rambear pack"),
			new KillObjective(10, new[] { MonsterId.Rambear }));

		AddObjective("killAlpha", L("Defeat the Stone-Scarred Alpha"),
			new KillObjective(1, new[] { MonsterId.Rambear }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

// Quest 1006 CLASS: The Cursed Perimeter
//-----------------------------------------------------------------------------

public class TheCursedPerimeterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1006);
		SetName(L("The Cursed Perimeter"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Grey Jukopus and Wand-Goblins along the cursed perimeter trail to reopen it to caravans."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Trail Master] Odessa"), "f_flash_59");

		AddObjective("killJukopus", L("Kill stone-wet Grey Jukopus"),
			new KillObjective(12, new[] { MonsterId.Jukopus_Gray }));

		AddObjective("killGoblins", L("Kill Wand-Goblins"),
			new KillObjective(12, new[] { MonsterId.Goblin2_Wand1 }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
