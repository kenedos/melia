using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Bulletmarker10 - Mozambique Drill: Ricochet.
	///
	/// Effect:
	/// - Allows Mozambique Drill to hit 1 additional target.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Bulletmarker10)]
	public class BulletMarker_MozambiqueDrillRicochetAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
