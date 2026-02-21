using System;
using System.Collections.Generic;
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

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Spiral Arrow.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_SpiralArrow)]
	public class Ranger_SpiralArrowOverride : IForceSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Called when the user starts casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the user stops casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles the skill, attacking targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			// Ranger45 drastically changes the way this skill works
			// It gains cast time and becomes a melee ground skill
			if (caster.IsAbilityActive(AbilityId.Ranger45))
			{
				var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 10, angle: 0);
				var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

				Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

				skill.Run(this.MultiAttack(skill, caster, splashArea));
			}
			else
			{
				this.SingleAttack(skill, caster, target);
			}
		}

		/// <summary>
		/// Executes the main version of the skill
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		public void SingleAttack(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (target == null)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill, null);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill, null);
				return;
			}

			var damageDelay = TimeSpan.FromMilliseconds(300);
			var skillHitDelay = TimeSpan.Zero;

			var modifier = SkillModifier.MultiHit(3);
			modifier.DefensePenetrationRate = 0.5f;

			if (caster.TryGetBuff(BuffId.SpiralArrow_Debuff, out var buff))
				modifier.HitCount += buff.OverbuffCounter;

			if (caster.TryGetAbility(AbilityId.Ranger6, out var ability))
				modifier.CritRateMultiplier += 0.10f * ability.Level;

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();

			if (skillHitResult.Damage > 0)
			{
				// Bizarrely, this is correct. SpiralArrow_Buff is unused,
				// and the "debuff" is applied to the caster.
				// It seems at one point this applied to targets instead.
				caster.StartBuff(BuffId.SpiralArrow_Debuff, skill.Level, 0, TimeSpan.FromSeconds(3), caster);
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			Ranger_CriticalShotOverride.TryActivateDoubleTake(skill, caster, target);

			caster.StartBuff(BuffId.Ranger_StrapingShot, skill.Level, 0, TimeSpan.FromSeconds(3), caster);
		}

		/// <summary>
		/// Executes the alternate version of the skill
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task MultiAttack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var hitDelay = TimeSpan.FromMilliseconds(100);
			var damageDelay = TimeSpan.FromMilliseconds(100);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(hitDelay);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var results = new List<SkillHitResult>();
			var hitTargets = new List<ICombatEntity>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(3);
				modifier.DefensePenetrationRate = 0.5f;

				if (caster.TryGetBuff(BuffId.SpiralArrow_Debuff, out var buff))
					modifier.HitCount += buff.OverbuffCounter;

				if (caster.TryGetAbility(AbilityId.Ranger6, out var ability))
					modifier.CritRateMultiplier += 0.10f * ability.Level;

				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);
				results.Add(skillHitResult);

				var hit = new HitInfo(caster, target, skill, skillHitResult);
				hit.ForceId = ForceId.GetNew();
				hit.ResultType = HitResultType.NoHitScript;

				if (skillHitResult.Damage > 0)
				{
					hitTargets.Add(target);
				}

				Send.ZC_NORMAL.PlayForceEffect(hit.ForceId, caster, caster, target, "I_arrow009_spiralarrow#Dummy_q_Force", 0.2f, "arrow_cast", "F_hit_good", 1, "arrow_blow", "SLOW", 800);
				Send.ZC_HIT_INFO(caster, target, hit);
			}

			if (hitTargets.Count != 0)
				caster.StartBuff(BuffId.SpiralArrow_Debuff, skill.Level, 0, TimeSpan.FromSeconds(30), caster);

			Ranger_CriticalShot.TryActivateDoubleTake(skill, caster, hitTargets);
			Ranger_CriticalShot.TryReduceCooldown(skill, caster, results);
			Ranger_Strafe.TryApplyStrafeBuff(caster);
		}
	}
}
