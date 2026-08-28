//--- Melia Script ----------------------------------------------------------
// Grynas Training Ground Quest NPCs
//--- Description -----------------------------------------------------------
// The Dievdirbys school, where carvers are taught to cut protective statues,
// and where the blackening coming down out of the hills is first measured.
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

public class FKatyn452QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Three Samples for the Blackening
		//---------------------------------------------------------------------
		AddNpc(156005, L("[Dievdirbys] Esol"), "f_katyn_45_2", -713, 1680, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1001);

			dialog.SetTitle(L("Esol"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's turning a split branch over in his hands, frowning at the cut face*{/}"));
				await dialog.Msg(L("You'll do. I need a pair of hands that isn't already exhausted from teaching — twenty-two years running this ground, sixty carvers taught, and this is the first summer the wood comes off the field black in the middle. Note that. First time. Twenty-two years."));
				await dialog.Msg(L("I need to know how far up the food chain it has gone. Bring me 4 Black Old Kepa Stems, 4 Red Puragi Hooks and 4 Blue Ridimed Leaves and I can chart it."));

				var response = await dialog.Select(L("Three species, three samples each. Will you collect them, or shall I write 'no assistant' into the report?"),
					Option(L("I'll bring all 3 sets"), "help"),
					Option(L("Black in the middle?"), "info"),
					Option(L("Ask your trainees"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the samples from things you kill on the field itself, not the edges. I want the middle of the ground, by the practice stumps. Edge samples skew the data."));
						break;

					case "info":
						await dialog.Msg(L("Cut a branch here and the outer two rings test pale, healthy. The heartwood is black as a burn. Dying inward-out — which, for the record, is not how anything is supposed to die."));
						await dialog.Msg(L("A statue carved from that wood holds a crystal for roughly a month before it splits. I've burned fourteen finished pieces this season. Fourteen. I counted."));
						break;

					case "leave":
						await dialog.Msg(L("My trainees are 15 and 16. I've already sent one of them out on this road once. I'd rather not repeat the experiment."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectStems", out var stemObj)) return;
				if (!quest.TryGetProgress("collectHooks", out var hookObj)) return;
				if (!quest.TryGetProgress("collectLeaves", out var leafObj)) return;

				if (stemObj.Done && hookObj.Done && leafObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lays the three sets out in a row and cuts one of each open*{/}"));
					await dialog.Msg(L("Black through the Kepa. Black through the Puragi. The Ridimed leaves are only edged — so it's moving upward, and it hasn't finished. Write that down somewhere, would you, in case I forget I said it."));
					await dialog.Msg(L("Take the school's fee. Reserved for guest carvers, and no guest carver's climbed this road in three years. Consider yourself the control group."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short on one of the three. All 4 of each, and from the middle of the field - the edges will read wrong."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I've pinned the chart in the workshop. Three lines on it. All three point at the hills past the ridge. I don't love a chart that agrees with itself that cleanly."));
			}
		});

		// Quest 1002: What the Stumps Are Doing
		//---------------------------------------------------------------------
		AddNpc(156005, L("[Dievdirbys] Rutalen"), "f_katyn_45_2", -20, 994, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1002);

			dialog.SetTitle(L("Rutalen"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He has one palm flat against a stump and doesn't move it, even as he looks up*{/}"));
				await dialog.Msg(L("Give me a moment. Please. I don't trust what I'm feeling right now and I don't want to lose count halfway through convincing myself I imagined it. Four practice stumps on this field — two hundred years of trainees cutting the same four, and I've put my own hands on every one of them."));
				await dialog.Msg(L("Esol wants his samples off dead animals. I want somebody else's hand on each of the 4 stumps, telling me what they feel — because I've stopped trusting mine, and that frightens me rather more than the wood does."));

				var response = await dialog.Select(L("Would you check them? Just — tell me if I'm imagining it. Please."),
					Option(L("I'll check all 4 stumps"), "help"),
					Option(L("Why not check them yourself?"), "info"),
					Option(L("Stumps are stumps"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Palm flat on the cut face and wait. A living stump is cool. A blackened one is warm and it will feel like it is warm for a reason."));
						break;

					case "info":
						await dialog.Msg(L("Because I've wanted them to be fine for six weeks straight, and a man who wants a particular answer is the worst possible instrument for finding the true one. I know that. Doesn't help."));
						await dialog.Msg(L("Esol taught me that lesson himself, on this very field, on one of these very stumps. I've been ignoring it since June, if you want the honest count."));
						break;

					case "leave":
						await dialog.Msg(L("These stumps have had the hands of every carver in the order on them. If they are not stumps any more, that matters more than the road does."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("checkStumps", out var stumpObj)) return;

				if (stumpObj.Done)
				{
					await dialog.Msg(L("All 4 warm. Not one cool. Six weeks I've been telling myself it was just the summer heat. Six weeks."));
					await dialog.Msg(L("Take my carving fee. I am not going to be taking work for a while and it will only sit in the box."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still stumps unchecked. Palm flat, and give it a slow count of 5 before you decide."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("We're cutting new practice stumps from the northern stand tomorrow. Two hundred years of hands on those four, and we're walking away from them in a single morning. I don't know how to feel about that, so I've decided not to."));
			}
		});

		// Quest 1002 inspection points - the practice stumps
		//---------------------------------------------------------------------
		void AddPracticeStump(int stumpNumber, string stumpName, string observation, int x, int z, int direction)
		{
			AddNpc(157008, stumpName, "f_katyn_45_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_2", 1002);
				var variableKey = $"Laima.Quests.f_katyn_45_2.Quest1002.Stump{stumpNumber}";
				var counterKey = "Laima.Quests.f_katyn_45_2.Quest1002.StumpsChecked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A practice stump, its cut face scarred by two centuries of trainees*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already checked this one*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Reading the cut face..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var checkedCount = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, checkedCount);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Stumps checked: {0}/4", checkedCount));

					if (checkedCount >= 4)
						character.ServerMessage(L("{#FFD700}All 4 stumps checked. Return to Dievdirbys Rutalen.{/}"));
				}
				else
				{
					character.ServerMessage(L("You take your hand off the stump."));
				}
			});
		}

		AddPracticeStump(1, L("Practice Stump"),
			L("First Stump: warm, and warmest at the heart of the cut."), -835, 1278, 0);
		AddPracticeStump(2, L("Practice Stump"),
			L("Second Stump: warm. A black ring shows two fingers in from the bark."), -393, 1642, 90);
		AddPracticeStump(3, L("Practice Stump"),
			L("Third Stump: warm, and the sap standing on the face runs dark."), -713, 1680, 180);
		AddPracticeStump(4, L("Practice Stump"),
			L("Fourth Stump: warm. Two hundred years of carved initials, and the newest ones are already blackening."), -950, 1750, 270);

		// Quest 1003: The Old Carving Knife
		//---------------------------------------------------------------------
		AddNpc(157005, L("[Trainee Carver] Lerid"), "f_katyn_45_2", -775, 1559, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1003);

			dialog.SetTitle(L("Lerid"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's pacing a tight circle in the grass, checking the same empty patch of ground over and over*{/}"));
				await dialog.Msg(L("You didn't happen to see a Kepa carrying a knife on your way in? No? Didn't think so. I put Rutalen's old carving knife down for one minute — one minute — and a Black Old Kepa took it. Not knocked it off. Took it. Walked off with it tucked under its arm like it had somewhere to be."));
				await dialog.Msg(L("That knife is 90 years old and it is not mine to lose. Kill 15 Black Old Kepa on the field, get it back, and let's — not mention the 'one minute' part to Rutalen."));

				var response = await dialog.Select(L("Will you find it? I'd rather not explain this twice."),
					Option(L("I'll hunt the Kepa and get the knife back"), "help"),
					Option(L("A Kepa picked something up?"), "info"),
					Option(L("Just tell him"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The one that has it will be slower than the rest and it will keep turning to face you. That's the whole tell and it's a good one."));
						break;

					case "info":
						await dialog.Msg(L("Old Kepas do not pick things up. Never have, not once, in the entire time this school's kept records. I've been on this field two years, and I watched one carry a knife like it knew what a knife was for."));
						await dialog.Msg(L("I told Esol. He wrote it down without a word, then sat very still for a long moment. That frightened me rather more than the Kepa did, if I'm honest."));
						break;

					case "leave":
						await dialog.Msg(L("I'm going to anyway. I'd simply rather walk back in there holding the knife than empty-handed, if it's all the same to you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killKepa", out var killObj)) return;
				if (!quest.TryGetProgress("findKnife", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He checks the edge against his thumbnail before anything else*{/}"));
					await dialog.Msg(L("Not a nick in it. Ninety years old, dragged around a field by a Kepa for a full day, and it's still true. Small mercies."));
					await dialog.Msg(L("Take everything in my kit box. I've a second knife and I'd rather owe you than owe Rutalen."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("You've thinned them right out and no knife. Keep going - the one carrying it doesn't run with the others."));
				}
				else
				{
					await dialog.Msg(L("Still Kepas out on the field. Look for the slow one."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I gave it back and told him the truth. He said a knife that gets stolen by a Kepa has a better story than most, and then he told me to sweep the workshop."));
			}
		});

		// Quest 1004: The Sculpture at the Broken Obelisk
		//---------------------------------------------------------------------
		AddNpc(157004, L("[Dievdirbys] Ajel"), "f_katyn_45_2", 1321, -398, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1004);
			var placedKey = "Laima.Quests.f_katyn_45_2.Quest1004.Placed";

			dialog.SetTitle(L("Ajel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's set a wrapped bundle down carefully in the grass, catching his breath from the climb*{/}"));
				await dialog.Msg(L("Ah — good, someone with legs that still work. Mine complained the whole climb up from the road, the moment the ring was lit. There's a broken obelisk on the eastern edge of this ground, oldest thing the order owns, older than my patience for scree slopes."));
				await dialog.Msg(L("I've carved a purifying sculpture to stand at its foot, but it needs charging. Bring me 5 Faintly Glowing Orbs off the Blue Ridimed and then set the sculpture at the obelisk yourself."));

				var response = await dialog.Select(L("Well? Will you charge it and set it, or shall I drag this bundle up the scree myself and give my knees something to really complain about?"),
					Option(L("I'll gather the orbs and set the sculpture"), "help"),
					Option(L("What broke the obelisk?"), "info"),
					Option(L("Set it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(668041, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("The orbs sit in the Ridimed's crown and go dark within a minute of the thing dying, so take them straight off. Carry the sculpture face-down until you set it — face-up, it just glowers at you the whole climb."));
						break;

					case "info":
						await dialog.Msg(L("Nobody knows. It was broken when the order arrived, and the order arrived four hundred years ago. Four centuries and we still can't read the writing on the standing half. Humbling, that."));
						await dialog.Msg(L("What I can tell you is that the blackening stops 40 paces short of it on every side. That is not nothing."));
						break;

					case "leave":
						await dialog.Msg(L("I would. I am 64 and the obelisk is up a scree slope and I have already had that argument with my knees this morning."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (character.Variables.Perm.GetBool(placedKey, false))
				{
					await dialog.Msg(L("It took? Good — didn't want to climb back up here to check. It'll hold that circle a year, and by then somebody should have gone up into the hills to find the source."));
					await dialog.Msg(L("Take the order's road purse - the second one. Esol will sign for it and grumble, which is how the order has always paid for anything."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Orbs first, then the obelisk. It's on the eastern edge, up the scree - you'll see the standing half from the field."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two rings lit on the road and one circle held at the obelisk. That is the most standing ground the order has had in a decade, and it is still not enough. Never is, in this line of work."));
			}
		});

		// Quest 1004 delivery point - the broken obelisk
		//---------------------------------------------------------------------
		AddNpc(147501, L("Broken Obelisk"), "f_katyn_45_2", 873, 5, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1004);
			var placedKey = "Laima.Quests.f_katyn_45_2.Quest1004.Placed";

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*Half an obelisk, snapped clean, with writing on it that nobody has read in 400 years*{/}"));
				return;
			}

			if (character.Variables.Perm.GetBool(placedKey, false))
			{
				await dialog.Msg(L("{#666666}*The purifying sculpture stands at the foot of the standing half, lit from inside*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("collectOrbs", out var orbObj)) return;

			if (!orbObj.Done)
			{
				await dialog.Msg(L("{#666666}*The sculpture is dead weight in your hands. It needs charging before it will do anything here*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(
				L("Setting the sculpture..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(5)
			);

			if (result == TimeActionResult.Completed)
			{
				character.Variables.Perm.Set(placedKey, true);
				character.Quests.CompleteObjective(questId, "placeSculpture");
				character.ServerMessage(L("{#FFD700}The sculpture takes the charge and lights. Return to Dievdirbys Ajel.{/}"));
			}
			else
			{
				character.ServerMessage(L("You lift the sculpture back onto your shoulder."));
			}
		});

		// Quest 1005: Esol's Verdict
		//---------------------------------------------------------------------
		AddNpc(156005, L("[Dievdirbys] Esol"), "f_katyn_45_2", -650, 1740, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1005);

			dialog.SetTitle(L("Esol"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_katyn_45_2", 1001))
					|| !character.Quests.HasCompleted(new QuestId("f_katyn_45_2", 1002))
					|| !character.Quests.HasCompleted(new QuestId("f_katyn_45_2", 1003))
					|| !character.Quests.HasCompleted(new QuestId("f_katyn_45_2", 1004)))
				{
					await dialog.Msg(L("I have samples, stump readings, a stolen knife and an obelisk to account for. Finish all 4 and then I will put them together in front of you."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's spread every report out on the workbench and hasn't looked up from them in a while*{/}"));
				await dialog.Msg(L("Good, you're back. I'd started talking to the reports instead of an actual person, which is a bad sign in a man my age. Three sample lines pointing at the ridge. Four warm stumps. A Kepa that carried a knife like it had been told to. A circle at the obelisk the blackening will not cross."));
				await dialog.Msg(L("I want to say what it adds up to out loud, with somebody standing there who has actually walked the ground. Once I write it down, the order has to act on it. Stay while I say it."));

				var response = await dialog.Select(L("Sit. Stand. I don't care which. Will you hear the verdict, or not?"),
					Option(L("I'll hear your verdict"), "help"),
					Option(L("You already know what it says"), "info"),
					Option(L("Write it and send it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Quests.CompleteObjective(questId, "hearVerdict");
						await dialog.Msg(L("It's not a sickness in the wood. Something in the hills past this ridge is calling, and the wood and the animals are both answering it. Simple, once you stop looking for a simpler answer."));
						await dialog.Msg(L("Four hundred years of carving on this field, and we've been treating the symptom the entire time. Go up into the hills. Whatever's there has been there longer than we have. Longer than the order, certainly."));
						break;

					case "info":
						await dialog.Msg(L("I know what the evidence says. Knowing and signing your name under it are different acts and only one of them sends trainees into the hills."));
						break;

					case "leave":
						await dialog.Msg(L("I have written 3 drafts. Every one of them ends with the same sentence and I keep burning them at that sentence."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("Take the school's whole reserve. If I am right about the hills there will not be a school here to spend it."));

				character.Quests.Complete(questId);
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The report went to Fedimian this morning under my name and Rutalen's. Ajel has gone up the ridge ahead of the reply, which is exactly what I would have done at his age and exactly what I told him not to do."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Three Samples for the Blackening
//-----------------------------------------------------------------------------

public class ThreeSamplesForTheBlackeningQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1001);
		SetName(L("Three Samples for the Blackening"));
		SetType(QuestType.Sub);
		SetDescription(L("Wood cut on the training field comes off black at the heart and pale at the bark, which is not how anything dies. The school's master needs samples from three species to chart how far the blackening has climbed."));
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Dievdirbys] Esol"), "f_katyn_45_2");

		AddObjective("collectStems", L("Collect Black Old Kepa Stems"),
			new CollectItemObjective(668032, 4));

		AddObjective("collectHooks", L("Collect Red Puragi Hooks"),
			new CollectItemObjective(668033, 4));

		AddObjective("collectLeaves", L("Collect Blue Ridimed Leaves"),
			new CollectItemObjective(668034, 4));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(668032, 0.45f, MonsterId.Pappus_Kepa_Purple);
		AddDrop(668033, 0.45f, MonsterId.Puragi_Red);
		AddDrop(668034, 0.45f, MonsterId.Ridimed_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668032, character.Inventory.CountItem(668032), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(668033, character.Inventory.CountItem(668033), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(668034, character.Inventory.CountItem(668034), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668032, character.Inventory.CountItem(668032), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(668033, character.Inventory.CountItem(668033), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(668034, character.Inventory.CountItem(668034), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: What the Stumps Are Doing
//-----------------------------------------------------------------------------

public class WhatTheStumpsAreDoingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1002);
		SetName(L("What the Stumps Are Doing"));
		SetType(QuestType.Sub);
		SetDescription(L("Trainees have cut the same 4 practice stumps for 200 years. The carver who owns the field has wanted them to be fine for 6 weeks and no longer trusts his own hands on them. Read all 4."));
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Dievdirbys] Rutalen"), "f_katyn_45_2");

		AddObjective("checkStumps", L("Read the 4 practice stumps on the training field"),
			new VariableCheckObjective("Laima.Quests.f_katyn_45_2.Quest1002.StumpsChecked", 4, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1002.StumpsChecked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1002.Stump{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1002.StumpsChecked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1002.Stump{i}");
	}
}

// Quest 1003 CLASS: The Old Carving Knife
//-----------------------------------------------------------------------------

public class TheOldCarvingKnifeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1003);
		SetName(L("The Old Carving Knife"));
		SetType(QuestType.Sub);
		SetDescription(L("A Black Old Kepa picked up a 90-year-old carving knife and walked off with it, which is not something an Old Kepa has ever done. Kill them on the field and bring the knife back to the trainee who lost it."));
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Trainee Carver] Lerid"), "f_katyn_45_2");

		AddObjective("killKepa", L("Kill Black Old Kepa on the training field"),
			new KillObjective(15, new[] { MonsterId.Pappus_Kepa_Purple }));

		AddObjective("findKnife", L("Recover the Old Carving Knife"),
			new CollectItemObjective(668036, 1));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(668036, 0.20f, MonsterId.Pappus_Kepa_Purple);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668036, character.Inventory.CountItem(668036), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668036, character.Inventory.CountItem(668036), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: The Sculpture at the Broken Obelisk
//-----------------------------------------------------------------------------

public class TheSculptureAtTheBrokenObeliskQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1004);
		SetName(L("The Sculpture at the Broken Obelisk"));
		SetType(QuestType.Sub);
		SetDescription(L("The blackening stops 40 paces short of the broken obelisk on every side. Charge a purifying sculpture with orbs taken from Blue Ridimed and set it at the obelisk's foot to hold that circle."));
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Dievdirbys] Ajel"), "f_katyn_45_2");

		AddObjective("collectOrbs", L("Collect Faintly Glowing Orbs from Blue Ridimed"),
			new CollectItemObjective(668040, 5));

		AddObjective("placeSculpture", L("Set the purifying sculpture at the broken obelisk"),
			new ManualObjective());

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(668040, 0.45f, MonsterId.Ridimed_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668040, character.Inventory.CountItem(668040), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(668041, character.Inventory.CountItem(668041), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1004.Placed");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668040, character.Inventory.CountItem(668040), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(668041, character.Inventory.CountItem(668041), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1004.Placed");
	}
}

// Quest 1005 CLASS: Esol's Verdict
//-----------------------------------------------------------------------------

public class EsolsVerdictQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1005);
		SetName(L("Esol's Verdict"));
		SetType(QuestType.Sub);
		SetDescription(L("Samples, stump readings, a stolen knife and a circle the blackening will not cross. The school's master wants to say what they add up to out loud, in front of someone who has walked the ground."));
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Dievdirbys] Esol"), "f_katyn_45_2");

		AddPrerequisite(new CompletedPrerequisite("f_katyn_45_2", 1001));
		AddPrerequisite(new CompletedPrerequisite("f_katyn_45_2", 1002));
		AddPrerequisite(new CompletedPrerequisite("f_katyn_45_2", 1003));
		AddPrerequisite(new CompletedPrerequisite("f_katyn_45_2", 1004));

		AddObjective("hearVerdict", L("Hear Dievdirbys Esol's verdict"),
			new ManualObjective());

		AddReward(new ExpReward(16000, 11000));
		AddReward(new SilverReward(20000));
		AddReward(new ItemReward(640084, 3)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}
}
