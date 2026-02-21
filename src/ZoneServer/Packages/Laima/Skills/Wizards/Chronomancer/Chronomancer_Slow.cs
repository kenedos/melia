using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Wizards.Chronomancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Chronomancer_Slow)]
	public class Chronomancer_SlowOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster is Character character)
				Send.ZC_NORMAL.Skill_DynamicCastStart(character, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster is Character character)
				Send.ZC_NORMAL.Skill_DynamicCastEnd(character, skill.Id, 2);
		}

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

			Send.ZC_NORMAL.RunPad(caster, skill, "Chronomancer_Slow", farPos, caster.Direction, 0.06292176f, 85.76556f, skillHandle, 60);

			var targetList = caster.Map.GetAttackableEnemiesInPosition(caster, farPos, (int)skill.Data.SplashRange * 4);

			Send.ZC_SKILL_RANGE_CIRCLE(caster, originPos, 50);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			foreach (var currentTarget in targetList.LimitBySDR(caster, skill))
			{
				Send.ZC_SYNC_START(caster, skillHandle, 1);

				var debuffDuration = TimeSpan.FromSeconds(5 + skill.Level);
				currentTarget.StartBuff(BuffId.Slow_Debuff, skill.Level, 0, debuffDuration, caster);

				Send.ZC_SYNC_END(caster, skillHandle, 0);
				Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(100));
			}
		}
	}
}
