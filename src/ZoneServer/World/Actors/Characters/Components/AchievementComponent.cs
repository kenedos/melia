using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Zone.Network;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Achievements
	/// </summary>
	public class AchievementComponent : CharacterComponent
	{
		private readonly Dictionary<int, bool> _achievements = new Dictionary<int, bool>();
		private readonly Dictionary<int, int> _achievementPoints = new Dictionary<int, int>();
		private readonly Dictionary<int, long> _achievementUnlockDates = new();

		private static readonly HashSet<int> PeriodAchievementIds = new HashSet<int>
		{
			6001, 6002, 6003, 7001, 7006
		};

		private const string AccountTitlePrefix = "Melia.Titles.Unlocked_";

		private AchievementStatRewardData _appliedTitleStatReward;

		public AchievementComponent(Character character) : base(character)
		{
		}

		public long GetAchievementUnlockDate(int achievementId)
		{
			lock (_achievements)
			{
				if (_achievementUnlockDates.TryGetValue(achievementId, out var unlockDate))
					return unlockDate;

				return 0;
			}
		}

		public void SetAchievementUnlockDate(int achievementId, long unlockDate)
		{
			lock (_achievements)
			{
				_achievementUnlockDates[achievementId] = unlockDate;
			}
		}

		public int[] GetAchievements()
		{
			lock (_achievements)
			{
				return _achievements.Keys.ToArray();
			}
		}

		public void UpdateAdventureBook()
		{
			int[] achievements;

			lock (_achievements)
			{
				achievements = _achievements
					.Where(x => x.Value)
					.Select(x => x.Key)
					.ToArray();
			}

			Send.ZC_ADVENTURE_BOOK_ACHIEVEMENTS(this.Character, achievements);
		}

		public int[] GetPointIds()
		{
			lock (_achievementPoints)
			{
				return _achievementPoints.Keys.ToArray();
			}
		}

		public int GetPoints(int pointId)
		{
			lock (_achievementPoints)
			{
				_achievementPoints.TryGetValue(pointId, out var points);
				return points;
			}
		}

		public bool HasAchievement(int achievementId)
		{
			lock (_achievements)
				if (_achievements.TryGetValue(achievementId, out var hasAchievement))
					return hasAchievement;
			return false;
		}

		/// <summary>
		/// Add achievement points by point class name (e.g., "MonKill", "PcKill", "Potion")
		/// </summary>
		/// <param name="pointClassName">The className from achievement_points.txt</param>
		/// <param name="points">Amount of points to add</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddAchievementPoints(string pointClassName, int points, bool silently = false)
		{
			if (!ZoneServer.Instance.Data.AchievementPointDb.TryFind(pointClassName, out var pointData))
			{
				Log.Warning("AddAchievementPoints: Achievement point not found with class name: {0}.", pointClassName);
				return;
			}

			this.AddAchievementPoints(pointData.Id, points, silently);
		}

		/// <summary>
		/// Add achievement points by point id
		/// </summary>
		/// <param name="achievementPointId">The id from achievement_points.txt</param>
		/// <param name="points">Amount of points to add</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddAchievementPoints(int achievementPointId, int points, bool silently = false)
		{
			lock (_achievementPoints)
			{
				if (_achievementPoints.ContainsKey(achievementPointId))
					_achievementPoints[achievementPointId] += points;
				else
					_achievementPoints.Add(achievementPointId, points);
			}

			if (!silently)
			{
				// Find the point data to get the class name for achievement checking
				if (ZoneServer.Instance.Data.AchievementPointDb.TryFind(achievementPointId, out var pointData))
				{
					Send.ZC_ACHIEVE_POINT(this.Character, pointData.Id, this.GetPoints(pointData.Id), 0);
					this.CheckAchievements(pointData);
				}
			}
		}

		/// <summary>
		/// Add monster kill achievement points (MonKill)
		/// </summary>
		/// <param name="points">Amount of points to add (default 1)</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddMonsterKillPoints(int points = 1, bool silently = false)
		{
			this.AddAchievementPoints("MonKill", points, silently);
		}

		/// <summary>
		/// Add monster kill achievement points (MonKill)
		/// </summary>
		/// <param name="points">Amount of points to add (default 1)</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddHanamingKillPoints(int points = 1, bool silently = false)
		{
			this.AddAchievementPoints("MonKill_hanaming", points, silently);
		}

		/// <summary>
		/// Add boss monster kill achievement points (MonBossKill)
		/// </summary>
		/// <param name="points">Amount of points to add (default 1)</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddBossMonsterKillPoints(int points = 1, bool silently = false)
		{
			this.AddAchievementPoints("MonBossKill", points, silently);
		}

		/// <summary>
		/// Add player kill achievement points (PcKill)
		/// </summary>
		/// <param name="points">Amount of points to add (default 1)</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddPlayerKillPoints(int points = 1, bool silently = false)
		{
			this.AddAchievementPoints("PcKill", points, silently);
		}

		/// <summary>
		/// Add player revive achievement points (PcRevive) - awarded to the resurrected player
		/// </summary>
		/// <param name="points">Amount of points to add (default 1)</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddRevivePoints(int points = 1, bool silently = false)
		{
			this.AddAchievementPoints("PcRevive", points, silently);
		}

		/// <summary>
		/// Add overkill achievement points (OverKill)
		/// </summary>
		/// <param name="points">Amount of points to add (default 1)</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddOverkillPoints(int points = 1, bool silently = false)
		{
			this.AddAchievementPoints("OverKill", points, silently);
		}

		/// <summary>
		/// Add potion use achievement points (Potion)
		/// </summary>
		/// <param name="points">Amount of points to add (default 1)</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddPotionUsePoints(int points = 1, bool silently = false)
		{
			this.AddAchievementPoints("Potion", points, silently);
		}

		/// <summary>
		/// Add quest completion achievement points (Quest)
		/// </summary>
		/// <param name="points">Amount of points to add (default 1)</param>
		/// <param name="silently">If true, doesn't send update packet or check achievements</param>
		public void AddQuestCompletionPoints(int points = 1, bool silently = false)
		{
			this.AddAchievementPoints("Quest", points, silently);
		}

		public void SetAchievementUnlockDateFromDb(int achievementId, long unlockDate)
		{
			lock (_achievements)
			{
				_achievementUnlockDates[achievementId] = unlockDate;
			}
		}

		/// <summary>
		/// Add an achievement
		/// </summary>
		/// <param name="achievementId"></param>
		/// <param name="silently"></param>
		public void AddAchievement(int achievementId, bool silently = false)
		{
			if (!ZoneServer.Instance.Data.AchievementDb.TryFind(achievementId, out var achievement))
			{
				Log.Warning("AddAchievement: Achievement with id: {0} not found.", achievementId);
				return;
			}

			if (!ZoneServer.Instance.Data.AchievementPointDb.TryFind(achievement.PointName, out var pointData))
			{
				Log.Warning("AddAchievement: Achievement with id: {0} not found.", achievementId);
				return;
			}

			lock (_achievements)
			{
				_achievements[achievementId] = true;

				if (!_achievementUnlockDates.ContainsKey(achievementId))
					_achievementUnlockDates[achievementId] = DateTime.UtcNow.ToFileTimeUtc();
			}

			if (!silently)
				this.RegisterTitleForAccount(achievement);

			if (!silently)
			{
				Send.ZC_ACHIEVE_POINT(this.Character, pointData.Id, this.GetPoints(pointData.Id), achievement.Id);
				this.UpdateAdventureBook();
				this.RecalculateTitleStatReward(true);
			}
		}

		/// <summary>
		/// Check if achievements are unlocked.
		/// </summary>
		/// <param name="pointData"></param>
		private void CheckAchievements(AchievementPointData pointData)
		{
			foreach (var possibleAchievements in ZoneServer.Instance.Data.AchievementDb.FindAll(a => a.PointName == pointData.ClassName))
			{
				if (this.HasAchievement(possibleAchievements.Id))
					continue;
				if (_achievementPoints[pointData.Id] >= possibleAchievements.PointCount)
				{
					this.AddAchievement(possibleAchievements.Id);
				}
			}
		}

		private bool IsTitleAchievement(AchievementData achievement)
		{
			if (achievement == null)
				return false;

			if (PeriodAchievementIds.Contains(achievement.Id))
				return false;

			if (achievement.Name == "None")
				return false;

			if (!ZoneServer.Instance.Data.AchievementPointDb.TryFind(achievement.PointName, out _))
				return false;

			return true;
		}

		private string GetAccountTitleFlag(int achievementId)
		{
			return AccountTitlePrefix + achievementId;
		}

		private void RegisterTitleForAccount(AchievementData achievement)
		{
			if (!this.IsTitleAchievement(achievement))
				return;

			var account = this.Character.Connection?.Account;
			if (account == null)
			{
				return;
			}

			var flag = this.GetAccountTitleFlag(achievement.Id);
			var alreadyUnlocked = account.Variables.Perm.GetBool(flag, false);

			if (!alreadyUnlocked)
			{
				account.Variables.Perm.SetBool(flag, true);
			}
		}

		public void SynchronizeAccountTitles()
		{
			var account = this.Character.Connection?.Account;

			if (account == null)
			{
				return;
			}

			var migrated = 0;

			// First migrate titles already owned by this character to the account.
			foreach (var achievementId in this.GetAchievements())
			{
				if (!ZoneServer.Instance.Data.AchievementDb.TryFind(achievementId, out var achievement))
					continue;

				if (!this.IsTitleAchievement(achievement))
					continue;

				if (!ZoneServer.Instance.Data.AchievementPointDb.TryFind(achievement.PointName, out var pointData))
					continue;

				var points = this.GetPoints(pointData.Id);

				if (points < achievement.PointCount)
					continue;

				this.RegisterTitleForAccount(achievement);
				migrated++;
			}

			// Then import all account titles into the current character.
			foreach (var achievement in ZoneServer.Instance.Data.AchievementDb.FindAll(a => this.IsTitleAchievement(a)))
			{
				if (!account.Variables.Perm.GetBool(this.GetAccountTitleFlag(achievement.Id), false))
					continue;

				if (!ZoneServer.Instance.Data.AchievementPointDb.TryFind(achievement.PointName, out var pointData))
					continue;

				var currentPoints = this.GetPoints(pointData.Id);

				if (currentPoints < achievement.PointCount)
				{
					lock (_achievementPoints)
						_achievementPoints[pointData.Id] = achievement.PointCount;
				}

				lock (_achievements)
					_achievements[achievement.Id] = true;
			}
		}

		public int GetEligibleTitleCount()
		{
			var count = 0;

			foreach (var achievementId in this.GetAchievements())
			{
				if (PeriodAchievementIds.Contains(achievementId))
					continue;

				if (!ZoneServer.Instance.Data.AchievementDb.TryFind(achievementId, out var achievement))
					continue;

				if (achievement.Name == "None")
					continue;

				if (!ZoneServer.Instance.Data.AchievementPointDb.TryFind(achievement.PointName, out var pointData))
					continue;

				if (this.GetPoints(pointData.Id) < achievement.PointCount)
					continue;

				count++;
			}

			return count;
		}

		public void RecalculateTitleStatReward(bool sendUpdate = false)
		{
			var titleCount = this.GetEligibleTitleCount();
			AchievementStatRewardData reward = null;

			foreach (var candidate in ZoneServer.Instance.Data.AchievementStatRewardDb.FindAll(x => x.AchieveCount <= titleCount))
			{
				if (reward == null || candidate.AchieveCount > reward.AchieveCount)
					reward = candidate;
			}

			if (reward == null)
				return;

			var oldStr = _appliedTitleStatReward?.Str ?? 0;
			var oldCon = _appliedTitleStatReward?.Con ?? 0;
			var oldInt = _appliedTitleStatReward?.Int ?? 0;
			var oldSpr = _appliedTitleStatReward?.Spr ?? 0;
			var oldDex = _appliedTitleStatReward?.Dex ?? 0;
			var oldPatk = _appliedTitleStatReward?.Patk ?? 0;
			var oldMatk = _appliedTitleStatReward?.Matk ?? 0;
			var oldDef = _appliedTitleStatReward?.Def ?? 0;
			var oldMdef = _appliedTitleStatReward?.Mdef ?? 0;
			var oldMsp = _appliedTitleStatReward?.Msp ?? 0;

			this.Character.Properties.Modify(PropertyName.STR_BM, reward.Str - oldStr);
			this.Character.Properties.Modify(PropertyName.CON_BM, reward.Con - oldCon);
			this.Character.Properties.Modify(PropertyName.INT_BM, reward.Int - oldInt);
			this.Character.Properties.Modify(PropertyName.MNA_BM, reward.Spr - oldSpr);
			this.Character.Properties.Modify(PropertyName.DEX_BM, reward.Dex - oldDex);
			this.Character.Properties.Modify(PropertyName.PATK_BM, reward.Patk - oldPatk);
			this.Character.Properties.Modify(PropertyName.MATK_BM, reward.Matk - oldMatk);
			this.Character.Properties.Modify(PropertyName.DEF_BM, reward.Def - oldDef);
			this.Character.Properties.Modify(PropertyName.MDEF_BM, reward.Mdef - oldMdef);
			this.Character.Properties.Modify(PropertyName.MSP_BM, reward.Msp - oldMsp);

			_appliedTitleStatReward = reward;

			this.Character.Properties.InvalidateAll();

			if (sendUpdate)
				Send.ZC_OBJECT_PROPERTY(this.Character);
		}
	}
}
