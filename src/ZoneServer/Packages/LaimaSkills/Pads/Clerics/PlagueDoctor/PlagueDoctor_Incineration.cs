using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Incineration pad, the burning ground the Plague
	/// Doctor's Incineration ignites enemies on.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.PlagueDoctor_Incineration)]
	public class PlagueDoctor_IncinerationOverride : ICreatePadHandler, IDestroyPadHandler
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
