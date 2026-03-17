using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Elementalist
{
	[Package("laima")]
	[PadHandler(PadName.Elemental_Burst_Pad_Fire2)]
	public class Elemental_Burst_Pad_Fire2Override : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(20f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(500);
			pad.Trigger.MaxActorCount = 5;
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

			if (!creator.IsEnemy(initiator))
				return;

			PadTargetDamage(pad, initiator, out var skillHit, RelationType.Enemy, 1f, 0, 0);

			if (skillHit != null && skillHit.HitInfo.Damage > 0)
			{
				var burnDamage = Math.Max(1, (int)(skillHit.HitInfo.Damage * 0.1f));
				initiator.StartBuff(BuffId.Fire, skill.Level, burnDamage, TimeSpan.FromSeconds(5), creator);
			}
		}
	}
}
