//--- Melia Script ----------------------------------------------------------
// Overlong Bridge Valley Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Overlong Bridge Valley.
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

public class FRokas26QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: The Bridge Wendigos
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Bridge-Ward] Orri"), "f_rokas_26", 1800, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1001);

			dialog.SetTitle(L("Orri"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Wendigos roam the bridge at night. The big ones howl until the timbers shake loose."));
				await dialog.Msg(L("Kill twenty-two Wendigos and the bridge quiets. My repair crew can't work until then."));

				var response = await dialog.Select(L("Clear the bridge?"),
					Option(L("I'll kill them"), "help"),
					Option(L("Why the howling?"), "info"),
					Option(L("Send the army"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two. They hunt in fours, mind your back."));
						break;

					case "info":
						await dialog.Msg(L("Territory. They see every crossing as a challenge. The howl warns the next pack."));
						break;

					case "leave":
						await dialog.Msg(L("Army's at Orsha. Bridge rots waiting."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killWendigos", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Quiet. Crew's already hammering."));
					await dialog.Msg(L("Take your pay, bridge-walker."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still howling. Keep going."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three new beams in. Bridge holds another decade."));
			}
		});

		// Quest 2: Dumaro Swarm
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Scout] Brynhild"), "f_rokas_26", 100, -400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1002);

			dialog.SetTitle(L("Brynhild"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Dumaros are breeding faster than we can count. They swarm my scout trails."));
				await dialog.Msg(L("Kill forty of them and bring five Dumaro skulls for the kill-count bounty."));

				var response = await dialog.Select(L("Forty Dumaros?"),
					Option(L("I'll swarm back"), "help"),
					Option(L("Why the skulls?"), "info"),
					Option(L("Let them breed"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Forty and five. The skulls count for the ledger."));
						break;

					case "info":
						await dialog.Msg(L("Bounty office wants verification. Skulls are it."));
						break;

					case "leave":
						await dialog.Msg(L("The valley chokes on Dumaros within the month if we don't."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDumaros", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSkulls", out var skullObj)) return;

				if (killObj.Done && skullObj.Done)
				{
					await dialog.Msg(L("Forty down, five skulls. Ledger's happy."));

					character.Inventory.Remove(650012, character.Inventory.CountItem(650012), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep pressing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Scout trails are walkable again."));
			}
		});

		// Quest 3: Wendigo Archers
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Fletcher-Apprentice] Sindri"), "f_rokas_26", -500, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1003);

			dialog.SetTitle(L("Sindri"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Wendigo Bows shoot poisoned arrows. My master wants six of their quivers to study the venom-cure."));
				await dialog.Msg(L("Kill twelve Wendigo Bows and bring back their quivers."));

				var response = await dialog.Select(L("Six quivers?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Venom-cure?"), "info"),
					Option(L("Dangerous work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Handle the quivers by the strap, not the leather - venom seeps."));
						break;

					case "info":
						await dialog.Msg(L("Two scouts fell last month. Cure or we lose more."));
						break;

					case "leave":
						await dialog.Msg(L("Only if you step into the arrow line."));
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
					await dialog.Msg(L("Six quivers. Master will have the cure brewed by week's end."));

					character.Inventory.Remove(650014, character.Inventory.CountItem(650014), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep collecting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cure works. Three scouts back on duty."));
			}
		});

		// Quest 4: Wendigo Mage Cabal
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Warlock-Investigator] Harald"), "f_rokas_26", -1000, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1004);

			dialog.SetTitle(L("Harald"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Wendigo Mages are casting something new. Every dusk, the valley fog thickens."));
				await dialog.Msg(L("Kill fifteen Wendigo Mages and bring five fog-knots from their ritual circles."));

				var response = await dialog.Select(L("Fifteen Mages?"),
					Option(L("I'll break the ritual"), "help"),
					Option(L("What fog?"), "info"),
					Option(L("Walk through it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fog-knots glow faint green. Look for them near chanting circles."));
						break;

					case "info":
						await dialog.Msg(L("Breath-eating fog. Pilgrims collapsed last week walking through the fourth hour."));
						break;

					case "leave":
						await dialog.Msg(L("Not for long."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;
				if (!quest.TryGetProgress("gatherKnots", out var knotObj)) return;

				if (killObj.Done && knotObj.Done)
				{
					await dialog.Msg(L("Fog's thinning already. Ritual's broken."));

					character.Inventory.Remove(650125, character.Inventory.CountItem(650125), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Valley's clear at dusk. Pilgrims walk safe again."));
			}
		});

		// Quest 5: Wendigo Alpha
		//-------------------------------------------------------------------------
		AddNpc(147418, L("[Bounty Hunter] Ingrid"), "f_rokas_26", 900, -1100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_rokas_26.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Ingrid"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Wendigo pack-alpha leads the howling. Thin his pack and he'll come for blood."));

				var response = await dialog.Select(L("Draw him out?"),
					Option(L("I'll hunt him"), "help"),
					Option(L("Big bounty?"), "info"),
					Option(L("Leave him"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten from his pack. Then he comes."));
						break;

					case "info":
						await dialog.Msg(L("Biggest in the valley. The bounty's triple."));
						break;

					case "leave":
						await dialog.Msg(L("He'll eat three pilgrims this season if we don't."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var packObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var alphaObj)) return;

				if (packObj.Done && alphaObj.Done)
				{
					await dialog.Msg(L("Alpha's down. Pack's scattered."));
					character.Variables.Perm.Remove(alphaSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (packObj.Done && !alphaObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Wendigo, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He's here. Move!"));
							character.ServerMessage(L("{#FF9966}The Wendigo Alpha lunges from the treeline!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("Finish him before he slips the pack-bounds."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten from the pack first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("No howls at dusk. That's the bounty's real pay."));
			}
		});

		// Quest 6: Valley Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Caravan Guard] Hroar"), "f_rokas_26", 1000, -1900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_26", 1006);

			dialog.SetTitle(L("Hroar"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Caravans want the south valley thinned. Twelve Dumaros, twelve Wendigos. Simple bounty."));

				var response = await dialog.Select(L("Standard sweep?"),
					Option(L("I'll sweep"), "help"),
					Option(L("Why twelve each?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Caravans roll at dawn if the count holds."));
						break;

					case "info":
						await dialog.Msg(L("The math works. Don't ask."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan's leaving with or without."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDumaros", out var dObj)) return;
				if (!quest.TryGetProgress("killWendigos", out var wObj)) return;

				if (dObj.Done && wObj.Done)
				{
					await dialog.Msg(L("South valley's walkable. Pay's earned."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravan made it through. Another tomorrow."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FRokas26Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1001);
		SetName(L("The Bridge Wendigos"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Wendigos prowling the Overlong Bridge so repairs can resume."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bridge-Ward] Orri"), "f_rokas_26");

		AddObjective("killWendigos", L("Kill Wendigos"),
			new KillObjective(22, new[] { MonsterId.Wendigo }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(5200));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}
}

public class FRokas26Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1002);
		SetName(L("Dumaro Swarm"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Dumaros and bring skulls to verify the kill count."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout] Brynhild"), "f_rokas_26");

		AddObjective("killDumaros", L("Kill Dumaros"),
			new KillObjective(40, new[] { MonsterId.Dumaro }));

		AddObjective("gatherSkulls", L("Bring Dumaro skulls"),
			new CollectItemObjective(650012, 5));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650012, character.Inventory.CountItem(650012), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650012, character.Inventory.CountItem(650012), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRokas26Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1003);
		SetName(L("Wendigo Venom-Cure"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Wendigo Bows and bring their quivers for venom-cure research."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fletcher-Apprentice] Sindri"), "f_rokas_26");

		AddObjective("killBows", L("Kill Wendigo Bows"),
			new KillObjective(12, new[] { MonsterId.Wendigo_Bow }));

		AddObjective("gatherQuivers", L("Gather Wendigo Bow quivers"),
			new CollectItemObjective(650014, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650014, character.Inventory.CountItem(650014), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650014, character.Inventory.CountItem(650014), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRokas26Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1004);
		SetName(L("Wendigo Mage Cabal"));
		SetType(QuestType.Sub);
		SetDescription(L("Break the Wendigo Mage fog ritual by killing mages and gathering fog-knots."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Warlock-Investigator] Harald"), "f_rokas_26");

		AddObjective("killMages", L("Kill Wendigo Mages"),
			new KillObjective(15, new[] { MonsterId.Wendigo_Mage }));

		AddObjective("gatherKnots", L("Gather fog-knots"),
			new CollectItemObjective(650125, 5));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650125, character.Inventory.CountItem(650125), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650125, character.Inventory.CountItem(650125), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRokas26Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1005);
		SetName(L("The Pack Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Wendigo pack to draw out and slay the Alpha."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Ingrid"), "f_rokas_26");

		AddObjective("killPack", L("Thin the Wendigo pack"),
			new KillObjective(10, new[] { MonsterId.Wendigo }));

		AddObjective("killAlpha", L("Defeat the Wendigo Alpha"),
			new KillObjective(1, new[] { MonsterId.Wendigo }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(9000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640012, 1));
	}
}

public class FRokas26Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_26", 1006);
		SetName(L("South Valley Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin Dumaros and Wendigos in the south valley for the caravan route."));
		SetLocation("f_rokas_26");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Guard] Hroar"), "f_rokas_26");

		AddObjective("killDumaros", L("Kill Dumaros"),
			new KillObjective(12, new[] { MonsterId.Dumaro }));

		AddObjective("killWendigos", L("Kill Wendigos"),
			new KillObjective(12, new[] { MonsterId.Wendigo }));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}
