using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Scouts.BulletMarker
{
	/// <summary>
	/// Bulletmarker8 - Bloody Overdrive: Ricochet.
	///
	/// Effect:
	/// - Grants 5% chance per ability level to hit one additional target.
	/// - Maximum level: 10.
	/// - Maximum chance: 50%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Bulletmarker8)]
	public class BulletMarker_BloodyOverdriveRicochetAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
