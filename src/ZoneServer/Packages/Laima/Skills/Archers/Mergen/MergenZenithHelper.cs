using System;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Archers.Mergen
{
	public static class MergenZenithHelper
	{
		private const int MaximumZenithLevel = 10;
		private const int MinimumAoeTargetLimit = 11;
		private const int MaximumAoeTargetLimit = 16;
		private const float FireForEffectBaseBonusPercent = 6f;
		private const float FireForEffectBonusPerLevelPercent = 2f;

		public static bool TryGetLevel(ICombatEntity caster, out int level)
		{
			level = 0;

			if (caster is not Character character)
				return false;

			if (!character.TryGetSkill(SkillId.Mergen_Zenith, out var skill))
				return false;

			if (skill.Level <= 0)
				return false;

			level = Math.Clamp(skill.Level, 1, MaximumZenithLevel);
			return true;
		}

		public static bool IsFireForEffectActive(ICombatEntity caster)
		{
			return caster is Character character &&
				character.IsAbilityActive(AbilityId.Mergen26);
		}

		public static int GetMaximumAoeTargets(ICombatEntity caster, int originalMaximum)
		{
			if (!TryGetLevel(caster, out var zenithLevel))
				return originalMaximum;

			if (IsFireForEffectActive(caster))
				return originalMaximum;

			var zenithMaximum =
				MinimumAoeTargetLimit +
				(int)MathF.Round(
					(MaximumAoeTargetLimit - MinimumAoeTargetLimit) *
					(zenithLevel - 1) /
					(float)(MaximumZenithLevel - 1)
				);

			return Math.Max(originalMaximum, zenithMaximum);
		}

		public static float GetDamageMultiplier(ICombatEntity caster)
		{
			if (!TryGetLevel(caster, out var zenithLevel))
				return 1f;

			if (!IsFireForEffectActive(caster))
				return 1f;

			var bonusPercent =
				FireForEffectBaseBonusPercent +
				(zenithLevel * FireForEffectBonusPerLevelPercent);

			return 1f + (bonusPercent / 100f);
		}
	}
}
