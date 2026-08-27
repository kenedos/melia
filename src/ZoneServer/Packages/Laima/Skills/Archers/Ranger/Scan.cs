using System;
using System.Linq;
using System.Reflection.Emit;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Scan.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_Scan)]
	public class Ranger_ScanOverride : IGroundSkillHandler
	{

		// How much accuracy is converted to crit resist multiplier reduction
		// per skill level.
		// At skill level 10, reduces crit resist by 50% of caster's accuracy.

		private const float SpreadRadius = 50f;
		private const int MaxTargets = 5;

		/// <summary>
		/// Handles skill, applying a debuff to the target
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (target == null)
			{
				caster.ServerMessage(Localization.Get("No target specified."));
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, null);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var duration = TimeSpan.FromSeconds(60);
			var accuracy = caster.Properties.GetFloat(PropertyName.HR);
			var critResistReduce = skill.Properties.GetFloat(PropertyName.CaptionRatio) + accuracy * (skill.Properties.GetFloat(PropertyName.CaptionRatio2) / 100f);

			var splashArea = new Circle(target.Position, SpreadRadius);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea)
				.Where(a => a != target)
				.OrderBy(a => a.IsBuffActive(BuffId.Ranger_Scan_Debuff))
				.ThenBy(a => a.Position.Get2DDistance(target.Position))
				.Take(MaxTargets - 1)
				.Prepend(target);

			foreach (var scanTarget in targets)
				scanTarget.StartBuff(BuffId.Ranger_Scan_Debuff, skill.Level, critResistReduce, duration, caster, skill.Id);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, null);
		}
	}
}
