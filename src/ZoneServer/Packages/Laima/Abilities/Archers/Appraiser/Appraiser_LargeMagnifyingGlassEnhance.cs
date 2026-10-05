using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Archers.Appraiser
{
	/// <summary>
	/// Appraiser10 - Huge Magnifier: Enhance.
	/// Increases damage by 0.5% per level.
	/// At level 100, adds another 10%, totaling 60%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Appraiser10)]
	public class Appraiser_LargeMagnifyingGlassEnhanceAbility :
		AbilityPropertyHandler
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
