using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.ShadowGaoler_circlewave)]
	public class ShadowGaoler_circlewave : ICreatePadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetRange(120f);
			pad.SetUpdateInterval(300);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(2000);
			pad.Trigger.MaxUseCount = 1;
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (initiator.MoveType == MoveType.Flying || initiator.IsJumping())
				return;

			PadTargetDamage(pad, initiator, RelationType.Enemy, 1f, 0, 0);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

		}
	}
}
