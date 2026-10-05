using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// StartUp: Fallen Blossom.
	/// Adds three Flowering stacks when Fallen Blossom is used while StartUp is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Blossomblader10)]
	public class BlossomBlader_StartUpFallenBlossomAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
