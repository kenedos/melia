using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Handler for the Mineloader laser pad. Deals damage while inside
	/// and moves towards the target position.
	/// </summary>
	[PadHandler(PadName.mineloader_laser)]
	public class mineloader_laser : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float PadDuration = 4300f;
		private const float MoveDistance = 200f;
		private const float MoveSpeed = MoveDistance / (PadDuration / 1000f);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(150f);
			pad.SetUpdateInterval(500);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PadDuration);

			if (skill.Vars.TryGet<Position>("Melia.Pad.TargetPos", out var targetPos))
			{
				pad.SetDestPos(targetPos, MoveSpeed, 0, false);
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadDamageEnemy(pad, 1f, 0, -1, "None", 1, 0f, 0f);
		}
	}
}
