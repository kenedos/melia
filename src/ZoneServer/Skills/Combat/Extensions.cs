using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Combat
{
	/// <summary>
	/// Extensions that help with handling skills.
	/// </summary>
	public static class SkillHelperExtensions
	{
		/// <summary>
		/// Iterates over targets and returns them until the skill's SR
		/// limit is reached.
		/// </summary>
		/// <param name="targets"></param>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		public static IEnumerable<ICombatEntity> LimitBySDR(this IEnumerable<ICombatEntity> targets, ICombatEntity caster, Skill skill)
		{
			var sr = skill.Properties.GetFloat(PropertyName.SkillSR);

			// Crank up SR if SDR is disabled, so we can effectively
			// hit all the targets
			if (ZoneServer.Instance.Conf.World.DisableSDR)
				sr = int.MaxValue;

			// Executes and returns a target at least once and then
			// keeps subtracting SDR from the skill's SR until it
			// reaches 0. Once it does, no more targets can be hit
			// and we return.
			// The targets are ordered by their SDR, so the ones with
			// the highest SDR are hit first and are able to potentially
			// tank the hit before it can hit other targets.

			targets = targets.OrderByDescending(a => a.Properties.GetFloat(PropertyName.SDR));

			foreach (var target in targets)
			{
				var sdr = target.Properties.GetFloat(PropertyName.SDR);
				yield return target;

				sr -= sdr;
				if (sr <= 0)
					break;
			}
		}

		/// <summary>
		/// Iterates over targets in a random order and returns them up
		/// to the the max amount.
		/// </summary>
		/// <param name="targets"></param>
		/// <param name="maxAmount"></param>
		/// <returns></returns>
		public static IEnumerable<ICombatEntity> LimitRandom(this IEnumerable<ICombatEntity> targets, int maxAmount)
		{
			var rnd = RandomProvider.Get();
			targets = targets.OrderBy(a => rnd.Next());

			return targets.Limit(maxAmount);
		}

		/// <summary>
		/// Iterates over targets and returns them up to the the max amount.
		/// </summary>
		/// <param name="targets"></param>
		/// <param name="maxAmount"></param>
		/// <returns></returns>
		public static IEnumerable<ICombatEntity> Limit(this IEnumerable<ICombatEntity> targets, int maxAmount)
		{
			var i = 0;
			foreach (var target in targets)
			{
				yield return target;

				if (++i >= maxAmount)
					break;
			}
		}

		/// <summary>
		/// Returns a random target near the main target to bounce the attack off to.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="mainTarget"></param>
		/// <param name="skill"></param>
		/// <param name="bounceTarget"></param>
		/// <returns></returns>
		public static bool TryGetBounceTarget(this ICombatEntity mainTarget, ICombatEntity caster, Skill skill, out ICombatEntity bounceTarget)
		{
			var splashPos = caster.Position;
			var splashParam = skill.GetSplashParameters(caster, splashPos, mainTarget.Position, length: 130, width: 60, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var eligibleTargets = targets.Where(a => a != mainTarget).ToList();

			if (!eligibleTargets.Any())
			{
				bounceTarget = null;
				return false;
			}

			// Sort targets by distance, but only consider the closest 50% of targets
			var sortedTargets = eligibleTargets.OrderBy(a => a.Position.Get2DDistance(mainTarget.Position)).ToList();
			var consideredTargetsCount = Math.Max(1, (int)(sortedTargets.Count * 0.5));
			var consideredTargets = sortedTargets.Take(consideredTargetsCount).ToList();

			// Randomly select one of the considered targets
			var rnd = new Random();
			bounceTarget = consideredTargets[rnd.Next(consideredTargets.Count)];
			return true;
		}
	}
}
