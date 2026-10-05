using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;
using static Melia.Shared.Util.TaskHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handler for the Doppelsoeldner skill Punish.
	/// </summary>
	[SkillHandler(SkillId.Doppelsoeldner_Punish)]
	public class Doppelsoeldner_Punish : IGroundSkillHandler
	{
		private const float MaxTargetDistance = 180f;
		private const float MaxMoveDistance = 140f;
		private const float MinimumSearchRadius = 100f;
		private const float KnockdownMultiplier = 1.5f;

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster == null || caster.IsDead || caster.Map == null)
				return;

			// Punish is ground-targeted and may arrive without a target. Resolve
			// the closest valid enemy around the selected position and prioritize
			// the packet target when it is part of that valid target set.
			var splashRadius = (float)skill.Data.SplashRange;
			var searchRadius = MathF.Max(MinimumSearchRadius, splashRadius);
			var nearbyTargets = caster.Map
				.GetAttackableEnemiesInPosition(caster, farPos, searchRadius)
				.Where(enemy => enemy != null && !enemy.IsDead)
				.Distinct()
				.ToList();

			var primaryTarget = target != null && nearbyTargets.Contains(target)
				? target
				: nearbyTargets
					.OrderBy(enemy => farPos.Get2DDistance(enemy.Position))
					.FirstOrDefault();

			if (primaryTarget == null)
			{
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			var targetPosition = primaryTarget.Position;
			var attackPosDist = (float)(caster.Position.Get2DDistance(targetPosition) - MaxTargetDistance);

			// Check distance before spending SP.
			if (attackPosDist > MaxMoveDistance)
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			Send.ZC_SKILL_READY(caster, skill, originPos, targetPosition);

			// If the caster is already in range, they won't move. Otherwise move
			// to the last valid ground position close to the selected enemy.
			if (attackPosDist > 0)
			{
				var endingPosition = caster.Position.GetRelative(targetPosition, attackPosDist);
				endingPosition = caster.Map.Ground.GetLastValidPosition(caster.Position, endingPosition);

				caster.SetPosition(endingPosition);
				Send.ZC_MOVE_STOP(caster, endingPosition, 1);
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(targetPosition);
			caster.SetAttackState(true);

			// Earthquake is now part of the base skill: the impact is centered on
			// the target and damages every valid enemy permitted by SDR.
			var splashArea = new Circle(targetPosition, splashRadius);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition, null);

			skill.Run(this.Attack(skill, caster, splashArea, primaryTarget));
		}

		/// <summary>
		/// Executes the actual attack after a delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea, ICombatEntity primaryTarget)
		{
			var hitDelay = TimeSpan.FromMilliseconds(700);
			var aniTime = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(hitDelay);

			if (caster.IsDead || caster.Map == null)
				return;

			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, splashArea)
				.Where(target => target != null && !target.IsDead)
				.Distinct()
				.OrderBy(target => target == primaryTarget ? 0 : 1)
				.ThenBy(target => primaryTarget.Position.Get2DDistance(target.Position));
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets.LimitBySDR(caster, skill).ToList())
			{
				var modifier = SkillModifier.MultiHit(2);

				if (target.IsStateActive(StateType.KnockedDown))
					modifier.DamageMultiplier = KnockdownMultiplier;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;
				hits.Add(skillHit);

				if (skillHitResult.Result != HitResultType.Dodge && skillHitResult.Damage > 0 && !target.IsDead)
				{
					target.StartBuff(BuffId.DecreaseHeal_Debuff, skill.Level, this.GetHealingReduction(skill), TimeSpan.FromSeconds(5), caster);
					this.PullTarget(skill, caster, target, skillHit);
				}
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private void PullTarget(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitInfo hit)
		{
			if (!target.IsKnockdownable() || target.Map != caster.Map)
				return;

			var destination = caster.Map.Ground.GetLastValidPosition(target.Position, caster.Position);
			var distance = target.Position.Get2DDistance(destination);
			if (distance <= 1f)
				return;

			var direction = target.Position.GetDirection(destination);
			var pullPower = Math.Max(1, (int)distance);
			hit.KnockBackInfo = new KnockBackInfo(target, KnockBackType.KnockBack, pullPower, 0, direction);
			hit.HitInfo.KnockBackType = KnockBackType.KnockBack;
			target.ApplyKnockback(caster, skill, hit);
		}

		/// <summary>
		/// Return the Healing Reduction value
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		private float GetHealingReduction(Skill skill)
		{
			return (3.3f * skill.Level) * 1000;
		}
	}
}


