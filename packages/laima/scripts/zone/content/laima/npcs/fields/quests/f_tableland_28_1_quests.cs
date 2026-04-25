//--- Melia Script ----------------------------------------------------------
// Mesafasla Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the Mesafasla tableland, paired with Stogas.
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

public class FTableland281QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Bunny Overrun
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Plateau-Warden] Stogas"), "f_tableland_28_1", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1001);

			dialog.SetTitle(L("Stogas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Repusbunnies strip the plateau grass to bare rock. Kill forty before the tableland starves."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Grass?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill forty Green Repusbunnies?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty. The warren's east of the crag."));
						}
						break;

					case "info":
						await dialog.Msg(L("Thin soil up here. Once the grass goes, the wind takes the dirt."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau bleeds."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Herd's trimmed. Grass gets a season."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Seedlings sprouting on the east ridge already."));
			}
		});

		// Quest 2: Bow-Bunny Fletchings
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Fletcher] Vaidile"), "f_tableland_28_1", 800, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1002);

			dialog.SetTitle(L("Vaidile"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bow-Repusbunnies carry reed-fletchings the wind-hunters envy. Kill twenty, bring eight fletchings."));

				var response = await dialog.Select(L("Fletchings?"),
					Option(L("I'll bring"), "help"),
					Option(L("Reed?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty Green Bow-Repusbunnies and bring eight fletchings?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Don't crease them."));
						}
						break;

					case "info":
						await dialog.Msg(L("Plateau-reed fletches straighter than anything lowland. Wind shapes it."));
						break;

					case "leave":
						await dialog.Msg(L("Arrows stay crooked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFletchings", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Eight good ones. Quiver's worth a week."));
					character.Inventory.Remove(650266, character.Inventory.CountItem(650266), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First quiver flies true. Crag-hunters owe me."));
			}
		});

		// Quest 3: Mage Ember-Dust
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Alchemist] Rutenis"), "f_tableland_28_1", -500, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1003);

			dialog.SetTitle(L("Rutenis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Red Saltisdaughter Mages burn ember-dust in their palms. Kill sixteen, bring six dust pinches."));

				var response = await dialog.Select(L("Dust?"),
					Option(L("I'll bring"), "help"),
					Option(L("Ember?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill sixteen Red Saltisdaughter Mages and bring six ember-dust pinches?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wax-paper packets. Don't breathe it."));
						}
						break;

					case "info":
						await dialog.Msg(L("Ember-dust holds heat weeks after the mage dies. Lamp-oil of the plateau."));
						break;

					case "leave":
						await dialog.Msg(L("Lamps stay cold."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherDust", out var dObj)) return;

				if (killObj.Done && dObj.Done)
				{
					await dialog.Msg(L("Six pinches. Lamps for a month."));
					character.Inventory.Remove(650267, character.Inventory.CountItem(650267), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Whole plateau lit tonight. First time in years."));
			}
		});

		// Quest 4: Rootcrystal Field
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Crystal-Cutter] Silute"), "f_tableland_28_1", -1100, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1004);

			dialog.SetTitle(L("Silute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tableland Rootcrystals split the bedrock. Break fourteen before they crack the bluff."));

				var response = await dialog.Select(L("Break?"),
					Option(L("I'll break"), "help"),
					Option(L("Bluff?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Break fourteen Rootcrystals?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Fourteen. Mind the spray."));
						}
						break;

					case "info":
						await dialog.Msg(L("Crack propagates through the shelf. One bad season, the edge drops a hundred feet."));
						break;

					case "leave":
						await dialog.Msg(L("Bluff crumbles."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Shelf holds another year."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Surveyors walked the bluff. Stable."));
			}
		});

		// Quest 5: The Warren-Matriarch
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Gediminas"), "f_tableland_28_1", 1800, -300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1005);
			var matriarchSpawnedKey = "Laima.Quests.f_tableland_28_1.Quest1005.MatriarchSpawned";

			dialog.SetTitle(L("Gediminas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Bow-Repusbunny Matriarch drives the warren. Kill ten archers to flush her from the deep burrow."));

				var response = await dialog.Select(L("Matriarch?"),
					Option(L("I'll face her"), "help"),
					Option(L("Burrow?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Green Bow-Repusbunnies and defeat the Matriarch when she emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten. Then she comes up shooting."));
						}
						break;

					case "info":
						await dialog.Msg(L("Deep burrow under the west crag. She won't leave unless the scouts stop coming home."));
						break;

					case "leave":
						await dialog.Msg(L("Warren grows."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killScouts", out var pObj)) return;
				if (!quest.TryGetProgress("killMatriarch", out var aObj)) return;

				if (pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("Warren's scattered."));
					character.Variables.Perm.Remove(matriarchSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(matriarchSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(matriarchSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Repusbunny_Bow_Green, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She comes!"));
							character.ServerMessage(L("{#FF9966}The Warren-Matriarch bursts from the deep burrow!{/}"));
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
				await dialog.Msg(L("Burrows collapsed. Plateau's quiet."));
			}
		});

		// Quest 6: Tableland Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Tautvydas"), "f_tableland_28_1", 1700, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1006);

			dialog.SetTitle(L("Tautvydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Plateau sweep. Twelve Repusbunnies, twelve Bow-Repusbunnies, twelve Saltisdaughter Mages."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - Green Repusbunnies, Green Bow-Repusbunnies, Red Saltisdaughter Mages?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Plateau stays wild."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var bObj)) return;
				if (!quest.TryGetProgress("killArchers", out var aObj)) return;
				if (!quest.TryGetProgress("killMages", out var mObj)) return;

				if (bObj.Done && aObj.Done && mObj.Done)
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
				await dialog.Msg(L("Militia camps the crag now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland281Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1001);
		SetName(L("Bunny Overrun"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Repusbunnies stripping the plateau grass."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Plateau-Warden] Stogas"), "f_tableland_28_1");

		AddObjective("killBunnies", L("Kill Green Repusbunnies"),
			new KillObjective(40, new[] { MonsterId.Repusbunny_Green }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FTableland281Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1002);
		SetName(L("Reed-Fletchings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Bow-Repusbunnies and bring reed-fletchings for the wind-hunters."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Vaidile"), "f_tableland_28_1");

		AddObjective("killArchers", L("Kill Green Bow-Repusbunnies"),
			new KillObjective(20, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddObjective("gatherFletchings", L("Gather reed-fletchings"),
			new CollectItemObjective(650266, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650266, character.Inventory.CountItem(650266), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650266, character.Inventory.CountItem(650266), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland281Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1003);
		SetName(L("Ember-Dust Pinches"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Saltisdaughter Mages and bring ember-dust for the plateau lamps."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Rutenis"), "f_tableland_28_1");

		AddObjective("killMages", L("Kill Red Saltisdaughter Mages"),
			new KillObjective(16, new[] { MonsterId.Saltisdaughter_Mage_Red }));

		AddObjective("gatherDust", L("Gather ember-dust pinches"),
			new CollectItemObjective(650267, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 13));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650267, character.Inventory.CountItem(650267), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650267, character.Inventory.CountItem(650267), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland281Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1004);
		SetName(L("Rootcrystal Field"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Rootcrystals splitting the tableland bedrock."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Cutter] Silute"), "f_tableland_28_1");

		AddObjective("breakCrystals", L("Break Rootcrystals"),
			new KillObjective(14, new[] { MonsterId.Rootcrystal_03 }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FTableland281Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1005);
		SetName(L("The Warren-Matriarch"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bow-Repusbunnies to flush the Warren-Matriarch from the deep burrow."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Gediminas"), "f_tableland_28_1");

		AddObjective("killScouts", L("Kill Green Bow-Repusbunnies"),
			new KillObjective(10, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddObjective("killMatriarch", L("Defeat the Warren-Matriarch"),
			new KillObjective(1, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}
}

public class FTableland281Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1006);
		SetName(L("Tableland Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Repusbunnies, Bow-Repusbunnies, and Saltisdaughter Mages."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Tautvydas"), "f_tableland_28_1");

		AddObjective("killBunnies", L("Kill Green Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny_Green }));

		AddObjective("killArchers", L("Kill Green Bow-Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddObjective("killMages", L("Kill Red Saltisdaughter Mages"),
			new KillObjective(12, new[] { MonsterId.Saltisdaughter_Mage_Red }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(75000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 13));
	}
}
