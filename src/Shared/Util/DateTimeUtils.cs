using System;
using Melia.Shared.Network;

namespace Melia.Shared.Util
{
	public static class DateTimeUtils
	{
		public static string ToSDateTimeNow => DateTime.Now.ToDateDayOfWeekTime();
		public static string ToSPropertyDTNow => DateTime.Now.ToPropertyDateTimeString();

		/// <summary>
		/// Converts a string in DateTime format (yyyy-MM-dd HH:mm:ss)
		/// to a DateTime.
		/// </summary>
		/// <param name="dateString"></param>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static bool TryGetDateTime(this string dateString, out DateTime dateTime)
			=> DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime);

		/// <summary>
		/// Converts a string in DateTime format (yyyyMM{DayOfWeek}ddHH:mm:ss)
		/// to a DateTime.
		/// </summary>
		/// <param name="dateString"></param>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static bool TryGetPropertyStringToDateTime(this string dateString, out DateTime dateTime)
		{
			var fixedDateFormat = string.Concat(dateString.AsSpan(0, 6), dateString.AsSpan(7));
			if (!DateTime.TryParseExact(fixedDateFormat, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTime))
				dateTime = DateTime.MinValue;

			return dateTime != DateTime.MinValue;
		}

		/// <summary>
		/// Returns date time in a year(yyyy), month(MM), date (dd),
		/// day of week(0-6), hour (HH), minutes (mm) and seconds (ss)
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToDateDayOfWeekTime(this DateTime dateTime)
		{
			var date = dateTime.ToString("yyyyMMdd");
			var time = dateTime.ToString("HHmmss");

			return date + $"{(int)dateTime.DayOfWeek}" + time;
		}

		/// <summary>
		/// Returns date time in a yyyy-MM-dd HH:mm:ss format.
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToDateTimeString(this DateTime dateTime)
			=> dateTime.ToString("yyyy-MM-dd HH:mm:ss");
		/// <summary>
		/// Returns date time in a year(yyyy), month(MM), day of week(0-6),
		/// date (dd), hour (HH), minutes (mm) and seconds (ss)
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToPropertyDateTimeString(this DateTime dateTime)
		{
			var yearMonth = dateTime.ToString("yyyyMM");
			var date = dateTime.ToString("dd");
			var time = dateTime.ToString("HHmmss");

			return yearMonth + $"{(int)dateTime.DayOfWeek}" + date + time;
		}

		/// <summary>
		/// Convert DateTime to a format for yyyyMMdd for property usage.
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ToPropertyDateString(this DateTime dateTime) => dateTime.ToString("yyyyMMdd");

		/// <summary>
		/// Convert DateTime to a format for yyyyMMdd for property usage.
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static float ToFloat(this DateTime dateTime) => float.Parse(ToPropertyDateString(dateTime));

		/// <summary>
		/// Converts DateTime to an int in the format of yyyyMMdd.
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static int ToInt32(this DateTime dateTime) => dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;

		/// <summary>
		/// Returns DateTime as 32-bit unix timestamp.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static int ToUnixTimeSeconds(this DateTime dt)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (int)(dt.ToUniversalTime() - epoch).TotalSeconds;
		}
	}
}
