//--- Melia Script ----------------------------------------------------------
// Pilgrim Road West Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the west section of Pilgrim Road.
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

public class FPilgrimroad414QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Purple Repusbunny Kill
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Tollwarden] Mindaugas"), "f_pilgrimroad_41_4", -400, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Repusbunnies swarm the west road. Forty-five kills and pilgrims walk again."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Pilgrims?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty-five. Mind the burrows."));
						break;

					case "info":
						await dialog.Msg(L("The caravan's been camped behind the waystones three days. Road needs clearing."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays blocked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Caravan's moving."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First pilgrims passed at dawn."));
			}
		});

		// Quest 2: Bowbunny Fletchings
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Caravan-Guard] Ruta"), "f_pilgrimroad_41_4", 600, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bow Repusbunnies pick pilgrims off the ridge. Kill thirty, bring eight fletchings so we can mark the range."));

				var response = await dialog.Select(L("Fletchings?"),
					Option(L("I'll bring"), "help"),
					Option(L("Mark?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fletchings, not arrows. Arrows snap."));
						break;

					case "info":
						await dialog.Msg(L("Fletchings tell us the draw. Draw tells us the range. Range tells us where to station shields."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims stay pincushioned."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBowBunnies", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFletchings", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Eight fletchings. Range mapped."));
					character.Inventory.Remove(650254, character.Inventory.CountItem(650254), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Shield line holds. Ridge is quieter."));
			}
		});

		// Quest 3: Mage-Stub Bark
		//-------------------------------------------------------------------------
		AddNpc(153142, L("[Hedge-Witch] Vaiva"), "f_pilgrimroad_41_4", -500, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1003);

			dialog.SetTitle(L("Vaiva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tree-Mage Stubs carry rootspell in their bark. Kill fifteen, bring five strips."));

				var response = await dialog.Select(L("Bark?"),
					Option(L("I'll bring"), "help"),
					Option(L("Rootspell?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Strip clean. Bark with moss won't read."));
						break;

					case "info":
						await dialog.Msg(L("Older than the road. Hedge-charms still pull from it when nothing else answers."));
						break;

					case "leave":
						await dialog.Msg(L("Hedge goes silent, then."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStubs", out var killObj)) return;
				if (!quest.TryGetProgress("gatherBark", out var bObj)) return;

				if (killObj.Done && bObj.Done)
				{
					await dialog.Msg(L("Five strips. Hedge wards will hold the month."));
					character.Inventory.Remove(650256, character.Inventory.CountItem(650256), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Wards burning green. Good sign."));
			}
		});

		// Quest 4: Waystone Rootcrystals
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Waystone-Keeper] Darius"), "f_pilgrimroad_41_4", -1100, -500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1004);

			dialog.SetTitle(L("Darius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Rootcrystals tangle the waystones off-route. Break twelve to straighten the road."));

				var response = await dialog.Select(L("Break?"),
					Option(L("I'll break"), "help"),
					Option(L("Tangle?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve. Keep count."));
						break;

					case "info":
						await dialog.Msg(L("They bend the ley. Pilgrims walking by eye end up a league off."));
						break;

					case "leave":
						await dialog.Msg(L("Road stays crooked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Ley's straight. Waystones read true."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravans walking straight lines again."));
			}
		});

		// Quest 5: The Warren-King
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Bounty Hunter] Saule"), "f_pilgrimroad_41_4", 1800, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_pilgrimroad_41_4.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Saule"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Warren-King runs the west warren. Kill ten to draw him up."));

				var response = await dialog.Select(L("King?"),
					Option(L("I'll face him"), "help"),
					Option(L("Warren?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten."));
						break;

					case "info":
						await dialog.Msg(L("Tunnel network runs the whole ridge. King sits the deepest chamber. Thin him ten and he comes up."));
						break;

					case "leave":
						await dialog.Msg(L("Warren swells."));
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
					await dialog.Msg(L("Warren's done."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Repusbunny_Purple, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Warren-King emerges from the west warren!{/}"));
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
				await dialog.Msg(L("Warren's collapsing on itself. Fine by me."));
			}
		});

		// Quest 6: West Road Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Road-Marshal] Aldona"), "f_pilgrimroad_41_4", 1700, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_pilgrimroad_41_4", 1006);

			dialog.SetTitle(L("Aldona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("West road sweep. Twelve Repusbunnies, twelve Bow Repusbunnies, twelve Tree-Mage Stubs."));

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
						await dialog.Msg(L("Road stays thick."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var bObj)) return;
				if (!quest.TryGetProgress("killBowBunnies", out var wObj)) return;
				if (!quest.TryGetProgress("killStubs", out var sObj)) return;

				if (bObj.Done && wObj.Done && sObj.Done)
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
				await dialog.Msg(L("Marshal's patrols hold the stretch."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FPilgrimroad414Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1001);
		SetName(L("Purple Repusbunny Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Repusbunnies blocking the west pilgrim road."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tollwarden] Mindaugas"), "f_pilgrimroad_41_4");

		AddObjective("killBunnies", L("Kill Purple Repusbunnies"),
			new KillObjective(45, new[] { MonsterId.Repusbunny_Purple }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FPilgrimroad414Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1002);
		SetName(L("Bowbunny Fletchings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bow Repusbunnies and bring fletchings to map the ridge shots."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan-Guard] Ruta"), "f_pilgrimroad_41_4");

		AddObjective("killBowBunnies", L("Kill Bow Repusbunnies"),
			new KillObjective(30, new[] { MonsterId.Repusbunny_Bow_Purple }));

		AddObjective("gatherFletchings", L("Gather fletchings"),
			new CollectItemObjective(650254, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650254, character.Inventory.CountItem(650254), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650254, character.Inventory.CountItem(650254), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad414Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1003);
		SetName(L("Mage-Stub Bark"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tree-Mage Stubs and bring rootspell bark for hedge wards."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hedge-Witch] Vaiva"), "f_pilgrimroad_41_4");

		AddObjective("killStubs", L("Kill Tree-Mage Stubs"),
			new KillObjective(15, new[] { MonsterId.Stub_Tree_Mage }));

		AddObjective("gatherBark", L("Gather bark strips"),
			new CollectItemObjective(650256, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650256, character.Inventory.CountItem(650256), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650256, character.Inventory.CountItem(650256), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FPilgrimroad414Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1004);
		SetName(L("Waystone Tangles"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Rootcrystals bending the waystone ley off the west road."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Waystone-Keeper] Darius"), "f_pilgrimroad_41_4");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(12, new[] { MonsterId.Rootcrystal_05 }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}

public class FPilgrimroad414Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1005);
		SetName(L("The Warren-King"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Repusbunnies to draw out the Warren-King from the west warren."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Saule"), "f_pilgrimroad_41_4");

		AddObjective("killPack", L("Kill Purple Repusbunnies"),
			new KillObjective(10, new[] { MonsterId.Repusbunny_Purple }));

		AddObjective("killAlpha", L("Defeat the Warren-King"),
			new KillObjective(1, new[] { MonsterId.Repusbunny_Purple }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}

public class FPilgrimroad414Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_pilgrimroad_41_4", 1006);
		SetName(L("West Road Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Purple Repusbunnies, Bow Repusbunnies, and Tree-Mage Stubs."));
		SetLocation("f_pilgrimroad_41_4");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road-Marshal] Aldona"), "f_pilgrimroad_41_4");

		AddObjective("killBunnies", L("Kill Purple Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny_Purple }));

		AddObjective("killBowBunnies", L("Kill Bow Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny_Bow_Purple }));

		AddObjective("killStubs", L("Kill Tree-Mage Stubs"),
			new KillObjective(12, new[] { MonsterId.Stub_Tree_Mage }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(18800));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}
