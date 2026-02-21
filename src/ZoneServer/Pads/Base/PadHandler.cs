using System;
using System.Threading;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Logging;

namespace Melia.Zone.Pads.Base
{
	public class PadHandler : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			if (args.Trigger is not Pad pad) return;
			if (pad.Creator is not ICombatEntity creator) return;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			if (args.Trigger is not Pad pad) return;
			if (pad.Creator is not ICombatEntity creator) return;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			throw new NotImplementedException();
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			throw new NotImplementedException();
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
