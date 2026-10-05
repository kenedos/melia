using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Contains shared StartUp and weapon-specialization checks used by Blossom Blader skills.
	/// </summary>
	public static class BlossomBladerStartUpHelper
	{
		public static bool IsActive(ICombatEntity caster)
		{
			return caster != null
				&& !caster.IsDead
				&& IsUsingOneHandSword(caster)
				&& caster.IsBuffActive(BuffId.StartUp_Buff);
		}

		public static bool IsUsingOneHandSword(ICombatEntity caster)
		{
			return caster is Character character
				&& IsOneHandSword(character)
				&& IsSubWeaponEmpty(character);
		}

		/// <summary>
		/// Garante que o slot de arma secundária/off-hand esteja totalmente limpo.
		/// </summary>
		public static bool IsSubWeaponEmpty(Character character)
		{
			if (character == null)
				return false;

			// Verifica o slot de mão esquerda padrão (LeftHand), o Set 2 (LeftHandSub) e Trinket
			if (HasItemEquipped(character, EquipSlot.LeftHand) ||
				HasItemEquipped(character, EquipSlot.LeftHandSub) ||
				HasItemEquipped(character, EquipSlot.Trinket))
			{
				return false;
			}

			return true;
		}

		private static bool HasItemEquipped(Character character, EquipSlot slot)
		{
			if (!character.TryGetEquipItem(slot, out var item) || item == null || item.Data == null)
				return false;

			// Ignora os itens dummy/padrão que o servidor coloca em slots vazios (NoWeapon/NoItem: ID 9999996, 4, 9, etc.)
			if (item.Data.Id == 9999996 || item.Data.Id == 4 || item.Data.Id == 9 || item.Data.Id <= 0)
				return false;

			return true;
		}

		public static bool IsBlossomBladerAttack(Skill skill)
		{
			return skill != null
				&& skill.Id != SkillId.BlossomBlader_StartUp
				&& skill.Id.ToString().StartsWith("BlossomBlader_", StringComparison.Ordinal);
		}

		public static bool IsBlossomShowerActive(ICombatEntity caster)
		{
			return IsActive(caster)
				&& caster.TryGetActiveAbility(AbilityId.Blossomblader20, out _);
		}

		public static int GetAdditionalHitCount(ICombatEntity caster)
		{
			return IsBlossomShowerActive(caster) ? 1 : 0;
		}

		private static bool IsOneHandSword(Character character)
		{
			// Checa se a espada está equipada na mão direita (Set 1 ou Set 2)
			if (character.TryGetEquipItem(EquipSlot.RightHand, out var weapon) && weapon?.Data?.EquipType1 == EquipType.Sword)
				return true;

			if (character.TryGetEquipItem(EquipSlot.RightHandSub, out var subWeapon) && subWeapon?.Data?.EquipType1 == EquipType.Sword)
				return true;

			return false;
		}
	}
}
