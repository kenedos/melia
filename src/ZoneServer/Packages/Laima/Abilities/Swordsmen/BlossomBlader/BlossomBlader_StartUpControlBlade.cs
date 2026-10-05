using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// StartUp: Control Blade.
	/// Reduces Control Blade cooldown by 1 second when used while StartUp is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Blossomblader9)]
	public class BlossomBlader_StartUpControlBladeAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
