using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Helpers
{
	public static class SkillResultHelper
	{
		public static void SkillResultKnockTarget(ICombatEntity caster, ICombatEntity target, Skill skill, SkillHitInfo ret, KnockType knockType, KnockDirection knockDirection, float power, float verticalAngle, float horizontalAngle, int bound, int knockdownRank = 0)
		{
			target ??= ret.Target;
			if (ret != null)
			{
				if (ret.HitInfo.ResultType == HitResultType.Dodge || ret.HitInfo.ResultType == HitResultType.Block)
					return;
				var key = GetSkillSyncKey(caster, ret.HitInfo);
				StartSyncPacket(caster, key);
				SkillToolKnockdown(caster, target, knockType, knockDirection, power, verticalAngle, horizontalAngle, bound, knockdownRank);
				EndSyncPacket(caster, key, 0.1f);
			}
			else
			{
				SkillToolKnockdown(caster, target, knockType, knockDirection, power, verticalAngle, horizontalAngle, bound, knockdownRank);
			}
		}

		/// <summary>
		/// Calculates diminishing returns for crowd control effects.
		/// Tracks recent CC applications on the target and reduces duration
		/// for repeated applications within a time window.
		/// </summary>
		/// <param name="target">The target receiving the CC effect.</param>
		/// <param name="caster">The caster applying the CC effect.</param>
		/// <param name="durationMs">The base duration in milliseconds.</param>
		/// <param name="minimum">The minimum duration floor in milliseconds.</param>
		/// <param name="className">The skill class name for tracking.</param>
		/// <param name="diminishCategory">Category for grouping similar CCs (0 = per-skill).</param>
		/// <returns>The adjusted duration in milliseconds after diminishing returns.</returns>
		private static float GetDiminishingMSTime(ICombatEntity target, ICombatEntity caster, int durationMs, int minimum, string className, int diminishCategory)
		{
			// Diminishing returns constants
			const float DiminishWindowMs = 15000f; // 15 second window for DR tracking
			const float DiminishReductionPerStack = 0.25f; // 25% reduction per recent application
			const int MaxDiminishStacks = 3; // After 3 applications, target becomes immune

			// Generate a key for tracking this specific CC type
			var drKey = diminishCategory > 0
				? $"Melia.DR.Category.{diminishCategory}"
				: $"Melia.DR.Skill.{className}";

			var drTimeKey = $"{drKey}.LastTime";
			var drStackKey = $"{drKey}.Stacks";

			// Use milliseconds since a fixed epoch for time tracking (fits in float)
			var nowMs = (float)(DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds;
			var lastApplicationMs = target.GetTempVar(drTimeKey);
			var currentStacks = (int)target.GetTempVar(drStackKey);

			// Check if we're still within the diminishing returns window
			var timeSinceLastApplicationMs = nowMs - lastApplicationMs;
			if (timeSinceLastApplicationMs > DiminishWindowMs || lastApplicationMs == 0)
			{
				// Window expired or first application, reset stacks
				currentStacks = 0;
			}

			// Check for immunity (max stacks reached)
			if (currentStacks >= MaxDiminishStacks)
			{
				// Grant temporary immunity and reset
				target.SetTempVar(drTimeKey, nowMs);
				target.SetTempVar(drStackKey, 0f);
				return 0; // Immune
			}

			// Calculate reduced duration
			var reductionMultiplier = 1f - (currentStacks * DiminishReductionPerStack);
			var reducedDuration = durationMs * reductionMultiplier;

			// Apply minimum floor
			if (minimum > 0 && reducedDuration < minimum)
				reducedDuration = minimum;

			// Update tracking
			target.SetTempVar(drTimeKey, nowMs);
			target.SetTempVar(drStackKey, (float)(currentStacks + 1));

			return reducedDuration;
		}

		/// <summary>
		/// Spins the target around itself and interrupts any skills target is
		/// currently casting.
		/// </summary>
		public static async Task SkillResultSpinObject(ICombatEntity caster, Skill skill, float spinDelay, int spinCount, float rotSecond, float velocityChangeTerm, float sleepTime, List<SkillHitInfo> hits = null)
		{
			if (caster.IsDead)
				return;

			foreach (var hit in hits)
			{
				if (hit.HitInfo.ResultType == HitResultType.Dodge || hit.HitInfo.ResultType == HitResultType.Block)
					continue;

				var target = hit.Target;

				if (target == null || target.IsDead)
					continue;

				if (target.Properties.GetString(PropertyName.Size) != "XL")
				{
					if (skill.Id == SkillId.Musketeer_HeadShot)
					{
						spinCount *= 1000;
						spinCount = (int)GetDiminishingMSTime(target, caster, spinCount, 0, skill.Data.ClassName, 0);
						spinCount = Convert.ToInt32(spinCount / 1000);
						if (spinCount == 0)
							Send.ZC_NORMAL.PlayTextEffect(target, caster, "I_SYS_Text_Effect_Skill", 0, ScpArgMsg("HeadShot_Spin_Immune"));
					}

					if (spinCount != 0)
					{
						if (target.IsCasting() && target.Components.TryGet<CombatComponent>(out var combat))
							combat.InterruptCasting();

						Send.ZC_NORMAL.SpinObject(target, spinDelay, spinCount, rotSecond, velocityChangeTerm);
					}
				}
			}
		}

		public static void SkillResultTargetBuff(ICombatEntity caster, Skill skill, BuffId buffId, int level, float arg2, float buffTime, int over, int percent, int updateTime, SkillHitInfo hit)
		{
			SkillResultTargetBuff(caster, skill, buffId, level, arg2, buffTime, over, percent, updateTime, new List<SkillHitInfo>() { hit });
		}

		public static void SkillResultTargetBuff(ICombatEntity caster, Skill skill, BuffId buffId, int level, float arg2, float buffTime, int over, int percent, int updateTime = -1, IList<SkillHitInfo> hits = null)
		{
			if (caster.IsDead)
				return;

			if (hits == null)
				return;

			var SCR_Calc_Status_Chance = ScriptableFunctions.Status.Get("SCR_Calc_Status_Chance");
			var SCR_Calc_Status_Duration = ScriptableFunctions.Status.Get("SCR_Calc_Status_Duration");

			foreach (var hit in hits)
			{
				var target = hit.Target;
				var ret = hit.HitInfo;

				if (ret == null || ret.Damage <= 0)
					continue;

				var finalChance = SCR_Calc_Status_Chance(caster, target, skill, buffId, percent);
				if (finalChance < 100 && RandomProvider.Next(1, 101) > finalChance)
					continue;

				var finalDuration = SCR_Calc_Status_Duration(caster, target, skill, buffId, buffTime);
				if (ret != null)
				{
					var key = GetSkillSyncKey(caster, ret);
					StartSyncPacket(caster, key);

					if (buffId == BuffId.BlandirCadena_Debuff)
						arg2 = ret.Damage;

					var buff = target.StartBuff(buffId, level, arg2, TimeSpan.FromMilliseconds(finalDuration), caster, skill.Id);
					if (buff != null)
					{
						buff.OverbuffCounter = over;

						if (updateTime != -1)
							buff.SetUpdateTime(updateTime);
					}

					EndSyncPacket(caster, key);
				}
				else
				{
					var buff = target.StartBuff(buffId, level, arg2, TimeSpan.FromMilliseconds(finalDuration), caster, skill.Id);
					if (buff != null)
					{
						buff.OverbuffCounter = over;

						if (updateTime != -1)
							buff.SetUpdateTime(updateTime);
					}
				}
			}
		}

		public static void SkillResultKnockTarget(ICombatEntity caster, Skill skill, KnockType knockType, KnockDirection knockDirection, float power, float verticalAngle, float horizontalAngle, int bound, int knockdownRank, List<SkillHitInfo> hits = null)
		{
			foreach (var hit in hits)
			{
				if (hit.HitInfo.ResultType == HitResultType.Dodge || hit.HitInfo.ResultType == HitResultType.Block)
					continue;
				var key = GetSkillSyncKey(caster, hit.HitInfo);
				StartSyncPacket(caster, key);
				SkillToolKnockdown(caster, hit.Target, knockType, knockDirection, power, verticalAngle, horizontalAngle, bound, knockdownRank);
				EndSyncPacket(caster, key, 0.1f);
			}
		}

		public static void SkillResultSelfBuff(ICombatEntity caster, Skill skill, BuffId buffId, int level, int arg2, float buffTime, int over, int percent, int updateTime)
		{
			if (caster is not Character && caster.CheckBoolTempVar("BUNSIN"))
				return;

			if (percent < 100 && RandomProvider.Next(1, 101) > percent)
				return;

			var buff = caster.StartBuff(buffId, level, arg2, TimeSpan.FromMilliseconds(buffTime), caster);
			if (buff == null)
			{
				Log.Warning($"SkillResultSelfBuff: Buff {buffId} is null.");
				return;
			}
			buff.OverbuffCounter = over;
			if (updateTime != -1)
				buff.SetUpdateTime(updateTime);
		}

		public static void SkillResultTargetBuff(ICombatEntity caster, Skill skill, SkillHitInfo ret, BuffId buffId, int level, int arg2, int applyTime, int over, float rate)
		{
			if (caster.IsDead)
				return;

			var tgtList = caster.GetTargets();
			if (tgtList.Length > 0)
			{
				var key = GetSkillSyncKey(caster, ret?.HitInfo);
				StartSyncPacket(caster, key);
				for (var i = 0; i < tgtList.Length; i++)
				{
					var currentTarget = tgtList[i];
					currentTarget.StartBuff(buffId, level, arg2, TimeSpan.FromMilliseconds(applyTime), caster);
				}
				EndSyncPacket(caster, key, 0);
			}
		}

		private static int GetSkillSyncKey(ICombatEntity caster, HitInfo ret)
		{
			return ZoneServer.Instance.World.CreateSkillHandle();
		}

		private static void StartSyncPacket(ICombatEntity caster, int key, float f1 = 1)
		{
			Send.ZC_SYNC_START(caster, key, f1);
		}

		private static void EndSyncPacket(ICombatEntity caster, int key, float f1 = 0)
		{
			Send.ZC_SYNC_END(caster, key, f1);
		}
	}
}
