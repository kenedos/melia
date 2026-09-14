//--- Melia Script ----------------------------------------------------------
// Narvas Approach - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_whitetrees_22_3 map. Five detection
// installations, and four of them are detectors.
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

public class FWhitetrees223QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Nothing Was Supposed to Come This Far
		// =====================================================================
		// Abbey Guard Tautvydas - the Black Hohens on the approach
		//---------------------------------------------------------------------
		AddNpc(20128, L("[Abbey Guard] Tautvydas"), "f_whitetrees_22_3", -606, -1052, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1001);

			dialog.SetTitle(L("Tautvydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A guard has a spear grounded and is watching a treeline he clearly did not expect to have to watch*{/}"));
				await dialog.Msg(L("Stay behind me a moment. ...Right, nothing yet. Sorry — you'll have to forgive the manners, this treeline's had me jumpy for a season."));
				await dialog.Msg(L("I have stood the Narvas approach 8 years. In 8 years the worst thing on this ground was a Yakmap eating somebody's lunch. This season we have Black Hohen Manes and nobody can tell me where they came from. Kill 30 and bring me 8 of their essence."));

				var response = await dialog.Select(L("Will you take the treeline?"),
					Option(L("I'll clear them and get the essence"), "help"),
					Option(L("Nobody knows where they came from?"), "info"),
					Option(L("Shut the approach"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They work the treeline and they will not come onto open grass, which is the one advantage this ground gives anybody. Fight them out of the trees."));
						await dialog.Msg(L("The essence sits behind the plate at the shoulder. Cut it out before it cools or it is not worth carrying up to the Abbey."));
						break;

					case "info":
						await dialog.Msg(L("Not one person. Brother Aistis has walked this ground every week for 11 years and he says they were not here in the spring and they were here by midsummer, and there is no road onto this shelf they could have used."));
						await dialog.Msg(L("Which means they walked. From wherever the Hohens actually live, all the way here, past everything in between."));
						break;

					case "leave":
						await dialog.Msg(L("Shut it and Narvas has no road. There are 60 people in that abbey and every loaf they eat comes over this ground."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killManes", out var killObj)) return;
				if (!quest.TryGetProgress("collectEssence", out var essObj)) return;

				if (killObj.Done && essObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lays the 8 essences out on his cloak and counts them twice, which is more attention than he has given anything all month*{/}"));
					await dialog.Msg(L("8 of them, and Aistis can send that up to Narvas and somebody in the library can go and find out what a Hohen is and why one is standing on our road."));
					await dialog.Msg(L("Take the guard-post's purse. We are paid out of the Abbey's alms and we have not had a thing to spend it on in 8 years."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Treeline's thinner. 8 essences, behind the shoulder plate, cut warm."));
				}
				else
				{
					await dialog.Msg(L("30 Manes and fight them out of the trees. They will not follow you onto grass."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Approach is walkable again. The bread cart came over this morning without an escort for the first time since midsummer and the carter did not even know there had been a problem."));
			}
		});

		// =====================================================================
		// QUEST 1002: Globes for the Fifth Station
		// =====================================================================
		// Chandler Ausma - the Abbey's supply of stored light
		//---------------------------------------------------------------------
		AddNpc(20116, L("[Chandler] Ausma"), "f_whitetrees_22_3", -1341, -1138, 285, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1002);

			dialog.SetTitle(L("Ausma"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A chandler is turning a spent globe over in her hands, looking at how it went out rather than that it did*{/}"));
				await dialog.Msg(L("Oh — don't mind me muttering. A new face is a nice change from talking to dead glass. Give me a moment and I'll tell you what's wrong with it."));
				await dialog.Msg(L("I keep Narvas in light. 40 lamps in the abbey and 5 out here on Aistis's stations, and the station globes drain 4 times as fast as the abbey ones. The Black Hohen Gulaks carry a globe that stores charge. Bring me 6."));

				var response = await dialog.Select(L("Will you bring me 6 globes?"),
					Option(L("I'll bring 6"), "help"),
					Option(L("Why do the stations drain faster?"), "info"),
					Option(L("Use ordinary lamps"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them charged. A Gulak that has been dead an hour has a flat globe and a flat globe is glass."));
						await dialog.Msg(L("They come at you straight and slowly and that is not a kindness, it is because they do not need to hurry."));
						break;

					case "info":
						await dialog.Msg(L("That is a very good question and I have asked it for 6 years and been told it is the weather. Same globe, same charge, same maker. In the abbey it lasts a season. On a station it lasts 6 weeks."));
						await dialog.Msg(L("Something out there is using them. I am a chandler, so what I know about it is that it costs me 34 globes a year."));
						break;

					case "leave":
						await dialog.Msg(L("The stations will not take a flame. There is no wick housing on any of the 5 - they were built for globes, by somebody who assumed whoever kept them would have globes."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectGlobes", out var globeObj)) return;

				if (globeObj.Done)
				{
					await dialog.Msg(L("{#666666}*She seats one in a test cradle and it comes up bright and stays bright, and she watches it for a full minute*{/}"));
					await dialog.Msg(L("Charged and holding. 6 globes is a year of stations, and I have not had a year of stations since I took the post."));
					await dialog.Msg(L("Take the chandlery's money. And go and stand at the fifth station some evening and tell me whether the globe in it looks like the globe in the other 4, because I have never been able to decide."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("6 globes, off the Gulaks, and take them charged. A flat one is glass."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("All 5 stations globed and 2 spare on the shelf. And the fifth one has drained a third already in 9 days, and the other 4 have not moved. I have written that in the chandlery book where somebody will have to read it."));
			}
		});

		// =====================================================================
		// QUEST 1003: The Mambo on the Ledge
		// =====================================================================
		// Yardsman Gedas - the east ledge above the abbey road
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Yardsman] Gedas"), "f_whitetrees_22_3", 291, -1149, 285, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1003);

			dialog.SetTitle(L("Gedas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A yardsman is stacking barrels and keeps stopping to look up at a ledge above the road*{/}"));
				await dialog.Msg(L("Mind the barrels — good, you're quick on your feet, that'll help. I could use someone who isn't already sick of hearing me complain about that ledge."));
				await dialog.Msg(L("I run the yard where the abbey road turns. Everything Narvas eats gets unloaded here and loaded again, and there are Yak Mambos on the ledge over the turn that come down onto it. Kill 25 of them."));

				var response = await dialog.Select(L("Will you clear the ledge?"),
					Option(L("I'll clear the Mambos"), "help"),
					Option(L("Aren't Mambos usually higher up?"), "info"),
					Option(L("Move the yard"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Do not fight them on the ledge. Fight them at the bottom of it where they have to come to you - a Mambo that is above you is a completely different animal to one that is not."));
						await dialog.Msg(L("They come down at dusk. So go at noon and take them one at a time out of the shade."));
						break;

					case "info":
						await dialog.Msg(L("They are. That is exactly it. Mambos hold the high ground and the ledge is not high ground, it is a shelf above a cart yard."));
						await dialog.Msg(L("Something moved them off the tops. I have run this yard 12 years and I have never once had to think about what is above the ledge, and now I do."));
						break;

					case "leave":
						await dialog.Msg(L("The road turns here because the ground turns here. There is no other yard. That is why there is one."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMambos", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He climbs the ledge himself, walks the length of it, and comes back down looking more thoughtful than relieved*{/}"));
					await dialog.Msg(L("Clear. And there is nothing up there. No den, no bones, no reason for a Mambo to be on it at all. They were not living there. They were pushed onto it."));
					await dialog.Msg(L("Take the yard's cut. And tell Aistis about the ledge - he keeps a list of things that have moved and I think this belongs on it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Fight them at the bottom of the ledge, at noon, one at a time. 25 of them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Yard's been clear 11 days and I have got 3 weeks of backlog off the turn. And every evening I look at that ledge, which I never did before, and I expect I always will now."));
			}
		});

		// =====================================================================
		// QUEST 1004: Five Stations
		// =====================================================================
		// Installer Bronius - reading the array he did not build
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Installer] Bronius"), "f_whitetrees_22_3", 453, 403, 236, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1004);

			dialog.SetTitle(L("Bronius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An installer has a station panel open and is looking into it the way you look into something you have opened many times and never understood*{/}"));
				await dialog.Msg(L("Ah, company. Come look at this with me — a second pair of eyes that hasn't gone numb to it might actually see something."));
				await dialog.Msg(L("Narvas Abbey keeps 5 auxiliary detection stations on this ground. I service them. I have serviced them 9 years and I could not tell you what any of them detects. Go and read the plate inside 4 of them and tell me what is written."));

				var response = await dialog.Select(L("Will you read the 4 plates?"),
					Option(L("I'll read all 4"), "help"),
					Option(L("Nine years and no idea?"), "info"),
					Option(L("Ask the Abbey"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The plate is behind the globe cradle, face down, and it is stamped not written, so it will not have faded. Lift the cradle, do not unseat it."));
						await dialog.Msg(L("Read the whole plate. Everybody reads the top line and stops, and the top line is the same on all 5."));
						break;

					case "info":
						await dialog.Msg(L("I know what to clean, what to replace, and how often. That is what an installer is given. Nobody has ever handed me a purpose along with a maintenance schedule."));
						await dialog.Msg(L("I have asked. Twice. Both times a very kind librarian looked in the register and told me the commissioning entry is there and the purpose column is blank."));
						break;

					case "leave":
						await dialog.Msg(L("The Abbey's answer is that they were installed before the current chapter and the current chapter has 60 people and a leaking roof. It is not a conspiracy. It is 200 years of nobody having time."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var read = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_22_3.Quest1004.Read", 0);

				if (read >= 4)
				{
					await dialog.Msg(L("{#666666}*He lays the 4 readings out on the open panel and goes very still over the fourth one*{/}"));
					await dialog.Msg(L("Three of them say RECEIVE. The fourth says RECEIVE as well. And I have serviced the fifth station 9 years and I can tell you from memory that its plate does not say RECEIVE."));
					await dialog.Msg(L("It is not a detection array with 5 stations. It is 4 detectors and 1 of something else, and the one that drains 4 times as fast is the one that is not a detector. Take this and go and find Aistis."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Behind the globe cradle, face down, stamped. Read the whole plate. {0} of 4 read.", read));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have written a proper schedule for the first time in 9 years, with a column for what each station is for. 4 of the 5 columns say RECEIVE and the fifth one I have left blank, because I would rather it was visibly blank than quietly filled in."));
			}
		});

		// =====================================================================
		// DETECTION STATIONS
		// =====================================================================
		// For Quest 1004 - Five Stations
		// =====================================================================

		void AddDetectionStation(int stationNumber, string stationName, string plate, int x, int z, int direction)
		{
			AddNpc(153015, L(stationName), "f_whitetrees_22_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_22_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*An auxiliary detection station on a low plinth, with a globe cradle at the top and a serviced, oiled panel*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_22_3.Quest1004.Station{stationNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*The cradle is back down on this one and the plate is under it again*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Lifting the cradle..."), "Cancel", "SITREAD", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Reading interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);

				var read = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_22_3.Quest1004.Read", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_whitetrees_22_3.Quest1004.Read", read);

				character.ServerMessage(L(plate));
				character.ServerMessage(LF("Station plates read: {0}/4", read));

				if (read >= 4)
					character.ServerMessage(L("{#FFD700}All 4 plates read. Return to Installer Bronius.{/}"));
			});
		}

		AddDetectionStation(1, "First Detection Station", "Top line: NARVAS AUXILIARY. Under it, stamped: RECEIVE.", 362, 304, 0);
		AddDetectionStation(2, "Second Detection Station", "Same top line. RECEIVE. Globe barely drained.", 473, 465, 0);
		AddDetectionStation(3, "Third Detection Station", "RECEIVE. The plate is bedded in older metal than the plinth.", 465, 261, 0);
		AddDetectionStation(4, "Fourth Detection Station", "RECEIVE, like the other three. Four of five, and all four only listen.", 541, 373, 0);

		// =====================================================================
		// QUEST 1005: The One That Answers
		// =====================================================================
		// Friar Aistis - eleven years of walking an array
		//---------------------------------------------------------------------
		AddNpc(155045, L("[Friar] Aistis"), "f_whitetrees_22_3", -128, 1203, 264, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_22_3", 1005);

			dialog.SetTitle(L("Aistis"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_whitetrees_22_3", 1004)))
				{
					await dialog.Msg(L("{#666666}*A friar looks up from a small logbook, pencil still against the page*{/}"));
					await dialog.Msg(L("Peace be with you — but I'm afraid I've nothing yet worth saying. Bronius has 4 plates he has never lifted a cradle to read. Go and read them with him. I have walked this array 11 years and I would rather hear it from the metal than from myself."));
					return;
				}

				await dialog.Msg(L("{#666666}*He closes the logbook without marking his place, which he has apparently never done before*{/}"));
				await dialog.Msg(L("Four say RECEIVE. I have walked all 5 of these stations every week for 11 years and reported nothing detected 570 times, and I was right every time, because 4 of them were listening for something that was never going to come from out here."));
				await dialog.Msg(L("The fifth does not listen. It answers, and it has been answering something for longer than this abbey has existed. The Yakmaps hold the ground round it - kill 30, and then take the 2 Gulaks that have been standing on the fifth plinth since midsummer."));

				var response = await dialog.Select(L("Will you go to the fifth station?"),
					Option(L("I'll take the fifth plinth"), "help"),
					Option(L("Answering what?"), "info"),
					Option(L("Switch it off"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663306, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take my note. It is 11 years of nothing detected, dated, with the 570th entry crossed out and rewritten, and if I do not come back off this ground it is the only version of this that exists."));
						await dialog.Msg(L("Yakmaps first, all round the plinth. The 2 Gulaks will not step off it and I would like to know why before you find out for me."));
						break;

					case "info":
						await dialog.Msg(L("I do not know. The register's commissioning entry names a builder and the builder's other work is listed as one line: further south, past Roxona."));
						await dialog.Msg(L("I have never been past Roxona. I have walked 4 miles of the same 5 stations for 11 years and written the same word 570 times, and I would very much like the 571st entry to say something else."));
						break;

					case "leave":
						await dialog.Msg(L("With what? It has no switch, no wick, no lever and no seam. It has a cradle for a globe and a plate that does not say RECEIVE, and everything else about it is one piece of worked stone."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearStationGround", out var groundObj)) return;
				if (!quest.TryGetProgress("takeThePlinth", out var plinthObj)) return;

				if (groundObj.Done && plinthObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks to the fifth station, lifts the cradle himself, and reads the plate out loud, and then reads it again quietly*{/}"));
					await dialog.Msg(L("SEND. One word, in the same stamp, on the same metal. Four listen and one sends, and Ausma has been feeding the one that sends 34 globes a year for 6 years without anybody once asking her why."));
					await dialog.Msg(L("Take this. It is the abbey's and I have asked for it and it has been given. My 571st entry is going to be a full page and I am going to carry it to Narvas myself, and after that I am going to walk further south than I have ever been."));

					character.Quests.Complete(questId);
				}
				else if (groundObj.Done)
				{
					await dialog.Msg(L("Ground's clear. The 2 on the plinth have not moved and they are facing outward, which I have been trying not to think about for a month."));
				}
				else
				{
					await dialog.Msg(L("30 Yakmaps first, all round it. I am not sending anybody onto that plinth with the ground still full."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("571 entries. The last one says SEND and it says what the plate on the fifth station says and it says who built it, and the abbey has read it and the abbey does not know either."));
				await dialog.Msg(L("Ausma has stopped globing the fifth station. 9 days and it is still lit, off nothing, which is the first thing this ground has ever told me without being asked."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Nothing Was Supposed to Come This Far
//-----------------------------------------------------------------------------

public class NothingWasSupposedToComeThisFarQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1001);
		SetName(L("Nothing Was Supposed to Come This Far"));
		SetType(QuestType.Sub);
		SetDescription(L("Black Hohen Manes arrived on the Narvas approach between spring and midsummer, and there is no road onto this shelf they could have used. Clear the treeline and bring 8 of their essence up to the abbey."));
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Abbey Guard] Tautvydas"), "f_whitetrees_22_3");

		AddObjective("killManes", L("Kill Black Hohen Manes on the treeline"),
			new KillObjective(30, new[] { MonsterId.Hohen_Mane_Black }));

		AddObjective("collectEssence", L("Cut essence from behind the shoulder plate"),
			new CollectItemObjective(663308, 8));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(663308, 0.35f, MonsterId.Hohen_Mane_Black);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663308, character.Inventory.CountItem(663308), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663308, character.Inventory.CountItem(663308), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Globes for the Fifth Station
//-----------------------------------------------------------------------------

public class GlobesForTheFifthStationQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1002);
		SetName(L("Globes for the Fifth Station"));
		SetType(QuestType.Sub);
		SetDescription(L("Narvas Abbey's 5 outlying stations drain 4 times faster than its 40 indoor lamps, and the stations will not take a flame. The Black Hohen Gulaks carry globes that store a charge."));
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Chandler] Ausma"), "f_whitetrees_22_3");

		AddObjective("collectGlobes", L("Take charged globes from Black Hohen Gulaks"),
			new CollectItemObjective(663309, 6));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion

		AddDrop(663309, 0.35f, MonsterId.Hohen_Gulak_Black);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663309, character.Inventory.CountItem(663309), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663309, character.Inventory.CountItem(663309), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Mambo on the Ledge
//-----------------------------------------------------------------------------

public class TheMamboOnTheLedgeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1003);
		SetName(L("The Mambo on the Ledge"));
		SetType(QuestType.Sub);
		SetDescription(L("Yak Mambos hold the high ground, and the ledge over Gedas's cart yard is not high ground. They have come down onto the abbey road turn and the yard cannot work under them."));
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Yardsman] Gedas"), "f_whitetrees_22_3");

		AddObjective("killMambos", L("Kill Yak Mambos at the foot of the ledge"),
			new KillObjective(25, new[] { MonsterId.Yakmambo }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Five Stations
//-----------------------------------------------------------------------------

public class FiveStationsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1004);
		SetName(L("Five Stations"));
		SetType(QuestType.Sub);
		SetDescription(L("Bronius has serviced the abbey's 5 detection stations for 9 years without being told what they detect, and the register's purpose column is blank. Lift the globe cradle in 4 of them and read the plate underneath."));
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Installer] Bronius"), "f_whitetrees_22_3");

		AddObjective("readPlates", L("Read the plate in 4 detection stations"),
			new VariableCheckObjective("Laima.Quests.f_whitetrees_22_3.Quest1004.Read", 4, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_22_3.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_22_3.Quest1004.Station{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_22_3.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_22_3.Quest1004.Station{i}");
	}
}

// Quest 1005 CLASS: The One That Answers
//-----------------------------------------------------------------------------

public class TheOneThatAnswersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_22_3", 1005);
		SetName(L("The One That Answers"));
		SetType(QuestType.Sub);
		SetDescription(L("Four of Narvas Abbey's stations are stamped RECEIVE. The fifth is not, it drains four times as fast, and two Black Hohen Gulaks have stood on its plinth facing outward since midsummer."));
		SetLocation("f_whitetrees_22_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Friar] Aistis"), "f_whitetrees_22_3");

		AddPrerequisite(new CompletedPrerequisite("f_whitetrees_22_3", 1004));

		AddObjective("clearStationGround", L("Kill Yakmaps around the fifth station"),
			new KillObjective(30, new[] { MonsterId.Yakmab }));

		AddObjective("takeThePlinth", L("Take the pair standing on the fifth plinth"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Hohen_Gulak_Black, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Hohen_Mane_Black, 3),
				},
				resetIdent: "clearStationGround",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(39000, 27000));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(583121, 1)); // Manosierdi Necklace
		AddReward(new ItemReward(640085, 3)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663306, character.Inventory.CountItem(663306), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663306, character.Inventory.CountItem(663306), InventoryItemRemoveMsg.Destroyed);
	}
}
