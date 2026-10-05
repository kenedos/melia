using MySqlConnector;

namespace Melia.Zone.Database
{
	public class HuntingTaskState
	{
		public long AccountId { get; set; }
		public int Points { get; set; }
		public int TasksCompleted { get; set; }
		public byte Status { get; set; }
		public int Option1MonsterId { get; set; }
		public int Option2MonsterId { get; set; }
		public int Option3MonsterId { get; set; }
		public int SelectedMonsterId { get; set; }
		public int RequiredKills { get; set; }
		public int CurrentKills { get; set; }
		public int RewardPoints { get; set; }
		public int RerollCount { get; set; }
	}

	public partial class ZoneDb
	{
		public HuntingTaskState GetHuntingTask(long accountId)
		{
			using var conn = this.GetConnection();
			using var cmd = new MySqlCommand("SELECT * FROM `hunting_tasks` WHERE `accountId` = @accountId", conn);
			cmd.Parameters.AddWithValue("@accountId", accountId);

			using var reader = cmd.ExecuteReader();

			if (!reader.Read())
				return null;

			return new HuntingTaskState
			{
				AccountId = reader.GetInt64("accountId"),
				Points = reader.GetInt32("points"),
				TasksCompleted = reader.GetInt32("tasksCompleted"),
				Status = reader.GetByte("status"),
				Option1MonsterId = reader.GetInt32("option1MonsterId"),
				Option2MonsterId = reader.GetInt32("option2MonsterId"),
				Option3MonsterId = reader.GetInt32("option3MonsterId"),
				SelectedMonsterId = reader.GetInt32("selectedMonsterId"),
				RequiredKills = reader.GetInt32("requiredKills"),
				CurrentKills = reader.GetInt32("currentKills"),
				RewardPoints = reader.GetInt32("rewardPoints"),
				RerollCount = reader.GetInt32("rerollCount"),
			};
		}

		public void SaveHuntingTask(HuntingTaskState state)
		{
			using var conn = this.GetConnection();
			using var cmd = new MySqlCommand(@"
INSERT INTO `hunting_tasks`
(
	`accountId`,
	`points`,
	`tasksCompleted`,
	`status`,
	`option1MonsterId`,
	`option2MonsterId`,
	`option3MonsterId`,
	`selectedMonsterId`,
	`requiredKills`,
	`currentKills`,
	`rewardPoints`,
	`rerollCount`
)
VALUES
(
	@accountId,
	@points,
	@tasksCompleted,
	@status,
	@option1MonsterId,
	@option2MonsterId,
	@option3MonsterId,
	@selectedMonsterId,
	@requiredKills,
	@currentKills,
	@rewardPoints,
	@rerollCount
)
ON DUPLICATE KEY UPDATE
	`points` = VALUES(`points`),
	`tasksCompleted` = VALUES(`tasksCompleted`),
	`status` = VALUES(`status`),
	`option1MonsterId` = VALUES(`option1MonsterId`),
	`option2MonsterId` = VALUES(`option2MonsterId`),
	`option3MonsterId` = VALUES(`option3MonsterId`),
	`selectedMonsterId` = VALUES(`selectedMonsterId`),
	`requiredKills` = VALUES(`requiredKills`),
	`currentKills` = VALUES(`currentKills`),
	`rewardPoints` = VALUES(`rewardPoints`),
	`rerollCount` = VALUES(`rerollCount`);", conn);

			cmd.Parameters.AddWithValue("@accountId", state.AccountId);
			cmd.Parameters.AddWithValue("@points", state.Points);
			cmd.Parameters.AddWithValue("@tasksCompleted", state.TasksCompleted);
			cmd.Parameters.AddWithValue("@status", state.Status);
			cmd.Parameters.AddWithValue("@option1MonsterId", state.Option1MonsterId);
			cmd.Parameters.AddWithValue("@option2MonsterId", state.Option2MonsterId);
			cmd.Parameters.AddWithValue("@option3MonsterId", state.Option3MonsterId);
			cmd.Parameters.AddWithValue("@selectedMonsterId", state.SelectedMonsterId);
			cmd.Parameters.AddWithValue("@requiredKills", state.RequiredKills);
			cmd.Parameters.AddWithValue("@currentKills", state.CurrentKills);
			cmd.Parameters.AddWithValue("@rewardPoints", state.RewardPoints);
			cmd.Parameters.AddWithValue("@rerollCount", state.RerollCount);

			cmd.ExecuteNonQuery();
		}
	}
}
