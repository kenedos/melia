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
	[PadHandler(PadName.Oracle_DivineMight)]
	public class Oracle_DivineMightOverride : ICreatePadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetRange(150f);
			pad.SetUpdateInterval(400);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(800);
			pad.Trigger.MaxUseCount = 5;
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			PadTargetBuff(pad, initiator, RelationType.Friendly, 0, -1, BuffId.DivineMight_Buff, skill.Level, 0, 0, 1, 100, false);
			PadTargetBuff(pad, initiator, RelationType.Enemy, 0, -1, BuffId.DivineMight_Hidden_Debuff, skill.Level, 0, 0, 1, 100, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
		}
	}
}
