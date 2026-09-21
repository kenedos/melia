using System;
using System.Threading;
using Melia.Zone.Network;
using Yggdrasil.Logging;

namespace Melia.Zone.Services
{
	/// <summary>
	/// Periodically checks all active connections and closes any that
	/// have not sent any packet within the timeout window. This catches
	/// clients that crashed or lost network connectivity without sending
	/// a TCP FIN. Liveness is stamped on every received packet in
	/// ZoneConnection.OnPacketReceived, so any client traffic counts.
	/// </summary>
	public class DeadConnectionSweepService : IDisposable
	{
		private readonly Timer _timer;
		private readonly TimeSpan _timeout;

		/// <summary>
		/// Connections that are still loading or warping get an extended
		/// timeout, since clients may legitimately not send packets during
		/// long loading screens. They're not skipped entirely so that
		/// clients that crash mid-load don't linger as ghosts forever.
		/// </summary>
		private const int LoadingGraceMultiplier = 5;

		/// <summary>
		/// Creates and starts the sweep service.
		/// </summary>
		/// <param name="sweepInterval">How often to check for stale connections.</param>
		/// <param name="timeout">How long a connection may go without any packet before it's considered dead.</param>
		public DeadConnectionSweepService(TimeSpan sweepInterval, TimeSpan timeout)
		{
			_timeout = timeout;
			_timer = new Timer(this.Sweep, null, sweepInterval, sweepInterval);
			Log.Info($"DeadConnectionSweepService: Started with {sweepInterval.TotalSeconds}s sweep interval, {timeout.TotalSeconds}s timeout.");
		}

		private void Sweep(object state)
		{
			var now = DateTime.UtcNow;

			foreach (var character in ZoneServer.Instance.World.GetCharacters())
			{
				if (character.Connection is not ZoneConnection conn)
					continue;

				var timeout = _timeout;
				if (!conn.GameReady || !conn.LoadComplete || character.IsWarping)
					timeout = TimeSpan.FromTicks(_timeout.Ticks * LoadingGraceMultiplier);

				if (conn.LastHeartBeat < now - timeout)
				{
					Log.Warning($"DeadConnectionSweep: Disconnecting '{character.Name}' (account: {conn.Account?.Name}), no packets received for {(now - conn.LastHeartBeat).TotalSeconds:F0}s.");

					try
					{
						// Run full cleanup via the shared method, then
						// close the underlying TCP socket. CleanupCharacter
						// nulls SelectedCharacter, so OnClosed will early-
						// return without running a competing cleanup.
						conn.CleanupCharacter(save: true);
						conn.Close();
					}
					catch (Exception ex)
					{
						Log.Error($"DeadConnectionSweep: Error cleaning up '{character.Name}': {ex.Message}");
					}
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
