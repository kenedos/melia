using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Scouts.Shinobi
{
	/// <summary>

	/// Shinobi17 - Bunshin no Jutsu: Tai.

	/// While Bunshin no Jutsu is active, grants the caster immunity to knockback

	/// and knockdown, increases movement speed by 10, and reduces damage received

	/// by the caster and the clones by 30%.

	/// </summary>

	[Package("laima")]
	[AbilityHandler(AbilityId.Shinobi17)]
	public class Shinobi_BunshinNoJutsuTaiAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
