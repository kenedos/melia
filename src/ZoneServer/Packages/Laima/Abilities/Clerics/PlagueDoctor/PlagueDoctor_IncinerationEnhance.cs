using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Incineration: Enhance.
	/// Its level is read directly by Incineration_DebuffOverride.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor2)]
	public class PlagueDoctor_IncinerationEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
