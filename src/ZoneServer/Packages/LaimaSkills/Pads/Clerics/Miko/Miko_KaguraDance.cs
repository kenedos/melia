using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for the leaves of Kagura, which only show the dance.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Miko_KaguraDance)]
	public class Miko_KaguraDanceOverride : ICreatePadHandler, IDestroyPadHandler
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
