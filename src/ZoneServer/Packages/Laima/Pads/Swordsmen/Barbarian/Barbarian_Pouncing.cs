using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Barbarian_Pouncing)]
	public class Barbarian_PouncingOverride : ICreatePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			// Note: The skill handler creates the Square area with proper dimensions
			// We don't override it here
			pad.SetUpdateInterval(100);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(3500);
			pad.Trigger.MaxActorCount = 6;
			pad.FollowsTarget(creator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (!creator.IsBuffActive(BuffId.Pouncing_Buff))
			{
				pad.Destroy();
				return;
			}

			pad.Direction = creator.Direction;
			pad.SetRectangleRange(creator.Direction, 35f, 70f);

			// Deal damage using PadDamageEnemy
			PadDamageEnemy(pad, 1f, 0, 0, "", 3, 0f, 0f);
		}
	}
}
