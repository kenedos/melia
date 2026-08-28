//--- Melia Script ----------------------------------------------------------
// Overlong Bridge Valley Quest NPCs
//--- Description -----------------------------------------------------------
// The second course of the Great King's seal: three stone pillars above the
// valley, and the supply road that keeps the canyon's postings alive.
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

public class FRokas26QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Four Hundred Paces of Bridge
		//---------------------------------------------------------------------
		AddNpc(20142, L("[Supply Captain] Bio"), "f_rokas_26", 42, -1171, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1001);

			dialog.SetTitle(L("Bio"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's counting supply crates against a manifest, tapping each one twice with his knuckle*{/}"));
				await dialog.Msg(L("You crossed the bridge, then — good, tell me it held steady. Everything that reaches the canyon's 3 postings crosses this bridge, and this bridge is 400 paces long with no cover on any of them. That is the whole of my professional life."));
				await dialog.Msg(L("The Wendigos have moved onto the approach and they bed down on the apynys, which is the plant my dressings are made of. Kill 25 of them and bring me 8 of the apynys back out of the nests."));

				var response = await dialog.Select(L("Will you take the approach?"),
					Option(L("I'll clear it and bring 8 apynys"), "help"),
					Option(L("Why not fortify the bridge?"), "info"),
					Option(L("Move the supply road"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the ones off the bridge first. On the approach they can back up. On the bridge they cannot, and neither can you."));
						break;

					case "info":
						await dialog.Msg(L("I have applied 4 times. Every application comes back approved in principle and unfunded, which is a sentence I have learned to read as no."));
						await dialog.Msg(L("The expedition is 90 years old and its bridge has no gate on either end. Somebody wants this valley crossable."));
						break;

					case "leave":
						await dialog.Msg(L("There is one bridge. The valley is 600 feet deep and 400 paces across and the alternative is 2 days round by the ridge with a cart."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killWendigo", out var killObj)) return;
				if (!quest.TryGetProgress("collectApynys", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He shakes each bundle out and picks the bedding straw off it before it goes in the chest*{/}"));
					await dialog.Msg(L("8 clean. That is dressings for the whole corps and 2 postings besides, and the approach is walkable for the first time since spring."));
					await dialog.Msg(L("Take it out of the corps chest. We are funded for casualties and I would rather spend it on not having any."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Approach is clear. The apynys is in the nests, not on the open ground - they pull it in."));
				}
				else
				{
					await dialog.Msg(L("Still on the approach. Clear them before you go rooting in nests, unless you want the owner back mid-root."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("3 carts across in 2 days and nobody carried back. I have written that in the corps book with the date, because it will not happen often."));
			}
		});

		// Quest 1002: Two Knives Left
		//---------------------------------------------------------------------
		AddNpc(20016, L("[Mercenary] Lint"), "f_rokas_26", 923, -749, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1002);

			dialog.SetTitle(L("Lint"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's sitting on a rock in his undershirt, turning two knives over in his hands like they're all he has left*{/}"));
				await dialog.Msg(L("Don't. Just — don't laugh, I already know exactly how this looks. I came off the east shelf with 2 knives and the shirt on my back. Wendigo Archers took my whole kit off the ledge while I was 30 feet below it, hanging off a root like an absolute fool."));
				await dialog.Msg(L("And they haven't even eaten it — they're wearing it! Bring me back 10 pieces of my own supplies, would you, before I have to write the company a very embarrassing letter."));

				var response = await dialog.Select(L("Will you get his kit back?"),
					Option(L("I'll bring you 10 pieces"), "help"),
					Option(L("How did they get above you?"), "info"),
					Option(L("Buy a new issue"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They shoot down the shelf and never across it. Get onto their level and an archer is just a spindly thing with a stick."));
						break;

					case "info":
						await dialog.Msg(L("They did not. They were already there. I climbed up into a shelf that has been theirs since before I took this contract and I have had 3 weeks to think about that."));
						await dialog.Msg(L("Eta says I am a fool. Eta is on the west side where the Dumaro cannot climb, so Eta can afford to be right."));
						break;

					case "leave":
						await dialog.Msg(L("A full issue is 4 months' pay. I would sooner go up that shelf again with the 2 knives."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectSupplies", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He counts it out on the rock and finds his own scratched initials on the second-last piece*{/}"));
					await dialog.Msg(L("All 10, and 9 of them still serviceable. I am going to sew the tenth back together out of spite."));
					await dialog.Msg(L("Take my winter money. I would rather be cold and equipped than warm and writing letters to a quartermaster."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough. Work the upper shelf where they roost - the ones down on the valley floor are carrying nothing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Kitted out and back on the shelf, and this time I went up the west end where they cannot see the ledge. 3 weeks to think of that."));
			}
		});

		// Quest 1003: The Dumaro Count
		//---------------------------------------------------------------------
		AddNpc(20016, L("[Mercenary] Eta"), "f_rokas_26", -406, -312, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1003);

			dialog.SetTitle(L("Eta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She doesn't look away from the slope, just holds up a hand for quiet, lips still moving on a count*{/}"));
				await dialog.Msg(L("212. Don't interrupt, I'm writing it down. I count. Not a hobby — a discipline, and it's the only reason command still reads my reports. Spring: 40 Dumaro on the west slope. Last week: 212. You don't need me to say what that means."));
				await dialog.Msg(L("They're not breeding. They're arriving, from up the valley, every single time. Kill 30. I need a number Fedimian can't ignore."));

				var response = await dialog.Select(L("Will you cut the slope down?"),
					Option(L("I'll kill 30 Dumaro"), "help"),
					Option(L("Arriving from where?"), "info"),
					Option(L("Nobody counts Dumaro"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come in a mass and turn as a mass. Stand where 2 slopes meet and they can only reach you 3 at a time."));
						break;

					case "info":
						await dialog.Msg(L("Up. Past the pillars, out of the head of the valley. I have watched the same direction 6 evenings in a row and it has never once been down."));
						await dialog.Msg(L("Lint thinks I am safe over here. I am on the side everything is walking towards."));
						break;

					case "leave":
						await dialog.Msg(L("Nobody counted them in spring either, which is why 212 is only a number I have and not a number anyone acts on."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDumaro", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("182 and dropping. I have the spring figure, the week's figure and today's, and 3 numbers is an argument where 1 number is a mood."));
					await dialog.Msg(L("Take the scouting fee. I draw it for reporting and this is the first report I have had that anyone will read twice."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still too many. Work the open west slope, not the scrub - in the scrub you will never know what you have and have not counted."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The count went to the bridge and Bio sent it up the canyon. And I stood here at dusk and counted 9 more coming down past the pillars while I was writing it."));
			}
		});

		// Quest 1004: Three Pillars, Three Hundred Characters
		//---------------------------------------------------------------------
		AddNpc(20139, L("[Epigrapher] Deltran"), "f_rokas_26", -1024, 1910, 264, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1004);

			dialog.SetTitle(L("Deltran"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's on his knees at the base of a pillar, cupping a handful of grey dust like it's something precious*{/}"));
				await dialog.Msg(L("Look — look at this! An entire evening's reading, and it just came off in my palm, just like that! Three pillars stand at the head of this valley, each one carrying some 300 characters, and I have been copying them for 5 years and I am barely two thirds through the first!"));
				await dialog.Msg(L("The surfaces are going, do you understand? Powdering, not weathering — it comes off in the hand like chalk dust! Go to all 3, tell me the state of every face, before I lose text that no living person has read yet!"));

				var response = await dialog.Select(L("Will you check the pillars?"),
					Option(L("I'll check all 3 pillars"), "help"),
					Option(L("What does the text say?"), "info"),
					Option(L("Copy faster"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Do not brush them. Look, and tell me what you see. A brush on a powdering face takes off 40 years of somebody's work in one stroke."));
						break;

					case "info":
						await dialog.Msg(L("The first pillar is a list of names and offices, which is dull, and then a line that is not dull at all: 'the second course holds while the three stand.'"));
						await dialog.Msg(L("Course of what? Nobody at this expedition will tell me. I have asked the recorder twice and been given a cup of tea twice."));
						break;

					case "leave":
						await dialog.Msg(L("5 years for two thirds of one pillar. Copy faster and you copy wrong, and a wrong copy of a thing that no longer exists is worse than nothing at all."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("checkPillars", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He writes down all 3 reports and then sits back and looks at the head of the valley for a while*{/}"));
					await dialog.Msg(L("All 3 powdering from the base up and the third worst of all. That is not weather. Weather takes a face from the top."));
					await dialog.Msg(L("Take my copying money. And I am done asking the recorder politely - 'the second course holds while the three stand' and all 3 are coming apart from underneath."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 3. They stand across the head of the valley - west rim, middle terrace, and the far one on the north lip."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Morkus Jonas read my 3 reports without a word and then asked me how long I thought the third pillar had. I said 2 seasons. He said 1, and he did not say it like a guess."));
			}
		});

		// Quest 1004 collection points - the stone pillars
		//---------------------------------------------------------------------
		void AddStonePillar(int pillarNumber, string observation, int x, int z, int direction)
		{
			AddNpc(47106, L("Stone Pillar"), "f_rokas_26", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_rokas_26", 1004);
				var variableKey = $"Laima.Quests.f_rokas_26.Quest1004.Pillar{pillarNumber}";
				var counterKey = "Laima.Quests.f_rokas_26.Quest1004.PillarsChecked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A carved stone pillar standing across the head of the valley*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already looked this one over*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Looking over the pillar face..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var checkedCount = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, checkedCount);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Pillars checked: {0}/3", checkedCount));

					if (checkedCount >= 3)
						character.ServerMessage(L("{#FFD700}All 3 pillars checked. Return to Deltran.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the pillar face alone."));
				}
			});
		}

		AddStonePillar(1,
			L("West Rim Pillar: the face is sound to head height and powdering below it. A hand's depth of grey dust lies round the base."), -1504, 232, 0);
		AddStonePillar(2,
			L("Middle Terrace Pillar: powdering to shoulder height, and the names in the lower list can no longer be read at all."), -1360, 1180, 0);
		AddStonePillar(3,
			L("North Lip Pillar: gone above the waist. What is left carries one legible line - 'and the third stands last'."), -144, 1497, 0);

		// Quest 1005: The Second Course
		//---------------------------------------------------------------------
		AddNpc(147405, L("[Recorder] Morkus Jonas"), "f_rokas_26", -340, -1440, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1005);

			dialog.SetTitle(L("Morkus Jonas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_rokas_26", 1004)))
				{
					await dialog.Msg(L("{#666666}*He's staring at a letter in his hand, folding and unfolding the same crease*{/}"));
					await dialog.Msg(L("Let Deltran have his 3 reports first. He has asked me the same question for 5 years and he has earned the right to ask it once with evidence."));
					return;
				}

				await dialog.Msg(L("{#666666}*He sets the letter down flat on the table, smoothing it like it might still change if he stares at it long enough*{/}"));
				await dialog.Msg(L("My brother sent word from the gateway. Line 9 says dark. I have been waiting 5 months for that sentence and I still had to sit down when it came."));
				await dialog.Msg(L("The pillars are the second course and they are being eaten from the base. Not weathered - fed on. Kill 20 Wendigo Magicians at the valley head, then put down the thing they are feeding. Carry the fragment while you go."));

				var response = await dialog.Select(L("Will you go to the valley head?"),
					Option(L("I'll put down what's feeding"), "help"),
					Option(L("What is the second course for?"), "info"),
					Option(L("Tell Deltran the truth first"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650393, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Hold the fragment where you can see it. When the pillar stone is close to something that eats pillar stone, the fragment goes warm. That is the only warning anyone gets."));
						break;

					case "info":
						await dialog.Msg(L("There are 3 courses. An outer gate at the gateway, this pillar line, and 3 barriers on Akmens Ridge over the tomb mouth itself. My family is the paperwork that keeps all 3 unexamined."));
						await dialog.Msg(L("The gate is open. If the pillars go, the only thing left between the Great King and the Fedimian road is 3 barriers and my nephew Rolandas, who is 29."));
						break;

					case "leave":
						await dialog.Msg(L("I will tell him. 5 years of tea and I will tell him tonight, and he is going to be furious in the quietest possible way."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var mageObj)) return;
				if (!quest.TryGetProgress("killFeeder", out var bossObj)) return;

				if (mageObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("{#666666}*He takes the fragment back and finds it cool, and lets out a breath he has plainly been holding since the spring*{/}"));
					await dialog.Msg(L("Cold. The second course stops losing stone tonight, and the 2 pillars that still have faces will still have them next year."));
					await dialog.Msg(L("Take the shield. It came off the valley head and it is Kingdom work, 400 years old, and no Kingdom soldier has been posted in this valley in my lifetime. Go up to Akmens Ridge and find Rolandas. He has the third course and he does not know the gate is open."));

					character.Quests.Complete(questId);
				}
				else if (mageObj.Done)
				{
					await dialog.Msg(L("The head is clear of magicians. What they were keeping fed is still down there and the fragment will tell you when you are close."));
				}
				else
				{
					await dialog.Msg(L("Too many magicians at the head. They are the reason it can feed in daylight - take them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Deltran has been up at the third pillar since dawn copying with both hands. He has not spoken to me. He has, however, taken my lantern, which I am choosing to read as forgiveness."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Four Hundred Paces of Bridge
//-----------------------------------------------------------------------------

public class FourHundredPacesOfBridgeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1001);
		SetName(L("Four Hundred Paces of Bridge"));
		SetType(QuestType.Sub);
		SetDescription(L("Everything that reaches the canyon's 3 postings crosses one 400-pace bridge with no cover on it. The Wendigos have moved onto the approach and bed down on the apynys the corps makes its dressings from."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Supply Captain] Bio"), "f_rokas_26");

		AddObjective("killWendigo", L("Kill Wendigos on the bridge approach"),
			new KillObjective(25, new[] { MonsterId.Wendigo }));

		AddObjective("collectApynys", L("Recover Canyon Apynys from the nests"),
			new CollectItemObjective(650389, 8));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(650389, 0.40f, MonsterId.Wendigo);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650389, character.Inventory.CountItem(650389), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650389, character.Inventory.CountItem(650389), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Two Knives Left
//-----------------------------------------------------------------------------

public class TwoKnivesLeftQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1002);
		SetName(L("Two Knives Left"));
		SetType(QuestType.Sub);
		SetDescription(L("The Wendigo Archers took a mercenary's full issue off the east shelf while he hung 30 feet below it. They are not eating it - they are wearing it."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mercenary] Lint"), "f_rokas_26");

		AddObjective("collectSupplies", L("Recover Lindt's Supplies from the Wendigo Archers"),
			new CollectItemObjective(650388, 10));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion

		AddDrop(650388, 0.45f, MonsterId.Wendigo_Bow);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650388, character.Inventory.CountItem(650388), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650388, character.Inventory.CountItem(650388), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Dumaro Count
//-----------------------------------------------------------------------------

public class TheDumaroCountQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1003);
		SetName(L("The Dumaro Count"));
		SetType(QuestType.Sub);
		SetDescription(L("A mercenary who counts things counted 40 Dumaro on the west slope in spring and 212 last week. They are not breeding - they are arriving, and always from up the valley."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mercenary] Eta"), "f_rokas_26");

		AddObjective("killDumaro", L("Kill Dumaro on the west slope"),
			new KillObjective(30, new[] { MonsterId.Dumaro }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Three Pillars, Three Hundred Characters
//-----------------------------------------------------------------------------

public class ThreePillarsThreeHundredCharactersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1004);
		SetName(L("Three Pillars, Three Hundred Characters"));
		SetType(QuestType.Sub);
		SetDescription(L("3 carved pillars stand across the head of the valley and their faces are powdering away from the base up. The epigrapher has copied two thirds of one in 5 years."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Epigrapher] Deltran"), "f_rokas_26");

		AddObjective("checkPillars", L("Check the faces of all 3 stone pillars"),
			new VariableCheckObjective("Laima.Quests.f_rokas_26.Quest1004.PillarsChecked", 3, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_26.Quest1004.PillarsChecked");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_26.Quest1004.Pillar{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_rokas_26.Quest1004.PillarsChecked");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_rokas_26.Quest1004.Pillar{i}");
	}
}

// Quest 1005 CLASS: The Second Course
//-----------------------------------------------------------------------------

public class TheSecondCourseQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1005);
		SetName(L("The Second Course"));
		SetType(QuestType.Sub);
		SetDescription(L("The pillars are the second course of the Great King's seal and their stone is not weathering - something is feeding on it from the base up. Clear the valley head and put down what the Wendigo Magicians have been keeping fed."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Recorder] Morkus Jonas"), "f_rokas_26");

		AddPrerequisite(new CompletedPrerequisite("f_rokas_26", 1004));

		AddObjective("killMages", L("Kill Wendigo Magicians at the valley head"),
			new KillObjective(20, new[] { MonsterId.Wendigo_Mage }));

		AddObjective("killFeeder", L("Put down the Denoptic feeding on the pillars"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Denoptic, 1) },
				resetIdent: "killMages",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(39000, 27000));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(223101, 1)); // Wall Guard
		AddReward(new ItemReward(640085, 3)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650393, character.Inventory.CountItem(650393), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650393, character.Inventory.CountItem(650393), InventoryItemRemoveMsg.Destroyed);
	}
}
