using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	public static class Druid_TransformationHelper
	{
		private const string MonsterIdVariable = "Druid.LastTransformation.MonsterId";
		private const string SkillCountVariable = "Druid.LastTransformation.SkillCount";
		private const int MaximumStoredSkills = 20;

		public static void SaveTransformation(Character character, Mob monster)
		{
			character.Variables.Temp.SetInt(MonsterIdVariable, (int)monster.Id);

			if (!monster.Components.TryGet<BaseSkillComponent>(out var component))
			{
				character.Variables.Temp.SetInt(SkillCountVariable, 0);
				return;
			}

			var skills = component.GetList()
				.Where(skill => skill != null && skill.Id != SkillId.None)
				.Take(MaximumStoredSkills)
				.ToArray();

			character.Variables.Temp.SetInt(SkillCountVariable, skills.Length);

			for (var i = 0; i < skills.Length; i++)
			{
				character.Variables.Temp.SetInt(SkillIdVariable(i), (int)skills[i].Id);
				character.Variables.Temp.SetInt(SkillLevelVariable(i), Math.Max(1, skills[i].Level));
			}
		}

		public static int GetMonsterId(Character character)
		{
			return character.Variables.Temp.GetInt(MonsterIdVariable);
		}

		public static void ApplyStoredSkills(Character character, Buff buff)
		{
			var skillCount = Math.Clamp(character.Variables.Temp.GetInt(SkillCountVariable), 0, MaximumStoredSkills);
			var mainAttack = SkillId.None;

			for (var i = 0; i < skillCount; i++)
			{
				var skillId = (SkillId)character.Variables.Temp.GetInt(SkillIdVariable(i));
				var skillLevel = Math.Max(1, character.Variables.Temp.GetInt(SkillLevelVariable(i)));

				if (skillId == SkillId.None)
					continue;

				if (mainAttack == SkillId.None)
					mainAttack = skillId;

				if (character.Skills.Has(skillId))
					continue;

				character.Skills.Add(new Skill(character, skillId, skillLevel));
				buff.Vars.SetInt(AddedSkillVariable(skillId), 1);
			}

			if (mainAttack != SkillId.None)
				Send.ZC_NORMAL.SetMainAttackSkill(character, mainAttack);
		}

		public static string AddedSkillVariable(SkillId skillId)
		{
			return $"Druid.ShapeShifting.Added.{(int)skillId}";
		}

		private static string SkillIdVariable(int index)
		{
			return $"Druid.LastTransformation.Skill.{index}.Id";
		}

		private static string SkillLevelVariable(int index)
		{
			return $"Druid.LastTransformation.Skill.{index}.Level";
		}
	}
}
