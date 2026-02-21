using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Quests.Objectives;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// A flexible, data-driven stage that executes a predefined timeline of
	/// spawn events. Ideal for complex, scripted encounters like Uphill Defense.
	/// </summary>
	public class TimelineStage : DungeonStage
	{
		private readonly List<SpawnEvent> _timeline;
		private readonly TimeSpan? _stageDuration;

		public TimelineStage(List<SpawnEvent> timeline, DungeonScript dungeonScript, string message, TimeSpan? duration = null)
			: base(dungeonScript, message)
		{
			_timeline = timeline;
			_stageDuration = duration;

			if (_stageDuration.HasValue)
			{
				this.Objectives.Add(new TimeElapsedObjective(_stageDuration.Value));
			}
		}

		/// <summary>
		/// Returns the expected number of monsters this stage will spawn from the timeline.
		/// </summary>
		public override int GetExpectedMonsterCount()
		{
			if (_timeline == null)
				return 0;

			return _timeline.Sum(e => e.Count);
		}

		public override async Task Initialize(InstanceDungeon instance)
		{
			await base.Initialize(instance);

			var ct = instance.StageCancellationToken;

			// Start the timeline execution but don't block the main thread.
			CallSafe(ExecuteTimeline(instance, ct));

			// If the stage has a fixed duration, wait for it to complete.
			if (_stageDuration.HasValue)
			{
				try
				{
					await Task.Delay(_stageDuration.Value, ct);
				}
				catch (OperationCanceledException) { return; }

				if (instance?.CurrentStage == this && !this.IsCompleted)
				{
					// Mark time-based objective as complete and move to next stage.
					foreach (var obj in this.Objectives)
						if (obj is TimeElapsedObjective timeObj) timeObj.SetCompleted();

					await instance.MoveToNextStage(this.DungeonScript);
				}
			}
		}

		private async Task ExecuteTimeline(InstanceDungeon instance, CancellationToken ct)
		{
			foreach (var spawnEvent in _timeline)
			{
				if (spawnEvent.Delay > TimeSpan.Zero)
				{
					try
					{
						await Task.Delay(spawnEvent.Delay, ct);
					}
					catch (OperationCanceledException) { return; }
				}

				// If the stage was completed while waiting, stop the timeline.
				if (this.IsCompleted || instance.CurrentStage != this) return;

				var mobs = this.DungeonScript.SpawnWave(instance, spawnEvent.MonsterId, spawnEvent.Position, spawnEvent.Count, spawnEvent.SpawnRadius, spawnEvent.Properties);

				// Apply dynamic properties after spawning
				foreach (var mob in mobs)
				{
					mob.Level = instance.RankCalculator.GetUphillLevelNormal1();
					if (!string.IsNullOrEmpty(spawnEvent.AiScript))
					{
						mob.Components.Add(new AiComponent(mob, spawnEvent.AiScript));
					}
				}
			}

			// If the stage is not based on a fixed duration, it completes when the timeline is finished.
			if (!_stageDuration.HasValue && instance?.CurrentStage == this && !this.IsCompleted)
			{
				await instance.MoveToNextStage(this.DungeonScript);
			}
		}
	}
}
