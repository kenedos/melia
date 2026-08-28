//--- Melia Script ----------------------------------------------------------
// Mishekan Forest Edge - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_whitetrees_23_3 map. Six royal seals were
// set on the white trees and five of them have gone dull.
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

public class FWhitetrees233QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: An Order He Cannot Read
		// =====================================================================
		// Markus - the officer posted to stop an explorer
		//---------------------------------------------------------------------
		AddNpc(20143, L("[Officer] Markus"), "f_whitetrees_23_3", -21, -1123, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1001);

			dialog.SetTitle(L("Markus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An officer is standing on the Pistis path with a sealed order in his belt that he keeps taking out and putting back*{/}"));
				await dialog.Msg(L("You're not with the explorer's party, are you? No — good, then maybe you can actually help instead of arguing with me."));
				await dialog.Msg(L("I was posted here to stop Indraja going any further into this forest. That is my whole order. It does not say why, it does not say what is in there, and it does not say what I am to do if she asks me. Kill 25 Cloverins and bring me 8 vials of the dew off them."));

				var response = await dialog.Select(L("Will you get me the dew?"),
					Option(L("I'll clear them and bring the dew"), "help"),
					Option(L("What do you need dew for?"), "info"),
					Option(L("Then stop her"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The dew is in the crown of the flower and it is gone by mid-morning. Work them early and work the wet ground by the path, not the dry."));
						await dialog.Msg(L("They are slow and there are a great many of them. If you feel like you are winning easily, count how many are behind you."));
						break;

					case "info":
						await dialog.Msg(L("Because there are stones in this forest with a royal seal on them and 5 of the 6 have gone dull, and syla dew is the only thing anybody knows of that brings a dull seal up enough to read."));
						await dialog.Msg(L("If I am guarding something I would like to know what it is. I have been a soldier 14 years and this is the first order I have ever been given that I could not repeat to the person it is about."));
						break;

					case "leave":
						await dialog.Msg(L("I did stop her. Twice. The third time she asked me what was written on the stones and I could not answer, and I have not stopped her since."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCloverin", out var killObj)) return;
				if (!quest.TryGetProgress("collectDew", out var dewObj)) return;

				if (killObj.Done && dewObj.Done)
				{
					await dialog.Msg(L("{#666666}*He tips one vial onto his own thumb and holds it against the sealed order in his belt, and the wax comes up bright*{/}"));
					await dialog.Msg(L("It works on wax. So it will work on a stone seal, and Indraja can read all 5 of the dull ones."));
					await dialog.Msg(L("Take the post's pay. I am going to sit here on this path all day being technically obedient while somebody else does the reading, and I am aware of exactly how that sounds."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Path's clearer. 8 vials off the crowns, early, before the sun gets at them."));
				}
				else
				{
					await dialog.Msg(L("25 Cloverins, wet ground by the path. And count what is behind you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Indraja has 8 vials and a copy of my order, which I gave her, which is a thing I will have to answer for eventually. I have decided I would rather answer for it than for the other thing."));
			}
		});

		// =====================================================================
		// QUEST 1002: Written Where Nobody Writes
		// =====================================================================
		// Copyist Danguole - scraps off the Rafflets
		//---------------------------------------------------------------------
		AddNpc(20116, L("[Copyist] Vaiva"), "f_whitetrees_23_3", 942, -382, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1002);

			dialog.SetTitle(L("Vaiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A copyist has a board across her knees and is redrawing a torn scrap letter by letter, at about one letter a minute*{/}"));
				await dialog.Msg(L("Give me just a moment, I'll lose the line if I look up too long — there. Sorry. You'll want to hear this, actually, if you're the sort who finds a mystery interesting."));
				await dialog.Msg(L("I copy things for a living and I have never had a job like this one. The Rafflets in the east thicket are carrying paper. Written paper, in a forest with nobody in it, and I have 3 scraps and 3 different hands. Bring me 6 more."));

				var response = await dialog.Select(L("Will you bring me 6 scraps?"),
					Option(L("I'll bring 6"), "help"),
					Option(L("Where would they get paper?"), "info"),
					Option(L("Paper rots. Leave it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them dry and take them flat. If you fold a scrap the ink comes off in the fold and I lose the middle of every line."));
						await dialog.Msg(L("They nest in the east thicket and they line the nests with it. Do not pull a nest apart - the paper is under the lining, not in it."));
						break;

					case "info":
						await dialog.Msg(L("That is the question and I have no answer to it. Rafflets do not go anywhere. They have lived in that thicket since before this forest was sealed."));
						await dialog.Msg(L("So either somebody has been in there writing, or the paper is older than the thicket. I am a copyist. I do not get to have opinions. I get to have 3 scraps and 3 hands."));
						break;

					case "leave":
						await dialog.Msg(L("It has not rotted. That is the other thing I would like explained, and I am hoping the sixth scrap does it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectScraps", out var scrapObj)) return;

				if (scrapObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays all 9 scraps out and moves them around each other for a long time before she gets a join*{/}"));
					await dialog.Msg(L("They are orders. Standing orders, in a military hand, and the top of one of them has a date on it that is 300 years before this forest was sealed."));
					await dialog.Msg(L("Take the copyist's fee. And when Indraja reads those stones, tell her somebody was garrisoning this forest before anybody thought to lock it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("East thicket, under the nest lining. 6 more, dry and flat and not folded."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("9 scraps, 3 hands, 1 date. I have made 4 fair copies and sent 3 of them out by 3 different roads, which Markus tells me is either very sensible or very frightening."));
			}
		});

		// =====================================================================
		// QUEST 1003: The Fragolin Ring
		// =====================================================================
		// Forester Kestas - the ring around the tree ground
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Forester] Rimtas"), "f_whitetrees_23_3", 771, 436, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1003);

			dialog.SetTitle(L("Rimtas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A forester is pacing a curve through the undergrowth and cutting a notch in a trunk every 30 paces*{/}"));
				await dialog.Msg(L("Watch your step, the undergrowth's thicker than it looks around here — there, good. You're the first person to walk up on me in a week who wasn't a Fragolin."));
				await dialog.Msg(L("I have cut 61 notches and they make a circle. The Fragolins sit inside it and they do not come out and nothing else goes in. Kill 25 of them so I can finish walking my own circle."));

				var response = await dialog.Select(L("Will you break the ring?"),
					Option(L("I'll kill 25 Fragolins"), "help"),
					Option(L("What's in the circle?"), "info"),
					Option(L("Notch around them"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They will not chase you past the notch line. I do not know why and I have tested it 20 times, which is the most useful thing I have found out here."));
						await dialog.Msg(L("So fight them at the edge. Step out, breathe, step in. It is not brave and it works."));
						break;

					case "info":
						await dialog.Msg(L("White trees. Six of them, in a ring, older than anything else standing in this forest, and every one has a stone at the root with a seal on it."));
						await dialog.Msg(L("I have foresteried this ground 30 years. I have never cut one of those 6 and neither did my father, and neither of us was ever told not to."));
						break;

					case "leave":
						await dialog.Msg(L("Then the circle in my book has a gap in it, and a circle with a gap is just a line, and a line does not tell you there is a middle."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFragolin", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks the last stretch of the curve and cuts the closing notch, then stands back to look at the whole shape*{/}"));
					await dialog.Msg(L("61 notches and it closes. It is a ring, it is 400 paces across, and there are exactly 6 white trees inside it."));
					await dialog.Msg(L("Take a forester's share. That is 30 years of walking past something and this is the week I finally drew it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Fight them at the notch line and step back out when you need to. 25 of them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ring's drawn and Indraja has copied it into her book beside her map, and the two of them are the same shape. Hers is 300 years old and mine is a fortnight old and they are the same shape."));
			}
		});

		// =====================================================================
		// QUEST 1004: Five Dull and One Bright
		// =====================================================================
		// Novice Sigute - reading the seals with the dew
		//---------------------------------------------------------------------
		AddNpc(147418, L("[Novice] Sigute"), "f_whitetrees_23_3", 175, 931, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1004);

			dialog.SetTitle(L("Sigute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A novice is sitting on a root with a borrowed map open, weighted at all four corners with stones*{/}"));
				await dialog.Msg(L("Oh — thank the goddess, an actual person. I've been talking to this map for an hour and it hasn't said a useful word back."));
				await dialog.Msg(L("Indraja lent me her map because I can read old script and she cannot, and then she went off to the middle and left me with it. There are 4 seal stones I can actually reach. Take the dew and read all 4 for me."));

				var response = await dialog.Select(L("Will you read the 4 seal stones?"),
					Option(L("I'll read all 4"), "help"),
					Option(L("What should they say?"), "info"),
					Option(L("Read them yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(666146, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the map. Wet the whole face of the stone, not just the seal - the script runs under the seal and that is the part nobody has ever bothered with."));
						await dialog.Msg(L("Read it out loud when it comes up. I have written down what you say at 3 stones already and every one of them was different from what I expected."));
						break;

					case "info":
						await dialog.Msg(L("A royal seal on a stone says one of two things. It says this is protected, or it says this is confined. They use the same seal for both and the difference is in the script underneath."));
						await dialog.Msg(L("I am 17 and I have read 40 seal stones in a seminary basement and not one of them prepared me for finding 6 of them in a wood."));
						break;

					case "leave":
						await dialog.Msg(L("I have a bad knee and the stones are 900 paces apart. I would love to. I have got as far as the near one and back and it took me an afternoon."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var read = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_3.Quest1004.Read", 0);

				if (read >= 4)
				{
					await dialog.Msg(L("{#666666}*She writes the fourth reading under the other three and then reads all four down the page twice*{/}"));
					await dialog.Msg(L("Confined. All 4. Not protected - confined, and each one names what is confined and the name is the same on every stone."));
					await dialog.Msg(L("They are not sealing the trees in. They are sealing something under the trees, and there are 6 stones and 5 have gone dull. Take this and go to Indraja. She is at the middle of the ring and she does not know yet."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Wet the whole face, not just the seal. Read it out loud. {0} of 4 stones done.", read));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have copied all 4 readings into 2 books and given 1 to Markus, because a soldier with a written thing in his hand is harder to give an unwritten order to."));
			}
		});

		// =====================================================================
		// SEAL STONES
		// =====================================================================
		// For Quest 1004 - Five Dull and One Bright
		// =====================================================================

		void AddSealStone(int stoneNumber, string stoneName, string reading, int modelId, int x, int z, int direction)
		{
			AddNpc(modelId, L(stoneName), "f_whitetrees_23_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_23_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A cut stone at the root of a white tree, with a royal seal on the face gone grey and flat*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_23_3.Quest1004.Stone{stoneNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*The dew has dried off this one. What you read is in Sigute's book*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Wetting the seal..."), "Cancel", "SITREAD", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Reading interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);
				character.Inventory.Add(666148, 1, InventoryAddType.PickUp);

				var read = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_3.Quest1004.Read", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_whitetrees_23_3.Quest1004.Read", read);

				character.ServerMessage(L(reading));
				character.ServerMessage(LF("Seal stones read: {0}/4", read));

				if (read >= 4)
					character.ServerMessage(L("{#FFD700}All 4 seal stones read. Return to Sigute.{/}"));
			});
		}

		AddSealStone(1, "First Seal Stone", "Seal dull. Under it: CONFINED, and a name, and a date in the old count.", 47253, 77, 532, 270);
		AddSealStone(2, "Second Seal Stone", "Dull. Same word. Same name. A different hand cut it.", 47103, -479, 395, 0);
		AddSealStone(3, "Third Seal Stone", "Dull, and cracked across the seal, and the crack is not weathering.", 47330, -706, 415, 0);
		AddSealStone(4, "Fourth Seal Stone", "Dull. CONFINED. The same name on all four, and none of them is the tree's.", 47103, 817, 1273, 0);

		// =====================================================================
		// QUEST 1005: The Sixth Is Still Bright
		// =====================================================================
		// Indraja - the middle of the ring
		//---------------------------------------------------------------------
		AddNpc(155145, L("[Explorer] Indraja"), "f_whitetrees_23_3", -441, 395, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1005);

			dialog.SetTitle(L("Indraja"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_whitetrees_23_3", 1004)))
				{
					await dialog.Msg(L("{#666666}*An explorer glances up from a half-filled notebook, then back down at the tree in front of her*{/}"));
					await dialog.Msg(L("You'll have to forgive me, I don't get many visitors out here. Sigute has my map and 4 stones she cannot walk to — go and read them with her first. I have been standing in the middle of this ring for 6 days and I can stand in it another afternoon."));
					return;
				}

				await dialog.Msg(L("{#666666}*She closes the notebook and finally turns to face you fully*{/}"));
				await dialog.Msg(L("Confined. Not protected. 6 stones, 5 dull, and the same name cut under every seal."));
				await dialog.Msg(L("The sixth stone is here, at the middle tree, and it is still bright - it is the only one of the 6 still doing whatever the 6 of them were set to do. The Rafflets have made the middle ground theirs. Kill 25 and take the pair sitting on the sixth stone."));

				var response = await dialog.Select(L("Will you clear the middle?"),
					Option(L("I'll take the pair off the stone"), "help"),
					Option(L("Whose name is on them?"), "info"),
					Option(L("Do not touch the last one"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(666147, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Carry the bright seal's rubbing. If the last stone goes while you are standing on it, I want the shape of it to exist somewhere that is not a stone."));
						await dialog.Msg(L("Clear the ground before you go near the middle tree. Whatever the pair on the stone are, they have not moved in 6 days and I have watched them not move."));
						break;

					case "info":
						await dialog.Msg(L("Not a person's. It is the name of a thing, in a script that had already stopped being written when these stones were cut, and I have found it in 2 places before - both of them under a tree, both of them sealed."));
						await dialog.Msg(L("There is a forest west of here where somebody lifted an old seed out of a flower bed a season ago. I would like very much to know what they did with it."));
						break;

					case "leave":
						await dialog.Msg(L("I am not going to touch it. I am going to stand in front of it and write down what it says while it still says anything, because in 20 years it will be as grey as the other 5 and then nobody will ever know what these 6 were for."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearMiddle", out var midObj)) return;
				if (!quest.TryGetProgress("takeThePair", out var pairObj)) return;

				if (midObj.Done && pairObj.Done)
				{
					await dialog.Msg(L("{#666666}*She goes to the sixth stone, takes a rubbing of the whole face, and then sits down on the root and looks at it for a long time*{/}"));
					await dialog.Msg(L("The sixth one does not say CONFINED. It says the same name and then it says RETURNED, and it is the only one of the 6 that has ever been recut."));
					await dialog.Msg(L("Somebody came back here and changed a seal, and after they did it 5 of the other stones stopped working. Take this. It is the best thing in my pack and it has been in it 9 years - and if you ever get to the Parias forest, tell the Kupole sisters what is cut on the sixth stone."));

					character.Quests.Complete(questId);
				}
				else if (midObj.Done)
				{
					await dialog.Msg(L("Middle ground's clear. The pair are still on the stone and they are still not moving."));
				}
				else
				{
					await dialog.Msg(L("25 first, all the way round the middle. I am not having you fight on that stone with the ring still full."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("6 rubbings, 4 fair copies, 3 roads. Vaiva did the copying and would not take payment for it, which after 9 years of buying help I did not know what to do with."));
				await dialog.Msg(L("The sixth seal is still bright. It has been bright since before there was a Kingdom to put a seal on anything, and one day it will not be, and somebody should be standing here when that happens."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: An Order He Cannot Read
//-----------------------------------------------------------------------------

public class AnOrderHeCannotReadQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1001);
		SetName(L("An Order He Cannot Read"));
		SetType(QuestType.Sub);
		SetDescription(L("Markus was posted to the Pistis path to stop an explorer, with no reason given. Syla dew brings a dull seal back up enough to read, and the Cloverins carry it in the crown of the flower."));
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Officer] Markus"), "f_whitetrees_23_3");

		AddObjective("killCloverin", L("Kill Cloverins on the wet ground by the path"),
			new KillObjective(25, new[] { MonsterId.Cloverin }));

		AddObjective("collectDew", L("Collect syla dandelion dew"),
			new CollectItemObjective(666143, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(666143, 0.35f, MonsterId.Cloverin);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666143, character.Inventory.CountItem(666143), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666143, character.Inventory.CountItem(666143), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Written Where Nobody Writes
//-----------------------------------------------------------------------------

public class WrittenWhereNobodyWritesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1002);
		SetName(L("Written Where Nobody Writes"));
		SetType(QuestType.Sub);
		SetDescription(L("The Rafflets in the east thicket line their nests with written paper, in a forest that has had nobody in it. Vaiva has 3 scraps in 3 different hands and wants 6 more."));
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Copyist] Vaiva"), "f_whitetrees_23_3");

		AddObjective("collectScraps", L("Recover written scraps from Rafflet nests"),
			new CollectItemObjective(666144, 6));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion

		AddDrop(666144, 0.30f, MonsterId.Rafflet);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666144, character.Inventory.CountItem(666144), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666144, character.Inventory.CountItem(666144), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Fragolin Ring
//-----------------------------------------------------------------------------

public class TheFragolinRingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1003);
		SetName(L("The Fragolin Ring"));
		SetType(QuestType.Sub);
		SetDescription(L("Rimtas has cut 61 notches around the tree ground and they make a circle. The Fragolins sit inside it, will not come out, and let nothing in."));
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forester] Rimtas"), "f_whitetrees_23_3");

		AddObjective("killFragolin", L("Kill Fragolins inside the notch line"),
			new KillObjective(25, new[] { MonsterId.Fragolin }));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1004 CLASS: Five Dull and One Bright
//-----------------------------------------------------------------------------

public class FiveDullAndOneBrightQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1004);
		SetName(L("Five Dull and One Bright"));
		SetType(QuestType.Sub);
		SetDescription(L("Six seal stones stand at the roots of six white trees and five of them have gone grey. Sigute can read the old script but cannot walk to the stones. Wet each face and read what runs under the seal."));
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Novice] Sigute"), "f_whitetrees_23_3");

		AddObjective("readStones", L("Read the 4 reachable seal stones"),
			new VariableCheckObjective("Laima.Quests.f_whitetrees_23_3.Quest1004.Read", 4, true));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666146, character.Inventory.CountItem(666146), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(666148, character.Inventory.CountItem(666148), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_3.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_3.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666146, character.Inventory.CountItem(666146), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(666148, character.Inventory.CountItem(666148), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_3.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_3.Quest1004.Stone{i}");
	}
}

// Quest 1005 CLASS: The Sixth Is Still Bright
//-----------------------------------------------------------------------------

public class TheSixthIsStillBrightQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1005);
		SetName(L("The Sixth Is Still Bright"));
		SetType(QuestType.Sub);
		SetDescription(L("Five seal stones read CONFINED and have gone grey. The sixth stands at the middle tree, still bright, with two Rafflets sitting on it that have not moved in 6 days."));
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Explorer] Indraja"), "f_whitetrees_23_3");

		AddPrerequisite(new CompletedPrerequisite("f_whitetrees_23_3", 1004));

		AddObjective("clearMiddle", L("Kill Rafflets holding the middle ground"),
			new KillObjective(25, new[] { MonsterId.Rafflet }));

		AddObjective("takeThePair", L("Take the pair sitting on the sixth stone"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Rafflet, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Fragolin, 3),
				},
				resetIdent: "clearMiddle",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(583105, 1)); // Abomination Necklace
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666147, character.Inventory.CountItem(666147), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666147, character.Inventory.CountItem(666147), InventoryItemRemoveMsg.Destroyed);
	}
}
