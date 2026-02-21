using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class QuestStaticData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public string Name { get; set; }
		public string QuestSSN { get; internal set; }
		public string QuestProperty { get; internal set; }
		public string QuestMode { get; internal set; }
		public string QuestStartMode { get; internal set; }
		public string QuestEndMode { get; internal set; }
		public string QStartZone { get; internal set; }
		public string StartMap { get; internal set; }
		public string StartLocation { get; internal set; }
		public string StartNPC { get; internal set; }
		public string ProgMap { get; internal set; }
		public string ProgLocation { get; internal set; }
		public string ProgNPC { get; internal set; }
		public string EndMap { get; internal set; }
		public string EndLocation { get; internal set; }
		public string EndNPC { get; internal set; }
		public string RequiredQuestItem { get; internal set; }
		public List<string> RequiredQuests { get; internal set; }
	}

	/// <summary>
	/// Quest database, indexed by quest id.
	/// </summary>
	public class QuestDb : DatabaseJsonIndexed<int, QuestStaticData>
	{
		/// <summary>
		/// Returns first Quest data entry with given class name, or null
		/// if it wasn't found.
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public QuestStaticData Find(string className)
		{
			return this.Entries.Values.FirstOrDefault(a => a.ClassName.ToLower() == className.ToLower());
		}

		public bool TryFind(string className, out QuestStaticData quest)
		{
			quest = this.Find(className);
			return quest != null;
		}

		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "className", "name");

			var info = new QuestStaticData();

			info.Id = entry.ReadInt("id");
			info.ClassName = entry.ReadString("className");
			info.Name = entry.ReadString("name");

			info.QuestSSN = entry.ReadString("questSSN");
			info.QuestProperty = entry.ReadString("questPropertyName");
			info.QuestMode = entry.ReadString("questMode");
			info.QuestStartMode = entry.ReadString("questStartMode");
			info.QuestEndMode = entry.ReadString("questEndMode");
			info.QStartZone = entry.ReadString("questStartZone");
			info.StartMap = entry.ReadString("startMap");
			info.StartLocation = entry.ReadString("startLocation");
			info.StartNPC = entry.ReadString("startNPC");
			info.ProgMap = entry.ReadString("progressMap");
			info.ProgLocation = entry.ReadString("progressLocation");
			info.ProgNPC = entry.ReadString("progressNPC");
			info.EndMap = entry.ReadString("endMap");
			info.EndLocation = entry.ReadString("endLocation");
			info.EndNPC = entry.ReadString("endNPC");
			info.RequiredQuestItem = entry.ReadString("requiredQuestItem");
			info.RequiredQuests = entry.ReadList<string>("requiredQuestName");

			this.Entries[info.Id] = info;
		}
	}
}
