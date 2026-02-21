using System;
using System.Threading;
using System.Threading.Tasks;
using Melia.Zone.Database;
using MySqlConnector;
using Yggdrasil.Logging;

namespace Melia.Zone.Services
{
	/// <summary>
	/// Service that periodically cleans up orphaned items from the database.
	/// Orphaned items are items that exist in the 'items' table but are not
	/// linked to any inventory, personal storage, or team storage.
	/// </summary>
	public class OrphanCleanupService : IDisposable
	{
		private readonly ZoneDb _database;
		private readonly Timer _timer;
		private readonly int _batchSize;
		private volatile bool _isRunning = false;

		/// <summary>
		/// Creates a new OrphanCleanupService.
		/// </summary>
		/// <param name="database">The database instance to use.</param>
		/// <param name="batchSize">How many items to delete per batch.</param>
		public OrphanCleanupService(ZoneDb database, int batchSize)
		{
			_database = database;
			_batchSize = batchSize;

			// Run initial cleanup after 5 minutes (let server stabilize)
			// Subsequent cleanups are triggered by AutoSaveService after full cycles
			_timer = new Timer(this.DoCleanup, null, TimeSpan.FromMinutes(5), Timeout.InfiniteTimeSpan);

			Log.Info($"OrphanCleanupService initialized: Initial cleanup in 5 minutes, then triggered by AutoSaveService. Batch size: {batchSize}.");
		}

		private async void DoCleanup(object state)
		{
			if (_isRunning)
			{
				Log.Warning("OrphanCleanupService: Previous cleanup still running. Skipping.");
				return;
			}

			_isRunning = true;

			try
			{
				Log.Info("OrphanCleanupService: Starting orphan cleanup...");

				var totalItemsDeleted = 0;
				var totalPropsDeleted = 0;
				var batches = 0;

				using (var conn = _database.GetConnection())
				{
					while (true)
					{
						batches++;

						// Step 1: Find orphaned item IDs (items not in any inventory)
						var orphanIds = await this.FindOrphanedItemIds(conn, _batchSize);

						if (orphanIds.Length == 0)
						{
							Log.Info("OrphanCleanupService: No more orphaned items found.");
							break;
						}

						// Step 2: Delete properties for these items first
						var propsDeleted = await this.DeleteItemProperties(conn, orphanIds);
						totalPropsDeleted += propsDeleted;

						// Step 3: Delete the orphaned items
						var itemsDeleted = await this.DeleteItems(conn, orphanIds);
						totalItemsDeleted += itemsDeleted;

						Log.Debug($"OrphanCleanupService: Batch {batches} - Deleted {itemsDeleted} items and {propsDeleted} properties.");

						// Small delay between batches to avoid hammering the DB
						await Task.Delay(100);

						// Safety limit - don't run forever
						if (batches >= 1000)
						{
							Log.Warning("OrphanCleanupService: Reached batch limit (1000). Stopping for this cycle.");
							break;
						}
					}
				}

				if (totalItemsDeleted > 0 || totalPropsDeleted > 0)
				{
					Log.Info($"OrphanCleanupService: Cleanup complete. Deleted {totalItemsDeleted} orphaned items and {totalPropsDeleted} properties in {batches} batches.");
				}
				else
				{
					Log.Info("OrphanCleanupService: Cleanup complete. No orphans found.");
				}
			}
			catch (Exception ex)
			{
				Log.Error($"OrphanCleanupService: Error during cleanup: {ex}");
			}
			finally
			{
				_isRunning = false;
			}
		}

		/// <summary>
		/// Finds orphaned item IDs that are not linked to any inventory, storage, or market.
		/// </summary>
		private async Task<long[]> FindOrphanedItemIds(MySqlConnection conn, int limit)
		{
			var sql = @"
				SELECT i.itemUniqueId
				FROM items i
				WHERE NOT EXISTS (SELECT 1 FROM inventory WHERE itemId = i.itemUniqueId)
				  AND NOT EXISTS (SELECT 1 FROM storage_personal WHERE itemId = i.itemUniqueId)
				  AND NOT EXISTS (SELECT 1 FROM storage_team WHERE itemId = i.itemUniqueId)
				  AND NOT EXISTS (SELECT 1 FROM market_items WHERE itemUniqueId = i.itemUniqueId)
				LIMIT @limit";

			using (var cmd = new MySqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@limit", limit);

				var ids = new System.Collections.Generic.List<long>();

				using (var reader = await cmd.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						ids.Add(reader.GetInt64(0));
					}
				}

				return ids.ToArray();
			}
		}

		/// <summary>
		/// Deletes item properties for the given item IDs.
		/// </summary>
		private async Task<int> DeleteItemProperties(MySqlConnection conn, long[] itemIds)
		{
			if (itemIds.Length == 0)
				return 0;

			var parameters = new string[itemIds.Length];
			for (int i = 0; i < itemIds.Length; i++)
				parameters[i] = $"@id{i}";

			var sql = $"DELETE FROM item_properties WHERE itemId IN ({string.Join(",", parameters)})";

			using (var cmd = new MySqlCommand(sql, conn))
			{
				for (int i = 0; i < itemIds.Length; i++)
					cmd.Parameters.AddWithValue(parameters[i], itemIds[i]);

				return await cmd.ExecuteNonQueryAsync();
			}
		}

		/// <summary>
		/// Deletes items with the given IDs.
		/// </summary>
		private async Task<int> DeleteItems(MySqlConnection conn, long[] itemIds)
		{
			if (itemIds.Length == 0)
				return 0;

			var parameters = new string[itemIds.Length];
			for (int i = 0; i < itemIds.Length; i++)
				parameters[i] = $"@id{i}";

			var sql = $"DELETE FROM items WHERE itemUniqueId IN ({string.Join(",", parameters)})";

			using (var cmd = new MySqlCommand(sql, conn))
			{
				for (int i = 0; i < itemIds.Length; i++)
					cmd.Parameters.AddWithValue(parameters[i], itemIds[i]);

				return await cmd.ExecuteNonQueryAsync();
			}
		}

		/// <summary>
		/// Manually triggers a cleanup cycle. Useful for testing or admin commands.
		/// </summary>
		public void TriggerCleanup()
		{
			if (_isRunning)
			{
				Log.Warning("OrphanCleanupService: Cleanup already running.");
				return;
			}

			Task.Run(() => this.DoCleanup(null));
		}

		public void Dispose()
		{
			_timer?.Dispose();
			Log.Info("OrphanCleanupService stopped.");
		}
	}
}
