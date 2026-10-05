namespace Melia.Zone.World.Quests.Daily
{
	public class DailyQuestDefinition
	{
		public DailyQuestType Type { get; set; }

		public DailyQuestDifficulty Difficulty { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public int Target { get; set; }

		public int Progress { get; set; }

		public int Reward { get; set; }

		public bool Completed { get; set; }

		public bool RewardClaimed { get; set; }

		public string TargetMapClassName { get; set; } = string.Empty;

		public int TargetRace { get; set; }

		public int TargetAttribute { get; set; }

		public int TargetSize { get; set; }
	}
}
