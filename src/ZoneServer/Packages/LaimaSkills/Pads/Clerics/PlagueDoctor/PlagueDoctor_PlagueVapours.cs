using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Black Death Steam pad, the cloud the Plague Doctor's
	/// Black Death Steam poisons enemies with.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.PlagueDoctor_PlagueVapours)]
	public class PlagueDoctor_PlagueVapoursOverride : ICreatePadHandler, IDestroyPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}
	}
}
