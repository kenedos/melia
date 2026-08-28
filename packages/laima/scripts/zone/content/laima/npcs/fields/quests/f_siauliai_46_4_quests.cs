//--- Melia Script ----------------------------------------------------------
// Dina Bee Farm Quest NPCs
//--- Description -----------------------------------------------------------
// Three hundred hives down to eighty-four, and something on this farm has
// learned to work the smoke drums.
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

public class FSiauliai464QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Forty-One Years of Mead
		//---------------------------------------------------------------------
		AddNpc(147476, L("[Brewer] Dorjen"), "f_siauliai_46_4", 1074, 483, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1001);

			dialog.SetTitle(L("Dorjen"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's swirling a jar of mead against the light, frowning at the color like it's betrayed him*{/}"));
				await dialog.Msg(L("Come to see the famous farm, have you? Ha. Don't — not this season, not unless you enjoy watching a man's life's work rot. Forty-one years I've brewed off this farm. Three hundred hives when I started. Eighty-four this morning. And the whole of Klaipeda still drinks what I make like nothing's wrong."));
				await dialog.Msg(L("The Rabbee have been going into the standing hives and carrying the jelly out whole — the absolute nerve of it. Kill 25 of them, bring me 8 combs, and I might just get one last autumn run out of this dying farm."));

				var response = await dialog.Select(L("Will you go at the Rabbee?"),
					Option(L("I'll bring you 8 combs"), "help"),
					Option(L("300 down to 84?"), "info"),
					Option(L("Buy jelly from Klaipeda"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them off the comb rows, not out in the open. A Rabbee away from a hive is fast. A Rabbee with its head in one is not."));
						break;

					case "info":
						await dialog.Msg(L("300 in my father's time and 84 now, and 9 of the 84 went in the last fortnight. Kirina has the count and she is not wrong about counts."));
						await dialog.Msg(L("A hive does not vanish. It is a box on legs. Somebody or something is taking them away entire."));
						break;

					case "leave":
						await dialog.Msg(L("Klaipeda buys its jelly from me. That is the joke and I have stopped finding it funny."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRabbee", out var killObj)) return;
				if (!quest.TryGetProgress("collectJelly", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He presses a thumb into each comb and smells it, and does not hurry about it*{/}"));
					await dialog.Msg(L("8 clean combs. That is the autumn run and 40 barrels, and 40 barrels is this farm still being a farm next spring."));
					await dialog.Msg(L("Take the brewing money. I have 41 years of it in a crock and no children to leave it to."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Rows are quiet. The combs will be down among the hive stands where they dropped them."));
				}
				else
				{
					await dialog.Msg(L("Still in the rows. Clear them first - I would rather lose a week than have you opened up over a comb."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("40 barrels down and the first of them is already going to Klaipeda. And 84 hives is still 84 hives, which is the part nobody in Klaipeda is going to notice."));
			}
		});

		// Quest 1002: Eleven Boxes a Week
		//---------------------------------------------------------------------
		AddNpc(147483, L("[Hivewright] Kleopas"), "f_siauliai_46_4", 1344, 388, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1002);

			dialog.SetTitle(L("Kleopas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's running a plane down a fresh board without looking up, curls of wood piling at his feet*{/}"));
				await dialog.Msg(L("Mind the shavings, they get everywhere. I make the boxes. 11 a week, planed and waxed, and I have made boxes for this farm since I was 14. Not interesting work. I'm extremely good at it."));
				await dialog.Msg(L("The Honeybeans hollow a hive out and then abandon the box in the scrub, and I cannot cut new timber fast enough to keep up. Bring me back 10 of the empty boxes and I can plane them out and re-wax them."));

				var response = await dialog.Select(L("Will you fetch the boxes?"),
					Option(L("I'll bring you 10 boxes"), "help"),
					Option(L("Why abandon a good box?"), "info"),
					Option(L("Cut new timber"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Any box with the lid still hinged is worth carrying. If the lid is off, leave it - a box without a lid is firewood with ambitions."));
						break;

					case "info":
						await dialog.Msg(L("They do not want the box. They want what is in it, and a Honeybean is not built to carry a box, so it takes the inside out and walks off."));
						await dialog.Msg(L("Which is what makes the 9 that vanished so strange, because those went box and all."));
						break;

					case "leave":
						await dialog.Msg(L("There is no timber on this farm worth a hive box. Every board I have used in 30 years came up from the Dina road on a cart."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectBoxes", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He runs a thumb along every joint before he stacks them, and one of them he sets aside*{/}"));
					await dialog.Msg(L("9 to plane and 1 to burn. That is 9 hives back on the stands inside a fortnight."));
					await dialog.Msg(L("Take the timber money. I have been paid for boards I did not have to buy, which makes it more yours than mine."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough. Try the scrub east of the comb rows - that is where they drag them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("9 boxes back on stands. And the one I set aside had its whole floor cut out square, from underneath, with something with an edge on it."));
			}
		});

		// Quest 1003: Sixty Head Down to Twenty-Two
		//---------------------------------------------------------------------
		AddNpc(147485, L("[Shepherd] Mikolas"), "f_siauliai_46_4", 1093, 792, 1, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1003);

			dialog.SetTitle(L("Mikolas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's counting heads at the pasture fence, lips moving, finger jabbing the air at each sheep*{/}"));
				await dialog.Msg(L("Twenty-two! Still twenty-two, bless every one of them. This flock is mine, every head of it — sixty in spring, twenty-two this morning, and yes, I can tell you the name of every single one of the thirty-eight I lost. Milda. Old Dot. Little Straw. All of them."));
				await dialog.Msg(L("It's not the bees doing this, it's the wild Siaulamb out of the north pasture — they wander in, mix with mine, and mine just follow them right back out like fools in love. Kill 30 of them, would you? I'd like to keep the twenty-two I've got left."));

				var response = await dialog.Select(L("Will you clear the north pasture?"),
					Option(L("I'll kill 30 wild Siaulamb"), "help"),
					Option(L("Yours just follow them?"), "info"),
					Option(L("Pen your flock"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Work the pasture from the farm side outward. If you come at them from the north they run in, and then they are in among mine and I cannot tell you which is which."));
						break;

					case "info":
						await dialog.Msg(L("Every time. A sheep has 1 idea in its head and that idea is whatever the sheep in front of it is doing."));
						await dialog.Msg(L("I have stood in that pasture and watched 6 of mine walk away from me in a line behind a wild one, and shouted their names, and been ignored by all 6."));
						break;

					case "leave":
						await dialog.Msg(L("Penned sheep do not put wool on. And the pen is 40 head and I would then be a man with 22 sheep in a 40-sheep pen thinking about it every morning."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSiaulamb", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("22 head this morning and 22 head tonight. You will not understand what that sentence is worth to me and that is all right."));
					await dialog.Msg(L("Take the fleece money. There is a year of it and no fleeces to speak of, which tells you how the year went."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still coming in. Work them from the farm side - drive them out, do not push them in."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("24. 2 of mine walked back out of the north on their own 3 days later, and I have not told anyone how long I sat down for."));
			}
		});

		// Quest 1004: Six Drums, Two Full
		//---------------------------------------------------------------------
		AddNpc(147407, L("[Smoke-Hand] Daren"), "f_siauliai_46_4", -227, -960, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1004);

			dialog.SetTitle(L("Daren"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's kneeling by an oil drum with an empty pan in hand, staring at it like it insulted him personally*{/}"));
				await dialog.Msg(L("Oh — sorry, didn't mean to just stare past you like that. I run the smoke. You cannot open a hive without it, so before every harvest I fill the pans off the drums on the west field. I am 19 and it is the only job on this farm nobody else wants."));
				await dialog.Msg(L("There are 6 drums out there and 2 of them have anything in. I did not use that oil. Go and look at 4 of them properly and tell me what you see."));

				var response = await dialog.Select(L("Will you check the drums?"),
					Option(L("I'll look at 4 of them"), "help"),
					Option(L("Could they be leaking?"), "info"),
					Option(L("Just order more oil"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Look at the bung and the ground under it. A drum that has leaked has a stain. A drum that has been emptied has a clean bung and dry grass."));
						break;

					case "info":
						await dialog.Msg(L("That was my first thought and I spent a day on my knees looking for the stain. There is no stain under any of the 4."));
						await dialog.Msg(L("Kirina says do not tell Dorjen until I know. Kirina says that about most things and she is usually right, and I do not enjoy it."));
						break;

					case "leave":
						await dialog.Msg(L("Oil comes up from Klaipeda 4 times a year and it is the single most expensive thing on this farm. If I order early I have to say why."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("checkDrums", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to all 4 and then sits down on the grass without meaning to*{/}"));
					await dialog.Msg(L("Bungs pulled and put back. All 4. That is not an animal knocking a drum over, that is somebody opening it and closing it again after."));
					await dialog.Msg(L("Take the smoke-hand's pay. And I am telling Kirina, because I am 19 and this is not mine to be right about on my own."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 4 yet. They are spread across the west field between the mushroom stands - you will smell them before you see them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Kirina went white and then went and got a lantern and walked the drum line at midnight on her own. She has not told me what she saw."));
			}
		});

		// Quest 1004 collection points - the smoke drums
		//---------------------------------------------------------------------
		void AddSmokeDrum(int drumNumber, string observation, int x, int z, int direction)
		{
			AddNpc(147459, L("Smoke Drum"), "f_siauliai_46_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_46_4", 1004);
				var variableKey = $"Laima.Quests.f_siauliai_46_4.Quest1004.Drum{drumNumber}";
				var counterKey = "Laima.Quests.f_siauliai_46_4.Quest1004.DrumsChecked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*An oil drum standing on the west field, used for filling the hive smoke pans*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already looked this one over*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Checking the bung and the ground..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var checkedCount = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, checkedCount);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Drums checked: {0}/4", checkedCount));

					if (checkedCount >= 4)
						character.ServerMessage(L("{#FFD700}All 4 drums checked. Return to Daren.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the drum alone."));
				}
			});
		}

		AddSmokeDrum(1,
			L("First Drum: empty, bung seated straight, and the grass under it dry to the root."), 352, 32, 0);
		AddSmokeDrum(2,
			L("Second Drum: empty. The bung has been out - there is a clean ring in the grime around the seat."), 108, 57, 0);
		AddSmokeDrum(3,
			L("Third Drum: empty, and something with a very wide hand has left oil marks on both sides of it."), 20, -154, 0);
		AddSmokeDrum(4,
			L("Fourth Drum: empty, bung back in and turned the wrong way round, which no farmhand on this place would do."), 588, -214, 0);

		// The last hive on the east stand
		//---------------------------------------------------------------------
		AddNpc(151025, L("Hive Stand"), "f_siauliai_46_4", 1370, -695, 300, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A hive box on its stand, one of the 84 still standing on the farm*{/}"));
			await dialog.Msg(L("{#666666}*The stand beside it is empty and the grass under it is flat and green. Whatever took that box lifted it - nothing was dragged*{/}"));
		});

		// Quest 1005: Whatever Learned the Smoke
		//---------------------------------------------------------------------
		AddNpc(147418, L("[Hivekeeper] Kirina"), "f_siauliai_46_4", -261, -980, 330, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1005);

			dialog.SetTitle(L("Kirina"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_siauliai_46_4", 1001)))
				{
					await dialog.Msg(L("{#666666}*She's standing at the hive stands with a lantern, though it's broad daylight*{/}"));
					await dialog.Msg(L("Get Dorjen his combs first. If I am right about this the farm is going to need an autumn run behind it, not in front of it."));
					return;
				}

				await dialog.Msg(L("{#666666}*She sets the lantern down at last, hands still faintly shaking*{/}"));
				await dialog.Msg(L("84 hives, 9 gone in a fortnight, 4 drums emptied and re-bunged, and a box with its floor cut out square from underneath. I walked the drum line at midnight and I saw them."));
				await dialog.Msg(L("Siaulogres. They fill a pan off a drum, they smoke a hive until it is quiet, and then they lift the whole box and walk. Kill 20 Honeybeans to open the north scrub, then take the 2 doing it."));

				var response = await dialog.Select(L("Will you go into the north scrub?"),
					Option(L("I'll take the 2 of them"), "help"),
					Option(L("Siaulogres using smoke?"), "info"),
					Option(L("Move the drums indoors"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(661012, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Take Dorjen's bottle. Break it upwind of them - they will come to the smell of it and they will come out of the scrub, and out of the scrub they are only very large."));
						break;

					case "info":
						await dialog.Msg(L("They watched us. That is the whole of it. This farm has smoked hives in the open for 200 years in front of anything that cared to look, and something finally looked."));
						await dialog.Msg(L("I have kept bees since I was 11 and I have never once been frightened by one. I am frightened of an animal that learned my job by watching me do it."));
						break;

					case "leave":
						await dialog.Msg(L("Then they take the pans, or the rows, or Daren, who fills the pans at first light on his own 4 times a year."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openScrub", out var scrubObj)) return;
				if (!quest.TryGetProgress("killOgres", out var ogreObj)) return;

				if (scrubObj.Done && ogreObj.Done)
				{
					await dialog.Msg(L("{#666666}*She walks into the scrub and comes back out carrying a hive box under each arm*{/}"));
					await dialog.Msg(L("7 of the 9 boxes are back there, stacked, lids on, and every one of them smoked out properly. Not broken open. Smoked."));
					await dialog.Msg(L("Take the chain out of the stack - it was in the bottom box and it is nobody's on this farm. 86 hives by the weekend, and I am going to keep counting them every single morning."));

					character.Quests.Complete(questId);
				}
				else if (scrubObj.Done)
				{
					await dialog.Msg(L("Scrub's open. The 2 are still in there with the boxes and they will not leave the boxes, which is the first thing they have done that I understand."));
				}
				else
				{
					await dialog.Msg(L("Too thick to get in. Open the scrub first or you will be fighting bees and ogres in the same 10 paces."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("86 hives and holding. Daren fills the pans with somebody standing with him now, and Dorjen has named the autumn run after him, which he is pretending to hate."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Forty-One Years of Mead
//-----------------------------------------------------------------------------

public class FortyOneYearsOfMeadQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1001);
		SetName(L("Forty-One Years of Mead"));
		SetType(QuestType.Sub);
		SetDescription(L("The Dina farm had 300 hives a generation ago and has 84 this morning. The Rabbee are going into the standing hives and carrying the jelly out whole, and the autumn run cannot start without it."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Brewer] Dorjen"), "f_siauliai_46_4");

		AddObjective("killRabbee", L("Kill Rabbee in the comb rows"),
			new KillObjective(25, new[] { MonsterId.Rabbee }));

		AddObjective("collectJelly", L("Recover combs of Royal Jelly"),
			new CollectItemObjective(650766, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(650766, 0.35f, MonsterId.Rabbee);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650766, character.Inventory.CountItem(650766), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650766, character.Inventory.CountItem(650766), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Eleven Boxes a Week
//-----------------------------------------------------------------------------

public class ElevenBoxesAWeekQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1002);
		SetName(L("Eleven Boxes a Week"));
		SetType(QuestType.Sub);
		SetDescription(L("The Honeybeans hollow a hive out and abandon the box in the scrub, and the farm's hivewright cannot cut new timber fast enough. The abandoned boxes can be planed out and re-waxed."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hivewright] Kleopas"), "f_siauliai_46_4");

		AddObjective("collectBoxes", L("Recover Empty Beehives from the scrub"),
			new CollectItemObjective(650763, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(650763, 0.45f, MonsterId.Honeybean);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650763, character.Inventory.CountItem(650763), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650763, character.Inventory.CountItem(650763), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Sixty Head Down to Twenty-Two
//-----------------------------------------------------------------------------

public class SixtyHeadDownToTwentyTwoQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1003);
		SetName(L("Sixty Head Down to Twenty-Two"));
		SetType(QuestType.Sub);
		SetDescription(L("The farm's flock is down from 60 head to 22. Wild Siaulamb come in off the north pasture, mix with the flock, and the flock follows them out. Kill 30 of them."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shepherd] Mikolas"), "f_siauliai_46_4");

		AddObjective("killSiaulamb", L("Kill wild Siaulamb on the north pasture"),
			new KillObjective(30, new[] { MonsterId.Siaulamb }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Six Drums, Two Full
//-----------------------------------------------------------------------------

public class SixDrumsTwoFullQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1004);
		SetName(L("Six Drums, Two Full"));
		SetType(QuestType.Sub);
		SetDescription(L("No hive can be opened without smoke, and 4 of the farm's 6 oil drums are empty with no stain in the grass under any of them. Look 4 of them over properly."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Smoke-Hand] Daren"), "f_siauliai_46_4");

		AddObjective("checkDrums", L("Check 4 of the smoke drums on the west field"),
			new VariableCheckObjective("Laima.Quests.f_siauliai_46_4.Quest1004.DrumsChecked", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_46_4.Quest1004.DrumsChecked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_4.Quest1004.Drum{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_46_4.Quest1004.DrumsChecked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_4.Quest1004.Drum{i}");
	}
}

// Quest 1005 CLASS: Whatever Learned the Smoke
//-----------------------------------------------------------------------------

public class WhateverLearnedTheSmokeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1005);
		SetName(L("Whatever Learned the Smoke"));
		SetType(QuestType.Sub);
		SetDescription(L("The Siaulogres in the north scrub fill a pan off the farm's own drums, smoke a hive until it is quiet, and lift the whole box away. Open the scrub and take the 2 that have been doing it."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Hivekeeper] Kirina"), "f_siauliai_46_4");

		AddPrerequisite(new CompletedPrerequisite("f_siauliai_46_4", 1001));

		AddObjective("openScrub", L("Kill Honeybeans on the way into the north scrub"),
			new KillObjective(20, new[] { MonsterId.Honeybean }));

		AddObjective("killOgres", L("Take the 2 Siaulogres holding the stacked hives"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Siaulogre, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Honeymeli, 3),
				},
				resetIdent: "openScrub",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583119, 1)); // Svijes Necklace
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661012, character.Inventory.CountItem(661012), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661012, character.Inventory.CountItem(661012), InventoryItemRemoveMsg.Destroyed);
	}
}
