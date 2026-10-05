using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster1 - Rune Caster: Skilled Casting.
	///
	/// Effect:
	/// - After using Rune Caster casting skills, applies/updates a casting buff.
	/// - 1 stack: Rune Caster cast time becomes 1 second.
	/// - 2 stacks: Rune Caster cast time becomes 0.5 seconds.
	/// - Duration: 5 seconds + 0.25 seconds per ability level.
	///
	/// This handler is only a marker.
	/// The real effect must be applied in Rune Caster skills/cast-time logic.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster1)]
	public class RuneCaster_SkilledCastingAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
