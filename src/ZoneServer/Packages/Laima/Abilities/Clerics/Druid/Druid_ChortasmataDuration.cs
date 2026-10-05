using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Druid
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Druid1)]
	public class Druid_ChortasmataDurationAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 5;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static int GetDurationBonus(Character character)
		{
			if (!character.TryGetActiveAbilityLevel(AbilityId.Druid1, out var level))
				return 0;

			return Math.Clamp(level, 0, MaximumLevel);
		}
	}
}
