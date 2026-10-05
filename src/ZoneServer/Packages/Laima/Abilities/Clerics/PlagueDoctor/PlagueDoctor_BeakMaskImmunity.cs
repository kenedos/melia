using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Beak Mask: Immunity.
	/// Behavior is handled directly by BeakMask_BuffOverride.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor7)]
	public class PlagueDoctor_BeakMaskImmunityAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
