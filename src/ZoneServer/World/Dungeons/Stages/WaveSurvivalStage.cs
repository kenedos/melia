using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.World.Dungeons.Stages
{
	public class WaveSurvivalStage : DungeonStage
	{
		public TimeSpan Duration { get; }
		public List<Wave> Waves { get; }

		public WaveSurvivalStage(TimeSpan duration, List<Wave> waves, DungeonScript dungeonScript, string stageId = null, string message = null)
			: base(dungeonScript, stageId, message)
		{
			this.Duration = duration;
			this.Waves = waves;
		}

		/// <summary>
		/// Returns the expected number of monsters this stage will spawn from all waves.
		/// </summary>
		public override int GetExpectedMonsterCount()
		{
			if (Waves == null)
				return 0;

			return Waves.Sum(w => w.Count);
		}

		public override async Task Initialize(InstanceDungeon instance)
		{
			await base.Initialize(instance);

			var ct = instance.StageCancellationToken;

			// Start all wave spawners concurrently
			foreach (var wave in this.Waves)
			{
				CallSafe(this.SpawnWaveAfterDelay(instance, wave, ct));
			}

			// After the total duration, move to the next stage
			_ = Task.Run(async () =>
			{
				try
				{
					await Task.Delay(this.Duration, ct);
				}
				catch (OperationCanceledException) { return; }

				if (this.Instance?.CurrentStage == this && !this.IsCompleted)
				{
					await this.Instance.MoveToNextStage(this.DungeonScript);
				}
			});
		}

		/// <summary>
		/// Overridden to prevent completion by external events like MonsterKilled.
		/// This stage's completion is handled exclusively by its internal timer.
		/// </summary>
		/// <returns>False, as this stage is not objective-based.</returns>
		public override bool IsObjectiveComplete()
		{
			return false;
		}

		private async Task SpawnWaveAfterDelay(InstanceDungeon instance, Wave wave, CancellationToken ct)
		{
			if (wave.Delay > TimeSpan.Zero)
			{
				try
				{
					await Task.Delay(wave.Delay, ct);
				}
				catch (OperationCanceledException) { return; }
			}

			// Check if the stage is still active before spawning
			if (this.Instance?.CurrentStage != this || this.IsCompleted) return;

			if (DungeonScript.TryGet(instance.Id, out var script))
			{
				for (var i = 0; i < wave.Count; i++)
				{
					// Add slight randomness to position to prevent perfect stacking
					var spawnPos = wave.Position.GetRandomInRange2D(1, 5);
					script.SpawnMonster(instance, wave.MonsterId, spawnPos);
				}
			}
		}
	}
}
