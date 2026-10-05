using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Flowering: Swift.
	/// While active and using a one-handed sword, grants Blossom Blader attacks
	/// +2% final damage and +2% critical chance per ability level against targets
	/// affected by the character's own Flowering.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Blossomblader2)]
	public class BlossomBlader_FloweringSwiftAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
