using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.Ironbaum_fire2)]
	public class Ironbaum_fire2 : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(30f);
			pad.SetUpdateInterval(200);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(2000);
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

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);
		}
	}
}
