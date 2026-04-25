//--- Melia Script ----------------------------------------------------------
// Arrow Path Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Arrow Path.
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

public class FKatyn133QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Desmodus Dusk
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Path-Ward] Vytas"), "f_katyn_13_3", 1600, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1001);

			dialog.SetTitle(L("Vytas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Desmodus packs swarm at dusk. Thirty-five kills earns the path a week of quiet."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("At dusk?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thirty-five. Pack-hunting fast."));
						break;

					case "info":
						await dialog.Msg(L("Nocturnal. Dawn-kills safest."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims get mauled."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDesmodus", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Path's quiet at dusk."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Week of quiet. Remarkable."));
			}
		});

		// Quest 2: Infrorocktor Cores
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Geologist] Lina"), "f_katyn_13_3", 1700, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1002);

			dialog.SetTitle(L("Lina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Infrorocktors carry mineral cores. Kill thirty, bring eight cores for assay."));

				var response = await dialog.Select(L("Cores?"),
					Option(L("I'll bring"), "help"),
					Option(L("What mineral?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Hammer them at the chest, not the limbs."));
						break;

					case "info":
						await dialog.Msg(L("Hematite. Orsha forges pay silver."));
						break;

					case "leave":
						await dialog.Msg(L("Forges run thin."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRocktors", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCores", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Eight cores. Assay by morning."));
					character.Inventory.Remove(650305, character.Inventory.CountItem(650305), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Assay's strong. Shipment booked."));
			}
		});

		// Quest 3: Ellom-Green Bells
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Bell-Founder] Dainius"), "f_katyn_13_3", 300, 200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1003);

			dialog.SetTitle(L("Dainius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Ellom bells ring a pure tone. Kill fifteen, bring six bells for the abbey."));

				var response = await dialog.Select(L("Bells?"),
					Option(L("I'll bring"), "help"),
					Option(L("Pure tone?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut the chain-rope. Don't shatter the bell."));
						break;

					case "info":
						await dialog.Msg(L("No metal match. Abbey pays triple."));
						break;

					case "leave":
						await dialog.Msg(L("Abbey rings nothing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killElloms", out var killObj)) return;
				if (!quest.TryGetProgress("gatherBells", out var bObj)) return;

				if (killObj.Done && bObj.Done)
				{
					await dialog.Msg(L("Six bells. Abbey sings."));
					character.Inventory.Remove(650122, character.Inventory.CountItem(650122), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Abbey's bell-tower chimes again."));
			}
		});

	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FKatyn133Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13_3", 1001);
		SetName(L("Desmodus Dusk"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Desmodus packs swarming the Arrow Path at dusk."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Path-Ward] Vytas"), "f_katyn_13_3");

		AddObjective("killDesmodus", L("Kill Desmodus"),
			new KillObjective(35, new[] { MonsterId.New_Desmodus }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FKatyn133Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13_3", 1002);
		SetName(L("Hematite Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Infrorocktors and bring mineral cores for assay."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Geologist] Lina"), "f_katyn_13_3");

		AddObjective("killRocktors", L("Kill Red Infrorocktors"),
			new KillObjective(30, new[] { MonsterId.InfroRocktor_Red }));

		AddObjective("gatherCores", L("Gather mineral cores"),
			new CollectItemObjective(650305, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650305, character.Inventory.CountItem(650305), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650305, character.Inventory.CountItem(650305), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn133Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13_3", 1003);
		SetName(L("Abbey Bells"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Elloms and bring bells for the abbey tower."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bell-Founder] Dainius"), "f_katyn_13_3");

		AddObjective("killElloms", L("Kill Green Elloms"),
			new KillObjective(15, new[] { MonsterId.Ellom_Green }));

		AddObjective("gatherBells", L("Gather bells"),
			new CollectItemObjective(650122, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650122, character.Inventory.CountItem(650122), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650122, character.Inventory.CountItem(650122), InventoryItemRemoveMsg.Destroyed);
	}
}

