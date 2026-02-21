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
			_queue.Add(action);
		}

		/// <summary>
		/// Signals the queue to stop accepting new work. Workers will
		/// drain remaining items and exit.
		/// </summary>
		public static void Stop()
		{
			_queue.CompleteAdding();
		}
	}
}
