//--- Melia Script ----------------------------------------------------------
// Goddess' Ancient Garden Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for f_remains_38 (ruined garden continuation of Stele Road).
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

public class FRemains38QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: InfroBurk Kill
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Ruin-Warden] Kazimieras"), "f_remains_38", -1850, 650, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1001);

			dialog.SetTitle(L("Kazimieras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("InfroBurks overran the garden's north tier. Forty kills clears the stele approach."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Stele?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. Watch the vines."));
						break;

					case "info":
						await dialog.Msg(L("Old stele runs through the tier. Escanciu survey team can't approach while the burks nest."));
						break;

					case "leave":
						await dialog.Msg(L("Stele stays choked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBurks", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Survey team moving in."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Stele's legible again."));
			}
		});

		// Quest 2: Lizardman Scale-Plates
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Stele-Archivist] Birute"), "f_remains_38", -950, -50, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1002);

			dialog.SetTitle(L("Birute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Lizardmen scavenge the inner garden. Kill twenty-eight, bring eight scale-plates for the ward relay."));

				var response = await dialog.Select(L("Plates?"),
					Option(L("I'll bring"), "help"),
					Option(L("Relay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Plates only. Clean scales, no cracks."));
						break;

					case "info":
						await dialog.Msg(L("Relay ward holds the garden's perimeter seal. Plates refract the signal."));
						break;

					case "leave":
						await dialog.Msg(L("Seal fades."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLizards", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPlates", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Eight plates. Relay humming."));
					character.Inventory.Remove(650258, character.Inventory.CountItem(650258), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Perimeter seal back to full."));
			}
		});

		// Quest 3: Stub Tree Mage Bark-Sheets
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Druid-Scholar] Morta"), "f_remains_38", -800, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1003);

			dialog.SetTitle(L("Morta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Stub Tree Mages wear bark-sheets carved with the garden's old litany. Kill sixteen, bring six sheets."));

				var response = await dialog.Select(L("Sheets?"),
					Option(L("I'll bring"), "help"),
					Option(L("Litany?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't crease them."));
						break;

					case "info":
						await dialog.Msg(L("The litany tells how the garden was sealed. The Escanciu line forgot it. We won't."));
						break;

					case "leave":
						await dialog.Msg(L("Litany stays lost."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSheets", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Six sheets. Litany reassembled tonight."));
					character.Inventory.Remove(650260, character.Inventory.CountItem(650260), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Litany translated. Sealing rite viable."));
			}
		});

		// Quest 4: Long Arm Sinew-Cord
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Garden-Forester] Gintaras"), "f_remains_38", 300, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1004);

			dialog.SetTitle(L("Gintaras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Long Arms cord the south ravines. Kill twenty for eight sinew-cords."));

				var response = await dialog.Select(L("Cords?"),
					Option(L("I'll gather"), "help"),
					Option(L("Use?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cords intact. Don't slash the tendon."));
						break;

					case "info":
						await dialog.Msg(L("Brace-lines for the ravine bridgeworks. Sinew holds where rope rots."));
						break;

					case "leave":
						await dialog.Msg(L("Bridgework waits."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArms", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCords", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Eight cords. Bridge crew laying them now."));
					character.Inventory.Remove(650261, character.Inventory.CountItem(650261), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ravine passable again."));
			}
		});

		// Quest 5: The Garden Devilglove
		//-------------------------------------------------------------------------
		AddNpc(155142, L("[Bounty Hunter] Saulius"), "f_remains_38", 1300, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1005);
			dialog.SetTitle(L("Saulius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Devilglove stalks the south orchard. Kill twelve InfroBurks to flush it out."));

				var response = await dialog.Select(L("Devilglove?"),
					Option(L("I'll face it"), "help"),
					Option(L("What is it?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve."));
						break;

					case "info":
						await dialog.Msg(L("Old garden warden turned feral. The burks hide its scent. Clear them and it shows."));
						break;

					case "leave":
						await dialog.Msg(L("It'll take the orchard."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBurks", out var pObj)) return;
				if (!quest.TryGetProgress("killBoss", out var bObj)) return;

				if (bObj.Done)
				{
					await dialog.Msg(L("Orchard's ours."));
					character.Quests.Complete(questId);
				}
				else if (pObj.Done)
				{
					await dialog.Msg(L("It comes! Find it and end it."));
				}
				else
				{
					await dialog.Msg(L("Twelve first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Orchard opens tomorrow."));
			}
		});

		// Quest 6: Garden Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Rimas"), "f_remains_38", 1400, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_remains_38", 1006);

			dialog.SetTitle(L("Rimas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Garden sweep. Twelve InfroBurks, twelve Lizardmen, twelve Long Arms."));

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
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Garden stays wild."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBurks", out var bObj)) return;
				if (!quest.TryGetProgress("killLizards", out var lObj)) return;
				if (!quest.TryGetProgress("killArms", out var aObj)) return;

				if (bObj.Done && lObj.Done && aObj.Done)
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
				await dialog.Msg(L("Militia patrols the garden now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FRemains38Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1001);
		SetName(L("InfroBurk Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill InfroBurks choking the garden's stele approach."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ruin-Warden] Kazimieras"), "f_remains_38");

		AddObjective("killBurks", L("Kill InfroBurks"),
			new KillObjective(40, new[] { MonsterId.InfroBurk }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FRemains38Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1002);
		SetName(L("Lizardman Scale-Plates"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Lizardmen and bring scale-plates for the ward relay."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Stele-Archivist] Birute"), "f_remains_38");

		AddObjective("killLizards", L("Kill Lizardmen"),
			new KillObjective(28, new[] { MonsterId.Lizardman }));

		AddObjective("gatherPlates", L("Gather scale-plates"),
			new CollectItemObjective(650258, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650258, character.Inventory.CountItem(650258), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650258, character.Inventory.CountItem(650258), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains38Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1003);
		SetName(L("Bark-Sheets"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Stub Tree Mages and bring bark-sheets carved with the old litany."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Druid-Scholar] Morta"), "f_remains_38");

		AddObjective("killMages", L("Kill Stub Tree Mages"),
			new KillObjective(16, new[] { MonsterId.Stub_Tree_Mage }));

		AddObjective("gatherSheets", L("Gather bark-sheets"),
			new CollectItemObjective(650260, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650260, character.Inventory.CountItem(650260), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650260, character.Inventory.CountItem(650260), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains38Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1004);
		SetName(L("Sinew-Cords"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Long Arms in the south ravines for sinew-cord bridge braces."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Garden-Forester] Gintaras"), "f_remains_38");

		AddObjective("killArms", L("Kill Long Arms"),
			new KillObjective(20, new[] { MonsterId.Long_Arm }));

		AddObjective("gatherCords", L("Gather sinew-cords"),
			new CollectItemObjective(650261, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650261, character.Inventory.CountItem(650261), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650261, character.Inventory.CountItem(650261), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRemains38Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1005);
		SetName(L("The Garden Devilglove"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill InfroBurks to flush out the feral Devilglove warden."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Bounty Hunter] Saulius"), "f_remains_38");

		AddObjective("killBurks", L("Kill InfroBurks"),
			new KillObjective(12, new[] { MonsterId.InfroBurk }));

		AddObjective("killBoss", L("Defeat the Devilglove"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Devilglove, 1) },
				resetIdent: "killBurks",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}

public class FRemains38Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_remains_38", 1006);
		SetName(L("Garden Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of InfroBurks, Lizardmen, and Long Arms."));
		SetLocation("f_remains_38");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Rimas"), "f_remains_38");

		AddObjective("killBurks", L("Kill InfroBurks"),
			new KillObjective(12, new[] { MonsterId.InfroBurk }));

		AddObjective("killLizards", L("Kill Lizardmen"),
			new KillObjective(12, new[] { MonsterId.Lizardman }));

		AddObjective("killArms", L("Kill Long Arms"),
			new KillObjective(12, new[] { MonsterId.Long_Arm }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}
