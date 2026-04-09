using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlConnector;

namespace Melia.Shared.Database
{
	public class BatchInsertCommand : IDisposable
	{
		private readonly MySqlConnection _conn;
		private readonly MySqlTransaction _trans;
		private readonly string _tableName;
		private readonly string _onDuplicateKeyUpdateSql;
		private readonly List<string> _columns = new();
		private readonly List<MySqlParameter> _parameters = new();
		private readonly List<string> _valueClauses = new();
		private int _paramIndex = 0;

		// Public properties to support extension methods
		public int RowCount => _valueClauses.Count;
		public bool HasRows => _valueClauses.Count > 0;

		public BatchInsertCommand(string tableName, string onDuplicateKeyUpdateSql, MySqlConnection conn, MySqlTransaction trans)
		{
			_tableName = tableName;
			_onDuplicateKeyUpdateSql = onDuplicateKeyUpdateSql;
			_conn = conn;
			_trans = trans;
		}

		public void AddRow(Dictionary<string, object> rowData)
		{
			var valueParams = new List<string>();
			foreach (var col in rowData.Keys)
			{
				if (_valueClauses.Count == 0 && !_columns.Contains(col))
				{
					_columns.Add(col);
				}

				var paramName = $"@p{_paramIndex++}";
				valueParams.Add(paramName);
				_parameters.Add(new MySqlParameter(paramName, rowData[col]));
			}
			_valueClauses.Add($"({string.Join(", ", valueParams)})");
		}

		public int Execute()
		{
			if (_valueClauses.Count == 0)
			{
				return 0;
			}

			var sql = new StringBuilder();
			sql.Append($"INSERT INTO `{_tableName}` ({string.Join(", ", _columns.Select(c => $"`{c}`"))}) VALUES ");
			sql.Append(string.Join(", ", _valueClauses));
			if (!string.IsNullOrEmpty(_onDuplicateKeyUpdateSql))
			{
				sql.Append($" {_onDuplicateKeyUpdateSql}");
			}

			using (var cmd = new MySqlCommand(sql.ToString(), _conn, _trans))
			{
				cmd.Parameters.AddRange(_parameters.ToArray());
				return cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Executes the batch insert and returns the first inserted ID.
		/// Note: MySQL only provides the first ID from batch inserts.
		/// </summary>
		/// <returns>The first inserted ID, or 0 if no rows were inserted.</returns>
		public long ExecuteAndGetFirstId()
		{
			if (_valueClauses.Count == 0)
			{
				return 0;
			}

			var sql = new StringBuilder();
			sql.Append($"INSERT INTO `{_tableName}` ({string.Join(", ", _columns.Select(c => $"`{c}`"))}) VALUES ");
			sql.Append(string.Join(", ", _valueClauses));
			if (!string.IsNullOrEmpty(_onDuplicateKeyUpdateSql))
			{
				sql.Append($" {_onDuplicateKeyUpdateSql}");
			}

			using (var cmd = new MySqlCommand(sql.ToString(), _conn, _trans))
			{
				cmd.Parameters.AddRange(_parameters.ToArray());
				var rowsAffected = cmd.ExecuteNonQuery();
				return rowsAffected > 0 ? cmd.LastInsertedId : 0;
			}
		}

		/// <summary>
		/// Clears all queued rows and resets the batch state.
		/// </summary>
		public void Clear()
		{
			_valueClauses.Clear();
			_parameters.Clear();
			_columns.Clear();
			_paramIndex = 0;
		}

		/// <summary>
		/// Implements IDisposable. The using statement will now be valid.
		/// This class doesn't own any unmanaged resources, so the method body can be empty.
		/// </summary>
		public void Dispose()
		{
			// No unmanaged resources to dispose of directly in this class.
			// The connection and transaction are managed by the calling code.
			// This method exists solely to satisfy the IDisposable interface
			// and allow the use of `using` blocks for good practice.
		}
	}

	#region Extension Methods
	/// <summary>
	/// Extension methods for BatchInsertCommand to improve usability.
	/// </summary>
	public static class BatchInsertCommandExtensions
	{
		/// <summary>
		/// Executes the batch only if it has rows to insert.
		/// </summary>
		/// <param name="batch">The BatchInsertCommand instance.</param>
		/// <returns>Number of rows affected, or 0 if no rows to insert.</returns>
		public static int ExecuteIfHasRows(this BatchInsertCommand batch)
		{
			return batch.HasRows ? batch.Execute() : 0;
		}

		/// <summary>
		/// Adds multiple rows to the batch from a collection.
		/// </summary>
		/// <param name="batch">The BatchInsertCommand instance.</param>
		/// <param name="rows">Collection of row data to add.</param>
		public static void AddRows(this BatchInsertCommand batch, IEnumerable<Dictionary<string, object>> rows)
		{
			foreach (var row in rows)
			{
				batch.AddRow(row);
			}
		}

		/// <summary>
		/// Executes the batch and then clears it for reuse.
		/// </summary>
		/// <param name="batch">The BatchInsertCommand instance.</param>
		/// <returns>Number of rows affected.</returns>
		public static int ExecuteAndClear(this BatchInsertCommand batch)
		{
			var result = batch.Execute();
			batch.Clear();
			return result;
		}
	}
	#endregion // Extension Methods
}
