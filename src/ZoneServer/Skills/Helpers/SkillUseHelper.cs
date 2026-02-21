using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Helpers
{
	public static class SkillUseHelper
	{
		public const int PET_COMMON_JOBID = 0; // Define the constant value or get it from the context

		public static bool PetSkillPreCheck(ICombatEntity caster, Skill skill)
		{
			if (!caster.TryGetActiveCompanion(out var companion))
			{
				caster.SendSysMsg("SummonedPetDoesNotExist");
				return false;
			}

			if (!PetCheckActiveState(caster, skill))
			{
				caster.SendSysMsg("CompanionIsNotActive");
				return false;
			}

			if (!PetCheckPausePetSkill(caster, skill))
			{
				caster.SendSysMsg("YouCanNotUseSkillCompanionCoolTime");
				return false;
			}

			if (!companion.IsVisible(caster))
			{
				return false;
			}

			if (companion.CompanionData.IsRideOnly)
			{
				caster.SendSysMsg("ThisCompanionIsNotPossible");
				return false;
			}

			return true;
		}

		private static bool PetCheckPausePetSkill(ICombatEntity caster, Skill skill)
		{
			if (!caster.TryGetActiveCompanion(out var companion))
				return false;
			return true;
		}

		private static bool PetCheckActiveState(ICombatEntity caster, Skill skill)
		{
			return caster.TryGetActiveCompanion(out _);
		}

		public static bool SkillCheckNearPad(ICombatEntity caster, Skill skill, string padName, bool isExist, float range)
		{
			var position = caster.Position;
			var padCount = caster.Map.GetPads(pad => pad.Name.ToLowerInvariant() == padName.ToLowerInvariant() && pad.Position.Get2DDistance(position) <= range).Length;

			if (!isExist)
				return padCount == 0;
			else
				return padCount > 0;
		}

		public static void SkillArcJump(ICombatEntity caster, Position position,
				float height, float angle, float time1, float easeIn, float time2, float easeOut)
		{
			// Validate position is within map boundaries
			if (!caster.Map.Ground.TryGetNearestValidPosition(position, out var validPosition))
				validPosition = caster.Position;

			var jumpDistance = (float)caster.Position.Get2DDistance(validPosition);
			caster.Position = validPosition;
			Send.ZC_NORMAL.LeapJump(caster, validPosition, height, angle, time1, easeIn, time2, easeOut);
		}

		public static void SkillCancelCancel(ICombatEntity caster, Skill skill)
		{
			Send.ZC_SKILL_DISABLE(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}
	}
}
