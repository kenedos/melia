using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Retreat Shot buff, which keeps firing behind the
	/// caster while it is active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.RetreatShot)]
	public class SchwarzerReiter_RetreatShot_BuffOverride : BuffHandler
	{
		private const float FireDistance = 150f;
		private const float FireWidth = 40f;
		private const int MaxTargets = 15;
		private const float BehindAngle = 180f;

		public override void WhileActive(Buff buff)
		{
			var caster = buff.Target;

			if (caster == null || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.Schwarzereiter_RetreatShot, out var skill))
				return;

			var targets = SkillSelectEnemiesInSquare(caster, caster.Position, BehindAngle, FireDistance, FireWidth, MaxTargets);
			if (targets.Count == 0)
				return;

			SkillTargetDamage(skill, caster, targets);
		}
	}
}
