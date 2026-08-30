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

namespace Melia.Zone.Skills.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Common skill Pardoner.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Common_Pardoner_IncreaseMagicDEF)]
	public class Pardoner_CommonIncreaseMagicDefOverride : IGroundSkillHandler
	{
		private const float BuffRange = 150f;
		private const int MaxTargets = 50;
		private const float BuffDurationMs = 1800000f;

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
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectAlliesInCircle(caster, targetPos, BuffRange, MaxTargets));

			await skill.Wait(TimeSpan.FromMilliseconds(90));

			SkillTargetBuff(skill, caster, caster.GetTargets(), BuffId.IncreaseMagicDEF_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(BuffDurationMs), skill.Id);
		}
	}
}
