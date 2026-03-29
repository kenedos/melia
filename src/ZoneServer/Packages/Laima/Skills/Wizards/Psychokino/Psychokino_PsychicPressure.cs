using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Handlers;

namespace Melia.Zone.Skills.HandlersOverrides.Wizards.Psychokino
{
	/// <summary>
	/// Handler for the Psychokino skill Psychic Pressure.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Psychokino_PsychicPressure)]
	public class Psychokino_PsychicPressureOverride : IGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_wiz_psychicpressure_shot", "voice_wiz_m_psychicpressure_shot");
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_SKILL_DISABLE(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			SkillRemovePad(caster, skill);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var targetPos = originPos.GetRelative(farPos, distance: 5);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.PsychicPressure_Pad);
		}
	}
}
