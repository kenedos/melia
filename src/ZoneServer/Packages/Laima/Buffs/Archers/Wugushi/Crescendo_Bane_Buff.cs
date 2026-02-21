using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handler for the Crescendo Bane self-buff.
	/// While active, any poison debuffs applied by the caster are
	/// automatically accelerated (faster tick rate and reduced duration).
	/// Also handles re-acceleration when accelerated poisons are
	/// refreshed or gain new stacks.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	///
	/// Hooks:
	/// - Buff.BuffActivated (static): Catches new poisons from a caster
	///   with CrescendoBane, and overbuffs where the counter changes.
	/// - BuffComponent.BuffStarted (per-entity): Subscribed on the target
	///   when a buff is first accelerated. Catches ALL re-applications
	///   including the Extend case (max overbuff) that BuffActivated
	///   misses. Works for any poison-tagged buff universally.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Crescendo_Bane_Buff)]
	public class Crescendo_Bane_BuffOverride : BuffHandler
	{
		private const string AccelerationLevelVar = "Melia.CrescendoBane.SkillLevel";

		static Crescendo_Bane_BuffOverride()
		{
			Buff.BuffActivated += OnBuffActivated;
		}

		/// <summary>
		/// When any poison buff is activated (new or overbuffed with
		/// counter change), check if the caster has CrescendoBane and
		/// accelerate the buff if so.
		/// </summary>
		private static void OnBuffActivated(Buff buff)
		{
			if (!buff.Data.Tags.HasAny(BuffTag.Poison))
				return;

			if (buff.Caster is not ICombatEntity caster)
				return;

			if (!caster.TryGetBuff(BuffId.Crescendo_Bane_Buff, out var crescendoBuff))
				return;

			var skillLevel = (int)crescendoBuff.NumArg1;
			if (skillLevel <= 0)
				return;

			AccelerateBuff(buff, skillLevel);
		}

		/// <summary>
		/// When any buff starts/overbuffs/extends on a target that has
		/// previously-accelerated poisons, re-apply the acceleration.
		/// This catches the Extend case (max overbuff) that the static
		/// BuffActivated event misses.
		/// </summary>
		private static void OnTargetBuffStarted(ICombatEntity entity, Buff buff)
		{
			if (!buff.Data.Tags.HasAny(BuffTag.Poison))
				return;

			if (!buff.Vars.TryGetInt(AccelerationLevelVar, out var skillLevel) || skillLevel <= 0)
				return;

			var rate = GetAccelerationRate(skillLevel);
			var multiplier = 1f - rate;

			if (buff.HasUpdateTime)
			{
				var expectedUpdateTime = TimeSpan.FromTicks((long)(buff.Data.UpdateTime.Ticks * multiplier));
				buff.UpdateTime = expectedUpdateTime;
				buff.NextUpdateTime = DateTime.Now.Add(expectedUpdateTime);
			}

			if (buff.HasDuration)
			{
				var acceleratedFullDuration = TimeSpan.FromTicks((long)(buff.Duration.Ticks * multiplier));
				if (buff.RemainingDuration > acceleratedFullDuration + TimeSpan.FromMilliseconds(500))
				{
					buff.IncreaseDuration(acceleratedFullDuration);
					buff.NotifyUpdate();
				}
			}
		}

		/// <summary>
		/// Returns the acceleration rate for the given skill level.
		/// Formula: 20% + 2% per level, hard capped at 60%.
		/// </summary>
		public static float GetAccelerationRate(int skillLevel)
		{
			return Math.Min(0.60f, 0.20f + 0.02f * skillLevel);
		}

		/// <summary>
		/// Accelerates a poison buff by reducing its tick interval
		/// and remaining duration. On first application, the remaining
		/// duration is reduced proportionally. On re-application after
		/// an overbuff (where ExtendDuration reset to full), the
		/// duration is capped to the accelerated full duration.
		/// Also subscribes to the target's BuffStarted event on first
		/// acceleration to handle future re-applications universally.
		/// </summary>
		public static void AccelerateBuff(Buff buff, int skillLevel)
		{
			var rate = GetAccelerationRate(skillLevel);
			var multiplier = 1f - rate;
			var isFirstAcceleration = !buff.Vars.TryGetInt(AccelerationLevelVar, out _);

			if (buff.HasUpdateTime)
			{
				var newUpdateTime = TimeSpan.FromTicks((long)(buff.Data.UpdateTime.Ticks * multiplier));
				buff.UpdateTime = newUpdateTime;
				buff.NextUpdateTime = DateTime.Now.Add(newUpdateTime);
			}

			if (buff.HasDuration)
			{
				var durationChanged = false;

				if (isFirstAcceleration)
				{
					var newRemaining = TimeSpan.FromTicks((long)(buff.RemainingDuration.Ticks * multiplier));
					if (newRemaining > TimeSpan.Zero)
					{
						buff.IncreaseDuration(newRemaining);
						durationChanged = true;
					}
				}
				else
				{
					var acceleratedFullDuration = TimeSpan.FromTicks((long)(buff.Duration.Ticks * multiplier));
					if (buff.RemainingDuration > acceleratedFullDuration)
					{
						buff.IncreaseDuration(acceleratedFullDuration);
						durationChanged = true;
					}
				}

				if (durationChanged)
					buff.NotifyUpdate();
			}

			buff.Vars.SetInt(AccelerationLevelVar, skillLevel);

			if (isFirstAcceleration)
			{
				var buffComp = buff.Target.Components.Get<BuffComponent>();
				if (buffComp != null)
				{
					buffComp.BuffStarted -= OnTargetBuffStarted;
					buffComp.BuffStarted += OnTargetBuffStarted;
				}
			}
		}
	}
}
