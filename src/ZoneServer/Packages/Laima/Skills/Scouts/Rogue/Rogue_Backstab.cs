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

namespace Melia.Zone.Skills.Handlers.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Rogue skill Backstab.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rogue_Backstab)]
	public class Rogue_BackstabOverride : IMeleeGroundSkillHandler
	{
		private const float BackAttackAngle = 90f;
		private const float BackAttackDamageMultiplier = 2.0f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 25, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var hitDelay = TimeSpan.FromMilliseconds(50);
			var damageDelay = TimeSpan.FromMilliseconds(250);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(hitDelay);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.Default;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				if (caster.IsBehind(target, BackAttackAngle) || modifier.ForcedBackAttack)
				{
					skillHitResult.Damage *= (1 + BackAttackDamageMultiplier);
					Send.ZC_NORMAL.PlayTextEffect(target, caster, "SHOW_CUSTOM_TEXT", 50, "Backstab!");
				}

				if (skillHitResult.Result == HitResultType.Crit)
					skillHitResult.Damage *= 1.5f;

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
