using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer24)]
	public class Cannoneer_SmokeGrenadeExceedLimitAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool IsActive(Character character)
		{
			return character != null && character.Abilities.TryGet(AbilityId.Cannoneer24, out var ability) && ability.Active;
		}
	}
}
