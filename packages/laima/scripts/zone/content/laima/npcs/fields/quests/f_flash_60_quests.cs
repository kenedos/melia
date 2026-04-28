//--- Melia Script ----------------------------------------------------------
// Roxona Market Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed quests for the Roxona Market ruins.
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

public class FFlash60QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Moya Scavengers
		//-------------------------------------------------------------------------
		AddNpc(20080, L("[Stallkeeper] Hedda"), "f_flash_60", -978, 1379, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1001);

			dialog.SetTitle(L("Hedda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Roxona Market used to bring three-wagon trains a day. Now it brings Moya - stone-streaked, grave-silent, picking at statued vendors."));
				await dialog.Msg(L("Every Moya is a stone seed waiting to spread. They graze on the curse and carry it where they roam."));
				await dialog.Msg(L("Thin them out. The wards along the south arcade can still be reset if the Moya count drops enough."));

				var response = await dialog.Select(L("Thin the scavengers?"),
					Option(L("I'll kill twenty-two Moya"), "help"),
					Option(L("Graze on the curse?"), "info"),
					Option(L("Abandon the arcade"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two. Move fast - their carcasses leave grey circles where they fall."));
						await dialog.Msg(L("Keep moving. The ground here holds a grudge."));
						break;

					case "info":
						await dialog.Msg(L("The curse in the soil is like lichen to them. They eat it, store it, and shed it as stone-dust behind them."));
						await dialog.Msg(L("Less Moya, less dust, less spread. Simple as that."));
						break;

					case "leave":
						await dialog.Msg(L("The arcade's three generations of my family. I'm not abandoning it while wards still hold."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMoya", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Grey circles are fading. The wards will hold another season."));
					await dialog.Msg(L("Take your pay, and a charm from the stall. It won't stop the curse, but it warns you."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More Moya than I'd like still grazing. Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("South arcade's repainted. Not to pre-curse standards, but enough to trade from."));
			}
		});

		// Quest 2: Silvered Cores
		//-------------------------------------------------------------------------
		AddNpc(20081, L("[Ward-Jeweler] Talvi"), "f_flash_60", -877, 331, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1002);

			dialog.SetTitle(L("Talvi"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Rootcrystals in Roxona carry a layer of market-silver - coins melted in when the curse passed over the vaults. You crack them right, you get a silvered core."));
				await dialog.Msg(L("Silver holds cursework twice as long as plain core. Five silvered cores fits a proper charm-necklace that lasts a year."));
				await dialog.Msg(L("I can't crack them myself. My breath has a grey tint now. Anyone else in the market has the same problem."));

				var response = await dialog.Select(L("Five silvered cores?"),
					Option(L("I'll crack the crystals"), "help"),
					Option(L("Grey breath?"), "info"),
					Option(L("Leave the charm-work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Silvered cores feel heavier than plain. If one feels light, it's a dud - leave it."));
						await dialog.Msg(L("Five proper ones. Thank you."));
						break;

					case "info":
						await dialog.Msg(L("Curse inhaled over years. Lungs are slow-setting. Doctors say another five years before it reaches my chest."));
						await dialog.Msg(L("Plenty of time to finish a thousand more charms. I work while I still can."));
						break;

					case "leave":
						await dialog.Msg(L("Every charm I don't make is a caravan driver who rolls through here unprotected. I'd rather keep making them."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var coreCount = character.Inventory.CountItem(650317);

				if (coreCount >= 5)
				{
					await dialog.Msg(L("Five heavy ones. Good eye. I can start four necklaces tonight."));
					await dialog.Msg(L("Take your pay. And a bead for your own pocket - it flares grey when curse-ground thickens."));

					character.Inventory.Remove(650317, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep cracking. {0} of five. Weigh them in your palm - the silvered ones sit like bricks.", coreCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four necklaces delivered. Three drivers wrote thank-you notes. That's a good week."));
			}
		});

		// Quest 3: The Vendor Ledgers
		//-------------------------------------------------------------------------
		AddNpc(20120, L("[Archivist] Brys"), "f_flash_60", 299, -43, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1003);

			dialog.SetTitle(L("Brys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Every stall in Roxona kept a day-ledger. When the curse swept through, the stallkeepers tucked their ledgers into the stall frames before stone took their hands."));
				await dialog.Msg(L("I can reconstruct half the market's history if I get four ledgers. Problem - Bavon nest in the stall frames, and they scatter when disturbed."));
				await dialog.Msg(L("Thin fifteen Bavon, pull four ledgers out. The archive will be grateful for decades."));

				var response = await dialog.Select(L("Bavon and ledgers?"),
					Option(L("I'll recover the ledgers"), "help"),
					Option(L("Why reconstruct a dead market?"), "info"),
					Option(L("Let the archive die"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fifteen should quiet the main nests. Ledgers are in the stall frames - look for the ones with carved initials."));
						await dialog.Msg(L("Handle gently. The paper's curse-dry - it crumbles if you grip wrong."));
						break;

					case "info":
						await dialog.Msg(L("Because Roxona isn't dead, it's stopped. And if it's going to start again, it needs to know what it was."));
						await dialog.Msg(L("The curse took the people. I won't let it take the memory."));
						break;

					case "leave":
						await dialog.Msg(L("That's one choice. I'm making the other."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBavon", out var killObj)) return;
				if (!quest.TryGetProgress("recoverLedgers", out var ledgerObj)) return;

				if (killObj.Done && ledgerObj.Done)
				{
					await dialog.Msg(L("Four ledgers. Four stall histories back in the archive. Two of them I already have partial records for - now I can complete them."));
					await dialog.Msg(L("Take your pay. I'll mention you in the margins when I transcribe."));

					character.Inventory.Remove(650475, character.Inventory.CountItem(650475), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Bavon. ");
					if (!ledgerObj.Done)
						status += L("Recover more vendor ledgers. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three ledgers transcribed. One names a spice merchant my grandmother bought from."));
			}
		});

		// Quest 4: The Saltisdaughter Cabal
		//-------------------------------------------------------------------------
		AddNpc(20082, L("[Curse-Warden] Pavel"), "f_flash_60", 267, 1668, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1004);

			dialog.SetTitle(L("Pavel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Saltisdaughter Mages came for the curse. They're not harvesting - they're worshipping. Every one carries a curse-brand, a sigil-plate that lets them channel petrification."));
				await dialog.Msg(L("Twelve mages, five brands. Brands burn at the temple furnace; the mages silence themselves by their own doing."));
				await dialog.Msg(L("Every brand we burn is one less caravan driver going home as a statue."));

				var response = await dialog.Select(L("Twelve and five?"),
					Option(L("I'll bring the brands"), "help"),
					Option(L("Why worship the curse?"), "info"),
					Option(L("Let the mages be"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Brands are worn on the breastbone. Check every mage you fell - they hide them under robes."));
						await dialog.Msg(L("Don't touch a brand with bare skin. Wrap them. I have a furnace ready."));
						break;

					case "info":
						await dialog.Msg(L("A god that turns flesh to stone is still a god. Some minds find that appealing. I don't pretend to understand."));
						await dialog.Msg(L("Understanding isn't required. Ending them is."));
						break;

					case "leave":
						await dialog.Msg(L("They won't leave us be. That math doesn't work both ways."));
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
					await dialog.Msg(L("Five brands. Twelve mages silenced. The furnace will take the brands tonight - smoke goes up blue when they burn, which means they're dying right."));
					await dialog.Msg(L("Take your pay. Wrap any stone-stiffness you've got in ward-cloth before you sleep."));

					character.Inventory.Remove(650615, character.Inventory.CountItem(650615), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Saltisdaughter Mages. ");
					if (!brandObj.Done)
						status += L("Recover more curse-brands. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Five brands went blue in the furnace. Smoke rose clean. The cabal's thinner by a finger today."));
			}
		});

		// Quest 5: The Stallmaster Alpha
		//-------------------------------------------------------------------------
		AddNpc(20083, L("[Bounty Hunter] Mira"), "f_flash_60", -896, -998, 44, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_flash_60.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Mira"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Bavon grew up in the market vaults and the curse warped it instead of killing it. Half-stone now, twice the size, and it rules the lesser Moya like a stallmaster."));
				await dialog.Msg(L("It won't engage while the Moya hold ranks. Thin ten Moya, and the Stallmaster emerges to punish the slackers."));
				await dialog.Msg(L("Bounty's set. Its plated hide is worth more than a wagon."));

				var response = await dialog.Select(L("Sounds like a fight."),
					Option(L("I'll take the Stallmaster"), "help"),
					Option(L("How plated?"), "info"),
					Option(L("Leave him be"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten. Pace yourself. Don't face him half-ready."));
						await dialog.Msg(L("He charges with the stone shoulder. Stay off his right."));
						break;

					case "info":
						await dialog.Msg(L("Curse-plated flanks, stone along the spine, the original hide only showing on the belly."));
						await dialog.Msg(L("Ward-smiths will carve the plates for months. Pays both of us."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe next month. The bounty keeps climbing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMoya", out var packObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var alphaObj)) return;

				if (packObj.Done && alphaObj.Done)
				{
					await dialog.Msg(L("Plates intact. That's him. Ward-smiths will be at my door by noon."));
					await dialog.Msg(L("Bounty paid, plus my share."));

					character.Variables.Perm.Remove(alphaSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (packObj.Done && !alphaObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Bavon, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Moya are thin. Stallmaster's coming - I can hear him over the stalls."));
							await dialog.Msg(L("{#FF9966}Move. Don't let him retreat to the vaults.{/}"));
							character.ServerMessage(L("{#FF9966}The Stallmaster Alpha emerges from the vaults!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out. Hunt him down before he slips back."));
					}
				}
				else
				{
					await dialog.Msg(L("Moya are still in ranks. Thin them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Plates shipped to the ward-smiths. Three new charm-plates in the inner wards already."));
			}
		});

		// Quest 6: Market Perimeter
		//-------------------------------------------------------------------------
		AddNpc(155160, L("[Caravan Master] Korin"), "f_flash_60", 789, -392, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1006);

			dialog.SetTitle(L("Korin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Roxona perimeter is the only trail that skirts the deep curse-ground. Right now it's choked on both sides."));
				await dialog.Msg(L("Moya on the low side, Bavon in the alley-nests. Neither stops me alone. Both together does."));

				var response = await dialog.Select(L("Both species?"),
					Option(L("I'll clear both"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Use another route"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve of each. Moya spread low dust, Bavon launch from the alleys. Mind both."));
						await dialog.Msg(L("Clear them, and three caravans roll through by week's end."));
						break;

					case "info":
						await dialog.Msg(L("Moya shed curse-dust that lingers. Bavon hit and vanish. Whichever one's touching you is worse."));
						await dialog.Msg(L("Together they're impassable. Apart they're endurable."));
						break;

					case "leave":
						await dialog.Msg(L("Other routes cross deeper curse-ground. Half my crew would come home statued."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMoya", out var moyaObj)) return;
				if (!quest.TryGetProgress("killBavon", out var bavonObj)) return;

				if (moyaObj.Done && bavonObj.Done)
				{
					await dialog.Msg(L("Both sides thinned. Caravans roll at dawn."));
					await dialog.Msg(L("Take your pay. Drivers will know your name."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!moyaObj.Done)
						status += L("Kill more Moya. ");
					if (!bavonObj.Done)
						status += L("Kill more Bavon. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four caravans through already. Market's faint but present."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class MoyaScavengersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_60", 1001);
		SetName(L("Moya Scavengers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Moya grazing on curse-ground around the Roxona Market so the south arcade wards can be reset."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stallkeeper] Hedda"), "f_flash_60");

		AddObjective("killMoya", L("Kill curse-grazing Moya"),
			new KillObjective(22, new[] { MonsterId.Moya }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class SilveredCoresQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_60", 1002);
		SetName(L("Silvered Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("Crack Rootcrystals in Roxona Market and bring the silvered cores to the ward-jeweler."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ward-Jeweler] Talvi"), "f_flash_60");

		AddObjective("gatherCores", L("Gather silvered vein cores"),
			new CollectItemObjective(650317, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650317, 0.40f, MonsterId.Moya);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650317, character.Inventory.CountItem(650317), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650317, character.Inventory.CountItem(650317), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheVendorLedgersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_60", 1003);
		SetName(L("The Vendor Ledgers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bavon nesting in the market stall frames and recover four vendor ledgers for the archive."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Archivist] Brys"), "f_flash_60");

		AddObjective("killBavon", L("Kill Bavon"),
			new KillObjective(15, new[] { MonsterId.Bavon }));

		AddObjective("recoverLedgers", L("Recover vendor ledgers"),
			new CollectItemObjective(650475, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650475, 0.40f, MonsterId.Bavon);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650475, character.Inventory.CountItem(650475), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650475, character.Inventory.CountItem(650475), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheSaltisdaughterCabalQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_60", 1004);
		SetName(L("The Saltisdaughter Cabal"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Saltisdaughter Mages worshipping the curse and recover their curse-brands for the furnace."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Warden] Pavel"), "f_flash_60");

		AddObjective("killMages", L("Kill Saltisdaughter Mages"),
			new KillObjective(12, new[] { MonsterId.Saltisdaughter_Mage }));

		AddObjective("gatherBrands", L("Recover curse-brands"),
			new CollectItemObjective(650615, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650615, 0.50f, MonsterId.Saltisdaughter_Mage);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650615, character.Inventory.CountItem(650615), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650615, character.Inventory.CountItem(650615), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheStallmasterAlphaQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_60", 1005);
		SetName(L("The Stallmaster Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Moya pack to draw out the curse-plated Stallmaster, then bring him down."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Mira"), "f_flash_60");

		AddObjective("killMoya", L("Thin the Moya pack"),
			new KillObjective(10, new[] { MonsterId.Moya }));

		AddObjective("killAlpha", L("Defeat the Stallmaster Alpha"),
			new KillObjective(1, new[] { MonsterId.Bavon }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

public class MarketPerimeterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_60", 1006);
		SetName(L("Market Perimeter"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Moya and Bavon along the Roxona perimeter trail to reopen it to caravans."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Korin"), "f_flash_60");

		AddObjective("killMoya", L("Kill Moya"),
			new KillObjective(12, new[] { MonsterId.Moya }));

		AddObjective("killBavon", L("Kill Bavon"),
			new KillObjective(12, new[] { MonsterId.Bavon }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
