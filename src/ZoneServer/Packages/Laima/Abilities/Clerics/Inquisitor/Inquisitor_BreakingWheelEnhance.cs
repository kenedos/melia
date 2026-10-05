using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Inquisitor13)]
	public class Inquisitor_BreakingWheelEnhanceAbility : AbilityPropertyHandler
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
			var level = Math.Min(character.Abilities.GetLevel(AbilityId.Inquisitor13), MaximumLevel);

			if (level <= 0)
				return 1f;

			var bonusPercent = level * 0.5f;

			if (level >= MaximumLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
