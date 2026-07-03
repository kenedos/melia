using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability marker for [Arts] Limacon: Enhanced Upgrade.
	/// The actual behavior should be handled inside the Limacon attack logic.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter23)]
	public class SchwarzerReiter_Limacon_EnhancedUpgradeAbility : IAbilityHandler
	{
		// This handler intentionally has no combat modifier.
		// The Limacon attack handler should check IsAbilityActive(...) and apply:
		// - increased Limacon damage;
		// - increased spread behavior if needed;
		// - any other custom arts behavior.
	}
}
