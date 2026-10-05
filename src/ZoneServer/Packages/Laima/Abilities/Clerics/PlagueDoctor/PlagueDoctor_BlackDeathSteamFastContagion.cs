using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Black Death Steam: Fast Contagion.
	/// Reduces the interval between Black Death Steam hits by 0.1 seconds.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor16)]
	public class PlagueDoctor_BlackDeathSteamFastContagionAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
