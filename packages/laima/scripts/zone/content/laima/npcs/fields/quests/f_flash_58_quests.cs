//--- Melia Script ----------------------------------------------------------
// Dingofasil District Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Dingofasil District (cursed petrification).
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

public class FFlash58QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Red Infroholder Raid
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[District-Ward] Vytautas"), "f_flash_58", 696, -106, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_58", 1001);

			dialog.SetTitle(L("Vytautas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Infroholders overrun Dingofasil since the curse fell. Forty-five kills and the petrified citizens get reach."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Citizens?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty-five. Watch the ground-haze."));
						break;

					case "info":
						await dialog.Msg(L("Hundreds petrified. We need room to work the counter-ritual."));
						break;

					case "leave":
						await dialog.Msg(L("Citizens stay stone."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killInfros", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Counter-ritual team in."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First citizen unpetrified yesterday."));
			}
		});

		// Quest 2: Socket Cores
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Cursebreaker] Algimantas"), "f_flash_58", 962, 527, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_58", 1002);

			dialog.SetTitle(L("Algimantas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Sockets anchor the petrification. Kill thirty, bring eight cores for the counter-ward."));

				var response = await dialog.Select(L("Cores?"),
					Option(L("I'll bring"), "help"),
					Option(L("Anchor?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cores only. The shells shatter."));
						break;

					case "info":
						await dialog.Msg(L("Each Socket pulls petrification in its radius. Break eight cores, the radius splits."));
						break;

					case "leave":
						await dialog.Msg(L("Curse deepens."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSockets", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCores", out var cObj)) return;

				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Eight cores. Counter-ward woven."));
					character.Inventory.Remove(650611, character.Inventory.CountItem(650611), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Counter-ward holds four blocks."));
			}
		});

		// Quest 3: Mage Pages
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Scribe] Egle-III"), "f_flash_58", -1109, -71, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_58", 1003);

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Infroholder Mages memorize the petrification cant. Kill fifteen, bring five cant-pages."));

				var response = await dialog.Select(L("Pages?"),
					Option(L("I'll bring"), "help"),
					Option(L("Reverse?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't read them aloud."));
						break;

					case "info":
						await dialog.Msg(L("Read backward, the cant lifts instead of lays. Maybe. Theory."));
						break;

					case "leave":
						await dialog.Msg(L("Theory stays theory."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPages", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Five pages. Reverse-cant tested tonight."));
					character.Inventory.Remove(650677, character.Inventory.CountItem(650677), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Reverse-cant works on fresh petrification. Old stone harder."));
			}
		});

		// Quest 5: The Stoneheart Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Ignas-II"), "f_flash_58", 1126, 1095, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_58", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_flash_58.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Ignas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("An Infroholder Stoneheart anchors the district curse. Kill ten to draw him from his plaza."));

				var response = await dialog.Select(L("Alpha?"),
					Option(L("I'll face him"), "help"),
					Option(L("Stoneheart?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten."));
						break;

					case "info":
						await dialog.Msg(L("His heartbeat drives the petrification pulse. End him, drop the pulse."));
						break;

					case "leave":
						await dialog.Msg(L("Curse beats on."));
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
					await dialog.Msg(L("Pulse stopped."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Infroholder_Red, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Stoneheart Alpha emerges from the plaza!{/}"));
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
				await dialog.Msg(L("Plaza's unlocked. Cursebreakers pouring in."));
			}
		});

	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FFlash58Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_58", 1001);
		SetName(L("Red Infroholder Raid"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Infroholders overrunning petrified Dingofasil."));
		SetLocation("f_flash_58");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[District-Ward] Vytautas"), "f_flash_58");

		AddObjective("killInfros", L("Kill Red Infroholders"),
			new KillObjective(45, new[] { MonsterId.Infroholder_Red }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FFlash58Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_58", 1002);
		SetName(L("Socket Curse-Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Sockets and bring curse-cores for the counter-ward."));
		SetLocation("f_flash_58");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Cursebreaker] Algimantas"), "f_flash_58");

		AddObjective("killSockets", L("Kill Purple Sockets"),
			new KillObjective(30, new[] { MonsterId.Socket_Purple }));

		AddObjective("gatherCores", L("Gather curse-cores from Purple Sockets"),
			new CollectItemObjective(650611, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650611, 0.40f, MonsterId.Socket_Purple);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650611, character.Inventory.CountItem(650611), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650611, character.Inventory.CountItem(650611), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FFlash58Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_58", 1003);
		SetName(L("Cant-Pages"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Infroholder Mages and bring cant-pages for reverse-study."));
		SetLocation("f_flash_58");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scribe] Egle"), "f_flash_58");

		AddObjective("killMages", L("Kill Green Infroholder Mages"),
			new KillObjective(15, new[] { MonsterId.Infroholder_Mage_Green }));

		AddObjective("gatherPages", L("Gather cant-pages from Green Infroholder Mages"),
			new CollectItemObjective(650677, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650677, 0.50f, MonsterId.Infroholder_Mage_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650677, character.Inventory.CountItem(650677), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650677, character.Inventory.CountItem(650677), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FFlash58Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_58", 1005);
		SetName(L("The Stoneheart Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Infroholders to draw out the Stoneheart Alpha anchoring the curse."));
		SetLocation("f_flash_58");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Ignas"), "f_flash_58");

		AddObjective("killPack", L("Kill Red Infroholders"),
			new KillObjective(10, new[] { MonsterId.Infroholder_Red }));

		AddObjective("killAlpha", L("Defeat the Stoneheart Alpha"),
			new KillObjective(1, new[] { MonsterId.Infroholder_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

