//--- Melia Script ----------------------------------------------------------
// Grynas Trails - Quest NPCs
//--- Description -----------------------------------------------------------
// Restless ghosts of those who died on the Grynas Trails.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.Scripting.Dialogues;

public class FKatyn451QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: Trail-Courier killed by archers
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Trail-Courier"), "f_katyn_45_1", -208, 939, 135, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1001);
			dialog.SetTitle(L("Trail-Courier"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The whistle... I heard the whistle twice... I should have moved..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Stoulet Archers... Blind bend... Two whistles from the same direction... Range-fire..."));
					await dialog.Msg(L("My pouch was full of letters... All for the southern post... All wet with my own blood..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will avenge you"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Twenty Archers... Twenty... So no other courier... Hears the whistle twice...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Twenty Archers... Twenty... So no other courier... Hears the whistle twice..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArcher", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("The bends are quiet... Take this courier's purse..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More whistles... More whistles in the wood..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Quiet bends... Couriers walk in pairs now... I rest..."));
			}
		});

		// Quest 1002 giver: Tracker-Elder ghost
		//-------------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Tracker-Elder"), "f_katyn_45_1", 388, -129, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1002);
			dialog.SetTitle(L("Tracker-Elder"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The book... My trail-book... Forty years... I never delivered it..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("My apprentice waits at the shrine... He waits forty years... Without the book... He learned the looped paths the hard way..."));
					await dialog.Msg(L("I never reached him... A wrong-fork took me... A wrong-fork I had warned others against..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will deliver the book"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Tautvydas... At the shrine... Carry the book... Don't open it on the way... Don't...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Tautvydas... At the shrine... Carry the book... Don't open it on the way... Don't..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverBook", out var deliverObj)) return;
				if (deliverObj.Done)
				{
					await dialog.Msg(L("He read it... He marked the west-fork... He guessed right... I can rest..."));
					await dialog.Msg(L("Take this elder's coin... My grey cord... You earned it..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("The shrine... Tautvydas waits... Forty years..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The trails have a tracker... Both of us can rest now... Both..."));
			}
		});

		// Quest 1002 recipient: Apprentice-Tracker ghost in front of shrine to the dead
		//-------------------------------------------------------------------------
		AddGhostNpc(155132, L("[Restless Soul] Apprentice-Tracker"), "f_katyn_45_1", 347, -559, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1002);
			dialog.SetTitle(L("Apprentice-Tracker"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_1.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("The shrine... I have stood at the shrine forty years... Forty years she did not come..."));
					await dialog.Msg(L("The lights... The candle-statues never go out... The dead are kind here..."));
					await dialog.Msg(L("The book... You bring the book... At last... At last..."));
					await dialog.Msg(L("I read it standing... West-fork is the looped one... I knew it... I knew it before I knew anything..."));
					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_1.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Trail-book delivered. Return to the Tracker-Elder.{/}"));
				}
				else await dialog.Msg(L("Tell her... Tell her I guessed right... I will leave the shrine soon..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The shrine fades behind me... I never thought I would walk away from it..."));
			}
			else
			{
				await dialog.Msg(L("Statues... Statues to the dead... I am one of them... I am one of them..."));
			}
		});

		// Quest 1003: Wards-Stitcher ghost
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Wards-Stitcher"), "f_katyn_45_1", -985, -1004, 44, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1003);
			dialog.SetTitle(L("Wards-Stitcher"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Silence... I sewed silence... I died in silence... No one heard..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Sockets stepped from between two trunks... I never heard them... My own stitches turned against me..."));
					await dialog.Msg(L("Eight hood-patches I had cut... I would have sewn them into linings... My needle still in my hand..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will finish your work"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Eight patches... Cut at the seam... Not the fabric... So the silence... Saves a warden's life...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Eight patches... Cut at the seam... Not the fabric... So the silence... Saves a warden's life..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var patchCount = character.Inventory.CountItem(650853);
				if (patchCount >= 8)
				{
					await dialog.Msg(L("Eight... Stitched clean... My linings will reach the wardens at last..."));
					await dialog.Msg(L("Take this stitcher's coin... My silence-cloth scrap... For your boots..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Cut at the seam... Eight... Eight..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The wardens walk silent now... So do I... So do I..."));
			}
		});

		// Quest 1004: Path-Warden ghost
		//-------------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Path-Warden"), "f_katyn_45_1", -2012, 292, 135, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1004);
			dialog.SetTitle(L("Path-Warden"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The cairn pointed at me... The cairn pointed at me... I should have walked opposite..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("West-fork cairn... Tip-stone pointed back at me... A trap, not a cairn... I should have walked opposite..."));
					await dialog.Msg(L("I tried to fix it... I touched the stones... The wood took me into the loop... Three days... Then nothing..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will read the cairns for you"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Four cairns... Compass on each tip-stone... Don't touch... Don't touch... Tell me which point where...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Four cairns... Compass on each tip-stone... Don't touch... Don't touch... Tell me which point where..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var cairnsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked", 0);
				if (cairnsChecked >= 4)
				{
					await dialog.Msg(L("Two correct... One turned... One trap... I will warn the next warden... Through the trees..."));
					await dialog.Msg(L("Take this warden's stipend... My compass-disc... Walk opposite to mirrors... To trap-cairns..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Four cairns... Don't touch... Just read..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The west-fork is roped off... I helped someone at last... I rest..."));
			}
		});

		// Trail-cairn inspection points (kept as inspection pattern)
		//-------------------------------------------------------------------------
		void AddTrailCairn(int cairnNumber, string cairnName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(cairnName), "f_katyn_45_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A small trail-cairn beside the fork*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_45_1.Quest1004.Cairn{cairnNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already read*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the tip-stone direction..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var cairnsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked", cairnsChecked + 1);
					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Cairns read: {0}/4", cairnsChecked + 1));
					if (cairnsChecked + 1 >= 4)
						character.ServerMessage(L("{#FFD700}All cairns read! Return to the Path-Warden.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddTrailCairn(1, "North-Fork Cairn", "North-Fork: tip-stone north-east. Correct.", -66, 413, 0);
		AddTrailCairn(2, "East-Fork Cairn", "East-Fork: tip-stone north-east. Correct.", 647, 300, 90);
		AddTrailCairn(3, "South-Fork Cairn", "South-Fork: tip-stone due south. Turned. Sends walkers into the loop.", 330, -595, 180);
		AddTrailCairn(4, "West-Fork Cairn", "West-Fork: tip-stone points back at the reader. A trap, not a cairn.", -435, 437, 270);

		// Quest 1005: Treasure-Seeker ghost — died at the chest
		//-------------------------------------------------------------------------
		AddGhostNpc(155132, L("[Restless Soul] Treasure-Seeker"), "f_katyn_45_1", -541, 663, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_1", 1005);
			dialog.SetTitle(L("Treasure-Seeker"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The chest... The chest is right there... I died reaching for it..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("I followed an old map... Sockets guarded the approach... I cut through them... My hand on the lid..."));
					await dialog.Msg(L("The last Socket... Behind me... I never saw the blow... My fingers slid off the lid... My fingers..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will clear the path for the next seeker"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Twenty-five Sockets... Cut them all... So my hand... My hand finally rests on the lid... Through another's fingers...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Twenty-five Sockets... Cut them all... So my hand... My hand finally rests on the lid... Through another's fingers..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSockets", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("The path... The path is clear at last... A seeker after me will reach the lid..."));
					await dialog.Msg(L("Take this... A seeker's purse... I never spent it... Spend it for me..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More Sockets... Behind every trunk..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("My fingers feel the lid... At last... At last..."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class TheArchersOnTheBendsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1001);
		SetName("The Trail-Courier's Whistle");
		SetType(QuestType.Sub);
		SetDescription("A courier's ghost begs for the Stoulet Archers that ranged him at the blind bend to be put down.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Trail-Courier", "f_katyn_45_1");
		AddObjective("killArcher", "Defeat Brown Stoulet Archers",
			new KillObjective(20, new[] { MonsterId.Stoulet_Bow_Blue }));
		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class TheTrailBookQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1002);
		SetName("The Trail-Book");
		SetType(QuestType.Sub);
		SetDescription("A tracker-elder's ghost begs you to deliver her trail-book to her apprentice's ghost waiting at the shrine.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Tracker-Elder", "f_katyn_45_1");
		AddObjective("deliverBook", "Deliver the trail-book to the Apprentice's ghost",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_1.Quest1002.Delivered", 1, true));
		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_1.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_1.Quest1002.Delivered");
	}
}

public class SocketHoodPatchesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1003);
		SetName("The Stitcher's Patches");
		SetType(QuestType.Sub);
		SetDescription("A wards-stitcher's ghost begs for eight Socket hood-patches so her unfinished cloak-linings may save the wardens she could not.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Wards-Stitcher", "f_katyn_45_1");
		AddObjective("collectPatches", "Cut hood-patches from Socket cloaks",
			new CollectItemObjective(650853, 8));
		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));

		AddDrop(650853, 0.40f, MonsterId.Socket_Mage_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650853, character.Inventory.CountItem(650853), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650853, character.Inventory.CountItem(650853), InventoryItemRemoveMsg.Destroyed);
	}
}

public class WhichCairnsWalkedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1004);
		SetName("Which Cairns Walked");
		SetType(QuestType.Sub);
		SetDescription("A path-warden's ghost begs for the four trail-fork cairns to be read so the next warden does not die at the trap-cairn he did.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Path-Warden", "f_katyn_45_1");
		AddObjective("readCairns", "Read the four trail-fork cairns",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked", 4, true));
		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1004.Cairn{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_1.Quest1004.CairnsChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_1.Quest1004.Cairn{i}");
	}
}

public class TheTreasureSeekerQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_1", 1005);
		SetName("The Treasure-Seeker's Hand");
		SetType(QuestType.Sub);
		SetDescription("A treasure-seeker's ghost died with his hand on the chest's lid. He begs for the Sockets that ambushed him to be cleared so the next seeker reaches what he could not.");
		SetLocation("f_katyn_45_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Treasure-Seeker", "f_katyn_45_1");
		AddObjective("killSockets", "Defeat Sockets guarding the chest",
			new KillObjective(25, new[] { MonsterId.Socket_Mage_Green }));
		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}
