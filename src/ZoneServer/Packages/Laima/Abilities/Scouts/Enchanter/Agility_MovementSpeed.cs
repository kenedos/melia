using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Enchanter8 - Agility: Movement Speed.
	/// Marker ability used by Enchanter_Agility.
	/// Effect:
	/// - Agility movement speed bonus +5.
	/// - Agility SP consumption +20%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Enchanter8)]
	public class Enchanter_AgilityMovementSpeedAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
