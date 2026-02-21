using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Shared.World;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Groups;
using MySqlConnector;
using Yggdrasil.Logging;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Contains methods related to Social (Party/Guild) persistence.
	/// </summary>
	public partial class ZoneDb
	{
		#region Party Methods
		public void CreateParty(Party party)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					using (var cmd = new InsertCommand("INSERT INTO `party` {0}", conn, trans))
					{
						party.DateCreated = DateTime.Now;
						cmd.Set("name", party.Name);
						cmd.Set("leaderId", party.LeaderDbId);
						cmd.Set("note", party.Note);
						cmd.Set("questSharing", (int)party.QuestSharing);
						cmd.Set("expDistribution", (int)party.ExpDistribution);
						cmd.Set("itemDistribution", (int)party.ItemDistribution);
						cmd.Set("dateCreated", party.DateCreated);
						cmd.Execute();
						party.DbId = cmd.LastId;
					}

					using (var cmdUpdateLeader = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn, trans))
					{
						cmdUpdateLeader.AddParameter("@characterId", party.LeaderDbId);
						cmdUpdateLeader.Set("partyId", party.DbId);
						if (cmdUpdateLeader.Execute() == 0)
						{
							Log.Error($"CreateParty: Failed to update partyId for leader character {party.LeaderDbId}.");
							throw new InvalidOperationException($"Failed to update leader character {party.LeaderDbId}");
						}
					}
					trans.Commit();
				}
				catch (Exception ex)
				{
					Log.Error($"CreateParty: Transaction failed (Leader: {party.LeaderDbId}): {ex}");
					try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"CreateParty: Rollback failed: {rbEx}"); }
					throw;
				}
			}
		}

		public void LoadParty(Character character)
		{
			if (character.PartyId <= 0)
				return;
			var party = ZoneServer.Instance.World.GetParty(character.PartyId);
			if (party == null)
			{
				using (var conn = this.GetConnection())
				using (var mc = new MySqlCommand("SELECT * FROM `party` WHERE `partyId` = @partyId", conn))
				{
					mc.Parameters.AddWithValue("@partyId", character.PartyId);
					using (var reader = mc.ExecuteReader())
					{
						if (reader.Read())
						{
							party = new Party(reader.GetInt64("partyId"), reader.GetInt64("leaderId"), reader.GetString("name"), reader.GetDateTime("dateCreated"), reader.GetString("note"),
								(PartyItemDistribution)reader.GetByte("itemDistribution"), (PartyExpDistribution)reader.GetByte("expDistribution"), (PartyQuestSharing)reader.GetByte("questSharing"));
							this.LoadPartyMembers(character, party);
							ZoneServer.Instance.World.Parties.Add(party);
						}
						else
						{
							// Party no longer exists in database - clear stale reference
							character.PartyId = 0;
						}
					}
				}
			}
			else
			{
				if (party.TryGetMember(character.ObjectId, out var member))
					member.IsOnline = true;
			}
		}

		private void LoadPartyMembers(Character loadCharacter, Party party)
		{
			using (var conn = this.GetConnection())
			{
				var offlineMembers = new List<PartyMember>();

				using (var mc = new MySqlCommand("SELECT * FROM `characters` WHERE `partyId` = @partyId", conn))
				{
					mc.Parameters.AddWithValue("@partyId", party.DbId);
					using (var reader = mc.ExecuteReader())
					{
						while (reader.Read())
						{
							var character = ZoneServer.Instance.World.GetCharacter(c => c.DbId == reader.GetInt64("characterId"));
							if (character == null)
							{
								var member = new PartyMember
								{
									DbId = reader.GetInt64("characterId"),
									AccountId = reader.GetInt64("accountId"),
									Name = reader.GetString("name"),
									TeamName = reader.GetString("teamName"),
									VisualJobId = (JobId)reader.GetInt16("job"),
									Gender = (Gender)reader.GetByte("gender"),
									Hair = reader.GetInt32("hair"),
									MapId = reader.GetInt32("zone"),
									Level = reader.GetInt32("level"),
								};
								var x = reader.GetFloat("x");
								var y = reader.GetFloat("y");
								var z = reader.GetFloat("z");
								member.Position = new Position(x, y, z);
								member.IsOnline = loadCharacter.DbId == member.DbId;
								offlineMembers.Add(member);
							}
							else
								party.AddMember(character, true);
						}
					}
				}

				// Load jobs for all offline members in a single query (avoids N+1)
				if (offlineMembers.Any())
				{
					var memberIds = offlineMembers.Select(m => m.DbId).ToList();
					var memberDict = offlineMembers.ToDictionary(m => m.DbId);
					var idParams = memberIds.Select((id, i) => $"@id{i}").ToArray();

					using (var mc = new MySqlCommand($"SELECT `characterId`, `jobId` FROM `jobs` WHERE `characterId` IN ({string.Join(",", idParams)}) ORDER BY `characterId`, `selectionDate` ASC", conn))
					{
						for (var i = 0; i < memberIds.Count; i++)
							mc.Parameters.AddWithValue(idParams[i], memberIds[i]);

						using (var reader = mc.ExecuteReader())
						{
							long currentCharId = 0;
							var jobIndex = 0;

							while (reader.Read())
							{
								var charId = reader.GetInt64("characterId");
								var jobId = (JobId)reader.GetInt32("jobId");

								if (charId != currentCharId)
								{
									currentCharId = charId;
									jobIndex = 0;
								}

								if (memberDict.TryGetValue(charId, out var member))
								{
									member.VisualJobId = jobId;
									switch (jobIndex)
									{
										case 0: member.FirstJobId = jobId; break;
										case 1: member.SecondJobId = jobId; break;
										case 2: member.ThirdJobId = jobId; break;
										case 3: member.FourthJobId = jobId; break;
									}
									jobIndex++;
								}
							}
						}
					}
				}

				foreach (var member in offlineMembers)
					party.AddMember(member);
			}
		}

		public void SaveParty(Character character)
		{
			var party = character.Connection.Party;
			if (party == null || !party.IsLeader(character.ObjectId))
				return;
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				using (var cmd = new UpdateCommand("UPDATE `party` SET {0} WHERE `partyId` = @partyId", conn, trans))
				{
					cmd.AddParameter("@partyId", party.DbId);
					cmd.Set("name", party.Name);
					cmd.Set("leaderId", party.LeaderDbId);
					cmd.Set("note", party.Note);
					cmd.Set("questSharing", (int)party.QuestSharing);
					cmd.Set("expDistribution", (int)party.ExpDistribution);
					cmd.Set("itemDistribution", (int)party.ItemDistribution);
					cmd.Execute();
				}
				trans.Commit();
			}
		}

		public void DeleteParty(Party party)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				using (var cmd = new MySqlCommand("DELETE FROM `party` WHERE `partyId` = @partyId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@partyId", party.DbId);
					cmd.ExecuteNonQuery();
				}
				foreach (var member in party.GetMembers())
				{
					using (var cmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn, trans))
					{
						cmd.AddParameter("@characterId", member.DbId);
						cmd.Set("partyId", 0);
						cmd.Execute();
					}
				}
				trans.Commit();
			}
		}

		public void UpdatePartyId(long dbId, long partyId = 0)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn))
			{
				cmd.AddParameter("@characterId", dbId);
				cmd.Set("partyId", partyId);
				cmd.Execute();
			}
		}
		#endregion

		#region Guild Methods
		// Removed: All Guild methods commented out - Guild type deleted during Laima merge

		/*
		public void CreateGuild(Guild guild)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					// Insert Guild with Leader Account ID
					using (var cmd = new InsertCommand("INSERT INTO `guilds` {0}", conn, trans))
					{
						guild.DateCreated = DateTime.Now;
						cmd.Set("name", guild.Name);
						cmd.Set("leaderId", guild.LeaderDbId);
						cmd.Set("dateCreated", guild.DateCreated);
						cmd.Set("profile", guild.Profile ?? "");
						cmd.Set("exp", guild.Exp);
						cmd.Set("level", guild.Level);
						cmd.Set("notice", guild.Notice ?? "");
						cmd.Execute();
						guild.DbId = cmd.LastId;
					}

					// Save guild global properties
					this.SaveProperties("guild_properties", "guildId", guild.DbId, guild.Properties, conn, trans);

					// Update members (Characters) to visually show the guild
					foreach (var member in guild.GetMembers())
					{
						// Visual link: Update character table so Zone knows to load guild on login
						// Note: We update ALL characters on this account if you want shared membership, 
						// but usually creation involves the specific character present.
						using (var cmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn, trans))
						{
							cmd.AddParameter("@characterId", member.DbId);
							cmd.Set("guildId", guild.DbId);
							cmd.Execute();
						}

						// Save guild member properties (Linked to ACCOUNT)
						if (member.Properties != null)
						{
							foreach (var property in member.Properties.GetAll())
							{
								var typeStr = property is FloatProperty ? "f" : "s";
								var valueStr = property.Serialize();

								using (var cmd = new InsertCommand("INSERT INTO `guild_member_properties` {0}", conn, trans))
								{
									cmd.Set("guildId", guild.DbId);
									cmd.Set("accountId", member.AccountId);
									cmd.Set("name", property.Ident);
									cmd.Set("type", typeStr);
									cmd.Set("value", valueStr);
									cmd.Execute();
								}
							}
						}
					}
					trans.Commit();
				}
				catch (Exception ex)
				{
					Log.Error($"CreateGuild: Transaction failed (Leader: {guild.LeaderDbId}): {ex}");
					try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"CreateGuild: Rollback failed: {rbEx}"); }
					throw;
				}
			}
		}

		/// <summary>
		/// Saves guild data and properties to the database.
		/// </summary>
		/// <param name="guild">The guild to save.</param>
		public void SaveGuild(Guild guild)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					using (var cmd = new UpdateCommand("UPDATE `guilds` SET {0} WHERE `guildId` = @guildId", conn, trans))
					{
						cmd.AddParameter("@guildId", guild.DbId);
						cmd.Set("name", guild.Name);
						cmd.Set("leaderId", guild.LeaderDbId);
						cmd.Set("profile", guild.Profile ?? "");
						cmd.Set("exp", guild.Exp);
						cmd.Set("level", guild.Level);
						cmd.Set("notice", guild.Notice ?? "");
						cmd.Execute();
					}

					// Save guild global properties
					this.SaveProperties("guild_properties", "guildId", guild.DbId, guild.Properties, conn, trans);

					trans.Commit();
				}
				catch (Exception ex)
				{
					Log.Error($"SaveGuild: Transaction failed (Guild: {guild.DbId}): {ex}");
					try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"SaveGuild: Rollback failed: {rbEx}"); }
					throw;
				}
			}
		}

		public void LoadGuild(Character character)
		{
			if (character.GuildId <= 0)
				return;

			var guild = ZoneServer.Instance.World.GetGuild(character.GuildId);
			if (guild == null)
			{
				using (var conn = this.GetConnection())
				using (var mc = new MySqlCommand("SELECT * FROM `guilds` WHERE `guildId` = @guildId", conn))
				{
					mc.Parameters.AddWithValue("@guildId", character.GuildId);
					using (var reader = mc.ExecuteReader())
					{
						if (reader.Read())
						{
							guild = new Guild();
							guild.DbId = reader.GetInt64("guildId");
							guild.Name = reader.GetString("name");
							guild.LeaderDbId = reader.GetInt64("leaderId");
							guild.DateCreated = reader.GetDateTime("dateCreated");
							guild.Profile = reader.GetString("profile");
							guild.Exp = reader.GetInt32("exp");
							guild.Notice = reader.GetString("notice");

							// Load guild global properties
							this.LoadGuildProperties(guild);

							// Load members and their account-bound properties
							this.LoadGuildMembers(character, guild);

							ZoneServer.Instance.World.Guilds.Add(guild);
						}
					}
				}
			}
			else
			{
				// Guild already loaded in memory, mark this char as online
				if (guild.TryGetMember(character.ObjectId, out var member))
					member.IsOnline = true;
			}
		}

		private void LoadGuildMembers(Character loadCharacter, Guild guild)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `characters` WHERE `guildId` = @guildId", conn))
			{
				mc.Parameters.AddWithValue("@guildId", guild.DbId);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var charDbId = reader.GetInt64("characterId");

						// Check if character is already online in World
						var character = ZoneServer.Instance.World.GetCharacter(c => c.DbId == charDbId);

						if (character == null)
						{
							// Create offline member representation
							var member = new Character
							{
								DbId = charDbId,
								AccountDbId = reader.GetInt64("accountId"),
								Name = reader.GetString("name"),
								TeamName = reader.GetString("teamName"),
								Gender = (Gender)reader.GetByte("gender"),
								Hair = reader.GetInt32("hair"),
								MapId = reader.GetInt32("zone"),
								SkinColor = reader.GetUInt32("skinColor"),
								Exp = reader.GetInt64("exp"),
								MaxExp = reader.GetInt64("maxExp"),
								TotalExp = reader.GetInt64("totalExp"),
								Position = new Position(reader.GetFloat("x"), reader.GetFloat("y"), reader.GetFloat("z")),
								IsOnline = loadCharacter.DbId == charDbId // Check if this is the loading player
							};

							var guildMember = GroupMember.ToGuildMember(member);
							guildMember.Level = reader.GetInt32("level");

							// Load Member Properties using ACCOUNT ID
							this.LoadProperties("guild_member_properties", "accountId", guildMember.AccountId, guildMember.Properties);

							guild.AddMember(guildMember);
						}
						else
						{
							// Character is online
							var guildMember = GroupMember.ToGuildMember(character);

							// Load guild member properties once
							this.LoadProperties("guild_member_properties", "accountId", guildMember.AccountId, guildMember.Properties);

							guild.AddMember(character, true);
						}
					}
				}
			}
		}

		/// <summary>
		/// Saves guild properties to the database.
		/// </summary>
		/// <param name="guild">The guild to save properties for.</param>
		public void SaveGuildProperties(Guild guild)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					this.InternalSaveProperties("guild_properties", "guildId", guild.DbId, guild.Properties, conn, trans);
					trans.Commit();
				}
				catch (Exception ex)
				{
					Log.Error($"SaveGuildProperties: Transaction failed (Guild: {guild.DbId}): {ex}");
					try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"SaveGuildProperties: Rollback failed: {rbEx}"); }
					throw;
				}
			}
		}

		/// <summary>
		/// Saves guild member properties to the database.
		/// </summary>
		/// <param name="guildId">The guild ID.</param>
		/// <param name="member">The guild member to save properties for.</param>
		public void SaveGuildMemberProperties(long guildId, IMember member)
		{
			if (member.Properties == null)
				return;

			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					// Delete based on AccountId + GuildId
					using (var cmd = new MySqlCommand("DELETE FROM `guild_member_properties` WHERE `guildId` = @guildId AND `accountId` = @accountId", conn, trans))
					{
						cmd.Parameters.AddWithValue("@guildId", guildId);
						cmd.Parameters.AddWithValue("@accountId", member.AccountId);
						cmd.ExecuteNonQuery();
					}

					// Save properties using AccountId
					foreach (var property in member.Properties.GetAll())
					{
						var typeStr = property is FloatProperty ? "f" : "s";
						var valueStr = property.Serialize();

						using (var cmd = new InsertCommand("INSERT INTO `guild_member_properties` {0}", conn, trans))
						{
							cmd.Set("guildId", guildId);
							cmd.Set("accountId", member.AccountId);
							cmd.Set("name", property.Ident);
							cmd.Set("type", typeStr);
							cmd.Set("value", valueStr);
							cmd.Execute();
						}
					}

					trans.Commit();
				}
				catch (Exception ex)
				{
					Log.Error($"SaveGuildMemberProperties: Transaction failed (Guild: {guildId}, Account: {member.AccountId}): {ex}");
					try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"SaveGuildMemberProperties: Rollback failed: {rbEx}"); }
					throw;
				}
			}
		}

		/// <summary>
		/// Loads guild properties from the database.
		/// </summary>
		/// <param name="guild">The guild to load properties for.</param>
		public void LoadGuildProperties(Guild guild)
		{
			this.LoadProperties("guild_properties", "guildId", guild.DbId, guild.Properties);
		}

		public void UpdateGuildId(long dbId, long guildId = 0)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn))
			{
				cmd.AddParameter("@characterId", dbId);
				cmd.Set("guildId", guildId);
				cmd.Execute();
			}
		}
		*/
		#endregion

		#region Duel Methods
		/// <summary>
		/// Save the duel result atomically.
		/// </summary>
		public void SaveDuelResult(long winnerId, long loserId)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					using (var cmd = new MySqlCommand("INSERT INTO duels (winnerId, loserId, duelTime) VALUES (@winnerId, @loserId, @duelTime)", conn, trans))
					{
						cmd.Parameters.AddWithValue("@winnerId", winnerId);
						cmd.Parameters.AddWithValue("@loserId", loserId);
						cmd.Parameters.AddWithValue("@duelTime", DateTime.UtcNow);
						cmd.ExecuteNonQuery();
					}

					using (var cmd = new UpdateCommand("UPDATE characters SET duelWins = duelWins + 1 WHERE characterId = @winnerId", conn, trans))
					{
						cmd.AddParameter("@winnerId", winnerId);
						cmd.Execute();
					}

					using (var cmd = new UpdateCommand("UPDATE characters SET duelLosses = duelLosses + 1 WHERE characterId = @loserId", conn, trans))
					{
						cmd.AddParameter("@loserId", loserId);
						cmd.Execute();
					}

					trans.Commit();
				}
				catch (Exception ex)
				{
					Log.Error($"Failed to save duel result for winner {winnerId} and loser {loserId}: {ex}");
					try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"Rollback failed for SaveDuelResult: {rbEx}"); }
					throw;
				}
			}
		}
		#endregion
	}
}
