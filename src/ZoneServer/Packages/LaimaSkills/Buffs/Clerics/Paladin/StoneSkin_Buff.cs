using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Shared.Data.Database;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.HandlersOverrides.Clerics.Paladin
{
	/// <summary>
	/// Handler for the Stone Skin buff.
	/// Reduces physical damage taken.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.StoneSkin_Buff)]
	public class StoneSkin_BuffOverride : BuffHandler
	{

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.StoneSkin_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.StoneSkin_Buff, out var buff))
				return;

			// Only reduce physical damage (not magic)
			if (skill.Data.AttackType == SkillAttackType.Magic)
				return;

			var skillLevel = buff.NumArg1;
			var damageReduction = GetCaptionRatio(buff, 1) / 100f;

			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var buffSkill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				damageReduction *= 1f + SCR_Get_AbilityReinforceRate(buffSkill);
			}

			// Apply damage reduction (Cap at 90%)
			skillHitResult.Damage *= Math.Max(0.1f, (1f - damageReduction));
		}
	}
}
