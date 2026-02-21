using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Assassin
{
	/// <summary>
	/// Handler for the Assassin skill Behead.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Assassin_Behead)]
	public class Assassin_BeheadOverride : IMeleeGroundSkillHandler
	{
		private const float BackAttackAngle = 90f;
		private const float BackAttackDamageMultiplier = 0.5f;

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

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 20, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);

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
			// The damageDelay1 is unusually long, but confirmed with official.

			var hitDelay = TimeSpan.FromMilliseconds(30);
			var damageDelay1 = TimeSpan.FromMilliseconds(240);
			var damageDelay2 = TimeSpan.FromMilliseconds(80);
			var delayBetweenHits = TimeSpan.FromMilliseconds(330);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(hitDelay);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.Default;
				modifier.HitCount = 3;

				// Increase damage by 10% if target is under the effect of
				// Assassination Target from the caster
				if (target.TryGetBuff(BuffId.Assassin_Target_Debuff, out var assassinTargetDebuff))
				{
					if (assassinTargetDebuff.Caster == caster)
						modifier.DamageMultiplier += 0.10f;
				}

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				// Check back attack AFTER SCR_SkillHit so card hooks can set ForcedBackAttack
				if (caster.IsBehind(target, BackAttackAngle) || modifier.ForcedBackAttack)
				{
					skillHitResult.Damage *= (1 + BackAttackDamageMultiplier);
					Send.ZC_NORMAL.PlayTextEffect(target, caster, "SHOW_CUSTOM_TEXT", 50, "Backstab!");
				}

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay1, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			await skill.Wait(delayBetweenHits);
			hits.Clear();

			targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.Default;
				modifier.HitCount = 3;

				// Increase damage by 10% if target is under the effect of
				// Assassination Target from the caster
				if (target.TryGetBuff(BuffId.Assassin_Target_Debuff, out var assassinTargetDebuff))
				{
					if (assassinTargetDebuff.Caster == caster)
						modifier.DamageMultiplier += 0.10f;
				}

				var skillHitResult2 = SCR_SkillHit(caster, target, skill, modifier);

				// Check back attack AFTER SCR_SkillHit so card hooks can set ForcedBackAttack
				if (caster.IsBehind(target, BackAttackAngle) || modifier.ForcedBackAttack)
					skillHitResult2.Damage *= (1 + BackAttackDamageMultiplier);

				target.TakeDamage(skillHitResult2.Damage, caster);

				var skillHit2 = new SkillHitInfo(caster, target, skill, skillHitResult2, damageDelay2, skillHitDelay);
				skillHit2.HitEffect = HitEffect.Impact;

				hits.Add(skillHit2);

				var bleedingDamage = skillHitResult2.Damage * 0.25f;

				// Assassin6 instead does 5% of their maximum HP, or 5% of the caster's
				// maximum HP, whichever is less.  It also doesn't work on bosses
				if (target.Rank != MonsterRank.Boss && caster.IsAbilityActive(AbilityId.Assassin6))
				{
					bleedingDamage = MathF.Min(caster.Properties.GetFloat(PropertyName.MHP) * 0.05f, target.Properties.GetFloat(PropertyName.MHP) * 0.05f);
				}

				if (skillHitResult2.Damage > 0)
				{
					target.StartBuff(BuffId.Behead_Debuff, skill.Level, bleedingDamage, TimeSpan.FromSeconds(7), caster);

					// Assassin5 adds 5 seconds of silence
					if (caster.IsAbilityActive(AbilityId.Assassin5))
						target.StartBuff(BuffId.Common_Silence, skill.Level, bleedingDamage, TimeSpan.FromSeconds(5), caster);
				}
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
