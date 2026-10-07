using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Enchanter's party enchantments.
	/// </summary>
	public static class EnchanterSkillHelper
	{
		public const string AuraPadVar = "Melia.Enchanter.AuraPad";

		private static readonly TimeSpan AbsenceGrace = TimeSpan.FromSeconds(20);
		private const string AbsentSinceVar = "Melia.Enchanter.CasterAbsentSince";

		private static readonly string[] MinPAtkFlat = [PropertyName.PATK_BM, PropertyName.MINPATK_BM, PropertyName.PATK_MAIN_BM, PropertyName.MINPATK_MAIN_BM];
		private static readonly string[] MinPAtkRate = [PropertyName.PATK_RATE_BM, PropertyName.MINPATK_RATE_BM, PropertyName.PATK_MAIN_RATE_BM, PropertyName.MINPATK_MAIN_RATE_BM];
		private static readonly string[] MaxPAtkFlat = [PropertyName.PATK_BM, PropertyName.MAXPATK_BM, PropertyName.PATK_MAIN_BM, PropertyName.MAXPATK_MAIN_BM];
		private static readonly string[] MaxPAtkRate = [PropertyName.PATK_RATE_BM, PropertyName.MAXPATK_RATE_BM, PropertyName.PATK_MAIN_RATE_BM, PropertyName.MAXPATK_MAIN_RATE_BM];
		private static readonly string[] MaxMAtkFlat = [PropertyName.MATK_BM, PropertyName.MAXMATK_BM];
		private static readonly string[] MaxMAtkRate = [PropertyName.MATK_RATE_BM, PropertyName.MAXMATK_RATE_BM];

		/// <summary>
		/// Returns the caster and their living party members within range
		/// who wear an item in the given slot, or everyone in range if the
		/// slot is None.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="range"></param>
		/// <param name="requiredSlot"></param>
		/// <returns></returns>
		public static IEnumerable<ICombatEntity> GetPartyTargets(ICombatEntity caster, float range, EquipSlot requiredSlot = EquipSlot.None)
		{
			var allies = PartySkillHelper.GetAlliesInRange(caster, caster.Position, range).Where(ally => ally is Character);
			if (requiredSlot == EquipSlot.None)
				return allies;

			return allies.Where(ally => ally.TryGetEquipItem(requiredSlot, out var item) && item is not DummyEquipItem);
		}

		/// <summary>
		/// Returns the entity's average physical attack without the
		/// bonuses of buffs.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static float GetBasePAtk(ICombatEntity entity)
		{
			var min = GetUnbuffedValue(entity, PropertyName.MINPATK, MinPAtkFlat, MinPAtkRate);
			var max = GetUnbuffedValue(entity, PropertyName.MAXPATK, MaxPAtkFlat, MaxPAtkRate);

			return (min + max) / 2f;
		}

		/// <summary>
		/// Returns the share of an enchantment's effect the target receives,
		/// which drops when the caster's highest physical attack is below
		/// the target's highest attack, buffs excluded.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static float GetEfficiency(ICombatEntity caster, ICombatEntity target)
		{
			if (caster == null || caster == target)
				return 1;

			var casterAtk = GetUnbuffedValue(caster, PropertyName.MAXPATK, MaxPAtkFlat, MaxPAtkRate);
			var targetAtk = Math.Max(
				GetUnbuffedValue(target, PropertyName.MAXPATK, MaxPAtkFlat, MaxPAtkRate),
				GetUnbuffedValue(target, PropertyName.MAXMATK, MaxMAtkFlat, MaxMAtkRate));

			if (targetAtk <= 0 || casterAtk >= targetAtk)
				return 1;

			return casterAtk / targetAtk;
		}

		/// <summary>
		/// Ends the enchantment once its caster has been logged out or out
		/// of the target's party for 20 seconds. Called on the buff's tick.
		/// </summary>
		/// <param name="buff"></param>
		public static void CheckCasterPresence(Buff buff)
		{
			if (buff.Caster is not Character caster || buff.Target is not Character target || caster.ObjectId == target.ObjectId)
				return;

			if (IsCasterPresent(caster, target))
			{
				buff.Vars.Remove(AbsentSinceVar);
				return;
			}

			if (!buff.Vars.TryGet<DateTime>(AbsentSinceVar, out var absentSince))
			{
				buff.Vars.Set(AbsentSinceVar, DateTime.Now);
				return;
			}

			if (DateTime.Now - absentSince >= AbsenceGrace)
				target.StopBuff(buff.Id);
		}

		/// <summary>
		/// Returns true if the caster is online and in the target's party.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		private static bool IsCasterPresent(Character caster, Character target)
		{
			var current = ZoneServer.Instance.World.GetCharacter(caster.ObjectId);
			if (current == null || !current.IsOnline)
				return false;

			var party = target.Connection?.Party;
			return party != null && current.Connection?.Party?.ObjectId == party.ObjectId;
		}

		/// <summary>
		/// Returns the property's value with the given flat and rate buff
		/// bonuses taken back out.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="propertyName"></param>
		/// <param name="flatProperties"></param>
		/// <param name="rateProperties"></param>
		/// <returns></returns>
		private static float GetUnbuffedValue(ICombatEntity entity, string propertyName, string[] flatProperties, string[] rateProperties)
		{
			var properties = entity.Properties;

			var flat = flatProperties.Sum(name => properties.GetFloat(name));
			var rate = rateProperties.Sum(name => properties.GetFloat(name));

			return Math.Max(0, (properties.GetFloat(propertyName) - flat) / Math.Max(0.01f, 1 + rate));
		}
	}
}
