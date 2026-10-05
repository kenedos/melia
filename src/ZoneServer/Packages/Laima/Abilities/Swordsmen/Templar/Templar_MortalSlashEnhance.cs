using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Mortal Slash: Enhance.
	/// Increases damage by 0.5% per level and adds another 10% at level 100.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Templar1)]
	public class Templar_MortalSlashEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
