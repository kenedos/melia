using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Pyromancer
{
	/// <summary>
	/// Handler for the Pyromancer skill Fire Ball.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pyromancer_FireBall)]
	public class Pyromancer_FireBallOverride : IForceSkillHandler, IDynamicCasted
	{
		private const int MaxTargets = 4;
		private const int BurnDurationMilliseconds = 5000;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

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
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			skillHit.ForceId = ForceId.GetNew();
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			var splashArea = new Circle(farPos, 70);
			var hitDelay = 200;
			var damageDelay = 400;
			var hits = new List<SkillHitInfo>();

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
				.Where(t => t != target)
				.OrderBy(t => t.Position.Get2DDistance(farPos))
				.Take(MaxTargets - 1)
				.ToList();

			targets.Insert(0, target);

			var burnChance = 0;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Pyromancer1, out var abilityLevel))
				burnChance = abilityLevel * 10;

			var rng = RandomProvider.Get();

			foreach (var currentTarget in targets)
			{
				var splashHitResult = SCR_SkillHit(caster, currentTarget, skill);
				currentTarget.TakeDamage(splashHitResult.Damage, caster);
				var splashHit = new SkillHitInfo(caster, currentTarget, skill, splashHitResult, TimeSpan.FromMilliseconds(damageDelay), TimeSpan.FromMilliseconds(hitDelay));
				hits.Add(splashHit);

				if ((rng.Next(100) < burnChance) && splashHitResult.Damage > 0)
					this.ApplyFireBuff(caster, skill, currentTarget, Math.Max(1, splashHitResult.Damage / 10));
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Ability effect to make targets burn
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <param name="hits"></param>
		/// <param name="value"></param>
		private void ApplyFireBuff(ICombatEntity caster, Skill skill, ICombatEntity target, float damage)
		{
			var buffDuration = BurnDurationMilliseconds;

			target.StartBuff(BuffId.Fire, skill.Level, damage, TimeSpan.FromMilliseconds(buffDuration), caster);
		}
	}
}
