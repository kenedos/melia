using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Bazooka: Steady Fire
	/// Aumenta em 15% o dano de todas as skills enquanto Bazooka está ativa.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer22)]
	public class Cannoneer_BazookaSteadyFireAbility : AbilityPropertyHandler
	{
		public const float AdditionalDamageMultiplier = 1.15f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool IsActive(Character character)
		{
			return character != null && character.Abilities.TryGet(AbilityId.Cannoneer22, out var ability) && ability.Active;
		}
	}
}
