//--- Melia Script ----------------------------------------------------------
// Akmens Ridge Quest NPCs
//--- Description -----------------------------------------------------------
// The third course of the Great King's seal: three barriers over the tomb
// mouth, and the youngest Jonas, who was never told what he is keeping.
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

public class FRokas27QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Eight Tokens Unaccounted
		//---------------------------------------------------------------------
		AddNpc(152002, L("[Archaeologist] Desig"), "f_rokas_27", 958, -1818, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1001);

			dialog.SetTitle(L("Desig"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's running a thumb down a long list of names, mouthing each one before he crosses to the next*{/}"));
				await dialog.Msg(L("Forgive me — I don't stop this count for just anyone, but you look like you're not here to dig. 41 years on this ridge. Everyone who works it carries a numbered token, and at season's end every token comes back to me. I write the number down and nobody thinks about it."));
				await dialog.Msg(L("8 have not come back. The Tucen have them - I have seen 2 on the same animal. Kill 25 and bring me all 8 tokens, because a number I cannot write down is a person."));

				var response = await dialog.Select(L("Will you go after the tokens?"),
					Option(L("I'll bring back all 8"), "help"),
					Option(L("Why tokens?"), "info"),
					Option(L("Write them off"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They swallow bright things and keep them. If an animal rattles when it moves, that is the one."));
						break;

					case "info":
						await dialog.Msg(L("Because a ridge like this loses people quietly. A man walks out to a barrier at first light and does not come back and there is no body and no story."));
						await dialog.Msg(L("41 years and I have written 8 numbers off in all that time. 8 more since spring."));
						break;

					case "leave":
						await dialog.Msg(L("I have written off 8 in 41 years and I remember every one of their names. I am not doing 8 in a season with a stroke of a pen."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTucen", out var killObj)) return;
				if (!quest.TryGetProgress("collectTokens", out var itemObj)) return;

				if (itemObj.Done && killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He reads each number off aloud and then writes it in the book without looking at the page*{/}"));
					await dialog.Msg(L("All 8. 3 of them are cable crew and 5 are people I sent out to the barriers myself, and now I know that instead of wondering it."));
					await dialog.Msg(L("Take the season's contingency. And do not tell Heinen which 3. I will do that, and I will do it tonight."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Enough of them down. Now go over the ground - the tokens are inside them, so look where they fell."));
				}
				else
				{
					await dialog.Msg(L("Not enough yet. Work the middle ridge where they den, not the south terraces."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("8 numbers, 8 names, and 8 letters to write. I have been doing this 41 years and it has never once got easier, which I have decided is correct."));
			}
		});

		// Quest 1002: Nine Miles of Cable
		//---------------------------------------------------------------------
		AddNpc(20102, L("[Engineer] Heinen"), "f_rokas_27", -195, -1683, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1002);

			dialog.SetTitle(L("Heinen"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's pacing beneath a ladder, glaring up at a dead splice like he could fix it by will alone*{/}"));
				await dialog.Msg(L("You! Good, finally, someone who isn't already up a ladder — 9 miles of signal cable across this whole ridge, 2 crews to run it, every splice done by hand in the wind, and William and Kanberg haven't spoken a civil word to each other in a year! It's a miracle anything up here works at all!"));
				await dialog.Msg(L("And now the Sauga are dragging the tool bags off the ladder foot the second a crew climbs — as if I don't have enough going wrong! Bring me back 10 bags and I can finally, finally put both lines back in service."));

				var response = await dialog.Select(L("Will you get the bags back?"),
					Option(L("I'll bring you 10 bags"), "help"),
					Option(L("What does the cable carry?"), "info"),
					Option(L("Guard the ladders instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They cache them. Find one bag and there will be 3 within 20 paces of it, so do not walk off after the first."));
						break;

					case "info":
						await dialog.Msg(L("Barrier readings. 3 barriers over the tomb mouth, each one wired down to the assistant's post, and if a barrier changes state the post knows inside a minute."));
						await dialog.Msg(L("Or it did. The left line has been out 11 days and I have been sleeping about 3 hours a night since."));
						break;

					case "leave":
						await dialog.Msg(L("I have 2 crews. If they stand at the ladder foot with a spear they are not up the ladder splicing, and the line stays dead either way."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectBags", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He empties each bag onto the rock and lines the splicing irons up by size without being asked to*{/}"));
					await dialog.Msg(L("10 bags and 7 full sets. Both lines back up by dark and I will sleep like a stone tonight, which I have earned."));
					await dialog.Msg(L("Take the works allowance. It is for replacing lost tools and you have just made that line in the book unnecessary."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough. Work the ladder feet along the left line - that is where they take them from."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Left line live at dusk. And the first thing it carried was the north barrier reading, and Airine came down off her post at a run, which she does not do."));
			}
		});

		// Quest 1003: The South Terraces
		//---------------------------------------------------------------------
		AddNpc(20128, L("[Mercenary] Glen"), "f_rokas_27", 151, -2362, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1003);

			dialog.SetTitle(L("Glen"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's standing at the top of the steps with a spear planted, watching you climb the whole way up*{/}"));
				await dialog.Msg(L("Heh. Long climb, that — most people turn back around the third step wheezing like a bellows, so congratulations, you've already impressed me more than most. I hold the south terraces. Five of them, stepped, only way a cart gets up onto this ridge from the valley. One job, four years, and I'm rather good at it, if I do say so."));
				await dialog.Msg(L("Only now the Ticen have taken all five terraces since the pillars started going, the cheeky things. Kill 30 and I'll get the corps' carts moving again — and maybe buy you a drink for the effort."));

				var response = await dialog.Select(L("Will you take the terraces?"),
					Option(L("I'll kill 30 Ticen"), "help"),
					Option(L("Since the pillars?"), "info"),
					Option(L("Carry the loads up by hand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Work down the steps, never up them. From below you are fighting 5 terraces of them at once and every one of them is above you."));
						break;

					case "info":
						await dialog.Msg(L("To the week. The bridge captain sent word that the pillar faces were coming off, and inside 6 days I had Ticen on the bottom terrace."));
						await dialog.Msg(L("I do not know what those 2 facts have to do with each other. I know that everyone up here who has been told anything has gone very quiet."));
						break;

					case "leave":
						await dialog.Msg(L("A barrier reading drum is 300 pounds. I would like to see the man who carries that up 5 terraces, and then I would like to hire him."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTicen", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("All 5 terraces walked and a cart up the whole flight this afternoon. 4 years on this job and that is the most of it I have ever actually done in one day."));
					await dialog.Msg(L("Take the terrace fee. It is paid per cart and I have just had 1 cart in 3 weeks, so the arithmetic is embarrassing and the purse is full."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still holding the steps. Take the top terrace first and come down - do not start at the bottom."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("3 carts up in 2 days, all of them barrier stores. Nobody has ever sent barrier stores up here in a hurry before."));
			}
		});

		// Quest 1004: Three Barriers
		//---------------------------------------------------------------------
		AddNpc(152001, L("[Assistant] Airine"), "f_rokas_27", 917, 381, 338, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1004);

			dialog.SetTitle(L("Airine"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's tapping a dead cable terminal with a stylus, waiting for a signal that isn't coming*{/}"));
				await dialog.Msg(L("Oh! Sorry — I didn't hear you come up the path, I was listening for the wrong thing entirely. I take the barrier readings. 3 barriers over the tomb mouth - west, east, north - read at dawn and dusk, 2 numbers each, into a book. 3 years and I've never had a number I could not explain."));
				await dialog.Msg(L("The cable has been out on the left line for 11 days and I cannot read the west and north from here. Go to all 3 and look at them with your own eyes."));

				var response = await dialog.Select(L("Will you walk the barriers?"),
					Option(L("I'll look at all 3"), "help"),
					Option(L("What are you reading for?"), "info"),
					Option(L("Wait for the cable"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Do not touch the face. Stand at the reading mark, which is the flat stone in front of each one, and tell me the colour and whether it hums."));
						break;

					case "info":
						await dialog.Msg(L("I have never been told. I was taught 2 numbers, a book, and the sentence 'if either number moves, send a rider'. That is my entire training."));
						await dialog.Msg(L("In 3 years neither number has moved. I have sometimes wondered whether the rider part was a joke."));
						break;

					case "leave":
						await dialog.Msg(L("11 days. Heinen says tonight and he said tonight 4 days ago, and he is not lying, he is just up a ladder in the wind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readBarriers", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*She writes all 3 into the book, and her hand stops halfway through the north entry*{/}"));
					await dialog.Msg(L("West steady. East steady. North not humming at all, and warm to stand in front of, and I have no number in 3 years to compare that to."));
					await dialog.Msg(L("Take the post's money. I am sending the rider. I have been trained for exactly one thing and today is the day it turns out to have been for."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 3. West is below the shoulder, east is out past the shelf, and north is up above them both."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The rider went to Rolandas and came back inside the hour, which means Rolandas did not have to think about it. That frightens me more than the north reading did."));
			}
		});

		// Quest 1004 collection points - the three barriers
		//---------------------------------------------------------------------
		void AddBarrier(int barrierNumber, string barrierName, string observation, int x, int z, int direction)
		{
			AddNpc(47109, barrierName, "f_rokas_27", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_rokas_27", 1004);
				var variableKey = $"Laima.Quests.f_rokas_27.Quest1004.Barrier{barrierNumber}";
				var counterKey = "Laima.Quests.f_rokas_27.Quest1004.BarriersRead";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A standing barrier cube set over the tomb mouth, with a flat reading stone in front of it*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already read this one*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Taking the reading..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var read = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, read);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Barriers read: {0}/3", read));

					if (read >= 3)
						character.ServerMessage(L("{#FFD700}All 3 barriers read. Return to Airine.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the barrier unread."));
				}
			});
		}

		AddBarrier(1, L("West Barrier"),
			L("West Barrier: pale blue, humming steadily, the reading stone cold underfoot."), 764, 239, 0);
		AddBarrier(2, L("East Barrier"),
			L("East Barrier: pale blue, humming steadily, and a hairline crack across the base that is packed with grey dust."), 1134, 341, 0);
		AddBarrier(3, L("North Barrier"),
			L("North Barrier: no colour at all, silent, and the reading stone is warm enough to feel through a boot."), 984, 606, 0);

		// Quest 1005: The Room That Was Empty
		//---------------------------------------------------------------------
		AddNpc(147399, L("[Recorder] Rolandas Jonas"), "f_rokas_27", -486, -3034, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_27", 1005);

			dialog.SetTitle(L("Rolandas Jonas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_rokas_27", 1004)))
				{
					await dialog.Msg(L("{#666666}*He's standing at a sealed chest, one hand resting flat on the lid without opening it*{/}"));
					await dialog.Msg(L("Airine's readings first. I am 29 and I have been handed this ridge and a sealed instruction, and I would like to be acting on a number rather than on a feeling."));
					return;
				}

				await dialog.Msg(L("{#666666}*He finally lifts the lid, two letters already unfolded on top of the chest*{/}"));
				await dialog.Msg(L("The gate is open at the gateway. The second course is being eaten in the valley. And the north barrier is dark and warm. My uncle's rider and my great-uncle's letter reached me in the same hour."));
				await dialog.Msg(L("I have a map my house has held for 90 years with an instruction never to open it. I am opening it. Kill 20 Sauga to clear the tomb approach, then be standing there with me when I do."));

				var response = await dialog.Select(L("Will you go down to the tomb mouth?"),
					Option(L("I'll clear the approach and stand there"), "help"),
					Option(L("Ninety years and nobody opened it?"), "info"),
					Option(L("Send for the Kingdom first"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650532, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("You carry it. If my hands are on it and I am wrong, the house takes it. If yours are, it is simply a thing that happened on the ridge."));
						break;

					case "info":
						await dialog.Msg(L("My great-grandfather could have. My great-uncle sat beside a dark Eye for 5 months writing 'lit'. My uncle drank tea at an epigrapher for 5 years."));
						await dialog.Msg(L("Every generation of us was told a little less than the one before, and every one of us decided that was somebody else's problem. It is now nobody else's."));
						break;

					case "leave":
						await dialog.Msg(L("The Kingdom approves this expedition's stores on 90 years of precedent and has never sent an inspector. If I send for them today, they arrive in 3 weeks."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearApproach", out var approachObj)) return;
				if (!quest.TryGetProgress("killTombLord", out var bossObj)) return;

				if (approachObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("{#666666}*He unrolls the map on the flat of a barrier stone and reads the chamber list twice before he speaks*{/}"));
					await dialog.Msg(L("It is not a treasure map. It is the tomb's own plan, with every chamber named, and beside the Great King's chamber there are 2 words in a much later hand: 'empty since'."));
					await dialog.Msg(L("Take the mace off the mouth. 90 years, 4 generations, 3 courses of seal, and my house has been guarding an empty room. Something walked out of here before my great-grandfather was born and nobody has ever asked where it went."));

					character.Quests.Complete(questId);
				}
				else if (approachObj.Done)
				{
					await dialog.Msg(L("Approach is clear. What is standing in the mouth is not a guardian - guardians face outward, and that one is facing in."));
				}
				else
				{
					await dialog.Msg(L("Too many Sauga on the approach. Clear it first. I would rather open the map once and read it properly."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have sent the map and the plain truth to Fedimian under my own name, and a copy to my uncle, and a copy to Gailas at the gateway. Whatever comes of that, no Jonas after me gets handed a sealed instruction."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Eight Tokens Unaccounted
//-----------------------------------------------------------------------------

public class EightTokensUnaccountedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1001);
		SetName(L("Eight Tokens Unaccounted"));
		SetType(QuestType.Sub);
		SetDescription(L("Everyone who works Akmens Ridge carries a numbered token and every token comes back at season's end. 8 have not, and the Tucen have them."));
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Archaeologist] Desig"), "f_rokas_27");

		AddObjective("killTucen", L("Kill Tucen on the middle ridge"),
			new KillObjective(25, new[] { MonsterId.Tucen }));

		AddObjective("collectTokens", L("Recover all 8 Archaeologist Tokens"),
			new CollectItemObjective(650505, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(650505, 0.35f, MonsterId.Tucen);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650505, character.Inventory.CountItem(650505), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650505, character.Inventory.CountItem(650505), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Nine Miles of Cable
//-----------------------------------------------------------------------------

public class NineMilesOfCableQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1002);
		SetName(L("Nine Miles of Cable"));
		SetType(QuestType.Sub);
		SetDescription(L("9 miles of signal cable carry the barrier readings down to the assistant's post, and the left line has been dead for 11 days. The Sauga take the crews' tool bags off the ladder foot the moment anyone climbs."));
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Engineer] Heinen"), "f_rokas_27");

		AddObjective("collectBags", L("Recover the cable crews' tool bags"),
			new CollectItemObjective(650504, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(650504, 0.45f, MonsterId.Sauga_S);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650504, character.Inventory.CountItem(650504), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650504, character.Inventory.CountItem(650504), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The South Terraces
//-----------------------------------------------------------------------------

public class TheSouthTerracesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1003);
		SetName(L("The South Terraces"));
		SetType(QuestType.Sub);
		SetDescription(L("The 5 stepped south terraces are the only cart road up onto Akmens Ridge, and the Ticen took all 5 in the week the pillars started coming apart. Kill 30 of them."));
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mercenary] Glen"), "f_rokas_27");

		AddObjective("killTicen", L("Kill Ticen on the south terraces"),
			new KillObjective(30, new[] { MonsterId.Ticen }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Three Barriers
//-----------------------------------------------------------------------------

public class ThreeBarriersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1004);
		SetName(L("Three Barriers"));
		SetType(QuestType.Sub);
		SetDescription(L("3 barriers stand over the tomb mouth and are read at dawn and dusk into a book. The cable that carries 2 of those readings has been dead for 11 days."));
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Assistant] Airine"), "f_rokas_27");

		AddObjective("readBarriers", L("Take readings at all 3 barriers"),
			new VariableCheckObjective("Laima.Quests.f_rokas_27.Quest1004.BarriersRead", 3, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_27.Quest1004.BarriersRead");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_27.Quest1004.Barrier{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_27.Quest1004.BarriersRead");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_27.Quest1004.Barrier{i}");
	}
}

// Quest 1005 CLASS: The Room That Was Empty
//-----------------------------------------------------------------------------

public class TheRoomThatWasEmptyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_27", 1005);
		SetName(L("The Room That Was Empty"));
		SetType(QuestType.Sub);
		SetDescription(L("The outer gate is open, the pillar course is being eaten and the north barrier has gone dark. The youngest Jonas intends to open the map his house has held unopened for 90 years, at the tomb mouth."));
		SetLocation("f_rokas_27");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Recorder] Rolandas Jonas"), "f_rokas_27");

		AddPrerequisite(new CompletedPrerequisite("f_rokas_27", 1004));

		AddObjective("clearApproach", L("Kill Sauga on the tomb approach"),
			new KillObjective(20, new[] { MonsterId.Sauga_S }));

		AddObjective("killTombLord", L("Defeat the Tomb Lord standing in the mouth"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_TombLord, 1) },
				resetIdent: "clearApproach",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(203201, 1)); // Aghaas Breaker
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650532, character.Inventory.CountItem(650532), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650532, character.Inventory.CountItem(650532), InventoryItemRemoveMsg.Destroyed);
	}
}
