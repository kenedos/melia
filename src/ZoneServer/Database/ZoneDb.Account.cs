using System;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.World;
using Melia.Zone.World.Maps;
using MySqlConnector;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Contains methods related to Account persistence.
	/// </summary>
	public partial class ZoneDb
	{
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
					account.Language = reader.GetStringSafe("language");
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

	}
}
