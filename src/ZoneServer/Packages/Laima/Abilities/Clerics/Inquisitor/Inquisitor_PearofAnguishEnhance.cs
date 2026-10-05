using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Inquisitor6)]
	public class Inquisitor_PearofAnguishEnhanceAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 100;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static float GetDamageMultiplier(Character character)
		{
			var abilityLevel = Math.Min(character.Abilities.GetLevel(AbilityId.Inquisitor6), MaximumLevel);

			if (abilityLevel <= 0)
				return 1f;

			var bonusPercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
