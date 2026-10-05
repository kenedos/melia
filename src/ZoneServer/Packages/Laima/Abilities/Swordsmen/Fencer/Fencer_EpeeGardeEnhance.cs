using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Fencer12 - Epee Garde: Enhance.
	///
	/// Effect:
	/// - Increases Epee Garde's skill damage bonus by 0.5% per ability level.
	/// - At level 100, adds an extra 10%.
	/// - Total at level 100: 60%.
	///
	/// Note:
	/// The actual bonus is applied in EpeeGarde_BuffOverride.
	/// This handler only serves as an active ability marker.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Fencer12)]
	public class Fencer_EpeeGardeEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
