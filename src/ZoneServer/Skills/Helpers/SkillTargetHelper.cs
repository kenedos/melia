using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;

namespace Melia.Zone.Skills.Helpers
{
	public static class SkillTargetHelper
	{
		public static void SkillTargetEffects(Skill skill, ICombatEntity caster, IList<ICombatEntity> targets, string effectName, float scale, bool isSorcerer)
		{
			if (caster.IsDead)
				return;

			if (caster is Character character && isSorcerer)
			{
				var followerList = character.Summons.GetSummons();
				var etc = character.Etc.Properties;
				foreach (var follower in followerList)
				{
					if (etc.GetString(PropertyName.Sorcerer_bosscardName1) == follower.ClassName || etc.GetString(PropertyName.Sorcerer_bosscardName2) == follower.ClassName)
						follower.PlayEffect(effectName, scale, 0, EffectLocation.Bottom);
				}
				return;
			}

			foreach (var target in targets)
			{
				target.PlayEffect(effectName, scale, 0, EffectLocation.Bottom);
			}
		}

		public static void SkillTargetEffects(Skill skill, ICombatEntity caster, string effectName, float scale, bool isSorcerer)
		{
			if (caster.IsDead)
				return;

			if (caster is Character character && isSorcerer)
			{
				var followerList = character.Summons.GetSummons();
				var etc = character.Etc.Properties;
				foreach (var follower in followerList)
				{
					if (etc.GetString(PropertyName.Sorcerer_bosscardName1) == follower.ClassName || etc.GetString(PropertyName.Sorcerer_bosscardName2) == follower.ClassName)
						follower.PlayEffect(effectName, scale, 0, EffectLocation.Bottom);
				}
				return;
			}

			foreach (var target in caster.GetTargets())
			{
				target.PlayEffect(effectName, scale, 0, EffectLocation.Bottom);
			}
		}

		/// <summary>
		/// Damage a target with a multiplier.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targets"></param>
		/// <param name="damageMultiplier"></param>
		public static List<SkillHitInfo> SkillTargetDamage(Skill skill, ICombatEntity caster, IList<ICombatEntity> targets, float damageMultiplier = 1f)
		{
			var hits = new List<SkillHitInfo>();
			if (caster.IsDead || targets == null || targets.Count == 0)
				return hits;

			foreach (var target in targets)
			{
				if (target == null || target.IsDead)
					continue;

				var skillHitResult = SCR_SkillHit(caster, target, skill);
				skillHitResult.Damage *= damageMultiplier;
				target.TakeDamage(skillHitResult.Damage, caster);
				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, skill.GetDamageDelay(), skill.GetHitDelay()));
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			return hits;
		}

		/// <summary>
		/// Start a buff for all targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targets"></param>
		/// <param name="buffId"></param>
		/// <param name="numArg1"></param>
		/// <param name="numArg2"></param>
		/// <param name="duration"></param>
		/// <param name="skillId"></param>
		public static void SkillTargetBuff(Skill skill, ICombatEntity caster, IList<ICombatEntity> targets,
			BuffId buffId, float numArg1, float numArg2, TimeSpan duration, SkillId skillId = SkillId.Normal_Attack)
		{
			if (caster.IsDead)
				return;

			foreach (var target in targets)
			{
				if (target == null || target.IsDead)
					continue;
				target.StartBuff(buffId, numArg1, numArg2, duration, caster, skillId);
			}
		}

		public static void SkillTargetKnockdown(ICombatEntity caster, Skill skill, IList<ICombatEntity> targets, KnockType knockType, KnockDirection knockDirection, float power, float verticalAngle, float horizontalAngle, int bound, int knockdownRank)
		{
			if (caster.IsAbilityActive(AbilityId.Monk8))
				return;

			foreach (var target in targets)
			{
				if (target.CheckBoolTempVar("NO_HIT"))
					continue;

				if (skill.Id == SkillId.Cryomancer_SnowRolling)
				{
					var immune = target.IsBuffActive(BuffId.SnowRollingTemporaryImmune);
					if (!immune)
					{
						SkillToolKnockdown(caster, target, knockType, knockDirection, power, verticalAngle, horizontalAngle, bound, knockdownRank);
						target.RemoveTempVar("NO_HIT");
					}
					else
					{
						target.PlayTextEffect("I_SYS_Text_Effect_Skill", "SHOW_GUNGHO");
					}

					if (caster is Character && target is Character)
					{
						target.RemoveBuff(BuffId.SnowRollingAttach);
					}
				}
				else
				{
					SkillToolKnockdown(caster, target, knockType, knockDirection, power, verticalAngle, horizontalAngle, bound, knockdownRank);
					target.RemoveTempVar("NO_HIT");
				}
			}
		}

		public static void SkillTargetBuffAbility(ICombatEntity caster, Skill skill, AbilityId abilityId, BuffId buffId, int level, int arg2, int applyTime, int addAbilTime, int over, int rate)
			=> SkillTargetBuffAbility(caster, skill, caster.GetTargets(), abilityId, buffId, level, arg2, applyTime, addAbilTime, over, rate);

		public static void SkillTargetBuffAbility(ICombatEntity caster, Skill skill, IList<ICombatEntity> targets, AbilityId abilityId, BuffId buffId, int level, int arg2, int applyTime, int addAbilTime, int over, int rate)
		{
			if (caster.TryGetActiveAbility(abilityId, out var ability))
			{
				foreach (var target in targets)
				{
					var adjustedArg2 = arg2 == -1 ? ability.Level : arg2;
					target.StartBuff(buffId, level, adjustedArg2, TimeSpan.FromMilliseconds(applyTime + addAbilTime * ability.Level), caster);
				}
			}
		}
	}
}
