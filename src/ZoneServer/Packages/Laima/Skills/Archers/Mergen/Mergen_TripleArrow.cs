using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Archers.Mergen
{
	[Package("laima")]
	[SkillHandler(SkillId.Mergen_TrickShot)]
	public class Mergen_TripleArrow : IGroundSkillHandler
	{
		private const int BaseArrowCount = 3;
		private const int MaximumArrowCount = 6;
		private const int MaximumExplosionTargets = 6;
		private const int MaximumEnhanceLevel = 100;
		private const float ExplosionRange = 120;
		private const float TripleDamageMultiplier = 0.50f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster == null || caster.IsDead)
				return;

			var primaryTarget = selectedTarget;

			if (primaryTarget == null || primaryTarget.IsDead)
				primaryTarget = caster.Map.GetAttackableEnemiesInPosition(caster, farPos, 20).Where(enemy => enemy != null && !enemy.IsDead).OrderBy(enemy => farPos.Get2DDistance(enemy.Position)).FirstOrDefault();

			if (primaryTarget == null)
			{
				caster.ServerMessage(Localization.Get("No valid target was found."));
				this.CancelSkill(skill, caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.CancelSkill(skill, caster);
				return;
			}

			var explosionPosition = primaryTarget.Position;

			caster.TurnTowards(explosionPosition);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, explosionPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, primaryTarget.Handle, originPos, originPos.GetDirection(explosionPosition), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, explosionPosition);

			var hasTriple = caster is Character character && character.IsAbilityActive(AbilityId.Mergen13);
			var arrowCount = this.GetArrowCount(caster, hasTriple);
			var damageMultiplier = this.GetEnhanceMultiplier(caster);
			damageMultiplier *= MergenZenithHelper.GetDamageMultiplier(caster);

			if (hasTriple)
				damageMultiplier *= TripleDamageMultiplier;

			var maximumExplosionTargets = MergenZenithHelper.GetMaximumAoeTargets(caster, MaximumExplosionTargets);
			var explosionArea = new Circle(explosionPosition, ExplosionRange);
			var explosionTargets = caster.Map
				.GetAttackableEnemiesIn(caster, explosionArea)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => explosionPosition.Get2DDistance(target.Position))
				.Take(maximumExplosionTargets)
				.ToList();

			if (!explosionTargets.Contains(primaryTarget))
			{
				explosionTargets.Insert(0, primaryTarget);

				if (explosionTargets.Count > maximumExplosionTargets)
					explosionTargets.RemoveAt(explosionTargets.Count - 1);
			}

			var hits = new List<SkillHitInfo>();

			for (var arrowIndex = 0; arrowIndex < arrowCount; arrowIndex++)
			{
				foreach (var target in explosionTargets)
				{
					if (target == null || target.IsDead)
						continue;

					var modifier = SkillModifier.Default;
					modifier.DamageMultiplier *= damageMultiplier;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

					if (skillHitResult.Result == HitResultType.Dodge)
						continue;

					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, skill.Data.DefaultHitDelay, skill.Data.DefaultHitDelay);
					skillHit.HitEffect = HitEffect.Impact;
					hits.Add(skillHit);
				}
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			skill.IncreaseOverheat();
			caster.SetAttackState(false);
		}

		private int GetArrowCount(ICombatEntity caster, bool hasTriple)
		{
			if (!hasTriple)
				return BaseArrowCount;

			var aoeAttackRatio = caster.Properties.GetFloat(PropertyName.SR);
			var additionalArrows = Math.Max(0, (int)MathF.Floor(aoeAttackRatio / 3f));

			return Math.Clamp(
				BaseArrowCount + additionalArrows,
				BaseArrowCount,
				MaximumArrowCount
			);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character || !character.TryGetAbility(AbilityId.Mergen5, out var ability))
				return 1f;

			var level = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var bonusPercent = level * 0.5f;

			if (level >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + (bonusPercent / 100f);
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}
	}
}
