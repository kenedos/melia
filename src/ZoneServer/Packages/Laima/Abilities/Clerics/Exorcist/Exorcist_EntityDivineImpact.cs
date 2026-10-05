using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Exorcist5)]
	public class Exorcist_EntityDivineImpactAbility : AbilityPropertyHandler
	{
		public const float OriginalDamageBonus = 0.10f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool IsActive(Character character)
		{
			return character != null && character.IsAbilityActive(AbilityId.Exorcist5);
		}

		public static float GetNonHiddenDamageMultiplier(Character character)
		{
			const float reducedDamageMultiplier = 0.70f;

			if (!IsActive(character))
				return reducedDamageMultiplier;

			return reducedDamageMultiplier + OriginalDamageBonus;
		}
	}
}
