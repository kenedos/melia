using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class WarpData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public string Name { get; internal set; }
		public string Zone { get; internal set; }
	}

	/// <summary>
	/// Warp database, used statue warps.
	/// </summary>
	public class WarpDb : DatabaseJsonIndexed<int, WarpData>
	{
		public bool TryFind(string zoneName, out WarpData data)
		{
			data = this.Entries.Values.FirstOrDefault(a => a.Zone == zoneName);
			return data != null;
		}

		protected override void ReadEntry(JObject entry)
		{
			// { classId: 10011, className: "WARP_F_SIAULIAI_WEST", name: "West Siauliai Woods", zone: "f_siauliai_west", city: "c_Klaipe", group: "Klaipeda Region", warpOpenQuest: "CMINE6_TO_KATYN7_2" },
			entry.AssertNotMissing("classId", "className", "name", "zone");

			var info = new WarpData();

			info.Id = entry.ReadInt("classId");
			info.ClassName = entry.ReadString("className");
			info.Name = entry.ReadString("name");
			info.Zone = entry.ReadString("zone");

			this.Entries.Add(info.Id, info);
		}
	}
}
