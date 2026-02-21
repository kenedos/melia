using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Yggdrasil.Logging;

namespace Melia.Zone.World
{
	/// <summary>
	/// Manages per-character locks to prevent concurrent saves of the same character.
	/// Uses Monitor-based locking for synchronous save operations.
	/// </summary>
	public static class CharacterLockManager
	{
		private static readonly ConcurrentDictionary<long, object> _locks = new ConcurrentDictionary<long, object>();

		private const long SlowLockThresholdMs = 2000;

		/// <summary>
		/// Gets the lock object for a specific character ID.
		/// Creates it if it doesn't exist.
		/// </summary>
		private static object GetLock(long characterId)
		{
			return _locks.GetOrAdd(characterId, id => new object());
		}

		/// <summary>
		/// Tries to acquire the lock for a character synchronously.
		/// Returns the lock object via out parameter so callers can pass
		/// it to Release, avoiding issues if TryRemoveLock runs concurrently.
		/// </summary>
		/// <param name="characterId">The ID of the character to lock.</param>
		/// <param name="timeout">The time to wait for the lock.</param>
		/// <param name="reason">A descriptive reason for acquiring the lock.</param>
		/// <param name="lockTaken">Set to true if the lock was acquired.</param>
		/// <param name="lockObj">The lock object that was acquired (for passing to Release).</param>
		public static void TryAcquire(long characterId, TimeSpan timeout, string reason, ref bool lockTaken, out object lockObj)
		{
			lockObj = GetLock(characterId);

			try
			{
				var sw = Stopwatch.StartNew();
				Monitor.TryEnter(lockObj, timeout, ref lockTaken);
				sw.Stop();

				if (!lockTaken)
				{
					Log.Warning($"CharacterLockManager: Timeout acquiring lock for Character ID {characterId} by '{reason}' after {timeout.TotalSeconds}s. (Thread {Thread.CurrentThread.ManagedThreadId})");
				}
				else if (sw.ElapsedMilliseconds > SlowLockThresholdMs)
				{
					Log.Warning($"CharacterLockManager: Slow lock acquisition for Character ID {characterId} by '{reason}': {sw.ElapsedMilliseconds}ms. (Thread {Thread.CurrentThread.ManagedThreadId})");
				}
			}
			catch (Exception ex)
			{
				Log.Error($"CharacterLockManager: Exception during TryAcquire for Character ID {characterId} by '{reason}': {ex}");
			}
		}

		/// <summary>
		/// Tries to acquire the lock for a character synchronously.
		/// </summary>
		public static void TryAcquire(long characterId, TimeSpan timeout, string reason, ref bool lockTaken)
		{
			TryAcquire(characterId, timeout, reason, ref lockTaken, out _);
		}

		/// <summary>
		/// Releases a lock using the lock object directly. Preferred over
		/// the characterId overload because it is immune to TryRemoveLock
		/// removing the dictionary entry while the lock is held.
		/// </summary>
		/// <param name="lockObj">The lock object returned by TryAcquire.</param>
		/// <param name="characterId">Character ID (for logging only).</param>
		/// <param name="reason">A descriptive reason for releasing the lock.</param>
		public static void Release(object lockObj, long characterId, string reason)
		{
			if (lockObj == null)
			{
				Log.Warning($"CharacterLockManager: Attempted to release null lock for Character ID {characterId} by '{reason}'.");
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
					Log.Warning($"CharacterLockManager: Attempted to release lock for Character ID {characterId} by '{reason}' that was not held.");
				}
			}
			catch (Exception ex)
			{
				Log.Error($"CharacterLockManager: Exception during Release for Character ID {characterId} by '{reason}': {ex}");
			}
		}

		/// <summary>
		/// Releases the lock for a character by looking it up in the dictionary.
		/// Prefer the overload that takes the lock object directly.
		/// </summary>
		/// <param name="characterId">The ID of the character to unlock.</param>
		/// <param name="reason">A descriptive reason for releasing the lock.</param>
		public static void Release(long characterId, string reason)
		{
			if (!_locks.TryGetValue(characterId, out var lockObj))
			{
				// This can happen benignly if TryRemoveLock ran on another
				// thread between acquire and release (e.g. warp/reconnect).
				Log.Debug($"CharacterLockManager: Lock for Character ID {characterId} was already removed from dictionary during '{reason}'. This is benign during warp/reconnect.");
				return;
			}

			Release(lockObj, characterId, reason);
		}

		/// <summary>
		/// Removes the lock for a character from the dictionary.
		/// Call this when a character logs out to prevent memory accumulation.
		/// </summary>
		/// <param name="characterId">The ID of the character.</param>
		public static void TryRemoveLock(long characterId)
		{
			_locks.TryRemove(characterId, out _);
		}
	}
}
