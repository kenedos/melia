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
	[PadHandler(PadName.Oracle_DivineMight_abil)]
	public class Oracle_DivineMight_abilOverride : ICreatePadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetRange(50f);
			pad.SetUpdateInterval(400);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(800);
			pad.Trigger.MaxUseCount = 10;
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (initiator.Race != RaceType.Velnias)
				return;

			PadTargetDamage(pad, initiator, RelationType.Enemy, 1f, 0, -1);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
		}
	}
}
