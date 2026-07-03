using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability handler for Limacon: Enhance.
	/// Increases Limacon damage while the Limacon buff is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter13)]
	public class SchwarzerReiter_Limacon_EnhanceAbility : IAbilityHandler
	{
		// This handler intentionally has no combat modifier.
		// The TargetSkill handler should check IsAbilityActive(...) and apply:
		// - increased Limacon damage;
		// - increased additional hit behavior;
		// - any other custom arts behavior.
	}
}
