using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// StartUp: Blossom Slash.
	/// Increases Blossom Slash damage by 30% while StartUp is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Blossomblader17)]
	public class BlossomBlader_StartUpBlossomSlashAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
