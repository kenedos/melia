//--- Melia Script ----------------------------------------------------------
// Steel Heights Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Steel Heights plateau (Tiny Mages, Keparis, Spions, Harugals).
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

public class FTableland74QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Tiny Mage Kill
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Plateau-Watch] Mindaugas"), "f_tableland_74", -700, -250, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tiny Brown Mages swarm the lower terraces. Forty killed and the climbing path opens."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Path?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill forty Tiny Brown Mages?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty. Mind their staves."));
						}
						break;

					case "info":
						await dialog.Msg(L("Caravans need the terrace path. Mages have it choked."));
						break;

					case "leave":
						await dialog.Msg(L("Path stays choked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Path opens."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First caravan crossed at dawn."));
			}
		});

		// Quest 2: Kepari Flesh
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Alchemist] Ruta"), "f_tableland_74", 150, -1050, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Keparis nest the rock-folds. Kill twenty-five, bring seven flesh-cuts for reagent work."));

				var response = await dialog.Select(L("Cuts?"),
					Option(L("I'll bring"), "help"),
					Option(L("Reagent?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-five Purple Keparis and bring seven flesh-cuts?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Cold-wrap the cuts. They sour fast."));
						}
						break;

					case "info":
						await dialog.Msg(L("Kepari flesh holds anti-petrification factors. Eight cuts, half a batch of salve."));
						break;

					case "leave":
						await dialog.Msg(L("Salve stays unmade."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killKeparis", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFlesh", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Seven cuts. Salve batch tonight."));
					character.Inventory.Remove(650776, character.Inventory.CountItem(650776), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salve works on early petrification. Late stone resists."));
			}
		});

		// Quest 3: White Spion Essence
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Aether-Scholar] Birute-IV"), "f_tableland_74", 1250, 1250, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1003);

			dialog.SetTitle(L("Birute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("White Spion Mages distill aether on the ridge. Kill eighteen, bring six essence-vials."));

				var response = await dialog.Select(L("Vials?"),
					Option(L("I'll bring"), "help"),
					Option(L("Aether?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill eighteen White Spion Mages and bring six essence-vials?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Don't uncork them."));
						}
						break;

					case "info":
						await dialog.Msg(L("Aether-essence drives counter-curse foci. Six vials, one foci-set."));
						break;

					case "leave":
						await dialog.Msg(L("Foci stay theory."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpions", out var killObj)) return;
				if (!quest.TryGetProgress("gatherEssence", out var eObj)) return;

				if (killObj.Done && eObj.Done)
				{
					await dialog.Msg(L("Six vials. Foci-set forged tonight."));
					character.Inventory.Remove(663131, character.Inventory.CountItem(663131), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Foci hum clean. Field-test next moon."));
			}
		});

		// Quest 4: Upper Terrace Kill
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Caravan-Master] Jurgita"), "f_tableland_74", -1650, -400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1004);

			dialog.SetTitle(L("Jurgita"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tiny Brown Mages drift the upper terrace. Eighteen killed, my caravan rolls."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Caravan?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill eighteen Tiny Brown Mages on the upper terrace?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Eighteen. Quick."));
						}
						break;

					case "info":
						await dialog.Msg(L("Salt-run. Plateau without salt is plateau without bread."));
						break;

					case "leave":
						await dialog.Msg(L("Wagons stay parked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMagesUpper", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Wheels rolling."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salt's down at the camp. Bread by tomorrow."));
			}
		});

		// Quest 5: The Blue Harugal Elite
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Bounty-Sergeant] Tomas-II"), "f_tableland_74", 1000, -130, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1005);
			var eliteSpawnedKey = "Laima.Quests.f_tableland_74.Quest1005.EliteSpawned";

			dialog.SetTitle(L("Tomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Blue Harugal Elite leads the pack on this plateau. Kill ten to draw him from his den."));

				var response = await dialog.Select(L("Elite?"),
					Option(L("I'll face him"), "help"),
					Option(L("Pack?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Blue Harugals and defeat the Elite when he emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("The Elite calls the pack. End him, the pack scatters."));
						break;

					case "leave":
						await dialog.Msg(L("Pack stays bold."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killElite", out var eObj)) return;

				if (pObj.Done && eObj.Done)
				{
					await dialog.Msg(L("Pack scatters."));
					character.Variables.Perm.Remove(eliteSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !eObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(eliteSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(eliteSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Harugal_Blue, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Blue Harugal Elite emerges from his den!{/}"));
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
				await dialog.Msg(L("Plateau quiet at last."));
			}
		});

		// Quest 6: Steel Heights Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Saulius"), "f_tableland_74", 200, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_74", 1006);

			dialog.SetTitle(L("Saulius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Plateau sweep. Twelve Tiny Brown Mages, twelve Purple Keparis, twelve Blue Hohen Gulaks."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - Tiny Brown Mages, Purple Keparis, Blue Hohen Gulaks?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau stays wild."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var mObj)) return;
				if (!quest.TryGetProgress("killKeparis", out var kObj)) return;
				if (!quest.TryGetProgress("killGulaks", out var gObj)) return;

				if (mObj.Done && kObj.Done && gObj.Done)
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
				await dialog.Msg(L("Militia patrols the heights now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland74Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1001);
		SetName(L("Tiny Mage Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tiny Brown Mages choking the lower terrace path."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Plateau-Watch] Mindaugas"), "f_tableland_74");

		AddObjective("killMages", L("Kill Tiny Brown Mages"),
			new KillObjective(40, new[] { MonsterId.Tiny_Mage_Brown }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 11));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FTableland74Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1002);
		SetName(L("Kepari Flesh-Cuts"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Keparis and bring flesh-cuts for the alchemist."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Ruta"), "f_tableland_74");

		AddObjective("killKeparis", L("Kill Purple Keparis"),
			new KillObjective(25, new[] { MonsterId.Kepari_Purple }));

		AddObjective("gatherFlesh", L("Gather flesh-cuts"),
			new CollectItemObjective(650776, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 4));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650776, character.Inventory.CountItem(650776), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650776, character.Inventory.CountItem(650776), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland74Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1003);
		SetName(L("White Spion Essence"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill White Spion Mages and bring essence-vials for foci-work."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Aether-Scholar] Birute"), "f_tableland_74");

		AddObjective("killSpions", L("Kill White Spion Mages"),
			new KillObjective(18, new[] { MonsterId.Spion_Mage_White }));

		AddObjective("gatherEssence", L("Gather essence-vials"),
			new CollectItemObjective(663131, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 11));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 5));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663131, character.Inventory.CountItem(663131), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663131, character.Inventory.CountItem(663131), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland74Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1004);
		SetName(L("Upper Terrace Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tiny Brown Mages on the upper terrace to clear the caravan route."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan-Master] Jurgita"), "f_tableland_74");

		AddObjective("killMagesUpper", L("Kill Tiny Brown Mages"),
			new KillObjective(18, new[] { MonsterId.Tiny_Mage_Brown }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 11));
		AddReward(new ItemReward(640007, 14));
	}
}

public class FTableland74Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1005);
		SetName(L("The Blue Harugal Elite"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Harugals to draw out the Elite leading the pack."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty-Sergeant] Tomas"), "f_tableland_74");

		AddObjective("killPack", L("Kill Blue Harugals"),
			new KillObjective(10, new[] { MonsterId.Harugal_Blue }));

		AddObjective("killElite", L("Defeat the Blue Harugal Elite"),
			new KillObjective(1, new[] { MonsterId.Harugal_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 5));
	}
}

public class FTableland74Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_74", 1006);
		SetName(L("Steel Heights Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Tiny Brown Mages, Purple Keparis, and Blue Hohen Gulaks."));
		SetLocation("f_tableland_74");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Saulius"), "f_tableland_74");

		AddObjective("killMages", L("Kill Tiny Brown Mages"),
			new KillObjective(12, new[] { MonsterId.Tiny_Mage_Brown }));

		AddObjective("killKeparis", L("Kill Purple Keparis"),
			new KillObjective(12, new[] { MonsterId.Kepari_Purple }));

		AddObjective("killGulaks", L("Kill Blue Hohen Gulaks"),
			new KillObjective(12, new[] { MonsterId.Hohen_Gulak_Blue }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(75000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 13));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 6));
	}
}
