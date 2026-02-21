using System;
using System.Diagnostics;
using System.Threading;
using Melia.Shared.Data.Database;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Shared.World;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Storages;
using MySqlConnector;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Handles all database interactions for the Zone server.
	/// This class is divided into partial files based on functionality.
	/// </summary>
	public partial class ZoneDb : MeliaDb
	{
		/// <summary>
		/// Returns given character, or null if it doesn't exist.
		/// </summary>
		public Character GetCharacter(long accountId, long characterId)
		{
			var adjustedCharId = characterId > ObjectIdRanges.Characters ? characterId - ObjectIdRanges.Characters : characterId;
			var charNameForLog = $"ID:{adjustedCharId}"; // Initial name for logging

			using (Debug.Profile($"TotalLoad: {charNameForLog}", 1000))
			{
				var character = new Character();

				using (Debug.Profile($"Load.CharacterTable: {charNameForLog}"))
				{
					using (var conn = this.GetConnection())
					using (var mc = new MySqlCommand("SELECT * FROM `characters` WHERE `accountId` = @accountId AND `characterId` = @characterId", conn))
					{
						mc.Parameters.AddWithValue("@accountId", accountId);
						mc.Parameters.AddWithValue("@characterId", adjustedCharId);

						using (var reader = mc.ExecuteReader())
						{
							if (!reader.Read())
								return null;

							// Populate character base data
							character.DbId = reader.GetInt64("characterId");
							character.AccountDbId = accountId;
							character.Name = reader.GetString("name");
							character.TeamName = reader.GetString("teamName");
							character.JobId = (JobId)reader.GetInt16("job");
							character.Gender = (Gender)reader.GetByte("gender");
							character.Hair = reader.GetInt32("hair");
							character.SkinColor = reader.GetUInt32("skinColor");
							character.MapId = reader.GetInt32("zone");
							character.Exp = reader.GetInt64("exp");
							character.MaxExp = reader.GetInt64("maxExp");
							character.TotalExp = reader.GetInt64("totalExp");
							character.VisibleEquip = (VisibleEquip)reader.GetInt32("equipVisibility");
							character.EquippedTitleId = reader.GetInt32("equippedTitleId");
							var x = reader.GetFloat("x");
							var y = reader.GetFloat("y");
							var z = reader.GetFloat("z");
							var dir = reader.GetFloat("dir");
							character.Position = new Position(x, y, z);
							character.Direction = new Direction(dir);
							character.PartyId = reader.GetInt64("partyId");
							character.GuildId = reader.GetInt64("guildId");
						}
					}
				}

				// Calls to partial class methods for loading components
				using (Debug.Profile($"Load.AllCharacterData: {charNameForLog}", 500))
				{
					this.LoadCharacterComponentData(character, charNameForLog);
				}

				using (Debug.Profile($"Load.FinalizeObject: {charNameForLog}", 50))
				{
					character.InitProperties();
					character.Properties.Stamina = (int)character.Properties.GetFloat(PropertyName.MaxSta);
					character.UpdateStance();

					// Initialize visibility PCEtc properties based on VisibleEquip bitmask
					// These properties control the client UI checkbox state (1 = visible, 0 = hidden)
					character.Etc.Properties.SetFloat(PropertyName.HAT_Visible, (character.VisibleEquip & VisibleEquip.Headgear1) != 0 ? 1 : 0);
					character.Etc.Properties.SetFloat(PropertyName.HAT_T_Visible, (character.VisibleEquip & VisibleEquip.Headgear2) != 0 ? 1 : 0);
					character.Etc.Properties.SetFloat(PropertyName.HAT_L_Visible, (character.VisibleEquip & VisibleEquip.Headgear3) != 0 ? 1 : 0);
					character.Etc.Properties.SetFloat(PropertyName.HAIR_WIG_Visible, (character.VisibleEquip & VisibleEquip.Wig) != 0 ? 1 : 0);
				}

				return character;
			}
		}

		/// <summary>
		/// Finishes loading data that depends on both Account and Character objects being present.
		/// </summary>
		public void AfterLoad(Account account, Character character)
		{
			account.TeamStorage = new TeamStorage(character);
			character.TeamStorage.InitSize();
			this.LoadStorage(character.TeamStorage, "storage_team", "accountId", character.AccountDbId);

			//account.Properties.ClearAllDirtyFlags();
			//character.Properties.ClearAllDirtyFlags();
			//character.Etc.Properties.ClearAllDirtyFlags();

			foreach (var item in account.TeamStorage.GetItems().Values)
			{
				//item.Properties.ClearAllDirtyFlags();
			}
		}

		/// <summary>
		/// Saves all character data within a single, robust, deadlock-aware transaction.
		/// Uses Monitor-based per-character locking to prevent concurrent saves.
		/// </summary>
		private const long SlowSaveThresholdMs = 3000;

		public void SaveCharacterData(Character character)
		{
			if (character == null)
				throw new ArgumentNullException(nameof(character));

			if (character.Variables.Temp.GetBool("Melia.NoSave", false)) return;

			var lockTaken = false;
			object acquiredLock = null;
			const int maxRetries = 3;
			var saveStopwatch = new Stopwatch();
			saveStopwatch.Start();
			long lockMs = 0, transMs = 0;

			using (Debug.Profile($"TotalSave: {character.Name} ({character.DbId})", 1000))
			{
				for (var i = 0; i < maxRetries; i++)
				{
					try
					{
						var lockStart = saveStopwatch.ElapsedMilliseconds;
						CharacterLockManager.TryAcquire(character.DbId, TimeSpan.FromSeconds(10), "SaveCharacterData", ref lockTaken, out acquiredLock);
						lockMs = saveStopwatch.ElapsedMilliseconds - lockStart;
						if (!lockTaken)
						{
							Log.Error($"SaveCharacterData: Failed to acquire C# lock for character {character.DbId} after 10s. Aborting save.");
							return;
						}

						var transStart = saveStopwatch.ElapsedMilliseconds;
						using (var conn = this.GetConnection())
						using (var trans = conn.BeginTransaction())
						{
							try
							{
								// --- Main Character Table ---
								using (Debug.Profile($"SaveChar.CharacterTable"))
								{
									using (var cmd = new UpdateCommand("UPDATE `characters` SET {0} WHERE `characterId` = @characterId", conn, trans))
									{
										cmd.AddParameter("@characterId", character.DbId);
										cmd.Set("name", character.Name);
										cmd.Set("job", (short)character.JobId);
										cmd.Set("gender", (byte)character.Gender);
										cmd.Set("hair", character.Hair);
										cmd.Set("skinColor", character.SkinColor);
										cmd.Set("zone", character.MapId);
										cmd.Set("x", character.Position.X);
										cmd.Set("y", character.Position.Y);
										cmd.Set("z", character.Position.Z);
										cmd.Set("dir", character.Direction.DegreeAngle);
										cmd.Set("exp", character.Exp);
										cmd.Set("maxExp", character.MaxExp);
										cmd.Set("totalExp", character.TotalExp);
										cmd.Set("level", character.Level);
										cmd.Set("equipVisibility", (int)character.VisibleEquip);
										cmd.Set("equippedTitleId", character.EquippedTitleId);
										cmd.Set("stamina", character.Properties.Stamina);
										cmd.Set("partyId", character.PartyId);
										cmd.Set("guildId", character.GuildId);
										cmd.Execute();
									}
								}

								// --- Call Internal Save Methods for all components ---
								using (Debug.Profile($"SaveChar.ItemsAndStorage"))
								{
									this.InternalSaveCharacterItems(character, conn, trans);
									this.InternalSaveStorage(character.PersonalStorage, "storage_personal", "characterId", character.DbId, conn, trans);
								}
								using (Debug.Profile($"SaveChar.PropertiesAndVars"))
								{
									this.InternalSaveVariables(character.Variables.Perm, "vars_characters", "characterId", character.DbId, conn, trans);
									this.InternalSaveProperties("character_properties", "characterId", character.DbId, character.Properties, conn, trans);
									this.InternalSaveProperties("character_etc_properties", "characterId", character.DbId, character.Etc.Properties, conn, trans);
								}
								using (Debug.Profile($"SaveChar.JobsSkillsAbilities"))
								{
									this.InternalSaveJobs(character, conn, trans);
									this.InternalSaveSkills(character, conn, trans);
									this.InternalSaveAbilities(character, conn, trans);
								}
								using (Debug.Profile($"SaveChar.BuffsCooldowns"))
								{
									this.InternalSaveBuffs(character, conn, trans);
									this.InternalSaveCooldowns(character, conn, trans);
								}
								using (Debug.Profile($"SaveChar.SessionObjects"))
									this.InternalSaveSessionObjects(character, conn, trans);
								using (Debug.Profile($"SaveChar.Quests"))
									this.InternalSaveQuests(character, conn, trans);

								if (character.HasCompanions)
								{
									using (Debug.Profile($"SaveChar.Companions"))
										this.InternalSaveCompanions(character, conn, trans);
								}
								using (Debug.Profile($"SaveChar.AchievementsAndCollections"))
								{
									this.InternalSaveAchievements(character, conn, trans);
									this.InternalSaveAchievementPoints(character, conn, trans);
									this.InternalSaveCollections(character, conn, trans);
								}
								using (Debug.Profile($"SaveChar.AdventureBook"))
								{
									this.InternalSaveAdventureBook(character, conn, trans);
									this.InternalSaveAdventureBookMonsterDrop(character, conn, trans);
									this.InternalSaveAdventureBookItems(character, conn, trans);
								}

								trans.Commit();
								character.LastSaved = DateTime.UtcNow;
								transMs = saveStopwatch.ElapsedMilliseconds - transStart;

								saveStopwatch.Stop();
								if (saveStopwatch.ElapsedMilliseconds > SlowSaveThresholdMs)
									Log.Warning($"SaveCharacterData: Slow save for '{character.Name}' ({character.DbId}): {saveStopwatch.ElapsedMilliseconds}ms [lock={lockMs}ms, transaction={transMs}ms]. (Thread {Thread.CurrentThread.ManagedThreadId})");

								return; // Success, exit retry loop
							}
							catch (MySqlException ex) when (ex.Number == 1213 || ex.Number == 1205) // 1213 = Deadlock, 1205 = Lock wait timeout
							{
								var errorType = ex.Number == 1213 ? "Deadlock" : "Lock wait timeout";
								Log.Warning($"SaveCharacterData: {errorType} detected for character {character.DbId} on attempt {i + 1}/{maxRetries}. Retrying...");
								try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"Rollback after {errorType} failed: {rbEx}"); }
								if (i == maxRetries - 1) throw;
								Thread.Sleep(50 + RandomProvider.Get().Next(100));
							}
							catch (Exception ex)
							{
								Log.Error($"Database error during SaveCharacterData for {character.DbId}: {ex}");
								try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"Rollback failed: {rbEx}"); }
								throw;
							}
						}
					}
					finally
					{
						if (lockTaken)
						{
							CharacterLockManager.Release(acquiredLock, character.DbId, "SaveCharacterData");
							lockTaken = false; // Reset for retry loop
						}
					}
				}
			}
		}
	}
}
