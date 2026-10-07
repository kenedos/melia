using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Mergen
{
	/// <summary>
	/// Handler for the Mergen skill Targeted Arrow, which fires five
	/// arrows down a line once the charge is released.
	/// </summary>
	/// <remarks>
	/// With Homing Arrow: Shackle, monsters other than bosses that are hit
	/// can't move for 2 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Mergen_FocusFire)]
	public class Mergen_FocusFireOverride : IGroundSkillHandler
	{
		private const int ArrowCount = 5;
		private static readonly TimeSpan ArrowInterval = TimeSpan.FromMilliseconds(50);
		private static readonly TimeSpan ShackleDuration = TimeSpan.FromSeconds(2);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 60, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).LimitBySDR(caster, skill).ToList();
			var hasShackle = caster.IsAbilityActive(AbilityId.Mergen29);

			var hits = new List<SkillHitInfo>();

			for (var arrow = 0; arrow < ArrowCount; arrow++)
			{
				var targetIndex = 0;

				foreach (var hitTarget in targets.Where(t => !t.IsDead))
				{
					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, hitTarget, skill, skillHitResult, ArrowInterval * arrow, TimeSpan.Zero);
					skillHit.ForceId = ForceId.GetNew();
					skillHit.HitFrameIndex = (byte)arrow;
					skillHit.TargetIndex = (byte)targetIndex++;

					hits.Add(skillHit);

					if (hasShackle && skillHitResult.Damage > 0 && hitTarget.Rank != MonsterRank.Boss)
						hitTarget.StartBuff(BuffId.Common_Hold, 1, 0, ShackleDuration, caster, skill.Id);
				}
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
