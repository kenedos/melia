using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// A stage that presents players with choices that affect the dungeon path.
	/// Example: "Defend the village" vs "Chase the boss" leading to different stages.
	/// </summary>
	public class ConditionalStage : DungeonStage
	{
		private readonly Action<InstanceDungeon, DungeonScript> _initializeAction;
		private readonly Dictionary<string, Func<InstanceDungeon, bool>> _conditionalPaths;

		public ConditionalStage(
			Action<InstanceDungeon, DungeonScript> initializeAction,
			Dictionary<string, Func<InstanceDungeon, bool>> conditionalPaths,
			DungeonScript dungeonScript,
			string stageId = null,
			string message = null)
			: base(dungeonScript, stageId, message)
		{
			_initializeAction = initializeAction;
			_conditionalPaths = conditionalPaths ?? new Dictionary<string, Func<InstanceDungeon, bool>>();
		}

		public override async Task Initialize(InstanceDungeon instance)
		{
			await base.Initialize(instance);
			_initializeAction?.Invoke(instance, this.DungeonScript);
		}

		public override string GetNextStageId(InstanceDungeon instance)
		{
			// Check each conditional path in order of priority
			foreach (var kvp in _conditionalPaths)
			{
				if (kvp.Value(instance))
				{
					return kvp.Key;
				}
			}

			// Fall back to default transition logic
			return base.GetNextStageId(instance);
		}
	}

	/// <summary>
	/// Example usage of conditional stages for branching dungeon paths.
	/// </summary>
	public class BranchingDungeonExample
	{
		public static ConditionalStage CreateDefendOrPursueStage(DungeonScript script)
		{
			var stage = new ConditionalStage(
				initializeAction: (instance, dungeonScript) =>
				{
					// Spawn a defensive objective
					var objective = dungeonScript.SpawnNpc(
						instance,
						123456,
						"Sacred Shrine",
						new Position(0, 0, 0),
						Direction.South
					);

					// Spawn attacking enemies
					dungeonScript.SpawnWave(instance, 999, new Position(100, 0, 100), 10);

					// Give players a choice through UI or NPC dialog
					//instance.BroadcastMessage("Defend the shrine or pursue the fleeing boss?", 10);
					instance.Vars.Set("PlayerChoice", "none");
				},
				conditionalPaths: new Dictionary<string, Func<InstanceDungeon, bool>>
				{
					// Path A: Players chose to defend
					{
						"defend_path_stage",
						inst => inst.Vars.GetString("PlayerChoice") == "defend"
					},
					// Path B: Players chose to pursue
					{
						"pursue_path_stage",
						inst => inst.Vars.GetString("PlayerChoice") == "pursue"
					},
					// Default: Timeout or no choice made
					{
						"default_path_stage",
						inst => inst.Vars.GetString("PlayerChoice") == "none" &&
								inst.GetElapsedTime() > TimeSpan.FromSeconds(30)
					}
				},
				dungeonScript: script,
				stageId: "choice_stage",
				message: "A critical decision awaits!"
			);

			return stage;
		}

		public static ConditionalStage CreatePerformanceBasedBranching(DungeonScript script)
		{
			var stage = new ConditionalStage(
				initializeAction: (instance, dungeonScript) =>
				{
					// Regular stage setup
					instance.Vars.Set("StageStartTime", DateTime.UtcNow);
				},
				conditionalPaths: new Dictionary<string, Func<InstanceDungeon, bool>>
				{
					// S-Rank path: Perfect clear in under 3 minutes
					{
						"srank_bonus_stage",
						inst => inst.InstanceKillPercentage >= 100 &&
								inst.GetElapsedTime() < TimeSpan.FromMinutes(3)
					},
					// A-Rank path: Good clear
					{
						"arank_bonus_stage",
						inst => inst.InstanceKillPercentage >= 80 &&
								inst.GetElapsedTime() < TimeSpan.FromMinutes(5)
					},
					// Standard path: Normal clear
					{
						"standard_path",
						inst => inst.CurrentStage?.IsObjectiveComplete() ?? false
					}
				},
				dungeonScript: script,
				stageId: "performance_check_stage"
			);

			return stage;
		}
	}
}
