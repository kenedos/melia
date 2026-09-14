//--- Melia Script ----------------------------------------------------------
// Arrow Path Quest NPCs
//--- Description -----------------------------------------------------------
// The Kingdom road crew reopening the pass between the Letas valley and
// Ramstis Ridge, and the courier who died on the last mail run through it.
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

public class FKatyn133QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: Wings Over the Cut
		//---------------------------------------------------------------------
		AddNpc(147484, L("[Crew Foreman] Zilvinas"), "f_katyn_13_3", 126, -449, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1001);

			dialog.SetTitle(L("Zilvinas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's staring up at the rock face, counting something under his breath that isn't stone*{/}"));
				await dialog.Msg(L("Don't just stand there gawking, you'll draw one down on us both. I've got 12 men clearing this pass and I've lost 3 of them to Desmodus in a fortnight. They come off the rock face at the eastern cut, straight onto whoever's holding the barrow."));
				await dialog.Msg(L("Kill 30 of them out at the cut. I'm not asking you to make it safe, I'm asking you to make it survivable."));

				var response = await dialog.Select(L("Will you take the cut?"),
					Option(L("I'll kill the Desmodus"), "help"),
					Option(L("Why reopen this road at all?"), "info"),
					Option(L("Pull your crew out instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fight with your back to the rock. If they can't get above you they have to come in level, and level is where they're bad at it."));
						break;

					case "info":
						await dialog.Msg(L("Because 34 people turned up alive out of the Letas valley last month and there's no way to feed them that doesn't come through this pass."));
						await dialog.Msg(L("The road's been shut 11 years. Nobody cared until there was somebody on the other end of it."));
						break;

					case "leave":
						await dialog.Msg(L("I'd need somewhere to pull them back to. The contract says the pass opens by first frost or the crew doesn't see a wage at all."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDesmodus", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("We moved 40 paces of roadbed today and nobody looked up once. You'd have to have worked this pass to know what that's worth."));
					await dialog.Msg(L("Take it out of the crew chest. I'll square it with the ledger and Ausra will shout at me, which is her job."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still wings on the cut. My men can hear them going over and they won't put the barrow down."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Nine men, one pass, and a fair chance of finishing it. That's the best set of numbers I've had all year."));
			}
		});

		// Quest 1002: Roadbed Iron
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Crew Smith] Birute"), "f_katyn_13_3", -114, -310, 135, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1002);

			dialog.SetTitle(L("Birute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's got tongs in one hand and a lump of glowing red rock in the other, squinting at it like it might explain itself*{/}"));
				await dialog.Msg(L("Mind the sparks, this one's hotter than it looks. Zilvinas wants roadbed and roadbed needs iron, and the nearest forge that'd sell me any is 4 days east — so I'm smelting Red Infrorocktors instead, which is not in any book I've read."));
				await dialog.Msg(L("Kill 20 of them and bring me 8 cores. The core is the only part worth the fire - the rest is just angry gravel."));

				var response = await dialog.Select(L("Will you get the cores?"),
					Option(L("I'll hunt them and bring the cores"), "help"),
					Option(L("You can smelt a monster?"), "info"),
					Option(L("Wait for the forge shipment"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Hit them low. The core sits under the shoulder plate and if you crack it in the fight you've got nothing but hot sand."));
						break;

					case "info":
						await dialog.Msg(L("You can smelt anything once. Doing it twice and getting the same bar out is the trick, and it took me 11 tries."));
						await dialog.Msg(L("The bars come out darker than proper iron and they ring wrong. They hold a road, though, and that's all Zilvinas is paying for."));
						break;

					case "leave":
						await dialog.Msg(L("That shipment's been 'coming' since spring. I've stopped setting a place at the table for it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRocktors", out var killObj)) return;
				if (!quest.TryGetProgress("collectCores", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She weighs each core in her palm before dropping it in the crucible*{/}"));
					await dialog.Msg(L("8 good ones. That's 60 paces of bed iron, and I get to stop apologising to the foreman every morning."));
					await dialog.Msg(L("Take the smith's share. It's honest coin and it's been in my apron since the crew formed."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty dead out there, not enough cores in my hand. Check what they leave before it cools."));
				}
				else
				{
					await dialog.Msg(L("Still 20 of them working the slope. They don't wander, so you'll have to go to them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The bed's laid as far as the second stone. First time in 11 years anything's gone over that ground on a wheel."));
			}
		});

		// Quest 1003: The Kingdom Tally
		//---------------------------------------------------------------------
		AddNpc(20116, L("[Tally Clerk] Ausra"), "f_katyn_13_3", -194, -449, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1003);

			dialog.SetTitle(L("Ausra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's stamping a returned claim so hard the desk rattles*{/}"));
				await dialog.Msg(L("Sorry, ignore that, I'm just — furious at a piece of paper. The Kingdom pays this crew a bounty per Old Kepa cleared off the verge, and it pays on teeth. Not on my word, not on Zilvinas's word. Teeth."));
				await dialog.Msg(L("Bring me 12 Kepa Teeth and I can file a claim that Fedimian won't send back. It's the third claim I've written this month and the first two came back stamped."));

				var response = await dialog.Select(L("Will you work the verge?"),
					Option(L("I'll bring you 12 teeth"), "help"),
					Option(L("Why did the claims come back?"), "info"),
					Option(L("That's a clerk's problem"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They sit in the verge grass on both sides of the road and they don't move until you're on top of them. Watch your ankles."));
						break;

					case "info":
						await dialog.Msg(L("The first one because I wrote 'approximately'. The second because the seal smudged in the rain. A clerk in Fedimian has never once had to explain to 9 men why there's no wage."));
						await dialog.Msg(L("So now I write nothing I can't put in a sack and hand across a desk."));
						break;

					case "leave":
						await dialog.Msg(L("It is my problem, right up until payday, and then it becomes everybody's problem very quickly indeed."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectTeeth", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She counts them twice, then writes a figure and underlines it*{/}"));
					await dialog.Msg(L("12, verified, sacked and sealed. Let them send that one back."));
					await dialog.Msg(L("Your share comes out of the bounty, which means for once I'm paying somebody with money that actually exists."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. The verge runs the whole length of the road, so there's no shortage of Kepas, only of walking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Paid in full, 6 days after filing. I've pinned the receipt to the tent pole where the crew can see it."));
			}
		});

		// Quest 1004: The Last Mail Run
		//---------------------------------------------------------------------
		AddGhostNpc(152001, L("[Restless Soul] Courier Jurate"), "f_katyn_13_3", -114, -588, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1004);

			dialog.SetTitle(L("Jurate"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's pacing the same stretch of road, boots making no sound at all on the gravel*{/}"));
				await dialog.Msg(L("You walk with weight — a living step. I'd know that sound anywhere by now. I ran this pass twice a month for 9 years. Letas valley to Ramstis Ridge, 4 road stones, and I never once missed a stone."));
				await dialog.Msg(L("The last run I made, the valley was already dead and I didn't know it. Walk my 4 road stones and read the marks on them - I need to know which of them I actually reached."));

				var response = await dialog.Select(L("Will you walk the stones?"),
					Option(L("I'll read the 4 road stones"), "help"),
					Option(L("What marks?"), "info"),
					Option(L("Let the road forget"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They're set along the road from the western warp to the eastern cut. The chisel marks are on the low face, near the ground - you'll have to crouch to see them."));
						break;

					case "info":
						await dialog.Msg(L("A courier cuts a notch on every stone she passes. It's how the Kingdom knows the mail moved and how far it got before it stopped."));
						await dialog.Msg(L("I have been standing here 11 years unable to remember whether I made the third stone. It is a very small thing to lose your mind over and I have lost mine over it anyway."));
						break;

					case "leave":
						await dialog.Msg(L("The road doesn't forget, that's the whole point of cutting the notches. It's only me that can't remember."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readStones", out var stoneObj)) return;

				if (stoneObj.Done)
				{
					await dialog.Msg(L("All 4. I made all 4 stones and the bag was still on my shoulder at the last one."));
					await dialog.Msg(L("Then I did my work. Whatever else happened out here, I did my work. Take the bag - the addresses on it don't exist any more."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More stones still unread. Low face, near the ground, and don't guess - I've done enough guessing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I've been walking the pass again, west to east, stone to stone. There's no mail on it. I find I don't mind."));
			}
		});

		// Quest 1004 inspection points - the Kingdom road stones
		//---------------------------------------------------------------------
		void AddRoadStone(int stoneNumber, string stoneName, string observation, int x, int z, int direction)
		{
			AddNpc(47190, stoneName, "f_katyn_13_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_13_3", 1004);
				var variableKey = $"Laima.Quests.f_katyn_13_3.Quest1004.Stone{stoneNumber}";
				var counterKey = "Laima.Quests.f_katyn_13_3.Quest1004.StonesRead";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A Kingdom road stone, notched down one face*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already read this one*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Reading the notches..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var read = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, read);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Road stones read: {0}/4", read));

					if (read >= 4)
						character.ServerMessage(L("{#FFD700}All 4 stones read. Return to Courier Jurate.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the stone unread."));
				}
			});
		}

		AddRoadStone(1, L("Road Stone"),
			L("First Stone: 218 notches, the last one cut clean and deep."), -1377, 351, 90);
		AddRoadStone(2, L("Road Stone"),
			L("Second Stone: 218 notches, and a second mark beside the last - she came back this way once."), 46, -588, 0);
		AddRoadStone(3, L("Road Stone"),
			L("Third Stone: 218 notches. She reached it. The cut is shallow, as though her hand was shaking."), 330, 275, 270);
		AddRoadStone(4, L("Road Stone"),
			L("Fourth Stone: 218 notches, and the mail-bag buckle wedged in the seam beneath them."), 1301, 298, 180);

		// Quest 1005: The Deadborn in the Spoil
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Road-Master] Gvidas"), "f_katyn_13_3", 46, -310, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1005);

			dialog.SetTitle(L("Gvidas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_katyn_13_3", 1004)))
				{
					await dialog.Msg(L("Somebody's been reading the old road stones. Finish whatever that is and then come back - I've a question that follows on from it."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's leaning on a shovel handle, watching his crew work the heap from a wary distance*{/}"));
				await dialog.Msg(L("You're the one who's been reading the road stones, then — good, saves me the explaining. You know the mail bag went into the spoil heap beside the fourth stone, and you know my crew's been digging that heap out for a week now."));
				await dialog.Msg(L("Yesterday we found what was underneath. A Deadborn, and it is awake now. Kill 20 Ellom off the spoil so it hasn't got a thicket to fight in, then finish it before it reaches my camp."));

				var response = await dialog.Select(L("Will you clear the spoil?"),
					Option(L("I'll kill the Deadborn"), "help"),
					Option(L("Why was it under a spoil heap?"), "info"),
					Option(L("Abandon the pass"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It fights in the heap because loose stone suits it and doesn't suit you. Draw it onto the finished bed - it's flat, and flat is ours."));
						break;

					case "info":
						await dialog.Msg(L("Because 11 years ago somebody closed this road in a hurry and piled 400 tons of rock over the reason. There's no record of it. There's never a record of it."));
						await dialog.Msg(L("Jurate walked past that heap on her last run and never mentioned it in a notch. I don't think she got the chance."));
						break;

					case "leave":
						await dialog.Msg(L("Then the Letas valley starves through winter, and 34 people who already walked out of one grave walk straight into another. I'd sooner keep digging."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearEllom", out var thicketObj)) return;
				if (!quest.TryGetProgress("killDeadborn", out var bossObj)) return;

				if (thicketObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It's down on the finished bed, which is exactly where I said it would go, and I intend to mention that for years."));
					await dialog.Msg(L("Take the pike we dug out of the heap with it. Nobody on this crew will touch the thing and I'd rather it left with somebody who can use it."));

					character.Quests.Complete(questId);
				}
				else if (thicketObj.Done)
				{
					await dialog.Msg(L("The spoil's clear of Ellom. It's standing in the open now with nothing to hide behind."));
				}
				else
				{
					await dialog.Msg(L("Too much Ellom still on the heap. It'll sit in the thicket and let them soak up everything you throw."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pass opens in 9 days. First cart through carries grain to the Letas valley and I am going to walk beside it the whole way."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Wings Over the Cut
//-----------------------------------------------------------------------------

public class WingsOverTheCutQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13_3", 1001);
		SetName(L("Wings Over the Cut"));
		SetType(QuestType.Sub);
		SetDescription(L("Desmodus drop off the rock face onto the road crew working the eastern cut, and the foreman has lost 3 men in a fortnight. Kill enough of them that the crew can put the barrow down."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crew Foreman] Zilvinas"), "f_katyn_13_3");

		AddObjective("killDesmodus", L("Kill Desmodus at the eastern cut"),
			new KillObjective(30, new[] { MonsterId.New_Desmodus }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1002 CLASS: Roadbed Iron
//-----------------------------------------------------------------------------

public class RoadbedIronQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13_3", 1002);
		SetName(L("Roadbed Iron"));
		SetType(QuestType.Sub);
		SetDescription(L("The crew smith has no forge within 4 days' travel, so she is smelting Red Infrorocktor cores into bed iron instead. Kill the Infrorocktors on the slope and bring her the cores."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crew Smith] Birute"), "f_katyn_13_3");

		AddObjective("killRocktors", L("Kill Red Infrorocktor on the slope"),
			new KillObjective(20, new[] { MonsterId.InfroRocktor_Red }));

		AddObjective("collectCores", L("Collect Red Infrorocktor Cores"),
			new CollectItemObjective(650558, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(650558, 0.45f, MonsterId.InfroRocktor_Red);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650558, character.Inventory.CountItem(650558), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650558, character.Inventory.CountItem(650558), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Kingdom Tally
//-----------------------------------------------------------------------------

public class TheKingdomTallyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13_3", 1003);
		SetName(L("The Kingdom Tally"));
		SetType(QuestType.Sub);
		SetDescription(L("The crew's bounty for clearing Old Kepa off the road verge is paid on teeth, not on word, and the tally clerk has had two claims sent back already. Bring her something Fedimian cannot refuse."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tally Clerk] Ausra"), "f_katyn_13_3");

		AddObjective("collectTeeth", L("Collect Kepa Teeth from Old Kepa on the verge"),
			new CollectItemObjective(662005, 12));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(662005, 0.50f, MonsterId.Pappus_Kepa);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662005, character.Inventory.CountItem(662005), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662005, character.Inventory.CountItem(662005), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: The Last Mail Run
//-----------------------------------------------------------------------------

public class TheLastMailRunQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13_3", 1004);
		SetName(L("The Last Mail Run"));
		SetType(QuestType.Sub);
		SetDescription(L("A courier who ran this pass for 9 years cannot remember how far she got on her last run. Read the notches on all 4 Kingdom road stones and tell her which she reached."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Courier Jurate"), "f_katyn_13_3");

		AddObjective("readStones", L("Read the notches on the 4 Kingdom road stones"),
			new VariableCheckObjective("Laima.Quests.f_katyn_13_3.Quest1004.StonesRead", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_13_3.Quest1004.StonesRead");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_13_3.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_13_3.Quest1004.StonesRead");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_13_3.Quest1004.Stone{i}");
	}
}

// Quest 1005 CLASS: The Deadborn in the Spoil
//-----------------------------------------------------------------------------

public class TheDeadbornInTheSpoilQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13_3", 1005);
		SetName(L("The Deadborn in the Spoil"));
		SetType(QuestType.Sub);
		SetDescription(L("Somebody closed this pass 11 years ago and piled 400 tons of rock over the reason. The crew has dug it back out, and it is awake. Clear the Ellom off the spoil heap, then put the Deadborn down."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Road-Master] Gvidas"), "f_katyn_13_3");

		AddPrerequisite(new CompletedPrerequisite("f_katyn_13_3", 1004));

		AddObjective("clearEllom", L("Kill Ellom on the spoil heap"),
			new KillObjective(20, new[] { MonsterId.Ellom }));

		AddObjective("killDeadborn", L("Defeat the Deadborn"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Deadbone, 1) },
				resetIdent: "clearEllom",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(253115, 1)); // Sacmet
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
