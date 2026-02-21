using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Items.Effects;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
// using Melia.Zone.World.Houses; // Removed: Houses namespace deleted
using Melia.Zone.World.Quests;
using MySqlConnector;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Contains methods for loading various Character components from the database.
	/// </summary>
	public partial class ZoneDb
	{
		/// <summary>
		/// Orchestrator for loading all character-related component data.
		/// </summary>
		private void LoadCharacterComponentData(Character character, string charNameForLog)
		{
			using (Debug.Profile($"Load.InventoryItems: {charNameForLog}", 200))
				this.LoadCharacterItems(character);

			using (Debug.Profile($"Load.MiscData: {charNameForLog}", 100))
			{
				this.LoadVars(character.Variables.Perm, "vars_characters", "characterId", character.DbId);
				this.LoadSessionObjects(character);
				this.LoadJobs(character);
				this.LoadSkills(character);
				this.LoadAbilities(character);
				this.LoadBuffs(character);
				this.LoadCooldowns(character);
			}

			using (Debug.Profile($"Load.Quests: {charNameForLog}", 100))
				this.LoadQuests(character);

			using (Debug.Profile($"Load.Properties: {charNameForLog}", 100))
			{
				this.LoadProperties("character_properties", "characterId", character.DbId, character.Properties);
				this.LoadProperties("character_etc_properties", "characterId", character.DbId, character.Etc.Properties);
				CardPropertyModifier.Instance.RestoreAllModifiers(character);
			}

			using (Debug.Profile($"Load.GroupsAndSocial: {charNameForLog}", 100))
			{
				this.LoadCompanions(character);
				this.LoadParty(character);
				// this.LoadGuild(character); // Removed: Guild type deleted
				this.LoadHelp(character);
			}

			using (Debug.Profile($"Load.AchievementsAndBooks: {charNameForLog}", 100))
			{
				this.LoadAchievements(character);
				this.LoadAchievementPoints(character);
				this.LoadAdventureBook(character);
				this.LoadAdventureBookMonsterDrop(character);
				this.LoadAdventureBookItems(character);
				this.LoadCollections(character);
			}

			using (Debug.Profile($"Load.PersonalStorage: {charNameForLog}", 100))
			{
				character.PersonalStorage.InitSize();
				this.LoadStorage(character.PersonalStorage, "storage_personal", "characterId", character.DbId);
			}
		}

		private void LoadAchievements(Character character)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `achievements` WHERE `characterId` = @characterId", conn))
			{
				cmd.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var achievementId = reader.GetInt32("achievementId");
						if (!character.Achievements.HasAchievement(achievementId))
							character.Achievements.AddAchievement(achievementId, true);
					}
				}
			}
		}

		private void LoadAchievementPoints(Character character)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `achievement_points` WHERE `characterId` = @characterId", conn))
			{
				cmd.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var pointId = reader.GetInt32("pointId");
						var points = reader.GetInt32("pointValue");
						character.Achievements.AddAchievementPoints(pointId, points, true);
					}
				}
			}
		}

		private void LoadHelp(Character character)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `help` WHERE `characterId` = @characterId", conn))
			{
				cmd.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var helpId = reader.GetInt32("helpId");
						var shown = reader.GetByte("shown");
						character.Tutorials.Add(helpId, shown == 1);
					}
				}
			}
			var defaultHelp = ZoneServer.Instance.Data.HelpDb.Entries.Values.Where(a => a.BasicHelp && a.DbSave);
			if (character.Tutorials.Count == 0 || defaultHelp.Count() != character.Tutorials.Count)
			{
				foreach (var help in defaultHelp)
				{
					character.Tutorials.Add(help.Id, false);
				}
			}
		}

		private void LoadSkills(Character character)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `skills` WHERE `characterId` = @characterId", conn))
			{
				mc.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var id = reader.GetInt32("id").ToString();
						if (!Enum.TryParse<SkillId>(id, out var skillId))
						{
							Log.Warning("ZoneDb.LoadSkills: Skill Id '{0}' not found, removing it from skills.", id);
							continue;
						}
						if (!ZoneServer.Instance.Data.SkillDb.Contains(skillId))
						{
							Log.Warning("ZoneDb.LoadSkills: Skill data '{0}' not found, removing it from skills.", id);
							continue;
						}
						var level = reader.GetInt32("level");
						var skill = new Skill(character, skillId, level);
						character.Skills.AddSilent(skill);
					}
				}
			}
		}

		private void LoadAbilities(Character character)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `abilities` WHERE `characterId` = @characterId", conn))
			{
				mc.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var abilityId = (AbilityId)reader.GetInt32("id");
						var level = reader.GetInt32("level");
						var active = reader.GetBoolean("active");
						var ability = new Ability(abilityId, level);
						ability.Active = active;
						character.Abilities.AddSilent(ability);
					}
				}
			}
		}

		private void LoadJobs(Character character)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `jobs` WHERE `characterId` = @characterId", conn))
			{
				mc.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var jobId = (JobId)reader.GetInt32("jobId");
						var circle = (JobCircle)reader.GetInt32("circle");
						var skillPoints = reader.GetInt32("skillPoints");
						var totalExp = reader.GetInt64("totalExp");
						var selectionDate = reader.GetDateTimeSafe("selectionDate");
						if (selectionDate == DateTime.MinValue)
							selectionDate = new DateTime(2015, 1, 1);

						// Load advancement date, backfill with selectionDate if NULL
						var advDate = reader.GetDateTimeSafe("advDate");
						if (advDate == DateTime.MinValue)
							advDate = selectionDate;

						// Prevent broken characters: ensure circle is at least First
						if (circle == JobCircle.None)
						{
							Log.Warning("LoadJobs: Character {0} has job {1} with circle=0, correcting to 1.", character.DbId, jobId);
							circle = JobCircle.First;
						}

						var job = new Job(character, jobId, totalExp, circle, skillPoints);
						job.SelectionDate = selectionDate;
						job.AdvancementDate = advDate;
						character.Jobs.AddSilent(job);
					}
				}
			}
			if (character.Jobs.Count == 0 || character.Jobs.Get(character.JobId) == null)
				character.Jobs.AddSilent(new Job(character, character.JobId));
		}

		private void LoadSessionObjects(Character character)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `session_objects_properties` WHERE `characterId` = @characterId", conn))
			{
				mc.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var sessionObjectId = reader.GetInt32("sessionObjectId");
						var propertyName = reader.GetString("name");
						var typeStr = reader.GetString("type");
						var valueStr = reader.GetString("value");
						if (Regex.IsMatch(propertyName, @"^[0-9]+$"))
							propertyName = PropertyTable.GetName("SessionObject", int.Parse(propertyName));
						var sessionObject = character.SessionObjects.GetOrCreate(sessionObjectId);
						var properties = sessionObject.Properties;
						if (typeStr == "f")
						{
							if (!properties.TryGet<FloatProperty>(propertyName, out var property))
								property = properties.Create(new FloatProperty(propertyName));
							property.Deserialize(valueStr);
						}
						else
						{
							if (!properties.TryGet<StringProperty>(propertyName, out var property))
								property = properties.Create(new StringProperty(propertyName));
							property.Deserialize(valueStr);
						}
					}
				}
			}
		}

		private void LoadBuffs(Character character)
		{
			var buffs = new Dictionary<long, Buff>();
			using (var conn = this.GetConnection())
			{
				using (var cmd = new MySqlCommand("SELECT * FROM `buffs` WHERE `characterId` = @characterId", conn))
				{
					cmd.Parameters.AddWithValue("@characterId", character.DbId);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var dbId = reader.GetInt64("buffId");
							var classId = (BuffId)reader.GetInt32("classId");
							var numArg1 = reader.GetInt32("numArg1");
							var numArg2 = reader.GetInt32("numArg2");
							var numArg3 = reader.GetInt32("numArg3");
							var numArg4 = reader.GetInt32("numArg4");
							var numArg5 = reader.GetInt32("numArg5");
							var duration = reader.GetTimeSpan("duration");
							var runTime = reader.GetTimeSpan("runTime");
							var skillId = (SkillId)reader.GetInt32("skillId");
							var overbuffCount = reader.GetInt32("overbuffCount");
							var buff = new Buff(classId, numArg1, numArg2, duration, runTime, character, character, skillId);
							buff.NumArg3 = numArg3;
							buff.NumArg4 = numArg4;
							buff.NumArg5 = numArg5;
							buff.OverbuffCounter = overbuffCount;
							buffs.Add(dbId, buff);
						}
					}
				}
				foreach (var buff in buffs)
				{
					this.LoadVars(conn, buff.Value.Vars, "vars_buffs", "buffId", buff.Key);
					character.Buffs.Restore(buff.Value);
				}
			}
		}

		private void LoadCooldowns(Character character)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `cooldowns` WHERE `characterId` = @characterId", conn))
			{
				cmd.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var classId = (CooldownId)reader.GetInt32("classId");
						var remaining = reader.GetTimeSpan("remaining");
						var duration = reader.GetTimeSpan("duration");
						var startTime = reader.GetDateTimeSafe("startTime");
						var endTime = startTime + duration;
						var updatedRemaining = Math2.Max(TimeSpan.Zero, endTime - DateTime.Now);
						if (updatedRemaining == TimeSpan.Zero)
							continue;
						var cooldown = new Cooldown(classId, updatedRemaining, duration, startTime);
						character.Components.Get<CooldownComponent>().Restore(cooldown);
					}
				}
			}
		}

		private void LoadQuests(Character character)
		{
			using (var conn = this.GetConnection())
			{
				var loadedQuests = new Dictionary<long, Quest>();
				using (var cmd = new MySqlCommand("SELECT * FROM `quests` WHERE `characterId` = @characterId", conn))
				{
					cmd.AddParameter("@characterId", character.DbId);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var questDbId = reader.GetInt64("questId");
							var questClassId = reader.GetInt64("classId");
							var status = (QuestStatus)reader.GetInt32("status");
							var startTime = reader.GetDateTimeSafe("startTime");
							var completeTime = reader.GetDateTimeSafe("completeTime");
							var tracked = reader.GetBoolean("tracked");
							var questId = questClassId switch
							{
								1000001 => new QuestId("Melia.Test", 1),
								1000002 => new QuestId("Melia.Test", 2),
								_ => new QuestId(questClassId)
							};
							if (!QuestScript.Exists(questId))
							{
								character.Quests.AddDisabledQuest(questDbId);
								continue;
							}
							var quest = Quest.Create(questId);
							quest.Status = status;
							quest.StartTime = startTime;
							quest.CompleteTime = completeTime;
							quest.Tracked = tracked;
							character.Quests.AddSilent(quest);
							loadedQuests.Add(questDbId, quest);
						}
					}
				}
				using (var cmd = new MySqlCommand("SELECT * FROM `quests_progress` WHERE `characterId` = @characterId", conn))
				{
					cmd.AddParameter("@characterId", character.DbId);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var questDbId = reader.GetInt64("questId");
							var ident = reader.GetStringSafe("ident");
							var count = reader.GetInt32("count");
							var done = reader.GetBoolean("done");
							var unlocked = reader.GetBoolean("unlocked");
							if (character.Quests.IsDisabled(questDbId))
								continue;
							if (!loadedQuests.TryGetValue(questDbId, out var quest))
							{
								Log.Warning("ZoneDb.LoadQuests: Progress '{0}' loaded for a quest that doesn't exist.", ident);
								continue;
							}
							if (!quest.TryGetProgress(ident, out var progress))
							{
								Log.Warning("ZoneDb.LoadQuests: Progress '{0}' loaded for quest '{1}', but the objective doesn't exist.", ident, quest.Data.Id);
								continue;
							}
							progress.Count = count;
							progress.Done = done;
							progress.Unlocked = unlocked;
						}
					}
				}
			}
		}

		private void LoadCompanions(Character character)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `companions` WHERE `characterId` = @characterId", conn))
			{
				mc.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var companion = new Companion(character, reader.GetInt32("monsterId"), RelationType.Friendly);
						companion.DbId = reader.GetInt64("companionId");
						companion.Name = reader.GetString("name");
						companion.Properties.SetFloat(PropertyName.HP, reader.GetInt32("hp"));
						companion.Properties.SetFloat(PropertyName.Stamina, reader.GetInt32("stamina"));
						companion.Exp = reader.GetInt64("exp");
						companion.MaxExp = reader.GetInt64("maxExp");
						companion.TotalExp = reader.GetInt64("totalExp");
						companion.Level = reader.GetInt32("level");
						companion.IsActivated = reader.GetByte("active") == 1;
						companion.Slot = reader.GetByte("slot");
						companion.IsAggressiveMode = reader.GetByte("isAggressiveMode") == 1;
						companion.Position = Position.Zero;
						companion.Direction = new Direction(0);
						companion.Properties.SetFloat(PropertyName.Stat_MHP, reader.GetInt32("stat_mhp"));
						companion.Properties.SetFloat(PropertyName.Stat_ATK, reader.GetInt32("stat_atk"));
						companion.Properties.SetFloat(PropertyName.Stat_DEF, reader.GetInt32("stat_def"));
						companion.Properties.SetFloat(PropertyName.Stat_MDEF, reader.GetInt32("stat_mdef"));
						companion.Properties.SetFloat(PropertyName.Stat_HR, reader.GetInt32("stat_hr"));
						companion.Properties.SetFloat(PropertyName.Stat_CRTHR, reader.GetInt32("stat_crthr"));
						companion.Properties.SetFloat(PropertyName.Stat_DR, reader.GetInt32("stat_dr"));
						companion.Properties.InvalidateAll();
						companion.InitAutoUpdates();
						character.Companions.AddCompanion(companion, true);
					}
				}
			}
		}

		private void LoadAdventureBook(Character character)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `adventure_book` WHERE `accountId` = @accountId", conn))
			{
				cmd.Parameters.AddWithValue("@accountId", character.AccountDbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var adventureBookType = (AdventureBookType)reader.GetByte("type");
						var classId = reader.GetInt32("classId");
						var count = reader.GetInt32("count");
						if (adventureBookType == AdventureBookType.MonsterKilled)
						{
							character.AdventureBook.AddMonsterKill(classId, count, true);
						}
					}
				}
			}
		}

		private void LoadAdventureBookMonsterDrop(Character character)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `adventure_book_monster_drops` WHERE `accountId` = @accountId", conn))
			{
				cmd.Parameters.AddWithValue("@accountId", character.AccountDbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var monsterId = reader.GetInt32("monsterId");
						var itemId = reader.GetInt32("itemId");
						var count = reader.GetInt32("count");
						character.AdventureBook.AddMonsterDrop(monsterId, itemId, count, true);
					}
				}
			}
		}

		private void LoadAdventureBookItems(Character character)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `adventure_book_items` WHERE `accountId` = @accountId", conn))
			{
				cmd.Parameters.AddWithValue("@accountId", character.AccountDbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var itemId = reader.GetInt32("itemId");
						var obtainCount = reader.GetInt32("obtainCount");
						var craftCount = reader.GetInt32("craftCount");
						var useCount = reader.GetInt32("useCount");
						character.AdventureBook.AddItem(itemId, craftCount, obtainCount, useCount);
					}
				}
			}
		}

		private void LoadCollections(Character character)
		{
			using (var conn = this.GetConnection())
			{
				using (var cmd = new MySqlCommand("SELECT `collectionid`, `timesRedeemed` FROM `collections` WHERE `accountid` = @accountId ", conn))
				{
					cmd.AddParameter("@accountId", character.AccountDbId);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var collectionId = reader.GetInt32("collectionId");
							var redeemCount = reader.GetInt32("timesRedeemed");
							character.Collections.InitAdd(collectionId, redeemCount);
						}
					}
				}

				using (var cmd = new MySqlCommand("SELECT `collectionid`, `itemid` FROM `collection_items` WHERE `accountid` = @accountId ", conn))
				{
					cmd.AddParameter("@accountId", character.AccountDbId);
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var collectionId = reader.GetInt32("collectionId");
							var itemId = reader.GetInt32("itemId");
							character.Collections.InitRegisterItem(collectionId, itemId);
						}
					}
				}
				character.Properties.InvalidateAll();
			}
		}

		/// <summary>
		/// Save Help (Tutorials) to database.
		/// </summary>
		public void SaveHelp(long characterId, int helpId, bool isShown)
		{
			if (characterId == 0)
				return;

			using (var conn = this.GetConnection())
			using (var cmd = new InsertCommand("INSERT INTO `help` {0} ON DUPLICATE KEY UPDATE `shown` = VALUES(`shown`)", conn))
			{
				cmd.Set("characterId", characterId);
				cmd.Set("helpId", helpId);
				cmd.Set("shown", isShown ? 1 : 0);

				cmd.Execute();
			}
		}

		/// <summary>
		/// Inserts companion in database.
		/// </summary>
		public void CreateCompanion(long accountId, long characterId, Companion companion)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				using (var cmd = new InsertCommand("INSERT INTO `companions` {0}", conn, trans))
				{
					companion.AdoptTime = DateTime.Now;

					cmd.Set("accountId", accountId);
					cmd.Set("characterId", characterId);
					cmd.Set("name", companion.Name);
					cmd.Set("monsterId", companion.Id);
					cmd.Set("hp", (int)companion.Properties.GetFloat(PropertyName.HP));
					cmd.Set("stamina", (int)companion.Properties.GetFloat(PropertyName.Stamina));
					cmd.Set("exp", companion.Exp);
					cmd.Set("maxExp", companion.MaxExp);
					cmd.Set("totalExp", companion.TotalExp);
					cmd.Set("level", companion.Level);
					cmd.Set("adoptTime", companion.AdoptTime);
					cmd.Set("active", companion.IsActivated ? 1 : 0);
					cmd.Set("slot", companion.Slot);
					cmd.Set("isAggressiveMode", companion.IsAggressiveMode ? 1 : 0);
					cmd.Set("barrackLayer", 1);
					cmd.Set("bx", 0f);
					cmd.Set("by", 0f);
					cmd.Set("bz", 0f);
					cmd.Set("dx", 0f);
					cmd.Set("dy", 0f);
					cmd.Set("stat_mhp", (int)companion.Properties.GetFloat(PropertyName.Stat_MHP));
					cmd.Set("stat_atk", (int)companion.Properties.GetFloat(PropertyName.Stat_ATK));
					cmd.Set("stat_def", (int)companion.Properties.GetFloat(PropertyName.Stat_DEF));
					cmd.Set("stat_mdef", (int)companion.Properties.GetFloat(PropertyName.Stat_MDEF));
					cmd.Set("stat_hr", (int)companion.Properties.GetFloat(PropertyName.Stat_HR));
					cmd.Set("stat_crthr", (int)companion.Properties.GetFloat(PropertyName.Stat_CRTHR));
					cmd.Set("stat_dr", (int)companion.Properties.GetFloat(PropertyName.Stat_DR));

					cmd.Execute();
					companion.DbId = cmd.LastId;
				}

				trans.Commit();
			}
		}

		/// <summary>
		/// Returns given companion, or null if it doesn't exist.
		/// </summary>
		public Companion GetCompanion(Character character)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand("SELECT * FROM `companions` WHERE `characterId` = @characterId", conn))
			{
				mc.Parameters.AddWithValue("@characterId", character.DbId);

				using (var reader = mc.ExecuteReader())
				{
					if (!reader.Read())
						return null;

					var companion = new Companion(character, reader.GetInt32("monsterId"), RelationType.Friendly)
					{
						DbId = reader.GetInt64("companionId"),
						Name = reader.GetString("name"),
						Exp = reader.GetInt64("exp"),
						IsActivated = reader.GetBoolean("active"),
						Slot = reader.GetByte("slot"),
						IsAggressiveMode = reader.GetBoolean("isAggressiveMode"),
						Position = Position.Zero,
						Direction = new Direction(0)
					};

					companion.Properties.SetFloat(PropertyName.HP, reader.GetInt32("hp"));
					companion.Properties.SetFloat(PropertyName.Stamina, reader.GetInt32("stamina"));
					companion.Properties.SetFloat(PropertyName.Stat_MHP, reader.GetInt32("stat_mhp"));
					companion.Properties.SetFloat(PropertyName.Stat_ATK, reader.GetInt32("stat_atk"));
					companion.Properties.SetFloat(PropertyName.Stat_DEF, reader.GetInt32("stat_def"));
					companion.Properties.SetFloat(PropertyName.Stat_MDEF, reader.GetInt32("stat_mdef"));
					companion.Properties.SetFloat(PropertyName.Stat_HR, reader.GetInt32("stat_hr"));
					companion.Properties.SetFloat(PropertyName.Stat_CRTHR, reader.GetInt32("stat_crthr"));
					companion.Properties.SetFloat(PropertyName.Stat_DR, reader.GetInt32("stat_dr"));
					companion.Properties.InvalidateAll();
					companion.InitAutoUpdates();

					return companion;
				}
			}
		}

		// Removed: LoadAllHouses and SaveHouse referenced PersonalHouse/Prop
		// from deleted Houses namespace. See below for commented-out methods.

		/*
		/// <summary>
		/// Loads all personal houses from the database.
		/// </summary>
		public Dictionary<long, PersonalHouse> LoadAllHouses()
		{
			var houses = new Dictionary<long, PersonalHouse>();
			using (var conn = this.GetConnection())
			{
				// Step 1: Load all house parent objects
				using (var cmd = new MySqlCommand("SELECT * FROM `houses`", conn))
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						try
						{
							var house = new PersonalHouse
							{
								Id = reader.GetInt64("houseId"),
								Name = reader.GetString("name"),
								OwnerId = reader.GetInt64("ownerId"),
								OwnerName = reader.GetString("ownerName"),
								MapId = reader.GetInt32("mapId"),
								LastEnterTime = reader.GetDateTimeSafe("lastEnterTime"),
							};
							houses.Add(house.Id, house);
						}
						catch (Exception ex)
						{
							Log.Error($"Failed to load a house: {ex.Message}");
						}
					}
				}

				// Step 2: Load all props and assign them to their houses
				using (var cmd = new MySqlCommand("SELECT * FROM `house_props`", conn))
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var houseId = reader.GetInt64("houseId");
						if (houses.TryGetValue(houseId, out var house))
						{
							try
							{
								var prop = new Prop(reader.GetInt32("furnitureId"))
								{
									MonsterId = reader.GetInt32("monsterId"),
									Position = new Position(reader.GetFloat("x"), reader.GetFloat("y"), reader.GetFloat("z")),
									Direction = Direction.FromCardinalDirection((CardinalDirection)reader.GetByte("dir")),
								};

								var usedCellsStr = reader.GetStringSafe("usedCells");
								if (!string.IsNullOrEmpty(usedCellsStr))
								{
									prop.UsedCells = usedCellsStr.Split(',').Select(s => int.TryParse(s, out var i) ? i : -1).Where(i => i != -1).ToArray();
								}

								house.Props.Add(prop);
							}
							catch (Exception ex)
							{
								Log.Error($"Failed to load prop for house {houseId}: {ex.Message}");
							}
						}
					}
				}

				// Step 3: Initialize all loaded houses
				foreach (var house in houses.Values)
				{
					try
					{
						house.Init();
					}
					catch (Exception ex)
					{
						Log.Error($"Failed to initialize house {house.Id}: {ex.Message}");
					}
				}
			}
			return houses;
		}

		/// <summary>
		/// Persists the character's personal house to the database.
		/// </summary>
		public void SaveHouse(PersonalHouse house)
		{
			if (house == null)
				return;

			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					// Delete children first to avoid foreign key violations
					using (var cmd = new MySqlCommand("DELETE FROM `house_props` WHERE `houseId` = @houseId", conn, trans))
					{
						cmd.Parameters.AddWithValue("@houseId", house.Id);
						cmd.ExecuteNonQuery();
					}

					// Then delete parent
					using (var cmd = new MySqlCommand("DELETE FROM `houses` WHERE `houseId` = @houseId", conn, trans))
					{
						cmd.Parameters.AddWithValue("@houseId", house.Id);
						cmd.ExecuteNonQuery();
					}

					// Re-insert parent
					using (var cmd = new InsertCommand("INSERT INTO `houses` {0}", conn, trans))
					{
						cmd.Set("houseId", house.Id);
						cmd.Set("name", house.Name);
						cmd.Set("ownerId", house.OwnerId);
						cmd.Set("ownerName", house.OwnerName);
						cmd.Set("mapId", house.MapId);
						cmd.Set("lastEnterTime", house.LastEnterTime);
						cmd.Execute();
					}

					// Re-insert children
					foreach (var prop in house.Props)
					{
						using (var cmd = new InsertCommand("INSERT INTO `house_props` {0}", conn, trans))
						{
							cmd.Set("houseId", house.Id);
							cmd.Set("monsterId", prop.MonsterId);
							cmd.Set("furnitureId", prop.FurnitureId);
							cmd.Set("x", prop.Position.X);
							cmd.Set("y", prop.Position.Y);
							cmd.Set("z", prop.Position.Z);
							cmd.Set("dir", prop.Direction.ToCardinalDirection());
							cmd.Execute();
						}
					}

					trans.Commit();
				}
				catch (Exception ex)
				{
					Log.Error($"Failed to save house {house.Id}: {ex}");
					try { trans.Rollback(); } catch (Exception rbEx) { Log.Error($"Rollback failed for SaveHouse: {rbEx}"); }
					throw;
				}
			}
		}
		*/
	}
}
