using System;
using System.Collections.Concurrent;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs
{
	/// <summary>
	/// Cloaking that only hides a buff's bearer from the observers its
	/// callback accepts.
	/// </summary>
	public static class ConditionalCloaking
	{
		private static readonly ConcurrentDictionary<BuffId, Func<ICombatEntity, ICombatEntity, bool>> Conditions = new();

		/// <summary>
		/// Makes the buff hide its bearer from every observer for which the
		/// callback, given the observer and the bearer, returns true.
		/// </summary>
		/// <param name="buffId"></param>
		/// <param name="condition"></param>
		public static void Register(BuffId buffId, Func<ICombatEntity, ICombatEntity, bool> condition)
		{
			Conditions[buffId] = condition;
		}

		/// <summary>
		/// Returns true if the target wears a conditional cloaking buff that
		/// hides it from the observer.
		/// </summary>
		/// <param name="observer"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsHiddenFrom(ICombatEntity observer, ICombatEntity target)
		{
			foreach (var (buffId, condition) in Conditions)
			{
				if (target.IsBuffActive(buffId) && condition(observer, target))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns true if the buff is a conditional cloaking buff.
		/// </summary>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public static bool IsConditionalCloak(BuffId buffId)
		{
			return Conditions.ContainsKey(buffId);
		}

		/// <summary>
		/// Returns true if the target wears any conditional cloaking buff,
		/// regardless of who is looking.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsActive(ICombatEntity target)
		{
			foreach (var buffId in Conditions.Keys)
			{
				if (target.IsBuffActive(buffId))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Removes every conditional cloaking buff from the target.
		/// </summary>
		/// <param name="target"></param>
		public static void Break(ICombatEntity target)
		{
			foreach (var buffId in Conditions.Keys)
				target.StopBuff(buffId);
		}
	}
}
