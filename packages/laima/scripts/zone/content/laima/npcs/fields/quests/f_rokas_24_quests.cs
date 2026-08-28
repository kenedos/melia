//--- Melia Script ----------------------------------------------------------
// Gateway of the Great King Quest NPCs
//--- Description -----------------------------------------------------------
// The expedition camp at the canyon mouth, where four generations of the Jonas
// house have run a dig that has never dug anything.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas24QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Ninety Years, Eleven Crates
		//---------------------------------------------------------------------
		AddNpc(156169, L("[Recorder] Gailas Jonas"), "f_rokas_24", 672, -2109, 283, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1001);

			dialog.SetTitle(L("Gailas Jonas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's stamping a ledger page, checking the seal against the light before he sets it aside*{/}"));
				await dialog.Msg(L("A visitor to the camp — well, you've picked a bad season for it. My house has recorded this excavation for 4 generations. 90 years of season returns, and the whole of what has come out of this canyon fits in 11 crates."));
				await dialog.Msg(L("The Hogma took the spring supply train in the west draw and I cannot file a return on stores I do not have. Kill 25 of them and bring me 8 bundles of the research supplies back."));

				var response = await dialog.Select(L("Will you go into the west draw?"),
					Option(L("I'll recover 8 bundles"), "help"),
					Option(L("Eleven crates in ninety years?"), "info"),
					Option(L("Order more from Fedimian"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They break the crates and keep the sacking, so look for canvas, not for boxes. Anything still tied is ours."));
						break;

					case "info":
						await dialog.Msg(L("11. I have counted them. My grandfather counted them. There is a ledger in the tent with 90 years of nothing in it, kept beautifully."));
						await dialog.Msg(L("I have asked my father twice what we are actually doing here. The first time he changed the subject and the second time he said I would be told when it was my turn."));
						break;

					case "leave":
						await dialog.Msg(L("Fedimian approves this expedition's stores on 90 years of precedent and 0 questions. I would rather not be the Jonas who made them look at it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHogma", out var killObj)) return;
				if (!quest.TryGetProgress("collectSupplies", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He checks each bundle against a manifest and initials the corner of the page*{/}"));
					await dialog.Msg(L("8 of 9. I can carry 1 lost bundle as spoilage and nobody in Fedimian will ever read the line."));
					await dialog.Msg(L("Take the recovery allowance. It exists because my great-grandfather thought supply trains would be attacked, and he was right for 90 years running."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Draw's clear. Now go over the ground - the bundles will be scattered where they broke the crates open."));
				}
				else
				{
					await dialog.Msg(L("Still Hogma in the draw. Clear them first or you will be carrying canvas with an axe behind you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Stores filed and the season signed off. 90 years and I am the first Jonas to lose a bundle, which I have decided to be quietly proud of."));
			}
		});

		// Quest 1002: Nineteen Years, No Trenches
		//---------------------------------------------------------------------
		AddNpc(20158, L("[Historian] Beard"), "f_rokas_24", 1598, -185, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1002);

			dialog.SetTitle(L("Beard"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's squinting up at a bird nest through a battered eyeglass, one hand shading the sun*{/}"));
				await dialog.Msg(L("Ah — company! Marvelous, hold this thought for me before it evaporates like all the good ones do. Nineteen years on this expedition, and I have opened precisely zero trenches. Not refused, mind you — never refused! Every season the permission is 'pending,' and every season it stays pending right up until the snow makes the whole question moot."));
				await dialog.Msg(L("So instead I read bird nests. Cockats, specifically — they line the things with whatever glittering nonsense they scrounge off the canyon floor, bless their thieving little hearts. Bring me 10 gold pieces out of them and I'll have a full assemblage without ever once touching a spade!"));

				var response = await dialog.Select(L("Will you go through the nests?"),
					Option(L("I'll bring you 10 pieces"), "help"),
					Option(L("Nineteen years of pending?"), "info"),
					Option(L("Dig anyway"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Off the birds, not out of the nests. A nest with a Cockat still on it is a nest you will be carried away from."));
						break;

					case "info":
						await dialog.Msg(L("Nineteen for me! Gorath's up to 24, if you can believe it. Kefek came out here a young man and now has grey in his beard, and he's never broken ground either. It's practically a tradition at this point."));
						await dialog.Msg(L("Four historians, seventy-eight years between the lot of us, and not one spadeful to show for it. At some point that stops being bad luck, doesn't it? And starts looking like a policy."));
						break;

					case "leave":
						await dialog.Msg(L("And be sent home, and the next man is told the same thing, and the ground stays shut. I would rather be here reading birds' nests than not here at all."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectPieces", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lays the pieces out on a board and groups them without touching any of them twice*{/}"));
					await dialog.Msg(L("All 10 the same alloy and 7 of them the same stamp. That is not scavenging, that is 1 hoard being carried up out of 1 place."));
					await dialog.Msg(L("Take my season's stipend. I have nowhere to spend it and no trench to spend it on."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough for an assemblage. Work the eastern shelf - that is where the big ones nest."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I filed the assemblage. Gailas signed it, and then he asked me, very carefully, where in the canyon the birds go down. I do not think he was making conversation."));
			}
		});

		// Quest 1003: Six Years on the Contract
		//---------------------------------------------------------------------
		AddNpc(147415, L("[Mercenary] Mirta"), "f_rokas_24", 1138, 998, 16, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1003);

			dialog.SetTitle(L("Mirta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's leaning against a boulder with a whetstone going, sizing you up without breaking rhythm*{/}"));
				await dialog.Msg(L("You walk like someone who's actually used that blade, not just carried it. Good, I like the odds better already. Six years on this contract, guarding an excavation where — get this — nobody has excavated one single thing. So what I actually guard is four old men and a tent full of paper. Thrilling work."));
				await dialog.Msg(L("Cockatrices came down onto the north shelf in numbers I've genuinely never seen, and I've seen a lot. Kill 30 of them before one of my old men wanders up there with a notebook and gets himself eaten."));

				var response = await dialog.Select(L("Will you clear the north shelf?"),
					Option(L("I'll kill 30 Cockatrices"), "help"),
					Option(L("Six years guarding paper?"), "info"),
					Option(L("Take the old men's notebooks away"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come at you in a line and the line has a middle. Break the middle and the 2 halves will not re-form, they just circle."));
						break;

					case "info":
						await dialog.Msg(L("It is the best contract I have ever had and it is the only one I have ever been frightened by. 6 years and the pay has never once been late."));
						await dialog.Msg(L("Nobody pays a mercenary on time for 6 years to watch nothing happen. Somebody is paying me to make sure nothing happens."));
						break;

					case "leave":
						await dialog.Msg(L("Try it. Beard will bite you. Gorath will write to Fedimian about you. I have thought about it more than I am comfortable admitting."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCockatrice", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Shelf is clear and I walked it twice. 6 years and that is the first honest afternoon's work I have been given."));
					await dialog.Msg(L("Take it out of the contract's contingency. There is 6 years of contingency in that purse and no contingencies."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still thick up there. Work the shelf edge - in the open they have to come to you across bare rock."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("They came back within the week and they came from the same direction, which is up the canyon. Nothing on this ridge should be walking up out of that canyon."));
			}
		});

		// Quest 1004: The Season's Returns
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Historian] Gorath"), "f_rokas_24", -1468, -1328, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1004);

			dialog.SetTitle(L("Gorath"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's rifling through a stack of blank forms, exhaling hard through his nose at every empty one*{/}"));
				await dialog.Msg(L("If you're looking for the historian who digs, wrong camp, wrong century. Twenty-four years here and I have become, against every ambition I ever had, the man who collects everyone else's paperwork. Not what I trained for. It is, apparently, what I am now. Wonderful."));
				await dialog.Msg(L("Three postings up the canyon, one reading device out west, season closes in 9 days, and none of them have sent so much as a scrap. Go to all 4, drag their returns out of them, and bring the lot back to me."));

				var response = await dialog.Select(L("Will you walk the postings?"),
					Option(L("I'll visit all 4"), "help"),
					Option(L("What reading device?"), "info"),
					Option(L("Make them come to you"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the reading off the device last. The 3 men will tell you what they think and the device will tell you what is, and I want those in that order."));
						break;

					case "info":
						await dialog.Msg(L("The Eye of the Great King. It sits out west and it does something none of us has ever been told, and it is on every season return since the expedition opened."));
						await dialog.Msg(L("Line 9. 'Eye: lit.' 90 years of line 9 saying lit, in 4 different hands."));
						break;

					case "leave":
						await dialog.Msg(L("They are 60, 58 and 71 and they are spread over 3 miles of canyon. If I could make them come to me I would have retired 6 years ago."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectReturns", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He writes the first 3 returns straight into the ledger and then stops with the pen down for a long time*{/}"));
					await dialog.Msg(L("Line 9. 90 years of 'Eye: lit' and I am about to write 'Eye: dark' under my own name."));
					await dialog.Msg(L("Take the collection fee and take it now, because the moment this ledger goes to Fedimian I expect this camp to stop being a quiet posting."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 4. Kefek is up at the north head, Badat is mid-canyon, Grinus is on the west rim, and the Eye is beyond Grinus."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The ledger went out on the fast rider. 24 years of line 9 and the first time I have written something in it that anybody will actually read."));
			}
		});

		// Quest 1004 collection points - the canyon postings
		//---------------------------------------------------------------------
		void AddPosting(int postingNumber, int model, string postingName, string observation, int x, int z, int direction)
		{
			AddNpc(model, postingName, "f_rokas_24", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_rokas_24", 1004);
				var variableKey = $"Laima.Quests.f_rokas_24.Quest1004.Posting{postingNumber}";
				var counterKey = "Laima.Quests.f_rokas_24.Quest1004.ReturnsCollected";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A canyon posting of the royal mausoleum expedition*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already took this return*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Taking the season's return..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var collected = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, collected);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Returns collected: {0}/4", collected));

					if (collected >= 4)
						character.ServerMessage(L("{#FFD700}All 4 returns collected. Take them to Gorath.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the posting without a return."));
				}
			});
		}

		AddPosting(1, 147422, L("[Historian] Kefek"),
			L("Kefek, north head: 'Nothing to report. Same as last season. Same as the 22 before it.'"), -1041, 1647, 0);
		AddPosting(2, 20109, L("[Historian] Badat"),
			L("Badat, mid-canyon: 'Cockat flocks moving up the canyon and not down it. I have written this 3 seasons running.'"), -574, -770, 270);
		AddPosting(3, 20139, L("[Historian] Grinus"),
			L("Grinus, west rim: 'Ground temperature at the rim up 4 degrees since spring. I would like somebody to tell me why that is not interesting.'"), -1593, 53, 251);
		AddPosting(4, 147475, L("Eye of the Great King"),
			L("The Eye: a mirrored cube on a socket, cold to the hand and completely dark. Line 9 has said 'lit' for 90 years."), -695, 264, 284);

		// Quest 1005: What Came Up the Canyon
		//---------------------------------------------------------------------
		AddNpc(147425, L("[Recorder] Florijonas"), "f_rokas_24", -745, 216, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1005);

			dialog.SetTitle(L("Florijonas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_rokas_24", 1004)))
				{
					await dialog.Msg(L("{#666666}*He's sitting cross-legged in front of the Eye, not looking up as your shadow falls across him*{/}"));
					await dialog.Msg(L("Walk Gorath's returns first. I have sat beside this thing for 31 years and I will not be the man who tells you before the ledger does."));
					return;
				}

				await dialog.Msg(L("{#666666}*He gets up slowly, joints stiff from sitting, and finally faces you*{/}"));
				await dialog.Msg(L("So you have seen it dark. I have sat beside the Eye for 31 years and it went out in the spring, and I have written 'lit' in every return since, in my own hand, because my son has 90 years of a family's word to carry."));
				await dialog.Msg(L("It is not a lamp. It is the outer gate, and it is open. The Cockats are not nesting here, they were pushed out of the canyon. Kill 20 Cockatrices to get to the gate mouth, and put down what has come up to sit in it."));

				var response = await dialog.Select(L("Will you go to the gate mouth?"),
					Option(L("I'll clear the gate mouth"), "help"),
					Option(L("You falsified the returns?"), "info"),
					Option(L("Tell Gailas yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650319, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Take the report. It is the only copy of what this expedition actually is, and if I am wrong about the gate I would rather it were in somebody's hands than in mine."));
						break;

					case "info":
						await dialog.Msg(L("For 5 months. Before you judge that, understand what the true return does: it says the Great King's outer gate is open and the Jonas house has been paid for 90 years to keep it shut."));
						await dialog.Msg(L("There is no dig. There has never been a dig. There is a seal in 3 courses and we are the paperwork that stops anyone asking about it."));
						break;

					case "leave":
						await dialog.Msg(L("I will. After. He asked me twice what we do here and both times I sent him away, and I am not doing it a third time with the gate open behind me."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearMouth", out var mouthObj)) return;
				if (!quest.TryGetProgress("killFlock", out var flockObj)) return;

				if (mouthObj.Done && flockObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks to the Eye, puts a hand flat on the socket, and holds it there until his arm shakes*{/}"));
					await dialog.Msg(L("Still cold. Clearing the mouth does not close a gate - it only means somebody can walk down to the second course and look."));
					await dialog.Msg(L("Take the chain off the gate mouth. And go down the canyon to the Overlong Bridge and find Morkus Jonas, who is my brother, and tell him line 9 says dark. He will know what to do with that and I no longer do."));

					character.Quests.Complete(questId);
				}
				else if (mouthObj.Done)
				{
					await dialog.Msg(L("The mouth is open. What is sitting in it did not come out of this ridge and it will not leave on its own."));
				}
				else
				{
					await dialog.Msg(L("Too many birds between here and the mouth. Clear them, or you will arrive at the gate with a flock behind you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I told Gailas. All of it, the falsified returns first, because that was the part I owed him. He wrote it down. Of course he wrote it down. He is a Jonas."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Ninety Years, Eleven Crates
//-----------------------------------------------------------------------------

public class NinetyYearsElevenCratesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1001);
		SetName(L("Ninety Years, Eleven Crates"));
		SetType(QuestType.Sub);
		SetDescription(L("The Jonas house has recorded the royal mausoleum expedition for 4 generations and 90 years. The Hogma took the spring supply train in the west draw and the season's return cannot be filed without the stores."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Recorder] Gailas Jonas"), "f_rokas_24");

		AddObjective("killHogma", L("Kill Hogma Warriors in the west draw"),
			new KillObjective(25, new[] { MonsterId.Hogma_Warrior }));

		AddObjective("collectSupplies", L("Recover Research Aid Supplies"),
			new CollectItemObjective(650317, 8));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(650317, 0.35f, MonsterId.Hogma_Warrior);
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

// Quest 1002 CLASS: Nineteen Years, No Trenches
//-----------------------------------------------------------------------------

public class NineteenYearsNoTrenchesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1002);
		SetName(L("Nineteen Years, No Trenches"));
		SetType(QuestType.Sub);
		SetDescription(L("A historian who has never been permitted to open a trench in 19 years reads the Cockat nests instead, since they line them with whatever they carry up off the canyon floor."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Historian] Beard"), "f_rokas_24");

		AddObjective("collectPieces", L("Recover Golden Pieces from the Cockats"),
			new CollectItemObjective(650316, 10));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion

		AddDrop(650316, 0.45f, MonsterId.Big_Cockatries);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650316, character.Inventory.CountItem(650316), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650316, character.Inventory.CountItem(650316), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Six Years on the Contract
//-----------------------------------------------------------------------------

public class SixYearsOnTheContractQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1003);
		SetName(L("Six Years on the Contract"));
		SetType(QuestType.Sub);
		SetDescription(L("The expedition's mercenary has been paid on time for 6 years to guard a dig that never digs. Cockatrices have come down onto the north shelf in numbers she has never seen."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mercenary] Mirta"), "f_rokas_24");

		AddObjective("killCockatrice", L("Kill Cockatrices on the north shelf"),
			new KillObjective(30, new[] { MonsterId.Cockatries }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1004 CLASS: The Season's Returns
//-----------------------------------------------------------------------------

public class TheSeasonsReturnsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1004);
		SetName(L("The Season's Returns"));
		SetType(QuestType.Sub);
		SetDescription(L("The season closes in 9 days and 3 postings up the canyon and a reading device out west have not sent in their returns. Line 9 of that return has read 'Eye: lit' for 90 years."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Historian] Gorath"), "f_rokas_24");

		AddObjective("collectReturns", L("Collect the returns from all 4 canyon postings"),
			new VariableCheckObjective("Laima.Quests.f_rokas_24.Quest1004.ReturnsCollected", 4, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_24.Quest1004.ReturnsCollected");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_24.Quest1004.Posting{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_24.Quest1004.ReturnsCollected");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_24.Quest1004.Posting{i}");
	}
}

// Quest 1005 CLASS: What Came Up the Canyon
//-----------------------------------------------------------------------------

public class WhatCameUpTheCanyonQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1005);
		SetName(L("What Came Up the Canyon"));
		SetType(QuestType.Sub);
		SetDescription(L("The Eye of the Great King is not a lamp - it is the outer gate, and it went dark in the spring. The Cockats were pushed out of the canyon, not nesting in it. Clear the gate mouth and put down what has come up to sit in it."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Recorder] Florijonas"), "f_rokas_24");

		AddPrerequisite(new CompletedPrerequisite("f_rokas_24", 1004));

		AddObjective("clearMouth", L("Kill Cockatrices between the rim and the gate mouth"),
			new KillObjective(20, new[] { MonsterId.Cockatries }));

		AddObjective("killFlock", L("Put down what is sitting in the gate mouth"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Big_Cockatries, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Big_Cockatries_Red, 3),
				},
				resetIdent: "clearMouth",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(39000, 27000));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(583101, 1)); // Conqueror
		AddReward(new ItemReward(640085, 3)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650319, character.Inventory.CountItem(650319), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650319, character.Inventory.CountItem(650319), InventoryItemRemoveMsg.Destroyed);
	}
}
