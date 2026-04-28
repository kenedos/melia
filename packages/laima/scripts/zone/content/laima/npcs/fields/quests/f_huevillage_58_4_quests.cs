//--- Melia Script ----------------------------------------------------------
// Septyni Glen Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Septyni Glen.
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

public class FHuevillage584QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Carcashu Shellfall
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Glen-Ward] Saulius"), "f_huevillage_58_4", -78, -129, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1001);

			dialog.SetTitle(L("Saulius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Carcashus swarm the glen paths. Forty kills opens the paths again."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll clear"), "help"),
					Option(L("Why forty?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty."));
						break;

					case "info":
						await dialog.Msg(L("They lay eggs fast."));
						break;

					case "leave":
						await dialog.Msg(L("Glen closes."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCarcashus", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Paths clear."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Glen walks open."));
			}
		});

		// Quest 2: Beeteros Wings
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Apothecary] Milda"), "f_huevillage_58_4", -9, -705, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1002);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Beeteros wings steep into an energy tonic. Kill fifteen, bring six wings."));

				var response = await dialog.Select(L("Tonic?"),
					Option(L("I'll bring"), "help"),
					Option(L("Does it work?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Whole wings. Don't tear."));
						break;

					case "info":
						await dialog.Msg(L("Farmhands swear by it."));
						break;

					case "leave":
						await dialog.Msg(L("Farmhands exhausted."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBeeteros", out var killObj)) return;
				if (!quest.TryGetProgress("gatherWings", out var wObj)) return;

				if (killObj.Done && wObj.Done)
				{
					await dialog.Msg(L("Six wings. Tonic brews."));
					character.Inventory.Remove(650099, character.Inventory.CountItem(650099), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Tonic's on every shelf."));
			}
		});

		// Quest 3: Mentiwood Sap
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Carpenter] Zigmas"), "f_huevillage_58_4", 1219, -565, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1003);

			dialog.SetTitle(L("Zigmas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Mentiwood sap seals wood better than pitch. Kill twenty, bring six sap-vials."));

				var response = await dialog.Select(L("Sap?"),
					Option(L("I'll bring"), "help"),
					Option(L("Why not pitch?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Vial at the sap-drip."));
						break;

					case "info":
						await dialog.Msg(L("Sap binds the grain. Keeps boards from warping."));
						break;

					case "leave":
						await dialog.Msg(L("Boards warp."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMentiwoods", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSap", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Six vials. Barn roof sealed."));
					character.Inventory.Remove(650103, character.Inventory.CountItem(650103), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Barn stands through winter."));
			}
		});

		// Quest 4: Tiny Mage Reagents
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Mage-Tutor] Gerda"), "f_huevillage_58_4", -1086, -660, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1004);

			dialog.SetTitle(L("Gerda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tiny Mages carry reagent-pouches. Kill twelve, bring five pouches for my students."));

				var response = await dialog.Select(L("Pouches?"),
					Option(L("I'll bring"), "help"),
					Option(L("Students need?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Intact pouches only."));
						break;

					case "info":
						await dialog.Msg(L("Starter kits. Hard to make from scratch."));
						break;

					case "leave":
						await dialog.Msg(L("Students cast nothing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPouches", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Five pouches. Students casting."));
					character.Inventory.Remove(650109, character.Inventory.CountItem(650109), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Class progressing. Two apprentices ready."));
			}
		});

		// Quest 5: The Beetow Queen
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Hive-Hunter] Nikas"), "f_huevillage_58_4", 1254, -894, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1005);
			var queenSpawnedKey = "Laima.Quests.f_huevillage_58_4.Quest1005.QueenSpawned";

			dialog.SetTitle(L("Nikas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Beetow Queen nests underground. Kill ten Beetows to force her surface."));

				var response = await dialog.Select(L("Queen?"),
					Option(L("I'll force her"), "help"),
					Option(L("How big?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten. She surfaces to lay new eggs when the drones thin."));
						break;

					case "info":
						await dialog.Msg(L("Triple size. Stinger as long as a blade."));
						break;

					case "leave":
						await dialog.Msg(L("Hive expands."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBeetows", out var bObj)) return;
				if (!quest.TryGetProgress("killQueen", out var qObj)) return;

				if (bObj.Done && qObj.Done)
				{
					await dialog.Msg(L("Queen's down. Hive collapses."));
					character.Variables.Perm.Remove(queenSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (bObj.Done && !qObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(queenSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(queenSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Beetow, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She surfaces!"));
							character.ServerMessage(L("{#FF9966}The Beetow Queen erupts from her tunnel!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find her before she burrows."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten drones first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Glen's quiet. Hive gone."));
			}
		});

	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FHuevillage584Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1001);
		SetName(L("Carcashu Shellfall"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Carcashus choking the Septyni glen paths."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Glen-Ward] Saulius"), "f_huevillage_58_4");

		AddObjective("killCarcashus", L("Kill Carcashus"),
			new KillObjective(40, new[] { MonsterId.Carcashu }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FHuevillage584Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1002);
		SetName(L("Energy Tonic Wings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Beeteros and bring wings for the apothecary's tonic."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Apothecary] Milda"), "f_huevillage_58_4");

		AddObjective("killBeeteros", L("Kill Beeteros"),
			new KillObjective(15, new[] { MonsterId.Beeteros }));

		AddObjective("gatherWings", L("Gather whole wings"),
			new CollectItemObjective(650099, 6));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));

		AddDrop(650099, 0.45f, MonsterId.Beeteros);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650099, character.Inventory.CountItem(650099), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650099, character.Inventory.CountItem(650099), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FHuevillage584Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1003);
		SetName(L("Mentiwood Sap"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Mentiwoods and bring sap-vials for the carpenter."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Carpenter] Zigmas"), "f_huevillage_58_4");

		AddObjective("killMentiwoods", L("Kill Mentiwoods"),
			new KillObjective(20, new[] { MonsterId.Mentiwood }));

		AddObjective("gatherSap", L("Gather sap-vials"),
			new CollectItemObjective(650103, 6));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));

		AddDrop(650103, 0.40f, MonsterId.Mentiwood);
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

public class FHuevillage584Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1004);
		SetName(L("Apprentice Reagents"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tiny Mages and bring reagent-pouches for mage students."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mage-Tutor] Gerda"), "f_huevillage_58_4");

		AddObjective("killMages", L("Kill Tiny Mages"),
			new KillObjective(12, new[] { MonsterId.Tiny_Mage }));

		AddObjective("gatherPouches", L("Gather reagent-pouches"),
			new CollectItemObjective(650109, 5));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));

		AddDrop(650109, 0.50f, MonsterId.Tiny_Mage);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650109, character.Inventory.CountItem(650109), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650109, character.Inventory.CountItem(650109), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FHuevillage584Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1005);
		SetName(L("The Beetow Queen"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Beetows to force the Queen to surface."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hive-Hunter] Nikas"), "f_huevillage_58_4");

		AddObjective("killBeetows", L("Kill Beetows"),
			new KillObjective(10, new[] { MonsterId.Beetow }));

		AddObjective("killQueen", L("Defeat the Beetow Queen"),
			new KillObjective(1, new[] { MonsterId.Beetow }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}

