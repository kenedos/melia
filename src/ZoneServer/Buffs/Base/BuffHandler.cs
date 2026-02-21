using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Newtonsoft.Json.Linq;
using Yggdrasil.Logging;

namespace Melia.Zone.Buffs.Base
{
	/// <summary>
	/// Base class for buff handlers.
	/// </summary>
	public abstract class BuffHandler : IBuffHandler
	{
		/// <summary>
		/// Prefix used for storing property modifiers in buff Vars.
		/// </summary>
		public const string ModifierVarPrefix = "Melia.Modifier.";

		/// <summary>
		/// Initializes buff handler.
		/// </summary>
		public BuffHandler()
		{
			ScriptableFunctions.Load(this);
		}

		/// <summary>
		/// Callback for when the buff is started or overbuffed. Not called
		/// once the max overbuff count is reached.
		/// </summary>
		/// <param name="buff"></param>
		[Obsolete("Use OnActivate instead.")]
		public virtual void OnStart(Buff buff)
		{
		}

		/// <summary>
		/// Callback for when the buff is activated, either by starting or
		/// overbuffing it. Not called once the max overbuff count is reached.
		/// </summary>
		/// <remarks>
		/// This callback is usually the right choice for most buffs that
		/// apply a simple bonus that stacks up until the max overbuff count
		/// is reached.
		/// </remarks>
		/// <param name="buff"></param>
		/// <param name="activationType"></param>
		public virtual void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		/// <summary>
		/// Callback for when the buff's duration is extended, regardless of
		/// whether the overbuff max count was reached or not.
		/// </summary>
		/// <remarks>
		/// This callback presents an alternative to OnActivate, in case it's
		/// ever necessary for the handler to react to continued extensions
		/// after the max overbuff count was reached.
		/// 
		/// OnExtend is called in addition to OnActivate up until the max
		/// overbuff count is reached. Afterwards, OnExtend is the only
		/// callback that is called.
		/// </remarks>
		/// <param name="buff"></param>
		public virtual void OnExtend(Buff buff)
		{
		}

		/// <summary>
		/// Callback for regular updates while the buff is active. Only called
		/// for buffs that have an update time.
		/// </summary>
		/// <param name="buff"></param>
		public virtual void WhileActive(Buff buff)
		{
		}

		/// <summary>
		/// Callback for when the buff runs out or is manually stopped.
		/// </summary>
		/// <param name="buff"></param>
		public virtual void OnEnd(Buff buff)
		{
		}

		/// <summary>
		/// Returns the name of the variable used to store modifiers for
		/// the given property.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		private static string GetModifierVarName(string propertyName)
			=> ModifierVarPrefix + propertyName;

		/// <summary>
		/// Returns true if the property is a transient buff modifier that should
		/// not be persisted to the database. This prevents desync issues where
		/// buff modifiers could persist even after the buff expires.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static bool IsBuffTransientProperty(string propertyName)
			=> propertyName.EndsWith("_BM");

		/// <summary>
		/// Modifies the property on the target and saves the value in the buff,
		/// to be able to later undo the change.
		/// </summary>
		/// <remarks>
		/// Repeated calls to this method will stack the modifications, while
		/// one call to RemovePropertyModifier will undo all of them.
		/// </remarks>
		/// <param name="buff"></param>
		/// <param name="target"></param>
		/// <param name="modifierName"></param>
		/// <param name="value"></param>
		protected static void AddModifier(Buff buff, ICombatEntity target, string modifierName, float value)
		{
			var varName = GetModifierVarName(modifierName);

			if (buff.Vars.TryGetFloat(varName, out var oldValue))
				value += oldValue;

			buff.Vars.SetFloat(varName, value);
		}

		/// <summary>
		/// Undoes the modifications done to the property on target from
		/// ApplyModifier.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="target"></param>
		/// <param name="modifierName"></param>
		protected static void RemoveModifier(Buff buff, ICombatEntity target, string modifierName)
		{
			var varName = GetModifierVarName(modifierName);

			if (buff.Vars.TryGetFloat(varName, out var value))
			{
				buff.Vars.Remove(varName);
			}
		}

		/// <summary>
		/// Modifies the property on the target and saves the value in the buff,
		/// to be able to later undo the change.
		/// </summary>
		/// <remarks>
		/// Repeated calls to this method will stack the modifications, while
		/// one call to RemovePropertyModifier will undo all of them.
		/// </remarks>
		/// <param name="buff"></param>
		/// <param name="target"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		protected static void AddPropertyModifier(Buff buff, ICombatEntity target, string propertyName, float value)
		{
			if (!PropertyTable.Exists(target.Properties.Namespace, propertyName))
			{
				Log.Warning($"AddPropertyModifier: {buff.Id} tried to add to property {propertyName} but doesn't exist in id namespace: {target.Properties.Namespace}.");
				return;
			}

			var varName = GetModifierVarName(propertyName);

			if (buff.Vars.TryGetFloat(varName, out var oldValue))
				value += oldValue;

			buff.Vars.SetFloat(varName, value);
			target.Properties.Modify(propertyName, value - oldValue);
		}

		/// <summary>
		/// Undoes the modifications done to the property on target from
		/// ApplyPropertyModifier.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="target"></param>
		/// <param name="propertyName"></param>
		protected static void RemovePropertyModifier(Buff buff, ICombatEntity target, string propertyName)
		{
			if (!PropertyTable.Exists(target.Properties.Namespace, propertyName))
			{
				Log.Warning($"RemovePropertyModifier: {buff.Id} tried to remove to property {propertyName} but doesn't exist in id namespace: {target.Properties.Namespace}.");
				return;
			}

			var varName = GetModifierVarName(propertyName);

			if (buff.Vars.TryGetFloat(varName, out var value))
			{
				target.Properties.Modify(propertyName, -value);
				buff.Vars.Remove(varName);
			}
		}

		/// <summary>
		/// Sets the value of a property modifier on the target.
		/// </summary>
		/// <remarks>
		/// Removes existing modifiers for the property and then applies the new one.
		/// This is essentially the same as calling RemovePropertyModifier followed
		/// by AddPropertyModifier.
		/// </remarks>
		/// <param name="buff"></param>
		/// <param name="target"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		protected static void UpdatePropertyModifier(Buff buff, ICombatEntity target, string propertyName, float value)
			=> SetPropertyModifier(buff, target, propertyName, value);

		/// <summary>
		/// Updates the value of a property modifier on the target.
		/// </summary>
		/// <remarks>
		/// Removes existing modifiers for the property and then applies the new one.
		/// This is essentially the same as calling RemovePropertyModifier followed
		/// by AddPropertyModifier.
		/// </remarks>
		/// <param name="buff"></param>
		/// <param name="target"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		protected static void SetPropertyModifier(Buff buff, ICombatEntity target, string propertyName, float value)
		{
			if (!PropertyTable.Exists(propertyName))
				return;

			RemovePropertyModifier(buff, target, propertyName);
			AddPropertyModifier(buff, target, propertyName, value);
		}
	}
}
