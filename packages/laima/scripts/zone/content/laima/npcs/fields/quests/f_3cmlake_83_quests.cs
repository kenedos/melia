//--- Melia Script ----------------------------------------------------------
// Pelke Shrine Ruins Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Pelke Shrine Ruins.
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

public class F3Cmlake83QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Rajatadpole Bloom — Kristupas
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Shrine-Ward] Kristupas"), "f_3cmlake_83", 645, 228, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_83", 1001);

			dialog.SetTitle(L("Kristupas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The shrine pond is overrun with Rajatadpoles. Pilgrims come here for blessed water and find filth instead."));
				await dialog.Msg(L("Please kill 45 of them. That should be enough to clear the pond."));

				var response = await dialog.Select(L("Will you help clear the pond?"),
					Option(L("I'll kill them."), "help"),
					Option(L("Why is this water sacred?"), "info"),
					Option(L("Not today."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thank you. Be careful — the pond is deeper than it looks."));
						break;

					case "info":
						await dialog.Msg(L("Pelke blessed this pond long ago. Pilgrims still travel here from across the lake to drink from it."));
						break;

					case "leave":
						await dialog.Msg(L("Come back if you change your mind. The pilgrims need clean water."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thinTadpoles", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The pond is clear. I can see the bottom for the first time in months. Thank you."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("There are still plenty left. Keep killing them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pilgrims have started returning to drink from the pond. We owe you for that."));
			}
		});

		// Quest 2: Forge Provisions — Milda-II
		//-------------------------------------------------------------------------
		AddNpc(147515, L("[Armorer] Milda-II"), "f_3cmlake_83", -884, 1075, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_83", 1002);

			dialog.SetTitle(L("Milda-II"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("My forge needs materials from the swamp — tadpole hide for padding and shaman thread for lining."));
				await dialog.Msg(L("Please kill 25 Rajatadpoles and 15 Merog Shamans. I'll collect what I need from the bodies."));

				var response = await dialog.Select(L("Will you take the job?"),
					Option(L("I'll do it."), "help"),
					Option(L("Why both?"), "info"),
					Option(L("I'll pass."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thank you. Watch the Shamans — they call other Merogs to help when they're losing."));
						break;

					case "info":
						await dialog.Msg(L("Tadpole hide is stretchy and good for joints. Shaman thread is strong. Together they make light, tough armor."));
						break;

					case "leave":
						await dialog.Msg(L("Come back if you change your mind. The forge is right here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("huntTadpoles", out var tObj)) return;
				if (!quest.TryGetProgress("huntShamans", out var sObj)) return;

				if (tObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Both quotas filled. Here's your pay."));
					character.Quests.Complete(questId);
				}
				else if (!tObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Shamans done. I still need the Rajatadpoles."));
				}
				else if (tObj.Done && !sObj.Done)
				{
					await dialog.Msg(L("Tadpoles done. I still need the Merog Shamans."));
				}
				else
				{
					await dialog.Msg(L("Still need both Rajatadpoles and Merog Shamans. Whichever is closer, hunt those first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The first set of pond armor is already in use. It's holding up well. Thank you."));
			}
		});

		// Quest 3: Stolen Scrolls — Donatas (pure collection)
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Archivist] Donatas"), "f_3cmlake_83", -892, -370, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_83", 1003);

			dialog.SetTitle(L("Donatas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Merog Shamans carry scroll-pouches. Their ancestors looted our shrine library centuries ago, and the scrolls have been passed down ever since."));
				await dialog.Msg(L("Please recover 6 scroll-pouches from them. Once I have those, we can start restoring the library."));

				var response = await dialog.Select(L("Will you recover the scrolls?"),
					Option(L("I'll bring them back."), "help"),
					Option(L("What library?"), "info"),
					Option(L("Another time."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Keep the pouches dry — wet parchment is ruined parchment. Not every Shaman carries one, so you may need to kill many."));
						break;

					case "info":
						await dialog.Msg(L("Our shrine library was the largest archive west of Klaipeda before the Merog raid. The scrolls they carry are copies, but they're all we have left."));
						break;

					case "leave":
						await dialog.Msg(L("Come back when you're ready. The shelves can wait a little longer."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("recoverScrolls", out var sObj)) return;

				if (sObj.Done)
				{
					await dialog.Msg(L("All six pouches, dry and intact. I'll have them catalogued by week's end. Thank you for restoring the library."));
					character.Inventory.Remove(650557, character.Inventory.CountItem(650557), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting Merog Shamans. Not all of them carry pouches, so you may need to kill many."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Scholars from as far as Fedimian have come to read the recovered texts. Your name is in our donor ledger."));
			}
		});

		// Quest 4: Water-Mana Shards — Eidante
		//-------------------------------------------------------------------------
		AddNpc(150183, L("[Crystal Scholar] Eidante"), "f_3cmlake_83", -1795, -390, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_83", 1004);

			dialog.SetTitle(L("Eidante"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Tree Root Crystals around the Pelke shrine give off an unusual energy I've never recorded anywhere else. I'm here to study them."));
				await dialog.Msg(L("Please break 3 of the active crystals so I can examine the fragments, then collect 10 loose water-mana shards from inside the shrine ruins."));

				var response = await dialog.Select(L("Will you help with my research?"),
					Option(L("I'll gather what you need."), "help"),
					Option(L("What's special about them?"), "info"),
					Option(L("Find someone else."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Break the crystals however you like. The loose shards in the ruins are more delicate — pick those up carefully, one at a time."));
						break;

					case "info":
						await dialog.Msg(L("The crystals here resonate at a frequency I've never measured anywhere else. I believe the shrine pond is the cause, and the academy wants proof."));
						break;

					case "leave":
						await dialog.Msg(L("That's fine. Come back if you change your mind — the shards aren't going anywhere."));
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
					await dialog.Msg(L("Ten clean shards. This is exactly what I needed — my research is going to make a real impact now."));
					character.Inventory.Remove(650779, character.Inventory.CountItem(650779), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep going. Break 3 active crystals and collect 10 loose shards from inside the ruins."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("My paper was accepted by the academy. Your name is in the credits."));
			}
		});

			void AddRuinCrystal(int spotNumber, int x, int z, int direction)
			{
				AddNpc(150029, L("Pelke Water-Mana Shard"), "f_3cmlake_83", x, z, direction, async dialog =>
				{
					var character = dialog.Player;
					var questId = new QuestId("f_3cmlake_83", 1004);

					if (!character.Quests.IsActive(questId))
					{
						await dialog.Msg(L("{#666666}*A loose water-mana shard rests in the rubble. Without the right tools, picking it up would only damage it.*{/}"));
						return;
					}

					var variableKey = $"Laima.Quests.f_3cmlake_83.Quest1004.Spot{spotNumber}";
					if (character.Variables.Perm.GetBool(variableKey, false))
					{
						await dialog.Msg(L("{#666666}*The shard that was here has already been collected.*{/}"));
						return;
					}

					var result = await character.TimeActions.StartAsync(L("Collecting the shard..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4));

					if (result == TimeActionResult.Completed)
					{
						character.Inventory.Add(650779, 1, InventoryAddType.PickUp);
						character.Variables.Perm.Set(variableKey, true);

						var currentCount = character.Inventory.CountItem(650779);
						character.ServerMessage(LF("Water-mana shards collected: {0}/10", currentCount));

						if (currentCount >= 10)
							character.ServerMessage(L("{#FFD700}All shards collected. Return to Crystal Scholar Eidante.{/}"));
					}
					else
					{
						character.ServerMessage(L("Collection interrupted."));
					}
				});
			}

			AddRuinCrystal(1, -1896, -382, 45);
			AddRuinCrystal(2, -2431, -455, 0);
			AddRuinCrystal(3, -2292, -411, 0);
			AddRuinCrystal(4, -2329, -559, 180);
			AddRuinCrystal(5, -2095, -528, 180);
			AddRuinCrystal(6, -1942, -463, 0);
			AddRuinCrystal(7, -1804, -549, 135);
			AddRuinCrystal(8, -1609, -605, 135);
			AddRuinCrystal(9, -1598, -782, 90);
			AddRuinCrystal(10, -1665, -964, 90);
			AddRuinCrystal(11, -1441, -913, 89);
			AddRuinCrystal(12, -1441, -1029, 225);
			AddRuinCrystal(13, -1437, -630, 315);
			AddRuinCrystal(14, -1437, -335, 315);
			AddRuinCrystal(15, -1487, -184, 0);

		// Quest 5: The Pond Hydra — Arnas
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Hunter] Arnas"), "f_3cmlake_83", -1505, 361, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_83", 1005);

			dialog.SetTitle(L("Arnas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("There's an ancient Hydra living in the pond. It won't come out unless its brood is threatened."));
				await dialog.Msg(L("Kill 10 Merog Stingers and the Hydra will surface to fight you."));

				var response = await dialog.Select(L("Will you fight it?"),
					Option(L("I'll face it."), "help"),
					Option(L("How old is it?"), "info"),
					Option(L("Not my fight."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Kill exactly 10 Stingers. The Hydra will surface as soon as the last one falls. Stay near the shore so you can fight on solid ground."));
						break;

					case "info":
						await dialog.Msg(L("Centuries old, by its size. Its hide has bronze fittings from the old shrine fused into it. Recover those and the priests can rebuild."));
						break;

					case "leave":
						await dialog.Msg(L("The offer stands. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thinStingers", out var cObj)) return;
				if (!quest.TryGetProgress("slayHydra", out var hObj)) return;

				if (hObj.Done)
				{
					await dialog.Msg(L("The Hydra is dead and the pond is safe again. The shrine bronze is being delivered to the priests. Good work."));
					character.Quests.Complete(questId);
				}
				else if (cObj.Done)
				{
					await dialog.Msg(L("The Hydra has surfaced. Find it and kill it before it dives again."));
				}
				else
				{
					await dialog.Msg(L("Kill 10 Stingers first. The Hydra won't surface for less."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The shrine has been restored, and the priests hung my old hunting bow on the wall as thanks."));
			}
		});

	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class F3Cmlake83Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_83", 1001);
		SetName(L("Rajatadpole Bloom"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin out the Rajatadpoles choking the Pelke shrine pond."));
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shrine-Ward] Kristupas"), "f_3cmlake_83");

		AddObjective("thinTadpoles", L("Thin Rajatadpoles"),
			new KillObjective(45, new[] { MonsterId.Rajatadpole }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class F3Cmlake83Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_83", 1002);
		SetName(L("Forge Provisions"));
		SetType(QuestType.Sub);
		SetDescription(L("Hunt Rajatadpoles and Merog Shamans for Milda's field forge."));
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Armorer] Milda-II"), "f_3cmlake_83");

		AddObjective("huntTadpoles", L("Hunt Rajatadpoles"),
			new KillObjective(25, new[] { MonsterId.Rajatadpole }));

		AddObjective("huntShamans", L("Hunt Merog Shamans"),
			new KillObjective(15, new[] { MonsterId.Sec_Merog_Wizzard }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

public class F3Cmlake83Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_83", 1003);
		SetName(L("Stolen Scrolls"));
		SetType(QuestType.Sub);
		SetDescription(L("Recover scroll-pouches from Merog Shamans for the shrine library."));
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Archivist] Donatas"), "f_3cmlake_83");

		AddObjective("recoverScrolls", L("Recover scroll-pouches from Merog Shaman"),
			new CollectItemObjective(650557, 6));

		this.AddDrop(650557, 0.35f, MonsterId.Sec_Merog_Wizzard);

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650557, character.Inventory.CountItem(650557), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650557, character.Inventory.CountItem(650557), InventoryItemRemoveMsg.Destroyed);
	}
}

public class F3Cmlake83Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_83", 1004);
		SetName(L("Water-Mana Shards"));
		SetType(QuestType.Sub);
		SetDescription(L("Strike the Pelke Shrine Ruins crystals and gather water-mana shards for Eidante."));
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal Scholar] Eidante"), "f_3cmlake_83");

		AddObjective("breakCrystals", L("Break Tree Root Crystals"),
			new KillObjective(3, new[] { MonsterId.Rootcrystal_01 }));

		AddObjective("gatherShards", L("Gather water-mana shards from the shrine ruins crystals"),
			new CollectItemObjective(650779, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650779, character.Inventory.CountItem(650779), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650779, character.Inventory.CountItem(650779), InventoryItemRemoveMsg.Destroyed);
	}
}

public class F3Cmlake83Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_83", 1005);
		SetName(L("The Pond Hydra"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin Merog Stingers to provoke the ancient Hydra of the shrine pond, then slay it."));
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Hunter] Arnas"), "f_3cmlake_83");

		AddObjective("thinStingers", L("Thin Merog Stingers"),
			new KillObjective(10, new[] { MonsterId.Sec_Merog_Wogu }));

		AddObjective("slayHydra", L("Slay the Hydra"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Hydra, 1) },
				resetIdent: "thinStingers",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(243112, 1));
		AddReward(new ItemReward(640086, 5));
		AddReward(new ItemReward(640004, 6));
		AddReward(new ItemReward(640007, 6));
		AddReward(new ItemReward(640013, 3));
	}
}

