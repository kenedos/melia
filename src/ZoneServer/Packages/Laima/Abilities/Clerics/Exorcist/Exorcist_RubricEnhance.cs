using System;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist
{
	public static class Exorcist_RubricEnhanceAbility
	{
		private const int MaximumLevel = 100;
		private const float DamagePerLevel = 0.005f;
		private const float MaximumLevelBonus = 0.10f;

		public static float GetDamageMultiplier(Character character)
		{
			if (character == null || !character.TryGetActiveAbilityLevel(AbilityId.Exorcist1, out var abilityLevel))
				return 1f;

			abilityLevel = Math.Clamp(abilityLevel, 1, MaximumLevel);

			var damageMultiplier = 1f + abilityLevel * DamagePerLevel;

			if (abilityLevel >= MaximumLevel)
				damageMultiplier += MaximumLevelBonus;

			return damageMultiplier;
		}
	}
}
