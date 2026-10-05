using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Shinobi
{
	/// <summary>

	/// Shinobi16 - Bunshin no Jutsu: Jin.

	/// Strengthens Katon no Jutsu, Raiton no Jutsu, and Mijin no Jutsu.

	/// </summary>

	[Package("laima")]
	[AbilityHandler(AbilityId.Shinobi16)]
	public class Shinobi_BunshinNoJutsuJinAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
