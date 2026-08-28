//--- Melia Script ----------------------------------------------------------
// Steel Heights - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_tableland_74 map. The outer watch above
// Kalejimas, and manifests that arrive already signed.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland74QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: The Corridor Is Not Held
		// =====================================================================
		// Gate-Sergeant Arvydas - Kepari in the approach
		//---------------------------------------------------------------------
		AddNpc(20144, L("[Gate-Sergeant] Arvydas"), "f_tableland_74", 179, -694, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1001);

			dialog.SetTitle(L("Arvydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A gate-sergeant is walking a fence line and putting his shoulder into every fourth stake to see which ones move*{/}"));
				await dialog.Msg(L("Hold up — let me finish this stake before I lose count of which ones failed. There. You're not on my roster, so you're either a visitor or trouble. I'll take either if it comes with a strong back."));
				await dialog.Msg(L("Steel Heights is the outer watch above Kalejimas. My job is a stockade corridor 900 paces long that everything going down to the prison walks through. The Black Kepari are in it. Kill 30 and bring me 8 pieces of the black stone they carry - I have 14 lamps and no fuel."));

				var response = await dialog.Select(L("Will you clear the corridor?"),
					Option(L("I'll clear it and get the stone"), "help"),
					Option(L("Everything walks through it?"), "info"),
					Option(L("Shut the corridor"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Work it from the north end down. If you start at the south they will simply move up ahead of you and you will do the whole 900 paces twice."));
						await dialog.Msg(L("The stone is in the gut, not the hand. It is a foul job and it is the only fuel on this ridge."));
						break;

					case "info":
						await dialog.Msg(L("Convoys down, empty carts up, and the prison's own traffic both ways. 14 of us hold it on contract. Not soldiers - contract. There has not been a soldier on Steel Heights in 6 years."));
						await dialog.Msg(L("I have asked for 40. I get 14 and a letter saying the establishment is under review, which it has been since before I took the post."));
						break;

					case "leave":
						await dialog.Msg(L("Then everything walks the open ground, in the dark, past Harugals. I would rather fight Kepari in a fence than explain that."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killKepari", out var killObj)) return;
				if (!quest.TryGetProgress("collectStone", out var stoneObj)) return;

				if (killObj.Done && stoneObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks the full 900 paces and lights every lamp on the way back, in order, without hurrying*{/}"));
					await dialog.Msg(L("14 lamps lit down a 900-pace fence. First time in 5 months anybody on this ridge has been able to see the whole corridor at once."));
					await dialog.Msg(L("Take the watch purse. 14 men on contract, paid for 40, and the difference has been sitting in a strongbox that I have never once been asked to account for."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Corridor's clear. 8 pieces of stone and I can light it."));
				}
				else
				{
					await dialog.Msg(L("30 Kepari, north end down. Do not work it from the south or you will do it twice."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Lit every night since. And Nijole has started standing at the gate when a convoy comes through, because now there is light to see faces by. She has not told me why she wants to see faces."));
			}
		});

		// =====================================================================
		// QUEST 1002: Something to Put People Under
		// =====================================================================
		// Contract Physician Nijole - salivary glands off the Tini Magicians
		//---------------------------------------------------------------------
		AddNpc(152065, L("[Contract Physician] Nijole"), "f_tableland_74", 1475, 742, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1002);

			dialog.SetTitle(L("Nijole"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A physician is rolling bandage off a bolt by eye, measuring nothing, getting it exactly right every time*{/}"));
				await dialog.Msg(L("Sit if you're hurt, stand if you're not — good, you're standing. Then you're either healthy or here to make my day more interesting."));
				await dialog.Msg(L("I am the only physician between Sventimas and the prison gate and I have no anaesthetic. Brown Tini Magicians carry a gland that numbs on contact. Bring me 6 and I can set a bone without 3 people holding somebody down."));

				var response = await dialog.Select(L("Will you get me 6 glands?"),
					Option(L("I'll bring 6"), "help"),
					Option(L("Who are you treating?"), "info"),
					Option(L("Requisition proper supplies"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut the gland out cold, not warm. If you take it off something still moving, the gland empties and you have brought me a bag."));
						await dialog.Msg(L("They are west and they are aggressive and they are not individually dangerous. It is the third one that arrives that kills people."));
						break;

					case "info":
						await dialog.Msg(L("The 14 on the watch. And whatever comes up the corridor from the prison, and whatever goes down it. I do not get told which of those I am allowed to treat and so I have decided I treat all of them."));
						await dialog.Msg(L("I have been here 2 years. In 2 years I have treated 9 convoy arrivals. 9, out of every convoy that has come through. Nobody arrives here needing a physician and that is not how walking 4 days works."));
						break;

					case "leave":
						await dialog.Msg(L("I have. 6 times. The answers come back approved and nothing comes up the road. I have 6 approvals and no anaesthetic and I have stopped finding that funny."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectGlands", out var glandObj)) return;

				if (glandObj.Done)
				{
					await dialog.Msg(L("{#666666}*She presses one against the back of her own wrist, counts to 20, and then pinches the skin and feels nothing*{/}"));
					await dialog.Msg(L("Full, all 6. That is a year of setting bones properly, and it came off a monster because the Kingdom would not send me a bottle."));
					await dialog.Msg(L("Take the physician's fee. I draw it and I spend it on bandage, and this month I have bandage, so it is yours."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("6 glands, off the Brown Tini Magicians, west. Cut them cold. And mind the third one."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Set 2 arms and a collarbone this week without anybody being held down. And I have started writing the number of convoy arrivals I see in the same book, which Arvydas has noticed and has not asked about."));
			}
		});

		// =====================================================================
		// QUEST 1003: Nine Miles of Line
		// =====================================================================
		// Wire-Runner Ignas - the Spion Mages on the signal run
		//---------------------------------------------------------------------
		AddNpc(147481, L("[Wire-Runner] Ignas"), "f_tableland_74", 1017, 1864, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1003);

			dialog.SetTitle(L("Ignas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A wire-runner has a coil over one shoulder and is following a line of poles with his eyes rather than walking it*{/}"));
				await dialog.Msg(L("Careful, don't step in the coil — there. First person I've talked to today who isn't a pole. That's not a complaint, it's just been a quiet nine miles."));
				await dialog.Msg(L("I keep the signal line from the prison gate up to the watch house. 9 miles of it and I walk all 9 twice a week. The White Spion Mages sit on the poles and they take the line down for the metal. Kill 20 and I will have a line that stays up."));

				var response = await dialog.Select(L("Will you clear the pole line?"),
					Option(L("I'll clear the line"), "help"),
					Option(L("What runs on the line?"), "info"),
					Option(L("Let it stay down"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They sit high and they drop on you. Watch the pole tops, not the ground. If a pole has no bird on it, something else is on it."));
						await dialog.Msg(L("Work north to south along the poles and you will not miss any. Off the line they scatter and you will be at it all day."));
						break;

					case "info":
						await dialog.Msg(L("Arrival counts, mostly. The gate signals a number up, the watch house writes it down, the watch house signals it on to Roxona."));
						await dialog.Msg(L("I have run that line 4 years and I have never once been told a number. I just keep the wire up so a number can travel along it."));
						break;

					case "leave":
						await dialog.Msg(L("Then the gate cannot signal and the watch house sends the number on anyway, out of the book, which is what happened for the 6 weeks the line was down last spring."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpionMages", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks the pole line with the coil and splices 3 breaks in it without stopping talking*{/}"));
					await dialog.Msg(L("Line's up and it will stay up. First continuous run gate to watch house since the spring."));
					await dialog.Msg(L("Take the line allowance. And I will tell you the thing I said last spring and nobody wanted: the numbers the watch house sent on while the line was down matched the numbers the gate had. Exactly. For 6 weeks. With no line."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Along the poles, north to south. Watch the tops. 20 of them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Line's held 11 days. Sigita has been comparing what comes up it against what she receives and she has gone very quiet, which for Sigita is a shout."));
			}
		});

		// =====================================================================
		// QUEST 1004: Four Gates on One Corridor
		// =====================================================================
		// Manifest Clerk Sigita - sheets that arrive already signed
		//---------------------------------------------------------------------
		AddNpc(20143, L("[Manifest Clerk] Sigita"), "f_tableland_74", 345, -140, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1004);

			dialog.SetTitle(L("Sigita"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A clerk has four manifest sheets laid out and is holding a fifth up to the light at the signature*{/}"));
				await dialog.Msg(L("One moment — I want a second pair of eyes on this and you'll do, since you clearly haven't got an opinion yet."));
				await dialog.Msg(L("Every convoy that comes down this corridor passes 4 gates and each gate stamps the sheet. That is the whole point of a corridor. Go and read the tally boards at all 4 gates and tell me the numbers on them."));

				var response = await dialog.Select(L("Will you read the 4 gate boards?"),
					Option(L("I'll read all 4"), "help"),
					Option(L("What's wrong with the sheet?"), "info"),
					Option(L("Just file it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663142, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the charge. The 2nd gate has been jammed shut 5 months and nobody has been sent to open it - blow the pin, do not lever it, or you will bring the whole panel down on the board."));
						await dialog.Msg(L("Read what is chalked, not what is scratched underneath. The underneath is old and it will make the count wrong."));
						break;

					case "info":
						await dialog.Msg(L("It arrived signed. Received and counted, at the bottom, in a hand that is not mine and not the gate-sergeant's and not anybody's at Kalejimas. And it arrived before the convoy did."));
						await dialog.Msg(L("I have 31 sheets in that drawer and I have just started checking. 9 of them are signed in that hand. All 9 are convoys nobody at the gate remembers seeing."));
						break;

					case "leave":
						await dialog.Msg(L("I have filed 31 of them. That is what a manifest clerk does and it is what I have done for 3 years, and I would like to be able to say I stopped."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var read = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_74.Quest1004.Read", 0);

				if (read >= 4)
				{
					await dialog.Msg(L("{#666666}*She writes the 4 gate counts under each other and puts the manifest beside them and does not need to say the number out loud*{/}"));
					await dialog.Msg(L("First gate 40. Second gate 40. Third gate 22. Fourth gate 22. And the sheet says 40 received, signed before any of it happened."));
					await dialog.Msg(L("18 people stop existing between the second gate and the third, inside my corridor, and the paper does not blink. Take this. Then go to Vytautas at the prison gate, and make him look at it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("4 gates on the corridor. Read what is chalked, not scratched. {0} of 4 read.", read));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have pulled all 31 sheets and set them against the gate books. The gap is always between the 2nd gate and the 3rd and it is never the same size, and it has been happening since before I was posted here."));
			}
		});

		// =====================================================================
		// CORRIDOR GATES
		// =====================================================================
		// For Quest 1004 - Four Gates on One Corridor
		// =====================================================================

		void AddCorridorGate(int gateNumber, string gateName, string tally, int x, int z, int direction)
		{
			AddNpc(153151, L(gateName), "f_tableland_74", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_74", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A stockade gate with a chalked tally board bolted to the post beside it*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_74.Quest1004.Gate{gateNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have this gate's number written down*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the tally board..."), "Cancel", "SITREAD", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Reading interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);

				var read = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_74.Quest1004.Read", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_tableland_74.Quest1004.Read", read);

				character.ServerMessage(L(tally));
				character.ServerMessage(LF("Gate boards read: {0}/4", read));

				if (read >= 4)
					character.ServerMessage(L("{#FFD700}All 4 gate boards read. Return to Manifest Clerk Sigita.{/}"));
			});
		}

		AddCorridorGate(1, "First Corridor Gate", "Chalked: 40 through. Stamp fresh, gate swings clean.", 256, -518, 263);
		AddCorridorGate(2, "Second Corridor Gate", "Chalked: 40 through. The gate itself has been jammed shut for 5 months.", 82, -551, 275);
		AddCorridorGate(3, "Third Corridor Gate", "Chalked: 22 through. Same convoy, same night, same hand.", 126, 252, 103);
		AddCorridorGate(4, "Fourth Corridor Gate", "Chalked: 22 through. Forty in at the top of the corridor and 22 out of the bottom.", 667, -213, 0);

		// =====================================================================
		// The Telepathy Device - atmosphere on the west ground
		//---------------------------------------------------------------------
		AddNpc(153156, L("Signalling Device"), "f_tableland_74", -583, -1038, 319, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A device on a levelled stone platform out on the west ground, well away from the corridor and the pole line both*{/}"));
			await dialog.Msg(L("{#666666}*It is not connected to anything. It faces southeast, back down the whole length of the shelf - past Sventimas, past Mandara, along the exact heading four sighting orbs and a figurine are turned to*{/}"));
			await dialog.Msg(L("{#666666}*This is the end the others are pointed at*{/}"));
		});

		// =====================================================================
		// QUEST 1005: Between the Second Gate and the Third
		// =====================================================================
		// Watch-Officer Vytautas - the man at the prison gate
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Watch-Officer] Vytautas"), "f_tableland_74", 1951, 2186, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1005);

			dialog.SetTitle(L("Vytautas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_tableland_74", 1004)))
				{
					await dialog.Msg(L("{#666666}*He straightens up from the gate log as you approach, sizing you up the way he sizes up everyone who walks this corridor*{/}"));
					await dialog.Msg(L("You're not on any manifest I've signed. Sigita has been trying to get somebody to read her gate boards for a fortnight — go and read them. Then come back and we will both be looking at the same thing."));
					return;
				}

				await dialog.Msg(L("{#666666}*He waves you inside without the usual once-over, already reaching for the sheet*{/}"));
				await dialog.Msg(L("40 in, 22 out, and a sheet signed before the convoy walked. I have held this gate 8 years and I have signed every one of those sheets on arrival without reading the top of them."));
				await dialog.Msg(L("There is a device out on the west ground that is not wired to anything and faces back down the whole shelf. The Blue Hohen Gulaks are on that ground and 2 Harugals are standing on the platform itself. Kill 25 Gulaks and take the pair."));

				var response = await dialog.Select(L("Will you go to the west platform?"),
					Option(L("I'll take the platform"), "help"),
					Option(L("Eight years and you never read them?"), "info"),
					Option(L("Send this to Roxona"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663143, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take Argis's altered charge - Sigita brought 2 up from Sventimas and did not tell me where from. Put it under the platform lip, not on the device. I want the ground opened, not the thing broken."));
						await dialog.Msg(L("Gulaks first. Harugals are lv93 and they will not leave that platform, which is the only reason 2 of them are a fight and not an execution."));
						break;

					case "info":
						await dialog.Msg(L("Never. A manifest comes to the gate and I put my name where the name goes. That is the job and 8 years of doing it correctly is why I have it."));
						await dialog.Msg(L("Gomen at Mesafasla signs 60 forms a day. I sign 4 a month. I have exactly one less excuse than he has and I have thought about that every day since Sigita showed me the drawer."));
						break;

					case "leave":
						await dialog.Msg(L("Roxona is where the number goes when it leaves this ridge. If I send this to Roxona I am sending it to the far end of the line I am trying to describe."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearWestGround", out var groundObj)) return;
				if (!quest.TryGetProgress("takeThePlatform", out var pairObj)) return;

				if (groundObj.Done && pairObj.Done)
				{
					await dialog.Msg(L("{#666666}*He goes out to the opened platform himself with 4 of the watch and they all come back walking slowly and not talking to each other*{/}"));
					await dialog.Msg(L("Boots. Under the platform, in courses, laces tied, and every pair facing the same way. Ignas counted 400 and stopped because he could not keep his hands steady."));
					await dialog.Msg(L("Take this. It is the gate-officer's, it is mine, and I am not going to be one much longer. Sigita is copying all 31 sheets and Nijole is writing what she has seen and I am taking the lot to Mavern by hand, because there is no post on this road that is not on the line."));

					character.Quests.Complete(questId);
				}
				else if (groundObj.Done)
				{
					await dialog.Msg(L("West ground's clear. The 2 on the platform have not stepped off it. Not once, not in 8 years that I know of."));
				}
				else
				{
					await dialog.Msg(L("25 Gulaks first. Nobody goes at a pair of Harugals with Gulaks still on the ground behind them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Gate's open, corridor's lit, and every sheet through it this month has been counted at all 4 boards and signed after. It is 4 sheets. It is not 828 people."));
				await dialog.Msg(L("Sigita asked me what we do now. I told her we count. That is the whole answer I have and it is more than this road has had in 8 years."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Corridor Is Not Held
//-----------------------------------------------------------------------------

public class TheCorridorIsNotHeldQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1001);
		SetName(L("The Corridor Is Not Held"));
		SetType(QuestType.Sub);
		SetDescription(L("Fourteen contract men hold a 900-pace stockade corridor that everything bound for Kalejimas walks through, and the Black Kepari are inside it. Arvydas also has 14 lamps and no fuel."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Gate-Sergeant] Arvydas"), "f_tableland_74");

		AddObjective("killKepari", L("Kill Black Kepari inside the corridor"),
			new KillObjective(30, new[] { MonsterId.Kepari_Purple }));

		AddObjective("collectStone", L("Recover black stone for the corridor lamps"),
			new CollectItemObjective(663140, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(663140, 0.35f, MonsterId.Kepari_Purple);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663140, character.Inventory.CountItem(663140), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663140, character.Inventory.CountItem(663140), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Something to Put People Under
//-----------------------------------------------------------------------------

public class SomethingToPutPeopleUnderQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1002);
		SetName(L("Something to Put People Under"));
		SetType(QuestType.Sub);
		SetDescription(L("Nijole is the only physician between Sventimas and the prison gate and has had 6 supply requisitions approved and nothing delivered. Brown Tini Magicians carry a gland that numbs on contact."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Contract Physician] Nijole"), "f_tableland_74");

		AddObjective("collectGlands", L("Cut salivary glands from Brown Tini Magicians"),
			new CollectItemObjective(663127, 6));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(663127, 0.35f, MonsterId.Tiny_Mage_Brown);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663127, character.Inventory.CountItem(663127), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663127, character.Inventory.CountItem(663127), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Nine Miles of Line
//-----------------------------------------------------------------------------

public class NineMilesOfLineQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1003);
		SetName(L("Nine Miles of Line"));
		SetType(QuestType.Sub);
		SetDescription(L("Ignas keeps 9 miles of signal line between the prison gate and the watch house, and the White Spion Mages sit on the poles and take it down for the metal."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Wire-Runner] Ignas"), "f_tableland_74");

		AddObjective("killSpionMages", L("Kill White Spion Mages along the pole line"),
			new KillObjective(20, new[] { MonsterId.Spion_Mage_White }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Four Gates on One Corridor
//-----------------------------------------------------------------------------

public class FourGatesOnOneCorridorQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1004);
		SetName(L("Four Gates on One Corridor"));
		SetType(QuestType.Sub);
		SetDescription(L("A manifest arrived at the prison gate already signed received and counted, in a hand nobody at Kalejimas knows, before the convoy did. Read the chalked tally at all 4 corridor gates."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Manifest Clerk] Sigita"), "f_tableland_74");

		AddObjective("readGates", L("Read the tally boards at the 4 corridor gates"),
			new VariableCheckObjective("Laima.Quests.f_tableland_74.Quest1004.Read", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663142, character.Inventory.CountItem(663142), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_74.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_74.Quest1004.Gate{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663142, character.Inventory.CountItem(663142), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_74.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_74.Quest1004.Gate{i}");
	}
}

// Quest 1005 CLASS: Between the Second Gate and the Third
//-----------------------------------------------------------------------------

public class BetweenTheSecondGateAndTheThirdQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1005);
		SetName(L("Between the Second Gate and the Third"));
		SetType(QuestType.Sub);
		SetDescription(L("A device stands on the west ground wired to nothing, facing back down the whole shelf along the heading the Sventimas orbs and the Mandara pillar are turned to. Two Blue Harugals stand on its platform."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Watch-Officer] Vytautas"), "f_tableland_74");

		AddPrerequisite(new CompletedPrerequisite("f_tableland_74", 1004));

		AddObjective("clearWestGround", L("Kill Blue Hohen Gulaks on the west ground"),
			new KillObjective(25, new[] { MonsterId.Hohen_Gulak_Blue }));

		AddObjective("takeThePlatform", L("Take the pair standing on the platform"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Harugal_Blue, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Hohen_Gulak_Blue, 3),
				},
				resetIdent: "clearWestGround",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(103120, 1)); // Pierene Sword
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663143, character.Inventory.CountItem(663143), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663143, character.Inventory.CountItem(663143), InventoryItemRemoveMsg.Destroyed);
	}
}
