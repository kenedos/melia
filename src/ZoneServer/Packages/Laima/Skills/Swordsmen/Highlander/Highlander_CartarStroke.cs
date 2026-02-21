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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Swordsman.Highlander
{
	/// <summary>
	/// Handler for the Highlander skill Cartar Stroke.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Highlander_CartarStroke)]
	public class Highlander_CartarStrokeOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int StunDurationMilliseconds = 3000;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 35, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

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
			var hitDelay = TimeSpan.FromMilliseconds(600);
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;
			var delayBetweenHits = TimeSpan.FromMilliseconds(150);

			await skill.Wait(hitDelay);

			List<ICombatEntity> targets;
			var hits = new List<SkillHitInfo>();

			for (var i = 0; i < 3; i++)
			{
				targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

				foreach (var target in targets.LimitBySDR(caster, skill))
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
					skillHit.HitEffect = HitEffect.Impact;

					hits.Add(skillHit);
				}

				Send.ZC_SKILL_HIT_INFO(caster, hits);

				hits.Clear();
				await skill.Wait(delayBetweenHits);
			}

			targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);

				target.StartBuff(BuffId.Stun, TimeSpan.FromMilliseconds(StunDurationMilliseconds));
				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
