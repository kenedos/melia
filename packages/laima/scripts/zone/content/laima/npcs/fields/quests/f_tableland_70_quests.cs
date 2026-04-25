//--- Melia Script ----------------------------------------------------------
// Ibre Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Ibre Plateau.
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

public class FTableland70QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Purple Hohen Herd
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Herd-Ward] Rokas"), "f_tableland_70", 2200, -2800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1001);

			dialog.SetTitle(L("Rokas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Mane Hohens stampede the plateau. Thirty need killing before my herd shares their lung-sick."));

				var response = await dialog.Select(L("Lung-sick?"),
					Option(L("I'll kill"), "help"),
					Option(L("Why purple?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't inhale the mane-dust."));
						break;

					case "info":
						await dialog.Msg(L("Purple's the sick ones. The disease bleaches their mane."));
						break;

					case "leave":
						await dialog.Msg(L("Whole plateau's stock dies."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHohens", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Plague contained."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Herd's healthy. Good work."));
			}
		});

		// Quest 2: Hohen Mage Horns
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Hornwright] Marta"), "f_tableland_70", 4200, -2600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1002);

			dialog.SetTitle(L("Marta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Hohen Mages grow horns used for signal-trumpets. Six horns for the Orsha garrison order."));

				var response = await dialog.Select(L("Trumpet horns?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Break at the base - don't crack the spiral."));
						break;

					case "info":
						await dialog.Msg(L("Garrison pays fair."));
						break;

					case "leave":
						await dialog.Msg(L("Garrison will find another hornwright."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherHorns", out var hObj)) return;

				if (killObj.Done && hObj.Done)
				{
					await dialog.Msg(L("Six horns. Garrison trumpets ready."));
					character.Inventory.Remove(650071, character.Inventory.CountItem(650071), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Orsha's trumpets ring from the walls."));
			}
		});

		// Quest 3: Cronewt Scales
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Scalesmith] Valdas"), "f_tableland_70", 3500, -3500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1003);

			dialog.SetTitle(L("Valdas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Cronewt scales make flexible armor. Forty kills, ten prime scales."));

				var response = await dialog.Select(L("Armor scales?"),
					Option(L("I'll kill"), "help"),
					Option(L("Why Cronewts?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Only the ridgeline scales. Belly scales are useless."));
						break;

					case "info":
						await dialog.Msg(L("Their scale overlap flexes like dragon-armor."));
						break;

					case "leave":
						await dialog.Msg(L("Orsha guard will go plate."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCronewts", out var killObj)) return;
				if (!quest.TryGetProgress("gatherScales", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Ten prime scales. Armor set going to the captain."));
					character.Inventory.Remove(650072, character.Inventory.CountItem(650072), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Captain's wearing the set. Swears it turns arrows."));
			}
		});

		// Quest 4: Lapasape Bow Quiver
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Archer-Sergeant] Jurate"), "f_tableland_70", 2600, -1800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1004);

			dialog.SetTitle(L("Jurate"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Lapasape Bows harass my archer line. Kill fifteen and bring five of their quivers for study."));

				var response = await dialog.Select(L("Quivers?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Weave?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Leave the straps. Cut only the bodies."));
						break;

					case "info":
						await dialog.Msg(L("Woven fiber, light as wool. My archers want the pattern."));
						break;

					case "leave":
						await dialog.Msg(L("Archers lose every day without this."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBows", out var killObj)) return;
				if (!quest.TryGetProgress("gatherQuivers", out var qObj)) return;

				if (killObj.Done && qObj.Done)
				{
					await dialog.Msg(L("Five quivers. Pattern copied by week's end."));
					character.Inventory.Remove(650073, character.Inventory.CountItem(650073), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Lightweight quivers across the whole line now."));
			}
		});

		// Quest 5: The Hohen Stampede-Lord
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Balys"), "f_tableland_70", 4000, -4300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1005);
			var lordSpawnedKey = "Laima.Quests.f_tableland_70.Quest1005.LordSpawned";

			dialog.SetTitle(L("Balys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Stampede-Lord leads the Hohens. Thin ten and he charges."));

				var response = await dialog.Select(L("Charge?"),
					Option(L("I'll meet him"), "help"),
					Option(L("Lord?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten. Brace."));
						break;

					case "info":
						await dialog.Msg(L("Bull of the herd. Double size."));
						break;

					case "leave":
						await dialog.Msg(L("Herd rules Ibre without him dead."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHohens", out var hObj)) return;
				if (!quest.TryGetProgress("killLord", out var lObj)) return;

				if (hObj.Done && lObj.Done)
				{
					await dialog.Msg(L("Lord's down. Herd's scattered."));
					character.Variables.Perm.Remove(lordSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (hObj.Done && !lObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(lordSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(lordSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Hohen_Mane_Purple, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Charge!"));
							character.ServerMessage(L("{#FF9966}The Stampede-Lord charges across the plateau!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find him before he regroups."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ibre's walkable. Herd's broken."));
			}
		});

		// Quest 6: Ibre Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Master] Vaidotas"), "f_tableland_70", 3000, -3300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1006);

			dialog.SetTitle(L("Vaidotas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Standard Ibre sweep. Twelve Cronewts, twelve Hohens."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Standard bounty."));
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan still rolls."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCronewts", out var cObj)) return;
				if (!quest.TryGetProgress("killHohens", out var hObj)) return;

				if (cObj.Done && hObj.Done)
				{
					await dialog.Msg(L("Sweep done."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep going."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravan's through."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland70Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1001);
		SetName(L("Purple Hohen Herd"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill plague-bearing Purple Mane Hohens."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herd-Ward] Rokas"), "f_tableland_70");

		AddObjective("killHohens", L("Kill Purple Mane Hohens"),
			new KillObjective(30, new[] { MonsterId.Hohen_Mane_Purple }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FTableland70Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1002);
		SetName(L("Trumpet Horns"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Hohen Mages and bring horns for signal-trumpets."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hornwright] Marta"), "f_tableland_70");

		AddObjective("killMages", L("Kill Blue Hohen Mages"),
			new KillObjective(15, new[] { MonsterId.Hohen_Mage_Blue }));

		AddObjective("gatherHorns", L("Gather horns"),
			new CollectItemObjective(650071, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650071, character.Inventory.CountItem(650071), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650071, character.Inventory.CountItem(650071), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland70Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1003);
		SetName(L("Cronewt Scales"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Cronewts and gather prime scales for flex-armor."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scalesmith] Valdas"), "f_tableland_70");

		AddObjective("killCronewts", L("Kill Blue Cronewts"),
			new KillObjective(40, new[] { MonsterId.Cronewt_Blue }));

		AddObjective("gatherScales", L("Gather prime scales"),
			new CollectItemObjective(650072, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650072, character.Inventory.CountItem(650072), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650072, character.Inventory.CountItem(650072), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland70Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1004);
		SetName(L("Lightweight Quivers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Lapasape Bows and bring woven quivers for pattern study."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Archer-Sergeant] Jurate"), "f_tableland_70");

		AddObjective("killBows", L("Kill Blue Lapasape Bows"),
			new KillObjective(15, new[] { MonsterId.Lapasape_Bow_Blue }));

		AddObjective("gatherQuivers", L("Gather woven quivers"),
			new CollectItemObjective(650073, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650073, character.Inventory.CountItem(650073), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650073, character.Inventory.CountItem(650073), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland70Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1005);
		SetName(L("The Stampede-Lord"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin Hohens to meet the Stampede-Lord's charge."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Balys"), "f_tableland_70");

		AddObjective("killHohens", L("Thin Purple Mane Hohens"),
			new KillObjective(10, new[] { MonsterId.Hohen_Mane_Purple }));

		AddObjective("killLord", L("Defeat the Stampede-Lord"),
			new KillObjective(1, new[] { MonsterId.Hohen_Mane_Purple }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}

public class FTableland70Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1006);
		SetName(L("Ibre Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Blue Cronewts and Purple Mane Hohens."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Vaidotas"), "f_tableland_70");

		AddObjective("killCronewts", L("Kill Blue Cronewts"),
			new KillObjective(12, new[] { MonsterId.Cronewt_Blue }));

		AddObjective("killHohens", L("Kill Purple Mane Hohens"),
			new KillObjective(12, new[] { MonsterId.Hohen_Mane_Purple }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 3));
	}
}
