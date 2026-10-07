using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for Seed Bomb's seed, which bursts on the enemies around the
	/// target when the target is hit or the seed runs out.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Hits of the burst
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Seedbomb_Buff)]
	public class Druid_Seedbomb_BuffOverride : BuffHandler
	{
		private const float BurstRange = 50f;

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (target.Map == null || buff.Caster is not ICombatEntity caster || caster.IsDead || !caster.TryGetSkill(SkillId.Druid_Seedbomb, out var skill))
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			var hitCount = Math.Max(1, (int)buff.NumArg2);
			var hits = new List<SkillHitInfo>();

			foreach (var victim in target.Map.GetAttackableEnemiesInPosition(caster, target.Position, BurstRange).Take(maxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, victim, skill, SkillModifier.MultiHit(hitCount));
				victim.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, victim, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Seedbomb_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id == SkillId.Druid_Seedbomb || skillHitResult.Damage <= 0)
				return;

			target.StopBuff(BuffId.Seedbomb_Buff);
		}
	}
}
