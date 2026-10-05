using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Necromancer7 - Raise Dead: Enhance.
	///
	/// Effect:
	/// - Increases Skeleton Soldier's attack stats by 0.5% per ability level.
	///
	/// Note:
	/// The actual bonus is applied in Necromancer_RaiseDeadOverride.
	/// This handler only serves as an active ability marker.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Necromancer7)]
	public class Necromancer_RaiseDeadEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
