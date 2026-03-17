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
	[PadHandler(PadName.Elemental_Burst_Pad_Ice)]
	public class Elemental_Burst_Pad_IceOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
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

			var damageRate = 1f;
			if (ZoneServer.Instance.World.IsPVP)
				damageRate = 0.5f;

			PadTargetDamage(pad, initiator, RelationType.Enemy, damageRate, 0, 0);
			initiator.StartBuff(BuffId.Freeze, skill.Level, 0, TimeSpan.FromSeconds(5), creator);
		}
	}
}
