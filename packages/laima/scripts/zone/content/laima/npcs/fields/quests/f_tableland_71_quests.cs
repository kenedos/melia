//--- Melia Script ----------------------------------------------------------
// Grand Yard Mesa Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Grand Yard Mesa (plateau extension beyond Ibre).
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

public class FTableland71QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Purple Ritter Push
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Mesa-Warden] Aistis"), "f_tableland_71", 200, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1001);

			dialog.SetTitle(L("Aistis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Purple Hohen Ritter hold the mesa flats past Ibre. Kill forty and the survey teams can stake rope."));

				var response = await dialog.Select(L("Kill?"),
					Option(L("I'll kill"), "help"),
					Option(L("Survey?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill forty Purple Hohen Ritter?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Forty. The flats are wide."));
						}
						break;

					case "info":
						await dialog.Msg(L("Mesa extension maps past the last Ibre marker. No stakes means no road."));
						break;

					case "leave":
						await dialog.Msg(L("Mesa stays unmapped."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRitter", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Surveyors riding out at dawn."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First rope-line staked yesterday."));
			}
		});

		// Quest 2: Cronewt Arrow-Fletch
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Fletcher] Rasa"), "f_tableland_71", 600, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1002);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Cronewt Bows shoot clean fletchings. Kill twenty-five, bring eight intact fletches for mesa patrols."));

				var response = await dialog.Select(L("Fletches?"),
					Option(L("I'll bring"), "help"),
					Option(L("Patrols?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-five Blue Cronewt Bows and bring eight fletches?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Keep the vanes flat."));
						}
						break;

					case "info":
						await dialog.Msg(L("Patrols need reach against the Tinys. Cronewt fletches carry."));
						break;

					case "leave":
						await dialog.Msg(L("Patrols stay short-range."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCronewts", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFletches", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Eight fletches. Patrols get reach."));
					character.Inventory.Remove(650271, character.Inventory.CountItem(650271), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Patrols cleared two ridges this week."));
			}
		});

		// Quest 3: Barkle Bark-Salves
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Herbalist] Milda"), "f_tableland_71", -400, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1003);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Hohen Barkle bark holds salve-resin. Kill twenty, bring six resin-pulls for the field kit."));

				var response = await dialog.Select(L("Resin?"),
					Option(L("I'll bring"), "help"),
					Option(L("Field kit?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty Blue Hohen Barkle and bring six resin-pulls?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Pull slow. The bark tears."));
						}
						break;

					case "info":
						await dialog.Msg(L("Surveyors sleep rough. Salve closes mesa-wind cuts overnight."));
						break;

					case "leave":
						await dialog.Msg(L("Cuts stay open."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBarkles", out var killObj)) return;
				if (!quest.TryGetProgress("gatherResin", out var rObj)) return;

				if (killObj.Done && rObj.Done)
				{
					await dialog.Msg(L("Six pulls. Kit's full."));
					character.Inventory.Remove(650272, character.Inventory.CountItem(650272), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Salve holds three days on a mesa-cut."));
			}
		});

		// Quest 4: Tiny Tide
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Ridgeman] Jogaila"), "f_tableland_71", -1000, -200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1004);

			dialog.SetTitle(L("Jogaila"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Blue Tinys swarm the mesa ridges. Kill sixty to thin the tide."));

				var response = await dialog.Select(L("Tide?"),
					Option(L("I'll kill"), "help"),
					Option(L("Sixty?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill sixty Blue Tinys across the ridges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Sixty. Watch the footing."));
						}
						break;

					case "info":
						await dialog.Msg(L("They come in waves. Sixty breaks a wave."));
						break;

					case "leave":
						await dialog.Msg(L("Ridges stay crowded."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTinys", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Ridges breathe."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep killing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Next wave's a week out."));
			}
		});

		// Quest 5: The Crystal-Warden Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Gediminas"), "f_tableland_71", 1400, 500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_tableland_71.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Gediminas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Ritter Alpha wards the mesa's rootcrystal vein. Kill ten to draw him off the stone."));

				var response = await dialog.Select(L("Alpha?"),
					Option(L("I'll face him"), "help"),
					Option(L("Vein?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Purple Hohen Ritter and defeat the Alpha when he emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten."));
						}
						break;

					case "info":
						await dialog.Msg(L("Vein feeds half the mesa's crystal pull. Warden drops, the vein opens."));
						break;

					case "leave":
						await dialog.Msg(L("Vein stays warded."));
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
					await dialog.Msg(L("Vein's open."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Hohen_Ritter_Purple, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes!"));
							character.ServerMessage(L("{#FF9966}The Ritter Alpha emerges from the vein!{/}"));
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
				await dialog.Msg(L("Crystal pulls are cleaner. Miners moving in."));
			}
		});

		// Quest 6: Mesa Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Survey-Captain] Vilma"), "f_tableland_71", 1300, 800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1006);

			dialog.SetTitle(L("Vilma"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Standard mesa sweep. Twelve Ritter, twelve Barkle, twelve Cronewts."));

				var response = await dialog.Select(L("Sweep?"),
					Option(L("I'll do it"), "help"),
					Option(L("Pay?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each - Purple Ritter, Blue Barkle, Blue Cronewt Bows?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six."));
						}
						break;

					case "info":
						await dialog.Msg(L("Fair."));
						break;

					case "leave":
						await dialog.Msg(L("Mesa stays open-range."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRitter", out var rObj)) return;
				if (!quest.TryGetProgress("killBarkles", out var bObj)) return;
				if (!quest.TryGetProgress("killCronewts", out var cObj)) return;

				if (rObj.Done && bObj.Done && cObj.Done)
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
				await dialog.Msg(L("Survey stakes running past the next ridge."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FTableland71Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1001);
		SetName(L("Purple Ritter Push"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Hohen Ritter holding the mesa flats past Ibre."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mesa-Warden] Aistis"), "f_tableland_71");

		AddObjective("killRitter", L("Kill Purple Hohen Ritter"),
			new KillObjective(40, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FTableland71Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1002);
		SetName(L("Cronewt Arrow-Fletch"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Cronewt Bows and bring intact fletches for mesa patrols."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher] Rasa"), "f_tableland_71");

		AddObjective("killCronewts", L("Kill Blue Cronewt Bows"),
			new KillObjective(25, new[] { MonsterId.Cronewt_Bow_Blue }));

		AddObjective("gatherFletches", L("Gather intact fletches"),
			new CollectItemObjective(650271, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650271, character.Inventory.CountItem(650271), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650271, character.Inventory.CountItem(650271), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland71Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1003);
		SetName(L("Barkle Bark-Salves"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Hohen Barkle and bring resin-pulls for the field kit."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Milda"), "f_tableland_71");

		AddObjective("killBarkles", L("Kill Blue Hohen Barkle"),
			new KillObjective(20, new[] { MonsterId.Hohen_Barkle_Blue }));

		AddObjective("gatherResin", L("Gather resin-pulls"),
			new CollectItemObjective(650272, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 13));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650272, character.Inventory.CountItem(650272), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650272, character.Inventory.CountItem(650272), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FTableland71Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1004);
		SetName(L("Tiny Tide"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Blue Tiny swarm along the mesa ridges."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ridgeman] Jogaila"), "f_tableland_71");

		AddObjective("killTinys", L("Kill Blue Tinys"),
			new KillObjective(60, new[] { MonsterId.Tiny_Blue }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
	}
}

public class FTableland71Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1005);
		SetName(L("The Ritter Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Hohen Ritter to draw out the Alpha warding the rootcrystal vein."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Gediminas"), "f_tableland_71");

		AddObjective("killPack", L("Kill Purple Hohen Ritter"),
			new KillObjective(10, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddObjective("killAlpha", L("Defeat the Ritter Alpha"),
			new KillObjective(1, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 15));
		AddReward(new ItemReward(640007, 15));
		AddReward(new ItemReward(640013, 14));
	}
}

public class FTableland71Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1006);
		SetName(L("Mesa Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Purple Ritter, Blue Barkle, and Blue Cronewt Bows."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Survey-Captain] Vilma"), "f_tableland_71");

		AddObjective("killRitter", L("Kill Purple Hohen Ritter"),
			new KillObjective(12, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddObjective("killBarkles", L("Kill Blue Hohen Barkle"),
			new KillObjective(12, new[] { MonsterId.Hohen_Barkle_Blue }));

		AddObjective("killCronewts", L("Kill Blue Cronewt Bows"),
			new KillObjective(12, new[] { MonsterId.Cronewt_Bow_Blue }));

		AddReward(new ExpReward(26400, 18000));
		AddReward(new SilverReward(75000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 13));
	}
}
