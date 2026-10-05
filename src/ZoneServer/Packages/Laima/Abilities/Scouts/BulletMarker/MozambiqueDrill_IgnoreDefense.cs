using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Bulletmarker9 - Mozambique Drill: Ignore Defense.
	///
	/// Effect:
	/// - Ignores 2% of the target's Defense per ability level.
	/// - Maximum level: 5.
	/// - Maximum Defense penetration: 10%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Bulletmarker9)]
	public class BulletMarker_MozambiqueDrillIgnoreDefenseAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
