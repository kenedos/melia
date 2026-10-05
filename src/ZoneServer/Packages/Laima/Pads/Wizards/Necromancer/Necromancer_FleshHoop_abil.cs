using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Pads.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[PadHandler(PadName.Necromancer_FleshHoop_abil)]
	public class Necromancer_FleshHoop_abilOverride : ICreatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			pad.SetRange(48f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
			pad.Trigger.MaxActorCount = 100;
		}
	}
}
