using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Events.Arguments;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.World.Quests.Modifiers
{
	/// <summary>
	/// A modifier that gives a chance to add drops to a monster.
	/// </summary>
	public class ItemDropModifier : QuestModifier
	{
		private const string PityVarName = "Melia.Quests.DropMisses.";

		/// <summary>
		/// Returns the item id that a monster drops.
		/// </summary>
		public int ItemId { get; }

		/// <summary>
		/// Returns the amount of chance the monster has to drop an item.
		/// </summary>
		public float DropChance { get; }

		/// <summary>
		/// Returns the tags which monsters must match to qualify for this
		/// objective.
		/// </summary>
		public HashSet<int> MonsterIds { get; }

		/// <summary>
		/// Returns the number of kills without a drop after which the drop
		/// is guaranteed, or 0 if there is no such pity counter.
		/// </summary>
		public int FixedCount { get; set; }

		/// <summary>
		/// Returns the amount of the item that drops at once.
		/// </summary>
		public int Amount { get; set; } = 1;

		public ItemDropModifier(int itemId, float dropChance, params int[] monsterIds)
		{
			this.ItemId = itemId;
			this.DropChance = dropChance;
			this.MonsterIds = new HashSet<int>(monsterIds);
		}

		public ItemDropModifier(int itemId, float dropChance, params string[] monsterIds)
		{
			this.ItemId = itemId;
			this.DropChance = dropChance;
			this.MonsterIds = new HashSet<int>(monsterIds.Length);

			for (var i = 0; i < monsterIds.Length; i++)
			{
				var monster = monsterIds[i];
				if (ZoneServer.Instance.Data.MonsterDb.TryFind(monster, out var data))
					this.MonsterIds.Add(data.Id);
				else
					Log.Warning("ItemDropModifier: Monster '{0}' not found, item {1} will not drop from it.", monster, itemId);
			}
		}

		/// <summary>
		/// Sets up event subscriptions.
		/// </summary>
		public override void Load()
		{
			ZoneServer.Instance.ServerEvents.EntityKilled.Subscribe(this.OnEntityKilled);
		}

		/// <summary>
		/// Cleans up event subscriptions.
		/// </summary>
		public override void Unload()
		{
			ZoneServer.Instance.ServerEvents.EntityKilled.Unsubscribe(this.OnEntityKilled);
		}

		/// <summary>
		/// Called when a character dies.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnEntityKilled(object sender, CombatEventArgs args)
		{
			if (args.Target is not Mob monster)
				return;

			if (args.Attacker is not Character character)
				return;

			character.Quests.UpdateModifiers<ItemDropModifier>((quest, modifier, progress) =>
			{
				if (!modifier.IsTarget(monster))
					return;

				var dropped = GameRandom.Get().NextDouble() < modifier.DropChance;

				if (modifier.FixedCount > 0)
				{
					var varName = PityVarName + modifier.ItemId;
					var misses = quest.Vars.GetInt(varName, 0);

					if (!dropped && ++misses >= modifier.FixedCount)
						dropped = true;

					quest.Vars.SetInt(varName, dropped ? 0 : misses);
				}

				if (dropped)
					character.Inventory.Add(modifier.ItemId, modifier.Amount, InventoryAddType.PickUp);
			});
		}

		/// <summary>
		/// Returns true if the given monster is a target for this objective.
		/// </summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		private bool IsTarget(IMonster monster)
		{
			return this.MonsterIds.Contains(monster.Id);
		}
	}
}
