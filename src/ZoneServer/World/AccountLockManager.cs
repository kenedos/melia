using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Yggdrasil.Logging;

namespace Melia.Zone.World
{
	/// <summary>
	/// Manages per-account locks to prevent concurrent saves of shared
	/// account data, such as account variables and team storage.
	/// </summary>
	public static class AccountLockManager
	{
		private static readonly ConcurrentDictionary<long, object> _locks = new ConcurrentDictionary<long, object>();

		private const long SlowLockThresholdMs = 2000;

		private static object GetLock(long accountId)
		{
			return _locks.GetOrAdd(accountId, id => new object());
		}

		public static void TryAcquire(long accountId, TimeSpan timeout, string reason, ref bool lockTaken, out object lockObj)
		{
			lockObj = GetLock(accountId);

			try
			{
				var sw = Stopwatch.StartNew();
				Monitor.TryEnter(lockObj, timeout, ref lockTaken);
				sw.Stop();

				if (!lockTaken)
				{
					Log.Warning($"AccountLockManager: Timeout acquiring lock for Account ID {accountId} by '{reason}' after {timeout.TotalSeconds}s. (Thread {Thread.CurrentThread.ManagedThreadId})");
				}
				else if (sw.ElapsedMilliseconds > SlowLockThresholdMs)
				{
					Log.Warning($"AccountLockManager: Slow lock acquisition for Account ID {accountId} by '{reason}': {sw.ElapsedMilliseconds}ms. (Thread {Thread.CurrentThread.ManagedThreadId})");
				}
			}
			catch (Exception ex)
			{
				Log.Error($"AccountLockManager: Exception during TryAcquire for Account ID {accountId} by '{reason}': {ex}");
			}
		}

		public static void Release(object lockObj, long accountId, string reason)
		{
			if (lockObj == null)
			{
				Log.Warning($"AccountLockManager: Attempted to release null lock for Account ID {accountId} by '{reason}'.");
				return;
			}

			try
			{
				if (Monitor.IsEntered(lockObj))
				{
					Monitor.Exit(lockObj);
				}
				else
				{
					Log.Warning($"AccountLockManager: Attempted to release lock for Account ID {accountId} by '{reason}' that was not held.");
				}
			}
			catch (Exception ex)
			{
				Log.Error($"AccountLockManager: Exception during Release for Account ID {accountId} by '{reason}': {ex}");
			}
		}
	}
}
