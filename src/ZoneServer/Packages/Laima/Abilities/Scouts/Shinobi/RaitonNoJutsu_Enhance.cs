using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Shinobi10 - Raiton no Jutsu: Enhance.
	/// Increases Raiton no Jutsu damage by 0.5% per level.
	/// At level 100, adds an additional 10%, for a total of 60%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Shinobi10)]
	public class Shinobi_RaitonNoJutsuEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
