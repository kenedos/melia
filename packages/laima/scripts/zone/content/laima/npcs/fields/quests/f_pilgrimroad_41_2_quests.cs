//--- Melia Script ----------------------------------------------------------
// Salvia Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Salvia Forest.
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

public class FPilgrimroad412QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Banshee Wailing
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Priest] Romualdas"), "f_pilgrimroad_41_2", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1001);

			dialog.SetTitle(L("Romualdas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Banshees wail every night. Forty silenced and pilgrims sleep through the dark again."));

				var response = await dialog.Select(L("Silence?"),
					Option(L("I'll kill"), "help"),
					Option(L("Wailing?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. Pray before each cut."));
						break;

					case "info":
						await dialog.Msg(L("Restless dead. Each silenced is one soul freed."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims won't pass."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBanshees", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Forest sleeps."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pilgrim parties back on the road."));
			}
		});

		// Quest 2: Tiny Mage Reagents
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Alchemist] Ruta-III"), "f_pilgrimroad_41_2", -1500, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Tiny Mages carry rare reagents. Kill thirty, bring eight reagent-pouches."));

				var response = await dialog.Select(L("Pouches?"),
					Option(L("I'll bring"), "help"),
					Option(L("Rare?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Eight. Seal them."));
						break;

					case "info":
						await dialog.Msg(L("Heal-elixir base. Triple silver."));
						break;

					case "leave":
						await dialog.Msg(L("Elixir shortage worsens."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPouches", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Eight pouches. Elixir brews tonight."));
					character.Inventory.Remove(650124, character.Inventory.CountItem(650124), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Elixirs stocked. Pilgrims heal faster."));
			}
		});

		// Quest 3: Banshee Shrouds
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Mystic] Emilija"), "f_pilgrimroad_41_2", 800, 900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1003);

			dialog.SetTitle(L("Emilija"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Banshee shrouds absorb necrotic aura. Kill twenty, bring six shrouds for warding."));

				var response = await dialog.Select(L("Shrouds?"),
					Option(L("I'll bring"), "help"),
					Option(L("Warding?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They dissolve in sunlight. Cover them."));
						break;

					case "info":
						await dialog.Msg(L("Woven into cloaks. Turns death-aura away."));
						break;

					case "leave":
						await dialog.Msg(L("Warders need them."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBanshees", out var killObj)) return;
				if (!quest.TryGetProgress("gatherShrouds", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Six shrouds. Cloaks woven."));
					character.Inventory.Remove(650144, character.Inventory.CountItem(650144), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Warders patrol safely now."));
			}
		});

		// Quest 4: Salvia Crystal Shards
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Crystal Scholar] Antanas"), "f_pilgrimroad_41_2", 900, -800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1004);

			dialog.SetTitle(L("Antanas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Salvia Rootcrystals resonate strangely - necrotic mana, maybe. Break fifteen for ten shards."));

				var response = await dialog.Select(L("Shards?"),
					Option(L("I'll break"), "help"),
					Option(L("Necrotic?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't hold them too long."));
						break;

					case "info":
						await dialog.Msg(L("Banshee breath soaked into the ley-line. Theory, at least."));
						break;

					case "leave":
						await dialog.Msg(L("Theory unproven."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;
				if (!quest.TryGetProgress("gatherShards", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Ten shards. Thesis holds!"));
					character.Inventory.Remove(650146, character.Inventory.CountItem(650146), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Thesis published. Academy buzzing."));
			}
		});

		// Quest 5: The Banshee Matron
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Exorcist] Basilijs-II"), "f_pilgrimroad_41_2", 200, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1005);
			var matronSpawnedKey = "Laima.Quests.f_pilgrimroad_41_2.Quest1005.MatronSpawned";

			dialog.SetTitle(L("Basilijs"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Banshee Matron anchors the haunting. Kill ten and she manifests."));

				var response = await dialog.Select(L("Face her?"),
					Option(L("I will"), "help"),
					Option(L("Strong?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten."));
						break;

					case "info":
						await dialog.Msg(L("Oldest of the haunt. Scream silences even prayers."));
						break;

					case "leave":
						await dialog.Msg(L("Haunting endures."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFlock", out var fObj)) return;
				if (!quest.TryGetProgress("killMatron", out var mObj)) return;

				if (fObj.Done && mObj.Done)
				{
					await dialog.Msg(L("Matron dispelled."));
					character.Variables.Perm.Remove(matronSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (fObj.Done && !mObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(matronSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(matronSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Sec_Banshee_Purple, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She manifests!"));
							character.ServerMessage(L("{#FF9966}The Banshee Matron manifests with a wail!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find her before the light."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Haunting broken. Forest rests."));
			}
		});

		// Quest 6: Salvia Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Pilgrim-Captain] Juozas"), "f_pilgrimroad_41_2", -400, 900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_2", 1006);

			dialog.SetTitle(L("Juozas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Salvia sweep. Twelve Banshees, twelve Tiny Mages."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Standard."));
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrim column waits."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBanshees", out var bObj)) return;
				if (!quest.TryGetProgress("killMages", out var mObj)) return;

				if (bObj.Done && mObj.Done)
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
				await dialog.Msg(L("Pilgrim column passes."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FPilgrimroad412Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1001);
		SetName(L("Silence the Wailing"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Banshees haunting Salvia Forest."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Priest] Romualdas"), "f_pilgrimroad_41_2");

		AddObjective("killBanshees", L("Kill Purple Banshees"),
			new KillObjective(40, new[] { MonsterId.Sec_Banshee_Purple }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FPilgrimroad412Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1002);
		SetName(L("Elixir Reagents"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Tiny Mages and bring reagent-pouches for healing elixirs."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Ruta"), "f_pilgrimroad_41_2");

		AddObjective("killMages", L("Kill Green Tiny Mages"),
			new KillObjective(30, new[] { MonsterId.Tiny_Mage_Green }));

		AddObjective("gatherPouches", L("Gather reagent-pouches"),
			new CollectItemObjective(650124, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650124, character.Inventory.CountItem(650124), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650124, character.Inventory.CountItem(650124), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad412Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1003);
		SetName(L("Banshee Shrouds"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Banshees and bring shrouds for death-aura warding."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mystic] Emilija"), "f_pilgrimroad_41_2");

		AddObjective("killBanshees", L("Kill Purple Banshees"),
			new KillObjective(20, new[] { MonsterId.Sec_Banshee_Purple }));

		AddObjective("gatherShrouds", L("Gather shrouds"),
			new CollectItemObjective(650144, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650144, character.Inventory.CountItem(650144), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650144, character.Inventory.CountItem(650144), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad412Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1004);
		SetName(L("Necrotic Shards"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Salvia Rootcrystals and bring necrotic-resonance shards."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal Scholar] Antanas"), "f_pilgrimroad_41_2");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(15, new[] { MonsterId.Rootcrystal_02 }));

		AddObjective("gatherShards", L("Gather necrotic shards"),
			new CollectItemObjective(650146, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650146, character.Inventory.CountItem(650146), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650146, character.Inventory.CountItem(650146), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad412Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1005);
		SetName(L("The Banshee Matron"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Banshees to manifest the Matron."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Exorcist] Basilijs"), "f_pilgrimroad_41_2");

		AddObjective("killFlock", L("Kill Purple Banshees"),
			new KillObjective(10, new[] { MonsterId.Sec_Banshee_Purple }));

		AddObjective("killMatron", L("Defeat the Matron"),
			new KillObjective(1, new[] { MonsterId.Sec_Banshee_Purple }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}

public class FPilgrimroad412Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_2", 1006);
		SetName(L("Salvia Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Purple Banshees and Green Tiny Mages."));
		SetLocation("f_pilgrimroad_41_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim-Captain] Juozas"), "f_pilgrimroad_41_2");

		AddObjective("killBanshees", L("Kill Purple Banshees"),
			new KillObjective(12, new[] { MonsterId.Sec_Banshee_Purple }));

		AddObjective("killMages", L("Kill Green Tiny Mages"),
			new KillObjective(12, new[] { MonsterId.Tiny_Mage_Green }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}
