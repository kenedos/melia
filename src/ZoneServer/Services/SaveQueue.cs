using System;
using System.Collections.Concurrent;
using System.Threading;
using Melia.Shared.Database;
using Melia.Zone.Database;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;

namespace Melia.Zone.Services
{
	/// <summary>
	/// Provides a dedicated thread pool for save operations,
	/// preventing thread pool starvation from synchronous DB I/O.
	/// </summary>
	public static class SaveQueue
	{
		private static readonly BlockingCollection<Action> _queue = new();
		private static Thread[] _workers;

		/// <summary>
		/// Returns true if the queue has been marked as complete
		/// and will no longer accept new items.
		/// </summary>
		public static bool IsAddingCompleted => _queue.IsAddingCompleted;

		/// <summary>
		/// Starts the save queue with the specified number of
		/// dedicated worker threads.
		/// </summary>
		/// <param name="workerCount"></param>
		/// <remarks>
		/// Uses a single worker to guarantee save operations for the
		/// same character/account are never processed concurrently.
		/// Multiple workers would require per-entity partitioning to
		/// avoid interleaved writes (e.g. DELETE-all + INSERT-all in
		/// SaveRevealedMaps racing with itself). If throughput becomes
		/// an issue, increase workerCount but add keyed partitioning
		/// so that saves for the same character always land on the
		/// same worker.
		/// </remarks>
		public static void Start(int workerCount = 1)
		{
			_workers = new Thread[workerCount];
			for (var i = 0; i < workerCount; i++)
			{
				_workers[i] = new Thread(ProcessQueue)
				{
					IsBackground = true,
					Name = $"SaveWorker-{i}",
				};
				_workers[i].Start();
			}
			Log.Info($"SaveQueue: Started with {workerCount} worker threads.");
		}

		private static void ProcessQueue()
		{
			foreach (var action in _queue.GetConsumingEnumerable())
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Log.Error($"SaveQueue: {ex}");
				}
			}
		}

		/// <summary>
		/// Enqueues a save operation to be processed by a dedicated
		/// worker thread.
		/// </summary>
		/// <param name="action"></param>
		public static void Enqueue(Action action)
		{
			try
			{
				_queue.Add(action);
			}
			catch (InvalidOperationException)
			{
				Log.Warning("SaveQueue: Queue is completing; executing save inline.");
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Log.Error($"SaveQueue inline fallback: {ex}");
				}
			}
		}

		/// <summary>
		/// Enqueues a character save operation.
		/// </summary>
		public static void SaveCharacter(Character character)
		{
			Enqueue(() =>
			{
				try
				{
					ZoneServer.Instance.Database.SaveCharacterData(character);
				}
				catch (Exception ex)
				{
					Log.Error($"SaveQueue: Failed to save character '{character?.Name}' ({character?.DbId}): {ex}");
				}
			});
		}

		/// <summary>
		/// Enqueues a character + account save operation.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="character"></param>
		/// <param name="sessionKey"></param>
		/// <param name="skipSessionCheck">Skip the DB session key check.
		/// Use when the caller has already validated the session and the
		/// key may be invalidated before the queue drains (e.g. moving
		/// to barracks where the barracks server issues a new key).</param>
		public static void SaveAccountAndCharacter(Account account, Character character, string sessionKey, bool skipSessionCheck = false)
		{
			Enqueue(() =>
			{
				try
				{
					if (account == null)
						return;

					if (!skipSessionCheck && !ZoneServer.Instance.Database.CheckSessionKey(account.Id, sessionKey))
					{
						Log.Warning("SaveQueue: Skipping save for account '{0}' and character '{1}' because session key does not match.", account.Name, character?.Name ?? "NULL");
						return;
					}

					// Save character FIRST — if this fails, we don't want
					// the account already marked as logged out with stale
					// character data in the DB.
					if (character != null)
					{
						ZoneServer.Instance.Database.SaveCharacterData(character);
					}

					ZoneServer.Instance.Database.SaveAccountData(account);
					ZoneServer.Instance.Database.UpdateLoginState(account.Id, 0, LoginState.LoggedOut);

					character?.ResetWarpSaveFlag();
				}
				catch (Exception ex)
				{
					Log.Error($"SaveQueue: Failed to save account/character '{character?.Name}': {ex}");
				}
			});
		}

		/// <summary>
		/// Signals the queue to stop accepting new work. Workers will
		/// drain remaining items and exit.
		/// </summary>
		public static void Stop()
		{
			if (!_queue.IsAddingCompleted)
				_queue.CompleteAdding();
		}

		/// <summary>
		/// Signals the queue to stop accepting new work, then blocks
		/// until all workers finish processing remaining items.
		/// </summary>
		/// <param name="timeoutMs">Maximum time in ms to wait for
		/// each worker thread. Default 30 seconds.</param>
		/// <returns>True if all workers drained in time.</returns>
		public static bool StopAndDrain(int timeoutMs = 30000)
		{
			if (!_queue.IsAddingCompleted)
				_queue.CompleteAdding();

			if (_workers == null)
				return true;

			var allDrained = true;
			foreach (var worker in _workers)
			{
				if (worker != null && worker.IsAlive)
				{
					if (!worker.Join(timeoutMs))
					{
						Log.Warning("SaveQueue: Worker '{0}' did not finish within {1}ms.", worker.Name, timeoutMs);
						allDrained = false;
					}
				}
			}

			Log.Info("SaveQueue: All workers {0}.", allDrained ? "drained successfully" : "timed out");
			return allDrained;
		}
	}
}
