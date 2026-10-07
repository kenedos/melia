using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
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
		private const float ShapeSkillMinRange = 50f;
		private const float ShapeSkillWidth = 40f;
		private const string ShapeMonsterVar = "Melia.Druid.ShapeMonsterId";
		private const string ShapeSkillsVar = "Melia.Druid.ShapeSkills";
		private const string AddedSkillsVar = "Melia.Druid.AddedSkills";

		/// <summary>
		/// The skills of the Lycanthropy wolf.
		/// </summary>
		public static readonly SkillId[] WolfSkills = [SkillId.Mon_pcskill_boss_werewolf_Skill_1, SkillId.Mon_pcskill_boss_werewolf_Skill_3, SkillId.Mon_pcskill_boss_werewolf_Skill_4, SkillId.Mon_pcskill_boss_werewolf_Skill_5];

		/// <summary>
		/// Returns true if the character is in one of Lycanthropy's forms,
		/// which can dash whatever the character's class.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static bool CanDash(Character character)
			=> character.IsBuffActive(BuffId.Lycanthropy_Buff) || character.IsBuffActive(BuffId.Lycanthropy_Half_Buff);

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

			return skillId == SkillId.Druid_Lycanthropy || skillId == SkillId.Common_StateClear || WolfSkills.Contains(skillId);
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
		/// Uses a skill lent by Transform whose handler only exists in the
		/// targeted form monsters use, aiming it at the given target or at
		/// the nearest enemy in front of the character. Returns false if
		/// the skill isn't one.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool TryUseShapeSkill(Character character, Skill skill, ICombatEntity target)
		{
			if (!character.IsBuffActive(BuffId.transform))
				return false;

			if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<ISkillHandler>(skill.Id, out var skillHandler) || skillHandler is not ITargetSkillHandler handler)
				return false;

			if (target == null)
			{
				var range = Math.Max(ShapeSkillMinRange, skill.Properties.GetFloat(PropertyName.MaxR));
				var area = new Square(character.Position, character.Direction, range, ShapeSkillWidth);

				target = character.Map.GetAttackableEnemiesIn(character, area).OrderBy(a => a.Position.Get2DDistance(character.Position)).FirstOrDefault();
			}

			if (target == null)
			{
				Send.ZC_SKILL_CAST_CANCEL(character);
				return true;
			}

			character.TurnTowards(target);
			skill.PrepareCancellation();
			handler.Handle(skill, character, target);

			return true;
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
			foreach (var skillId in skillIds)
				character.Skills.Add(CreateLentSkill(character, skillId, level));

			buff.Vars.Set(AddedSkillsVar, skillIds);

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

		/// <summary>
		/// Lends the character a monster form's skills for as long as the
		/// buff lasts, bound to the buff so the client lays them over its
		/// quickslot bar, plus the slot that ends the form.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="character"></param>
		/// <param name="skillIds"></param>
		/// <param name="level"></param>
		public static void AddFormSkills(Buff buff, Character character, SkillId[] skillIds, int level)
		{
			var formSkillIds = skillIds.Append(SkillId.Common_StateClear).ToArray();

			foreach (var skillId in formSkillIds)
			{
				var skill = CreateLentSkill(character, skillId, level);
				skill.IsFormSkill = true;
				character.Skills.Add(skill, true);
			}

			foreach (var skillId in formSkillIds)
				Send.ZC_NORMAL.ApplyBuff(character, buff.Data.ClassName, skillId, true);

			buff.Vars.Set(AddedSkillsVar, formSkillIds);
		}

		/// <summary>
		/// Creates a skill that is never saved with the character, at the
		/// given level.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		private static Skill CreateLentSkill(Character character, SkillId skillId, int level)
		{
			var skill = new Skill(character, skillId, 0);
			skill.Vars.SetFloat("FixedLevel", Math.Max(1, level));
			skill.Properties.InvalidateAll();

			return skill;
		}

		/// <summary>
		/// Plays the smoke the Druid's forms end in.
		/// </summary>
		/// <param name="actor"></param>
		public static void PlayFormEndEffect(IActor actor)
			=> Send.ZC_NORMAL.PlayEffect(actor, 0, EffectLocation.Middle, 0, 0.5f, "F_cleric_ShapeShifting_shot_smoke3");

		/// <summary>
		/// Takes back the monster form's skills and unbinds them from the
		/// buff.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="character"></param>
		public static void RemoveFormSkills(Buff buff, Character character)
		{
			Send.ZC_NORMAL.RemoveBuff(character, buff.Data.ClassName);

			if (!buff.Vars.TryGet<SkillId[]>(AddedSkillsVar, out var added))
				return;

			foreach (var skillId in added)
				character.Skills.Remove(skillId);
		}
	}
}
