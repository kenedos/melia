using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Combination.
	/// Coordinated attack where both caster and hawk strike the target simultaneously.
	/// Deals extra damage if the target is marked by Aiming.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Combination)]
	public class Falconer_CombinationOverride : IForceSkillHandler
	{
		private const float AimingBonusDamageMultiplier = 1.5f;

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

			var forceId = ForceId.GetNew();
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, forceId, null);

			skill.Run(this.HandleSkill(skill, caster, target, forceId));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, ICombatEntity target, int forceId)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(200));

			if (target.IsDead)
				return;

			var hits = new List<SkillHitInfo>();
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			// Check if target has Aiming debuff for bonus damage
			var hasAiming = target.IsBuffActive(BuffId.Aiming_Hawk_Buff);
			var modifier = new SkillModifier();

			if (hasAiming)
			{
				modifier.DamageMultiplier = AimingBonusDamageMultiplier;
				// Visual indicator for combo bonus
				target.PlayEffect("F_archer_combination_bonus", scale: 1f);
			}

			// First hit - Caster's strike
			var casterHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(casterHitResult.Damage, caster);

			var casterHit = new SkillHitInfo(caster, target, skill, casterHitResult, damageDelay, skillHitDelay);
			casterHit.ForceId = forceId;
			casterHit.HitEffect = HitEffect.Impact;
			hits.Add(casterHit);

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			// Short delay for hawk attack
			await skill.Wait(TimeSpan.FromMilliseconds(150));

			if (target.IsDead)
				return;

			hits.Clear();

			// Second hit - Hawk's strike
			target.PlayEffect("F_archer_combination_hawk", scale: 1f);

			var hawkHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(hawkHitResult.Damage, caster);

			var hawkHit = new SkillHitInfo(caster, target, skill, hawkHitResult, damageDelay, skillHitDelay);
			hawkHit.ForceId = ForceId.GetNew();
			hawkHit.HitEffect = HitEffect.Impact;
			hits.Add(hawkHit);

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			// Falconer9: Combination: Enhance - increased damage
			// Note: This bonus is typically applied via skill data reinforceAbility

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
