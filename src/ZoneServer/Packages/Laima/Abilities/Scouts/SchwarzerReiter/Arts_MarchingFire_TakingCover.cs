using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability marker for [Arts] Marching Fire: Taking Cover.
	/// The actual behavior should be handled inside the Marching Fire skill handler.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter30)]
	public class SchwarzerReiter_MarchingFire_TakingCoverAbility : IAbilityHandler
	{
		// This handler intentionally has no combat modifier.
		// The Marching Fire skill handler should check IsAbilityActive(...) and apply:
		// - defensive behavior while channeling;
		// - reduced incoming damage;
		// - movement or attack behavior changes if needed.
	}
}
