using System;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Zone.World.Actors.Monsters
{
	/// <summary>
	/// A representation of an item on a map.
	/// </summary>
	public class ItemMonster : MonsterInName
	{
		/// <summary>
		/// Returns the item this monster represents.
		/// </summary>
		public Item Item { get; private set; }

		/// <summary>
		/// Gets or sets whether the item was picked up.
		/// </summary>
		public bool PickedUp { get; set; }

		/// <summary>
		/// Returns if the item's associated monster it was dropped from.
		/// </summary>
		public int MonsterId { get; set; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="monsterId"></param>
		private ItemMonster(Item item, int monsterId) : base(monsterId)
		{
			this.Item = item;
			//this.Properties = item.Properties;
		}

		/// <summary>
		/// Creates item monster from item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ItemMonster Create(Item item)
		{
			if (!ZoneServer.Instance.Data.ItemMonsterDb.TryFind(item.Id, out var data))
				throw new ArgumentException($"No monster id found for item '{item.Id}'.");

			var monster = new ItemMonster(item, data.MonsterId);
			return monster;
		}

		/// <summary>
		/// Creates item monster from item.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="monsterLevel">Level of the monster that dropped the item.</param>
		/// <returns></returns>
		public static ItemMonster Create(Item item, int monsterLevel)
		{
			if (!ZoneServer.Instance.Data.ItemMonsterDb.TryFind(item.Id, out var data))
				throw new ArgumentException($"No monster id found for item '{item.Id}'.");

			var monster = new ItemMonster(item, data.MonsterId);

			if (ZoneServer.Instance.Conf.World.EnableMonsterLevelItemBonus)
				GenerateAdditionalProperties(item, monsterLevel);

			return monster;
		}

		/// <summary>
		/// Generates additional item properties based on the monster's level.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="monsterLevel"></param>
		private static void GenerateAdditionalProperties(Item item, int monsterLevel)
		{
			if (monsterLevel <= 0)
				return;

			var levelBonus = monsterLevel * 0.01f; // 2% bonus per level

			var properties = item.Data.GetMainStatProperties();

			foreach (var prop in properties)
			{
				item.Properties.Modify(prop, item.Properties[prop] * levelBonus);
			}
			item.GenerateRandomOptions(1, 1);
		}

		/// <summary>
		/// Returns true if the entity is allowed to pick up the item.
		/// </summary>
		/// <param name="actor">The actor attempting to pick up the item.</param>
		/// <returns></returns>
		public bool CanBePickedUpBy(IActor actor)
		{
			var isOriginalOwner = false;
			if (actor is Character character && this.Item.OriginalOwnerCharacterId != 0)
			{
				isOriginalOwner = (this.Item.OriginalOwnerCharacterId == character.ObjectId);
			}
			else
			{
				isOriginalOwner = (this.Item.OriginalOwnerHandle == actor.Handle);
			}

			if (isOriginalOwner)
			{
				if (DateTime.Now < this.Item.RePickUpTime)
					return false;

				if (!this.Position.InRange2D(actor.Position, 20))
					return false;
			}

			if (DateTime.Now < this.Item.LootProtectionEnd)
			{
				var isLootOwner = false;
				if (actor is Character possibleOwner && this.Item.OwnerCharacterId != 0)
				{
					isLootOwner = (this.Item.OwnerCharacterId == possibleOwner.ObjectId);
				}
				else
				{
					isLootOwner = (this.Item.OwnerHandle == actor.Handle);
				}
				return isLootOwner;
			}

			return true;
		}
	}

	/// <summary>
	/// Represents an item stack that is to be dropped.
	/// </summary>
	public class DropStack
	{
		/// <summary>
		/// Returns the class id of the item.
		/// </summary>
		public int ItemId { get; set; }

		/// <summary>
		/// Returns the amount of items in the stack.
		/// </summary>
		public int Amount { get; set; }

		/// <summary>
		/// Returns the original, unaltered drop chance.
		/// </summary>
		public float DropChance { get; set; }

		/// <summary>
		/// Returns the drop chance adjusted by the drop rate.
		/// </summary>
		public float AdjustedDropChance { get; set; }

		/// <summary>
		/// Creates a new stack.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <param name="dropChance"></param>
		/// <param name="adjustedDropChance"></param>
		public DropStack(int itemId, int amount, float dropChance, float adjustedDropChance)
		{
			this.ItemId = itemId;
			this.Amount = amount;
			this.DropChance = dropChance;
			this.AdjustedDropChance = adjustedDropChance;
		}
	}
}
