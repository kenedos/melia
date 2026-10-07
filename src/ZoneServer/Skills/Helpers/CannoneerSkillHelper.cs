using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Cannoneer's cannon skills.
	/// </summary>
	public static class CannoneerSkillHelper
	{
		private const float BazookaRange = 200f;
		private const float RangeLeeway = 1.25f;

		/// <summary>
		/// Returns true if the position is in range of the skill, which
		/// reaches 200 while Bazooka is active.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static bool InCannonRange(ICombatEntity caster, Skill skill, Position position)
		{
			if (!caster.IsBuffActive(BuffId.Bazooka_Buff))
				return caster.InSkillUseRange(skill, position);

			return caster.Position.InRange2D(position, BazookaRange * RangeLeeway);
		}

		/// <summary>
		/// Returns the enemies around the position the skill's AoE Attack
		/// Ratio lets it hit, the main target first.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		/// <param name="mainTarget"></param>
		/// <returns></returns>
		public static List<ICombatEntity> GetSplashTargets(ICombatEntity caster, Skill skill, Position position, float radius, ICombatEntity mainTarget)
		{
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, position, radius);

			if (mainTarget != null && !mainTarget.IsDead)
			{
				targets.Remove(mainTarget);
				targets.Insert(0, mainTarget);
			}

			return targets.LimitBySDR(caster, skill).ToList();
		}
	}
}
