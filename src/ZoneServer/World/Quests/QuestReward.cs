using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests
{
	/// <summary>
	/// Base class for quest rewards.
	/// </summary>
	public abstract class QuestReward
	{
		/// <summary>
		/// Returns the quest progress necessary to receive this reward.
		/// </summary>
		public float RequiredProgress { get; }

		/// <summary>
		/// Returns the icon to display for this reward.
		/// </summary>
		public virtual string Icon { get; } = "icon_item_silver";

		/// <summary>
		/// Returns whether the reward is listed in the client's reward
		/// display.
		/// </summary>
		public virtual bool Displayed { get; } = true;

		/// <summary>
		/// Gives reward to character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		public abstract void Give(Character character, Quest quest);

		/// <summary>
		/// Initializes reward.
		/// </summary>
		/// <param name="type"></param>
		protected QuestReward()
		{
		}

		/// <summary>
		/// Returns a string representation of the reward.
		/// </summary>
		/// <returns></returns>
		public abstract override string ToString();
	}
}
