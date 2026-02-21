using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Wizards.Chronomancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Chronomancer_TimeForward)]
	public class Chronomancer_TimeForwardOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var targetList = caster.Map.GetAttackableEnemiesInPosition(caster, farPos, (int)skill.Data.SplashRange * 4);

			foreach (var currentTarget in targetList.LimitBySDR(caster, skill))
			{
				Send.ZC_SYNC_START(caster, skillHandle, 1);

				var duration = TimeSpan.FromSeconds(5 + skill.Level);
				currentTarget.StartBuff(BuffId.TimeForward_Debuff, skill.Level, 0, duration, caster);

				Send.ZC_SYNC_END(caster, skillHandle, 0);
				Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(100));
			}
		}
	}
}
