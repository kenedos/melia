using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Scout
{
	/// <summary>
	/// Handles the Scout skill Oblique Fire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Scout_ObliqueFire)]
	public class Scout_ObliqueFireOverride : IForceSkillHandler
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

			caster.StartBuff(BuffId.ObliqueFire_Buff, skill.Level, 0, TimeSpan.FromSeconds(10), caster);

			skill.Run(this.Attack(skill, caster, target));
		}

		/// <summary>
		/// Executes the actual attack.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var damageDelay = TimeSpan.FromMilliseconds(270);
			var skillHitDelay = TimeSpan.Zero;
			var hitDelay = TimeSpan.FromMilliseconds(100);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, forceId, null);

			await skill.Wait(hitDelay);

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = forceId;

			Send.ZC_SKILL_HIT_INFO(caster, skillHit);

			if (skillHitResult.Damage <= 0)
				return;

			// Bounce on up to 1 additional target after the first
			var bounceHitDelay = TimeSpan.FromMilliseconds(220);
			await skill.Wait(bounceHitDelay);

			var hitTargets = new List<ICombatEntity> { target };
			var lastTarget = target;

			// Only bounce once (i < 1)
			for (var i = 0; i < 1; i++)
			{
				if (this.TryGetBounceTarget(caster, lastTarget, skill, hitTargets, out var bounceTarget))
				{
					var bounceSkillHitResult = SCR_SkillHit(caster, bounceTarget, skill);
					bounceTarget.TakeDamage(bounceSkillHitResult.Damage, caster);

					var hit = new HitInfo(caster, bounceTarget, skill, bounceSkillHitResult);
					hit.ForceId = ForceId.GetNew();

					Send.ZC_NORMAL.PlayForceEffect(hit.ForceId, caster, lastTarget, bounceTarget, "I_arrow009_red", 0.7f, "arrow_cast", "F_hit_good", 1, "arrow_blow", "SLOW", 800);
					Send.ZC_HIT_INFO(caster, bounceTarget, hit);

					if (bounceSkillHitResult.Damage <= 0)
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
		/// <param name="hitTargets"></param>
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
