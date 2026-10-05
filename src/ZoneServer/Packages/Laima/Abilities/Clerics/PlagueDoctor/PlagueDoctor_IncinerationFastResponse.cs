using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Incineration: Fast Response.
	/// Reduces the interval between Incineration hits by 0.2 seconds.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor15)]
	public class PlagueDoctor_IncinerationFastResponseAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
