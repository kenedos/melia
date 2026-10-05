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
		// Quest 1: Dadan Pond Watch
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Scout] Tovin"), "f_bracken_63_3", 698, 600, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1001);

			dialog.SetTitle(L("Tovin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Dadan Jungle. You made it through Knidos - good. But the corruption here is older. Deeper."));
				await dialog.Msg(L("Quiet a moment. See those bushes ringing the pond? Gosaru are crouched in every one of them, watching the water."));
				await dialog.Msg(L("They've fully given in to the poison. We need them thinned before they decide to break out and rush whoever passes through next."));

				var response = await dialog.Select(L("Will you flush them out for me?"),
					Option(L("I'll drive the Gosaru back"), "help"),
					Option(L("Why is it worse here?"), "info"),
					Option(L("Later"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty should clear the bushes around the pond. They'll come at you the moment you push into the reeds."));
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
					await dialog.Msg(L("Twenty down. The reeds are quiet for the first time in a week."));
					await dialog.Msg(L("Take your pay. You earned every coin of it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More Gosaru still crouching in the bushes around the pond. Keep thinning them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pond's quiet. Nothing moving in the reeds anymore - patrols can pass without bleeding."));
			}
		});

		// Quest 2: Heart Crystals
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Researcher] Miela"), "f_bracken_63_3", -94, 524, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1002);

			dialog.SetTitle(L("Miela"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Vila sent me her notes from Knidos. The sap showed everything - except the core reaction."));
				await dialog.Msg(L("To finish the antidote formula, I need five pristine crystal shards from the oldest Root Crystals here in Dadan."));
				await dialog.Msg(L("They're the last ones still holding clean mineral matrix. Younger crystals in other jungles are already saturated."));

				var response = await dialog.Select(L("Will you bring me the shards?"),
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
		void AddPristineCrystal(int nodeNum, double x, double z, double direction)
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

		AddPristineCrystal(1, -448.715, 288.18466, 45);
		AddPristineCrystal(2, -924.7725, -15.992916, 45);
		AddPristineCrystal(3, -37.748558, -425.46445, 45);
		AddPristineCrystal(4, 272.47775, -610.41724, 45);
		AddPristineCrystal(5, 179.1873, 172.795, 45);
		AddPristineCrystal(6, 523.845, 108.45006, 45);
		AddPristineCrystal(7, -26.807753, 623.41736, 45);

		// Quest 3: The Beetle Blight
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Exterminator] Gregor"), "f_bracken_63_3", 653, -721, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1003);

			dialog.SetTitle(L("Gregor"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Bush Beetles are swarming the ridges north-east of here, right up against the Novaha Monastery gate."));
				await dialog.Msg(L("Six infested hives are pumping out corrupted beetles faster than the monastery wardens can burn them."));
				await dialog.Msg(L("Each hive needs to be scorched down to the wax, and the adult beetles cleared before they rebuild."));

				var response = await dialog.Select(L("Will you torch the hives for me?"),
					Option(L("I'll burn the hives and kill the swarm"), "help"),
					Option(L("Why the hive focus?"), "info"),
					Option(L("Not my specialty"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take this torch oil - just pour it on the hive and it'll ignite on contact with the wax."));
						await dialog.Msg(L("The hives sit clustered along the slope north-east of here, on the approach to Novaha Monastery. You'll smell them before you see them."));
						await dialog.Msg(L("Watch your step - getting close to a hive is enough to wake the brood inside. They'll come boiling out at you."));
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
					await dialog.Msg(L("Hives ash, swarm cleared. The Novaha approach will be quiet for a long while."));
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
				await dialog.Msg(L("Slope's clean all the way to the monastery gate. First breath of fresh air I've had in months."));
			}
		});

		// Infested Hive Burn Points
		//-------------------------------------------------------------------------
		void AddInfestedHive(int hiveNum, int modelId, double x, double z, double direction)
		{
			AddNpc(modelId, L("Infested Hive"), "f_bracken_63_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_3", 1003);
				var variableKey = $"Laima.Quests.f_bracken_63_3.Quest1003.Hive{hiveNum}";

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

			AddAreaTrigger("f_bracken_63_3", x, z, 50, async (args) =>
			{
				if (args.Initiator is not Character character)
					return;

				if (character.IsDead)
					return;

				var questId = new QuestId("f_bracken_63_3", 1003);
				if (!character.Quests.IsActive(questId))
					return;

				var variableKey = $"Laima.Quests.f_bracken_63_3.Quest1003.Hive{hiveNum}";
				if (character.Variables.Perm.GetBool(variableKey, false))
					return;

				if (RandomProvider.Get().Next(100) >= 20)
					return;

				var amount = RandomProvider.Get().Next(2, 4);
				if (SpawnTempMonsters(character, MonsterId.Bush_Beetle, amount, 80, TimeSpan.FromMinutes(1)))
					character.ServerMessage(L("{#FF6666}Beetles boil out of the hive!{/}"));
			});
		}

		AddInfestedHive(1, 151025, -467, -1034, 90);
		AddInfestedHive(2, 151026, -361, -1316, 90);
		AddInfestedHive(3, 151027, -75, -1217, 90);
		AddInfestedHive(4, 151025, -74, -1036, 90);
		AddInfestedHive(5, 151026, -212, -1108, 90);
		AddInfestedHive(6, 151027, -159, -930, 90);

		// Quest 4: The Missing Warden
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Warden] Saskia"), "f_bracken_63_3", -990, 862, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1004);

			dialog.SetTitle(L("Saskia"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("My partner Yuris went north two days ago to inspect the warden pillars on the ridge. He's overdue."));
				await dialog.Msg(L("Each pillar has a slot where wardens chisel their patrol notes. If he passed any of them, his marking will be on them."));
				await dialog.Msg(L("And the Raffly up there have been unusually bold lately. I'd rather not assume the worst, but I need to know."));

				var response = await dialog.Select(L("Will you go look for him?"),
					Option(L("I'll read the pillars and clear the Raffly"), "help"),
					Option(L("What did Yuris look like?"), "info"),
					Option(L("Hope he turns up"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thank you. There are four warden pillars in a loop from the ridge back. Reading three is enough to be sure of his path."));
						await dialog.Msg(L("Bring me a rubbing from each one you find. And the Raffly... if they're hunting up there, thin them down."));
						break;

					case "info":
						await dialog.Msg(L("Taller than me, quiet, always whittling something between patrols."));
						await dialog.Msg(L("He never skips a pillar - if he passed by, his mark will be on the stone. If he didn't, the stones tell me that too."));
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
				var pillarCount = character.Variables.Perm.GetInt("Laima.Quests.f_bracken_63_3.Quest1004.PillarsRead", 0);

				if (killObj.Done && pillarCount >= 3)
				{
					await dialog.Msg(L("Three pillars read - and one of them carried his mark with the words 'returning to Orsha' chiseled underneath."));
					await dialog.Msg(L("So he's alive. Walked off the route on his own. That's a relief and a reckoning all at once."));
					await dialog.Msg(L("Take this. I'll have words with him next time he passes through, but you've earned my thanks."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Raffly. ");
					if (pillarCount < 3)
						status += L("Read more warden pillars. ");

					await dialog.Msg(LF("Keep searching. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Yuris in Orsha. Of all the places. He'll get an earful when he walks back through that gate."));
			}
		});

		// Warden Pillar Reading Points
		//-------------------------------------------------------------------------
		void AddWardenPillar(int pillarNum, int modelId, int x, int z, int direction)
		{
			AddNpc(modelId, L("Warden Pillar"), "f_bracken_63_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_3", 1004);
				var variableKey = $"Laima.Quests.f_bracken_63_3.Quest1004.Pillar{pillarNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered stone pillar with a chisel-slot for warden marks*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already taken a rubbing from this pillar*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Taking a rubbing..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var counterKey = "Laima.Quests.f_bracken_63_3.Quest1004.PillarsRead";
					var currentCount = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, currentCount);

					character.ServerMessage(L("Read: Warden Pillar"));
					character.ServerMessage(LF("Pillars read: {0}/3", currentCount));

					if (currentCount >= 3)
					{
						await dialog.Msg(L("{#666666}*Yuris's mark is here - and beneath it, scraped fresh into the stone: 'returning to Orsha'*{/}"));
						character.ServerMessage(L("{#FFD700}Yuris went back to Orsha. Return to Saskia.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Rubbing interrupted."));
				}
			});
		}

		AddWardenPillar(1, 47190, -1285, 1178, 90);
		AddWardenPillar(2, 47190, -1057, 1428, 90);
		AddWardenPillar(3, 47190, -869, 1269, 90);
		AddWardenPillar(4, 47190, -1471, 1086, 90);

		// Quest 5: The Ellogua Trial
		//-------------------------------------------------------------------------
		AddNpc(20109, L("[Hunter] Dovas"), "f_bracken_63_3", 676, 34, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1005);

			dialog.SetTitle(L("Dovas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("You've heard of the Ellogua? They're rare, even here. Maybe one in every hundred jungle beasts."));
				await dialog.Msg(L("They're cunning. They hunt in perfect silence, and they've been drawn to the corruption in ways the other monsters haven't."));
				await dialog.Msg(L("Three of them prowl this stretch of jungle. I'm offering a bounty - one Ellogua fang each."));

				var response = await dialog.Select(L("Will you take the bounty?"),
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
		AddNpc(155145, L("[Pathfinder] Rozalie"), "f_bracken_63_3", -144.26146, -158.07031, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_3", 1006);

			dialog.SetTitle(L("Rozalie"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("See that elevator behind me? It runs up to Badoca Hill - the high ridge above the canopy."));
				await dialog.Msg(L("I'm convinced the old shrine sits up there. The Knidos maps marked a ruined altar near the cliff edge, and that elevator is the only way up."));
				await dialog.Msg(L("Take the elevator, find the altar, and bring back any fragment you can pry loose. Miela's research needs proof the shrine is real."));

				var response = await dialog.Select(L("Will you go to Badoca Hill?"),
					Option(L("I'll ride the elevator and find the altar"), "help"),
					Option(L("Why does the shrine matter?"), "info"),
					Option(L("Not now"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The altar's somewhere on the western ledge of Badoca Hill. Watch yourself - whatever is rooted there has been listening for a long time."));
						await dialog.Msg(L("Touching the altar may stir what's drawn to it. Don't linger. Take a fragment and come back."));
						break;

					case "info":
						await dialog.Msg(L("An old shrine. Older than Klaipeda, older than Orsha. Whatever the corruption is rooted in may be rooted there."));
						await dialog.Msg(L("If Miela's antidote works at all, we'll want to pour it at that shrine first."));
						break;

					case "leave":
						await dialog.Msg(L("The elevator isn't going anywhere. Come back when you're ready."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var fragmentCount = character.Inventory.CountItem(650601);

				if (fragmentCount >= 1)
				{
					await dialog.Msg(L("You found it. The altar is real - and this fragment proves it."));
					await dialog.Msg(L("Miela can work with this. Take this with my thanks."));

					character.Inventory.Remove(650601, 1, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Ride the elevator up to Badoca Hill. The altar is on the western ledge - bring me a fragment."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The fragment you brought matches the old Knidos sketches. The shrine is exactly where I thought."));
			}
		});

		// Shrine Ruin Altar helper (Q1006)
		//-------------------------------------------------------------------------
		void AddShrineRuinAltar(double x, double z, double direction)
		{
			AddNpc(152008, L("Shrine Ruin Altar"), "f_bracken_63_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_3", 1006);
				var variableKey = "Laima.Quests.f_bracken_63_3.Quest1006.AltarTaken";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A shattered altar of black stone, half-swallowed by roots.*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*The altar's surface is bare where you pried the fragment loose.*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 200, 500);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The altar pulses - the hill stirs against you!{{/}}"));

				var result = await character.TimeActions.StartAsync(
					L("Prying altar fragment..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650601, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Destroyed Altar Fragment"));
					character.ServerMessage(L("{#FFD700}Return to Rozalie.{/}"));
				}
				else
				{
					character.ServerMessage(L("You let go of the altar. The fragment stays with the stone."));
				}
			});
		}

		AddShrineRuinAltar(-566.1832, -652.59015, 0);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Dadan Pond Watch
//-----------------------------------------------------------------------------

public class DadanPondWatchQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_3", 1001);
		SetName(L("Dadan Pond Watch"));
		SetType(QuestType.Sub);
		SetDescription(L("Flush out the corrupted Gosaru hiding in the bushes around the pond in Dadan Jungle."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout] Tovin"), "f_bracken_63_3");

		AddObjective("killGosaru", L("Flush corrupted Gosaru from the pond bushes"),
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

		for (int i = 1; i <= 7; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1002.Shard{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1002.Shard{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650726, character.Inventory.CountItem(650726), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 7; i++)
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
		SetDescription(L("Kill Bush Beetles and scorch the infested hives breeding them on the slope below Novaha Monastery."));
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

		for (int i = 1; i <= 6; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1003.Hive{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650809, character.Inventory.CountItem(650809), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 6; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1003.Hive{i}");
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
		SetDescription(L("Read three of the four warden pillars on the ridge and clear the emboldened Raffly to find out what happened to Yuris."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Warden] Saskia"), "f_bracken_63_3");

		AddObjective("killRaffly", L("Kill emboldened Raffly"),
			new KillObjective(12, new[] { MonsterId.Raffly }));

		AddObjective("readPillars", L("Read warden pillars on the ridge"),
			new VariableCheckObjective("Laima.Quests.f_bracken_63_3.Quest1004.PillarsRead", 3, true));

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
		character.Variables.Perm.Remove("Laima.Quests.f_bracken_63_3.Quest1004.PillarsRead");

		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1004.Pillar{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_bracken_63_3.Quest1004.PillarsRead");

		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_3.Quest1004.Pillar{i}");
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
		AddReward(new ItemReward(512186, 1)); // Tyla Plate Boots
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
		SetDescription(L("Ride the Badoca Hill elevator and pry a fragment from the ruined shrine altar for Rozalie."));
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pathfinder] Rozalie"), "f_bracken_63_3");

		AddObjective("collectAltarFragment", L("Recover a Destroyed Altar Fragment from the Badoca Hill shrine"),
			new CollectItemObjective(650601, 1));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(502186, 1)); // Tyla Plate Gauntlets
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650601, character.Inventory.CountItem(650601), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_bracken_63_3.Quest1006.AltarTaken");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650601, character.Inventory.CountItem(650601), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_bracken_63_3.Quest1006.AltarTaken");
	}
}
