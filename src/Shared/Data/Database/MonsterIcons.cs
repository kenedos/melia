using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Melia.Shared.Data.Database
{

	public class MonsterIconData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public string Icon { get; set; }
		public string IconFile { get; set; }
	}
	public class MonsterIconDb : DatabaseJsonIndexed<string, MonsterIconData>
	{
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("monsterId", "className");

			var icon = new MonsterIconData();

			icon.Id = entry.ReadInt("monsterId");
			icon.ClassName = entry.ReadString("className");
			icon.Icon = entry.ReadString("icon");
			icon.IconFile = entry.ReadString("iconFile");

			this.Entries[icon.ClassName] = icon;
		}
	}
}
