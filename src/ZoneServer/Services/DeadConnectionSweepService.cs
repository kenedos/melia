using System;
using System.Threading;
using Melia.Zone.Network;
using Yggdrasil.Logging;

namespace Melia.Zone.Services
{
	/// <summary>
	/// Periodically checks all active connections and closes any that
	/// have not sent a heartbeat within the timeout window. This catches
	/// clients that crashed or lost network connectivity without sending
	/// a TCP FIN.
	/// </summary>
	public class DeadConnectionSweepService : IDisposable
	{
		private readonly Timer _timer;
		private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(60);

		/// <summary>
		/// Creates and starts the sweep service.
		/// </summary>
		/// <param name="sweepInterval">How often to check for stale connections.</param>
		public DeadConnectionSweepService(TimeSpan sweepInterval)
		{
			_timer = new Timer(this.Sweep, null, sweepInterval, sweepInterval);
			Log.Info($"DeadConnectionSweepService: Started with {sweepInterval.TotalSeconds}s sweep interval.");
		}

		private void Sweep(object state)
		{
			var now = DateTime.Now;

			foreach (var character in ZoneServer.Instance.World.GetCharacters())
			{
				var conn = character.Connection;
				if (conn == null || conn is DummyConnection)
					continue;

				if (conn.LastHeartBeat < now - Timeout)
				{
					Log.Info($"DeadConnectionSweep: Closing stale connection for '{character.Name}' (last heartbeat: {conn.LastHeartBeat}).");
					conn.Close();
				}
			}
		}

		/// <summary>
		/// Stops the sweep timer.
		/// </summary>
		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
