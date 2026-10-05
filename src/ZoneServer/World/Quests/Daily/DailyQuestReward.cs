using Melia.Zone.World.Quests.Daily;

public static class DailyQuestRewards
{
	public const int RewardItemId = 647016;

	public static int GetReward(DailyQuestDifficulty difficulty)
	{
		return difficulty switch
		{
			DailyQuestDifficulty.Easy => 1,
			DailyQuestDifficulty.Medium => 2,
			DailyQuestDifficulty.Hard => 4,
			_ => 0,
		};
	}

	public static int GetCompletionBonus(DailyQuestDifficulty difficulty)
	{
		return difficulty switch
		{
			DailyQuestDifficulty.Easy => 3,
			DailyQuestDifficulty.Medium => 6,
			DailyQuestDifficulty.Hard => 12,
			_ => 0,
		};
	}
}
