using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using MySqlConnector;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Comprehensive rollback system for character and account state management.
	/// Handles creating snapshots before major operations and rolling back when needed.
	/// </summary>
	public partial class ZoneDb
	{
		#region Rollback System Core

		/// <summary>
		/// Creates a complete snapshot of character state before a potentially risky operation.
		/// </summary>
		public long CreateCharacterSnapshot(Character character, string reason, string operationDetails = null)
		{
			if (character == null)
				throw new ArgumentNullException(nameof(character));

			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					// Create main snapshot record
					long snapshotId;
					using (var cmd = new InsertCommand("INSERT INTO `character_snapshots` {0}", conn, trans))
					{
						cmd.Set("characterId", character.DbId);
						cmd.Set("accountId", character.AccountDbId);
						cmd.Set("snapshotReason", reason);
						cmd.Set("operationDetails", operationDetails);
						cmd.Set("createdAt", DateTime.UtcNow);
						cmd.Set("gameVersion", ZoneServer.Instance.Version?.ToString() ?? "Unknown");
						cmd.Set("characterLevel", character.Level);
						cmd.Set("mapId", character.MapId);
						cmd.Execute();
						snapshotId = cmd.LastId;
					}

					// Snapshot all character components
					this.SnapshotCharacterData(character, snapshotId, conn, trans);
					this.SnapshotInventoryAndItems(character, snapshotId, conn, trans);
					this.SnapshotCharacterProperties(character, snapshotId, conn, trans);
					this.SnapshotJobsAndSkills(character, snapshotId, conn, trans);
					this.SnapshotQuests(character, snapshotId, conn, trans);
					this.SnapshotSocialData(character, snapshotId, conn, trans);
					this.SnapshotStorageData(character, snapshotId, conn, trans);

					trans.Commit();
					Log.Info($"Character snapshot {snapshotId} created for {character.Name} ({character.DbId}). Reason: {reason}");
					return snapshotId;
				}
				catch (Exception ex)
				{
					try { trans.Rollback(); } catch { }
					Log.Error($"Failed to create character snapshot for {character.Name}: {ex}");
					throw;
				}
			}
		}

		/// <summary>
		/// Rolls back character to a specific snapshot.
		/// </summary>
		public bool RollbackCharacterToSnapshot(long characterId, long snapshotId, string rollbackReason)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					// Verify snapshot exists and belongs to character
					bool snapshotValid = false;
					DateTime snapshotDate = default;
					string originalReason = "";

					using (var cmd = new MySqlCommand("SELECT createdAt, snapshotReason FROM `character_snapshots` WHERE `snapshotId` = @snapshotId AND `characterId` = @characterId", conn, trans))
					{
						cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
						cmd.Parameters.AddWithValue("@characterId", characterId);
						using (var reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								snapshotValid = true;
								snapshotDate = reader.GetDateTime("createdAt");
								originalReason = reader.GetString("snapshotReason");
							}
						}
					}

					if (!snapshotValid)
					{
						Log.Warning($"Attempted to rollback to invalid snapshot {snapshotId} for character {characterId}");
						return false;
					}

					// Create a pre-rollback snapshot for safety
					using (var preRollbackCmd = new InsertCommand("INSERT INTO `character_snapshots` {0}", conn, trans))
					{
						preRollbackCmd.Set("characterId", characterId);
						preRollbackCmd.Set("snapshotReason", "PRE_ROLLBACK_SAFETY");
						preRollbackCmd.Set("operationDetails", $"Before rollback to snapshot {snapshotId} ({originalReason})");
						preRollbackCmd.Set("createdAt", DateTime.UtcNow);
						preRollbackCmd.Execute();
						var safetySnapshotId = preRollbackCmd.LastId;

						// We don't create full safety snapshot data here to avoid infinite recursion,
						// but we log it for reference
						Log.Info($"Safety snapshot {safetySnapshotId} created before rollback");
					}

					// Perform the rollback
					this.RestoreCharacterData(characterId, snapshotId, conn, trans);
					this.RestoreInventoryAndItems(characterId, snapshotId, conn, trans);
					this.RestoreCharacterProperties(characterId, snapshotId, conn, trans);
					this.RestoreJobsAndSkills(characterId, snapshotId, conn, trans);
					this.RestoreQuests(characterId, snapshotId, conn, trans);
					this.RestoreSocialData(characterId, snapshotId, conn, trans);
					this.RestoreStorageData(characterId, snapshotId, conn, trans);

					// Log the rollback
					using (var logCmd = new InsertCommand("INSERT INTO `rollback_log` {0}", conn, trans))
					{
						logCmd.Set("characterId", characterId);
						logCmd.Set("snapshotId", snapshotId);
						logCmd.Set("rollbackReason", rollbackReason);
						logCmd.Set("rolledBackAt", DateTime.UtcNow);
						logCmd.Set("originalSnapshotReason", originalReason);
						logCmd.Set("originalSnapshotDate", snapshotDate);
						logCmd.Execute();
					}

					trans.Commit();
					Log.Info($"Successfully rolled back character {characterId} to snapshot {snapshotId}. Reason: {rollbackReason}");
					return true;
				}
				catch (Exception ex)
				{
					try { trans.Rollback(); } catch { }
					Log.Error($"Failed to rollback character {characterId} to snapshot {snapshotId}: {ex}");
					return false;
				}
			}
		}

		/// <summary>
		/// Mass rollback multiple characters to snapshots created within a time range.
		/// Useful for rolling back after discovering a game-breaking bug.
		/// </summary>
		public RollbackResult MassRollback(DateTime fromTime, DateTime toTime, string bugDescription, List<long> specificCharacterIds = null)
		{
			var result = new RollbackResult();

			using (var conn = this.GetConnection())
			{
				// Find eligible snapshots
				var eligibleSnapshots = new List<(long CharacterId, long SnapshotId, DateTime CreatedAt, string Reason)>();

				var whereClause = "createdAt BETWEEN @fromTime AND @toTime AND snapshotReason != 'PRE_ROLLBACK_SAFETY'";
				if (specificCharacterIds?.Any() == true)
				{
					var charParams = specificCharacterIds.Select((id, i) => $"@char{i}").ToArray();
					whereClause += $" AND characterId IN ({string.Join(",", charParams)})";
				}

				using (var cmd = new MySqlCommand($"SELECT characterId, snapshotId, createdAt, snapshotReason FROM `character_snapshots` WHERE {whereClause} ORDER BY characterId, createdAt DESC", conn))
				{
					cmd.Parameters.AddWithValue("@fromTime", fromTime);
					cmd.Parameters.AddWithValue("@toTime", toTime);

					if (specificCharacterIds?.Any() == true)
					{
						for (int i = 0; i < specificCharacterIds.Count; i++)
							cmd.Parameters.AddWithValue($"@char{i}", specificCharacterIds[i]);
					}

					using (var reader = cmd.ExecuteReader())
					{
						var processedCharacters = new HashSet<long>();
						while (reader.Read())
						{
							var charId = reader.GetInt64("characterId");
							// Only take the latest snapshot per character in the time range
							if (!processedCharacters.Contains(charId))
							{
								eligibleSnapshots.Add((charId, reader.GetInt64("snapshotId"), reader.GetDateTime("createdAt"), reader.GetString("snapshotReason")));
								processedCharacters.Add(charId);
							}
						}
					}
				}

				Log.Info($"Mass rollback identified {eligibleSnapshots.Count} characters to rollback");

				// Process each character
				foreach (var (charId, snapshotId, createdAt, reason) in eligibleSnapshots)
				{
					try
					{
						if (this.RollbackCharacterToSnapshot(charId, snapshotId, $"MASS_ROLLBACK: {bugDescription}"))
						{
							result.SuccessfulRollbacks.Add(new CharacterRollbackInfo
							{
								CharacterId = charId,
								SnapshotId = snapshotId,
								OriginalSnapshotDate = createdAt,
								OriginalReason = reason
							});
						}
						else
						{
							result.FailedRollbacks.Add(new CharacterRollbackInfo
							{
								CharacterId = charId,
								SnapshotId = snapshotId,
								OriginalSnapshotDate = createdAt,
								OriginalReason = reason,
								ErrorMessage = "Rollback returned false"
							});
						}
					}
					catch (Exception ex)
					{
						result.FailedRollbacks.Add(new CharacterRollbackInfo
						{
							CharacterId = charId,
							SnapshotId = snapshotId,
							OriginalSnapshotDate = createdAt,
							OriginalReason = reason,
							ErrorMessage = ex.Message
						});
						Log.Error($"Mass rollback failed for character {charId}: {ex}");
					}
				}

				// Log the mass rollback operation
				using (var logCmd = new InsertCommand("INSERT INTO `mass_rollback_log` {0}", conn))
				{
					logCmd.Set("bugDescription", bugDescription);
					logCmd.Set("fromTime", fromTime);
					logCmd.Set("toTime", toTime);
					logCmd.Set("totalAttempted", eligibleSnapshots.Count);
					logCmd.Set("successfulCount", result.SuccessfulRollbacks.Count);
					logCmd.Set("failedCount", result.FailedRollbacks.Count);
					logCmd.Set("executedAt", DateTime.UtcNow);
					logCmd.Set("resultDetails", JsonSerializer.Serialize(result));
					logCmd.Execute();
				}
			}

			return result;
		}

		#endregion

		#region Snapshot Creation Methods

		private void SnapshotCharacterData(Character character, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			using (var cmd = new InsertCommand("INSERT INTO `snapshot_character_data` {0}", conn, trans))
			{
				cmd.Set("snapshotId", snapshotId);
				cmd.Set("characterId", character.DbId);
				cmd.Set("name", character.Name);
				cmd.Set("teamName", character.TeamName);
				cmd.Set("jobId", (short)character.JobId);
				cmd.Set("gender", (byte)character.Gender);
				cmd.Set("hair", character.Hair);
				cmd.Set("skinColor", character.SkinColor);
				cmd.Set("mapId", character.MapId);
				cmd.Set("x", character.Position.X);
				cmd.Set("y", character.Position.Y);
				cmd.Set("z", character.Position.Z);
				cmd.Set("dir", character.Direction.DegreeAngle);
				cmd.Set("exp", character.Exp);
				cmd.Set("maxExp", character.MaxExp);
				cmd.Set("totalExp", character.TotalExp);
				cmd.Set("equipVisibility", (int)character.VisibleEquip);
				cmd.Set("equippedTitleId", character.EquippedTitleId);
				cmd.Set("stamina", character.Properties.Stamina);
				cmd.Set("level", character.Level);
				cmd.Execute();
			}
		}

		private void SnapshotInventoryAndItems(Character character, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Snapshot all inventory items
			var allItems = character.Inventory.GetItems().Values
				.Concat(character.Inventory.GetEquip().Values.Where(i => i != null && i is not DummyEquipItem))
				.Concat(character.Inventory.GetCards().Values.Where(i => i != null))
				.ToList();

			foreach (var item in allItems)
			{
				using (var cmd = new InsertCommand("INSERT INTO `snapshot_items` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("originalItemId", item.DbId);
					cmd.Set("itemId", item.Id);
					cmd.Set("amount", item.Amount);

					// Determine if item is equipped and what kind
					var isRegularEquip = character.Inventory.GetEquip().Values.Contains(item);
					var equipSlot = character.Inventory.GetEquip().FirstOrDefault(kvp => kvp.Value == item).Key;

					// Check if it's an equipped card
					var cardSlot = character.Inventory.GetCards().FirstOrDefault(kvp => kvp.Value == item).Key;
					if (cardSlot > 0) // Card slots are 1-15
					{
						// Store card slots as 100-115 in the equipSlot field
						cmd.Set("isEquipped", true);
						cmd.Set("equipSlot", (byte)(100 + cardSlot - 1));
						cmd.Set("inventoryIndex", 0);
					}
					else if (isRegularEquip)
					{
						cmd.Set("isEquipped", true);
						cmd.Set("equipSlot", equipSlot);
						cmd.Set("inventoryIndex", 0);
					}
					else
					{
						cmd.Set("isEquipped", false);
						cmd.Set("equipSlot", (byte)0x7F);
						cmd.Set("inventoryIndex", character.Inventory.GetItems().FirstOrDefault(kvp => kvp.Value == item).Key);
					}

					cmd.Execute();
					var snapshotItemId = cmd.LastId;

					// Snapshot item properties
					foreach (var property in item.Properties.GetAll())
					{
						if (property is IUnsettableProperty) continue;

						using (var propCmd = new InsertCommand("INSERT INTO `snapshot_item_properties` {0}", conn, trans))
						{
							propCmd.Set("snapshotItemId", snapshotItemId);
							propCmd.Set("name", property.Ident);
							propCmd.Set("type", property is FloatProperty ? "f" : "s");
							propCmd.Set("value", property.Serialize());
							propCmd.Execute();
						}
					}
				}
			}
		}

		private void SnapshotCharacterProperties(Character character, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Character properties
			foreach (var property in character.Properties.GetAll())
			{
				if (property is IUnsettableProperty) continue;

				using (var cmd = new InsertCommand("INSERT INTO `snapshot_character_properties` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("name", property.Ident);
					cmd.Set("type", property is FloatProperty ? "f" : "s");
					cmd.Set("value", property.Serialize());
					cmd.Execute();
				}
			}

			// ETC properties
			foreach (var property in character.Etc.Properties.GetAll())
			{
				if (property is IUnsettableProperty) continue;

				using (var cmd = new InsertCommand("INSERT INTO `snapshot_character_etc_properties` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("name", property.Ident);
					cmd.Set("type", property is FloatProperty ? "f" : "s");
					cmd.Set("value", property.Serialize());
					cmd.Execute();
				}
			}

			// Variables
			foreach (var varKvp in character.Variables.Perm.GetList())
			{
				using (var cmd = new InsertCommand("INSERT INTO `snapshot_variables` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("variableName", varKvp.Key);
					cmd.Set("variableType", GetVariableTypeString(varKvp.Value));
					cmd.Set("variableValue", GetVariableValueString(varKvp.Value));
					cmd.Execute();
				}
			}
		}

		private void SnapshotJobsAndSkills(Character character, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Jobs
			foreach (var job in character.Jobs.GetList())
			{
				using (var cmd = new InsertCommand("INSERT INTO `snapshot_jobs` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("jobId", (int)job.Id);
					cmd.Set("circle", (int)job.Circle);
					cmd.Set("skillPoints", job.SkillPoints);
					cmd.Set("totalExp", job.TotalExp);
					cmd.Set("selectionDate", job.SelectionDate);
					cmd.Set("advDate", job.AdvancementDate);
					cmd.Execute();
				}
			}

			// Skills
			foreach (var skill in character.Skills.GetList())
			{
				using (var cmd = new InsertCommand("INSERT INTO `snapshot_skills` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("skillId", (int)skill.Id);
					cmd.Set("level", skill.Level);
					cmd.Execute();
				}
			}

			// Abilities
			foreach (var ability in character.Abilities.GetList())
			{
				using (var cmd = new InsertCommand("INSERT INTO `snapshot_abilities` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("abilityId", (int)ability.Id);
					cmd.Set("level", ability.Level);
					cmd.Set("active", ability.Active);
					cmd.Execute();
				}
			}
		}

		private void SnapshotQuests(Character character, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			foreach (var quest in character.Quests.GetList())
			{
				using (var cmd = new InsertCommand("INSERT INTO `snapshot_quests` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("questClassId", quest.Data.Id.Value);
					cmd.Set("status", (int)quest.Status);
					cmd.Set("startTime", quest.StartTime);
					cmd.Set("completeTime", quest.CompleteTime);
					cmd.Set("tracked", quest.Tracked);
					cmd.Execute();
					var snapshotQuestId = cmd.LastId;

					// Quest progress
					foreach (var progress in quest.Progresses)
					{
						if (progress.Objective == null) continue;

						using (var progCmd = new InsertCommand("INSERT INTO `snapshot_quest_progress` {0}", conn, trans))
						{
							progCmd.Set("snapshotQuestId", snapshotQuestId);
							progCmd.Set("ident", progress.Objective.Ident);
							progCmd.Set("count", progress.Count);
							progCmd.Set("done", progress.Done);
							progCmd.Set("unlocked", progress.Unlocked);
							progCmd.Execute();
						}
					}
				}
			}
		}

		private void SnapshotSocialData(Character character, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			using (var cmd = new InsertCommand("INSERT INTO `snapshot_social_data` {0}", conn, trans))
			{
				cmd.Set("snapshotId", snapshotId);
				cmd.Set("partyId", character.PartyId);
				cmd.Set("guildId", character.GuildId);
				cmd.Execute();
			}
		}

		private void SnapshotStorageData(Character character, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Personal storage
			foreach (var itemKvp in character.PersonalStorage.GetItems())
			{
				using (var cmd = new InsertCommand("INSERT INTO `snapshot_storage_personal` {0}", conn, trans))
				{
					cmd.Set("snapshotId", snapshotId);
					cmd.Set("position", itemKvp.Key);
					cmd.Set("itemId", itemKvp.Value.Id);
					cmd.Set("amount", itemKvp.Value.Amount);
					cmd.Set("originalItemDbId", itemKvp.Value.DbId);
					cmd.Execute();
				}
			}

			// Team storage (if accessible)
			if (character.TeamStorage != null)
			{
				foreach (var itemKvp in character.TeamStorage.GetItems())
				{
					using (var cmd = new InsertCommand("INSERT INTO `snapshot_storage_team` {0}", conn, trans))
					{
						cmd.Set("snapshotId", snapshotId);
						cmd.Set("position", itemKvp.Key);
						cmd.Set("itemId", itemKvp.Value.Id);
						cmd.Set("amount", itemKvp.Value.Amount);
						cmd.Set("originalItemDbId", itemKvp.Value.DbId);
						cmd.Execute();
					}
				}
			}
		}

		#endregion

		#region Restore Methods

		private void RestoreCharacterData(long characterId, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_character_data` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						using (var updateCmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn, trans))
						{
							updateCmd.AddParameter("@characterId", characterId);
							updateCmd.Set("name", reader.GetString("name"));
							updateCmd.Set("teamName", reader.GetString("teamName"));
							updateCmd.Set("job", reader.GetInt16("jobId"));
							updateCmd.Set("gender", reader.GetByte("gender"));
							updateCmd.Set("hair", reader.GetInt32("hair"));
							updateCmd.Set("skinColor", reader.GetUInt32("skinColor"));
							updateCmd.Set("zone", reader.GetInt32("mapId"));
							updateCmd.Set("x", reader.GetFloat("x"));
							updateCmd.Set("y", reader.GetFloat("y"));
							updateCmd.Set("z", reader.GetFloat("z"));
							updateCmd.Set("dir", reader.GetFloat("dir"));
							updateCmd.Set("exp", reader.GetInt64("exp"));
							updateCmd.Set("maxExp", reader.GetInt64("maxExp"));
							updateCmd.Set("totalExp", reader.GetInt64("totalExp"));
							updateCmd.Set("equipVisibility", reader.GetInt32("equipVisibility"));
							updateCmd.Set("equippedTitleId", reader.GetInt32("equippedTitleId"));
							updateCmd.Set("stamina", reader.GetInt32("stamina"));
							updateCmd.Execute();
						}
					}
				}
			}
		}

		private void RestoreInventoryAndItems(long characterId, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Clear existing inventory links
			using (var clearCmd = new MySqlCommand("DELETE FROM `inventory` WHERE `characterId` = @characterId", conn, trans))
			{
				clearCmd.Parameters.AddWithValue("@characterId", characterId);
				clearCmd.ExecuteNonQuery();
			}


			// Restore items from snapshot
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_items` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					var itemsToRestore = new List<dynamic>();
					while (reader.Read())
					{
						itemsToRestore.Add(new
						{
							SnapshotItemId = reader.GetInt64("snapshotItemId"),
							ItemId = reader.GetInt32("itemId"),
							Amount = reader.GetInt32("amount"),
							IsEquipped = reader.GetBoolean("isEquipped"),
							EquipSlot = reader.IsDBNull(reader.GetOrdinal("equipSlot")) ? (byte?)null : reader.GetByte("equipSlot"),
							InventoryIndex = reader.IsDBNull(reader.GetOrdinal("inventoryIndex")) ? (int?)null : reader.GetInt32("inventoryIndex")
						});
					}

					foreach (var itemData in itemsToRestore)
					{
						// Create new item in items table
						long newItemId;
						using (var itemCmd = new InsertCommand("INSERT INTO `items` {0}", conn, trans))
						{
							itemCmd.Set("itemId", itemData.ItemId);
							itemCmd.Set("amount", itemData.Amount);
							itemCmd.Execute();
							newItemId = itemCmd.LastId;
						}

						// Restore item properties
						using (var propCmd = new MySqlCommand("SELECT * FROM `snapshot_item_properties` WHERE `snapshotItemId` = @snapshotItemId", conn, trans))
						{
							propCmd.Parameters.AddWithValue("@snapshotItemId", itemData.SnapshotItemId);
							using (var propReader = propCmd.ExecuteReader())
							{
								while (propReader.Read())
								{
									using (var insertPropCmd = new InsertCommand("INSERT INTO `item_properties` {0}", conn, trans))
									{
										insertPropCmd.Set("itemId", newItemId);
										insertPropCmd.Set("name", propReader.GetString("name"));
										insertPropCmd.Set("type", propReader.GetString("type"));
										insertPropCmd.Set("value", propReader.GetString("value"));
										insertPropCmd.Execute();
									}
								}
							}
						}

						// Link to inventory
						using (var invCmd = new InsertCommand("INSERT INTO `inventory` {0}", conn, trans))
						{
							invCmd.Set("characterId", characterId);
							invCmd.Set("itemId", newItemId);
							invCmd.Set("sort", itemData.InventoryIndex ?? 0);
							invCmd.Set("equipSlot", itemData.EquipSlot ?? 0x7F);
							invCmd.Execute();
						}
					}
				}
			}

		}

		private void RestoreCharacterProperties(long characterId, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Clear existing properties
			using (var clearCmd = new MySqlCommand("DELETE FROM `character_properties` WHERE `characterId` = @characterId", conn, trans))
			{
				clearCmd.Parameters.AddWithValue("@characterId", characterId);
				clearCmd.ExecuteNonQuery();
			}

			using (var clearEtcCmd = new MySqlCommand("DELETE FROM `character_etc_properties` WHERE `characterId` = @characterId", conn, trans))
			{
				clearEtcCmd.Parameters.AddWithValue("@characterId", characterId);
				clearEtcCmd.ExecuteNonQuery();
			}

			using (var clearVarsCmd = new MySqlCommand("DELETE FROM `vars_characters` WHERE `characterId` = @characterId", conn, trans))
			{
				clearVarsCmd.Parameters.AddWithValue("@characterId", characterId);
				clearVarsCmd.ExecuteNonQuery();
			}

			// Restore character properties
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_character_properties` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					using (var batch = new BatchInsertCommand("character_properties", null, conn, trans))
					{
						while (reader.Read())
						{
							batch.AddRow(new Dictionary<string, object>
							{
								{ "characterId", characterId },
								{ "name", reader.GetString("name") },
								{ "type", reader.GetString("type") },
								{ "value", reader.GetString("value") }
							});
						}
						if (batch.HasRows) batch.Execute();
					}
				}
			}

			// Restore ETC properties
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_character_etc_properties` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					using (var batch = new BatchInsertCommand("character_etc_properties", null, conn, trans))
					{
						while (reader.Read())
						{
							batch.AddRow(new Dictionary<string, object>
							{
								{ "characterId", characterId },
								{ "name", reader.GetString("name") },
								{ "type", reader.GetString("type") },
								{ "value", reader.GetString("value") }
							});
						}
						if (batch.HasRows) batch.Execute();
					}
				}
			}

			// Restore variables
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_variables` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					using (var batch = new BatchInsertCommand("vars_characters", null, conn, trans))
					{
						while (reader.Read())
						{
							batch.AddRow(new Dictionary<string, object>
							{
								{ "characterId", characterId },
								{ "name", reader.GetString("variableName") },
								{ "type", reader.GetString("variableType") },
								{ "value", reader.GetString("variableValue") }
							});
						}
						if (batch.HasRows) batch.Execute();
					}
				}
			}
		}

		private void RestoreJobsAndSkills(long characterId, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Clear existing data
			using (var clearJobsCmd = new MySqlCommand("DELETE FROM `jobs` WHERE `characterId` = @characterId", conn, trans))
			{
				clearJobsCmd.Parameters.AddWithValue("@characterId", characterId);
				clearJobsCmd.ExecuteNonQuery();
			}

			using (var clearSkillsCmd = new MySqlCommand("DELETE FROM `skills` WHERE `characterId` = @characterId", conn, trans))
			{
				clearSkillsCmd.Parameters.AddWithValue("@characterId", characterId);
				clearSkillsCmd.ExecuteNonQuery();
			}

			using (var clearAbilitiesCmd = new MySqlCommand("DELETE FROM `abilities` WHERE `characterId` = @characterId", conn, trans))
			{
				clearAbilitiesCmd.Parameters.AddWithValue("@characterId", characterId);
				clearAbilitiesCmd.ExecuteNonQuery();
			}

			// Restore jobs
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_jobs` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					using (var batch = new BatchInsertCommand("jobs", null, conn, trans))
					{
						while (reader.Read())
						{
							batch.AddRow(new Dictionary<string, object>
							{
								{ "characterId", characterId },
								{ "jobId", reader.GetInt32("jobId") },
								{ "circle", reader.GetInt32("circle") },
								{ "skillPoints", reader.GetInt32("skillPoints") },
								{ "totalExp", reader.GetInt64("totalExp") },
								{ "selectionDate", reader.GetDateTime("selectionDate") },
								{ "advDate", reader.GetDateTime("advDate") }
							});
						}
						if (batch.HasRows) batch.Execute();
					}
				}
			}

			// Restore skills
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_skills` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					using (var batch = new BatchInsertCommand("skills", null, conn, trans))
					{
						while (reader.Read())
						{
							batch.AddRow(new Dictionary<string, object>
							{
								{ "characterId", characterId },
								{ "id", reader.GetInt32("skillId") },
								{ "level", reader.GetInt32("level") }
							});
						}
						if (batch.HasRows) batch.Execute();
					}
				}
			}

			// Restore abilities
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_abilities` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					using (var batch = new BatchInsertCommand("abilities", null, conn, trans))
					{
						while (reader.Read())
						{
							batch.AddRow(new Dictionary<string, object>
							{
								{ "characterId", characterId },
								{ "id", reader.GetInt32("abilityId") },
								{ "level", reader.GetInt32("level") },
								{ "active", reader.GetBoolean("active") }
							});
						}
						if (batch.HasRows) batch.Execute();
					}
				}
			}
		}

		private void RestoreQuests(long characterId, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Clear existing quests and progress
			using (var clearProgressCmd = new MySqlCommand("DELETE FROM `quests_progress` WHERE `characterId` = @characterId", conn, trans))
			{
				clearProgressCmd.Parameters.AddWithValue("@characterId", characterId);
				clearProgressCmd.ExecuteNonQuery();
			}

			using (var clearQuestsCmd = new MySqlCommand("DELETE FROM `quests` WHERE `characterId` = @characterId", conn, trans))
			{
				clearQuestsCmd.Parameters.AddWithValue("@characterId", characterId);
				clearQuestsCmd.ExecuteNonQuery();
			}

			// Restore quests
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_quests` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					var questMapping = new Dictionary<long, long>(); // snapshot quest ID -> new quest ID

					while (reader.Read())
					{
						var snapshotQuestId = reader.GetInt64("snapshotQuestId");

						using (var insertCmd = new InsertCommand("INSERT INTO `quests` {0}", conn, trans))
						{
							insertCmd.Set("characterId", characterId);
							insertCmd.Set("classId", reader.GetInt64("questClassId"));
							insertCmd.Set("status", reader.GetInt32("status"));
							insertCmd.Set("startTime", reader.GetDateTime("startTime"));
							insertCmd.Set("completeTime", reader.GetDateTime("completeTime"));
							insertCmd.Set("tracked", reader.GetBoolean("tracked"));
							insertCmd.Execute();
							questMapping[snapshotQuestId] = insertCmd.LastId;
						}
					}

					// Restore quest progress
					using (var progressCmd = new MySqlCommand("SELECT * FROM `snapshot_quest_progress` WHERE `snapshotQuestId` IN (" +
						string.Join(",", questMapping.Keys) + ")", conn, trans))
					{
						using (var progressReader = progressCmd.ExecuteReader())
						{
							using (var batch = new BatchInsertCommand("quests_progress", null, conn, trans))
							{
								while (progressReader.Read())
								{
									var snapshotQuestId = progressReader.GetInt64("snapshotQuestId");
									if (questMapping.TryGetValue(snapshotQuestId, out long newQuestId))
									{
										batch.AddRow(new Dictionary<string, object>
										{
											{ "questId", newQuestId },
											{ "characterId", characterId },
											{ "ident", progressReader.GetString("ident") },
											{ "count", progressReader.GetInt32("count") },
											{ "done", progressReader.GetBoolean("done") },
											{ "unlocked", progressReader.GetBoolean("unlocked") }
										});
									}
								}
								if (batch.HasRows) batch.Execute();
							}
						}
					}
				}
			}
		}

		private void RestoreSocialData(long characterId, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_social_data` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						using (var updateCmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn, trans))
						{
							updateCmd.AddParameter("@characterId", characterId);
							updateCmd.Set("partyId", reader.GetInt64("partyId"));
							updateCmd.Set("guildId", reader.GetInt64("guildId"));
							updateCmd.Execute();
						}
					}
				}
			}
		}

		private void RestoreStorageData(long characterId, long snapshotId, MySqlConnection conn, MySqlTransaction trans)
		{
			// Clear existing storage
			using (var clearPersonalCmd = new MySqlCommand("DELETE FROM `storage_personal` WHERE `characterId` = @characterId", conn, trans))
			{
				clearPersonalCmd.Parameters.AddWithValue("@characterId", characterId);
				clearPersonalCmd.ExecuteNonQuery();
			}

			// Restore personal storage
			using (var cmd = new MySqlCommand("SELECT * FROM `snapshot_storage_personal` WHERE `snapshotId` = @snapshotId", conn, trans))
			{
				cmd.Parameters.AddWithValue("@snapshotId", snapshotId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						// Create new item
						long newItemId;
						using (var itemCmd = new InsertCommand("INSERT INTO `items` {0}", conn, trans))
						{
							itemCmd.Set("itemId", reader.GetInt32("itemId"));
							itemCmd.Set("amount", reader.GetInt32("amount"));
							itemCmd.Execute();
							newItemId = itemCmd.LastId;
						}

						// Link to storage
						using (var linkCmd = new InsertCommand("INSERT INTO `storage_personal` {0}", conn, trans))
						{
							linkCmd.Set("characterId", characterId);
							linkCmd.Set("itemId", newItemId);
							linkCmd.Set("position", reader.GetInt32("position"));
							linkCmd.Execute();
						}
					}
				}
			}

			// Note: Team storage restoration would need account-level coordination
			// and is more complex due to shared nature
		}

		#endregion

		#region Helper Methods

		private string GetVariableTypeString(object value)
		{
			if (value is byte) return "1u";
			else if (value is ushort) return "2u";
			else if (value is uint) return "4u";
			else if (value is ulong) return "8u";
			else if (value is sbyte) return "1";
			else if (value is short) return "2";
			else if (value is int) return "4";
			else if (value is long) return "8";
			else if (value is float) return "f";
			else if (value is double) return "d";
			else if (value is bool) return "b";
			else if (value is string) return "s";
			else if (value is DateTime) return "dt";
			else if (value is TimeSpan) return "ts";
			else return "s"; // Default to string
		}

		private string GetVariableValueString(object value)
		{
			return value switch
			{
				float f => f.ToString(System.Globalization.CultureInfo.InvariantCulture),
				double d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
				DateTime dt => dt.Ticks.ToString(),
				TimeSpan ts => ts.Ticks.ToString(),
				_ => value?.ToString() ?? ""
			};
		}

		#endregion

		#region Management and Cleanup Methods

		/// <summary>
		/// Lists available snapshots for a character with filtering options.
		/// </summary>
		public List<SnapshotInfo> GetCharacterSnapshots(long characterId, DateTime? fromDate = null, DateTime? toDate = null, string reasonFilter = null)
		{
			var snapshots = new List<SnapshotInfo>();

			using (var conn = this.GetConnection())
			{
				var whereClause = "characterId = @characterId";
				var parameters = new List<(string, object)> { ("@characterId", characterId) };

				if (fromDate.HasValue)
				{
					whereClause += " AND createdAt >= @fromDate";
					parameters.Add(("@fromDate", fromDate.Value));
				}

				if (toDate.HasValue)
				{
					whereClause += " AND createdAt <= @toDate";
					parameters.Add(("@toDate", toDate.Value));
				}

				if (!string.IsNullOrEmpty(reasonFilter))
				{
					whereClause += " AND snapshotReason LIKE @reasonFilter";
					parameters.Add(("@reasonFilter", $"%{reasonFilter}%"));
				}

				using (var cmd = new MySqlCommand($"SELECT * FROM `character_snapshots` WHERE {whereClause} ORDER BY createdAt DESC", conn))
				{
					foreach (var (paramName, paramValue) in parameters)
					{
						cmd.Parameters.AddWithValue(paramName, paramValue);
					}

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							snapshots.Add(new SnapshotInfo
							{
								SnapshotId = reader.GetInt64("snapshotId"),
								CharacterId = reader.GetInt64("characterId"),
								AccountId = reader.GetInt64("accountId"),
								Reason = reader.GetString("snapshotReason"),
								OperationDetails = reader.GetStringSafe("operationDetails"),
								CreatedAt = reader.GetDateTime("createdAt"),
								GameVersion = reader.GetStringSafe("gameVersion"),
								CharacterLevel = reader.GetInt32("characterLevel"),
								MapId = reader.GetInt32("mapId")
							});
						}
					}
				}
			}

			return snapshots;
		}

		/// <summary>
		/// Cleans up old snapshots based on retention policy.
		/// </summary>
		public int CleanupOldSnapshots(TimeSpan retentionPeriod)
		{
			var cutoffDate = DateTime.UtcNow - retentionPeriod;
			var deletedCount = 0;

			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					// Get snapshot IDs to delete
					var snapshotIdsToDelete = new List<long>();
					using (var cmd = new MySqlCommand("SELECT snapshotId FROM `character_snapshots` WHERE createdAt < @cutoffDate AND snapshotReason != 'CRITICAL_PRESERVE'", conn, trans))
					{
						cmd.Parameters.AddWithValue("@cutoffDate", cutoffDate);
						using (var reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								snapshotIdsToDelete.Add(reader.GetInt64(0));
							}
						}
					}

					if (snapshotIdsToDelete.Any())
					{
						var idParams = snapshotIdsToDelete.Select((id, i) => $"@id{i}").ToArray();
						var inClause = string.Join(",", idParams);

						// Delete snapshot data (order matters due to foreign keys)
						var tables = new[] {
							"snapshot_item_properties",
							"snapshot_items",
							"snapshot_cards",
							"snapshot_character_properties",
							"snapshot_character_etc_properties",
							"snapshot_variables",
							"snapshot_jobs",
							"snapshot_skills",
							"snapshot_abilities",
							"snapshot_quest_progress",
							"snapshot_quests",
							"snapshot_social_data",
							"snapshot_storage_personal",
							"snapshot_storage_team",
							"snapshot_character_data",
							"character_snapshots"
						};

						foreach (var table in tables)
						{
							using (var deleteCmd = new MySqlCommand($"DELETE FROM `{table}` WHERE snapshotId IN ({inClause})", conn, trans))
							{
								for (int i = 0; i < snapshotIdsToDelete.Count; i++)
								{
									deleteCmd.Parameters.AddWithValue(idParams[i], snapshotIdsToDelete[i]);
								}
								deleteCmd.ExecuteNonQuery();
							}
						}

						deletedCount = snapshotIdsToDelete.Count;
					}

					trans.Commit();
					Log.Info($"Cleaned up {deletedCount} old character snapshots (older than {cutoffDate})");
				}
				catch (Exception ex)
				{
					try { trans.Rollback(); } catch { }
					Log.Error($"Failed to cleanup old snapshots: {ex}");
					throw;
				}
			}

			return deletedCount;
		}

		/// <summary>
		/// Gets rollback history for auditing purposes.
		/// </summary>
		public List<RollbackLogEntry> GetRollbackHistory(long? characterId = null, DateTime? fromDate = null, DateTime? toDate = null)
		{
			var history = new List<RollbackLogEntry>();

			using (var conn = this.GetConnection())
			{
				var whereClause = "1=1";
				var parameters = new List<(string, object)>();

				if (characterId.HasValue)
				{
					whereClause += " AND characterId = @characterId";
					parameters.Add(("@characterId", characterId.Value));
				}

				if (fromDate.HasValue)
				{
					whereClause += " AND rolledBackAt >= @fromDate";
					parameters.Add(("@fromDate", fromDate.Value));
				}

				if (toDate.HasValue)
				{
					whereClause += " AND rolledBackAt <= @toDate";
					parameters.Add(("@toDate", toDate.Value));
				}

				using (var cmd = new MySqlCommand($"SELECT * FROM `rollback_log` WHERE {whereClause} ORDER BY rolledBackAt DESC", conn))
				{
					foreach (var (paramName, paramValue) in parameters)
					{
						cmd.Parameters.AddWithValue(paramName, paramValue);
					}

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							history.Add(new RollbackLogEntry
							{
								LogId = reader.GetInt64("logId"),
								CharacterId = reader.GetInt64("characterId"),
								SnapshotId = reader.GetInt64("snapshotId"),
								RollbackReason = reader.GetString("rollbackReason"),
								RolledBackAt = reader.GetDateTime("rolledBackAt"),
								OriginalSnapshotReason = reader.GetString("originalSnapshotReason"),
								OriginalSnapshotDate = reader.GetDateTime("originalSnapshotDate")
							});
						}
					}
				}
			}

			return history;
		}

		#endregion
	}

	#region Data Transfer Objects

	public class SnapshotInfo
	{
		public long SnapshotId { get; set; }
		public long CharacterId { get; set; }
		public long AccountId { get; set; }
		public string Reason { get; set; }
		public string OperationDetails { get; set; }
		public DateTime CreatedAt { get; set; }
		public string GameVersion { get; set; }
		public int CharacterLevel { get; set; }
		public int MapId { get; set; }
	}

	public class RollbackResult
	{
		public List<CharacterRollbackInfo> SuccessfulRollbacks { get; set; } = new();
		public List<CharacterRollbackInfo> FailedRollbacks { get; set; } = new();

		public int TotalAttempted => SuccessfulRollbacks.Count + FailedRollbacks.Count;
		public bool HasFailures => FailedRollbacks.Any();
	}

	public class CharacterRollbackInfo
	{
		public long CharacterId { get; set; }
		public long SnapshotId { get; set; }
		public DateTime OriginalSnapshotDate { get; set; }
		public string OriginalReason { get; set; }
		public string ErrorMessage { get; set; }
	}

	public class RollbackLogEntry
	{
		public long LogId { get; set; }
		public long CharacterId { get; set; }
		public long SnapshotId { get; set; }
		public string RollbackReason { get; set; }
		public DateTime RolledBackAt { get; set; }
		public string OriginalSnapshotReason { get; set; }
		public DateTime OriginalSnapshotDate { get; set; }
	}

	#endregion
}
