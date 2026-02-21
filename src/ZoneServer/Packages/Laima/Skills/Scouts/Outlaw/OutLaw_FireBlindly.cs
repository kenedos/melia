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

namespace Melia.Zone.Skills.Handlers.Scouts.OutLaw
{
	/// <summary>
	/// Handler for the Outlaw skill Blindfire
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.OutLaw_FireBlindly)]
	public class OutLaw_FireBlindlyOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 40, angle: 180);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Executes the actual attack after a delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var hitDelay = TimeSpan.FromMilliseconds(360);
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			// Outlaw9 gives guaranteed evade throughout the whole animation
			if (caster.IsAbilityActive(AbilityId.Outlaw9))
				caster.StartBuff(BuffId.FireBlindly_Buff, skill.Level, 0, TimeSpan.FromMilliseconds(600), caster);

			// First attack hits with no delay

			var hits = new List<SkillHitInfo>();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			var obliqueFireDamageMultiplier = 1f;
			if (caster.TryGetBuff(BuffId.ObliqueFire_Buff, out var obliqueFireBuff))
			{
				obliqueFireDamageMultiplier += 0.05f * obliqueFireBuff.OverbuffCounter;
				caster.RemoveBuff(BuffId.ObliqueFire_Buff);
			}

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(4);
				modifier.DamageMultiplier *= obliqueFireDamageMultiplier;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				// Outlaw10 adds 30% damage on a critical
				if (caster.IsAbilityActive(AbilityId.Outlaw10) && skillHitResult.Result == HitResultType.Crit)
					skillHitResult.Damage *= 1.3f;

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;
				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
			hits.Clear();
			await skill.Wait(hitDelay);

			targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(4);
				modifier.DamageMultiplier *= obliqueFireDamageMultiplier;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				// Outlaw10 adds 30% damage on a critical
				if (caster.IsAbilityActive(AbilityId.Outlaw10) && skillHitResult.Result == HitResultType.Crit)
					skillHitResult.Damage *= 1.3f;

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;
				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
