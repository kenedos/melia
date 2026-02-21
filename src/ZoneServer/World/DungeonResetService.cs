using System;
using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Logging;

namespace Melia.Zone.World
{
	/// <summary>
	/// Manages the reset of dungeon entry counters at daily (00:00) and
	/// weekly (Monday 00:00) intervals. Handles both online players via
	/// scheduled tasks and offline players via login checks.
	/// </summary>
	public class DungeonResetService
	{
		private readonly HashSet<int> _dailyResetTypes = new();
		private readonly HashSet<int> _weeklyResetTypes = new();

		/// <summary>
		/// Initializes the dungeon reset service and subscribes to events.
		/// </summary>
		public void Initialize()
		{
			// Cache the reset types to avoid iterating the database repeatedly
			this.CacheResetTypes();

			// Subscribe to HourTick for scheduled resets at midnight
			ZoneServer.Instance.ServerEvents.HourTick.Subscribe(this.OnHourTick);

			// Subscribe to PlayerLoggedIn for login-based resets
			ZoneServer.Instance.ServerEvents.PlayerLoggedIn.Subscribe(this.OnPlayerLoggedIn);

			Log.Info("DungeonResetService: Initialized with {0} daily and {1} weekly reset types.",
				_dailyResetTypes.Count, _weeklyResetTypes.Count);
		}

		/// <summary>
		/// Caches the playPerResetType values for daily and weekly dungeons.
		/// </summary>
		private void CacheResetTypes()
		{
			foreach (var dungeon in ZoneServer.Instance.Data.InstanceDungeonDb.Entries.Values)
			{
				if (dungeon.WeeklyEnterableCount != 0)
				{
					_weeklyResetTypes.Add(dungeon.PlayPerResetType);
				}
				else
				{
					_dailyResetTypes.Add(dungeon.PlayPerResetType);
				}
			}
		}

		/// <summary>
		/// Called every hour to check if it's reset time for scheduled resets.
		/// </summary>
		private void OnHourTick(object sender, TimeEventArgs e)
		{
			var conf = ZoneServer.Instance.Conf.World;
			var resetHour = conf.InstancedDungeonResetHour;
			var resetMinute = conf.InstancedDungeonResetMinute;
			var weeklyResetDay = conf.InstancedDungeonWeeklyResetDay;

			// Check if current time matches reset time
			if (e.Now.Hour != resetHour)
				return;

			// Skip if reset minute is configured for later in the hour
			// (login-based resets will handle the exact minute for offline players)
			if (resetMinute > 0 && e.Now.Minute < resetMinute)
				return;

			var isWeeklyResetDay = e.Now.DayOfWeek == weeklyResetDay;

			Log.Info("DungeonResetService: Daily reset triggered at {0:00}:{1:00}. Weekly reset: {2}", resetHour, resetMinute, isWeeklyResetDay);

			// Reset all online characters
			foreach (var character in ZoneServer.Instance.World.GetCharacters())
			{
				if (character?.Connection == null)
					continue;

				this.ResetDailyCounters(character);
				this.SetLastResetTime(character, PropertyName.Indun_ResetTime, DateTime.Now);
				this.SetLastResetTimeEtc(character, PropertyName.Indun_ResetTime, DateTime.Now);

				if (isWeeklyResetDay)
				{
					this.ResetWeeklyCounters(character);
					this.SetLastResetTime(character, PropertyName.IndunWeeklyResetTime, DateTime.Now);
					this.SetLastResetTimeEtc(character, PropertyName.IndunWeeklyResetTime, DateTime.Now);
				}
			}
		}

		/// <summary>
		/// Called when a player logs in to check if resets are needed.
		/// </summary>
		private void OnPlayerLoggedIn(object sender, PlayerEventArgs e)
		{
			this.CheckAndResetForCharacter(e.Character);
		}

		/// <summary>
		/// Checks if daily/weekly resets are needed for a character and performs them.
		/// </summary>
		/// <param name="character">The character to check and reset.</param>
		public void CheckAndResetForCharacter(Character character)
		{
			if (character?.Connection == null)
				return;

			var now = DateTime.Now;

			// Check daily reset for Account-based properties
			var lastDailyResetAccount = this.GetLastResetTime(character, PropertyName.Indun_ResetTime);
			var needsDailyReset = this.NeedsDailyReset(lastDailyResetAccount);

			// Check daily reset for PCEtc-based properties (per-character)
			var lastDailyResetEtc = this.GetLastResetTimeEtc(character, PropertyName.Indun_ResetTime);
			var needsDailyResetEtc = this.NeedsDailyReset(lastDailyResetEtc);

			if (needsDailyReset || needsDailyResetEtc)
			{
				this.ResetDailyCounters(character);

				if (needsDailyReset)
					this.SetLastResetTime(character, PropertyName.Indun_ResetTime, now);

				// Always update PCEtc timestamp for this character
				this.SetLastResetTimeEtc(character, PropertyName.Indun_ResetTime, now);
			}

			// Check weekly reset for Account-based properties
			var lastWeeklyResetAccount = this.GetLastResetTime(character, PropertyName.IndunWeeklyResetTime);
			var needsWeeklyReset = this.NeedsWeeklyReset(lastWeeklyResetAccount);

			// Check weekly reset for PCEtc-based properties (per-character)
			var lastWeeklyResetEtc = this.GetLastResetTimeEtc(character, PropertyName.IndunWeeklyResetTime);
			var needsWeeklyResetEtc = this.NeedsWeeklyReset(lastWeeklyResetEtc);

			if (needsWeeklyReset || needsWeeklyResetEtc)
			{
				this.ResetWeeklyCounters(character);

				if (needsWeeklyReset)
					this.SetLastResetTime(character, PropertyName.IndunWeeklyResetTime, now);

				// Always update PCEtc timestamp for this character
				this.SetLastResetTimeEtc(character, PropertyName.IndunWeeklyResetTime, now);
			}
		}

		/// <summary>
		/// Checks if a daily reset is needed based on the last reset time.
		/// </summary>
		private bool NeedsDailyReset(DateTime lastReset)
		{
			var conf = ZoneServer.Instance.Conf.World;
			var resetHour = conf.InstancedDungeonResetHour;
			var resetMinute = conf.InstancedDungeonResetMinute;

			// Get today's reset time
			var now = DateTime.Now;
			var todayResetTime = now.Date.AddHours(resetHour).AddMinutes(resetMinute);

			// If we haven't reached today's reset time yet, use yesterday's reset time
			if (now < todayResetTime)
				todayResetTime = todayResetTime.AddDays(-1);

			return lastReset < todayResetTime;
		}

		/// <summary>
		/// Checks if a weekly reset is needed based on the last reset time.
		/// Weekly resets occur on the configured day at the configured time.
		/// </summary>
		private bool NeedsWeeklyReset(DateTime lastReset)
		{
			var conf = ZoneServer.Instance.Conf.World;
			var resetHour = conf.InstancedDungeonResetHour;
			var resetMinute = conf.InstancedDungeonResetMinute;
			var weeklyResetDay = conf.InstancedDungeonWeeklyResetDay;

			// Get the most recent weekly reset time
			var now = DateTime.Now;
			var daysSinceResetDay = ((int)now.DayOfWeek - (int)weeklyResetDay + 7) % 7;
			var lastResetDay = now.Date.AddDays(-daysSinceResetDay).AddHours(resetHour).AddMinutes(resetMinute);

			// If we haven't reached this week's reset time yet, use last week's
			if (now < lastResetDay)
				lastResetDay = lastResetDay.AddDays(-7);

			return lastReset < lastResetDay;
		}

		/// <summary>
		/// Resets daily dungeon entry counters for a character (public API for GM commands).
		/// </summary>
		/// <param name="character">The character to reset.</param>
		public void ResetDailyForCharacter(Character character)
		{
			if (character?.Connection == null)
				return;

			this.ResetDailyCounters(character);
			this.SetLastResetTime(character, PropertyName.Indun_ResetTime, DateTime.Now);
			this.SetLastResetTimeEtc(character, PropertyName.Indun_ResetTime, DateTime.Now);
		}

		/// <summary>
		/// Resets weekly dungeon entry counters for a character (public API for GM commands).
		/// </summary>
		/// <param name="character">The character to reset.</param>
		public void ResetWeeklyForCharacter(Character character)
		{
			if (character?.Connection == null)
				return;

			this.ResetWeeklyCounters(character);
			this.SetLastResetTime(character, PropertyName.IndunWeeklyResetTime, DateTime.Now);
			this.SetLastResetTimeEtc(character, PropertyName.IndunWeeklyResetTime, DateTime.Now);
		}

		/// <summary>
		/// Resets daily dungeon entry counters for a character.
		/// </summary>
		private void ResetDailyCounters(Character character)
		{
			foreach (var resetType in _dailyResetTypes)
			{
				var propertyName = "InDunCountType_" + resetType;
				this.ResetAccountProperty(character, propertyName);
				this.ResetEtcProperty(character, propertyName);
			}
		}

		/// <summary>
		/// Resets weekly dungeon entry counters for a character.
		/// </summary>
		private void ResetWeeklyCounters(Character character)
		{
			foreach (var resetType in _weeklyResetTypes)
			{
				var propertyName = "IndunWeeklyEnteredCount_" + resetType;
				this.ResetAccountProperty(character, propertyName);
			}

			// Also reset specific CheckCountName properties for weekly dungeons
			foreach (var dungeon in ZoneServer.Instance.Data.InstanceDungeonDb.Entries.Values)
			{
				if (dungeon.WeeklyEnterableCount == 0)
					continue;

				if (!string.IsNullOrEmpty(dungeon.CheckCountName) && dungeon.CheckCountName != "None")
				{
					this.ResetAccountProperty(character, dungeon.CheckCountName);
				}
			}
		}

		/// <summary>
		/// Resets an account property to 0.
		/// </summary>
		private void ResetAccountProperty(Character character, string propertyName)
		{
			var accountProps = character.Connection.Account.Properties;
			accountProps.SetFloat(propertyName, 0);
			Send.ZC_OBJECT_PROPERTY(character.Connection, character.Connection.Account, propertyName);
		}

		/// <summary>
		/// Resets a character ETC property to 0.
		/// </summary>
		private void ResetEtcProperty(Character character, string propertyName)
		{
			var etcProps = character.Etc.Properties;
			etcProps.SetFloat(propertyName, 0);
			Send.ZC_OBJECT_PROPERTY(character, propertyName);
		}

		/// <summary>
		/// Gets the last reset time from account properties.
		/// </summary>
		private DateTime GetLastResetTime(Character character, string propertyName)
		{
			var accountProps = character.Connection.Account.Properties;
			if (accountProps.TryGetString(propertyName, out var dateStr))
			{
				if (dateStr.TryGetPropertyStringToDateTime(out var dt))
					return dt;
			}
			return DateTime.MinValue; // Force reset if no timestamp
		}

		/// <summary>
		/// Gets the last reset time from character PCEtc properties.
		/// </summary>
		private DateTime GetLastResetTimeEtc(Character character, string propertyName)
		{
			var etcProps = character.Etc.Properties;
			if (etcProps.TryGetString(propertyName, out var dateStr))
			{
				if (dateStr.TryGetPropertyStringToDateTime(out var dt))
					return dt;
			}
			return DateTime.MinValue; // Force reset if no timestamp
		}

		/// <summary>
		/// Sets the last reset time in account properties.
		/// </summary>
		private void SetLastResetTime(Character character, string propertyName, DateTime time)
		{
			var accountProps = character.Connection.Account.Properties;
			accountProps.SetString(propertyName, time.ToPropertyDateTimeString());
			Send.ZC_OBJECT_PROPERTY(character.Connection, character.Connection.Account, propertyName);
		}

		/// <summary>
		/// Sets the last reset time in character PCEtc properties.
		/// </summary>
		private void SetLastResetTimeEtc(Character character, string propertyName, DateTime time)
		{
			var etcProps = character.Etc.Properties;
			etcProps.SetString(propertyName, time.ToPropertyDateTimeString());
			Send.ZC_OBJECT_PROPERTY(character, propertyName);
		}
	}
}
