//--- Melia Script ----------------------------------------------------------
// Sekta Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The last camp before the Grynas road, where the pilgrim board is the only
// institution left and the whole forest has been moving in one direction.
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

public class FPilgrimroad414QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Eleven Days Since the Cart
		//---------------------------------------------------------------------
		AddNpc(155045, L("[Friar] Dorma"), "f_pilgrimroad_41_4", 1222, 334, 282, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1001);

			dialog.SetTitle(L("Dorma"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's dragging a single knife across a whetstone, testing the edge against his thumbnail after every pass*{/}"));
				await dialog.Msg(L("If you've come to pray, I'm afraid I've traded the chapel in for a butcher's block. 40 walkers in this camp, 11 days since a supply cart came up from the lake, and 1 knife between the lot of us. I took orders to keep a chapel. I am running a butcher's yard."));
				await dialog.Msg(L("The Blue Lepusbunnies are the only meat left in Sekta and there are far too many of them. Kill 25 and bring me back 8 cuts and the camp eats twice tomorrow."));

				var response = await dialog.Select(L("Will you go out for meat?"),
					Option(L("I'll bring you 8 cuts"), "help"),
					Option(L("Why are there so many of them?"), "info"),
					Option(L("Walk them down to the lake"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them in the open. In the thicket they go to ground and come back at you from behind, and I have had 2 people opened up learning that."));
						break;

					case "info":
						await dialog.Msg(L("Because they came here. This forest held maybe 60 of them in a good year and there are hundreds now, all of them west of the fork and none of them east."));
						await dialog.Msg(L("They didn't breed into that. Something moved them, and it moved them the same way it moved everything else on this road."));
						break;

					case "leave":
						await dialog.Msg(L("40 people, half of them with feet they can't stand on, down a road with a Minos warband on it. I'd be walking them to a quieter place to die."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLepusbunny", out var killObj)) return;
				if (!quest.TryGetProgress("collectMeat", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He weighs the cuts in both hands before he looks up*{/}"));
					await dialog.Msg(L("8. That's a pot tonight and a pot in the morning, and nobody has to be told they're waiting until tomorrow."));
					await dialog.Msg(L("Take the chapel box. There has been no chapel to spend it on since the roof came in, and there are 40 people who ate because of you."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty down out there. Go back over them - the cuts are on the ground where you left them, not further out."));
				}
				else
				{
					await dialog.Msg(L("Not enough yet. West of the fork, in the open, where they don't have thicket to fall back into."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("2 hot meals and an argument about salt. I have never in my life been so glad to hear people complain about something small."));
			}
		});

		// Quest 1002: Forty Pairs of Feet
		//---------------------------------------------------------------------
		AddNpc(155035, L("[Pilgrim] Vados"), "f_pilgrimroad_41_4", 1175, 335, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1002);

			dialog.SetTitle(L("Vados"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's holding an empty jar up to the light, scraping the last of the salve out with one finger*{/}"));
				await dialog.Msg(L("Ah - fresh feet, good, mine gave out three roads ago. I made foot salve in Orsha for 22 years and I walked out here to stop. Now there are 40 pairs of feet in this camp and 2 jars left, so that plan is finished."));
				await dialog.Msg(L("The root it draws from is the pale one under the old plantings, and the Stumpy Tree Magicians pull it up and cart it off whole. Get me 10 roots and I'll have salve in 6 days."));

				var response = await dialog.Select(L("Will you get the roots?"),
					Option(L("I'll bring you 10 roots"), "help"),
					Option(L("What do the Magicians want with roots?"), "info"),
					Option(L("6 days is too long"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Whole roots only. If it's snapped the milk runs out of it in an hour and you've carried a stick back to me."));
						break;

					case "info":
						await dialog.Msg(L("Nothing. That's what's odd. A Stumpy Tree Magician has no use for a root and they've cleared 3 plantings of it since spring."));
						await dialog.Msg(L("The friar says everything in this forest moved one way at once. I only know that everything that grows in it is going the same way."));
						break;

					case "leave":
						await dialog.Msg(L("It is. It's also 6 days, which is a number, and 'we have no salve' is not a number. I'll take the 6 days."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectRoots", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He snaps one root at the tip and watches the milk bead up before nodding*{/}"));
					await dialog.Msg(L("Every one of them whole. That's 9 jars, which is 40 pairs of feet twice over with some left for the ones who go on to Grynas."));
					await dialog.Msg(L("Take my walking money. I had it saved for the abbey gate offering and I am not going to reach the gate on these feet regardless."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. Work the old plantings west of the board - that's where they're digging now."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("9 jars and a queue. I have not made salve in 4 years and my hands remembered it before I did, which was unsettling and quite nice."));
			}
		});

		// Quest 1003: The Second Watch
		//---------------------------------------------------------------------
		AddNpc(155036, L("[Pilgrim] Eli"), "f_pilgrimroad_41_4", 1245, 395, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1003);

			dialog.SetTitle(L("Eli"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's sitting bolt upright against a supply crate, eyes fixed on the scrub line even in broad daylight*{/}"));
				await dialog.Msg(L("Sorry - I don't stop watching easily anymore, even now. I keep the second watch. 3 shifts a night between 4 of us who can still stand up, and the second is the one where they come."));
				await dialog.Msg(L("The Blue Lepusbunny Assassins have been into the camp itself twice. Kill 25 of them off the camp edge and the second watch stops being the one nobody will take."));

				var response = await dialog.Select(L("Will you clear the camp edge?"),
					Option(L("I'll kill 25 of the Assassins"), "help"),
					Option(L("What happened the 2 times?"), "info"),
					Option(L("Move the camp"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come in low along the scrub line, always from the same 2 directions. Stand in the open and make them cross it and you'll see them coming."));
						break;

					case "info":
						await dialog.Msg(L("The first time they took a pack and the second time they took a man. He was sleeping 8 feet from me and I was awake and I did not hear it happen."));
						await dialog.Msg(L("I have kept the second watch every night since and I will keep keeping it, but I would like to be keeping it against something noisier."));
						break;

					case "leave":
						await dialog.Msg(L("This is the only flat ground between the fork and the Grynas road with water on it. We moved twice already to get here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killAssassins", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Nothing crossed the scrub line last night. I stood the whole second watch listening to 40 people breathe and it was the best 3 hours I've had out here."));
					await dialog.Msg(L("Take the watch purse. It's what we'd have paid a guard with if any guard would take this road."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still coming in. Work the scrub line rather than the deep thicket - that's the ground they use."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("2 people volunteered for the second watch this morning. I said yes before either of them could think about it properly."));
			}
		});

		// Quest 1004: Sixty-One Notices
		//---------------------------------------------------------------------
		AddNpc(155034, L("[Pilgrim] George"), "f_pilgrimroad_41_4", 646, -44, 278, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1004);

			dialog.SetTitle(L("George"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's standing very straight in front of the board, hands clasped, the posture of a man guarding something he can't actually verify*{/}"));
				await dialog.Msg(L("Traveler - can you read? Please say yes, and don't ask why that's my first question. I've minded this board 3 weeks. People hand me a paper, I pin it up, and everyone who comes through reads it and picks a road off it. 61 notices up there now."));
				await dialog.Msg(L("I can't read one of them. Walk the main board and the 2 fork boards and tell me what's actually written, because I've been sending people down roads on faith."));

				var response = await dialog.Select(L("Will you read the boards?"),
					Option(L("I'll read all 3 boards"), "help"),
					Option(L("Why are you minding a board you can't read?"), "info"),
					Option(L("Take them all down"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Read me the dates as well as the words. A notice can be perfectly true and 5 weeks old, and out here that's the same as a lie."));
						break;

					case "info":
						await dialog.Msg(L("Because the man who minded it before me died at the fork and somebody had to stand here. Nobody asked me whether I could read and I did not think it was the moment to say so."));
						await dialog.Msg(L("I've kept them in the order they were handed to me. That's the one thing I could do properly and I have done it every day."));
						break;

					case "leave":
						await dialog.Msg(L("Then people pick a road with nothing at all to pick it with. A wrong notice is bad. No notice is how the last man ended up at the fork."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readBoards", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He makes you repeat the Grynas notice twice and gets very still*{/}"));
					await dialog.Msg(L("So the Salvia road is open, the lake road is open, and the Grynas notice is 5 weeks old and says a thing that stopped being true 4 weeks ago."));
					await dialog.Msg(L("Take the board keeper's money. I'm pulling that notice down myself and I'm going to remember which pin it was on."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 3. The fork boards are out where the roads split - one east toward Grynas, one north toward Salvia."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I've had 9 people through since and I told all 9 of them the truth. One of them offered to teach me letters on the way to the abbey and I have said yes."));
			}
		});

		// Quest 1004 collection points - the pilgrim boards
		//---------------------------------------------------------------------
		void AddPilgrimBoard(int boardNumber, string observation, int x, int z, int direction)
		{
			AddNpc(152007, L("Pilgrim Board"), "f_pilgrimroad_41_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_4", 1004);
				var variableKey = $"Laima.Quests.f_pilgrimroad_41_4.Quest1004.Board{boardNumber}";
				var counterKey = "Laima.Quests.f_pilgrimroad_41_4.Quest1004.BoardsRead";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pilgrim board, layered with pinned paper*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already read this board*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Reading the notices..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var read = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, read);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Boards read: {0}/3", read));

					if (read >= 3)
						character.ServerMessage(L("{#FFD700}All 3 boards read. Return to George.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the board unread."));
				}
			});
		}

		AddPilgrimBoard(1,
			L("Main Board: 61 notices, dated in the order they were handed over. The newest is 3 days old and says the lake road is open."), 596, -44, 278);
		AddPilgrimBoard(2,
			L("Salvia Fork: a fresh hand, 6 days old - the Salvia altar is closed and the waystation is a hospital, but the road itself is walkable."), -62, 1308, 93);
		AddPilgrimBoard(3,
			L("Grynas Fork: 5 weeks old, and it says the Grynas statue ring is dark and the road is shut. It was reopened 4 weeks ago."), 1782, -1371, 262);

		// Quest 1005: Everything Went One Way
		//---------------------------------------------------------------------
		AddNpc(155037, L("[Pilgrim] David"), "f_pilgrimroad_41_4", 1128, 370, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1005);

			dialog.SetTitle(L("David"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_pilgrimroad_41_4", 1001)))
				{
					await dialog.Msg(L("Eat first and let the camp eat. What I've got to say has kept 4 days and it will keep until Dorma has meat in the pot."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's sitting apart from the rest of the camp, drawing lines in the dirt with a stick and rubbing them out just as fast*{/}"));
				await dialog.Msg(L("You've eaten? Good, then you'll actually listen. I came down from the Grynas road in 4 days and I counted the whole way, because I had nothing else to do with my head. Every animal I passed was moving west and none of them were moving east."));
				await dialog.Msg(L("They aren't fleeing this forest. They're being packed into it, and the warren west of the fork is where it stops. Kill 20 Blue Lepusbunnies to open the warren, then put down the 2 does that hold it. Carry the friar's rosary while you do."));

				var response = await dialog.Select(L("Will you go into the warren?"),
					Option(L("I'll break the warren"), "help"),
					Option(L("Packed in by what?"), "info"),
					Option(L("Then we should go east, not west"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(666103, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("The 2 does don't run and they don't chase. Everything else in there does both, so clear the ground before you go near them."));
						break;

					case "info":
						await dialog.Msg(L("I don't know. I know a carver on the Grynas road told me the same thing happened to his statue ring and that it always fails from the hills down, never from the road up."));
						await dialog.Msg(L("The lake monk says her warband came down the Ouaas road. The friar in Salvia says widlings emptied his altar. Every one of us is pointing at a different piece of the same direction."));
						break;

					case "leave":
						await dialog.Msg(L("East is Grynas and Grynas is already answered. West of the fork is 40 people's food and the reason none of us can stay here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openWarren", out var warrenObj)) return;
				if (!quest.TryGetProgress("killDoes", out var doesObj)) return;

				if (warrenObj.Done && doesObj.Done)
				{
					await dialog.Msg(L("{#666666}*He turns the rosary over and finds the abbey mark rubbed flat on one side*{/}"));
					await dialog.Msg(L("Worn smooth. Dorma's carried that 30 years and it went dull in 4 days out there, and I'd like somebody at Ouaas to tell me why."));
					await dialog.Msg(L("Keep the chain off the warren floor - there was abbey silver in that hole and no abbey walker ever went into it. I'm carrying word north to Monk Matas at the memorial, and I'm going by the lake so Stella hears it too."));

					character.Quests.Complete(questId);
				}
				else if (warrenObj.Done)
				{
					await dialog.Msg(L("The warren's open. The 2 does are still standing over the middle of it and they will not come out to you."));
				}
				else
				{
					await dialog.Msg(L("Too many still in the runs. Open it up first or you'll have the whole warren at your back at the does."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The forest is quiet west of the fork and 3 deer walked back east through the camp this morning. First thing I've seen go that way in 4 days."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Eleven Days Since the Cart
//-----------------------------------------------------------------------------

public class ElevenDaysSinceTheCartQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1001);
		SetName(L("Eleven Days Since the Cart"));
		SetType(QuestType.Sub);
		SetDescription(L("40 walkers are camped in Sekta Forest with no supply cart in 11 days. The Blue Lepusbunnies are the only meat left, and there are far too many of them for a forest this size."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Friar] Dorma"), "f_pilgrimroad_41_4");

		AddObjective("killLepusbunny", L("Kill Blue Lepusbunnies west of the fork"),
			new KillObjective(25, new[] { MonsterId.Repusbunny_Purple }));

		AddObjective("collectMeat", L("Recover Lepusbunny Meat"),
			new CollectItemObjective(666106, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(666106, 0.40f, MonsterId.Repusbunny_Purple);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666106, character.Inventory.CountItem(666106), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666106, character.Inventory.CountItem(666106), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Forty Pairs of Feet
//-----------------------------------------------------------------------------

public class FortyPairsOfFeetQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1002);
		SetName(L("Forty Pairs of Feet"));
		SetType(QuestType.Sub);
		SetDescription(L("The camp is down to 2 jars of foot salve for 40 walkers. The pale root it draws from grows under the old plantings, and the Stumpy Tree Magicians have been pulling it up whole and carting it off."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim] Vados"), "f_pilgrimroad_41_4");

		AddObjective("collectRoots", L("Recover Abandoned Plant Roots from the Stumpy Tree Magicians"),
			new CollectItemObjective(666105, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(666105, 0.55f, MonsterId.Stub_Tree_Mage);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666105, character.Inventory.CountItem(666105), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666105, character.Inventory.CountItem(666105), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Second Watch
//-----------------------------------------------------------------------------

public class TheSecondWatchQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1003);
		SetName(L("The Second Watch"));
		SetType(QuestType.Sub);
		SetDescription(L("Blue Lepusbunny Assassins have come into the Sekta camp twice, and the second watch is the shift nobody will take. Kill 25 of them off the camp edge."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim] Eli"), "f_pilgrimroad_41_4");

		AddObjective("killAssassins", L("Kill Blue Lepusbunny Assassins on the camp edge"),
			new KillObjective(25, new[] { MonsterId.Repusbunny_Bow_Purple }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Sixty-One Notices
//-----------------------------------------------------------------------------

public class SixtyOneNoticesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1004);
		SetName(L("Sixty-One Notices"));
		SetType(QuestType.Sub);
		SetDescription(L("The man minding the Sekta pilgrim board cannot read a word of it, and everyone coming through picks a road off what is pinned there. Read the main board and both fork boards for him."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim] George"), "f_pilgrimroad_41_4");

		AddObjective("readBoards", L("Read the main board and both fork boards"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_4.Quest1004.BoardsRead", 3, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_4.Quest1004.BoardsRead");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_4.Quest1004.Board{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_4.Quest1004.BoardsRead");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_4.Quest1004.Board{i}");
	}
}

// Quest 1005 CLASS: Everything Went One Way
//-----------------------------------------------------------------------------

public class EverythingWentOneWayQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1005);
		SetName(L("Everything Went One Way"));
		SetType(QuestType.Sub);
		SetDescription(L("A walker down from Grynas counted every animal he passed and every one was moving west. The Lepusbunnies are not fleeing Sekta - they are being packed into it, and the warren west of the fork is where it stops."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Pilgrim] David"), "f_pilgrimroad_41_4");

		AddPrerequisite(new CompletedPrerequisite("f_pilgrimroad_41_4", 1001));

		AddObjective("openWarren", L("Kill Blue Lepusbunnies in the warren runs"),
			new KillObjective(20, new[] { MonsterId.Repusbunny_Purple }));

		AddObjective("killDoes", L("Put down the 2 does holding the warren"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Repusbunny_Purple, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Repusbunny_Bow_Purple, 3),
				},
				resetIdent: "openWarren",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583113, 1)); // Pejnus Necklace
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666103, character.Inventory.CountItem(666103), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666103, character.Inventory.CountItem(666103), InventoryItemRemoveMsg.Destroyed);
	}
}
