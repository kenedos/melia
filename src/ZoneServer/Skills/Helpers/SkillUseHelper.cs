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
			if (!caster.TryGetActiveGroundCompanion(out var companion))
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
			if (!caster.TryGetActiveGroundCompanion(out var companion))
				return false;
			return true;
		}

		private static bool PetCheckActiveState(ICombatEntity caster, Skill skill)
		{
			return caster.TryGetActiveGroundCompanion(out _);
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

		/// <summary>
		/// Moves the caster's server position along its skill's client-side movement, without notifying the client.
		/// </summary>
		/// <remarks>
		/// Each frame is an offset from where the caster stood and faced when the skill started, reached at
		/// the frame's time in milliseconds from the start of the skill.
		/// </remarks>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <param name="frames"></param>
		public static async Task MonsterSkillFollowMovePath(ICombatEntity caster, Skill skill, params (int Time, float Distance, float Angle)[] frames)
		{
			var originPos = caster.Position;
			var direction = caster.Direction;
			var fromPos = originPos;
			var fromTime = 0;

			foreach (var (time, distance, angle) in frames)
			{
				var toPos = caster.Map.Ground.GetLastValidPosition(originPos, originPos.GetRelative(direction.AddDegreeAngle(angle), distance));

				var lastElapsed = fromTime;
				for (var elapsed = fromTime + MovePathStep; elapsed < time; elapsed += MovePathStep)
				{
					await skill.Wait(TimeSpan.FromMilliseconds(MovePathStep));
					var ratio = (float)(elapsed - fromTime) / (time - fromTime);
					caster.Position = new Position(fromPos.X + (toPos.X - fromPos.X) * ratio, fromPos.Y + (toPos.Y - fromPos.Y) * ratio, fromPos.Z + (toPos.Z - fromPos.Z) * ratio);
					lastElapsed = elapsed;
				}

				if (time > lastElapsed)
					await skill.Wait(TimeSpan.FromMilliseconds(time - lastElapsed));

				caster.Position = toPos;
				fromPos = toPos;
				fromTime = time;
			}
		}

		/// <summary>
		/// Interval in milliseconds between server position updates while following a skill's movement.
		/// </summary>
		private const int MovePathStep = 50;

		public static void SkillCancelCancel(ICombatEntity caster, Skill skill)
		{
			Send.ZC_SKILL_DISABLE(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}
	}
}
