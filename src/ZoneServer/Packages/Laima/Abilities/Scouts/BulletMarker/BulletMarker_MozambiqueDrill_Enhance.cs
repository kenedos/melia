using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Bulletmarker5 - Mozambique Drill: Enhance.
	///
	/// Effect:
	/// - Increases Mozambique Drill damage by 0.5% per ability level.
	/// - At level 100, adds an extra 10%.
	/// - Total at level 100: 60%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Bulletmarker5)]
	public class BulletMarker_MozambiqueDrillEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
