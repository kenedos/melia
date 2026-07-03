using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster10 - Rune of Protection: Enhance.
	///
	/// Effect:
	/// - Increases the effectiveness of Rune of Protection by 0.5% per ability level.
	/// - At level 100, adds an extra 10%.
	/// - Total at level 100: 60%.
	///
	/// Note:
	/// The actual bonus is applied in the Rune of Protection BuffHandler.
	/// This ability only serves as a marker.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster10)]
	public class RuneCaster_RuneOfProtectionEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
