using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// A stage that requires killing a specified percentage of monsters to progress.
	/// Default is 70% but can be configured.
	/// </summary>
	public class KillMonstersStage : DungeonStage
	{
		/// <summary>
		/// Default minimum kill percentage required to progress.
		/// Uses config value from instanced_dungeon_min_stage_kill_percentage.
		/// </summary>
		public static float DefaultMinimumKillPercentage => ZoneServer.Instance.Conf.World.InstancedDungeonMinStageKillPercentage / 100f;

		/// <summary>
		/// Monsters to spawn and kill in this stage.
		/// Format: MonsterId -> (Position -> Count)
		/// </summary>
		public Dictionary<int, Dictionary<Position, int>> MonstersToKill { get; set; }

		/// <summary>
		/// Optional map name for spawning (defaults to dungeon's main map).
		/// </summary>
		public string MapName { get; set; }

		/// <summary>
		/// Minimum percentage of monsters that must be killed to progress (0.0 to 1.0).
		/// Default uses config value from instanced_dungeon_min_stage_kill_percentage.
		/// </summary>
		public float MinimumKillPercentage { get; set; }

		/// <summary>
		/// If true, uses exact kill count objectives instead of percentage-based progression.
		/// When false (default), the stage progresses when MinimumKillPercentage is reached.
		/// </summary>
		public bool UseExactKillCount { get; set; } = false;

		/// <summary>
		/// Reference to the percentage objective for tracking kills (when UseExactKillCount is false).
		/// </summary>
		private PercentageKillObjective _percentageObjective;

		/// <summary>
		/// Creates a KillMonstersStage with default minimum kill requirement from config.
		/// </summary>
		public KillMonstersStage(Dictionary<int, Dictionary<Position, int>> monstersToKill, DungeonScript dungeonScript, string mapName = null, string stageId = null, string message = null)
			: base(dungeonScript, stageId, message)
		{
			this.MonstersToKill = monstersToKill;
			this.MapName = mapName;
			this.MinimumKillPercentage = DefaultMinimumKillPercentage;
		}

		/// <summary>
		/// Creates a KillMonstersStage with a custom minimum kill percentage.
		/// </summary>
		public KillMonstersStage(Dictionary<int, Dictionary<Position, int>> monstersToKill, float minimumKillPercentage, DungeonScript dungeonScript, string mapName = null, string stageId = null, string message = null)
			: this(monstersToKill, dungeonScript, mapName, stageId, message)
		{
			this.MinimumKillPercentage = Math.Clamp(minimumKillPercentage, 0.0f, 1.0f);
		}

		/// <summary>
		/// Returns the expected number of monsters this stage will spawn.
		/// </summary>
		public override int GetExpectedMonsterCount()
		{
			if (MonstersToKill == null)
				return 0;

			return MonstersToKill.Sum(kvp => kvp.Value.Sum(posKvp => posKvp.Value));
		}

		/// <summary>
		/// Initialize the stage.
		/// </summary>
		public override async Task Initialize(InstanceDungeon instance)
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			await base.Initialize(instance);

			// Spawn monsters for this stage
			if (!DungeonScript.TryGet(instance.Id, out var script))
			{
				Log.Error("Failed to get dungeon script for instance {InstanceId}.", instance.Id);
				return;
			}

			// Calculate total monsters for this stage
			var totalMonsters = this.MonstersToKill.Sum(kvp => kvp.Value.Sum(posKvp => posKvp.Value));

			// Define objectives based on mode
			if (this.UseExactKillCount)
			{
				// Traditional mode: exact kill objectives per monster type
				foreach (var kvp in this.MonstersToKill)
				{
					var monsterId = kvp.Key;
					var totalAmount = kvp.Value.Sum(posKvp => posKvp.Value);

					if (totalAmount > 0)
					{
						this.Objectives.Add(new KillObjective(totalAmount, monsterId)
						{
							Text = $"Kill {totalAmount} x {this.GetMonsterName(monsterId)}",
						});
					}
				}
			}
			else
			{
				// Percentage-based mode: single objective tracking total kills
				_percentageObjective = new PercentageKillObjective(totalMonsters, this.MinimumKillPercentage)
				{
					Text = $"Defeat enemies ({(int)(this.MinimumKillPercentage * 100)}% required)"
				};
				this.Objectives.Add(_percentageObjective);
			}

			// Spawn all monsters
			foreach (var kvp in this.MonstersToKill)
			{
				var monsterId = kvp.Key;
				foreach (var kvp2 in kvp.Value)
				{
					var amount = kvp2.Value;
					var position = kvp2.Key;
					for (var i = 0; i < amount; i++)
					{
						script.SpawnMonster(instance, monsterId, position, this.MapName, this);
					}
				}
			}
		}

		/// <summary>
		/// Called when a monster in this stage is killed.
		/// Updates the percentage objective if in percentage mode.
		/// </summary>
		public void OnMonsterKilled()
		{
			if (!this.UseExactKillCount && _percentageObjective != null)
			{
				_percentageObjective.RecordKill();
			}
		}

		/// <summary>
		/// Overridden to support percentage-based completion.
		/// </summary>
		public override bool IsObjectiveComplete()
		{
			if (this.UseExactKillCount)
			{
				// Use standard objective completion
				return base.IsObjectiveComplete();
			}

			// Percentage-based completion
			var killPercentage = this.GetDeadMonstersPercentage() / 100.0f;
			return killPercentage >= this.MinimumKillPercentage;
		}

		/// <summary>
		/// Factory method for creating a KillMonstersStage with fluent configuration.
		/// </summary>
		public static KillMonstersStage Create(
			DungeonScript dungeonScript,
			string stageId,
			Dictionary<int, Dictionary<Position, int>> monstersToKill,
			string successStageId,
			string failStageId = null,
			float minimumKillPercentage = -1f,
			TimeSpan? timeLimit = null,
			string message = null)
		{
			// Use default if not specified (negative value indicates use default)
			if (minimumKillPercentage < 0)
				minimumKillPercentage = DefaultMinimumKillPercentage;

			var stage = new KillMonstersStage(monstersToKill, minimumKillPercentage, dungeonScript, null, stageId, message);

			if (!string.IsNullOrEmpty(successStageId) && successStageId != "TBD")
				stage.OnSuccess(successStageId);

			if (timeLimit.HasValue && !string.IsNullOrEmpty(failStageId))
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

		/// <summary>
		/// Returns monster name for a given monsterId.
		/// </summary>
		private string GetMonsterName(int monsterId)
		{
			if (ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
			{
				return monsterData.Name;
			}
			return $"Monster {monsterId}";
		}
	}

	/// <summary>
	/// A quest objective that tracks kills based on percentage rather than exact count.
	/// </summary>
	public class PercentageKillObjective : QuestObjective
	{
		/// <summary>
		/// Total number of monsters in the stage.
		/// </summary>
		public int TotalMonsters { get; }

		/// <summary>
		/// Required kill percentage (0.0 to 1.0).
		/// </summary>
		public float RequiredPercentage { get; }

		/// <summary>
		/// Number of required kills based on total and percentage.
		/// </summary>
		public int RequiredKills => this.TargetCount;

		/// <summary>
		/// Current kill count.
		/// </summary>
		public int CurrentKills => this.Progress;

		public PercentageKillObjective(int totalMonsters, float requiredPercentage)
		{
			this.TotalMonsters = totalMonsters;
			this.RequiredPercentage = requiredPercentage;
			this.TargetCount = (int)Math.Ceiling(totalMonsters * requiredPercentage);
			this.Progress = 0;
		}

		/// <summary>
		/// Records a kill and checks for completion.
		/// </summary>
		public void RecordKill()
		{
			this.IncrementProgress();
		}

		/// <summary>
		/// Gets the current progress as a percentage (0-100).
		/// </summary>
		public int GetProgressPercentage()
		{
			if (this.RequiredKills == 0) return 100;
			return (int)((this.CurrentKills / (float)this.RequiredKills) * 100);
		}
	}
}
