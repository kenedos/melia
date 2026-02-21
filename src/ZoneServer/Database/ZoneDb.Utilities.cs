using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Melia.Shared.Database;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Scripting;
using MySqlConnector;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Contains generic utility and helper methods for database interaction.
	/// </summary>
	public partial class ZoneDb
	{
		public void LoadPropertiesInBatch(string tableName, string idFieldName, List<long> ids, Dictionary<long, Properties> propertyMap)
		{
			if (ids == null || !ids.Any()) return;

			const int CHUNK_SIZE = 1000;
			for (var i = 0; i < ids.Count; i += CHUNK_SIZE)
			{
				var chunk = ids.Skip(i).Take(CHUNK_SIZE).ToList();
				var idParams = chunk.Select((id, index) => $"@id{i + index}").ToArray();

				using (var conn = this.GetConnection())
				using (var cmd = new MySqlCommand($"SELECT `{idFieldName}`, `name`, `type`, `value` FROM `{tableName}` WHERE `{idFieldName}` IN ({string.Join(",", idParams)})", conn))
				{
					for (var j = 0; j < chunk.Count; j++)
					{
						cmd.Parameters.AddWithValue(idParams[j], chunk[j]);
					}

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var ownerId = reader.GetInt64(0);
							var propName = reader.GetString(1);
							var propType = reader.GetString(2);
							var propValue = reader.GetString(3);

							if (propertyMap.TryGetValue(ownerId, out var properties))
							{
								if (properties.TryGet<IProperty>(propName, out var existingProp) && existingProp is IUnsettableProperty)
								{
									continue;
								}

								if (propType == "f")
								{
									if (!properties.TryGet<FloatProperty>(propName, out var p)) p = properties.Create(new FloatProperty(propName));
									p.Deserialize(propValue);
								}
								else
								{
									if (!properties.TryGet<StringProperty>(propName, out var p)) p = properties.Create(new StringProperty(propName));
									p.Deserialize(propValue);
								}
							}
						}
					}
				}
			}
		}

		public void SaveVariables(Variables vars, string tableName, string? ownerField = null, long ownerId = 0)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				this.InternalSaveVariables(vars, tableName, ownerField, ownerId, conn, trans);
				trans.Commit();
			}
		}

		public void LoadVars(Variables vars, string tableName, string? ownerField = null, long ownerId = 0)
		{
			using (var conn = this.GetConnection())
				this.LoadVars(conn, vars, tableName, ownerField, ownerId);
		}

		protected void LoadVars(MySqlConnection conn, Variables vars, string tableName, string ownerField, long ownerId)
		{
			var where = !string.IsNullOrEmpty(ownerField) ? $"`{ownerField}` = @ownerId" : "1";
			using (var mc = new MySqlCommand($"SELECT * FROM `{tableName}` WHERE {where}", conn))
			{
				if (!string.IsNullOrEmpty(ownerField))
					mc.Parameters.AddWithValue("@ownerId", ownerId);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var name = reader.GetString("name");
						var type = reader.GetString("type");
						var val = reader.GetStringSafe("value");
						if (val == null) continue;
						try
						{
							switch (type)
							{
								case "1u": vars.Set(name, byte.Parse(val)); break;
								case "2u": vars.Set(name, ushort.Parse(val)); break;
								case "4u": vars.Set(name, uint.Parse(val)); break;
								case "8u": vars.Set(name, ulong.Parse(val)); break;
								case "1": vars.Set(name, sbyte.Parse(val)); break;
								case "2": vars.Set(name, short.Parse(val)); break;
								case "4": vars.Set(name, int.Parse(val)); break;
								case "8": vars.Set(name, long.Parse(val)); break;
								case "f": vars.Set(name, float.Parse(val, CultureInfo.InvariantCulture)); break;
								case "d": vars.Set(name, double.Parse(val, CultureInfo.InvariantCulture)); break;
								case "b": vars.Set(name, bool.Parse(val)); break;
								case "s": vars.Set(name, val); break;
								case "dt": vars.Set(name, new DateTime(long.Parse(val))); break;
								case "ts": vars.Set(name, new TimeSpan(long.Parse(val))); break;
								default: Log.Warning("LoadVars: Unknown variable type '{0}'.", type); continue;
							}
						}
						catch (FormatException) { Log.Warning("LoadVars: Variable '{0}' could not be parsed as type '{1}'. Value: '{2}'", name, type, val); }
						catch (OverflowException) { Log.Warning("LoadVars: Value '{2}' of variable '{0}' doesn't fit into type '{1}'.", name, type, val); }
					}
				}
			}
		}

		public bool SaveSessionKey(long characterId, string key)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn))
			{
				cmd.AddParameter("@characterId", characterId);
				cmd.Set("sessionKey", key);
				return cmd.Execute() == 1;
			}
		}

		public void LogItemTransaction(long characterId, string characterName, long itemObjectId, long itemDbId, int itemId, string itemName, int amount, string transactionType, string reason)
		{
			if (amount == 0) return;
			using (var conn = this.GetConnection())
			using (var cmd = new InsertCommand("INSERT INTO `log_item_transactions` (`characterId`, `characterName`, `itemObjectId`, `itemDbId`, `itemId`, `itemName`, `amount`, `transactionType`, `reason`, `timestamp`) VALUES (@characterId, @characterName, @itemObjectId, @itemDbId, @itemId, @itemName, @amount, @transactionType, @reason, @timestamp)", conn))
			{
				cmd.AddParameter("@characterId", characterId);
				cmd.AddParameter("@characterName", characterName);
				cmd.AddParameter("@itemObjectId", itemObjectId);
				cmd.AddParameter("@itemDbId", itemDbId);
				cmd.AddParameter("@itemId", itemId);
				cmd.AddParameter("@itemName", itemName);
				cmd.AddParameter("@amount", amount);
				cmd.AddParameter("@transactionType", transactionType);
				cmd.AddParameter("@reason", reason);
				cmd.AddParameter("@timestamp", DateTime.UtcNow);
				cmd.Execute();
			}
		}

		public enum LogChatType { Normal, Whisper, Party, Guild, Command }

		public void LogChat(long characterId, string characterName, int chatType, string message, string targetName = null)
		{
			if (string.IsNullOrEmpty(message)) return;
			using (var conn = this.GetConnection())
			using (var cmd = new InsertCommand("INSERT INTO `log_chat` (`characterId`, `characterName`, `chatType`, `message`, `targetName`, `timestamp`) VALUES (@characterId, @characterName, @chatType, @message, @targetName, @timestamp)", conn))
			{
				cmd.AddParameter("@characterId", characterId);
				cmd.AddParameter("@characterName", characterName);
				cmd.AddParameter("@chatType", chatType);
				cmd.AddParameter("@message", message);
				cmd.AddParameter("@targetName", targetName ?? (object)DBNull.Value);
				cmd.AddParameter("@timestamp", DateTime.UtcNow);
				cmd.Execute();
			}
		}

		/// <summary>
		/// Returns global job count.
		/// </summary>
		public Dictionary<int, int> GetGlobalJobCount()
		{
			var jobDictionary = new Dictionary<int, int>();

			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT `jobId`, count(*) as `jobCount` FROM `jobs` GROUP BY `jobId` ORDER BY `jobCount` DESC", conn))
			{
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						jobDictionary.Add(reader.GetInt32("jobId"), reader.GetInt32("jobCount"));
					}
				}
			}

			return jobDictionary;
		}
	}
}
