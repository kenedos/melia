using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// ShinobiAruki - Shinobi: Ninja Walk.
	///
	/// Effect:
	/// - Movement Speed +10.
	/// - Stamina consumption reduced by 25%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.ShinobiAruki)]
	public class Shinobi_NinjaWalkAbility : AbilityPropertyHandler
	{
		private const float MoveSpeedBonus = 10f;

		public override void OnActivate(Ability ability, Character character)
		{
			if (!ability.Active)
				return;

			character.Properties.Modify(PropertyName.MSPD_BM, MoveSpeedBonus);
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
			character.Properties.Modify(PropertyName.MSPD_BM, -MoveSpeedBonus);
		}
	}
}
