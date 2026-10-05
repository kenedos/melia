using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Daily;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Weekly;
using static Melia.Zone.Scripting.Shortcuts;

public abstract class WeeklyQuestTrackerBase : QuestScript
{
	public abstract string TrackerNamespace { get; }
	public abstract int TrackerNumber { get; }
	public QuestId TrackerId => new(this.TrackerNamespace, this.TrackerNumber);
	protected abstract string TrackerName { get; }
	protected abstract bool Includes(WeeklyQuestDefinition quest);

	protected override void Load()
	{
		this.SetId(this.TrackerNamespace, this.TrackerNumber);
		this.SetName(L(this.TrackerName));
		this.SetType(QuestType.Repeat);
		this.SetDescription(L("Complete weekly objectives delivered by Joao, the Weekly Quest Manager in Klaipeda."));
		this.SetLocation("c_Klaipe");
		this.SetAutoTracked(true);
		this.SetReceive(QuestReceiveType.Manual);
		this.SetCancelable(false);
		this.SetUnlock(QuestUnlockType.AllAtOnce);
		this.AddQuestGiver(L("[Weekly Quest Manager] Joao"), "c_Klaipe");
	}

	public QuestData CreateQuestData(Character character)
	{
		var manager = new WeeklyQuestManager();
		if (!manager.IsActive(character))
			return null;

		var quests = manager.Load(character).Quests.Where(this.Includes).ToArray();
		if (quests.Length == 0 || quests.Any(quest => quest.Target <= 0))
			return null;

		var data = new QuestData
		{
			Id = this.TrackerId,
			Name = L(this.TrackerName),
			Description = L("Complete weekly objectives delivered by Joao, the Weekly Quest Manager in Klaipeda."),
			Type = QuestType.Repeat,
			Location = "c_Klaipe",
			QuestGiverLocation = "c_Klaipe",
			Cancelable = false,
			AutoTrack = true,
			UnlockType = QuestUnlockType.AllAtOnce,
			ReceiveType = QuestReceiveType.Manual,
		};

		for (var i = 0; i < quests.Length; i++)
			data.Objectives.Add(this.CreateObjective(quests[i], i));

		return data;
	}

	private QuestObjective CreateObjective(WeeklyQuestDefinition quest, int id)
	{
		if (quest.Type == WeeklyQuestType.Dungeon)
		{
			return new WeeklyDungeonObjective(quest.Target)
			{
				Id = id,
				Ident = GetIdent(quest.Key),
				Text = L("Complete Dungeons"),
			};
		}

		Func<Melia.Zone.World.Actors.Monsters.Mob, Character, bool> matcher = quest.Type switch
		{
			WeeklyQuestType.Monster => (mob, character) => !mob.IsMythicMonster() && mob.Rank != MonsterRank.Boss && mob.Rank != MonsterRank.Elite && !mob.IsBuffActive(BuffId.EliteMonsterBuff),
			WeeklyQuestType.Elite => (mob, character) => !mob.IsMythicMonster() && (mob.Rank == MonsterRank.Elite || mob.IsBuffActive(BuffId.EliteMonsterBuff)),
			WeeklyQuestType.Boss => (mob, character) => !mob.IsMythicMonster() && mob.Rank == MonsterRank.Boss,
			WeeklyQuestType.Mythic => (mob, character) => mob.IsMythicMonster(),
			WeeklyQuestType.Map => (mob, character) => string.Equals(mob.Map.ClassName, quest.TargetMapClassName, StringComparison.OrdinalIgnoreCase),
			WeeklyQuestType.Race => (mob, character) => (int)mob.Race == quest.TargetRace,
			WeeklyQuestType.Attribute => (mob, character) => (int)mob.Attribute == quest.TargetAttribute,
			WeeklyQuestType.Size => (mob, character) => (int)mob.EffectiveSize == quest.TargetSize,
			_ => throw new ArgumentOutOfRangeException(nameof(quest.Type)),
		};

		return new WeeklyKillObjective(quest.Key, quest.Target, matcher)
		{
			Id = id,
			Ident = GetIdent(quest.Key),
			Text = L(GetObjectiveText(quest)),
		};
	}

	private static string GetIdent(string key) => "weekly_" + key.Replace('.', '_');

	private static string GetObjectiveText(WeeklyQuestDefinition quest)
	{
		return quest.Type switch
		{
			WeeklyQuestType.Monster => "Defeat Monsters",
			WeeklyQuestType.Elite => "Defeat Elite Monsters",
			WeeklyQuestType.Boss => "Defeat Boss Monsters",
			WeeklyQuestType.Mythic => "Defeat Mythic Monsters",
			WeeklyQuestType.Map => "Defeat monsters in " + DailyQuestDisplayNames.GetMapDisplayName(quest.TargetMapClassName),
			WeeklyQuestType.Race => "Defeat " + DailyQuestDisplayNames.GetRaceDisplayName(quest.TargetRace) + " monsters",
			WeeklyQuestType.Attribute => "Defeat " + DailyQuestDisplayNames.GetAttributeDisplayName(quest.TargetAttribute) + " monsters",
			WeeklyQuestType.Size => "Defeat " + DailyQuestDisplayNames.GetSizeDisplayName(quest.TargetSize) + " monsters",
			_ => quest.Type.ToString(),
		};
	}
}

public class WeeklyQuestGeneralTracker : WeeklyQuestTrackerBase
{
	public override string TrackerNamespace => "weekly_quests";
	public override int TrackerNumber => 1002;
	protected override string TrackerName => "Weekly Quests: General";
	protected override bool Includes(WeeklyQuestDefinition quest) => quest.Type is WeeklyQuestType.Monster or WeeklyQuestType.Elite or WeeklyQuestType.Boss or WeeklyQuestType.Mythic or WeeklyQuestType.Dungeon;
}

public class WeeklyQuestRaceTracker : WeeklyQuestTrackerBase
{
	public override string TrackerNamespace => "weekly_quests";
	public override int TrackerNumber => 1003;
	protected override string TrackerName => "Weekly Quests: Races";
	protected override bool Includes(WeeklyQuestDefinition quest) => quest.Type == WeeklyQuestType.Race;
}

public class WeeklyQuestAttributeTracker : WeeklyQuestTrackerBase
{
	public override string TrackerNamespace => "weekly_quests";
	public override int TrackerNumber => 1004;
	protected override string TrackerName => "Weekly Quests: Attributes";
	protected override bool Includes(WeeklyQuestDefinition quest) => quest.Type == WeeklyQuestType.Attribute;
}

public class WeeklyQuestSizeTracker : WeeklyQuestTrackerBase
{
	public override string TrackerNamespace => "weekly_quests";
	public override int TrackerNumber => 1005;
	protected override string TrackerName => "Weekly Quests: Sizes";
	protected override bool Includes(WeeklyQuestDefinition quest) => quest.Type == WeeklyQuestType.Size;
}

public class WeeklyQuestRegionTracker : WeeklyQuestTrackerBase
{
	public override string TrackerNamespace => "weekly_quests";
	public override int TrackerNumber => 1006;
	protected override string TrackerName => "Weekly Quests: Regions";
	protected override bool Includes(WeeklyQuestDefinition quest) => quest.Type == WeeklyQuestType.Map;
}
