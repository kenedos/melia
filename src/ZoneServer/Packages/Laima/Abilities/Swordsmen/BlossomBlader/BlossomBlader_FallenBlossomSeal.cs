using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Fallen Blossom: Seal.
	/// Applies Silence when Fallen Blossom hits an enemy with at least three Flowering stacks.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Blossomblader4)]
	public class BlossomBlader_FallenBlossomSealAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
