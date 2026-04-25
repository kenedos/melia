//--- Melia Script ----------------------------------------------------------
// Vedas Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Vedas Plateau.
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

public class FTableland111QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Saltisdaughter Raid
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Plateau-Ward] Linas"), "f_tableland_11_1", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1001);

			dialog.SetTitle(L("Linas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Saltisdaughters are raiding the plateau. Their brand-rituals leave the stone salt-scarred."));
				await dialog.Msg(L("Kill twenty-two Red Saltisdaughters and the raids stop for a month."));

				var response = await dialog.Select(L("Raid stop?"),
					Option(L("I'll kill them"), "help"),
					Option(L("Brand-rituals?"), "info"),
					Option(L("Let them raid"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-two Red Saltisdaughters?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twenty-two. They hunt in threes."));
						}
						break;

					case "info":
						await dialog.Msg(L("Same cabal as the flash-cities. Different brand, same cult."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau dies if unchallenged."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSaltis", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Raids quiet. Plateau breathes."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("A month of peace. Strange."));
			}
		});

		// Quest 2: Green-Bow Archers
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Scout-Captain] Aiste"), "f_tableland_11_1", 800, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1002);

			dialog.SetTitle(L("Aiste"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Saltisdaughter Bows pick off my scouts at range. Kill fifteen and bring six of their brand-arrows for study."));

				var response = await dialog.Select(L("Brand-arrows?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Why the brand?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Green Saltisdaughter Bows and bring six brand-arrows?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Arrowheads only. Leave the shafts."));
						}
						break;

					case "info":
						await dialog.Msg(L("Curse-brand. If the arrow grazes, the curse spreads. We need the pattern for counterward."));
						break;

					case "leave":
						await dialog.Msg(L("Three scouts lost this month."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBows", out var killObj)) return;
				if (!quest.TryGetProgress("gatherArrows", out var aObj)) return;

				if (killObj.Done && aObj.Done)
				{
					await dialog.Msg(L("Six arrows. Counterward begins tonight."));
					character.Inventory.Remove(650052, character.Inventory.CountItem(650052), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Counterward works. No scouts lost this week."));
			}
		});

		// Quest 3: Repusbunny Mages
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Warder] Dovile"), "f_tableland_11_1", -600, -1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1003);

			dialog.SetTitle(L("Dovile"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Repusbunny Mages cast wards around their warrens. Kill fifteen and bring five ward-stones for analysis."));

				var response = await dialog.Select(L("Ward-stones?"),
					Option(L("I'll bring them"), "help"),
					Option(L("What wards?"), "info"),
					Option(L("Leave them"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Green Repusbunny Mages and bring five ward-stones?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Green-glowing. Don't touch the core."));
						}
						break;

					case "info":
						await dialog.Msg(L("They ward their own. My warders need the pattern."));
						break;

					case "leave":
						await dialog.Msg(L("Warrens spread underground."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherStones", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Five stones. Pattern's decoded by morning."));
					character.Inventory.Remove(650065, character.Inventory.CountItem(650065), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Our wards match theirs now. Balance restored."));
			}
		});

		// Quest 4: White Groll Kill
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Hunter] Povilas"), "f_tableland_11_1", 300, -1200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1004);

			dialog.SetTitle(L("Povilas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("White Grolls flock on the south plateau. Their tusks fetch fine coin - and they gore cattle if uncountered."));
				await dialog.Msg(L("Kill thirty White Grolls and bring eight tusks."));

				var response = await dialog.Select(L("Tusks?"),
					Option(L("I'll kill"), "help"),
					Option(L("Worth it?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty White Grolls and bring eight tusks?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Pull the tusks clean. Snapped tusks sell for half."));
						}
						break;

					case "info":
						await dialog.Msg(L("Klaipeda's carvers pay gold per pair. Always worth it."));
						break;

					case "leave":
						await dialog.Msg(L("Cattle pay the bill you won't."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGrolls", out var killObj)) return;
				if (!quest.TryGetProgress("gatherTusks", out var tObj)) return;

				if (killObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Eight clean tusks. Carvers will fight over them."));
					character.Inventory.Remove(650068, character.Inventory.CountItem(650068), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Tusks shipped. Cattle safe."));
			}
		});

		// Quest 5: Saltis-Champion
		//-------------------------------------------------------------------------
		AddNpc(20011, L("[Bounty Captain] Girtas"), "f_tableland_11_1", 900, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1005);
			var championSpawnedKey = "Laima.Quests.f_tableland_11_1.Quest1005.ChampionSpawned";

			dialog.SetTitle(L("Girtas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Saltisdaughter Champion leads the raid band. Kill her shield-sisters and she'll demand the duel."));

				var response = await dialog.Select(L("Duel?"),
					Option(L("I accept"), "help"),
					Option(L("Why duel?"), "info"),
					Option(L("Decline"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Red Saltisdaughters and defeat the Champion when she answers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten. She comes fast after."));
						}
						break;

					case "info":
						await dialog.Msg(L("Honor code. Kill her sisters, she has to answer."));
						break;

					case "leave":
						await dialog.Msg(L("Raid band without her breaks."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSisters", out var sObj)) return;
				if (!quest.TryGetProgress("killChampion", out var cObj)) return;

				if (sObj.Done && cObj.Done)
				{
					await dialog.Msg(L("Champion's down. Band's scattered."));
					character.Variables.Perm.Remove(championSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (sObj.Done && !cObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(championSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(championSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Saltisdaughter_Red, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She answers the duel!"));
							character.ServerMessage(L("{#FF9966}The Saltisdaughter Champion answers the challenge!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("She's on the plateau. Finish it."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten shield-sisters first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Raid band's done. Plateau's safe."));
			}
		});

		// Quest 6: Vedas Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Guard] Mindaugas"), "f_tableland_11_1", -900, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_11_1", 1006);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Standard plateau sweep. Twelve Saltis, twelve Grolls."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Red Saltisdaughters and twelve White Grolls?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Standard bounty."));
						}
						break;

					case "info":
						await dialog.Msg(L("Bounty plus bonus."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan doesn't care."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSaltis", out var sObj)) return;
				if (!quest.TryGetProgress("killGrolls", out var gObj)) return;

				if (sObj.Done && gObj.Done)
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

public class FTableland111Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1001);
		SetName(L("Saltisdaughter Raid"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Saltisdaughters raiding Vedas Plateau."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Plateau-Ward] Linas"), "f_tableland_11_1");

		AddObjective("killSaltis", L("Kill Red Saltisdaughters"),
			new KillObjective(22, new[] { MonsterId.Saltisdaughter_Red }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FTableland111Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1002);
		SetName(L("Brand-Arrow Counterward"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Saltisdaughter Bows and bring brand-arrows for counterward research."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout-Captain] Aiste"), "f_tableland_11_1");

		AddObjective("killBows", L("Kill Green Saltisdaughter Bows"),
			new KillObjective(15, new[] { MonsterId.Saltisdaughter_Bow_Green }));

		AddObjective("gatherArrows", L("Gather brand-arrows"),
			new CollectItemObjective(650052, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650052, character.Inventory.CountItem(650052), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650052, character.Inventory.CountItem(650052), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland111Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1003);
		SetName(L("Repusbunny Ward-Stones"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Repusbunny Mages and gather ward-stones for pattern study."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Warder] Dovile"), "f_tableland_11_1");

		AddObjective("killMages", L("Kill Green Repusbunny Mages"),
			new KillObjective(15, new[] { MonsterId.Repusbunny_Mage_Green }));

		AddObjective("gatherStones", L("Gather ward-stones"),
			new CollectItemObjective(650065, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 13));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650065, character.Inventory.CountItem(650065), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650065, character.Inventory.CountItem(650065), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland111Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1004);
		SetName(L("White Groll Tusks"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill White Grolls and bring tusks for Klaipeda carvers."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hunter] Povilas"), "f_tableland_11_1");

		AddObjective("killGrolls", L("Kill White Grolls"),
			new KillObjective(30, new[] { MonsterId.Groll_White }));

		AddObjective("gatherTusks", L("Gather tusks"),
			new CollectItemObjective(650068, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650068, character.Inventory.CountItem(650068), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650068, character.Inventory.CountItem(650068), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland111Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1005);
		SetName(L("The Saltis Champion"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill shield-sisters to draw out the Saltisdaughter Champion."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Captain] Girtas"), "f_tableland_11_1");

		AddObjective("killSisters", L("Kill shield-sisters"),
			new KillObjective(10, new[] { MonsterId.Saltisdaughter_Red }));

		AddObjective("killChampion", L("Defeat the Champion"),
			new KillObjective(1, new[] { MonsterId.Saltisdaughter_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}
}

public class FTableland111Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_11_1", 1006);
		SetName(L("Vedas Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Red Saltisdaughters and White Grolls."));
		SetLocation("f_tableland_11_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Guard] Mindaugas"), "f_tableland_11_1");

		AddObjective("killSaltis", L("Kill Red Saltisdaughters"),
			new KillObjective(12, new[] { MonsterId.Saltisdaughter_Red }));

		AddObjective("killGrolls", L("Kill White Grolls"),
			new KillObjective(12, new[] { MonsterId.Groll_White }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 13));
	}
}
