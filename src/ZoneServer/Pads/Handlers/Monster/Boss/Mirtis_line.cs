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
	/// <summary>
	/// Handler for Mirtis's rotating laser line, which can be jumped over.
	/// </summary>
	[PadHandler(PadName.Mirtis_line)]
	public class Mirtis_line : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float Range = 200f;
		private const float HalfWidth = 10f;
		private const float UpdateTermMs = 200f;
		private const float SpinRate = 0.9f;
		private const float SpinDegreesPerUpdate = SpinRate * (180f / MathF.PI) * (UpdateTermMs / 1000f);
		private const float BackstopLifeTimeMs = 30000f;

		// The client draws the laser 45° off the pad's direction.
		private const float AreaAngleOffset = 45f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			SetLineArea(pad);
			pad.SetUpdateInterval(UpdateTermMs);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(BackstopLifeTimeMs);
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

			if (initiator.IsJumping())
				return;

			PadTargetDamage(pad, initiator, RelationType.Enemy, 1f, 0, 0);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			pad.Direction = pad.Direction.AddDegreeAngle(SpinDegreesPerUpdate);
			SetLineArea(pad);

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f, damageCondition: target => !target.IsJumping());
		}

		/// <summary>
		/// Sets the pad's area to a line through its center along its direction.
		/// </summary>
		/// <param name="pad"></param>
		private static void SetLineArea(Pad pad)
		{
			pad.Area = Square.Centered(pad.Position, pad.Direction.AddDegreeAngle(AreaAngleOffset), Range * 2, HalfWidth);
			pad.Trigger.Area = pad.Area;
		}
	}
}
