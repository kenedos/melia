using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Oracle
{
	[Package("laima")]
	[PadHandler(PadName.Oracle_Foretell)]
	public class Oracle_ForetellOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(80f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(20000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			var buffId = BuffId.Foretell_Buff;
			if (creator.IsBuffActive(BuffId.ITEM_BUFF_VIBORA_AURA_LV4))
				buffId = BuffId.Foretell_Vibora_Buff;

			PadTargetBuff(pad, initiator, RelationType.Party, 0, 0, buffId, skill.Level, 0, 0, 1, 100, false);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Foretell_Buff, false);
			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Foretell_Vibora_Buff, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
		}
	}
}
