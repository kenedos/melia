//--- Melia Script ----------------------------------------------------------
// Klaipeda Quests
//--- Description -----------------------------------------------------------
// The city quests of Klaipeda, reusing the client's own quest ids so they keep
// the identity the client data knows them by.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

// 1027: The Bishop's Dream
//-----------------------------------------------------------------------------
public class KlapedaGoToEastQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1027);
		SetName(L("The Bishop's Dream"));
		SetDescription(L("Knight Commander Uska, who dreamed of the goddess, is looking for the people who came to Klaipeda."));
		SetType(QuestType.Main);
		SetLocation("c_Klaipe");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"));
		SetPhase(QuestStatus.InProgress, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"));
		SetPhase(QuestStatus.Success, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"));

		AddObjective("hearDream", L("Talk to Knight Commander Uska"), new ManualObjective());

		AddReward(new ItemReward("Scroll_Warp_Klaipe", 5));
	}
}

// 20236: The Goddess's Dream (1)
//-----------------------------------------------------------------------------
public class EastPrepareQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20236);
		SetName(L("The Goddess's Dream (1)"));
		SetDescription(L("Knight Commander Uska asks the Revelators to prepare for the journey to the eastern woods."));
		SetType(QuestType.Main);
		SetLocation("c_Klaipe");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KLAPEDA_USKA", "c_Klaipe", L("Move to Klaipeda and talk to Knight Commander Uska"));
		SetPhase(QuestStatus.InProgress, "KLAIPE_AUSRINE_PRAYER", "c_Klaipe", L("Pray at the Statue of Goddess Ausrine"));
		SetPhase(QuestStatus.Success, "EMILIA", "c_Klaipe", L("Talk to the General Merchant"));

		AddPrerequisite(new QuestStatusPrerequisite(1015, QuestStatus.Completed));

		AddObjective("pray", L("Pray at the Statue of Goddess Ausrine"), new ManualObjective());

		AddReward(new ItemReward("Scroll_Warp_quest", 10));
	}
}

// 40010: The Goddess's Dream (2)
//-----------------------------------------------------------------------------
public class EastPrepare1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40010);
		SetName(L("The Goddess's Dream (2)"));
		SetDescription(L("The accessory merchant has been handing out gifts to the Revelators."));
		SetType(QuestType.Main);
		SetLocation("c_Klaipe");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMILIA", "c_Klaipe", L("Talk to the General Merchant"));
		SetPhase(QuestStatus.InProgress, "EMILIA", "c_Klaipe", L("Talk to the General Merchant"));
		SetPhase(QuestStatus.Success, "ALFONSO", "c_Klaipe", L("Talk to the Accessory Merchant"));

		AddPrerequisite(new QuestStatusPrerequisite(20236, QuestStatus.Completed));

		AddObjective("visitRonesa", L("Talk to the Accessory Merchant"), new ManualObjective());

		AddReward(new ItemReward("BRC01_122", 1));
	}
}
