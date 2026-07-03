using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability marker for [Arts] Serial Bullet: Enhanced Upgrade.
	/// The actual behavior should be handled inside the Serial Bullet attack logic.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter29)]
	public class SchwarzerReiter_SerialBullet_EnhancedUpgradeAbility : IAbilityHandler
	{
		// This handler intentionally has no combat modifier.
		// The TargetSkill handler should check IsAbilityActive(...) and apply:
		// - increased Serial Bullet damage;
		// - increased additional hit behavior;
		// - any other custom arts behavior.
	}
}
