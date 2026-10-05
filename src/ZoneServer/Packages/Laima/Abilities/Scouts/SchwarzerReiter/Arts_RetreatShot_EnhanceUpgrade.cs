using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability marker for [Arts] Retreat Shot: Enhanced Upgrade.
	/// The actual behavior should be handled inside the Retreat Shot skill or buff handler.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter24)]
	public class SchwarzerReiter_RetreatShot_EnhancedUpgradeAbility : IAbilityHandler
	{
		// This handler intentionally has no combat modifier.
		// The Retreat Shot skill or buff handler should check IsAbilityActive(...) and apply:
		// - increased damage multiplier;
		// - increased hit behavior;
		// - any other custom arts behavior.
	}
}
