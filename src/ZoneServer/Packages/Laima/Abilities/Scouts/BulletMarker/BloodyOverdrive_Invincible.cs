using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Scouts.BulletMarker
{
	/// <summary>
	/// Bulletmarker12 - Bloody Overdrive: Invincible.
	///
	/// Effect:
	/// - Grants invincibility during Bloody Overdrive.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Bulletmarker12)]
	public class BulletMarker_BloodyOverdriveInvincibleAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
