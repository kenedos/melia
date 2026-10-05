using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Exorcist8)]
	public class Exorcist_EngkrateiaPatienceAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 3;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static int GetDurationBonus(Character character)
		{
			if (character == null || !character.TryGetActiveAbilityLevel(AbilityId.Exorcist8, out var abilityLevel))
				return 0;

			return Math.Clamp(abilityLevel, 1, MaximumLevel);
		}
	}
}
