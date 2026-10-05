using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Necromancer skill Flesh Strike.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_FleshStrike)]
	public class Necromancer_FleshStrikeOverride : IGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_wiz_f_flashstrike_shot", "voice_wiz_m_flashstrike_shot");
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_wiz_f_flashstrike_shot", "voice_wiz_m_flashstrike_shot");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
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

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			caster.MissileFall(skill.Data.ClassName, "I_wizard_FleshStrike_force", 1f, targetPos, 35f, 0f, 0.55f, 300f, 1f, "F_wizard_FleshStrike_hit", 1.5f, 0f, "None", 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(750));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Necromancer_FleshStrike);
		}
	}
}
