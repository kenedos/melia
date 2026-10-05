using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability marker for [Arts] Evasive Action: Duration.
	/// The actual duration increase should be handled inside the Evasive Action skill handler.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter34)]
	public class SchwarzerReiter_EvasiveAction_DurationAbility : IAbilityHandler
	{
		// This handler intentionally has no direct effect.
		// The Evasive Action skill handler should check IsAbilityActive(...)
		// and increase the buff duration when the skill is used.
	}
}
