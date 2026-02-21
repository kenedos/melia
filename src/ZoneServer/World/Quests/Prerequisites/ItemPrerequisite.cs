using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Prerequisites
{
	/// <summary>
	/// A prerequisite to reach a have certain item and quantity.
	/// </summary>
	public class ItemPrerequisite : QuestPrerequisite
	{
		/// <summary>
		/// Returns the item required, to meet this prerequisite.
		/// </summary>
		public int ItemId { get; }

		/// <summary>
		/// Returns the minimum item amount to reach, to meet this prerequisite.
		/// </summary>
		public int MinItemAmount { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="minItemAmount"></param>
		public ItemPrerequisite(int itemId, int minItemAmount = 1)
		{
			this.ItemId = itemId;
			this.MinItemAmount = minItemAmount;
		}

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="minItemAmount"></param>
		public ItemPrerequisite(string itemId, int minItemAmount = 1)
		{
			var item = ZoneServer.Instance.Data.ItemDb.FindByClass(itemId);

			this.ItemId = item.Id;
			this.MinItemAmount = minItemAmount;
		}

		/// <summary>
		/// Returns true if the character's item amount is equal or higher than
		/// this prerequisite's minimum item amount required.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public override bool Met(Character character)
		{
			return character.Inventory.CountItem(this.ItemId) >= this.MinItemAmount;
		}
	}
}
