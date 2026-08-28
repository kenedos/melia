//--- Melia Script ----------------------------------------------------------
// North Parias Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Kupole Astra's three flower beds, which are how the north of Parias Forest
// tells anyone who can read colour what is actually wrong with it.
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

public class FMaple243QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: What the Beds Say
		//---------------------------------------------------------------------
		AddNpc(154013, L("[Kupole] Astra"), "f_maple_24_3", 639, 199, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1001);

			dialog.SetTitle(L("Astra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's standing over a flower bed with her back to you, arms crossed, refusing to look down at it*{/}"));
				await dialog.Msg(L("You'll forgive me if I don't turn around straight away. I have kept 3 flower beds in this forest for a hundred years - one yellow, one red, one white. They are not decoration. They are the instrument."));
				await dialog.Msg(L("This year I have been reading them badly on purpose, because I did not like the reading. Go to all 3 and look at them yourself, and tell me what you see."));

				var response = await dialog.Select(L("Will you read the beds?"),
					Option(L("I'll look at all 3 beds"), "help"),
					Option(L("How does a flower bed measure anything?"), "info"),
					Option(L("Trust your own eyes"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Count what has opened, not what is alive. A bed can be perfectly healthy and closed, and a closed bed is the whole message."));
						break;

					case "info":
						await dialog.Msg(L("The yellow opens to the forest's water, the red to its blood, the white to whatever is under it. Three questions, three answers, once a year, for a hundred years."));
						await dialog.Msg(L("My sisters walk wards and sit with a seed. I look at flowers. They have been polite about it for a century and I have noticed every single time."));
						break;

					case "leave":
						await dialog.Msg(L("My own eyes have spent a year telling me it was a dry summer. That is exactly the problem with my own eyes."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readBeds", out var bedObj)) return;

				if (bedObj.Done)
				{
					await dialog.Msg(L("Yellow open, red shut, white open and turned the wrong way. Say it in that order and it is a sentence, and the sentence is about something under the ground."));
					await dialog.Msg(L("Take this. It is the fee my sisters send me at midwinter and I have never once spent it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Beds still unread. All 3 - the reading is the pattern between them, not any one of them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have written to both my sisters. In a hundred years I have written to them 4 times and 2 of those were to say the beds were fine."));
			}
		});

		// Quest 1001 interaction points - the three flower beds
		//---------------------------------------------------------------------
		void AddFlowerBed(int bedNumber, int model, string bedName, string observation, int x, int z, int direction)
		{
			AddNpc(model, bedName, "f_maple_24_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_maple_24_3", 1001);
				var variableKey = $"Laima.Quests.f_maple_24_3.Quest1001.Bed{bedNumber}";
				var counterKey = "Laima.Quests.f_maple_24_3.Quest1001.BedsRead";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A planted bed, laid out in careful rows*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already counted this bed*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Counting what has opened..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var read = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, read);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Beds read: {0}/3", read));

					if (read >= 3)
						character.ServerMessage(L("{#FFD700}All 3 beds read. Return to Kupole Astra.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the bed uncounted."));
				}
			});
		}

		AddFlowerBed(1, 47246, L("Yellow Bed"),
			L("Yellow Bed: open, every row of it. The forest's water is sound."), -636, 413, 90);
		AddFlowerBed(2, 47247, L("Red Bed"),
			L("Red Bed: not one bloom open. The buds are formed and they have simply refused."), -740, 177, 180);
		AddFlowerBed(3, 152017, L("White Bed"),
			L("White Bed: open, and every flower in it has turned to face the same point downhill."), -922, -84, 270);

		// Quest 1002: A True Yellow, a True Red, a True White
		//---------------------------------------------------------------------
		AddNpc(154013, L("[Kupole] Astra"), "f_maple_24_3", -63, 1047, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1002);

			dialog.SetTitle(L("Astra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's pacing a short line between two beds, muttering the same phrase over and over*{/}"));
				await dialog.Msg(L("Good, you're back - talking to myself was getting me nowhere. A closed red bed is either a sick bed or a lying instrument, and I cannot tell which without something to compare it to."));
				await dialog.Msg(L("The wild flowers still grow on the creatures out here. Bring me 4 Yellow off the Cloverin, 4 Red off the Fragolin and 4 White off the Blueberrin."));

				var response = await dialog.Select(L("Will you gather all 3 colours?"),
					Option(L("I'll bring 4 of each colour"), "help"),
					Option(L("Flowers grow on them?"), "info"),
					Option(L("Replant the red bed instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them whole, with the stem. A flower cut short lies about its colour within the hour and I have been lied to enough this year."));
						break;

					case "info":
						await dialog.Msg(L("On them, in them, out of them - the small things of this forest and the flowers of this forest have never been fully separate and that is not a defect."));
						await dialog.Msg(L("A Cloverin is closer to a meadow than to an animal. So is a Fragolin. So, honestly, am I."));
						break;

					case "leave":
						await dialog.Msg(L("Replant it and I have a new bed with no hundred years behind it. The value of the instrument is that it is old."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectYellow", out var yObj)) return;
				if (!quest.TryGetProgress("collectRed", out var rObj)) return;
				if (!quest.TryGetProgress("collectWhite", out var wObj)) return;

				if (yObj.Done && rObj.Done && wObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays the three sets side by side and looks at them for a long time without saying anything*{/}"));
					await dialog.Msg(L("The wild red is true. My bed is not sick and my instrument is not lying - the bed is being told not to open. That is a third possibility I had not allowed for."));
					await dialog.Msg(L("Take this. It was in the white bed's soil when I planted it and I have never known what it was for."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short of a colour. All three, and 4 of each - one flower proves nothing at all."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Being told not to open. I have said that sentence out loud about 40 times now and it has not improved."));
			}
		});

		// Quest 1003: Fragolin in the Fern
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Dye-Maker] Ona"), "f_maple_24_3", -1363, 546, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1003);

			dialog.SetTitle(L("Ona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's got a dye-stained apron on and a basket that's noticeably lighter than it should be*{/}"));
				await dialog.Msg(L("Mind the fern edge, it's crawling today. I make dye out of this forest and sell it in Klaipeda, and the nymphs let me because I've never taken more than the wood grows back."));
				await dialog.Msg(L("The Fragolin have come up out of the western fern in numbers I have never seen and they eat everything I dye with. Kill 30 of them or I am out of a trade by autumn."));

				var response = await dialog.Select(L("Will you go into the fern?"),
					Option(L("I'll kill the Fragolin"), "help"),
					Option(L("Numbers you've never seen?"), "info"),
					Option(L("Buy your dyestuff instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They're small and there are a great many of them, so don't let them ring you. Keep moving through and they'll never get behind you."));
						break;

					case "info":
						await dialog.Msg(L("Twenty years on this ground. A good Fragolin year is 3 or 4 in a stand of fern. I counted 60 in one stand last Tuesday and gave up counting."));
						await dialog.Msg(L("And they're all headed the same way. Downhill, east, steady as a cart. Whatever's calling them isn't calling me and I'm glad of it."));
						break;

					case "leave":
						await dialog.Msg(L("Buy it from who? I am the person other people buy it from. That's the whole trade."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFragolin", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The western fern's standing again and I got a full basket off it this morning. First full basket since the spring."));
					await dialog.Msg(L("Take the season's first money. It's bad luck to spend it and I've never believed in luck."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still Fragolin in the fern. They're thickest where the ground drops away toward the west."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Kupole Astra came and asked me which direction they'd been walking. I told her east and downhill and she went white and thanked me."));
			}
		});

		// Quest 1004: Transparent Crystal
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Crystal-Cutter] Vidas"), "f_maple_24_3", -732, -1155, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1004);

			dialog.SetTitle(L("Vidas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's got a loupe screwed into one eye, examining a shard of something up to the light*{/}"));
				await dialog.Msg(L("One moment - almost got this one. There. I cut lens crystal. Not gems - lenses, for people who need to look at something small and be sure of what they saw."));
				await dialog.Msg(L("The Blueberrin here grow a transparent crystal in the gut and it is better glass than anything I can quarry. Kill 20 of them and bring me 8 crystals."));

				var response = await dialog.Select(L("Will you get the crystals?"),
					Option(L("I'll hunt the Blueberrin and bring 8 crystals"), "help"),
					Option(L("Who needs a lens out here?"), "info"),
					Option(L("Quarry it properly"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Do not crack the body open with anything heavy. A cracked crystal is a paperweight and I already own several."));
						break;

					case "info":
						await dialog.Msg(L("The nymph with the flower beds ordered 3 last spring and has ordered 6 more since. She will not say why and I have stopped asking."));
						await dialog.Msg(L("She looks at seeds through them. That is all I know and it is more than I am supposed to."));
						break;

					case "leave":
						await dialog.Msg(L("The nearest lens quarry is in Orsha and its output goes to the army. I have written 3 times."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBlueberrin", out var killObj)) return;
				if (!quest.TryGetProgress("collectCrystals", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He holds one up to his eye and reads the grain of his own thumbprint through it*{/}"));
					await dialog.Msg(L("Eight, and 6 of them clear enough to grind. That is a good afternoon by any measure I have."));
					await dialog.Msg(L("Take the cutting fee. And take a lens - the seventh one is flawed and it is still better than most things you can buy."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty down and not enough crystal. The young ones have not grown one yet - go for the big slow ones."));
				}
				else
				{
					await dialog.Msg(L("Still Blueberrin in the southern hollow. They cluster where the ground stays wet."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("She came for all 6 the same evening. Did not haggle, which she always does, and left one of them behind on my bench by accident."));
			}
		});

		// Quest 1005: The Mysterious Seed
		//---------------------------------------------------------------------
		AddNpc(154013, L("[Kupole] Astra"), "f_maple_24_3", -881, -1081, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1005);

			dialog.SetTitle(L("Astra"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_maple_24_3", 1001)))
				{
					await dialog.Msg(L("Read my 3 beds first. Everything I am about to ask you rests on what the white one is doing."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's kneeling right at the white bed now, finally looking at it, hands trembling slightly over the soil*{/}"));
				await dialog.Msg(L("The white bed faces downhill because there is a second seed in the ground under it. Not the Divine Tree's - something older, and my bed has been pointing at it for a hundred years."));
				await dialog.Msg(L("Bring me 6 Yellow Butterfly Leaves off the Cloverin to steady the ground, and then stand with me while I take it up. Everything in this forest that has been walking east will arrive at once."));

				var response = await dialog.Select(L("Will you help lift the seed?"),
					Option(L("I'll bring the leaves and stand with you"), "help"),
					Option(L("Older than the Divine Tree?"), "info"),
					Option(L("Leave it in the ground"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Yellow leaves only. Red will steady nothing and white will make the ground let go entirely, which is the opposite of the plan."));
						break;

					case "info":
						await dialog.Msg(L("Older. My grandmother planted the white bed over it deliberately and told nobody, and I have inherited a hundred years of her not telling anybody."));
						await dialog.Msg(L("Every reading I have ever taken has been about this seed and I have been writing them down as weather."));
						break;

					case "leave":
						await dialog.Msg(L("Leave it, and every Fragolin in the north keeps walking east until there is no north. I have watched that happen slowly for a year."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectLeaves", out var leafObj)) return;
				if (!quest.TryGetProgress("holdTheBed", out var holdObj)) return;

				if (leafObj.Done && holdObj.Done)
				{
					await dialog.Msg(L("It is up and it is in a crystal case and it is going to my sisters tonight, all three of us in one room for the first time in a century."));
					await dialog.Msg(L("Take these. They were my grandmother's and she left them under the bed she planted, which I now understand was a note."));

					character.Quests.Complete(questId);
				}
				else if (leafObj.Done)
				{
					await dialog.Msg(L("The ground is steady. Stand close - I am lifting it now and I will not be able to help you while I do."));
				}
				else
				{
					await dialog.Msg(L("Still short of leaves. Yellow, off the Cloverin, and I need all 6 before I put a hand in that soil."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The red bed opened this morning. Every row. A hundred years of readings and the last one is the only good news in the set."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: What the Beds Say
//-----------------------------------------------------------------------------

public class WhatTheBedsSayQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1001);
		SetName(L("What the Beds Say"));
		SetType(QuestType.Sub);
		SetDescription(L("Three flower beds have measured the north of Parias Forest for a hundred years - yellow for its water, red for its blood, white for whatever is under it. Count what has opened in each."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kupole] Astra"), "f_maple_24_3");

		AddObjective("readBeds", L("Read the 3 flower beds"),
			new VariableCheckObjective("Laima.Quests.f_maple_24_3.Quest1001.BedsRead", 3, true));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_3.Quest1001.BedsRead");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_3.Quest1001.Bed{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_24_3.Quest1001.BedsRead");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_24_3.Quest1001.Bed{i}");
	}
}

// Quest 1002 CLASS: A True Yellow, a True Red, a True White
//-----------------------------------------------------------------------------

public class ATrueYellowATrueRedATrueWhiteQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1002);
		SetName(L("A True Yellow, a True Red, a True White"));
		SetType(QuestType.Sub);
		SetDescription(L("A closed red bed is either a sick bed or a lying instrument, and there is no telling which without a wild flower of each colour to compare against."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Kupole] Astra"), "f_maple_24_3");

		AddObjective("collectYellow", L("Collect Yellow Flowers from Cloverin"),
			new CollectItemObjective(667230, 4));

		AddObjective("collectRed", L("Collect Red Flowers from Fragolin"),
			new CollectItemObjective(667231, 4));

		AddObjective("collectWhite", L("Collect White Flowers from Blueberrin"),
			new CollectItemObjective(667232, 4));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(667230, 0.45f, MonsterId.Cloverin);
		AddDrop(667231, 0.45f, MonsterId.Fragolin);
		AddDrop(667232, 0.45f, MonsterId.Blueberrin);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667230, character.Inventory.CountItem(667230), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(667231, character.Inventory.CountItem(667231), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(667232, character.Inventory.CountItem(667232), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667230, character.Inventory.CountItem(667230), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(667231, character.Inventory.CountItem(667231), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(667232, character.Inventory.CountItem(667232), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Fragolin in the Fern
//-----------------------------------------------------------------------------

public class FragolinInTheFernQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1003);
		SetName(L("Fragolin in the Fern"));
		SetType(QuestType.Sub);
		SetDescription(L("A good Fragolin year is 3 or 4 in a stand of fern. The forest's dye-maker counted 60 in one stand and gave up counting, and they were all walking the same way."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Dye-Maker] Ona"), "f_maple_24_3");

		AddObjective("killFragolin", L("Kill Fragolin in the western fern"),
			new KillObjective(30, new[] { MonsterId.Fragolin }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1004 CLASS: Transparent Crystal
//-----------------------------------------------------------------------------

public class TransparentCrystalQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1004);
		SetName(L("Transparent Crystal"));
		SetType(QuestType.Sub);
		SetDescription(L("Blueberrin grow a transparent crystal in the gut that grinds into better lens glass than anything the nearest quarry produces, and the quarry's whole output goes to the army anyway."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Cutter] Vidas"), "f_maple_24_3");

		AddObjective("killBlueberrin", L("Kill Blueberrin in the southern hollow"),
			new KillObjective(20, new[] { MonsterId.Blueberrin }));

		AddObjective("collectCrystals", L("Collect Transparent Crystals"),
			new CollectItemObjective(667236, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(667236, 0.45f, MonsterId.Blueberrin);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667236, character.Inventory.CountItem(667236), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667236, character.Inventory.CountItem(667236), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Mysterious Seed
//-----------------------------------------------------------------------------

public class TheMysteriousSeedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1005);
		SetName(L("The Mysterious Seed"));
		SetType(QuestType.Sub);
		SetDescription(L("The white bed faces downhill because there is a second seed under it, older than the Divine Tree, planted over deliberately and never mentioned. Steady the ground with Yellow Butterfly Leaves, then hold it while it is lifted."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Kupole] Astra"), "f_maple_24_3");

		AddPrerequisite(new CompletedPrerequisite("f_maple_24_3", 1001));

		AddObjective("collectLeaves", L("Collect Yellow Butterfly Leaves from Cloverin"),
			new CollectItemObjective(667233, 6));

		AddObjective("holdTheBed", L("Hold the white bed while the seed is lifted"),
			new LayeredKillObjective(
				spawnList: new[] {
					new KillSpec(MonsterId.Blueberrin, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Fragolin, 3),
				},
				resetIdent: "collectLeaves",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(513103, 1)); // Vine Boots
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(667233, 0.45f, MonsterId.Cloverin);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667233, character.Inventory.CountItem(667233), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667233, character.Inventory.CountItem(667233), InventoryItemRemoveMsg.Destroyed);
	}
}
