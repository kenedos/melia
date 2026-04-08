using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	/// <summary>
	/// Buff override database, indexed by buff id.
	/// </summary>
	public class BuffOverrideDb : DatabaseJsonIndexed<BuffId, BuffData>
	{
		private readonly BuffDb _buffDb;
		public BuffOverrideDb(BuffDb buffDb)
		{
			this._buffDb = buffDb;
		}

		/// <summary>
		/// Reads given entry and applies overrides to existing buff data.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			var buffId = (BuffId)entry.ReadInt("id");
			var className = entry.ReadString("className");

			if (!_buffDb.TryFind(buffId, out var data) || className != data.ClassName)
				_buffDb.TryFind(className, out data);

			if (data == null)
				return;

			if (entry.ContainsKey("name"))
				data.Name = entry.ReadString("name");
			if (entry.ContainsKey("type"))
				data.Type = entry.ReadEnum<BuffType>("type");
			if (entry.ContainsKey("level"))
				data.Level = entry.ReadInt("level");
			if (entry.ContainsKey("duration"))
				data.Duration = TimeSpan.FromMilliseconds(entry.ReadInt("duration"));
			if (entry.ContainsKey("updateTime"))
				data.UpdateTime = TimeSpan.FromMilliseconds(entry.ReadInt("updateTime"));
			if (entry.ContainsKey("overBuff"))
				data.OverBuff = entry.ReadInt("overBuff");
			if (entry.ContainsKey("buffExpUp"))
				data.BuffUpExp = entry.ReadInt("buffExpUp");
			if (entry.ContainsKey("removable"))
				data.Removable = entry.ReadBool("removable");
			if (entry.ContainsKey("removeOnDeath"))
				data.RemoveOnDeath = entry.ReadBool("removeOnDeath");
			if (entry.ContainsKey("removeBySkill"))
				data.RemoveBySkill = entry.ReadBool("removeBySkill");
			if (entry.ContainsKey("save"))
				data.Save = entry.ReadBool("save");
			if (entry.ContainsKey("updateProperties"))
				data.UpdateProperties = entry.ReadArray("updateProperties", new string[0]);
			if (entry.ContainsKey("tags"))
				data.Tags = new Tags(entry.ReadList<string>("tags", []));

			this.AddOrReplace(data.Id, data);
		}
	}
}
