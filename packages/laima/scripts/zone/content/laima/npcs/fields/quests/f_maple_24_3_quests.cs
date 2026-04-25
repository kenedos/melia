//--- Melia Script ----------------------------------------------------------
// Northern Parias Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Northern Parias Forest (f_maple_24_3).
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

public class FMaple243QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Fragolin Thinning
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Forest-Ward] Rasa"), "f_maple_24_3", -1500, -1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1001);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Fragolins swarm the north trail in Parias. Forty-five killed clears the pilgrim road."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Pilgrims?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill forty-five Fragolins?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty-five. Watch the berry-patches."));
						}
						break;

					case "info":
						await dialog.Msg(L("Road runs to the Parias shrine. Swarm keeps pilgrims out."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays closed."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFragolins", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Pilgrims already passing."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Road's open. Shrine receives again."));
			}
		});

		// Quest 2: Cloverin Pollen
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Herbalist] Vaiva"), "f_maple_24_3", -600, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1002);

			dialog.SetTitle(L("Vaiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Cloverin pollen binds the forest wards. Kill thirty, bring eight pollen sacs."));

				var response = await dialog.Select(L("Pollen?"),
					Option(L("I'll bring"), "help"),
					Option(L("Wards?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty Cloverins and bring eight pollen sacs?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Sacs whole. They spoil if torn."));
						}
						break;

					case "info":
						await dialog.Msg(L("Wards keep rootcrystal corruption off the glade. Pollen feeds them."));
						break;

					case "leave":
						await dialog.Msg(L("Wards thin."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCloverins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPollen", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Eight sacs. Wards refed."));
					character.Inventory.Remove(650248, character.Inventory.CountItem(650248), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Wards hum steady again."));
			}
		});

		// Quest 3: Blueberrin Essence
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Alchemist] Eimis"), "f_maple_24_3", 400, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1003);

			dialog.SetTitle(L("Eimis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blueberrin essence cures the pilgrim fever. Kill fifteen, bring five essence-vials."));

				var response = await dialog.Select(L("Essence?"),
					Option(L("I'll bring"), "help"),
					Option(L("Fever?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Blueberrins and bring five essence-vials?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Don't shake them."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fever catches pilgrims off the road. Essence breaks it in a night."));
						break;

					case "leave":
						await dialog.Msg(L("Fever spreads."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBlueberrins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherEssence", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Five vials. Fever ward ready."));
					character.Inventory.Remove(650249, character.Inventory.CountItem(650249), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Fever broken in six pilgrims already."));
			}
		});

		// Quest 4: Rootcrystal Killing
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Crystal-Warder] Audra"), "f_maple_24_3", -900, -400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1004);

			dialog.SetTitle(L("Audra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Parias Rootcrystals pull the forest off-tone. Break forty-five to quiet the glade."));

				var response = await dialog.Select(L("Break?"),
					Option(L("I'll break"), "help"),
					Option(L("Off-tone?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Break forty-five Parias Rootcrystals?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty-five. Gloves on."));
						}
						break;

					case "info":
						await dialog.Msg(L("Forest hums flat. Pilgrims hear it and turn back."));
						break;

					case "leave":
						await dialog.Msg(L("Hum flat."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Glade tones clean."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Parias sings true again."));
			}
		});

		// Quest 5: The Fragolin Mother
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Tadas"), "f_maple_24_3", -1700, -1100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_maple_24_3.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Tadas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Fragolin Mother nests under the north ridge. Kill ten of her brood to draw her out."));

				var response = await dialog.Select(L("Mother?"),
					Option(L("I'll face her"), "help"),
					Option(L("Nest?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Fragolins and defeat the Mother when she emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("She breeds the swarm. End her, end the swarm."));
						break;

					case "leave":
						await dialog.Msg(L("Swarm grows."));
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
					await dialog.Msg(L("Swarm done."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Fragolin, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She comes!"));
							character.ServerMessage(L("{#FF9966}The Fragolin Mother emerges from the ridge!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find her."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ridge quiet. Pilgrims sleep easy."));
			}
		});

		// Quest 6: Parias Forest Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Ranger-Captain] Mindaugas"), "f_maple_24_3", 200, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_24_3", 1006);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Forest sweep. Twelve Fragolins, twelve Cloverins, twelve Blueberrins."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - Fragolins, Cloverins, Blueberrins?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Forest stays thick."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFragolins", out var fObj)) return;
				if (!quest.TryGetProgress("killCloverins", out var cObj)) return;
				if (!quest.TryGetProgress("killBlueberrins", out var bObj)) return;

				if (fObj.Done && cObj.Done && bObj.Done)
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
				await dialog.Msg(L("Rangers patrol the trail now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FMaple243Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1001);
		SetName(L("Fragolin Thinning"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Fragolins swarming the Parias pilgrim road."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forest-Ward] Rasa"), "f_maple_24_3");

		AddObjective("killFragolins", L("Kill Fragolins"),
			new KillObjective(45, new[] { MonsterId.Fragolin }));

		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(640081, 1));
		AddReward(new ItemReward(640002, 10));
		AddReward(new ItemReward(640005, 10));
	}
}

public class FMaple243Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1002);
		SetName(L("Cloverin Pollen"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Cloverins and bring pollen sacs for the forest wards."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Vaiva"), "f_maple_24_3");

		AddObjective("killCloverins", L("Kill Cloverins"),
			new KillObjective(30, new[] { MonsterId.Cloverin }));

		AddObjective("gatherPollen", L("Gather pollen sacs"),
			new CollectItemObjective(650248, 8));

		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(6500));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640002, 10));
		AddReward(new ItemReward(640005, 10));
		AddReward(new ItemReward(640008, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650248, character.Inventory.CountItem(650248), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650248, character.Inventory.CountItem(650248), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple243Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1003);
		SetName(L("Blueberrin Essence"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blueberrins and bring essence-vials for the pilgrim fever cure."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Eimis"), "f_maple_24_3");

		AddObjective("killBlueberrins", L("Kill Blueberrins"),
			new KillObjective(15, new[] { MonsterId.Blueberrin }));

		AddObjective("gatherEssence", L("Gather essence-vials"),
			new CollectItemObjective(650249, 5));

		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(6500));
		AddReward(new ItemReward(640081, 2));
		AddReward(new ItemReward(640002, 10));
		AddReward(new ItemReward(640005, 10));
		AddReward(new ItemReward(640008, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650249, character.Inventory.CountItem(650249), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650249, character.Inventory.CountItem(650249), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FMaple243Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1004);
		SetName(L("Rootcrystal Killing"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Parias Rootcrystals pulling the forest off-tone."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Warder] Audra"), "f_maple_24_3");

		AddObjective("breakCrystals", L("Break Parias Rootcrystals"),
			new KillObjective(45, new[] { MonsterId.Rootcrystal_01 }));

		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(640081, 1));
		AddReward(new ItemReward(640002, 10));
		AddReward(new ItemReward(640005, 10));
	}
}

public class FMaple243Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1005);
		SetName(L("The Fragolin Mother"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Fragolins to draw out the Fragolin Mother nesting under the ridge."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Tadas"), "f_maple_24_3");

		AddObjective("killPack", L("Kill Fragolins"),
			new KillObjective(10, new[] { MonsterId.Fragolin }));

		AddObjective("killAlpha", L("Defeat the Fragolin Mother"),
			new KillObjective(1, new[] { MonsterId.Fragolin }));

		AddReward(new ExpReward(1600, 1100));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640081, 3));
		AddReward(new ItemReward(640002, 12));
		AddReward(new ItemReward(640005, 12));
		AddReward(new ItemReward(640008, 4));
	}
}

public class FMaple243Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_24_3", 1006);
		SetName(L("Parias Forest Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Fragolins, Cloverins, and Blueberrins."));
		SetLocation("f_maple_24_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger-Captain] Mindaugas"), "f_maple_24_3");

		AddObjective("killFragolins", L("Kill Fragolins"),
			new KillObjective(12, new[] { MonsterId.Fragolin }));

		AddObjective("killCloverins", L("Kill Cloverins"),
			new KillObjective(12, new[] { MonsterId.Cloverin }));

		AddObjective("killBlueberrins", L("Kill Blueberrins"),
			new KillObjective(12, new[] { MonsterId.Blueberrin }));

		AddReward(new ExpReward(1600, 1100));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640081, 3));
		AddReward(new ItemReward(640002, 12));
		AddReward(new ItemReward(640005, 12));
		AddReward(new ItemReward(640008, 4));
	}
}
