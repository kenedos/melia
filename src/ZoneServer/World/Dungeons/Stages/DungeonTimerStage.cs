using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// A background stage that monitors dungeon time and ends the dungeon after a time limit.
	/// This is the default dungeon timer behavior and can be customized or disabled per dungeon.
	/// </summary>
	public class DungeonTimerStage : BackgroundStage
	{
		/// <summary>
		/// Total duration before the dungeon automatically ends.
		/// </summary>
		public TimeSpan Duration { get; }

		/// <summary>
		/// Warning messages to display at specific times before the dungeon ends.
		/// Key: Time remaining when warning should be shown.
		/// Value: Message to display.
		/// </summary>
		public Dictionary<TimeSpan, string> Warnings { get; }

		/// <summary>
		/// Whether to send the dungeon fail message when time runs out.
		/// </summary>
		public bool SendFailOnTimeout { get; set; } = true;

		/// <summary>
		/// Optional callback when the timer expires.
		/// </summary>
		public Action<InstanceDungeon> OnTimeExpired { get; set; }

		/// <summary>
		/// Creates a dungeon timer with default 1-hour duration.
		/// </summary>
		public DungeonTimerStage(string stageId = "dungeon_timer")
			: this(TimeSpan.FromHours(1), stageId)
		{
		}

		/// <summary>
		/// Creates a dungeon timer with a custom duration.
		/// </summary>
		public DungeonTimerStage(TimeSpan duration, string stageId = "dungeon_timer")
			: base(stageId)
		{
			this.Duration = duration;
			this.Warnings = new Dictionary<TimeSpan, string>
			{
				{ TimeSpan.FromMinutes(10), "The dungeon will close in 10 minutes." },
				{ TimeSpan.FromMinutes(5), "The dungeon will close in 5 minutes." },
				{ TimeSpan.FromMinutes(1), "The dungeon will close in 1 minute!" }
			};
		}

		/// <summary>
		/// Creates a dungeon timer with custom duration and warnings.
		/// </summary>
		public DungeonTimerStage(TimeSpan duration, Dictionary<TimeSpan, string> warnings, string stageId = "dungeon_timer")
			: base(stageId)
		{
			this.Duration = duration;
			this.Warnings = warnings ?? new Dictionary<TimeSpan, string>();
		}

		protected override async Task Execute()
		{
			var startTime = DateTime.UtcNow;
			var endTime = startTime + Duration;
			var sentWarnings = new HashSet<TimeSpan>();

			while (!CancellationToken.IsCancellationRequested)
			{
				var now = DateTime.UtcNow;
				var remaining = endTime - now;

				// Check if time has expired
				if (remaining <= TimeSpan.Zero)
				{
					await HandleTimeExpired();
					return;
				}

				// Check for warnings to send
				foreach (var warning in Warnings)
				{
					if (!sentWarnings.Contains(warning.Key) && remaining <= warning.Key)
					{
						BroadcastMessage("NOTICE_Dm_scroll", warning.Value, 5);
						sentWarnings.Add(warning.Key);
					}
				}

				// Check every second
				await DelayAsync(TimeSpan.FromSeconds(1));
			}
		}

		private async Task HandleTimeExpired()
		{
			// Call custom handler if provided
			OnTimeExpired?.Invoke(Instance);

			// End the dungeon
			if (SendFailOnTimeout && DungeonScript != null)
			{
				BroadcastMessage("NOTICE_Dm_scroll", "Time's up! The dungeon is closing.", 5);

				// Give players a moment to see the message
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(3), CancellationToken);
				}
				catch (OperationCanceledException)
				{
					// Dungeon may have ended during the delay
				}

				// Only end if the dungeon hasn't already been completed
				if (!Instance.IsComplete)
				{
					DungeonScript.ForceInstanceExpiration(Instance);
				}
			}

			this.IsCompleted = true;
		}

		/// <summary>
		/// Creates a default 1-hour dungeon timer with standard warnings.
		/// </summary>
		public static DungeonTimerStage CreateDefault()
		{
			return new DungeonTimerStage(TimeSpan.FromHours(1));
		}

		/// <summary>
		/// Creates a dungeon timer configured like the Catacombs dungeon (3600 seconds = 1 hour).
		/// </summary>
		public static DungeonTimerStage CreateCatacombsStyle()
		{
			return new DungeonTimerStage(
				TimeSpan.FromSeconds(3600),
				new Dictionary<TimeSpan, string>
				{
					{ TimeSpan.FromMinutes(10), "The mission will end after 10 minutes." },
					{ TimeSpan.FromMinutes(5), "The mission will end after 5 minutes." }
				},
				"catacombs_timer"
			);
		}
	}

	/// <summary>
	/// A background stage that periodically checks conditions and triggers actions.
	/// Useful for complex dungeon mechanics that need to run alongside main progression.
	/// </summary>
	public class PeriodicCheckStage : BackgroundStage
	{
		private readonly TimeSpan _checkInterval;
		private readonly Func<InstanceDungeon, bool> _condition;
		private readonly Action<InstanceDungeon, DungeonScript> _onConditionMet;
		private readonly bool _stopOnConditionMet;

		/// <summary>
		/// Creates a periodic check stage.
		/// </summary>
		/// <param name="checkInterval">How often to check the condition.</param>
		/// <param name="condition">Condition to evaluate.</param>
		/// <param name="onConditionMet">Action to execute when condition is met.</param>
		/// <param name="stopOnConditionMet">Whether to stop checking after condition is met once.</param>
		/// <param name="stageId">Unique stage identifier.</param>
		public PeriodicCheckStage(
			TimeSpan checkInterval,
			Func<InstanceDungeon, bool> condition,
			Action<InstanceDungeon, DungeonScript> onConditionMet,
			bool stopOnConditionMet = true,
			string stageId = null)
			: base(stageId)
		{
			_checkInterval = checkInterval;
			_condition = condition;
			_onConditionMet = onConditionMet;
			_stopOnConditionMet = stopOnConditionMet;
		}

		protected override async Task Execute()
		{
			while (!CancellationToken.IsCancellationRequested)
			{
				await DelayAsync(_checkInterval);

				if (_condition(Instance))
				{
					_onConditionMet?.Invoke(Instance, DungeonScript);

					if (_stopOnConditionMet)
					{
						this.IsCompleted = true;
						return;
					}
				}
			}
		}
	}

	/// <summary>
	/// A background stage that spawns waves of monsters at regular intervals.
	/// </summary>
	public class PeriodicSpawnStage : BackgroundStage
	{
		private readonly TimeSpan _spawnInterval;
		private readonly Func<InstanceDungeon, DungeonScript, Task> _spawnAction;
		private readonly int _maxSpawns;
		private int _spawnCount;

		/// <summary>
		/// Creates a periodic spawn stage.
		/// </summary>
		/// <param name="spawnInterval">Time between spawns.</param>
		/// <param name="spawnAction">Action that spawns monsters.</param>
		/// <param name="maxSpawns">Maximum number of spawn waves (0 for unlimited).</param>
		/// <param name="stageId">Unique stage identifier.</param>
		public PeriodicSpawnStage(
			TimeSpan spawnInterval,
			Func<InstanceDungeon, DungeonScript, Task> spawnAction,
			int maxSpawns = 0,
			string stageId = null)
			: base(stageId)
		{
			_spawnInterval = spawnInterval;
			_spawnAction = spawnAction;
			_maxSpawns = maxSpawns;
			_spawnCount = 0;
		}

		protected override async Task Execute()
		{
			while (!CancellationToken.IsCancellationRequested)
			{
				await DelayAsync(_spawnInterval);

				if (_maxSpawns > 0 && _spawnCount >= _maxSpawns)
				{
					this.IsCompleted = true;
					return;
				}

				await _spawnAction(Instance, DungeonScript);
				_spawnCount++;
			}
		}
	}
}
