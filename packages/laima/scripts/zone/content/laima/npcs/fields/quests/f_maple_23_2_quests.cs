//--- Melia Script ----------------------------------------------------------
// Pystis Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Pystis Forest.
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

public class FMaple232QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Leafnut Infestation
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Forester] Vytenis"), "f_maple_23_2", 900, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1001);

			dialog.SetTitle(L("Vytenis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Leafnut swarms are chewing the canopy bald. Good for the nuts, terrible for the shade."));
				await dialog.Msg(L("Kill twenty-two Yellow Leafnuts and the canopy heals by autumn."));

				var response = await dialog.Select(L("Clear the canopy?"),
					Option(L("I'll kill them"), "help"),
					Option(L("Yellow ones?"), "info"),
					Option(L("Let nature sort it"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-two Yellow Leafnuts?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twenty-two. Mind the drop - they fall in clusters."));
						}
						break;

					case "info":
						await dialog.Msg(L("Yellow is spawning-color. Thin them now or double by autumn."));
						break;

					case "leave":
						await dialog.Msg(L("Nature has an accountant, and the ledger is me."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLeafnuts", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Canopy's got a fighting chance now."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep climbing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("New leaves by next month. Shade's back."));
			}
		});

		// Quest 2: Colimen Shells
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Lacquer-Crafter] Ruta"), "f_maple_23_2", 1200, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Colimen shells make the finest lacquer in Klaipeda. Hard to get, worth the trip."));
				await dialog.Msg(L("Kill twenty Red Colimens for six intact shells."));

				var response = await dialog.Select(L("Six shells?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Why only red?"), "info"),
					Option(L("Use something else"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty Red Colimens and bring six intact shells?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Don't shatter them. Strike along the seam, not the dome."));
						}
						break;

					case "info":
						await dialog.Msg(L("The red pigment bonds. Other colors just flake."));
						break;

					case "leave":
						await dialog.Msg(L("The order's for a Fedimian noble. No substitutes."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killColimens", out var killObj)) return;
				if (!quest.TryGetProgress("gatherShells", out var shellObj)) return;

				if (killObj.Done && shellObj.Done)
				{
					await dialog.Msg(L("Six shells, none cracked. The lacquer will glow."));
					character.Inventory.Remove(650015, character.Inventory.CountItem(650015), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Lacquer shipped. Noble paid in gold."));
			}
		});

		// Quest 3: Caro Pelts
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Tailor] Jurgis"), "f_maple_23_2", -200, -600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1003);

			dialog.SetTitle(L("Jurgis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Yellow Caros have the softest underfur of any beast. Cloaks made from them keep out winter."));
				await dialog.Msg(L("Kill thirty Yellow Caros for eight prime pelts."));

				var response = await dialog.Select(L("Prime pelts?"),
					Option(L("I'll kill and skin"), "help"),
					Option(L("Why yellow?"), "info"),
					Option(L("Use wool instead"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty Yellow Caros and bring eight prime pelts?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Only the prime ones - chest and back sections intact."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fur's thickest. Yellow is the winter coat."));
						break;

					case "leave":
						await dialog.Msg(L("Wool can't match it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCaros", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPelts", out var peltObj)) return;

				if (killObj.Done && peltObj.Done)
				{
					await dialog.Msg(L("Eight pelts. Ten cloaks from these."));
					character.Inventory.Remove(650016, character.Inventory.CountItem(650016), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cloaks sold before stitching. Winter's demanding."));
			}
		});

		// Quest 4: Rootcrystal Research
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Crystal Scholar] Austeja"), "f_maple_23_2", 100, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1004);

			dialog.SetTitle(L("Austeja"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Pystis Rootcrystals ring differently than the ones in Rokas. Higher pitch, more mana."));
				await dialog.Msg(L("Break twelve Rootcrystals and bring me seven sonorous shards."));

				var response = await dialog.Select(L("Seven shards?"),
					Option(L("I'll break them"), "help"),
					Option(L("What's the difference?"), "info"),
					Option(L("Study Rokas instead"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Break twelve Rootcrystals and gather seven sonorous shards?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Listen for the ring. If it doesn't ring, it's not sonorous."));
						}
						break;

					case "info":
						await dialog.Msg(L("Ley-line proximity. Pystis sits near a confluence. The crystals tune themselves to it."));
						break;

					case "leave":
						await dialog.Msg(L("Rokas is old news."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("gatherShards", out var shardObj)) return;

				if (killObj.Done && shardObj.Done)
				{
					await dialog.Msg(L("Seven shards, all singing. My thesis writes itself."));
					character.Inventory.Remove(650145, character.Inventory.CountItem(650145), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep tuning."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Thesis accepted. The Academy's jealous."));
			}
		});

		// Quest 5: The Grove Titan
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Huntress] Egle"), "f_maple_23_2", 400, -600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1005);
			var titanSpawnedKey = "Laima.Quests.f_maple_23_2.Quest1005.TitanSpawned";

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Caro titan roams south of here. Twice the size, tears apart pelts worthless."));
				await dialog.Msg(L("Thin ten Yellow Caros to draw him from his den."));

				var response = await dialog.Select(L("Draw him out?"),
					Option(L("I'll hunt the titan"), "help"),
					Option(L("Why does size matter?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Yellow Caros and slay the titan when he emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten. He's territorial."));
						}
						break;

					case "info":
						await dialog.Msg(L("Territorial creatures count their kin. He'll come when the count shifts."));
						break;

					case "leave":
						await dialog.Msg(L("He's tearing out the grove."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCaros", out var cObj)) return;
				if (!quest.TryGetProgress("killTitan", out var tObj)) return;

				if (cObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Titan's down. Grove breathes."));
					character.Variables.Perm.Remove(titanSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (cObj.Done && !tObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(titanSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(titanSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Caro_Yellow, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He's here!"));
							character.ServerMessage(L("{#FF9966}The Caro Titan crashes through the grove!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's loose - find him."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Grove saplings are growing again."));
			}
		});

		// Quest 6: Pystis Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Master] Kestutis"), "f_maple_23_2", -1200, -1200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1006);

			dialog.SetTitle(L("Kestutis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Caravans want Pystis swept before the trade season."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("How much?"), "info"),
					Option(L("Reroute"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Leafnuts, twelve Caros, and twelve Colimens?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Three dozen total. Pystis quiets down for the season."));
						}
						break;

					case "info":
						await dialog.Msg(L("Twelve each. Standard bounty."));
						break;

					case "leave":
						await dialog.Msg(L("No reroute runs this way."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLeafnuts", out var lObj)) return;
				if (!quest.TryGetProgress("killCaros", out var cObj)) return;
				if (!quest.TryGetProgress("killColimens", out var coObj)) return;

				if (lObj.Done && cObj.Done && coObj.Done)
				{
					await dialog.Msg(L("Swept clean. Trade rolls."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravan season's looking strong."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FMaple232Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1001);
		SetName(L("Leafnut Infestation"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Yellow Leafnuts defoliating the Pystis canopy."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forester] Vytenis"), "f_maple_23_2");

		AddObjective("killLeafnuts", L("Kill Yellow Leafnuts"),
			new KillObjective(22, new[] { MonsterId.Leafnut_Yellow }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
	}
}

public class FMaple232Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1002);
		SetName(L("Colimen Shells"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Colimens and bring intact shells for lacquer crafting."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Lacquer-Crafter] Ruta"), "f_maple_23_2");

		AddObjective("killColimens", L("Kill Red Colimens"),
			new KillObjective(20, new[] { MonsterId.Colimen_Red }));

		AddObjective("gatherShells", L("Gather intact shells"),
			new CollectItemObjective(650015, 6));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650015, character.Inventory.CountItem(650015), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650015, character.Inventory.CountItem(650015), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple232Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1003);
		SetName(L("Caro Winter Pelts"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Yellow Caros and bring prime pelts for winter cloaks."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tailor] Jurgis"), "f_maple_23_2");

		AddObjective("killCaros", L("Kill Yellow Caros"),
			new KillObjective(30, new[] { MonsterId.Caro_Yellow }));

		AddObjective("gatherPelts", L("Gather prime pelts"),
			new CollectItemObjective(650016, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650016, character.Inventory.CountItem(650016), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650016, character.Inventory.CountItem(650016), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple232Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1004);
		SetName(L("Sonorous Shards"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Pystis Rootcrystals to harvest sonorous research shards."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal Scholar] Austeja"), "f_maple_23_2");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(12, new[] { MonsterId.Rootcrystal_05 }));

		AddObjective("gatherShards", L("Gather sonorous shards"),
			new CollectItemObjective(650145, 7));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650145, character.Inventory.CountItem(650145), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650145, character.Inventory.CountItem(650145), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple232Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1005);
		SetName(L("The Grove Titan"));
		SetType(QuestType.Sub);
		SetDescription(L("Draw out and slay the Caro Titan tearing through the grove."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Huntress] Egle"), "f_maple_23_2");

		AddObjective("killCaros", L("Thin Yellow Caros"),
			new KillObjective(10, new[] { MonsterId.Caro_Yellow }));

		AddObjective("killTitan", L("Defeat the Caro Titan"),
			new KillObjective(1, new[] { MonsterId.Caro_Yellow }));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}
}

public class FMaple232Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1006);
		SetName(L("Pystis Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Leafnuts, Caros, and Colimens for the trade-season sweep."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Kestutis"), "f_maple_23_2");

		AddObjective("killLeafnuts", L("Kill Leafnuts"),
			new KillObjective(12, new[] { MonsterId.Leafnut_Yellow }));

		AddObjective("killCaros", L("Kill Caros"),
			new KillObjective(12, new[] { MonsterId.Caro_Yellow }));

		AddObjective("killColimens", L("Kill Colimens"),
			new KillObjective(12, new[] { MonsterId.Colimen_Red }));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}
}
