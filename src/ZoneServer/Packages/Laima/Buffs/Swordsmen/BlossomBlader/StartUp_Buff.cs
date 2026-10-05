using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Packages.Laima.Skills.Swordsmen.BlossomBlader;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Grants the StartUp final-damage bonuses while a one-handed sword is equipped.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.StartUp_Buff)]
	public class StartUp_BuffOverride : BuffHandler, IBuffCombatAttackBeforeCalcHandler, IBuffCombatAttackAfterCalcHandler
	{
		private const float GeneralDamageMultiplier = 1.10f;
		private const float DexPerCriticalRate = 5f;
		private const int UpdateIntervalMilliseconds = 1000;
		private const float BaseFinalDamageBonus = 0.20f;
		private const float FloweringFinalDamageBonus = 0.20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM);
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM, GetCriticalRateBonus(buff.Target));
			buff.SetUpdateTime(UpdateIntervalMilliseconds);
		}

		public override void WhileActive(Buff buff)
		{
			if (!ValidateWeapon(buff))
				return;

			UpdatePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM, GetCriticalRateBonus(buff.Target));
		}

		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker != buff.Target || !ValidateWeapon(buff))
				return;

			UpdatePropertyModifier(buff, attacker, PropertyName.CRTHR_BM, GetCriticalRateBonus(attacker));
		}

		private static float GetCriticalRateBonus(ICombatEntity caster)
		{
			if (caster == null || caster.IsDead || !BlossomBladerStartUpHelper.IsUsingOneHandSword(caster))
				return 0f;
			return (float)Math.Floor(Math.Max(0f, caster.Properties.GetFloat(PropertyName.DEX)) / DexPerCriticalRate);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM);
		}

		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker != buff.Target || target == null || target.IsDead || skillHitResult.Damage <= 0f)
				return;

			if (!ValidateWeapon(buff))
				return;

			skillHitResult.Damage *= GeneralDamageMultiplier;

			if (!BlossomBladerStartUpHelper.IsBlossomBladerAttack(skill))
				return;

			var finalDamageBonus = BaseFinalDamageBonus;

			if (target.TryGetBuff(BuffId.Flowering_Debuff, out var floweringDebuff)
				&& floweringDebuff.Caster == attacker)
			{
				finalDamageBonus += FloweringFinalDamageBonus;
			}

			skillHitResult.Damage *= 1f + finalDamageBonus;
		}

		private static bool ValidateWeapon(Buff buff)
		{
			if (BlossomBladerStartUpHelper.IsUsingOneHandSword(buff.Target))
				return true;

			buff.Target.RemoveBuff(BuffId.StartUp_Buff);
			return false;
		}
	}
}
