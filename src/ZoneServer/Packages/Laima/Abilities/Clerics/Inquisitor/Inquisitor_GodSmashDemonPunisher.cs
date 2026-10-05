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
	[AbilityHandler(AbilityId.Inquisitor12)]
	public class Inquisitor_GodSmashDemonPunisherAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 3;
		private const float DamageBonusPerLevel = 0.05f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static float GetDamageMultiplier(Character character)
		{
			if (!character.IsBuffActive(BuffId.Judgment_Buff))
				return 1f;

			if (!character.TryGetActiveAbilityLevel(AbilityId.Inquisitor12, out var abilityLevel))
				return 1f;

			abilityLevel = Math.Clamp(abilityLevel, 1, MaximumLevel);

			return 1f + abilityLevel * DamageBonusPerLevel;
		}
	}
}
