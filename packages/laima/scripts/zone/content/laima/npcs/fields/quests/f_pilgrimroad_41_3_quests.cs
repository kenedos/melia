//--- Melia Script ----------------------------------------------------------
// Rasvoy Lake Quest NPCs
//--- Description -----------------------------------------------------------
// The abbey's lakeside relay, where nothing has come down the Mavern road in
// weeks and a Green Minos warband is sitting on the north shore cut.
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

public class FPilgrimroad413QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Orbs off the Supply Party
		//---------------------------------------------------------------------
		AddNpc(155126, L("[Monk] Stella"), "f_pilgrimroad_41_3", -723, 545, 279, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1001);

			dialog.SetTitle(L("Stella"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's kneeling at the water's edge with her ear tilted toward a small stone bowl, utterly still*{/}"));
				await dialog.Msg(L("One moment - I was listening for a warmth that hasn't come. There, it's gone now, and so are you a stranger instead of a returning face. I have kept the lake relay for Mavern Abbey for 8 years. Everything that walks to the abbey stops here first and everything the abbey sends down passes through my hands."));
				await dialog.Msg(L("3 supply parties are overdue and the Green Minos on the abbey road are carrying the relay orbs from the last one. Kill 25 of them and bring me 6 orbs back."));

				var response = await dialog.Select(L("Will you go up the abbey road?"),
					Option(L("I'll recover 6 orbs"), "help"),
					Option(L("What is a relay orb for?"), "info"),
					Option(L("3 parties is a search, not an errand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They hang them off the belt because they're warm. Follow the road up from the water and you'll be walking through the warband inside 200 paces."));
						break;

					case "info":
						await dialog.Msg(L("It holds a name. When a party leaves the abbey the orbs are keyed to who is in it, and when the party arrives the orbs go cold."));
						await dialog.Msg(L("All 6 of the ones I can hear from here are still warm, which is the part I have not written down and do not intend to."));
						break;

					case "leave":
						await dialog.Msg(L("It would be, if there were anyone to search. There is a boatman who won't go north and 4 pilgrims stranded on the far shore, and there is me."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var killObj)) return;
				if (!quest.TryGetProgress("collectOrbs", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays the orbs out in a row on the stone and holds a hand flat above each one*{/}"));
					await dialog.Msg(L("6 warm. I'll send the names up to the abbey tonight and the abbey will do what abbeys do, which is send 4 more people down the same road."));
					await dialog.Msg(L("Take the relay purse. It exists to pay boatmen and I have not been able to pay one to go anywhere useful in a month."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The road's quieter. The orbs will be on the ones you've already put down - go back over the ground rather than pushing further up."));
				}
				else
				{
					await dialog.Msg(L("Still too many on the road. Work it from the water upward, not from the top down."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("6 names went up and 6 lamps were lit for them at the abbey. I could see them from here for the first time in weeks, and I sat on this stone and watched until they went out."));
			}
		});

		// Quest 1002: The East Shore Is Stripped
		//---------------------------------------------------------------------
		AddNpc(152064, L("[Weary Pilgrim] Danute"), "f_pilgrimroad_41_3", 962, 110, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1002);

			dialog.SetTitle(L("Danute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's sitting with her knees drawn up, watching the far shore like it might move closer if she stares hard enough*{/}"));
				await dialog.Msg(L("You came from the water side? Then maybe you're the first useful thing to wash up here in a week. 12 days on the wrong shore. There are 5 of us, the boatman won't cross at night, and 2 of my party have been drinking lake water since the flasks ran out."));
				await dialog.Msg(L("The herb that settles it grows all along this shore and the Brown Lapasape Mages have stripped every stand of it and are carrying it around in bundles. Get me 10 of them."));

				var response = await dialog.Select(L("Will you get the herb?"),
					Option(L("I'll bring you 10 bundles"), "help"),
					Option(L("Why are they collecting it?"), "info"),
					Option(L("Boil the water instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They keep the bundles dry, so they hold them up when they wade. That's the moment - they're slow and they won't drop them."));
						break;

					case "info":
						await dialog.Msg(L("I've watched them for 12 days and they aren't eating it. They carry it east and come back without it, and then they go and get more."));
						await dialog.Msg(L("Something east of this shore wants a great deal of a plant that only settles a stomach, and I have stopped trying to make that make sense."));
						break;

					case "leave":
						await dialog.Msg(L("With what? We have 1 pot, and the last time we lit a fire on this shore we had Lapasape in the camp inside an hour."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectHerb", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She breaks a stem, smells the cut, and starts sorting the bundles without waiting*{/}"));
					await dialog.Msg(L("All 10 still green. That's both of them dosed for 5 days and enough over for whoever gets sick next, and someone will."));
					await dialog.Msg(L("Take this. It's the offering I've carried since Orsha and I am not going to reach the abbey with it anyway."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough. Try further up the shore where the reeds thicken - that's where they're working now."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Both are keeping food down. One of them walked to the water and back on her own this morning, which 12 days ago I would not have counted as news."));
			}
		});

		// Quest 1003: The West Shore Path
		//---------------------------------------------------------------------
		AddNpc(156110, L("[Boatman] Row"), "f_pilgrimroad_41_3", -737, 378, 8, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1003);

			dialog.SetTitle(L("Row"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's bailing an empty boat that clearly doesn't need bailing, just to have something to do with his hands*{/}"));
				await dialog.Msg(L("Don't mind me, habit from the navy days. You waiting on a crossing, or just wandering? Either way, sit - 26 years on salt water and 4 on this lake, and I'll tell you the lake is worse. On salt water the thing that wants you is in the water."));
				await dialog.Msg(L("Green Minos Archers have the west shore path, and that path is how my passengers get down to the boat. Kill 25 of them and I can load in daylight like a man with a trade."));

				var response = await dialog.Select(L("Will you clear the shore path?"),
					Option(L("I'll kill 25 Minos Archers"), "help"),
					Option(L("Why won't you cross at night?"), "info"),
					Option(L("Move your landing"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They shoot down the path, not across it. Get off the path into the scrub and they have to come to you, and a Minos coming to you is a much simpler animal."));
						break;

					case "info":
						await dialog.Msg(L("Because they wade. In the dark you don't see them wade and the first you know is the boat sitting 3 inches lower than it should."));
						await dialog.Msg(L("I've been boarded twice in my life. Once by the Kingdom navy and once on this lake, and the navy was the polite one."));
						break;

					case "leave":
						await dialog.Msg(L("There's 1 shelf on this whole west shore you can bring a loaded boat onto. I know, because I spent a fortnight looking for a second."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Walked the path down at noon carrying a barrel on my shoulder and nothing put a shaft in it. First time this year."));
					await dialog.Msg(L("Here's your fare back and then some. And if you want crossing, you get crossing, and you don't pay - that's the arrangement now."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still shooting down the path. Work the upper end where the scrub comes in close, they've no lane there."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Took 5 across yesterday and 5 back, in daylight, with the boat sitting where it ought to. I have started whistling again and the monk has noticed."));
			}
		});

		// Quest 1004: Three Lamps Unlit
		//---------------------------------------------------------------------
		AddNpc(155035, L("[Abbey Courier] Nerijus"), "f_pilgrimroad_41_3", 850, 127, 27, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1004);

			dialog.SetTitle(L("Nerijus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's pacing a short line of shore, satchel clutched to his chest, glancing at the water every few steps*{/}"));
				await dialog.Msg(L("You're not the boatman - shame, but talk to me anyway. I carry for the abbey and I am on the wrong side of a lake with a sealed satchel, which is the single most useless thing a courier can be."));
				await dialog.Msg(L("The abbey signals across by lamp. 3 signal stones on this shore, all 3 dark, and until one lights the far side thinks nobody is here. Go to all 3 and see what's wrong with them."));

				var response = await dialog.Select(L("Will you walk the signal stones?"),
					Option(L("I'll check all 3 stones"), "help"),
					Option(L("Can't you just shout?"), "info"),
					Option(L("Give me the satchel, I'll walk it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Look at the bowl and the wick channel both. A stone that won't light and a stone that has been made not to light look nothing alike up close."));
						break;

					case "info":
						await dialog.Msg(L("Across 2 miles of open water into the wind. The stones exist because shouting was tried, and there is a monk buried at Ouaas who is the reason it stopped being tried."));
						await dialog.Msg(L("The lamps are also how the abbey counts who is still on the road. 3 dark stones reads, up there, as nobody left alive on this shore."));
						break;

					case "leave":
						await dialog.Msg(L("It's sealed to my hand. If you open it the wax records that it was opened, and then it isn't a message any more, it's a rumour."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("checkStones", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to all 3 accounts without interrupting, then swears quietly and precisely*{/}"));
					await dialog.Msg(L("Wick channels packed with lake clay. All 3. Wind doesn't pack clay into a channel from underneath."));
					await dialog.Msg(L("Take the courier's road money. I'm going to clear those channels tonight and light all 3 at once, and whoever packed them can watch me do it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 3 yet. They're spread along the shore between the reeds and the north point - you'll not miss a stone, they're chest high."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("3 lamps burning by dark and an answering lamp off the abbey wall inside the hour. My satchel goes up with the next crossing and I get to stop being useless."));
			}
		});

		// Quest 1004 collection points - the abbey signal stones
		//---------------------------------------------------------------------
		void AddSignalStone(int stoneNumber, string observation, int x, int z, int direction)
		{
			AddNpc(47190, L("Abbey Signal Stone"), "f_pilgrimroad_41_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_pilgrimroad_41_3", 1004);
				var variableKey = $"Laima.Quests.f_pilgrimroad_41_3.Quest1004.Stone{stoneNumber}";
				var counterKey = "Laima.Quests.f_pilgrimroad_41_3.Quest1004.StonesChecked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A chest-high shore stone with a lamp bowl cut into the top*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already looked this one over*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Looking over the signal stone..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var checkedCount = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, checkedCount);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Signal stones checked: {0}/3", checkedCount));

					if (checkedCount >= 3)
						character.ServerMessage(L("{#FFD700}All 3 stones checked. Return to Nerijus.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the stone unchecked."));
				}
			});
		}

		AddSignalStone(1,
			L("First Stone: dry oil in the bowl and a wick channel packed solid with grey lake clay."), 780, 426, 0);
		AddSignalStone(2,
			L("Second Stone: the same clay, packed from underneath, and pressed in with something narrower than a finger."), 989, 1036, 0);
		AddSignalStone(3,
			L("Third Stone: clay again, and 4 shallow scrapes on the bowl rim where a hand braced to reach in."), 1179, 429, 258);

		// Quest 1005: What the Mirror Shows
		//---------------------------------------------------------------------
		AddNpc(155126, L("[Monk] Stella"), "f_pilgrimroad_41_3", 1001, 1197, 275, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1005);

			dialog.SetTitle(L("Stella"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_pilgrimroad_41_3", 1001)))
				{
					await dialog.Msg(L("Bring me the orbs off the supply party first. I'll not go to the north cut guessing at how many are already dead up there."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's holding a small hand mirror up to the lamplight, turning it slowly, and doesn't look away from it as you approach*{/}"));
				await dialog.Msg(L("Good, you're back - I need eyes I trust for this one. A letter came up from Salvia Forest. Friar Clark's altar was emptied by widlings and he wanted to know whether anything like it had happened here. Something like it has been happening here for a month."));
				await dialog.Msg(L("The warband holds the north cut where the road turns for Ouaas, and 2 of them do nothing but stand over a standard. Kill 20 Green Minos to open the cut, then take the standard down. Carry my Mirror of Truth while you do it."));

				var response = await dialog.Select(L("Will you take the north cut?"),
					Option(L("I'll break the warband's standard"), "help"),
					Option(L("What does the mirror do?"), "info"),
					Option(L("Send to the abbey for soldiers"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(666107, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Keep it out of the satchel and facing the fight. It shows a thing as it is, and a thing as it is has to be looked at while it's still standing up."));
						break;

					case "info":
						await dialog.Msg(L("Every relay monk is issued one. It shows what a thing actually is rather than what it is doing, and it is worth nothing at all in a quiet year."));
						await dialog.Msg(L("Mine has been blank for 8 years. It stopped being blank the week the first supply party failed to arrive."));
						break;

					case "leave":
						await dialog.Msg(L("The abbey has 40 people in it and 31 of them are over 60. That is why there is a monk keeping a relay hut on a lake instead of a garrison."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openCut", out var cutObj)) return;
				if (!quest.TryGetProgress("takeStandard", out var standardObj)) return;

				if (cutObj.Done && standardObj.Done)
				{
					await dialog.Msg(L("{#666666}*She takes the mirror back and turns it over twice before she says anything*{/}"));
					await dialog.Msg(L("The warband didn't come out of this forest. It came down the Ouaas road, and what the mirror caught behind it was not a Minos and was not standing on the ground."));
					await dialog.Msg(L("Take the blade off the standard. It came out of the abbey armoury 40 years ago and it should not have been on that road. I'm writing to Ouaas Memorial tonight - Monk Matas keeps the ground up there, and I would like him to walk his ring and count."));

					character.Quests.Complete(questId);
				}
				else if (cutObj.Done)
				{
					await dialog.Msg(L("The cut's open. The standard is still up and the 2 who tend it will not leave it, so it has to be done at the standard itself."));
				}
				else
				{
					await dialog.Msg(L("The cut is still packed with them. Open it first - you don't want the warband behind you when the standard comes down."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("A supply party came down the road on its own feet this week and the orbs went cold in my hand as they walked in. 8 years I've waited to feel that and it took 4 seconds."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Orbs off the Supply Party
//-----------------------------------------------------------------------------

public class TheOrbsOffTheSupplyPartyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1001);
		SetName(L("The Orbs off the Supply Party"));
		SetType(QuestType.Sub);
		SetDescription(L("3 supply parties out of Mavern Abbey are overdue, and the Green Minos holding the abbey road are wearing the last party's relay orbs on their belts. Kill them and bring the orbs back to the lakeside relay."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Monk] Stella"), "f_pilgrimroad_41_3");

		AddObjective("killMinos", L("Kill Green Minos on the abbey road"),
			new KillObjective(25, new[] { MonsterId.Minos_Green }));

		AddObjective("collectOrbs", L("Recover Maven Abbey Orbs"),
			new CollectItemObjective(666114, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(666114, 0.35f, MonsterId.Minos_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666114, character.Inventory.CountItem(666114), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666114, character.Inventory.CountItem(666114), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: The East Shore Is Stripped
//-----------------------------------------------------------------------------

public class TheEastShoreIsStrippedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1002);
		SetName(L("The East Shore Is Stripped"));
		SetType(QuestType.Sub);
		SetDescription(L("5 pilgrims have been stranded on the far shore for 12 days and 2 of them are drinking lake water. The herb that settles it has been stripped off the whole shore by Brown Lapasape Mages, who are carrying it east in bundles."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Weary Pilgrim] Danute"), "f_pilgrimroad_41_3");

		AddObjective("collectHerb", L("Take Herb of Restoration bundles off the Lapasape Mages"),
			new CollectItemObjective(666116, 10));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion

		AddDrop(666116, 0.45f, MonsterId.Lapasape_Mage_Brown);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666116, character.Inventory.CountItem(666116), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666116, character.Inventory.CountItem(666116), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The West Shore Path
//-----------------------------------------------------------------------------

public class TheWestShorePathQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1003);
		SetName(L("The West Shore Path"));
		SetType(QuestType.Sub);
		SetDescription(L("The only path down to the lake's one loading shelf is held by Green Minos Archers, and the boatman cannot bring passengers to his boat in daylight. Kill 25 of them."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Boatman] Row"), "f_pilgrimroad_41_3");

		AddObjective("killArchers", L("Kill Green Minos Archers on the west shore path"),
			new KillObjective(25, new[] { MonsterId.Minos_Bow_Green }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Three Lamps Unlit
//-----------------------------------------------------------------------------

public class ThreeLampsUnlitQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1004);
		SetName(L("Three Lamps Unlit"));
		SetType(QuestType.Sub);
		SetDescription(L("Mavern Abbey signals across the lake by lamp, and all 3 signal stones on this shore are dark - which the abbey reads as nobody left alive on it. Walk all 3 and find out what is wrong with them."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Abbey Courier] Nerijus"), "f_pilgrimroad_41_3");

		AddObjective("checkStones", L("Check all 3 abbey signal stones"),
			new VariableCheckObjective("Laima.Quests.f_pilgrimroad_41_3.Quest1004.StonesChecked", 3, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_3.Quest1004.StonesChecked");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_3.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_pilgrimroad_41_3.Quest1004.StonesChecked");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_pilgrimroad_41_3.Quest1004.Stone{i}");
	}
}

// Quest 1005 CLASS: What the Mirror Shows
//-----------------------------------------------------------------------------

public class WhatTheMirrorShowsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1005);
		SetName(L("What the Mirror Shows"));
		SetType(QuestType.Sub);
		SetDescription(L("The Green Minos warband holds the north cut where the abbey road turns for Ouaas, and 2 of them do nothing but stand over a standard. Open the cut and take the standard down while carrying the relay monk's Mirror of Truth."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Monk] Stella"), "f_pilgrimroad_41_3");

		AddPrerequisite(new CompletedPrerequisite("f_pilgrimroad_41_3", 1001));

		AddObjective("openCut", L("Kill Green Minos holding the north cut"),
			new KillObjective(20, new[] { MonsterId.Minos_Green }));

		AddObjective("takeStandard", L("Take down the warband's standard"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Minos_Green, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Minos_Bow_Green, 3),
				},
				resetIdent: "openCut",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(16000, 11000));
		AddReward(new SilverReward(20000));
		AddReward(new ItemReward(103114, 1)); // Holy Blade
		AddReward(new ItemReward(640084, 3)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666107, character.Inventory.CountItem(666107), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666107, character.Inventory.CountItem(666107), InventoryItemRemoveMsg.Destroyed);
	}
}
