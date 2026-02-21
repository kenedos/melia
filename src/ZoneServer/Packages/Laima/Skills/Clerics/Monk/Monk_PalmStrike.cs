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
	/// Handler for the Monk skill Palm Strike.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Monk_PalmStrike)]
	public class Monk_PalmStrikeOverride : IMeleeGroundSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(350);
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
			await skill.Wait(TimeSpan.FromMilliseconds(400));

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 55, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 150;
			var damageDelay = 350;
			Debug.ShowShape(caster.Map, splashArea, TimeSpan.FromMilliseconds(500));

			await skill.Wait(TimeSpan.FromMilliseconds(hitDelay));

			if (caster.IsDead) return;

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();
			var skillHitDelay = skill.Properties.HitDelay;

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				if (!caster.IsEnemy(target))
					continue;

				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(damageDelay), skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				if (skillHitResult.Damage > 0 && target.IsKnockdownable())
				{
					if (caster.IsAbilityActive(AbilityId.Monk3))
						skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, skillHit.Target, HitType.KnockDown, 70, 90);
					else
						skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, skillHit.Target, HitType.KnockDown, 180, 33);

					skillHit.HitInfo.Type = HitType.KnockDown;
					target.ApplyKnockdown(caster, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
			caster.StartBuff(BuffId.HandKnife_Buff, 1f, 0f, TimeSpan.FromMilliseconds(2000f), caster);
		}
	}
}
