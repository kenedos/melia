//--- Melia Script ----------------------------------------------------------
// Doppelsoeldner: Tough
//--- Description -----------------------------------------------------------
// Reduces maximum stamina and increases critical chance and AoE Attack Ratio.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handler for Doppelsoeldner: Tough.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Doppelsoeldner24)]
	public class Doppelsoeldner_ToughAbility : AbilityPropertyHandler
	{
		private const float MaximumStaminaConversionRate = 0.50f;
		private const float CriticalChanceBonusPerLevel = 0.03f;
		private const float AoeAttackRatioBonusPerLevel = 1f;

		public override void OnActivate(Ability ability, Character character)
		{
			var abilityLevel = Math.Clamp(ability.Level, 1, 10);
			var baseCriticalRate = Math.Max(0f, character.Properties.GetFloat(PropertyName.CRTHR));
			var criticalRateBonus = baseCriticalRate * abilityLevel * CriticalChanceBonusPerLevel;
			var maximumStamina = Math.Max(0f, character.Properties.GetFloat(PropertyName.MaxSta_BM));
			var staminaPenalty = -(maximumStamina * MaximumStaminaConversionRate);

			AddPropertyModifier(ability, character, PropertyName.MaxSta_BM, staminaPenalty);
			AddPropertyModifier(ability, character, PropertyName.CRTHR_BM, criticalRateBonus);
			AddPropertyModifier(ability, character, PropertyName.SR_BM, abilityLevel * AoeAttackRatioBonusPerLevel);
			NotifyProperties(character);
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
			RemovePropertyModifier(ability, character, PropertyName.MaxSta_BM);
			RemovePropertyModifier(ability, character, PropertyName.CRTHR_BM);
			RemovePropertyModifier(ability, character, PropertyName.SR_BM);
			NotifyProperties(character);
		}

		private static void NotifyProperties(Character character)
		{
			if (character.Connection == null)
				return;

			Send.ZC_OBJECT_PROPERTY(character);
		}
	}
}
