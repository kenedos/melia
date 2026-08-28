//--- Melia Script ----------------------------------------------------------
// Goddess' Ancient Garden Quest NPCs
//--- Description -----------------------------------------------------------
// Five memorial stones cut for one woman, spread across a garden, and a
// necromancer who came to ask her a question and got nothing back.
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

public class FRemains38QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: What the Roots Bring Up
		//---------------------------------------------------------------------
		AddNpc(20114, L("[Rubbing-Clerk] Egle"), "f_remains_38", -1390, -2182, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1001);

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's crouched over a spread of paper, laying one rubbing beside another without looking up*{/}"));
				await dialog.Msg(L("Mind the edges! Don't put a boot through my paper — two days I've carried these flat, and I am not starting over for anybody's boot. Ruta sent 4 rubbings down from the Stele Road ahead of me. They say the road stones are a boundary. I came to see where it goes."));
				await dialog.Msg(L("It goes under the Long-Branched Trees. Rude of them, honestly — rooting up the older fallen stones and carrying the pieces about like it's nothing. Kill 25 and bring me 8 fragments, and mind you don't crease them."));

				var response = await dialog.Select(L("Will you go into the tree ground?"),
					Option(L("I'll bring you 8 fragments"), "help"),
					Option(L("What does a boundary stone say?"), "info"),
					Option(L("Take the rubbings back"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The pieces sit up in the root ball, not on the ground. Look at what falls out of them, not at what they were standing on."));
						break;

					case "info":
						await dialog.Msg(L("'From this stone to the next, and no further.' A name, that line, and no date of death on any of them."));
						await dialog.Msg(L("You do not carve a memorial without a date. You carve a marker without a date, and then you put a name on it so that nobody will ever move it."));
						break;

					case "leave":
						await dialog.Msg(L("Two days I walked with these! I am not walking two more back just to tell a pair of stubborn men they were both slightly wrong."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTrees", out var killObj)) return;
				if (!quest.TryGetProgress("collectFragments", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays each fragment beside a rubbing from her satchel and matches 3 of them by the letter shapes alone*{/}"));
					await dialog.Msg(L("Same hand as the road. Same day, near enough. Whoever cut the Stele Road cut this garden, and they did it in one go."));
					await dialog.Msg(L("Take the field allowance. It is meant for a cart and I have been walking."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Ground's clear. Now go back over the root balls - the fragments drop out of them when the tree comes down."));
				}
				else
				{
					await dialog.Msg(L("Still too many standing. They will simply pick the pieces back up while you are looking at the last one."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have written to Ruta that the boundary crosses into the garden and does not stop. She will already know. She usually does and never says so first."));
			}
		});

		// Quest 1002: Eleven Years of Lizardmen
		//---------------------------------------------------------------------
		AddNpc(47245, L("[Hunter] Talus"), "f_remains_38", 563, -303, 283, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1002);

			dialog.SetTitle(L("Talus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's crouched low over a print in the mud, reading it the way most people read a letter*{/}"));
				await dialog.Msg(L("Ha! Snuck up on a hunter, did you — not many manage that. Eleven years I've worked this garden. I could find you every den in it blindfolded, and for nine of those years the Lizardmen held the water, same as any Lizardman does. Predictable animals, Lizardmen."));
				await dialog.Msg(L("Not anymore, though. Two seasons now they've held a line instead, and every one I gut has a stone in it that it sure as hell didn't grow. Bring me 10 of those stones and I'll owe you a hunter's respect, which isn't handed out cheap."));

				var response = await dialog.Select(L("Will you get the stones?"),
					Option(L("I'll bring you 10 stones"), "help"),
					Option(L("What line are they holding?"), "info"),
					Option(L("Stones happen"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They do not scatter when one goes down, which is new and which is useful. Take the outside of a group and work inward and they will stand there and let you."));
						break;

					case "info":
						await dialog.Msg(L("A straight one. West bank to east bank across the whole garden, and every animal on it faces the same way, which is inward."));
						await dialog.Msg(L("An animal does not hold a line. An animal holds ground it wants. There is nothing on that line any Lizardman has ever wanted."));
						break;

					case "leave":
						await dialog.Msg(L("Ha! Tell that to my knife. Eleven years I've gutted these things, and I found nothing before two seasons ago and stones in every one since. Stones don't happen. Believe an old hunter."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectStones", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He rolls the stones out on a hide and pushes them into a line with the back of his knife*{/}"));
					await dialog.Msg(L("10 stones, 10 the same. Cut, not river-worn - somebody made these and somebody fed them to my garden."));
					await dialog.Msg(L("Take the hunter's price. I get paid by the hide and I have not been able to sell a Lizardman hide since they started tasting of this."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough to say it with. Work the west bank - that end of the line is packed thickest."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The necromancer wanted all 10 and I gave him 9. I am keeping the last one, because a man who wants all of a thing is a man I want to still have one of."));
			}
		});

		// Quest 1003: Nine Paths
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Ruin-Warden] Tadas"), "f_remains_38", 1516, -228, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1003);

			dialog.SetTitle(L("Tadas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's leaning on his billhook, glaring down the nearest shut path like it personally offended him*{/}"));
				await dialog.Msg(L("Oh, wonderful. Another traveler. Well, you can walk further than these knees can these days, so pay attention — 9 paths through this garden, and I keep all 9. Kept, mind you. Past tense. Twenty good years at this post and now it's 4 impassable paths and one old man with a billhook and a bad attitude."));
				await dialog.Msg(L("The Infroburks filled the east hollow and won't be walked around, the ungrateful lumps. Kill 30 of them and I get my 4 paths back, and maybe my dignity along with them."));

				var response = await dialog.Select(L("Will you clear the east hollow?"),
					Option(L("I'll kill 30 Infroburks"), "help"),
					Option(L("Why the hollow?"), "info"),
					Option(L("Reroute the paths"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They bank up the heat before they come at you and you can see it happen. Move on the tell, not on the charge - the charge is too late."));
						break;

					case "info":
						await dialog.Msg(L("It is the low ground and it is warm and there is nothing in it. They came in from the east 2 seasons ago and they have not gone up onto the ridge once."));
						await dialog.Msg(L("The hunter says his Lizardmen face inward. Mine are packed into the lowest hole on the map and will not leave it, and I do not think those are 2 different facts."));
						break;

					case "leave":
						await dialog.Msg(L("Oh, just reroute them, he says! Simple as that! There are 9 lines across this garden a cart can actually follow, and I did not choose a single one of them — they were chosen 300 years ago by people who never had to fight an Infroburk for the privilege."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killInfroburk", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Walked all 4 with a barrow this morning. First time in 2 seasons I have finished a round and had nothing to write in the book."));
					await dialog.Msg(L("Take the warden's stipend. It has been accumulating on the grounds that there was nothing to spend it on and 4 shut paths."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still packed in down there. Work the hollow floor rather than the lip - on the lip they come up at you from below."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("9 paths, all 9 open. And with the hollow empty you can see the ground down there is cut flat, which is not something a hollow does by itself."));
			}
		});

		// Quest 1004: Five Stones, One Name
		//---------------------------------------------------------------------
		AddNpc(20138, L("[Stone-Counter] Zita"), "f_remains_38", 1446, 1513, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1004);

			dialog.SetTitle(L("Zita"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's muttering a tally under her breath, tapping a finger against her notebook with each count*{/}"));
				await dialog.Msg(L("—five! Five, I said five, didn't I— oh, you made me lose count twice just walking up, but it's five, I'm sure of it now. The archive sent me to count the memorials here and report the figure, and every single one says LYDIA SCHAFFEN, and there is only ever supposed to be one of anybody!"));
				await dialog.Msg(L("Four of them can be walked to — I've checked, I've checked twice — go and take a rubbing off all 4 so I can send the archive something they can't just file and forget about, hm? Please?"));

				var response = await dialog.Select(L("Will you rub the four stones?"),
					Option(L("I'll do all 4 stones"), "help"),
					Option(L("What about the fifth?"), "info"),
					Option(L("Five graves, five Lydias"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Get the bottom edge as well as the face. The line under the name is the part the archive will pretend it cannot read."));
						break;

					case "info":
						await dialog.Msg(L("The fifth is on the west bank in the middle of the Lizardmen and I have watched them for a week. They do not sleep on it and they do not walk on it. They stand around it."));
						await dialog.Msg(L("I count things. 4 stones anybody can reach and 1 that has an animal cordon on it is a figure that means something."));
						break;

					case "leave":
						await dialog.Msg(L("Then the parish rolls are wrong! They say one, I count five, and someone in this arrangement is very mistaken and it is not me — I checked, twice, and the mason cut all 5 in the same week, which is a very odd way to bury a family, don't you think?"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("rubStones", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*She sets the 4 rubbings out in the order they stand and traces the line between them with a finger*{/}"));
					await dialog.Msg(L("They are not scattered. Corner, corner, corner, corner - and the fifth sits where the 2 long sides would cross."));
					await dialog.Msg(L("Take the counting fee. I am sending the archive a drawing instead of a figure, which will annoy them, which is the point."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 4 yet. One in the west trees, one on the south path, one above the east hollow, one up here by the village gate."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The archive wrote back within the week, which they have never done. They asked me to stop counting and come home, and I have written back asking why."));
			}
		});

		// Quest 1004 collection points - the Lydia Schaffen stones
		//---------------------------------------------------------------------
		void AddLydiaStone(int stoneNumber, string observation, int x, int z, int direction)
		{
			AddNpc(47190, L("Lydia Schaffen's Memorial"), "f_remains_38", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_remains_38", 1004);
				var variableKey = $"Laima.Quests.f_remains_38.Quest1004.Stone{stoneNumber}";
				var counterKey = "Laima.Quests.f_remains_38.Quest1004.StonesRubbed";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A memorial stone cut with the name LYDIA SCHAFFEN and no date*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already took this rubbing*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Taking the rubbing..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var rubbed = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, rubbed);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Stones rubbed: {0}/4", rubbed));

					if (rubbed >= 4)
						character.ServerMessage(L("{#FFD700}All 4 reachable stones rubbed. Return to Zita.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the stone untouched."));
				}
			});
		}

		AddLydiaStone(1,
			L("West Trees: LYDIA SCHAFFEN, and beneath it 'from this stone to the next'. The face points east."), -756, -968, 120);
		AddLydiaStone(2,
			L("South Path: the same name and the same line, and the face points north."), 212, -1347, 22);
		AddLydiaStone(3,
			L("Above the East Hollow: the same again, facing west, and the base sits on a cut foundation course."), 1297, -675, 358);
		AddLydiaStone(4,
			L("Village Gate: the same, facing south, and 'and no further' finished off underneath."), 1429, 908, 353);

		// The offering jar
		//---------------------------------------------------------------------
		AddNpc(47258, L("Offering Jar"), "f_remains_38", -78, 424, 288, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A wide clay jar set into the ground where the garden's paths cross, with fresh water standing in it*{/}"));
			await dialog.Msg(L("{#666666}*Somebody has been filling it. There is no path to it and no track around it, and the water has not been left long enough to go green*{/}"));
		});

		// Quest 1005: Where the Sides Cross
		//---------------------------------------------------------------------
		AddNpc(154022, L("[Necromancer] Drasius"), "f_remains_38", -1053, -1249, 18, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1005);

			dialog.SetTitle(L("Drasius"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_remains_38", 1004)))
				{
					await dialog.Msg(L("{#666666}*He does not turn from the memorial he's facing, though he has plainly heard you approach*{/}"));
					await dialog.Msg(L("Let the counter finish her 4 stones. I would rather walk in there knowing the shape of the thing than knowing my own opinion of it."));
					return;
				}

				await dialog.Msg(L("{#666666}*He turns at last, unhurried, as though he'd been expecting someone eventually*{/}"));
				await dialog.Msg(L("I came to this garden to put one question to Lydia Schaffen and I have put it at 4 memorials over 3 weeks. Nothing has answered. Not refused - absent. There is nobody under any of them."));
				await dialog.Msg(L("The counter says the fifth stone stands where the 2 long sides cross, and the Lizardmen stand around it and will not tread on it. Kill 20 of them to open the ground, then deal with what is rooted on the stone itself."));

				var response = await dialog.Select(L("Will you go to the fifth stone?"),
					Option(L("I'll open the fifth stone"), "help"),
					Option(L("Nobody is buried here at all?"), "info"),
					Option(L("Leave it standing"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650755, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Take the rubbing tool. Whatever is on that face, I want it on wax before either of us decides what it says."));
						break;

					case "info":
						await dialog.Msg(L("Not one. 5 stones, 3 names, 431 more of them up the road, and no remains anywhere in the whole line. They are not memorials. They are a fence with names on it."));
						await dialog.Msg(L("Somebody used real people to build it, because a marker gets moved and a memorial does not. I would like to know who was worth 434 stones of politeness."));
						break;

					case "leave":
						await dialog.Msg(L("Somebody burned the Ruklys face off this year. The fence is being taken down a stone at a time by a hand that is not mine, and I would prefer to be early."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openGround", out var groundObj)) return;
				if (!quest.TryGetProgress("killDevilglove", out var bossObj)) return;

				if (groundObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("{#666666}*He pulls the wax off the fifth face and reads it twice before he lets you see it*{/}"));
					await dialog.Msg(L("No name. The 5th stone has the line and no name at all, and the space where a name goes has been left blank and polished."));
					await dialog.Msg(L("Take the necklace off the root ball - it is older than the garden and it was not buried, it was set there. I am going on to Escanciu. There are 5 Agayla Fleury stones in that village and a 6th somebody smashed, and I intend to reach it before the burner does."));

					character.Quests.Complete(questId);
				}
				else if (groundObj.Done)
				{
					await dialog.Msg(L("The cordon is broken. What is rooted on the stone did not move when they died, which tells you which of the 2 was holding the other."));
				}
				else
				{
					await dialog.Msg(L("Too many still standing round it. Open the ground first - you will not get a rubbing done with a cordon on your back."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I put the question to the blank stone. Something answered, and it did not give a name, and I have written down exactly what it did say and nothing else."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: What the Roots Bring Up
//-----------------------------------------------------------------------------

public class WhatTheRootsBringUpQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1001);
		SetName(L("What the Roots Bring Up"));
		SetType(QuestType.Sub);
		SetDescription(L("The Stele Road's boundary formula carries on into the garden. The Long-Branched Trees have rooted up the older fallen stones and carry the pieces about in their root balls."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Rubbing-Clerk] Egle"), "f_remains_38");

		AddObjective("killTrees", L("Kill Long-Branched Trees on the west ground"),
			new KillObjective(25, new[] { MonsterId.Long_Arm }));

		AddObjective("collectFragments", L("Recover Broken Tombstone Fragments"),
			new CollectItemObjective(650542, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(650542, 0.35f, MonsterId.Long_Arm);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650542, character.Inventory.CountItem(650542), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650542, character.Inventory.CountItem(650542), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Eleven Years of Lizardmen
//-----------------------------------------------------------------------------

public class ElevenYearsOfLizardmenQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1002);
		SetName(L("Eleven Years of Lizardmen"));
		SetType(QuestType.Sub);
		SetDescription(L("For 9 years the garden's Lizardmen held the water. For 2 seasons they have held a straight line from bank to bank instead, and every one carries a cut stone it did not grow."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hunter] Talus"), "f_remains_38");

		AddObjective("collectStones", L("Recover Lizardman Mind Control Stones"),
			new CollectItemObjective(650753, 10));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion

		AddDrop(650753, 0.45f, MonsterId.Lizardman);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650753, character.Inventory.CountItem(650753), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650753, character.Inventory.CountItem(650753), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Nine Paths
//-----------------------------------------------------------------------------

public class NinePathsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1003);
		SetName(L("Nine Paths"));
		SetType(QuestType.Sub);
		SetDescription(L("The garden has 9 paths and a warden who keeps all 9. The Infroburks have filled the east hollow and shut 4 of them. Kill 30 of them."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ruin-Warden] Tadas"), "f_remains_38");

		AddObjective("killInfroburk", L("Kill Infroburks in the east hollow"),
			new KillObjective(30, new[] { MonsterId.InfroBurk }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Five Stones, One Name
//-----------------------------------------------------------------------------

public class FiveStonesOneNameQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1004);
		SetName(L("Five Stones, One Name"));
		SetType(QuestType.Sub);
		SetDescription(L("The archive wants a count of the garden's memorials. The count is 5, all of them cut LYDIA SCHAFFEN in the same week, and 4 of them can be walked to."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stone-Counter] Zita"), "f_remains_38");

		AddObjective("rubStones", L("Take rubbings from the 4 reachable memorials"),
			new VariableCheckObjective("Laima.Quests.f_remains_38.Quest1004.StonesRubbed", 4, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_38.Quest1004.StonesRubbed");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_38.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_38.Quest1004.StonesRubbed");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_38.Quest1004.Stone{i}");
	}
}

// Quest 1005 CLASS: Where the Sides Cross
//-----------------------------------------------------------------------------

public class WhereTheSidesCrossQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1005);
		SetName(L("Where the Sides Cross"));
		SetType(QuestType.Sub);
		SetDescription(L("The 4 reachable memorials stand at 4 corners and the fifth sits where the long sides would cross. The Lizardmen cordon it and will not tread on it, and a Cursed Devilglove is rooted on the stone itself."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Necromancer] Drasius"), "f_remains_38");

		AddPrerequisite(new CompletedPrerequisite("f_remains_38", 1004));

		AddObjective("openGround", L("Kill Lizardmen cordoning the fifth stone"),
			new KillObjective(20, new[] { MonsterId.Lizardman }));

		AddObjective("killDevilglove", L("Defeat the Cursed Devilglove on the stone"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Devilglove, 1) },
				resetIdent: "openGround",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(16000, 11000));
		AddReward(new SilverReward(20000));
		AddReward(new ItemReward(583103, 1)); // Electus
		AddReward(new ItemReward(640084, 3)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650755, character.Inventory.CountItem(650755), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650755, character.Inventory.CountItem(650755), InventoryItemRemoveMsg.Destroyed);
	}
}
