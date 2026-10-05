using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// StartUp: Blossom Shower.
	/// Adds one hit to Control Blade, Fallen Blossom, and Blossom Slash while a fully charged StartUp buff is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Blossomblader20)]
	public class BlossomBlader_StartUpBlossomShowerAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
