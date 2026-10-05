using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Bazooka: Veteran Mercenary
	/// Remove a distância mínima de ataque da Bazooka e aumenta ASPD.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer16)]
	public class Cannoneer_BazookaVeteranMercenaryAbility : AbilityPropertyHandler
	{
		public const float AttackSpeedBonus = 100f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool IsActive(Character character)
		{
			return character != null && character.Abilities.TryGet(AbilityId.Cannoneer16, out var ability) && ability.Active;
		}
	}
}
