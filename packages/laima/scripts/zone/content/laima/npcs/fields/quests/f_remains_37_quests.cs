//--- Melia Script ----------------------------------------------------------
// Stele Road Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Stele Road.
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

public class FRemains37QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Stub Tree Clearance
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Stele-Ward] Eimantas"), "f_remains_37", 1400, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1001);

			dialog.SetTitle(L("Eimantas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Stub Trees crowd the Stele Road. Thirty-five clears the pilgrim path to the stones."));

				var response = await dialog.Select(L("Clear the road?"),
					Option(L("I'll kill"), "help"),
					Option(L("Why tend the steles?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thirty-five. They ambush in clusters."));
						break;

					case "info":
						await dialog.Msg(L("Old standing-stones. Pilgrims come to read the runes."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims pay tolls."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStubs", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Road's open. Pilgrims through."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep clearing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Stele road's busy. Good work."));
			}
		});

		// Quest 2: Tama Husks
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Archivist] Skaiste"), "f_remains_37", 700, 2700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1002);

			dialog.SetTitle(L("Skaiste"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tama husks preserve parchment. Kill thirty and bring seven clean husks."));

				var response = await dialog.Select(L("Husks?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Why Tama?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Whole husks only."));
						break;

					case "info":
						await dialog.Msg(L("The husk-lining absorbs damp. Keeps scrolls a century."));
						break;

					case "leave":
						await dialog.Msg(L("Archive will rot."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTamas", out var killObj)) return;
				if (!quest.TryGetProgress("gatherHusks", out var hObj)) return;

				if (killObj.Done && hObj.Done)
				{
					await dialog.Msg(L("Seven husks. Archive's safe."));
					character.Inventory.Remove(650094, character.Inventory.CountItem(650094), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep gathering."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Archive stable another decade."));
			}
		});

		// Quest 3: TreeAmbulo Bark
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Bark-Binder] Audrius"), "f_remains_37", 500, -1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1003);

			dialog.SetTitle(L("Audrius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("TreeAmbulo bark binds book covers. Thirty kills, six prime planks."));

				var response = await dialog.Select(L("Bark binding?"),
					Option(L("I'll bring"), "help"),
					Option(L("Why Ambulo?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Chest sections. Thick as a hand."));
						break;

					case "info":
						await dialog.Msg(L("Lives and walks - the bark's alive. Preserves ink like nothing else."));
						break;

					case "leave":
						await dialog.Msg(L("Old books die without it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killAmbulos", out var killObj)) return;
				if (!quest.TryGetProgress("gatherBark", out var bObj)) return;

				if (killObj.Done && bObj.Done)
				{
					await dialog.Msg(L("Six planks. Covers start tomorrow."));
					character.Inventory.Remove(650098, character.Inventory.CountItem(650098), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Bound books shipped to the Orsha library."));
			}
		});

		// Quest 4: Root Crystals
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Crystalwright] Gabija"), "f_remains_37", 600, -200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1004);

			dialog.SetTitle(L("Gabija"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Stele Road Rootcrystals hum with old mana. Break twelve and bring eight shards."));

				var response = await dialog.Select(L("Shards?"),
					Option(L("I'll break them"), "help"),
					Option(L("Old mana?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Eight. They hum when they're ready."));
						break;

					case "info":
						await dialog.Msg(L("The road's ley-line is the oldest on the continent."));
						break;

					case "leave":
						await dialog.Msg(L("Mana's wasted."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("gatherShards", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Eight shards. Commissions booked."));
					character.Inventory.Remove(650235, character.Inventory.CountItem(650235), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Commissions delivered. Collectors happy."));
			}
		});

		// Quest 5: The Ancient Stub
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Druid] Daina"), "f_remains_37", -600, -2300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1005);
			var ancientSpawnedKey = "Laima.Quests.f_remains_37.Quest1005.AncientSpawned";

			dialog.SetTitle(L("Daina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("An Ancient Stub root-walks the deep grove. Thin its saplings and it emerges."));

				var response = await dialog.Select(L("Draw it out?"),
					Option(L("I'll face it"), "help"),
					Option(L("How old?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten. It feels each."));
						break;

					case "info":
						await dialog.Msg(L("Pre-Medzio. Centuries old."));
						break;

					case "leave":
						await dialog.Msg(L("Grove rots if it stays."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStubs", out var sObj)) return;
				if (!quest.TryGetProgress("killAncient", out var aObj)) return;

				if (sObj.Done && aObj.Done)
				{
					await dialog.Msg(L("Ancient's fallen. Grove breathes."));
					character.Variables.Perm.Remove(ancientSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (sObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(ancientSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(ancientSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Stub_Tree, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("It walks!"));
							character.ServerMessage(L("{#FF9966}The Ancient Stub uproots itself!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find it before it takes new root."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Grove's alive again."));
			}
		});

		// Quest 6: Stele Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Master] Laimonas"), "f_remains_37", 900, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_37", 1006);

			dialog.SetTitle(L("Laimonas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Stele sweep. Twelve Stubs, twelve Tamas, twelve Ambulos."));

				var response = await dialog.Select(L("Three species?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thirty-six total. Standard."));
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan rolls."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStubs", out var sObj)) return;
				if (!quest.TryGetProgress("killTamas", out var tObj)) return;
				if (!quest.TryGetProgress("killAmbulos", out var aObj)) return;

				if (sObj.Done && tObj.Done && aObj.Done)
				{
					await dialog.Msg(L("Sweep done."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep going."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravan through."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FRemains37Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1001);
		SetName(L("Stub Tree Clearance"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Stub Trees blocking the Stele Road."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stele-Ward] Eimantas"), "f_remains_37");

		AddObjective("killStubs", L("Kill Stub Trees"),
			new KillObjective(35, new[] { MonsterId.Stub_Tree }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
	}
}

public class FRemains37Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1002);
		SetName(L("Tama Preservation Husks"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tamas and bring husks for archive preservation."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Archivist] Skaiste"), "f_remains_37");

		AddObjective("killTamas", L("Kill Tamas"),
			new KillObjective(30, new[] { MonsterId.Tama }));

		AddObjective("gatherHusks", L("Gather preservation husks"),
			new CollectItemObjective(650094, 7));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
		AddReward(new ItemReward(640011, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650094, character.Inventory.CountItem(650094), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650094, character.Inventory.CountItem(650094), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains37Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1003);
		SetName(L("Bark-Plank Bindings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill TreeAmbulos and bring bark-planks for book covers."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bark-Binder] Audrius"), "f_remains_37");

		AddObjective("killAmbulos", L("Kill TreeAmbulos"),
			new KillObjective(30, new[] { MonsterId.TreeAmbulo }));

		AddObjective("gatherBark", L("Gather bark-planks"),
			new CollectItemObjective(650098, 6));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
		AddReward(new ItemReward(640011, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650098, character.Inventory.CountItem(650098), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650098, character.Inventory.CountItem(650098), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains37Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1004);
		SetName(L("Humming Shards"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Stele Road Rootcrystals and bring humming shards."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystalwright] Gabija"), "f_remains_37");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(12, new[] { MonsterId.Rootcrystal_01 }));

		AddObjective("gatherShards", L("Gather humming shards"),
			new CollectItemObjective(650235, 8));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
		AddReward(new ItemReward(640011, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650235, character.Inventory.CountItem(650235), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650235, character.Inventory.CountItem(650235), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains37Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1005);
		SetName(L("The Ancient Stub"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Stub Trees to draw out the pre-Medzio Ancient."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Druid] Daina"), "f_remains_37");

		AddObjective("killStubs", L("Kill Stub Trees"),
			new KillObjective(10, new[] { MonsterId.Stub_Tree }));

		AddObjective("killAncient", L("Defeat the Ancient Stub"),
			new KillObjective(1, new[] { MonsterId.Stub_Tree }));

		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(6000));
		AddReward(new ItemReward(640082, 3));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
		AddReward(new ItemReward(640011, 1));
	}
}

public class FRemains37Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_37", 1006);
		SetName(L("Stele Road Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Stub Trees, Tamas, and TreeAmbulos."));
		SetLocation("f_remains_37");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Laimonas"), "f_remains_37");

		AddObjective("killStubs", L("Kill Stub Trees"),
			new KillObjective(12, new[] { MonsterId.Stub_Tree }));

		AddObjective("killTamas", L("Kill Tamas"),
			new KillObjective(12, new[] { MonsterId.Tama }));

		AddObjective("killAmbulos", L("Kill TreeAmbulos"),
			new KillObjective(12, new[] { MonsterId.TreeAmbulo }));

		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(6000));
		AddReward(new ItemReward(640082, 3));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
		AddReward(new ItemReward(640011, 1));
	}
}
