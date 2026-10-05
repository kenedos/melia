using System;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class AchievementStatRewardData
	{
		public int Id { get; set; }
		public int AchieveCount { get; set; }
		public int Str { get; set; }
		public int Con { get; set; }
		public int Int { get; set; }
		public int Spr { get; set; }
		public int Dex { get; set; }
		public int Patk { get; set; }
		public int Matk { get; set; }
		public int Def { get; set; }
		public int Mdef { get; set; }
		public int Msp { get; set; }
	}

	public class AchievementStatRewardDb : DatabaseJsonIndexed<int, AchievementStatRewardData>
	{
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "achieveCount", "str", "con", "int", "spr", "dex", "patk", "matk", "def", "mdef", "msp");

			var data = new AchievementStatRewardData();

			data.Id = entry.ReadInt("id");
			data.AchieveCount = entry.ReadInt("achieveCount");
			data.Str = entry.ReadInt("str");
			data.Con = entry.ReadInt("con");
			data.Int = entry.ReadInt("int");
			data.Spr = entry.ReadInt("spr");
			data.Dex = entry.ReadInt("dex");
			data.Patk = entry.ReadInt("patk");
			data.Matk = entry.ReadInt("matk");
			data.Def = entry.ReadInt("def");
			data.Mdef = entry.ReadInt("mdef");
			data.Msp = entry.ReadInt("msp");

			this.AddOrReplace(data.Id, data);
		}
	}
}
