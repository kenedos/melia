using System;
using MySqlConnector;

namespace Melia.Shared.Database
{
	/// <summary>
	/// Extensions for the MySqlDataReader.
	/// </summary>
	public static class MySqlDataReaderExtension
	{
		/// <summary>
		/// Returns true if value at index is null.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private static bool IsDBNull(this MySqlDataReader reader, string index)
		{
			return reader.IsDBNull(reader.GetOrdinal(index));
		}

		/// <summary>
		/// Same as GetString, except for a is null check. Returns null if NULL.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string GetStringSafe(this MySqlDataReader reader, string index)
		{
			if (IsDBNull(reader, index))
				return null;
			else
				return reader.GetString(index);
		}

		/// <summary>
		/// Returns DateTime of the index, or DateTime.MinValue, if value is null.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static DateTime GetDateTimeSafe(this MySqlDataReader reader, string index)
		{
			return reader[index] as DateTime? ?? DateTime.MinValue;
		}

		/// <summary>
		/// Same as GetInt32, except for a null check. Returns defaultValue if NULL.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="index"></param>
		/// <param name="defaultValue">Value to return if the column is null</param>
		/// <returns></returns>
		public static int GetInt32Safe(this MySqlDataReader reader, string index, int defaultValue = 0)
		{
			if (IsDBNull(reader, index))
				return defaultValue;
			else
				return reader.GetInt32(index);
		}
	}

	/// <summary>
	/// Extensions for MySqlCommand.
	/// </summary>
	public static class MySqlCommandExtensions
	{
		/// <summary>
		/// Shortcut for Parameters.AddWithValue, for consistency with
		/// the simplified commands.
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public static void AddParameter(this MySqlCommand cmd, string name, object value)
		{
			cmd.Parameters.AddWithValue(name, value);
		}
	}
}
