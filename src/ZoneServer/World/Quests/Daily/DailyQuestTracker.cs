//--- Melia Script ----------------------------------------------------------
// Daily Quest Tracker
//--- Description -----------------------------------------------------------
// Generates and displays the character's active daily objectives.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Daily;
using static Melia.Zone.Scripting.Shortcuts;

public class DailyQuestTrackerQuest : QuestScript
{
	public const string QuestNamespace = "daily_quests";
	public const int QuestNumber = 1001;

	public static readonly QuestId TrackerQuestId =
		new QuestId(
			QuestNamespace,
			QuestNumber);

	protected override void Load()
	{
		this.SetId(
			QuestNamespace,
			QuestNumber);

		this.SetName(
			L("Daily Quests"));

		this.SetType(
			QuestType.Repeat);

		this.SetDescription(
			L(
				"Complete the daily objectives selected through Vitor, " +
				"the Daily Quest Manager in Klaipeda."
			));

		this.SetLocation(
			"c_Klaipe");

		this.SetAutoTracked(
			true);

		this.SetReceive(
			QuestReceiveType.Manual);

		this.SetCancelable(
			false);

		this.SetUnlock(
			QuestUnlockType.AllAtOnce);

		this.AddQuestGiver(
			L("[Daily Quest Manager] Vitor"),
			"c_Klaipe");
	}

	public QuestData CreateQuestData(Character character)
	{
		var manager = new DailyQuestManager();

		if (!manager.HasChosenDifficulty(character))
			return null;

		var difficulty =
			manager.GetDifficulty(character);

		var monsterQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Monster);

		var eliteQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Elite);

		var bossQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Boss);

		var mythicQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Mythic);

		var dungeonQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Dungeon);

		var mapQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Map);

		var raceQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Race);

		var attributeQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Attribute);

		var sizeQuest =
			manager.GetQuest(
				character,
				DailyQuestType.Size);

		if (monsterQuest == null ||
			eliteQuest == null ||
			bossQuest == null ||
			mythicQuest == null ||
			dungeonQuest == null ||
			mapQuest == null ||
			raceQuest == null ||
			attributeQuest == null ||
			sizeQuest == null)
		{
			return null;
		}

		if (monsterQuest.Target <= 0 ||
			eliteQuest.Target <= 0 ||
			bossQuest.Target <= 0 ||
			mythicQuest.Target <= 0 ||
			dungeonQuest.Target <= 0 ||
			mapQuest.Target <= 0 ||
			raceQuest.Target <= 0 ||
			attributeQuest.Target <= 0 ||
			sizeQuest.Target <= 0)
		{
			return null;
		}

		var data = new QuestData
		{
			Id = TrackerQuestId,

			Name = L("Daily Quests"),

			Description = L("Complete daily objectives selected through Vitor, the Daily Quest Manager in Klaipeda."),

			Type = QuestType.Repeat,

			Location = "c_Klaipe",

			QuestGiverLocation = "c_Klaipe",

			Cancelable = false,

			AutoTrack = true,

			UnlockType = QuestUnlockType.AllAtOnce,

			ReceiveType = QuestReceiveType.Manual,
		};

		data.Objectives.Add(this.CreateMonsterObjective(monsterQuest.Target));

		data.Objectives.Add(this.CreateEliteObjective(eliteQuest.Target));

		data.Objectives.Add(this.CreateBossObjective(bossQuest.Target));

		data.Objectives.Add(this.CreateMythicObjective(mythicQuest.Target));

		data.Objectives.Add(this.CreateDungeonObjective(dungeonQuest.Target));

		if (mapQuest != null && mapQuest.Target > 0 && !string.IsNullOrWhiteSpace(mapQuest.TargetMapClassName))
		{
			data.Objectives.Add(this.CreateMapObjective(mapQuest));
		}

		data.Objectives.Add(this.CreateRaceObjective(raceQuest));

		data.Objectives.Add(this.CreateAttributeObjective(attributeQuest));

		if (sizeQuest != null && sizeQuest.Target > 0)
			data.Objectives.Add(this.CreateSizeObjective(sizeQuest));

		return data;
	}

	private DailyKillObjective CreateMonsterObjective(
		int targetCount)
	{
		return new DailyKillObjective(
			DailyQuestType.Monster,
			targetCount,
			(mob, character) =>
			{
				if (mob.IsMythicMonster())
					return false;

				if (mob.Rank == MonsterRank.Boss)
					return false;

				if (mob.Rank == MonsterRank.Elite)
					return false;

				if (mob.IsBuffActive(BuffId.EliteMonsterBuff))
					return false;

				return true;
			})
		{
			Id = 0,
			Ident = "dailyMonsters",
			Text = L("Defeat Monsters"),
		};
	}

	private DailyKillObjective CreateEliteObjective(
		int targetCount)
	{
		return new DailyKillObjective(
			DailyQuestType.Elite,
			targetCount,
			(mob, character) =>
			{
				if (mob.IsMythicMonster())
					return false;

				return
					mob.Rank == MonsterRank.Elite ||
					mob.IsBuffActive(BuffId.EliteMonsterBuff);
			})
		{
			Id = 1,
			Ident = "dailyElites",
			Text = L("Defeat Elite Monsters"),
		};
	}

	private DailyKillObjective CreateBossObjective(
		int targetCount)
	{
		return new DailyKillObjective(
			DailyQuestType.Boss,
			targetCount,
			(mob, character) =>
			{
				if (mob.IsMythicMonster())
					return false;

				return mob.Rank == MonsterRank.Boss;
			})
		{
			Id = 2,
			Ident = "dailyBosses",
			Text = L("Defeat Boss Monsters"),
		};
	}

	private DailyKillObjective CreateMythicObjective(
		int targetCount)
	{
		return new DailyKillObjective(
			DailyQuestType.Mythic,
			targetCount,
			(mob, character) =>
			{
				return mob.IsMythicMonster();
			})
		{
			Id = 3,
			Ident = "dailyMythics",
			Text = L("Defeat Mythic Monsters"),
		};
	}

	private DailyDungeonObjective CreateDungeonObjective(
		int targetCount)
	{
		return new DailyDungeonObjective(
			targetCount)
		{
			Id = 4,
			Ident = "dailyDungeons",
			Text = L("Complete Dungeons"),
		};
	}

	private DailyKillObjective CreateMapObjective(
		DailyQuestDefinition quest)
	{
		if (quest == null)
			throw new ArgumentNullException(nameof(quest));

		if (quest.Target <= 0)
		{
			throw new InvalidOperationException(
					$"Invalid daily map quest definition. " +
					$"Target must be greater than zero, but was {quest.Target}. " +
					$"Map: '{quest.TargetMapClassName}'.");
		}

		if (string.IsNullOrWhiteSpace(
				quest.TargetMapClassName))
		{
			throw new InvalidOperationException(
					"Invalid daily map quest definition. " +
					"TargetMapClassName is empty.");
		}

		return new DailyKillObjective(
				DailyQuestType.Map,
				quest.Target,
				(mob, character) =>
						string.Equals(
								mob.Map.ClassName,
								quest.TargetMapClassName,
								StringComparison.OrdinalIgnoreCase))
		{
			Id = 5,
			Ident = "dailyMap",
			Text = L(
				"Defeat monsters in " +
				DailyQuestDisplayNames.GetMapDisplayName(
					quest.TargetMapClassName)),
		};
	}

	private DailyKillObjective CreateRaceObjective(
		DailyQuestDefinition quest)
	{
		return new DailyKillObjective(
			DailyQuestType.Race,
			quest.Target,
			(mob, character) =>
				(int)mob.Race == quest.TargetRace)
		{
			Id = 6,
			Ident = "dailyRace",
			Text = L(
				"Defeat " +
				DailyQuestDisplayNames.GetRaceDisplayName(
					quest.TargetRace) +
				" monsters"),
		};
	}

	private DailyKillObjective CreateAttributeObjective(
		DailyQuestDefinition quest)
	{
		return new DailyKillObjective(
			DailyQuestType.Attribute,
			quest.Target,
			(mob, character) =>
				(int)mob.Attribute == quest.TargetAttribute)
		{
			Id = 7,
			Ident = "dailyAttribute",
			Text = L(
				"Defeat " +
				DailyQuestDisplayNames.GetAttributeDisplayName(
					quest.TargetAttribute) +
				" monsters"),
		};
	}

	private DailyKillObjective CreateSizeObjective(
		DailyQuestDefinition quest)
	{
		return new DailyKillObjective(
			DailyQuestType.Size,
			quest.Target,
			(mob, character) =>
				(int)mob.EffectiveSize == quest.TargetSize)
		{
			Id = 8,
			Ident = "dailySize",
			Text = L(
				"Defeat " +
				DailyQuestDisplayNames.GetSizeDisplayName(
					quest.TargetSize) +
				" monsters"),
		};
	}

	private static string GetMonsterSizeDisplayName(
	int size)
	{
		var sizeType =
			(SizeType)size;

		switch (sizeType)
		{
			case SizeType.VS:
				return "Very Small";

			case SizeType.SS:
				return "Super Small";

			case SizeType.S:
				return "Small";

			case SizeType.M:
				return "Medium";

			case SizeType.L:
				return "Large";

			case SizeType.XL:
				return "Extra Large";

			case SizeType.XXL:
				return "Extra Extra Large";

			default:
				return sizeType.ToString();
		}
	}

	private static string GetMapDisplayName(
		string mapClassName)
	{
		if (string.IsNullOrWhiteSpace(
			mapClassName))
		{
			return "Unknown Map";
		}

		var normalized =
			mapClassName
				.Trim()
				.ToLowerInvariant();

		switch (normalized)
		{
			case "c_klaipeda":
			case "c_klaipe":
				return "Klaipeda";

			case "c_orsha":
				return "Orsha";

			case "c_fedimian":
				return "Fedimian";
		}

		if (normalized.StartsWith("f_") ||
			normalized.StartsWith("c_") ||
			normalized.StartsWith("d_"))
		{
			normalized =
				normalized.Substring(2);
		}

		var parts =
			normalized.Split(
				'_',
				System.StringSplitOptions.RemoveEmptyEntries);

		// Remove todos os códigos numéricos encontrados no final.
		while (parts.Length > 1 &&
			int.TryParse(
				parts[parts.Length - 1],
				out _))
		{
			System.Array.Resize(
				ref parts,
				parts.Length - 1);
		}

		for (var i = 0; i < parts.Length; i++)
		{
			if (parts[i].Length == 0)
				continue;

			parts[i] =
				char.ToUpperInvariant(
					parts[i][0]) +
				parts[i]
					.Substring(1)
					.ToLowerInvariant();
		}

		return string.Join(
			" ",
			parts);
	}

	private static string FormatEnumDisplayName(
	string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return "Unknown";

		return value
			.Replace("_", " ")
			.Replace("-", " ");
	}
}
