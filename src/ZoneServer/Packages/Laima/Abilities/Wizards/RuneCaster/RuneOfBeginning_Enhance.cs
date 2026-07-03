using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster25 - Rune of Beginning: Enhance.
	///
	/// Effect:
	/// - Increases Rune of Beginning's final damage bonus by 0.5% per ability level.
	/// - At level 100, adds an extra 10%.
	/// - Total at level 100: 60%.
	///
	/// Note:
	/// The actual bonus is applied in RuneCaster_BerkanaOverride.
	/// This handler only serves as an active ability marker.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster25)]
	public class RuneCaster_RuneOfBeginningEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
