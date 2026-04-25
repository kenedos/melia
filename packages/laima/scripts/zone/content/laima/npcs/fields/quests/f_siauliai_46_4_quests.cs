//--- Melia Script ----------------------------------------------------------
// Dina Bee Farm Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Dina Bee Farm (Siauliai region continuation).
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

public class FSiauliai464QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Rabbee Overrun
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Apiary-Keeper] Jogaila"), "f_siauliai_46_4", 1100, -800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1001);

			dialog.SetTitle(L("Jogaila"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Rabbees swarm the hives. Forty-five killed and the honey flow resumes."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Hives?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill forty-five Rabbees?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty-five. Mind the stingers."));
						}
						break;

					case "info":
						await dialog.Msg(L("Combs hang heavy, nothing gets pulled with the swarm thick."));
						break;

					case "leave":
						await dialog.Msg(L("Honey stays locked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRabbees", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Hives breathe again."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First harvest in weeks yesterday."));
			}
		});

		// Quest 2: Honeybean Wax
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Chandler] Rasa"), "f_siauliai_46_4", 1200, -850, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1002);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Honeybeans carry wax-pods for the candle run. Kill thirty, bring eight pods."));

				var response = await dialog.Select(L("Pods?"),
					Option(L("I'll bring"), "help"),
					Option(L("Candles?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty Honeybeans and bring eight wax-pods?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Eight pods. Don't squash."));
						}
						break;

					case "info":
						await dialog.Msg(L("Temple order, two hundred candles by moon-end."));
						break;

					case "leave":
						await dialog.Msg(L("Order slips."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBeans", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPods", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Eight pods. Candles pour tonight."));
					character.Inventory.Remove(650262, character.Inventory.CountItem(650262), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Temple paid in full."));
			}
		});

		// Quest 3: Siaulamb Fleece
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Weaver] Audra"), "f_siauliai_46_4", 0, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1003);

			dialog.SetTitle(L("Audra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Siaulambs went feral in the north pasture. Kill fifteen, bring five fleece-bundles."));

				var response = await dialog.Select(L("Fleece?"),
					Option(L("I'll bring"), "help"),
					Option(L("Feral?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Siaulambs and bring five fleece-bundles?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Five bundles. Shear clean."));
						}
						break;

					case "info":
						await dialog.Msg(L("Shepherds gone. Flock turned on the walkers."));
						break;

					case "leave":
						await dialog.Msg(L("Loom stays idle."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLambs", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFleece", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Five bundles. Loom sings."));
					character.Inventory.Remove(650263, character.Inventory.CountItem(650263), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First bolt on the cart to Klaipeda."));
			}
		});

		// Quest 4: Rootcrystal Dust
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Crystal-Warder] Ruta"), "f_siauliai_46_4", 900, 200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1004);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Rootcrystals bleed dust the bees can't abide. Break twelve for eight dust-sachets."));

				var response = await dialog.Select(L("Dust?"),
					Option(L("I'll break"), "help"),
					Option(L("Bees?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Break twelve Rootcrystals and bring eight dust-sachets?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Eight sachets. Cloth-tight."));
						}
						break;

					case "info":
						await dialog.Msg(L("Dust pushes Rabbees off combs. Simple arithmetic."));
						break;

					case "leave":
						await dialog.Msg(L("Combs stay lost."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("gatherDust", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Eight sachets. Combs reclaim tonight."));
					character.Inventory.Remove(650264, character.Inventory.CountItem(650264), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Bees resettled. Work resumes."));
			}
		});

		// Quest 5: The Pendinmire
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Vaidas"), "f_siauliai_46_4", 300, 1300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1005);
			var bossSpawnedKey = "Laima.Quests.f_siauliai_46_4.Quest1005.BossSpawned";

			dialog.SetTitle(L("Vaidas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Pendinmire pulls the mire-rot through the marsh. Kill ten Siaulambs gone sour, then face it."));

				var response = await dialog.Select(L("Pendinmire?"),
					Option(L("I'll face it"), "help"),
					Option(L("Mire-rot?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Siaulambs and defeat the Pendinmire when it emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("Black water under the pasture. End the Pendinmire, water clears."));
						break;

					case "leave":
						await dialog.Msg(L("Rot spreads."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killBoss", out var aObj)) return;

				if (pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("Marsh clears."));
					character.Variables.Perm.Remove(bossSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(bossSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(bossSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Pendinmire, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("It surfaces!"));
							character.ServerMessage(L("{#FF9966}The Pendinmire rises from the marsh!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find it."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Water's clean. Lambs return."));
			}
		});

		// Quest 6: Dina Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Tauras"), "f_siauliai_46_4", 400, -200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_4", 1006);

			dialog.SetTitle(L("Tauras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Farm sweep. Twelve Rabbees, twelve Honeybeans, twelve Siaulambs."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - Rabbees, Honeybeans, Siaulambs?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Farm stays shut."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRabbees", out var rObj)) return;
				if (!quest.TryGetProgress("killBeans", out var bObj)) return;
				if (!quest.TryGetProgress("killLambs", out var lObj)) return;

				if (rObj.Done && bObj.Done && lObj.Done)
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
				await dialog.Msg(L("Militia walks the fence-line now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FSiauliai464Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1001);
		SetName(L("Rabbee Overrun"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Rabbees swarming the Dina Bee Farm hives."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Apiary-Keeper] Jogaila"), "f_siauliai_46_4");

		AddObjective("killRabbees", L("Kill Rabbees"),
			new KillObjective(45, new[] { MonsterId.Rabbee }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FSiauliai464Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1002);
		SetName(L("Honeybean Wax-Pods"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Honeybeans and bring wax-pods for the chandler's candle run."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Chandler] Rasa"), "f_siauliai_46_4");

		AddObjective("killBeans", L("Kill Honeybeans"),
			new KillObjective(30, new[] { MonsterId.Honeybean }));

		AddObjective("gatherPods", L("Gather wax-pods"),
			new CollectItemObjective(650262, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650262, character.Inventory.CountItem(650262), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650262, character.Inventory.CountItem(650262), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FSiauliai464Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1003);
		SetName(L("Feral Fleece"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill feral Siaulambs and bring fleece-bundles for the weaver."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Weaver] Audra"), "f_siauliai_46_4");

		AddObjective("killLambs", L("Kill Siaulambs"),
			new KillObjective(15, new[] { MonsterId.Siaulamb }));

		AddObjective("gatherFleece", L("Gather fleece-bundles"),
			new CollectItemObjective(650263, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 13));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650263, character.Inventory.CountItem(650263), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650263, character.Inventory.CountItem(650263), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FSiauliai464Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1004);
		SetName(L("Dust-Sachets"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Rootcrystals to gather dust-sachets that repel Rabbees."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Warder] Ruta"), "f_siauliai_46_4");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(12, new[] { MonsterId.Rootcrystal_01 }));

		AddObjective("gatherDust", L("Gather dust-sachets"),
			new CollectItemObjective(650264, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650264, character.Inventory.CountItem(650264), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650264, character.Inventory.CountItem(650264), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FSiauliai464Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1005);
		SetName(L("The Pendinmire"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Siaulambs to draw out the Pendinmire polluting the marsh."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Vaidas"), "f_siauliai_46_4");

		AddObjective("killPack", L("Kill Siaulambs"),
			new KillObjective(10, new[] { MonsterId.Siaulamb }));

		AddObjective("killBoss", L("Defeat the Pendinmire"),
			new KillObjective(1, new[] { MonsterId.Pendinmire }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}
}

public class FSiauliai464Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_4", 1006);
		SetName(L("Dina Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Rabbees, Honeybeans, and Siaulambs around the farm."));
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Tauras"), "f_siauliai_46_4");

		AddObjective("killRabbees", L("Kill Rabbees"),
			new KillObjective(12, new[] { MonsterId.Rabbee }));

		AddObjective("killBeans", L("Kill Honeybeans"),
			new KillObjective(12, new[] { MonsterId.Honeybean }));

		AddObjective("killLambs", L("Kill Siaulambs"),
			new KillObjective(12, new[] { MonsterId.Siaulamb }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(75000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 13));
	}
}
