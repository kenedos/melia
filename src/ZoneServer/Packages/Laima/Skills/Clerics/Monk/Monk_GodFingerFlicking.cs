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
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Monk
{
	/// <summary>
	/// Handler for the Monk skill God Finger Flicking.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Monk_God_Finger_Flicking)]
	public class Monk_God_Finger_FlickingOverride : IMeleeGroundSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(200);
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets?.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var damageDelay = 200;

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();
			var skillHitDelay = skill.Properties.HitDelay;

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				if (!caster.IsEnemy(target))
					continue;

				var modifier = new SkillModifier();

				if (target.IsBuffActive(BuffId.OneInchPunch_Debuff))
					modifier.DamageMultiplier += 1f;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(damageDelay), skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				if (skillHitResult.Damage > 0 && target.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, skillHit.Target, HitType.KnockBack, 250, 10);
					skillHit.HitInfo.Type = HitType.KnockBack;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
			caster.StartBuff(BuffId.God_Finger_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(30000f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
		}
	}
}
