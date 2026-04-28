//--- Melia Script ----------------------------------------------------------
// Poslinkis Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Restless ghosts of those who died in Poslinkis Forest.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.Scripting.Dialogues;

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

		// Quest 1001: Border-ward speared at the treeline
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Border-Ward"), "f_katyn_13", -413, -2306, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1001);
			dialog.SetTitle(L("Border-Ward"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Spears... Spears in the dark... I never saw them coming..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Treeline patrol... Hedgerow flank... High Bube Spears... From the bracken... So fast..."));
					await dialog.Msg(L("Three of them... All at once... I fell where I stand..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will avenge you"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Forty... Forty Spears... So no other ward... Falls in the bracken...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Forty... Forty Spears... So no other ward... Falls in the bracken..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpears", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("The treeline... Quiet at last... Take this... A ward's purse..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More Spears... I can still hear them... In the bracken..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Quiet... So quiet... Thank you... Thank you..."));
			}
		});

		// Quest 1002: Forester ghost killed by Archers
		//-------------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Pierced Forester"), "f_katyn_13", 1417, -826, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1002);
			dialog.SetTitle(L("Pierced Forester"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Arrows... Arrows in the rain... The pitch held the feathers..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("I tracked the Archer-caste... To learn their pitch-recipe... Resin and ash... For our stillroom..."));
					await dialog.Msg(L("They saw me first... Eight arrows... All eight held in the wet..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will get the recipe"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Thirty Archers... Eight fletchings... So our stillroom... Has the recipe at last...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Thirty Archers... Eight fletchings... So our stillroom... Has the recipe at last..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFletchings", out var fObj)) return;
				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Eight pitch-fletchings... The stillroom will crack the recipe... My work was not wasted..."));
					await dialog.Msg(L("Take this... Forester's purse... Damp from the rain... Honest..."));
					character.Inventory.Remove(650239, character.Inventory.CountItem(650239), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More Archers... More fletchings... The pitch must hold..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The stillroom... I can almost smell the resin from here... Almost..."));
			}
		});

		// Quest 1003: Herbwife ghost dead in butterfly bush
		//-------------------------------------------------------------------------
		AddGhostNpc(155132, L("[Restless Soul] Poisoned Herbwife"), "f_katyn_13", -1102, 2448, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1003);
			dialog.SetTitle(L("Poisoned Herbwife"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("This bush... I died in this bush... The butterflies came after... So many butterflies..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("I followed a Pokubu Green... To study its tongue... For my salve... For the dose..."));
					await dialog.Msg(L("It turned... It turned and bit me... My own tongue swelled green... I crawled into this bush... I never crawled out..."));
					await dialog.Msg(L("The butterflies... They settle on me... They have settled on me for years..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will finish your salve"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Twenty-five Pokubu... Six tongues... Wrap them in oiled cloth... So my salve... Reaches the village...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Twenty-five Pokubu... Six tongues... Wrap them in oiled cloth... So my salve... Reaches the village..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPokubu", out var killObj)) return;
				if (!quest.TryGetProgress("gatherTongues", out var tObj)) return;
				if (killObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Six green tongues... The salve will reach the village... My work... My work finishes..."));
					await dialog.Msg(L("Take this herbwife's purse... The butterflies will follow you out... I do not need them anymore..."));
					character.Inventory.Remove(650241, character.Inventory.CountItem(650241), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More tongues... Six tongues... For the salve..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The butterflies are leaving... So am I... So am I..."));
			}
		});

		// Quest 1005: Cemetery ghost — Fallen Statue boss bounty
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Cemetery Hunter"), "f_katyn_13", 766, 1011, 89, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1005);
			dialog.SetTitle(L("Cemetery Hunter"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Graves... Graves in front of me... My grave is among them... I do not know which..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Bounty hunter... I tracked a Fallen Statue to the old shrine... The stone remembered me... It always remembers..."));
					await dialog.Msg(L("Bushspiders at the approach... Then the Statue rose... I fell at its feet... They buried me here... With the others... With the others..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will end the Statue"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Ten Bushspiders first... Then the Statue rises... End it... End it... So I may know which grave is mine...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Ten Bushspiders first... Then the Statue rises... End it... End it... So I may know which grave is mine..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpiders", out var pObj)) return;
				if (!quest.TryGetProgress("killStatue", out var sObj)) return;
				if (sObj.Done)
				{
					await dialog.Msg(L("The Statue is broken... I can see my grave now... The third from the gate... The third from the gate..."));
					await dialog.Msg(L("Take this purse... I will lay myself in my own grave at last..."));
					character.Inventory.Remove(650243, character.Inventory.CountItem(650243), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done) await dialog.Msg(L("It rises... It rises... Find it... End it..."));
				else await dialog.Msg(L("Bushspiders first... Bring back a shrine-fragment..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("My grave... I know it now... I will rest..."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FKatyn13Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1001);
		SetName(L("The Border-Ward's Spears"));
		SetType(QuestType.Sub);
		SetDescription(L("A speared border-ward's ghost begs for the High Bube Spears that killed him to be put down."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Border-Ward"), "f_katyn_13");

		AddObjective("killSpears", L("Kill High Bube Spears"),
			new KillObjective(40, new[] { MonsterId.HighBube_Spear }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FKatyn13Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1002);
		SetName(L("The Pierced Forester"));
		SetType(QuestType.Sub);
		SetDescription(L("A forester's ghost begs for the High Bube Archers' pitch-fletchings to finish his stillroom work."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Pierced Forester"), "f_katyn_13");

		AddObjective("killArchers", L("Kill High Bube Archers"),
			new KillObjective(30, new[] { MonsterId.HighBube_Archer }));

		AddObjective("gatherFletchings", L("Gather pitch-fletchings"),
			new CollectItemObjective(650239, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650239, 0.40f, MonsterId.HighBube_Archer);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650239, character.Inventory.CountItem(650239), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650239, character.Inventory.CountItem(650239), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn13Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1003);
		SetName(L("The Poisoned Herbwife"));
		SetType(QuestType.Sub);
		SetDescription(L("An herbwife's ghost in a butterfly bush begs for six Pokubu Green tongues so her unfinished salve may reach the village."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Poisoned Herbwife"), "f_katyn_13");

		AddObjective("killPokubu", L("Kill Arburn Pokubu Greens"),
			new KillObjective(25, new[] { MonsterId.Arburn_Pokubu_Green }));

		AddObjective("gatherTongues", L("Gather green tongues"),
			new CollectItemObjective(650241, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650241, 0.45f, MonsterId.Arburn_Pokubu_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650241, character.Inventory.CountItem(650241), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650241, character.Inventory.CountItem(650241), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn13Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1005);
		SetName(L("The Cemetery Hunter"));
		SetType(QuestType.Sub);
		SetDescription(L("A bounty hunter's ghost in the cemetery begs for the Fallen Statue that killed him to be ended at last."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Restless Soul] Cemetery Hunter"), "f_katyn_13");

		AddObjective("killSpiders", L("Kill Bushspiders at the shrine approach"),
			new KillObjective(10, new[] { MonsterId.Bushspider }));

		AddObjective("killStatue", L("Defeat the Fallen Statue"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Fallen_Statue, 1) },
				resetIdent: "killSpiders",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650243, character.Inventory.CountItem(650243), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650243, character.Inventory.CountItem(650243), InventoryItemRemoveMsg.Destroyed);
	}
}
