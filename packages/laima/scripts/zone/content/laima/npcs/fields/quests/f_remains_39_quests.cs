//--- Melia Script ----------------------------------------------------------
// Escanciu Village Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Escanciu Village.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
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

public class FRemains39QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Gravegolem Graveyard
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Gravekeeper] Ema"), "f_remains_39", 1000, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1001);

			dialog.SetTitle(L("Ema"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Gravegolems have risen in the east plots. Twenty-five need breaking before the ceremony."));

				var response = await dialog.Select(L("Break the golems?"),
					Option(L("I'll kill"), "help"),
					Option(L("Ceremony?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Shatter the cores. They won't rebuild."));
						break;

					case "info":
						await dialog.Msg(L("Village remembrance rite. Every year."));
						break;

					case "leave":
						await dialog.Msg(L("Villagers grieve anyway."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGolems", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Plots quiet. Ceremony tomorrow."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ceremony went well. Thank you."));
			}
		});

		// Quest 2: Zolem Cores
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Stonewright] Vilis"), "f_remains_39", 400, -500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1002);

			dialog.SetTitle(L("Vilis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Zolems carry earth-cores. Kill twenty, bring six clean cores for masonry binding."));

				var response = await dialog.Select(L("Cores?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Masonry?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Heart of the beast. Sift the rubble."));
						break;

					case "info":
						await dialog.Msg(L("Bind stone to stone. Stronger than mortar."));
						break;

					case "leave":
						await dialog.Msg(L("Wall crumbles without."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killZolems", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCores", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Six cores. Wall holds."));
					character.Inventory.Remove(650240, character.Inventory.CountItem(650240), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Wall stands a generation now."));
			}
		});

		// Quest 3: Hook Thorn
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Seamstress] Rima"), "f_remains_39", -500, 200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1003);

			dialog.SetTitle(L("Rima"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Hook thorns work as curved needles. Kill twenty Hooks, five prime thorns."));

				var response = await dialog.Select(L("Needles?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Curved?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Longest thorn from the hump."));
						break;

					case "info":
						await dialog.Msg(L("For stitching boots. Saddler can't work without."));
						break;

					case "leave":
						await dialog.Msg(L("Boots unmade."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHooks", out var killObj)) return;
				if (!quest.TryGetProgress("gatherThorns", out var tObj)) return;

				if (killObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Five thorns. Boots go out this week."));
					character.Inventory.Remove(650245, character.Inventory.CountItem(650245), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Dozen pairs of boots shipped."));
			}
		});

		// Quest 4: Flog Swarm
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Beekeeper-Monk] Ambrozijs"), "f_remains_39", -600, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1004);

			dialog.SetTitle(L("Ambrozijs"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Flying Flogs chase my bees from the flowers. Twenty-five kills and my hives recover."));

				var response = await dialog.Select(L("Save the bees?"),
					Option(L("I'll kill"), "help"),
					Option(L("Why Flogs?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Bless your blade."));
						break;

					case "info":
						await dialog.Msg(L("Territorial. Same flowers, same hours."));
						break;

					case "leave":
						await dialog.Msg(L("Monastery starves."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFlogs", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Hives bouncing back."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Honey flows again."));
			}
		});

		// Quest 5: The Hallowventor Menace
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Exorcist] Basilijs"), "f_remains_39", 200, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1005);
			var champSpawnedKey = "Laima.Quests.f_remains_39.Quest1005.ChampSpawned";

			dialog.SetTitle(L("Basilijs"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Hallowventor champion anchors the haunting. Kill his flock and he manifests."));

				var response = await dialog.Select(L("Exorcise?"),
					Option(L("I'll face him"), "help"),
					Option(L("Haunting?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten. Recite as you cut."));
						break;

					case "info":
						await dialog.Msg(L("Village dead won't rest while he rules."));
						break;

					case "leave":
						await dialog.Msg(L("Then the haunting spreads."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFlock", out var fObj)) return;
				if (!quest.TryGetProgress("killChamp", out var cObj)) return;

				if (fObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Champion dispelled. Village rests."));
					character.Variables.Perm.Remove(champSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (fObj.Done && !cObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(champSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(champSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Hallowventor, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He manifests!"));
							character.ServerMessage(L("{#FF9966}The Hallowventor Champion manifests!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him before he fades."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Village sleeps. Dreams again."));
			}
		});

		// Quest 6: Escanciu Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Village Elder] Augustas"), "f_remains_39", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_39", 1006);

			dialog.SetTitle(L("Augustas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Village patrol. Twelve Gravegolems, twelve Zolems, twelve Hooks."));

				var response = await dialog.Select(L("Full patrol?"),
					Option(L("I'll patrol"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thirty-six."));
						break;

					case "info":
						await dialog.Msg(L("Village fund. Paid fair."));
						break;

					case "leave":
						await dialog.Msg(L("Village pays another."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGolems", out var gObj)) return;
				if (!quest.TryGetProgress("killZolems", out var zObj)) return;
				if (!quest.TryGetProgress("killHooks", out var hObj)) return;

				if (gObj.Done && zObj.Done && hObj.Done)
				{
					await dialog.Msg(L("Patrol done. Village's safer."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep patrolling."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Another week of peace."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FRemains39Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1001);
		SetName(L("Gravegolem Graveyard"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Gravegolems risen in the east plots before the remembrance rite."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Gravekeeper] Ema"), "f_remains_39");

		AddObjective("killGolems", L("Kill Gravegolems"),
			new KillObjective(25, new[] { MonsterId.Gravegolem }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FRemains39Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1002);
		SetName(L("Earth-Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Zolems and gather earth-cores for masonry binding."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stonewright] Vilis"), "f_remains_39");

		AddObjective("killZolems", L("Kill Zolems"),
			new KillObjective(20, new[] { MonsterId.Zolem }));

		AddObjective("gatherCores", L("Gather earth-cores"),
			new CollectItemObjective(650240, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650240, character.Inventory.CountItem(650240), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650240, character.Inventory.CountItem(650240), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains39Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1003);
		SetName(L("Curved Needles"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hooks and gather thorns for seamstress needles."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Seamstress] Rima"), "f_remains_39");

		AddObjective("killHooks", L("Kill Hooks"),
			new KillObjective(20, new[] { MonsterId.Hook }));

		AddObjective("gatherThorns", L("Gather thorns"),
			new CollectItemObjective(650245, 5));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650245, character.Inventory.CountItem(650245), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650245, character.Inventory.CountItem(650245), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains39Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1004);
		SetName(L("Flog Swarm"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Flying Flogs raiding the monastery bee hives."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Beekeeper-Monk] Ambrozijs"), "f_remains_39");

		AddObjective("killFlogs", L("Kill Flying Flogs"),
			new KillObjective(25, new[] { MonsterId.Flying_Flog }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FRemains39Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1005);
		SetName(L("The Hallowventor Champion"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hallowventors to draw out their champion."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Exorcist] Basilijs"), "f_remains_39");

		AddObjective("killFlock", L("Kill Hallowventors"),
			new KillObjective(10, new[] { MonsterId.Hallowventor }));

		AddObjective("killChamp", L("Defeat the Champion"),
			new KillObjective(1, new[] { MonsterId.Hallowventor }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}

public class FRemains39Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_39", 1006);
		SetName(L("Village Patrol"));
		SetType(QuestType.Sub);
		SetDescription(L("Patrol Escanciu Village killing Gravegolems, Zolems, and Hooks."));
		SetLocation("f_remains_39");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Village Elder] Augustas"), "f_remains_39");

		AddObjective("killGolems", L("Kill Gravegolems"),
			new KillObjective(12, new[] { MonsterId.Gravegolem }));

		AddObjective("killZolems", L("Kill Zolems"),
			new KillObjective(12, new[] { MonsterId.Zolem }));

		AddObjective("killHooks", L("Kill Hooks"),
			new KillObjective(12, new[] { MonsterId.Hook }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}
