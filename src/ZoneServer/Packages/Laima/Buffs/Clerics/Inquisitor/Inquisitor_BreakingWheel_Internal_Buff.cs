using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Inquisitor
{
	[Package("laima")]
	[BuffHandler(BuffId.Inquisitor_BreakingWheel_Internal_Buff)]
	public class Inquisitor_BreakingWheel_Internal_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const float ExpandedRange = 100f;
		private const int MaximumPropagationTargets = 5;
		private const float AdditionalDamageRate = 0.12f;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (buff.Target == null || buff.Target.IsDead || skillHitInfo == null || skillHitInfo.Target != buff.Target)
				return;

			if (buff.Caster is not Character caster || caster.IsDead || caster.Map == null)
				return;

			if (skillHitInfo.Attacker is not Character attacker || attacker.IsDead || attacker.Map != caster.Map || (attacker != caster && !attacker.IsAlly(caster)))
				return;

			if (!caster.IsAbilityActive(AbilityId.Inquisitor8) || skillHitInfo.HitInfo == null || skillHitInfo.HitInfo.Damage <= 0)
				return;

			this.ApplyAdditionalDamage(buff, caster, skillHitInfo.HitInfo.Damage);
		}

		private void ApplyAdditionalDamage(Buff buff, Character caster, float sourceDamage)
		{
			if (caster == null || caster.IsDead || caster.Map == null || buff.Target == null || buff.Target.IsDead || buff.Target.Map == null || buff.Target.Map != caster.Map)
				return;

			if (!caster.TryGetSkill(SkillId.Inquisitor_BreakingWheel, out var wheelSkill))
				wheelSkill = new Skill(caster, SkillId.Inquisitor_BreakingWheel, Math.Max(1, (int)buff.NumArg1));

			if (wheelSkill == null || wheelSkill.Data == null)
				return;

			var area = new Circle(buff.Target.Position, ExpandedRange);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead && target != buff.Target)
				.OrderBy(target => buff.Target.Position.Get2DDistance(target.Position))
				.Take(MaximumPropagationTargets)
				.ToList();

			if (targets.Count == 0)
				return;

			var additionalDamage = Math.Max(1, (int)(sourceDamage * AdditionalDamageRate));
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var result = SCR_SkillHit(caster, target, wheelSkill, SkillModifier.Default);

				if (result.Result == HitResultType.Dodge)
					result.Damage = 0;
				else
				{
					result.Damage = additionalDamage;
					target.TakeDamage(additionalDamage, caster);
				}

				hits.Add(new SkillHitInfo(caster, target, wheelSkill, result, HitAnimationTime, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
