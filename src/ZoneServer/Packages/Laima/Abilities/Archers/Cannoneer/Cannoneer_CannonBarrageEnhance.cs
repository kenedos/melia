using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Cannon Barrage: Enhance
	/// Aumenta o dano em 0,5% por nível.
	/// No nível 100 concede 10% adicionais, totalizando 60%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer9)]
	public class Cannoneer_CannonBarrageEnhanceAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 100;
		private const float BonusPerLevel = 0.005f;
		private const float MaximumLevelBonus = 0.10f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static float GetDamageMultiplier(Character character)
		{
			if (!character.TryGetActiveAbilityLevel(AbilityId.Cannoneer9, out var abilityLevel))
				return 1f;

			abilityLevel = Math.Clamp(abilityLevel, 0, MaximumLevel);
			var multiplier = 1f + abilityLevel * BonusPerLevel;

			if (abilityLevel >= MaximumLevel)
				multiplier += MaximumLevelBonus;

			return multiplier;
		}
	}
}
