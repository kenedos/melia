//--- Melia Script ----------------------------------------------------------
// Stele Road Quest NPCs
//--- Description -----------------------------------------------------------
// The memorial road out of Fedimian, where two epigraphers have spent 9 years
// transcribing 431 stones and are short exactly one.
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

public class FRemains37QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Four Hundred and Thirty-One
		//---------------------------------------------------------------------
		AddNpc(20157, L("[Epigrapher] Raymond"), "f_remains_37", 439, -1643, 27, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1001);

			dialog.SetTitle(L("Raymond"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's bent low over a stone rubbing, tracing a letter with one finger without once glancing up at your approach*{/}"));
				await dialog.Msg(L("Careful where you put your feet, there's a transcription drying at your ankle. 431 stones on this road and I have transcribed 431 stones. 9 years. I can tell you which of them was cut by a left-handed mason and I can prove it."));
				await dialog.Msg(L("What I cannot do is stop the Tree Ambulos walking over the fallen fragments in the middle field and grinding the faces off them. Kill 25 of them and bring me 8 fragments before the count becomes 430."));

				var response = await dialog.Select(L("Will you go into the middle field?"),
					Option(L("I'll recover 8 fragments"), "help"),
					Option(L("What is on the fragments?"), "info"),
					Option(L("They're already broken"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Pick up the faced side, not the biggest piece. A blank slab weighs the same and tells me nothing."));
						break;

					case "info":
						await dialog.Msg(L("The same 3 names, over and over, the whole length of the road. Ruklys. Lydia Schaffen. Agayla Fleury."));
						await dialog.Msg(L("431 stones and 3 names. I have written that sentence in 4 reports and the archive has never once asked me about it."));
						break;

					case "leave":
						await dialog.Msg(L("Broken is not gone. A broken stone with a face is a record. A ground-down stone is gravel and 300 years of somebody's work with it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killAmbulo", out var killObj)) return;
				if (!quest.TryGetProgress("collectFragments", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lays the fragments face-up on the trestle and sorts them by letter height without hesitating once*{/}"));
					await dialog.Msg(L("8, and 6 of them join. That is 2 stones back in the count and I did not expect either of them this year."));
					await dialog.Msg(L("Take the survey allowance. It is issued for hiring hands and I have never in 9 years found a hand worth hiring."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The field is quiet. Now go over the ground properly - the fragments lie flat and they are the colour of the soil."));
				}
				else
				{
					await dialog.Msg(L("Still Ambulos out there. Clear them or they will simply walk over everything you set down."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("433. I have written to the archive to correct the figure and I expect no reply, and I have written it anyway."));
			}
		});

		// Quest 1002: Twenty-Two Trenches
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Treasure Hunter] Edan"), "f_remains_37", 410, 35, 73, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1002);

			dialog.SetTitle(L("Edan"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He climbs up out of a trench mid-sentence, mud to the elbows, and doesn't bother brushing any of it off*{/}"));
				await dialog.Msg(L("Perfect timing - grab that rope before it slides back in, would you. 22 trenches open on the east terrace and 3 shoring props left. Raymond calls it looting. Raymond has never gone down a hole with 8 feet of wet clay over his head."));
				await dialog.Msg(L("The only timber up here walks about on roots. Kill Stumpy Trees for me and bring back 10 sound trunks and I can shore the lot."));

				var response = await dialog.Select(L("Will you get the timber?"),
					Option(L("I'll bring you 10 trunks"), "help"),
					Option(L("Is it looting?"), "info"),
					Option(L("Fill the trenches in"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the ones standing in the dry ground east of the terrace. The wet-ground ones are soft the whole way through and they'll fold on me at the worst possible moment."));
						break;

					case "info":
						await dialog.Msg(L("I write down the depth, the layer and which way the thing was lying, every time, and I have 6 years of it in a book. He writes down letters."));
						await dialog.Msg(L("Between the 2 of us there is one complete record of this road and neither of us will sit in the same tent as the other."));
						break;

					case "leave":
						await dialog.Msg(L("22 trenches and 4 of them are down onto worked stone that nobody has seen since it was buried. I am not filling those in for a man who won't say good morning."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectTrunks", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He stands each trunk on end and leans his whole weight on it before he accepts it*{/}"));
					await dialog.Msg(L("All 10 sound. That shores 22 trenches with 2 lengths spare, and I sleep tonight instead of lying awake listening to clay."));
					await dialog.Msg(L("Here. And take a look at trench 19 on your way past - there is a foundation course down there that runs straight under the road, and the road is supposed to be older."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough. Try further east where the ground dries out - the ones near the terrace edge are all soft."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("22 shored and trench 19 opened up another 4 feet. That foundation course keeps going and it is heading for the north garden, which is a very long way for a wall to go."));
			}
		});

		// Quest 1003: The North Garden Road
		//---------------------------------------------------------------------
		AddNpc(20158, L("[Epigrapher] Smeade"), "f_remains_37", 318, 2902, 20, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1003);

			dialog.SetTitle(L("Smeade"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's leaning on a walking stick at the garden gate, catching her breath, and waves you over before you can pass her by*{/}"));
				await dialog.Msg(L("Good, a fresh pair of legs. Mine have done all they're going to today. I have walked this road 300 times and Raymond has walked it once, very carefully, and written down more than I ever will. We are both right and it is unbearable."));
				await dialog.Msg(L("The Tama have come up out of the north garden onto the road itself and I can no longer get a clerk to walk the last stretch. Kill 30 of them and the north half gets surveyed this season."));

				var response = await dialog.Select(L("Will you clear the north stretch?"),
					Option(L("I'll kill 30 Tama"), "help"),
					Option(L("Why does the north half matter?"), "info"),
					Option(L("Survey it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come off the garden side, never the road side. Keep the stones on your left going north and nothing gets behind you."));
						break;

					case "info":
						await dialog.Msg(L("Because the road is a sentence and Raymond is reading it as a list. The names change order 3 times and each time they change, they change at a bend."));
						await dialog.Msg(L("You cannot see that from a transcription. You can only see it by walking, which is why I am 68 and still walking."));
						break;

					case "leave":
						await dialog.Msg(L("I have. Twice this month. The second time a Tama took my rubbing kit off my back and I did not notice for a mile, which was the moment I stopped."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTama", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Walked to the garden gate and back with a kit on my back and nothing came off the garden side. I had forgotten what that stretch looks like when you are not watching the grass."));
					await dialog.Msg(L("Take the north survey's money. It has been sitting unspent because there has been no north survey."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still coming up. Work the garden edge rather than the road - that is where they gather before they cross."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("2 clerks walked the north half this week. The order changes at the third bend, exactly as I said, and I have sent that to Raymond with no covering note at all."));
			}
		});

		// Quest 1004: Rubbings Nobody Has Taken
		//---------------------------------------------------------------------
		AddNpc(20114, L("[Rubbing-Clerk] Ruta"), "f_remains_37", 487, -2555, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1004);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's on her knees over a crate of wax blocks, sorting them by size without looking up, wrists visibly aching*{/}"));
				await dialog.Msg(L("If you're not one of the epigraphers, you can actually be useful to me. 1,100 rubbings in this crate and 2 hands to take them with, both of them mine. The epigraphers read. I kneel on wet stone with a wax block and do the actual work."));
				await dialog.Msg(L("There are 4 tablets lying flat in the middle field that neither of them will admit exist, because they are face-down and someone has to turn them over. Go and do all 4."));

				var response = await dialog.Select(L("Will you take the rubbings?"),
					Option(L("I'll do all 4 tablets"), "help"),
					Option(L("Why won't they admit they exist?"), "info"),
					Option(L("Turn them over yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Clear the face before you press, and press from the middle out. A rubbing taken over grit reads as a letter that was never cut."));
						break;

					case "info":
						await dialog.Msg(L("Because a face-down tablet is not on the road. Raymond counts stones standing on the road. Smeade counts bends. Neither method has anywhere to put a thing lying in the grass."));
						await dialog.Msg(L("I have been saying this for 4 years to 2 men who each think the other is the one not listening."));
						break;

					case "leave":
						await dialog.Msg(L("A faced tablet is 400 pounds of wet stone. I turn one a day and my back has opinions about the other 3."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("takeRubbings", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays the 4 rubbings side by side on the crate lid and goes very quiet*{/}"));
					await dialog.Msg(L("Same 3 names. But not memorials - every one of these is a boundary formula. 'From this stone to the next, and no further.'"));
					await dialog.Msg(L("Take the clerk's fee. And I am going to walk these to Raymond myself and stand there while he reads them."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 4. They are lying flat in the grass between the woodpiles and the south flats - you will feel them underfoot before you see them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("He read all 4 without saying a word and then asked me, very politely, whether I would take rubbings of the other 427. I said I would think about it."));
			}
		});

		// Quest 1004 collection points - the face-down tablets
		//---------------------------------------------------------------------
		void AddFallenTablet(int tabletNumber, string observation, int x, int z, int direction)
		{
			AddNpc(147464, L("Fallen Tablet"), "f_remains_37", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_remains_37", 1004);
				var variableKey = $"Laima.Quests.f_remains_37.Quest1004.Tablet{tabletNumber}";
				var counterKey = "Laima.Quests.f_remains_37.Quest1004.RubbingsTaken";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A worked slab lying face-down in the grass*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already took this rubbing*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Taking the rubbing..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var taken = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, taken);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Rubbings taken: {0}/4", taken));

					if (taken >= 4)
						character.ServerMessage(L("{#FFD700}All 4 rubbings taken. Return to Ruta.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the tablet face-down."));
				}
			});
		}

		AddFallenTablet(1,
			L("First Tablet: RUKLYS, and under the name a line reading 'from this stone to the next'."), 837, -1128, 312);
		AddFallenTablet(2,
			L("Second Tablet: LYDIA SCHAFFEN, and the same line, and no date of death anywhere on it."), 529, -898, 78);
		AddFallenTablet(3,
			L("Third Tablet: AGAYLA FLEURY, cut by the same hand as the first two on the same day."), 391, -1358, 25);
		AddFallenTablet(4,
			L("Fourth Tablet: all 3 names together, in order, and beneath them 'and no further'."), 672, -1335, 0);

		// The Ruklys Memorial
		//---------------------------------------------------------------------
		AddNpc(47192, L("Ruklys Memorial"), "f_remains_37", 433, 2880, 95, async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.HasCompleted(new QuestId("f_remains_37", 1005)))
			{
				await dialog.Msg(L("{#666666}*The soot has been lifted off the face and the cut shows through, shallow but whole*{/}"));
				await dialog.Msg(L("{#666666}*RUKLYS. And beneath the name, in the same hand as the fallen tablets: 'from this stone to the next, and no further'*{/}"));
				return;
			}

			await dialog.Msg(L("{#666666}*A tall memorial at the garden gate, its whole face carbonized black*{/}"));
			await dialog.Msg(L("{#666666}*The stone is cold. The soot is not weathered into it, and it comes away grey on a fingertip*{/}"));
		});

		// Quest 1005: The Burned Face
		//---------------------------------------------------------------------
		AddNpc(20157, L("[Epigrapher] Raymond"), "f_remains_37", 1168, -2684, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1005);

			dialog.SetTitle(L("Raymond"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_remains_37", 1001)))
				{
					await dialog.Msg(L("Bring me the fragments out of the middle field first. I will not stand in a flat arguing about fire while the count is still short."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's rubbing a grey smudge between finger and thumb over and over, staring at it like it's lying to him*{/}"));
				await dialog.Msg(L("Good, you're back - I need someone who isn't afraid of a fight for this. Smeade sent a piece down from the Ruklys Memorial. That is the one stone of 431 I have never transcribed, because its face is burned black, and I have always written that up as old damage."));
				await dialog.Msg(L("It is not old. The soot comes off grey on a thumb. The only fire on this road hot enough sits in the stub-tree flats behind you. Kill 20 Stumpy Trees to open the flats, then put down the Magburk in the middle of them."));

				var response = await dialog.Select(L("Will you go into the flats?"),
					Option(L("I'll kill the Magburk"), "help"),
					Option(L("Somebody burned a memorial?"), "info"),
					Option(L("Just chisel the soot off"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650744, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Carry the piece. If the char on it matches what comes off that thing then I have a date, and a date is the whole argument."));
						break;

					case "info":
						await dialog.Msg(L("Somebody walked a Magburk up 3 miles of road to a memorial and set it at the face. That is not vandalism, that is an erasure, and it was done recently enough that the soot is still loose."));
						await dialog.Msg(L("Ruta's rubbings say these stones are a boundary, not graves. Somebody wants one end of that boundary unreadable."));
						break;

					case "leave":
						await dialog.Msg(L("And destroy the only physical evidence of when it was done. No. The stone can stay black until I can say who blacked it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openFlats", out var flatsObj)) return;
				if (!quest.TryGetProgress("killMagburk", out var bossObj)) return;

				if (flatsObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("{#666666}*He holds the memorial piece against the scorched ground and turns it until the grain of the char lines up*{/}"));
					await dialog.Msg(L("Same burn. Same direction. That stone was blacked within the year by a thing that was walked here on purpose, and I can write that down."));
					await dialog.Msg(L("Take the bangle out of the flats - it came off somebody who did not walk back out of them. I am sending Ruta's rubbings to the garden. There are 5 Lydia Schaffen stones down there and only one woman, and I would like that explained."));

					character.Quests.Complete(questId);
				}
				else if (flatsObj.Done)
				{
					await dialog.Msg(L("The flats are open. It is sitting in the middle of them and it will not come to the edge, so the edge is no use to you."));
				}
				else
				{
					await dialog.Msg(L("Too many Stumpy Trees between you and the middle. Open the ground first - a fire fight with roots at your back is a short fight."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The soot lifted off the Ruklys face in one sheet and there is a boundary formula under it, word for word what Ruta pulled out of the grass. 431 stones, 3 names, 1 fence."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Four Hundred and Thirty-One
//-----------------------------------------------------------------------------

public class FourHundredAndThirtyOneQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1001);
		SetName(L("Four Hundred and Thirty-One"));
		SetType(QuestType.Sub);
		SetDescription(L("An epigrapher has transcribed all 431 stones of the Stele Road in 9 years. The Tree Ambulos in the middle field are walking over the fallen fragments and grinding the faces off them."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Epigrapher] Raymond"), "f_remains_37");

		AddObjective("killAmbulo", L("Kill Tree Ambulos in the middle field"),
			new KillObjective(25, new[] { MonsterId.TreeAmbulo }));

		AddObjective("collectFragments", L("Recover faced Ruins Fragments"),
			new CollectItemObjective(650739, 8));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion

		AddDrop(650739, 0.35f, MonsterId.TreeAmbulo);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650739, character.Inventory.CountItem(650739), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650739, character.Inventory.CountItem(650739), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Twenty-Two Trenches
//-----------------------------------------------------------------------------

public class TwentyTwoTrenchesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1002);
		SetName(L("Twenty-Two Trenches"));
		SetType(QuestType.Sub);
		SetDescription(L("A treasure hunter has 22 open trenches on the east terrace and 3 shoring props left, under 8 feet of wet clay. The only timber on the terrace walks about on roots."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Treasure Hunter] Edan"), "f_remains_37");

		AddObjective("collectTrunks", L("Recover sound Stubby Tree Trunks"),
			new CollectItemObjective(650540, 10));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion

		AddDrop(650540, 0.45f, MonsterId.Stub_Tree);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650540, character.Inventory.CountItem(650540), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650540, character.Inventory.CountItem(650540), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The North Garden Road
//-----------------------------------------------------------------------------

public class TheNorthGardenRoadQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1003);
		SetName(L("The North Garden Road"));
		SetType(QuestType.Sub);
		SetDescription(L("The Tama have come up out of the north garden onto the road itself, and no clerk will walk the last stretch to the garden gate. Kill 30 of them so the north half can be surveyed."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Epigrapher] Smeade"), "f_remains_37");

		AddObjective("killTama", L("Kill Tama on the north garden stretch"),
			new KillObjective(30, new[] { MonsterId.Tama }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1004 CLASS: Rubbings Nobody Has Taken
//-----------------------------------------------------------------------------

public class RubbingsNobodyHasTakenQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1004);
		SetName(L("Rubbings Nobody Has Taken"));
		SetType(QuestType.Sub);
		SetDescription(L("4 worked tablets lie face-down in the middle field, and neither epigrapher's method has anywhere to put a stone that is not standing on the road. Turn all 4 and take the rubbings."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Rubbing-Clerk] Ruta"), "f_remains_37");

		AddObjective("takeRubbings", L("Take rubbings from all 4 fallen tablets"),
			new VariableCheckObjective("Laima.Quests.f_remains_37.Quest1004.RubbingsTaken", 4, true));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_37.Quest1004.RubbingsTaken");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_37.Quest1004.Tablet{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_remains_37.Quest1004.RubbingsTaken");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_remains_37.Quest1004.Tablet{i}");
	}
}

// Quest 1005 CLASS: The Burned Face
//-----------------------------------------------------------------------------

public class TheBurnedFaceQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1005);
		SetName(L("The Burned Face"));
		SetType(QuestType.Sub);
		SetDescription(L("The Ruklys Memorial is the one stone of 431 that has never been transcribed, because its face is burned black - and the soot still comes away loose. The only fire on this road hot enough sits in the stub-tree flats."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Epigrapher] Raymond"), "f_remains_37");

		AddPrerequisite(new CompletedPrerequisite("f_remains_37", 1001));

		AddObjective("openFlats", L("Kill Stumpy Trees to open the stub-tree flats"),
			new KillObjective(20, new[] { MonsterId.Stub_Tree }));

		AddObjective("killMagburk", L("Defeat the Magburk in the flats"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_MagBurk, 1) },
				resetIdent: "openFlats",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(10000, 7000));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(603105, 1)); // Zachariel Bangle
		AddReward(new ItemReward(640082, 3)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650744, character.Inventory.CountItem(650744), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650744, character.Inventory.CountItem(650744), InventoryItemRemoveMsg.Destroyed);
	}
}
