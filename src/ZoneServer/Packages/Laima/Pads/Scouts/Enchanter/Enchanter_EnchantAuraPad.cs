using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Pad handler for Enchant Aura.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Enchanter_EnchantAura)]
	public class Enchanter_EnchantAuraPadOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float AuraRadius = 100f;
		private const int UpdateInterval = 500;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			pad.SetRange(AuraRadius);
			pad.SetUpdateInterval(UpdateInterval);
			Send.ZC_NORMAL.PadUpdate(pad, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			if (creator == null || creator.IsDead || !creator.IsBuffActive(BuffId.EnchantAura_Buff))
				pad.Destroy();
		}
	}
}
