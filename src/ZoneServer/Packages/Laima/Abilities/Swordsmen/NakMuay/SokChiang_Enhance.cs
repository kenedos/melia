using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// NakMuay2 - Sok Chiang: Enhance.
	///
	/// Effect:
	/// - Increases Sok Chiang damage by 0.5% per ability level.
	/// - At level 100, adds an extra 10%.
	/// - Total at level 100: 60%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.NakMuay2)]
	public class NakMuay_SokChiangEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
