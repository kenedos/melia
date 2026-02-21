using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Maps;
using MySqlConnector;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Contains methods related to Account persistence.
	/// </summary>
	public partial class ZoneDb
	{
		public bool SaveAccountData(Account account)
		{
			if (account == null)
				throw new ArgumentNullException(nameof(account));

			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `accounts` SET {0} WHERE `accountId` = @accountId", conn))
			{
				cmd.AddParameter("@accountId", account.Id);
				cmd.Set("settings", account.Settings.ToString());
				cmd.Set("premiumTokenExpiration", account.Premium.Token.Expiration);
				cmd.Set("medals", account.Medals);
				cmd.Set("giftMedals", account.GiftMedals);
				cmd.Set("premiumMedals", account.PremiumMedals);

				if (cmd.Execute() == 0)
					return false;
			}

			this.SaveVariables(account.Variables.Perm, "vars_accounts", "accountId", account.Id);
			this.SaveProperties("account_properties", "accountId", account.Id, account.Properties);
			this.SaveChatMacros(account);
			this.SaveRevealedMaps(account);
			this.SaveStorage(account.TeamStorage, "storage_team", "accountId", account.Id);

			return true;
		}

		public Account GetAccount(string name)
		{
			var account = new Account();

			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `accounts` WHERE `name` = @name", conn))
			{
				mc.Parameters.AddWithValue("@name", name);

				using (var reader = mc.ExecuteReader())
				{
					if (!reader.Read())
						return null;

					account.Id = reader.GetInt64("accountId");
					account.Name = reader.GetStringSafe("name");
					account.TeamName = reader.GetStringSafe("teamName");
					account.Authority = reader.GetInt32("authority");
					account.PermissionLevel = (PermissionLevel)reader.GetByte("type");
					account.Settings.Parse(reader.GetStringSafe("settings"));
					account.Medals = reader.GetInt32("medals");
					account.GiftMedals = reader.GetInt32("giftMedals");
					account.PremiumMedals = reader.GetInt32("premiumMedals");
					account.Premium.Token.Expiration = reader.GetDateTimeSafe("premiumTokenExpiration");
					var creationDate = reader.GetDateTimeSafe("creationDate");

					if (creationDate == DateTime.MinValue)
						creationDate = new DateTime(2015, 1, 1);
					account.CreationDate = creationDate;
				}
			}

			this.LoadProperties("account_properties", "accountId", account.Id, account.Properties);
			this.LoadVars(account.Variables.Perm, "vars_accounts", "accountId", account.Id);
			this.LoadChatMacros(account);
			this.LoadRevealedMaps(account);

			return account;
		}

		private void LoadChatMacros(Account account)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `chatmacros` WHERE `accountId` = @accountId ORDER BY `index` DESC", conn))
			{
				mc.Parameters.AddWithValue("@accountId", account.Id);

				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var index = reader.GetInt32("index");
						var message = reader.GetString("message");
						var pose = reader.GetInt32("pose");

						var macro = new ChatMacro(index, message, pose);
						account.AddChatMacro(macro);
					}
				}
			}
		}

		private void SaveChatMacros(Account account)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				var macros = account.GetChatMacros().ToList();

				if (!macros.Any())
				{
					using (var mc = new MySqlCommand("DELETE FROM `chatmacros` WHERE `accountId` = @accountId", conn, trans))
					{
						mc.Parameters.AddWithValue("@accountId", account.Id);
						mc.ExecuteNonQuery();
					}
					trans.Commit();
					return;
				}

				var macroIndicesInMemory = new HashSet<int>(macros.Select(m => m.Index));

				foreach (var macro in macros)
				{
					using (var cmd = new MySqlCommand(
						"INSERT INTO `chatmacros` (`accountId`, `index`, `message`, `pose`) VALUES (@accountId, @index, @message, @pose) " +
						"ON DUPLICATE KEY UPDATE `message` = VALUES(`message`), `pose` = VALUES(`pose`)", conn, trans))
					{
						cmd.Parameters.AddWithValue("@accountId", account.Id);
						cmd.Parameters.AddWithValue("@index", macro.Index);
						cmd.Parameters.AddWithValue("@message", macro.Message);
						cmd.Parameters.AddWithValue("@pose", macro.Pose);
						cmd.ExecuteNonQuery();
					}
				}

				var macroIndicesInDb = new HashSet<int>();
				using (var cmd = new MySqlCommand("SELECT `index` FROM `chatmacros` WHERE `accountId` = @accountId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@accountId", account.Id);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							macroIndicesInDb.Add(reader.GetInt32(0));
					}
				}

				var indicesToDelete = macroIndicesInDb.Except(macroIndicesInMemory).ToList();
				if (indicesToDelete.Any())
				{
					var deleteParams = indicesToDelete.Select((idx, i) => $"@idx{i}").ToArray();
					using (var cmd = new MySqlCommand($"DELETE FROM `chatmacros` WHERE `accountId` = @accountId AND `index` IN ({string.Join(",", deleteParams)})", conn, trans))
					{
						cmd.Parameters.AddWithValue("@accountId", account.Id);
						for (var i = 0; i < indicesToDelete.Count; i++)
							cmd.Parameters.AddWithValue(deleteParams[i], indicesToDelete[i]);
						cmd.ExecuteNonQuery();
					}
				}

				trans.Commit();
			}
		}

		private void LoadRevealedMaps(Account account)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `revealedmaps` WHERE `accountId` = @accountId", conn))
			{
				mc.Parameters.AddWithValue("@accountId", account.Id);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var map = reader.GetInt32("map");
						var explored = (byte[])reader["explored"];
						var percentage = reader.GetFloat("percentage");

						var revealedMap = new RevealedMap(map, explored, percentage);
						account.AddRevealedMap(revealedMap);
					}
				}
			}
		}

		private void SaveRevealedMaps(Account account)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				using (var mc = new MySqlCommand("DELETE FROM `revealedmaps` WHERE `accountId` = @accountId", conn, trans))
				{
					mc.Parameters.AddWithValue("@accountId", account.Id);
					mc.ExecuteNonQuery();
				}

				foreach (var revealedMap in account.GetRevealedMaps())
				{
					using (var cmd = new InsertCommand("INSERT INTO `revealedmaps` {0}", conn, trans))
					{
						cmd.Set("accountId", account.Id);
						cmd.Set("map", revealedMap.MapId);
						cmd.Set("explored", revealedMap.Explored);
						cmd.Set("percentage", revealedMap.Percentage);
						cmd.Execute();
					}
				}
				trans.Commit();
			}
		}
	}
}
