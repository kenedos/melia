using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Archers.Mergen
{
	/// <summary>
	/// Spread Shot: Ricochet.
	/// Adds a 1% ricochet chance per Spread Shot skill level.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Mergen9)]
	public class Mergen_SpreadShotRicochetAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
