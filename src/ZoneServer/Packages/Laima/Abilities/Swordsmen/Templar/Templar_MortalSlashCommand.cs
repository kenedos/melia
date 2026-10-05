using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Mortal Slash: Command.
	/// Reduces Mortal Slash cooldown by 2 seconds while active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Templar3)]
	public class Templar_MortalSlashCommandAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
