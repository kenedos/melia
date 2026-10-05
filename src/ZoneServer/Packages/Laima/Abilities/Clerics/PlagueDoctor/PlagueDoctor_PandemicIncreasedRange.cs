using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Pandemic: Increased Range.
	/// Adds 5 range per ability level.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor10)]
	public class PlagueDoctor_PandemicIncreasedRangeAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
