using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Inquisitor19)]
	public class Inquisitor_RipperArmorBreakAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 5;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool TryGetLevel(Character character, out int level)
		{
			level = 0;
			if (character == null || !character.TryGetActiveAbilityLevel(AbilityId.Inquisitor19, out var activeLevel))
				return false;

			level = Math.Clamp(activeLevel, 1, MaximumLevel);
			return true;
		}
	}
}
