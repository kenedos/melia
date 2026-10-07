using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for Gohei's Mental Breakdown, which raises the magic damage
	/// the target takes by 10% per buff Gohei stripped.
	/// </summary>
	/// <remarks>
	/// NumArg2: Buffs stripped
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.MentalCollapse_Debuff)]
	public class Miko_MentalCollapse_DebuffOverride : BuffHandler
	{
		private const float RatePerBuff = 0.10f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.MentalCollapse_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Data.AttackType == SkillAttackType.Magic && target.TryGetBuff(BuffId.MentalCollapse_Debuff, out var buff))
				modifier.DamageMultiplier += buff.NumArg2 * RatePerBuff;
		}
	}

	/// <summary>
	/// Handler for Gohei's Mental Recovery, which lowers the magic damage the
	/// target takes by 10% per debuff Gohei cured.
	/// </summary>
	/// <remarks>
	/// NumArg2: Debuffs cured
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.MentalRecovery_Buff)]
	public class Miko_MentalRecovery_BuffOverride : BuffHandler
	{
		private const float RatePerDebuff = 0.10f;
		private const float MaxReduction = 0.9f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.MentalRecovery_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Data.AttackType == SkillAttackType.Magic && target.TryGetBuff(BuffId.MentalRecovery_Buff, out var buff))
				skillHitResult.Damage *= Math.Max(1 - MaxReduction, 1 - buff.NumArg2 * RatePerDebuff);
		}
	}

	/// <summary>
	/// Handler for Hamaya's mark, which lowers critical resistance by the
	/// skill's ratio in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Hamaya_TakeDamage)]
	public class Miko_Hamaya_TakeDamageOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.CRTDR_RATE_BM, -GetCaptionRatio(buff, 1) / 100f);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_RATE_BM);
		}
	}

	/// <summary>
	/// Handler for [Arts] Hamaya: Heal, which heals 5% of max HP every 0.5
	/// seconds.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Hamaya_Buff)]
	public class Miko_Hamaya_BuffOverride : BuffHandler
	{
		private const float HealRate = 0.05f;

		public override void WhileActive(Buff buff)
		{
			if (!buff.Target.IsDead)
				buff.Target.Heal(buff.Target.MaxHp * HealRate, 0);
		}
	}

	/// <summary>
	/// Handler for Clap, which raises damage by the skill's ratio in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Kasiwade_Buff)]
	public class Miko_Kasiwade_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Kasiwade_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker.TryGetBuff(BuffId.Kasiwade_Buff, out var buff))
				modifier.DamageMultiplier += GetCaptionRatio(buff, 2) / 100f;
		}
	}

	/// <summary>
	/// Handler for Kagura's blessing, which raises damage by the percentage
	/// the dance reached, and with Kagura: Nightingale Dance ignores 3 + level
	/// % of the defense of enemies it weakened.
	/// </summary>
	/// <remarks>
	/// NumArg2: Damage bonus in percent
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.KaguraDance_Buff)]
	public class Miko_KaguraDance_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.KaguraDance_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.KaguraDance_Buff, out var buff))
				return;

			modifier.DamageMultiplier += buff.NumArg2 / 100f;

			if (target.TryGetBuff(BuffId.Kagura_Crtdr_Debuff, out var debuff))
				modifier.DefensePenetrationRate += (3 + debuff.NumArg2) / 100f;
		}
	}

	/// <summary>
	/// Handler for Kagura: Nightingale Dance's weakening, which lowers
	/// critical resistance by 3% per ability level.
	/// </summary>
	/// <remarks>
	/// NumArg2: Ability level
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Kagura_Crtdr_Debuff)]
	public class Miko_Kagura_Crtdr_DebuffOverride : BuffHandler
	{
		private const float RatePerLevel = 0.03f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.CRTDR_RATE_BM, -RatePerLevel * buff.NumArg2);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_RATE_BM);
		}
	}

	/// <summary>
	/// Handler for Great Blessing: Honor, which raises physical and magic
	/// attack by Omikuji's ratio in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Honor_Buff)]
	public class Miko_Honor_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var rate = GetCaptionRatio(buff, 1) / 100f;

			UpdatePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM, rate);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM, rate);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM);
		}
	}

	/// <summary>
	/// Handler for Great Blessing: Hope, which raises block penetration,
	/// accuracy and critical rate by Omikuji's second ratio.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Wish_Buff)]
	public class Miko_Wish_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = GetCaptionRatio(buff, 2);

			UpdatePropertyModifier(buff, buff.Target, PropertyName.BLK_BREAK_BM, bonus);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.HR_BM, bonus);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BREAK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM);
		}
	}

	/// <summary>
	/// Handler for Great Blessing: Safety, which raises block, evasion and
	/// critical resistance by Omikuji's second ratio.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Safety_Buff)]
	public class Miko_Safety_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = GetCaptionRatio(buff, 2);

			UpdatePropertyModifier(buff, buff.Target, PropertyName.BLK_BM, bonus);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.DR_BM, bonus);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}

	/// <summary>
	/// Handler for Great Blessing: Health, which lowers the physical and
	/// magic damage taken by Omikuji's third ratio in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Healthy_Buff)]
	public class Miko_Healthy_BuffOverride : BuffHandler
	{
		private const float MaxReduction = 0.9f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Healthy_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target.TryGetBuff(BuffId.Healthy_Buff, out var buff))
				skillHitResult.Damage *= Math.Max(1 - MaxReduction, 1 - GetCaptionRatio(buff, 3) / 100f);
		}
	}

	/// <summary>
	/// Handler for [Arts] Omikuji: Financial Fortune, which raises the
	/// looting chance by 500.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Money_Buff)]
	public class Miko_Money_BuffOverride : BuffHandler
	{
		private const float LootingChance = 500f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.LootingChance_BM, LootingChance);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.LootingChance_BM);
		}
	}
}
