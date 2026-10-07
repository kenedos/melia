using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the White Mask's poison puff, which hits the enemies
	/// around the Plague Doctor with Black Death Steam every second.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.WhiteBeakMask_Damage_Buff)]
	public class PlagueDoctor_WhiteBeakMask_Damage_BuffOverride : BuffHandler
	{
		private const float PuffRange = 80f;

		public override void WhileActive(Buff buff)
		{
			var caster = buff.Target;

			if (caster.IsDead || !caster.TryGetSkill(SkillId.PlagueDoctor_PlagueVapours, out var skill))
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			var hits = new List<SkillHitInfo>();

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, PuffRange).Take(maxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
