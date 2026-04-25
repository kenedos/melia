//--- Melia Script ----------------------------------------------------------
// Ouaas Memorial Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the Ouaas Memorial pilgrim road.
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

public class FPilgrimroad415QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Brown Nuka Kill
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Pilgrim-Warden] Mindaugas"), "f_pilgrimroad_41_5", -250, -50, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Brown Nukas swarm Ouaas's road. Forty kills and the pilgrims pass clean."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Pilgrims?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill forty Brown Nukas?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty. Watch their charge."));
						}
						break;

					case "info":
						await dialog.Msg(L("Salvia faithful walk this road to the memorial. Nukas trample them."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays unsafe."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killNukas", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Pilgrims cross the road tonight."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First caravan reached the memorial."));
			}
		});

		// Quest 2: Fire Hardened Arrowheads
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Fletcher] Algirdas"), "f_pilgrimroad_41_5", 1100, 200, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1002);

			dialog.SetTitle(L("Algirdas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Brown Lapasape archers carry fire-hardened heads. Kill twenty-eight, bring seven heads."));

				var response = await dialog.Select(L("Heads?"),
					Option(L("I'll bring"), "help"),
					Option(L("Why?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-eight Brown Lapasape and bring seven arrowheads?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Heads only. Shafts splinter."));
						}
						break;

					case "info":
						await dialog.Msg(L("Refit them for the memorial guard. Cheaper than forging."));
						break;

					case "leave":
						await dialog.Msg(L("Guard runs short."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLapasape", out var killObj)) return;
				if (!quest.TryGetProgress("gatherHeads", out var hObj)) return;

				if (killObj.Done && hObj.Done)
				{
					await dialog.Msg(L("Seven heads. Quivers refilled."));
					character.Inventory.Remove(650756, character.Inventory.CountItem(650756), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Memorial guard fully shafted."));
			}
		});

		// Quest 3: Yellow-Eyed Petals
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Memorial-Tender] Birute"), "f_pilgrimroad_41_5", -300, 870, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1003);

			dialog.SetTitle(L("Birute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Elma bloom yellow-eyed petals along Ouaas's verge. Kill eighteen, gather six petals."));

				var response = await dialog.Select(L("Petals?"),
					Option(L("I'll gather"), "help"),
					Option(L("Use?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill eighteen Red Elma and bring six yellow-eyed petals?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Press them flat. Don't crush."));
						}
						break;

					case "info":
						await dialog.Msg(L("Memorial offering. Ouaas favored yellow."));
						break;

					case "leave":
						await dialog.Msg(L("Altar stays bare."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killElma", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPetals", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Six petals. Altar dressed at dusk."));
					character.Inventory.Remove(661076, character.Inventory.CountItem(661076), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep gathering."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pilgrims wept at the offering."));
			}
		});

		// Quest 4: Deadbornscab Archers
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Road-Marshal] Kestutis"), "f_pilgrimroad_41_5", 700, 380, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1004);

			dialog.SetTitle(L("Kestutis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Deadbornscab archers ambush from the cliffs. Kill eighteen and the road clears."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Cliffs?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill eighteen Deadbornscab archers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Eighteen. Mind the high ground."));
						}
						break;

					case "info":
						await dialog.Msg(L("They nest above the bend. Pilgrims walk under their volleys."));
						break;

					case "leave":
						await dialog.Msg(L("Volleys keep falling."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Cliffs silent."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Bend's safe. Pilgrims walk upright."));
			}
		});

		// Quest 5: The Trampler
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Hunter] Vytenis"), "f_pilgrimroad_41_5", -850, -400, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1005);
			var bossSpawnedKey = "Laima.Quests.f_pilgrimroad_41_5.Quest1005.BossSpawned";

			dialog.SetTitle(L("Vytenis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A bull Nuka leads the trampling herd. Kill ten to draw him out."));

				var response = await dialog.Select(L("Bull?"),
					Option(L("I'll face him"), "help"),
					Option(L("Bull?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Brown Nukas and defeat the bull when he emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("Twice the size, scarred flank. He breaks the carriages."));
						break;

					case "leave":
						await dialog.Msg(L("Carriages keep splintering."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHerd", out var hObj)) return;
				if (!quest.TryGetProgress("killBull", out var bObj)) return;

				if (hObj.Done && bObj.Done)
				{
					await dialog.Msg(L("Bull's down."));
					character.Variables.Perm.Remove(bossSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (hObj.Done && !bObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(bossSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(bossSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Nuka_Brown, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He charges!"));
							character.ServerMessage(L("{#FF9966}The bull Nuka thunders down the road!{/}"));
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
				await dialog.Msg(L("Herd scatters without him."));
			}
		});

		// Quest 6: Pilgrim Road Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Gediminas"), "f_pilgrimroad_41_5", 450, -300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_5", 1006);

			dialog.SetTitle(L("Gediminas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Road sweep. Twelve Nukas, twelve Lapasape, twelve Deadbornscab."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - Brown Nukas, Brown Lapasape, Deadbornscab archers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays loud."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killNukas", out var nObj)) return;
				if (!quest.TryGetProgress("killLapasape", out var lObj)) return;
				if (!quest.TryGetProgress("killArchers", out var aObj)) return;

				if (nObj.Done && lObj.Done && aObj.Done)
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
				await dialog.Msg(L("Militia patrols the road now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FPilgrimroad415Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1001);
		SetName(L("Brown Nuka Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Nukas trampling the pilgrim road."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim-Warden] Mindaugas"), "f_pilgrimroad_41_5");

		AddObjective("killNukas", L("Kill Brown Nukas"),
			new KillObjective(40, new[] { MonsterId.Nuka_Brown }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 11));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FPilgrimroad415Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1002);
		SetName(L("Fire Hardened Arrowheads"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Lapasape archers and bring fire hardened arrowheads."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Algirdas"), "f_pilgrimroad_41_5");

		AddObjective("killLapasape", L("Kill Brown Lapasape"),
			new KillObjective(28, new[] { MonsterId.Lapasape_Bow_Brown }));

		AddObjective("gatherHeads", L("Gather fire hardened arrowheads"),
			new CollectItemObjective(650756, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 5));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650756, character.Inventory.CountItem(650756), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650756, character.Inventory.CountItem(650756), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad415Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1003);
		SetName(L("Yellow-Eyed Petals"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Elma and gather yellow-eyed petals for Ouaas's altar."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Memorial-Tender] Birute"), "f_pilgrimroad_41_5");

		AddObjective("killElma", L("Kill Red Elma"),
			new KillObjective(18, new[] { MonsterId.Elma_Red }));

		AddObjective("gatherPetals", L("Gather yellow-eyed petals"),
			new CollectItemObjective(661076, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(72000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661076, character.Inventory.CountItem(661076), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661076, character.Inventory.CountItem(661076), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad415Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1004);
		SetName(L("Cliffside Archers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Deadbornscab archers ambushing the road from the cliffs."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Marshal] Kestutis"), "f_pilgrimroad_41_5");

		AddObjective("killArchers", L("Kill Deadbornscab archers"),
			new KillObjective(18, new[] { MonsterId.Deadbornscab_Bow }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(70000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 13));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FPilgrimroad415Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1005);
		SetName(L("The Trampler"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Nukas to draw out the bull leading the herd."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hunter] Vytenis"), "f_pilgrimroad_41_5");

		AddObjective("killHerd", L("Kill Brown Nukas"),
			new KillObjective(10, new[] { MonsterId.Nuka_Brown }));

		AddObjective("killBull", L("Defeat the bull Nuka"),
			new KillObjective(1, new[] { MonsterId.Nuka_Brown }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(75000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 13));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 7));
	}
}

public class FPilgrimroad415Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_5", 1006);
		SetName(L("Pilgrim Road Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Brown Nukas, Brown Lapasape, and Deadbornscab archers."));
		SetLocation("f_pilgrimroad_41_5");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Gediminas"), "f_pilgrimroad_41_5");

		AddObjective("killNukas", L("Kill Brown Nukas"),
			new KillObjective(12, new[] { MonsterId.Nuka_Brown }));

		AddObjective("killLapasape", L("Kill Brown Lapasape"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Bow_Brown }));

		AddObjective("killArchers", L("Kill Deadbornscab archers"),
			new KillObjective(12, new[] { MonsterId.Deadbornscab_Bow }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(75000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 13));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 7));
	}
}
