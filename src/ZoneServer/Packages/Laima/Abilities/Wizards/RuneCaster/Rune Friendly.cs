using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster19 - Rune Friendly.
	///
	/// Effect:
	/// - If the equipped weapon is Staff or Rod,
	///   Rune Caster skills with Neutral, Psychokinesis/Soul, or Earth property
	///   deal 40% more damage.
	///
	/// This ability is a marker.
	/// The actual damage bonus is applied in RuneCasterFriendlyHelper.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster19)]
	public class RuneCaster_RuneFriendlyAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
