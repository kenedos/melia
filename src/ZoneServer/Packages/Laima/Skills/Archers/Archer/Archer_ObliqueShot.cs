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
using Melia.Zone.Skills.Handlers.Archers.Ranger;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Archer
{
	/// <summary>
	/// Handles the Archer skill Oblique Shot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Archer_ObliqueShot)]
	public class Archer_ObliqueShotOverride : IForceSkillHandler
	{
		/// <summary>
		/// Handles the skill, shoot missile at enemy that spreads to another target.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var damageDelay = TimeSpan.FromMilliseconds(45);
			var skillHitDelay = TimeSpan.Zero;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			Ranger_CriticalShotOverride.TryActivateDoubleTake(skill, caster, target);

			if (skillHitResult.Damage <= 0)
				return;

			skill.Run(this.HandleSkill(caster, skill, target));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, ICombatEntity target)
		{
			var bounceHitDelay = TimeSpan.FromMilliseconds(220);
			await skill.Wait(bounceHitDelay);

			// Bounce on up to 3 additional targets
			var hitTargets = new List<ICombatEntity> { target };
			var lastTarget = target;
			for (var i = 0; i < 3; i++)
			{
				if (this.TryGetBounceTarget(caster, lastTarget, skill, hitTargets, out var bounceTarget))
				{
					var skillHitResult = SCR_SkillHit(caster, bounceTarget, skill);
					bounceTarget.TakeDamage(skillHitResult.Damage, caster);

					var hit = new HitInfo(caster, bounceTarget, skill, skillHitResult);
					hit.ForceId = ForceId.GetNew();

					Send.ZC_NORMAL.PlayForceEffect(hit.ForceId, caster, lastTarget, bounceTarget, "I_arrow009_red", 0.7f, "arrow_cast", "F_hit_good", 1, "arrow_blow", "SLOW", 800);
					Send.ZC_HIT_INFO(caster, bounceTarget, hit);

					if (skillHitResult.Damage <= 0)
						return;

					hitTargets.Add(bounceTarget);
					lastTarget = bounceTarget;
				}
				else
				{
					break;
				}
			}
		}

		/// <summary>
		/// Returns the closest target to the main target to bounce the
		/// attack off to.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="mainTarget"></param>
		/// <param name="skill"></param>
		/// <param name="bounceTarget"></param>
		/// <returns></returns>
		private bool TryGetBounceTarget(ICombatEntity caster, ICombatEntity mainTarget, Skill skill, List<ICombatEntity> hitTargets, out ICombatEntity bounceTarget)
		{
			var splashRadius = 100;
			var splashArea = new Circle(mainTarget.Position, splashRadius);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			if (!targets.Any())
			{
				bounceTarget = null;
				return false;
			}

			bounceTarget = targets.Where(a => !hitTargets.Contains(a))
								  .OrderBy(a => a.Position.Get2DDistance(mainTarget.Position))
								  .FirstOrDefault();

			return bounceTarget != null;
		}
	}
}
