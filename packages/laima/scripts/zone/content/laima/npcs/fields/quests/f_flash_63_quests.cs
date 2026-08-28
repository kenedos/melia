//--- Melia Script ----------------------------------------------------------
// Downtown Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed quests for the Downtown ruins.
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

public class FFlash63QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Roosting Floor
		//-------------------------------------------------------------------------
		AddNpc(20141, L("[Kingdom Army] Rofhdel"), "f_flash_63", 936, 920, 273, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1001);

			dialog.SetTitle(L("Rofhdel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's marking names off a tally board without looking up*{/}"));
				await dialog.Msg(L("Great, another straggler. Or are you actually good for something? Nine of us left holding half of Downtown, and I haven't got the patience left to find out which one you are the slow way."));
				await dialog.Msg(L("Lemurs took the upper floors of the counting house and they scream all night — every night, like clockwork, the universe's idea of a joke. Two of my nine haven't slept in a week. Kill 22 of them and I get my watch rotation back, and maybe my sense of humor with it."));

				var response = await dialog.Select(L("Will you clear the roosts for me?"),
					Option(L("I'll kill the Lemurs"), "help"),
					Option(L("Nine soldiers? That's all?"), "info"),
					Option(L("Not right now"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the stairwells, not the street. They drop on you from the ledges out there."));
						break;

					case "info":
						await dialog.Msg(L("We had forty when the curse came down off the plateau. Eleven greyed over standing at their posts — didn't even get to sit down first, lucky them. The rest walked, and I don't blame them one bit."));
						await dialog.Msg(L("The relief column keeps not arriving. That's the whole story of this district, really. Write it on my tombstone, save everyone the trouble later."));
						break;

					case "leave":
						await dialog.Msg(L("Suit yourself. We'll still be here — awake, miserable, waiting — whenever your nerve catches up to you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLemurs", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Quiet up there, finally. My night watch heard nothing but rain last night, and one of them actually cried about it. Grown man. Tears. Over rain."));
					await dialog.Msg(L("Pay's yours. Take a drink from the post barrel on your way out — you've earned it more than half this garrison has."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still screaming up on the counting house floors. Go finish it before I lose what's left of my nerves along with theirs."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Rotation's running properly again — four on, eight off. First good sleep this garrison's had since spring, and I intend to enjoy every miserable hour of it."));
			}
		});

		// Quest 1002: Bills of the Relief Column
		//-------------------------------------------------------------------------
		AddNpc(20128, L("[Kalis Knights] Adjutant Hans"), "f_flash_63", 160, -1097, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1002);

			dialog.SetTitle(L("Hans"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's holding a torn bill flat against a crate, comparing it line by line to one already in his hand*{/}"));
				await dialog.Msg(L("You'll want to look at this too, then. A stranger's eye counts for something, in matters like these. The Kingdom Army has bills nailed to every board in this quarter — a promised relief column, a full reclamation of Downtown by winter, signed and sealed."));
				await dialog.Msg(L("There is no column. I have checked the muster rolls in Fedimian myself, in person, twice. Families are sitting in cursed houses on the strength of a lie, and every week a few more of them grey over waiting for it to arrive. Pull down 7 of those bills and bring them to me — I intend to file every one as evidence."));

				var response = await dialog.Select(L("Will you take the bills down?"),
					Option(L("I'll bring you the bills"), "help"),
					Option(L("Why not just tell Rofhdel?"), "info"),
					Option(L("That's between soldiers"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The boards are along the old market run, south of the army post. Peel them off whole — the seal must remain intact when I present one before a magistrate. A torn seal is worth nothing in a court of law."));
						await dialog.Msg(L("And watch yourself while you work. The Lemurs down there have learned that a person standing still is a person not looking up, and I would rather not add your name to a different kind of file."));
						break;

					case "info":
						await dialog.Msg(L("I did. He did not write them — someone above him did, and Rofhdel is too exhausted and too loyal to argue with an official seal."));
						await dialog.Msg(L("So I will argue with it in his place. That, precisely, is what an adjutant is for."));
						break;

					case "leave":
						await dialog.Msg(L("It stopped being soldier's business the day civilians began believing them. Now it is a matter of record, and record-keeping happens to be my business."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectBills", out var billObj)) return;

				if (billObj.Done)
				{
					await dialog.Msg(L("Seals intact, every one. That is sufficient to place before the magistrate's court in Fedimian, and I intend to do exactly that the moment the ink dries on my report."));
					await dialog.Msg(L("Payment comes from the Knights' own purse, not the Army's. I would rather it came from us — call it a matter of principle."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still bills on the boards. Continue down the market run — there are more of them than the Army would like anyone counting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two families packed up and left for Orsha the very day the boards went bare. Two families who will not grey over on the strength of a lie. I consider that a conviction, whether or not any court ever sees the file."));
			}
		});

		// Quest 1002 collection points - Royal Army bills on the market-run boards
		//-------------------------------------------------------------------------
		void AddPropagandaBoard(int boardNum, int x, int z, int direction)
		{
			AddNpc(154038, L("Royal Army Bill"), "f_flash_63", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_flash_63", 1002);
				var variableKey = $"Laima.Quests.f_flash_63.Quest1002.Board{boardNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A wax-sealed army bill nailed to a market board, promising relief by winter*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already stripped this board*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 350, 300);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}Something drops off the ledges - {0} closing in!{{/}}", luredCount));

				var result = await character.TimeActions.StartAsync(
					L("Peeling the bill loose..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(664013, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Royal Army Propaganda"));

					var currentCount = character.Inventory.CountItem(664013);
					character.ServerMessage(LF("Bills recovered: {0}/5", currentCount));

					if (currentCount >= 5)
						character.ServerMessage(L("{#FFD700}That's enough for the court. Return to Adjutant Hans.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the bill on the board."));
				}
			});
		}

		AddPropagandaBoard(1, 460, -551, 0);
		AddPropagandaBoard(2, 338, -523, 0);
		AddPropagandaBoard(3, 294, -595, 0);
		AddPropagandaBoard(4, 300, -928, 0);
		AddPropagandaBoard(5, 157, -776, 0);
		AddPropagandaBoard(6, 355, -1053, 0);

		// Quest 1003: What the Chargers Took
		//-------------------------------------------------------------------------
		AddNpc(20103, L("[Grave-Robber] Steponas"), "f_flash_63", -258, 300, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1003);

			dialog.SetTitle(L("Steponas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He freezes with a crowbar half-wedged under a cellar door, then relaxes when he sees you're no Knight*{/}"));
				await dialog.Msg(L("Oh — not the watch! Ha, good, you had me worried for a second there. Look, before you say anything: yes, I dig up other people's cellars, and no, I don't feel bad about it. Everyone down here's dead or gone, and silver's never once cared whose pocket it ends up in."));
				await dialog.Msg(L("Only trouble is some Goblin Chargers found my cache under the bathhouse and hauled off six whole bags of it — cheeky little thieves, stealing from a thief! Kill 15 of them, bring me back 4 bags, and I'll cut you in. Honest coin, dishonest silver — funny how that works out."));

				var response = await dialog.Select(L("Will you get my bags back?"),
					Option(L("I'll get your bags"), "help"),
					Option(L("Who do you dig for?"), "info"),
					Option(L("Find your own bags"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They nest west of the bathhouse steps. Look for the one running heavy with a limp — that's your bag-carrier, hehe."));
						break;

					case "info":
						await dialog.Msg(L("Amanda runs the crew, out past the north gate in the Enceinte. Lost twelve people under a wall footing last month, poor thing, hasn't been right since."));
						await dialog.Msg(L("Me? I stick to boring cellars and quiet corpses. Boring's how you keep your skin on, friend — remember that."));
						break;

					case "leave":
						await dialog.Msg(L("Suit yourself! The bags aren't going anywhere either way — goblins don't spend silver, they just sit on it like fat little dragons. It'll keep."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killChargers", out var killObj)) return;
				if (!quest.TryGetProgress("recoverBags", out var bagObj)) return;

				if (killObj.Done && bagObj.Done)
				{
					await dialog.Msg(L("Four bags! And two of them still sealed with wax — meaning nobody's pawed through them, hehe, lucky us."));
					await dialog.Msg(L("Here's your cut. The real cut, mind you, not the one I'd normally try to talk you down to."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Goblin Chargers still nesting by the steps. ");
					if (!bagObj.Done)
						status += L("More bags still out there. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Bought myself a proper lockbox with the last bag and buried it somewhere no goblin will ever sniff out. Probably. Hehe."));
			}
		});

		// Quest 1004: Faded Stones
		//-------------------------------------------------------------------------
		AddNpc(20121, L("[Curse-Scholar] Vaida"), "f_flash_63", 98, -130, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1004);

			dialog.SetTitle(L("Vaida"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She lowers a spyglass from the ruined windowsill and looks at you like you're a new variable*{/}"));
				await dialog.Msg(L("You walk steady, for someone standing this close to a petrification district. Interesting. I've logged the Goblin Magicians through this glass for three days running. Small grey stones, worn at the belt. Not rocks. Fragments of people, catalogued and carried like trophies."));
				await dialog.Msg(L("Someone is breaking up the petrified and distributing the fragments as charms. Unacceptable, and also, professionally, fascinating. Kill 12 of the Magicians, bring me 5 stones, and I will read the grain to determine who they were."));

				var response = await dialog.Select(L("Will you bring me the stones?"),
					Option(L("I'll bring the stones"), "help"),
					Option(L("You can read a person from stone?"), "info"),
					Option(L("That's grim work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Wrap them in cloth before pocketing. Bare skin against a faded stone numbs the fingers for roughly a day — I've tested this personally, more than once."));
						break;

					case "info":
						await dialog.Msg(L("The curse fixes a subject mid-moment. The grain preserves that moment precisely — the direction of a turn, the position of the hands."));
						await dialog.Msg(L("It is how the Saltisdaughter cabal was traced through the market district. Same method. Worse district, worse results."));
						break;

					case "leave":
						await dialog.Msg(L("Grim, yes. Correctly noted. But left unread, they remain rocks in perpetuity, which I consider the greater failure."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMagicians", out var killObj)) return;
				if (!quest.TryGetProgress("gatherStones", out var stoneObj)) return;

				if (killObj.Done && stoneObj.Done)
				{
					await dialog.Msg(L("Five stones. Five names, by tomorrow night. Three from the same household — the grain runs identically in all three, which is its own small tragedy."));
					await dialog.Msg(L("Take your payment. I have letters to write, and I do not expect to enjoy it."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Goblin Magicians still carrying charms. ");
					if (!stoneObj.Done)
						status += L("More faded stones still to recover. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four of the five had family in Orsha. The letters went out with the Knights' courier. One family wrote back. I filed the reply. I did not expect to keep rereading it."));
			}
		});

		// Quest 1005: The Stone Froster
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Kestas"), "f_flash_63", -113, 1407, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1005);

			dialog.SetTitle(L("Kestas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He looks you over once, head to boots, the way he'd size up a bounty board*{/}"));
				await dialog.Msg(L("You'll do. There's a thing living in the old cistern under the north block — big, slow, and cold enough that the air in front of it snows in summer."));
				await dialog.Msg(L("The Stone Froster, they call it. What it breathes on frosts first and greys after — the curse, walking around on legs like it owns the place. Kill 10 Goblin Chargers by the cistern mouth, make a racket, and it'll come up to see who's disturbing its nap."));

				var response = await dialog.Select(L("So? Want the contract?"),
					Option(L("I'll take the contract"), "help"),
					Option(L("Where did it come from?"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't stand in the white air. Looks like fog, isn't fog — that's the front edge of its breath, and it'll cost you fingers."));
						await dialog.Msg(L("Keep moving, keep it in front of you. Slow and cold doesn't mean stupid, but it does mean it can't turn worth a damn."));
						break;

					case "info":
						await dialog.Msg(L("Nobody knows, and I've stopped asking. The scholars figure the curse settled into something already down there and grew itself a body."));
						await dialog.Msg(L("Kill it clean and Vaida gets a whole specimen to study instead of scraps off a belt. Worth more than my fee, honestly — not that I'll be telling her that."));
						break;

					case "leave":
						await dialog.Msg(L("The bounty climbs every month nobody takes it. I'll be right here when the number finally tempts you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("drawItOut", out var drawObj)) return;
				if (!quest.TryGetProgress("killStoneFroster", out var bossObj)) return;

				if (drawObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It's down. Cistern's dripping instead of frosting — first good sign this block's had in a year, and I'll take credit for all of it."));
					await dialog.Msg(L("Full bounty. And this — came off a Knight who didn't need it anymore. Better worn than sitting in a drawer gathering dust."));

					character.Quests.Complete(questId);
				}
				else if (drawObj.Done)
				{
					await dialog.Msg(L("It heard you. Get back to the cistern mouth before it settles again."));
				}
				else
				{
					await dialog.Msg(L("Not enough noise yet. It won't climb for a scuffle."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Vaida's got the carcass under canvas and hasn't slept in two days. Says the grain runs clean through it. I don't know what that means and I've decided I don't want to."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Roosting Floor
//-----------------------------------------------------------------------------

public class TheRoostingFloorQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1001);
		SetName(L("The Roosting Floor"));
		SetType(QuestType.Sub);
		SetDescription(L("Lemurs have roosted in the counting house above Rofhdel's post and his watch can't sleep through the noise. Kill enough of them to give the garrison its rotation back."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kingdom Army] Rofhdel"), "f_flash_63");

		AddObjective("killLemurs", L("Kill Lemurs around the counting house"),
			new KillObjective(22, new[] { MonsterId.Lemur }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1002 CLASS: Bills of the Relief Column
//-----------------------------------------------------------------------------

public class BillsOfTheReliefColumnQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1002);
		SetName(L("Bills of the Relief Column"));
		SetType(QuestType.Sub);
		SetDescription(L("Adjutant Hans says the Kingdom Army's relief column does not exist, and its bills are keeping families in cursed houses. Strip the bills off the market-run boards and bring them to him with the seals intact."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kalis Knights] Adjutant Hans"), "f_flash_63");

		AddObjective("collectBills", L("Strip Royal Army bills from the market boards"),
			new CollectItemObjective(664013, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(664013, character.Inventory.CountItem(664013), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 9; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_63.Quest1002.Board{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(664013, character.Inventory.CountItem(664013), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 9; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_63.Quest1002.Board{i}");
	}
}

// Quest 1003 CLASS: What the Chargers Took
//-----------------------------------------------------------------------------

public class WhatTheChargersTookQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1003);
		SetName(L("What the Chargers Took"));
		SetType(QuestType.Sub);
		SetDescription(L("Goblin Chargers raided Steponas's salvage cache under the bathhouse. Clear the nest and recover the stuffed bags they hauled off."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Grave-Robber] Steponas"), "f_flash_63");

		AddObjective("killChargers", L("Kill Goblin Chargers"),
			new KillObjective(15, new[] { MonsterId.Goblin2_Hammer }));

		AddObjective("recoverBags", L("Recover Stuffed Bags"),
			new CollectItemObjective(663219, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(663219, 0.40f, MonsterId.Goblin2_Hammer);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663219, character.Inventory.CountItem(663219), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663219, character.Inventory.CountItem(663219), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Faded Stones
//-----------------------------------------------------------------------------

public class FadedStonesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1004);
		SetName(L("Faded Stones"));
		SetType(QuestType.Sub);
		SetDescription(L("The Goblin Magicians of Downtown wear charms cut from petrified people. Kill them and bring Vaida the faded stones so the dead can be named."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Scholar] Vaida"), "f_flash_63");

		AddObjective("killMagicians", L("Kill Goblin Magicians"),
			new KillObjective(12, new[] { MonsterId.Goblin2_Wand3 }));

		AddObjective("gatherStones", L("Recover Faded Stones"),
			new CollectItemObjective(667121, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(667121, 0.50f, MonsterId.Goblin2_Wand3);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667121, character.Inventory.CountItem(667121), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667121, character.Inventory.CountItem(667121), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Stone Froster
//-----------------------------------------------------------------------------

public class TheStoneFrosterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1005);
		SetName(L("The Stone Froster"));
		SetType(QuestType.Sub);
		SetDescription(L("A Stone Froster nests in the cistern under the north block and its breath greys whatever it touches. Make enough noise at the cistern mouth to draw it up, then put it down."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Bounty Hunter] Kestas"), "f_flash_63");

		AddObjective("drawItOut", L("Kill Goblin Chargers by the cistern mouth"),
			new KillObjective(10, new[] { MonsterId.Goblin2_Hammer }));

		AddObjective("killStoneFroster", L("Defeat the Stone Froster"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Stonefroster, 1) },
				resetIdent: "drawItOut",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(603116, 1)); // Basme Bracelet
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
