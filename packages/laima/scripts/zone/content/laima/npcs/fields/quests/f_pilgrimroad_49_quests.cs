//--- Melia Script ----------------------------------------------------------
// Genar Field Quest NPCs
//--- Description -----------------------------------------------------------
// The last open ground before the altar road, where the markers are cut for
// the pilgrims who do not finish the walk.
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

public class FPilgrimroad49QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Forty Blanks in the Yard
		//---------------------------------------------------------------------
		AddNpc(155035, L("[Stonecutter] Antanas"), "f_pilgrimroad_49", 1423, -425, 250, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1001);

			dialog.SetTitle(L("Antanas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's counting an empty row of stone blanks with his finger, frowning harder each time the count comes up short*{/}"));
				await dialog.Msg(L("You're not here for a marker, are you? Good, I've had enough of those this week. 311 stones in 19 years. That's how many people started this walk and stopped on my stretch of it, and every one of them has a marker because I cut it."));
				await dialog.Msg(L("The Green Tini Archers have been dragging my blanks off the yard to build up their shooting berms. Kill 25 of them and bring me back 8 blanks before I'm cutting names into nothing."));

				var response = await dialog.Select(L("Will you go out to the berms?"),
					Option(L("I'll bring back 8 blanks"), "help"),
					Option(L("They're using gravestones as walls?"), "info"),
					Option(L("Quarry more stone"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The blanks are the flat ones with a squared foot. Anything rounded is field rock and I don't want it back."));
						break;

					case "info":
						await dialog.Msg(L("Blanks so far. Blanks are stone and stone is stone and I can be reasonable about blanks."));
						await dialog.Msg(L("Last week they took a finished one off the south trail. It had a name on it. I have been unreasonable ever since and I intend to stay that way."));
						break;

					case "leave":
						await dialog.Msg(L("The seam that gives this stone is 4 days east and it takes 3 men and a cart. I have me and a barrow."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("collectBlanks", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He stands each blank on its foot and taps it once with a knuckle, listening*{/}"));
					await dialog.Msg(L("8, and 7 of them ring clean. That's 7 people who get a marker this month who weren't going to."));
					await dialog.Msg(L("Take the yard money. I get paid by the stone and I have been paid for stones I couldn't cut, which has been sitting badly with me."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Berms are quiet. Now go and pull the blanks out of them - they're built in flat side down."));
				}
				else
				{
					await dialog.Msg(L("Still shooting off those berms. Clear them before you start hauling or you'll be carrying stone with arrows in you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("7 cut and set this week. A woman came up the road looking for one of the names and I was able to walk her to it, which is the whole job in one sentence."));
			}
		});

		// Quest 1002: Six Stones a Day
		//---------------------------------------------------------------------
		AddNpc(152065, L("[Letterer] Giedra"), "f_pilgrimroad_49", -266, 1171, 304, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1002);

			dialog.SetTitle(L("Giedra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's holding a chisel an inch above stone, waiting for her hand to stop shaking before she taps it*{/}"));
				await dialog.Msg(L("Don't talk for a second, I nearly ruined that one. There. Antanas cuts them and I letter them. 4 strokes to a letter, 6 stones on a good day, and the cut only shows once it's had the lime wash."));
				await dialog.Msg(L("The lime comes off the chalk banks and the Brown Tini have been rolling in it and carrying it away in their coats. Bring me 10 measures of the powder."));

				var response = await dialog.Select(L("Will you get the lime?"),
					Option(L("I'll bring you 10 measures"), "help"),
					Option(L("Why do they roll in it?"), "info"),
					Option(L("Letter them without the wash"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Beat it out of the coat rather than scraping - scraped lime comes off grey and grey wash makes a name harder to read, not easier."));
						break;

					case "info":
						await dialog.Msg(L("Mites. It kills whatever lives in the coat and they've clearly worked that out, which is more than the Tini on the east side have worked out about anything."));
						await dialog.Msg(L("I have been trying to resent them for it for a month and I keep failing, because it is genuinely quite clever."));
						break;

					case "leave":
						await dialog.Msg(L("Then in 3 years the cut weathers over and the stone says nothing. A blank stone is worse than no stone. It looks like somebody meant to and didn't."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectLime", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She wets a thumb, touches the powder, and holds it up against the white of her sleeve*{/}"));
					await dialog.Msg(L("Bright. That's a month of washing at 6 stones a day and I'll not have to water it down once."));
					await dialog.Msg(L("Take this. It's what I'd have spent on lime if I'd had to buy it from Fedimian, and Fedimian charges like the stones are for them."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough. Work the north field where the chalk comes up - that's the ground they're rolling on."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("31 stones washed and every one of them readable from the road. That was the whole point and it took a month to be able to do it."));
			}
		});

		// Quest 1003: Nine Miles of South Trail
		//---------------------------------------------------------------------
		AddNpc(155038, L("[Trail-Warden] Kestas"), "f_pilgrimroad_49", -1192, -2045, 302, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1003);

			dialog.SetTitle(L("Kestas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He stops mid-stride at the sound of your footsteps, head tilted, like he's checking your gait against a list of ones he knows*{/}"));
				await dialog.Msg(L("New boots, new stride - you're not from around here. I walk 9 miles of south trail and back, every day, and I have walked it 6 years. I know it by the sound my boots make on it."));
				await dialog.Msg(L("The Brown Tini have taken the last 2 miles of it and I've lost 2 markers off that stretch already. Kill 30 of them and I get my trail back."));

				var response = await dialog.Select(L("Will you take the south trail?"),
					Option(L("I'll kill 30 Brown Tini"), "help"),
					Option(L("What happened to the markers?"), "info"),
					Option(L("Reroute the trail"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They pile up in the low ground and come at you all at once out of it. Fight on the rise and they have to come up to you one at a time."));
						break;

					case "info":
						await dialog.Msg(L("Gone. Not broken, not knocked over - gone, foot and all, and a marker foot is buried 2 feet down."));
						await dialog.Msg(L("Something took the trouble to dig 2 gravestones out of the ground and carry them off, and I have walked this trail 6 years and cannot tell you why."));
						break;

					case "leave":
						await dialog.Msg(L("The trail goes where the 300 stones are. You don't move a trail away from the stones, you move the trouble away from the trail."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTini", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Walked the full 9 this morning and back again. Boots sounded the same the whole way, which they have not done since spring."));
					await dialog.Msg(L("Take the warden's purse. I draw it for walking a trail and I've spent a month walking 7 miles of one."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still thick down there. Work the low ground at the far end - the last 2 miles, not the near stretch."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I found where the 2 markers went. Both of them are out east on the Manahas road, stood up in a wall with the letters facing inward, and I have not told Antanas yet."));
			}
		});

		// Quest 1004: Fourteen Hundred Names
		//---------------------------------------------------------------------
		AddNpc(155034, L("[Binder] Tomalov"), "f_pilgrimroad_49", -87, -329, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1004);

			dialog.SetTitle(L("Tomalov"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He looks up slowly from a thick, well-worn ledger, adjusting a pair of spectacles that have slid halfway down his nose*{/}"));
				await dialog.Msg(L("Ah - another name to add to the book, or a pair of legs I can borrow? Either's welcome. I keep the road register. 1,400 names in it, every one of them a person who came through Genar Field and which way they went out, and I stitch and bind the thing myself."));
				await dialog.Msg(L("Walkers cut their mark on the register stone at whichever road end they leave by. There are 3 stones and I have not been out to any of them in 5 weeks. Go and read all 3 for me."));

				var response = await dialog.Select(L("Will you read the register stones?"),
					Option(L("I'll read all 3 stones"), "help"),
					Option(L("Why does the register matter?"), "info"),
					Option(L("Walk out and read them yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Count the marks and look at the depth of them. A mark cut by somebody in a hurry looks nothing like a mark cut by somebody who has decided to turn back."));
						break;

					case "info":
						await dialog.Msg(L("Because Antanas cuts a stone for anyone who stops here and a stone needs a name. The register is where the name comes from."));
						await dialog.Msg(L("If somebody walks out of Genar Field and is never seen again, the register is the difference between a marker and a blank slab, and I have already explained to you what a blank slab looks like."));
						break;

					case "leave":
						await dialog.Msg(L("I have. For 11 years. I am 71 and the west stone is a mile and a half out and I got halfway to it last month and had to sit down in the road."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readStones", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He copies all 3 counts into the register in a hand that has not changed in 40 years*{/}"));
					await dialog.Msg(L("34 out west, 19 out north, and 4 east. 4, on the Manahas road, in 5 weeks - and Manahas is the road everybody takes."));
					await dialog.Msg(L("Take the binder's money. And go and tell Antanas about the east stone, because I think he already knows and has not said it out loud."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 3 yet. West at the field road, north at the altar road, east where the Manahas road leaves the ground."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("1,400 names and I have started a second column beside the east ones. I would very much like that column to stop growing."));
			}
		});

		// Quest 1004 collection points - the register stones
		//---------------------------------------------------------------------
		void AddRegisterStone(int stoneNumber, string observation, int x, int z, int direction)
		{
			AddNpc(47190, L("Register Stone"), "f_pilgrimroad_49", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_49", 1004);
				var variableKey = $"Laima.Quests.f_pilgrimroad_49.Quest1004.Stone{stoneNumber}";
				var counterKey = "Laima.Quests.f_pilgrimroad_49.Quest1004.StonesRead";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A register stone at the road end, its face crowded with cut marks*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already counted this one*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Counting the marks..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var read = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, read);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Register stones read: {0}/3", read));

					if (read >= 3)
						character.ServerMessage(L("{#FFD700}All 3 stones read. Return to Tomalov.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the stone uncounted."));
				}
			});
		}

		AddRegisterStone(1,
			L("West Stone: 34 marks in 5 weeks, most of them cut shallow and fast by people going back the way they came."), -2275, -648, 255);
		AddRegisterStone(2,
			L("North Stone: 19 marks, all of them deep and squared off. Nobody cuts a mark like that unless they mean to finish the walk."), -86, 1279, 89);
		AddRegisterStone(3,
			L("East Stone: 4 marks in 5 weeks on the road everybody takes, and the stone's foot has been dug at and packed back in."), 2338, 145, 34);

		// Quest 1005: The Wall with the Letters Inward
		//---------------------------------------------------------------------
		AddNpc(155035, L("[Stonecutter] Antanas"), "f_pilgrimroad_49", 2338, 225, 214, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_49", 1005);

			dialog.SetTitle(L("Antanas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_pilgrimroad_49", 1001)))
				{
					await dialog.Msg(L("Get my blanks off the berms first. I'll not walk up this road to count what's out here until the yard can still cut a stone when I get back."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's standing at the yard gate with his coat already on, chisel bag slung over one shoulder like he means to march somewhere*{/}"));
				await dialog.Msg(L("Good, you again - I was about to go looking for you. Kestas found his 2 markers. They're out on the Manahas road stood up in a wall, letters facing inward, and there are 40 more stones in that wall that came out of my ground."));
				await dialog.Msg(L("Tomalov's east stone says 4 people took this road in 5 weeks and it's the road everybody takes. Kill 20 Green Tini Archers to open the wall, then take down the 2 Magicians standing behind it."));

				var response = await dialog.Select(L("Will you go up the Manahas road?"),
					Option(L("I'll pull the wall down"), "help"),
					Option(L("Why letters inward?"), "info"),
					Option(L("It's only stone"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The 2 behind it don't move off the wall. Everything else does, so empty the ground first and then walk in at them."));
						break;

					case "info":
						await dialog.Msg(L("Because somebody wanted the names read from the inside. Tini don't read. 40 stones, squared, courses laid true, and letters turned in to face whatever is standing in there."));
						await dialog.Msg(L("I cut 40 of those stones. I know what every one of them says and I would like to know who it is being said to."));
						break;

					case "leave":
						await dialog.Msg(L("It's 311 stones in 19 years and 40 of them are in a wall. It stopped being only stone somewhere around the third one."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openWall", out var wallObj)) return;
				if (!quest.TryGetProgress("killMagicians", out var mageObj)) return;

				if (wallObj.Done && mageObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks the fallen courses reading every stone aloud, foot to foot, and does not skip one*{/}"));
					await dialog.Msg(L("40. I've got all 40 and I can set every one of them back where it came from, and I'm going to do it in the order I cut them."));
					await dialog.Msg(L("Take the hammer off the wall foot. It's Orsha work, it isn't mine, and somebody laid those courses with it. Tomalov's second column can stop where it is."));

					character.Quests.Complete(questId);
				}
				else if (wallObj.Done)
				{
					await dialog.Msg(L("Ground's clear. The 2 behind the wall are still standing there and they'll stand there until you go in."));
				}
				else
				{
					await dialog.Msg(L("Too many archers on the courses. Take them off it first - you don't want to be pulling stone with shafts coming down at you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("31 stones back in the ground and 9 to go, and Giedra has re-washed every one. Tomalov put 11 marks on the east stone this week, which is what that road ought to look like."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Forty Blanks in the Yard
//-----------------------------------------------------------------------------

public class FortyBlanksInTheYardQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1001);
		SetName(L("Forty Blanks in the Yard"));
		SetType(QuestType.Sub);
		SetDescription(L("The stonecutter has cut 311 markers in 19 years for the pilgrims who stop on this stretch. The Green Tini Archers have been dragging his uncut blanks off the yard to build up their shooting berms."));
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stonecutter] Antanas"), "f_pilgrimroad_49");

		AddObjective("killArchers", L("Kill Green Tini Archers on the berms"),
			new KillObjective(25, new[] { MonsterId.Tiny_Bow_Green }));

		AddObjective("collectBlanks", L("Recover the tombstone blanks"),
			new CollectItemObjective(662159, 8));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(662159, 0.40f, MonsterId.Tiny_Bow_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662159, character.Inventory.CountItem(662159), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662159, character.Inventory.CountItem(662159), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Six Stones a Day
//-----------------------------------------------------------------------------

public class SixStonesADayQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1002);
		SetName(L("Six Stones a Day"));
		SetType(QuestType.Sub);
		SetDescription(L("A cut name does not show until it has had the lime wash, and without it the stone weathers blank in 3 years. The Brown Tini have been rolling in the chalk banks and carrying the lime away in their coats."));
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Letterer] Giedra"), "f_pilgrimroad_49");

		AddObjective("collectLime", L("Beat Lime Powder out of the Brown Tini coats"),
			new CollectItemObjective(662160, 10));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion

		AddDrop(662160, 0.45f, MonsterId.Tiny_Brown);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662160, character.Inventory.CountItem(662160), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662160, character.Inventory.CountItem(662160), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Nine Miles of South Trail
//-----------------------------------------------------------------------------

public class NineMilesOfSouthTrailQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1003);
		SetName(L("Nine Miles of South Trail"));
		SetType(QuestType.Sub);
		SetDescription(L("The Brown Tini have taken the last 2 miles of the south trail, and 2 markers have gone off that stretch - dug out foot and all. Kill 30 of them."));
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Trail-Warden] Kestas"), "f_pilgrimroad_49");

		AddObjective("killTini", L("Kill Brown Tini on the last 2 miles of south trail"),
			new KillObjective(30, new[] { MonsterId.Tiny_Brown }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Fourteen Hundred Names
//-----------------------------------------------------------------------------

public class FourteenHundredNamesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1004);
		SetName(L("Fourteen Hundred Names"));
		SetType(QuestType.Sub);
		SetDescription(L("The road register holds 1,400 names and which way each of them left Genar Field, and the marks are cut on 3 register stones at the road ends. The binder is 71 and has not reached one in 5 weeks."));
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Binder] Tomalov"), "f_pilgrimroad_49");

		AddObjective("readStones", L("Count the marks on all 3 register stones"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_49.Quest1004.StonesRead", 3, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_49.Quest1004.StonesRead");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_49.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_49.Quest1004.StonesRead");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_49.Quest1004.Stone{i}");
	}
}

// Quest 1005 CLASS: The Wall with the Letters Inward
//-----------------------------------------------------------------------------

public class TheWallWithTheLettersInwardQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_49", 1005);
		SetName(L("The Wall with the Letters Inward"));
		SetType(QuestType.Sub);
		SetDescription(L("40 finished markers are standing in a wall out on the Manahas road, courses laid true and every name turned to face inward. Open the wall and take down the 2 Tini Magicians standing behind it."));
		SetLocation("f_pilgrimroad_49");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Stonecutter] Antanas"), "f_pilgrimroad_49");

		AddPrerequisite(new CompletedPrerequisite("f_pilgrimroad_49", 1001));

		AddObjective("openWall", L("Kill Green Tini Archers on the wall courses"),
			new KillObjective(20, new[] { MonsterId.Tiny_Bow_Green }));

		AddObjective("killMagicians", L("Take down the 2 Magicians behind the wall"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Tiny_Mage, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Tiny_Bow_Green, 3),
				},
				resetIdent: "openWall",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(39000, 27000));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(203110, 1)); // Iron Fist
		AddReward(new ItemReward(640085, 3)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}
}
