using System.Collections.Generic;

namespace Melia.Zone.World.Quests.Weekly
{
	public class WeeklyQuestProgress
	{
		public List<WeeklyQuestDefinition> Quests { get; } = new();
		public bool Active { get; set; }
		public long LastResetUnixTime { get; set; }

		public WeeklyQuestDefinition GetQuest(string key)
		{
			return this.Quests.Find(quest => quest.Key == key);
		}

		public bool IsRewardClaimed(string key)
		{
			var quest = this.GetQuest(key);
			return quest != null && quest.RewardClaimed;
		}
	}
}
