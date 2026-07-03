using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster11 - Rune of Protection: Endurance.
	///
	/// Effect:
	/// - When Rune of Protection is cast, also grants knockback/knockdown immunity.
	/// - Changes Rune of Protection duration to 5 minutes.
	///
	/// This handler is only a marker.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster11)]
	public class RuneCaster_RuneOfProtectionEnduranceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
