using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchant Glove buff.
	/// Adds Accuracy based on the character's current Accuracy when the buff is activated.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Enchantglove_Buff)]
	public class Enchanter_EnchantGlove_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is Character character)
			{
				var accuracyBonus = this.GetAccuracyBonus(buff, character);
				buff.NumArg2 = accuracyBonus;
				character.Properties.Modify(PropertyName.HR_BM, accuracyBonus);

				var criticalBonus = this.GetCriticalRateBonus(buff, character);
				buff.NumArg3 = criticalBonus;
				character.Properties.Modify(PropertyName.CRTHR_BM, criticalBonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.HR,
					PropertyName.HR_BM,
					PropertyName.CRTHR,
					PropertyName.CRTHR_BM);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
			{
				var accuracyBonus = buff.NumArg2;
				character.Properties.Modify(PropertyName.HR_BM, -accuracyBonus);

				var criticalBonus = buff.NumArg3;
				character.Properties.Modify(PropertyName.CRTHR_BM, -criticalBonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.HR,
					PropertyName.HR_BM,
					PropertyName.CRTHR,
					PropertyName.CRTHR_BM);
			}
		}

		private float GetAccuracyBonus(Buff buff, Character character)
		{
			var skillLevel = buff.NumArg1;

			// Base skill:
			// Lv1 = 6%, Lv10 = 15%
			var bonusRate = 0.05f + (0.01f * skillLevel);

			// Ability: Enchant Glove: Enhance
			if (character.Abilities.TryGet(AbilityId.Enchanter9, out var ability) && ability.Active)
			{
				var enhanceRate = ability.Level * 0.005f;

				if (ability.Level >= 100)
					enhanceRate += 0.10f;

				bonusRate *= 1f + enhanceRate;
			}

			var currentAccuracy = character.Properties.GetFloat(PropertyName.HR);

			return currentAccuracy * bonusRate;
		}

		private float GetCriticalRateBonus(Buff buff, Character character)
		{
			if (!character.Abilities.TryGet(AbilityId.Enchanter14, out var ability) || !ability.Active)
				return 0;

			var currentCriticalRate = character.Properties.GetFloat(PropertyName.CRTHR);

			// Ability has only 1 level.
			// Bonus scales with Enchant Glove skill level.
			// Lv1 = 1%, Lv10 = 10%.
			var bonusRate = 0.01f * buff.NumArg1;

			return currentCriticalRate * bonusRate;
		}
	}
}
