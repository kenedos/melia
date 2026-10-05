using System.Collections.Generic;

namespace Melia.Zone.World.Quests.Daily
{
	/// <summary>
	/// Representa o progresso das Daily Quests de uma conta.
	/// Nesta primeira versão é apenas um container de dados.
	/// A integração com Account Variables será feita na próxima etapa.
	/// </summary>
	public class DailyQuestProgress
	{
		public List<DailyQuestDefinition> Quests { get; } = new();

		public DailyQuestDifficulty Difficulty { get; set; }

		public bool HasChosenDifficulty { get; set; }

		public long LastResetUnixTime { get; set; }

		public bool IsRewardClaimed(DailyQuestType type)
		{
			var quest = this.Quests.Find(q => q.Type == type);

			return quest != null && quest.RewardClaimed;
		}

		public DailyQuestDefinition GetQuest(DailyQuestType type)
		{
			return this.Quests.Find(q => q.Type == type);
		}
	}
}
