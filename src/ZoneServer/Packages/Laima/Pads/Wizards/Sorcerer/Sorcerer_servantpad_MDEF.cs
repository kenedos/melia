using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Wizards.Sorcerer
{
	[Package("laima")]
	[PadHandler(PadName.servantpad_MDEF)]
	public class Sorcerer_servantpad_MDEFOverride : ICreatePadHandler, IEnterPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			pad.SetRange(100f);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(30);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;
			var level = Math.Clamp(pad.Skill.Level, 1, 10);

			PadTargetBuff(pad, initiator, RelationType.Party, 0, 0, BuffId.ServantMDEF_Buff, level, 0, 1800000, 1, 100, false);
		}
	}
}
