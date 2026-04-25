//--- Melia Script ----------------------------------------------------------
// Stogas Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Stogas Plateau.
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

public class FTableland282QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Blue Siaulav Flocks
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Sky-Ward] Algirdas"), "f_tableland_28_2", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1001);

			dialog.SetTitle(L("Algirdas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Siaulav flocks choke the Stogas sky. They dive on travelers and shepherds alike."));
				await dialog.Msg(L("Kill twenty-five and the sky clears."));

				var response = await dialog.Select(L("Clear the sky?"),
					Option(L("I'll kill"), "help"),
					Option(L("Why dive?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-five."));
						break;

					case "info":
						await dialog.Msg(L("Territorial and hungry."));
						break;

					case "leave":
						await dialog.Msg(L("Sheep can't fly, can they?"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSiaulavs", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Sky's clear."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Shepherds grateful."));
			}
		});

		// Quest 2: Siaulav Mage Feathers
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Fletcher] Grazina"), "f_tableland_28_2", -800, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1002);

			dialog.SetTitle(L("Grazina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Siaulav Mages drop mana-laced feathers. Six of them fletch a magic arrow."));

				var response = await dialog.Select(L("Six feathers?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Magic arrows?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Only the tail-shafts have mana. Pluck cleanly."));
						break;

					case "info":
						await dialog.Msg(L("Orsha archers pay triple."));
						break;

					case "leave":
						await dialog.Msg(L("Fine. Next fletcher then."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFeathers", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Six mana feathers. Magic flight guaranteed."));
					character.Inventory.Remove(650069, character.Inventory.CountItem(650069), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Orsha order paid out double."));
			}
		});

		// Quest 3: Siaulav Bow Sinew
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Bowyer] Ramunas"), "f_tableland_28_2", 400, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1003);

			dialog.SetTitle(L("Ramunas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Siaulav Bows have sinew stronger than any beast on the plateau. Seven sinews make a masterwork bow."));

				var response = await dialog.Select(L("Sinews?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Why so strong?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Seven. Don't shred them."));
						break;

					case "info":
						await dialog.Msg(L("Airborne tension. They draw against wind all their lives."));
						break;

					case "leave":
						await dialog.Msg(L("Then the bow stays unmade."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBows", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSinews", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Seven sinews. Bow gets strung this week."));
					character.Inventory.Remove(650070, character.Inventory.CountItem(650070), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Bow's with a Fedimian ranger. Draws twenty paces further."));
			}
		});

		// Quest 4: Lapasape Swarm
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Shepherd] Ausra"), "f_tableland_28_2", -500, -1400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1004);

			dialog.SetTitle(L("Ausra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Lapasapes flock through my pasture. They eat grass in rolls, leaving bare earth."));
				await dialog.Msg(L("Kill thirty-five so my flock has pasture again."));

				var response = await dialog.Select(L("Thirty-five?"),
					Option(L("I'll kill"), "help"),
					Option(L("Too many?"), "info"),
					Option(L("Move pasture"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They flock aggressive. Don't get surrounded."));
						break;

					case "info":
						await dialog.Msg(L("They breed faster than the grass grows."));
						break;

					case "leave":
						await dialog.Msg(L("Pasture's been here since my grandfather."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLapasapes", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Pasture's resting. Sheep fat again by month-end."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Flock's fat. Wool prices up."));
			}
		});

		// Quest 5: Siaulav King
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Falconer] Ignas"), "f_tableland_28_2", 1200, 1200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1005);
			var kingSpawnedKey = "Laima.Quests.f_tableland_28_2.Quest1005.KingSpawned";

			dialog.SetTitle(L("Ignas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Siaulav King rules the upper winds. Thin his flock and he dives."));

				var response = await dialog.Select(L("Dives?"),
					Option(L("I'll provoke him"), "help"),
					Option(L("How big?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten. He's proud."));
						break;

					case "info":
						await dialog.Msg(L("Wingspan twenty feet. Claws the size of kitchen knives."));
						break;

					case "leave":
						await dialog.Msg(L("His flock's splitting already."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFlock", out var fObj)) return;
				if (!quest.TryGetProgress("killKing", out var kObj)) return;

				if (fObj.Done && kObj.Done)
				{
					await dialog.Msg(L("King's down. Upper winds belong to us again."));
					character.Variables.Perm.Remove(kingSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (fObj.Done && !kObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(kingSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(kingSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Siaulav_Blue, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Dive incoming!"));
							character.ServerMessage(L("{#FF9966}The Siaulav King dives from the upper winds!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him before he climbs again."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Sky's quiet. My falcons hunt again."));
			}
		});

		// Quest 6: Stogas Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Master] Leokadija"), "f_tableland_28_2", -1200, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1006);

			dialog.SetTitle(L("Leokadija"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Standard Stogas sweep. Twelve Siaulavs, twelve Lapasapes."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Standard bounty."));
						break;

					case "info":
						await dialog.Msg(L("Fair pay."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan still rolls."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSiaulavs", out var sObj)) return;
				if (!quest.TryGetProgress("killLapasapes", out var lObj)) return;

				if (sObj.Done && lObj.Done)
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
				await dialog.Msg(L("Caravan's through."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland282Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1001);
		SetName(L("Blue Siaulav Flocks"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Siaulavs dominating the Stogas sky."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Sky-Ward] Algirdas"), "f_tableland_28_2");

		AddObjective("killSiaulavs", L("Kill Blue Siaulavs"),
			new KillObjective(25, new[] { MonsterId.Siaulav_Blue }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FTableland282Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1002);
		SetName(L("Mana Feathers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Siaulav Mages and bring mana feathers for magic arrows."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Grazina"), "f_tableland_28_2");

		AddObjective("killMages", L("Kill Blue Siaulav Mages"),
			new KillObjective(15, new[] { MonsterId.Siaulav_Mage_Blue }));

		AddObjective("gatherFeathers", L("Gather mana feathers"),
			new CollectItemObjective(650069, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650069, character.Inventory.CountItem(650069), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650069, character.Inventory.CountItem(650069), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland282Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1003);
		SetName(L("Masterwork Sinews"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Siaulav Bows and bring sinews for a masterwork bow."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bowyer] Ramunas"), "f_tableland_28_2");

		AddObjective("killBows", L("Kill Blue Siaulav Bows"),
			new KillObjective(15, new[] { MonsterId.Siaulav_Bow_Blue }));

		AddObjective("gatherSinews", L("Gather sinews"),
			new CollectItemObjective(650070, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650070, character.Inventory.CountItem(650070), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650070, character.Inventory.CountItem(650070), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland282Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1004);
		SetName(L("Lapasape Swarm"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Lapasapes stripping the Stogas pasture."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shepherd] Ausra"), "f_tableland_28_2");

		AddObjective("killLapasapes", L("Kill Blue Lapasapes"),
			new KillObjective(35, new[] { MonsterId.Lapasape_Blue }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FTableland282Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1005);
		SetName(L("The Siaulav King"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Siaulav flock to draw down the King from the upper winds."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Falconer] Ignas"), "f_tableland_28_2");

		AddObjective("killFlock", L("Thin Blue Siaulavs"),
			new KillObjective(10, new[] { MonsterId.Siaulav_Blue }));

		AddObjective("killKing", L("Defeat the Siaulav King"),
			new KillObjective(1, new[] { MonsterId.Siaulav_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}

public class FTableland282Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1006);
		SetName(L("Stogas Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Blue Siaulavs and Blue Lapasapes."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Leokadija"), "f_tableland_28_2");

		AddObjective("killSiaulavs", L("Kill Blue Siaulavs"),
			new KillObjective(12, new[] { MonsterId.Siaulav_Blue }));

		AddObjective("killLapasapes", L("Kill Blue Lapasapes"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}
