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
	[PadHandler(PadName.FireClaw_Pad)]
	public class FireClaw_PadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		private const int BurnDurationMilliseconds = 5000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(20f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1500);
			pad.Trigger.MaxActorCount = 3;
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

			if (!PadTargetDamage(pad, initiator, out var skillHit, RelationType.Enemy, 1f, 0, -1))
				return;

			if (skillHit.HitInfo.Damage > 0)
			{
				var burnDamage = Math.Max(1, skillHit.HitInfo.Damage / 10);
				initiator.StartBuff(BuffId.Fire, skill.Level, burnDamage, TimeSpan.FromMilliseconds(BurnDurationMilliseconds), creator);
			}
		}
	}
}
