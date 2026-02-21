using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Network;
using Melia.Shared.Network.Inter.Messages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;

namespace Melia.Zone.Util
{
	/// <summary>
	/// Manages graceful server shutdown with countdown broadcasts.
	/// </summary>
	public class ServerShutdownManager
	{
		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static ServerShutdownManager _instance;

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		public static ServerShutdownManager Instance => _instance ??= new ServerShutdownManager();

		/// <summary>
		/// Cancellation token source for pending shutdown.
		/// </summary>
		private CancellationTokenSource _shutdownCts;

		/// <summary>
		/// Lock object for shutdown operations.
		/// </summary>
		private readonly object _shutdownLock = new object();

		/// <summary>
		/// Whether a shutdown is currently pending.
		/// </summary>
		public bool IsShutdownPending { get; private set; }

		/// <summary>
		/// The scheduled shutdown time.
		/// </summary>
		public DateTime ScheduledShutdownTime { get; private set; }

		/// <summary>
		/// The reason for the shutdown.
		/// </summary>
		public string ShutdownReason { get; private set; }

		/// <summary>
		/// Whether the shutdown was initiated by the ISC (coordinator).
		/// </summary>
		public bool IsIscInitiated { get; private set; }

		/// <summary>
		/// Default broadcast intervals in minutes before shutdown.
		/// </summary>
		private static readonly int[] BroadcastIntervalsMinutes = { 15, 10, 5, 1 };

		/// <summary>
		/// Broadcast intervals in seconds for the final countdown.
		/// </summary>
		private static readonly int[] BroadcastIntervalsSeconds = { 5, 4, 3, 2, 1 };

		/// <summary>
		/// Private constructor for singleton pattern.
		/// </summary>
		private ServerShutdownManager()
		{
		}

		/// <summary>
		/// Handles a shutdown message received from the ISC coordinator.
		/// </summary>
		/// <param name="message">The shutdown message.</param>
		public void HandleShutdownMessage(ShutdownMessage message)
		{
			switch (message.Type)
			{
				case ShutdownType.Immediate:
					Log.Info("Received IMMEDIATE shutdown command from ISC. Reason: {0}", message.Reason);
					ExecuteImmediateShutdown(message.Reason, message.Delay);
					break;

				case ShutdownType.Graceful:
					var delayMinutes = (int)message.Delay.TotalMinutes;
					Log.Info("Received GRACEFUL shutdown command from ISC. Delay: {0} minute(s), Reason: {1}",
						delayMinutes, message.Reason);
					ScheduleShutdown(delayMinutes, message.Reason, message.Initiator, isIscInitiated: true);
					break;

				case ShutdownType.Cancel:
					Log.Info("Received shutdown CANCELLATION from ISC.");
					CancelShutdown(message.Initiator);
					break;
			}
		}

		/// <summary>
		/// Executes an immediate shutdown with a brief delay for message display.
		/// </summary>
		private void ExecuteImmediateShutdown(string reason, TimeSpan delay)
		{
			lock (_shutdownLock)
			{
				// Cancel any pending graceful shutdown
				if (IsShutdownPending)
				{
					_shutdownCts?.Cancel();
					IsShutdownPending = false;
				}

				ShutdownReason = reason;
			}

			// Broadcast final message and execute
			Send.ZC_TEXT(NoticeTextType.GoldRed, $"[Server] Server is shutting down NOW for {reason}. Thank you for playing!");

			// Wait for the specified delay, then execute
			Task.Run(async () =>
			{
				await Task.Delay(delay);
				ExecuteShutdown();
			});
		}

		/// <summary>
		/// Schedules a graceful server shutdown.
		/// </summary>
		/// <param name="delayMinutes">Delay in minutes before shutdown (0 for immediate).</param>
		/// <param name="reason">Reason for the shutdown.</param>
		/// <param name="initiator">Name of the person/system initiating the shutdown.</param>
		/// <param name="isIscInitiated">Whether this shutdown was initiated by the ISC.</param>
		/// <returns>True if shutdown was scheduled, false if one is already pending.</returns>
		public bool ScheduleShutdown(int delayMinutes, string reason, string initiator = "System", bool isIscInitiated = false)
		{
			lock (_shutdownLock)
			{
				if (IsShutdownPending)
				{
					Log.Warning("[{0}] attempted to schedule shutdown, but one is already pending.", initiator);
					return false;
				}

				ShutdownReason = reason;
				IsIscInitiated = isIscInitiated;

				if (delayMinutes <= 0)
				{
					Log.Info("[{0}] initiated immediate server shutdown. Reason: {1}", initiator, reason);
					IsShutdownPending = false;
					ExecuteShutdown();
					return true;
				}

				IsShutdownPending = true;
				ScheduledShutdownTime = DateTime.Now.AddMinutes(delayMinutes);
				_shutdownCts = new CancellationTokenSource();

				Log.Info("[{0}] scheduled server shutdown in {1} minute(s). Reason: {2}{3}",
					initiator, delayMinutes, reason,
					isIscInitiated ? " (ISC-initiated)" : "");
			}

			StartShutdownCountdown(delayMinutes, reason);
			return true;
		}

		/// <summary>
		/// Cancels a pending shutdown.
		/// </summary>
		/// <param name="initiator">Name of the person/system cancelling the shutdown.</param>
		/// <returns>True if a shutdown was cancelled, false if none was pending.</returns>
		public bool CancelShutdown(string initiator = "System")
		{
			lock (_shutdownLock)
			{
				if (!IsShutdownPending)
				{
					return false;
				}

				try
				{
					_shutdownCts?.Cancel();
					IsShutdownPending = false;
					IsIscInitiated = false;
					ShutdownReason = null;
					Log.Info("[{0}] cancelled the pending server shutdown.", initiator);
					return true;
				}
				catch (Exception ex)
				{
					Log.Error("Error cancelling shutdown: {0}", ex.Message);
					return false;
				}
			}
		}

		/// <summary>
		/// Gets the remaining time until shutdown.
		/// </summary>
		/// <returns>Remaining time, or null if no shutdown is pending.</returns>
		public TimeSpan? GetRemainingTime()
		{
			lock (_shutdownLock)
			{
				if (!IsShutdownPending)
					return null;

				var remaining = ScheduledShutdownTime - DateTime.Now;
				return remaining.TotalSeconds > 0 ? remaining : TimeSpan.Zero;
			}
		}

		/// <summary>
		/// Gets a formatted string of the remaining time.
		/// </summary>
		public string GetRemainingTimeFormatted()
		{
			var remaining = GetRemainingTime();
			if (!remaining.HasValue)
				return "No shutdown pending";

			return FormatTimeSpan(remaining.Value);
		}

		/// <summary>
		/// Starts the shutdown countdown task.
		/// </summary>
		private void StartShutdownCountdown(int delayMinutes, string reason)
		{
			var token = _shutdownCts.Token;

			// Broadcast initial announcement
			BroadcastWarningMinutes(delayMinutes, reason);

			Task.Run(async () =>
			{
				try
				{
					var remainingSeconds = delayMinutes * 60;

					while (remainingSeconds > 0)
					{
						if (token.IsCancellationRequested)
						{
							BroadcastCancelled();
							return;
						}

						// Check if we should broadcast a minute warning (at exact minute boundaries)
						if (remainingSeconds % 60 == 0)
						{
							var remainingMinutes = remainingSeconds / 60;
							if (remainingMinutes > 0 && remainingMinutes < delayMinutes && BroadcastIntervalsMinutes.Contains(remainingMinutes))
							{
								BroadcastWarningMinutes(remainingMinutes, reason);
							}
						}

						// Final countdown: broadcast at specific second intervals
						if (remainingSeconds <= 60 && BroadcastIntervalsSeconds.Contains(remainingSeconds))
						{
							BroadcastWarningSeconds(remainingSeconds);
						}

						// Wait 1 second
						await Task.Delay(1000, token);
						remainingSeconds--;

						// Update scheduled time for status display
						lock (_shutdownLock)
						{
							ScheduledShutdownTime = DateTime.Now.AddSeconds(remainingSeconds);
						}
					}

					// Time's up - execute shutdown
					if (!token.IsCancellationRequested)
					{
						lock (_shutdownLock)
						{
							IsShutdownPending = false;
						}

						ExecuteShutdown();
					}
				}
				catch (TaskCanceledException)
				{
					BroadcastCancelled();
				}
				catch (Exception ex)
				{
					Log.Error("Error during shutdown countdown: {0}", ex.Message);
				}
				finally
				{
					lock (_shutdownLock)
					{
						IsShutdownPending = false;
						_shutdownCts?.Dispose();
						_shutdownCts = null;
					}
				}
			});
		}

		/// <summary>
		/// Broadcasts a shutdown warning (minutes remaining) using bold notice text.
		/// </summary>
		private void BroadcastWarningMinutes(int minutes, string reason)
		{
			var timeStr = minutes == 1 ? "1 minute" : $"{minutes} minutes";
			var message = $"[Server] The server will shut down in {timeStr} for {reason}.";

			Log.Info("Shutdown broadcast: {0} remaining.", timeStr);
			Send.ZC_TEXT(NoticeTextType.GoldRed, message);
		}

		/// <summary>
		/// Broadcasts a shutdown warning (seconds remaining).
		/// </summary>
		private void BroadcastWarningSeconds(int seconds)
		{
			var timeStr = seconds == 1 ? "1 second" : $"{seconds} seconds";
			var message = $"[Server] The server will shut down in {timeStr}!";

			Log.Info("Shutdown broadcast: {0} remaining.", timeStr);
			BroadcastToAllPlayers(message);
		}

		/// <summary>
		/// Broadcasts that the shutdown has been cancelled using bold notice text.
		/// </summary>
		private void BroadcastCancelled()
		{
			var message = "[Server] The scheduled server shutdown has been cancelled.";
			Log.Info("Shutdown cancelled - broadcasting to all players.");
			Send.ZC_TEXT(NoticeTextType.GoldRed, message);
		}

		/// <summary>
		/// Broadcasts a message to all connected players.
		/// </summary>
		private void BroadcastToAllPlayers(string message)
		{
			try
			{
				var characters = ZoneServer.Instance.World.GetCharacters().ToList();
				foreach (var character in characters)
				{
					try
					{
						character.ServerMessage(message);
					}
					catch (Exception ex)
					{
						Log.Debug("Failed to send broadcast to {0}: {1}", character.Name, ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Failed to broadcast message: {0}", ex.Message);
			}
		}

		/// <summary>
		/// Executes the actual server shutdown.
		/// </summary>
		private void ExecuteShutdown()
		{
			Task.Run(() =>
			{
				try
				{
					Log.Info("========================================");
					Log.Info("       INITIATING SERVER SHUTDOWN       ");
					Log.Info("========================================");
					Log.Info("Reason: {0}", ShutdownReason ?? "No reason specified");
					if (IsIscInitiated)
						Log.Info("(Shutdown was initiated by ISC coordinator)");

					var characters = ZoneServer.Instance.World.GetCharacters().ToList();
					Log.Info("Players to save and disconnect: {0}", characters.Count);

					// Final broadcast (system message, followed by MsgBox popup per player)
					var finalMessage = "[Server] Server is shutting down NOW. Thank you for playing!";
					BroadcastToAllPlayers(finalMessage);

					// Give players a moment to see the message
					Thread.Sleep(2000);

					// Save and disconnect all players
					var savedCount = 0;
					var failedCount = 0;

					foreach (var character in characters)
					{
						try
						{
							Log.Info("  Saving: {0} (ID: {1})", character.Name, character.DbId);

							if (character.Connection?.Account != null)
							{
								ZoneServer.Instance.Database.SaveCharacterData(character);
								ZoneServer.Instance.Database.SaveAccountData(character.Connection.Account);
								ZoneServer.Instance.Database.UpdateLoginState(character.AccountDbId, 0, LoginState.LoggedOut);
							}

							character.MsgBox(
								Localization.Get("Server Shutdown"),
								Localization.Get("The server is shutting down: {0}"),
								ShutdownReason ?? "maintenance"
							);
							character.Connection?.Close(1000);

							savedCount++;
							Log.Info("    ✓ Saved and disconnected");
						}
						catch (Exception ex)
						{
							failedCount++;
							Log.Error("    ✗ Error: {0}", ex.Message);
							character.Connection?.Close();
						}
					}

					Log.Info("Player save complete - Saved: {0}, Failed: {1}", savedCount, failedCount);

					// Update server status
					ZoneServer.Instance.ServerInfo.Status = ServerStatus.Offline;
					var serverUpdateMessage = new ServerUpdateMessage(
						ServerType.Zone,
						ZoneServer.Instance.ServerInfo.Id,
						0,
						ServerStatus.Offline
					);

					ZoneServer.Instance.ServerList.Update(serverUpdateMessage);
					ZoneServer.Instance.Communicator.Broadcast("ServerUpdates", serverUpdateMessage);

					Log.Info("========================================");
					Log.Info("       SERVER SHUTDOWN COMPLETE         ");
					Log.Info("========================================");
					Log.Info("The server process can now be safely terminated.");

					// Optionally exit the process
					// Environment.Exit(0);
				}
				catch (Exception ex)
				{
					Log.Error("Critical error during shutdown: {0}", ex);
				}
			});
		}

		/// <summary>
		/// Formats a timespan into a human-readable string.
		/// </summary>
		private static string FormatTimeSpan(TimeSpan time)
		{
			if (time.TotalHours >= 1)
				return $"{(int)time.TotalHours}h {time.Minutes}m {time.Seconds}s";
			if (time.TotalMinutes >= 1)
				return $"{time.Minutes}m {time.Seconds}s";
			return $"{time.Seconds}s";
		}
	}
}
