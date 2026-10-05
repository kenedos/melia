using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster2 - Rune of Ice: Slow.
	///
	/// Effect:
	/// Rune of Earth applies Slow to enemies hit.
	///
	/// This handler is only a marker.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster2)]
	public class RuneCaster_RuneOfEarthSlowAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
