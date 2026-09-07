using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Zone.World.Storages
{
	/// <summary>
	/// Offering box of a Pardoner, holding the items other players
	/// donated to them until they are given to the church or taken back.
	/// </summary>
	/// <remarks>
	/// The box is not browsed through the storage window, so it neither
	/// opens nor sends the storage packets. Its contents reach the client
	/// through the offering box UI instead.
	/// </remarks>
	public class OblationStorage : Storage
	{
		/// <summary>
		/// Number of items the box holds per level of Oblation.
		/// </summary>
		public const int SlotsPerSkillLevel = 7;

		/// <summary>
		/// Number of slots the box is allocated, enough for the skill at
		/// its highest level.
		/// </summary>
		public const int MaxSize = SlotsPerSkillLevel * 15;

		private readonly Dictionary<long, int> _pricesPaid = new();

		/// <summary>
		/// Character that owns this offering box.
		/// </summary>
		public Character Owner { get; }

		/// <summary>
		/// When the box last filled up, or null while it has room.
		/// </summary>
		/// <remarks>
		/// A full box takes no more offers, so the shop standing in a town
		/// is only in the way; this is what the wait before closing it is
		/// measured from. It is deliberately not saved - a box that was
		/// full before a restart gets the full wait again.
		/// </remarks>
		public DateTime? FullSince { get; set; }

		/// <summary>
		/// Creates new offering box.
		/// </summary>
		/// <param name="owner"></param>
		public OblationStorage(Character owner) : base()
		{
			this.Owner = owner;
		}

		/// <summary>
		/// Does nothing, the box has no storage window to open.
		/// </summary>
		/// <returns></returns>
		public override StorageResult Open()
			=> StorageResult.InvalidOperation;

		/// <summary>
		/// Does nothing, the box has no storage window to close.
		/// </summary>
		/// <returns></returns>
		public override StorageResult Close()
			=> StorageResult.InvalidOperation;

		/// <summary>
		/// Does nothing, items enter the box by being offered to it.
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public override StorageResult StoreItem(long objectId, int amount)
			=> StorageResult.InvalidOperation;

		/// <summary>
		/// Does nothing, items leave the box through TakeBack.
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public override StorageResult RetrieveItem(long objectId, int amount)
			=> StorageResult.InvalidOperation;

		/// <summary>
		/// Puts the given item into the box, remembering the silver its
		/// owner paid for it.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="pricePaid"></param>
		/// <returns></returns>
		public StorageResult Offer(Item item, int pricePaid)
		{
			var result = this.Add(item, out var position);
			if (result != StorageResult.Success)
				return result;

			// A stackable offer merges into whatever already sits there,
			// and the price is per unit, so it belongs to that item.
			var storedItem = this.GetItemAtPosition(position) ?? item;
			_pricesPaid[storedItem.ObjectId] = pricePaid;

			return StorageResult.Success;
		}

		/// <summary>
		/// Moves the item with the given object id out of the box and into
		/// the owner's inventory, keeping its properties.
		/// </summary>
		/// <param name="objectId"></param>
		/// <returns></returns>
		public StorageResult TakeBack(long objectId)
		{
			var result = this.GetItem(objectId, out var item, out _);
			if (result != StorageResult.Success)
				return result;

			var takenItem = new Item(item);

			result = this.Remove(item, item.Amount, out _, out var removedAmount);
			if (result != StorageResult.Success)
				return result;

			_pricesPaid.Remove(objectId);

			takenItem.Amount = removedAmount;
			this.Owner.Inventory.Add(takenItem, InventoryAddType.New);

			return StorageResult.Success;
		}

		/// <summary>
		/// Removes the item with the given object id from the box without
		/// giving it to anyone, as happens when it's donated.
		/// </summary>
		/// <param name="objectId"></param>
		/// <returns></returns>
		public StorageResult Consume(long objectId)
		{
			var result = this.GetItem(objectId, out var item, out _);
			if (result != StorageResult.Success)
				return result;

			result = this.Remove(item, item.Amount, out _, out _);
			if (result != StorageResult.Success)
				return result;

			_pricesPaid.Remove(objectId);

			return StorageResult.Success;
		}

		/// <summary>
		/// Returns the silver the owner paid for the item with the given
		/// object id, or 0 if the box doesn't hold it.
		/// </summary>
		/// <param name="objectId"></param>
		/// <returns></returns>
		public int GetPricePaid(long objectId)
		{
			_pricesPaid.TryGetValue(objectId, out var pricePaid);
			return pricePaid;
		}

		/// <summary>
		/// Sets the silver the owner paid for the item with the given
		/// object id, as read back from the database.
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="pricePaid"></param>
		public void SetPricePaid(long objectId, int pricePaid)
			=> _pricesPaid[objectId] = pricePaid;
	}
}
