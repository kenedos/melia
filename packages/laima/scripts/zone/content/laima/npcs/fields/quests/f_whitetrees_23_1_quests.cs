//--- Melia Script ----------------------------------------------------------
// Nobreer Approach - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_whitetrees_23_1 map. A refugee camp at a
// bonfire, and nine thorn flowers nobody planted by accident.
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

public class FWhitetrees231QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Thatch Before the Rain
		// =====================================================================
		// Refugee Brandon - a camp with no roofs
		//---------------------------------------------------------------------
		AddNpc(20155, L("[Refugee] Brandon"), "f_whitetrees_23_1", 1240, 1496, 257, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1001);

			dialog.SetTitle(L("Brandon"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A man is lashing a lean-to frame together out of green wood, with nothing at all to put over it*{/}"));
				await dialog.Msg(L("You're not one of ours — travelling through, then? Sit if you like, though we haven't much to offer besides the fire."));
				await dialog.Msg(L("We came out of the Pistis forest 3 weeks ago - 31 of us, because two lords have been arguing over that wood for 11 years and neither of them has ever once argued about us. I have 9 frames up and no thatch. Kill 30 Kugheri Lyoni and bring me 8 bundles of the whitetailed grass they bed in."));

				var response = await dialog.Select(L("Will you get the camp its thatch?"),
					Option(L("I'll clear them and cut grass"), "help"),
					Option(L("Why not go back?"), "info"),
					Option(L("Nine frames for 31 people?"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The grass is under them, in the beds, and it is the only long dry grass on this whole side of the ridge."));
						await dialog.Msg(L("Lyoni are not much on their own. They arrive though. Do not sit down in a bed to cut, cut standing."));
						break;

					case "info":
						await dialog.Msg(L("Because a schoolmistress in Pistis found a chest of deeds that says neither lord ever owned the forest, and now there are two very angry households and 31 of us who would rather not be standing between them when it gets decided."));
						await dialog.Msg(L("She gave the chest to us, not to them. That is a wonderful thing to be given and it does not keep rain off."));
						break;

					case "leave":
						await dialog.Msg(L("9 frames is what I have wood for. People will sleep 4 to a frame and the rest will sleep at the fire, which is what we did last night and the night before."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLyoni", out var killObj)) return;
				if (!quest.TryGetProgress("collectGrass", out var grassObj)) return;

				if (killObj.Done && grassObj.Done)
				{
					await dialog.Msg(L("{#666666}*He gets the first bundle onto a frame and lashed down before he says anything, and then does the second one too*{/}"));
					await dialog.Msg(L("8 bundles is 6 roofs. 6 roofs is everybody under something except me and I have slept outside for 3 weeks and I have got used to it."));
					await dialog.Msg(L("Take this. It is the camp's, it was collected in a hat, and 31 people all put something in it, so do not tell me it is too much."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Ground's clear enough. 8 bundles out of the beds and cut standing."));
				}
				else
				{
					await dialog.Msg(L("30 Lyoni first. They will keep arriving while you cut otherwise."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("6 roofs and it rained on the second night and not one of them leaked. Vilhelmas came up and stood looking at them and then went away and came back with 2 more frames' worth of wood."));
			}
		});

		// =====================================================================
		// QUEST 1002: Something Is Wrong with the Sacks
		// =====================================================================
		// Snarer Dovile - poison that is too strong
		//---------------------------------------------------------------------
		AddNpc(20116, L("[Snarer] Dovile"), "f_whitetrees_23_1", 1451, 432, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1002);

			dialog.SetTitle(L("Dovile"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A snarer is holding a cut poison sac at arm's length on the end of a stick and frowning at how full it is*{/}"));
				await dialog.Msg(L("Don't come too close to this one, just in case. Good timing though — I need someone with steady hands and no attachment to their fingers."));
				await dialog.Msg(L("I have snared Kugheri on this ridge 16 years and I know what a Sommi's poison sac looks like. This is 3 times the size and the poison in it is clear instead of yellow. Bring me 6 more so I know whether it is this one animal or all of them."));

				var response = await dialog.Select(L("Will you bring me 6 sacs?"),
					Option(L("I'll bring 6"), "help"),
					Option(L("Does it matter what colour it is?"), "info"),
					Option(L("Snare something else"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut behind the sac, not through it. Through it and you get the poison on the blade and then on your hand and then you find out how strong it is the interesting way."));
						await dialog.Msg(L("Sommi work the east and the south. Take them one at a time - a stimulated one is faster than you expect and I would like to be wrong about that."));
						break;

					case "info":
						await dialog.Msg(L("It matters because clear means concentrated. A yellow sac makes a rabbit sleep. I put a drop of this on a snared hare and it stopped breathing before I had straightened up."));
						await dialog.Msg(L("Brandon's camp is 3 weeks old and it has children in it and it is 400 paces from Sommi ground."));
						break;

					case "leave":
						await dialog.Msg(L("I do not snare for sport. I snare because 31 people at a fire need meat and there is nothing else on this ridge to catch."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectSacs", out var sacObj)) return;

				if (sacObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lines all 6 up on a board on the ends of 6 sticks and does not touch any of them*{/}"));
					await dialog.Msg(L("All 6. Every one oversized and every one clear. It is not one animal. It is the whole population and it has happened since midsummer."));
					await dialog.Msg(L("Take the snare money. And keep off the wet ground southwest of here until somebody has worked out what they have been eating, because I have a guess and I do not like it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("East and south, one at a time, cut behind the sac. 6 of them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("6 sacs on a board and Gintare has taken all of them. She went very quiet when she saw the colour, which from a herbalist is not a comfort."));
			}
		});

		// =====================================================================
		// QUEST 1003: Between the Camp and the Pass
		// =====================================================================
		// Trailwatch Aurimas - the west road
		//---------------------------------------------------------------------
		AddNpc(20161, L("[Trailwatch] Aurimas"), "f_whitetrees_23_1", -1135, -454, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1003);

			dialog.SetTitle(L("Aurimas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A trailwatch is sitting on a rock at a junction with a stick across his knees and a very clear view of two paths at once*{/}"));
				await dialog.Msg(L("Ah — good, someone with legs that haven't been walked ragged. Sit a moment if you want, though I imagine you'd rather hear why I'm sitting here at all."));
				await dialog.Msg(L("I watch the junction. West goes to the refuge, north goes to the camp, and between them is a stretch full of Kugheri Tot that nobody has been able to carry a load through in a month. Kill 25 of them."));

				var response = await dialog.Select(L("Will you clear the junction stretch?"),
					Option(L("I'll clear the Tot"), "help"),
					Option(L("What needs carrying?"), "info"),
					Option(L("Carry it the long way"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Tot go for the legs and they go in threes and the third one comes from behind. Put your back to a trunk and you have turned a bad fight into an easy one."));
						await dialog.Msg(L("Work west from the junction. They thin out after 300 paces and there is no point going further."));
						break;

					case "info":
						await dialog.Msg(L("Everything. Tekel Refuge has grain and Brandon's camp has 31 people and 9 frames. The refuge would send it. Nobody can walk it."));
						await dialog.Msg(L("I have carried 3 loads through myself in a month and I am 52 and I ran the last one and I am not proud of any of it."));
						break;

					case "leave":
						await dialog.Msg(L("The long way is round the ridge and over the pass and it is 2 days with a load. People at a fire in autumn do not have 2 days."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTot", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks 300 paces west and back at his own pace, without running any of it*{/}"));
					await dialog.Msg(L("Open. I will have a grain load up from the refuge by tomorrow evening and Brandon will not have to say no to anybody for a week."));
					await dialog.Msg(L("Take the watch fee. It is small and it is honest and it is the only kind I have ever drawn."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("West of the junction, 300 paces. Back to a trunk. 25 of them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("4 loads through in 6 days and 1 of them was blankets. I sat on this rock and counted them past and it is the best week I have had at this junction."));
			}
		});

		// =====================================================================
		// QUEST 1004: Nine Plants in a Ring
		// =====================================================================
		// Herbalist Gintare - the thorn flowers
		//---------------------------------------------------------------------
		AddNpc(147418, L("[Herbalist] Gintare"), "f_whitetrees_23_1", -590, 1159, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1004);

			dialog.SetTitle(L("Gintare"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A herbalist has 6 poison sacs on a board in front of her and has not touched any of them for an hour*{/}"));
				await dialog.Msg(L("Sorry — I heard you walk up and didn't look away from these. They've earned the stare. You'll do for a second opinion, if you don't mind an unpleasant one."));
				await dialog.Msg(L("Clear poison in an oversized sac means an animal eating something it did not evolve to eat. There is a yellow thorn flower growing on the southwest ground that has no business on this ridge at all. Go and look at 4 of them for me."));

				var response = await dialog.Select(L("Will you look at the thorn flowers?"),
					Option(L("I'll look at 4 of them"), "help"),
					Option(L("What is a thorn flower doing here?"), "info"),
					Option(L("Burn them all"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(666132, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the stimulant sample and hold it against each plant - if it is the same thing the sample goes cloudy. And look at the ground round the base, not just the flower."));
						await dialog.Msg(L("Do not pick any of them. If I am right about what these are, whoever put them there counts them."));
						break;

					case "info":
						await dialog.Msg(L("That is the question. It is a lowland plant. It wants heat and it wants tending and it is growing on a cold ridge in autumn and it is thriving."));
						await dialog.Msg(L("I have been a herbalist 20 years. Plants do not turn up. They are brought, or they spread from somewhere they were brought to."));
						break;

					case "leave":
						await dialog.Msg(L("Burn 9 stands of an unknown poisonous plant upwind of a camp with children in it? I would like to know what it is first, and I would like to know why there are 9."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var looked = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_1.Quest1004.Looked", 0);

				if (looked >= 4)
				{
					await dialog.Msg(L("{#666666}*She plots the 4 positions on a scrap of bark and then keeps turning the bark round, which does not help*{/}"));
					await dialog.Msg(L("Even spacing. All 4, to within 20 paces, and the ground at each base has been turned and it has been turned with a tool."));
					await dialog.Msg(L("That is not a plant spreading. That is a crop, planted in a ring, on ground nobody owns, 400 paces from a refugee camp. Take this and go to Vilhelmas - he has hunted this ridge 30 years and he is the only person who might know when they went in."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Southwest ground. Hold the sample to each one and look at the base. {0} of 4 looked at. Do not pick any.", looked));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("9 plants, even spacing, turned earth at every base. I have drawn it 4 times now hoping to get a different shape and it is a ring every time."));
			}
		});

		// =====================================================================
		// THORN FLOWER STANDS
		// =====================================================================
		// For Quest 1004 - Nine Plants in a Ring
		// =====================================================================

		void AddThornStand(int standNumber, string standName, string finding, int x, int z, int direction)
		{
			AddNpc(155146, L(standName), "f_whitetrees_23_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_23_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A stand of yellow thorn flower, thriving on cold ground in autumn where it has no business growing at all*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_23_1.Quest1004.Stand{standNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have this one's position and the state of its ground*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Testing the stand..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Testing interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);

				var looked = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_1.Quest1004.Looked", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_whitetrees_23_1.Quest1004.Looked", looked);

				character.ServerMessage(L(finding));
				character.ServerMessage(LF("Stands examined: {0}/4", looked));

				if (looked >= 4)
					character.ServerMessage(L("{#FFD700}All 4 stands examined. Return to Gintare.{/}"));
			});
		}

		AddThornStand(1, "First Thorn Stand", "Sample goes cloudy at once. Earth at the base turned, and turned with a blade.", -1402, -58, 347);
		AddThornStand(2, "Second Thorn Stand", "Cloudy. Same distance from the first as the first is from the path.", -1311, -398, 314);
		AddThornStand(3, "Third Thorn Stand", "Cloudy. Kugheri tracks all round it, in and out, packed hard.", -1066, 12, 274);
		AddThornStand(4, "Fourth Thorn Stand", "Cloudy. Even spacing again. Nothing about this is where a seed would land.", -760, 220, 0);

		// =====================================================================
		// The Camp Fire - atmosphere at Brandon's camp
		//---------------------------------------------------------------------
		AddNpc(154060, L("Camp Fire"), "f_whitetrees_23_1", -1002, 1448, 0, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A fire kept going day and night in a ring of stones, with more people sitting round it than there are frames behind it*{/}"));
			await dialog.Msg(L("{#666666}*Somebody has scratched a tally into the flattest stone. It goes to 31 and then there is a second mark beside it, added later, and the second mark is 33*{/}"));
		});

		// =====================================================================
		// QUEST 1005: Somebody Counts Them
		// =====================================================================
		// Vilhelmas - thirty years on the ridge
		//---------------------------------------------------------------------
		AddNpc(155142, L("[Hunter] Vilhelmas"), "f_whitetrees_23_1", -969, 1472, 297, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_1", 1005);

			dialog.SetTitle(L("Vilhelmas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_whitetrees_23_1", 1004)))
				{
					await dialog.Msg(L("{#666666}*A hunter doesn't turn his head, just his eyes, tracking you the last few steps in*{/}"));
					await dialog.Msg(L("You walk quiet for someone I didn't hear coming. Gintare has 4 plants to look at and a sample to hold against them — go and do that first. I have hunted this ridge 30 years and I would rather answer her question than guess at it."));
					return;
				}

				await dialog.Msg(L("{#666666}*He sets down the knife he was whittling with, unhurried, like he's been waiting for exactly this conversation*{/}"));
				await dialog.Msg(L("A ring. Even spacing, turned earth. Then I can tell you when: they went in over 2 nights at the end of last spring, because I walked that southwest ground on a Tuesday and it was bare and I walked it on a Thursday and it was not."));
				await dialog.Msg(L("Whoever did it comes back. The Kugheri Sommi hold the southwest now and they hold it hard - kill 25 and take the pair that have made a den at the middle of the ring, because whatever visits those plants has to get past them and something has been getting past them."));

				var response = await dialog.Select(L("Will you go into the ring?"),
					Option(L("I'll take the den at the middle"), "help"),
					Option(L("Two nights?"), "info"),
					Option(L("Move the camp instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(666134, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the paralysis draught. It is theirs, off a stimulated sac, and it works on them. One coating on a blade and a Sommi stops, which is the only advantage anybody has ever had over one."));
						await dialog.Msg(L("Clear the ground before the den. A stimulated Sommi is fast and a stimulated pair in a den is a thing I would not walk into for money."));
						break;

					case "info":
						await dialog.Msg(L("2 nights, 9 plants, in a ring 600 paces across, on ground that takes 4 hours to walk. That is not one person with a bag of seed."));
						await dialog.Msg(L("30 years I have had this ridge to myself. In 30 years the only people who have come up here are lost, hunting, or running. Whoever did this was none of the three and they knew exactly where they were going."));
						break;

					case "leave":
						await dialog.Msg(L("Move it where? East is the pass, north is the refuge and the refuge is full, west is the ring. The camp is where it is because it is the last piece of ground nobody wanted."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearRing", out var ringObj)) return;
				if (!quest.TryGetProgress("takeTheDen", out var denObj)) return;

				if (ringObj.Done && denObj.Done)
				{
					await dialog.Msg(L("{#666666}*He goes into the den himself with a torch and comes back out holding a folded square of oiled canvas*{/}"));
					await dialog.Msg(L("A seed roll. 9 pockets, 3 of them still full, and a sewn tally on the flap in a proper hand - 9 in, 9 checked, and a date from 4 days ago."));
					await dialog.Msg(L("Somebody walked this ring 4 days ago and counted their crop while Brandon's camp slept 400 paces away. Take my old bow-brace. And tell Gintare not to burn the plants yet, because whoever comes to count them will come again, and I intend to be sitting in that ring when they do."));

					character.Quests.Complete(questId);
				}
				else if (ringObj.Done)
				{
					await dialog.Msg(L("Southwest ground's thin. The pair are still in the den at the middle and they have not come out for it."));
				}
				else
				{
					await dialog.Msg(L("25 first. Nobody goes into that den with the ring still full behind them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("9 plants still standing and 1 hunter sitting 40 paces off them every night since. Nothing yet. I have waited longer than this for a deer and been less certain of getting one."));
				await dialog.Msg(L("Brandon has put a frame up for me at the fire. I have not slept under a roof in 30 years and I am not going to start, but I look at it every evening on my way out."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Thatch Before the Rain
//-----------------------------------------------------------------------------

public class ThatchBeforeTheRainQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1001);
		SetName(L("Thatch Before the Rain"));
		SetType(QuestType.Sub);
		SetDescription(L("Thirty-one people came out of the Pistis forest 3 weeks ago and Brandon has 9 frames up with nothing to put over them. The Kugheri Lyoni bed in the only long dry grass on the ridge."));
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Refugee] Brandon"), "f_whitetrees_23_1");

		AddObjective("killLyoni", L("Kill Kugheri Lyoni around the camp ground"),
			new KillObjective(30, new[] { MonsterId.Kucarry_Lioni }));

		AddObjective("collectGrass", L("Cut bundles of whitetailed grass"),
			new CollectItemObjective(666131, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(666131, 0.35f, MonsterId.Kucarry_Lioni);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666131, character.Inventory.CountItem(666131), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666131, character.Inventory.CountItem(666131), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Something Is Wrong with the Sacks
//-----------------------------------------------------------------------------

public class SomethingIsWrongWithTheSacksQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1002);
		SetName(L("Something Is Wrong with the Sacks"));
		SetType(QuestType.Sub);
		SetDescription(L("Dovile has snared Kugheri on this ridge for 16 years and a Sommi poison sac has never been oversized and clear before. She needs 6 more to know whether it is one animal or all of them."));
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Snarer] Dovile"), "f_whitetrees_23_1");

		AddObjective("collectSacs", L("Cut poison sacs from Kugheri Sommi"),
			new CollectItemObjective(666133, 6));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion

		AddDrop(666133, 0.30f, MonsterId.Kucarry_Somy);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666133, character.Inventory.CountItem(666133), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666133, character.Inventory.CountItem(666133), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Between the Camp and the Pass
//-----------------------------------------------------------------------------

public class BetweenTheCampAndThePassQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1003);
		SetName(L("Between the Camp and the Pass"));
		SetType(QuestType.Sub);
		SetDescription(L("Tekel Refuge has grain and Brandon's camp has 31 people, and the stretch of trail between them is full of Kugheri Tot that nobody has carried a load through in a month."));
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Trailwatch] Aurimas"), "f_whitetrees_23_1");

		AddObjective("killTot", L("Kill Kugheri Tot west of the junction"),
			new KillObjective(25, new[] { MonsterId.Kucarry_Tot }));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1004 CLASS: Nine Plants in a Ring
//-----------------------------------------------------------------------------

public class NinePlantsInARingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1004);
		SetName(L("Nine Plants in a Ring"));
		SetType(QuestType.Sub);
		SetDescription(L("A lowland thorn flower is thriving on a cold ridge in autumn and the Kugheri that eat it carry three times the poison they should. Test 4 of the stands and look at the ground around each base."));
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Gintare"), "f_whitetrees_23_1");

		AddObjective("examineStands", L("Examine 4 thorn flower stands"),
			new VariableCheckObjective("Laima.Quests.f_whitetrees_23_1.Quest1004.Looked", 4, true));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666132, character.Inventory.CountItem(666132), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_1.Quest1004.Looked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_1.Quest1004.Stand{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666132, character.Inventory.CountItem(666132), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_1.Quest1004.Looked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_1.Quest1004.Stand{i}");
	}
}

// Quest 1005 CLASS: Somebody Counts Them
//-----------------------------------------------------------------------------

public class SomebodyCountsThemQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_1", 1005);
		SetName(L("Somebody Counts Them"));
		SetType(QuestType.Sub);
		SetDescription(L("Nine plants went into a 600-pace ring over two nights at the end of last spring, on ground Vilhelmas has had to himself for 30 years. The Kugheri Sommi have made a den at the middle of it."));
		SetLocation("f_whitetrees_23_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Hunter] Vilhelmas"), "f_whitetrees_23_1");

		AddPrerequisite(new CompletedPrerequisite("f_whitetrees_23_1", 1004));

		AddObjective("clearRing", L("Kill Kugheri Sommi holding the southwest ground"),
			new KillObjective(25, new[] { MonsterId.Kucarry_Somy }));

		AddObjective("takeTheDen", L("Take the pair denned at the middle of the ring"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Kucarry_Somy, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Kucarry_Tot, 3),
				},
				resetIdent: "clearRing",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(603106, 1)); // Abomination Bracelet
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666134, character.Inventory.CountItem(666134), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666134, character.Inventory.CountItem(666134), InventoryItemRemoveMsg.Destroyed);
	}
}
