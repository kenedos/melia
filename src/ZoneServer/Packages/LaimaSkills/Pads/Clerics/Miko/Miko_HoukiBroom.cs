using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for a patch swept by Sweeping, which marks the allies standing
	/// on it.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Miko_HoukiBroom)]
	public class Miko_HoukiBroomOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.HoukiBroomSkllvup_Buff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (initiator == args.Creator || initiator.IsAlly(args.Creator))
				initiator.StartBuff(BuffId.HoukiBroomSkllvup_Buff, pad.Skill.Level, 0, pad.Trigger.RemainingLifeTime, args.Creator, pad.Skill.Id);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			args.Initiator.StopBuff(BuffId.HoukiBroomSkllvup_Buff);
		}
	}
}
