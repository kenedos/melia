using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability marker for [Arts] Caracole: Enhanced Upgrade.
	/// The actual behavior should be handled inside the Caracole skill handler.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter22)]
	public class SchwarzerReiter_Caracole_EnhancedUpgradeAbility : IAbilityHandler
	{
		// This handler intentionally has no combat modifier.
		// The Caracole skill handler should check IsAbilityActive(...) and apply:
		// - increased damage multiplier;
		// - increased area or target count;
		// - any other custom arts behavior.
	}
}
