using System;
using System.Threading;
using Melia.Zone.Database;
using MySqlConnector;
using Yggdrasil.Logging;

namespace Melia.Zone.Services
{
	/// <summary>
	/// Periodically trims log/snapshot tables to prevent unbounded growth
	/// that degrades save performance.
	/// </summary>
	public class LogCleanupService : IDisposable
	{
		private readonly ZoneDb _database;
		private readonly Timer _timer;
		private readonly int _retentionDays;

		private static readonly string[] LogTables = new[]
		{
			"character_properties_log",
			"character_etc_properties_log",
			"account_properties_log",
			"item_properties_log",
			"log_item_transactions",
			"log_chat",
			"log_bot",
		};

		private static readonly string[] TimestampColumns = new[]
		{
			"backupTimestamp",
			"backupTimestamp",
			"backupTimestamp",
			"backupTimestamp",
			"timestamp",
			"timestamp",
			"timestamp",
		};

		public LogCleanupService(ZoneDb database, int retentionDays, TimeSpan interval)
		{
			_database = database;
			_retentionDays = retentionDays;

			// First run after 2 minutes, then repeat on interval
			_timer = new Timer(this.DoCleanup, null, TimeSpan.FromMinutes(2), interval);

			Log.Info($"LogCleanupService initialized: Retaining {retentionDays} days of logs, running every {interval.TotalHours}h.");
		}

		private void DoCleanup(object state)
		{
			var cutoff = DateTime.UtcNow.AddDays(-_retentionDays);

			for (var i = 0; i < LogTables.Length; i++)
			{
				try
				{
					var deleted = this.DeleteOldRows(LogTables[i], TimestampColumns[i], cutoff);
					if (deleted > 0)
						Log.Info($"LogCleanupService: Deleted {deleted} rows from {LogTables[i]} older than {_retentionDays} days.");
				}
				catch (Exception ex)
				{
					Log.Warning($"LogCleanupService: Failed to clean {LogTables[i]}: {ex.Message}");
				}
			}
		}

		private int DeleteOldRows(string tableName, string timestampColumn, DateTime cutoff)
		{
			var totalDeleted = 0;
			const int batchSize = 5000;

			// Delete in batches to avoid long-running locks
			while (true)
			{
				using (var conn = _database.GetConnection())
				using (var cmd = new MySqlCommand($"DELETE FROM `{tableName}` WHERE `{timestampColumn}` < @cutoff LIMIT {batchSize}", conn))
				{
					cmd.Parameters.AddWithValue("@cutoff", cutoff);
					var deleted = cmd.ExecuteNonQuery();
					totalDeleted += deleted;

					if (deleted < batchSize)
						break;
				}

				// Small pause between batches to avoid starving other queries
				Thread.Sleep(50);
			}

			return totalDeleted;
		}

		public void Dispose()
		{
			_timer?.Dispose();
			Log.Info("LogCleanupService stopped.");
		}
	}
}
