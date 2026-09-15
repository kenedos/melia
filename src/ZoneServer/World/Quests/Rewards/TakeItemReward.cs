using System;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Rewards
{
	/// <summary>
	/// A reward that takes items back from the player when the quest is
	/// completed, such as the items a collection objective gathered.
	/// </summary>
	public class TakeItemReward : QuestReward
	{
		/// <summary>
		/// Returns the id of the item this reward takes.
		/// </summary>
		public int ItemClassId { get; }

		/// <summary>
		/// Returns the english name of the item.
		/// </summary>
		public string ItemName { get; }

		/// <summary>
		/// Returns the amount that is taken, or -1 to take everything the
		/// character has.
		/// </summary>
		public int Amount { get; }

		/// <summary>
		/// Returns the icon to display for this reward.
		/// </summary>
		public override string Icon => "Item:" + this.ItemClassId;

		/// <summary>
		/// Returns whether the reward is listed in the client's reward
		/// display.
		/// </summary>
		public override bool Displayed => false;

		/// <summary>
		/// Creates a reward that takes back the given item.
		/// </summary>
		/// <param name="itemClassName"></param>
		/// <param name="amount">Amount to take, or -1 for all of them.</param>
		public TakeItemReward(string itemClassName, int amount = -1)
		{
			var itemData = ZoneServer.Instance.Data.ItemDb.FindByClass(itemClassName);

			if (itemData == null)
				throw new ArgumentException($"Unknown item '{itemClassName}'.");

			this.ItemClassId = itemData.Id;
			this.ItemName = itemData.Name;
			this.Amount = amount;
		}

		/// <summary>
		/// Creates a reward that takes back the given item.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount">Amount to take, or -1 for all of them.</param>
		public TakeItemReward(int itemId, int amount = -1)
		{
			if (!ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out var itemData))
				throw new ArgumentException($"Unknown item '{itemId}'.");

			this.ItemClassId = itemId;
			this.ItemName = itemData.Name;
			this.Amount = amount;
		}

		/// <summary>
		/// Removes the items from the character's inventory.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		public override void Give(Character character, Quest quest)
		{
			var amount = this.Amount < 0 ? character.Inventory.CountItem(this.ItemClassId) : this.Amount;
			if (amount <= 0)
				return;

			character.Inventory.Remove(this.ItemClassId, amount, InventoryItemRemoveMsg.Given);
		}

		/// <summary>
		/// Returns a string representation of the reward.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var itemName = Localization.Get(this.ItemName);

			if (this.Amount < 0)
				return itemName;

			var format = Localization.GetPlural("{0}", "{1}x {0}", this.Amount);
			return string.Format(format, itemName, this.Amount);
		}
	}
}
