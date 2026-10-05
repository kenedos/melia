using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsman.Peltasta
{
	/// <summary>
	/// Handler for the Guardian buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Guardian_Buff)]
	public class Guardian_BuffOverride : BuffHandler
	{
		public const float DamageReductionBase = 0.25f;
		public const float DamageReductionPerLevel = 0.025f;
		public const float SDRBonusBase = 2f;
		public const float SDRBonusPerLevel = 1f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.SDR_BM, this.GetSDRBonus(buff));
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.SDR_BM);
		}

		/// <summary>
		/// Returns the SDR bonus for the buff.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns></returns>
		private float GetSDRBonus(Buff buff)
		{
			return Math.Min(SDRBonusBase + buff.NumArg1 * SDRBonusPerLevel, 11);
		}

		/// <summary>
		/// Damage reduction
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Guardian_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Guardian_Buff, out var buff))
				return;

			var skillLevel = buff.NumArg1;
			var multiplierReduction = DamageReductionBase + skillLevel * DamageReductionPerLevel;

			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var buffSkill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				multiplierReduction *= 1f + SCR_Get_AbilityReinforceRate(buffSkill);
			}

			multiplierReduction = Math.Min(0.80f, multiplierReduction);

			if (!target.IsAbilityActive(AbilityId.Peltasta39))
				modifier.DamageMultiplier *= (1f - multiplierReduction);
		}

		/// <summary>
		/// Damage reflect
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Guardian_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Guardian_Buff, out var buff))
				return;

			var skillLevel = buff.NumArg1;
			var multiplierReduction = DamageReductionBase + skillLevel * DamageReductionPerLevel;

			var reflectedDamage = skillHitResult.Damage * multiplierReduction;

			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var buffSkill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				reflectedDamage *= 1f + SCR_Get_AbilityReinforceRate(buffSkill);
			}

			reflectedDamage = Math.Min(1.9f, reflectedDamage);

			// Peltasta39 turns this into a damage reflect instead,
			// though the damage taken isn't reduced.
			if (target.IsAbilityActive(AbilityId.Peltasta39))
				CallSafe(this.ReflectDamage(attacker, target, reflectedDamage));
		}

		/// <summary>
		/// Hits target with a reflected hit.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skillHitResult"></param>
		/// <param name="damage"></param>
		/// <returns></returns>
		private async Task ReflectDamage(ICombatEntity attacker, ICombatEntity target, float damage)
		{
			// We delay the reflect hit so the animation looks better
			await Task.Delay(100);

			attacker.TakeSimpleHit(damage, target, SkillId.None);
		}
	}
}
