using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using Yggdrasil.Logging;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class TreasureDropData
	{
		public ItemData Item { get; set; }
		public float ProbabilityFactor { get; set; }
	}

	public class TreasureDropDb : DatabaseJsonIndexed<int, TreasureDropData>
	{
		private readonly ItemDb _itemDb;
		public TreasureDropDb(ItemDb itemDb)
		{
			this._itemDb = itemDb;
		}

		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("itemId", "className", "name", "type", "group", "probabilityFactor");

			var data = new TreasureDropData();

			var itemId = entry.ReadInt("itemId");
			var itemClassName = entry.ReadString("className");

			data.ProbabilityFactor = entry.ReadFloat("probabilityFactor");

			data.Item = _itemDb.Find(itemId);
			if (data.Item == null)
				Log.Warning($"TreasureDropDb: Unable to add item '{itemId}' with className '{itemClassName}' to global drops");
			else
			{
				if (data.Item.ClassName != itemClassName)
					Log.Warning($"TreasureDropDb: mismatch between itemId '{itemId}' and className '{itemClassName}'");

				this.Entries.Add(itemId, data);
			}
		}
	}
}
