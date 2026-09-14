//--- Melia Script ----------------------------------------------------------
// Escanciu Village Quest NPCs
//--- Description -----------------------------------------------------------
// A village that grew up inside the fence, five inscriptions standing around
// it, and a sixth in the square that somebody broke this month.
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

public class FRemains39QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Sixty-One Households
		//---------------------------------------------------------------------
		AddNpc(20118, L("[Village Elder] Moje"), "f_remains_39", 363, 144, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1001);

			dialog.SetTitle(L("Moje"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's bent over a household roll, running a thumb down a list of names longer than it should be*{/}"));
				await dialog.Msg(L("A stranger. In Escanciu. Sit down, don't sit down, I don't care — we get few enough of you that I've forgotten what's polite. 61 households when I took the roll. Nine walked out this spring, and every last one gave me the same non-answer: they simply did not want to be here anymore."));
				await dialog.Msg(L("Now the Gravegolems are hauling the broken stone out of my square, piece by piece, like carrion birds with better manners. Kill 25 of them, bring me back 8 pieces, before there's nothing left of it to put together."));

				var response = await dialog.Select(L("Will you go after the Gravegolems?"),
					Option(L("I'll recover 8 pieces"), "help"),
					Option(L("Why did 9 households leave?"), "info"),
					Option(L("It's only broken stone"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They take it east and they do not come back the same way. Follow the drag marks rather than the golems and you will find where the pieces are going."));
						break;

					case "info":
						await dialog.Msg(L("Because the square stone came down and nobody in this village will say out loud that it mattered. It has stood there since before Escanciu was a village."));
						await dialog.Msg(L("You do not put a settlement on a bare hill and then build 61 houses in a ring around a rock. You build the ring because the rock is why the hill is safe."));
						break;

					case "leave":
						await dialog.Msg(L("So is a wall. I have been elder 22 years and I can tell the difference between a thing falling down and a thing being taken down, and that stone was taken down."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGolems", out var killObj)) return;
				if (!quest.TryGetProgress("collectPieces", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He turns each piece until the broken edges face him, then sets them down in the shape they used to make*{/}"));
					await dialog.Msg(L("8, and that is most of a face. Linas can set that. 22 years and this is the first useful hour I have had since the spring."));
					await dialog.Msg(L("Take the village purse. 52 households paid into it against a bad winter and this is worse than a bad winter."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("East ground's quieter. Now go over it - the pieces are lying where the golems dropped them, not still being carried."));
				}
				else
				{
					await dialog.Msg(L("Still hauling. Clear them out first or they will carry off what you set down while your back is turned."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("2 of the 9 households have written asking whether the square stone is going back up. I have not answered yet, because I would like the answer to be yes."));
			}
		});

		// Quest 1002: Forty Years in the West Cut
		//---------------------------------------------------------------------
		AddNpc(20158, L("[Woodcutter] Ruoval"), "f_remains_39", -1265, 507, 277, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1002);

			dialog.SetTitle(L("Ruoval"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He lowers his axe mid-swing when he hears you on the path and doesn't pick it back up*{/}"));
				await dialog.Msg(L("You'll forgive me not finishing that cut — truth be told I'm glad of any excuse to put the axe down these days. Forty years I've cut the west stand. Thirty-nine of them, the Zolems out there were just rocks with moss on. A rock, mind — you walked past it, it stayed a rock, that was the whole arrangement between us."));
				await dialog.Msg(L("Now they walk. And each one's got a stone rattling around its middle that rings like a bell when you split it open. Bring me 10 of those, would you? I need to know whether I've gone mad, and I'd rather hear it from someone else's mouth."));

				var response = await dialog.Select(L("Will you split some open?"),
					Option(L("I'll bring you 10 stones"), "help"),
					Option(L("What do you think it is?"), "info"),
					Option(L("Cut somewhere else"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Hit them on the seam where the moss stops. Anywhere else and you will blunt an axe and annoy a rock."));
						break;

					case "info":
						await dialog.Msg(L("I think somebody is winding the whole valley up like a clock. The frogs came up out of the water, the golems came in off the east, and my rocks stood up."));
						await dialog.Msg(L("40 years is long enough to know that all 3 of those happening in one spring is not 3 things."));
						break;

					case "leave":
						await dialog.Msg(L("The west stand is the only stand. Everything else within a day of here is either the square or somebody's roof."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectStones", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He taps each one against his axe head and listens with his eyes shut*{/}"));
					await dialog.Msg(L("All 10 ring the same note. Rocks do not do that. Worked stone does that, and somebody worked 10 of these and put them inside 10 animals."));
					await dialog.Msg(L("Take my cutting money. I am not mad, which is worth more to me than the money is."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough to be sure with. Work the deep cut south of me - that is where they stood up first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I took one to Linas. He turned it over twice and said it was cut by the same hand as the square stone, and then he sat down on his own doorstep."));
			}
		});

		// Quest 1003: As Far As the Second Stone
		//---------------------------------------------------------------------
		AddNpc(20154, L("[Village Youth] Cahill"), "f_remains_39", 211, 425, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1003);

			dialog.SetTitle(L("Cahill"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's pacing a short line back and forth near the second stone, like he's daring himself to cross it*{/}"));
				await dialog.Msg(L("Oh — hah! Don't mind me, I'm just— you actually walked in from outside! Past the stones and everything! Nineteen years old and I've never once gone further than the second inscription. Nobody ever forbade it, we just don't, and until this spring I never once stopped to ask myself why that was!"));
				await dialog.Msg(L("But now the Winged Frogs are coming up out of the west water and into the lane, right past the line, bold as anything — so! Kill 25 of them, and I swear I'll walk out there myself and see what's actually on the other side."));

				var response = await dialog.Select(L("Will you clear the west lane?"),
					Option(L("I'll kill 25 Winged Frogs"), "help"),
					Option(L("Nobody stopped you?"), "info"),
					Option(L("Stay inside the stones"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They go up before they come at you, so they are always above you when they land. Watch the shadow, not the frog."));
						break;

					case "info":
						await dialog.Msg(L("No. That is the part I cannot get past. There is no rule, no story, no old woman warning anybody. 61 households and every single one of us just turns round at the stones."));
						await dialog.Msg(L("I asked my father and he had to sit and think about it and then he could not answer."));
						break;

					case "leave":
						await dialog.Msg(L("That is what everyone says and nobody can tell me why they say it. I have decided that is not a good enough reason for the rest of my life."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFrogs", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Lane's clear and I got as far as the third inscription and stood on the far side of it for a count of 20. Nothing happened at all."));
					await dialog.Msg(L("Take this. It is what I had saved to leave with, and I have decided I would rather stay and know why."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still coming up the lane. Work the water edge west of the inscription rather than the lane itself."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have walked to 4 of the 5 now. The fifth is up by the Fedimian road and I am saving it, which is a stupid thing to do and I am doing it anyway."));
			}
		});

		// Quest 1004: Agayla Fleury, Five Times
		//---------------------------------------------------------------------
		AddNpc(20152, L("[Stonewright] Linas"), "f_remains_39", 430, 421, 267, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1004);

			dialog.SetTitle(L("Linas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's running a thumbnail along a chisel mark, muttering a decade to himself before he notices you've stopped beside him*{/}"));
				await dialog.Msg(L("Give me a moment — no, actually, stay, tell me what you make of this, since you clearly have an eye. I cut stone for this village and I can date a chisel-mark to the decade, thank you very much. The 5 inscriptions round Escanciu were cut 300 years ago by one man, one season, one hand — I'd stake my reputation on it."));
				await dialog.Msg(L("The square stone was broken open this month, and a mason does not simply set a sixth stone without first confirming the other five are sound — that would be sloppy, and I am many things but I am not sloppy. Go to all 5 and look at the faces for me."));

				var response = await dialog.Select(L("Will you check the inscriptions?"),
					Option(L("I'll check all 5"), "help"),
					Option(L("One man cut 5 stones?"), "info"),
					Option(L("Set the sixth first"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Look at the base as much as the face. A stone that has been leaned on shows it at the foot 10 years before it shows anywhere else."));
						break;

					case "info":
						await dialog.Msg(L("One man cut 434, if the Stele Road clerks are right, and they are. I have seen a rubbing off the road and it is the same tooth, the same angle, the same tired left hand at the end of a long line."));
						await dialog.Msg(L("Nobody carves 434 stones for the dead. You carve 434 stones because you are drawing a shape and you need it to still be there in 300 years."));
						break;

					case "leave":
						await dialog.Msg(L("If the shape is broken somewhere else as well then setting the sixth mends nothing, and I will have spent a week telling 61 households it was fixed."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("checkStones", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to all 5 reports and draws the village in the dirt with the point of a chisel*{/}"));
					await dialog.Msg(L("All 5 sound, all 5 facing in, and the square stone in the middle of them. It is not a boundary at all - the 5 are a ring and the sixth is the middle of it."));
					await dialog.Msg(L("Take the mason's fee. I can set the sixth, and I would like the spirit in the west lane to be standing there when I do."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 5. 2 west of the village, 2 east of it, and 1 up by the Fedimian road."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("434 stones on one line up the road, and a ring of 5 around one village at the end of it. I have cut stone 30 years and I have never once been frightened by a shape before."));
			}
		});

		// Quest 1004 collection points - the Agayla Fleury inscriptions
		//---------------------------------------------------------------------
		void AddAgaylaStone(int stoneNumber, string observation, int x, int z, int direction)
		{
			AddNpc(47191, L("Agayla Fleury's Inscription"), "f_remains_39", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_remains_39", 1004);
				var variableKey = $"Laima.Quests.f_remains_39.Quest1004.Stone{stoneNumber}";
				var counterKey = "Laima.Quests.f_remains_39.Quest1004.StonesChecked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A standing inscription cut with the name AGAYLA FLEURY, its face turned toward the village*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already looked this one over*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Checking the face and the base..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var checkedCount = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, checkedCount);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Inscriptions checked: {0}/5", checkedCount));

					if (checkedCount >= 5)
						character.ServerMessage(L("{#FFD700}All 5 inscriptions checked. Return to Linas.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the inscription unchecked."));
				}
			});
		}

		AddAgaylaStone(1,
			L("South-West Stone: sound, base undisturbed, and the face is turned in toward the village square."), -623, -377, 350);
		AddAgaylaStone(2,
			L("West Stone: sound, and the line under the name reads 'and no further' with nothing after it."), -611, 214, 350);
		AddAgaylaStone(3,
			L("South-East Stone: sound, and somebody has been keeping the moss off the face for a long time."), 930, -98, 350);
		AddAgaylaStone(4,
			L("East Stone: sound, and the foot sits on a cut course that runs off west under the village."), 881, 467, 350);
		AddAgaylaStone(5,
			L("North Stone: sound, facing in like the rest, and its shadow at noon falls straight down the lane to the square."), 1001, 1294, 350);

		// The broken square stone
		//---------------------------------------------------------------------
		AddNpc(47192, L("Broken Square Stone"), "f_remains_39", 156, 172, 301, async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.HasCompleted(new QuestId("f_remains_39", 1005)))
			{
				await dialog.Msg(L("{#666666}*The stone stands again, set on its old foot, the joins packed with lead*{/}"));
				await dialog.Msg(L("{#666666}*The name is cut small and low, the way a man signs the last of 434: LUKAS OF ESCANCIU, MASON. And under it, the only date on any stone in the whole line*{/}"));
				return;
			}

			await dialog.Msg(L("{#666666}*The stump of a memorial in the middle of the square, snapped off a hand's width above the foot*{/}"));
			await dialog.Msg(L("{#666666}*The break is bright. Whatever did this was done within the month, and it was struck from the outside of the ring inward*{/}"));
		});

		// The soldier's pack
		//---------------------------------------------------------------------
		AddNpc(47161, L("Old Military Pack"), "f_remains_39", -766, 413, 330, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A soldier's pack rotted down to buckles and a mason's kit: 4 chisels, a wooden mallet, and a wax block worn to nothing*{/}"));
			await dialog.Msg(L("{#666666}*It has lain here a very long time and nothing has taken it apart, which for a leather pack in an open lane should not be possible*{/}"));
		});

		// Quest 1005: The Name That Was Struck Off
		//---------------------------------------------------------------------
		AddNpc(154017, L("[Wandering Spirit] The Mason"), "f_remains_39", -644, 479, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1005);

			dialog.SetTitle(L("Wandering Spirit"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_remains_39", 1004)))
				{
					await dialog.Msg(L("{#666666}*It turns toward you the moment you're close, as though it had been waiting for footsteps and not caring whose*{/}"));
					await dialog.Msg(L("{#666666}*Its mouth moves, and no name comes out of it*{/}"));
					await dialog.Msg(L("Have the stonewright check the 5 first. I cannot tell you which of them is wrong. I cannot tell you anything with a name in it."));
					return;
				}

				await dialog.Msg(L("{#666666}*It does not turn this time — it has clearly been standing here, waiting, since you were last seen*{/}"));
				await dialog.Msg(L("I cut 434 stones. Ruklys, Lydia Schaffen, Agayla Fleury - 3 friends who let me use their names, because a marker gets moved and a memorial does not, and I needed the line to hold longer than anybody's memory of why."));
				await dialog.Msg(L("The sixth is mine and it is the only real grave in 434 stones. Somebody broke it from the outside this month. Kill 20 Gravegolems to get the last pieces back off the east ground, and then stand with me while what has been leaning on the ring comes through it."));

				var response = await dialog.Select(L("Will you stand at the square?"),
					Option(L("I'll get the pieces and stand with you"), "help"),
					Option(L("What was the ring built around?"), "info"),
					Option(L("Say your own name"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650520, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Carry my sword. It is chipped because I cut 434 stones with a mason's kit and defended none of them with this, and I would like it to be there at the end."));
						break;

					case "info":
						await dialog.Msg(L("Nothing. That is what everybody gets wrong. The ring was not drawn around a thing - the ring is the safe ground, and the line up the road is the long side of it."));
						await dialog.Msg(L("61 households live inside it and not one of them knows they turn round at the stones because I made them do it 300 years ago."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*Its mouth moves for a long time and nothing comes*{/}"));
						await dialog.Msg(L("It is cut on the stone in the square, low down, the way a man signs the last one. When the stone stands I will be able to hear it said."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("recoverPieces", out var piecesObj)) return;
				if (!quest.TryGetProgress("killReaverpede", out var bossObj)) return;

				if (piecesObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("{#666666}*It watches Linas pack the joins with lead and does not move until the stone is standing*{/}"));
					await dialog.Msg(L("Lukas. That is the whole of it. 300 years and it took a stonewright, a woodcutter and a boy of 19 who would not stop asking why."));
					await dialog.Msg(L("Take the blade out of the square. It was under the foot of my stone and it is not mine - somebody put it there, and whoever burned the Ruklys face and blanked the garden stone is still walking the line taking it down. The ring holds tonight. It will not hold on its own."));

					character.Quests.Complete(questId);
				}
				else if (piecesObj.Done)
				{
					await dialog.Msg(L("The pieces are back. Now stand at the square, because it comes up through the broken foot and it will not wait for the lead to set."));
				}
				else
				{
					await dialog.Msg(L("Pieces first. A stone set short is a gap, and a gap in the ring is the same as no ring at all."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*It stands in the lane facing the square, and for the first time it is not walking*{/}"));
				await dialog.Msg(L("61 households and 9 of them coming back. Tell the epigraphers on the road what the shape is. They have the 434 and they do not have the middle."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Sixty-One Households
//-----------------------------------------------------------------------------

public class SixtyOneHouseholdsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1001);
		SetName(L("Sixty-One Households"));
		SetType(QuestType.Sub);
		SetDescription(L("The memorial in the Escanciu square was broken this spring and 9 households have walked out since. The Gravegolems are carrying the broken pieces off to the east ground."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Village Elder] Moje"), "f_remains_39");

		AddObjective("killGolems", L("Kill Gravegolems on the east ground"),
			new KillObjective(25, new[] { MonsterId.Gravegolem }));

		AddObjective("collectPieces", L("Recover Destroyed Tombstone Fragments"),
			new CollectItemObjective(650545, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(650545, 0.35f, MonsterId.Gravegolem);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650545, character.Inventory.CountItem(650545), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650545, character.Inventory.CountItem(650545), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Forty Years in the West Cut
//-----------------------------------------------------------------------------

public class FortyYearsInTheWestCutQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1002);
		SetName(L("Forty Years in the West Cut"));
		SetType(QuestType.Sub);
		SetDescription(L("For 39 years the Zolems in the west stand were rocks with moss on them. This spring they stood up, and each one carries a stone at its centre that rings when it is split."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Woodcutter] Ruoval"), "f_remains_39");

		AddObjective("collectStones", L("Recover Zolem Magic Stones from the west cut"),
			new CollectItemObjective(650761, 10));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion

		AddDrop(650761, 0.45f, MonsterId.Zolem);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650761, character.Inventory.CountItem(650761), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650761, character.Inventory.CountItem(650761), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: As Far As the Second Stone
//-----------------------------------------------------------------------------

public class AsFarAsTheSecondStoneQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1003);
		SetName(L("As Far As the Second Stone"));
		SetType(QuestType.Sub);
		SetDescription(L("Nobody in Escanciu walks past the inscriptions and nobody can say why. The Winged Frogs have come up out of the west water past the line and into the lane. Kill 25 of them."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Village Youth] Cahill"), "f_remains_39");

		AddObjective("killFrogs", L("Kill Winged Frogs in the west lane"),
			new KillObjective(25, new[] { MonsterId.Flying_Flog }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Agayla Fleury, Five Times
//-----------------------------------------------------------------------------

public class AgaylaFleuryFiveTimesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1004);
		SetName(L("Agayla Fleury, Five Times"));
		SetType(QuestType.Sub);
		SetDescription(L("5 inscriptions stand around Escanciu, all cut 300 years ago by one hand in one season. The stonewright will not try to set the broken sixth until he knows the other 5 are still whole."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stonewright] Linas"), "f_remains_39");

		AddObjective("checkStones", L("Check the faces and bases of all 5 inscriptions"),
			new VariableCheckObjective("Laima.Quests.f_remains_39.Quest1004.StonesChecked", 5, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_39.Quest1004.StonesChecked");

		for (var i = 1; i <= 5; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_39.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_39.Quest1004.StonesChecked");

		for (var i = 1; i <= 5; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_39.Quest1004.Stone{i}");
	}
}

// Quest 1005 CLASS: The Name That Was Struck Off
//-----------------------------------------------------------------------------

public class TheNameThatWasStruckOffQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1005);
		SetName(L("The Name That Was Struck Off"));
		SetType(QuestType.Sub);
		SetDescription(L("The mason who cut all 434 stones put his own name on the sixth, in the middle of the ring, and it is the only real grave in the whole line. Somebody broke it from the outside this month."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Wandering Spirit] The Mason"), "f_remains_39");

		AddPrerequisite(new CompletedPrerequisite("f_remains_39", 1004));

		AddObjective("recoverPieces", L("Kill Gravegolems holding the last of the pieces"),
			new KillObjective(20, new[] { MonsterId.Gravegolem }));

		AddObjective("killReaverpede", L("Hold the square against what comes up through the broken foot"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Reaverpede, 1) },
				resetIdent: "recoverPieces",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(16000, 11000));
		AddReward(new SilverReward(20000));
		AddReward(new ItemReward(123103, 1)); // Naktis
		AddReward(new ItemReward(640084, 3)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650520, character.Inventory.CountItem(650520), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650520, character.Inventory.CountItem(650520), InventoryItemRemoveMsg.Destroyed);
	}
}
