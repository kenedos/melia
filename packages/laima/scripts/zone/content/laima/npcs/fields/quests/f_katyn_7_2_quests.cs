//--- Melia Script ----------------------------------------------------------
// Katyn 7-2 Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Katyn 7-2 (coastal graveyard, crypts, reef shore).
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

public class FKatyn72QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Sakmoli Tide
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Shore-Warden] Mindaugas"), "f_katyn_7_2", 2750, 750, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Sakmoli wash up the dunes after every tide. Forty kills and the graves stay covered."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Graves?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty. Watch the wet sand."));
						break;

					case "info":
						await dialog.Msg(L("They drag the bones up. We rebury at dusk, they unbury by dawn."));
						break;

					case "leave":
						await dialog.Msg(L("Tide keeps pulling."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSakmoli", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Shore quiets."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Reburied two rows yesterday."));
			}
		});

		// Quest 2: Ellomago Shades
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Exorcist] Ruta"), "f_katyn_7_2", 2200, -2800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ellomago shades drift the crypt aisles. Kill twenty-eight, bring seven fading spirits to anchor the ward."));

				var response = await dialog.Select(L("Spirits?"),
					Option(L("I'll bring"), "help"),
					Option(L("Anchor?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Salt the jars before sealing."));
						break;

					case "info":
						await dialog.Msg(L("Each spirit weighs the ward. Seven and the crypt door holds till next moon."));
						break;

					case "leave":
						await dialog.Msg(L("Door creaks open."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killEllomago", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSpirits", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Seven spirits sealed."));
					character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ward holds. Crypt's quiet for once."));
			}
		});

		// Quest 3: Red Jellyfish Reef
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Reef-Harvester] Egle"), "f_katyn_7_2", 2000, 600, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1003);

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Jellyfish strangle the coral. Kill eighteen, bring six blue corals before the bloom suffocates the reef."));

				var response = await dialog.Select(L("Coral?"),
					Option(L("I'll bring"), "help"),
					Option(L("Bloom?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut clean. Their sting holds an hour after they die."));
						break;

					case "info":
						await dialog.Msg(L("Reef breathes through the coral. Coral dies, fishery dies, village starves."));
						break;

					case "leave":
						await dialog.Msg(L("Reef chokes."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killJelly", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCoral", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Six corals. Reef can rebuild from these."));
					character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Coral grafts taking. Reef colors back by autumn."));
			}
		});

		// Quest 4: Ridimed Bog
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Bog-Warden] Jurate"), "f_katyn_7_2", 400, -600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1004);

			dialog.SetTitle(L("Jurate"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ridimed crawl the west bog. Kill eighteen and the path opens to the salt-flats."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Path?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Eighteen. Don't sink past the knee."));
						break;

					case "info":
						await dialog.Msg(L("Salt-flats hold our preserve stones. No salt, no winter food."));
						break;

					case "leave":
						await dialog.Msg(L("Path stays drowned."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRidimed", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Bog clears."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salt-cart through this morning."));
			}
		});

		// Quest 5: The Sakmoli Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Algirdas"), "f_katyn_7_2", 3500, -4200, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_katyn_7_2.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Algirdas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Sakmoli Alpha leads the tide-pack. Kill ten of his kin to drag him from the breakers."));

				var response = await dialog.Select(L("Alpha?"),
					Option(L("I'll face him"), "help"),
					Option(L("Alpha?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten."));
						break;

					case "info":
						await dialog.Msg(L("He pulls the tide. End him, the shore stays buried."));
						break;

					case "leave":
						await dialog.Msg(L("Tide rolls on."));
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
					await dialog.Msg(L("Tide stilled."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Sakmoli, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Sakmoli Alpha rises from the breakers!{/}"));
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
				await dialog.Msg(L("Pack's leaderless. Shore breathes."));
			}
		});

		// Quest 6: Katyn Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Tomas"), "f_katyn_7_2", 2700, 2400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1006);

			dialog.SetTitle(L("Tomas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Coastal sweep. Twelve Sakmoli, twelve Ridimed, twelve Red Jellyfish."));

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
						await dialog.Msg(L("Coast stays choked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSakmoli", out var sObj)) return;
				if (!quest.TryGetProgress("killRidimed", out var rObj)) return;
				if (!quest.TryGetProgress("killJelly", out var jObj)) return;

				if (sObj.Done && rObj.Done && jObj.Done)
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
				await dialog.Msg(L("Patrols cover the dunes now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FKatyn72Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1001);
		SetName(L("Sakmoli Tide"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Sakmoli washing up the Katyn shore."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shore-Warden] Mindaugas"), "f_katyn_7_2");

		AddObjective("killSakmoli", L("Kill Sakmoli"),
			new KillObjective(40, new[] { MonsterId.Sakmoli }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FKatyn72Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1002);
		SetName(L("Crypt Shades"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ellomago and gather fading spirits to anchor the crypt ward."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Exorcist] Ruta"), "f_katyn_7_2");

		AddObjective("killEllomago", L("Kill Ellomago"),
			new KillObjective(28, new[] { MonsterId.Ellomago }));

		AddObjective("gatherSpirits", L("Gather fading spirits"),
			new CollectItemObjective(664096, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn72Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1003);
		SetName(L("Reef Harvest"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Jellyfish and gather blue corals before the bloom kills the reef."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Reef-Harvester] Egle"), "f_katyn_7_2");

		AddObjective("killJelly", L("Kill Red Jellyfish"),
			new KillObjective(18, new[] { MonsterId.Jellyfish_Red }));

		AddObjective("gatherCoral", L("Gather blue corals"),
			new CollectItemObjective(668044, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn72Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1004);
		SetName(L("Bog Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Ridimed crawling the west Katyn bog."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bog-Warden] Jurate"), "f_katyn_7_2");

		AddObjective("killRidimed", L("Kill Ridimed"),
			new KillObjective(18, new[] { MonsterId.Ridimed }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FKatyn72Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1005);
		SetName(L("The Sakmoli Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Sakmoli to drag the tide-pack Alpha from the breakers."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Algirdas"), "f_katyn_7_2");

		AddObjective("killPack", L("Kill Sakmoli"),
			new KillObjective(10, new[] { MonsterId.Sakmoli }));

		AddObjective("killAlpha", L("Defeat the Sakmoli Alpha"),
			new KillObjective(1, new[] { MonsterId.Sakmoli }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

public class FKatyn72Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1006);
		SetName(L("Katyn Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Sakmoli, Ridimed, and Red Jellyfish along the Katyn coast."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Tomas"), "f_katyn_7_2");

		AddObjective("killSakmoli", L("Kill Sakmoli"),
			new KillObjective(12, new[] { MonsterId.Sakmoli }));

		AddObjective("killRidimed", L("Kill Ridimed"),
			new KillObjective(12, new[] { MonsterId.Ridimed }));

		AddObjective("killJelly", L("Kill Red Jellyfish"),
			new KillObjective(12, new[] { MonsterId.Jellyfish_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
