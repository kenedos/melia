using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Cannon Shot: Chain Explosion.
	/// Adds three hits to Cannon Shot, resulting in five total hits.
	/// Reduces the damage of each hit by 33%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer17)]
	public class Cannoneer_CannonShotChainExplosionAbility : AbilityPropertyHandler
	{
		private const int AdditionalHitCount = 3;
		private const float DamageMultiplier = 0.67f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static int GetAdditionalHitCount(Character character)
		{
			return IsActive(character) ? AdditionalHitCount : 0;
		}

		public static float GetDamageMultiplier(Character character)
		{
			return IsActive(character) ? DamageMultiplier : 1f;
		}

		private static bool IsActive(Character character)
		{
			return character != null && character.Abilities.TryGet(AbilityId.Cannoneer17, out var ability) && ability.Active;
		}
	}
}
