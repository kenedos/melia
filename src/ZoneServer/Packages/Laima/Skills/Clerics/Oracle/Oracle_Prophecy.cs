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

namespace Melia.Zone.Skills.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handler for the Oracle skill Prophecy.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Oracle_Prophecy)]
	public class Oracle_ProphecyOverride : IGroundSkillHandler
	{
		private static readonly TimeSpan BuffDelay = TimeSpan.FromMilliseconds(500);

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
			await skill.Wait(BuffDelay);

			var duration = skill.Properties.CaptionTime;
			if (ZoneServer.Instance.World.IsPVP)
				duration /= 2;

			caster.StartBuff(BuffId.Prophecy_Buff, skill.Level, 0f, duration, caster, skill.Id);
		}
	}
}
