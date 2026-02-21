using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlConnector;

namespace Melia.Shared.Database
{
	/// <summary>
	/// Base class for simplified MySQL commands.
	/// </summary>
	public abstract class SimpleCommand : IDisposable
	{
		protected MySqlCommand _mc;
		protected Dictionary<string, object> _set;
		protected MySqlTransaction _transaction;

		/// <summary>
		/// Initializes internal objects.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="conn"></param>
		/// <param name="trans"></param>
		protected SimpleCommand(string command, MySqlConnection conn, MySqlTransaction trans = null)
		{
			_transaction = trans;
			_mc = new MySqlCommand(command, conn, trans);
			_set = new Dictionary<string, object>();
		}

		/// <summary>
		/// Adds a parameter that's not handled by Set.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void AddParameter(string name, object value)
		{
			_mc.Parameters.AddWithValue(name, value);
		}

		/// <summary>
		/// Sets value for field.
		/// </summary>
		/// <param name="field"></param>
		/// <param name="value"></param>
		public void Set(string field, object value)
		{
			_set[field] = value;
		}

		/// <summary>
		/// Executes command.
		/// </summary>
		/// <returns></returns>
		public abstract int Execute();

		/// <summary>
		/// Disposes internal, wrapped objects.
		/// </summary>
		public void Dispose()
		{
			_mc.Dispose();
		}
	}

	/// <summary>
	/// Wrapper around MySqlCommand, for easier, cleaner updating.
	/// </summary>
	/// <remarks>
	/// Include one {0} where the set statements normally would be.
	/// It's automatically inserted, based on what was passed to "Set".
	/// </remarks>
	/// <example>
	/// <code>
	/// using (var conn = db.Instance.Connection)
	/// using (var cmd = new UpdateCommand("UPDATE `accounts` SET {0} WHERE `accountId` = @accountId", conn))
	/// {
	/// 	cmd.Parameters.AddWithValue("@accountId", account.Id);
	/// 	cmd.Set("authority", (byte)account.Authority);
	/// 	cmd.Set("lastlogin", account.LastLogin);
	/// 	cmd.Set("banReason", account.BanReason);
	/// 	cmd.Set("banExpiration", account.BanExpiration);
	/// 
	/// 	cmd.Execute();
	/// }
	/// </code>
	/// </example>
	public class UpdateCommand : SimpleCommand
	{
		/// <summary>
		/// Creates new update command.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="conn"></param>
		/// <param name="trans"></param>
		public UpdateCommand(string command, MySqlConnection conn, MySqlTransaction trans = null)
			: base(command, conn, trans)
		{
		}

		/// <summary>
		/// Runs MySqlCommand.ExecuteNonQuery
		/// </summary>
		/// <returns></returns>
		public override int Execute()
		{
			// Build setting part
			var sb = new StringBuilder();
			foreach (var parameter in _set.Keys)
				sb.AppendFormat("`{0}` = @{0}, ", parameter);

			// Add setting part
			_mc.CommandText = string.Format(_mc.CommandText, sb.ToString().Trim(' ', ','));

			// Add parameters
			foreach (var parameter in _set)
				_mc.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);

			_mc.Transaction = this._transaction;

			// Run
			return _mc.ExecuteNonQuery();
		}
	}

	/// <summary>
	/// Wrapper around MySqlCommand, for easier, cleaner inserting.
	/// </summary>
	/// <remarks>
	/// Include one {0} where the "(...) VALUES (...) part normally would be.
	/// It's automatically inserted, based on what was passed to "Set".
	/// </remarks>
	/// <example>
	/// <code>
	/// using (var cmd = new InsertCommand("INSERT INTO `keywords` {0}", conn, transaction))
	/// {
	/// 	cmd.Set("creatureId", creature.CreatureId);
	/// 	cmd.Set("keywordId", keywordId);
	/// 
	/// 	cmd.Execute();
	/// }
	/// </code>
	/// </example>
	public class InsertCommand : SimpleCommand
	{
		/// <summary>
		/// Returns last insert id.
		/// </summary>
		public long LastId { get { return _mc.LastInsertedId; } }

		/// <summary>
		/// Creates new insert command.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="conn"></param>
		/// <param name="transaction"></param>
		public InsertCommand(string command, MySqlConnection conn, MySqlTransaction transaction = null)
			: base(command, conn, transaction)
		{
		}

		/// <summary>
		/// Runs MySqlCommand.ExecuteNonQuery
		/// </summary>
		/// <returns></returns>
		public override int Execute()
		{
			// Build values part
			var sb1 = new StringBuilder();
			var sb2 = new StringBuilder();
			foreach (var parameter in _set.Keys)
			{
				sb1.AppendFormat("`{0}`, ", parameter);
				sb2.AppendFormat("@{0}, ", parameter);
			}

			// Add values part
			var values = "(" + (sb1.ToString().Trim(' ', ',')) + ") VALUES (" + (sb2.ToString().Trim(' ', ',')) + ")";
			_mc.CommandText = string.Format(_mc.CommandText, values);

			// Add parameters
			foreach (var parameter in _set)
				_mc.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);

			_mc.Transaction = this._transaction;

			// Run
			return _mc.ExecuteNonQuery();
		}
	}

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
