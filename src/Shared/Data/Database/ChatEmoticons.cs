using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Melia.Shared.Data.Database
{

	public class ChatEmoticonData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public bool CheckServer { get; set; }
	}
	public class ChatEmoticonDb : DatabaseJsonIndexed<string, ChatEmoticonData>
	{
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "className");

			var emoticon = new ChatEmoticonData();

			emoticon.Id = entry.ReadInt("id");
			emoticon.ClassName = entry.ReadString("className");
			emoticon.CheckServer = entry.ReadBool("checkServer");

			this.Entries[emoticon.ClassName] = emoticon;
		}
	}
}
