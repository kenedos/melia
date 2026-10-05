using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Sweeping Cannon: Enhance.
	/// Aumenta o dano em 0,5% por nível.
	/// No nível 100 recebe um bônus adicional de 10%.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer15)]
	public class Cannoneer_SweepingCannonEnhanceAbility : AbilityPropertyHandler
	{
		private const int MaximumLevel = 100;
		private const float DamageBonusPerLevel = 0.005f;
		private const float MaximumLevelBonus = 0.10f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static float GetDamageMultiplier(Character character)
		{
			if (character == null)
				return 1f;

			var abilityLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.Cannoneer15), 0, MaximumLevel);

			if (abilityLevel <= 0)
				return 1f;

			var damageBonus = abilityLevel * DamageBonusPerLevel;

			if (abilityLevel >= MaximumLevel)
				damageBonus += MaximumLevelBonus;

			return 1f + damageBonus;
		}
	}
}
