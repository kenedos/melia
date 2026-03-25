using System;
using System.Collections.Concurrent;
using System.Threading;
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
		public static void Start(int workerCount = 4)
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
