using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.gesti_Slow)]
	public class gesti_Slow : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const int SlowDurationMs = 2000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(30f);
			pad.SetUpdateInterval(800);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadRemoveBuff(pad, RelationType.Enemy, 0, 0, BuffId.UC_slowdown);
			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (!PadActivate(pad, initiator, RelationType.Enemy)) return;
			PadTargetBuffMon(pad, initiator, RelationType.Enemy, 0, 0, BuffId.UC_slowdown, 1, 0, SlowDurationMs, 1, 100);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (!PadDeactivate(pad, initiator, RelationType.Enemy)) return;
			PadTargetBuffRemoveMonster(pad, initiator, RelationType.Enemy, 0, 0, BuffId.UC_slowdown);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadBuffEnemyMonster(pad, RelationType.Enemy, 0, 0, BuffId.UC_slowdown, 1, 0, SlowDurationMs, 1, 100);
			PadDamageEnemy(pad, 0.3f, 0, 0, "F_hit_good", 0.3f);
		}
	}

	[PadHandler(PadName.BW_gesti_slow)]
	public class BW_gesti_slow : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const int SlowDurationMs = 2000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(60f);
			pad.SetUpdateInterval(800);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadRemoveBuff(pad, RelationType.Enemy, 0, 0, BuffId.UC_slowdown);
			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (!PadActivate(pad, initiator, RelationType.Enemy)) return;
			PadTargetBuffMon(pad, initiator, RelationType.Enemy, 0, 0, BuffId.UC_slowdown, 1, 0, SlowDurationMs, 1, 100);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (!PadDeactivate(pad, initiator, RelationType.Enemy)) return;
			PadTargetBuffRemoveMonster(pad, initiator, RelationType.Enemy, 0, 0, BuffId.UC_slowdown);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadBuffEnemyMonster(pad, RelationType.Enemy, 0, 0, BuffId.UC_slowdown, 1, 0, SlowDurationMs, 1, 100);
			PadDamageEnemy(pad, 0.3f, 0, 0, "F_hit_good", 0.3f);
		}
	}

	[PadHandler(PadName.gesti_spread1)]
	public class gesti_spread1 : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		private const float OuterRange = 100f;
		private const float InnerRange = 60f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.Area = new Donut(pad.Position, OuterRange, InnerRange);
			pad.Trigger.Area = pad.Area;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (initiator.MoveType == MoveType.Flying || initiator.IsJumping())
				return;

			PadTargetDamage(pad, initiator, RelationType.Enemy, 1f, 0, 0);
		}
	}

	[PadHandler(PadName.gesti_spread2)]
	public class gesti_spread2 : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		private const float OuterRange = 140f;
		private const float InnerRange = 100f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.Area = new Donut(pad.Position, OuterRange, InnerRange);
			pad.Trigger.Area = pad.Area;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (initiator.MoveType == MoveType.Flying || initiator.IsJumping())
				return;

			PadTargetDamage(pad, initiator, RelationType.Enemy, 1f, 0, 0);
		}
	}

	[PadHandler(PadName.gesti_spread3)]
	public class gesti_spread3 : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		private const float OuterRange = 180f;
		private const float InnerRange = 140f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.Area = new Donut(pad.Position, OuterRange, InnerRange);
			pad.Trigger.Area = pad.Area;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (initiator.MoveType == MoveType.Flying || initiator.IsJumping())
				return;

			PadTargetDamage(pad, initiator, RelationType.Enemy, 1f, 0, 0);
		}
	}
}
