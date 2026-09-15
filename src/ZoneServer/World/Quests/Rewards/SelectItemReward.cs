using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Rewards
{
	/// <summary>
	/// A reward that lets the player pick one item out of a list.
	/// </summary>
	/// <remarks>
	/// The pick is made before the quest is completed, via
	/// QuestComponent.SelectReward. Without a pick the first option is
	/// given.
	/// </remarks>
	public class SelectItemReward : QuestReward
	{
		/// <summary>
		/// The name of the quest variable the player's pick is stored in.
		/// </summary>
		public const string SelectionVarName = "Melia.Quests.SelectedReward";

		/// <summary>
		/// Returns the item ids the player may choose between.
		/// </summary>
		public IReadOnlyList<int> ItemClassIds { get; }

		/// <summary>
		/// Returns the amount of the chosen item that is given.
		/// </summary>
		public int Amount { get; }

		/// <summary>
		/// Returns the icon to display for this reward.
		/// </summary>
		public override string Icon => "Item:" + this.ItemClassIds[0];

		/// <summary>
		/// Creates a reward that gives one of the given items.
		/// </summary>
		/// <param name="itemClassNames"></param>
		public SelectItemReward(params string[] itemClassNames)
			: this(1, itemClassNames)
		{
		}

		/// <summary>
		/// Creates a reward that gives one of the given items.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="itemClassNames"></param>
		public SelectItemReward(int amount, params string[] itemClassNames)
		{
			if (itemClassNames == null || itemClassNames.Length == 0)
				throw new ArgumentException("Must specify at least one item.");

			var ids = new List<int>(itemClassNames.Length);

			foreach (var itemClassName in itemClassNames)
			{
				var itemData = ZoneServer.Instance.Data.ItemDb.FindByClass(itemClassName);

				if (itemData == null)
					throw new ArgumentException($"Unknown item '{itemClassName}'.");

				ids.Add(itemData.Id);
			}

			this.ItemClassIds = ids;
			this.Amount = amount;
		}

		/// <summary>
		/// Creates a reward that gives one of the given items.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="itemIds"></param>
		public SelectItemReward(int amount, params int[] itemIds)
		{
			if (itemIds == null || itemIds.Length == 0)
				throw new ArgumentException("Must specify at least one item.");

			foreach (var itemId in itemIds)
			{
				if (!ZoneServer.Instance.Data.ItemDb.Contains(itemId))
					throw new ArgumentException($"Unknown item '{itemId}'.");
			}

			this.ItemClassIds = itemIds.ToList();
			this.Amount = amount;
		}

		/// <summary>
		/// Returns true if the given item is one of this reward's options.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public bool IsOption(int itemId)
			=> this.ItemClassIds.Contains(itemId);

		/// <summary>
		/// Gives the item the player picked, or the first option if they
		/// didn't pick one.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		public override void Give(Character character, Quest quest)
		{
			var itemId = this.ItemClassIds[0];
			var selectedItemId = quest?.Vars.GetInt(SelectionVarName, 0) ?? 0;

			if (this.IsOption(selectedItemId))
				itemId = selectedItemId;

			character.Inventory.Add(itemId, this.Amount, InventoryAddType.PickUp);
		}

		/// <summary>
		/// Returns a string representation of the reward.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var names = this.ItemClassIds.Select(a => ZoneServer.Instance.Data.ItemDb.TryFind(a, out var data) ? Localization.Get(data.Name) : a.ToString());
			return string.Join(" / ", names);
		}
	}
}
