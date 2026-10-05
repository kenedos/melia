using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Archers.Mergen
{
	/// <summary>
	/// Spread Shot: Backward Shot.
	/// Its behavior is handled directly by Mergen_Unload.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Mergen8)]
	public class Mergen_SpreadShotBackwardShotAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
