using System;
using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Yggdrasil.Logging;

namespace Melia.Zone.World
{
	/// <summary>
	/// Manages the world's global variables.
	/// </summary>
	public class GlobalVariables
	{
		private bool _initialized = false;
		private readonly VariablesContainer _variables = new();
		private readonly string _tableName;
		private readonly string _ownerField;
		private readonly long _ownerId;

		public GlobalVariables(string tableName = "vars_global", string ownerField = null, long ownerId = 0)
		{
			_tableName = tableName;
			this._ownerField = ownerField;
			this._ownerId = ownerId;
		}

		/// <summary>
		/// Returns the variable containers.
		/// </summary>
		public VariablesContainer Variables
		{
			get
			{
				if (!_initialized)
					throw new InvalidOperationException("The global variables were not yet initialized.");

				return _variables;
			}
		}

		/// <summary>
		/// Initializes the global variables.
		/// </summary>
		internal void Init()
		{
			if (_initialized)
				throw new InvalidOperationException("The global variables were initialized already.");

			ZoneServer.Instance.Database.LoadVars(_variables.Perm, _tableName, _ownerField, _ownerId);
			ZoneServer.Instance.ServerEvents.MinuteTick.Subscribe(this.OnMinuteTick);

			_initialized = true;
		}

		/// <summary>
		/// Saves the permanent global variables to the database in regular
		/// intervals.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMinuteTick(object? sender, TimeEventArgs e)
		{
			if (!_initialized)
				return;

			// TODO: Technically we could check if any variables actually
			//   changed and only save if necessary, but I'm not gonna
			//   bother with that for now.

			ZoneServer.Instance.Database.SaveVariables(this.Variables.Perm, _tableName, _ownerField, _ownerId);

			// Disabled due to spam.
			// Log.Info($"Global {this.Variables.Perm.Count} variables saved to database.");
		}
	}
}
