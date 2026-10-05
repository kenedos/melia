using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.Packages.Laima.Abilities.Wizards.Sage
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Sage20)]
	public class Sage_HoleOfDarknessBlackHoleAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
