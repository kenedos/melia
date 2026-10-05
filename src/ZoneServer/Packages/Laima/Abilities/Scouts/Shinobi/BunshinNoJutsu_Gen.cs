using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Shinobi
{
	/// <summary>

	/// Shinobi18 - Bunshin no Jutsu: Gen.

	/// Allows Bunshin clones to randomly inflict Slow, Blind, Silence, or Stun.

	/// </summary>

	[Package("laima")]
	[AbilityHandler(AbilityId.Shinobi18)]
	public class Shinobi_BunshinNoJutsuGenAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
