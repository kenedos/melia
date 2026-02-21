using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Storages;
using MySqlConnector;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Database
{
	public partial class ZoneDb
	{
		#region Internal Save Methods (Transaction-aware)

		/// <summary>
		/// INTERNAL USE: Saves character's inventory (equipped and non-equipped) within an existing transaction.
		/// This version is optimized for performance using batch database operations.
		/// </summary>
		internal void InternalSaveCharacterItems(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			using (Debug.Profile($"SaveItems.Total: {character.Name}", 500))
			{
				var allItemsInInventory = character.Inventory.GetItems().Values
					.Concat(character.Inventory.GetEquip().Values)
					.Concat(character.Inventory.GetCards().Values)
					.Where(item => item != null && !(item is DummyEquipItem) && !InventoryDefaults.EquipItems.Contains(item.Id))
					.ToList();

				if (!allItemsInInventory.Any())
				{
					// Clear inventory links if no items exist
					using (var cmd = new MySqlCommand("DELETE FROM `inventory` WHERE `characterId` = @characterId", conn, trans))
					{
						cmd.Parameters.AddWithValue("@characterId", character.DbId);
						cmd.ExecuteNonQuery();
					}
					return;
				}

				// Step 1 & 2: Persist all items in memory to the `items` table.
				// After this call, every item in `allItemsInInventory` has a valid, non-zero DbId.
				this.PersistItemBatch(allItemsInInventory, conn, trans);

				// Step 3: Save properties for all items.
				this.InternalSaveProperties("item_properties", "itemId", allItemsInInventory, conn, trans);

				// --- Step 4: Reconcile Inventory - The Critical Part ---

				// Get the complete set of item IDs that SHOULD be in the character's inventory table.
				// We already have this list from above (allItemsInInventory).
				var currentValidItemDbIds = new HashSet<long>(allItemsInInventory.Select(i => i.DbId));

				// Get the set of item IDs that ARE CURRENTLY linked to the character in the database.
				var linkedItemDbIdsInDb = new HashSet<long>();
				using (var cmd = new MySqlCommand("SELECT itemId FROM `inventory` WHERE `characterId` = @characterId FOR UPDATE", conn, trans))
				{
					// "FOR UPDATE" locks the selected rows to prevent another transaction from modifying them.
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							linkedItemDbIdsInDb.Add(reader.GetInt64(0));
						}
					}
				}

				// A) Find items to UNLINK: They are in the DB but not in memory anymore.
				var itemsToUnlink = linkedItemDbIdsInDb.Except(currentValidItemDbIds).ToList();
				if (itemsToUnlink.Any())
				{
					var unlinkParams = itemsToUnlink.Select((id, i) => $"@unlink{i}").ToArray();
					using (var cmd = new MySqlCommand($"DELETE FROM `inventory` WHERE `characterId` = @characterId AND `itemId` IN ({string.Join(",", unlinkParams)})", conn, trans))
					{
						cmd.Parameters.AddWithValue("@characterId", character.DbId);
						for (int k = 0; k < itemsToUnlink.Count; k++) cmd.Parameters.AddWithValue(unlinkParams[k], itemsToUnlink[k]);
						cmd.ExecuteNonQuery();
					}
					// NOTE: We do NOT delete from the `items` table here. The item might have been traded
					// to another player. Orphan cleanup should be a separate maintenance task.
				}

				// B) Find items to LINK/UPDATE: They are in memory.
				using (var batchLinks = new BatchInsertCommand("inventory",
					"ON DUPLICATE KEY UPDATE `sort` = VALUES(`sort`), `equipSlot` = VALUES(`equipSlot`)",
					conn, trans))
				{
					var i = 0;
					// Add non-equipped items
					foreach (var itemKV in character.Inventory.GetItems().OrderBy(a => a.Key))
					{
						var item = itemKV.Value;
						// Skip items that weren't persisted (filtered out or null)
						if (item == null || !currentValidItemDbIds.Contains(item.DbId))
							continue;

						batchLinks.AddRow(new Dictionary<string, object> {
							{"characterId", character.DbId},
							{"itemId", item.DbId},
							{"sort", i++},
							{"equipSlot", 0x7F}
						});
					}

					// Add equipped items
					foreach (var equipKVP in character.Inventory.GetEquip())
					{
						var item = equipKVP.Value;
						// Skip items that weren't persisted (filtered out, DummyEquipItem, or default equip)
						if (item == null || !currentValidItemDbIds.Contains(item.DbId))
							continue;

						batchLinks.AddRow(new Dictionary<string, object> {
							{"characterId", character.DbId},
							{"itemId", item.DbId},
							{"sort", 0},
							{"equipSlot", (byte)equipKVP.Key}
						});
					}

					// Add equipped cards (using slots 100-115 for card slots 1-15)
					// Note: Using 100-115 to fit within TINYINT range (max 127)
					foreach (var cardKVP in character.Inventory.GetCards())
					{
						var card = cardKVP.Value;
						// Skip cards that weren't persisted (filtered out or null)
						if (card == null || !currentValidItemDbIds.Contains(card.DbId))
							continue;

						// Map card slots 1-15 to inventory equipSlot values 100-115
						var cardEquipSlot = (byte)(100 + cardKVP.Key - 1);
						batchLinks.AddRow(new Dictionary<string, object> {
							{"characterId", character.DbId},
							{"itemId", card.DbId},
							{"sort", 0},
							{"equipSlot", cardEquipSlot}
						});
					}

					if (batchLinks.HasRows)
						batchLinks.Execute();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves storage items within an existing transaction.
		/// </summary>
		internal void InternalSaveStorage(Storage storage, string tableName, string idFieldName, long id, MySqlConnection conn, MySqlTransaction trans)
		{
			var itemsToSave = storage.GetItems();

			// Persist all items in the storage first.
			this.PersistItemBatch(itemsToSave.Values, conn, trans);
			this.InternalSaveProperties("item_properties", "itemId", itemsToSave.Values.ToList(), conn, trans);

			// UPSERT storage links (avoids gap lock deadlocks from DELETE-all)
			var itemIdsInMemory = new HashSet<long>();
			if (itemsToSave.Any())
			{
				using (var batchLinks = new BatchInsertCommand(tableName, "ON DUPLICATE KEY UPDATE `position` = VALUES(`position`)", conn, trans))
				{
					foreach (var itemKVP in itemsToSave)
					{
						itemIdsInMemory.Add(itemKVP.Value.DbId);
						batchLinks.AddRow(new Dictionary<string, object>
						{
							{ idFieldName, id },
							{ "itemId", itemKVP.Value.DbId },
							{ "position", itemKVP.Key }
						});
					}

					if (batchLinks.HasRows)
						batchLinks.Execute();
				}
			}

			// Only delete storage links that were removed from memory
			var itemIdsInDb = new HashSet<long>();
			using (var cmd = new MySqlCommand($"SELECT `itemId` FROM `{tableName}` WHERE `{idFieldName}` = @id", conn, trans))
			{
				cmd.Parameters.AddWithValue("@id", id);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						itemIdsInDb.Add(reader.GetInt64(0));
				}
			}

			var linksToDelete = itemIdsInDb.Except(itemIdsInMemory).ToList();
			if (linksToDelete.Any())
			{
				var delParams = linksToDelete.Select((_, i) => $"@del{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `{tableName}` WHERE `{idFieldName}` = @id AND `itemId` IN ({string.Join(",", delParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@id", id);
					for (var i = 0; i < linksToDelete.Count; i++)
						cmd.Parameters.AddWithValue(delParams[i], linksToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves owner's variables within an existing transaction.
		/// Uses UPSERT pattern to avoid gap lock deadlocks (only for owner-scoped tables).
		/// </summary>
		internal void InternalSaveVariables(Variables vars, string tableName, string ownerField, long ownerId, MySqlConnection conn, MySqlTransaction trans)
		{
			var checkOwner = !string.IsNullOrEmpty(ownerField);
			var varList = vars.GetList().Where(kv => kv.Value != null).ToList();

			if (!varList.Any())
			{
				// No variables, clear all for this owner
				var where = checkOwner ? $"`{ownerField}` = @ownerId" : "1";
				using (var mc = new MySqlCommand($"DELETE FROM `{tableName}` WHERE {where}", conn, trans))
				{
					if (checkOwner)
						mc.Parameters.AddWithValue("@ownerId", ownerId);
					mc.ExecuteNonQuery();
				}
				return;
			}

			// For global variables (no owner), use original DELETE+INSERT pattern
			// For owner-scoped variables, use UPSERT pattern to avoid deadlocks
			if (!checkOwner)
			{
				// Global variables - use simple DELETE ALL + INSERT (no deadlock risk as there's only one global save)
				using (var mc = new MySqlCommand($"DELETE FROM `{tableName}`", conn, trans))
				{
					mc.ExecuteNonQuery();
				}

				foreach (var varKV in varList)
				{
					if (varKV.Value == null) continue;
					var varValue = varKV.Value;
					var varKey = varKV.Key;

					string type = GetVariableType(varValue);
					if (type == null)
					{
						Log.Warning("InternalSaveVariables: Skipping variable '{0}', unsupported type '{1}'.", varKey, varValue.GetType().Name);
						continue;
					}

					var val = SerializeVariableValue(varValue, type);
					if (val == null || val.Length > (1 << 24) - 1)
					{
						Log.Warning("InternalSaveVariables: Skipping variable '{0}', value too big or invalid.", varKey);
						continue;
					}

					using (var cmd = new InsertCommand($"INSERT INTO `{tableName}` {{0}}", conn, trans))
					{
						cmd.Set("name", varKey);
						cmd.Set("type", type);
						cmd.Set("value", val);
						cmd.Execute();
					}
				}
				return;
			}

			var varNamesInMemory = new HashSet<string>(varList.Select(kv => kv.Key));

			foreach (var varKV in varList)
			{
				if (varKV.Value == null) continue;
				var varValue = varKV.Value;
				var varKey = varKV.Key;

				var type = GetVariableType(varValue);
				if (type == null)
				{
					Log.Warning("InternalSaveVariables: Skipping variable '{0}', unsupported type '{1}'.", varKey, varValue.GetType().Name);
					continue;
				}

				var val = SerializeVariableValue(varValue, type);

				if (val.Length > (1 << 24) - 1) // Check length (mediumtext limit)
				{
					Log.Warning("InternalSaveVariables: Skipping variable '{0}', value too big.", varKey);
					varNamesInMemory.Remove(varKey); // Don't try to keep this in DB either
					continue;
				}

				// Use UPSERT to avoid gap lock deadlocks
				using (var cmd = new MySqlCommand(
					$"INSERT INTO `{tableName}` (`{ownerField}`, `name`, `type`, `value`) VALUES (@ownerId, @name, @type, @value) " +
					$"ON DUPLICATE KEY UPDATE `type` = VALUES(`type`), `value` = VALUES(`value`)", conn, trans))
				{
					cmd.Parameters.AddWithValue("@ownerId", ownerId);
					cmd.Parameters.AddWithValue("@name", varKey);
					cmd.Parameters.AddWithValue("@type", type);
					cmd.Parameters.AddWithValue("@value", val);
					cmd.ExecuteNonQuery();
				}
			}

			// Only delete variables that were removed (not all variables)
			var varNamesInDb = new HashSet<string>();
			using (var cmd = new MySqlCommand($"SELECT `name` FROM `{tableName}` WHERE `{ownerField}` = @ownerId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@ownerId", ownerId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						varNamesInDb.Add(reader.GetString(0));
				}
			}

			var varsToDelete = varNamesInDb.Except(varNamesInMemory).ToList();
			if (varsToDelete.Any())
			{
				var deleteParams = varsToDelete.Select((name, i) => $"@name{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `{tableName}` WHERE `{ownerField}` = @ownerId AND `name` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@ownerId", ownerId);
					for (var i = 0; i < varsToDelete.Count; i++)
						cmd.Parameters.AddWithValue(deleteParams[i], varsToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves character's session objects within an existing transaction.
		/// </summary>
		internal void InternalSaveSessionObjects(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			using (var cmdDel = new MySqlCommand("DELETE FROM `session_objects_properties` WHERE `characterId` = @characterId", conn, trans))
			{
				cmdDel.Parameters.AddWithValue("@characterId", character.DbId);
				cmdDel.ExecuteNonQuery();
			}

			var sessionObjects = character.SessionObjects.GetList();
			if (!sessionObjects.Any()) return;

			using (var batch = new BatchInsertCommand("session_objects_properties", null, conn, trans))
			{
				foreach (var sessionObject in sessionObjects)
				{
					foreach (var property in sessionObject.Properties.GetAll())
					{
						var typeStr = property is FloatProperty ? "f" : "s";
						var valueStr = property.Serialize();

						batch.AddRow(new Dictionary<string, object>
						{
							{ "characterId", character.DbId },
							{ "sessionObjectId", sessionObject.Id },
							{ "name", property.Ident },
							{ "type", typeStr },
							{ "value", valueStr }
						});
					}
				}

				if (batch.HasRows)
					batch.Execute();
			}
		}

		/// <summary>
		/// A helper method to persist a batch of items (INSERT new, UPDATE existing) and their properties.
		/// Uses INSERT ... ON DUPLICATE KEY UPDATE to handle items that may or may not exist in DB.
		/// </summary>
		private void PersistItemBatch(ICollection<Item> items, MySqlConnection conn, MySqlTransaction trans)
		{
			if (!items.Any()) return;

			using (Debug.Profile($"PersistItemBatch ({items.Count})", 100))
			{
				// Step 1: Insert NEW items (DbId == 0) to get their IDs.
				// This is done one-by-one to reliably get the last inserted ID.
				var newItems = items.Where(i => i.DbId == 0).ToList();
				if (newItems.Any())
				{
					foreach (var item in newItems)
					{
						try
						{
							using (var cmd = new InsertCommand("INSERT INTO `items` {0}", conn, trans))
							{
								cmd.Set("itemId", item.Id);
								cmd.Set("amount", item.Amount);
								cmd.Execute();
								item.DbId = cmd.LastId; // Assign the correct ID immediately.

								if (item.DbId <= 0)
								{
									// This is a critical failure, the transaction should be rolled back.
									throw new InvalidOperationException($"Failed to retrieve a valid database ID for new item {item.Id}.");
								}
							}
						}
						catch (Exception ex)
						{
							Log.Error($"Failed to insert single new item {item.Id}: {ex.Message}");
							// Re-throw to ensure the transaction is rolled back.
							throw;
						}
					}
				}

				// Step 2: Use INSERT ... ON DUPLICATE KEY UPDATE for existing items.
				// This handles the case where an item has a DbId but doesn't exist in the database.
				var existingItems = items.Where(i => i.DbId > 0).ToList();
				if (existingItems.Any())
				{
					try
					{
						using (var batch = new BatchInsertCommand("items",
							"ON DUPLICATE KEY UPDATE `itemId` = VALUES(`itemId`), `amount` = VALUES(`amount`)",
							conn, trans))
						{
							foreach (var item in existingItems)
							{
								batch.AddRow(new Dictionary<string, object>
								{
									{ "itemUniqueId", item.DbId },
									{ "itemId", item.Id },
									{ "amount", item.Amount }
								});
							}

							if (batch.HasRows)
								batch.Execute();
						}
					}
					catch (Exception ex)
					{
						Log.Error($"Failed to upsert existing items in batch: {ex.Message}");
						throw;
					}
				}

				// Final validation: ensure ALL items now have valid DbIds
				var itemsStillWithoutIds = items.Where(i => i.DbId <= 0).ToList();
				if (itemsStillWithoutIds.Any())
				{
					var itemInfo = string.Join(", ", itemsStillWithoutIds.Select(i => $"ItemId:{i.Id}"));
					throw new InvalidOperationException($"After PersistItemBatch, these items still have invalid DbIds: {itemInfo}");
				}
			}
		}
		internal void InternalSaveProperties(string databaseName, string idName, List<Item> items, MySqlConnection conn, MySqlTransaction trans)
		{
			if (!items.Any()) return;

			var allProperties = items
				.SelectMany(item => item.Properties.GetAll()
					.Where(p => !(p is IUnsettableProperty))
					.Select(prop => new { ItemId = item.DbId, Property = prop })
				).ToList();

			if (!allProperties.Any())
			{
				return;
			}

			// Build set of (itemId, propName) pairs in memory for later comparison
			var propsInMemory = new Dictionary<long, HashSet<string>>();
			foreach (var item in items)
			{
				var names = new HashSet<string>();
				foreach (var property in item.Properties.GetAll())
				{
					if (property is IUnsettableProperty) continue;
					names.Add(property.Ident);
				}
				propsInMemory[item.DbId] = names;
			}

			// UPSERT all properties (avoids gap lock deadlocks)
			using (var batch = new BatchInsertCommand(databaseName, "ON DUPLICATE KEY UPDATE `type` = VALUES(`type`), `value` = VALUES(`value`)", conn, trans))
			{
				foreach (var item in items)
				{
					foreach (var property in item.Properties.GetAll())
					{
						if (property is IUnsettableProperty) continue;
						batch.AddRow(new Dictionary<string, object>
						{
							{ idName, item.DbId },
							{ "name", property.Ident },
							{ "type", property is FloatProperty ? "f" : "s" },
							{ "value", property.Serialize() }
						});
					}
				}

				if (batch.HasRows)
					batch.Execute();
			}

			// Only delete properties that were removed from memory
			var itemIds = items.Select(i => i.DbId).Distinct().ToList();
			var idParams = itemIds.Select((id, i) => $"@id{i}").ToArray();

			var propsInDb = new Dictionary<long, HashSet<string>>();
			using (var cmd = new MySqlCommand($"SELECT `{idName}`, `name` FROM `{databaseName}` WHERE `{idName}` IN ({string.Join(",", idParams)})", conn, trans))
			{
				for (var i = 0; i < itemIds.Count; i++)
					cmd.Parameters.AddWithValue(idParams[i], itemIds[i]);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var dbItemId = reader.GetInt64(0);
						var dbName = reader.GetString(1);
						if (!propsInDb.TryGetValue(dbItemId, out var set))
							propsInDb[dbItemId] = set = new HashSet<string>();
						set.Add(dbName);
					}
				}
			}

			foreach (var itemId in itemIds)
			{
				if (!propsInDb.TryGetValue(itemId, out var dbNames)) continue;
				var memNames = propsInMemory.TryGetValue(itemId, out var m) ? m : new HashSet<string>();
				var toDelete = dbNames.Except(memNames).ToList();
				if (!toDelete.Any()) continue;

				var delParams = toDelete.Select((name, i) => $"@name{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `{databaseName}` WHERE `{idName}` = @itemId AND `name` IN ({string.Join(",", delParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@itemId", itemId);
					for (var i = 0; i < toDelete.Count; i++)
						cmd.Parameters.AddWithValue(delParams[i], toDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}
		internal void InternalSaveProperties(string databaseName, string idName, long id, Properties properties, MySqlConnection conn, MySqlTransaction trans)
		{
			var allProperties = properties.GetAll()
				.Where(p => p is not IUnsettableProperty)
				.Where(p => databaseName != "character_properties" || !BuffHandler.IsBuffTransientProperty(p.Ident))
				.ToList();

			if (!allProperties.Any())
			{
				return; // Nothing to save.
			}

			// --- Step 1: Conditional Snapshotting ---
			string logTableName = GetLogTableName(databaseName);
			string logIdName = GetLogIdName(databaseName);

			if (logTableName != null && logIdName != null)
			{
				try
				{
					// Snapshot all properties we are about to overwrite.
					var propNamesToSnapshot = allProperties.Select(p => p.Ident).ToList();

					if (propNamesToSnapshot.Any())
					{
						var snapshotParams = propNamesToSnapshot.Select((p, i) => $"@p{i}").ToArray();

						var snapshotSql = $"INSERT INTO `{logTableName}` ({logIdName}, name, type, value, backupReason) " +
										  $"SELECT @id, `name`, `type`, `value`, @backupReason " +
										  $"FROM `{databaseName}` WHERE `{idName}` = @id AND `name` IN ({string.Join(",", snapshotParams)})";

						using (var cmdSnapshot = new MySqlCommand(snapshotSql, conn, trans))
						{
							cmdSnapshot.Parameters.AddWithValue("@id", id);
							cmdSnapshot.Parameters.AddWithValue("@backupReason", $"pre_save_{databaseName}");
							for (var i = 0; i < propNamesToSnapshot.Count; i++)
							{
								cmdSnapshot.Parameters.AddWithValue(snapshotParams[i], propNamesToSnapshot[i]);
							}
							cmdSnapshot.ExecuteNonQuery();
						}
					}
				}
				catch (Exception ex)
				{
					Log.Warning($"Failed to snapshot properties for {idName}:{id} from {databaseName}. Continuing with save. Error: {ex.Message}");
				}
			}

			// --- Step 2: Use UPSERT to insert/update properties (avoids gap lock deadlocks) ---
			var propNamesInMemory = new HashSet<string>(allProperties.Select(p => p.Ident));

			using (var batch = new BatchInsertCommand(
				databaseName,
				"ON DUPLICATE KEY UPDATE `type` = VALUES(`type`), `value` = VALUES(`value`)",
				conn,
				trans))
			{
				foreach (var property in allProperties)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ idName, id },
						{ "name", property.Ident },
						{ "type", property is FloatProperty ? "f" : "s" },
						{ "value", property.Serialize() }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}

			// --- Step 3: Only delete properties that were removed ---
			var propNamesInDb = new HashSet<string>();
			using (var cmd = new MySqlCommand($"SELECT `name` FROM `{databaseName}` WHERE `{idName}` = @id", conn, trans))
			{
				cmd.Parameters.AddWithValue("@id", id);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						propNamesInDb.Add(reader.GetString(0));
				}
			}

			var propsToDelete = propNamesInDb.Except(propNamesInMemory).ToList();
			if (propsToDelete.Any())
			{
				var deleteParams = propsToDelete.Select((name, i) => $"@name{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `{databaseName}` WHERE `{idName}` = @id AND `name` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@id", id);
					for (var i = 0; i < propsToDelete.Count; i++)
						cmd.Parameters.AddWithValue(deleteParams[i], propsToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}
		/// <summary>
		/// Helper method to determine the log table name based on the main table name.
		/// </summary>
		private static string GetLogTableName(string databaseName)
		{
			if (databaseName.Contains("item"))
				return "item_properties_log";
			else if (databaseName.Contains("character"))
				return "character_properties_log";
			else if (databaseName.Contains("account"))
				return "account_properties_log";
			else if (databaseName.Contains("etc"))
				return "character_etc_properties_log";

			return null;
		}

		/// <summary>
		/// Helper method to determine the log ID field name based on the main table name.
		/// </summary>
		private static string GetLogIdName(string databaseName)
		{
			if (databaseName.Contains("item"))
				return "itemId";
			else if (databaseName.Contains("character"))
				return "characterId";
			else if (databaseName.Contains("account"))
				return "accountId";
			else if (databaseName.Contains("etc"))
				return "characterId";

			return null;
		}

		/// <summary>
		/// INTERNAL USE: Saves character's jobs within an existing transaction.
		/// Enhanced with better error handling.
		/// </summary>
		internal void InternalSaveJobs(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			var jobsToSave = character.Jobs.GetList();
			if (!jobsToSave.Any())
			{
				// Clear jobs if none exist
				using (var cmd = new MySqlCommand("DELETE FROM `jobs` WHERE `characterId` = @characterId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					cmd.ExecuteNonQuery();
				}
				return;
			}

			// Use BatchInsert with ON DUPLICATE KEY UPDATE to handle both new and existing jobs atomically.
			using (var batch = new BatchInsertCommand(
				"jobs",
				"ON DUPLICATE KEY UPDATE `circle`=VALUES(`circle`), `skillPoints`=VALUES(`skillPoints`), `totalExp`=VALUES(`totalExp`), `selectionDate`=VALUES(`selectionDate`), `advDate`=VALUES(`advDate`)",
				conn,
				trans))
			{
				foreach (var job in jobsToSave)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ "characterId", character.DbId },
						{ "jobId", job.Id },
						{ "circle", job.Circle },
						{ "skillPoints", job.SkillPoints },
						{ "totalExp", job.TotalExp },
						{ "selectionDate", job.SelectionDate },
						{ "advDate", job.AdvancementDate }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}

			// Handle job removal (if character has job reset functionality)
			var jobIdsInMemory = new HashSet<int>(jobsToSave.Select(j => (int)j.Id));
			var jobIdsInDb = new HashSet<int>();
			using (var cmd = new MySqlCommand("SELECT jobId FROM `jobs` WHERE `characterId` = @charId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@charId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						jobIdsInDb.Add(reader.GetInt32(0));
				}
			}

			var jobsToDelete = jobIdsInDb.Except(jobIdsInMemory).ToList();
			if (jobsToDelete.Any())
			{
				var deleteParams = jobsToDelete.Select((id, i) => $"@id{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `jobs` WHERE `characterId` = @charId AND `jobId` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@charId", character.DbId);
					for (var i = 0; i < jobsToDelete.Count; i++)
						cmd.Parameters.AddWithValue(deleteParams[i], jobsToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves character's skills within an existing transaction.
		/// Uses UPSERT pattern to avoid gap lock deadlocks.
		/// </summary>
		internal void InternalSaveSkills(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			var skillsToSave = character.Skills.GetList().Where(skill => skill.LevelByDB > 0).ToList();

			if (!skillsToSave.Any())
			{
				// No skills to save, clear all skills for this character
				using (var cmd = new MySqlCommand("DELETE FROM `skills` WHERE `characterId` = @characterId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					cmd.ExecuteNonQuery();
				}
				return;
			}

			// Use UPSERT to insert or update skills atomically
			using (var batch = new BatchInsertCommand(
				"skills",
				"ON DUPLICATE KEY UPDATE `level`=VALUES(`level`)",
				conn,
				trans))
			{
				foreach (var skill in skillsToSave)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ "characterId", character.DbId },
						{ "id", skill.Id },
						{ "level", skill.LevelByDB }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}

			// Only delete skills that were removed (not all skills)
			var skillIdsInMemory = new HashSet<int>(skillsToSave.Select(s => (int)s.Id));
			var skillIdsInDb = new HashSet<int>();
			using (var cmd = new MySqlCommand("SELECT `id` FROM `skills` WHERE `characterId` = @charId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@charId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						skillIdsInDb.Add(reader.GetInt32(0));
				}
			}

			var skillsToDelete = skillIdsInDb.Except(skillIdsInMemory).ToList();
			if (skillsToDelete.Any())
			{
				var deleteParams = skillsToDelete.Select((id, i) => $"@id{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `skills` WHERE `characterId` = @charId AND `id` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@charId", character.DbId);
					for (var i = 0; i < skillsToDelete.Count; i++)
						cmd.Parameters.AddWithValue(deleteParams[i], skillsToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves character's abilities within an existing transaction.
		/// Uses UPSERT pattern to avoid gap lock deadlocks.
		/// </summary>
		internal void InternalSaveAbilities(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			var abilitiesToSave = character.Abilities.GetList();

			if (!abilitiesToSave.Any())
			{
				// No abilities to save, clear all abilities for this character
				using (var cmd = new MySqlCommand("DELETE FROM `abilities` WHERE `characterId` = @characterId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					cmd.ExecuteNonQuery();
				}
				return;
			}

			// Use UPSERT to insert or update abilities atomically
			using (var batch = new BatchInsertCommand(
				"abilities",
				"ON DUPLICATE KEY UPDATE `level`=VALUES(`level`), `active`=VALUES(`active`)",
				conn,
				trans))
			{
				foreach (var ability in abilitiesToSave)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ "characterId", character.DbId },
						{ "id", ability.Id },
						{ "level", ability.Level },
						{ "active", ability.Active }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}

			// Only delete abilities that were removed (not all abilities)
			var abilityIdsInMemory = new HashSet<int>(abilitiesToSave.Select(a => (int)a.Id));
			var abilityIdsInDb = new HashSet<int>();
			using (var cmd = new MySqlCommand("SELECT `id` FROM `abilities` WHERE `characterId` = @charId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@charId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						abilityIdsInDb.Add(reader.GetInt32(0));
				}
			}

			var abilitiesToDelete = abilityIdsInDb.Except(abilityIdsInMemory).ToList();
			if (abilitiesToDelete.Any())
			{
				var deleteParams = abilitiesToDelete.Select((id, i) => $"@id{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `abilities` WHERE `characterId` = @charId AND `id` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@charId", character.DbId);
					for (var i = 0; i < abilitiesToDelete.Count; i++)
						cmd.Parameters.AddWithValue(deleteParams[i], abilitiesToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves character's buffs within an existing transaction.
		/// Enhanced with better orphan handling and batch operations.
		/// </summary>
		internal void InternalSaveBuffs(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			// First, get IDs of buffs that will be deleted to clean up their variables
			var buffIdsToDelete = new List<long>();
			using (var cmdGetIds = new MySqlCommand("SELECT buffId FROM `buffs` WHERE `characterId` = @characterId", conn, trans))
			{
				cmdGetIds.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = cmdGetIds.ExecuteReader())
				{
					while (reader.Read())
					{
						buffIdsToDelete.Add(reader.GetInt64(0));
					}
				}
			}

			// Clean up variables for buffs that will be deleted
			if (buffIdsToDelete.Any())
			{
				var deleteParams = buffIdsToDelete.Select((id, i) => $"@bid{i}").ToArray();
				using (var cmdDelVars = new MySqlCommand($"DELETE FROM `vars_buffs` WHERE `buffId` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					for (int i = 0; i < buffIdsToDelete.Count; i++)
						cmdDelVars.Parameters.AddWithValue(deleteParams[i], buffIdsToDelete[i]);
					cmdDelVars.ExecuteNonQuery();
				}
			}

			// Delete existing buffs
			using (var cmdDel = new MySqlCommand("DELETE FROM `buffs` WHERE `characterId` = @characterId", conn, trans))
			{
				cmdDel.Parameters.AddWithValue("@characterId", character.DbId);
				cmdDel.ExecuteNonQuery();
			}

			var savableBuffs = character.Buffs.GetList().Where(buff => buff.Data.Save).ToList();
			if (!savableBuffs.Any()) return;

			// Insert new buffs and save their variables
			foreach (var buff in savableBuffs)
			{
				long buffDbId = 0;
				using (var cmd = new InsertCommand("INSERT INTO `buffs` {0}", conn, trans))
				{
					cmd.Set("characterId", character.DbId);
					cmd.Set("classId", buff.Id);
					cmd.Set("numArg1", buff.NumArg1);
					cmd.Set("numArg2", buff.NumArg2);
					cmd.Set("numArg3", buff.NumArg3);
					cmd.Set("numArg4", buff.NumArg4);
					cmd.Set("numArg5", buff.NumArg5);
					cmd.Set("duration", buff.Duration);
					cmd.Set("runTime", buff.RunTime);
					cmd.Set("skillId", (int)buff.SkillId);
					cmd.Set("overbuffCount", buff.OverbuffCounter);
					cmd.Execute();
					buffDbId = cmd.LastId;
				}

				// Save buff variables *within the transaction*
				if (buff.Vars != null)
					this.InternalSaveVariables(buff.Vars, "vars_buffs", "buffId", buffDbId, conn, trans);
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves character's cooldowns within an existing transaction.
		/// Uses UPSERT pattern to avoid gap lock deadlocks.
		/// </summary>
		internal void InternalSaveCooldowns(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			var cooldownComponent = character.Components.Get<CooldownComponent>();
			if (cooldownComponent == null)
			{
				// No cooldown component, clear all cooldowns for this character
				using (var cmd = new MySqlCommand("DELETE FROM `cooldowns` WHERE `characterId` = @characterId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					cmd.ExecuteNonQuery();
				}
				return;
			}

			var activeCooldowns = cooldownComponent.GetAll().Where(cd => cd.Remaining > TimeSpan.Zero).ToList();

			if (!activeCooldowns.Any())
			{
				// No active cooldowns, clear all cooldowns for this character
				using (var cmd = new MySqlCommand("DELETE FROM `cooldowns` WHERE `characterId` = @characterId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					cmd.ExecuteNonQuery();
				}
				return;
			}

			// Use UPSERT to insert or update cooldowns atomically
			using (var batch = new BatchInsertCommand(
				"cooldowns",
				"ON DUPLICATE KEY UPDATE `remaining`=VALUES(`remaining`), `duration`=VALUES(`duration`), `startTime`=VALUES(`startTime`)",
				conn,
				trans))
			{
				foreach (var cooldown in activeCooldowns)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ "characterId", character.DbId },
						{ "classId", cooldown.Id },
						{ "remaining", cooldown.Remaining },
						{ "duration", cooldown.Duration },
						{ "startTime", cooldown.StartTime }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}

			// Only delete cooldowns that are no longer active
			var cooldownIdsInMemory = new HashSet<int>(activeCooldowns.Select(c => (int)c.Id));
			var cooldownIdsInDb = new HashSet<int>();
			using (var cmd = new MySqlCommand("SELECT `classId` FROM `cooldowns` WHERE `characterId` = @charId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@charId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						cooldownIdsInDb.Add(reader.GetInt32(0));
				}
			}

			var cooldownsToDelete = cooldownIdsInDb.Except(cooldownIdsInMemory).ToList();
			if (cooldownsToDelete.Any())
			{
				var deleteParams = cooldownsToDelete.Select((id, i) => $"@id{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `cooldowns` WHERE `characterId` = @charId AND `classId` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@charId", character.DbId);
					for (var i = 0; i < cooldownsToDelete.Count; i++)
						cmd.Parameters.AddWithValue(deleteParams[i], cooldownsToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves the character's quests within an existing transaction.
		/// Enhanced with better error handling and batch operations.
		/// </summary>
		internal void InternalSaveQuests(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			// Delete existing progress first (child table)
			using (var cmdDelProg = new MySqlCommand("DELETE FROM `quests_progress` WHERE `characterId` = @characterId", conn, trans))
			{
				cmdDelProg.Parameters.AddWithValue("@characterId", character.DbId);
				cmdDelProg.ExecuteNonQuery();
			}

			// Delete existing quests (parent table)
			using (var cmdDelQuest = new MySqlCommand("DELETE FROM `quests` WHERE `characterId` = @characterId", conn, trans))
			{
				cmdDelQuest.Parameters.AddWithValue("@characterId", character.DbId);
				cmdDelQuest.ExecuteNonQuery();
			}

			var validQuests = character.Quests.GetList()
					.Where(quest => quest.Data != null && quest.Data.Id != null)
					.ToList();

			if (!validQuests.Any()) return;

			// Process quests in batches for better performance
			var questBatch = new BatchInsertCommand("quests", null, conn, trans);
			var progressBatch = new BatchInsertCommand("quests_progress", "ON DUPLICATE KEY UPDATE `count` = VALUES(`count`), `done` = VALUES(`done`), `unlocked` = VALUES(`unlocked`)", conn, trans);

			var questIdMapping = new Dictionary<object, long>(); // Map quest objects to their DB IDs

			foreach (var quest in validQuests)
			{
				// Add quest to batch
				var questRow = new Dictionary<string, object>
				{
						{ "characterId", character.DbId },
						{ "classId", quest.Data.Id.Value },
						{ "status", (int)quest.Status },
						{ "startTime", quest.StartTime },
						{ "completeTime", quest.CompleteTime },
						{ "tracked", quest.Tracked }
				};
				questBatch.AddRow(questRow);
			}

			if (questBatch.HasRows)
			{
				// Execute the batch insert
				questBatch.Execute();

				// Query back the IDs we just inserted
				using (var cmd = new MySqlCommand(
						"SELECT questId FROM `quests` WHERE `characterId` = @characterId ORDER BY questId DESC LIMIT @count",
						conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					cmd.Parameters.AddWithValue("@count", validQuests.Count);

					using (var reader = cmd.ExecuteReader())
					{
						var ids = new List<long>();
						while (reader.Read())
							ids.Add(reader.GetInt64("questId"));
						ids.Reverse(); // They come back in DESC order, reverse to match validQuests order

						// Map quest objects to their new DB IDs
						for (int i = 0; i < validQuests.Count && i < ids.Count; i++)
						{
							questIdMapping[validQuests[i]] = ids[i];
						}
					}
				}

				// Now add progress entries
				foreach (var quest in validQuests)
				{
					if (!questIdMapping.TryGetValue(quest, out long questDbId)) continue;

					foreach (var progress in quest.Progresses)
					{
						if (progress.Objective == null) continue;

						progressBatch.AddRow(new Dictionary<string, object>
								{
										{ "questId", questDbId },
										{ "characterId", character.DbId },
										{ "ident", progress.Objective.Ident },
										{ "count", progress.Count },
										{ "done", progress.Done },
										{ "unlocked", progress.Unlocked }
								});
					}
				}

				if (progressBatch.HasRows)
					progressBatch.Execute();
			}
		}

		/// <summary>
		/// INTERNAL USE: Persists the character's companions within an existing transaction.
		/// Enhanced with better error handling.
		/// </summary>
		internal void InternalSaveCompanions(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			if (!character.HasCompanions) return;

			var validCompanions = character.Companions.GetList()
				.Where(companion => companion.DbId > 0)
				.ToList();

			if (!validCompanions.Any()) return;

			using (var batch = new MySqlBatch(conn, trans))
			{
				foreach (var companion in validCompanions)
				{
					var cmd = new MySqlBatchCommand("UPDATE `companions` SET `characterId`=@characterId, `name`=@name, `hp`=@hp, `stamina`=@stamina, `exp`=@exp, `maxExp`=@maxExp, `totalExp`=@totalExp, `level`=@level, `active`=@active, `isAggressiveMode`=@isAggressiveMode, `stat_mhp`=@stat_mhp, `stat_atk`=@stat_atk, `stat_def`=@stat_def, `stat_mdef`=@stat_mdef, `stat_hr`=@stat_hr, `stat_crthr`=@stat_crthr, `stat_dr`=@stat_dr WHERE `companionId`=@companionId");
					cmd.Parameters.Add(new MySqlParameter("@companionId", companion.DbId));
					cmd.Parameters.Add(new MySqlParameter("@characterId", character.DbId));
					cmd.Parameters.Add(new MySqlParameter("@name", companion.Name));
					cmd.Parameters.Add(new MySqlParameter("@hp", (int)companion.Properties.GetFloat(PropertyName.HP)));
					cmd.Parameters.Add(new MySqlParameter("@stamina", (int)companion.Properties.GetFloat(PropertyName.Stamina)));
					cmd.Parameters.Add(new MySqlParameter("@exp", companion.Exp));
					cmd.Parameters.Add(new MySqlParameter("@maxExp", companion.MaxExp));
					cmd.Parameters.Add(new MySqlParameter("@totalExp", companion.TotalExp));
					cmd.Parameters.Add(new MySqlParameter("@level", companion.Level));
					cmd.Parameters.Add(new MySqlParameter("@active", companion.IsActivated ? 1 : 0));
					cmd.Parameters.Add(new MySqlParameter("@isAggressiveMode", companion.IsAggressiveMode ? 1 : 0));
					cmd.Parameters.Add(new MySqlParameter("@stat_mhp", (int)companion.Properties.GetFloat(PropertyName.Stat_MHP)));
					cmd.Parameters.Add(new MySqlParameter("@stat_atk", (int)companion.Properties.GetFloat(PropertyName.Stat_ATK)));
					cmd.Parameters.Add(new MySqlParameter("@stat_def", (int)companion.Properties.GetFloat(PropertyName.Stat_DEF)));
					cmd.Parameters.Add(new MySqlParameter("@stat_mdef", (int)companion.Properties.GetFloat(PropertyName.Stat_MDEF)));
					cmd.Parameters.Add(new MySqlParameter("@stat_hr", (int)companion.Properties.GetFloat(PropertyName.Stat_HR)));
					cmd.Parameters.Add(new MySqlParameter("@stat_crthr", (int)companion.Properties.GetFloat(PropertyName.Stat_CRTHR)));
					cmd.Parameters.Add(new MySqlParameter("@stat_dr", (int)companion.Properties.GetFloat(PropertyName.Stat_DR)));
					batch.BatchCommands.Add(cmd);
				}

				if (batch.BatchCommands.Any())
					batch.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves character's achievements within an existing transaction.
		/// Uses UPSERT pattern to avoid gap lock deadlocks.
		/// </summary>
		internal void InternalSaveAchievements(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			var achievements = character.Achievements.GetAchievements();

			if (!achievements.Any())
			{
				// No achievements, clear all for this character
				using (var cmd = new MySqlCommand("DELETE FROM `achievements` WHERE `characterId` = @characterId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					cmd.ExecuteNonQuery();
				}
				return;
			}

			// Use UPSERT to insert achievements atomically (achievements don't change, just exist or not)
			using (var batch = new BatchInsertCommand("achievements", "ON DUPLICATE KEY UPDATE `achievementId` = VALUES(`achievementId`)", conn, trans))
			{
				foreach (var achievementId in achievements)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ "characterId", character.DbId },
						{ "achievementId", achievementId }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}

			// Only delete achievements that were removed
			var achievementIdsInMemory = new HashSet<int>(achievements);
			var achievementIdsInDb = new HashSet<int>();
			using (var cmd = new MySqlCommand("SELECT `achievementId` FROM `achievements` WHERE `characterId` = @charId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@charId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						achievementIdsInDb.Add(reader.GetInt32(0));
				}
			}

			var achievementsToDelete = achievementIdsInDb.Except(achievementIdsInMemory).ToList();
			if (achievementsToDelete.Any())
			{
				var deleteParams = achievementsToDelete.Select((id, i) => $"@id{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `achievements` WHERE `characterId` = @charId AND `achievementId` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@charId", character.DbId);
					for (var i = 0; i < achievementsToDelete.Count; i++)
						cmd.Parameters.AddWithValue(deleteParams[i], achievementsToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves character's achievement points within an existing transaction.
		/// Uses UPSERT pattern to avoid gap lock deadlocks.
		/// </summary>
		internal void InternalSaveAchievementPoints(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			var pointIds = character.Achievements.GetPointIds();

			if (!pointIds.Any())
			{
				// No points, clear all for this character
				using (var cmd = new MySqlCommand("DELETE FROM `achievement_points` WHERE `characterId` = @characterId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					cmd.ExecuteNonQuery();
				}
				return;
			}

			// Use UPSERT to insert or update achievement points atomically
			using (var batch = new BatchInsertCommand("achievement_points", "ON DUPLICATE KEY UPDATE `pointValue` = VALUES(`pointValue`)", conn, trans))
			{
				foreach (var pointId in pointIds)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ "characterId", character.DbId },
						{ "pointId", pointId },
						{ "pointValue", character.Achievements.GetPoints(pointId) }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}

			// Only delete point entries that were removed
			var pointIdsInMemory = new HashSet<int>(pointIds);
			var pointIdsInDb = new HashSet<int>();
			using (var cmd = new MySqlCommand("SELECT `pointId` FROM `achievement_points` WHERE `characterId` = @charId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@charId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
						pointIdsInDb.Add(reader.GetInt32(0));
				}
			}

			var pointsToDelete = pointIdsInDb.Except(pointIdsInMemory).ToList();
			if (pointsToDelete.Any())
			{
				var deleteParams = pointsToDelete.Select((id, i) => $"@id{i}").ToArray();
				using (var cmd = new MySqlCommand($"DELETE FROM `achievement_points` WHERE `characterId` = @charId AND `pointId` IN ({string.Join(",", deleteParams)})", conn, trans))
				{
					cmd.Parameters.AddWithValue("@charId", character.DbId);
					for (var i = 0; i < pointsToDelete.Count; i++)
						cmd.Parameters.AddWithValue(deleteParams[i], pointsToDelete[i]);
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves the Adventure Book (Monster Kills) within an existing transaction.
		/// Enhanced with batch operations.
		/// </summary>
		internal void InternalSaveAdventureBook(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			long accountId = character.AccountDbId;

			using (var cmdDel = new MySqlCommand("DELETE FROM `adventure_book` WHERE `accountId` = @accountId", conn, trans))
			{
				cmdDel.Parameters.AddWithValue("@accountId", accountId);
				cmdDel.ExecuteNonQuery();
			}

			var monsterKilledSnapshot = character.AdventureBook.GetListSnapshot(AdventureBookType.MonsterKilled);
			if (monsterKilledSnapshot.Length == 0) return;

			using (var batch = new BatchInsertCommand("adventure_book", null, conn, trans))
			{
				foreach (var info in monsterKilledSnapshot)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ "accountId", accountId },
						{ "type", AdventureBookType.MonsterKilled },
						{ "classId", info.Key },
						{ "count", info.Value }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves Adventure Book monster drops within an existing transaction.
		/// Enhanced with batch operations.
		/// </summary>
		internal void InternalSaveAdventureBookMonsterDrop(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			long accountId = character.AccountDbId;

			using (var cmdDel = new MySqlCommand("DELETE FROM `adventure_book_monster_drops` WHERE `accountId` = @accountId", conn, trans))
			{
				cmdDel.Parameters.AddWithValue("@accountId", accountId);
				cmdDel.ExecuteNonQuery();
			}

			var monsterDropsSnapshot = character.AdventureBook.GetMonsterDropsSnapshot();
			if (monsterDropsSnapshot.Count == 0) return;

			using (var batch = new BatchInsertCommand("adventure_book_monster_drops", null, conn, trans))
			{
				foreach (var info in monsterDropsSnapshot)
				{
					foreach (var itemInfo in info.Value)
					{
						batch.AddRow(new Dictionary<string, object>
						{
							{ "accountId", accountId },
							{ "monsterId", info.Key },
							{ "itemId", itemInfo.Key },
							{ "count", itemInfo.Value }
						});
					}
				}

				if (batch.HasRows)
					batch.Execute();
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves Adventure Book items within an existing transaction.
		/// Enhanced with batch operations.
		/// </summary>
		internal void InternalSaveAdventureBookItems(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			long accountId = character.AccountDbId;

			using (var cmdDel = new MySqlCommand("DELETE FROM `adventure_book_items` WHERE `accountId` = @accountId", conn, trans))
			{
				cmdDel.Parameters.AddWithValue("@accountId", accountId);
				cmdDel.ExecuteNonQuery();
			}

			var itemsSnapshot = character.AdventureBook.GetItemsSnapshot();
			if (itemsSnapshot.Length == 0) return;

			using (var batch = new BatchInsertCommand("adventure_book_items", null, conn, trans))
			{
				foreach (var info in itemsSnapshot)
				{
					batch.AddRow(new Dictionary<string, object>
					{
						{ "accountId", accountId },
						{ "itemId", info.Key },
						{ "craftCount", info.Value.CraftedCount },
						{ "obtainCount", info.Value.ObtainedCount },
						{ "useCount", info.Value.UsedCount }
					});
				}

				if (batch.HasRows)
					batch.Execute();
			}
		}

		/// <summary>
		/// INTERNAL USE: Saves the character's collections within an existing transaction.
		/// Enhanced with batch operations and better error handling.
		/// </summary>
		internal void InternalSaveCollections(Character character, MySqlConnection conn, MySqlTransaction trans)
		{
			long accountId = character.AccountDbId;

			// Delete child table first
			using (var cmdDelItems = new MySqlCommand("DELETE FROM `collection_items` WHERE `accountId` = @accountId", conn, trans))
			{
				cmdDelItems.Parameters.AddWithValue("@accountId", accountId);
				cmdDelItems.ExecuteNonQuery();
			}

			// Delete parent table
			using (var cmdDelColl = new MySqlCommand("DELETE FROM `collections` WHERE `accountId` = @accountId", conn, trans))
			{
				cmdDelColl.Parameters.AddWithValue("@accountId", accountId);
				cmdDelColl.ExecuteNonQuery();
			}

			var collections = character.Collections.GetList();
			if (!collections.Any()) return;

			// Insert collections in batch
			using (var collectionBatch = new BatchInsertCommand("collections", null, conn, trans))
			{
				foreach (var collection in collections)
				{
					collectionBatch.AddRow(new Dictionary<string, object>
					{
						{ "accountId", accountId },
						{ "collectionId", collection.Id },
						{ "isComplete", collection.IsComplete },
						{ "timesRedeemed", collection.RedeemCount }
					});
				}

				if (collectionBatch.HasRows)
					collectionBatch.Execute();
			}

			// Insert collection items in batch
			using (var itemsBatch = new BatchInsertCommand("collection_items", null, conn, trans))
			{
				foreach (var collection in collections)
				{
					foreach (var itemId in collection.GetRegisteredItems())
					{
						itemsBatch.AddRow(new Dictionary<string, object>
						{
							{ "accountId", accountId },
							{ "collectionId", collection.Id },
							{ "itemId", itemId }
						});
					}
				}

				if (itemsBatch.HasRows)
					itemsBatch.Execute();
			}
		}

		#endregion // Internal Save Methods

		#region Helper Methods

		/// <summary>
		/// Validates that a character has a valid DbId before performing database operations.
		/// </summary>
		private static void ValidateCharacterDbId(Character character)
		{
			if (character?.DbId <= 0)
				throw new ArgumentException($"Character {character?.Name ?? "null"} has invalid DbId: {character?.DbId ?? 0}");
		}

		/// <summary>
		/// Validates that a connection and transaction are valid for database operations.
		/// </summary>
		private static void ValidateConnectionAndTransaction(MySqlConnection conn, MySqlTransaction trans)
		{
			if (conn == null)
				throw new ArgumentNullException(nameof(conn), "Database connection cannot be null");

			if (trans == null)
				throw new ArgumentNullException(nameof(trans), "Database transaction cannot be null");

			if (conn.State != System.Data.ConnectionState.Open)
				throw new InvalidOperationException("Database connection must be open");
		}

		/// <summary>
		/// Helper method to safely execute batch operations with proper error handling.
		/// </summary>
		private static void SafeExecuteBatch(BatchInsertCommand batch, string operationName)
		{
			try
			{
				if (batch.HasRows)
					batch.Execute();
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to execute batch operation '{operationName}': {ex.Message}");
				throw;
			}
		}

		/// <summary>
		/// Gets the type string for a variable value.
		/// </summary>
		private static string GetVariableType(object varValue)
		{
			if (varValue is byte) return "1u";
			if (varValue is ushort) return "2u";
			if (varValue is uint) return "4u";
			if (varValue is ulong) return "8u";
			if (varValue is sbyte) return "1";
			if (varValue is short) return "2";
			if (varValue is int) return "4";
			if (varValue is long) return "8";
			if (varValue is float) return "f";
			if (varValue is double) return "d";
			if (varValue is bool) return "b";
			if (varValue is string) return "s";
			if (varValue is DateTime) return "dt";
			if (varValue is TimeSpan) return "ts";
			return null;
		}

		/// <summary>
		/// Serializes a variable value to a string for database storage.
		/// </summary>
		private static string SerializeVariableValue(object varValue, string type)
		{
			switch (type)
			{
				case "f": return ((float)varValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
				case "d": return ((double)varValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
				case "dt": return ((DateTime)varValue).Ticks.ToString();
				case "ts": return ((TimeSpan)varValue).Ticks.ToString();
				default: return varValue.ToString();
			}
		}

		#endregion // Helper Methods
	}
}
