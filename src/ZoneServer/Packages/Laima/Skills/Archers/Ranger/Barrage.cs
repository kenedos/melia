using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using System.Threading.Tasks;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Barrage.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_Barrage)]
	public class BarrageOverride : IForceSkillHandler
	{
		/// <summary>
		/// Handles the skill, do two consecutive hits on the enemy and bounce to targets behind.
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

			var damageDelay = TimeSpan.FromMilliseconds(200);
			var skillHitDelay = skill.Properties.HitDelay;

			var modifier = SkillModifier.MultiHit(3);
			modifier.DamageMultiplier = 3;

			// Wild throw reduces base hit rate by 70% but cuts cooldown by 5s
			if (caster.IsAbilityActive(AbilityId.Ranger39))
			{
				modifier.HitRateMultiplier -= 0.7f;
				skill.ReduceCooldown(TimeSpan.FromSeconds(5));
			}

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();
			skillHit.UnkFloat = 8;
			skillHit.VarInfoCount = 1;

			// Add Knockback adds knockback only to the skill's final charge
			// (ie, the usage that puts the skill on overheat cooldown)
			if (skillHitResult.Damage > 0 && caster.TryGetActiveAbilityLevel(AbilityId.Ranger1, out var level) && skill.OverheatCounter == 0 && target.IsKnockdownable())
			{
				// TODO: The knockback power / scaling of this ability is unknown.
				skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, HitType.KnockBack, 50 + 5 * level, 10);
				skillHit.HitInfo.Type = HitType.KnockBack;
				target.ApplyKnockback(caster, skill, skillHit);
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			Ranger_CriticalShotOverride.TryActivateDoubleTake(skill, caster, target);
			Ranger_StrafeOverride.TryApplyStrafeBuff(caster);

			skill.Run(this.HandleSplashAttack(caster, skill, target));
		}

		private async Task HandleSplashAttack(ICombatEntity caster, Skill skill, ICombatEntity target)
		{
			// Splash shot
			var bounceHitDelay = TimeSpan.FromMilliseconds(320);
			await skill.Wait(bounceHitDelay);

			var hitTargets = new HashSet<ICombatEntity> { target };
			for (var i = 0; i < 6; i++)
			{
				if (this.TryGetBounceTarget(caster, target, skill, hitTargets, out var bounceTarget))
				{
					this.ApplyBounceShot(caster, skill, bounceTarget);
					hitTargets.Add(bounceTarget);
				}
			}
		}

		private void ApplyBounceShot(ICombatEntity caster, Skill skill, ICombatEntity bounceTarget)
		{
			var forceId = ForceId.GetNew();
			var splashEffect1 = "I_arrow009_red#Dummy_arrow";
			var scale1 = 0.7f;
			var splashEffect2 = "arrow_cast";
			var splashEffect3 = "F_archer_bodkinpoint_hit_explosion";
			var scale2 = 0.7f;
			var splashEffect4 = "arrow_blow";
			var splashEffect5 = "SLOW";
			var speed = 700f;
			Send.ZC_NORMAL.PlayForceEffect(caster, caster, bounceTarget, forceId, splashEffect1, scale1, splashEffect2, splashEffect3, scale2, splashEffect4, splashEffect5, speed, 1, 5, 10, 0);

			var skillEffect = "F_archer_shot_light_orange";
			var skillScale = 0.3f;
			var str1 = "Dummy_arrow_effect";
			var str2 = "None";
			Send.ZC_NORMAL.PlayEffectNode(caster, skillEffect, skillScale, str1, str2);

			var modifier = SkillModifier.MultiHit(3);
			var skillHitResult = SCR_SkillHit(caster, bounceTarget, skill, modifier);
			bounceTarget.TakeDamage(skillHitResult.Damage, caster);

			var hit = new HitInfo(caster, bounceTarget, skill, skillHitResult);
			hit.UnkFloat1 = -1f;
			Send.ZC_HIT_INFO(caster, bounceTarget, hit);
		}

		private bool TryGetBounceTarget(ICombatEntity caster, ICombatEntity mainTarget, Skill skill, HashSet<ICombatEntity> hitTargets, out ICombatEntity bounceTarget)
		{
			var splashPos = caster.Position;
			var splashParam = skill.GetSplashParameters(caster, splashPos, mainTarget.Position, length: 130, width: 60, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var eligibleTargets = targets.Where(a => a != mainTarget && !hitTargets.Contains(a) && this.IsBehindTarget(caster, mainTarget, a)).ToList();

			if (!eligibleTargets.Any())
			{
				bounceTarget = null;
				return false;
			}

			// Sort targets by distance
			var sortedTargets = eligibleTargets.OrderBy(a => a.Position.Get2DDistance(mainTarget.Position)).ToList();
			var consideredTargetsCount = Math.Max(1, (int)(sortedTargets.Count));
			var consideredTargets = sortedTargets.Take(consideredTargetsCount).ToList();

			// Randomly select one of the considered targets
			var rnd = new Random();
			bounceTarget = consideredTargets[rnd.Next(consideredTargets.Count)];
			return true;
		}

		private bool IsBehindTarget(ICombatEntity caster, ICombatEntity mainTarget, ICombatEntity potentialTarget)
		{
			var casterToMainVector = mainTarget.Position - caster.Position;
			var casterToPotentialVector = potentialTarget.Position - caster.Position;

			var dotProduct = casterToMainVector.X * casterToPotentialVector.X + casterToMainVector.Z * casterToPotentialVector.Z;

			return dotProduct > 0;
		}
	}
}
