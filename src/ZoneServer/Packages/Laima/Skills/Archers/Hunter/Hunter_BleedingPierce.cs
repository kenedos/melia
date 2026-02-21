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
using Melia.Zone.Pads.Helpers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using Yggdrasil.Extensions;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Hunter
{
	/// <summary>
	/// Handler for the Hunter skill Bleeding Pierce.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hunter_BleedingPierce)]
	public class Hunter_BleedingPierceOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_atk_long_cast_f", "voice_war_atk_long_cast");
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

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			// Create pad with direction matching caster's facing
			var position = originPos.GetRelative(farPos);
			var padAngle = caster.Direction.DegreeAngle;

			var pad = SkillCreatePad(caster, skill, position, padAngle, PadName.shootpad_BleedingPierce);
			if (pad == null)
				return;

			// Set rectangular area before moving
			var padLength = 170f;
			var padWidth = 30f;
			pad.Direction = caster.Direction;
			pad.SetRectangleRange(caster.Direction, padWidth, padLength);

			// Move pad forward
			var moveRange = 150f;
			var speed = 600f;
			var accel = 50f;
			var destPos = caster.Position.GetRelative(caster.Direction, moveRange);
			await pad.SetDestPosWithDelay(destPos, speed, accel, true, 0f);
		}
	}
}
