//--- Melia Script ----------------------------------------------------------
// Cobalt Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Cobalt Forest.
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

public class FHuevillage583QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Caro Overpopulation
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Forester] Akvile"), "f_huevillage_58_3", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1001);

			dialog.SetTitle(L("Akvile"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Caros overpopulate Cobalt Forest. Kill forty before they strip the undergrowth."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Why forty?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty."));
						break;

					case "info":
						await dialog.Msg(L("Any less and they bounce back by autumn."));
						break;

					case "leave":
						await dialog.Msg(L("Forest needs balance."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCaros", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Balanced. Undergrowth's back."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Forest floor green again."));
			}
		});

		// Quest 2: Tipio Feathers
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Fletcher] Ruta-II"), "f_huevillage_58_3", -1200, -1200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tipio feathers fletch the fastest arrows. Thirty kills, eight prime feathers."));

				var response = await dialog.Select(L("Arrows?"),
					Option(L("I'll bring"), "help"),
					Option(L("Why Tipio?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Tail-feathers. Long and stiff."));
						break;

					case "info":
						await dialog.Msg(L("Aerodynamic. Two feet more range per fletch."));
						break;

					case "leave":
						await dialog.Msg(L("Orsha archers disappointed."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTipios", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFeathers", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Eight feathers. Fast arrows incoming."));
					character.Inventory.Remove(650275, character.Inventory.CountItem(650275), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Orsha order exceeded."));
			}
		});

		// Quest 3: Tiny Bow Quivers
		//-------------------------------------------------------------------------
		AddNpc(20011, L("[Scout-Captain] Norbertas"), "f_huevillage_58_3", 500, 200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1003);

			dialog.SetTitle(L("Norbertas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tiny Bows ambush my scouts. Kill fifteen, bring five quivers for ambush-pattern study."));

				var response = await dialog.Select(L("Quivers?"),
					Option(L("I'll bring"), "help"),
					Option(L("Why study?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Smallest quivers, hardest to spot."));
						break;

					case "info":
						await dialog.Msg(L("Ambush-placement tells me their routes."));
						break;

					case "leave":
						await dialog.Msg(L("More scouts die."));
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
					await dialog.Msg(L("Five quivers. Routes mapped."));
					character.Inventory.Remove(650280, character.Inventory.CountItem(650280), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ambushes failing. Scouts safer."));
			}
		});

		// Quest 4: Doyor Pack
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Huntress] Giedre"), "f_huevillage_58_3", 1700, -500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1004);

			dialog.SetTitle(L("Giedre"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Doyor packs plague the east. Thirty kills and they retreat."));

				var response = await dialog.Select(L("Packs?"),
					Option(L("I'll kill"), "help"),
					Option(L("Retreat?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thirty. They hunt in sixes."));
						break;

					case "info":
						await dialog.Msg(L("Pack thins, they scatter."));
						break;

					case "leave":
						await dialog.Msg(L("They reach the village next."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDoyors", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Packs retreated. Village safe."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Peace for a season."));
			}
		});

		// Quest 5: The Upent Lord
		//-------------------------------------------------------------------------
		AddNpc(147418, L("[Bounty Hunter] Egle-II"), "f_huevillage_58_3", 1100, -700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1005);
			var lordSpawnedKey = "Laima.Quests.f_huevillage_58_3.Quest1005.LordSpawned";

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Upents roam solitary, huge. Kill ten Caros to anger their Upent Lord."));

				var response = await dialog.Select(L("Lord?"),
					Option(L("I'll face him"), "help"),
					Option(L("Why Caros?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The Caros are his territory's lesser. He defends them."));
						break;

					case "info":
						await dialog.Msg(L("Protector-beast. Old code."));
						break;

					case "leave":
						await dialog.Msg(L("He eats cattle too."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCaros", out var cObj)) return;
				if (!quest.TryGetProgress("killLord", out var lObj)) return;

				if (cObj.Done && lObj.Done)
				{
					await dialog.Msg(L("Lord's down."));
					character.Variables.Perm.Remove(lordSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (cObj.Done && !lObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(lordSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(lordSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Upent, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Upent Lord thunders in!{/}"));
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
				await dialog.Msg(L("Cobalt's quiet."));
			}
		});

		// Quest 6: Cobalt Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Guard] Mindaugas-II"), "f_huevillage_58_3", -600, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1006);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Sweep. Twelve Caros, twelve Tipios, twelve Doyors."));

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
						await dialog.Msg(L("Caravan rolls."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCaros", out var cObj)) return;
				if (!quest.TryGetProgress("killTipios", out var tObj)) return;
				if (!quest.TryGetProgress("killDoyors", out var dObj)) return;

				if (cObj.Done && tObj.Done && dObj.Done)
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
				await dialog.Msg(L("Caravan through."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FHuevillage583Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1001);
		SetName(L("Caro Overpopulation"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Caros overpopulating Cobalt Forest."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forester] Akvile"), "f_huevillage_58_3");

		AddObjective("killCaros", L("Kill Caros"),
			new KillObjective(40, new[] { MonsterId.Caro }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FHuevillage583Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1002);
		SetName(L("Fast-Arrow Feathers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tipios and bring prime feathers for fast arrows."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Ruta"), "f_huevillage_58_3");

		AddObjective("killTipios", L("Kill Tipios"),
			new KillObjective(30, new[] { MonsterId.Tipio }));

		AddObjective("gatherFeathers", L("Gather prime feathers"),
			new CollectItemObjective(650275, 8));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650275, character.Inventory.CountItem(650275), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650275, character.Inventory.CountItem(650275), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FHuevillage583Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1003);
		SetName(L("Ambush Quivers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tiny Bows and bring ambush quivers for scout study."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout-Captain] Norbertas"), "f_huevillage_58_3");

		AddObjective("killBows", L("Kill Tiny Bows"),
			new KillObjective(15, new[] { MonsterId.Tiny_Bow }));

		AddObjective("gatherQuivers", L("Gather ambush quivers"),
			new CollectItemObjective(650280, 5));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650280, character.Inventory.CountItem(650280), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650280, character.Inventory.CountItem(650280), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FHuevillage583Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1004);
		SetName(L("Doyor Pack Retreat"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Doyor packs to drive them from the east of Cobalt."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Huntress] Giedre"), "f_huevillage_58_3");

		AddObjective("killDoyors", L("Kill Doyors"),
			new KillObjective(30, new[] { MonsterId.Doyor }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FHuevillage583Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1005);
		SetName(L("The Upent Lord"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Caros to draw out the territory-defending Upent Lord."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Egle"), "f_huevillage_58_3");

		AddObjective("killCaros", L("Kill Caros"),
			new KillObjective(10, new[] { MonsterId.Caro }));

		AddObjective("killLord", L("Defeat the Upent Lord"),
			new KillObjective(1, new[] { MonsterId.Upent }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}

public class FHuevillage583Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_3", 1006);
		SetName(L("Cobalt Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Caros, Tipios, Doyors."));
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Guard] Mindaugas"), "f_huevillage_58_3");

		AddObjective("killCaros", L("Kill Caros"),
			new KillObjective(12, new[] { MonsterId.Caro }));

		AddObjective("killTipios", L("Kill Tipios"),
			new KillObjective(12, new[] { MonsterId.Tipio }));

		AddObjective("killDoyors", L("Kill Doyors"),
			new KillObjective(12, new[] { MonsterId.Doyor }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(14200));
		AddReward(new ItemReward(640085, 3));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}
