using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data;
using Yggdrasil.Data.JSON;
using Yggdrasil.Logging;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class EquipCardData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public CardUseType UseType { get; set; }
		public CardScriptData ConditionScript { get; set; }
		public CardScriptData Script { get; set; }
	}

	[Serializable]
	public class CardScriptData
	{
		public string Function { get; set; }
		public string StrArg { get; set; }
		public string StrArg2 { get; set; }
		public string StrArg3 { get; set; }
		public string StrArg4 { get; set; }
		public float NumArg1 { get; set; }
		public float NumArg2 { get; set; }
	}

	/// <summary>
	/// Equip card database.
	/// </summary>
	public class EquipCardDb : DatabaseJsonIndexed<string, EquipCardData>
	{
		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <remarks>
		/// Uses the JSON database to be fed its entries, but doesn't
		/// adhere to the Entries format and uses custom lists instead.
		/// </remarks>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "className", "useType");

			var data = new EquipCardData();

			data.Id = entry.ReadInt("id");
			data.ClassName = entry.ReadString("className");
			data.UseType = entry.ReadEnum("useType", CardUseType.Always);

			if (entry.TryGetObject("condScript", out var scriptEntry))
			{
				var scriptData = new CardScriptData();

				scriptData.Function = scriptEntry.ReadString("function");
				scriptData.StrArg = scriptEntry.ReadString("strArg");
				scriptData.StrArg2 = scriptEntry.ReadString("strArg2", "None");
				scriptData.StrArg3 = scriptEntry.ReadString("strArg3", "None");
				scriptData.StrArg4 = scriptEntry.ReadString("strArg4");
				scriptData.NumArg1 = scriptEntry.ReadFloat("numArg1");
				scriptData.NumArg2 = scriptEntry.ReadFloat("numArg2");

				data.ConditionScript = scriptData;
			}

			if (entry.TryGetObject("script", out scriptEntry))
			{
				var scriptData = new CardScriptData();

				scriptData.Function = scriptEntry.ReadString("function");
				scriptData.StrArg = scriptEntry.ReadString("strArg");
				scriptData.StrArg2 = scriptEntry.ReadString("strArg2");
				scriptData.StrArg3 = scriptEntry.ReadString("strArg3");
				scriptData.StrArg4 = scriptEntry.ReadString("strArg4");
				scriptData.NumArg1 = scriptEntry.ReadFloat("numArg1");
				scriptData.NumArg2 = scriptEntry.ReadFloat("numArg2");

				data.Script = scriptData;
			}

			this.AddOrReplace(data.ClassName, data);
		}
	}
}
