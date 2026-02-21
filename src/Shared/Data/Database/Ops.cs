using System;
using Melia.Shared.Network;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using Yggdrasil.Logging;

namespace Melia.Shared.Data.Database
{
	public class OpData
	{
		public Op Op { get; set; }
		public int Code { get; set; }
		public string Name { get; set; }
		public int Size { get; set; }
	}

	public class OpDb : DatabaseJson<OpData>
	{
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("opName", "opCode", "size");

			var opData = new OpData();
			opData.Op = entry.ReadEnum("opName", Op.Undefined);
			opData.Name = entry.ReadString("opName");
			var opCodeStr = entry.ReadString("opCode");
			opData.Code = opCodeStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
				? Convert.ToInt32(opCodeStr.Substring(2), 16)
				: Convert.ToInt32(opCodeStr);
			opData.Size = entry.ReadInt("size");

			if (opData.Op == Op.Undefined)
			{
				Log.Warning($"Enum OP not defined for {opData.Name}");
			}

			OpTable.AddOp(opData);
			this.Entries.Add(opData);
		}
	}
}
