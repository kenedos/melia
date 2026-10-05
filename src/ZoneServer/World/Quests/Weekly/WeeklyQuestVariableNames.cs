using System;

namespace Melia.Zone.World.Quests.Weekly
{
	public static class WeeklyQuestVariableNames
	{
		private const string Prefix = "ProjectBaixada.WeeklyQuest.";

		public const string Active = Prefix + "Active";
		public const string LastResetUnixTime = Prefix + "LastResetUnixTime";
		public const string TrackerLayoutVersion = Prefix + "TrackerLayoutVersion";

		public static string GetTarget(string key) => Prefix + NormalizeKey(key) + ".Target";
		public static string GetCurrent(string key) => Prefix + NormalizeKey(key) + ".Current";
		public static string GetReward(string key) => Prefix + NormalizeKey(key) + ".Reward";
		public static string GetCompleted(string key) => Prefix + NormalizeKey(key) + ".Completed";
		public static string GetRewardClaimed(string key) => Prefix + NormalizeKey(key) + ".RewardClaimed";
		public static string GetTargetMap(string key) => Prefix + NormalizeKey(key) + ".TargetMap";
		public static string GetTargetRace(string key) => Prefix + NormalizeKey(key) + ".TargetRace";
		public static string GetTargetAttribute(string key) => Prefix + NormalizeKey(key) + ".TargetAttribute";
		public static string GetTargetSize(string key) => Prefix + NormalizeKey(key) + ".TargetSize";

		private static string NormalizeKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentException("Weekly quest key cannot be empty.", nameof(key));

			return key.Trim();
		}
	}
}
