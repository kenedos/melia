using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class CubeGachaData
	{
		public int Id { get; set; }
		public string Group { get; set; }
		public string ItemName { get; set; }
		public int Count { get; set; }
		public float Ratio { get; set; }
	}
	public class CubeGachaDb : DatabaseJsonIndexed<int, CubeGachaData>
	{
		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "group", "itemName", "count", "ratio");

			var data = new CubeGachaData();

			data.Id = entry.ReadInt("id");
			data.Group = entry.ReadString("group");
			data.ItemName = entry.ReadString("itemName");
			data.Count = entry.ReadInt("count");
			data.Ratio = entry.ReadFloat("ratio");

			this.AddOrReplace(data.Id, data);
		}
	}
}
