using Melia.Shared.Game.Const;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers.Wizards.RuneCaster
{
	public static class RuneCasterFriendlyHelper
	{
		private const float DamageBonus = 0.40f;

		public static void Apply(ICombatEntity caster, Skill skill, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.RuneCaster19, out var ability) || !ability.Active)
				return;

			if (!HasValidWeapon(character))
				return;

			if (!IsRuneCasterSkill(skill.Id))
				return;

			if (!IsValidAttribute(skill.Data.Attribute))
				return;

			skillHitResult.Damage *= 1f + DamageBonus;
		}

		private static bool HasValidWeapon(Character character)
		{
			character.TryGetEquipItem(EquipSlot.RightHand, out var weapon);

			if (weapon == null)
				return false;

			return weapon.Data.EquipType1 == EquipType.Staff;
		}

		private static bool IsRuneCasterSkill(SkillId skillId)
		{
			return
				skillId == SkillId.RuneCaster_Hagalaz ||
				skillId == SkillId.RuneCaster_Isa ||
				skillId == SkillId.RuneCaster_Thurisaz ||
				skillId == SkillId.RuneCaster_Tiwaz ||
				skillId == SkillId.RuneCaster_Stan ||
				skillId == SkillId.RuneCaster_Ehwaz;
		}

		private static bool IsValidAttribute(AttributeType attribute)
		{
			return
				attribute == AttributeType.Melee ||
				attribute == AttributeType.Soul ||
				attribute == AttributeType.Earth;
		}
	}
}
