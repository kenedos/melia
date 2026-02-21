using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Elementalist
{
	/// <summary>
	/// Handler override for the Elementalist skill Elemental Essence (Elemental Burst).
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Elementalist_ElementalEssence)]
	public class Elementalist_ElementalEssenceOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			caster.PlaySound("voice_elementalist_f_elementalessence_cast", "voice_elementalist_m_elementalessence_cast");
			caster.PlaySound("skl_eff_elementalist_elementalessence_cast");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			caster.StopSound("skl_eff_elementalist_elementalessence_cast");
			caster.StopSound("voice_elementalist_f_elementalessence_cast", "voice_elementalist_m_elementalessence_cast");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
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

			var target = targets.FirstOrDefault();
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var effectPosition = originPos.GetRelative(caster.Position, height: -10);
			Send.ZC_GROUND_EFFECT(caster, effectPosition, "E_wizard_burst_shot_line", 0.5f, 0, 0, caster.Direction.DegreeAngle);

			skill.Run(this.CreateAdditionalPads(caster, skill));

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);
		}

		private Position GetRelativePos(ICombatEntity caster, float angleRad, float dist)
		{
			var degreeOffset = (float)(angleRad * (180.0 / Math.PI));
			var padDir = caster.Direction.AddDegreeAngle(degreeOffset);
			return caster.Position.GetRelative(padDir, dist);
		}

		private async Task CreateAdditionalPads(ICombatEntity caster, Skill skill)
		{
			var padAngle = caster.Direction.DegreeAngle;

			// Time 210ms: Fire, Ice, Fire, Lightning
			await skill.Wait(TimeSpan.FromMilliseconds(168));
			var pos1 = GetRelativePos(caster, 0f, 50f);
			var pos2 = GetRelativePos(caster, -0.315f, 33.5f);
			var pos3 = GetRelativePos(caster, 0f, 50f);
			var pos3b = GetRelativePos(caster, 0.25f, 45f);
			SkillCreatePad(caster, skill, pos1, padAngle, PadName.Elemental_Burst_Pad_Fire);
			SkillCreatePad(caster, skill, pos2, padAngle, PadName.Elemental_Burst_Pad_Ice);
			SkillCreatePad(caster, skill, pos3, padAngle, PadName.Elemental_Burst_Pad_Fire);
			SkillCreatePad(caster, skill, pos3b, padAngle, PadName.Elemental_Burst_Pad_Lightning);

			// Time 350ms: Lightning, Fire, Ice
			await skill.Wait(TimeSpan.FromMilliseconds(112));
			var pos4 = GetRelativePos(caster, 0.075f, 88f);
			var pos4b = GetRelativePos(caster, 0.2f, 85f);
			var pos4c = GetRelativePos(caster, -0.15f, 90f);
			SkillCreatePad(caster, skill, pos4, padAngle, PadName.Elemental_Burst_Pad_Lightning);
			SkillCreatePad(caster, skill, pos4b, padAngle, PadName.Elemental_Burst_Pad_Fire);
			SkillCreatePad(caster, skill, pos4c, padAngle, PadName.Elemental_Burst_Pad_Ice);

			// Time 400ms: Lightning, Fire, Ice
			await skill.Wait(TimeSpan.FromMilliseconds(40));
			var pos5 = GetRelativePos(caster, 0.17f, 25f);
			var pos5b = GetRelativePos(caster, 0.3f, 30f);
			var pos5c = GetRelativePos(caster, -0.1f, 28f);
			SkillCreatePad(caster, skill, pos5, padAngle, PadName.Elemental_Burst_Pad_Lightning);
			SkillCreatePad(caster, skill, pos5b, padAngle, PadName.Elemental_Burst_Pad_Fire);
			SkillCreatePad(caster, skill, pos5c, padAngle, PadName.Elemental_Burst_Pad_Ice);

			// Time 450ms: Fire, Ice, Lightning
			await skill.Wait(TimeSpan.FromMilliseconds(40));
			var pos6 = GetRelativePos(caster, -0.033f, 125f);
			var pos7 = GetRelativePos(caster, -0.18f, 79f);
			var pos7b = GetRelativePos(caster, 0.15f, 100f);
			SkillCreatePad(caster, skill, pos6, padAngle, PadName.Elemental_Burst_Pad_Fire);
			SkillCreatePad(caster, skill, pos7, padAngle, PadName.Elemental_Burst_Pad_Ice);
			SkillCreatePad(caster, skill, pos7b, padAngle, PadName.Elemental_Burst_Pad_Lightning);

			// Time 680ms: Fire2 x3, Ice, Lightning
			await skill.Wait(TimeSpan.FromMilliseconds(184));
			var pos8 = GetRelativePos(caster, 0f, 40f);
			var pos9 = GetRelativePos(caster, 0f, 100f);
			var pos10 = GetRelativePos(caster, 0f, 150f);
			var pos10b = GetRelativePos(caster, -0.15f, 120f);
			var pos10c = GetRelativePos(caster, 0.15f, 140f);
			SkillCreatePad(caster, skill, pos8, padAngle, PadName.Elemental_Burst_Pad_Fire2);
			SkillCreatePad(caster, skill, pos9, padAngle, PadName.Elemental_Burst_Pad_Fire2);
			SkillCreatePad(caster, skill, pos10, padAngle, PadName.Elemental_Burst_Pad_Fire2);
			SkillCreatePad(caster, skill, pos10b, padAngle, PadName.Elemental_Burst_Pad_Ice);
			SkillCreatePad(caster, skill, pos10c, padAngle, PadName.Elemental_Burst_Pad_Lightning);

			// Wave 6: Fire, Ice, Lightning
			await skill.Wait(TimeSpan.FromMilliseconds(112));
			var pos11 = GetRelativePos(caster, 0.1f, 170f);
			var pos11b = GetRelativePos(caster, -0.2f, 160f);
			var pos11c = GetRelativePos(caster, 0.25f, 155f);
			SkillCreatePad(caster, skill, pos11, padAngle, PadName.Elemental_Burst_Pad_Fire);
			SkillCreatePad(caster, skill, pos11b, padAngle, PadName.Elemental_Burst_Pad_Ice);
			SkillCreatePad(caster, skill, pos11c, padAngle, PadName.Elemental_Burst_Pad_Lightning);

			// Wave 7: Fire2, Ice, Lightning
			await skill.Wait(TimeSpan.FromMilliseconds(112));
			var pos12 = GetRelativePos(caster, -0.05f, 200f);
			var pos12b = GetRelativePos(caster, 0.18f, 190f);
			var pos12c = GetRelativePos(caster, -0.15f, 185f);
			SkillCreatePad(caster, skill, pos12, padAngle, PadName.Elemental_Burst_Pad_Fire2);
			SkillCreatePad(caster, skill, pos12b, padAngle, PadName.Elemental_Burst_Pad_Ice);
			SkillCreatePad(caster, skill, pos12c, padAngle, PadName.Elemental_Burst_Pad_Lightning);

		}
	}
}
