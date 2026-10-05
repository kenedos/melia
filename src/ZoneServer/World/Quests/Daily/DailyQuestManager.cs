using System;
using System.Threading.Tasks;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Shared.Game.Const;

namespace Melia.Zone.World.Quests.Daily
{
	public class DailyQuestManager
	{
		private static readonly QuestId TrackerQuestId = DailyQuestTrackerQuest.TrackerQuestId;
		private readonly DailyQuestGenerator generator = new();
		private const int DailyResetHourUtc = 8;

		public bool HasChosenDifficulty(Character character)
		{
			return character.Variables.Perm.GetBool(
				DailyQuestVariableNames.HasChosenDifficulty,
				false);
		}

		public DailyQuestDifficulty GetDifficulty(Character character)
		{
			return (DailyQuestDifficulty)character.Variables.Perm.GetInt(
				DailyQuestVariableNames.Difficulty,
				(int)DailyQuestDifficulty.Easy);
		}

		public async Task ChooseDifficulty(
			Character character,
			DailyQuestDifficulty difficulty)
		{
			if (this.HasChosenDifficulty(character))
				return;

			var progress = this.Create(
				character,
				difficulty);

			progress.HasChosenDifficulty = true;
			progress.LastResetUnixTime =
				DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			this.Save(character, progress);
			this.SaveVariables(character);

			await this.RestartQuest(character);
		}

		public DailyQuestProgress Create(Character character, DailyQuestDifficulty difficulty)
		{
			var progress = new DailyQuestProgress
			{
				Difficulty = difficulty,
				HasChosenDifficulty = true,
			};

			progress.Quests.AddRange(this.generator.Generate(difficulty));

			this.Save(character, progress);

			return progress;
		}

		public DailyQuestProgress Load(Character character)
		{
			var progress = new DailyQuestProgress
			{
				Difficulty =
					(DailyQuestDifficulty)character.Variables.Perm.GetInt(
						DailyQuestVariableNames.Difficulty,
						(int)DailyQuestDifficulty.Easy),

				HasChosenDifficulty =
					character.Variables.Perm.GetBool(
						DailyQuestVariableNames.HasChosenDifficulty,
						false),

				LastResetUnixTime =
					character.Variables.Perm.GetLong(
						DailyQuestVariableNames.LastResetUnixTime,
						0)
			};

			foreach (DailyQuestType type in Enum.GetValues<DailyQuestType>())
			{
				progress.Quests.Add(
					this.LoadQuest(
						character,
						progress.Difficulty,
						type));
			}

			return progress;
		}

		private DailyQuestDefinition LoadQuest(
			Character character,
			DailyQuestDifficulty difficulty,
			DailyQuestType type)
		{
			return new DailyQuestDefinition
			{
				Type = type,
				Difficulty = difficulty,

				Target = character.Variables.Perm.GetInt(
					DailyQuestVariableNames.GetTarget(type),
					0),

				Progress = character.Variables.Perm.GetInt(
					DailyQuestVariableNames.GetCurrent(type),
					0),

				Reward = character.Variables.Perm.GetInt(
					DailyQuestVariableNames.GetReward(type),
					0),

				TargetMapClassName = character.Variables.Perm.GetString(
					DailyQuestVariableNames.GetTargetMap(type),
					string.Empty),

				TargetRace = character.Variables.Perm.GetInt(
					DailyQuestVariableNames.GetTargetRace(type),
					0),

				TargetAttribute = character.Variables.Perm.GetInt(
					DailyQuestVariableNames.GetTargetAttribute(type),
					0),

				TargetSize = character.Variables.Perm.GetInt(
					DailyQuestVariableNames.GetTargetSize(type),
					0),

				Completed = character.Variables.Perm.GetBool(
					DailyQuestVariableNames.GetCompleted(type),
					false),

				RewardClaimed = character.Variables.Perm.GetBool(
					DailyQuestVariableNames.GetRewardClaimed(type),
					false),
			};
		}

		public DailyQuestDefinition GetQuest(Character character, DailyQuestType type)
		{
			if (!this.HasChosenDifficulty(character))
				return null;

			var difficulty = this.GetDifficulty(character);

			return this.LoadQuest(character, difficulty, type);
		}

		public bool ClaimReward(Character character, DailyQuestType type)
		{
			var quest = this.GetQuest(character, type);

			if (quest == null)
				return false;

			if (!quest.Completed)
				return false;

			if (quest.RewardClaimed)
				return false;

			if (quest.Reward <= 0)
				return false;

			character.AddItem(DailyQuestRewards.RewardItemId, quest.Reward);
			character.Variables.Perm.SetBool(DailyQuestVariableNames.GetRewardClaimed(type), true);
			this.SaveVariables(character);

			return true;
		}

		public async Task<bool> Reset(Character character)
		{
			if (!this.HasChosenDifficulty(character))
				return false;

			var difficulty = this.GetDifficulty(character);

			var progress = this.Create(character, difficulty);

			progress.HasChosenDifficulty = true;
			progress.LastResetUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			this.Save(character, progress);

			await this.RestartQuest(character);

			return true;
		}

		public bool IncreaseProgress(Character character, DailyQuestType type, int amount = 1)
		{
			if (!this.HasChosenDifficulty(character))
				return false;

			if (amount <= 0)
				return false;

			var difficulty = this.GetDifficulty(character);

			var quest = this.LoadQuest(character, difficulty, type);

			if (quest.Target <= 0)
				return false;

			if (quest.Completed)
				return false;

			quest.Progress = Math.Min(quest.Progress + amount, quest.Target);
			quest.Completed = quest.Progress >= quest.Target;

			character.Variables.Perm.SetInt(DailyQuestVariableNames.GetCurrent(type), quest.Progress);
			character.Variables.Perm.SetBool(DailyQuestVariableNames.GetCompleted(type), quest.Completed);

			if (quest.Completed)
				this.SaveVariables(character);

			return true;
		}

		public void Save(Character character, DailyQuestProgress progress)
		{
			var vars = character.Variables.Perm;

			vars.SetInt(
				DailyQuestVariableNames.Difficulty,
				(int)progress.Difficulty);

			vars.SetBool(
				DailyQuestVariableNames.HasChosenDifficulty,
				progress.HasChosenDifficulty);

			vars.SetLong(
				DailyQuestVariableNames.LastResetUnixTime,
				progress.LastResetUnixTime);

			foreach (var quest in progress.Quests)
			{
				vars.SetInt(
					DailyQuestVariableNames.GetTarget(quest.Type),
					quest.Target);

				vars.SetInt(
					DailyQuestVariableNames.GetCurrent(quest.Type),
					quest.Progress);

				vars.SetString(
					DailyQuestVariableNames.GetTargetMap(quest.Type),
					quest.TargetMapClassName);

				vars.SetInt(
					DailyQuestVariableNames.GetTargetRace(quest.Type),
					quest.TargetRace);

				vars.SetInt(
					DailyQuestVariableNames.GetTargetAttribute(quest.Type),
					quest.TargetAttribute);

				vars.SetInt(
					DailyQuestVariableNames.GetTargetSize(quest.Type),
					quest.TargetSize);

				vars.SetInt(
					DailyQuestVariableNames.GetReward(quest.Type),
					quest.Reward);

				vars.SetBool(
					DailyQuestVariableNames.GetCompleted(quest.Type),
					quest.Completed);

				vars.SetBool(
					DailyQuestVariableNames.GetRewardClaimed(quest.Type),
					quest.RewardClaimed);
			}
		}

		private static DateTime GetDailyResetPeriodStartUtc(DateTime utcNow)
		{
			var resetToday = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, DailyResetHourUtc, 0, 0, DateTimeKind.Utc);

			if (utcNow >= resetToday)
				return resetToday;

			return resetToday.AddDays(-1);
		}

		public bool IsDailyResetRequired(Character character)
		{
			if (!this.HasChosenDifficulty(
				character))
			{
				return false;
			}

			var lastResetUnixTime = character.Variables.Perm.GetLong(DailyQuestVariableNames.LastResetUnixTime, 0);

			if (lastResetUnixTime <= 0)
				return true;

			var lastResetUtc = DateTimeOffset.FromUnixTimeSeconds(lastResetUnixTime).UtcDateTime;

			var currentUtc = DateTime.UtcNow;

			var currentPeriodStartUtc = GetDailyResetPeriodStartUtc(currentUtc);

			return lastResetUtc < currentPeriodStartUtc;
		}

		public async Task<bool> ResetIfNewDay(Character character)
		{
			if (!this.IsDailyResetRequired(
				character))
			{
				return false;
			}

			return await this.ResetDifficultySelection(
				character,
				true);
		}

		public bool CanManuallyReset(Character character)
		{
			if (!this.HasChosenDifficulty(character))
				return false;

			if (this.HasClaimedAnyReward(character))
				return false;

			return true;
		}

		public bool HasClaimedAnyReward(Character character)
		{
			if (!this.HasChosenDifficulty(character))
				return false;

			foreach (DailyQuestType type in Enum.GetValues<DailyQuestType>())
			{
				if (character.Variables.Perm.GetBool(DailyQuestVariableNames.GetRewardClaimed(type), false))
					return true;
			}

			return false;
		}

		public bool HasClaimedAllQuestRewards(Character character)
		{
			if (!this.HasChosenDifficulty(character))
				return false;

			foreach (DailyQuestType type in Enum.GetValues<DailyQuestType>())
			{
				if (!character.Variables.Perm.GetBool(DailyQuestVariableNames.GetRewardClaimed(type), false))
					return false;
			}

			return true;
		}

		public bool HasClaimedCompletionBonus(Character character)
		{
			return character.Variables.Perm.GetBool(DailyQuestVariableNames.CompletionBonusClaimed, false);
		}

		public void MarkCompletionBonusClaimed(Character character)
		{
			character.Variables.Perm.SetBool(DailyQuestVariableNames.CompletionBonusClaimed, true);
			this.SaveVariables(character);
		}

		public async Task<bool> ResetDifficultySelection(Character character, bool force = false)
		{
			if (!force &&
				!this.CanManuallyReset(
					character))
			{
				return false;
			}

			var vars = character.Variables.Perm;

			await this.CancelQuest(character);

			vars.SetInt(DailyQuestVariableNames.Difficulty, (int)DailyQuestDifficulty.Easy);
			vars.SetBool(DailyQuestVariableNames.HasChosenDifficulty, false);
			vars.SetLong(DailyQuestVariableNames.LastResetUnixTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
			vars.SetBool(DailyQuestVariableNames.CompletionBonusClaimed, false);

			foreach (DailyQuestType type in Enum.GetValues<DailyQuestType>())
			{
				vars.SetInt(DailyQuestVariableNames.GetTarget(type), 0);
				vars.SetInt(DailyQuestVariableNames.GetCurrent(type), 0);
				vars.SetString(DailyQuestVariableNames.GetTargetMap(type), string.Empty);
				vars.SetInt(DailyQuestVariableNames.GetTargetRace(type), 0);
				vars.SetInt(DailyQuestVariableNames.GetTargetAttribute(type), 0);
				vars.SetInt(DailyQuestVariableNames.GetTargetSize(type), 0);
				vars.SetInt(DailyQuestVariableNames.GetReward(type), 0);
				vars.SetBool(DailyQuestVariableNames.GetCompleted(type), false);
				vars.SetBool(DailyQuestVariableNames.GetRewardClaimed(type), false);
			}

			this.SaveVariables(character);

			return true;
		}

		public bool IsQuestRunning(Character character)
		{
			return character.Quests.IsActive(
				TrackerQuestId);
		}

		public async Task StartQuest(Character character)
		{
			if (!this.HasChosenDifficulty(character))
				return;

			if (this.IsQuestRunning(character))
				return;

			var tracker =
				new DailyQuestTrackerQuest();

			var questData =
				tracker.CreateQuestData(character);

			if (questData == null)
				return;

			await character.Quests.StartGeneratedQuest(
				questData,
				tracker);
		}

		public Task RestartQuest(Character character)
		{
			if (!this.HasChosenDifficulty(
				character))
			{
				return Task.CompletedTask;
			}

			var tracker =
				new DailyQuestTrackerQuest();

			var questData =
				tracker.CreateQuestData(
					character);

			if (questData == null)
				return Task.CompletedTask;

			character.Quests.ReplaceGeneratedQuest(
				questData,
				tracker);

			return Task.CompletedTask;
		}

		public Task CancelQuest(Character character)
		{
			if (!character.Quests.TryGetById(
				TrackerQuestId,
				out var existingQuest))
			{
				return Task.CompletedTask;
			}

			if (existingQuest.InProgress)
				character.Quests.Cancel(existingQuest);

			return Task.CompletedTask;
		}

		public void RestoreQuest(Character character)
		{
			if (!this.HasChosenDifficulty(character))
				return;

			Quest quest;

			if (!character.Quests.TryGetById(
				TrackerQuestId,
				out quest))
			{
				var tracker = new DailyQuestTrackerQuest();

				var questData =
					tracker.CreateQuestData(character);

				if (questData == null)
					return;

				quest = character.Quests.RestoreGeneratedQuest(
					questData,
					tracker);
			}

			foreach (var progress in quest.Progresses)
			{
				DailyQuestType? type =
					progress.Objective.Ident switch
					{
						"dailyMonsters" => DailyQuestType.Monster,
						"dailyElites" => DailyQuestType.Elite,
						"dailyBosses" => DailyQuestType.Boss,
						"dailyMythics" => DailyQuestType.Mythic,
						"dailyDungeons" => DailyQuestType.Dungeon,
						"dailyMap" => DailyQuestType.Map,
						"dailyRace" => DailyQuestType.Race,
						"dailyAttribute" => DailyQuestType.Attribute,
						"dailySize" => DailyQuestType.Size,
						_ => null,
					};

				if (type == null)
					continue;

				var savedQuest = this.GetQuest(
					character,
					type.Value);

				if (savedQuest == null)
					continue;

				progress.Count = Math.Min(
					savedQuest.Progress,
					progress.Objective.TargetCount);

				if (progress.Count >= progress.Objective.TargetCount)
					progress.SetDone();
				else
					progress.Done = false;
			}

			quest.UpdateUnlock();

			quest.Status = quest.ObjectivesCompleted
				? QuestStatus.Success
				: QuestStatus.InProgress;

			character.Quests.UpdateClient();
		}

		private void SaveVariables(Character character)
		{
			ZoneServer.Instance.Database.SaveVariables(character.Variables.Perm, "vars_characters", "characterId", character.DbId);
		}
	}
}
