//--- Melia Script ----------------------------------------------------------
// Central Parias Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The Kupole sisters keep the last seed of the Divine Tree of Parias, and the
// central grove is where the seed's cradle and its sealing scroll are kept.
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

public class FMaple241QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Vivacious Grass
		//---------------------------------------------------------------------
		AddNpc(154011, L("[Kupole] Ilona"), "f_maple_24_1", 1402, 1444, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1001);

			dialog.SetTitle(L("Ilona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's on her knees at the edge of a grass bed, pressing bare soil flat with both palms*{/}"));
				await dialog.Msg(L("You walk quiet for someone that size - I didn't hear you at all. The Divine Tree of Parias died a hundred years ago and left exactly one seed. My sisters and I have kept it alive in this grass bed since, and the bed is failing."));
				await dialog.Msg(L("Cloverin have eaten the meadow down to soil. Kill 30 of them and bring me 10 lots of Vivacious Grass out of the ground they haven't reached yet."));

				var response = await dialog.Select(L("Will you clear the meadow?"),
					Option(L("I'll kill the Cloverin and gather the grass"), "help"),
					Option(L("Why keep a seed for a hundred years?"), "info"),
					Option(L("Plant it and be done"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The grass is under them, not around them. Whatever a Cloverin is standing on is what I want, so take the ground where they fall."));
						break;

					case "info":
						await dialog.Msg(L("Because a seed that had to be sealed is a seed somebody wanted. My sister Yulia keeps the wards in the south, Astra keeps the flower beds in the north, and I keep the seed."));
						await dialog.Msg(L("None of us has ever been told who sealed it. We were told to hold it and we are not the sort to ask twice."));
						break;

					case "leave":
						await dialog.Msg(L("Plant it and it grows, and whatever wanted it a hundred years ago gets a tree instead of a seed. That is not obviously an improvement."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCloverin", out var killObj)) return;
				if (!quest.TryGetProgress("collectGrass", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She shakes the soil off each handful and lays them in a ring*{/}"));
					await dialog.Msg(L("Ten, and every one of them still green at the root. The bed will hold another season on this."));
					await dialog.Msg(L("Take what the grove gives. We have no coin, but travellers leave things at the stone and none of it belongs to us."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The meadow is quieter and I have no grass. Take it from the bare ground where they were standing."));
				}
				else
				{
					await dialog.Msg(L("Still Cloverin in the meadow. There is no way to gather around them - they simply eat it as you cut."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The bed is green to the edge again. I sat with it all night, which is what I do, and it is the first night in a while that was only sitting."));
			}
		});

		// Quest 1002: The Holy Branch
		//---------------------------------------------------------------------
		AddNpc(154011, L("[Kupole] Ilona"), "f_maple_24_1", 428, -63, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1002);

			dialog.SetTitle(L("Ilona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's holding a length of pale wood up to the light, turning it slowly*{/}"));
				await dialog.Msg(L("Careful where you step near this stump - and while you're here, a moment of your time. The seed's cradle is cut from the Divine Tree's own wood, and there is no more of that wood being made. What fell when the tree died is all there will ever be."));
				await dialog.Msg(L("The Delione have been dragging the fallen branches into their hollows to build with. Bring me 6 Holy Branches back before they are chewed into bedding."));

				var response = await dialog.Select(L("Will you recover the branches?"),
					Option(L("I'll bring you 6 Holy Branches"), "help"),
					Option(L("Do they know what they're taking?"), "info"),
					Option(L("Cut a cradle from ordinary wood"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("A Holy Branch is heavier than it looks and it does not bend. If it bends, it is an ordinary branch and I do not want it."));
						break;

					case "info":
						await dialog.Msg(L("No. A Delione takes the straightest thing within reach and there is nothing straighter in this forest. They are not enemies of the grove, they are just tidy."));
						await dialog.Msg(L("I have spent a hundred years being furious at animals for behaving like animals. It has never once helped."));
						break;

					case "leave":
						await dialog.Msg(L("The cradle is not a box. It is the only thing the seed will consent to sleep in, and we have tried the alternative twice."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectBranches", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays each branch across her forearm and lets go of one end; not one of them dips*{/}"));
					await dialog.Msg(L("All 6 true. That is a new cradle and 2 spare, which is 2 more than we have had in 40 years."));
					await dialog.Msg(L("Take this from the stone. Somebody left it for the grove and the grove has never needed anything you can carry."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. Their hollows run along the southern edge, and they build with the straight wood at the bottom of the pile."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The new cradle is cut and the old one is on the fire, which is the only respectful thing to do with it."));
			}
		});

		// Quest 1003: Elavine Under the Roots
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Woodcutter] Petras"), "f_maple_24_1", -840, 1150, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1003);

			dialog.SetTitle(L("Petras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's got an ear pressed to a tree trunk, waving you quiet with the other hand*{/}"));
				await dialog.Msg(L("Listen to that - hear it? No? Good ears aren't a given out here. I've cut deadwood in this forest 30 years and the nymphs have never once complained, because I only take what's already down. That's the arrangement."));
				await dialog.Msg(L("The Rudas Elavine have got under the root plates in the western stand and they're pushing the standing trees over. Kill 30 of them before they make my whole arrangement pointless."));

				var response = await dialog.Select(L("Will you go into the western stand?"),
					Option(L("I'll kill the Rudas Elavine"), "help"),
					Option(L("Pushing trees over?"), "info"),
					Option(L("Take the deadwood while it lasts"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come up out of the leaf litter, so watch the ground and not the trees. Everybody gets that wrong once."));
						break;

					case "info":
						await dialog.Msg(L("They tunnel under the root plate and the plate loses its grip. Next wind, the tree goes, and it goes without warning because the trunk's perfectly healthy."));
						await dialog.Msg(L("Lost a man in Rokas to that when I was young. Big oak, no rot in it, straight over in a breeze."));
						break;

					case "leave":
						await dialog.Msg(L("That's what a younger man would do. I've got 30 years in this wood and I'd like there to be a wood when I stop."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killElavine", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Walked the western stand this morning and put my shoulder against 6 trunks. Not one of them gave."));
					await dialog.Msg(L("Take my season's cut money. I'll make it back and I'd have made none of it with the stand on the ground."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still Elavine under the plates. You'll hear them before you see them - it's a sound like rain underground."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Kupole Ilona came and stood at the edge of my cutting yesterday. Didn't say a word, just looked at the stand and nodded and left. Thirty years and that's the first time."));
			}
		});

		// Quest 1004: The Sealing Scroll
		//---------------------------------------------------------------------
		AddNpc(154012, L("[Kupole] Yulia"), "f_maple_24_1", 577, -64, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1004);

			dialog.SetTitle(L("Yulia"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's holding a scroll at arm's length, studying it the way you'd study a wound*{/}"));
				await dialog.Msg(L("You'll do - better a stranger than my sister right now. I keep the wards in the southern forest and I've come up to say something Ilona does not want to hear: the sealing scroll on the seed is going pale."));
				await dialog.Msg(L("It recharges off the ward stones, 3 of them, spread round the central grove. Carry the scroll to each stone and press it flat against the face."));

				var response = await dialog.Select(L("Will you carry the scroll?"),
					Option(L("I'll press it at all 3 ward stones"), "help"),
					Option(L("Why doesn't Ilona want to hear it?"), "info"),
					Option(L("Write a new scroll"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(667240, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Flat against the face, and hold it until the ink darkens. If you lift it early the stone gives you nothing and you cannot ask it twice."));
						break;

					case "info":
						await dialog.Msg(L("Because a scroll going pale means the thing under it is pushing. She has kept that seed a hundred years by believing it sleeps."));
						await dialog.Msg(L("I have kept the wards the same hundred years by believing it does not. One of us is going to be wrong very publicly."));
						break;

					case "leave":
						await dialog.Msg(L("Nobody alive knows the hand it was written in. We can recharge it. We cannot replace it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("pressScroll", out var pressObj)) return;

				if (pressObj.Done)
				{
					await dialog.Msg(L("{#666666}*She holds the scroll up and the ink is black to the edges*{/}"));
					await dialog.Msg(L("Full. That buys the grove 3 years, maybe 5, and it does not answer a single question I actually have."));
					await dialog.Msg(L("Take this. It came off a traveller who died at the southern ward 60 years ago and I have carried it since out of a sense of obligation I cannot explain."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Stones still unpressed. All 3, and in one carry - a scroll charged at 2 stones and left overnight loses both."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ilona has seen the scroll and said nothing about it, which from her is a whole conversation."));
			}
		});

		// Quest 1004 interaction points - the grove ward stones
		//---------------------------------------------------------------------
		void AddWardStone(int stoneNumber, string observation, int x, int z, int direction)
		{
			AddNpc(47190, L("Grove Ward Stone"), "f_maple_24_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_maple_24_1", 1004);
				var variableKey = $"Laima.Quests.f_maple_24_1.Quest1004.Stone{stoneNumber}";
				var counterKey = "Laima.Quests.f_maple_24_1.Quest1004.StonesPressed";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A low ward stone with a smooth pressing face*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This stone has already given what it had*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Pressing the scroll..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var pressed = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, pressed);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Ward stones pressed: {0}/3", pressed));

					if (pressed >= 3)
						character.ServerMessage(L("{#FFD700}The scroll is black to the edges. Return to Kupole Yulia.{/}"));
				}
				else
				{
					character.ServerMessage(L("You lift the scroll too early and the stone goes quiet."));
				}
			});
		}

		AddWardStone(1, L("The ink takes at the first stone and runs a third of the way up the scroll."), 1062, 695, 225);
		AddWardStone(2, L("The second stone is warm and gives its charge without hesitating."), 494, -696, 0);
		AddWardStone(3, L("The third stone takes a long moment, then floods the scroll black."), -770, 1121, 90);

		// Quest 1005: Holding the Cradle
		//---------------------------------------------------------------------
		AddNpc(154011, L("[Kupole] Ilona"), "f_maple_24_1", 383, 1403, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1005);

			dialog.SetTitle(L("Ilona"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_maple_24_1", 1004)))
				{
					await dialog.Msg(L("My sister has you carrying a scroll round the ward stones. Finish that first. Nothing else on this ground matters until the seal is black."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's already moving when she sees you, waving you over without breaking stride*{/}"));
				await dialog.Msg(L("Good, you came back - I need the extra pair of hands more than I want to admit. Yulia was right and I was wrong, and I've had about an hour to be gracious about it. The seed has to be moved to the new cradle tonight, seal or no seal."));
				await dialog.Msg(L("The moment it is out of the old bed, every Atti in the central grove will come for it. Kill 25 of them first, then stand over the cradle while I do the work."));

				var response = await dialog.Select(L("Will you stand over the cradle?"),
					Option(L("I'll clear the grove and hold the cradle"), "help"),
					Option(L("The Atti want the seed?"), "info"),
					Option(L("Wait for a better night"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They will come in low and all at once. Keep the cradle behind you and do not chase - the moment you step off it they will go round you, and they are faster than you."));
						break;

					case "info":
						await dialog.Msg(L("Not want. They are called. That is the part that has kept Yulia awake for a century and that I have been declining to look at."));
						await dialog.Msg(L("Something has been calling the small things of this forest toward that seed for a hundred years, patiently, and patience is a thing I understand very well."));
						break;

					case "leave":
						await dialog.Msg(L("The cradle is cut and the old bed is failing. There is no better night and there will not be one."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearGrove", out var groveObj)) return;
				if (!quest.TryGetProgress("holdCradle", out var holdObj)) return;

				if (groveObj.Done && holdObj.Done)
				{
					await dialog.Msg(L("It is in the new cradle and the seal is black and the grove is quiet. I have not seen it quiet since the tree came down."));
					await dialog.Msg(L("Take these. A traveller left them at the stone 60 years ago and Yulia has been waiting for a reason to hand them on."));

					character.Quests.Complete(questId);
				}
				else if (groveObj.Done)
				{
					await dialog.Msg(L("The grove is clear. Get to the cradle - what comes next will not come from the grove."));
				}
				else
				{
					await dialog.Msg(L("Too many Atti still in the grove. I cannot lift the seed with that many of them awake to it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Yulia has gone back south to walk her wards and she took the old cradle's ashes with her. She would not say why and I have decided not to ask."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Vivacious Grass
//-----------------------------------------------------------------------------

public class VivaciousGrassQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1001);
		SetName(L("Vivacious Grass"));
		SetType(QuestType.Sub);
		SetDescription(L("The last seed of the Divine Tree of Parias has been kept alive in a bed of Vivacious Grass for a hundred years, and the Cloverin have eaten the meadow down to soil. Clear them and gather what is left."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kupole] Ilona"), "f_maple_24_1");

		AddObjective("killCloverin", L("Kill Cloverin in the northern meadow"),
			new KillObjective(30, new[] { MonsterId.Cloverin }));

		AddObjective("collectGrass", L("Collect Vivacious Grass"),
			new CollectItemObjective(667242, 10));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(667242, 0.50f, MonsterId.Cloverin);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667242, character.Inventory.CountItem(667242), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667242, character.Inventory.CountItem(667242), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: The Holy Branch
//-----------------------------------------------------------------------------

public class TheHolyBranchQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1002);
		SetName(L("The Holy Branch"));
		SetType(QuestType.Sub);
		SetDescription(L("The seed's cradle can only be cut from the Divine Tree's own fallen wood, and no more of it is being made. Delione have been dragging the branches into their hollows to build with."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kupole] Ilona"), "f_maple_24_1");

		AddObjective("collectBranches", L("Recover Holy Branches from Delione hollows"),
			new CollectItemObjective(667243, 6));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(667243, 0.45f, MonsterId.Delione);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667243, character.Inventory.CountItem(667243), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667243, character.Inventory.CountItem(667243), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Elavine Under the Roots
//-----------------------------------------------------------------------------

public class ElavineUnderTheRootsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1003);
		SetName(L("Elavine Under the Roots"));
		SetType(QuestType.Sub);
		SetDescription(L("Rudas Elavine have tunnelled under the root plates of the western stand. The trunks are healthy and will go over in the first real wind, without warning. Kill them before they do."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Woodcutter] Petras"), "f_maple_24_1");

		AddObjective("killElavine", L("Kill Rudas Elavine in the western stand"),
			new KillObjective(30, new[] { MonsterId.Rudas_Elavine }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1004 CLASS: The Sealing Scroll
//-----------------------------------------------------------------------------

public class TheSealingScrollQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1004);
		SetName(L("The Sealing Scroll"));
		SetType(QuestType.Sub);
		SetDescription(L("The sealing scroll on the Divine Tree's seed is going pale, and a pale seal means the thing beneath it is pushing. Carry the scroll to all 3 grove ward stones in one trip and press it flat."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kupole] Yulia"), "f_maple_24_1");

		AddObjective("pressScroll", L("Press the sealing scroll at the 3 grove ward stones"),
			new VariableCheckObjective("Laima.Quests.f_maple_24_1.Quest1004.StonesPressed", 3, true));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667240, character.Inventory.CountItem(667240), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_1.Quest1004.StonesPressed");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_1.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667240, character.Inventory.CountItem(667240), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_1.Quest1004.StonesPressed");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_1.Quest1004.Stone{i}");
	}
}

// Quest 1005 CLASS: Holding the Cradle
//-----------------------------------------------------------------------------

public class HoldingTheCradleQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1005);
		SetName(L("Holding the Cradle"));
		SetType(QuestType.Sub);
		SetDescription(L("The seed must be moved to its new cradle tonight, and the moment it leaves the old bed every Atti in the central grove will come for it. Clear the grove, then stand over the cradle."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Kupole] Ilona"), "f_maple_24_1");

		AddPrerequisite(new CompletedPrerequisite("f_maple_24_1", 1004));

		AddObjective("clearGrove", L("Kill Atti in the central grove"),
			new KillObjective(25, new[] { MonsterId.Atti }));

		AddObjective("holdCradle", L("Hold the cradle while the seed is moved"),
			new LayeredKillObjective(
				spawnList: new[] {
					new KillSpec(MonsterId.Atti, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Delione, 3),
				},
				resetIdent: "clearGrove",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(503103, 1)); // Vine Gloves
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}
}
