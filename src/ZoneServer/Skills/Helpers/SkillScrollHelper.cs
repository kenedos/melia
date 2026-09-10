using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for skills cast from skill scrolls.
	/// </summary>
	public static class SkillScrollHelper
	{
		private const string ScrollItemPrefix = "Scroll_SkillItem_";

		/// <summary>
		/// Pose held while a scroll is being cast by a character whose
		/// class has no animations for the skill.
		/// </summary>
		private const string CastAnimationName = "SCROLL";

		/// <summary>
		/// Pose played when a scroll goes off for a character whose class
		/// has no animations for the skill.
		/// </summary>
		private const string ReleaseAnimationName = "PUBLIC_BUFF";

		/// <summary>
		/// Returns the skill the given scroll casts, if it is a skill
		/// scroll.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		public static bool TryGetScrollSkill(Character character, Item item, out Skill skill)
		{
			skill = null;

			if (item == null)
				return false;

			var skillId = GetScrollSkillId(item);
			if (skillId == SkillId.None)
				return false;

			item.Properties.TryGetFloat(PropertyName.SkillLevel, out var skillLevel);

			if (skillLevel == 0)
			{
				var skillTree = ZoneServer.Instance.Data.SkillTreeDb.Find(entry => entry.SkillId == skillId);
				skillLevel = skillTree?.MaxLevel ?? 1;
			}

			skill = new Skill(character, skillId, (int)skillLevel, isItemSkill: true);

			return true;
		}

		/// <summary>
		/// Returns the skill cast by the first scroll in the character's
		/// inventory that carries the given skill.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		public static bool TryGetScrollSkill(Character character, SkillId skillId, out Skill skill)
		{
			skill = null;

			if (skillId == SkillId.None)
				return false;

			var item = character.Inventory.GetItems().Values.FirstOrDefault(a => a.IsSkillScroll && GetScrollSkillId(a) == skillId);
			if (item == null)
				return false;

			return TryGetScrollSkill(character, item, out skill);
		}

		/// <summary>
		/// Plays the pose the character holds while casting the given
		/// scroll skill.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		public static void PlayCastAnimation(Character character, Skill skill)
		{
			if (!UsesGenericAnimation(character, skill))
				return;

			character.PlayAnimation(CastAnimationName, stopOnLastFrame: true);
		}

		/// <summary>
		/// Substitutes the pose the client plays as the given scroll skill
		/// goes off, until the skill's animation is over.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		public static void SetReleaseAnimation(Character character, Skill skill)
		{
			if (!UsesGenericAnimation(character, skill))
				return;

			var shootTime = SkillTimingHelper.GetClientShootTime(character, skill, skill.Properties.GetFloat(PropertyName.ShootTime));

			Send.ZC_NORMAL.SkillChangeAnimation(character, skill.Id, ReleaseAnimationName);
			CallSafe(ClearReleaseAnimation(character, skill.Id, TimeSpan.FromMilliseconds(shootTime)));
		}

		/// <summary>
		/// Drops the substituted release pose after the given delay.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		/// <param name="delay"></param>
		private static async Task ClearReleaseAnimation(Character character, SkillId skillId, TimeSpan delay)
		{
			await GameClock.Delay(delay);

			Send.ZC_NORMAL.SkillChangeAnimation(character, skillId);
		}

		/// <summary>
		/// Drops the pose held during the given scroll skill's cast.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		public static void ResetCastAnimation(Character character, Skill skill)
		{
			if (!UsesGenericAnimation(character, skill))
				return;

			Send.ZC_NORMAL.ResetStdAnim(character);
		}

		/// <summary>
		/// Returns the skill the given item carries, or None if it is not
		/// a skill scroll.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private static SkillId GetScrollSkillId(Item item)
		{
			item.Properties.TryGetFloat(PropertyName.SkillType, out var skillType);

			var skillId = (SkillId)skillType;

			if (skillId == SkillId.None && item.Data.ClassName.StartsWith(ScrollItemPrefix))
				Enum.TryParse(item.Data.ClassName.Substring(ScrollItemPrefix.Length), out skillId);

			return skillId;
		}

		/// <summary>
		/// Returns true if the character's class carries no animations for
		/// the given scroll skill, so a generic pose has to stand in.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		private static bool UsesGenericAnimation(Character character, Skill skill)
		{
			if (skill == null || !skill.IsItemSkill)
				return false;

			var skillTree = ZoneServer.Instance.Data.SkillTreeDb.Find(entry => entry.SkillId == skill.Id);
			if (skillTree == null)
				return true;

			return skillTree.JobId.ToClass() != character.JobClass;
		}
	}
}
