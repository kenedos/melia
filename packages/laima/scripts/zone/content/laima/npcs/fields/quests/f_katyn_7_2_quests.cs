//--- Melia Script ----------------------------------------------------------
// Katyn 7-2 Quest NPCs
//--- Description -----------------------------------------------------------
// Restless ghost soldiers haunting the Katyn 7-2 coast. Each one died here.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
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
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.Scripting.Dialogues;

public class FKatyn72QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: Drowned by Sakmoli — ghost between trees
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Drowned Soldier"), "f_katyn_7_2", 1833, 2343, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1001);
			dialog.SetTitle(L("Drowned Soldier"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tide... Tide... I drowned in the tide..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Patrol... I walked the patrol... The Sakmoli came up the dunes... Their teeth... Their teeth in the wet sand..."));
					await dialog.Msg(L("They dragged me... Under... Under... I could not rise..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will avenge you"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Forty... Kill forty... So my soul... So my soul can rest...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Forty... Kill forty... So my soul... So my soul can rest..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSakmoli", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("They are dead... They are dead... I can feel the tide letting go..."));
					await dialog.Msg(L("Take this... A soldier's purse... I have no use for coin... Where I am going..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More... More of them... I still hear them in the surf..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Quiet... So quiet now... Thank you... Thank you..."));
			}
		});

		// Quest 1002: Lost in the crypts — ghost between trees
		//-------------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Lost Sentinel"), "f_katyn_7_2", 1213, 2087, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1002);
			dialog.SetTitle(L("Lost Sentinel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Walls... Walls all the same... I cannot find the door..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Crypt... I went into the crypt... The shades came... Ellomago... Eyes everywhere... Eyes... Eyes..."));
					await dialog.Msg(L("My torch went out... I never saw the door again..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will silence the shades"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Twenty-eight... Drive them down... Drive them down... So I may find the door...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Twenty-eight... Drive them down... Drive them down... So I may find the door..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killEllomago", out var killObj)) return;
				if (!quest.TryGetProgress("gatherSpirits", out var sObj)) return;
				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Light... I can see the door... I can see the door..."));
					await dialog.Msg(L("Take this purse... I will not need it past the door..."));
					character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More eyes... I still hear them whispering... Whispering my name..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The door... The door is open... I will pass through soon... Soon..."));
			}
		});

		// Quest 1003: Killed by jellyfish — ghost between trees (reef victim)
		//-------------------------------------------------------------------------
		AddGhostNpc(155132, L("[Restless Soul] Stung Diver"), "f_katyn_7_2", 1753, 884, 135, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1003);
			dialog.SetTitle(L("Stung Diver"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Burning... Burning... The water was burning..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Reef... I dove for coral... For my sister's wedding-bowl... Red... Red in the water... So many of them..."));
					await dialog.Msg(L("They wrapped me... Like ropes... Like ropes of fire..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will cut them down"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Eighteen... Cut them... Cut them... Bring me the corals... So my sister... My sister still gets her bowl...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Eighteen... Cut them... Cut them... Bring me the corals... So my sister... My sister still gets her bowl..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killJelly", out var killObj)) return;
				if (!quest.TryGetProgress("gatherCoral", out var cObj)) return;
				if (killObj.Done && cObj.Done)
				{
					await dialog.Msg(L("The corals... Six... Blue... My sister... She will know..."));
					await dialog.Msg(L("Take this... My diving-purse... Salt-stained... Honest..."));
					character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Burning... Still burning in the water..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("She has the bowl... She has the bowl... I felt her hold it..."));
			}
		});

		// Quest 1004: Sunk in the bog — ghost between trees
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Bog-Sunken"), "f_katyn_7_2", 3595, 652, 135, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1004);
			dialog.SetTitle(L("Bog-Sunken"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Down... Down... The bog took me down..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Salt-cart... Wheel cracked... I knelt to lift it... The Ridimed... They came up through the mud..."));
					await dialog.Msg(L("Hands... So many hands... Pulling... Pulling..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will end the Ridimed"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Eighteen... End them... End them... So no other soldier... No other soldier...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Eighteen... End them... End them... So no other soldier... No other soldier..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRidimed", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("The mud... The mud is quiet... I can feel the ground holding firm..."));
					await dialog.Msg(L("Take this... A bog-soaked purse... The coin still good..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More hands... I still feel them... Reaching up..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The salt-carts roll... I can hear the wheels... Going on without me..."));
			}
		});

		// Quest 1005: Killed by Sakmoli Alpha — ghost between trees
		//-------------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Torn Hunter"), "f_katyn_7_2", 601, -349, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1005);
			dialog.SetTitle(L("Torn Hunter"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Big... So big... Bigger than the others..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("I hunted the pack... I was good... I was so good... Until the Alpha came..."));
					await dialog.Msg(L("Three times my size... Scarred old jaws... He took my arm... Then my throat..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will kill the Alpha"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Ten of his pack... Then him... Then him... Avenge... Avenge me...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Ten of his pack... Then him... Then him... Avenge... Avenge me..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var aObj)) return;
				var alphaKey = "Laima.Quests.f_katyn_7_2.Quest1005.AlphaSpawned";
				if (pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("He is dead... He is dead... I can feel my arm again..."));
					await dialog.Msg(L("My hunting-purse... It is yours... Yours..."));
					character.Variables.Perm.Remove(alphaKey);
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaKey, true);
						if (SpawnTempMonsters(character, MonsterId.Sakmoli, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("He comes... He comes for you now..."));
							character.ServerMessage(L("{#FF9966}The Sakmoli Alpha rises from the breakers!{/}"));
						}
					}
					else await dialog.Msg(L("Find him... Find him..."));
				}
				else await dialog.Msg(L("The pack first... Thin them... Thin them..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Avenged... I am avenged... I can rest..."));
			}
		});

		// Quest 1006: Died at the walls (siege victim) — different setting, two walls
		//-------------------------------------------------------------------------
		AddGhostNpc(155132, L("[Restless Soul] Wall-Slain Captain"), "f_katyn_7_2", 2347, -2545, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_7_2", 1006);
			dialog.SetTitle(L("Wall-Slain Captain"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("They came over the wall... They came over both walls..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Siege... My company held this corner... The walls were our shield... Sakmoli came from the west wall... Ridimed from the east... Jellyfish in the moat..."));
					await dialog.Msg(L("Surrounded... Three sides at once... I... I fell where I stand now..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will break the siege"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Twelve of each... Twelve... Twelve... So my company... My company is finally relieved...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Twelve of each... Twelve... Twelve... So my company... My company is finally relieved..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSakmoli", out var sObj)) return;
				if (!quest.TryGetProgress("killRidimed", out var rObj)) return;
				if (!quest.TryGetProgress("killJelly", out var jObj)) return;
				if (sObj.Done && rObj.Done && jObj.Done)
				{
					await dialog.Msg(L("The walls... The walls are quiet... My company is relieved..."));
					await dialog.Msg(L("Captain's purse... Take it... Take it... I will dissolve at last..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Three sides... Still three sides... Cut them all..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Relieved... My company is relieved... I am... I am..."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FKatyn72Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1001);
		SetName(L("The Drowned Soldier"));
		SetType(QuestType.Sub);
		SetDescription(L("A drowned soldier's ghost begs for the Sakmoli that took him to be put down."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Drowned Soldier"), "f_katyn_7_2");

		AddObjective("killSakmoli", L("Kill Sakmoli on the dunes"),
			new KillObjective(40, new[] { MonsterId.Sakmoli }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FKatyn72Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1002);
		SetName(L("The Lost Sentinel"));
		SetType(QuestType.Sub);
		SetDescription(L("A sentinel's ghost lost in the crypts begs for the Ellomago shades to be silenced."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Lost Sentinel"), "f_katyn_7_2");

		AddObjective("killEllomago", L("Kill Ellomago"),
			new KillObjective(28, new[] { MonsterId.Ellomago }));

		AddObjective("gatherSpirits", L("Gather fading spirits"),
			new CollectItemObjective(664096, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(664096, 0.40f, MonsterId.Ellomago);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(664096, character.Inventory.CountItem(664096), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn72Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1003);
		SetName(L("The Stung Diver"));
		SetType(QuestType.Sub);
		SetDescription(L("A drowned diver begs for the Red Jellyfish to be cut down and the corals he died for to be brought."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Stung Diver"), "f_katyn_7_2");

		AddObjective("killJelly", L("Kill Red Jellyfish"),
			new KillObjective(18, new[] { MonsterId.Jellyfish_Red }));

		AddObjective("gatherCoral", L("Gather blue corals"),
			new CollectItemObjective(668044, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(668044, 0.45f, MonsterId.Jellyfish_Red);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668044, character.Inventory.CountItem(668044), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FKatyn72Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1004);
		SetName(L("The Bog-Sunken"));
		SetType(QuestType.Sub);
		SetDescription(L("A bog-drowned soldier begs for the Ridimed that pulled him under to be ended."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Bog-Sunken"), "f_katyn_7_2");

		AddObjective("killRidimed", L("Kill Ridimed"),
			new KillObjective(18, new[] { MonsterId.Ridimed }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 3));
	}
}

public class FKatyn72Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1005);
		SetName(L("The Torn Hunter"));
		SetType(QuestType.Sub);
		SetDescription(L("A hunter's ghost begs for the Sakmoli Alpha that tore him apart to fall."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Torn Hunter"), "f_katyn_7_2");

		AddObjective("killPack", L("Kill Sakmoli pack"),
			new KillObjective(10, new[] { MonsterId.Sakmoli }));

		AddObjective("killAlpha", L("Defeat the Sakmoli Alpha"),
			new KillObjective(1, new[] { MonsterId.Sakmoli }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

public class FKatyn72Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_7_2", 1006);
		SetName(L("The Wall-Slain Captain"));
		SetType(QuestType.Sub);
		SetDescription(L("A captain who fell at the walls begs for the three monsters that overran his company to be put down."));
		SetLocation("f_katyn_7_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Restless Soul] Wall-Slain Captain"), "f_katyn_7_2");

		AddObjective("killSakmoli", L("Kill Sakmoli"),
			new KillObjective(12, new[] { MonsterId.Sakmoli }));

		AddObjective("killRidimed", L("Kill Ridimed"),
			new KillObjective(12, new[] { MonsterId.Ridimed }));

		AddObjective("killJelly", L("Kill Red Jellyfish"),
			new KillObjective(12, new[] { MonsterId.Jellyfish_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
