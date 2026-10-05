using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer33)]
	public class Cannoneer_SmokeGrenadeEnhanceAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 100;
		private const float BonusPerLevel = 0.005f;
		private const float MaximumLevelBonus = 0.10f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static float GetDamageMultiplier(Character character)
		{
			if (character == null || !character.Abilities.TryGet(AbilityId.Cannoneer33, out var ability) || !ability.Active)
				return 1f;

			var abilityLevel = Math.Clamp(ability.Level, 0, MaximumLevel);
			var damageBonus = abilityLevel * BonusPerLevel;

			if (abilityLevel >= MaximumLevel)
				damageBonus += MaximumLevelBonus;

			return 1f + damageBonus;
		}
	}
}
