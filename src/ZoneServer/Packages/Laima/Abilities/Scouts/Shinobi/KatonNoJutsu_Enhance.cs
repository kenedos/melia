using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Shinobi2 - Katon no Jutsu: Enhance.
	/// Increases Katon no Jutsu damage by 0.5% per level.
	/// At level 100, adds an additional 10%, for a total of 60%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Shinobi2)]
	public class Shinobi_KatonNoJutsuEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
