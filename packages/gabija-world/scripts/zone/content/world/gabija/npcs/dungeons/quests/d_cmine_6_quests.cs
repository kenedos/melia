//--- Melia Script ----------------------------------------------------------
// Crystal Mine 3F Quest NPCs
//--- Description -----------------------------------------------------------
// Vaidotas, the villagers the Vubbe took, and the sealed area where Mirtis
// and the slate are waiting.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine6QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Rescue1 = new QuestId(1045);
	private readonly static QuestId Rescue3 = new QuestId(1047);
	private readonly static QuestId Boss = new QuestId(1048);
	private readonly static QuestId Enter = new QuestId(4220);
	private readonly static QuestId Slate = new QuestId(20050);
	private readonly static QuestId Repeat1 = new QuestId(60150);

	private readonly static double[,] CrystalMagicSpots =
	{
		{ -1861, -1691 }, { -1765, -1902 }, { -985, -1393 }, { -730, -816 },
		{ -46, -1201 }, { 118, -1377 }, { -291, -1539 }, { -1985, -1578 },
		{ -505, -587 }, { -737, -233 }, { -945, -313 }, { -664, 78 },
		{ -1237, 75 }, { -319, -4 }, { 113, -252 }, { -16, -942 },
		{ -926, -1228 }, { -419, -1358 }, { -537, -1095 },
	};

	protected override void Load()
	{
		// Vaidotas
		//-------------------------------------------------------------------------
		AddNpc(20110, L("[Alchemist Master]{nl}Vaidotas"), "MINE_3_ALCHEMIST", "d_cmine_6", -2181, -1677, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Vaidotas"));
			dialog.SetPortrait("Dlg_port_ALCHEMIST_1");

			if (!character.Quests.Has(Rescue1) && character.Quests.MeetsPrerequisites(Rescue1))
			{
				await dialog.Msg(L("It's a single road from here on so you'll be able to find the people easily."));

				var answer = await dialog.SelectQuestOffer(Rescue1, L("The villagers the Vubbe took are trapped somewhere on this floor."),
					Option(L("Rescue the villagers and then search for the Light of Salvation"), "accept"),
					Option(L("Quit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Rescue1);
					await dialog.Msg(L("I knew you would."));
					await dialog.Msg(L("One more thing, the ground near the Closed Area is weak so the entrance has been blocked for a long time."));
				}

				return;
			}

			if (character.Quests.IsActive(Rescue1))
			{
				await dialog.Msg(L("One thing, there's something that keeps bothering me."));
				await dialog.Msg(L("A vague feeling of something alien."));
				return;
			}

			await dialog.Msg(L("The air is breathable down here at last. Whatever is sealed below is another matter."));
		});

		// Miner
		//-------------------------------------------------------------------------
		AddNpc(20150, L("Miner"), "MINE_3_RESIENT1", "d_cmine_6", -1188, 104, 28, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Miner"));

			if (character.Quests.IsActive(Rescue1))
			{
				if (!character.Quests.IsCompletable(Rescue1))
				{
					await dialog.Msg(L("The Crystal Spiders are still on us. Drive them off first."));
					character.Quests.ClearQuestTrack(Rescue1);
					return;
				}

				await dialog.Msg(L("Thank you for saving us."));
				await dialog.Msg(L("It's like a dream to be saved by the Revelator."));
				await dialog.CompleteQuest(Rescue1);
				return;
			}

			await dialog.Msg(L("We will find our own way out from here. Go on ahead."));
		});

		// Girl
		//-------------------------------------------------------------------------
		AddNpc(47236, L("Girl"), "MINE_3_GIRL", "d_cmine_6", -1207, 87, 72, async dialog =>
		{
			await dialog.Msg(L("I want to go home. Is the road outside safe now?"));
		});

		// Village Aunt
		//-------------------------------------------------------------------------
		AddNpc(20114, L("Village Aunt"), "D_CMINE_NPC01", "d_cmine_6", -1184, 111, 77, async dialog =>
		{
			await dialog.Msg(L("They kept us down here in the dark for days. I never want to see a crystal again."));
		});

		// Village Girl
		//-------------------------------------------------------------------------
		AddNpc(147473, L("Village Girl"), "D_CMINE_NPC02", "d_cmine_6", -1200, 98, 5, async dialog =>
		{
			await dialog.Msg(L("You came all the way down here for us? Thank you."));
		});

		// Mine Crystal
		//-------------------------------------------------------------------------
		AddNpc(151013, L("Mine Crystal"), "MINE_3_RESQUE3", "d_cmine_6", -669, -55, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Mine Crystal"));

			if (character.Quests.IsActive(Rescue3))
			{
				if (!character.Quests.IsCompletable(Rescue3))
				{
					await dialog.Msg(L("Netherbovine suddenly appeared and it looks very dangerous. Defeat it."));
					character.Quests.ReplayQuestTrack(Rescue3);
					return;
				}

				await dialog.Msg(L("The Netherbovine is down. Whatever the crystal was calling, it will not come now."));
				await dialog.CompleteQuest(Rescue3);
				return;
			}

			if (!character.Quests.Has(Rescue3) && character.Quests.MeetsPrerequisites(Rescue3))
			{
				var answer = await dialog.SelectQuestOffer(Rescue3, L("The crystal is pulsing. Something in the dark is answering it."),
					Option(L("Touch the crystal"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var looked1047 = await character.TimeActions.StartAsync(L("Looking it over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

					if (looked1047 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Rescue3);
				}

				return;
			}

			await dialog.Msg(L("The crystal sits dull and quiet."));
		});

		// Crystal Wall of the Closed Area
		//-------------------------------------------------------------------------
		AddNpc(151014, L("Barrier Stone of the Closed Area"), "CMINE3_BOSSROOM_OPEN", "d_cmine_6", 129, -112, 4, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Barrier Stone of the Closed Area"));

			if (character.Quests.IsActive(Enter))
			{
				if (!character.Quests.IsCompletable(Enter))
				{
					await dialog.Msg(L("Vubbe Magic Stones are needed to enter the Closed Area. Defeat the Vubbes and collect Vubbe Magic Stones."));
					return;
				}

				await dialog.Msg(L("You press the magic stones into the barrier. The stone blocking the Closed Area crumbles."));
				await dialog.CompleteQuest(Enter);
				return;
			}

			if (!character.Quests.Has(Enter) && character.Quests.MeetsPrerequisites(Enter))
			{
				var answer = await dialog.SelectQuestOffer(Enter, L("The Vubbes are disappearing near the Crystal Wall in the Closed Area. Check the barrier stone of the closed area."),
					Option(L("Examine the barrier stone"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var looked4220 = await character.TimeActions.StartAsync(L("Looking it over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

					if (looked4220 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Enter);
					await dialog.Msg(L("The barrier will only give way to the stones the Vubbe carry."));
				}

				return;
			}

			await dialog.Msg(L("The barrier stone lies in pieces. The Closed Area is open."));
		});

		// Crystal Pillar
		//-------------------------------------------------------------------------
		AddNpc(47233, L("Crystal Pillar"), "CMINE6_TO_KATYN7_1_START", "d_cmine_6", 2048, 1753, 243, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Crystal Pillar"));

			if (character.Quests.IsActive(Slate))
			{
				if (!character.Quests.IsCompletable(Slate))
				{
					await dialog.Msg(L("The pillar is still closing itself around the slate."));
					character.Quests.ReplayQuestTrack(Slate);
					return;
				}

				await dialog.Msg(L("Acquired the Mysterious Slate that was inside the Crystal Pillar."));
				await dialog.Msg(L("Return to Klaipeda and talk to Knight Commander Uska about it."));
				await dialog.CompleteQuest(Slate);
				dialog.HideNPC("CMINE6_TO_KATYN7_1_START");
				return;
			}

			if (character.Quests.IsActive(Boss))
			{
				if (!character.Quests.IsCompletable(Boss))
				{
					await dialog.Msg(L("There were demons in the Closed Area. Defeat Mirtis."));
					character.Quests.ReplayQuestTrack(Boss);
					return;
				}

				await dialog.Msg(L("Mirtis is gone. The pillar has stopped screaming."));
				await dialog.CompleteQuest(Boss);
				return;
			}

			if (!character.Quests.Has(Boss) && character.Quests.MeetsPrerequisites(Boss))
			{
				var answer = await dialog.SelectQuestOffer(Boss, L("Something is sealed in the Crystal Pillar. Examine the pillar."),
					Option(L("Examine the seal"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var looked1048 = await character.TimeActions.StartAsync(L("Looking it over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

					if (looked1048 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Boss);
				}

				return;
			}

			if (!character.Quests.Has(Slate) && character.Quests.HasCompleted(Boss) && character.Quests.MeetsPrerequisites(Slate))
			{
				var answer = await dialog.SelectQuestOffer(Slate, L("There is a huge Crystal Pillar in the closed area. Check out the Crystal Pillar."),
					Option(L("Check the pillar"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var read20050 = await character.TimeActions.StartAsync(L("Reading the notice..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

					if (read20050 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Slate);
				}

				return;
			}

			await dialog.Msg(L("The Crystal Pillar stands cracked and empty."));
		});

		// 3F Purifier
		//-------------------------------------------------------------------------
		AddNpc(151006, L("3F Purifier"), "CMINE6_RP_1_NPC", "d_cmine_6", -79, -990, 65, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("3F Purifier"));

			if (character.Quests.IsActive(Repeat1))
			{
				if (!character.Quests.IsCompletable(Repeat1))
				{
					await dialog.Msg(L("It seems as if the purifier does not have sufficient power. Charge a crystal at the emergency power source."));
					return;
				}

				await dialog.Msg(L("You feed the charged crystal into the purifier. It draws hard and the haze thins."));
				await dialog.CompleteQuest(Repeat1);
				return;
			}

			if (!character.Quests.Has(Repeat1) && character.Quests.MeetsPrerequisites(Repeat1))
			{
				await dialog.Msg(L("Attention! Please read."));
				await dialog.Msg(L("Should the purifier be disabled, toxic substances will fill Crystal Mine 3F."));

				var answer = await dialog.SelectQuestOffer(Repeat1, L("The purifier is running low on power again."),
					Option(L("I'll find the power"), "accept"),
					Option(L("Not really my problem"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Repeat1);

				return;
			}

			await dialog.Msg(L("The 3F Purifier draws steadily."));
		});

		// Crystal Magic
		//-------------------------------------------------------------------------
		for (var i = 0; i < CrystalMagicSpots.GetLength(0); ++i)
		{
			var uniqueName = "CMINE6_RP_1_OBJ_" + (i + 1);

			AddNpc(20025, L("Crystal Magic"), uniqueName, "d_cmine_6", CrystalMagicSpots[i, 0], CrystalMagicSpots[i, 1], 90, async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Crystal Magic"));

				if (character.Quests.IsActive(Repeat1) && !character.Quests.IsCompletable(Repeat1))
				{
					await dialog.Msg(L("The crystal takes the charge and holds it. This will carry the purifier."));
					character.Inventory.Add(ItemId.CMINE6_RP_1_ITEM, 1);
					return;
				}

				await dialog.Msg(L("A seam of crystal, faintly charged."));
			});
		}

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("MINE_3_RESQUE1", "d_cmine_6", -1251, -1730, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Rescue1) && !character.Quests.IsCompletable(Rescue1))
				character.Quests.StartQuestTrack(Rescue1);

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 1045: Rescue the Villagers
//-----------------------------------------------------------------------------
public class Mine3Resque1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1045);
		SetName(L("Rescue the Villagers"));
		SetDescription(L("The villagers the Vubbe dragged off are held somewhere on the third floor. Find them."));
		SetType(QuestType.Main);
		SetLocation("d_cmine_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_3_ALCHEMIST", "d_cmine_6", L("Talk to Vaidotas"));
		SetPhase(QuestStatus.InProgress, "MINE_3_RESIENT1", "d_cmine_6", L("Search for the villagers"));
		SetPhase(QuestStatus.Success, "MINE_3_RESIENT1", "d_cmine_6", L("Talk to the Miner"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "MINE_3_RESQUE1_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(4467, QuestStatus.Completed));

		AddObjective("killSpiders", L("Defeat the Crystal Spiders that suddenly appeared"), new KillObjective(7, "Quartz_weaver") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 3));
		AddReward(new TakeItemReward("CMINE_COMPASS_ITEM"));
	}
}

// 1047: Netherbovine's Attack
//-----------------------------------------------------------------------------
public class Mine3Resque3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1047);
		SetName(L("Netherbovine's Attack"));
		SetDescription(L("A Netherbovine answers the mine crystal and comes up out of the dark."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MINE_3_RESQUE3", "d_cmine_6", L("Defeat Netherbovine"));
		SetPhase(QuestStatus.InProgress, "MINE_3_RESQUE3", "d_cmine_6", L("Defeat Netherbovine"));
		SetPhase(QuestStatus.Success, "MINE_3_RESQUE3", "d_cmine_6", L("Defeat Netherbovine"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "MINE_3_RESQUE3_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(12));

		AddObjective("killBovine", L("Defeat Netherbovine"), new KillObjective(1, "boss_NetherBovine") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 3));
		AddReward(new ItemReward("FOOT02_115", 1));
	}
}

// 1048: Demons of the Closed Area
//-----------------------------------------------------------------------------
public class Mine3BossQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1048);
		SetName(L("Demons of the Closed Area"));
		SetDescription(L("Something is sealed inside the crystal pillar of the closed area, and it is awake."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CMINE6_TO_KATYN7_1_START", "d_cmine_6", L("Examine the suspicious seal"));
		SetPhase(QuestStatus.InProgress, "CMINE6_TO_KATYN7_1_START", "d_cmine_6", L("Defeat Demon Mirtis"));
		SetPhase(QuestStatus.Success, "CMINE6_TO_KATYN7_1_START", "d_cmine_6", L("Defeat Demon Mirtis"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "MINE_3_BOSS_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(12));

		AddObjective("killMirtis", L("Defeat Mirtis"), new KillObjective(1, "boss_mirtis") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 5));
		AddReward(new ItemReward("BRC01_111", 1));
	}
}

// 4220: Crystal Wall of the Closed Area
//-----------------------------------------------------------------------------
public class Act4Mine3EnterQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4220);
		SetName(L("Crystal Wall of the Closed Area"));
		SetDescription(L("The barrier stone will only give way to the magic stones the Vubbe carry."));
		SetType(QuestType.Sub);
		SetLocation("d_cmine_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CMINE3_BOSSROOM_OPEN", "d_cmine_6", L("Check the Crystal Pillar in the Closed Area"));
		SetPhase(QuestStatus.InProgress, "CMINE3_BOSSROOM_OPEN", "d_cmine_6", L("Collect Vubbe Magic Stones"));
		SetPhase(QuestStatus.Success, "CMINE3_BOSSROOM_OPEN", "d_cmine_6", L("Insert Vubbe Magic Stones into the Crystal Wall"));

		SetTrack(QuestStatus.Success, QuestStatus.Success, "ACT4_MINE3_ENTER_TRACK", 2000);

		AddPrerequisite(new LevelPrerequisite(12));

		AddPityDrop("D_Bube_Mane", 1.0f, 0, 1, "bubbe_mage_priest", "GoblinWarrior");

		AddObjective("collectStones", L("Defeat Vubbes and obtain Vubbe Magic Stones"), new CollectItemObjective("D_Bube_Mane", 10));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new TakeItemReward("D_Bube_Mane"));
	}
}

// 20050: Mysterious Slate (1)
//-----------------------------------------------------------------------------
public class Cmine6ToKatyn71Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20050);
		SetName(L("Mysterious Slate (1)"));
		SetDescription(L("The crystal pillar was holding a slate. Knight Commander Uska in Klaipeda may know what it is."));
		SetType(QuestType.Main);
		SetLocation("d_cmine_6");
		SetAutoTracked(true);

		SetPhase(QuestStatus.Possible, "CMINE6_TO_KATYN7_1_START", "d_cmine_6", L("Check the Crystal Pillar in the Closed Area"));
		SetPhase(QuestStatus.InProgress, "CMINE6_TO_KATYN7_1_START", "d_cmine_6", L("Obtained the Mysterious Slate"));
		SetPhase(QuestStatus.Success, "CMINE6_TO_KATYN7_1_START", "d_cmine_6", L("Obtained the Mysterious Slate"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "MINE_3_BOSS_2boss", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(1045, QuestStatus.Completed));

		AddObjective("takeSlate", L("Obtained the Mysterious Slate"), new ManualObjective());

		AddReward(new ItemReward("stonetablet01_noread", 1));
		AddReward(new ItemReward("expCard2", 3));
		AddReward(new StatPointReward(3));
	}
}

// 60150: Dangerous Mine
//-----------------------------------------------------------------------------
public class Cmine6Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60150);
		SetName(L("Dangerous Mine"));
		SetDescription(L("The third floor's purifier is running short of power. Charge a crystal and feed it in."));
		SetType(QuestType.Repeat);
		SetLocation("d_cmine_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CMINE6_RP_1_NPC", "d_cmine_6", L("Check the Purifier in 3F"));
		SetPhase(QuestStatus.InProgress, "CMINE6_RP_1_NPC", "d_cmine_6", L("Collect Crystal Magic"));
		SetPhase(QuestStatus.Success, "CMINE6_RP_1_NPC", "d_cmine_6", L("Supply energy to the 3F Purifier"));

		AddPrerequisite(new LevelPrerequisite(12));

		AddObjective("collectMagic", L("Collect Crystal Magic"), new CollectItemObjective("CMINE6_RP_1_ITEM", 1));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new TakeItemReward("CMINE6_RP_1_ITEM"));
	}
}
