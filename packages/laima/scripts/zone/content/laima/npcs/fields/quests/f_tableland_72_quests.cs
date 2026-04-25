//--- Melia Script ----------------------------------------------------------
// Tableland 72 Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the high tableland beyond Ibre.
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

public class FTableland72QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: White Spion Sweep
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Plateau-Ward] Mindaugas"), "f_tableland_72", 400, -1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("White Spions cluster the upper terraces. Forty kills and the herd-trails open again."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Trails?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill forty White Spions?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty. Mind the wind on the ledges."));
						}
						break;

					case "info":
						await dialog.Msg(L("Cattle drovers can't reach the high pasture while Spions own it."));
						break;

					case "leave":
						await dialog.Msg(L("Trails stay closed."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpions", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Pasture's clear."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First herd up the ledges yesterday."));
			}
		});

		// Quest 2: Lapasape Moss
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Herbalist] Ruta"), "f_tableland_72", -450, -950, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Brown Lapasapes hoard blue moss in their fur. Kill twenty-five and bring six tufts."));

				var response = await dialog.Select(L("Moss?"),
					Option(L("I'll bring"), "help"),
					Option(L("Why blue?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-five Brown Lapasapes and bring six blue moss tufts?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Tufts only. The roots wilt off-stone."));
						}
						break;

					case "info":
						await dialog.Msg(L("Blue moss steeps a salve that mends wind-burn. Plateau winters demand it."));
						break;

					case "leave":
						await dialog.Msg(L("Salve runs out by next moon."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLapasapes", out var killObj)) return;
				if (!quest.TryGetProgress("gatherMoss", out var mObj)) return;

				if (killObj.Done && mObj.Done)
				{
					await dialog.Msg(L("Six tufts. Salve sets tonight."));
					character.Inventory.Remove(668020, character.Inventory.CountItem(668020), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salve jars on the way to Ibre already."));
			}
		});

		// Quest 3: Cronewt Needler Crystals
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Crystal-Cutter] Birute-IV"), "f_tableland_72", 1250, -550, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1003);

			dialog.SetTitle(L("Birute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Cronewt Mages grow needler crystals along their spines. Kill eighteen, bring five crystals."));

				var response = await dialog.Select(L("Crystals?"),
					Option(L("I'll bring"), "help"),
					Option(L("Use?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill eighteen Blue Cronewt Mages and bring five needler crystals?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Pinch the base. They snap clean."));
						}
						break;

					case "info":
						await dialog.Msg(L("Cut right, they replace lens-glass for far-sight scopes. Plateau watchtowers want a dozen."));
						break;

					case "leave":
						await dialog.Msg(L("Towers stay near-blind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCrystals", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Five crystals. Cutting begins at dusk."));
					character.Inventory.Remove(663123, character.Inventory.CountItem(663123), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First scope tested. Watchtower sees the river bend now."));
			}
		});

		// Quest 4: Red Hohen Orben
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Hunter] Jurgita"), "f_tableland_72", 500, -200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1004);

			dialog.SetTitle(L("Jurgita"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Hohen Orben drift the wind-eddies and snatch hawks mid-flight. Burst fifteen."));

				var response = await dialog.Select(L("Burst?"),
					Option(L("I'll burst"), "help"),
					Option(L("Hawks?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Burst fifteen Red Hohen Orben?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Strike the underside. Tops are tough."));
						}
						break;

					case "info":
						await dialog.Msg(L("Falconers can't fly the upper passes while Orben blanket the eddies."));
						break;

					case "leave":
						await dialog.Msg(L("Hawks stay grounded."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("burstOrben", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Eddies clear."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep bursting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Hawks back on the wing this morning."));
			}
		});

		// Quest 5: The White Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Tomas-II"), "f_tableland_72", 700, 1200, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_tableland_72.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Tomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A White Spion alpha leads the upper-terrace pack. Kill ten to draw him out of his den."));

				var response = await dialog.Select(L("Alpha?"),
					Option(L("I'll face him"), "help"),
					Option(L("Den?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten White Spions and defeat the alpha when he emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("North ridge cave. He won't leave it for less than ten of his pack."));
						break;

					case "leave":
						await dialog.Msg(L("Pack grows."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var aObj)) return;

				if (pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("Pack broken."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Spion_White, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The White Alpha emerges from the ridge cave!{/}"));
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
				await dialog.Msg(L("Terrace's quiet. Drovers picking routes already."));
			}
		});

		// Quest 6: Tableland Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Kazimieras"), "f_tableland_72", -400, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_72", 1006);

			dialog.SetTitle(L("Kazimieras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Plateau sweep. Twelve White Spions, twelve Blue Cronewt Mages, twelve Brown Lapasapes."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - White Spions, Blue Cronewt Mages, Brown Lapasapes?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau stays unwalked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpions", out var sObj)) return;
				if (!quest.TryGetProgress("killMages", out var mObj)) return;
				if (!quest.TryGetProgress("killLapasapes", out var lObj)) return;

				if (sObj.Done && mObj.Done && lObj.Done)
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
				await dialog.Msg(L("Militia walks the upper road now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland72Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1001);
		SetName(L("White Spion Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill White Spions overrunning the plateau terraces."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Plateau-Ward] Mindaugas"), "f_tableland_72");

		AddObjective("killSpions", L("Kill White Spions"),
			new KillObjective(40, new[] { MonsterId.Spion_White }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 11));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FTableland72Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1002);
		SetName(L("Blue Lapasape Moss"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Brown Lapasapes and bring blue moss tufts for wind-burn salve."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Ruta"), "f_tableland_72");

		AddObjective("killLapasapes", L("Kill Brown Lapasapes"),
			new KillObjective(25, new[] { MonsterId.Lapasape_Brown }));

		AddObjective("gatherMoss", L("Gather blue moss tufts"),
			new CollectItemObjective(668020, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 5));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668020, character.Inventory.CountItem(668020), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668020, character.Inventory.CountItem(668020), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland72Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1003);
		SetName(L("Needler Crystals"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Cronewt Mages and bring needler crystals for far-sight scopes."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Cutter] Birute"), "f_tableland_72");

		AddObjective("killMages", L("Kill Blue Cronewt Mages"),
			new KillObjective(18, new[] { MonsterId.Cronewt_Mage_Blue }));

		AddObjective("gatherCrystals", L("Gather needler crystals"),
			new CollectItemObjective(663123, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 11));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 4));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663123, character.Inventory.CountItem(663123), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663123, character.Inventory.CountItem(663123), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland72Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1004);
		SetName(L("Wind-Eddy Burst"));
		SetType(QuestType.Sub);
		SetDescription(L("Burst Red Hohen Orben to clear the upper-pass eddies for falconers."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hunter] Jurgita"), "f_tableland_72");

		AddObjective("burstOrben", L("Burst Red Hohen Orben"),
			new KillObjective(15, new[] { MonsterId.Hohen_Orben_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 5));
	}
}

public class FTableland72Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1005);
		SetName(L("The White Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill White Spions to draw out the alpha leading the upper-terrace pack."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Tomas"), "f_tableland_72");

		AddObjective("killPack", L("Kill White Spions"),
			new KillObjective(10, new[] { MonsterId.Spion_White }));

		AddObjective("killAlpha", L("Defeat the White Alpha"),
			new KillObjective(1, new[] { MonsterId.Spion_White }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 13));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 5));
	}
}

public class FTableland72Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_72", 1006);
		SetName(L("Tableland Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of White Spions, Blue Cronewt Mages, and Brown Lapasapes."));
		SetLocation("f_tableland_72");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Kazimieras"), "f_tableland_72");

		AddObjective("killSpions", L("Kill White Spions"),
			new KillObjective(12, new[] { MonsterId.Spion_White }));

		AddObjective("killMages", L("Kill Blue Cronewt Mages"),
			new KillObjective(12, new[] { MonsterId.Cronewt_Mage_Blue }));

		AddObjective("killLapasapes", L("Kill Brown Lapasapes"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Brown }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(75000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 13));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 6));
	}
}
