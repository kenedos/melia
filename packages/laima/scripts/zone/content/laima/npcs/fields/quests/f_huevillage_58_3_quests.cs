//--- Melia Script ----------------------------------------------------------
// Cobalt Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The stretch of forest where Andale gave up on blessed obelisks and tried to
// blast a firebreak instead.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage583QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Too Many Caro
		//---------------------------------------------------------------------
		AddNpc(147407, L("[Scout-Captain] Norbertas"), "f_huevillage_58_3", 940, -900, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1001);

			dialog.SetTitle(L("Norbertas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's got a survey map pinned to a stump, redrawing a line on it with a stub of charcoal*{/}"));
				await dialog.Msg(L("Give me a second, I'm re-marking this before I lose the exact spot. Six scouts and a map of a forest that keeps getting bigger — every week the treeline's closer to Andale than it was."));
				await dialog.Msg(L("The Caro are the worst of it - they've bred out of all proportion and they strip a scout's cache in a night. Kill 30 of them on the east runs and I can put caches back out."));

				var response = await dialog.Select(L("Will you thin them?"),
					Option(L("I'll kill the Caro"), "help"),
					Option(L("The forest is getting bigger?"), "info"),
					Option(L("Not today"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Work the east runs. They're thickest there and they don't scatter, so you'll get a good rhythm going."));
						break;

					case "info":
						await dialog.Msg(L("Measured it myself against the old survey pegs. Forty paces a season on this side, and the pegs don't move."));
						await dialog.Msg(L("The village used to hold the line with blessed stones. When that stopped working they tried gunpowder. Ask Akvile how that went."));
						break;

					case "leave":
						await dialog.Msg(L("Understood. My scouts keep going out with a day's food and coming back with none, same as always."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCaro", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("East runs are quiet. I sent two caches out yesterday and both were untouched this morning."));
					await dialog.Msg(L("Scout pay, which isn't much, plus what I keep back for exactly this."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still too many on the east runs. Keep going."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Six caches standing. My scouts can range two days out now instead of one. That's the difference between a map and a guess."));
			}
		});

		// Quest 1002: The Firebreak That Wasn't
		//---------------------------------------------------------------------
		AddNpc(147420, L("[Forester] Akvile"), "f_huevillage_58_3", -238, 293, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1002);

			dialog.SetTitle(L("Akvile"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She flinches at the crack of a snapped branch, then forces her shoulders back down*{/}"));
				await dialog.Msg(L("Sorry — every sound out here still does that to me. Two springs ago the village decided that if blessed stones wouldn't hold the forest back, a hundred-pace firebreak would. They sent nine of us in to lay it."));
				await dialog.Msg(L("We got four barrels placed before something came through the trees and we ran. There are 7 barrels still sitting out there in the wet. Bring me 5 of them before the whole clearing goes up."));

				var response = await dialog.Select(L("Will you fetch them out?"),
					Option(L("I'll recover the barrels"), "help"),
					Option(L("What came through the trees?"), "info"),
					Option(L("Powder is not my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Lift, don't drag. Two springs of damp means the seals have gone soft and a soft seal on a full barrel is how you lose an arm."));
						break;

					case "info":
						await dialog.Msg(L("I didn't see it. I heard it go through the canopy above us and I heard Petras stop shouting, and then I was three miles away with no memory of running."));
						await dialog.Msg(L("Una's been asking around about it. She thinks it's still in there. I think she's right and I'd rather she wasn't."));
						break;

					case "leave":
						await dialog.Msg(L("Not mine either. That didn't stop anyone handing me a barrel two springs ago, and look how that went."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("recoverBarrels", out var barrelObj)) return;

				if (barrelObj.Done)
				{
					await dialog.Msg(L("Five out, and three of those seals were weeping. Another wet season and this clearing would have taken the tree line with it."));
					await dialog.Msg(L("Your pay. The village still owes for the powder, so this is out of my own purse and I don't want to discuss it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More barrels still out in the clearing. Lift them, don't drag them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The powder's back in a dry shed and nobody's proposed a second firebreak. I'd call that two victories."));
			}
		});

		// Quest 1002 collection points - abandoned powder barrels
		//---------------------------------------------------------------------
		void AddPowderBarrel(int barrelNum, int modelId, int x, int z, int direction)
		{
			AddNpc(modelId, L("Abandoned Powder Barrel"), "f_huevillage_58_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_huevillage_58_3", 1002);
				var variableKey = $"Laima.Quests.f_huevillage_58_3.Quest1002.Barrel{barrelNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A blasting barrel left out in the wet, its seal swollen and weeping*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already carried this one out*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Lifting the barrel..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650664, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Useful Bomb"));

					var currentCount = character.Inventory.CountItem(650664);
					character.ServerMessage(LF("Barrels recovered: {0}/5", currentCount));

					if (currentCount >= 5)
						character.ServerMessage(L("{#FFD700}That's five out of the wet. Return to Forester Akvile.{/}"));
				}
				else
				{
					character.ServerMessage(L("You set the barrel back down carefully."));
				}
			});
		}

		AddPowderBarrel(1, 147458, -156, 234, 0);
		AddPowderBarrel(2, 147458, -240, 228, 0);
		AddPowderBarrel(3, 147458, -120, 328, 0);
		AddPowderBarrel(4, 147458, -236, 141, 0);
		AddPowderBarrel(5, 147458, -328, 118, 0);
		AddPowderBarrel(6, 147459, -322, 216, 0);
		AddPowderBarrel(7, 147459, -241, 352, 0);

		// Quest 1003: Shafts Off the Tini
		//---------------------------------------------------------------------
		AddNpc(147419, L("[Fletcher] Daiva"), "f_huevillage_58_3", -1064, -705, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1003);

			dialog.SetTitle(L("Daiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's running a thumbnail down a warped shaft, sorting it into the discard pile*{/}"));
				await dialog.Msg(L("Half a moment — this one's warped past saving. Norbertas wants forty arrows a week for his scouts, and Andale hasn't had a shaft-wood shipment since the road closed."));
				await dialog.Msg(L("So I've become a scavenger. The Tini Archers out here shoot straight-grained shafts and I can re-fletch anything that isn't split. Kill 15 of them and bring me 6 arrows worth saving."));

				var response = await dialog.Select(L("Will you go pull some shafts?"),
					Option(L("I'll bring the arrows"), "help"),
					Option(L("You reuse enemy arrows?"), "info"),
					Option(L("Buy your own wood"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them out of the quiver, not out of the ground. A shaft that's been in dirt will warp on me inside a month."));
						break;

					case "info":
						await dialog.Msg(L("Every fletcher does and none of us admit it. A good shaft is a good shaft. It doesn't know who cut it."));
						await dialog.Msg(L("Besides, there's a certain justice in a scout's quiver being full of what shot at him last week."));
						break;

					case "leave":
						await dialog.Msg(L("Buy it with what, exactly? The road's shut and the village is in debt to a powder merchant already. There is no wood to buy."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTini", out var killObj)) return;
				if (!quest.TryGetProgress("gatherArrows", out var arrowObj)) return;

				if (killObj.Done && arrowObj.Done)
				{
					await dialog.Msg(L("Six, and four of them are better than anything I could cut myself. Look at that grain."));
					await dialog.Msg(L("Take your pay. Norbertas gets his forty this week and he'll never ask where from."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Tini Archers still out there. ");
					if (!arrowObj.Done)
						status += L("More usable arrows still to gather. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("One of the scouts noticed the fletching was wrong and asked about it. I told him it was a new supplier. He seemed satisfied."));
			}
		});

		// Quest 1004: The Soul Flower
		//---------------------------------------------------------------------
		AddNpc(147408, L("[Village Priest] Rimas"), "f_huevillage_58_3", 439, -598, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1004);

			dialog.SetTitle(L("Rimas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's turning a dried, spent flower stem over in his hands like it still might tell him something*{/}"));
				await dialog.Msg(L("Come, sit if you like, though I'll warn you I'm poor company today. Vaidas up the valley still paints his boundary stones, and I don't laugh at him for it. But the rite needs a soul flower, and one blooms only once every eleven years or so."));
				await dialog.Msg(L("There's one open right now in this forest and a Doyor pack has denned around it. Kill 20 of them and cut the flower for me while the bloom holds."));

				var response = await dialog.Select(L("Will you get it for me?"),
					Option(L("I'll bring the flower"), "help"),
					Option(L("Why does the rite need it?"), "info"),
					Option(L("Let it bloom in peace"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut the stem low and don't crush it. If the scent goes out of it on the walk back it's just a flower."));
						await dialog.Msg(L("The Doyor den in a ring around it. You'll know you're close when the smell hits you before you see anything."));
						break;

					case "info":
						await dialog.Msg(L("The oil is only oil. The flower is what makes the words stick to the stone - my great-grandmother's word for it, and she was closer to right than any of us."));
						await dialog.Msg(L("Andale has had eleven of these blooms since the village was founded. We've used nine of them. That should tell you how badly this is going."));
						break;

					case "leave":
						await dialog.Msg(L("It'll bloom in peace for another eleven days, then rot on the stalk unremarked. Peace isn't the thing it needs right now."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearDen", out var denObj)) return;
				if (!quest.TryGetProgress("cutFlower", out var flowerObj)) return;

				if (denObj.Done && flowerObj.Done)
				{
					await dialog.Msg(L("{#666666}*He unwraps the stem and the whole clearing smells of it*{/}"));
					await dialog.Msg(L("Still holding. That's the tenth of eleven, and I intend to make it count."));
					await dialog.Msg(L("Take your pay. And if you pass Vaidas, tell him the flower's cut. He'll want to know before I do the rite."));

					character.Quests.Complete(questId);
				}
				else if (denObj.Done)
				{
					await dialog.Msg(L("Den's broken. Get to the flower before something else finds it."));
				}
				else
				{
					await dialog.Msg(L("The Doyor are still ringed around it. You won't get near the stem with the pack intact."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The rite held for nine days on the nearest stone before it faded. Nine days is not eleven years, but it is not nothing either."));
			}
		});

		// Quest 1004 interaction point - the blooming soul flower
		//---------------------------------------------------------------------
		AddNpc(147412, L("Strongly Scented Soul Flower"), "f_huevillage_58_3", 498, 512, 330, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1004);

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A pale bloom on a thick stem. The scent reaches you well before the flower does*{/}"));
				return;
			}

			if (!character.Quests.TryGetById(questId, out var quest)) return;
			if (!quest.TryGetProgress("clearDen", out var denObj)) return;

			if (!denObj.Done)
			{
				await dialog.Msg(L("{#666666}*Doyor circle the clearing. You won't get a clean cut with the pack this close*{/}"));
				return;
			}

			if (character.Inventory.HasItem(650663))
			{
				await dialog.Msg(L("{#666666}*The stem is already cut. Get it to Priest Rimas before the scent fades*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(
				L("Cutting the stem..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
			);

			if (result == TimeActionResult.Completed)
			{
				character.Inventory.Add(650663, 1, InventoryAddType.PickUp);
				character.ServerMessage(L("{#FFD700}Soul flower cut. Return to Village Priest Rimas.{/}"));
			}
			else
			{
				character.ServerMessage(L("You leave the bloom on the stalk."));
			}
		});

		// Quest 1005: What Came Through the Canopy
		//---------------------------------------------------------------------
		AddNpc(147418, L("[Bounty Hunter] Una"), "f_huevillage_58_3", 152, -761, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1005);

			dialog.SetTitle(L("Una"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's cleaning a crossbow with mechanical focus, not looking up until you're close*{/}"));
				await dialog.Msg(L("You've got the look of someone who just talked to Akvile. Nine went in to lay the firebreak. Seven came out. She'll tell you she ran, and she did, and it was the right thing to do."));
				await dialog.Msg(L("It was a Colimencia. It works the canopy and drops on what's underneath it, and it has been eating well out here for two springs. Kill 25 Tipio off the low ground and it will come down looking for what's making the noise."));

				var response = await dialog.Select(L("Want the contract?"),
					Option(L("I'll take the Colimencia"), "help"),
					Option(L("How do you know what it was?"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Keep out from under the heavy branches and keep looking up. Everything it does, it does from above until it can't."));
						break;

					case "info":
						await dialog.Msg(L("Petras was the one who stopped shouting. He was my brother-in-law and I went back in for what was left, which was a boot and a claw-mark I could put my whole hand into."));
						await dialog.Msg(L("I've been in this forest most of two years matching that mark against things. It's a Colimencia. I'm not guessing any more."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. It's waited two springs already. It can wait a while longer for whoever's next through here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("makeNoise", out var noiseObj)) return;
				if (!quest.TryGetProgress("killColimencia", out var bossObj)) return;

				if (noiseObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It's down. I went and looked at it and I didn't feel any of the things I expected to feel."));
					await dialog.Msg(L("Take the whole bounty. Akvile put in half of it and she asked me not to tell you that, so don't say anything."));

					character.Quests.Complete(questId);
				}
				else if (noiseObj.Done)
				{
					await dialog.Msg(L("Branches are moving up there. Get back under the canopy before it settles."));
				}
				else
				{
					await dialog.Msg(L("Not enough noise on the low ground yet. It won't come down for two or three."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Akvile walked the clearing yesterday without anyone with her. First time since. She stood there a while and then came back and got on with her day."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Too Many Caro
//-----------------------------------------------------------------------------

public class TooManyCaroQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1001);
		SetName(L("Too Many Caro"));
		SetType(QuestType.Sub);
		SetDescription(L("Caro have bred out of all proportion on the east runs and strip Scout-Captain Norbertas's caches overnight. Thin them so his scouts can range further than a day."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout-Captain] Norbertas"), "f_huevillage_58_3");

		AddObjective("killCaro", L("Kill Caro on the east runs"),
			new KillObjective(30, new[] { MonsterId.Caro }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1002 CLASS: The Firebreak That Wasn't
//-----------------------------------------------------------------------------

public class TheFirebreakThatWasntQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1002);
		SetName(L("The Firebreak That Wasn't"));
		SetType(QuestType.Sub);
		SetDescription(L("Two springs ago Andale tried to blast a firebreak through Cobalt Forest and the crew ran. Their powder barrels are still lying in the wet with the seals going soft. Carry them out for Forester Akvile."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forester] Akvile"), "f_huevillage_58_3");

		AddObjective("recoverBarrels", L("Recover abandoned powder barrels"),
			new CollectItemObjective(650664, 5));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650664, character.Inventory.CountItem(650664), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 7; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_3.Quest1002.Barrel{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650664, character.Inventory.CountItem(650664), InventoryItemRemoveMsg.Destroyed);

		for (var i = 1; i <= 7; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_3.Quest1002.Barrel{i}");
	}
}

// Quest 1003 CLASS: Shafts Off the Tini
//-----------------------------------------------------------------------------

public class ShaftsOffTheTiniQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1003);
		SetName(L("Shafts Off the Tini"));
		SetType(QuestType.Sub);
		SetDescription(L("Andale has had no shaft-wood since the road closed, so Fletcher Daiva re-fletches whatever the forest shoots at her scouts. Kill Tini Archers and bring back arrows worth saving."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Daiva"), "f_huevillage_58_3");

		AddObjective("killTini", L("Kill Tini Archers"),
			new KillObjective(15, new[] { MonsterId.Tiny_Bow }));

		AddObjective("gatherArrows", L("Recover Useful Arrows"),
			new CollectItemObjective(666083, 6));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(666083, 0.50f, MonsterId.Tiny_Bow);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666083, character.Inventory.CountItem(666083), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666083, character.Inventory.CountItem(666083), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: The Soul Flower
//-----------------------------------------------------------------------------

public class TheSoulFlowerQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1004);
		SetName(L("The Soul Flower"));
		SetType(QuestType.Sub);
		SetDescription(L("A soul flower blooms once in eleven years and Priest Rimas needs this one for the boundary rite. A Doyor pack has denned in a ring around it. Break the den and cut the stem while the bloom holds."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Village Priest] Rimas"), "f_huevillage_58_3");

		AddObjective("clearDen", L("Kill the Doyor denned around the flower"),
			new KillObjective(20, new[] { MonsterId.Doyor }));

		AddObjective("cutFlower", L("Cut the Strongly Scented Soul Flower"),
			new CollectItemObjective(650663, 1));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650663, character.Inventory.CountItem(650663), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650663, character.Inventory.CountItem(650663), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: What Came Through the Canopy
//-----------------------------------------------------------------------------

public class WhatCameThroughTheCanopyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1005);
		SetName(L("What Came Through the Canopy"));
		SetType(QuestType.Sub);
		SetDescription(L("The thing that broke the firebreak crew works the canopy and drops on what passes beneath it. Make enough noise on the low ground to bring the Colimencia down, then kill it."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Bounty Hunter] Una"), "f_huevillage_58_3");

		AddObjective("makeNoise", L("Kill Tipio on the low ground"),
			new KillObjective(25, new[] { MonsterId.Tipio }));

		AddObjective("killColimencia", L("Defeat the Colimencia"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Colimencia, 1) },
				resetIdent: "makeNoise",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(39000, 27000));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(223107, 1)); // Otrava Shield
		AddReward(new ItemReward(640085, 3)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}
}
