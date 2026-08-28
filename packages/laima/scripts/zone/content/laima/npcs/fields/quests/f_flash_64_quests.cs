//--- Melia Script ----------------------------------------------------------
// Enceinte District Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed quests for the inner wall district of Roxona.
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

public class FFlash64QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Lemuria Swarm
		//-------------------------------------------------------------------------
		AddNpc(20142, L("[Kalis Knights] Quartermaster Dovas"), "f_flash_64", 556, 341, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1001);

			dialog.SetTitle(L("Dovas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's counting crates against a manifest, lips moving with the numbers*{/}"));
				await dialog.Msg(L("Give me a moment — lose count of these and I start the whole row over, and I will start over, I've done it four times today already. I run supply for whatever's left of the Knights in this wall: mostly me, two carts, and a list so short I've got it memorized twice over."));
				await dialog.Msg(L("Lemuria are stripping the east yards faster than I can log the losses — rope, canvas, dried stores, anything with a handle they can drag off. Kill 22 of them and my carts can finish one blessed run without an escort, for once."));

				var response = await dialog.Select(L("Will you thin them out for me?"),
					Option(L("I'll kill the Lemuria"), "help"),
					Option(L("Why not just move the stores?"), "info"),
					Option(L("Not today"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They work the east yards in threes. Break the third one off and the other two lose their nerve."));
						break;

					case "info":
						await dialog.Msg(L("Move them where, exactly? Everything past the inner wall is grey. Write it down if you like: this district is the last dry storehouse Roxona has left."));
						await dialog.Msg(L("If the stores go, the Knights go with them, and then there's nobody holding this line at all — and I will have failed to restock a list, which, believe it or not, keeps me up at night."));
						break;

					case "leave":
						await dialog.Msg(L("Fine. My carts go out short-handed either way. I keep a tally of disappointments too, if you're curious — you'd be quite far down the page."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLemuria", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Both carts came back loaded this morning. No escort, no losses, nothing chewed — I checked twice, I always check twice."));
					await dialog.Msg(L("That's your pay. Rounded up, because for once the ledger actually allows it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still losing rope off the east yards, and I've had to recount the manifest twice because of it. Go finish the job."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Restocked the whole list. Every last item, even the canvas — first time since the curse came down, and I have crossed it off in ink, not pencil."));
			}
		});

		// Quest 1002: Names on the Wall
		//-------------------------------------------------------------------------
		AddNpc(20106, L("[Kalis Knights] Wilhelmina Karriat"), "f_flash_64", -366, -1320, 287, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1002);

			dialog.SetTitle(L("Wilhelmina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's folding a blank letter closed, then unfolding it again, unsatisfied*{/}"));
				await dialog.Msg(L("You're not one of mine, are you. Good. My own soldiers can't stand to look at this wall anymore, and I can't say I blame them. The garrison that held this stretch is still standing on it. All of them. Grey to the boot buckles, and not one of them ever given the order to fall back."));
				await dialog.Msg(L("We cannot move them. We cannot bury them. But every one carries something — a ring, a token, a folded letter never sent. Bring me 4, and I can finally write 4 families something true, instead of the blank pages I've been staring at for a week."));

				var response = await dialog.Select(L("Will you collect the mementos?"),
					Option(L("I'll bring the mementos"), "help"),
					Option(L("Can't they be thawed?"), "info"),
					Option(L("Let them rest"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They stand along the wall walk south of the shrine. Take only what comes loose — if it has fused into the stone, leave it. I will not have you breaking what's left of them for a keepsake."));
						break;

					case "info":
						await dialog.Msg(L("Saliamonas believes so, eventually. One success, forty failures, and the failures leave nothing behind to bury. I have stopped counting on it."));
						await dialog.Msg(L("So we write the first letter now. If the thawing ever succeeds, I will write a second, gladly — but I am done waiting on hope to do a clerk's job."));
						break;

					case "leave":
						await dialog.Msg(L("They would rest easier knowing their families were told. That is the whole of it, and I will not dress it up as anything grander."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectMementos", out var memObj)) return;

				if (memObj.Done)
				{
					await dialog.Msg(L("Three names. Two of them brothers — the same ring, one size apart. I had to sit with that a moment before I could write it down."));
					await dialog.Msg(L("Take your payment. I'll be at this desk until every letter is finished, however long that takes."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still more of them out on the walk. Keep going south along the wall."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The letters went out with the Fedimian courier. One family had already held a funeral without a body. They're holding a second one now — properly, this time, with a name and a ring to bury."));
			}
		});

		// Quest 1002 collection points - petrified wall guards along the walk
		//-------------------------------------------------------------------------
		void AddPetrifiedGuard(int guardNum, int modelId, int x, int z, int direction)
		{
			AddNpc(modelId, L("Petrified Wall Guard"), "f_flash_64", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_flash_64", 1002);
				var variableKey = $"Laima.Quests.f_flash_64.Quest1002.Guard{guardNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A wall guard turned to grey stone mid-stride, spear still braced*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already took what was loose on this one*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Searching the guard..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(660003, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Soldier's Memento"));

					var currentCount = character.Inventory.CountItem(660003);
					character.ServerMessage(LF("Mementos recovered: {0}/3", currentCount));

					if (currentCount >= 3)
						character.ServerMessage(L("{#FFD700}That's three letters worth. Return to Wilhelmina Karriat.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the guard undisturbed."));
				}
			});
		}

		AddPetrifiedGuard(1, 154026, 13, -387, 0);
		AddPetrifiedGuard(2, 154026, -194, -221, 96);
		AddPetrifiedGuard(3, 154023, 124, -518, 201);
		AddPetrifiedGuard(4, 154029, -91, -738, 58);

		// Quest 1003: The Thawing Liquid
		//-------------------------------------------------------------------------
		AddNpc(154022, L("[Kalis Knights] Alchemist Saliamonas"), "f_flash_64", 29, 357, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1003);

			dialog.SetTitle(L("Saliamonas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's decanting a cloudy liquid between two flasks, hands steady despite the smell*{/}"));
				await dialog.Msg(L("Stand clear of the fumes, unless you fancy losing your eyebrows! Forty-one failures and exactly one success behind me — and the success, delightfully, was a dog. I have been very, very careful about what I say to people since."));
				await dialog.Msg(L("But the liquid works best on the recently turned, and wouldn't you know it, Amanda's diggers went grey in the west yards only last month! Take a flask, pour it on 4 of them, and we shall finally learn whether that dog was luck or genuine method."));

				var response = await dialog.Select(L("Will you carry the flask out there?"),
					Option(L("I'll pour the liquid"), "help"),
					Option(L("What happened to the forty-one?"), "info"),
					Option(L("That's a lot of failures"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(660006, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Pour it at the base of the neck — never the face, the face is precisely where all forty-one went catastrophically wrong."));
						await dialog.Msg(L("And do keep your head up while you work! The west yards belong to the Lepusbunnies now, and they do so love an unattended back."));
						break;

					case "info":
						await dialog.Msg(L("They crumbled! Not all at once, mind — over about a minute, spreading out from wherever the liquid first touched. Rather fascinating, in a way I try not to enjoy too openly."));
						await dialog.Msg(L("Their families were told it was simply the curse finishing its work. My decision, that, and I stand by it. Mostly."));
						break;

					case "leave":
						await dialog.Msg(L("A lot of failures, yes! Also forty-one more honest attempts than anyone else in this kingdom has bothered making. I choose to find that admirable."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thawDiggers", out var thawObj)) return;

				if (thawObj.Done)
				{
					await dialog.Msg(L("Four treated, and one of them steamed instead of cracking, you say? Steam! That is precisely what the dog did!"));
					await dialog.Msg(L("Take your pay — quickly, please, I need to write this down before I manage to convince myself I imagined the whole thing."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More of them still standing out there. The flask holds plenty - keep pouring."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("One of the four has soft hands again! Not awake — not yet — but soft, undeniably soft. Amanda comes by every morning to check, and I let her believe it's for her sake."));
			}
		});

		// Quest 1003 interaction points - petrified grave-robbers in the west yards
		//-------------------------------------------------------------------------
		void AddPetrifiedDigger(int diggerNum, int modelId, int x, int z, int direction)
		{
			AddNpc(modelId, L("Petrified Grave-Robber"), "f_flash_64", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_flash_64", 1003);
				var variableKey = $"Laima.Quests.f_flash_64.Quest1003.Digger{diggerNum}";
				var counterKey = "Laima.Quests.f_flash_64.Quest1003.DiggersThawed";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A grave-robber turned to grey stone, one hand still on a shovel haft*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already treated this one. The stone is damp where the liquid ran*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 400, 350);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The smell of the flask carries - {0} coming in behind you!{{/}}", luredCount));

				var result = await character.TimeActions.StartAsync(
					L("Pouring the thawing liquid..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var thawed = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, thawed);
					character.ServerMessage(LF("Grave-robbers treated: {0}/4", thawed));

					if (thawed >= 4)
						character.ServerMessage(L("{#FFD700}That's four. Return to Alchemist Saliamonas.{/}"));
				}
				else
				{
					character.ServerMessage(L("You cap the flask and step back."));
				}
			});
		}

		AddPetrifiedDigger(1, 154030, -1529, -517, 0);
		AddPetrifiedDigger(2, 154024, -1180, -516, 0);
		AddPetrifiedDigger(3, 154024, -1495, -19, 0);
		AddPetrifiedDigger(4, 154024, -1236, 539, 0);
		AddPetrifiedDigger(5, 154031, -871, 396, 0);
		AddPetrifiedDigger(6, 154030, -706, 760, 0);

		// Quest 1004: The Saltisdaughter Archers
		//-------------------------------------------------------------------------
		AddNpc(154019, L("[Kalis Knights] Bokor Edita"), "f_flash_64", -444, 358, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1004);

			dialog.SetTitle(L("Edita"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's kneeling over a scorched altar stone, tracing the burn pattern with one finger*{/}"));
				await dialog.Msg(L("Don't touch it. The ash is still warm — that means recent, that means close. I've traced the Saltisdaughter cabal through three districts now: burned plates in the market, brands on livestock, charms cut from the dead. The trail ends here, and I intend to end it here."));
				await dialog.Msg(L("Their archers hold the north yards. Each one carries a dark crystal feeding the curse outward. Kill 15, bring me 5 crystals, and the whole chain goes cold — tonight, if you move fast enough."));

				var response = await dialog.Select(L("Will you break the chain?"),
					Option(L("I'll bring the crystals"), "help"),
					Option(L("Who are the Saltisdaughters?"), "info"),
					Option(L("That's an army's job"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't hold a crystal longer than you must. They pull the warmth out of a hand fast."));
						await dialog.Msg(L("They shoot from high ground. Close the distance or don't bother wasting the arrows on you."));
						break;

					case "info":
						await dialog.Msg(L("Curse-worshippers. Organized ones. They believe petrification is a blessing, and that they are its instrument."));
						await dialog.Msg(L("They began as a handful of Roxona merchants who wanted the curse to stop at their own street. It did not stay that small, and neither did their ambitions."));
						break;

					case "leave":
						await dialog.Msg(L("The army wrote this district off two seasons ago. It's a Knight's job now, and I happen to be the Knight standing here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCrystals", out var crystalObj)) return;

				if (killObj.Done && crystalObj.Done)
				{
					await dialog.Msg(L("Five crystals. The north yards have gone quiet in a way they haven't in a year — I intend to make sure it stays that way."));
					await dialog.Msg(L("Payment's yours. The crystals go into a sealed box tonight, a Fedimian vault by week's end, and nowhere near daylight again."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Saltisdaughter Archers still on the high ground. ");
					if (!crystalObj.Done)
						status += L("More dark crystals still to recover. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The cabal has gone to ground. Not finished - gone to ground. There's a difference, and I intend to find out how big it is."));
			}
		});

		// Quest 1005: The Gargoyle of the Inner Wall
		//-------------------------------------------------------------------------
		AddNpc(153040, L("[Crew Boss] Amanda"), "f_flash_64", -586, 1560, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1005);

			dialog.SetTitle(L("Amanda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's staring west toward the wall footing, arms crossed like she's bracing against something*{/}"));
				await dialog.Msg(L("You picked a rotten afternoon to wander over. Come here anyway. Six of my people are standing grey in the west yards, and it's on me — I sent them to dig a footing I'd already been warned about, and I sent them anyway."));
				await dialog.Msg(L("Whatever warned me is still sitting up there — a Gargoyle, been on that wall so long people mistake it for masonry. Kill 10 Rubabos off the footing and it'll come down off the ledge. I want it dead before I risk one more person out here. Not one more."));

				var response = await dialog.Select(L("So? Will you take the contract?"),
					Option(L("I'll take the contract"), "help"),
					Option(L("Steponas says you were stone"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It drops instead of charging. Hear grit coming off the wall above you, you move first and you look after — don't stand there thinking about it."));
						break;

					case "info":
						await dialog.Msg(L("Steponas tells that story so people quit asking him for a cut of his digging. I'm alive, I'm standing right here, and he still owes me for two carts."));
						await dialog.Msg(L("Tell him I said the ledger's still open, and it's not closing on his say-so."));
						break;

					case "leave":
						await dialog.Msg(L("Suit yourself. It isn't going anywhere, which is rather the whole problem with it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearFooting", out var footObj)) return;
				if (!quest.TryGetProgress("killGargoyle", out var bossObj)) return;

				if (footObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It came off the wall and stayed off. Footing's diggable now, and my crew — what's left of them able to dig — can finally be brought home."));
					await dialog.Msg(L("Full contract, and this on top. Pulled it out of a Roxona cellar years back, been sitting in a crate since. Better it does something useful."));

					character.Quests.Complete(questId);
				}
				else if (footObj.Done)
				{
					await dialog.Msg(L("It's moving. Get back to the footing before it settles onto the ledge again."));
				}
				else
				{
					await dialog.Msg(L("The Rubabos are still thick on the footing. It won't stir for a quiet yard."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Saliamonas has two of my twelve going soft under the canvas. I sit with them in the mornings. It's not much, but it's more than I had."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Lemuria Swarm
//-----------------------------------------------------------------------------

public class TheLemuriaSwarmQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1001);
		SetName(L("The Lemuria Swarm"));
		SetType(QuestType.Sub);
		SetDescription(L("Lemuria are stripping the Knights' east supply yards faster than Quartermaster Dovas can restock them. Thin the swarm so his carts can run unescorted."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kalis Knights] Quartermaster Dovas"), "f_flash_64");

		AddObjective("killLemuria", L("Kill Lemuria in the east supply yards"),
			new KillObjective(22, new[] { MonsterId.Lemuria }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1002 CLASS: Names on the Wall
//-----------------------------------------------------------------------------

public class NamesOnTheWallQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1002);
		SetName(L("Names on the Wall"));
		SetType(QuestType.Sub);
		SetDescription(L("The garrison that held the inner wall is still standing on it, petrified where it stood. Search the wall walk and bring Wilhelmina Karriat their mementos so their families can be written to."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kalis Knights] Wilhelmina Karriat"), "f_flash_64");

		AddObjective("collectMementos", L("Recover mementos from the petrified wall guards"),
			new CollectItemObjective(660003, 3));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(660003, character.Inventory.CountItem(660003), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 6; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_64.Quest1002.Guard{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(660003, character.Inventory.CountItem(660003), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 6; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_64.Quest1002.Guard{i}");
	}
}

// Quest 1003 CLASS: The Thawing Liquid
//-----------------------------------------------------------------------------

public class TheThawingLiquidQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1003);
		SetName(L("The Thawing Liquid"));
		SetType(QuestType.Sub);
		SetDescription(L("Alchemist Saliamonas has one success and forty-one failures behind his petrification cure. Carry his flask into the west yards and treat the grave-robbers who went grey last month."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kalis Knights] Alchemist Saliamonas"), "f_flash_64");

		AddObjective("thawDiggers", L("Treat petrified grave-robbers in the west yards"),
			new VariableCheckObjective("Laima.Quests.f_flash_64.Quest1003.DiggersThawed", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(660006, character.Inventory.CountItem(660006), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_flash_64.Quest1003.DiggersThawed");

		for (var i = 1; i <= 12; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_64.Quest1003.Digger{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(660006, character.Inventory.CountItem(660006), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_flash_64.Quest1003.DiggersThawed");

		for (var i = 1; i <= 12; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_64.Quest1003.Digger{i}");
	}
}

// Quest 1004 CLASS: The Saltisdaughter Archers
//-----------------------------------------------------------------------------

public class TheSaltisdaughterArchersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1004);
		SetName(L("The Saltisdaughter Archers"));
		SetType(QuestType.Sub);
		SetDescription(L("Bokor Edita has traced the Saltisdaughter cabal across three districts to the north yards of the Enceinte. Kill their archers and recover the dark crystals feeding the curse outward."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kalis Knights] Bokor Edita"), "f_flash_64");

		AddObjective("killArchers", L("Kill Saltisdaughter Archers"),
			new KillObjective(15, new[] { MonsterId.Saltisdaughter_Bow }));

		AddObjective("gatherCrystals", L("Recover Dark Crystals"),
			new CollectItemObjective(660004, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(660004, 0.50f, MonsterId.Saltisdaughter_Bow);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(660004, character.Inventory.CountItem(660004), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(660004, character.Inventory.CountItem(660004), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Gargoyle of the Inner Wall
//-----------------------------------------------------------------------------

public class TheGargoyleOfTheInnerWallQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1005);
		SetName(L("The Gargoyle of the Inner Wall"));
		SetType(QuestType.Sub);
		SetDescription(L("A Gargoyle has sat on the west wall footing so long that people take it for masonry, and Amanda's crew went grey beneath it. Clear the Rubabos off the footing to bring it down, then kill it."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Crew Boss] Amanda"), "f_flash_64");

		AddObjective("clearFooting", L("Kill Rubabos on the west wall footing"),
			new KillObjective(10, new[] { MonsterId.Rubabos }));

		AddObjective("killGargoyle", L("Defeat the Gargoyle"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Gargoyle, 1) },
				resetIdent: "clearFooting",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(203202, 1)); // Vieretta Mace
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
