using System;
using System.Threading;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Dungeons.Stages
{
	/// <summary>
	/// Base class for background stages that run in parallel with the main stage progression.
	/// Background stages are ideal for:
	/// - Dungeon-wide timers (time limits, warnings)
	/// - Periodic events (spawning reinforcements, environmental effects)
	/// - Global condition monitoring (party wipe detection, objective tracking)
	/// </summary>
	public abstract class BackgroundStage
	{
		/// <summary>
		/// Unique identifier for this background stage.
		/// </summary>
		public string StageId { get; protected set; }

		/// <summary>
		/// Whether this background stage is currently active.
		/// </summary>
		public bool IsActive { get; protected set; }

		/// <summary>
		/// Whether this background stage has completed its task.
		/// </summary>
		public bool IsCompleted { get; protected set; }

		/// <summary>
		/// Reference to the dungeon script for callbacks and utilities.
		/// </summary>
		protected DungeonScript DungeonScript { get; private set; }

		/// <summary>
		/// Reference to the dungeon instance.
		/// </summary>
		protected InstanceDungeon Instance { get; private set; }

		/// <summary>
		/// Cancellation token for stopping the background task.
		/// </summary>
		protected CancellationToken CancellationToken { get; private set; }

		protected BackgroundStage(string stageId = null)
		{
			this.StageId = stageId ?? Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Starts the background stage. This runs asynchronously in parallel with main stages.
		/// </summary>
		public async Task Start(InstanceDungeon instance, DungeonScript dungeonScript, CancellationToken cancellationToken)
		{
			this.Instance = instance;
			this.DungeonScript = dungeonScript;
			this.CancellationToken = cancellationToken;
			this.IsActive = true;

			try
			{
				await this.Execute();
			}
			catch (OperationCanceledException)
			{
				// Expected when dungeon ends
			}
			catch (Exception ex)
			{
				Log.Error($"BackgroundStage '{StageId}' error: {ex.Message}");
			}
			finally
			{
				this.IsActive = false;
			}
		}

		/// <summary>
		/// Override this to implement the background stage's logic.
		/// This method runs asynchronously and should periodically check CancellationToken.
		/// </summary>
		protected abstract Task Execute();

		/// <summary>
		/// Stops the background stage.
		/// </summary>
		public virtual void Stop()
		{
			this.IsActive = false;
			this.IsCompleted = true;
		}

		/// <summary>
		/// Helper method for async delays that respect cancellation.
		/// </summary>
		protected async Task DelayAsync(TimeSpan delay)
		{
			await Task.Delay(delay, CancellationToken);
		}

		/// <summary>
		/// Sends a message to all players in the dungeon.
		/// </summary>
		protected void BroadcastMessage(string functionName, string message, int duration)
		{
			DungeonScript?.MGameMessage(Instance, functionName, message, duration);
		}
	}
}
