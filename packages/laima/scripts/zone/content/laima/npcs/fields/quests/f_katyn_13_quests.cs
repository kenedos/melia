//--- Melia Script ----------------------------------------------------------
// Poslinkis Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Katyn Forest (Poslinkis, high bube warband).
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

public class FKatyn13QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Spearline Kill
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Border-Ward] Mindaugas"), "f_katyn_13", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("High Bube spears push the treeline. Kill forty, the warband backs off a week."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Warband?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill forty High Bube Spears?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty. Watch the hedge-flanks."));
						}
						break;

					case "info":
						await dialog.Msg(L("Poslinkis tribe. Spear-caste forward, arrow-caste behind. Same camp."));
						break;

					case "leave":
						await dialog.Msg(L("Treeline keeps pushing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpears", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Treeline holds."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Warband's limping. A week, maybe ten days."));
			}
		});

		// Quest 2: Arrow-Caste Fletching
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Forester] Ruta"), "f_katyn_13", 800, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("High Bube Archers fletch with a pitch we can't match. Kill thirty, bring eight fletchings."));

				var response = await dialog.Select(L("Fletchings?"),
					Option(L("I'll bring"), "help"),
					Option(L("Pitch?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty High Bube Archers and bring eight fletchings?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Pitch side up. Don't smudge."));
						}
						break;

					case "info":
						await dialog.Msg(L("Resin-and-ash. Holds the feather through rain. Our foresters want the recipe."));
						break;

					case "leave":
						await dialog.Msg(L("Recipe stays theirs."));
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
					await dialog.Msg(L("Eight fletchings. Pitch sample to the stillroom."));
					character.Inventory.Remove(650239, character.Inventory.CountItem(650239), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Stillroom cracked the pitch. Our arrows hold now."));
			}
		});

		// Quest 3: Pokubu Green-Tongues
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Herbwife] Aldona-IV"), "f_katyn_13", -500, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1003);

			dialog.SetTitle(L("Aldona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Arburn Pokubu Greens chew a root that swells their tongues green. Kill twenty-five, bring six tongues."));

				var response = await dialog.Select(L("Tongues?"),
					Option(L("I'll bring"), "help"),
					Option(L("Root?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-five Arburn Pokubu Greens and bring six tongues?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wrap in oiled cloth. Tongues dry to dust fast."));
						}
						break;

					case "info":
						await dialog.Msg(L("Root's a swamp-ivy, poison to us but the pokubu thrives. Tongue tells me the dose."));
						break;

					case "leave":
						await dialog.Msg(L("Dose stays guess."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPokubu", out var killObj)) return;
				if (!quest.TryGetProgress("gatherTongues", out var tObj)) return;

				if (killObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Six tongues. I'll have a salve by week-end."));
					character.Inventory.Remove(650241, character.Inventory.CountItem(650241), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salve cures a swamp-ivy sting in an hour."));
			}
		});

		// Quest 4: Crystal Line
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Crystal-Warder] Jurga"), "f_katyn_13", -1100, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1004);

			dialog.SetTitle(L("Jurga"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Poslinkis Rootcrystals line the old border. Break fifteen, they won't hum a warning to the warband."));

				var response = await dialog.Select(L("Break?"),
					Option(L("I'll break"), "help"),
					Option(L("Warning?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Break fifteen Poslinkis Rootcrystals?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Fifteen. Work quiet."));
						}
						break;

					case "info":
						await dialog.Msg(L("Warband planted them as a tripwire. Crystal hums, scouts come running."));
						break;

					case "leave":
						await dialog.Msg(L("Tripwire hums on."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Line's dead."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Scouts don't come running anymore."));
			}
		});

		// Quest 5: The Fallen Statue
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Tadas"), "f_katyn_13", 1800, -300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1005);
			var bossSpawnedKey = "Laima.Quests.f_katyn_13.Quest1005.BossSpawned";

			dialog.SetTitle(L("Tadas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Fallen Statue woke in the old shrine. Kill ten Bushspiders guarding the approach, then end it."));

				var response = await dialog.Select(L("Statue?"),
					Option(L("I'll face it"), "help"),
					Option(L("Woke?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Bushspiders and defeat the Fallen Statue when it rises?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("Shrine was Poslinkis before the tribe. Something in the stone remembers."));
						break;

					case "leave":
						await dialog.Msg(L("Stone keeps waking."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpiders", out var pObj)) return;
				if (!quest.TryGetProgress("killStatue", out var sObj)) return;

				if (pObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Shrine's still."));
					character.Inventory.Remove(650243, character.Inventory.CountItem(650243), InventoryItemRemoveMsg.Given);
					character.Variables.Perm.Remove(bossSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !sObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(bossSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(bossSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Boss_Fallen_Statue, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("It rises!"));
							character.ServerMessage(L("{#FF9966}The Fallen Statue rises from the shrine!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Find it."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten first. Bring back a shrine-fragment."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Shrine's quiet. Stone forgot again."));
			}
		});

		// Quest 6: Poslinkis Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Militia-Captain] Kestutis"), "f_katyn_13", 1700, 1000, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_13", 1006);

			dialog.SetTitle(L("Kestutis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Forest sweep. Twelve Spears, twelve Archers, twelve Pokubu Greens."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - High Bube Spears, High Bube Archers, Arburn Pokubu Greens?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Forest stays loud."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSpears", out var spObj)) return;
				if (!quest.TryGetProgress("killArchers", out var arObj)) return;
				if (!quest.TryGetProgress("killPokubu", out var poObj)) return;

				if (spObj.Done && arObj.Done && poObj.Done)
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
				await dialog.Msg(L("Militia patrols the treeline now."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FKatyn13Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1001);
		SetName(L("Spearline Kill"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill High Bube Spears pushing the Katyn treeline."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Border-Ward] Mindaugas"), "f_katyn_13");

		AddObjective("killSpears", L("Kill High Bube Spears"),
			new KillObjective(40, new[] { MonsterId.HighBube_Spear }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
	}
}

public class FKatyn13Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1002);
		SetName(L("Arrow-Caste Fletchings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill High Bube Archers and bring pitch-fletchings for the stillroom."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forester] Ruta"), "f_katyn_13");

		AddObjective("killArchers", L("Kill High Bube Archers"),
			new KillObjective(30, new[] { MonsterId.HighBube_Archer }));

		AddObjective("gatherFletchings", L("Gather pitch-fletchings"),
			new CollectItemObjective(650239, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650239, character.Inventory.CountItem(650239), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650239, character.Inventory.CountItem(650239), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn13Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1003);
		SetName(L("Pokubu Green-Tongues"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Arburn Pokubu Greens and bring green tongues for the herbwife."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbwife] Aldona"), "f_katyn_13");

		AddObjective("killPokubu", L("Kill Arburn Pokubu Greens"),
			new KillObjective(25, new[] { MonsterId.Arburn_Pokubu_Green }));

		AddObjective("gatherTongues", L("Gather green tongues"),
			new CollectItemObjective(650241, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650241, character.Inventory.CountItem(650241), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650241, character.Inventory.CountItem(650241), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn13Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1004);
		SetName(L("Crystal Tripwire"));
		SetType(QuestType.Sub);
		SetDescription(L("Break Poslinkis Rootcrystals to silence the warband's tripwire line."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Warder] Jurga"), "f_katyn_13");

		AddObjective("breakCrystals", L("Break Poslinkis Rootcrystals"),
			new KillObjective(15, new[] { MonsterId.Rootcrystal_02 }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
	}
}

public class FKatyn13Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1005);
		SetName(L("The Fallen Statue"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bushspiders at the shrine approach and bring a shrine-fragment, then defeat the Fallen Statue when it rises."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Tadas"), "f_katyn_13");

		AddObjective("killSpiders", L("Kill Bushspiders and gather a shrine-fragment"),
			new KillObjective(10, new[] { MonsterId.Bushspider }));

		AddObjective("killStatue", L("Defeat the Fallen Statue"),
			new KillObjective(1, new[] { MonsterId.Boss_Fallen_Statue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650243, character.Inventory.CountItem(650243), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650243, character.Inventory.CountItem(650243), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn13Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_13", 1006);
		SetName(L("Poslinkis Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of High Bube Spears, High Bube Archers, and Arburn Pokubu Greens."));
		SetLocation("f_katyn_13");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Kestutis"), "f_katyn_13");

		AddObjective("killSpears", L("Kill High Bube Spears"),
			new KillObjective(12, new[] { MonsterId.HighBube_Spear }));

		AddObjective("killArchers", L("Kill High Bube Archers"),
			new KillObjective(12, new[] { MonsterId.HighBube_Archer }));

		AddObjective("killPokubu", L("Kill Arburn Pokubu Greens"),
			new KillObjective(12, new[] { MonsterId.Arburn_Pokubu_Green }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}
}
