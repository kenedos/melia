using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Healing Factor: Heal Party.
	/// The behavior is handled directly by PlagueDoctor_HealingFactor.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor18)]
	public class PlagueDoctor_HealingFactorHealPartyAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
