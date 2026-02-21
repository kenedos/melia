using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const.Web;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class RewardAttendanceData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public int DayOffset { get; set; }
		public int RewardCount { get; set; }
		public int RewardItemCnt_1 { get; set; }
		public int AppendPropertyStatus { get; set; }
		public string GroupName { get; set; }
		public string Grade { get; set; }
		public string RewardItem_1 { get; set; }
		public string AppendPropertyName { get; set; }
	}

	/// <summary>
	/// Reward Attendance database, used statue warps.
	/// </summary>
	public class RewardAttendanceDb : DatabaseJsonIndexed<int, RewardAttendanceData>
	{
		public bool TryFind(string className, out RewardAttendanceData data)
		{
			data = this.Entries.Values.FirstOrDefault(a => a.ClassName == className);
			return data != null;
		}

		public List<RewardAttendanceData> FindByGroupName(string groupName)
		{
			return this.Entries.Values.Where(a => a.GroupName == groupName).ToList();
		}

		protected override void ReadEntry(JObject entry)
		{
			// { id: 0, dayOffset: 0, rewardCount: 1, rewardItemCnt_1: 1, appendPropertyStatus: 0, className: "VakarinePackage_00", grade: "S", rewardItem_1: "Vakarine_Box_01", appendPropertyName: "None"},
			entry.AssertNotMissing("id", "dayOffset", "rewardCount", "rewardItemCnt_1", "appendPropertyStatus", "className", "groupName", "grade", "rewardItem_1", "appendPropertyName");

			var info = new RewardAttendanceData();

			info.Id = entry.ReadInt("id");
			info.ClassName = entry.ReadString("className");
			info.DayOffset = entry.ReadInt("dayOffset");
			info.RewardCount = entry.ReadInt("rewardCount");
			info.RewardItemCnt_1 = entry.ReadInt("rewardItemCnt_1");
			info.AppendPropertyStatus = entry.ReadInt("appendPropertyStatus");
			info.RewardItem_1 = entry.ReadString("rewardItem_1");
			info.GroupName = entry.ReadString("groupName");
			info.Grade = entry.ReadString("grade");
			info.AppendPropertyName = entry.ReadString("appendPropertyName");
			
			this.Entries.Add(info.Id, info);
		}
	}
}
