using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Shared.Util;
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
	[Package("laima-skills")]
	[SkillHandler(SkillId.Pyromancer_FireBall)]
	public class Pyromancer_FireBallOverride : IForceSkillHandler, IDynamicCasted
	{
		private const int MaxTargets = 4;
		private const int BurnDurationMilliseconds = 5000;
		private const int SplashRange = 70;

		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(200);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.CanDamage(target))
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);
				return;
			}

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);

			target.TakeDamage(skillHitResult.Damage, caster);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			skill.Run(this.HandleExplosion(skill, caster, target, farPos));
		}

		/// <summary>
		/// Applies the fire ball's explosion damage after the projectile
		/// impact delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="farPos"></param>
		private async Task HandleExplosion(Skill skill, ICombatEntity caster, ICombatEntity target, Position farPos)
		{
			await skill.Wait(this.DamageDelay);

			if (caster.IsDead)
				return;

			var splashArea = new Circle(farPos, SplashRange);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
				.Where(t => t != target)
				.OrderBy(t => t.Position.Get2DDistance(farPos))
				.Take(MaxTargets - 1)
				.ToList();

			targets.Insert(0, target);

			var burnChance = 0;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Pyromancer1, out var abilityLevel))
				burnChance = abilityLevel * 10;

			var hits = new List<SkillHitInfo>();

			foreach (var currentTarget in targets)
			{
				if (currentTarget.IsDead || !caster.CanDamage(currentTarget))
					continue;

				var splashHitResult = SCR_SkillHit(caster, currentTarget, skill);
				var splashHit = new SkillHitInfo(caster, currentTarget, skill, splashHitResult, TimeSpan.Zero, TimeSpan.Zero);

				currentTarget.TakeDamage(splashHitResult.Damage, caster);
				hits.Add(splashHit);

				if ((GameRandom.Get().Next(100) < burnChance) && splashHitResult.Damage > 0)
					this.ApplyFireBuff(caster, skill, currentTarget, Math.Max(1, splashHitResult.Damage / 10));
			}

			if (hits.Count > 0)
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
