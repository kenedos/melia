//--- Melia Script ----------------------------------------------------------
// South Parias Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Kupole Yulia's ward ring in the south, the three Kugheri clans it holds
// out, and the woman who came up the road with unbinding scrolls.
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

public class FMaple242QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Kugheri at the East Ward
		//---------------------------------------------------------------------
		AddNpc(154012, L("[Kupole] Yulia"), "f_maple_24_2", 1286, 1075, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1001);

			dialog.SetTitle(L("Yulia"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's stopped dead on the path, staring at something you can't quite see through the trees ahead*{/}"));
				await dialog.Msg(L("Don't come any closer yet - actually, no, come here, I could use a second pair of eyes. I walk 9 wards in this southern forest and have walked them every day for a hundred years. The eastern ward has Kugheri Zabbi standing on it."));
				await dialog.Msg(L("Standing on it, not passing it. They have never been able to do that. Kill 30 of them and let me get close enough to see what has changed."));

				var response = await dialog.Select(L("Will you clear the east ward?"),
					Option(L("I'll kill the Kugheri Zabbi"), "help"),
					Option(L("What do the wards do?"), "info"),
					Option(L("Walk a different ward"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They fight in threes and the third one hangs back to circle. Kill the two in front fast or you will spend the afternoon turning around."));
						break;

					case "info":
						await dialog.Msg(L("They hold the three Kugheri clans out of the central grove. Not out of the forest - out of the grove, where my sister keeps the seed."));
						await dialog.Msg(L("For a hundred years a Kugheri could not put a foot on a ward stone. This week 30 of them are sitting on one."));
						break;

					case "leave":
						await dialog.Msg(L("There are 8 other wards and I have walked all of them this morning. This is the one that is wrong."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killZabbi", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("I got to the stone. The binding on the east face has been lifted - not broken, lifted, the way you take a lid off a pot."));
					await dialog.Msg(L("Take this. It is the least useful thing I own and it is the only thing I own."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still Zabbi on the ward. I cannot read a stone with 30 Widling sitting on it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Lifted, and lifted carefully. Whoever did it wanted the ward to survive. That frightens me considerably more than a smashed stone would."));
			}
		});

		// Quest 1002: Natural Energy
		//---------------------------------------------------------------------
		AddNpc(154011, L("[Kupole] Ilona"), "f_maple_24_2", 514, 465, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1002);

			dialog.SetTitle(L("Ilona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's standing at an unfamiliar crossing, looking distinctly out of place away from her own grove*{/}"));
				await dialog.Msg(L("Oh - thank goodness, a person. I came down from the central grove because my sister sent for me, and Yulia does not send for me. In a hundred years she has sent for me twice."));
				await dialog.Msg(L("A lifted ward has to be re-set and re-setting takes natural energy. The Kugheri Numani carry it in their chest orbs. Bring me 10 of them."));

				var response = await dialog.Select(L("Will you gather the orbs?"),
					Option(L("I'll bring you 10 orbs"), "help"),
					Option(L("Why does she never send for you?"), "info"),
					Option(L("Take energy from the grove instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The orb sits behind the ribs and it holds its charge about a day. Ten in one afternoon is better than twelve over two."));
						break;

					case "info":
						await dialog.Msg(L("Because for a hundred years she has believed the seed should never have been sealed, and I have believed she is wrong, and neither of us wanted to have that conversation."));
						await dialog.Msg(L("So she walks 9 wards and I sit with a seed and we write each other polite notes at midwinter."));
						break;

					case "leave":
						await dialog.Msg(L("The grove's energy is what is holding the seed asleep. Spending it on a ward is robbing a house to pay for its own lock."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectOrbs", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She holds two orbs together and they hum against each other*{/}"));
					await dialog.Msg(L("Ten, all charged. That is the east ward re-set and enough left over for whichever one goes next, and one will go next."));
					await dialog.Msg(L("Take this from the grove stone. I keep telling people the grove has nothing and then handing them things off it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. The Numani hold to the low ground in the middle of the forest, where the ferns are thickest."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The east ward is re-set. Yulia thanked me, in words, out loud. I have had a strange sort of day."));
			}
		});

		// Quest 1003: Red Banterer Essence
		//---------------------------------------------------------------------
		AddNpc(154013, L("[Kupole] Astra"), "f_maple_24_2", -1356, -608, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1003);

			dialog.SetTitle(L("Astra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's kneeling over a bed of unopened flowers, close enough to be talking to them rather than you*{/}"));
				await dialog.Msg(L("...all three of us in the same forest on the same day. That's happened 4 times in a hundred years and none of the other 3 were good. Sorry - didn't mean to think out loud at a stranger."));
				await dialog.Msg(L("I read the forest by colour and I need a red I cannot get in the north. Kill 20 Kugheri Zeuni and bring me 8 lots of Red Banterer Essence out of them."));

				var response = await dialog.Select(L("Will you get the essence?"),
					Option(L("I'll hunt the Zeuni and bring the essence"), "help"),
					Option(L("You read the forest by colour?"), "info"),
					Option(L("What were the other three days?"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The essence goes brown within the hour, so keep it in the dark. My pouch is lined for it - use that and not your own."));
						break;

					case "info":
						await dialog.Msg(L("I keep three flower beds in the northern forest, one yellow, one red, one white. What the forest is doing shows in which bed opens and which does not."));
						await dialog.Msg(L("My red bed has not opened this year at all. I need a true red to test against and there is no true red left in the north to test with."));
						break;

					case "leave":
						await dialog.Msg(L("The tree dying. The seed being sealed. A man coming up this road in my grandmother's time asking for it politely."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killZeuni", out var killObj)) return;
				if (!quest.TryGetProgress("collectEssence", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She holds a vial against her palm and compares it to nothing you can see*{/}"));
					await dialog.Msg(L("That is a true red and my northern bed is 3 shades off it. The north has been going pale for a year and I have been calling it a dry summer."));
					await dialog.Msg(L("Take this. It has been in my pouch since the man came up the road and I have never wanted to look at it."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty of Zeuni down and not enough essence. Not every one of them carries it - the older the redder."));
				}
				else
				{
					await dialog.Msg(L("Still Zeuni in the western hollows. They keep to the deep shade, which is where the essence stays true."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three shades. I have written it down and I will not be the sister who kept a bad reading to herself for a century."));
			}
		});

		// Quest 1004: The Unbinding Scrolls
		//---------------------------------------------------------------------
		AddNpc(154102, L("[Wanderer] Neringa"), "f_maple_24_2", -1349, -556, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1004);

			dialog.SetTitle(L("Neringa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She startles at your footsteps, then forces her shoulders back down from around her ears*{/}"));
				await dialog.Msg(L("Sorry - sorry, you just gave me a fright. I came up this road 3 weeks ago carrying 9 unbinding scrolls, one for each ward, and I've been telling everyone I'm a pilgrim."));
				await dialog.Msg(L("The Kugheri have taken 6 of them off me and scattered them. Get those 6 back and I will tell the Kupole exactly what I came here to do, in front of you, before I lose the nerve."));

				var response = await dialog.Select(L("Will you recover the scrolls?"),
					Option(L("I'll bring back the 6 scrolls"), "help"),
					Option(L("What did you come here to do?"), "info"),
					Option(L("Tell them first"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They will be in Kugheri hands - the clans take paper for nesting. A scroll that has been chewed is still binding, so bring me the pieces."));
						break;

					case "info":
						await dialog.Msg(L("My family sealed that seed. My great-grandmother wrote the scroll that is on it and she wrote it in a hurry and she wrote it wrong."));
						await dialog.Msg(L("The seal is not holding the seed asleep. It is holding it half-awake, and a hundred years of half-awake is what has been pulling the small things of this forest toward it."));
						break;

					case "leave":
						await dialog.Msg(L("With 6 of my scrolls loose in the forest and 3 in my bag? They would hear 'I have come to unbind your wards' and nothing after it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectScrolls", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She counts them, then counts them again, and then puts all 9 face-down on the ground*{/}"));
					await dialog.Msg(L("Nine. Right. I am going to go and find Yulia and say the sentence, and you are going to stand there and make it impossible for me to stop halfway."));
					await dialog.Msg(L("Take this first. If it goes badly I would rather it was already yours."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still scrolls out there. Every one of them is a ward somebody could lift by accident, so I would rather have them all."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I said it. Yulia did not shout. Ilona did, once, and then stopped and asked me to say the part about the wrong hand again."));
			}
		});

		// Quest 1005: What Neringa Came For
		//---------------------------------------------------------------------
		AddNpc(154012, L("[Kupole] Yulia"), "f_maple_24_2", -1278, 731, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1005);

			dialog.SetTitle(L("Yulia"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_maple_24_2", 1004)))
				{
					await dialog.Msg(L("There is a woman on the southern road with something to say to me and she has not said it yet. Go and get it said."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's standing very still at the ward stone, both hands pressed flat against it*{/}"));
				await dialog.Msg(L("A hundred years of walking wards and the answer was a woman with a bag of paper, apologising on her great-grandmother's behalf. I am not angry. I am something worse and I have no word for it."));
				await dialog.Msg(L("We reseal it properly tonight, with her hand and mine both on it. The Kugheri Numani will come the moment the old seal comes off - kill 25 of them first, then hold the ward while we work."));

				var response = await dialog.Select(L("Will you hold the ward?"),
					Option(L("I'll clear the Numani and hold the ward"), "help"),
					Option(L("You trust her?"), "info"),
					Option(L("Reseal it without her"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("What comes at the end will be clan leaders, not scouts. They will not break off and they will not go round. Stand on the stone and do not step off it."));
						break;

					case "info":
						await dialog.Msg(L("I trust that she walked 3 weeks up a road to admit her family broke something. That is a great deal more than most people manage and considerably more than I have."));
						await dialog.Msg(L("I have believed the sealing was wrong for a century and I have never once written to my sister to say so."));
						break;

					case "leave":
						await dialog.Msg(L("Her great-grandmother's hand wrote it. It will not come off cleanly for anyone else and we would be back here in ten years with a worse problem."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearWard", out var wardObj)) return;
				if (!quest.TryGetProgress("holdWard", out var holdObj)) return;

				if (wardObj.Done && holdObj.Done)
				{
					await dialog.Msg(L("Sealed, and sealed asleep this time. The pull stopped the moment the new scroll went down - I felt the whole southern forest let go at once."));
					await dialog.Msg(L("Take this. It was left at the southern ward 60 years ago by somebody who never came back for it, and I have finally worked out that it was never mine."));

					character.Quests.Complete(questId);
				}
				else if (wardObj.Done)
				{
					await dialog.Msg(L("The ward is clear and the old seal is coming off. Get on the stone."));
				}
				else
				{
					await dialog.Msg(L("Too many Numani still in the low ground. We cannot lift a seal with the clans that close."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Astra has gone north to look at her red bed again. She thinks it will open now. I hope she is right and I have stopped predicting."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Kugheri at the East Ward
//-----------------------------------------------------------------------------

public class KugheriAtTheEastWardQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1001);
		SetName(L("Kugheri at the East Ward"));
		SetType(QuestType.Sub);
		SetDescription(L("For a hundred years no Kugheri could put a foot on a ward stone. This week 30 Kugheri Zabbi are sitting on the eastern one. Clear them off so the ward can be read."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kupole] Yulia"), "f_maple_24_2");

		AddObjective("killZabbi", L("Kill Kugheri Zabbi at the eastern ward"),
			new KillObjective(30, new[] { MonsterId.Kucarry_Zabbi }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1002 CLASS: Natural Energy
//-----------------------------------------------------------------------------

public class NaturalEnergyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1002);
		SetName(L("Natural Energy"));
		SetType(QuestType.Sub);
		SetDescription(L("A lifted ward has to be re-set, and re-setting takes natural energy the Kupole cannot spare from the grove. Kugheri Numani carry it in their chest orbs."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kupole] Ilona"), "f_maple_24_2");

		AddObjective("collectOrbs", L("Collect Orbs filled with Natural Energy from Kugheri Numani"),
			new CollectItemObjective(667245, 10));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(667245, 0.50f, MonsterId.Kucarry_Numani);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667245, character.Inventory.CountItem(667245), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667245, character.Inventory.CountItem(667245), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Red Banterer Essence
//-----------------------------------------------------------------------------

public class RedBantererEssenceQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1003);
		SetName(L("Red Banterer Essence"));
		SetType(QuestType.Sub);
		SetDescription(L("Kupole Astra reads the forest by colour, and her northern red bed has not opened at all this year. She needs a true red from the southern hollows to test against."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kupole] Astra"), "f_maple_24_2");

		AddObjective("killZeuni", L("Kill Kugheri Zeuni in the western hollows"),
			new KillObjective(20, new[] { MonsterId.Kucarry_Zeuni }));

		AddObjective("collectEssence", L("Collect Red Banterer Essence"),
			new CollectItemObjective(667246, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(667246, 0.45f, MonsterId.Kucarry_Zeuni);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667246, character.Inventory.CountItem(667246), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667246, character.Inventory.CountItem(667246), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: The Unbinding Scrolls
//-----------------------------------------------------------------------------

public class TheUnbindingScrollsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1004);
		SetName(L("The Unbinding Scrolls"));
		SetType(QuestType.Sub);
		SetDescription(L("A woman walked up the southern road with 9 unbinding scrolls and a story she has not told anyone yet. The Kugheri clans have scattered 6 of them. Get them back before a ward comes off by accident."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Wanderer] Neringa"), "f_maple_24_2");

		AddObjective("collectScrolls", L("Recover the scattered Unbinding Scrolls"),
			new CollectItemObjective(667244, 6));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(667244, 0.20f, MonsterId.Kucarry_Zabbi);
		AddDrop(667244, 0.20f, MonsterId.Kucarry_Numani);
		AddDrop(667244, 0.20f, MonsterId.Kucarry_Zeuni);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667244, character.Inventory.CountItem(667244), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667244, character.Inventory.CountItem(667244), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: What Neringa Came For
//-----------------------------------------------------------------------------

public class WhatNeringaCameForQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1005);
		SetName(L("What Neringa Came For"));
		SetType(QuestType.Sub);
		SetDescription(L("The seal on the Divine Tree's seed was written in a hurry and written wrong, and a hundred years of half-awake is what has been pulling this forest toward it. Hold the southern ward while it is done properly."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Kupole] Yulia"), "f_maple_24_2");

		AddPrerequisite(new CompletedPrerequisite("f_maple_24_2", 1004));

		AddObjective("clearWard", L("Kill Kugheri Numani around the southern ward"),
			new KillObjective(25, new[] { MonsterId.Kucarry_Numani }));

		AddObjective("holdWard", L("Hold the ward while the seal is rewritten"),
			new LayeredKillObjective(
				spawnList: new[] {
					new KillSpec(MonsterId.Kucarry_Zeuni, 1, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Kucarry_Zabbi, 1, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Kucarry_Numani, 1, BuffId.EliteMonsterBuff),
				},
				resetIdent: "clearWard",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(583104, 1)); // Saphie Necklace
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}
}
