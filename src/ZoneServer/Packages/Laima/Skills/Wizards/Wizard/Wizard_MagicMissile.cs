using System;
using System.Collections.Generic;
using System.Linq;
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
using Yggdrasil.Extensions;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Wizard
{
	/// <summary>
	/// Handles the Wizard skill Magic Missile.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wizard_MagicMissile)]
	public class Wizard_MagicMissileOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int BulletsPerUse = 3;
		private const float SubSplashAreaSize = 200;

		/// <summary>
		/// Handles the skill, shooting missiles at enemies.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 130, width: 60, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var aniTime = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = skill.Properties.HitDelay;

			var skillHits = new List<SkillHitInfo>();
			var hits = new List<HitInfo>();

			if (targets.Any())
			{
				for (var i = 0; i < BulletsPerUse; ++i)
				{
					var missileTarget = targets.Random();

					var skillHitResult = SCR_SkillHit(caster, missileTarget, skill);
					missileTarget.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, missileTarget, skill, skillHitResult, aniTime, skillHitDelay);
					skillHits.Add(skillHit);
				}
			}

			foreach (var skillHit in skillHits)
			{
				var sourceTarget = skillHit.Target;

				var subSplashArea = Square.Centered(sourceTarget.Position, caster.Direction, SubSplashAreaSize, SubSplashAreaSize / 2);
				var subTargets = caster.Map.GetAttackableEnemiesIn(caster, subSplashArea).Where(a => a != sourceTarget);

				if (!subTargets.Any())
					continue;

				var richochetBulletsPerHit = targets.Count - 1;

				for (var i = 0; i < richochetBulletsPerHit; ++i)
				{
					var subTarget = subTargets.Random();

					var skillHitResult = SCR_SkillHit(caster, sourceTarget, skill);
					subTarget.TakeDamage(skillHitResult.Damage, caster);

					var hit = new HitInfo(caster, subTarget, skill, skillHitResult.Damage, skillHitResult.Result);
					hits.Add(hit);

					Send.ZC_NORMAL.PlayForceEffect(hit.ForceId, caster, sourceTarget, subTarget, "I_force001_yellow", 1, "arrow_cast", "I_explosion004_yellow", 1, "arrow_blow", "SLOW", 150);
				}
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, skillHits);

			foreach (var hit in hits)
				Send.ZC_HIT_INFO(hit.Attacker, hit.Target, hit);
		}

		// A shot into a bunch of monsters. The character hit 3 different
		// monsters once and each monster creates at least 3 bullets of
		// its own, hitting other nearby monsters. The numbers are
		// shortened handles.
		// Notably, each original target gets hit 3 times in total. What's
		// still unclear is what "5 Bullets" in the description means.
		// If it were a limit, you would expect more than 4 hits out
		// of 829, because as the first ricochet source it should be
		// able to go up to the max.
		// 
		// character -> 829
		// character -> 805
		// character -> 460
		// 829 -> 460
		// 829 -> 810
		// 829 -> 805
		// 829 -> 460
		// 805 -> 810
		// 805 -> 886
		// 805 -> 829
		// 460 -> 805
		// 460 -> 829
		// 460 -> 810
		// 
		// Shot at 3 monsters
		// 
		// character -> 459
		// character -> 422
		// character -> 459
		// 459 -> 519
		// 459 -> 422
		// 422 -> 459
		// 422 -> 519
		// 459 -> 519
		// 459 -> 422
		// 
		// New theory. The numer of ricochets is the number of monsters
		// in the splash area - 1. If you have two targets, you get one
		// additional bullet out of each hit, matching up our findings
		// for hitting two targets. If you hit three targets, you get
		// two additional bullets, matching the test above.
		// The first log above is more difficult to explain, because
		// 829 sent four bullets and the others only three, but maybe
		// 829 had four targets in range and the others only three...?
		// Although it was a large group and that seems unlikely as well.
		// Regardless, there does appear to be some kind of scaling based
		// on the targets involved.
	}
}
