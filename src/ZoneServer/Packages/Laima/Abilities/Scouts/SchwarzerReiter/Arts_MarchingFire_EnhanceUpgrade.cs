using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability marker for [Arts] Marching Fire: Enhanced Upgrade.
	/// The actual behavior should be handled inside the Marching Fire skill handler.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter25)]
	public class SchwarzerReiter_MarchingFire_EnhancedUpgradeAbility : IAbilityHandler
	{
		// This handler intentionally has no combat modifier.
		// The Marching Fire skill handler should check IsAbilityActive(...) and apply:
		// - increased damage multiplier;
		// - increased hit behavior;
		// - any other custom arts behavior.
	}
}
