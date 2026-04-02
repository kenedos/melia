using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scout
{
	/// <summary>
	/// Handler for the Cloaking buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Cloaking_Buff)]
	public class Cloaking_BuffOverride : BuffHandler
	{
		/// <summary>
		/// Applies the buff's effects during the combat calculations.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		/// <exception cref="NotImplementedException"></exception>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Cloaking_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Cloaking_Buff, out var buff))
				return;

			var damageReduction = 0.30f + 0.06f * skill.Level;
			var byAbility = 1f;
			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var buffSkill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				byAbility += SCR_Get_AbilityReinforceRate(buffSkill);
			}
			damageReduction *= byAbility;

			damageReduction = (float)Math.Min(0.95, damageReduction);

			modifier.DamageMultiplier *= (1f - damageReduction);
		}

		/// <summary>
		/// Removes buff if cloaking user takes damage
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Cloaking_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Cloaking_Buff, out var buff))
				return;

			if (skillHitResult.Damage > 0)
			{
				target.StopBuffByTag(BuffTag.Cloaking);
			}
		}
	}
}
