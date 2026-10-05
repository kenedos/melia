using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.Templar
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Templar6)]
	public class Templar_BattleOrdersEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(
			Ability ability,
			Character character
		)
		{
		}

		public override void OnDeactivate(
			Ability ability,
			Character character
		)
		{
		}
	}
}
