//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods Quests
//--- Description -----------------------------------------------------------
// The field quests of West Siauliai Woods, reusing the client's own quest ids
// so they keep the identity the client data knows them by.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

// 1001: To Knight Titas (1)
//-----------------------------------------------------------------------------
public class SiaulWestMeetTitasQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1001);
		SetName(L("To Knight Titas (1)"));
		SetDescription(L("Ask the sentry the way to Klaipeda, then report to Knight Titas at the West Forest camp."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		// The client's StartNPC is an auto-grant trigger; the Sentry is who the player talks to.
		SetPhase(QuestStatus.Possible, "SIAU_FRON_NPC_01", "f_siauliai_west", L("Talk to the Sentry"));
		SetPhase(QuestStatus.InProgress, "SIAU_FRON_NPC_01", "f_siauliai_west", L("Talk to the Sentry"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Talk to Knight Titas at the West Forest camp"));

		// The arrival cutscene completes the objective; the turn-in is at
		// Knight Titas.
		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU_WEST_START_TRACK", 1000, autoStart: false);

		AddObjective("meetTitas", L("Talk to Knight Titas at the West Forest camp"), new ManualObjective());
	}
}

// 1002: To Knight Titas (2)
//-----------------------------------------------------------------------------
public class SiaulWestWestForestQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1002);
		SetName(L("To Knight Titas (2)"));
		SetDescription(L("Knight Titas asks you to carry the assembly order to his soldiers before you leave for Klaipeda."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Talk to Knight Titas"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Talk to Knight Titas again"));

		AddPrerequisite(new QuestStatusPrerequisite(1001, QuestStatus.Completed));

		AddObjective("acceptOrder", L("Talk to Knight Titas again"), new ManualObjective());

		AddReward(new ItemReward("Drug_HP1_Q", 3));
	}
}

// 1003: To the Scout (1)
//-----------------------------------------------------------------------------
public class SiaulWestDrasius1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1003);
		SetName(L("To the Scout (1)"));
		SetDescription(L("Give the assembly order to the scout on the western road, and see him through the Kepa that swarm him."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Talk to the Scout at the marked spot"));
		SetPhase(QuestStatus.InProgress, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Kill the swarming Kepa"));
		SetPhase(QuestStatus.Success, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Check on the Scout"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_DRASIUS1_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(1002, QuestStatus.Completed));

		AddObjective("killKepa", L("Kill the swarming Kepa"), new KillObjective(4, "Onion"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 20127: Using Stats
//-----------------------------------------------------------------------------
public class SiaulWestStatusTuto1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20127);
		SetName(L("Using Stats"));
		SetDescription(L("The scout explains that a level up lets you raise one of your stats."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Talk to the Scout"));
		SetPhase(QuestStatus.InProgress, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Open the info window with 'F1' and spend a stat point"));
		SetPhase(QuestStatus.Success, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Talk to the Scout"));

		AddPrerequisite(new QuestStatusPrerequisite(1003, QuestStatus.Completed));

		AddObjective("spendStat", L("Open the info window with 'F1' and spend a stat point"), new VariableCheckObjective(NormalTxFunctionsScript.StatPointsSpentVarName, 1, isPermanent: true));
	}
}

// 1004: To the Scout (2)
//-----------------------------------------------------------------------------
public class SiaulWestDrasius2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1004);
		SetName(L("To the Scout (2)"));
		SetDescription(L("The scout will not fall back until he has his belongings. Take them off the Leaf Bugs that stole them."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Remind the Scout of the order to fall back to Klaipeda"));
		SetPhase(QuestStatus.InProgress, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Kill Leaf Bugs and collect the bundles"));
		SetPhase(QuestStatus.Success, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Hand the bundles to the Scout"));

		AddPrerequisite(new QuestStatusPrerequisite(20127, QuestStatus.Completed));

		AddPityDrop(650405, 0.1f, 10, 1, 401501);

		AddObjective("collectBundles", L("Kill Leaf Bugs to obtain Soldier's Belongings"), new CollectItemObjective("SIAUL_WEST_DRASIUS2_Bag", 5));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("Drug_SP1_Q", 3));
		AddReward(new TakeItemReward("SIAUL_WEST_DRASIUS2_Bag"));
	}
}

// 1013: To Knight Titas (3)
//-----------------------------------------------------------------------------
public class SiaulWestKnightQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1013);
		SetName(L("To Knight Titas (3)"));
		SetDescription(L("The squad leader will pass on the rest of the order himself. Report that to Knight Titas."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Talk to the Squad Leader"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Report to Knight Titas"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Report to Knight Titas"));

		AddPrerequisite(new QuestStatusPrerequisite(1021, QuestStatus.Completed));

		AddObjective("reportToTitas", L("Report to Knight Titas"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new SelectItemReward("SWD01_113", "STF01_113", "TBW01_113", "MAC01_113"));
	}
}

// 1014: To the Searcher
//-----------------------------------------------------------------------------
public class SiaulWestMeetNaglisQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1014);
		SetName(L("To the Searcher"));
		SetDescription(L("Give the assembly order to the searcher up the northern road, and put down the Large Kepa that charges him."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Find the Searcher at the marked spot"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Tell the Searcher his relief has come"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Talk to the Searcher"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_MEET_NAGLIS_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(1004, QuestStatus.Completed));

		AddObjective("killLargeKepa", L("Kill the charging Large Kepa"), new KillObjective(1, "Onion_Big"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1023: A Bad Feeling
//-----------------------------------------------------------------------------
public class SiaulWestOnionBigQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1023);
		SetName(L("A Bad Feeling"));
		SetDescription(L("The searcher is sure another Large Kepa is hiding nearby. Go and find out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Talk to the Searcher"));
		SetPhase(QuestStatus.InProgress, "SIALUL_WEST_ONION_BIG_TRIGGER", "f_siauliai_west", L("Kill the Large Kepa"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Go back to the Searcher"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_ONION_BIG_TRACK", 4000, autoStart: false);

		AddObjective("killHiddenKepa", L("Kill the Large Kepa"), new KillObjective(1, "Onion_Big_Q1"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1015: Laimonas' Favor
//-----------------------------------------------------------------------------
public class SiaulWestLaimonas1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1015);
		SetName(L("Laimonas' Favor"));
		SetDescription(L("Laimonas asks you to pay your respects at the Statue of Goddess Zemyna at the end of the eastern road."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_LAIMONAS", "f_siauliai_west", L("Talk to Laimonas"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_LAIMONAS3_TRIGGER", "f_siauliai_west", L("Pay respects at the goddess statue"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_LAIMONAS", "f_siauliai_west", L("Talk to Laimonas"));

		AddPrerequisite(new QuestStatusPrerequisite(1013, QuestStatus.Completed));

		AddObjective("prayAtStatue", L("Pay respects at the goddess statue"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 20128: The Way Back
//-----------------------------------------------------------------------------
public class SiaulWestLaimonas32Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20128);
		SetName(L("The Way Back"));
		SetDescription(L("A Mushcaria breaks in on your prayer at the statue."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "F_SIAULIAI_WEST_EV_55_001", "f_siauliai_west", L("To the road back"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_LAIMONAS3_2_TRIGGER", "f_siauliai_west", L("Kill the Mushcaria"));
		SetPhase(QuestStatus.Success, "F_SIAULIAI_WEST_EV_55_001", "f_siauliai_west", L("Kill the Mushcaria"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_LAIMONAS3_2_TRACK", 4000, autoStart: false);

		AddObjective("killMushcaria", L("Kill the Mushcaria"), new KillObjective(1, "boss_mushcaria"));

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Drug_SP1_Q", 3));
	}
}

// 1020: To the Squad Leader (1)
//-----------------------------------------------------------------------------
public class SiaulWestSoldier3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1020);
		SetName(L("To the Squad Leader (1)"));
		SetDescription(L("The squad leader will only fall back once his own task is done. Clear the Hanaming around his post."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Give the Squad Leader the order to fall back"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_HANAMING_TRIGGER", "f_siauliai_west", L("Move to where the Hanaming appear"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_HANAMING_TRIGGER", "f_siauliai_west", L("Chase the Hanaming"));

		AddPrerequisite(new QuestStatusPrerequisite(8350, QuestStatus.Completed));

		AddObjective("killHanaming", L("Kill the Hanaming around you"), new KillObjective(8, "Hanaming"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1021: To the Squad Leader (2)
//-----------------------------------------------------------------------------
public class SiaulWestHamingLeafQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1021);
		SetName(L("To the Squad Leader (2)"));
		SetDescription(L("Collect Hanaming Petals for the squad leader's survey so he can leave his post."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_HANAMING_TRIGGER", "f_siauliai_west", L("Move to the Hanaming habitat"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_HANAMING_TRIGGER", "f_siauliai_west", L("Collect Hanaming Petals for the survey"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Hand the Hanaming Petals to the Squad Leader"));

		AddPrerequisite(new QuestStatusPrerequisite(1020, QuestStatus.Completed));

		AddPityDrop(645024, 0.35f, 5, 1, 400941);

		AddObjective("collectPetals", L("Collect Hanaming Petals for the survey"), new CollectItemObjective("leaf_hanaming", 3));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("Drug_STA1_Q", 3));
		AddReward(new TakeItemReward("leaf_hanaming", 3));
	}
}

// 1022: The Start of the Trouble
//-----------------------------------------------------------------------------
public class SiaulWestBossGolemQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1022);
		SetName(L("The Start of the Trouble"));
		SetDescription(L("The squad leader's men are overdue at Delong Rest Stop. Go and look for them."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Talk to the Squad Leader"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_BOSS_GOLEM_TRIGGER", "f_siauliai_west", L("Search the dangerous-looking area"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Report to the Squad Leader"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_BOSS_GOLEM_TRACK", 4000, autoStart: false);

		AddObjective("killGolem", L("Kill the Golem"), new KillObjective(1, "boss_Golem"));

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Drug_Haste1_Q", 3));
	}
}

// 1018: The Road to Klaipeda (1)
//-----------------------------------------------------------------------------
public class SiaulWestLaimonas4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1018);
		SetName(L("The Road to Klaipeda (1)"));
		SetDescription(L("Laimonas asks you to clear the Infrorocktors off the road to Klaipeda."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_LAIMONAS", "f_siauliai_west", L("Talk to Laimonas"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_LAIMONAS", "f_siauliai_west", L("Kill Infrorocktors near the marked area"));
		SetPhase(QuestStatus.Success, "SIAUL_ST1_ST2", "f_siauliai_west", L("Talk to the Klaipeda Guard Captain"));

		AddObjective("killInfrorocktors", L("Kill Infrorocktors"), new KillObjective(7, "InfroRocktor"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1019: The Road to Klaipeda (2)
//-----------------------------------------------------------------------------
public class SiaulWestWoodSpiritQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1019);
		SetName(L("The Road to Klaipeda (2)"));
		SetDescription(L("A Rocktortuga is about to hit the Klaipeda checkpoint. Hold the line with the guards."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_ST1_ST2", "f_siauliai_west", L("Head for Klaipeda"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_ROCK", "f_siauliai_west", L("Kill the Rocktortuga that appeared"));
		SetPhase(QuestStatus.Success, "SIAUL_ST1_ST2", "f_siauliai_west", L("Talk to the Klaipeda Guard Captain"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_WOOD_SPIRIT_TRACK", 4000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(1018, QuestStatus.Completed));

		AddObjective("killRocktortuga", L("Kill the Rocktortuga that appeared"), new KillObjective(1, "boss_Rocktortuga"));

		AddReward(new ItemReward("expCard1", 3));
	}
}

// 9100: Reinforcements
//-----------------------------------------------------------------------------
public class SiaulWestHq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9100);
		SetName(L("Reinforcements"));
		SetDescription(L("Knight Titas sends you to Dvasia Peak, where Julian's unit is opening the road to the Great King's Gate."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west", "d_thorn_22");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Talk to Knight Titas"));
		SetPhase(QuestStatus.InProgress, "THORN22_JULIAN", "d_thorn_22", L("Kill monsters in Dvasia Peak"));
		SetPhase(QuestStatus.Success, "THORN22_JULIAN", "d_thorn_22", L("Report to Commander Julian"));

		AddPrerequisite(new LevelPrerequisite(100));

		AddObjective("clearDvasiaPeak", L("Kill monsters in Dvasia Peak"), new KillObjective(100, "Meleech", "RavineLerva", "TreeGool", "wood_goblin"));
	}
}

// 8350: Let's Learn a Skill
//-----------------------------------------------------------------------------
public class TutoSkillRunQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8350);
		SetName(L("Let's Learn a Skill"));
		SetDescription(L("The searcher points out that your attacks are lacking, and that a skill would serve you better."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Talk to the Searcher"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Learn how to pick up a skill"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Talk to the Searcher"));

		AddPrerequisite(new QuestStatusPrerequisite(1014, QuestStatus.Completed));

		AddObjective("learnSkill", L("Learn how to pick up a skill"), new VariableCheckObjective(NormalTxFunctionsScript.SkillPointsSpentVarName, 1, isPermanent: true));

		AddReward(new ItemReward("Drug_HP1_Q", 1));
	}
}
