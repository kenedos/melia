using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Incineration: Infect.
	/// Transfers Incineration when an affected enemy dies from its periodic damage.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor13)]
	public class PlagueDoctor_IncinerationInfectAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
