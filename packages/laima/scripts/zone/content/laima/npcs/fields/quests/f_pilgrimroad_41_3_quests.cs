//--- Melia Script ----------------------------------------------------------
// Pilgrim Road Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the Salvia pilgrim road, overrun by Minos and Lapasape.
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

public class FPilgrimroad413QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Clear the Road
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Pilgrim-Warden] Brone"), "f_pilgrimroad_41_3", -900, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1001);

			dialog.SetTitle(L("Brone"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Minos packs choke the Salvia road. Pilgrims can't pass. Kill forty Green Minos and the caravan moves tonight."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Caravan?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. The road opens step by step."));
						break;

					case "info":
						await dialog.Msg(L("Grain wagons. Relic boxes. Folk on foot. All stopped a week."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims keep waiting."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Road's walkable. Caravan rolls at dawn."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First wagon through. Pilgrims blessed your name."));
			}
		});

		// Quest 2: Bow-Minos Quivers
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Road-Marshal] Ysanne"), "f_pilgrimroad_41_3", 200, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1002);

			dialog.SetTitle(L("Ysanne"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bow-Minos pick pilgrims from the ridges. Kill twenty-five, bring eight quivers for the militia armoury."));

				var response = await dialog.Select(L("Quivers?"),
					Option(L("I'll bring"), "help"),
					Option(L("Ridges?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Eight quivers. Intact if you can."));
						break;

					case "info":
						await dialog.Msg(L("North ridge, east ridge. They shoot from cover. Close the distance fast."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims keep falling."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBows", out var killObj)) return;
				if (!quest.TryGetProgress("gatherQuivers", out var qObj)) return;

				if (killObj.Done && qObj.Done)
				{
					await dialog.Msg(L("Eight quivers. Militia scouts arm up tonight."));
					character.Inventory.Remove(650251, character.Inventory.CountItem(650251), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Scouts arrow the ridges clean each morning now."));
			}
		});

		// Quest 3: Lapasape Grimoires
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Cantor] Ilse-II"), "f_pilgrimroad_41_3", -1000, -300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1003);

			dialog.SetTitle(L("Ilse"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Brown Lapasape Mages hex the pilgrim wells. Kill thirty, bring six hex-grimoires for the shrine."));

				var response = await dialog.Select(L("Grimoires?"),
					Option(L("I'll bring"), "help"),
					Option(L("Wells?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't read the margins."));
						break;

					case "info":
						await dialog.Msg(L("Pilgrims drink, pilgrims sicken. Grimoires let the shrine trace the hex and break it."));
						break;

					case "leave":
						await dialog.Msg(L("Wells stay poisoned."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherGrimoires", out var gObj)) return;

				if (killObj.Done && gObj.Done)
				{
					await dialog.Msg(L("Six grimoires. Shrine breaks the hex by matins."));
					character.Inventory.Remove(650252, character.Inventory.CountItem(650252), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two wells cleansed. Four to go."));
			}
		});

		// Quest 4: Crystal Breaking
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Road-Mason] Caelum"), "f_pilgrimroad_41_3", 800, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1004);

			dialog.SetTitle(L("Caelum"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Rootcrystals have punched through the pilgrim path. Break twenty before the road splits."));

				var response = await dialog.Select(L("Break?"),
					Option(L("I'll break"), "help"),
					Option(L("Split?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Mind your feet. They shatter sharp."));
						break;

					case "info":
						await dialog.Msg(L("Another fortnight and the flagstones crack straight through. Salvia loses the route."));
						break;

					case "leave":
						await dialog.Msg(L("Road will split."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Twenty down. Masons re-lay the stones tomorrow."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Flagstones relaid. Pilgrims walk steady."));
			}
		});

		// Quest 5: The Minos Warchief
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Doran-III"), "f_pilgrimroad_41_3", -800, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1005);
			var warchiefSpawnedKey = "Laima.Quests.f_pilgrimroad_41_3.Quest1005.WarchiefSpawned";

			dialog.SetTitle(L("Doran"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Minos warchief drives the Salvia packs. Kill twelve Green Minos to bait him out, then end him."));

				var response = await dialog.Select(L("Warchief?"),
					Option(L("I'll face him"), "help"),
					Option(L("Bait?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve. He won't stay hidden long."));
						break;

					case "info":
						await dialog.Msg(L("He watches. Kill enough of his pack, pride drags him out."));
						break;

					case "leave":
						await dialog.Msg(L("Packs keep their chief."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killWarchief", out var wObj)) return;

				if (pObj.Done && wObj.Done)
				{
					await dialog.Msg(L("Warchief down. Packs scatter by morning."));
					character.Variables.Perm.Remove(warchiefSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !wObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(warchiefSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(warchiefSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Minos_Green, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Minos Warchief storms the pilgrim road!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else
				{
					await dialog.Msg(L("Twelve first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Packs without a chief. Road holds."));
			}
		});

		// Quest 6: Pilgrim Road Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Marek"), "f_pilgrimroad_41_3", 1000, 900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_3", 1006);

			dialog.SetTitle(L("Marek"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Full sweep. Twelve Green Minos, twelve Bow-Minos, twelve Brown Lapasape Mages."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
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
						await dialog.Msg(L("Salvia coin. Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays contested."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMinos", out var mObj)) return;
				if (!quest.TryGetProgress("killBows", out var bObj)) return;
				if (!quest.TryGetProgress("killMages", out var gObj)) return;

				if (mObj.Done && bObj.Done && gObj.Done)
				{
					await dialog.Msg(L("Done."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep going."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Militia patrols the pilgrim road hourly now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FPilgrimroad413Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1001);
		SetName(L("Clear the Road"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Minos choking the Salvia pilgrim road."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim-Warden] Brone"), "f_pilgrimroad_41_3");

		AddObjective("killMinos", L("Kill Green Minos"),
			new KillObjective(40, new[] { MonsterId.Minos_Green }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FPilgrimroad413Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1002);
		SetName(L("Bow-Minos Quivers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bow-Minos and bring quivers for the militia armoury."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Marshal] Ysanne"), "f_pilgrimroad_41_3");

		AddObjective("killBows", L("Kill Bow-Minos"),
			new KillObjective(25, new[] { MonsterId.Minos_Bow_Green }));

		AddObjective("gatherQuivers", L("Gather Bow-Minos quivers"),
			new CollectItemObjective(650251, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650251, character.Inventory.CountItem(650251), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650251, character.Inventory.CountItem(650251), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad413Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1003);
		SetName(L("Hex-Grimoires"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Lapasape Mages and bring hex-grimoires for the shrine."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Cantor] Ilse"), "f_pilgrimroad_41_3");

		AddObjective("killMages", L("Kill Brown Lapasape Mages"),
			new KillObjective(30, new[] { MonsterId.Lapasape_Mage_Brown }));

		AddObjective("gatherGrimoires", L("Gather hex-grimoires"),
			new CollectItemObjective(650252, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650252, character.Inventory.CountItem(650252), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650252, character.Inventory.CountItem(650252), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad413Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1004);
		SetName(L("Crystal Breaking"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Rootcrystals splitting the pilgrim road."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Mason] Caelum"), "f_pilgrimroad_41_3");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(20, new[] { MonsterId.Rootcrystal_01 }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FPilgrimroad413Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1005);
		SetName(L("The Minos Warchief"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Minos to bait the warchief, then end him."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Doran"), "f_pilgrimroad_41_3");

		AddObjective("killPack", L("Kill Green Minos"),
			new KillObjective(12, new[] { MonsterId.Minos_Green }));

		AddObjective("killWarchief", L("Defeat the Minos Warchief"),
			new KillObjective(1, new[] { MonsterId.Minos_Green }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640012, 1));
	}
}

public class FPilgrimroad413Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_3", 1006);
		SetName(L("Pilgrim Road Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Green Minos, Bow-Minos, and Brown Lapasape Mages."));
		SetLocation("f_pilgrimroad_41_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Marek"), "f_pilgrimroad_41_3");

		AddObjective("killMinos", L("Kill Green Minos"),
			new KillObjective(12, new[] { MonsterId.Minos_Green }));

		AddObjective("killBows", L("Kill Bow-Minos"),
			new KillObjective(12, new[] { MonsterId.Minos_Bow_Green }));

		AddObjective("killMages", L("Kill Brown Lapasape Mages"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Mage_Brown }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}
