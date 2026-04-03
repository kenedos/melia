using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Melia.Zone.Database;
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
				var capturedSlot = slotToSave;

				foreach (var character in charactersInSlot)
				{
					try
					{
						var conn = character.Connection;
						var account = conn?.Account;
						var sessionKey = conn?.SessionKey;
						var isAutoTrading = character.IsAutoTrading;

						if (account == null)
						{
							Log.Warning($"AutoSaveService: Skipping save for {character.Name} ({character.DbId}), account is null.");
							failedCount++;
						}
						else
						{
							var sessionValid = isAutoTrading || _database.CheckSessionKey(account.Id, sessionKey);

							if ((character.IsOnline || isAutoTrading) && sessionValid)
							{
								_database.SaveCharacterData(character);
								if (!isAutoTrading)
									_database.SaveAccountData(account);
								savedCount++;
							}
							else
							{
								Log.Warning($"AutoSaveService: Skipping save for {character.Name} (logged out or session mismatch).");
								failedCount++;
							}
						}
					}
					catch (Exception ex)
					{
						Log.Error($"AutoSaveService: Error saving character {character?.Name ?? "Unknown"} (ID: {character?.DbId ?? 0}): {ex}");
						failedCount++;
					}
				}

				Log.Info($"AutoSaveService: Finished save for slot {capturedSlot}. Saved: {savedCount}, Skipped/Failed: {failedCount}.");

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

		/// <summary>
		/// Stops the periodic timer and saves every online character
		/// synchronously on the calling thread.
		/// </summary>
		/// <returns>Number of characters saved successfully.</returns>
		public int SaveAllNow()
		{
			// Stop the timer so no new callbacks fire.
			_timer?.Change(Timeout.Infinite, Timeout.Infinite);

			var allChars = _zoneServer.World.GetCharacters().ToList();
			if (allChars.Count == 0)
				return 0;

			var savedCount = 0;
			var failedCount = 0;

			foreach (var character in allChars)
			{
				try
				{
					if (character == null || character.Connection?.Account == null)
						continue;

					_database.SaveCharacterData(character);
					_database.SaveAccountData(character.Connection.Account);
					savedCount++;
				}
				catch (Exception ex)
				{
					failedCount++;
					Log.Error("SaveAllNow: Error saving {0} (ID: {1}): {2}", character?.Name ?? "?", character?.DbId ?? 0, ex.Message);
				}
			}

			Log.Info("SaveAllNow: Saved {0}, Failed {1}.", savedCount, failedCount);
			return savedCount;
		}

		public void Dispose()
		{
			_timer?.Dispose();
			Log.Info("AutoSaveService stopped.");
		}
	}
}
