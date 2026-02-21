using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.World.Dungeons.Stages
{
	public class BossStage : DungeonStage
	{
		public int BossId { get; set; }
		public Position BossSpawnPosition { get; set; }
		public Dictionary<int, Dictionary<Position, int>> SupportMonsters { get; set; }
		public string MapName { get; set; }

		public BossStage(int bossId, Position bossSpawnPosition, Dictionary<int, Dictionary<Position, int>> supportMonsters, DungeonScript dungeonScript, string mapName = null, string stageId = null, string message = null)
			: base(dungeonScript, stageId, message)
		{
			this.BossId = bossId;
			this.MapName = mapName;
			this.BossSpawnPosition = bossSpawnPosition;
			this.SupportMonsters = supportMonsters ?? new Dictionary<int, Dictionary<Position, int>>();
		}

		/// <summary>
		/// Returns the expected number of monsters this stage will spawn.
		/// Includes the boss (1) plus all support monsters.
		/// </summary>
		public override int GetExpectedMonsterCount()
		{
			// Boss counts as 1
			var count = 1;

			// Add support monsters
			if (this.SupportMonsters != null)
			{
				count += SupportMonsters.Sum(kvp => kvp.Value.Sum(posKvp => posKvp.Value));
			}

			return count;
		}

		public override async Task Initialize(InstanceDungeon instance)
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			await base.Initialize(instance);

			if (!DungeonScript.TryGet(instance.Id, out var script)) return;

			// Create objectives for all monsters in the stage.
			var totalSupportMonsters = new Dictionary<int, int>();
			foreach (var kvp in this.SupportMonsters)
			{
				var monsterId = kvp.Key;
				var totalCount = kvp.Value.Sum(posKvp => posKvp.Value);
				if (totalCount > 0)
				{
					totalSupportMonsters[monsterId] = totalCount;
					this.Objectives.Add(new KillObjective(totalCount, monsterId));
				}
			}

			// Add a kill objective for the boss itself.
			this.Objectives.Add(new KillObjective(1, this.BossId) { Text = "Defeat the boss" });

			// Spawn support monsters.
			foreach (var kvp in this.SupportMonsters)
			{
				var monsterId = kvp.Key;
				foreach (var posKvp in kvp.Value)
				{
					for (var i = 0; i < posKvp.Value; i++)
					{
						script.SpawnMonster(instance, monsterId, posKvp.Key, this.MapName);
					}
				}
			}

			// Spawn the boss and hook the death event
			var boss = script.SpawnMonster(instance, this.BossId, this.BossSpawnPosition, this.MapName);
			if (boss != null)
			{
				boss.Died += (deadBoss, killer) =>
				{
					// Call the script's BossKilled method for custom behavior
					script.BossKilled(instance, boss);
				};
			}
		}

		public static BossStage Create(
			DungeonScript dungeonScript,
			string stageId,
			int bossId,
			Position bossSpawnPosition,
			string successStageId,
			string failStageId,
			TimeSpan? timeLimit = null,
			bool allowEscape = true, // Not used by BossStage, but present in generator call
			Dictionary<int, Dictionary<Position, int>> supports = null,
			string message = null)
		{
			var stage = new BossStage(bossId, bossSpawnPosition, supports, dungeonScript, null, stageId, message);

			if (!string.IsNullOrEmpty(successStageId) && successStageId != "TBD")
				stage.OnSuccess(successStageId);

			if (timeLimit.HasValue)
			{
				Func<InstanceDungeon, bool> failCondition = inst =>
				{
					if (!inst.Vars.Has("StageStartTime")) return false;
					var startTime = inst.Vars.Get<DateTime>("StageStartTime");
					return (DateTime.UtcNow - startTime) > timeLimit.Value && !stage.IsObjectiveComplete();
				};
				stage.OnFailure(failStageId, failCondition, inst =>
				{
					inst.Characters.ForEach(c => c?.AddonMessage("NOTICE_Dm_scroll", "Time's up!", 5));
				});
			}

			return stage;
		}
	}
}
