//--- Melia Script ----------------------------------------------------------
// Southern Parias Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Southern Parias Forest (f_maple_24_2).
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

public class FMaple242QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Zeuni Thinning
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Ranger] Aurimas"), "f_maple_24_2", -1100, -550, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1001);

			dialog.SetTitle(L("Aurimas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Zeuni Kucarries crowd the southern ridge. Forty-five thinned and the trail opens again."));

				var response = await dialog.Select(L("Thin them?"),
					Option(L("I'll thin"), "help"),
					Option(L("Trail?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Thin forty-five Zeuni Kucarries?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty-five. Mind the burrows."));
						}
						break;

					case "info":
						await dialog.Msg(L("Southern trail links to Parias proper. Blocked since the pack moved in."));
						break;

					case "leave":
						await dialog.Msg(L("Trail stays shut."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killZeuni", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Trail walks clean."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep thinning."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Scouts moved through yesterday."));
			}
		});

		// Quest 2: Numani Pelts
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Tanner] Daiva"), "f_maple_24_2", -400, -500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1002);

			dialog.SetTitle(L("Daiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Numani pelts thicken against cold. Kill thirty, bring eight clean pelts."));

				var response = await dialog.Select(L("Pelts?"),
					Option(L("I'll bring"), "help"),
					Option(L("Clean?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty Numani Kucarries and bring eight clean pelts?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Cut along the flank, not the spine."));
						}
						break;

					case "info":
						await dialog.Msg(L("No tears, no burrs. Clean means clean."));
						break;

					case "leave":
						await dialog.Msg(L("Coats stay thin."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killNumani", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPelts", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Eight pelts. Coats by week's end."));
					character.Inventory.Remove(650244, character.Inventory.CountItem(650244), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Village stays warm this winter."));
			}
		});

		// Quest 3: Zabbi Fangs
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Alchemist] Rima-VII"), "f_maple_24_2", 900, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1003);

			dialog.SetTitle(L("Rima"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Zabbi fangs grind into fever-salve. Kill fifteen, bring five paired fangs."));

				var response = await dialog.Select(L("Fangs?"),
					Option(L("I'll bring"), "help"),
					Option(L("Salve?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Zabbi Kucarries and bring five paired fangs?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Paired means both from one jaw."));
						}
						break;

					case "info":
						await dialog.Msg(L("Pulped fang breaks marsh-fever. Two farmsteads down with it already."));
						break;

					case "leave":
						await dialog.Msg(L("Fever climbs."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killZabbi", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFangs", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Five pairs. Salve by nightfall."));
					character.Inventory.Remove(650246, character.Inventory.CountItem(650246), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Both farmsteads walking again."));
			}
		});

		// Quest 4: Crystal Resonance
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Surveyor] Linas"), "f_maple_24_2", -250, 450, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1004);

			dialog.SetTitle(L("Linas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Rootcrystals hum wrong across the southern grid. Break twelve and bring eight resonant slivers."));

				var response = await dialog.Select(L("Slivers?"),
					Option(L("I'll break"), "help"),
					Option(L("Wrong?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Break twelve Rootcrystals and bring eight resonant slivers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wrap them. They sing to each other."));
						}
						break;

					case "info":
						await dialog.Msg(L("Grid tone shifted a semitone last moon. Something underneath."));
						break;

					case "leave":
						await dialog.Msg(L("Grid drifts."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSlivers", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Eight slivers. Now I can map the shift."));
					character.Inventory.Remove(650247, character.Inventory.CountItem(650247), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Shift's a bore, not a drift. Digging team sent for."));
			}
		});

		// Quest 5: The Pack Elder
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Mantas"), "f_maple_24_2", 1300, -50, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1005);
			var elderSpawnedKey = "Laima.Quests.f_maple_24_2.Quest1005.ElderSpawned";

			dialog.SetTitle(L("Mantas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Zabbi Pack-Elder runs the southern Kucarries. Kill ten to draw him from the den."));

				var response = await dialog.Select(L("Elder?"),
					Option(L("I'll face him"), "help"),
					Option(L("Pack-Elder?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Zabbi Kucarries and defeat the Pack-Elder when he emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("He calls the burrow moves. End him, pack scatters."));
						break;

					case "leave":
						await dialog.Msg(L("Pack holds."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killElder", out var eObj)) return;

				if (pObj.Done && eObj.Done)
				{
					await dialog.Msg(L("Pack scatters tonight."));
					character.Variables.Perm.Remove(elderSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !eObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(elderSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(elderSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Kucarry_Zabbi, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Pack-Elder bursts from the den!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Den's empty. Scouts confirm."));
			}
		});

		// Quest 6: Southern Parias Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Vaclovas"), "f_maple_24_2", 400, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_2", 1006);

			dialog.SetTitle(L("Vaclovas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Southern sweep. Twelve Zeuni, twelve Numani, twelve Zabbi."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - Zeuni, Numani, Zabbi Kucarries?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Forest stays wild."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killZeuni", out var zObj)) return;
				if (!quest.TryGetProgress("killNumani", out var nObj)) return;
				if (!quest.TryGetProgress("killZabbi", out var bObj)) return;

				if (zObj.Done && nObj.Done && bObj.Done)
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
				await dialog.Msg(L("Militia patrols the ridge now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FMaple242Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1001);
		SetName(L("Zeuni Thinning"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin Zeuni Kucarries blocking the southern trail."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger] Aurimas"), "f_maple_24_2");

		AddObjective("killZeuni", L("Kill Zeuni Kucarries"),
			new KillObjective(45, new[] { MonsterId.Kucarry_Zeuni }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
	}
}

public class FMaple242Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1002);
		SetName(L("Numani Pelts"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Numani Kucarries and bring clean pelts for winter coats."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tanner] Daiva"), "f_maple_24_2");

		AddObjective("killNumani", L("Kill Numani Kucarries"),
			new KillObjective(30, new[] { MonsterId.Kucarry_Numani }));

		AddObjective("gatherPelts", L("Gather clean pelts"),
			new CollectItemObjective(650244, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650244, character.Inventory.CountItem(650244), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650244, character.Inventory.CountItem(650244), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple242Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1003);
		SetName(L("Zabbi Fangs"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Zabbi Kucarries and bring paired fangs for fever-salve."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Rima"), "f_maple_24_2");

		AddObjective("killZabbi", L("Kill Zabbi Kucarries"),
			new KillObjective(15, new[] { MonsterId.Kucarry_Zabbi }));

		AddObjective("gatherFangs", L("Gather paired fangs"),
			new CollectItemObjective(650246, 5));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650246, character.Inventory.CountItem(650246), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650246, character.Inventory.CountItem(650246), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple242Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1004);
		SetName(L("Crystal Resonance"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Rootcrystals to gather resonant slivers for the surveyor."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Surveyor] Linas"), "f_maple_24_2");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(12, new[] { MonsterId.Rootcrystal_01 }));

		AddObjective("gatherSlivers", L("Gather resonant slivers"),
			new CollectItemObjective(650247, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650247, character.Inventory.CountItem(650247), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650247, character.Inventory.CountItem(650247), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple242Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1005);
		SetName(L("The Pack Elder"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Zabbi Kucarries to draw out the Pack-Elder leading the southern pack."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Mantas"), "f_maple_24_2");

		AddObjective("killPack", L("Kill Zabbi Kucarries"),
			new KillObjective(10, new[] { MonsterId.Kucarry_Zabbi }));

		AddObjective("killElder", L("Defeat the Pack-Elder"),
			new KillObjective(1, new[] { MonsterId.Kucarry_Zabbi }));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}
}

public class FMaple242Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_2", 1006);
		SetName(L("Southern Parias Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Zeuni, Numani, and Zabbi Kucarries."));
		SetLocation("f_maple_24_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Vaclovas"), "f_maple_24_2");

		AddObjective("killZeuni", L("Kill Zeuni Kucarries"),
			new KillObjective(12, new[] { MonsterId.Kucarry_Zeuni }));

		AddObjective("killNumani", L("Kill Numani Kucarries"),
			new KillObjective(12, new[] { MonsterId.Kucarry_Numani }));

		AddObjective("killZabbi", L("Kill Zabbi Kucarries"),
			new KillObjective(12, new[] { MonsterId.Kucarry_Zabbi }));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}
}
