using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster7 - Rune of Protection: Knockdown Resistance.
	///
	/// Effect:
	/// - While Rune of Protection is active, gives a chance to prevent
	///   knockback/knockdown.
	/// - Chance is based on ability level.
	/// - SP cost increase should be handled in the skill cost logic later.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster7)]
	public class RuneCaster_RuneOfProtectionKnockdownResistanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
