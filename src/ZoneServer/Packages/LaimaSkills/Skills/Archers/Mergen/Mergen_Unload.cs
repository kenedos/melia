using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace Melia.Zone.Skills.Handlers.Archers.Mergen
{
	/// <summary>
	/// Handler for the Mergen skill Spread Shot, which fires five arrows
	/// at the enemies in front, each hit ricocheting to a nearby enemy.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Mergen_Unload)]
	public class Mergen_UnloadOverride : IGroundSkillHandler
	{
		private const int ArrowCount = 5;
		private const float RicochetRange = 50f;
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(180);
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(380);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos);
			var splashArea = skill.GetSplashArea(skill.Data.SplashType, splashParam);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Fires the arrows and their ricochets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			await skill.Wait(HitDelay);

			var hits = new List<SkillHitInfo>();
			var hitTargets = new HashSet<ICombatEntity>();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).LimitBySDR(caster, skill).ToList();

			foreach (var target in targets)
			{
				hits.Add(this.Hit(skill, caster, target, ArrowCount));
				hitTargets.Add(target);
			}

			foreach (var target in targets)
			{
				var ricochetTarget = caster.Map.GetAttackableEnemiesInPosition(caster, target.Position, RicochetRange)
					.FirstOrDefault(e => !hitTargets.Contains(e));

				if (ricochetTarget == null)
					continue;

				hits.Add(this.Hit(skill, caster, ricochetTarget, 1));
				hitTargets.Add(ricochetTarget);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Deals one arrow hit to the target.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="hitCount"></param>
		/// <returns></returns>
		private SkillHitInfo Hit(Skill skill, ICombatEntity caster, ICombatEntity target, int hitCount)
		{
			var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(hitCount));
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, AniTime, TimeSpan.Zero);
			skillHit.ForceId = ForceId.GetNew();

			return skillHit;
		}
	}
}
