using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class SocketPriceData
	{
		public int ItemLevel { get; set; }
		public int AddPrice { get; set; }
		public int RemovePrice { get; set; }
	}

	/// <summary>
	/// Select Item database.
	/// </summary>
	public class SocketPriceDb : DatabaseJsonIndexed<int, SocketPriceData>
	{
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("itemLevel", "addPrice", "removePrice");

			var data = new SocketPriceData();

			data.ItemLevel = entry.ReadInt("itemLevel");
			data.AddPrice = entry.ReadInt("addPrice");
			data.RemovePrice = entry.ReadInt("removePrice");

			this.Entries[data.ItemLevel] = data;
		}
	}
}
