//--- Melia Script ----------------------------------------------------------
// Letas Stream Quest NPCs
//--- Description -----------------------------------------------------------
// The Kingdom water-survey station on the Letas, and the five people who died
// at their posts when the stream turned.
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

public class FKatyn12QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: Iron in the Letas
		//---------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Warden Auksuolis"), "f_katyn_12", -289, -1009, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1001);

			dialog.SetTitle(L("Auksuolis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He turns at the sound of a living footstep — the first he's heard in longer than he can say*{/}"));
				await dialog.Msg(L("You can see me. That alone is worth stopping for. Warden Auksuolis, I ran this station — five staff, one stream, and a weekly report to Fedimian on whether their drinking water was fit. For nine years it was."));
				await dialog.Msg(L("Then the Corrupt Chupacabra came down to the bend and started drinking, and whatever's in them went into the water, and I signed off on it because it looked clear. Kill 20 of them at the bend so nobody downstream repeats my week."));

				var response = await dialog.Select(L("Will you clear the bend?"),
					Option(L("I'll kill the Chupacabra"), "help"),
					Option(L("You signed off on it?"), "info"),
					Option(L("Rest, warden"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They water at the bend twice a day and they're slow getting up the bank. That's the whole tactic and it's the one I should have used."));
						break;

					case "info":
						await dialog.Msg(L("Clear water, no smell, no sediment. Every test I had said fit. The tests were nine years old and written for a stream nothing had ever poisoned."));
						await dialog.Msg(L("Girenas told me to hold the report. I told him we'd miss the courier. He was the clerk and I was the warden, so we sent it."));
						break;

					case "leave":
						await dialog.Msg(L("I have tried resting. It turns out meaning well doesn't buy you that, not even eleven years on."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killChupacabra", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The bend's clear. Give it a season and the Letas will run clean again on its own - it did before, it will again."));
					await dialog.Msg(L("Take the station purse. Nobody's come for it in eleven years and it isn't doing anything down here."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More of them still at the bend. They come down to drink twice a day."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I went and stood in the shallows this morning. It tastes like water. That's all I wanted and it took eleven years."));
			}
		});

		// Quest 1002: The Undelivered Sample
		//---------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Waterclerk Girenas"), "f_katyn_12", -2241, 526, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1002);
			var deliveredKey = "Laima.Quests.f_katyn_12.Quest1002.Delivered";

			dialog.SetTitle(L("Girenas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's patting down a satchel that's been slung over the same shoulder for eleven years, as if checking it's still there*{/}"));
				await dialog.Msg(L("A living visitor. How novel. Waterclerk Girenas — I drew a second sample the day the fatal report went out and never got it to the chemist. It is sitting in this satchel, stoppered, exactly where I left it."));
				await dialog.Msg(L("Auksuolis will tell you the tests were old. Fine. This sample would have shown it anyway, because Vilte tested differently and better. Carry it to her and let her finish the work."));

				var response = await dialog.Select(L("Will you take it to her?"),
					Option(L("I'll carry the sample to Vilte"), "help"),
					Option(L("Does it matter now?"), "info"),
					Option(L("Let it go"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(661053, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Her bench is downstream on the east bank. Keep the stopper in - she'll want to break the seal herself, she always did."));
						break;

					case "info":
						await dialog.Msg(L("It matters to me. I was the clerk. My entire function was that the right paper reached the right desk, and one time it did not, and here I still am."));
						await dialog.Msg(L("You may consider that pathetic. I have had eleven years to consider it and I have arrived at the same conclusion."));
						break;

					case "leave":
						await dialog.Msg(L("I have tried letting it go, believe me. It is remarkably difficult to set down a satchel you've carried eleven years."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("She read it? And she said what, precisely?"));
					await dialog.Msg(L("{#666666}*He listens without interrupting, which is not something he did in life*{/}"));
					await dialog.Msg(L("Then the sample was sound and the method was sound and the only thing wrong was the courier schedule. Thank you. Take the satchel - I have no further use for a satchel."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Vilte's bench is downstream on the east bank. She will be at it. She was always at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Eleven years of assuming I had failed, and it turns out I had merely been overruled. There is a great deal of difference between those."));
			}
		});

		// Quest 1002 recipient - the station chemist
		//---------------------------------------------------------------------
		AddGhostNpc(155132, L("[Restless Soul] Chemist Vilte"), "f_katyn_12", 2100, -200, 179, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1002);
			var deliveredKey = "Laima.Quests.f_katyn_12.Quest1002.Delivered";

			dialog.SetTitle(L("Vilte"));

			if (character.Quests.IsActive(questId) && character.Inventory.HasItem(661053))
			{
				if (!character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("{#666666}*She breaks the seal herself and holds the vial up against the light*{/}"));
					await dialog.Msg(L("There it is. Sediment ring at the neck, faint green cast - that's the second sample I asked him for and never got, and it would have failed the water in about four minutes."));
					await dialog.Msg(L("Tell him his sample was good. Tell him his method was good. Tell him I said so and that I am not in the habit of saying so."));

					character.Variables.Perm.Set(deliveredKey, true);
					character.Quests.CompleteObjective(questId, "deliverSample");
					character.ServerMessage(L("{#FFD700}Vilte has read the sample. Return to Waterclerk Girenas.{/}"));
				}
				else
				{
					await dialog.Msg(L("Go on, then. He's been waiting eleven years for a verdict and I've given you one."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("He came down here afterward. Eleven years on the same bank and neither of us had walked twenty minutes to speak to the other. We are both very stupid people."));
			}
			else
			{
				await dialog.Msg(L("I tested this water four times a day for six years — I could've told you its temperature by taste alone. That skill is entirely useless to me now, and it still nags at me."));
			}
		});

		// Quest 1003: What the Fog Carries
		//---------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Mist-Reader Adomas"), "f_katyn_12", -3044, 1496, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1003);

			dialog.SetTitle(L("Adomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's watching the fog roll off the water, tracking it out of old habit though there's no report left to file*{/}"));
				await dialog.Msg(L("You walk through fog like a living person does — I'd nearly forgotten what that looked like. Mist-Reader Adomas. My job was the morning fog: where it sat, how long it held, whether it carried anything. Everyone thought it was the softest post on the roster, and they were right."));
				await dialog.Msg(L("But the Blue Operor drink the fog now, and what comes off them when they die isn't water. It's people. Bring me 8 of them and I'll find out which people."));

				var response = await dialog.Select(L("Will you gather them?"),
					Option(L("I'll bring the fading spirits"), "help"),
					Option(L("It's people?"), "info"),
					Option(L("That's a grim errand"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take what comes off them straight away. It fades within a minute or two, which is more or less how it got its name."));
						break;

					case "info":
						await dialog.Msg(L("There were eleven settlements up this valley before the station. Nobody left, exactly. They stopped filing tallies and the Kingdom stopped counting them."));
						await dialog.Msg(L("The fog sat over all eleven for a season. I wrote it down as unremarkable. I have had a long time to think about that word."));
						break;

					case "leave":
						await dialog.Msg(L("It is grim work, I won't argue that. It is also the only census those eleven settlements will ever get."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectSpirits", out var spiritObj)) return;

				if (spiritObj.Done)
				{
					await dialog.Msg(L("Eight, and six of them still had names in them. Six names is a start on eleven settlements."));
					await dialog.Msg(L("Take my glass. It won't show you anything, but it's good glass and it was expensive."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More Operor still working the fog line. Take what comes off them before it goes."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Six names, cut into the bank stone where the fog sits thickest. It isn't a monument. It's a tally, which is what I was for."));
			}
		});

		// Quest 1004: The Chapel-Stones
		//---------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Chapel-Keeper Zenonas"), "f_katyn_12", 900, 1675, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1004);

			dialog.SetTitle(L("Zenonas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's pressing his palm flat to the chapel wall, the way he must have done ten thousand times before, and once more now for you*{/}"));
				await dialog.Msg(L("You've wandered into a dead man's chapel — mind the step. Chapel-Keeper Zenonas. The station had one because the Kingdom builds a chapel wherever it posts five people for more than a year. Four benediction stones, and I kept them warm."));
				await dialog.Msg(L("They're the only reason the Rodelin out there haven't come further in. Go to all 4 and put a hand on each - I need to know how many are still holding."));

				var response = await dialog.Select(L("Will you check them?"),
					Option(L("I'll read the 4 stones"), "help"),
					Option(L("Rodelin?"), "info"),
					Option(L("Not my business"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Warm means holding. Cold means drained but recoverable. If one is silent, tell me straight - don't soften it."));
						break;

					case "info":
						await dialog.Msg(L("The eleven settlements up the valley. They came down looking for the station and they are still coming down, and the stones are what turns them at the wall."));
						await dialog.Msg(L("I don't blame them. I would come looking too."));
						break;

					case "leave":
						await dialog.Msg(L("It becomes everyone's business the day the last stone finally goes silent, and not a day before."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readStones", out var stoneObj)) return;

				if (stoneObj.Done)
				{
					await dialog.Msg(L("Two warm, one cold, one silent. That's the crypt step, and I had a feeling it would be."));
					await dialog.Msg(L("Three will hold another decade. That's long enough for somebody who isn't dead to do something about it. Take this and tell Auksuolis when you see him."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More stones still unread. Palm flat, and give it a moment before you decide."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I've moved my watch to the crypt step. If it's the one that goes first, it's the one I should be standing on."));
			}
		});

		// Quest 1004 inspection points - the chapel benediction stones
		//---------------------------------------------------------------------
		void AddChapelStone(int stoneNumber, string stoneName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, stoneName, "f_katyn_12", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_12", 1004);
				var variableKey = $"Laima.Quests.f_katyn_12.Quest1004.Stone{stoneNumber}";
				var counterKey = "Laima.Quests.f_katyn_12.Quest1004.StonesChecked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A squat benediction stone set into the chapel wall*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already read this one*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Reading the stone..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var read = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, read);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Stones read: {0}/4", read));

					if (read >= 4)
						character.ServerMessage(L("{#FFD700}All four read. Return to Chapel-Keeper Zenonas.{/}"));
				}
				else
				{
					character.ServerMessage(L("You take your hand off the stone."));
				}
			});
		}

		AddChapelStone(1, L("Nave Stone"),
			L("Nave Stone: warm to the palm. The benediction is holding."), 1525, 1250, 0);
		AddChapelStone(2, L("Altar Stone"),
			L("Altar Stone: warm, and the warmest of the four."), 925, 1425, 90);
		AddChapelStone(3, L("West-Aisle Stone"),
			L("West-Aisle Stone: cold. Drained, but something is still in it."), 900, 875, 180);
		AddChapelStone(4, L("Crypt-Step Stone"),
			L("Crypt-Step Stone: silent. Nothing at all answers your hand."), 1250, 225, 270);

		// Quest 1005: What Came Down the Letas
		//---------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Warden Auksuolis"), "f_katyn_12", -361, -940, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1005);

			dialog.SetTitle(L("Auksuolis"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_katyn_12", 1004)))
				{
					await dialog.Msg(L("Zenonas has you reading the chapel stones. Finish that first - what he finds decides whether I ask you the next thing at all."));
					return;
				}

				await dialog.Msg(L("{#666666}*He goes very still at the news, the way the living go still before bad words instead of after*{/}"));
				await dialog.Msg(L("Silent, on the crypt step. Then I'll say the part I've been avoiding since you first found me. The water didn't kill everyone here. The water killed three of us. Something else came down the Letas for the other two."));
				await dialog.Msg(L("It's a Werewolf and it dens upstream past the Puragi ground. Kill 20 Puragi between here and the den and it will come down the bank to find out who's clearing its yard."));

				var response = await dialog.Select(L("Will you go upstream?"),
					Option(L("I'll take the Werewolf"), "help"),
					Option(L("Why avoid saying it?"), "info"),
					Option(L("That's beyond me"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It fights on the bank and it will try to put you in the water. Don't let it - once you're in, the footing is its and not yours."));
						break;

					case "info":
						await dialog.Msg(L("Because for eleven years the story I told myself was that I killed my station with a bad signature. That version has the advantage of being entirely my fault, which is oddly easier."));
						await dialog.Msg(L("The true version is that I got three of them killed and then failed to protect the last two. I'd rather the signature."));
						break;

					case "leave":
						await dialog.Msg(L("Then leave it be. It has waited up there eleven years. It can wait a twelfth."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearBank", out var bankObj)) return;
				if (!quest.TryGetProgress("killWerewolf", out var bossObj)) return;

				if (bankObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It's dead on the bank where it put Vilte's assistant. I've stood here eleven years looking at that stretch of gravel."));
					await dialog.Msg(L("Take everything in the strongbox. The station has no further need of a contingency fund."));

					character.Quests.Complete(questId);
				}
				else if (bankObj.Done)
				{
					await dialog.Msg(L("It's coming down the bank. Get to the water's edge before it works out you're not a Puragi."));
				}
				else
				{
					await dialog.Msg(L("Still too many Puragi between here and the den. It won't stir for a quiet bank."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Five of us at this station and I can account for all five now. That is not the same as resting, but it is the first honest ledger I've had since I died."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Iron in the Letas
//-----------------------------------------------------------------------------

public class IronInTheLetasQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1001);
		SetName(L("Iron in the Letas"));
		SetType(QuestType.Sub);
		SetDescription(L("Corrupt Chupacabra water at the Letas bend and whatever is in them goes into the stream. Clear the bend so nobody downstream drinks what the survey station drank."));
		SetLocation("f_katyn_12");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Warden Auksuolis"), "f_katyn_12");

		AddObjective("killChupacabra", L("Kill Corrupt Chupacabra at the stream bend"),
			new KillObjective(20, new[] { MonsterId.Chupacabra_Green }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1002 CLASS: The Undelivered Sample
//-----------------------------------------------------------------------------

public class TheUndeliveredSampleQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1002);
		SetName(L("The Undelivered Sample"));
		SetType(QuestType.Sub);
		SetDescription(L("Waterclerk Girenas drew a second Letas sample the day the fatal report went out and never got it to the station chemist. Carry it downstream to Vilte and bring him her verdict."));
		SetLocation("f_katyn_12");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Waterclerk Girenas"), "f_katyn_12");

		AddObjective("deliverSample", L("Take the sample to Chemist Vilte"),
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
		character.Inventory.Remove(661053, character.Inventory.CountItem(661053), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661053, character.Inventory.CountItem(661053), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: What the Fog Carries
//-----------------------------------------------------------------------------

public class WhatTheFogCarriesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1003);
		SetName(L("What the Fog Carries"));
		SetType(QuestType.Sub);
		SetDescription(L("Blue Operor drink the valley fog, and what comes off them when they die carries the names of eleven settlements the Kingdom stopped counting. Gather them for Mist-Reader Adomas."));
		SetLocation("f_katyn_12");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Mist-Reader Adomas"), "f_katyn_12");

		AddObjective("collectSpirits", L("Gather Fading Spirits from Blue Operor"),
			new CollectItemObjective(664096, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(664096, 0.50f, MonsterId.Operor_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: The Chapel-Stones
//-----------------------------------------------------------------------------

public class TheChapelStonesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1004);
		SetName(L("The Chapel-Stones"));
		SetType(QuestType.Sub);
		SetDescription(L("Four benediction stones are all that turns the Rodelin at the station wall, and Chapel-Keeper Zenonas cannot leave his post to check them. Read each stone and report which are still holding."));
		SetLocation("f_katyn_12");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Chapel-Keeper Zenonas"), "f_katyn_12");

		AddObjective("readStones", L("Read the four chapel benediction stones"),
			new VariableCheckObjective("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1004.StonesChecked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1004.StonesChecked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1004.Stone{i}");
	}
}

// Quest 1005 CLASS: What Came Down the Letas
//-----------------------------------------------------------------------------

public class WhatCameDownTheLetasQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1005);
		SetName(L("What Came Down the Letas"));
		SetType(QuestType.Sub);
		SetDescription(L("The bad water killed three of the station's five. A Werewolf denning upstream took the other two. Clear the Puragi off the bank to draw it down, then finish it."));
		SetLocation("f_katyn_12");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Restless Soul] Warden Auksuolis"), "f_katyn_12");

		AddPrerequisite(new CompletedPrerequisite("f_katyn_12", 1004));

		AddObjective("clearBank", L("Kill Puragi on the upstream bank"),
			new KillObjective(20, new[] { MonsterId.Puragi }));

		AddObjective("killWerewolf", L("Defeat the Werewolf"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Werewolf, 1) },
				resetIdent: "clearBank",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583112, 1)); // Kietas Necklace
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
