//--- Melia Script ----------------------------------------------------------
// East Siauliai Woods Quests
//--- Description -----------------------------------------------------------
// The field quests of East Siauliai Woods, reusing the client's own quest ids
// so they keep the identity the client data knows them by.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

// 1031: The Camp in Danger
//-----------------------------------------------------------------------------
public class SiaulEastCamp4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1031);
		SetName(L("The Camp in Danger"));
		SetDescription(L("Monsters pour out of the eastern woods and threaten the outpost. Knight Ares wants the Poata that came for its young put down."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Move to the eastern woods camp"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Kill the Poata at the camp"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Talk to Knight Ares"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_CAMP4_TRACK", 4000);

		AddPrerequisite(new LevelPrerequisite(2));

		AddObjective("killPoata", L("Kill the Poata at the camp"), new KillObjective(1, "boss_poata"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1032: Threats of the Eastern Woods
//-----------------------------------------------------------------------------
public class SiaulEastReclaim1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1032);
		SetName(L("Threats of the Eastern Woods"));
		SetDescription(L("Knight Ares asks the Revelators to help settle the eastern woods before they can reach the mining village."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Ask Knight Ares about the mining village"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_RECLAIM1", "f_siauliai_2", L("Kill the Pokubu on the farm"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Report to Knight Ares"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_RECLAIM1_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(40010, QuestStatus.Completed));

		AddObjective("killPokubu", L("Kill the Pokubu on the farm"), new KillObjective(4, "Pokubu"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1033: A Border Guard's Request (1)
//-----------------------------------------------------------------------------
public class SiaulEastReclaim2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1033);
		SetName(L("A Border Guard's Request (1)"));
		SetDescription(L("The border guard asks for help against the Chupacabra that threaten the unit."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Hear the guard's plan"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Hunt the Chupacabra"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Report to the border guard"));

		AddPrerequisite(new LevelPrerequisite(6));

		AddObjective("killChupacabra", L("Hunt the Chupacabra"), new KillObjective(7, "Chupacabra_Blue"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1034: A Border Guard's Request (2)
//-----------------------------------------------------------------------------
public class SiaulEastReclaim3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1034);
		SetName(L("A Border Guard's Request (2)"));
		SetDescription(L("The supply depot has been overrun. Clear the Chupacabra out of it."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Hear the guard's explanation"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_RECLAIM3", "f_siauliai_2", L("Retake the supply depot"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Report to the border guard"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_RECLAIM3_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(1033, QuestStatus.Completed));

		AddObjective("killChupacabra", L("Clear the supply depot of Chupacabra"), new KillObjective(10, "Chupacabra_Blue", "Chupacabra_Ibory"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1036: Nothing Goes as Planned (3)
//-----------------------------------------------------------------------------
public class SiaulEastReclaim6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1036);
		SetName(L("Nothing Goes as Planned (3)"));
		SetDescription(L("The supply officer wants the large gray Chupacabra that steals the supplies dealt with."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Hear the supply officer's request"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Kill the large gray Chupacabra"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Report to the supply officer"));

		AddPrerequisite(new QuestStatusPrerequisite(20131, QuestStatus.Completed));

		AddObjective("killElite", L("Kill the large gray Chupacabra"), new KillObjective(7, "Chupacabra_Gray_Elite"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1037: Nothing Goes as Planned (4)
//-----------------------------------------------------------------------------
public class SiaulEastReclaim7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1037);
		SetName(L("Nothing Goes as Planned (4)"));
		SetDescription(L("The Weaver by the lower stream are disrupting the supply route. Clear them out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Talk to the supply officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_RECLAIM7", "f_siauliai_2", L("Move below the supply depot"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Report to the supply officer"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_RECLAIM7_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(1036, QuestStatus.Completed));

		AddObjective("killWeaver", L("Kill the Weaver"), new KillObjective(5, "Weaver"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1038: Ares' Commission (1)
//-----------------------------------------------------------------------------
public class SiaulEastRequest1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1038);
		SetName(L("Ares' Commission (1)"));
		SetDescription(L("Knight Ares wants the cause of the multiplying monsters found. Search the Popolion for a clue."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Talk to Knight Ares"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Find a clue from the Popolion"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Deliver the Bube cloth piece"));

		AddPrerequisite(new QuestStatusPrerequisite(1032, QuestStatus.Completed));

		AddObjective("findClue", L("Find a clue from the Popolion"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new TakeItemReward("SIAUL_EAST_REQUEST1_Blood"));
	}
}

// 1039: Ares' Commission (2)
//-----------------------------------------------------------------------------
public class SiaulEastRequest2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1039);
		SetName(L("Ares' Commission (2)"));
		SetDescription(L("The operations officer suspects the Bube are pushing in from the mining village. Scout the northern woods."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Talk to the operations officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_REQUEST2", "f_siauliai_2", L("Scout the northern area"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Report to the operations officer"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_REQUEST2_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(1038, QuestStatus.Completed));

		AddObjective("killBube", L("Kill the Vubbe Fighter's minions"), new KillObjective(8, "Goblin_Miners", "Popolion_Blue", "Goblin_Spear_Q1", "Goblin_Spear_summon"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1041: A Supply Soldier's Request (1)
//-----------------------------------------------------------------------------
public class SiaulEastRequest4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1041);
		SetName(L("A Supply Soldier's Request (1)"));
		SetDescription(L("The supply soldier needs Weaver Claws for the mining village shipment."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Talk to the supply soldier"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Collect Weaver Claws"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Deliver the Weaver Claws"));

		AddPrerequisite(new LevelPrerequisite(7));

		AddPityDrop(650408, 0.1f, 0, 1, 41280);

		AddObjective("collectClaws", L("Kill Weaver to collect Weaver Claws"), new CollectItemObjective("SIAUL_EAST_REQUEST4_Claw", 6));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new TakeItemReward("SIAUL_EAST_REQUEST4_Claw", 6));
	}
}

// 1042: A Supply Soldier's Request (2)
//-----------------------------------------------------------------------------
public class SiaulEastRequest5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1042);
		SetName(L("A Supply Soldier's Request (2)"));
		SetDescription(L("The supply soldier cannot work with the Pokubu raiding the supplies. Thin them out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Talk to the supply soldier"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Kill the Pokubu"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Talk to the supply soldier"));

		AddPrerequisite(new QuestStatusPrerequisite(1041, QuestStatus.Completed));

		AddObjective("killPokubu", L("Kill the Pokubu"), new KillObjective(12, "Pokubu"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1043: Ares' Commission (3)
//-----------------------------------------------------------------------------
public class SiaulEastRequest6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1043);
		SetName(L("Ares' Commission (3)"));
		SetDescription(L("The scout found the Vubbe Fighter at the Nudegi logging camp. Deal with it before it grows bolder."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER8", "f_siauliai_2", L("Find the scout"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_REQUEST6", "f_siauliai_2", L("Hunt the Vubbe Fighter"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER8", "f_siauliai_2", L("Tell the scout the Vubbe Fighter is dead"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_REQUEST6_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(1039, QuestStatus.Completed));

		AddObjective("killFighter", L("Kill the Vubbe Fighter"), new KillObjective(1, "boss_Goblin_Warrior"));

		AddReward(new ItemReward("expCard1", 2));
	}
}

// 1044: Entering the Mining Village
//-----------------------------------------------------------------------------
public class SiaulEastRequest7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1044);
		SetName(L("Entering the Mining Village"));
		SetDescription(L("The Bube have pushed the refugees back. Clear the monsters chasing them, then speak with Ares again."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Report back to Knight Ares"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Kill the monsters chasing the refugees"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Talk to Knight Ares"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_REQUEST7_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(1043, QuestStatus.Completed));

		AddObjective("killChasers", L("Kill the monsters chasing the refugees"), new KillObjective(7, "Goblin_Spear_Q1", "Goblin_Archer_Q1"));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("TOP01_116", 1));
	}
}

// 4203: Nothing Goes as Planned (1)
//-----------------------------------------------------------------------------
public class Act2Diss1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4203);
		SetName(L("Nothing Goes as Planned (1)"));
		SetDescription(L("Monsters stole the supplies bound for the mining village. Help the supply officer recover them."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Talk to the supply officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Recover the scattered supply crates"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Deliver the supply crates"));

		AddPrerequisite(new LevelPrerequisite(2));

		AddObjective("recoverSupplies", L("Recover the scattered supply crates"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 20131: Nothing Goes as Planned (2)
//-----------------------------------------------------------------------------
public class Act2Diss1_2BossQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20131);
		SetName(L("Nothing Goes as Planned (2)"));
		SetDescription(L("A Tutu ambushes the supply officer. Put it down."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Talk to the supply officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Kill the Tutu"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Talk to the supply officer"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ACT2_DISS1_2_BOSS_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(4203, QuestStatus.Completed));

		AddObjective("killTutu", L("Kill the Tutu"), new KillObjective(1, "boss_tutu"));

		AddReward(new ItemReward("expCard1", 1));
	}
}
