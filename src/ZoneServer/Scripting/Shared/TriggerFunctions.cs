using System;
using System.Collections.Generic;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Scripting.Shared
{
	public class TriggerFunctions
	{
		protected Dictionary<string, TriggerActorFuncAsync> _functions = new Dictionary<string, TriggerActorFuncAsync>();

		public int Count => _functions.Count;

		/// <summary>
		/// Loads methods with the TriggerFunctionAttribute inside this class.
		/// </summary>
		public void LoadMethods()
		{
			foreach (var method in typeof(TriggerFunctions).GetMethods())
			{
				foreach (TriggerFunctionAttribute attr in method.GetCustomAttributes(typeof(TriggerFunctionAttribute), false))
				{
					var func = (TriggerActorFuncAsync)Delegate.CreateDelegate(typeof(TriggerActorFuncAsync), method);
					if (attr.DialogIds?.Length > 0)
						foreach (var dialog in attr.DialogIds)
							this.Add(dialog, func);
					else
						this.Add(method.Name, func);
				}
			}
			foreach (var method in typeof(Triggers).GetMethods())
			{
				foreach (TriggerFunctionAttribute attr in method.GetCustomAttributes(typeof(TriggerFunctionAttribute), false))
				{
					var func = (TriggerActorFuncAsync)Delegate.CreateDelegate(typeof(TriggerActorFuncAsync), method);
					if (attr.DialogIds?.Length > 0)
						foreach (var dialog in attr.DialogIds)
							this.Add(dialog, func);
					else
						this.Add(method.Name, func);
				}
			}
		}

		/// <summary>
		/// Sets handler for the given dialog name.
		/// </summary>
		/// <param name="dialogName"></param>
		/// <param name="func"></param>
		public void Add(string dialogName, TriggerActorFuncAsync func)
		{
			_functions[dialogName] = func;
		}

		public bool TryGet(string dialogName, out TriggerActorFuncAsync func)
		{
			return _functions.TryGetValue(dialogName, out func);
		}
	}

	/// <summary>
	/// Specifies which triggers a TriggerFunction should be used for.
	/// </summary>
	public class TriggerFunctionAttribute : Attribute
	{
		/// <summary>
		/// Returns list of ids of dialogs the script should be used for.
		/// </summary>
		public string[] DialogIds { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="dialogIds"></param>
		public TriggerFunctionAttribute(params string[] dialogIds)
		{
			this.DialogIds = dialogIds;
		}
	}
}
