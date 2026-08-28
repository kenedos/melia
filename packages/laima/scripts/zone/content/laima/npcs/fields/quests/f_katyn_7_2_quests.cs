//--- Melia Script ----------------------------------------------------------
// Katyn 7-2 Quest NPCs
//--- Description -----------------------------------------------------------
// The Katyn war graves and the bog below them, where the dead were buried
// without their kit and have not stayed down since.
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

public class FKatyn72QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: What the Bog Kept
		//---------------------------------------------------------------------
		AddNpc(20150, L("[Gravekeeper] Romas"), "f_katyn_7_2", 2445, -243, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1001);

			dialog.SetTitle(L("Romas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's on one knee scraping moss off a stone marker, back to the path*{/}"));
				await dialog.Msg(L("Watch your footing near the shelf edge, stranger. Four hundred and six graves up here and I dug most of them myself. What I didn't dig was the part that mattered - the army buried them empty."));
				await dialog.Msg(L("Kit, rings, letters, all of it stripped before the carts came up. That's why they walk. Go down into the bog and bring me back 6 bundles of what's still down there and I'll bury it where it belongs."));

				var response = await dialog.Select(L("Will you go down and look?"),
					Option(L("I'll bring up their belongings"), "help"),
					Option(L("Stripped by who?"), "info"),
					Option(L("Grave work isn't for me"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The bog holds things whole. Anything you find down there will look like it was dropped yesterday, and it wasn't, and that takes some getting used to."));
						break;

					case "info":
						await dialog.Msg(L("Quartermasters. Not looters, not the enemy - our own supply officers, working off a list. Everything issued had to be accounted for and returned."));
						await dialog.Msg(L("So four hundred and six men went into the ground with nothing on them, by regulation. There's a signature on the order. I've seen it."));
						break;

					case "leave":
						await dialog.Msg(L("Nor was it for me. I came up here to cut peat and I've been forty years at this instead."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("recoverBelongings", out var bagObj)) return;

				if (bagObj.Done)
				{
					await dialog.Msg(L("Six bundles. I'll open them tonight and match what I can to the stones by regiment number."));
					await dialog.Msg(L("Take this. It's what the parish sends me for lamp oil and I'd rather it went on this."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More still down in the bog. Look where the ground goes soft and dark - that's where the carts went over."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Matched nineteen bundles to nineteen stones. Nineteen out of four hundred and six, and I have been happier this week than I have been in a decade."));
			}

			// Quest 1003 delivery - Vakaris's urn
			if (character.Quests.IsActive(new QuestId("f_katyn_7_2", 1003)) && character.Inventory.HasItem(650433))
			{
				var deliveredKey = "Laima.Quests.f_katyn_7_2.Quest1003.Delivered";
				if (!character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("{#666666}*He takes the urn and turns it until the scratched name catches the light*{/}"));
					await dialog.Msg(L("Vakaris. Second company. He's on the shelf, third row, and his stone has been blank for eleven years because nobody could tell me which one he was."));
					await dialog.Msg(L("Tell him it's done. Tell him I'll cut the name in the morning."));

					character.Variables.Perm.Set(deliveredKey, true);
					character.Quests.CompleteObjective(new QuestId("f_katyn_7_2", 1003), "deliverUrn");
					character.ServerMessage(L("{#FFD700}Romas has the urn. Return to Vakaris.{/}"));
				}
			}
		});

		// Quest 1001 collection points - kit lost in the bog
		//---------------------------------------------------------------------
		void AddSunkenKit(int kitNum, int x, int z, int direction)
		{
			AddNpc(154035, L("Sunken Kit"), "f_katyn_7_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_7_2", 1001);
				var variableKey = $"Laima.Quests.f_katyn_7_2.Quest1001.Kit{kitNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*Something bundled in oilcloth, half out of the peat and not rotted at all*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already pulled this one out of the peat*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Working the bundle loose..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650429, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Soldiers' Belongings"));

					var currentCount = character.Inventory.CountItem(650429);
					character.ServerMessage(LF("Bundles recovered: {0}/6", currentCount));

					if (currentCount >= 6)
						character.ServerMessage(L("{#FFD700}That's six. Return to Gravekeeper Romas.{/}"));
				}
				else
				{
					character.ServerMessage(L("You let the bundle settle back into the peat."));
				}
			});
		}

		AddSunkenKit(1, 407, -912, 0);
		AddSunkenKit(2, 575, -665, 45);
		AddSunkenKit(3, 25, -830, 90);
		AddSunkenKit(4, 265, -675, 135);
		AddSunkenKit(5, 140, -425, 180);
		AddSunkenKit(6, 155, -1070, 225);
		AddSunkenKit(7, 1340, -1880, 270);
		AddSunkenKit(8, 1783, -2649, 315);

		// Quest 1002: The Drowned Patrol
		//---------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Drowned Soldier"), "f_katyn_7_2", 1852, 2080, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1002);

			dialog.SetTitle(L("Drowned Soldier"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He turns at the sound of footsteps in the sand, and for a moment looks almost hopeful it's one of his own*{/}"));
				await dialog.Msg(L("Not one of mine. No — of course not. They don't walk anymore. Only I do that, apparently. Eleven men on the dune patrol, and I lost all eleven inside a hundred paces. Not to the enemy. To the Sakmoli, denned under the wet sand — I never even saw the ground move."));
				await dialog.Msg(L("They still den there and they still take anyone who walks that line. Kill 30 of them so the next patrol gets further than I did."));

				var response = await dialog.Select(L("Will you walk it for me? Someone should get further than a hundred paces. I never did."),
					Option(L("I'll kill the Sakmoli"), "help"),
					Option(L("There is no next patrol"), "info"),
					Option(L("Rest, soldier"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come up through the sand, not across it. Watch the ground, not the horizon - that was my mistake and it took about four seconds."));
						break;

					case "info":
						await dialog.Msg(L("There's a gravekeeper up on the shelf who walks the dune line every week to check the markers. He is sixty-something and he goes alone."));
						await dialog.Msg(L("So there is a next patrol. There's just one of him."));
						break;

					case "leave":
						await dialog.Msg(L("I would, if resting were a thing that took. Eleven years I've been trying. It doesn't take."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSakmoli", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The sand's quiet. I walked the whole hundred paces just now and nothing came up under me."));
					await dialog.Msg(L("There's a satchel buried at the third marker. It was mine and I have no use for it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still moving under the sand. Watch the ground."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Romas came down the dune line yesterday and got all the way to the end of it. He stopped and looked around like he'd lost something. He hadn't."));
			}
		});

		// Quest 1003: The Urn of Private Vakaris
		//---------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Vakaris"), "f_katyn_7_2", 300, -900, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1003);
			var deliveredKey = "Laima.Quests.f_katyn_7_2.Quest1003.Delivered";

			dialog.SetTitle(L("Vakaris"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He straightens up out of the peat as you approach, like he's been waiting rather than resting*{/}"));
				await dialog.Msg(L("You can see me. Good — it's been a long while since anyone could. My stone up on the shelf has no name on it. I've stood in front of that blank a great many times, for a dead man with nowhere else to be, and I still can't do a thing about it."));
				await dialog.Msg(L("But I scratched my name into my urn the night before the line broke, because I had a feeling. It's still down here in the peat with me. Carry it up to the gravekeeper and he'll know which stone is mine."));

				var response = await dialog.Select(L("Would you carry it up for me? I'd do it myself, but eleven years of trying tells me that's not how this works."),
					Option(L("I'll carry the urn to Romas"), "help"),
					Option(L("Why do you have an urn?"), "info"),
					Option(L("I'm not going that way"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650433, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("The name's on the underside, scratched with a mess tin. It isn't neat. He'll be able to read it."));
						break;

					case "info":
						await dialog.Msg(L("Every man in second company carried one. You were supposed to hand it to a friend if it came to that, and he'd carry it home."));
						await dialog.Msg(L("I gave mine to Petras and Petras went into the bog about forty feet from where I did. So much for the arrangement."));
						break;

					case "leave":
						await dialog.Msg(L("Then don't. I've waited eleven years and I'll wait longer. That's the one thing I'm good at now."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("He'll cut it in the morning. You're sure? He said the name out loud?"));
					await dialog.Msg(L("{#666666}*The light around him steadies for a moment*{/}"));
					await dialog.Msg(L("Then that's finished. Take whatever's in the peat here - I put it down eleven years ago and I've not needed it since."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The gravekeeper works the shelf above the bog. He'll be somewhere near the stones - he always is."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I went up and looked at it. Cut deep, and spelled right, and he'd put the company number under it without being asked."));
			}
		});

		// Quest 1004: Leaves for the Censer
		//---------------------------------------------------------------------
		AddNpc(152001, L("[Herbwife] Danguole"), "f_katyn_7_2", 200, -500, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1004);

			dialog.SetTitle(L("Danguole"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's turning an empty basket over, checking the bottom for anything she might have missed*{/}"));
				await dialog.Msg(L("Oh, headed toward the bog, are you? Well, since you're going that way anyway, you might as well make yourself useful to me too, hm? Romas burns Ridimed leaf at the graves every seventh day — only thing that quiets them, and a quiet night's worth a great deal to a man who lives up there all alone with four hundred neighbors who won't stay put."));
				await dialog.Msg(L("I supply him and I've run dry. Kill 20 Ridimed in the bog and bring me 5 good leaf-bundles off them."));

				var response = await dialog.Select(L("Be a dear and gather it for me?"),
					Option(L("I'll bring the leaves"), "help"),
					Option(L("It quiets them?"), "info"),
					Option(L("Ask someone else"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the leaf off the crown, not the stem. Stem-leaf goes bitter in the censer and Romas will notice and be too polite to say."));
						break;

					case "info":
						await dialog.Msg(L("Something in the smoke. My mother said it smells like a kitchen and that's what settles them - not holiness, just something ordinary."));
						await dialog.Msg(L("I don't know if she was right. I know the shelf is quiet on the seventh night and loud on the other six."));
						break;

					case "leave":
						await dialog.Msg(L("There's nobody else. There's me, and the bog, and a man on a hill with four hundred graves."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRidimed", out var killObj)) return;
				if (!quest.TryGetProgress("gatherLeaves", out var leafObj)) return;

				if (killObj.Done && leafObj.Done)
				{
					await dialog.Msg(L("Crown-leaf, every one. That'll see him through to autumn and then some."));
					await dialog.Msg(L("Your pay, and take a twist of it for yourself. If you sleep rough out here you'll want it."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Ridimed still out in the bog. ");
					if (!leafObj.Done)
						status += L("More leaf-bundles still to gather. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("He's burning it nightly now instead of weekly. I told him he'd run out by winter. He said that was a problem for winter."));
			}
		});

		// Quest 1005: The Throneweaver
		//---------------------------------------------------------------------
		AddNpc(147481, L("[Bog-Trapper] Kazimieras"), "f_katyn_7_2", 3100, 900, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1005);

			dialog.SetTitle(L("Kazimieras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's crouched at the pool's edge, prodding a trap line that's come back empty again*{/}"));
				await dialog.Msg(L("Third empty trap this week. You've picked an odd place to wander, but stay a moment anyway — I've trapped these pools eleven years, know every channel in them blind. Two summers back the east pools started coming up webbed, right across the surface. Thick enough to walk on, if a person were fool enough to try."));
				await dialog.Msg(L("That's a Throneweaver, and it's been sitting under there getting fat on Red Meduja ever since. Kill 20 of them off the east pools and it'll have to come up and see who's taking its food."));

				var response = await dialog.Select(L("So. You in, or just here to watch an old trapper complain?"),
					Option(L("I'll take the Throneweaver"), "help"),
					Option(L("Webbed across the water?"), "info"),
					Option(L("Not for me"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't fight it on the web. It knows where every strand is and you don't, and that difference is the whole fight."));
						break;

					case "info":
						await dialog.Msg(L("Like a skin over the pool. Birds land on it and don't get up again. I lost a dog to it before I understood what I was looking at."));
						await dialog.Msg(L("Romas thinks it's why the bog's been giving things up lately - it's dragging the peat about down there. That's his theory and it's better than mine."));
						break;

					case "leave":
						await dialog.Msg(L("Suit yourself. It's not going anywhere and neither, apparently, am I."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thinMeduja", out var medujaObj)) return;
				if (!quest.TryGetProgress("killThroneweaver", out var bossObj)) return;

				if (medujaObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("The web's gone slack across the whole east pool. I put a line in this morning and pulled up an eel, which I have not done since before all this."));
					await dialog.Msg(L("Full price, and this on top - it came up in a trap two winters back and I never found anyone who wanted it."));

					character.Quests.Complete(questId);
				}
				else if (medujaObj.Done)
				{
					await dialog.Msg(L("Something's moving under the web. Get back to the east pools."));
				}
				else
				{
					await dialog.Msg(L("Still too many Meduja in the east pools. It's got no reason to come up."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four traps out and all four working. Romas came down and asked whether the bog had settled. I said it had. He looked like I'd handed him something."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: What the Bog Kept
//-----------------------------------------------------------------------------

public class WhatTheBogKeptQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1001);
		SetName(L("What the Bog Kept"));
		SetType(QuestType.Sub);
		SetDescription(L("Four hundred and six Katyn graves were filled with men the quartermasters had stripped, and the dead have not settled since. Recover their belongings from the bog for Gravekeeper Romas."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Gravekeeper] Romas"), "f_katyn_7_2");

		AddObjective("recoverBelongings", L("Recover Soldiers' Belongings from the bog"),
			new CollectItemObjective(650429, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650429, character.Inventory.CountItem(650429), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 8; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_7_2.Quest1001.Kit{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650429, character.Inventory.CountItem(650429), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 8; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_7_2.Quest1001.Kit{i}");
	}
}

// Quest 1002 CLASS: The Drowned Patrol
//-----------------------------------------------------------------------------

public class TheDrownedPatrolQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1002);
		SetName(L("The Drowned Patrol"));
		SetType(QuestType.Sub);
		SetDescription(L("Sakmoli denning under the wet sand took an entire dune patrol, and they still take anyone who walks that line. Kill enough of them that the gravekeeper can check his markers safely."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Drowned Soldier"), "f_katyn_7_2");

		AddObjective("killSakmoli", L("Kill Sakmoli along the dune line"),
			new KillObjective(30, new[] { MonsterId.Sakmoli }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1003 CLASS: The Urn of Private Vakaris
//-----------------------------------------------------------------------------

public class TheUrnOfPrivateVakarisQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1003);
		SetName(L("The Urn of Private Vakaris"));
		SetType(QuestType.Sub);
		SetDescription(L("Vakaris scratched his name into his own urn the night before the line broke, and his grave-stone has been blank for eleven years. Carry the urn up to Gravekeeper Romas."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Vakaris"), "f_katyn_7_2");

		AddObjective("deliverUrn", L("Take the Purified Urn to Gravekeeper Romas"),
			new ManualObjective());

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650433, character.Inventory.CountItem(650433), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_katyn_7_2.Quest1003.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650433, character.Inventory.CountItem(650433), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_katyn_7_2.Quest1003.Delivered");
	}
}

// Quest 1004 CLASS: Leaves for the Censer
//-----------------------------------------------------------------------------

public class LeavesForTheCenserQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1004);
		SetName(L("Leaves for the Censer"));
		SetType(QuestType.Sub);
		SetDescription(L("Ridimed leaf burned at the graves quiets the Katyn dead for a night, and Herbwife Danguole has run dry. Kill Ridimed in the bog and bring her crown-leaf bundles."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbwife] Danguole"), "f_katyn_7_2");

		AddObjective("killRidimed", L("Kill Ridimed in the bog"),
			new KillObjective(20, new[] { MonsterId.Ridimed }));

		AddObjective("gatherLeaves", L("Gather Ridimed Leaves"),
			new CollectItemObjective(650431, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(650431, 0.45f, MonsterId.Ridimed);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650431, character.Inventory.CountItem(650431), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650431, character.Inventory.CountItem(650431), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Throneweaver
//-----------------------------------------------------------------------------

public class TheThroneweaverQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1005);
		SetName(L("The Throneweaver"));
		SetType(QuestType.Sub);
		SetDescription(L("A Throneweaver has webbed the east pools over and grown fat on the Red Meduja beneath. Thin its food supply to bring it up, then kill it."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Bog-Trapper] Kazimieras"), "f_katyn_7_2");

		AddObjective("thinMeduja", L("Kill Red Meduja in the east pools"),
			new KillObjective(20, new[] { MonsterId.Jellyfish_Red }));

		AddObjective("killThroneweaver", L("Defeat the Throneweaver"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Throneweaver, 1) },
				resetIdent: "thinMeduja",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(603110, 1)); // Smurto Bracelet
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
