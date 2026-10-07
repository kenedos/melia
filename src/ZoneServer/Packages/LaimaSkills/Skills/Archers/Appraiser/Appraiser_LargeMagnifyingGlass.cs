using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Appraiser skill Huge Magnifier, which strikes the
	/// enemies in front of the Appraiser four times.
	/// </summary>
	/// <remarks>
	/// When the main target is devalued, the devaluation spreads to every
	/// enemy hit and they take more damage for its remaining duration.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Appraiser_LargeMagnifyingGlass)]
	public class Appraiser_LargeMagnifyingGlassOverride : IGroundSkillHandler
	{
		private static readonly (int HitDelay, int AniTime)[] HitTimings = [(400, 200), (430, 30), (460, 30), (500, 40)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 40, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, splashArea, target));
		}

		/// <summary>
		/// Strikes the enemies in the area once per hit frame.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		/// <param name="mainTarget"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea, ICombatEntity mainTarget)
		{
			var hitTargets = new HashSet<ICombatEntity>();

			foreach (var timing in HitTimings)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(timing.AniTime));

				var hits = new List<SkillHitInfo>();
				var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

				foreach (var target in targets.LimitBySDR(caster, skill))
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);
					target.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(timing.HitDelay), TimeSpan.Zero));
					hitTargets.Add(target);
				}

				Send.ZC_SKILL_HIT_INFO(caster, hits);
			}

			this.SpreadDevaluation(skill, caster, mainTarget, hitTargets);
		}

		/// <summary>
		/// Spreads the main target's devaluation to the enemies that were hit.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="mainTarget"></param>
		/// <param name="hitTargets"></param>
		private void SpreadDevaluation(Skill skill, ICombatEntity caster, ICombatEntity mainTarget, IEnumerable<ICombatEntity> hitTargets)
		{
			if (mainTarget == null || !mainTarget.TryGetBuff(BuffId.Devaluation_Debuff, out var devaluation))
				return;

			var duration = devaluation.RemainingDuration;
			if (duration <= TimeSpan.Zero)
				return;

			foreach (var target in hitTargets.Where(t => !t.IsDead))
			{
				if (target != mainTarget)
					target.StartBuff(BuffId.Devaluation_Debuff, devaluation.NumArg1, 0, duration, caster, SkillId.Appraiser_Devaluation);

				target.StartBuff(BuffId.Devaluation_Damage_Debuff, skill.Level, 0, duration, caster, skill.Id);
			}
		}
	}
}
