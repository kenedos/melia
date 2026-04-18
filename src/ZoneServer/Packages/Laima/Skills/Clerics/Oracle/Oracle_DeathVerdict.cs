using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handler for the Oracle skill Death Verdict.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Oracle_DeathVerdict)]
	public class Oracle_DeathVerdictOverride : IGroundSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			var value = skill.GetPVPValue(5 + skill.Level);
			var skillTargets = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 100f, 40f, value);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			var time = 25000;
			if (caster.IsAbilityActive(AbilityId.Oracle18))
				time = 11000 + caster.GetAbilityLevel(AbilityId.Oracle18) * 1000;
			SkillTargetBuff(skill, caster, skillTargets, BuffId.DeathVerdict_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(time));
			var time2 = 25000;
			if (caster.IsAbilityActive(AbilityId.Oracle18))
				time2 = 11000 + caster.GetAbilityLevel(AbilityId.Oracle18) * 1000;
			SkillTargetBuffAbility(caster, skill, AbilityId.Oracle8, BuffId.DeathVerdict_Slow_Debuff, 1, -1, time2, 0, 1, 100);
		}
	}
}
