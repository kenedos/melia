using System;
using Melia.Zone.Events;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Objectives
{
	/// <summary>
	/// Objective that shows the amount of one item as a meter and is
	/// completed by owning a different item.
	/// </summary>
	public class ItemMeterObjective : QuestObjective
	{
		/// <summary>
		/// Returns the id of the item whose amount fills the meter.
		/// </summary>
		public int MeterItemId { get; }

		/// <summary>
		/// Returns the id of the item that completes the objective.
		/// </summary>
		public int GoalItemId { get; }

		/// <summary>
		/// Creates an objective whose meter follows the amount of the
		/// meter item, and which is completed by owning the goal item.
		/// </summary>
		/// <param name="meterItemClassName"></param>
		/// <param name="goalItemClassName"></param>
		/// <param name="target"></param>
		public ItemMeterObjective(string meterItemClassName, string goalItemClassName, int target)
		{
			if (!ZoneServer.Instance.Data.ItemDb.TryFind(meterItemClassName, out var meterData))
				throw new ArgumentException($"ItemMeterObjective: Unknown item '{meterItemClassName}'.");

			if (!ZoneServer.Instance.Data.ItemDb.TryFind(goalItemClassName, out var goalData))
				throw new ArgumentException($"ItemMeterObjective: Unknown item '{goalItemClassName}'.");

			this.MeterItemId = meterData.Id;
			this.GoalItemId = goalData.Id;
			this.TargetCount = target;
		}

		/// <summary>
		/// Sets up event subscriptions.
		/// </summary>
		public override void Load()
		{
			ZoneServer.Instance.ServerEvents.PlayerAddedItem.Subscribe(this.OnPlayerAddedOrRemovedItem);
			ZoneServer.Instance.ServerEvents.PlayerRemovedItem.Subscribe(this.OnPlayerAddedOrRemovedItem);
		}

		/// <summary>
		/// Cleans up event subscriptions.
		/// </summary>
		public override void Unload()
		{
			ZoneServer.Instance.ServerEvents.PlayerAddedItem.Unsubscribe(this.OnPlayerAddedOrRemovedItem);
			ZoneServer.Instance.ServerEvents.PlayerRemovedItem.Unsubscribe(this.OnPlayerAddedOrRemovedItem);
		}

		/// <summary>
		/// Called when a player gets and starts a quest with this objective.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		public override void OnStart(Character character, Quest quest)
		{
			quest.UpdateObjectives<ItemMeterObjective>((quest, objective, progress) =>
			{
				progress.Count = objective.GetCount(character, out var done);
				progress.Done = done;
			});
		}

		/// <summary>
		/// Called when a player got or lost an item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnPlayerAddedOrRemovedItem(object sender, PlayerItemEventArgs args)
		{
			var character = args.Character;

			if (args.ItemId != this.MeterItemId && args.ItemId != this.GoalItemId)
				return;

			character.Quests.UpdateObjectives<ItemMeterObjective>((quest, objective, progress) =>
			{
				if (objective != this || progress.Done)
					return;

				var count = objective.GetCount(character, out var done);
				if (count == progress.Count && !done)
					return;

				progress.Count = count;
				progress.Done = done;

				character.Quests.UpdateQuestProgress(quest.Data.Id.Value, objective.Id);
			});
		}

		private int GetCount(Character character, out bool done)
		{
			done = character.Inventory.CountItem(this.GoalItemId) > 0;

			return done ? this.TargetCount : Math.Min(this.TargetCount, character.Inventory.CountItem(this.MeterItemId));
		}
	}
}
