using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer10)]
	public class Cannoneer_SmokeGrenadeAdditionalDamageAbility : AbilityPropertyHandler
	{
		private const float DamageBonusPerLevel = 0.10f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static float GetDamageMultiplier(Character character)
		{
			if (character == null || !character.Abilities.TryGet(AbilityId.Cannoneer10, out var ability) || !ability.Active)
				return 1f;

			var abilityLevel = Math.Max(0, ability.Level);
			return 1f + abilityLevel * DamageBonusPerLevel;
		}
	}
}
