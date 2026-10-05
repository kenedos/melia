using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// StartUp: Flash.
	/// Reduces damage received by 10% for 5 seconds when Flash is used while StartUp is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Blossomblader16)]
	public class BlossomBlader_StartUpFlashAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
