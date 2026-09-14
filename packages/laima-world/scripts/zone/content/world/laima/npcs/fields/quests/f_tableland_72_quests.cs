//--- Melia Script ----------------------------------------------------------
// Sventimas Exile Zone - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_tableland_72 map. Thirty-one convoys
// received and a village that does not add up.
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

public class FTableland72QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: A Village Nobody Provisioned
		// =====================================================================
		// Priest Kaleims - the White Spions in the plots
		//---------------------------------------------------------------------
		AddNpc(155043, L("[Priest] Kaleims"), "f_tableland_72", -425, 70, 266, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1001);

			dialog.SetTitle(L("Kaleims"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A village priest is turning a spade in a vegetable plot, in a cassock, badly and with great determination*{/}"));
				await dialog.Msg(L("Bless me, a face I don't know. You're not sentenced here, are you — no, you'd have the look. Come, walk the row with me while I ruin it."));
				await dialog.Msg(L("Nobody provisions an exile zone. The Kingdom walks people up here, sets them down, and the word delivered goes in a ledger. So we grow food, and the White Spions eat it. Kill 30 of them and bring me 8 of their essence - it goes into the ground and doubles what the plots give."));

				var response = await dialog.Select(L("Will you clear the plots?"),
					Option(L("I'll clear them and get the essence"), "help"),
					Option(L("Nobody sends anything at all?"), "info"),
					Option(L("Petition Roxona"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come in off the west scrub at dusk, always the same 4 gaps in the hedge. Stand in a gap and you will not have to walk far."));
						await dialog.Msg(L("The essence is in the sac behind the head. Take it whole - a burst one is worth nothing to the ground."));
						break;

					case "info":
						await dialog.Msg(L("Salt, twice a year, because salt is in the sentence. Nothing else. I have been priest here 6 years and I have received salt 12 times and nothing else 12 times."));
						await dialog.Msg(L("I do not say that bitterly any more. Sventimas feeds Sventimas. It is the one thing here that nobody has to be told to do."));
						break;

					case "leave":
						await dialog.Msg(L("I have written 9 petitions. 4 were answered. All 4 answers said the sentence had been carried out correctly, which was not the question in any of them."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpions", out var killObj)) return;
				if (!quest.TryGetProgress("collectEssence", out var essObj)) return;

				if (killObj.Done && essObj.Done)
				{
					await dialog.Msg(L("{#666666}*He works all 8 into the row he has just turned and then stands with the spade and looks at the plot for a while*{/}"));
					await dialog.Msg(L("That is a second crop where there was one. 412 people eat off these plots and every one of them will notice by the middle of next month."));
					await dialog.Msg(L("Take the chapel's box. It is alms, given by people who have nothing, for exactly this - somebody from outside doing something for Sventimas without being sentenced to it."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plots are quiet. 8 whole sacs and I can dress the ground before the frost."));
				}
				else
				{
					await dialog.Msg(L("30 of them. Stand in one of the hedge gaps at dusk and they will come to you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Second crop is up. Arntas has started saying it is a miracle and I have started correcting him in front of people, which he enjoys enormously."));
			}
		});

		// =====================================================================
		// QUEST 1002: Herbs for a Village with No Physician
		// =====================================================================
		// Villager Argis - sikljien off the Brown Lapasapes
		//---------------------------------------------------------------------
		AddNpc(155035, L("[Villager] Argis"), "f_tableland_72", -46, -42, 199, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1002);

			dialog.SetTitle(L("Argis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A villager is packing something into a clay shell with a stick, very slowly and with his tongue between his teeth*{/}"));
				await dialog.Msg(L("Don't come closer than that, if it's all the same to you. Not that I don't want the company — I just wouldn't want to be the reason it ends."));
				await dialog.Msg(L("I was sentenced up here for making things that go off. I still make them, because it turns out an exile zone has a great deal of use for a man who can move rock. What we have not got is a physician, or any herb worth the name. Bring me 6 sikljien off the Brown Lapasapes."));

				var response = await dialog.Select(L("Will you get me the sikljien?"),
					Option(L("I'll bring 6"), "help"),
					Option(L("You still make bombs?"), "info"),
					Option(L("Grow your own herbs"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663135, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take one of mine. Not to fight with - to get out. If a Lapasape group closes on you, put it behind them and go, and do not be proud about it."));
						await dialog.Msg(L("Sikljien grows in the wet under them, north in the wood. Pull the whole root or it is just a leaf."));
						break;

					case "info":
						await dialog.Msg(L("I make them for a well shaft and a road cut and once for a rockfall over the north path. Same hands, same trade, and up here nobody has ever asked me to stop."));
						await dialog.Msg(L("Kaleims knew what I was inside a week and he has never mentioned it once. That is the only sermon he has ever preached at me and it worked."));
						break;

					case "leave":
						await dialog.Msg(L("On this? The plots barely make turnips. Sikljien wants wet shade and there is none inside the hedge."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectHerbs", out var herbObj)) return;

				if (herbObj.Done)
				{
					await dialog.Msg(L("{#666666}*He splits a root with a thumbnail, smells it, and looks genuinely relieved*{/}"));
					await dialog.Msg(L("Whole roots, all 6. That is fever draught for the winter for 412 people, made by a bomb-maker in a shed, which is the most Sventimas sentence I have ever said."));
					await dialog.Msg(L("Take this. I dug it out of the north cut and it is not mine and it is not anybody's, and I would rather it went down the road with somebody than sat in my shed."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("North, in the wet wood, under the Brown Lapasapes. 6 whole roots."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Draught's made and stoppered and Kaleims has it in the chapel where nobody has to ask me for it. That was his idea and it was a good one."));
			}
		});

		// =====================================================================
		// QUEST 1003: The Magicians in the North Cut
		// =====================================================================
		// Villager D'Ailan - the road out of the village
		//---------------------------------------------------------------------
		AddNpc(155038, L("[Villager] D'Ailan"), "f_tableland_72", -73, 24, 173, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1003);

			dialog.SetTitle(L("D'Ailan"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A villager is stacking cut wood against a wall in courses, squaring every layer before starting the next*{/}"));
				await dialog.Msg(L("You're not from the village. Fine, doesn't matter — I've got no patience left for pleasantries anyway, so I'll just say it."));
				await dialog.Msg(L("There is 1 path off this shelf that a person can walk carrying something, and it is the north cut, and the Blue Cronewt Magicians have been in it since midsummer. Kill 20 of them. That is the whole of what I want and I am not going to dress it up."));

				var response = await dialog.Select(L("Will you clear the north cut?"),
					Option(L("I'll clear the cut"), "help"),
					Option(L("Where does the cut go?"), "info"),
					Option(L("You're exiles. Stay put"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They fight from the walls of the cut, not the floor. Do not look for them at your own height."));
						await dialog.Msg(L("And they will not follow you out of it. Whatever holds them in that cut holds them tighter than any of them wants."));
						break;

					case "info":
						await dialog.Msg(L("Kadumel, and then the fortress outpost, and then anywhere. Not that any of us are going. A sentence is a sentence and I am 11 years into 20."));
						await dialog.Msg(L("It is about the carrying. If the north cut is shut we cannot trade a single thing off this shelf, and a village that cannot trade is a village that is only ever going to be what the Kingdom left here."));
						break;

					case "leave":
						await dialog.Msg(L("I have stayed put 11 years. What I have not done is stop being a man who can move 40 pounds of worked timber to somewhere that will pay for it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMagicians", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks up into the cut with a load already on his shoulder before you have finished telling him*{/}"));
					await dialog.Msg(L("Open. 11 years and this is the first autumn Sventimas is going to sell something instead of eating it."));
					await dialog.Msg(L("Take the carry-money. It is the first coin this village has earned and I would like the first of it to leave the shelf in somebody's pocket who chose to come here."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("North cut. They fight from the walls. 20 of them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("4 loads down to Kadumel and 4 loads of coin back. Kaleims wanted it in the chapel box and the village voted him down, which he has told me 3 times was the best thing that has happened here."));
			}
		});

		// =====================================================================
		// QUEST 1004: Four Hundred and Twelve
		// =====================================================================
		// Villager Emils - the count that does not work
		//---------------------------------------------------------------------
		AddNpc(152065, L("[Villager] Emils"), "f_tableland_72", -98, 113, 208, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1004);

			dialog.SetTitle(L("Emils"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A villager has a wall of the hut covered in scratched fives, in blocks, going back years*{/}"));
				await dialog.Msg(L("Someone new. Good — I need a person who hasn't already decided the answer, and everyone in this village decided it years ago."));
				await dialog.Msg(L("31 convoys have been received at Sventimas. 40 to a convoy. That is 1,240 people and this village has 412, and I have counted the 412 myself, twice, by name. Nobody has ever asked where the rest are. Go and read the 4 orbs out on the east ground for me."));

				var response = await dialog.Select(L("Will you read the 4 orbs?"),
					Option(L("I'll read all 4"), "help"),
					Option(L("Could people just have died?"), "info"),
					Option(L("Your count is wrong"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663132, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the detection orb. Hold it up to each of theirs and it will show you which way theirs is turned. That is all I want - the direction, 4 times."));
						await dialog.Msg(L("They are a long walk east and there are 6 of them out there. Read any 4. If 4 agree I do not need the other 2."));
						break;

					case "info":
						await dialog.Msg(L("828 of them? We bury our dead in a field I can see from here and I have counted the field as well. There are 61 in it."));
						await dialog.Msg(L("I have been doing this 4 years. Every answer anybody gives me falls apart in an afternoon and this one falls apart faster than most."));
						break;

					case "leave":
						await dialog.Msg(L("Then correct it. I will give you the list and you can walk the village and read it out and I will thank you sincerely, because I have wanted to be wrong about this since the second year."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var read = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_72.Quest1004.Read", 0);

				if (read >= 4)
				{
					await dialog.Msg(L("{#666666}*They scratch the 4 directions onto the wall under the blocks of fives and then stand back from the wall*{/}"));
					await dialog.Msg(L("All 4 turned the same way. Not at the village. Not at the north cut. At the road - the stretch of the Ibre road where the convoys come up."));
					await dialog.Msg(L("Somebody put 6 orbs on this shelf pointed at the exact piece of ground where 828 people stopped existing. Take this. Then go to Arntas, because he has been asking his figurine the same question for 3 years."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("6 orbs on the east ground. Hold mine to theirs and read the direction. {0} of 4 read.", read));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have written 412 and 1,240 at the top of the wall with the direction under them, and I have stopped scratching fives. There is no more counting to do. There is only somebody who will read it."));
			}
		});

		// =====================================================================
		// WATCHING ORBS
		// =====================================================================
		// For Quest 1004 - Four Hundred and Twelve
		// =====================================================================

		void AddWatchingOrb(int orbNumber, string orbName, string reading, int x, int z, int direction)
		{
			AddNpc(151022, L(orbName), "f_tableland_72", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_72", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*An orb on a worked stone base, turned deliberately, not settled*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_72.Quest1004.Orb{orbNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have this one's direction already*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the orb..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Reading interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);

				var read = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_72.Quest1004.Read", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_tableland_72.Quest1004.Read", read);

				character.ServerMessage(L(reading));
				character.ServerMessage(LF("Orbs read: {0}/4", read));

				if (read >= 4)
					character.ServerMessage(L("{#FFD700}All 4 orbs read. Return to Emils.{/}"));
			});
		}

		AddWatchingOrb(1, "North Watching Orb", "Turned northwest. The base has been reset at least once - there are two seatings.", 1057, 818, 0);
		AddWatchingOrb(2, "Middle Watching Orb", "Northwest. Same angle to within a hand's width.", 984, 554, 0);
		AddWatchingOrb(3, "South Watching Orb", "Northwest again, from 400 paces further south, so it is not pointed at a place near itself.", 1464, 435, 0);
		AddWatchingOrb(4, "Ridge Watching Orb", "Northwest. Four orbs, one heading, and the heading crosses the Ibre road.", 1440, 796, 0);

		// =====================================================================
		// The Watch-Orb - atmosphere at the village edge
		//---------------------------------------------------------------------
		AddNpc(153157, L("Village Watch-Orb"), "f_tableland_72", -422, -62, 298, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A squat device on a post at the edge of the village, which everybody here calls the monitor and assumes is watching them*{/}"));
			await dialog.Msg(L("{#666666}*It faces outward. It has always faced outward. In 6 years nobody in Sventimas has walked round the back of it to check what outward means*{/}"));
		});

		// =====================================================================
		// QUEST 1005: What the Figurine Faces
		// =====================================================================
		// Priest Arntas - three years of asking the same question
		//---------------------------------------------------------------------
		AddNpc(155046, L("[Priest] Arntas"), "f_tableland_72", -394, 83, 248, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1005);

			dialog.SetTitle(L("Arntas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_tableland_72", 1004)))
				{
					await dialog.Msg(L("{#666666}*He looks up from a small worn figurine cupped in both hands*{/}"));
					await dialog.Msg(L("Forgive me, I was praying — or trying to. Emils has 6 orbs on the east ground and no directions. Go and get them for him. I have asked my question for 3 years and it will keep another day."));
					return;
				}

				await dialog.Msg(L("{#666666}*He sets the figurine down very carefully, like it might be listening*{/}"));
				await dialog.Msg(L("Northwest. Across the Ibre road. Then the figurine and the orbs and whatever is under Mandara are all one thing, and it is a very long instrument, and Sventimas is somewhere in the middle of it."));
				await dialog.Msg(L("The figurine stands out in the south scrub and I have prayed at it 3 years without once asking the obvious question, which is what it is looking at. The Red Hohen Orbens hold that ground. Kill 25 and take the 2 standing over it."));

				var response = await dialog.Select(L("Will you go to the figurine?"),
					Option(L("I'll take the ground and the pair"), "help"),
					Option(L("Why did nobody ask?"), "info"),
					Option(L("Break the figurine"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663136, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the purification sphere. It will not cleanse anything - I have stopped pretending it will. What it does is keep your own head your own for about 4 minutes, and 4 minutes is enough to look at something and walk back."));
						await dialog.Msg(L("Orbens are lv92 and they are not from this shelf and they do not behave like anything else on it. Do not fight more than 2 at once and do not fight any of them uphill."));
						break;

					case "info":
						await dialog.Msg(L("Because it is called the cursed statue, and once a thing has a name people stop looking at it. I have said the word cursed at that figurine 3 years running and it saved me from having to say anything more useful."));
						await dialog.Msg(L("Kaleims asked once, in the second year, what it faced. I told him it faced away from the village and that was a mercy. That was the last time either of us mentioned it."));
						break;

					case "leave":
						await dialog.Msg(L("If it is one end of an instrument, breaking it tells whoever built it that somebody in Sventimas has worked it out. There are 412 people here and no walls."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearScrub", out var scrubObj)) return;
				if (!quest.TryGetProgress("takeThePair", out var pairObj)) return;

				if (scrubObj.Done && pairObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks out to the figurine himself with the sphere in one hand and comes back with his other hand shaking, and does not hide it*{/}"));
					await dialog.Msg(L("Northwest. The same heading as the orbs, the same heading as whatever is under Mandara. It faces the Ibre road and it has faced it since before this village existed."));
					await dialog.Msg(L("Take the chapel's own. It came up the road with me 3 years ago and I have not needed it once. Go on to the Kalejimas road - the Steel Heights above the prison. If this line goes anywhere, it goes there, and there is nobody on that ground who is not paid to be."));

					character.Quests.Complete(questId);
				}
				else if (scrubObj.Done)
				{
					await dialog.Msg(L("Scrub's clear. The 2 over the figurine have not moved. They are standing where anybody praying at it would have to stand."));
				}
				else
				{
					await dialog.Msg(L("25 Orbens first, and never more than 2 at once. I am not going to bury somebody for my 3 years of not asking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have moved the chapel service out to the figurine. 412 people standing behind a thing that faces northwest, once a week, looking the way it looks. Kaleims says that is not a service and I have told him it is the first honest one I have held."));
			}
		});

		// =====================================================================
		// The Cursed Figurine - atmosphere in the south scrub
		//---------------------------------------------------------------------
		AddNpc(153155, L("Cursed Figurine"), "f_tableland_72", -576, -1223, 334, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A worked figure standing alone in the south scrub, older than the village and set into ground that was levelled for it*{/}"));
			await dialog.Msg(L("{#666666}*It faces northwest, across the shelf, over the road the convoys come up. It has never been turned and there is no mechanism by which it could be*{/}"));
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: A Village Nobody Provisioned
//-----------------------------------------------------------------------------

public class AVillageNobodyProvisionedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1001);
		SetName(L("A Village Nobody Provisioned"));
		SetType(QuestType.Sub);
		SetDescription(L("Sventimas receives salt twice a year and nothing else. The plots feed 412 people and the White Spions are eating them. Kaleims needs the plots cleared and 8 whole essence sacs to dress the ground."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Priest] Kaleims"), "f_tableland_72");

		AddObjective("killSpions", L("Kill White Spions in the village plots"),
			new KillObjective(30, new[] { MonsterId.Spion_White }));

		AddObjective("collectEssence", L("Recover whole White Spion essence"),
			new CollectItemObjective(663131, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(663131, 0.35f, MonsterId.Spion_White);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663131, character.Inventory.CountItem(663131), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663131, character.Inventory.CountItem(663131), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Herbs for a Village with No Physician
//-----------------------------------------------------------------------------

public class HerbsForAVillageWithNoPhysicianQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1002);
		SetName(L("Herbs for a Village with No Physician"));
		SetType(QuestType.Sub);
		SetDescription(L("Sventimas has no physician and no herb worth the name. Argis can make fever draught for the whole village out of 6 whole sikljien roots, which grow in the wet under the Brown Lapasapes."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Villager] Argis"), "f_tableland_72");

		AddObjective("collectHerbs", L("Pull whole sikljien roots in the north wood"),
			new CollectItemObjective(663134, 6));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(663134, 0.35f, MonsterId.Lapasape_Brown);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663134, character.Inventory.CountItem(663134), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(663135, character.Inventory.CountItem(663135), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663134, character.Inventory.CountItem(663134), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(663135, character.Inventory.CountItem(663135), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Magicians in the North Cut
//-----------------------------------------------------------------------------

public class TheMagiciansInTheNorthCutQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1003);
		SetName(L("The Magicians in the North Cut"));
		SetType(QuestType.Sub);
		SetDescription(L("The north cut is the only path off the shelf a person can walk carrying something, and the Blue Cronewt Magicians have held it since midsummer. Without it Sventimas cannot trade a single thing."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Villager] D'Ailan"), "f_tableland_72");

		AddObjective("killMagicians", L("Kill Blue Cronewt Magicians in the north cut"),
			new KillObjective(20, new[] { MonsterId.Cronewt_Mage_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Four Hundred and Twelve
//-----------------------------------------------------------------------------

public class FourHundredAndTwelveQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1004);
		SetName(L("Four Hundred and Twelve"));
		SetType(QuestType.Sub);
		SetDescription(L("Thirty-one convoys of 40 have been received at Sventimas and Emils has counted 412 people by name, twice. Six orbs stand on the east ground. Read the direction of 4 of them."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Villager] Emils"), "f_tableland_72");

		AddObjective("readOrbs", L("Read the direction of 4 watching orbs"),
			new VariableCheckObjective("Laima.Quests.f_tableland_72.Quest1004.Read", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663132, character.Inventory.CountItem(663132), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_72.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_72.Quest1004.Orb{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663132, character.Inventory.CountItem(663132), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_72.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_72.Quest1004.Orb{i}");
	}
}

// Quest 1005 CLASS: What the Figurine Faces
//-----------------------------------------------------------------------------

public class WhatTheFigurineFacesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1005);
		SetName(L("What the Figurine Faces"));
		SetType(QuestType.Sub);
		SetDescription(L("Arntas has prayed at the cursed figurine for 3 years without asking what it is looking at. The Red Hohen Orbens hold the south scrub around it, and 2 of them stand exactly where a person praying would have to stand."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Priest] Arntas"), "f_tableland_72");

		AddPrerequisite(new CompletedPrerequisite("f_tableland_72", 1004));

		AddObjective("clearScrub", L("Kill Red Hohen Orbens in the south scrub"),
			new KillObjective(25, new[] { MonsterId.Hohen_Orben_Red }));

		AddObjective("takeThePair", L("Take the pair standing over the figurine"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Hohen_Orben_Red, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Lapasape_Brown, 3),
				},
				resetIdent: "clearScrub",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(603128, 1)); // Schaffen Bracelet Fragment
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663136, character.Inventory.CountItem(663136), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663136, character.Inventory.CountItem(663136), InventoryItemRemoveMsg.Destroyed);
	}
}
