using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Modafinil: Enhance.
	/// The ability level is read directly by Modafinil_BuffOverride.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor17)]
	public class PlagueDoctor_ModafinilEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
