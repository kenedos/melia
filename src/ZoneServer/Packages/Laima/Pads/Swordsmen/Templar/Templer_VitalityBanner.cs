using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Swordsmen.Templar
{
	[Package("laima")]
	[PadHandler(PadName.Templer_VitalityBanner)]
	public class Templer_VitalityBannerPadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler
	{
		private const float AreaMultiplier = 4f;
		private static readonly TimeSpan BannerDuration = TimeSpan.FromSeconds(10);
		private static readonly object StateLock = new();
		private static readonly Dictionary<Pad, BannerState> ActiveBanners = new();

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var areaRadius = Math.Max(1f, pad.Skill.Data.SplashRange * AreaMultiplier);

			pad.SetRange(areaRadius);
			pad.Trigger.LifeTime = BannerDuration;
			pad.Trigger.MaxActorCount = int.MaxValue;

			lock (StateLock)
				ActiveBanners[pad] = new BannerState(args.Creator);

			Send.ZC_NORMAL.PadUpdate(pad, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			BannerState state;

			lock (StateLock)
			{
				if (!ActiveBanners.TryGetValue(pad, out state))
					state = null;
				else
					ActiveBanners.Remove(pad);
			}

			if (state != null)
			{
				foreach (var target in state.Targets)
					this.RemoveBuff(target, state.Caster);
			}

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (initiator == null || initiator.IsDead)
				return;

			var skillLevel = Math.Clamp(pad.Skill.Level, 1, 10);
			var remainingDuration = Math.Max(1, (int)pad.Trigger.RemainingLifeTime.TotalMilliseconds);

			PadTargetBuff(pad, initiator, RelationType.Party, 0, 0, BuffId.VitalityBanner_Buff, skillLevel, 0, remainingDuration, 1, 100, false);

			lock (StateLock)
			{
				if (ActiveBanners.TryGetValue(pad, out var state))
					state.Targets.Add(initiator);
			}
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (initiator == null)
				return;

			object caster = args.Creator;

			lock (StateLock)
			{
				if (ActiveBanners.TryGetValue(pad, out var state))
				{
					state.Targets.Remove(initiator);
					caster = state.Caster;
				}
			}

			this.RemoveBuff(initiator, caster);
		}

		private void RemoveBuff(ICombatEntity target, object expectedCaster)
		{
			if (target == null)
				return;

			if (!target.TryGetBuff(BuffId.VitalityBanner_Buff, out var vitalityBuff))
				return;

			if (expectedCaster != null && !ReferenceEquals(vitalityBuff.Caster, expectedCaster))
				return;

			target.StopBuff(BuffId.VitalityBanner_Buff);
		}

		private sealed class BannerState
		{
			public object Caster { get; }
			public HashSet<ICombatEntity> Targets { get; } = new();

			public BannerState(object caster)
			{
				this.Caster = caster;
			}
		}
	}
}
