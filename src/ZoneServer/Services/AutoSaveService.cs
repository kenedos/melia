using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Melia.Zone.Database;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;

namespace Melia.Zone.Services
{
	public class AutoSaveService : IDisposable
	{
		private readonly ZoneServer _zoneServer;
		private readonly ZoneDb _database;
		private readonly Timer _timer;
		private readonly int _numberOfSlots;
		private int _currentSlot = 0;
		private int _completedCycles = 0;
		private readonly int _orphanCleanupCycles;

		public AutoSaveService(ZoneServer zoneServer, ZoneDb database, int numberOfSlots, TimeSpan saveIntervalPerSlot, int orphanCleanupCycles)
		{
			_zoneServer = zoneServer;
			_database = database;
			_numberOfSlots = numberOfSlots;
			_orphanCleanupCycles = orphanCleanupCycles;
			_timer = new Timer(this.DoAutoSave, null, saveIntervalPerSlot, saveIntervalPerSlot);

			Log.Info($"AutoSaveService initialized: Saving slot every {saveIntervalPerSlot.TotalSeconds}s ({_numberOfSlots} slots total). Orphan cleanup every {orphanCleanupCycles} cycles.");
		}

		private void DoAutoSave(object? state)
		{
			var slotToSave = _currentSlot;
			_currentSlot = (_currentSlot + 1) % _numberOfSlots;

			try
			{
				var allChars = _zoneServer.World.GetCharacters().ToList();
				if (allChars.Count == 0)
					return;

				var charactersInSlot = new List<Character>();
				foreach (var c in allChars)
				{
					if (c == null) continue;
					if ((c.DbId % _numberOfSlots) != slotToSave) continue;
					if ((c.IsOnline || c.IsAutoTrading) && c.Connection != null)
						charactersInSlot.Add(c);
				}

				if (charactersInSlot.Count == 0)
					return;

				var savedCount = 0;
				var failedCount = 0;
				var remaining = charactersInSlot.Count;
				var capturedSlot = slotToSave;

				foreach (var character in charactersInSlot)
				{
					var capturedChar = character;

					SaveQueue.Enqueue(() =>
					{
						var lockTaken = false;
						try
						{
							CharacterLockManager.TryAcquire(capturedChar.DbId, TimeSpan.Zero, "AutoSave", ref lockTaken);
							if (lockTaken)
							{
								var account = capturedChar.Connection?.Account;
								var isAutoTrading = capturedChar.IsAutoTrading;
								var sessionValid = isAutoTrading || _database.CheckSessionKey(account.Id, capturedChar.Connection.SessionKey);

								if (account != null && (capturedChar.IsOnline || isAutoTrading) && sessionValid)
								{
									_database.SaveCharacterData(capturedChar);
									if (!isAutoTrading)
										_database.SaveAccountData(account);
									Interlocked.Increment(ref savedCount);
								}
								else
								{
									Log.Warning($"AutoSaveService: Skipping save for {capturedChar.Name} (logged out, session mismatch, or lock issue during check).");
									Interlocked.Increment(ref failedCount);
								}
							}
							else
							{
								Log.Debug($"AutoSaveService: Skipping '{capturedChar.Name}' ({capturedChar.DbId}), character lock is busy.");
								Interlocked.Increment(ref failedCount);
							}
						}
						catch (Exception ex)
						{
							Log.Error($"AutoSaveService: Error saving character {capturedChar?.Name ?? "Unknown"} (ID: {capturedChar?.DbId ?? 0}): {ex}");
							Interlocked.Increment(ref failedCount);
						}
						finally
						{
							if (lockTaken)
								CharacterLockManager.Release(capturedChar.DbId, "AutoSave");

							if (Interlocked.Decrement(ref remaining) == 0)
								Log.Info($"AutoSaveService: Finished save for slot {capturedSlot}. Saved: {savedCount}, Skipped/Failed: {failedCount}.");
						}
					});
				}

				if (_currentSlot == 0)
				{
					_completedCycles++;

					if (_orphanCleanupCycles > 0 && _completedCycles % _orphanCleanupCycles == 0)
					{
						Log.Info($"AutoSaveService: Completed {_completedCycles} cycles. Triggering orphan cleanup.");
						_zoneServer.OrphanCleanupService?.TriggerCleanup();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error($"AutoSaveService: Unhandled exception during auto-save cycle for slot {slotToSave}: {ex}");
			}
		}

		public void Dispose()
		{
			_timer?.Dispose();
			Log.Info("AutoSaveService stopped.");
		}
	}
}
