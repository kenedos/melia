using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests.Objectives;

namespace Melia.Zone.World.Dungeons.Stages
{
	public class TimedSurvivalStage : DungeonStage
	{
		public TimeSpan Duration { get; set; }
		public Dictionary<int, Dictionary<Position, int>> MonstersToSpawn { get; set; }

		public TimedSurvivalStage(TimeSpan duration, Dictionary<int, Dictionary<Position, int>> monstersToSpawn, DungeonScript dungeonScript, string stageId = null, string message = null)
			: base(dungeonScript, stageId, message)
		{
			this.Duration = duration;
			this.MonstersToSpawn = monstersToSpawn;
		}

		/// <summary>
		/// Returns the expected number of monsters this stage will spawn.
		/// </summary>
		public override int GetExpectedMonsterCount()
		{
			if (MonstersToSpawn == null)
				return 0;

			return MonstersToSpawn.Sum(kvp => kvp.Value.Sum(posKvp => posKvp.Value));
		}

		public override async Task Initialize(InstanceDungeon instance)
		{
			await base.Initialize(instance);

			if (this.MonstersToSpawn != null && DungeonScript.TryGet(instance.Id, out var script))
			{
				foreach (var kvp in this.MonstersToSpawn)
				{
					foreach (var posKvp in kvp.Value)
					{
						for (var i = 0; i < posKvp.Value; i++)
						{
							script.SpawnMonster(instance, kvp.Key, posKvp.Key);
						}
					}
				}
			}

			var timeObjective = new TimeElapsedObjective(this.Duration) { Text = "Survive" };
			this.Objectives.Add(timeObjective);

			// Use a non-blocking delay to complete the objective and trigger a transition check.
			var ct = instance.StageCancellationToken;
			_ = Task.Run(async () =>
			{
				try
				{
					await Task.Delay(this.Duration, ct);
				}
				catch (OperationCanceledException) { return; }

				// After the delay, ensure the stage is still active before proceeding.
				if (this.Instance?.CurrentStage == this && !this.IsCompleted)
				{
					timeObjective.SetCompleted();
					if (this.IsObjectiveComplete())
					{
						await this.Instance.MoveToNextStage(this.DungeonScript);
					}
				}
			});
		}
	}
}
