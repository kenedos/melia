using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Enchant Weapon buff.
	/// Increases Critical Rate and controls Enchant Weapon: Lightning stacks.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EnchantLightning_Buff)]
	public class Enchanter_EnchantWeapon_BuffOverride : BuffHandler, ISkillCombatAttackAfterCalcHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is Character character)
			{
				var bonus = this.GetCriticalRateBonus(buff, character);

				buff.NumArg2 = bonus;

				character.Properties.Modify(PropertyName.CRTHR_BM, bonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.CRTHR,
					PropertyName.CRTHR_BM);

				EnchanterLightningHelper.Reset(character);

				buff.UpdateTime = TimeSpan.FromSeconds(1);
				buff.NextUpdateTime = DateTime.Now.Add(buff.UpdateTime);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
			{
				var bonus = buff.NumArg2;

				character.Properties.Modify(PropertyName.CRTHR_BM, -bonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.CRTHR,
					PropertyName.CRTHR_BM);

				EnchanterLightningHelper.Reset(character);
			}
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character)
				EnchanterLightningHelper.DecayStack(character);
		}

		public void OnAttackAfterCalc(
			Skill skill,
			ICombatEntity attacker,
			ICombatEntity target,
			Skill attackerSkill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			if (attacker is not Character character)
				return;

			if (!EnchanterLightningHelper.CanUseLightningStacks(character))
				return;

			EnchanterLightningHelper.AddStack(character);

			var multiplier = EnchanterLightningHelper.GetFinalDamageMultiplier(character);

			skillHitResult.Damage *= multiplier;
		}

		private float GetCriticalRateBonus(Buff buff, Character character)
		{
			var currentCriticalRate = character.Properties.GetFloat(PropertyName.CRTHR);

			// Base skill:
			// Lv1 = +2%
			// Lv10 = +20%
			var bonusRate = 0.02f * buff.NumArg1;

			// Ability: Enchant Weapon: Enhance
			if (character.Abilities.TryGet(AbilityId.Enchanter11, out var ability) && ability.Active)
			{
				var enhanceRate = ability.Level * 0.005f;

				if (ability.Level >= 100)
					enhanceRate += 0.10f;

				bonusRate *= 1f + enhanceRate;
			}

			return currentCriticalRate * bonusRate;
		}
	}
}
