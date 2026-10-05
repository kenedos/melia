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
	[AbilityHandler(AbilityId.Druid19)]
	public class Druid_ThornBleedingAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 5;
		private const int ChancePerLevel = 20;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static int GetChance(Character character)
		{
			if (!character.TryGetActiveAbilityLevel(AbilityId.Druid19, out var level))
				return 0;

			return Math.Clamp(level, 0, MaximumLevel) * ChancePerLevel;
		}
	}
}
