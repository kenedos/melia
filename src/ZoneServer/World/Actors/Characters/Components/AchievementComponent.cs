using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
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

		public AchievementComponent(Character character) : base(character)
		{
		}

		public int[] GetAchievements()
		{
			lock (_achievements)
			{
				return _achievements.Keys.ToArray();
			}
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
				_achievements[achievementId] = true;

			if (!silently)
				Send.ZC_ACHIEVE_POINT(this.Character, pointData.Id, _achievementPoints[pointData.Id], achievement.Id);
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
	}
}
