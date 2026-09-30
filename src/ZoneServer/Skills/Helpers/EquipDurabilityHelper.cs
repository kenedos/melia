using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Wears down equipment durability as a result of combat hits.
	/// </summary>
	public static class EquipDurabilityHelper
	{
		private const int WearChance = 10;
		private const int MinimumWear = 1;
		private const int MaximumWear = 100;
		private const int ShieldBlockWear = 1;
		private const float WeaponWearMultiplier = 1f;
		private const float ArmorWearMultiplier = 1f;

		/// <summary>
		/// Wears the target's armor for a hit it took, weighting each piece
		/// by its share of the equipment defense.
		/// </summary>
		/// <remarks>
		/// A piece's potential wear is its share of the damage minus its own
		/// defense, never below the minimum, and lands on a per-piece roll.
		/// A block always costs the shield the minimum. Dodges, DoTs and
		/// direct damage never reach this method.
		/// </remarks>
		/// <param name="target"></param>
		/// <param name="result"></param>
		/// <param name="isMagic">Whether the hit is magic, which uses MDEF instead of DEF.</param>
		/// <param name="damage">The final damage the hit deals, 0 for a nullified block.</param>
		public static void WearOnHitTaken(ICombatEntity target, HitResultType result, bool isMagic, float damage)
		{
			if (target is not Character character || character is DummyCharacter)
				return;

			if (result is not (HitResultType.Hit or HitResultType.Crit or HitResultType.Block))
				return;

			if (IsWearExempt(character))
				return;

			var defProperty = isMagic ? PropertyName.MDEF : PropertyName.DEF;
			var addDefProperty = isMagic ? PropertyName.ADD_MDEF : PropertyName.ADD_DEF;

			var pieces = new List<(Item Item, float Def)>();
			var totalDef = 0f;

			foreach (var (slot, item) in character.Inventory.GetEquip())
			{
				if (!IsArmorPiece(slot, item))
					continue;

				var def = item.Properties.GetFloat(defProperty, 0) + item.Properties.GetFloat(addDefProperty, 0);
				pieces.Add((item, def));
				totalDef += def;
			}

			var isBlock = result == HitResultType.Block;
			var rnd = GameRandom.Get();

			foreach (var (item, def) in pieces)
			{
				var wear = 0;

				if (damage > 0 && rnd.Next(100) < WearChance)
				{
					var share = totalDef > 0 ? damage * def / totalDef : damage / pieces.Count;
					wear = Math.Clamp((int)((share - def) * ArmorWearMultiplier), MinimumWear, MaximumWear);
				}

				if (isBlock && item.Data.EquipType1 == EquipType.Shield)
					wear = Math.Max(wear, ShieldBlockWear);

				item.WearDurability(character, wear);
			}
		}

		/// <summary>
		/// Wears the attacker's weapons for a hit that connected.
		/// </summary>
		/// <remarks>
		/// Each weapon rolls individually and takes the base wear scaled by the
		/// target's defense over the damage dealt, so heavy hits and crits wear
		/// less than many weak ones. Misses and dodges never reach this method.
		/// </remarks>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="result"></param>
		/// <param name="isMagic">Whether the hit is magic, which uses MDEF instead of DEF.</param>
		/// <param name="damage">The final damage the hit deals, 0 for a nullified block.</param>
		public static void WearOnHitDealt(ICombatEntity attacker, ICombatEntity target, HitResultType result, bool isMagic, float damage)
		{
			if (attacker is not Character character || character is DummyCharacter)
				return;

			if (result is not (HitResultType.Hit or HitResultType.Crit or HitResultType.Block))
				return;

			if (IsWearExempt(character))
				return;

			var targetDef = target.Properties.GetFloat(isMagic ? PropertyName.MDEF : PropertyName.DEF);

			var wear = damage > 0
				? Math.Clamp((int)(WeaponWearMultiplier * targetDef / damage), MinimumWear, MaximumWear)
				: MinimumWear;

			var rnd = GameRandom.Get();

			foreach (var (slot, item) in character.Inventory.GetEquip())
			{
				if (!IsWeapon(slot, item))
					continue;

				if (rnd.Next(100) >= WearChance)
					continue;

				item.WearDurability(character, wear);
			}
		}

		/// <summary>
		/// Returns whether the character is somewhere equipment must not
		/// lose durability, such as a duel, a city or a PvP map.
		/// </summary>
		/// <param name="character"></param>
		public static bool IsWearExempt(Character character)
		{
			var map = character.Map;

			return map.IsGTW || map.IsCity || map.IsPVP || character.IsDueling;
		}

		private static bool IsArmorPiece(EquipSlot slot, Item item)
		{
			if (!IsWearable(item))
				return false;

			if (slot is EquipSlot.RightHand or EquipSlot.RightHandSub or EquipSlot.LeftHandSub)
				return false;

			return !IsWeaponType(item);
		}

		private static bool IsWeapon(EquipSlot slot, Item item)
		{
			if (!IsWearable(item))
				return false;

			if (slot is EquipSlot.RightHandSub or EquipSlot.LeftHandSub)
				return false;

			return IsWeaponType(item);
		}

		private static bool IsWearable(Item item)
			=> item is not DummyEquipItem && item.MaxDurability > 0 && item.Durability > 0;

		private static bool IsWeaponType(Item item)
		{
			var equipType = item.Data.EquipType1;

			return item.Data.IsWeapon() || equipType == EquipType.Dagger || equipType == EquipType.Trinket;
		}
	}
}
