using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.Druid
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Druid22)]
	public class Druid_PhysicalStatAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
			character.Properties.InvalidateAll();
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
			character.Properties.InvalidateAll();
		}
	}
}
