using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Melia.Zone.World.Spawning;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;

/**
public class RandomKillQuestScript : QuestScript
{
	private static readonly Random Random = new();

	private const int MinQuestLevel = 2;
	private const int MaxQuestLevel = 10;

	private const int MinMonsterAmount = 5;
	private const int MaxMonsterAmount = 20;

	private const int MinSilverReward = 100;
	private const int MaxSilverReward = 10000;

	private const int MinExpReward = 50;
	private const int MaxExpReward = 2000;

	private static readonly Dictionary<int, List<MonsterSpawner>> MonsterSpawnsByLevel = new();

	protected override void Load()
	{
		// Cache monster spawns by level for performance
		foreach (var spawn in ZoneServer.Instance.World.Spawns.Where(x => x.Monster != null))
		{
			var monsterLevel = spawn.Monster.Data.Level;
			if (!MonsterSpawnsByLevel.ContainsKey(monsterLevel))
				MonsterSpawnsByLevel[monsterLevel] = new List<MonsterSpawnData>();
			MonsterSpawnsByLevel[monsterLevel].Add(spawn);
		}

		for (var level = MinQuestLevel; level <= MaxQuestLevel; ++level)
		{
			CreateKillQuest(level);
		}
	}

	private void CreateKillQuest(int level)
	{
		var questId = GenerateQuestId(level);
		var monsterToKill = GetRandomMonsterForLevel(level);

		if (monsterToKill == null)
		{
			Log.Warning($"No monster found for level {level}, skipping quest generation.");
			return;
		}

		var amountToKill = Random.Next(MinMonsterAmount, MaxMonsterAmount + 1);
		var silverReward = Random.Next(MinSilverReward, MaxSilverReward + 1) * level;
		var expReward = Random.Next(MinExpReward, MaxExpReward + 1) * level;

		SetId(questId);
		SetName($"Exterminate the {monsterToKill.Data.Name}s (Level {level})");
		SetDescription($"Those pesky {monsterToKill.Data.Name}s are causing trouble again! Please defeat {amountToKill} of them.");

		AddObjective("kill", $"Defeat {amountToKill} {monsterToKill.Data.Name}s", new KillObjective(amountToKill, monsterToKill.Data.Id));

		SetReceive(QuestReceiveType.Auto);
		AddPrerequisite(new LevelPrerequisite(level));

		AddReward(new SilverReward(silverReward));
		AddReward(new ExpReward(expReward, expReward / 2));
	}

	private static int GenerateQuestId(int level)
	{
		// Example: 2000002 for level 2
		return 2000000 + level;
	}

	private static IMonsterBase GetRandomMonsterForLevel(int level)
	{
		// Allow a level variance for more quest variety
		var levelVariance = 1;
		var minLevel = Math.Max(MinQuestLevel, level - levelVariance);
		var maxLevel = Math.Min(MaxQuestLevel, level + levelVariance);

		var eligibleSpawns = MonsterSpawnsByLevel.Where(x => x.Key >= minLevel && x.Key <= maxLevel).SelectMany(x => x.Value).ToList();
		return eligibleSpawns.Count > 0 ? eligibleSpawns.Random().Monster : null;
	}
}
**/
