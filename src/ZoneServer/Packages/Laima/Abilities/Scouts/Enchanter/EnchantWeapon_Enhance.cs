using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Enchanter11 - Enchant Weapon: Enhance.
	///
	/// Marker ability.
	///
	/// Effect:
	/// - Increases Enchant Weapon Critical Rate bonus by 0.5% per ability level.
	/// - At Lv100 grants an additional 10%.
	/// - Total enhancement at Lv100 = 60%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Enchanter11)]
	public class Enchanter_EnchantWeaponEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
