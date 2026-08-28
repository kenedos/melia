//--- Melia Script ----------------------------------------------------------
// Salvia Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The first waystation on the pilgrim road to Mavern Abbey, where the roadside
// altar has been emptied and the Salvia bramble has stopped keeping its dead.
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

public class FPilgrimroad412QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Cases from Thaumas
		//---------------------------------------------------------------------
		AddNpc(155043, L("[Friar] Brutus"), "f_pilgrimroad_41_2", -149, -55, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1001);

			dialog.SetTitle(L("Brutus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He straightens up from a row of bodies laid on the ground, wiping his hands on a rag that was clean this morning*{/}"));
				await dialog.Msg(L("You're standing and walking — that already puts you ahead of most people I've spoken to today. No offense meant, none taken, I hope. This was a waystation. Roof, well, two benches. Now it's a hospital with 31 people on the ground and 4 doses of medicine left in the box. Funny how a title changes."));
				await dialog.Msg(L("The rest is out on the west road in cracked cases — the Green Tini Magicians learned what a sealed case is worth. Kill 20 of them, bring me back 6 doses. Quickly, if you don't mind. I'm rationing hours same as medicine."));

				var response = await dialog.Select(L("Will you go out to the cases? I'd go myself, but then who patches the 31?"),
					Option(L("I'll get the medicine back"), "help"),
					Option(L("Why are there 31 people here?"), "info"),
					Option(L("Send to the abbey for more"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Look for the wax. The doses are stoppered in green wax and the Magicians can't get it off, so they carry them around unopened. Small mercy — take it."));
						break;

					case "info":
						await dialog.Msg(L("Because the altar at the east bend went bad, and every walker who touched it on the way past has been carried back here since."));
						await dialog.Msg(L("I'm not going to call it a curse to your face. I will say I've got 31 people and not one of them fell ill anywhere else. Draw your own conclusion — I already have."));
						break;

					case "leave":
						await dialog.Msg(L("I sent. Eleven days ago. The road between here and the abbey has Banshees on it, which is presumably why nothing's come back down it. Presumably. I try not to think too hard about the alternative."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMagicians", out var killObj)) return;
				if (!quest.TryGetProgress("collectMedicine", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He holds each dose up and checks the wax seal before it goes in the box*{/}"));
					await dialog.Msg(L("Six unbroken. That's 10 in the box, which buys me five days instead of two. Practically a holiday."));
					await dialog.Msg(L("Take the waystation purse. It was set aside for roof repairs. The roof will simply have to keep leaking a while longer — priorities."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Road's quieter. Now find the doses — they'll be on the ground where the cases were opened, not on the Magicians. They can't open what they can't unwrap, bless their little claws."));
				}
				else
				{
					await dialog.Msg(L("Still Magicians on the west road. Clear them first, or they'll carry the rest off while you're busy picking up after them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four of the 31 sat up this week. One asked for bread — first thing anyone here's asked me for that I could actually give. I nearly wept over a heel of bread. It's been that kind of month."));
			}
		});

		// Quest 1002: What the Bramble Keeps
		//---------------------------------------------------------------------
		AddNpc(155033, L("[Pilgrim] Diane"), "f_pilgrimroad_41_2", -283, 538, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1002);

			dialog.SetTitle(L("Diane"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She doesn't look up from her needle, just shifts sideways to let you see the pile of half-finished coats*{/}"));
				await dialog.Msg(L("Sit if you want. I don't mind an audience, so long as you don't expect conversation — I sew. That's all I've done since I got here. Nine nights out under the trees, two blankets between 31 people. So I sew."));
				await dialog.Msg(L("The Black Banshees drag what they kill into the Salvia bramble, leave the hides in the nests. Pack-spion, mostly. Bring me 12 pelts and I'll line 6 coats. No more talk needed than that."));

				var response = await dialog.Select(L("Will you go into the bramble?"),
					Option(L("I'll bring you 12 pelts"), "help"),
					Option(L("Whose spions were they?"), "info"),
					Option(L("Cut up the tent instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Go in where the thorn's dead and the gaps are wide. Nests sit at the base of a live bramble, always downwind. That's all you need to know."));
						break;

					case "info":
						await dialog.Msg(L("Caravan animals. The Order ran three pack strings up this road every spring, and none have come through since the altar turned."));
						await dialog.Msg(L("So the pelts are the caravan, and the caravan was the medicine, and I'm sewing coats out of the reason we don't have any. I don't say that lightly. I say it once and go back to sewing."));
						break;

					case "leave":
						await dialog.Msg(L("The tent is already four coats short. If it goes, 31 people sleep in open rain, and I'll have made things worse for the trouble. No."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectFur", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She runs a thumb through the pile, sorting by nap without looking down*{/}"));
					await dialog.Msg(L("Good winter coat on all 12. That's 6 lined. First 2 done before dark, if my hands hold."));
					await dialog.Msg(L("Here. It's what I was carrying to the abbey. The abbey can wait longer than you can."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough yet. Try the nests further in — the ones near the road are already picked over."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Six coats, and the 6 coldest got them. Nobody argued about who was coldest. That surprised me more than anything else this week, and I don't surprise easy."));
			}
		});

		// Quest 1003: The Western Milestones
		//---------------------------------------------------------------------
		AddNpc(155035, L("[Pilgrim] Jordan"), "f_pilgrimroad_41_2", -1144, 337, 5, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1003);

			dialog.SetTitle(L("Jordan"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's staring west down the road, arms folded, the same stance a man holds when he's already lost an argument with himself*{/}"));
				await dialog.Msg(L("You headed further out? Then you'll want to hear this first — precisely this, not the short version. I walked this road for the Orsha surveyors before I ever walked it as a pilgrim. Seven milestones between the Thaumas gate and the waystation. I know every one by the chip in it. Every single chip."));
				await dialog.Msg(L("I've not reached the fourth in a fortnight — fourteen days exactly. The Black Banshees hold the whole western stretch now. Kill 30 of them and I walk my own road again."));

				var response = await dialog.Select(L("Will you clear the west?"),
					Option(L("I'll kill 30 Black Banshees"), "help"),
					Option(L("Why does the 4th matter?"), "info"),
					Option(L("Walk it with an escort"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come out of the ground, not the trees. Watch the branches and you're watching the wrong thing entirely — I made that mistake once. Only once."));
						break;

					case "info":
						await dialog.Msg(L("The fourth is the halfway stone. Everyone who walks this road touches it, and everyone who turns back turns back before it. Always. I've tracked it for twenty years."));
						await dialog.Msg(L("I've got 40 people behind me at the Thaumas gate waiting to hear if the road's walkable. They'll believe me about the fourth stone. They won't believe me about anything else, and frankly, why would they."));
						break;

					case "leave":
						await dialog.Msg(L("An escort of who, precisely? The friar has 31 people on the ground and a woman sewing coats. That is the entire garrison, and I've counted twice."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBanshees", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("I reached the fourth stone this morning and put my hand on it. Chip's still there, same as it was in '31. Twenty years and it hasn't moved an inch."));
					await dialog.Msg(L("Take the surveyor's fee. Paid for twenty years to say where a road goes, and today's the first time in a while it actually went anywhere."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still too many out there. Work the open ground west of here — that's where they come up thickest, every time, without fail."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Word went back to the Thaumas gate. Forty people started walking this morning, and I intend to be at the fourth stone to count every single one of them in."));
			}
		});

		// Quest 1004: Three Who Cannot Hold a Cup
		//---------------------------------------------------------------------
		AddNpc(152065, L("[Stricken Pilgrim] Rasa"), "f_pilgrimroad_41_2", -216, -381, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1004);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Don't crouch, I can hear you fine — it's my hands that don't work, not my ears. I touched the altar on the sixth and I've been on this ground since the ninth. Only thing wrong with me is my hands won't close. Small mercy, all things considered."));
				await dialog.Msg(L("Brutus is out of hours before he's out of medicine. Three in this camp can't hold a cup anymore. Take the doses round to them and stay while they drink. Someone should."));

				var response = await dialog.Select(L("Will you go round the camp?"),
					Option(L("I'll dose all 3 of them"), "help"),
					Option(L("Why you and not the friar?"), "info"),
					Option(L("They should be moved indoors"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Tip it slow, against the cheek, not the tongue. Say their name first — they can hear. They just can't answer. Small comfort, but it's something."));
						break;

					case "info":
						await dialog.Msg(L("Because the friar has 31 people and 4 doses, and he has to decide who gets them. He'll be doing that until dark, poor man."));
						await dialog.Msg(L("I've lain here nine days counting who stops talking, and on which day. That's the only useful thing left for me to do on this ground, so I do it. Don't pity me for it."));
						break;

					case "leave":
						await dialog.Msg(L("Indoors is a waystation with a roof over two benches. There is no indoors. There is this, and there is rain. Pick one."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("doseWalkers", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*She listens to the camp for a long moment before she answers*{/}"));
					await dialog.Msg(L("All three swallowed. I heard the last one cough, and a cough is a thing a body does on purpose. Good sign, that."));
					await dialog.Msg(L("There's a purse under my pack. Was for the abbey offering. I'd rather it went to somebody who actually came out here and did something."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all three yet. They're spread along the north side of the camp, and none of them will call out to you. You'll have to go looking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two of the three were talking by evening. The third said my name, got it wrong, and went back to sleep. I'll take it. At this point, I'll take anything."));
			}
		});

		// Quest 1004 collection points - the walkers who cannot hold a cup
		//---------------------------------------------------------------------
		void AddStrickenWalker(int walkerNumber, int model, string observation, int x, int z, int direction)
		{
			AddNpc(model, L("Stricken Walker"), "f_pilgrimroad_41_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_2", 1004);
				var variableKey = $"Laima.Quests.f_pilgrimroad_41_2.Quest1004.Walker{walkerNumber}";
				var counterKey = "Laima.Quests.f_pilgrimroad_41_2.Quest1004.WalkersDosed";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A walker laid out on the camp's north side, eyes open, hands slack*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This one has already had a dose*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Giving the dose..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var dosed = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, dosed);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Walkers dosed: {0}/3", dosed));

					if (dosed >= 3)
						character.ServerMessage(L("{#FFD700}All 3 have swallowed. Return to Rasa.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the walker undosed."));
				}
			});
		}

		AddStrickenWalker(1, 155044,
			L("The friar who carried them in. He takes the dose without a sound and his eyes track you the whole way."), -127, -194, 0);
		AddStrickenWalker(2, 155033,
			L("She grips your wrist once, hard, then can't do it again."), -126, -102, 0);
		AddStrickenWalker(3, 155035,
			L("He swallows on the second try and coughs, which is the first noise he has made in 4 days."), -207, -319, 0);

		// Quest 1005: What Was Taken from the Altar
		//---------------------------------------------------------------------
		AddNpc(155044, L("[Friar] Clark"), "f_pilgrimroad_41_2", 539, 664, 311, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1005);

			dialog.SetTitle(L("Clark"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_pilgrimroad_41_2", 1001)))
				{
					await dialog.Msg(L("Get Brutus his doses first. I'm not sending anyone east of me while there are 31 people back there with nothing in the box."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's on his knees at the altar, ear almost against the stone, and only sits back when your footsteps reach him*{/}"));
				await dialog.Msg(L("Forgive me - I was listening for something that isn't there anymore. I've swept that altar every morning for 6 years and I know the sound of it. 2 months ago it started ringing hollow, and it took me until last week to work out why. It's empty."));
				await dialog.Msg(L("The Green Tini Magicians prised the reliquary out of the chest and set it up in their own circle west of the bend. That's what's holding the bramble open for the Banshees. Kill 20 Black Banshees to reach the circle, then break it."));

				var response = await dialog.Select(L("Will you go to the circle?"),
					Option(L("I'll break the circle"), "help"),
					Option(L("Widlings robbed an altar?"), "info"),
					Option(L("An empty altar is still an altar"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("2 of them stand over it and the rest fetch. Take the 2 standing still and the circle stops working before you've finished the rest."));
						break;

					case "info":
						await dialog.Msg(L("They did on the Grynas road too. Stoulets went statue to statue there prising crystals out of the chests, and nobody has ever explained what told them to."));
						await dialog.Msg(L("The same hand emptied my altar and I would very much like to know whose it is. So would the abbey."));
						break;

					case "leave":
						await dialog.Msg(L("It is. It has also been an open door for 2 months and 31 people came through it, so I'd like it shut."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearApproach", out var approachObj)) return;
				if (!quest.TryGetProgress("breakCircle", out var circleObj)) return;

				if (approachObj.Done && circleObj.Done)
				{
					await dialog.Msg(L("{#666666}*He sets the reliquary back in the chest and the altar takes it with a sound like a door closing*{/}"));
					await dialog.Msg(L("Listen to that. That is the noise it made every morning for 6 years and I never once heard it until it stopped."));
					await dialog.Msg(L("Keep the chain off it - the reliquary sits in the stone without it, and you've earned more than the abbey will ever send me. I'm writing ahead to Monk Stella at Rasvoy Lake tonight, because whatever told the widlings to do this did not start here."));

					character.Quests.Complete(questId);
				}
				else if (approachObj.Done)
				{
					await dialog.Msg(L("The bend is clear. The circle's still up - go west of it and don't let the 2 standing ones finish anything."));
				}
				else
				{
					await dialog.Msg(L("Too many Banshees between you and the circle. Kill them off or you'll be fighting both at once."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Nobody new has been carried in for 3 days. Brutus has started arguing about the roof again, which is how I know it's over."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Cases from Thaumas
//-----------------------------------------------------------------------------

public class TheCasesFromThaumasQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1001);
		SetName(L("The Cases from Thaumas"));
		SetType(QuestType.Sub);
		SetDescription(L("The Salvia waystation is a hospital with 31 people on the ground and 4 doses of medicine left. The rest is out on the west road in cases the Green Tini Magicians have cracked open. Kill them and recover the doses."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Friar] Brutus"), "f_pilgrimroad_41_2");

		AddObjective("killMagicians", L("Kill Green Tini Magicians on the west road"),
			new KillObjective(20, new[] { MonsterId.Tiny_Mage_Green }));

		AddObjective("collectMedicine", L("Recover the Order's Secret Medicine"),
			new CollectItemObjective(666101, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(666101, 0.35f, MonsterId.Tiny_Mage_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666101, character.Inventory.CountItem(666101), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666101, character.Inventory.CountItem(666101), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: What the Bramble Keeps
//-----------------------------------------------------------------------------

public class WhatTheBrambleKeepsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1002);
		SetName(L("What the Bramble Keeps"));
		SetType(QuestType.Sub);
		SetDescription(L("The Black Banshees drag what they kill into the Salvia bramble and leave the hides in their nests. A pilgrim sewing linings for 31 people needs 12 pack-spion pelts out of them."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim] Diane"), "f_pilgrimroad_41_2");

		AddObjective("collectFur", L("Recover Spion Fur from the Banshee nests"),
			new CollectItemObjective(666099, 12));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(666099, 0.45f, MonsterId.Sec_Banshee_Purple);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666099, character.Inventory.CountItem(666099), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666099, character.Inventory.CountItem(666099), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Western Milestones
//-----------------------------------------------------------------------------

public class TheWesternMilestonesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1003);
		SetName(L("The Western Milestones"));
		SetType(QuestType.Sub);
		SetDescription(L("A surveyor turned pilgrim has not reached the halfway stone in a fortnight. 40 people are waiting at the Thaumas gate to hear whether the road is walkable. Kill 30 Black Banshees on the western stretch."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim] Jordan"), "f_pilgrimroad_41_2");

		AddObjective("killBanshees", L("Kill Black Banshees on the western stretch"),
			new KillObjective(30, new[] { MonsterId.Sec_Banshee_Purple }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Three Who Cannot Hold a Cup
//-----------------------------------------------------------------------------

public class ThreeWhoCannotHoldACupQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1004);
		SetName(L("Three Who Cannot Hold a Cup"));
		SetType(QuestType.Sub);
		SetDescription(L("The friar is out of hours before he is out of medicine. A stricken pilgrim who has spent 9 days counting who stops talking knows which 3 in the camp can no longer take a dose themselves. Carry it round to all 3."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stricken Pilgrim] Rasa"), "f_pilgrimroad_41_2");

		AddObjective("doseWalkers", L("Dose the 3 walkers on the camp's north side"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_2.Quest1004.WalkersDosed", 3, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_2.Quest1004.WalkersDosed");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_2.Quest1004.Walker{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_2.Quest1004.WalkersDosed");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_2.Quest1004.Walker{i}");
	}
}

// Quest 1005 CLASS: What Was Taken from the Altar
//-----------------------------------------------------------------------------

public class WhatWasTakenFromTheAltarQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1005);
		SetName(L("What Was Taken from the Altar"));
		SetType(QuestType.Sub);
		SetDescription(L("The roadside altar has been ringing hollow for 2 months because the Green Tini Magicians prised the reliquary out of it and set it up in a circle of their own. Clear the approach and break the circle."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Friar] Clark"), "f_pilgrimroad_41_2");

		AddPrerequisite(new CompletedPrerequisite("f_pilgrimroad_41_2", 1001));

		AddObjective("clearApproach", L("Kill Black Banshees on the approach to the circle"),
			new KillObjective(20, new[] { MonsterId.Sec_Banshee_Purple }));

		AddObjective("breakCircle", L("Break the Green Tini Magicians' circle"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Tiny_Mage_Green, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Sec_Banshee_Purple, 3),
				},
				resetIdent: "clearApproach",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583118, 1)); // Mejstra Necklace
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
