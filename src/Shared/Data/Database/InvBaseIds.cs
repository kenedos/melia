using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class InvBaseIdData
	{
		public int Id { get; set; }
		public int BaseId { get; set; }
		public InventoryCategory Type { get; set; }
	}

	/// <summary>
	/// Inventory category database, indexed by the category type.
	/// </summary>
	public class InvBaseIdDb : DatabaseJsonIndexed<InventoryCategory, InvBaseIdData>
	{
		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "baseId", "name");

			var data = new InvBaseIdData();

			data.Id = entry.ReadInt("id");
			data.BaseId = entry.ReadInt("baseId");
			data.Type = entry.ReadEnum<InventoryCategory>("name");

			this.AddOrReplace(data.Type, data);
		}

		/// <summary>
		/// Returns the category the given index belongs to.
		/// </summary>
		/// <example>
		/// GetCategory(50042) => InventoryCategory.Weapon_Mace
		/// GetCategory(155021) => InventoryCategory.HairAcc_Acc1
		/// </example>
		/// <param name="inventoryIndex"></param>
		/// <returns></returns>
		public void SplitCategoryIndex(int inventoryIndex, out InventoryCategory category, out int index)
		{
			// Go through the categories in order and return the one
			// before the last one that is lower than the given index.
			// E.g. if the given index is 50042, we want to return
			// Weapon_Mace, 50001 (Weapon_Mace) is lower than 50042,
			// but 55001 (Weapon_Musket) is not.

			var entries = this.Entries.Values.OrderBy(a => a.BaseId);
			var prevEntry = (InvBaseIdData)null;

			foreach (var entry in entries)
			{
				if (inventoryIndex < entry.BaseId)
					break;

				prevEntry = entry;
			}

			if (prevEntry == null)
			{
				category = InventoryCategory.None;
				index = 0;
				return;
			}

			category = prevEntry.Type;
			index = inventoryIndex - prevEntry.BaseId;
		}
	}
}
