//--- Melia Script ----------------------------------------------------------
// Vedas Plateau - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_tableland_11_1 map. A company went up the
// shelf, the return says the action concluded, and there is no list.
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

public class FTableland111QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Forty-One Names
		// =====================================================================
		// Necromancer Adomas - the list the army would not write
		//---------------------------------------------------------------------
		AddNpc(156022, L("[Necromancer] Adomas"), "f_tableland_11_1", 370, -1293, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1001);

			dialog.SetTitle(L("Adomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A necromancer sits on the warp stone with a folded paper open on his knee, reading the same side of it again*{/}"));
				await dialog.Msg(L("You'll want to keep walking if you're here for the plateau's scenery. There isn't any. 41 names. Families gave me these, one at a time, in doorways on Rukles Street, because the Kingdom's return for this plateau says the action concluded and lists nobody."));
				await dialog.Msg(L("I am not licensed to be here and I am not leaving. The Red Saltisdaughters are wearing what those men were carrying - kill 25 and bring me 8 pieces of it."));

				var response = await dialog.Select(L("Will you walk up the shelf with me?"),
					Option(L("I'll bring back what they took"), "help"),
					Option(L("Concluded how?"), "info"),
					Option(L("This is the army's business"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Belongings, not trophies. A buckle with a company stamp is worth more to me than a sword. The stamp is the only thing on this plateau nobody has thought to burn."));
						await dialog.Msg(L("They carry it openly. Widlings do not hide what they take, which is the one mercy in any of this."));
						break;

					case "info":
						await dialog.Msg(L("A word. One word, in a ledger, in Roxona, written by a clerk who has never been above the market wall. Concluded means no pension, no grave detail, and no reason for anybody official to come up here and look."));
						await dialog.Msg(L("41 families were handed that word and told it was an answer. I have been a necromancer 30 years and I have never been asked to do anything as ordinary as give people a list."));
						break;

					case "leave":
						await dialog.Msg(L("It was. For 8 weeks it was the army's business, and the army's business was to write one word and close the book."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSaltis", out var killObj)) return;
				if (!quest.TryGetProgress("collectBelongings", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lays the 8 pieces out on the stone in a line and reads the stamps off them one after another*{/}"));
					await dialog.Msg(L("Same company. All 8. Two of these names are on my paper and I can write a line through them tonight, and the families will hate me for it and thank me for it in the same hour."));
					await dialog.Msg(L("Take the fee. It is not the Kingdom's coin - it is 41 households' coin, and every one of them put in what they had."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The ground is quieter. I still need 8 pieces with a stamp on them - a stamp, not a story."));
				}
				else
				{
					await dialog.Msg(L("25 of the Red ones, and take the pieces off them as you go. They wear it where you can see it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("11 names crossed off in 3 weeks. I read them out at the camp each evening whether anyone is listening or not, because a name that is only written down is halfway to being a word in a ledger."));
			}
		});

		// =====================================================================
		// QUEST 1002: What Kept Its Edge
		// =====================================================================
		// Cryomancer Elema - keeping the evidence cold
		//---------------------------------------------------------------------
		AddNpc(156021, L("[Cryomancer] Elema"), "f_tableland_11_1", 26, 650, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1002);

			dialog.SetTitle(L("Elema"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A cryomancer kneels over a low bed of packed frost with cloth-wrapped bundles laid on it in rows*{/}"));
				await dialog.Msg(L("Stay off the frost bed. I would rather not explain a boot print to Faustas. I do no magic on the dead. None. I keep them cold, that is the entirety of my function here."));
				await dialog.Msg(L("The plateau is warm. Tissue softens after 8 weeks. Faustas cannot read a wound that has closed on itself. Bring me 6 clean samples off the Green Saltisdaughter Archers, cut properly, and I will do the rest."));

				var response = await dialog.Select(L("Will you cut samples for me?"),
					Option(L("I'll get you 6 samples"), "help"),
					Option(L("Why the Archers?"), "info"),
					Option(L("Ask a surgeon"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut deep and cut once. A sample sawed at 3 times tells me about your knife, not about the creature."));
						await dialog.Msg(L("And do not carry them against your body. Cold is the whole of my contribution here and I would like it to survive the walk."));
						break;

					case "info":
						await dialog.Msg(L("Because the wounds on the recovered men are narrow and clean and go straight through, and I want to know whether anything on this plateau can actually do that."));
						await dialog.Msg(L("I have a suspicion about the answer and I am not going to say it out loud until I have 6 samples. Suspicions said out loud have a way of becoming the thing everybody looks for."));
						break;

					case "leave":
						await dialog.Msg(L("Roxona has 40 surgeons and not one of them will sign a note about a company that officially came home."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectSamples", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays the 6 samples in a row on the frost bed and works down them without looking up*{/}"));
					await dialog.Msg(L("Nothing here makes that wound. Not the jaw, not the claw, not the arrowhead. I have 6 samples and the answer is no."));
					await dialog.Msg(L("Which means the men were not killed by the plateau. Take this before I go and say that to Adomas, because I am not sure he is going to be able to pay anybody after he hears it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("6 samples, off the Green Archers, cut once. They range the high ground north and east of the camp."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The samples are still on the bed and I have not let them thaw once in 9 days. Somebody official is going to ask to see them eventually and I intend to be able to hand them over."));
			}
		});

		// =====================================================================
		// QUEST 1003: Ground She Cannot Work
		// =====================================================================
		// Necromancer Lemija - the west ground
		//---------------------------------------------------------------------
		AddNpc(156020, L("[Necromancer] Lemija"), "f_tableland_11_1", -1051, -426, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1003);

			dialog.SetTitle(L("Lemija"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A necromancer is pacing a slow grid across open ground, setting a peg every dozen steps, and doesn't break stride*{/}"));
				await dialog.Msg(L("Don't mind me, I count steps when I talk or I lose the count. I walk. That is my whole job in this camp - I walk the ground in lines and I put a peg where something lay."));
				await dialog.Msg(L("I have done 14 of the 19 sectors and I cannot do the last 5 because the Green Lepusbunny Magicians hold them. Kill 20 of them."));

				var response = await dialog.Select(L("Will you clear the west sectors?"),
					Option(L("I'll clear the sectors"), "help"),
					Option(L("Why walk it at all?"), "info"),
					Option(L("Peg round them"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(667034, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take my amulet. Their wards read a living body as a shape and the amulet spoils the shape - it will not stop an arrow, it will stop 3 of them agreeing on where you are."));
						await dialog.Msg(L("They hold the low ground southwest of me and they hold it in threes. Break the threes."));
						break;

					case "info":
						await dialog.Msg(L("Because a body that has been on open ground 8 weeks is not where it fell any more, and Faustas can only ask a question of something he is standing over."));
						await dialog.Msg(L("A peg is not magic. A peg is just somebody having bothered. I have put 340 of them in this plateau and I will put in as many more as it takes."));
						break;

					case "leave":
						await dialog.Msg(L("Then the last 5 sectors go in the report as unwalked, and unwalked is how 41 men became a word in a ledger the first time."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMagicians", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*She is already 200 paces out on the cleared ground with a peg in each hand*{/}"));
					await dialog.Msg(L("19 of 19. It took me 6 weeks to do 14 and you gave me the other 5 in an afternoon."));
					await dialog.Msg(L("Keep the amulet's fee and give me the amulet back. It was my mother's, and she also walked ground nobody had bothered to walk."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Southwest, low ground, and they work in threes. 20 of them and I have my last 5 sectors."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("All 19 sectors pegged and drawn. And the pegs are not scattered. They are in a line, and the line is pointed the way a column walks."));
			}
		});

		// =====================================================================
		// QUEST 1004: Where They Went Down
		// =====================================================================
		// Necromancer Faustas - the fall-marks
		//---------------------------------------------------------------------
		AddNpc(156019, L("[Necromancer] Faustas"), "f_tableland_11_1", 25, 604, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1004);

			dialog.SetTitle(L("Faustas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A necromancer stands over a pegged square of turf with both hands loose at his sides, waiting rather than working*{/}"));
				await dialog.Msg(L("You'll pardon me not shaking your hand — I've been standing on cold ground all morning and I'm not sure mine still work right. Lemija pegs where they lay. I stand on the peg and ask one question: not how they died, but which way they were facing."));
				await dialog.Msg(L("Go to 4 of her marks and tell me what the ground says at each."));

				var response = await dialog.Select(L("Will you read the 4 marks?"),
					Option(L("I'll read the 4 marks"), "help"),
					Option(L("Why which way they were facing?"), "info"),
					Option(L("Ask them how they died"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(667031, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take Adomas' list with you. Stand on the mark, hold the paper, and do not say anything. The names do the work; you are only there to be a warm thing standing where a cold one was."));
						await dialog.Msg(L("It is 3 seconds and it is deeply unpleasant. I have done it 340 times and it has not once become ordinary."));
						break;

					case "info":
						await dialog.Msg(L("Because a man who died fighting is facing his enemy and a man who died withdrawing is not. That is the entire difference between an action and something else, and it is written into the ground under him."));
						await dialog.Msg(L("Nobody has ever paid me to find out which way a corpse was pointed. It is the most useful thing I have ever been asked."));
						break;

					case "leave":
						await dialog.Msg(L("How they died I can guess from Elema's samples. Which way they were facing, only the ground knows, and only for a while longer."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var read = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_11_1.Quest1004.Read", 0);

				if (read >= 4)
				{
					await dialog.Msg(L("{#666666}*He sets the 4 readings out on the turf in the order you walked them and stares at the arrangement for a long time*{/}"));
					await dialog.Msg(L("All 4 facing the same way. Not outward, not scattered, not turned to meet anything. In file. They were walking, in column, away, and something took them from behind while they did it."));
					await dialog.Msg(L("That is not a battle. That is a withdrawal. Take the camp's fee - and understand that you have just made this a very different kind of illegal."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Stand on the mark with the list. {0} of 4 read. Do not speak while you are on it.", read));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("340 pegs and every reading in the same direction. A company does not withdraw on its own. Somebody sent an order up this shelf and somebody else made sure it was never carried back down."));
			}
		});

		// =====================================================================
		// FALL-MARKS
		// =====================================================================
		// For Quest 1004 - Where They Went Down
		// =====================================================================

		void AddFallMark(int markNumber, string markName, string reading, int x, int z, int direction)
		{
			AddNpc(47251, L(markName), "f_tableland_11_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_11_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pegged square of turf with the grass still lying the wrong way inside it*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_11_1.Quest1004.Mark{markNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have already stood on this one. The cold of it is still in your boots*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Standing on the mark..."), "Cancel", "PRAY", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Reading interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);

				var read = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_11_1.Quest1004.Read", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_tableland_11_1.Quest1004.Read", read);

				character.ServerMessage(L(reading));
				character.ServerMessage(LF("Marks read: {0}/4", read));

				if (read >= 4)
					character.ServerMessage(L("{#FFD700}All 4 marks read. Return to Faustas.{/}"));
			});
		}

		AddFallMark(1, "Fall-Mark 41", "Facing northeast. Weapon hand empty, pack still on both shoulders.", 114, -225, 0);
		AddFallMark(2, "Fall-Mark 106", "Facing northeast. Nothing under the hands. He went down without turning.", 503, 88, 0);
		AddFallMark(3, "Fall-Mark 219", "Facing northeast. Boots dug in forward, not braced back.", 688, 580, 0);
		AddFallMark(4, "Fall-Mark 288", "Facing northeast, the same as the other 3. Every one of them walking the same way.", 352, 935, 0);

		// =====================================================================
		// The Named Stone - atmosphere at the camp
		//---------------------------------------------------------------------
		AddNpc(47251, L("The One Named Stone"), "f_tableland_11_1", 21, 682, 282, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A cut stone standing at the edge of the camp with a single name on it and a ring set into the base*{/}"));
			await dialog.Msg(L("{#666666}*The ring is an engagement ring. His fiancee gave it to Adomas as the only way anyone could tell him from the others, and Adomas gave it back to the ground with him*{/}"));
			await dialog.Msg(L("{#666666}*One stone. Forty more names on a folded paper, and a camp of 4 people working through them one at a time*{/}"));
		});

		// =====================================================================
		// QUEST 1005: The Cart That Only Went Up
		// =====================================================================
		// Cart Manager - the requisition that was cancelled two days early
		//---------------------------------------------------------------------
		AddNpc(20150, L("[Cart Manager] Vitas"), "f_tableland_11_1", -2320, 2046, 284, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1005);

			dialog.SetTitle(L("Vitas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_tableland_11_1", 1004)))
				{
					await dialog.Msg(L("You are the third person off that camp to walk 3400 paces to ask me a question. Go and finish asking the ground first. I will still be here - I am always here, that is the job."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's been waiting at the winch, and waves you over the moment he sees you're back*{/}"));
				await dialog.Msg(L("Ah, good, you're back — sit, sit, I've been dying to tell someone this properly. I run the shelf cart. Forty-one men went up on it in 6 loads and I have never once brought one back down. And here's the part nobody's thought to ask me in 8 weeks: the downward requisition was cancelled 2 days before the action."));
				await dialog.Msg(L("Two days before, mind you — not after, before! Somebody knew there'd be nothing to bring down. Armaos has been sitting on that ground ever since and nothing official will come up here while it's there. Kill 20 of the White Grolls off the haul line and go take it, would you?"));

				var response = await dialog.Select(L("Will you clear my line and take Armaos?"),
					Option(L("I'll take Armaos"), "help"),
					Option(L("Cancelled by whom?"), "info"),
					Option(L("Show this to a magistrate"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(667035, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Carry the slip. It is the cancellation, and it has an Assistant Commander's mark on it from the Mesafasla post, and I have kept it in a tin under the winch house since the week it came."));
						await dialog.Msg(L("Grolls first or you will be fighting the haul line and the boss together. And do not fight Armaos on a slope. It has 4 legs and you have 2, and the slope is its idea."));
						break;

					case "info":
						await dialog.Msg(L("The mark on the slip is an Assistant Commander's, from Mesafasla, one warp northwest of here. I have never met him. I have carried his cancellations for 6 years."));
						await dialog.Msg(L("I do not think a man cancels a cart because he wants 41 people dead. I think he cancels it because he was told to and it was one line on a form. That is somehow the worse of the two."));
						break;

					case "leave":
						await dialog.Msg(L("I offered it to a magistrate in the third week. He asked me which company, I told him, and he told me that company came home. Then he wrote nothing down and I walked back up the shelf."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearLine", out var lineObj)) return;
				if (!quest.TryGetProgress("killArmaos", out var bossObj)) return;

				if (lineObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("{#666666}*He runs the cart down empty, all the way, for the first time in 8 weeks, and stands listening to it come back*{/}"));
					await dialog.Msg(L("Line is clear and the ground is open and Adomas can put a grave detail on it. 41 stones instead of 1."));
					await dialog.Msg(L("Take this out of the winch house - it was pinned over the door when I got the post and I have never known whose it was. And take the slip to Mesafasla. I am too old to walk it and I have wanted somebody to walk it for 8 weeks."));

					character.Quests.Complete(questId);
				}
				else if (lineObj.Done)
				{
					await dialog.Msg(L("Haul line's clear. Armaos is still on the ground and it will not leave the ground, which tells you something on its own."));
				}
				else
				{
					await dialog.Msg(L("20 Grolls off the line first. I am not sending anybody at Armaos with a haul line full of Grolls behind them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Grave detail came up on the cart 4 days ago - 6 people, no uniforms, paid out of 41 households. They are still up there. I take them bread twice a day and they have not asked me for anything else."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Forty-One Names
//-----------------------------------------------------------------------------

public class FortyOneNamesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1001);
		SetName(L("Forty-One Names"));
		SetType(QuestType.Sub);
		SetDescription(L("The Kingdom's return for the Vedas Plateau says the action concluded and lists nobody. Adomas has 41 names from 41 doorways. The Red Saltisdaughters are wearing what the company carried."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Necromancer] Adomas"), "f_tableland_11_1");

		AddObjective("killSaltis", L("Kill Red Saltisdaughters holding the shelf"),
			new KillObjective(25, new[] { MonsterId.Saltisdaughter_Red }));

		AddObjective("collectBelongings", L("Recover stamped soldier's belongings"),
			new CollectItemObjective(667036, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(667036, 0.35f, MonsterId.Saltisdaughter_Red);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667036, character.Inventory.CountItem(667036), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667036, character.Inventory.CountItem(667036), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: What Kept Its Edge
//-----------------------------------------------------------------------------

public class WhatKeptItsEdgeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1002);
		SetName(L("What Kept Its Edge"));
		SetType(QuestType.Sub);
		SetDescription(L("The wounds on the recovered men are narrow, clean and straight through. Elema wants 6 samples off the Green Saltisdaughter Archers to find out whether anything on this plateau can actually make one."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Cryomancer] Elema"), "f_tableland_11_1");

		AddObjective("collectSamples", L("Cut samples from Green Saltisdaughter Archers"),
			new CollectItemObjective(667033, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(667033, 0.35f, MonsterId.Saltisdaughter_Bow_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667033, character.Inventory.CountItem(667033), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667033, character.Inventory.CountItem(667033), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Last Five Sectors
//-----------------------------------------------------------------------------

public class TheLastFiveSectorsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1003);
		SetName(L("The Last Five Sectors"));
		SetType(QuestType.Sub);
		SetDescription(L("Lemija has walked 14 of the plateau's 19 sectors on foot and pegged every place a body lay. The Green Lepusbunny Magicians hold the other 5."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Necromancer] Lemija"), "f_tableland_11_1");

		AddObjective("killMagicians", L("Kill Green Lepusbunny Magicians in the west sectors"),
			new KillObjective(20, new[] { MonsterId.Repusbunny_Mage_Green }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667034, character.Inventory.CountItem(667034), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667034, character.Inventory.CountItem(667034), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Where They Went Down
//-----------------------------------------------------------------------------

public class WhereTheyWentDownQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1004);
		SetName(L("Where They Went Down"));
		SetType(QuestType.Sub);
		SetDescription(L("Faustas does not want to know how the company died. He wants to know which way each man was facing. Stand on 4 of Lemija's fall-marks with Adomas' list and read what the ground says."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Necromancer] Faustas"), "f_tableland_11_1");

		AddObjective("readMarks", L("Read the 4 fall-marks"),
			new VariableCheckObjective("Laima.Quests.f_tableland_11_1.Quest1004.Read", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667031, character.Inventory.CountItem(667031), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_11_1.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_11_1.Quest1004.Mark{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667031, character.Inventory.CountItem(667031), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_11_1.Quest1004.Read");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_11_1.Quest1004.Mark{i}");
	}
}

// Quest 1005 CLASS: The Cart That Only Went Up
//-----------------------------------------------------------------------------

public class TheCartThatOnlyWentUpQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1005);
		SetName(L("The Cart That Only Went Up"));
		SetType(QuestType.Sub);
		SetDescription(L("The downward requisition for the company was cancelled 2 days before the action, over an Assistant Commander's mark from Mesafasla. Armaos holds the ground where they fell and nothing official will come up while it is there."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Cart Manager] Vitas"), "f_tableland_11_1");

		AddPrerequisite(new CompletedPrerequisite("f_tableland_11_1", 1004));

		AddObjective("clearLine", L("Kill White Grolls off the haul line"),
			new KillObjective(20, new[] { MonsterId.Groll_White }));

		AddObjective("killArmaos", L("Take Armaos off the ground where they fell"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Boss_Armaox, 1),
					new KillSpec(MonsterId.Groll_White, 3, BuffId.EliteMonsterBuff),
				},
				resetIdent: "clearLine",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583122, 1)); // General's Gift
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667035, character.Inventory.CountItem(667035), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667035, character.Inventory.CountItem(667035), InventoryItemRemoveMsg.Destroyed);
	}
}
