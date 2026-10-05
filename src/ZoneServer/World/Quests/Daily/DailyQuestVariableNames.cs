namespace Melia.Zone.World.Quests.Daily
{
    /// <summary>
    /// Nomes das variáveis usadas para persistir o sistema de Daily Quests.
    /// </summary>
    public static class DailyQuestVariableNames
    {
        private const string Prefix = "ProjectBaixada.DailyQuest.";

        public const string Difficulty = Prefix + "Difficulty";
        public const string HasChosenDifficulty = Prefix + "HasChosenDifficulty";
        public const string LastResetUnixTime = Prefix + "LastResetUnixTime";
		public const string CompletionBonusClaimed = Prefix + "CompletionBonusClaimed";

		public static string GetProgress(DailyQuestType type)
        {
            return Prefix + type + ".Progress";
        }

        public static string GetCompleted(DailyQuestType type)
        {
            return Prefix + type + ".Completed";
        }

        public static string GetRewardClaimed(DailyQuestType type)
        {
            return Prefix + type + ".RewardClaimed";
        }

		public static string GetTarget(DailyQuestType type)
		{
			return Prefix + type + ".Target";
		}

		public static string GetReward(DailyQuestType type)
		{
			return Prefix + type + ".Reward";
		}

		public static string GetCurrent(DailyQuestType type)
		{
			return Prefix + type + ".Current";
		}

		public static string GetTargetMap(DailyQuestType type)
		{
			return Prefix + type + ".TargetMap";
		}

		public static string GetTargetRace(DailyQuestType type)
		{
			return Prefix + type + ".TargetRace";
		}

		public static string GetTargetAttribute(DailyQuestType type)
		{
			return Prefix + type + ".TargetAttribute";
		}

		public static string GetTargetSize(DailyQuestType type)
		{
			return Prefix + type + ".TargetSize";
		}
	}
}
