using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Enchanter14 - Enchant Glove: Critical.
	///
	/// Marker ability.
	///
	/// Effect:
	/// - When Enchant Glove is active, also increases Critical Rate.
	/// - Bonus is based on Enchant Glove skill level.
	/// - Lv1 = +1%
	/// - Lv10 = +10%
	/// - Ability itself has only 1 level.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Enchanter14)]
	public class Enchanter_EnchantGloveCriticalAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
