//--- Melia Script ----------------------------------------------------------
// Uskis Arable Land Quest NPCs
//--- Description -----------------------------------------------------------
// Priest Raeli's seal tower, the bee tree branches it is restored with, and
// Goddess Austeja who comes out once it holds.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai462QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq0101 = new QuestId(16401);
	private readonly static QuestId Mq01 = new QuestId(16400);
	private readonly static QuestId Mq02 = new QuestId(16410);
	private readonly static QuestId Mq03 = new QuestId(16420);
	private readonly static QuestId Mq04 = new QuestId(16430);
	private readonly static QuestId Mq05 = new QuestId(16440);
	private readonly static QuestId Sq01 = new QuestId(16500);
	private readonly static QuestId Sq02 = new QuestId(16510);
	private readonly static QuestId Sq03 = new QuestId(16520);
	private readonly static QuestId Sq04 = new QuestId(16530);
	private readonly static QuestId Sq05 = new QuestId(16540);
	private readonly static QuestId Party100 = new QuestId(50044);
	private readonly static QuestId Party101 = new QuestId(50045);
	private readonly static QuestId Party102 = new QuestId(50046);

	private const int BranchesNeeded = 5;
	private const int PlanksNeeded = 10;

	// The bee trees near the village.
	private readonly static double[,] BeeTrees =
	{
		{ -1638.03, 3754.25 }, { -1587.20, 3779.22 }, { -1484.86, 3875.62 },
		{ -2264.28, 3238.18 }, { -2157.31, 3290.18 },
	};

	private readonly static double[] BeeTreeFacings = { 120, 45, -30, 30, 90 };

	// The piles of oak wood Druva's scarecrow is built from.
	private readonly static double[,] WoodPiles =
	{
		{ -1526.56, 3599.98 }, { -1304.13, 3588.93 }, { -1757.11, 3125.29 },
		{ -1980.49, 3057.13 }, { -1217.58, 3778.56 },
	};

	private readonly static double[] WoodPileFacings = { 100, 60, 120, 40, 190 };

	protected override void Load()
	{
		// Priest Raeli
		//-------------------------------------------------------------------------
		AddNpc(147491, L("Priest Raeli"), "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", -1220.56, 4364.20, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Raeli"));

			if (character.Quests.IsActive(Mq0101) && character.Quests.IsCompletable(Mq0101))
			{
				await dialog.Msg(L("For now, we can compose ourselves a bit."));
				await dialog.Msg(L("Sorry for the late welcome, I am Goddess Austeja's priest Raeli."));
				await dialog.CompleteQuest(Mq0101);
				character.Quests.Start(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("You brought them. Good."));
				await dialog.CompleteQuest(Mq01);
				character.Quests.Start(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Well done."));
				await dialog.Msg(L("It is uncomfortable to use the demons as sacrifice but... we're now reaching the final stage."));
				await dialog.CompleteQuest(Mq02);
				character.Quests.Start(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("This will be enough."));
				await dialog.Msg(L("Please prepare before you restore the seal. It might become a tough battle."));
				await dialog.CompleteQuest(Mq03);
				character.Quests.Start(Mq04);
				return;
			}

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("My word, the monsters were hiding."));
				await dialog.Msg(L("But it's a relief that you are safe."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Party100) && character.Quests.IsCompletable(Party100))
			{
				await dialog.Msg(L("What happened to the seal?"));
				await dialog.Msg(L("I don't know how many times I prayed for you to be safe."));
				await dialog.CompleteQuest(Party100);
				return;
			}

			if (character.Quests.IsActive(Party102) && character.Quests.IsCompletable(Party102))
			{
				await dialog.Msg(L("I am so grateful since you are helping us with the restoration of the seal."));
				await dialog.Msg(L("But I don't know how long this seal would hold up... I hope Austeja comes back soon."));
				await dialog.CompleteQuest(Party102);
				return;
			}

			if (!character.Quests.Has(Mq0101) && character.Quests.MeetsPrerequisites(Mq0101))
			{
				var answer = await dialog.SelectQuestOffer(Mq0101, L("I would briefly explain the situation to you now, but I sense monsters coming this way. We've prepared a guardian stone at the edge of the village for times like this."),
					Option(L("I will activate the Guardian Stone"), "accept"),
					Option(L("About the treatment at Vilna Forest"), "explain"),
					Option(L("Let's just leave it"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("The purification back then?"));
					await dialog.Msg(L("Unlike the other Revelators who succumbed to the evil energy, I purified you in the name of the goddess."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq0101);
					await dialog.Msg(L("I am relieved by your words."));
					await dialog.Msg(L("If we can't hold them back all of Siauliai Woods may be destroyed, so I am counting on you."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("After the evil energy gushed out, plants mutated and enlarged abnormally. I'm worried those mutated plants might harm the villagers."),
					Option(L("I will check it out"), "accept"),
					Option(L("Don't mind it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("That would be so thankful."));
					await dialog.Msg(L("But you don't know how dangerous it could be so be careful."));
					return;
				}
			}

			if (!character.Quests.Has(Party100) && character.Quests.MeetsPrerequisites(Party100))
			{
				var answer = await dialog.SelectQuestOffer(Party100, L("The sealed tower that was protected by the Revelator is being attacked again. But it can't be protected by us and the villagers."),
					Option(L("I will help to protect the sealed tower"), "accept"),
					Option(L("Tell her that it can't be helped"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Party100);
					await dialog.Msg(L("The first seal of the sealed tower is already destroyed by the attacks from the demons."));
					await dialog.Msg(L("The second seal still remains, but I don't know how long we can hold up."));
					return;
				}
			}

			if (!character.Quests.Has(Party101) && character.Quests.MeetsPrerequisites(Party101))
			{
				var answer = await dialog.SelectQuestOffer(Party101, L("Now is the time to restore the first seal. I know it's going to be hard, but I can't seek help for this."),
					Option(L("I will help the rest"), "accept"),
					Option(L("Say farewell"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Party101);
					character.Inventory.Add(ItemId.PARTY_Q10_CRYSTAL, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Are you really going to help? How grateful..."));
					await dialog.Msg(L("First fill the crest of Goddess Austeja with the sacred energy near the altar."));
					return;
				}
			}

			if (character.Quests.IsActive(Mq0101))
			{
				await dialog.Msg(L("I will explain everything to you in detail after you come back."));
				await dialog.Msg(L("Please get rid of the monsters."));
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("We should get this done quickly, before the monsters rush in again."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Obviously, the goddess' powers can only indirectly affect demons."));
				await dialog.Msg(L("You will have to exhaust some of the demon's HP before using a branch on it."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Palama Cliff is the nearest place you can do this at, so please head there."));
				await dialog.Msg(L("The demons will be marching in soon, so please hurry."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Please hurry. I can feel that the seal is weak."));
				await dialog.Msg(L("May you have the goddess' blessings.."));
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("So could it be the evil forces that turned it to become so grotesque?"));
				return;
			}

			if (character.Quests.IsActive(Party100))
			{
				await dialog.Msg(L("The goddess asked us to protect the seal."));
				await dialog.Msg(L("Now, the only one we can trust is you, the Revelator."));
				return;
			}

			if (character.Quests.IsActive(Party101))
			{
				await dialog.Msg(L("Even if you've done the dedication, you can't seal the vicious energy completely."));
				await dialog.Msg(L("This attack will not be the last. I hope the goddess returns fast."));
				return;
			}

			if (character.Quests.IsActive(Party102))
			{
				await dialog.Msg(L("We can't just rely everything on the goddess."));
				await dialog.Msg(L("But I don't know if we could hold up anymore."));
				return;
			}

			await dialog.Msg(L("With the demons unhindered by the weakening seal, and the villagers' thirst for action, we could be inviting disaster."));
		});

		// Goddess Austeja
		//-------------------------------------------------------------------------
		AddConditionalNpc(151041, L("Goddess Austeja"), "SIAULIAI_46_2_AUSTEJA", "f_siauliai_46_2", 1070, 4905, 0, this.IsAustejaPresent, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Goddess Austeja"));
			dialog.SetPortrait("Dlg_port_Austeja2");

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Are you the Revelator who purified Seal Tower?"));
				await dialog.Msg(L("Finally... you're here."));
				await dialog.CompleteQuest(Mq04);
				character.Quests.Start(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Thanks to you, the seal of Uskis Arable Land is now stable."));
				await dialog.Msg(L("However, many other Revelators have lost their consciousness to the evil energy of the seal in Spring Light Woods."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("I see, you're the one the bees have been whispering about. I am Austeja, the goddess of destiny."),
					Option(L("Listen to the story"), "accept"),
					Option(L("Let's rest for a while"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.Quests.CompleteObjective(Mq05, "hearAusteja");
					return;
				}
				return;
			}

			await dialog.Msg(L("The goddess of destiny, standing beside a tower she can no longer hold shut alone."));
		});

		// Seal Tower
		//-------------------------------------------------------------------------
		AddNpc(147414, L("Seal Tower"), "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", 1079, 4705, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Seal Tower"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				if (character.Inventory.CountItem(ItemId.SIAULIAI_46_2_MQ_03_ITEM) == 0)
				{
					await dialog.Msg(L("The tower is cracked through and there is nothing here to mend it with."));
					return;
				}

				var purified = await character.TimeActions.StartAsync(L("Purifying the seal tower..."), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (purified != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.SIAULIAI_46_2_MQ_03_ITEM, 1);
				character.Quests.StartQuestTrack(Mq04);
				return;
			}

			if (character.Quests.IsActive(Party101) && !character.Quests.IsCompletable(Party101))
			{
				await dialog.Msg(L("The scripture is still empty. Fill it at the altar first."));
				return;
			}

			if (character.Quests.IsActive(Party102) && !character.Quests.IsCompletable(Party102))
			{
				var offered = await character.TimeActions.StartAsync(L("Restoring the seal..."), L("Cancel"), "SITABSORB", TimeSpan.FromSeconds(2));

				if (offered != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.PARTY_Q10_CRYSTAL, 1);
				character.Quests.CompleteObjective(Party102, "offerScripture");
				character.ServerMessage(L("The first seal of the Seal Tower is restored!"));
				return;
			}

			if (character.Quests.IsActive(Party101) && character.Quests.IsCompletable(Party101))
			{
				await dialog.Msg(L("The scripture is full, and the tower takes what it carries."));
				await dialog.CompleteQuest(Party101);
				character.Quests.Start(Party102);
				return;
			}

			if (character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Taumas is still standing over the tower."));
				character.Quests.ReplayQuestTrack(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("The tower has been struck at from the outside, and whatever did it has not gone far."),
					Option(L("Check the weakened Seal Tower"), "accept"),
					Option(L("Leave the tower be"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					character.ServerMessage(L("Demon Lord Taumas comes for the tower!"));
					return;
				}
				return;
			}

			await dialog.Msg(L("An obelisk of the goddess, holding shut what was put under the arable land."));
		});

		// Guardian Stone
		//-------------------------------------------------------------------------
		AddNpc(147413, L("Guardian Stone"), "SIAULIAI_46_2_GUARDIAN", "f_siauliai_46_2", -1954, 3248, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Guardian Stone"));

			if (character.Quests.IsActive(Mq0101) && !character.Quests.IsCompletable(Mq0101))
			{
				var started = await character.TimeActions.StartAsync(L("Activating the Guardian Stone..."), L("Cancel"), "SITABSORB", TimeSpan.FromSeconds(2));

				if (started != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq0101);
				return;
			}

			await dialog.Msg(L("A guardian stone of the village, set at the edge of the fields for exactly this."));
		});

		// Farmer Druva
		//-------------------------------------------------------------------------
		AddNpc(20139, L("Farmer Druva"), "SIAULIAI_46_2_SQ_03_NPC", "f_siauliai_46_2", -1152.33, 4832.52, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Farmer Druva"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("You've gathered a lot."));
				await dialog.Msg(L("I think this will be enough."));
				await dialog.CompleteQuest(Sq03);
				character.Quests.Start(Sq04);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("Let me tell you a secret not even the villagers know about."));

				var answer = await dialog.SelectQuestOffer(Sq03, L("I went all the way to Fedimian and bought this expensive charm. They said it will drive away the monsters."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("I knew you'd get it."));
					await dialog.Msg(L("Please get me some planks of Oak Wood from around the village."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("I'd be really rich if things go well this time. Hehe.."));
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("The farm is located on the left of Arkllui Crossroads."));
				await dialog.Msg(L("It's a bit far but that's how I stay low from the villagers."));
				return;
			}

			await dialog.Msg(L("A farmer with a charm from Fedimian and a great deal of confidence in it."));
		});

		// Ruined Bee Farmer Logen
		//-------------------------------------------------------------------------
		AddNpc(147478, L("Ruined Bee Farmer Logen"), "SIAULIAI_46_2_SQ_05_NPC", "f_siauliai_46_2", -1507.17, 4216.36, 91, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Ruined Bee Farmer Logen"));

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("That makes me feel better."));
				await dialog.Msg(L("How I wish they'd all be gone forever."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("It's all gone... I have lost everything... I took care of the beehives for years... and the monsters destroyed everything."),
					Option(L("Comfort him"), "accept"),
					Option(L("Ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("Are you trying to cheer me up?"));
					await dialog.Msg(L("Then nothing would be better than teaching those monsters a lesson in pain."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("I know the priest is right, but I can still rant about it, right?"));
				return;
			}

			await dialog.Msg(L("A beekeeper with no bees, no hives and nothing else he knows how to do."));
		});

		// Giant Mutated Plant
		//-------------------------------------------------------------------------
		AddConditionalNpc(47203, L("Giant Mutated Plant"), "SIAULIAI_46_2_SQ_01_NPC", "f_siauliai_46_2", 1075, 5290, 91, this.IsMutatedPlantStanding, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Giant Mutated Plant"));

			if (character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01))
			{
				var looked = await character.TimeActions.StartAsync(L("Inspecting the plant..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (looked != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Sq01);
				return;
			}

			await dialog.Msg(L("A stand of grass grown to the height of a house, and something inside it is breathing."));
		});

		// Druva's scarecrow
		//-------------------------------------------------------------------------
		AddConditionalNpc(40095, L("Scarecrow Post"), "SIAULIAI_46_2_SQ_04_01", "f_siauliai_46_2", 470, 5734, 90, this.IsScarecrowPostEmpty, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Scarecrow Post"));

			if (character.Quests.IsActive(Sq04) && !character.Quests.IsCompletable(Sq04))
			{
				var raised = await character.TimeActions.StartAsync(L("Setting up the scarecrow..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (raised != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Sq04, "raiseScarecrow");
				character.LookAround();
				character.ServerMessage(L("The scarecrow is up with the charm on it, and the monsters do not react at all."));
				return;
			}

			await dialog.Msg(L("A bare post in the middle of Druva's field, waiting for a scarecrow."));
		});

		AddConditionalNpc(151032, L("Scarecrow"), "SIAULIAI_46_2_SQ_04_01_AFTER", "f_siauliai_46_2", 470, 5734, 91, this.IsScarecrowRaised, async dialog =>
		{
			await dialog.Msg(L("Druva's scarecrow, with a Fedimian charm nailed to its chest and nothing keeping away from it."));
		});

		// Bee Tree Branches
		//-------------------------------------------------------------------------
		for (var i = 0; i < BeeTrees.GetLength(0); ++i)
		{
			AddNpc(151023, L("Bee Tree Branch"), i == 0 ? "SIAULIAI_46_2_BEETREE" : "SIAULIAI_46_2_BEETREE_" + (i + 1), "f_siauliai_46_2",
				BeeTrees[i, 0], BeeTrees[i, 1], BeeTreeFacings[i], this.CutBeeTreeBranch);
		}

		// Piles of wood
		//-------------------------------------------------------------------------
		for (var i = 0; i < WoodPiles.GetLength(0); ++i)
		{
			AddNpc(151031, L("Pile of wood"), i == 0 ? "SIAULIAI_46_2_WOODPIECE" : "SIAULIAI_46_2_WOODPIECE_" + (i + 1), "f_siauliai_46_2",
				WoodPiles[i, 0], WoodPiles[i, 1], WoodPileFacings[i], this.SplitOakPlanks);
		}

		// Hidden triggers
		//-------------------------------------------------------------------------
		// Palama Cliff, where the altar fills whatever is carried to it.
		AddQuestTrigger("SIAULIAI_46_2_MQ_03_TRIGGER", "f_siauliai_46_2", -676, 3692, 250, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				character.Quests.CompleteObjective(Mq03, "chargeOrb");
				character.ServerMessage(L("The orb fills with the holy energy of the cliff. Take it back to Raeli."));
			}

			if (character.Quests.IsActive(Party101) && !character.Quests.IsCompletable(Party101))
			{
				character.Quests.CompleteObjective(Party101, "chargeScripture");
				character.ServerMessage(L("The scripture fills with the holy energy of the altar. Offer it to the Seal Tower."));
			}

			await Task.CompletedTask;
		});

		// Vulvini Farm and the fields beyond it, where the branches are used.
		AddQuestTrigger("SIAULIAI_46_2_MQ_02_AREA", "f_siauliai_46_2", -1425, 3555, 400, this.BurnDemonsWithBranches);
		AddQuestTrigger("SIAULIAI_46_2_MQ_02_AREA_2", "f_siauliai_46_2", -457, 4259, 400, this.BurnDemonsWithBranches);
	}

	/// <summary>
	/// Burns the demons of the arable land with a bee tree branch.
	/// </summary>
	/// <param name="args"></param>
	private async Task BurnDemonsWithBranches(TriggerActorArgs args)
	{
		if (args.Initiator is not Character character)
			return;

		if (!character.Quests.IsActive(Mq02) || character.Quests.IsCompletable(Mq02))
			return;

		if (character.Inventory.CountItem(ItemId.SIAULIAI_46_2_MQ_01_ITEM) < BranchesNeeded)
			return;

		character.Inventory.Add(ItemId.SIAULIAI_46_2_MQ_02_ITEM, BranchesNeeded, InventoryAddType.PickUp);
		character.ServerMessage(L("The branches burn through the demons and leave their ashes behind."));

		await Task.CompletedTask;
	}

	/// <summary>
	/// Returns whether Goddess Austeja has come out to the seal tower.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAustejaPresent(Character character)
		=> character.Quests.IsActive(Mq04) || character.Quests.Has(Mq05);

	/// <summary>
	/// Returns whether the giant mutated plant is still standing.
	/// </summary>
	/// <param name="character"></param>
	private bool IsMutatedPlantStanding(Character character)
		=> !character.Quests.HasCompleted(Sq01);

	/// <summary>
	/// Returns whether Druva's scarecrow post is still bare.
	/// </summary>
	/// <param name="character"></param>
	private bool IsScarecrowPostEmpty(Character character)
		=> !character.Quests.IsCompletable(Sq04) && !character.Quests.HasCompleted(Sq04);

	/// <summary>
	/// Returns whether Druva's scarecrow has been put up.
	/// </summary>
	/// <param name="character"></param>
	private bool IsScarecrowRaised(Character character)
		=> character.Quests.IsCompletable(Sq04) || character.Quests.HasCompleted(Sq04);

	/// <summary>
	/// Cuts a branch off one of the bee trees near the village.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task CutBeeTreeBranch(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Bee Tree Branch"));

		if (!character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("A bee tree of the arable land, and the bees still work it."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.SIAULIAI_46_2_MQ_01_ITEM) >= BranchesNeeded)
		{
			await dialog.Msg(L("You have as many branches as Raeli asked for."));
			return;
		}

		var cut = await character.TimeActions.StartAsync(L("Cutting the branch..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(2));

		if (cut != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.SIAULIAI_46_2_MQ_01_ITEM, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("The branch comes away clean, and the cut smells of honey."));
	}

	/// <summary>
	/// Splits oak planks off one of the piles of wood near the village.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task SplitOakPlanks(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Pile of wood"));

		if (!character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("A pile of oak, cut and stacked and never used."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.SIAULIAI_46_2_SQ_03_ITEM) >= PlanksNeeded)
		{
			await dialog.Msg(L("You have as many planks as Druva asked for."));
			return;
		}

		var split = await character.TimeActions.StartAsync(L("Splitting the planks..."), L("Cancel"), "HAMMERING", TimeSpan.FromSeconds(2));

		if (split != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.SIAULIAI_46_2_SQ_03_ITEM, 2, InventoryAddType.PickUp);
		await dialog.Msg(L("You split a couple of planks off the pile."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 16401: Secret of the Farmland (1)
//-----------------------------------------------------------------------------
public class Siauliai462Mq0101Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16401);
		SetName(L("Secret of the Farmland (1)"));
		SetDescription(L("The village keeps a guardian stone at the edge of the fields for the nights the demons come."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("It seems that Priest Raeli has something to say. Meet Priest Raeli."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_GUARDIAN", "f_siauliai_46_2", L("Activate the Guardian Stone"), L("Priest Raeli told you that more demons are incoming. Activate the Guardian Stone and stop the demons."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("Stopped the monster attack. Return to Priest Raeli."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_2_MQ_01_01_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(16240, QuestStatus.Completed));

		AddObjective("killSurge", L("Defeat the surging monsters"), new KillObjective(8, "chupaluka", "mushroom_ent_black") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 16400: Secret of the Farmland (2)
//-----------------------------------------------------------------------------
public class Siauliai462Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16400);
		SetName(L("Secret of the Farmland (2)"));
		SetDescription(L("The bee trees near the village carry branches the goddess' power will hold."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("Priest Raeli seems to be quite angry. Meet Priest Raeli."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_BEETREE", "f_siauliai_46_2", L("Collect Bee Tree Branches"), L("To quell a possible insurgency of the villagers, Priest Raeli requested you obtain branches of bee trees. Collect the branches of bee trees near the village."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("You've collected enough branches. Return to Priest Raeli."));

		AddPrerequisite(new QuestStatusPrerequisite(16401, QuestStatus.Completed));

		AddObjective("collectBranches", L("Collect Bee Tree Branches"), new CollectItemObjective("SIAULIAI_46_2_MQ_01_ITEM", 5));

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 16410: Power Within the Bee Tree Branch
//-----------------------------------------------------------------------------
public class Siauliai462Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16410);
		SetName(L("Power Within the Bee Tree Branch"));
		SetDescription(L("A branch driven into a weakened demon burns it to ash, and the ash is what the orb is made of."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("Ask Priest Raeli what to do with the branches."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_MQ_02_AREA", "f_siauliai_46_2", L("Defeat the demons with the branches and collect the ashes of the demons"), L("Priest Raeli told you to use the branches to burn demons when their HP is below half, using the power of the Goddess Austeja, so you can get ashes from the demons."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("You've collected the ashes of the demons. Talk to Priest Raeli."));

		AddPrerequisite(new QuestStatusPrerequisite(16400, QuestStatus.Completed));

		AddObjective("collectAshes", L("Collect the Ashes of the Demons"), new CollectItemObjective("SIAULIAI_46_2_MQ_02_ITEM", 5));

		AddReward(new ItemReward("expCard9", 1));
		AddReward(new TakeItemReward("SIAULIAI_46_2_MQ_01_ITEM"));
		AddReward(new TakeItemReward("SIAULIAI_46_2_MQ_02_ITEM"));
	}
}

// 16420: Land Bestowed with the Goddess' Power (1)
//-----------------------------------------------------------------------------
public class Siauliai462Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16420);
		SetName(L("Land Bestowed with the Goddess' Power (1)"));
		SetDescription(L("The orb of ash has to be filled at Palama Cliff before it is worth anything."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("Priest Raeli made the orb using the ashes of the demons. Ask her what to do with the orb."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_MQ_03_TRIGGER", "f_siauliai_46_2", L("Recharge the orb made of ash with holy energy"), L("The land Goddess Austeja used to oversee is full of holy energy. Go to Palama Cliff and put the orb made of ashes near Austeja's altar to recharge its energy."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("You've filled the orb made of ash with holy energy. Return to Priest Raeli."));

		AddPrerequisite(new QuestStatusPrerequisite(16410, QuestStatus.Completed));

		AddObjective("chargeOrb", L("Recharge the orb made of ash with holy energy"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}

	public override void OnStart(Character character, Quest quest)
	{
		base.OnStart(character, quest);

		character.Inventory.Add(ItemId.SIAULIAI_46_2_MQ_03_ITEM, 1, InventoryAddType.PickUp);
	}
}

// 16430: Land Bestowed with the Goddess' Power (2)
//-----------------------------------------------------------------------------
public class Siauliai462Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16430);
		SetName(L("Land Bestowed with the Goddess' Power (2)"));
		SetDescription(L("The seal tower at Stulr Road is restored with the filled orb, and the goddess comes out to it."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("The orb made of ash has gathered enough holy energy. Ask Priest Raeli what to do next with this."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", L("Restore the weakened seal tower"), L("Restore the weakened seal tower at Stulr Road. Demons may be there to break the seal, so you should hurry."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_AUSTEJA", "f_siauliai_46_2", L("Talk to Goddess Austeja"), L("As you restored the seal tower, Goddess Austeja has appeared. Listen to Goddess Austeja's story."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_2_MQ_04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(16420, QuestStatus.Completed));

		AddObjective("restoreSeal", L("Restore the weakened seal tower"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 16440: Goddess Austeja's Situation
//-----------------------------------------------------------------------------
public class Siauliai462Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16440);
		SetName(L("Goddess Austeja's Situation"));
		SetDescription(L("Everything at the apiary was the demons working at the seal that holds a Demon Lord under it."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_AUSTEJA", "f_siauliai_46_2", L("Listen to Goddess Austeja's story"), L("Goddess Austeja told you that everything that is occurring at the apiary is the act of the demons that are trying to release the seal that is placed upon the Demon Lord."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_AUSTEJA", "f_siauliai_46_2", L("Listen to Goddess Austeja's story"), L("Goddess Austeja told you that everything that is occurring at the apiary is the act of the demons that are trying to release the seal that is placed upon the Demon Lord."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_AUSTEJA", "f_siauliai_46_2", L("Listen to Goddess Austeja's story"), L("Goddess Austeja told you that everything that is occurring at the apiary is the act of the demons that are trying to release the seal that is placed upon the Demon Lord."));

		AddPrerequisite(new QuestStatusPrerequisite(16430, QuestStatus.Completed));

		AddObjective("hearAusteja", L("Listen to Goddess Austeja's story"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 16500: The identity of the gigantic transformed plant
//-----------------------------------------------------------------------------
public class Siauliai462Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16500);
		SetName(L("The identity of the gigantic transformed plant"));
		SetDescription(L("A stand of grass grew to the height of a house, and something is living inside it."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("Priest Raeli is worrying about something. Ask her what to do."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_SQ_01_NPC", "f_siauliai_46_2", L("Check the large mutant plant"), L("The large plant looks somewhat dangerous. Inspect it."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk with Priest Raeli"), L("The large mutant plant was hiding a Golem. Report about it to Priest Raeli."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_2_SQ_01_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(156));

		AddObjective("killGolem", L("Defeat Golem"), new KillObjective(1, "boss_Golem") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 16510: The Advent of Disaster
//-----------------------------------------------------------------------------
public class Siauliai462Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16510);
		SetName(L("The Advent of Disaster"));
		SetDescription(L("Demon Lord Taumas has come for the weakened Seal Tower himself."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", L("Check the weakened Seal Tower"), L("Check the Seal Tower that was weakened by demon attacks."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", L("Defeat Demon Lord Taumas"), L("Demon Lords have appeared and are aiming for the weakened Seal Tower. Defeat Demon Lord Taumas."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", L("Defeat Demon Lord Taumas"), L("Demon Lords have appeared and are aiming for the weakened Seal Tower. Defeat Demon Lord Taumas."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_2_SQ_02_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(156));

		AddObjective("killTaumas", L("Defeat Demon Lord Taumas"), new KillObjective(1, "boss_Taumas") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Taumas is down and the Seal Tower is still standing."));
		character.Quests.Complete(this.QuestId);
	}
}

// 16520: Scarecrow's Hand (1)
//-----------------------------------------------------------------------------
public class Siauliai462Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16520);
		SetName(L("Scarecrow's Hand (1)"));
		SetDescription(L("Druva wants oak planks for a scarecrow he means to hang a Fedimian charm on."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_SQ_03_NPC", "f_siauliai_46_2", L("Talk to Farmer Druva"), L("Farmer Druva is looking for someone who could help him. Talk to Farmer Druva."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_WOODPIECE", "f_siauliai_46_2", L("Collect Oak Wood Planks"), L("Farmer Druva wants to set the scarecrow that will drive out the monsters. Collect oak wood planks as materials for the scarecrow."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_SQ_03_NPC", "f_siauliai_46_2", L("Talk to Farmer Druva"), L("You have collected all the oak wood planks. Return to Farmer Druva."));

		AddPrerequisite(new LevelPrerequisite(156));

		AddObjective("collectPlanks", L("Collect Oak Wood Planks near the village"), new CollectItemObjective("SIAULIAI_46_2_SQ_03_ITEM", 10));

		AddReward(new ItemReward("expCard9", 1));
		AddReward(new TakeItemReward("SIAULIAI_46_2_SQ_03_ITEM"));
	}
}

// 16530: Scarecrow's Hand (2)
//-----------------------------------------------------------------------------
public class Siauliai462Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16530);
		SetName(L("Scarecrow's Hand (2)"));
		SetDescription(L("The scarecrow goes up on Druva's own field, well out of sight of the village."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_SQ_03_NPC", "f_siauliai_46_2", L("Talk to Farmer Druva"), L("As Farmer Druva requested, you've obtained oak wood planks. Ask him what to do next."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_SQ_04_01", "f_siauliai_46_2", L("Place the scarecrow with the charm attached"), L("Put up Farmer Druva's new scarecrow with his charm at his farm, which is at the left side of Arkllui Crossroads and check the monsters' response to it."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_SQ_04_01", "f_siauliai_46_2", L("Place the scarecrow with the charm attached"), L("Put up Farmer Druva's new scarecrow with his charm at his farm, which is at the left side of Arkllui Crossroads and check the monsters' response to it."));

		AddPrerequisite(new QuestStatusPrerequisite(16520, QuestStatus.Completed));

		AddObjective("raiseScarecrow", L("Place the scarecrow with the charm attached"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// Putting the scarecrow up is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The charm does nothing at all, and Druva is watching from the hill."));
		character.Quests.Complete(this.QuestId);
	}
}

// 16540: Half Honey, Half Monster
//-----------------------------------------------------------------------------
public class Siauliai462Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16540);
		SetName(L("Half Honey, Half Monster"));
		SetDescription(L("Logen lost every hive he had and would like the monsters to hear about it."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_SQ_05_NPC", "f_siauliai_46_2", L("Talk to Ruined Bee Farmer Logen"), L("It seems like Logen is in a bad mood. Ask him what's wrong."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_SQ_05_NPC", "f_siauliai_46_2", L("Defeat the monsters nearby"), L("Bee Farmer Logen says his bee farm had to close down because of the monsters. Enact revenge against the monsters for Logen's sake."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_SQ_05_NPC", "f_siauliai_46_2", L("Talk to Ruined Bee Farmer Logen"), L("Defeated the monsters as Logen requested. Return to Logen."));

		AddPrerequisite(new LevelPrerequisite(156));

		AddObjective("killNearby", L("Defeat the nearby monsters"), new KillObjective(15, "mushroom_ent_black", "zigri_red", "Siaumire", "Big_Siaulamb"));

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 50044: The Sealed Tower of the Goddess (1)
//-----------------------------------------------------------------------------
public class PartyQ100Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50044);
		SetName(L("The Sealed Tower of the Goddess (1)"));
		SetDescription(L("The Seal Tower is under attack again and the village cannot hold it alone."));
		SetType(QuestType.Party);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk to Priest Ramelie"), L("Priest Ramelie is urgently looking for you. Talk to Priest Ramelie about what the problem is."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", L("Protect the Sealed Tower"), L("The demons are going after the Sealed Tower of the Goddess. Protect the Sealed Tower from the demons."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk to Priest Ramelie"), L("You have protected the Sealed Tower. Tell Priest Ramelie about it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PARTY_Q_100_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(156));

		AddObjective("holdTheTower", L("Protect the Sealed Tower"), new ManualObjective());
	}
}

// 50045: The Sealed Tower of the Goddess (2)
//-----------------------------------------------------------------------------
public class PartyQ101Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50045);
		SetName(L("The Sealed Tower of the Goddess (2)"));
		SetDescription(L("Austeja's Scripture is filled at the altar and then offered to the tower."));
		SetType(QuestType.Party);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk to Priest Ramelie"), L("You've protected the Sealed Tower. Talk to Priest Ramelie."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_MQ_03_TRIGGER", "f_siauliai_46_2", L("Charge the Goddess Austeja's Scripture"), L("Please help us restore the first seal. First, fill the Goddess Austeja's Scripture with sacred energy near the altar."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", L("Offer the Goddess Austeja's Scripture to the Sealed Tower"), L("You've filled the Goddess Austeja's Scripture with sacred energy. Offer it to the Sealed Tower."));

		AddPrerequisite(new QuestStatusPrerequisite(50044, QuestStatus.Completed));

		AddObjective("chargeScripture", L("Charge the Goddess Austeja's Scripture"), new ManualObjective());
	}
}

// 50046: The Sealed Tower of the Goddess (3)
//-----------------------------------------------------------------------------
public class PartyQ102Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50046);
		SetName(L("The Sealed Tower of the Goddess (3)"));
		SetDescription(L("The filled scripture goes into the tower and restores the first seal."));
		SetType(QuestType.Party);
		SetLocation("f_siauliai_46_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", L("Offer the Goddess Austeja's Scripture to the Sealed Tower"), L("You've filled the Goddess Austeja's Scripture with sacred energy. Offer it to the Sealed Tower."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_2_SEAL", "f_siauliai_46_2", L("Offer the Goddess Austeja's Scripture to the Sealed Tower"), L("You've filled the Goddess Austeja's Scripture with sacred energy. Offer it to the Sealed Tower."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_2_MQ01_NPC", "f_siauliai_46_2", L("Talk to Priest Ramelie"), L("You've restored the seal. Tell Ramelie about it."));

		AddPrerequisite(new QuestStatusPrerequisite(50045, QuestStatus.Completed));

		AddObjective("offerScripture", L("Offer the Goddess Austeja's Scripture to the Sealed Tower"), new ManualObjective());
	}
}
