using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Flash: Rush.
	/// Reduces Flash cooldown by 5 seconds when the skill lands a critical hit.
	/// Effect cooldown: 15 seconds.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Blossomblader7)]
	public class BlossomBlader_FlashRushAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
