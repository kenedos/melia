using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// A flexible stage that executes a provided action to perform custom setup,
	/// like spawning monsters with dynamic properties.
	/// </summary>
	public class ActionStage : DungeonStage
	{
		private readonly Func<InstanceDungeon, DungeonScript, Task> _initializeAction;

		/// <summary>
		/// Expected monster count for this stage. If not explicitly set,
		/// this will be auto-calculated from KillObjective counts.
		/// </summary>
		public int ExpectedMonsters { get; set; } = -1;

		public ActionStage(Func<InstanceDungeon, DungeonScript, Task> action, List<QuestObjective> objectives, DungeonScript dungeonScript, string stageId = null, string message = null)
			: base(dungeonScript, stageId, message)
		{
			_initializeAction = action;
			if (objectives != null)
			{
				foreach (var objective in objectives)
				{
					this.Objectives.Add(objective);
				}
			}
		}

		/// <summary>
		/// Creates an ActionStage with a specified expected monster count.
		/// </summary>
		public ActionStage(Func<InstanceDungeon, DungeonScript, Task> action, List<QuestObjective> objectives, int expectedMonsters, DungeonScript dungeonScript, string stageId = null, string message = null)
			: this(action, objectives, dungeonScript, stageId, message)
		{
			this.ExpectedMonsters = expectedMonsters;
		}

		/// <summary>
		/// Returns the expected number of monsters this stage will spawn.
		/// If ExpectedMonsters was not explicitly set, calculates it from KillObjective counts.
		/// </summary>
		public override int GetExpectedMonsterCount()
		{
			if (this.ExpectedMonsters >= 0)
				return this.ExpectedMonsters;

			// Auto-calculate from KillObjective counts
			var count = 0;
			foreach (var objective in this.Objectives)
			{
				if (objective is KillObjective killObjective)
					count += killObjective.TargetCount;
			}
			return count;
		}

		public override async Task Initialize(InstanceDungeon instance)
		{
			await base.Initialize(instance);
			if (_initializeAction != null)
			{
				await _initializeAction(instance, this.DungeonScript);
			}

			// If this stage has no objectives (or all objectives are already complete), 
			// automatically progress to the next stage after the action completes
			if (this.Objectives.Count == 0 || this.IsObjectiveComplete())
			{
				// Ensure we're still the current stage before transitioning
				if (instance.CurrentStage == this && !this.IsCompleted)
				{
					await instance.MoveToNextStage(this.DungeonScript);
				}
			}
		}
	}
}
