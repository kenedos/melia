using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class EventAttendanceData
	{
		public int Id { get; set; }
		public int TotalNumberDays { get; set; }
		public string ClassName { get; set; }
		public string Name { get; set; }
		public bool AttendancePass { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string PeriodType { get; set; }
	}

	/// <summary>
	/// Event Attendance database, used statue warps.
	/// </summary>
	public class EventAttendanceDb : DatabaseJsonIndexed<int, EventAttendanceData>
	{
		public bool TryFind(string className, out EventAttendanceData data)
		{
			data = this.Entries.Values.FirstOrDefault(a => a.ClassName == className);
			return data != null;
		}

		protected override void ReadEntry(JObject entry)
		{
			// { id: 1, totalNumberDays: 28, className: "VakarinePackage", name: "Vakarine Package", attendancePass: "NO", startTime: "None", endTime: "None", periodType: "None"},
			entry.AssertNotMissing("id", "totalNumberDays", "className", "name", "attendancePass", "startTime", "endTime", "periodType");

			var info = new EventAttendanceData();

			info.Id = entry.ReadInt("id");
			info.TotalNumberDays = entry.ReadInt("totalNumberDays");
			info.ClassName = entry.ReadString("className");
			info.Name = entry.ReadString("name");
			info.AttendancePass = entry.ReadString("attendancePass") == "YES" ? true : false;

			var startTimeString = entry.ReadString("startTime");
			if (startTimeString != "None" && DateTime.TryParseExact(startTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
			{
				info.StartTime = startTime;
			} else
			{
				info.StartTime = DateTime.MaxValue;
			}

			var endTimeString = entry.ReadString("endTime");
			if (endTimeString != "None" && DateTime.TryParseExact(endTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endTime))
			{
				info.EndTime = endTime;
			} else
			{
				info.EndTime = DateTime.MinValue;
			}

			info.PeriodType = entry.ReadString("periodType");
			
			this.Entries.Add(info.Id, info);
		}
	}
}
