using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Elementalist
{
	/// <summary>
	/// Handler override for the Elementalist skill Fire Claw.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Elementalist_FireClaw)]
	public class Elementalist_FireClawOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(670));
			MonsterSkillPadDirMissile(caster, skill, targetPos, PadName.FireClaw_Pad, 200f, 1, 150f, 0f, 0f, 0f, 1);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			MonsterSkillPadDirMissile(caster, skill, targetPos, PadName.FireClaw_Pad, 200f, 1, 150f, 0f, 0f, 45f, 1);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			MonsterSkillPadDirMissile(caster, skill, targetPos, PadName.FireClaw_Pad, 200f, 1, 150f, 0f, 0f, 90f, 1);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			MonsterSkillPadDirMissile(caster, skill, targetPos, PadName.FireClaw_Pad, 200f, 1, 150f, 0f, 0f, 135f, 1);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			MonsterSkillPadDirMissile(caster, skill, targetPos, PadName.FireClaw_Pad, 200f, 1, 150f, 0f, 0f, -45f, 1);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			MonsterSkillPadDirMissile(caster, skill, targetPos, PadName.FireClaw_Pad, 200f, 1, 150f, 0f, 0f, -90f, 1);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			MonsterSkillPadDirMissile(caster, skill, targetPos, PadName.FireClaw_Pad, 200f, 1, 150f, 0f, 0f, -135f, 1);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			MonsterSkillPadDirMissile(caster, skill, targetPos, PadName.FireClaw_Pad, 200f, 1, 150f, 0f, 0f, 180f, 1);
		}
	}
}
