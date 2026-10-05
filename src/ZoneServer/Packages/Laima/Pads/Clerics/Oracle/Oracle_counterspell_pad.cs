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
	[PadHandler(PadName.counterspell_pad)]
	public class Oracle_counterspell_padOverride : ICreatePadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var skill = pad.Skill;

			pad.SetRange(50f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000 + skill.Level * 1000);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
			if (!creator.IsAlly(initiator)) return;

			PadTargetBuff(pad, initiator, RelationType.Friendly, 0, 0, BuffId.CounterSpell_Buff, skill.Level, 0, 0, 1, 100, false);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			if (!creator.IsAlly(initiator)) return;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.CounterSpell_Buff, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
		}
	}
}
