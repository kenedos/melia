using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handler for the Oracle skill Divine Might.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Oracle_DivineMight)]
	public class Oracle_DivineMightOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			if (!caster.IsAbilityActive(AbilityId.Oracle23))
			{
				var time = 20000 + skill.Level * 500;
				if (caster.IsAbilityActive(AbilityId.Oracle20))
					time += caster.GetAbilityLevel(AbilityId.Oracle20) * 1000;
				caster.StartBuff(BuffId.DivineMight_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(time), caster, skill.Id);
			}
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Oracle_DivineMight);
		}
	}
}
