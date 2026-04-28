//--- Melia Script ----------------------------------------------------------
// Arrow Path Quest NPCs
//--- Description -----------------------------------------------------------
// Restless ghosts of those who died on the Arrow Path.
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

public class FKatyn133QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: Path-Ward by the stream
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Path-Ward"), "f_katyn_13_3", -47, -203, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1001);
			dialog.SetTitle(L("Path-Ward"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The stream... I died beside the stream... My blood ran into it..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Dusk patrol... I knelt to drink... Desmodus... A whole pack... From above... From above..."));
					await dialog.Msg(L("Wings... So many wings... I could not raise my blade in time..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will avenge you"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Thirty-five... Thirty-five Desmodus... So no other ward... Drinks at dusk and dies...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Thirty-five... Thirty-five Desmodus... So no other ward... Drinks at dusk and dies..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDesmodus", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("The wings are quiet... The stream runs clean again... Take this purse... Path-ward's coin..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More wings... More wings at dusk..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Quiet stream... I can rest by it... I can rest..."));
			}
		});

		// Quest 1002: Geologist ghost
		//-------------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Geologist"), "f_katyn_13_3", 1369, 401, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1002);
			dialog.SetTitle(L("Geologist"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Cores... I had eight cores... Eight... Eight..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Hematite assay... For the Orsha forges... I had broken open eight Infrorocktors... Eight cores in my pack..."));
					await dialog.Msg(L("A ninth... I struck a ninth... It struck back... My pack scattered... My cores rolled..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will gather them again"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Thirty Rocktors... Eight cores... Strike at the chest... So my assay... My assay reaches Orsha...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Thirty Rocktors... Eight cores... Strike at the chest... So my assay... My assay reaches Orsha..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRocktors", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCores", out var cObj)) return;
				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Eight cores... I can almost see Orsha from here... Almost..."));
					await dialog.Msg(L("Take this purse... Geologist's coin... Hematite-stained..."));
					character.Inventory.Remove(650305, character.Inventory.CountItem(650305), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More cores... I had eight... Bring me eight..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Orsha forges... Lit by my cores at last... I rest..."));
			}
		});

		// Quest 1003: Bell-Founder ghost
		//-------------------------------------------------------------------------
		AddGhostNpc(155132, L("[Restless Soul] Bell-Founder"), "f_katyn_13_3", -1750, 450, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13_3", 1003);
			dialog.SetTitle(L("Bell-Founder"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bells... Bells silent... The abbey rang nothing the day I died..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Green Ellom bells... Pure tone... No metal match... For the abbey tower..."));
					await dialog.Msg(L("I cut six chains... Carrying them home... The Elloms came back for them... Took the bells... Took my breath..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will deliver the bells"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Fifteen Elloms... Six bells... Cut clean... Don't shatter... So the abbey... The abbey rings at last...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Fifteen Elloms... Six bells... Cut clean... Don't shatter... So the abbey... The abbey rings at last..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killElloms", out var killObj)) return;
				if (!quest.TryGetProgress("gatherBells", out var bObj)) return;
				if (killObj.Done && bObj.Done)
				{
					await dialog.Msg(L("Six bells... I can hear them already... The abbey is ringing... Ringing..."));
					await dialog.Msg(L("Take this... Founder's purse... Triple silver... The abbey paid honest..."));
					character.Inventory.Remove(650122, character.Inventory.CountItem(650122), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Cut clean... Don't shatter the bells..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The bells... I hear them ringing... I rest in their tone... I rest..."));
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
		SetName(L("The Path-Ward by the Stream"));
		SetType(QuestType.Sub);
		SetDescription(L("A path-ward's ghost by the stream begs for the Desmodus pack that killed him at dusk to be put down."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Path-Ward"), "f_katyn_13_3");

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
		SetName(L("The Geologist's Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("A geologist's ghost begs for eight Infrorocktor cores so his hematite assay may reach the Orsha forges."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Geologist"), "f_katyn_13_3");

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

		AddDrop(650305, 0.40f, MonsterId.InfroRocktor_Red);
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
		SetName(L("The Bell-Founder's Bells"));
		SetType(QuestType.Sub);
		SetDescription(L("A bell-founder's ghost begs for six Green Ellom bells so the abbey tower may ring at last."));
		SetLocation("f_katyn_13_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Bell-Founder"), "f_katyn_13_3");

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

		AddDrop(650122, 0.45f, MonsterId.Ellom_Green);
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
