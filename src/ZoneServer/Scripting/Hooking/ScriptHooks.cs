using System;
using System.Collections.Generic;
using System.Linq;

namespace Melia.Zone.Scripting.Hooking
{
	/// <summary>
	/// Global script hook registry.
	/// </summary>
	public static class ScriptHooks
	{
		private readonly static Dictionary<string, List<IScriptHook>> Hooks = new();

		/// <summary>
		/// Registers a new hook.
		/// </summary>
		/// <typeparam name="THook"></typeparam>
		/// <param name="hook"></param>
		public static void Register<THook>(THook hook) where THook : IScriptHook
		{
			var ident = GetHookIdent(hook.OwnerName, hook.HookName);

			lock (Hooks)
			{
				if (!Hooks.TryGetValue(ident, out var hookList))
					Hooks[ident] = hookList = new List<IScriptHook>();

				hookList.Add(hook);
			}
		}

		/// <summary>
		/// Returns all hooks for the given owner and name.
		/// </summary>
		/// <typeparam name="THook"></typeparam>
		/// <param name="ownerName"></param>
		/// <param name="hookName"></param>
		/// <returns></returns>
		public static THook[] GetAll<THook>(string ownerName, string hookName) where THook : IScriptHook
		{
			var ident = GetHookIdent(ownerName, hookName);

			lock (Hooks)
			{
				if (!Hooks.TryGetValue(ident, out var hookList))
					return [];

				return hookList.OfType<THook>().ToArray();
			}
		}

		/// <summary>
		/// Removes all registered hooks.
		/// </summary>
		public static void Clear()
		{
			lock (Hooks)
				Hooks.Clear();
		}

		/// <summary>
		/// Returns a string identifier for the given values.
		/// </summary>
		/// <param name="ownerName"></param>
		/// <param name="hookName"></param>
		/// <returns></returns>
		internal static string GetHookIdent(string ownerName, string hookName)
			=> ownerName + "::" + hookName;
	}

	/// <summary>
	/// Specifies which items an DialogHook should be used for.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class DialogHookAttribute : Attribute
	{
		/// <summary>
		/// Returns list of ids of dialogs the script should be used for.
		/// </summary>
		public string[] DialogHooks { get; }

		/// <summary>
		/// Creates new attribute that uses the name of the method it's
		/// on as the script function name.
		/// </summary>
		public DialogHookAttribute()
		{
			// Getting the method name actually happens in the dialog
			// function loading code, see DialogHooks.
		}

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="dialogHooks"></param>
		public DialogHookAttribute(params string[] dialogHooks)
		{
			this.DialogHooks = dialogHooks;
		}
	}
}
