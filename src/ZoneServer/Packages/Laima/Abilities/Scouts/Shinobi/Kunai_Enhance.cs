using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Shinobi1 - Kunai: Enhance.
	/// Increases Kunai damage by 0.5% per level.
	/// At level 100, adds an additional 10%, for a total of 60%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Shinobi1)]
	public class Shinobi_KunaiEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
