using Melia.Shared.L10N;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Rewards
{
	/// <summary>
	/// A reward that gives bonus stat points.
	/// </summary>
	public class StatPointReward : QuestReward
	{
		/// <summary>
		/// Returns the amount of stat points the player gets.
		/// </summary>
		public int Amount { get; }

		/// <summary>
		/// Returns the icon to display for this reward.
		/// </summary>
		public override string Icon => "expup_img";

		/// <summary>
		/// Creates a quest reward for bonus stat points.
		/// </summary>
		/// <param name="amount"></param>
		public StatPointReward(int amount)
		{
			this.Amount = amount;
		}

		/// <summary>
		/// Gives the stat points to the character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		public override void Give(Character character, Quest quest)
		{
			character.AddStatPoints(this.Amount);
		}

		/// <summary>
		/// Returns a string representation of the reward.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(Localization.Get("{0} Stat Points"), this.Amount);
		}
	}
}
