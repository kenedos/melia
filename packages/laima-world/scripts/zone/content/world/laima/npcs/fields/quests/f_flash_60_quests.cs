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
		AddNpc(20146, L("[Stallkeeper] Hedda"), "f_flash_60", -978, 1379, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1001);

			dialog.SetTitle(L("Hedda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Welcome to what's left of Roxona Market. Three generations of my family ran the south arcade. Now it's me, four wards, and the scavengers."));
				await dialog.Msg(L("The Moya graze on the cursed soil. It doesn't kill them - it just makes them carriers. Wherever they walk, the grey spreads a little further."));
				await dialog.Msg(L("Thin them and the soil gets a chance to recover. That's all I'm asking."));

				var response = await dialog.Select(L("Will you kill the Moya for us?"),
					Option(L("I'll handle the Moya"), "help"),
					Option(L("What happened here?"), "info"),
					Option(L("I'll come back"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two should buy us a season. Don't linger where one of them dies - the dust clings."));
						await dialog.Msg(L("If you want more work after, talk to Brys at the archive. He's trying to save the records."));
						break;

					case "info":
						await dialog.Msg(L("Curse swept the market at noon. People froze at their stalls. Bread on the boards, coins in their hands. They're still there."));
						await dialog.Msg(L("That was thirty years ago. Nobody comes to trade anymore. We just maintain."));
						break;

					case "leave":
						await dialog.Msg(L("I'll be here. So will the curse."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMoya", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The grey rings around their kill-spots are fading. The wards will hold another season."));
					await dialog.Msg(L("Take your pay. And a charm from the stall - it warms when curse-ground thickens. Cheap warning."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still too many grazing. Keep at them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("South arcade got a fresh coat of paint. First time since I was a girl."));
			}
		});

		// Quest 2: The Vendor Ledgers
		//-------------------------------------------------------------------------
		AddNpc(20120, L("[Archivist] Brys"), "f_flash_60", 299, -43, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1003);

			dialog.SetTitle(L("Brys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Hedda send you? Good. I need hands and she has a way of finding them."));
				await dialog.Msg(L("Every stallkeeper in Roxona kept a day-ledger. When the curse came, they tucked their ledgers into the stall frames - small thing to do, last thing they did."));
				await dialog.Msg(L("Bavon nest in the frames now. They tear the paper for bedding. Get me four ledgers before they're gone."));

				var response = await dialog.Select(L("Will you bring me the ledgers?"),
					Option(L("I'll bring you ledgers"), "help"),
					Option(L("Why bother?"), "info"),
					Option(L("Maybe later"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fifteen Bavon should quiet the main nests. Look for ledgers with carved initials on the spine - those are the keepers'."));
						await dialog.Msg(L("Don't grip the paper hard. It's been thirty years dry. It crumbles."));
						break;

					case "info":
						await dialog.Msg(L("Roxona isn't dead. It's stopped. If it ever starts again, it'll need to know what it used to be."));
						await dialog.Msg(L("My grandfather sold spices in stall thirty-eight. I want to know who else stood beside him."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. The paper isn't going anywhere fast - though it's going."));
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
					await dialog.Msg(L("Four. Two of them are partial matches to records I already have - I can complete the histories."));
					await dialog.Msg(L("Take your pay. I'll write your name in the margin when I transcribe."));

					character.Inventory.Remove(650475, character.Inventory.CountItem(650475), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Bavon need clearing. ");
					if (!ledgerObj.Done)
						status += L("More ledgers still in the frames. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("One of the ledgers names a spice merchant my grandmother bought from. I never knew his name. Now I do."));
			}
		});

		// Quest 3: The Saltisdaughter Cabal
		//-------------------------------------------------------------------------
		AddNpc(20128, L("[Curse-Warden] Pavel"), "f_flash_60", 267, 1668, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1004);

			dialog.SetTitle(L("Pavel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("There's a cabal calling itself the Saltisdaughter operating out of the cursed quarter. They aren't fighting the curse - they're worshipping it."));
				await dialog.Msg(L("Every one of their mages carries a brand-plate on the chest. Sigil burned into iron. It lets them channel a thread of the curse, on command."));
				await dialog.Msg(L("Twelve of them, five plates. The plates burn clean in the temple furnace. The rest is just stopping them before they grow."));

				var response = await dialog.Select(L("Will you bring me the plates?"),
					Option(L("I'll bring the plates"), "help"),
					Option(L("Who worships this?"), "info"),
					Option(L("Not my fight"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Plates are worn under the robes, against the breastbone. Wrap them - bare skin invites trouble."));
						await dialog.Msg(L("Furnace is ready when you are."));
						break;

					case "info":
						await dialog.Msg(L("People who think a god that turns flesh to stone is still a god worth following. I don't pretend to understand it. We just stop them."));
						await dialog.Msg(L("Same cabal's working in Ruklys and the Enceinte. Inspectors are building a case across districts."));
						break;

					case "leave":
						await dialog.Msg(L("They won't leave us alone. So we can't leave them alone either. Reconsider."));
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
					await dialog.Msg(L("Five plates. Furnace takes them tonight - the smoke goes blue when they burn properly. That's how I'll know."));
					await dialog.Msg(L("Pay's yours. Wrap any stiffness you've picked up before you sleep."));

					character.Inventory.Remove(650615, character.Inventory.CountItem(650615), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Saltisdaughter Mages out there. ");
					if (!brandObj.Done)
						status += L("More plates to recover. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Smoke went blue. Five plates dead. Inspector Thane in Ruklys says my burn lines up with his evidence - case is firming up."));
			}
		});

		// Quest 4: The Stallmaster Alpha
		//-------------------------------------------------------------------------
		AddNpc(147509, L("[Bounty Hunter] Mira"), "f_flash_60", -896, -998, 44, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_flash_60.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Mira"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("You hunt? I have a contract nobody else wants."));
				await dialog.Msg(L("There's a Bavon grew up in the market vaults. Curse warped it instead of killing it - twice the size, half stone-plate, the rest bad temper. Lesser Moya defer to it like it's a stallmaster."));
				await dialog.Msg(L("Drop ten Moya and it'll come out swinging. Bounty's on the plates. Wardmakers will pay top coin."));

				var response = await dialog.Select(L("So? Want the bounty?"),
					Option(L("I'll take the contract"), "help"),
					Option(L("How tough?"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten Moya first. Don't engage him fresh - if you're tired, his stone shoulder will end you."));
						await dialog.Msg(L("He charges right. Stay left."));
						break;

					case "info":
						await dialog.Msg(L("Curse-plates along the spine, original hide on the belly. Fast, for what he is."));
						await dialog.Msg(L("He's killed three hunters. Take that as you will."));
						break;

					case "leave":
						await dialog.Msg(L("Bounty climbs. I'll be here."));
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
					await dialog.Msg(L("Plates intact. That's him. Wardmakers will be at my door before noon."));
					await dialog.Msg(L("Bounty's yours, plus my cut. I wasn't going to take him myself anytime this year."));

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
							await dialog.Msg(L("Hear that crash? That's him. Pack's gone, he's got nothing to hide behind."));
							await dialog.Msg(L("{#FF9966}Move - keep him out of the vaults.{/}"));
							character.ServerMessage(L("{#FF9966}The Stallmaster Alpha emerges from the vaults!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's loose. Don't let him slip back into the vaults - he heals up in there."));
					}
				}
				else
				{
					await dialog.Msg(L("Pack's still tight. He won't show. Keep thinning."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Plates are at the wardmakers. Three new charm-plates lit in the inner ring already."));
			}
		});

		// Quest 5: Market Perimeter
		//-------------------------------------------------------------------------
		AddNpc(20156, L("[Caravan Master] Korin"), "f_flash_60", 789, -392, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_60", 1006);

			dialog.SetTitle(L("Korin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I run caravans through here when I can. Right now I can't."));
				await dialog.Msg(L("Moya graze the low side, Bavon launch from the alleys. Each on its own I can drive past. Both together turns the road into a kill-zone."));
				await dialog.Msg(L("Thin them. Three caravans are sitting in town with full holds and nowhere to go."));

				var response = await dialog.Select(L("Will you clear both sides of the road?"),
					Option(L("I'll handle both sides"), "help"),
					Option(L("Why this road?"), "info"),
					Option(L("Take a different route"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve and twelve. Watch the alley mouths - that's where Bavon launch from."));
						await dialog.Msg(L("Get a charm from Hedda before you go. She'll hand them out for nothing if you mention me."));
						break;

					case "info":
						await dialog.Msg(L("Other roads cross the inner curse. Lost a driver and his team there last spring. Found the team. Sort of."));
						await dialog.Msg(L("This road or no road. That's what it comes down to."));
						break;

					case "leave":
						await dialog.Msg(L("I'll be here. The carts will too, until they aren't."));
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
					await dialog.Msg(L("Both sides quiet enough. First cart rolls at first light."));
					await dialog.Msg(L("Pay's yours. Drivers will know your name by next month."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!moyaObj.Done)
						status += L("More Moya on the low side. ");
					if (!bavonObj.Done)
						status += L("More Bavon in the alleys. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four caravans through this week. One driver left a flask on the bench for you - I'm holding it."));
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
		SetDescription(L("Hedda's wards are slipping as the Moya graze cursed dust deeper into the market. Thin them out so the soil can recover."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stallkeeper] Hedda"), "f_flash_60");

		AddObjective("killMoya", L("Kill cursed Moya"),
			new KillObjective(22, new[] { MonsterId.Moya }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class TheVendorLedgersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_60", 1003);
		SetName(L("The Vendor Ledgers"));
		SetType(QuestType.Sub);
		SetDescription(L("Brys is trying to reconstruct Roxona's market history. Clear the Bavon nesting in the stall frames and recover four vendor ledgers."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Archivist] Brys"), "f_flash_60");

		AddObjective("killBavon", L("Kill Bavon nesting in stall frames"),
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
		SetDescription(L("A curse-worshipping cabal works out of Roxona. Kill twelve of their mages and burn their five brand-plates in Pavel's furnace."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Warden] Pavel"), "f_flash_60");

		AddObjective("killMages", L("Kill Saltisdaughter Mages"),
			new KillObjective(12, new[] { MonsterId.Saltisdaughter_Mage }));

		AddObjective("gatherBrands", L("Recover brand-plates"),
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
		SetDescription(L("Mira has a contract on a curse-warped Bavon ruling the market vaults. Thin the Moya pack first; the Stallmaster won't show himself otherwise."));
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
		SetDescription(L("Korin can't roll caravans until both perimeter flanks are thinned. Clear the Moya on the low side and the Bavon launching from the alleys."));
		SetLocation("f_flash_60");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Korin"), "f_flash_60");

		AddObjective("killMoya", L("Kill Moya on the low side"),
			new KillObjective(12, new[] { MonsterId.Moya }));

		AddObjective("killBavon", L("Kill Bavon in the alleys"),
			new KillObjective(12, new[] { MonsterId.Bavon }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
