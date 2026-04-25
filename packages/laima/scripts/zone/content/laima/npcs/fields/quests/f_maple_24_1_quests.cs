//--- Melia Script ----------------------------------------------------------
// Central Parias Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Central Parias Forest.
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

public class FMaple241QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Rudas Bloom
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Florist] Morta"), "f_maple_24_1", 0, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1001);

			dialog.SetTitle(L("Morta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Rudas Elavines bloom in Parias. The petals fetch silver at every Klaipeda flower stall."));
				await dialog.Msg(L("Kill twenty-five Rudas Elavines to drop enough petals for one season's stall."));

				var response = await dialog.Select(L("Petal run?"),
					Option(L("I'll harvest"), "help"),
					Option(L("Just petals?"), "info"),
					Option(L("Buy roses"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-five Rudas Elavines for their petals?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twenty-five. They shed only under stress."));
						}
						break;

					case "info":
						await dialog.Msg(L("The petals preserve - that's the magic. Cut flowers wilt in a day; Rudas petals keep a month."));
						break;

					case "leave":
						await dialog.Msg(L("Roses die. Rudas endure."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRudas", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Stall will be bright for weeks."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep harvesting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Petals sold out in three days."));
			}
		});

		// Quest 2: Atti Pollen
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Beekeeper] Kovas"), "f_maple_24_1", -1100, -600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1002);

			dialog.SetTitle(L("Kovas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Atti creatures carry rare pollen on their legs. Bees can't reach those blooms - but Attis walk right through them."));
				await dialog.Msg(L("Kill fifteen Attis and bring five pollen-clusters."));

				var response = await dialog.Select(L("Pollen run?"),
					Option(L("I'll kill and scrape"), "help"),
					Option(L("Why Attis?"), "info"),
					Option(L("Use honey"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Attis and bring five pollen-clusters?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Scrape the leg-joints. That's where the pollen packs."));
						}
						break;

					case "info":
						await dialog.Msg(L("They walk the inner grove. Bees stay perimeter."));
						break;

					case "leave":
						await dialog.Msg(L("Honey's only half of beekeeping."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killAttis", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPollen", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Five clusters. The hives will triple."));
					character.Inventory.Remove(650041, character.Inventory.CountItem(650041), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Hives are booming. Queens laying double."));
			}
		});

		// Quest 3: Delione Thinning
		//-------------------------------------------------------------------------
		AddNpc(20118, L("[Groundskeeper] Dovydas"), "f_maple_24_1", -900, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1003);

			dialog.SetTitle(L("Dovydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Deliones choke the inner paths. Thirty-five of them and the paths walk again."));

				var response = await dialog.Select(L("Thirty-five?"),
					Option(L("I'll thin them"), "help"),
					Option(L("Why so many?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty-five Deliones along the inner paths?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("They nest in clusters. Easy counting."));
						}
						break;

					case "info":
						await dialog.Msg(L("They breed faster than I can rake."));
						break;

					case "leave":
						await dialog.Msg(L("Paths will vanish by spring."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDeliones", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Paths walkable. Good work."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep thinning."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Visitors back on the inner paths."));
			}
		});

		// Quest 4: Cloverin Clovers
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Alchemist] Rasa"), "f_maple_24_1", 50, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1004);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Cloverins carry a four-lobed clover in their leaf-patterns. My luck potion needs eight of them."));
				await dialog.Msg(L("Kill twenty Cloverins for the lucky clovers."));

				var response = await dialog.Select(L("Lucky clovers?"),
					Option(L("I'll harvest"), "help"),
					Option(L("Does luck work?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty Cloverins and bring eight lucky clovers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Eight. Don't hold out for nine - that's superstition."));
						}
						break;

					case "info":
						await dialog.Msg(L("It works if you believe it works. Same as prayer."));
						break;

					case "leave":
						await dialog.Msg(L("Your loss."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCloverins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherClovers", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Eight clovers. Potion brews tonight."));
					character.Inventory.Remove(650051, character.Inventory.CountItem(650051), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep going."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Potion sold to a gambler. He tripled his purse."));
			}
		});

		// Quest 5: Elavine Matriarch
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Druid] Vaiva"), "f_maple_24_1", 1300, -100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1005);
			var matriarchSpawnedKey = "Laima.Quests.f_maple_24_1.Quest1005.MatriarchSpawned";

			dialog.SetTitle(L("Vaiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("An Elavine Matriarch gestates at the grove's heart. She seeds new generations. Kill enough of her daughters and she'll emerge in fury."));

				var response = await dialog.Select(L("Emerge how?"),
					Option(L("I'll draw her"), "help"),
					Option(L("Why kill?"), "info"),
					Option(L("Leave her"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Rudas Elavines and defeat the Matriarch when she emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten. She'll feel each one."));
						}
						break;

					case "info":
						await dialog.Msg(L("Her bloom spreads a rot. The grove dies by her."));
						break;

					case "leave":
						await dialog.Msg(L("Grove dies without this."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killElavines", out var eObj)) return;
				if (!quest.TryGetProgress("killMatriarch", out var mObj)) return;

				if (eObj.Done && mObj.Done)
				{
					await dialog.Msg(L("Matriarch's down. Grove's cleansed."));
					character.Variables.Perm.Remove(matriarchSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (eObj.Done && !mObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(matriarchSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(matriarchSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Rudas_Elavine, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She bursts from the heart-grove!"));
							character.ServerMessage(L("{#FF9966}The Elavine Matriarch erupts in bloom!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find her before she retreats."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten daughters first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Grove's healing. New growth everywhere."));
			}
		});

		// Quest 6: Parias Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Ranger] Justinas"), "f_maple_24_1", -300, 900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_1", 1006);

			dialog.SetTitle(L("Justinas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Standard Parias sweep - twelve each of the three main species."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Which three?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Rudas, twelve Deliones, and twelve Cloverins?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six. Standard count."));
						}
						break;

					case "info":
						await dialog.Msg(L("Rudas, Delione, Cloverin. The triad."));
						break;

					case "leave":
						await dialog.Msg(L("Next ranger will ask the same."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRudas", out var rObj)) return;
				if (!quest.TryGetProgress("killDeliones", out var dObj)) return;
				if (!quest.TryGetProgress("killCloverins", out var cObj)) return;

				if (rObj.Done && dObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Sweep done. Parias stable."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep sweeping."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Parias is walkable."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FMaple241Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1001);
		SetName(L("Rudas Bloom"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Rudas Elavines for preserving petals."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Florist] Morta"), "f_maple_24_1");

		AddObjective("killRudas", L("Kill Rudas Elavines"),
			new KillObjective(25, new[] { MonsterId.Rudas_Elavine }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
	}
}

public class FMaple241Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1002);
		SetName(L("Atti Pollen"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Attis and scrape inner-grove pollen for the beekeeper."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Beekeeper] Kovas"), "f_maple_24_1");

		AddObjective("killAttis", L("Kill Attis"),
			new KillObjective(15, new[] { MonsterId.Atti }));

		AddObjective("gatherPollen", L("Gather pollen-clusters"),
			new CollectItemObjective(650041, 5));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650041, character.Inventory.CountItem(650041), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650041, character.Inventory.CountItem(650041), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple241Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1003);
		SetName(L("Delione Thinning"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Deliones choking the inner Parias paths."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Groundskeeper] Dovydas"), "f_maple_24_1");

		AddObjective("killDeliones", L("Kill Deliones"),
			new KillObjective(35, new[] { MonsterId.Delione }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
	}
}

public class FMaple241Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1004);
		SetName(L("Lucky Clovers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Cloverins and gather lucky four-lobed clovers for luck potions."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Rasa"), "f_maple_24_1");

		AddObjective("killCloverins", L("Kill Cloverins"),
			new KillObjective(20, new[] { MonsterId.Cloverin }));

		AddObjective("gatherClovers", L("Gather lucky clovers"),
			new CollectItemObjective(650051, 8));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650051, character.Inventory.CountItem(650051), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650051, character.Inventory.CountItem(650051), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple241Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1005);
		SetName(L("The Elavine Matriarch"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Rudas Elavines to draw out the Matriarch rotting the heart-grove."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Druid] Vaiva"), "f_maple_24_1");

		AddObjective("killElavines", L("Kill Rudas Elavines"),
			new KillObjective(10, new[] { MonsterId.Rudas_Elavine }));

		AddObjective("killMatriarch", L("Defeat the Matriarch"),
			new KillObjective(1, new[] { MonsterId.Rudas_Elavine }));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}
}

public class FMaple241Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_1", 1006);
		SetName(L("Parias Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill the Parias triad: Rudas, Deliones, Cloverins."));
		SetLocation("f_maple_24_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger] Justinas"), "f_maple_24_1");

		AddObjective("killRudas", L("Kill Rudas Elavines"),
			new KillObjective(12, new[] { MonsterId.Rudas_Elavine }));

		AddObjective("killDeliones", L("Kill Deliones"),
			new KillObjective(12, new[] { MonsterId.Delione }));

		AddObjective("killCloverins", L("Kill Cloverins"),
			new KillObjective(12, new[] { MonsterId.Cloverin }));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}
}
