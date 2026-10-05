using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;

namespace Melia.Zone.World.Quests.Weekly
{
	public class WeeklyQuestManager
	{
		private readonly WeeklyQuestGenerator generator = new();
		private const int WeeklyResetDay = (int)DayOfWeek.Monday;
		private const int WeeklyResetHourUtc = 6;
		private const int CurrentTrackerLayoutVersion = 2;

		public bool IsActive(Character character) => character.Variables.Perm.GetBool(WeeklyQuestVariableNames.Active, false);

		public async Task<bool> Activate(Character character)
		{
			if (character == null || this.IsActive(character))
				return false;

			var progress = this.Create(character);
			this.Save(character, progress);
			this.SaveVariables(character);
			await this.StartQuest(character);
			return true;
		}

		private WeeklyQuestProgress Create(Character character)
		{
			var progress = new WeeklyQuestProgress
			{
				Active = true,
				LastResetUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
			};

			progress.Quests.AddRange(this.generator.Generate());
			return progress;
		}

		public WeeklyQuestProgress Load(Character character)
		{
			var progress = new WeeklyQuestProgress
			{
				Active = this.IsActive(character),
				LastResetUnixTime = character.Variables.Perm.GetLong(WeeklyQuestVariableNames.LastResetUnixTime, 0),
			};

			foreach (var key in WeeklyQuestKeys.GetAll())
				progress.Quests.Add(this.LoadQuest(character, key));

			return progress;
		}

		public WeeklyQuestDefinition GetQuest(Character character, string key)
		{
			if (character == null || !this.IsActive(character))
				return null;

			return this.LoadQuest(character, key);
		}

		private WeeklyQuestDefinition LoadQuest(Character character, string key)
		{
			return new WeeklyQuestDefinition
			{
				Key = key,
				Type = WeeklyQuestKeys.GetQuestType(key),
				Target = character.Variables.Perm.GetInt(WeeklyQuestVariableNames.GetTarget(key), 0),
				Progress = character.Variables.Perm.GetInt(WeeklyQuestVariableNames.GetCurrent(key), 0),
				Reward = character.Variables.Perm.GetInt(WeeklyQuestVariableNames.GetReward(key), 0),
				TargetMapClassName = character.Variables.Perm.GetString(WeeklyQuestVariableNames.GetTargetMap(key), string.Empty),
				TargetRace = character.Variables.Perm.GetInt(WeeklyQuestVariableNames.GetTargetRace(key), 0),
				TargetAttribute = character.Variables.Perm.GetInt(WeeklyQuestVariableNames.GetTargetAttribute(key), 0),
				TargetSize = character.Variables.Perm.GetInt(WeeklyQuestVariableNames.GetTargetSize(key), 0),
				Completed = character.Variables.Perm.GetBool(WeeklyQuestVariableNames.GetCompleted(key), false),
				RewardClaimed = character.Variables.Perm.GetBool(WeeklyQuestVariableNames.GetRewardClaimed(key), false),
			};
		}

		public bool IncreaseProgress(Character character, string key, int amount = 1)
		{
			if (character == null || !this.IsActive(character) || amount <= 0)
				return false;

			var quest = this.LoadQuest(character, key);
			if (quest.Target <= 0 || quest.Completed)
				return false;

			quest.Progress = Math.Min(quest.Progress + amount, quest.Target);
			quest.Completed = quest.Progress >= quest.Target;
			character.Variables.Perm.SetInt(WeeklyQuestVariableNames.GetCurrent(key), quest.Progress);
			character.Variables.Perm.SetBool(WeeklyQuestVariableNames.GetCompleted(key), quest.Completed);

			if (quest.Completed)
				this.SaveVariables(character);

			return true;
		}

		public bool ClaimReward(Character character, string key)
		{
			var quest = this.GetQuest(character, key);
			if (quest == null || !quest.Completed || quest.RewardClaimed || quest.Reward <= 0)
				return false;

			character.AddItem(WeeklyQuestRewards.RewardItemId, quest.Reward);
			character.Variables.Perm.SetBool(WeeklyQuestVariableNames.GetRewardClaimed(key), true);
			this.SaveVariables(character);
			return true;
		}

		public void Save(Character character, WeeklyQuestProgress progress)
		{
			var vars = character.Variables.Perm;
			vars.SetBool(WeeklyQuestVariableNames.Active, progress.Active);
			vars.SetLong(WeeklyQuestVariableNames.LastResetUnixTime, progress.LastResetUnixTime);

			foreach (var quest in progress.Quests)
			{
				quest.Reward = WeeklyQuestRewards.RewardPerQuest;
				vars.SetInt(WeeklyQuestVariableNames.GetTarget(quest.Key), quest.Target);
				vars.SetInt(WeeklyQuestVariableNames.GetCurrent(quest.Key), quest.Progress);
				vars.SetInt(WeeklyQuestVariableNames.GetReward(quest.Key), quest.Reward);
				vars.SetString(WeeklyQuestVariableNames.GetTargetMap(quest.Key), quest.TargetMapClassName);
				vars.SetInt(WeeklyQuestVariableNames.GetTargetRace(quest.Key), quest.TargetRace);
				vars.SetInt(WeeklyQuestVariableNames.GetTargetAttribute(quest.Key), quest.TargetAttribute);
				vars.SetInt(WeeklyQuestVariableNames.GetTargetSize(quest.Key), quest.TargetSize);
				vars.SetBool(WeeklyQuestVariableNames.GetCompleted(quest.Key), quest.Completed);
				vars.SetBool(WeeklyQuestVariableNames.GetRewardClaimed(quest.Key), quest.RewardClaimed);
			}
		}

		private static DateTime GetWeeklyResetPeriodStartUtc(DateTime utcNow)
		{
			var daysSinceMonday = ((int)utcNow.DayOfWeek - WeeklyResetDay + 7) % 7;
			var monday = utcNow.Date.AddDays(-daysSinceMonday).AddHours(WeeklyResetHourUtc);
			return utcNow >= monday ? monday : monday.AddDays(-7);
		}

		public bool IsWeeklyResetRequired(Character character)
		{
			if (character == null || !this.IsActive(character))
				return false;

			var lastReset = character.Variables.Perm.GetLong(WeeklyQuestVariableNames.LastResetUnixTime, 0);
			if (lastReset <= 0)
				return true;

			return DateTimeOffset.FromUnixTimeSeconds(lastReset).UtcDateTime < GetWeeklyResetPeriodStartUtc(DateTime.UtcNow);
		}

		public async Task<bool> ResetIfNewWeek(Character character)
		{
			if (!this.IsWeeklyResetRequired(character))
				return false;

			var progress = this.Create(character);
			this.Save(character, progress);
			this.SaveVariables(character);
			await this.RestartQuest(character);
			return true;
		}

		public async Task StartQuest(Character character)
		{
			if (character == null || !this.IsActive(character))
				return;

			var vars = character.Variables.Perm;
			var replaceOldLayout = vars.GetInt(WeeklyQuestVariableNames.TrackerLayoutVersion, 0) < CurrentTrackerLayoutVersion;
			foreach (var tracker in CreateTrackers())
			{
				var data = tracker.CreateQuestData(character);
				if (data == null)
					continue;

				if (character.Quests.TryGetById(tracker.TrackerId, out var existingQuest))
				{
					if (replaceOldLayout)
						character.Quests.ReplaceGeneratedQuest(data, tracker);
				}
				else
				{
					await character.Quests.StartGeneratedQuest(data, tracker);
				}
			}

			vars.SetInt(WeeklyQuestVariableNames.TrackerLayoutVersion, CurrentTrackerLayoutVersion);
			this.SaveVariables(character);
			this.RestoreQuestProgress(character);
			character.Quests.UpdateClient();
		}

		public Task RestartQuest(Character character)
		{
			if (!this.IsActive(character))
				return Task.CompletedTask;

			foreach (var tracker in CreateTrackers())
			{
				var data = tracker.CreateQuestData(character);
				if (data != null)
					character.Quests.ReplaceGeneratedQuest(data, tracker);
			}

			character.Variables.Perm.SetInt(WeeklyQuestVariableNames.TrackerLayoutVersion, CurrentTrackerLayoutVersion);
			this.SaveVariables(character);
			this.RestoreQuestProgress(character);
			character.Quests.UpdateClient();

			return Task.CompletedTask;
		}

		public void RestoreQuest(Character character)
		{
			if (character == null || !this.IsActive(character))
				return;

			if (character.Variables.Perm.GetInt(WeeklyQuestVariableNames.TrackerLayoutVersion, 0) < CurrentTrackerLayoutVersion)
			{
				this.RestartQuest(character);
				return;
			}

			foreach (var tracker in CreateTrackers())
			{
				if (character.Quests.TryGetById(tracker.TrackerId, out _))
					continue;

				var data = tracker.CreateQuestData(character);
				if (data == null)
					continue;

				character.Quests.RestoreGeneratedQuest(data, tracker);
			}

			this.RestoreQuestProgress(character);
			character.Quests.UpdateClient();
		}

		private void RestoreQuestProgress(Character character)
		{
			foreach (var tracker in CreateTrackers())
			{
				if (!character.Quests.TryGetById(tracker.TrackerId, out var quest))
					continue;

				foreach (var objectiveProgress in quest.Progresses)
				{
					string key = null;
					if (objectiveProgress.Objective is WeeklyKillObjective killObjective)
						key = killObjective.Key;
					else if (objectiveProgress.Objective is WeeklyDungeonObjective)
						key = WeeklyQuestKeys.Dungeon;

					if (key == null)
						continue;

					var saved = this.GetQuest(character, key);
					if (saved == null)
						continue;

					objectiveProgress.Count = Math.Min(saved.Progress, objectiveProgress.Objective.TargetCount);
					if (objectiveProgress.Count >= objectiveProgress.Objective.TargetCount)
						objectiveProgress.SetDone();
					else
						objectiveProgress.Done = false;
				}

				quest.UpdateUnlock();
				quest.Status = quest.ObjectivesCompleted ? QuestStatus.Success : QuestStatus.InProgress;
			}
		}

		private static WeeklyQuestTrackerBase[] CreateTrackers()
		{
			return new WeeklyQuestTrackerBase[]
			{
				new WeeklyQuestGeneralTracker(),
				new WeeklyQuestRaceTracker(),
				new WeeklyQuestAttributeTracker(),
				new WeeklyQuestSizeTracker(),
				new WeeklyQuestRegionTracker(),
			};
		}

		private void SaveVariables(Character character)
		{
			ZoneServer.Instance.Database.SaveVariables(character.Variables.Perm, "vars_characters", "characterId", character.DbId);
		}
	}
}
