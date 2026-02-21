using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Corsair skill Impale Dagger.
	/// Dashes forward and impales enemies, carrying them along
	/// while dealing multiple hits.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Corsair_ImpaleDagger)]
	public class Corsair_ImpaleDaggerOverride : IMeleeGroundSkillHandler
	{
		private const float MaxDashDistance = 50f;
		private const int HitCount = 4;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(skill, caster, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(350));

			// Get closest target before movement
			var splashParam = skill.GetSplashParameters(caster, caster.Position, farPos, length: 50f, width: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var primaryTarget = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
				.OrderBy(t => caster.Position.Get2DDistance(t.Position))
				.FirstOrDefault();

			var dashDistance = MaxDashDistance;
			if (primaryTarget != null)
			{
				var distanceToTarget = (float)caster.Position.Get2DDistance(primaryTarget.Position);
				dashDistance = Math.Min(distanceToTarget - 25, MaxDashDistance);
				dashDistance = Math.Max(0, dashDistance);
			}

			var moveTargets = primaryTarget != null ? new List<ICombatEntity> { primaryTarget } : new List<ICombatEntity>();

			if (dashDistance > 0)
			{
				caster.Position = caster.Position.GetRelative(caster.Direction, dashDistance);
				SkillTargetMove(skill, caster, moveTargets, 0f, dashDistance, 0f, 0f, 0f, 0f, 0.2f, 0.2f, 0);
			}

			// Get targets after movement for damage
			var newFarPos = caster.Position.GetRelative(caster.Direction, 50f);
			splashParam = skill.GetSplashParameters(caster, caster.Position, newFarPos, length: 50f, width: 30f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aoeTargets = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
				.OrderByDescending(t => t.IsBuffActive(BuffId.IronHooked))
				.LimitBySDR(caster, skill)
				.ToList();

			for (var i = 0; i < HitCount; i++)
			{
				foreach (var target in aoeTargets)
				{
					if (target.IsDead)
						continue;

					var modifier = SkillModifier.Default;

					if (target.IsBuffActive(BuffId.IronHooked))
						modifier.CritRateMultiplier += 2.0f;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
					skillHit.HitEffect = HitEffect.Impact;

					Send.ZC_HIT_INFO(caster, target, skillHit.HitInfo);
				}

				if (i < HitCount - 1)
					await skill.Wait(TimeSpan.FromMilliseconds(130));
			}
		}

		private static void SkillTargetMove(Skill skill, ICombatEntity caster, List<ICombatEntity> targets,
			float jumpHeight, float distance, float angle, float spdChangeRate,
			float time1, float easing1, float time2, float easing2, int usedByTarget)
		{
			if (caster == null || caster.IsDead)
				return;

			if (targets == null)
				return;

			var position = caster.Position;
			float distRate = 1;

			if (usedByTarget == 1 && targets.Count != 0)
			{
				foreach (var target in targets)
				{
					if (target == null || target.IsDead)
						continue;

					position = target.Position;
					var dist = (float)caster.Position.Get2DDistance(target.Position);
					distRate = dist / distance;
					target.SetTempVar("CHARGE_DIST", distRate);
				}
			}

			spdChangeRate *= distRate;
			time1 *= distRate;
			time2 *= distRate;
			Send.ZC_NORMAL.LeapJump(caster, position, jumpHeight, spdChangeRate, time1, easing1, time2, easing2);
		}
	}
}
