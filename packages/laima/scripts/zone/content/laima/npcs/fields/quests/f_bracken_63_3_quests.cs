//--- Melia Script ----------------------------------------------------------
// Dadan Jungle Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Dadan Jungle - the deepest reach of the Bracken corruption.
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

public class FBracken633QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Dadan Gate Watch
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Scout] Tovin"), "f_bracken_63_3", 1500, -20, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1001);

			dialog.SetTitle(L("Tovin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Dadan Jungle. You made it through Knidos - good. But the corruption here is older. Deeper."));
				await dialog.Msg(L("The Gosaru at the gate are the first wave. They've fully given in to the poison. We need them pushed back before anyone else comes through."));

				var response = await dialog.Select(L("How can I help?"),
					Option(L("I'll drive the Gosaru back"), "help"),
					Option(L("Why is it worse here?"), "info"),
					Option(L("Later"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty should clear the gate approach. They'll be all around you, just inside the treeline."));
						await dialog.Msg(L("Watch their claws - the corruption here burns as well as poisons."));
						break;

					case "info":
						await dialog.Msg(L("Dadan sits on an old shrine. Whatever the poison is rooted in, its source is near."));
						await dialog.Msg(L("The creatures here have been breathing it for generations, not weeks. They aren't turning - they are the corruption now."));
						break;

					case "leave":
						await dialog.Msg(L("Don't go deeper alone. The jungle will swallow you whole."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGosaru", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Twenty down. The gate is ours for now."));
					await dialog.Msg(L("Take your pay. You earned every coin of it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More Gosaru still press the gate. Keep thinning them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The gate is holding. Patrols can rotate through now without bleeding."));
			}
		});

		// Quest 2: Heart Crystals
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Researcher] Miela"), "f_bracken_63_3", -100, 220, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1002);

			dialog.SetTitle(L("Miela"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Vila sent me her notes from Knidos. The sap showed everything - except the core reaction."));
				await dialog.Msg(L("To finish the antidote formula, I need five pristine crystal shards from the oldest Root Crystals here in Dadan."));
				await dialog.Msg(L("They're the last ones still holding clean mineral matrix. Younger crystals in other jungles are already saturated."));

				var response = await dialog.Select(L("Can you harvest them?"),
					Option(L("I'll bring five shards"), "help"),
					Option(L("Why pristine shards?"), "info"),
					Option(L("Too dangerous"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Use a hammer and chisel - a blade shatters the matrix. Strike firmly, once."));
						await dialog.Msg(L("The pristine crystals glow faintly blue under the moss. They're scattered deep in the jungle."));
						break;

					case "info":
						await dialog.Msg(L("The antidote works by binding to the corruption's molecular signature."));
						await dialog.Msg(L("I need a clean template to model it against. Corrupted sap alone only shows me the disease - I need the healthy cousin to shape the cure."));
						break;

					case "leave":
						await dialog.Msg(L("I'll find another way. Eventually."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var shardCount = character.Inventory.CountItem(650726);

				if (shardCount >= 5)
				{
					await dialog.Msg(L("All five. And look at that inner glow - these are pristine, untouched."));
					await dialog.Msg(L("With these, I can finish the formula. We might actually have a working antidote within the month."));

					character.Inventory.Remove(650726, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("I need more shards. You have {0} of five.", shardCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The antidote trials are promising. Crude, but it slows the corruption in lab samples. Real progress."));
			}
		});

		// Pristine Crystal Harvest Points
		//-------------------------------------------------------------------------
		void AddPristineCrystal(int nodeNum, int x, int z, int direction)
		{
			AddNpc(46218, L("Pristine Root Crystal"), "f_bracken_63_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_3", 1002);
				var variableKey = $"Laima.Quests.f_bracken_63_3.Quest1002.Shard{nodeNum}";
				var spawnedKey = $"Laima.Quests.f_bracken_63_3.Quest1002.Shard{nodeNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A root crystal glows faintly blue beneath the moss*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This crystal has already been chipped*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 40)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Gosaru, 2, 90, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}The strike draws Gosaru from the deep brush!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Chiselling crystal..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650726, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Harvested: Pristine Crystal Shard"));

					var currentCount = character.Inventory.CountItem(650726);
					character.ServerMessage(LF("Shards harvested: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All shards harvested! Return to Miela.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Harvest interrupted."));
				}
			});
		}

		AddPristineCrystal(1, -684, -701, 0);
		AddPristineCrystal(2, -555, -450, 0);
		AddPristineCrystal(3, -250, 300, 0);
		AddPristineCrystal(4, 400, 130, 0);
		AddPristineCrystal(5, -1100, 1050, 0);

		// Quest 3: The Beetle Blight
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Exterminator] Gregor"), "f_bracken_63_3", 500, -420, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1003);

			dialog.SetTitle(L("Gregor"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bush Beetles are swarming the southern basin. Normal for this time of year."));
				await dialog.Msg(L("What isn't normal is the hives. Four infested hives are pumping out corrupted beetles faster than I can burn them."));
				await dialog.Msg(L("Each hive needs to be scorched down to the wax, and the adult beetles cleared before they rebuild."));

				var response = await dialog.Select(L("How do I burn the hives?"),
					Option(L("I'll burn the hives and kill the swarm"), "help"),
					Option(L("Why the hive focus?"), "info"),
					Option(L("Not my specialty"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take this torch oil - just pour it on the hive and it'll ignite on contact with the wax."));
						await dialog.Msg(L("The hives sit in clusters along the southern basin. You can smell them before you see them."));
						break;

					case "info":
						await dialog.Msg(L("Kill the adults, and the hive breeds a new generation in a week."));
						await dialog.Msg(L("Burn the hive, and it's gone. That's the only way to actually stop them."));
						break;

					case "leave":
						await dialog.Msg(L("Fire work isn't for everyone. I'll keep at it alone."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBeetles", out var killObj)) return;
				if (!quest.TryGetProgress("burnHives", out var hiveObj)) return;

				if (killObj.Done && hiveObj.Done)
				{
					await dialog.Msg(L("All four hives, and the swarm cleared. The basin will be quiet for a long while."));
					await dialog.Msg(L("Good work. Here's your pay, with interest for the dirty part."));

					character.Inventory.Remove(650809, character.Inventory.CountItem(650809), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Bush Beetles. ");
					if (!hiveObj.Done)
						status += L("Scorch the remaining hives. ");

					await dialog.Msg(LF("Finish the job. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Basin's clean. First breath of fresh air I've had in months."));
			}
		});

		// Infested Hive Burn Points
		//-------------------------------------------------------------------------
		void AddInfestedHive(int hiveNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Infested Hive"), "f_bracken_63_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_3", 1003);
				var variableKey = $"Laima.Quests.f_bracken_63_3.Quest1003.Hive{hiveNum}";
				var spawnedKey = $"Laima.Quests.f_bracken_63_3.Quest1003.Hive{hiveNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A bloated hive pulses with corrupted life*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This hive is already ash*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Bush_Beetle, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}Beetles swarm from the breached hive!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Pouring torch oil..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(5)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650809, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Scorched: Infested Hive"));

					var currentCount = character.Inventory.CountItem(650809);
					character.ServerMessage(LF("Hives scorched: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All hives burned! Return to Gregor.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Torch oil spilled uselessly. Try again."));
				}
			});
		}

		AddInfestedHive(1, 668, -808, 0);
		AddInfestedHive(2, 834, -176, 0);
		AddInfestedHive(3, 500, -600, 0);
		AddInfestedHive(4, 300, -900, 0);

		// Quest 4: The Missing Warden
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Warden] Saskia"), "f_bracken_63_3", -900, 900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1004);

			dialog.SetTitle(L("Saskia"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("My partner Yuris went north two days ago to plant fresh trail markers before the caravans arrive."));
				await dialog.Msg(L("He hasn't come back. The markers are still missing - I can see his handwriting should be on them."));
				await dialog.Msg(L("And the Raffly up there have been unusually bold. I fear the worst."));

				var response = await dialog.Select(L("What do you need me to do?"),
					Option(L("I'll find the markers and kill the Raffly"), "help"),
					Option(L("What did Yuris look like?"), "info"),
					Option(L("Hope he turns up"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thank you. The markers would be in a loop from here to the ridge and back."));
						await dialog.Msg(L("Bring me even one and I'll know. And the Raffly... if they got him, at least thin them down."));
						break;

					case "info":
						await dialog.Msg(L("Taller than me, quiet, always whittling something between patrols."));
						await dialog.Msg(L("If he's out there and alive, the markers will tell me. If he's not, the markers tell me that too."));
						break;

					case "leave":
						await dialog.Msg(L("Hoping doesn't bring him home. Someone has to go look."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRaffly", out var killObj)) return;
				var markerCount = character.Inventory.CountItem(650683);

				if (killObj.Done && markerCount >= 5)
				{
					await dialog.Msg(L("Five markers. And the writing on the last one... it cuts off mid-word."));
					await dialog.Msg(L("So he's gone. I knew it. But thanks to you, I know. That's worth something."));
					await dialog.Msg(L("Take this - it was half his anyway."));

					character.Inventory.Remove(650683, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Raffly. ");
					if (markerCount < 5)
						status += L("Find more trail markers. ");

					await dialog.Msg(LF("Keep searching. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I buried the markers under the old willow at the ridge. Closest thing to a grave I could give him."));
			}
		});

		// Trail Marker Recovery Points
		//-------------------------------------------------------------------------
		void AddTrailMarker(int markerNum, int x, int z, int direction)
		{
			AddNpc(47190, L("Warden's Trail Marker"), "f_bracken_63_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_3", 1004);
				var variableKey = $"Laima.Quests.f_bracken_63_3.Quest1004.Marker{markerNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A carved wooden marker lies fallen in the moss*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already recovered this marker*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Recovering marker..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650683, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Trail Marker"));

					var currentCount = character.Inventory.CountItem(650683);
					character.ServerMessage(LF("Markers recovered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All markers recovered! Return to Saskia.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Recovery cancelled."));
				}
			});
		}

		AddTrailMarker(1, -1134, 1211, 0);
		AddTrailMarker(2, -850, 1000, 0);
		AddTrailMarker(3, -1200, 700, 0);
		AddTrailMarker(4, -700, 1150, 0);
		AddTrailMarker(5, -1350, 450, 0);

		// Quest 5: The Ellogua Trial
		//-------------------------------------------------------------------------
		AddNpc(20109, L("[Hunter] Dovas"), "f_bracken_63_3", 900, -200, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1005);

			dialog.SetTitle(L("Dovas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("You've heard of the Ellogua? They're rare, even here. Maybe one in every hundred jungle beasts."));
				await dialog.Msg(L("They're cunning. They hunt in perfect silence, and they've been drawn to the corruption in ways the other monsters haven't."));
				await dialog.Msg(L("Three of them prowl this stretch of jungle. I'm offering a bounty - one Ellogua fang each."));

				var response = await dialog.Select(L("Three is a tall order."),
					Option(L("I'll take the bounty"), "help"),
					Option(L("How do I find them?"), "info"),
					Option(L("Find another hunter"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Good. Don't engage unless you have room to move - they circle, they never charge straight."));
						await dialog.Msg(L("Bring their fangs and I'll pay per kill."));
						break;

					case "info":
						await dialog.Msg(L("They hunt the central and eastern reaches of the jungle. Listen for silence - the other birds stop calling when an Ellogua is near."));
						await dialog.Msg(L("That silence is your only warning before the strike."));
						break;

					case "leave":
						await dialog.Msg(L("The bounty'll be here. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killElloguas", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("All three. I can see it in the way you carry yourself - none of them got an easy pass."));
					await dialog.Msg(L("Bounty paid in full. And a little extra, for the living proof that it could be done."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still hunting. Good - don't come back until all three are dead. Half-work attracts more Ellogua."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Not a new Ellogua sighting in weeks. Might be safe to guide parties through this stretch soon."));
			}
		});

		// Quest 6: The Shrine Approach
		//-------------------------------------------------------------------------
		AddNpc(155145, L("[Pathfinder] Rozalie"), "f_bracken_63_3", -200, -300, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1006);

			dialog.SetTitle(L("Rozalie"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("There's an old shrine deeper in the jungle. Most travelers don't know it exists - and that's fine."));
				await dialog.Msg(L("But if the antidote researchers need to reach it, the path needs to be clear. Gosaru and Raffly have overrun the approach."));

				var response = await dialog.Select(L("Can I open the path?"),
					Option(L("I'll clear the approach"), "help"),
					Option(L("What's at the shrine?"), "info"),
					Option(L("Not worth the trouble"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Both territories overlap along the approach path. You'll run into both on every stretch."));
						await dialog.Msg(L("Thirty clears a reliable corridor. Anything less and they close it again within a day."));
						break;

					case "info":
						await dialog.Msg(L("An old shrine. Older than Klaipeda, older than Orsha. Whatever the corruption is rooted in may be rooted there."));
						await dialog.Msg(L("If Miela's antidote works at all, we'll want to pour it at that shrine first."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe you're right. But someone has to open the path eventually."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGosaru", out var gosaruObj)) return;
				if (!quest.TryGetProgress("killRaffly", out var rafflyObj)) return;

				if (gosaruObj.Done && rafflyObj.Done)
				{
					await dialog.Msg(L("Approach is open. I can scout the shrine itself now."));
					await dialog.Msg(L("This matters more than you know. Take this with my thanks."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!gosaruObj.Done)
						status += L("Kill more Gosaru. ");
					if (!rafflyObj.Done)
						status += L("Kill more Raffly. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The approach is quiet. When the antidote's ready, the shrine will be waiting."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Dadan Gate Watch
//-----------------------------------------------------------------------------

public class DadanGateWatchQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_3", 1001);
		SetName(L("Dadan Gate Watch"));
		SetType(QuestType.Sub);
		SetDescription(L("Drive back Gosaru pressing on the eastern gate of Dadan Jungle."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout] Tovin"), "f_bracken_63_3");

		AddObjective("killGosaru", L("Drive back corrupted Gosaru"),
			new KillObjective(20, new[] { MonsterId.Gosaru }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1002 CLASS: Heart Crystals
//-----------------------------------------------------------------------------

public class HeartCrystalsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_3", 1002);
		SetName(L("Heart Crystals"));
		SetType(QuestType.Sub);
		SetDescription(L("Harvest pristine crystal shards from the oldest Root Crystals for the antidote research."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Researcher] Miela"), "f_bracken_63_3");

		AddObjective("harvestShards", L("Harvest pristine crystal shards"),
			new CollectItemObjective(650726, 5));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650726, character.Inventory.CountItem(650726), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1002.Shard{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1002.Shard{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650726, character.Inventory.CountItem(650726), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1002.Shard{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1002.Shard{i}.Spawned");
		}
	}
}

// Quest 1003 CLASS: The Beetle Blight
//-----------------------------------------------------------------------------

public class TheBeetleBlightQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_3", 1003);
		SetName(L("The Beetle Blight"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Bush Beetles and scorch the infested hives breeding them in the southern basin."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Exterminator] Gregor"), "f_bracken_63_3");

		AddObjective("killBeetles", L("Kill Bush Beetles"),
			new KillObjective(15, new[] { MonsterId.Bush_Beetle }));

		AddObjective("burnHives", L("Scorch infested hives"),
			new CollectItemObjective(650809, 4));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
		AddReward(new ItemReward(926012, 1)); // Recipe - Shield Breaker
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650809, character.Inventory.CountItem(650809), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1003.Hive{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1003.Hive{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650809, character.Inventory.CountItem(650809), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1003.Hive{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1003.Hive{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Missing Warden
//-----------------------------------------------------------------------------

public class TheMissingWardenQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_3", 1004);
		SetName(L("The Missing Warden"));
		SetType(QuestType.Sub);
		SetDescription(L("Recover the lost trail markers and kill the emboldened Raffly that may have killed the missing warden."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Warden] Saskia"), "f_bracken_63_3");

		AddObjective("killRaffly", L("Kill emboldened Raffly"),
			new KillObjective(12, new[] { MonsterId.Raffly }));

		AddObjective("findMarkers", L("Recover lost trail markers"),
			new CollectItemObjective(650683, 5));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
		AddReward(new ItemReward(941035, 1)); // Recipe - Ferret Marauder Shield
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650683, character.Inventory.CountItem(650683), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1004.Marker{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650683, character.Inventory.CountItem(650683), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1004.Marker{i}");
		}
	}
}

// Quest 1005 CLASS: The Ellogua Trial
//-----------------------------------------------------------------------------

public class TheElloguaTrialQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_3", 1005);
		SetName(L("The Ellogua Trial"));
		SetType(QuestType.Sub);
		SetDescription(L("Hunt and slay three elite Ellogua for the hunter's bounty."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Hunter] Dovas"), "f_bracken_63_3");

		AddObjective("killElloguas", L("Slay elite Ellogua"),
			new KillObjective(3, new[] { MonsterId.Ellogua }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(531122, 1)); // Plate Armor
	}
}

// Quest 1006 CLASS: The Shrine Approach
//-----------------------------------------------------------------------------

public class TheShrineApproachQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_3", 1006);
		SetName(L("The Shrine Approach"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear Gosaru and Raffly from the approach to the ancient shrine deep in Dadan Jungle."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pathfinder] Rozalie"), "f_bracken_63_3");

		AddObjective("killGosaru", L("Kill Gosaru from the approach"),
			new KillObjective(15, new[] { MonsterId.Gosaru }));

		AddObjective("killRaffly", L("Kill Raffly from the approach"),
			new KillObjective(15, new[] { MonsterId.Raffly }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(502186, 1)); // Tyla Plate Gauntlets
	}
}
