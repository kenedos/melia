//--- Melia Script ----------------------------------------------------------
// Poslinkis Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The survivor camp on the cemetery road - the people who walked out of the
// eleven settlements the Kingdom stopped counting, and what they woke.
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

public class FKatyn13QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: The Vubbe Spears
		//---------------------------------------------------------------------
		AddNpc(147481, L("[Camp-Leader] Eimantas"), "f_katyn_13", -250, -2050, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1001);

			dialog.SetTitle(L("Eimantas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He straightens from counting heads at the camp's edge, wary until he sees you're not one of theirs*{/}"));
				await dialog.Msg(L("You're not with the spears, so — welcome, I suppose. Eimantas, I lead this camp. We're 34 people camped on a cemetery road because everything behind us is worse. I didn't pick this spot, I just ran out of road."));
				await dialog.Msg(L("The High Vubbe come down the track every evening and count us. They're working out whether we're worth the trouble. Kill 25 of them and they'll decide we're not."));

				var response = await dialog.Select(L("Will you take the track for us?"),
					Option(L("I'll kill the High Vubbe"), "help"),
					Option(L("Where did you all come from?"), "info"),
					Option(L("Find somewhere safer"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them on the track itself, not in the trees. In the open they come one at a time. In the trees they come all at once."));
						break;

					case "info":
						await dialog.Msg(L("Up the valley. There were eleven villages up there and none of them are villages now. We're what's left of four of them, walking together because it was that or walking alone."));
						await dialog.Msg(L("Asana lost her whole street. Rimvydas lost his sons. I lost an argument about whether we should leave, which is why we left three weeks late."));
						break;

					case "leave":
						await dialog.Msg(L("Safer is south, and south is a graveyard holding 406 men. I'd rather explain the Vubbe to my people than explain that to them."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killVubbe", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Nobody came down the track last night. First evening in three weeks that nobody had to sit up with a stick."));
					await dialog.Msg(L("Take the camp purse. It's four villages' worth of coin and it buys nothing out here."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still plenty of them on the track. They come at dusk, so you've got until then."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Rimvydas slept through the whole night. He hasn't done that since we left. I sat up anyway - habit's a hard thing to put down."));
			}
		});

		// Quest 1002: Arrows in the Palisade
		//---------------------------------------------------------------------
		AddNpc(152001, L("[Camp-Weaver] Asana"), "f_katyn_13", -260, -2140, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1002);

			dialog.SetTitle(L("Asana"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's working an arrow shaft loose from the palisade board, testing its straightness against her eye*{/}"));
				await dialog.Msg(L("Hold still a moment, you're standing in my light. Asana, I weave for the camp. Eimantas built this palisade out of cart boards and it's got 60 Vubbe arrows stuck in it — I pull them out every morning, the shafts make good loom rod."));
				await dialog.Msg(L("Kill 20 of the High Vubbe Archers so they stop refilling my wall, and bring me 6 of their arrows while you're at it. I've got a warp to finish and no rod left."));

				var response = await dialog.Select(L("Will you clear the archers?"),
					Option(L("I'll hunt the archers and bring the arrows"), "help"),
					Option(L("You're weaving out here?"), "info"),
					Option(L("Buy rod instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They shoot from the tree line and they're useless close up. Get inside their range and they panic and run, and a running Vubbe drops everything it's carrying."));
						break;

					case "info":
						await dialog.Msg(L("34 people and one blanket each. That's not enough for a winter and we've got one coming. So yes, I'm weaving out here, and I'll keep weaving until somebody stops shooting at the frame."));
						await dialog.Msg(L("My street had 19 houses. I know because I wove a coverlet for every wedding on it. There's nobody left to wed."));
						break;

					case "leave":
						await dialog.Msg(L("Buy it from who, exactly? The nearest market is three days off, and I'd have to walk past whatever ate my village to reach it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("collectArrows", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She snaps a shaft over her knee, checks the grain, and nods*{/}"));
					await dialog.Msg(L("Good wood. Hard, straight, and it'll hold tension. Their fletchers are better than ours were, which I'm choosing not to think about."));
					await dialog.Msg(L("Take this. It was going to be somebody's wedding coverlet and there's no wedding."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The tree line's quiet, but I still need arrows. Check what the dead ones dropped before something else takes it."));
				}
				else
				{
					await dialog.Msg(L("They're still up in the tree line. Nothing came out of the wall this morning but splinters."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Six blankets finished, on Vubbe rod. If anyone up the valley is still alive to laugh at that, I'd like to hear it."));
			}
		});

		// Quest 1003 giver - the woodcarver who died with his shrine unfinished
		//---------------------------------------------------------------------
		AddGhostNpc(152002, L("[Restless Soul] Woodcarver Mindaugas"), "f_katyn_13", -436, -2035, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1003);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He watches the living walk past him without a flicker of notice, then brightens when your eyes actually find him*{/}"));
				await dialog.Msg(L("Wait — you see me? Nobody's managed that in some time. Woodcarver Mindaugas. These people camp 20 paces from me and not one of them can see me, which is fine, I was never much company alive either."));
				await dialog.Msg(L("I cut a shrine figure for the forest track 40 years ago and something broke it into 3 pieces. Find the upper piece, the lower piece and the pedestal along the old track and I'll tell you where to set them."));

				var response = await dialog.Select(L("Will you find the pieces?"),
					Option(L("I'll find the 3 pieces"), "help"),
					Option(L("What was the figure?"), "info"),
					Option(L("Leave it broken"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They're under rubble along the track south of the crystal field. Stone doesn't wash far - whatever broke it, it broke it there."));
						break;

					case "info":
						await dialog.Msg(L("A watcher. Head, shoulders, hands turned out. You set one on a track so travellers know somebody thought about them once."));
						await dialog.Msg(L("It stood 40 years and nothing bad ever came up that track. I'd call that proof, and I'd be the only one."));
						break;

					case "leave":
						await dialog.Msg(L("Then it stays in 3 pieces, and the track stays open regardless. Those two facts may or may not be connected."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("upperPiece", out var upObj)) return;
				if (!quest.TryGetProgress("lowerPiece", out var lowObj)) return;
				if (!quest.TryGetProgress("pedestal", out var padObj)) return;

				if (upObj.Done && lowObj.Done && padObj.Done)
				{
					await dialog.Msg(L("{#666666}*He tries to touch the pedestal and his hand goes through it*{/}"));
					await dialog.Msg(L("Set it on the track where the ruts split. Pedestal first, then the lower piece, then the head - and do it now, before the light goes."));
					await dialog.Msg(L("Take the tool roll from the rubble. I carved a watcher with those and they're no use to me."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still pieces missing. Look under the rubble piles, not on top of them - somebody covered the work deliberately."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("It's standing. It's standing and I feel worse than I did in 3 pieces, and I would very much like to know why."));
			}
		});

		// Quest 1003 collection points - the broken shrine figure
		//---------------------------------------------------------------------
		void AddSculpturePiece(int pieceNumber, int model, int itemId, string pieceName, string foundText, int x, int z, int direction)
		{
			AddNpc(model, pieceName, "f_katyn_13", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_13", 1003);
				var variableKey = $"Laima.Quests.f_katyn_13.Quest1003.Piece{pieceNumber}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A rubble pile beside the old forest track*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Nothing left under this one*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Clearing the rubble..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(itemId, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					character.ServerMessage(foundText);
				}
				else
				{
					character.ServerMessage(L("You leave the rubble where it is."));
				}
			});
		}

		AddSculpturePiece(1, 47220, 650368, L("Rubble Pile"),
			L("Found the Upper Piece of Sculpture - a carved head, chipped at the jaw."), 830, -1030, 0);
		AddSculpturePiece(2, 47221, 650369, L("Rubble Pile"),
			L("Found the Lower Piece of Sculpture - shoulders and two open hands."), 1067, -828, 90);
		AddSculpturePiece(3, 47222, 650370, L("Rubble Pile"),
			L("Found the Sculpture Pedestal - squared off, with 40 years of names cut into the side."), 1125, -549, 270);

		// Quest 1004: Wings Off the Track
		//---------------------------------------------------------------------
		AddNpc(20138, L("[Camp Elder] Rimvydas"), "f_katyn_13", -353, -2081, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1004);

			dialog.SetTitle(L("Rimvydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's watching the western slope through a cracked spyglass, jaw tight*{/}"));
				await dialog.Msg(L("Come look at this, tell me I'm not imagining it. Rimvydas, camp elder. Eimantas worries about the Vubbe because they're loud. I worry about the Green Pokuborn — they've been walking that slope in a line, which is not a thing animals do."));
				await dialog.Msg(L("Kill 20 of them out there and bring me 5 wing fragments off them. If the wings are grown wrong the same way my sons' were, I'll know what we're actually running from."));

				var response = await dialog.Select(L("Will you go up the western slope?"),
					Option(L("I'll hunt the Pokuborn and bring the fragments"), "help"),
					Option(L("What happened to your sons?"), "info"),
					Option(L("That's a long walk"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They hold formation until one of them drops, then they scatter and come back around behind you. Kill the one on the end first and the line falls apart."));
						break;

					case "info":
						await dialog.Msg(L("They went out to the barn one evening and came back three days later, and the thing that came back had their faces and the wrong shoulders."));
						await dialog.Msg(L("I did what you do. I've been trying ever since to find out what the word for it is, and 'corruption' is not a word, it's a shrug."));
						break;

					case "leave":
						await dialog.Msg(L("It is a long walk. I'd have made it myself if my knee hadn't given out somewhere around the fourth village."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPokuborn", out var killObj)) return;
				if (!quest.TryGetProgress("collectWings", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lays the fragments out in a row and counts the joints on each*{/}"));
					await dialog.Msg(L("Same wrong bend. Five out of five. Whatever did the valley is still doing it, and it's doing it out here, close."));
					await dialog.Msg(L("Take my strongbox. I've been carrying it since the second village and I have finally worked out that I am carrying it for nobody."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The slope's thinner now. I still need fragments - the wings are the part that tells the story."));
				}
				else
				{
					await dialog.Msg(L("Still a line of them out on the slope. Break the line and the rest is easy."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I've written it all down and given the sheet to Eimantas. He can't read, but he can carry, and carrying is what's needed."));
			}
		});

		// Quest 1005: What the Watcher Was Holding Down
		//---------------------------------------------------------------------
		AddNpc(147481, L("[Camp-Leader] Eimantas"), "f_katyn_13", -350, -2000, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1005);

			dialog.SetTitle(L("Eimantas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_katyn_13", 1003)))
				{
					await dialog.Msg(L("Something set a stone figure back up on the forest track this week and I'd dearly like to know who. Find that out first and then come back to me."));
					return;
				}

				await dialog.Msg(L("You put that watcher back together. Two hours later the ground under the ruts opened and a Corrupted walked out of it, and now I know why the figure was standing there."));
				await dialog.Msg(L("It wasn't protecting the track. It was sitting on the lid. Kill 15 High Vubbe Archers between here and the ruts so it hasn't got a screen to hide behind, then finish it."));

				var response = await dialog.Select(L("Will you close what you opened?"),
					Option(L("I'll kill the Corrupted"), "help"),
					Option(L("Nobody could have known"), "info"),
					Option(L("That's beyond me"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It fights in the ruts and it doesn't leave them. Stay on the high side of the track and it has to climb to reach you, and it climbs badly."));
						break;

					case "info":
						await dialog.Msg(L("The dead carver knew. Not the lid part, but he knew the figure mattered and he couldn't say why, and he asked anyway."));
						await dialog.Msg(L("I'm not angry at you. I'm angry that 40 years of somebody's careful work turns out to have been a stopper in a bottle, and nobody ever told the man who carved it."));
						break;

					case "leave":
						await dialog.Msg(L("Then we break camp tonight and walk south past 406 graves with 34 people and one working knee. Tell me that's the better plan."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearScreen", out var screenObj)) return;
				if (!quest.TryGetProgress("killCorrupted", out var bossObj)) return;

				if (screenObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It's down in the ruts and the ground closed behind it. Mindaugas stood over the hole for an hour and then he went quiet in a way I didn't like."));
					await dialog.Msg(L("Take the whole camp fund. We're staying, so we're going to need friends more than we need coin."));

					character.Quests.Complete(questId);
				}
				else if (screenObj.Done)
				{
					await dialog.Msg(L("The archers are off the track. It's out there in the ruts with nothing in front of it now."));
				}
				else
				{
					await dialog.Msg(L("Too many archers still on the track. It'll sit in the ruts and let them shoot for it all day."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("We're building proper walls. 34 people, one cemetery road, and a stone watcher that nobody is ever going to move again."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Vubbe Spears
//-----------------------------------------------------------------------------

public class TheVubbeSpearsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1001);
		SetName(L("The Vubbe Spears"));
		SetType(QuestType.Sub);
		SetDescription(L("High Vubbe come down the cemetery road every evening to count the survivor camp and decide whether it is worth raiding. Kill enough of them on the track that they decide against it."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Camp-Leader] Eimantas"), "f_katyn_13");

		AddObjective("killVubbe", L("Kill High Vubbe on the cemetery road"),
			new KillObjective(25, new[] { MonsterId.HighBube_Spear }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1002 CLASS: Arrows in the Palisade
//-----------------------------------------------------------------------------

public class ArrowsInThePalisadeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1002);
		SetName(L("Arrows in the Palisade"));
		SetType(QuestType.Sub);
		SetDescription(L("High Vubbe Archers put 60 arrows into the camp's cart-board palisade and the camp weaver needs both the shooting stopped and the shafts for her loom. Clear the tree line and bring back arrows."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Camp-Weaver] Asana"), "f_katyn_13");

		AddObjective("killArchers", L("Kill High Vubbe Archers at the tree line"),
			new KillObjective(20, new[] { MonsterId.HighBube_Archer }));

		AddObjective("collectArrows", L("Collect Vubbe's Wooden Arrows"),
			new CollectItemObjective(650728, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(650728, 0.40f, MonsterId.HighBube_Archer);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650728, character.Inventory.CountItem(650728), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650728, character.Inventory.CountItem(650728), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Broken Sculpture
//-----------------------------------------------------------------------------

public class TheBrokenSculptureQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1003);
		SetName(L("The Broken Sculpture"));
		SetType(QuestType.Sub);
		SetDescription(L("A dead woodcarver's shrine figure stood on the forest track for 40 years until something broke it into 3 pieces and buried them under rubble. Dig out the head, the shoulders and the pedestal so it can stand again."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Woodcarver Mindaugas"), "f_katyn_13");

		AddObjective("upperPiece", L("Recover the Upper Piece of Sculpture"),
			new CollectItemObjective(650368, 1));

		AddObjective("lowerPiece", L("Recover the Lower Piece of Sculpture"),
			new CollectItemObjective(650369, 1));

		AddObjective("pedestal", L("Recover the Sculpture Pedestal"),
			new CollectItemObjective(650370, 1));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650368, character.Inventory.CountItem(650368), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(650369, character.Inventory.CountItem(650369), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(650370, character.Inventory.CountItem(650370), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_13.Quest1003.Piece{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650368, character.Inventory.CountItem(650368), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(650369, character.Inventory.CountItem(650369), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(650370, character.Inventory.CountItem(650370), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_13.Quest1003.Piece{i}");
	}
}

// Quest 1004 CLASS: Wings Off the Track
//-----------------------------------------------------------------------------

public class WingsOffTheTrackQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1004);
		SetName(L("Wings Off the Track"));
		SetType(QuestType.Sub);
		SetDescription(L("Green Pokuborn are walking the western slope in formation, which animals do not do. The camp elder wants them killed and their wings brought back so he can compare the growth to what took his sons."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Camp Elder] Rimvydas"), "f_katyn_13");

		AddObjective("killPokuborn", L("Kill Green Pokuborn on the western slope"),
			new KillObjective(20, new[] { MonsterId.Arburn_Pokubu_Green }));

		AddObjective("collectWings", L("Collect Wing Fragments"),
			new CollectItemObjective(650103, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(650103, 0.35f, MonsterId.Arburn_Pokubu_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650103, character.Inventory.CountItem(650103), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650103, character.Inventory.CountItem(650103), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: What the Watcher Was Holding Down
//-----------------------------------------------------------------------------

public class WhatTheWatcherWasHoldingDownQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1005);
		SetName(L("What the Watcher Was Holding Down"));
		SetType(QuestType.Sub);
		SetDescription(L("Standing the shrine figure back up did not protect the forest track - it unsealed it, and a Corrupted climbed out of the ruts. Clear the archers screening it, then put it back in the ground."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Camp-Leader] Eimantas"), "f_katyn_13");

		AddPrerequisite(new CompletedPrerequisite("f_katyn_13", 1003));

		AddObjective("clearScreen", L("Kill High Vubbe Archers along the forest track"),
			new KillObjective(15, new[] { MonsterId.HighBube_Archer }));

		AddObjective("killCorrupted", L("Defeat the Corrupted"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Fallen_Statue, 1) },
				resetIdent: "clearScreen",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(603111, 1)); // Ismintis Bracelet
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
