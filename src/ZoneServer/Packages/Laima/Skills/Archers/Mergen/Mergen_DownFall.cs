using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Archers.Mergen
{
	[Package("laima")]
	[SkillHandler(SkillId.Mergen_DownFall)]
	public class Mergen_DownFall : IGroundSkillHandler
	{
		private const int MaximumSkillLevel = 10;
		private const float TargetSearchRange = 120;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster == null || caster.IsDead)
				return;

			var primaryTarget = target;

			if (primaryTarget == null || primaryTarget.IsDead)
				primaryTarget = caster.Map.GetAttackableEnemiesInPosition(caster, farPos, TargetSearchRange).Where(enemy => enemy != null && !enemy.IsDead).OrderBy(enemy => farPos.Get2DDistance(enemy.Position)).FirstOrDefault();

			if (primaryTarget == null)
			{
				caster.ServerMessage(Localization.Get("No valid target was found."));
				this.CancelSkill(skill, caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.CancelSkill(skill, caster);
				return;
			}

			var targetPosition = primaryTarget.Position;
			var duration = this.GetDuration(skill.Level);

			caster.TurnTowards(targetPosition);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, primaryTarget.Handle, originPos, originPos.GetDirection(targetPosition), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);

			primaryTarget.StartBuff(
				BuffId.DownFall_Debuff,
				skill.Level,
				0,
				duration,
				caster,
				skill.Id
			);

			skill.IncreaseOverheat();
			caster.SetAttackState(false);
		}

		private TimeSpan GetDuration(int skillLevel)
		{
			var level = Math.Clamp(skillLevel, 1, MaximumSkillLevel);
			var durationMilliseconds = 4000d + ((level - 1) * (4000d / (MaximumSkillLevel - 1)));
			return TimeSpan.FromMilliseconds(durationMilliseconds);
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}
	}
}
