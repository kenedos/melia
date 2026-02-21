using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Melia.Shared.Data.Database
{

	public class ItemIconData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public string Icon { get; set; }
		public string IconFile { get; set; }
	}
	public class ItemIconDb : DatabaseJsonIndexed<string, ItemIconData>
	{
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("itemId", "className");

			var icon = new ItemIconData();

			icon.Id = entry.ReadInt("itemId");
			icon.ClassName = entry.ReadString("className");
			icon.Icon = entry.ReadString("icon");
			icon.IconFile = entry.ReadString("iconFile");

			this.Entries[icon.ClassName] = icon;
		}
	}
}
