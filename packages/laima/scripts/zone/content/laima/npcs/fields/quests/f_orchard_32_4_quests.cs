//--- Melia Script ----------------------------------------------------------
// Seir Rainforest Quest NPCs
//--- Description -----------------------------------------------------------
// The Ferret trade country, the saplings that will no longer take root in it,
// and the demon bleeding into the soil under all of it.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard324QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Ceyral Saplings
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Sapling-Keeper] Ruta"), "f_orchard_32_4", 905, 549, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1001);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's on her knees over an empty planting row, staring at a hole where a sapling should be*{/}"));
				await dialog.Msg(L("Third one this row, gone by morning. I plant Ceyral - the only tree that will hold this soil once the rain gets into it - and I have put 400 saplings into this rainforest in 6 years."));
				await dialog.Msg(L("The Ferret Searchers dig them up the same week and carry them off. Get me 10 saplings back off them so I have something to replant with."));

				var response = await dialog.Select(L("Will you get the saplings back?"),
					Option(L("I'll recover 10 saplings"), "help"),
					Option(L("Why do they dig them up?"), "info"),
					Option(L("Plant somewhere else"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("A Searcher carries them in a hip sling and it will drop the sling before it drops anything else. Push one hard and you get the whole sling."));
						break;

					case "info":
						await dialog.Msg(L("They did not, for the first 4 years. They walked past my rows the way you walk past a fence. Something changed 2 years ago and now they take every single one."));
						await dialog.Msg(L("They are not eating them. I have followed a Searcher for half a day and watched it put a sapling down on bare rock and walk away."));
						break;

					case "leave":
						await dialog.Msg(L("There is nowhere else. This is the slope that goes into the river when the rain comes, and it comes every year."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectSaplings", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She checks each root ball with a thumbnail before setting it down*{/}"));
					await dialog.Msg(L("Ten, and 8 of them still alive. That is a full row and I will have them in the ground before dark."));
					await dialog.Msg(L("Take the planting fund. The Kingdom pays it by the sapling and it has never once asked whether the saplings survived."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. The Searchers work the northern gullies - that is where my rows were."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The row is in and 2 of them have gone brown at the tip already. That is not a Searcher. That is the ground."));
			}
		});

		// Quest 1002: The Archers on the Trail
		//---------------------------------------------------------------------
		AddNpc(147484, L("[Trail-Warden] Sabas"), "f_orchard_32_4", 1435, 34, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1002);

			dialog.SetTitle(L("Sabas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He steps out from behind a tree trunk with a hand still on his knife, then lets go of it once he sees your face*{/}"));
				await dialog.Msg(L("Sorry - can't be too careful on this stretch any more. I have walked the Seir trail for 9 years and traded with the Ferrets the whole time. Salt for resin, and neither side ever counted too hard."));
				await dialog.Msg(L("Now their Archers shoot at the trail from the canopy and I have lost 2 porters. Kill 30 of them, because I would very much like to go back to trading."));

				var response = await dialog.Select(L("Will you clear the trail?"),
					Option(L("I'll kill the Ferret Archers"), "help"),
					Option(L("You traded with them?"), "info"),
					Option(L("Trade a different route"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They shoot from the canopy and drop when you get close. Kill them on the ground - in the branches you will spend all day looking up."));
						break;

					case "info":
						await dialog.Msg(L("Nine years. I know their supply soldiers by sight. One of them used to leave a resin block on the trail stone for me and take the salt without ever showing itself."));
						await dialog.Msg(L("The block stopped 2 years ago. I put salt out for a month anyway, which tells you what sort of trader I am."));
						break;

					case "leave":
						await dialog.Msg(L("There is one trail through this rainforest and it is this one. The rest is river and gulley."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Walked the trail end to end with a full pack and nothing came out of the canopy. I did not enjoy it as much as I expected to."));
					await dialog.Msg(L("Take the warden's cut. It is 9 years of salt money and it has been sitting in a strongbox getting damp."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still archers up there. You will hear the branch move before you hear anything else."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I put a salt block on the trail stone again last night. It was gone this morning and there was no resin. So something is still taking it, and it is not trading."));
			}
		});

		// Quest 1003: Purifying the Soil
		//---------------------------------------------------------------------
		AddNpc(152064, L("[Priest of Vakarine] Ivona"), "f_orchard_32_4", -1051, -660, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1003);

			dialog.SetTitle(L("Ivona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's kneeling at the shrine, sorting a stack of scrolls into a neat, deliberate row without looking up right away*{/}"));
				await dialog.Msg(L("You have good timing - I was about to go looking for someone with steady legs. Ruta plants trees that die. Sabas trades with Ferrets that shoot at him. Both date it to 2 years ago, and neither has walked out to the western gullies to look at the ground."));
				await dialog.Msg(L("I have. There are 4 patches out there where the soil runs black and nothing grows. Take these purification scrolls and burn one on each patch."));

				var response = await dialog.Select(L("Will you burn the scrolls?"),
					Option(L("I'll purify all 4 patches"), "help"),
					Option(L("What's in the soil?"), "info"),
					Option(L("That won't fix a rainforest"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(667024, 4, InventoryAddType.PickUp);
						await dialog.Msg(L("Burn it flat on the patch, not held up. The scroll has to be touching what it is cleaning or it purifies a very holy piece of air."));
						break;

					case "info":
						await dialog.Msg(L("Blood. Not animal blood and not spilled - seeping, from underneath, steadily, for about 2 years."));
						await dialog.Msg(L("A Ceyral sapling put into that will die in a fortnight. A Ferret that walks over it every day for 2 years will do something else, and it has."));
						break;

					case "leave":
						await dialog.Msg(L("No. It will tell me how deep the source is by how fast the black comes back, which is the only thing I can measure from up here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("purifyPatches", out var patchObj)) return;

				if (patchObj.Done)
				{
					await dialog.Msg(L("All 4 clean, and 3 of them went black again before you got back to me. The source is directly under this forest and it is not deep."));
					await dialog.Msg(L("Take what the shrine has. It has been collecting offerings for 2 years from people whose trees keep dying and it has never once helped them."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Patches still unburned. All 4, out in the western gullies where the ground drops."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three of four back to black inside an hour. If I have the arithmetic right, the thing bleeding is about 40 paces down and directly beneath the searcher gullies."));
			}
		});

		// Quest 1003 interaction points - the blackened soil patches
		//---------------------------------------------------------------------
		void AddTaintedPatch(int patchNumber, string observation, int x, int z, int direction)
		{
			AddNpc(47200, L("Blackened Soil"), "f_orchard_32_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_32_4", 1003);
				var variableKey = $"Laima.Quests.f_orchard_32_4.Quest1003.Patch{patchNumber}";
				var counterKey = "Laima.Quests.f_orchard_32_4.Quest1003.PatchesPurified";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A patch of soil gone black, with nothing growing on it*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*The ash of your scroll is already darkening again*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 500, 400);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The burning scroll carries - {0} drawn in!{{/}}", luredCount));

				var result = await character.TimeActions.StartAsync(
					L("Burning the purification scroll..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(5)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var purified = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, purified);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Patches purified: {0}/4", purified));

					if (purified >= 4)
						character.ServerMessage(L("{#FFD700}All 4 patches burned. Return to Priest Ivona.{/}"));
				}
				else
				{
					character.ServerMessage(L("The scroll goes out before it catches."));
				}
			});
		}

		AddTaintedPatch(1, L("The first patch runs clean and pale under the ash."), -183, 1370, 0);
		AddTaintedPatch(2, L("The second patch clears, and the black creeps back in at the edge as you watch."), -835, 992, 90);
		AddTaintedPatch(3, L("The third patch takes twice as long to burn through."), 9, 669, 180);
		AddTaintedPatch(4, L("The fourth patch clears and is grey again before you have stood up."), -295, 1420, 270);

		// Quest 1004: The Earth Flower
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Sapling-Keeper] Ruta"), "f_orchard_32_4", 470, 912, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1004);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She waves you over before you've even finished walking up, already mid-thought*{/}"));
				await dialog.Msg(L("Good, you're still here - I've been turning an idea over since this morning. The Earth Flower used to grow all through these gullies. Put one at the head of a Ceyral row and the whole row takes; it does something to the soil I cannot do with a spade."));
				await dialog.Msg(L("There has not been one in 2 years, but the Ferret Merchants hoard seeds. Kill 15 of them and bring me 6 Earth Flower Seeds."));

				var response = await dialog.Select(L("Will you get the seeds?"),
					Option(L("I'll hunt the Merchants and bring 6 seeds"), "help"),
					Option(L("The Merchants hoard seeds?"), "info"),
					Option(L("A flower won't beat black soil"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("A Merchant carries its hoard on its back and it will not put the pack down for anything, so the pack comes off it dead or not at all."));
						break;

					case "info":
						await dialog.Msg(L("They hoard everything. That is the whole of their trade. What is unusual is that they have kept these particular seeds for 2 years and never once put one in the ground."));
						await dialog.Msg(L("Sabas thinks they are saving them. I think something told them to take them out of the ground and nobody told them what to do next."));
						break;

					case "leave":
						await dialog.Msg(L("Ivona says the source is 40 paces down. I cannot dig 40 paces. I can put a flower at the head of a row and I am going to."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMerchants", out var killObj)) return;
				if (!quest.TryGetProgress("collectSeeds", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She rolls a seed between her fingers and it leaves a green smear*{/}"));
					await dialog.Msg(L("Alive. Two years in a Ferret pack and still alive. That is a tougher seed than I am a gardener."));
					await dialog.Msg(L("Take the rest of the planting fund. If the flower takes, I will have earned it back by spring, and if it does not, I will not need it."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty of Merchants down and I still need seeds. Not every pack has them - go for the heavy ones."));
				}
				else
				{
					await dialog.Msg(L("Still Merchants moving through the gullies. They travel loaded and they travel slow."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("One of the 6 has opened. An Earth Flower of Vigor at the head of my northern row and the whole row is still green. First green row in 2 years."));
			}
		});

		// Quest 1005: What Is Bleeding Under the Gullies
		//---------------------------------------------------------------------
		AddNpc(152064, L("[Priest of Vakarine] Ivona"), "f_orchard_32_4", -1714, 870, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_32_4", 1005);

			dialog.SetTitle(L("Ivona"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_orchard_32_4", 1003)))
				{
					await dialog.Msg(L("Burn the 4 scrolls first. Until I know how fast the black comes back I am guessing, and I would rather not send anybody down a hole on a guess."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's already standing, scroll case slung over one shoulder, waiting for you rather than working*{/}"));
				await dialog.Msg(L("Forty paces down under the searcher gullies there is a cut in the rock, and there is a Zaura sitting in it, bleeding into the water table."));
				await dialog.Msg(L("That is 2 years of demon blood in everything that grows here and everything that walks on it. Kill 25 Ferret Searchers off the gully mouth so it cannot use them, then go down and finish it."));

				var response = await dialog.Select(L("Will you go down the cut?"),
					Option(L("I'll kill the Zaura"), "help"),
					Option(L("Why is it just sitting there?"), "info"),
					Option(L("Seal the cut instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It fights in the water and the water is its. Get it onto the dry shelf at the cut mouth - it is slow to follow and it does not like the change."));
						break;

					case "info":
						await dialog.Msg(L("Because it is not hunting. It is watering. Two years of Searchers digging up saplings and putting them down on bare rock, and I have finally understood that it was clearing ground."));
						await dialog.Msg(L("Something down there wants nothing growing over it. I have no idea what comes next and I would like the bleeding stopped before I find out."));
						break;

					case "leave":
						await dialog.Msg(L("Seal it and the water table carries the blood anyway. The rock is not the problem. The thing in it is."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearGullies", out var gullyObj)) return;
				if (!quest.TryGetProgress("killZaura", out var bossObj)) return;

				if (gullyObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It is dead and the water in the cut ran clear within the hour. I have never seen anything reverse that fast and I do not entirely trust it."));
					await dialog.Msg(L("Take this from the shrine. Somebody left it 2 years ago with a note asking for their orchard back, and their orchard is back."));

					character.Quests.Complete(questId);
				}
				else if (gullyObj.Done)
				{
					await dialog.Msg(L("The gully mouth is clear. Go down. It will know you are coming the moment you are in the water."));
				}
				else
				{
					await dialog.Msg(L("Too many Searchers at the gully mouth. It will pull them down on top of you the moment you are in the cut."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Sabas found a resin block on the trail stone this morning. Nine years of that, then 2 years of nothing, then a resin block. They are coming back to themselves."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Ceyral Saplings
//-----------------------------------------------------------------------------

public class CeyralSaplingsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1001);
		SetName(L("Ceyral Saplings"));
		SetType(QuestType.Sub);
		SetDescription(L("Ceyral is the only tree that holds the Seir slope when the rains come, and Ferret Searchers have dug up every sapling planted in 2 years - then set them down on bare rock and walked away."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Sapling-Keeper] Ruta"), "f_orchard_32_4");

		AddObjective("collectSaplings", L("Recover Ceyral Saplings from Ferret Searchers"),
			new CollectItemObjective(664104, 10));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(664104, 0.50f, MonsterId.Ferret_Searcher);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(664104, character.Inventory.CountItem(664104), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(664104, character.Inventory.CountItem(664104), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: The Archers on the Trail
//-----------------------------------------------------------------------------

public class TheArchersOnTheTrailQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1002);
		SetName(L("The Archers on the Trail"));
		SetType(QuestType.Sub);
		SetDescription(L("Nine years of quiet trade between the Seir trail and the Ferret folk ended 2 years ago. Their archers now shoot the trail from the canopy and the trail-warden has lost 2 porters."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Trail-Warden] Sabas"), "f_orchard_32_4");

		AddObjective("killArchers", L("Kill Ferret Archers along the Seir trail"),
			new KillObjective(30, new[] { MonsterId.Ferret_Archer }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1003 CLASS: Purifying the Soil
//-----------------------------------------------------------------------------

public class PurifyingTheSoilQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1003);
		SetName(L("Purifying the Soil"));
		SetType(QuestType.Sub);
		SetDescription(L("Four patches in the western gullies run black and grow nothing, and the priest wants to know how fast the black returns after a burning - it is the only way she can measure how deep the source lies."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Priest of Vakarine] Ivona"), "f_orchard_32_4");

		AddObjective("purifyPatches", L("Burn a purification scroll on each of the 4 blackened patches"),
			new VariableCheckObjective("Laima.Quests.f_orchard_32_4.Quest1003.PatchesPurified", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667024, character.Inventory.CountItem(667024), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_orchard_32_4.Quest1003.PatchesPurified");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_32_4.Quest1003.Patch{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667024, character.Inventory.CountItem(667024), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_orchard_32_4.Quest1003.PatchesPurified");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_32_4.Quest1003.Patch{i}");
	}
}

// Quest 1004 CLASS: The Earth Flower
//-----------------------------------------------------------------------------

public class TheEarthFlowerQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1004);
		SetName(L("The Earth Flower"));
		SetType(QuestType.Sub);
		SetDescription(L("An Earth Flower at the head of a Ceyral row makes the whole row take. There has not been one in the Seir gullies for 2 years, but the Ferret Merchants have been hoarding the seeds without ever planting one."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Sapling-Keeper] Ruta"), "f_orchard_32_4");

		AddObjective("killMerchants", L("Kill Ferret Merchants in the gullies"),
			new KillObjective(15, new[] { MonsterId.Ferret_Bearer_Elite }));

		AddObjective("collectSeeds", L("Collect Earth Flower Seeds"),
			new CollectItemObjective(667025, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(667025, 0.45f, MonsterId.Ferret_Bearer_Elite);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667025, character.Inventory.CountItem(667025), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667025, character.Inventory.CountItem(667025), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: What Is Bleeding Under the Gullies
//-----------------------------------------------------------------------------

public class WhatIsBleedingUnderTheGulliesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_32_4", 1005);
		SetName(L("What Is Bleeding Under the Gullies"));
		SetType(QuestType.Sub);
		SetDescription(L("Forty paces below the searcher gullies a Zaura sits in a cut in the rock, bleeding into the water table. Two years of that is in everything that grows here and everything that walks on it."));
		SetLocation("f_orchard_32_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Priest of Vakarine] Ivona"), "f_orchard_32_4");

		AddPrerequisite(new CompletedPrerequisite("f_orchard_32_4", 1003));

		AddObjective("clearGullies", L("Kill Ferret Searchers at the gully mouth"),
			new KillObjective(25, new[] { MonsterId.Ferret_Searcher }));

		AddObjective("killZaura", L("Defeat the Zaura"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Zawra, 1) },
				resetIdent: "clearGullies",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583114, 1)); // Nelajmes Necklace
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
