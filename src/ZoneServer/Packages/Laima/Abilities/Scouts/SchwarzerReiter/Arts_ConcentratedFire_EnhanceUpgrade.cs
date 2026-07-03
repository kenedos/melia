using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability marker for [Arts] Concentrated Fire: Enhanced Upgrade.
	/// The actual behavior is handled inside the Concentrated Fire skill handler.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter21)]
	public class SchwarzerReiter_ConcentratedFire_EnhancedUpgradeAbility : IAbilityHandler
	{
		// This handler intentionally has no combat modifier.
		// The skill handler checks IsAbilityActive(...) and applies:
		// - increased hit count;
		// - increased damage multiplier;
		// - any other custom skill behavior.
	}
}
