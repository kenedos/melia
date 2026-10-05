using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cryomancer_Gust)]
	public class Cryomancer_GustOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int FreezeDurationMilliSeconds = 7000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var skill = pad.Skill;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRectangleRange(creator.Direction, 40, 120);
			pad.SetUpdateInterval(100);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1200);
			var maxTargets = 2 + skill.Level;
			pad.Trigger.MaxUseCount = maxTargets;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			var targets = pad.Trigger.GetAttackableEntities(creator)
				.Where(t => creator.IsEnemy(t))
				.OrderBy(t => t.IsBuffActive(BuffId.Cryomancer_Freeze) ? 1 : 0);

			foreach (var target in targets)
			{
				if (pad.Trigger.IncreaseUseCount())
					break;

				target.StartBuff(BuffId.Cryomancer_Freeze, TimeSpan.FromMilliseconds(FreezeDurationMilliSeconds), creator);
			}
		}
	}
}
