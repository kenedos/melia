using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Shared.Util.TaskHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handler for the Doppelsoeldner skill Zwerchhau.
	/// </summary>
	[SkillHandler(SkillId.Doppelsoeldner_Zwerchhau)]
	public class Doppelsoeldner_Zwerchhau : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 0, angle: 50);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var hitDelay = TimeSpan.FromMilliseconds(170);
			var aniTime = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(hitDelay);

			var hits = new List<SkillHitInfo>();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hitSomething = false;
			var vintActive = caster.IsAbilityActive(AbilityId.Doppelsoeldner38);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(3);

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);

				// Vint pulls each successfully hit target towards the caster. The
				// attack is represented by one SkillHitInfo with MultiHit(3), so
				// the displacement is applied only once per target.
				if (vintActive
					&& skillHitResult.Damage > 0
					&& !target.IsDead
					&& target.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(
						caster,
						target,
						KnockBackType.KnockDown,
						50,
						10,
						KnockDirection.TowardsCaster
					);
					skillHit.HitInfo.KnockBackType = KnockBackType.KnockDown;
					target.ApplyKnockdown(caster, skill, skillHit);
				}
				else
				{
					skillHit.HitEffect = HitEffect.Impact;
				}

				hits.Add(skillHit);
				hitSomething = true;
			}

			if (caster.IsAbilityActive(AbilityId.Doppelsoeldner26) && hitSomething)
			{
				var duration = TimeSpan.FromSeconds(3);
				caster.StartBuff(BuffId.Zucken_Buff, skill.Level, 0, duration, caster);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Reduces Zwerchhau's maximum overheat to one while Zwerchhau: Vint is active.
		/// </summary>
		[SkillOverheatOverride(SkillId.Doppelsoeldner_Zwerchhau)]
		public float GetOverheatMaxCount(Skill skill)
		{
			return skill.Owner.IsAbilityActive(AbilityId.Doppelsoeldner38)
				? 1
				: skill.Data.OverheatCount;
		}
	}
}
