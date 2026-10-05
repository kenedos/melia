using System;
using Melia.Shared.Game.Const;

namespace Melia.Zone.World.Items
{
	/// <summary>
	/// A dummy item used in empty equipment slots.
	/// </summary>
	/// <remarks>
	/// Useful because equipment slots are never truly empty. Even if
	/// no item is equipped, a dummy item with a class id and a zeroed
	/// object id is still present.
	/// </remarks>
	public class DummyEquipItem : Item
	{
		/// <summary>
		/// Returns an empty object id.
		/// </summary>
		/// <remarks>
		/// Officials set the object id of dummy items to 0, though
		/// whether this is actually necessary is currently unknown.
		/// This does affect packet structures however.
		/// </remarks>
		public override long ObjectId => 0;

		/// <summary>
		/// Creates new dummy item.
		/// </summary>
		/// <param name="slot"></param>
		public DummyEquipItem(EquipSlot slot) : base(InventoryDefaults.EquipItems[(int)slot], 1)
		{
			this.Amount = 0;
		}

		protected override void LoadData()
		{
			if (this.Id == 0)
				throw new InvalidOperationException("Item id wasn't set before calling LoadData.");

			this.Data = ZoneServer.Instance.Data.ItemDb.Find(this.Id);
			if (this.Data == null)
				throw new NullReferenceException("No item data found for '" + this.Id + "'.");
		}
	}
}
