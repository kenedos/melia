using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Scouts.BulletMarker
{
	/// <summary>
	/// Bulletmarker24 - Freeze Bullet: Silver Pellet.
	///
	/// Effect:
	/// - While Freeze Bullet is active, basic attacks have a 30% chance
	///   to deal an additional Holy damage hit.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Bulletmarker24)]
	public class BulletMarker_FreezeBulletSilverPelletAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
