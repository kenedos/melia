using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Scales Flowering: Swift bonuses with STR, DEX, and the caster's Flowering skill level.
	/// </summary>
	public static class BlossomBladerFloweringSwiftHelper
	{
		private const float StrengthPerDamagePercent = 45f;
		private const float DexterityPerCriticalChance = 15f;

		public static void Apply(ICombatEntity caster, ICombatEntity target, Skill skill, SkillModifier modifier)
		{
			if (caster is not Character character || target == null || target.IsDead || modifier == null)
				return;

			if (!character.TryGetActiveAbility(AbilityId.Blossomblader2, out _))
				return;

			if (!BlossomBladerStartUpHelper.IsUsingOneHandSword(character))
				return;

			if (!BlossomBladerStartUpHelper.IsBlossomBladerAttack(skill))
				return;

			if (!target.TryGetBuff(BuffId.Flowering_Debuff, out var floweringDebuff)
				|| floweringDebuff.Caster != character)
			{
				return;
			}

			if (!character.TryGetSkill(SkillId.BlossomBlader_Flowering, out var floweringSkill) || floweringSkill.Level <= 0)
				return;

			var strength = Math.Max(0f, character.Properties.GetFloat(PropertyName.STR));
			var dexterity = Math.Max(0f, character.Properties.GetFloat(PropertyName.DEX));
			var damageBonusPercent = (float)Math.Floor(strength / StrengthPerDamagePercent) * floweringSkill.Level;
			var criticalChanceBonus = (float)Math.Floor(dexterity / DexterityPerCriticalChance) * floweringSkill.Level;

			modifier.DamageMultiplier *= 1f + damageBonusPercent / 100f;
			modifier.BonusCritChance += criticalChanceBonus;
		}
	}
}
