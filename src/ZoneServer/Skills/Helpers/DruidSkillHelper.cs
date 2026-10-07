using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Druid's transformations.
	/// </summary>
	public static class DruidSkillHelper
	{
		private const int MaxShapeSkills = 10;
		private const string ShapeMonsterVar = "Melia.Druid.ShapeMonsterId";
		private const string ShapeSkillsVar = "Melia.Druid.ShapeSkills";
		private const string AddedSkillsVar = "Melia.Druid.AddedSkills";

		/// <summary>
		/// The skills of the Lycanthropy wolf.
		/// </summary>
		public static readonly SkillId[] WolfSkills = [SkillId.Mon_pcskill_boss_werewolf_Skill_1, SkillId.Mon_pcskill_boss_werewolf_Skill_3, SkillId.Mon_pcskill_boss_werewolf_Skill_4, SkillId.Mon_pcskill_boss_werewolf_Skill_5];

		/// <summary>
		/// Returns false if the character is in wolf form and the skill isn't
		/// one of the wolf's or Lycanthropy itself.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public static bool CanUseSkill(Character character, SkillId skillId)
		{
			if (!character.IsBuffActive(BuffId.Lycanthropy_Buff))
				return true;

			return skillId == SkillId.Druid_Lycanthropy || WolfSkills.Contains(skillId);
		}

		/// <summary>
		/// Remembers the monster the character shape shifted into and its
		/// skills, for Transform to bring back.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="monster"></param>
		public static void SaveShape(Character character, Mob monster)
		{
			var skillIds = monster.Data.Skills.Select(a => (int)a.SkillId).Take(MaxShapeSkills).ToArray();

			character.Variables.Temp.SetInt(ShapeMonsterVar, monster.Id);
			character.Variables.Temp.Set(ShapeSkillsVar, skillIds);
		}

		/// <summary>
		/// Returns the monster the character last shape shifted into, or 0.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static int GetShapeMonsterId(Character character)
			=> character.Variables.Temp.GetInt(ShapeMonsterVar);

		/// <summary>
		/// Returns the skills of the monster the character last shape
		/// shifted into.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static SkillId[] GetShapeSkills(Character character)
		{
			if (!character.Variables.Temp.TryGet<int[]>(ShapeSkillsVar, out var skillIds))
				return [];

			return skillIds.Select(a => (SkillId)a).ToArray();
		}

		/// <summary>
		/// Lends the character the skills for as long as the buff lasts, the
		/// first one as their basic attack.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="character"></param>
		/// <param name="skillIds"></param>
		/// <param name="level"></param>
		public static void AddTemporarySkills(Buff buff, Character character, SkillId[] skillIds, int level)
		{
			var added = skillIds.Where(a => !character.Skills.Has(a)).ToArray();

			foreach (var skillId in added)
				character.Skills.Add(new Skill(character, skillId, level));

			buff.Vars.Set(AddedSkillsVar, added);

			if (skillIds.Length > 0)
				Send.ZC_NORMAL.SetMainAttackSkill(character, skillIds[0]);
		}

		/// <summary>
		/// Takes back the skills the buff lent the character.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="character"></param>
		public static void RemoveTemporarySkills(Buff buff, Character character)
		{
			if (buff.Vars.TryGet<SkillId[]>(AddedSkillsVar, out var added))
			{
				foreach (var skillId in added)
					character.Skills.Remove(skillId);
			}

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
		}
	}
}
