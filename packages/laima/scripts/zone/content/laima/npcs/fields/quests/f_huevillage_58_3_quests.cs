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
		AddNpc(20060, L("[Forester] Akvile"), "f_huevillage_58_3", -238, 293, 90, async dialog =>
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
		AddNpc(20114, L("[Fletcher] Ruta-II"), "f_huevillage_58_3", -1064, -705, 45, async dialog =>
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
		AddNpc(20011, L("[Scout-Captain] Norbertas"), "f_huevillage_58_3", 468, -592, 0, async dialog =>
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
		AddNpc(147473, L("[Huntress] Giedre"), "f_huevillage_58_3", 1735, -544, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1004);

			dialog.SetTitle(L("Giedre"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A huntress fits a fresh string to a worn yew-bow, testing the draw against her hip*{/}"));
				await dialog.Msg(L("Doyor packs were always east-of-Cobalt - my mother hunted them, my grandmother hunted them. They stayed in the deep woods because the deep woods fed them."));
				await dialog.Msg(L("This year they came west. Past my mother's old hide. Past the village rim. Two children went missing in the spring planting. The village won't say it out loud, but everyone knows what happened."));

				var response = await dialog.Select(L("Kill 30 Doyors to push them back east, then chalk-mark the four pack-trails I've staked with bone-cairns. Patrols use the chalk-marks to hold the line. Will you take the bow?"),
					Option(L("I'll thin the packs and chalk the trails"), "help"),
					Option(L("Why bone-cairns instead of stone?"), "info"),
					Option(L("That's the village's fight"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She presses a chalk-stub into your palm and closes your fingers around it*{/}"));
						await dialog.Msg(L("Thirty Doyors, no shortcuts. Pack of six is normal - draw the lead first, the rest scatter slow."));
						await dialog.Msg(L("Bone-cairns are at the four trail-junctions. Kneel by each, chalk a deep cross on the largest bone. The patrols read it as 'pack confirmed, hold the line here.'"));
						break;

					case "info":
						await dialog.Msg(L("Doyors avoid bone. Old hunter-trick - pack a cairn with deer-bone, the Doyor route around it. Stone they ignore. Bone tells them a hunter knows the trail."));
						await dialog.Msg(L("Chalk on the bone is for the patrols, not the Doyors. Two languages, one cairn. Saves work."));
						break;

					case "leave":
						await dialog.Msg(L("It is the village's fight. The village asked me. I'm asking you. The next missing child won't have a name yet either."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDoyors", out var killObj)) return;
				if (!quest.TryGetProgress("chalkTrails", out var tObj)) return;

				if (killObj.Done && tObj.Done)
				{
					await dialog.Msg(L("{#666666}*She unstrings the yew-bow with deliberate care, the way her mother taught her*{/}"));
					await dialog.Msg(L("Packs are pushed back east, trails are chalked. The patrols will walk the bone-cairn line at first light."));
					await dialog.Msg(L("Take this. Huntress's purse, plus a stone the children's mother sent for whoever did this. She didn't say more. She didn't need to."));
					character.Quests.Complete(questId);
				}
				else if (!killObj.Done)
				{
					await dialog.Msg(L("Thirty Doyors first. Chalking trails while packs still roam east-of-village just waves a flag for them."));
				}
				else
				{
					await dialog.Msg(L("Packs are broken. Now the four bone-cairns - chalk a deep cross on the largest bone of each. The patrols read the marks before sundown."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("No missing children this season. The patrols walk the bone-line every dawn. The mother who sent the stone - she came by last week and asked your name. I told her I didn't know. She left a second stone."));
			}
		});

		// Doyor pack-trail chalk points for Quest 1004
		//-------------------------------------------------------------------------
		void AddPackTrail(int trailNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Doyor Pack-Trail"), "f_huevillage_58_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_huevillage_58_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A bone-cairn beside a worn pack-trail through the cobalt undergrowth*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_huevillage_58_3.Quest1004.Trail{trailNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already chalked for the patrols*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Chalking trail..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_huevillage_58_3.Quest1004.TrailsChalked", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_huevillage_58_3.Quest1004.TrailsChalked", count);
					character.ServerMessage(LF("Trails chalked: {0}/4", count));

					if (count >= 4)
						character.ServerMessage(L("{#FFD700}All trails chalked! Return to Huntress Giedre.{/}"));
				}
				else
				{
					character.ServerMessage(L("Chalking interrupted."));
				}
			});
		}

		AddPackTrail(1, 1500, -700, 0);
		AddPackTrail(2, 1900, -300, 90);
		AddPackTrail(3, 1600, -900, 180);
		AddPackTrail(4, 2000, -600, 270);

		// Quest 5: The Upent Lord
		//-------------------------------------------------------------------------
		AddNpc(147418, L("[Bounty Hunter] Egle-II"), "f_huevillage_58_3", 152, -761, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_3", 1005);
			var lordSpawnedKey = "Laima.Quests.f_huevillage_58_3.Quest1005.LordSpawned";

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A bounty hunter unrolls a hide-map across a stump, weighting the corners with musk-marked stones*{/}"));
				await dialog.Msg(L("Upent Lord ranges the deep cobalt. Solitary, three times the size of a normal Upent, marks his territory with these musk-stones I've collected from his perimeter."));
				await dialog.Msg(L("He's old enough to remember when humans were hunted, not the hunters. The protector-instinct runs iron in him - kill enough of his territory's smaller beasts and he comes down personal."));

				var response = await dialog.Select(L("Scatter the three Upent territory-stones to insult him, then kill 10 Caros - they're his territory's lesser, he'll come down to defend them. Will you face the Lord?"),
					Option(L("I'll face the Lord"), "help"),
					Option(L("Why does scattering stones insult him?"), "info"),
					Option(L("That's old-hunter work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("{#666666}*She rolls the map back up and tucks it into her pack*{/}"));
						await dialog.Msg(L("Three musk-stones in his territory - boot them apart, smear the musk into the dirt. He'll know inside an hour."));
						await dialog.Msg(L("Then ten Caros. He won't show before the tenth. When he does, fight in the open - he uses thrown stones as well as claws, and the canopy throws the trajectory."));
						break;

					case "info":
						await dialog.Msg(L("Upent territory-marking is older than any cult. The musk-stone says 'this ground is mine and I will protect what lives on it.' Scattering the stones tells him a rival has claimed his ground."));
						await dialog.Msg(L("A rival is something he must answer in person. A hunter, he could ignore. A claim on his territory - never."));
						break;

					case "leave":
						await dialog.Msg(L("It is old-hunter work. The old hunters are mostly dead. Cobalt's quieter for it. The Upent isn't."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("scatterStones", out var sObj)) return;
				if (!quest.TryGetProgress("killCaros", out var cObj)) return;
				if (!quest.TryGetProgress("killLord", out var lObj)) return;

				if (sObj.Done && cObj.Done && lObj.Done)
				{
					await dialog.Msg(L("{#666666}*She tosses you a leather purse heavy with old guild-silver*{/}"));
					await dialog.Msg(L("Stones scattered, Lord's down. The territory's open ground now - the Caros will recolonise within a year, but no Upent will hold the lordship for a generation."));
					await dialog.Msg(L("Take this. Hunter's bounty plus a stipend from the village - they'll sleep easier knowing the protector-beast is gone."));
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
				else if (!sObj.Done)
				{
					await dialog.Msg(L("The three musk-stones first - boot them apart, smear the musk. He won't come for less than a real claim on his ground."));
				}
				else
				{
					await dialog.Msg(L("Stones are scattered, the insult's read. Now ten of his Caros and he comes down personal."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cobalt's quiet again. The Upent's musk-line has faded; new Caros are nesting where the old territory ran. The forest forgets fast."));
			}
		});

		// Upent territory-stones for Quest 1005
		//-------------------------------------------------------------------------
		void AddTerritoryStone(int stoneNumber, int x, int z, int direction)
		{
			AddNpc(47190, L("Upent Territory-Stone"), "f_huevillage_58_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_huevillage_58_3", 1005);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A musk-marked boulder, dragged here by something massive*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_huevillage_58_3.Quest1005.Stone{stoneNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already scattered to gravel*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Scattering stone..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var count = character.Variables.Perm.GetInt("Laima.Quests.f_huevillage_58_3.Quest1005.StonesScattered", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_huevillage_58_3.Quest1005.StonesScattered", count);
					character.ServerMessage(LF("Territory-stones scattered: {0}/3", count));

					if (count >= 3)
						character.ServerMessage(L("{#FFD700}All territory-stones scattered! Now bait out the Upent Lord.{/}"));
				}
				else
				{
					character.ServerMessage(L("Scattering interrupted."));
				}
			});
		}

		AddTerritoryStone(1, 1100, -700, 0);
		AddTerritoryStone(2, 1300, -500, 90);
		AddTerritoryStone(3, 900, -800, 180);

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

		AddDrop(650275, 0.40f, MonsterId.Tipio);
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

		AddDrop(650280, 0.40f, MonsterId.Tiny_Bow);
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

		AddObjective("chalkTrails", L("Chalk the four Doyor pack-trails"),
			new VariableCheckObjective("Laima.Quests.f_huevillage_58_3.Quest1004.TrailsChalked", 4, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_3.Quest1004.TrailsChalked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_3.Quest1004.Trail{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_3.Quest1004.TrailsChalked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_3.Quest1004.Trail{i}");
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

		AddObjective("scatterStones", L("Scatter the three Upent territory-stones"),
			new VariableCheckObjective("Laima.Quests.f_huevillage_58_3.Quest1005.StonesScattered", 3, true));

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

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_3.Quest1005.StonesScattered");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_3.Quest1005.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_3.Quest1005.StonesScattered");
		for (int i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_3.Quest1005.Stone{i}");
	}
}

