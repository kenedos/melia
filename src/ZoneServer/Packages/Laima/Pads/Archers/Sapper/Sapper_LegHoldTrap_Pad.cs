using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.HandlersOverride.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper Leg Hold Trap damage pad.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Sapper_LegHoldTrap_Pad)]
	public class Sapper_LegHoldTrap_PadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float PadRange = 100f;
		private const int UpdateIntervalMs = 1000;
		private const int PadLifetimeMs = 10000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(PadRange);
			pad.SetUpdateInterval(UpdateIntervalMs);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PadLifetimeMs);
			pad.Trigger.MaxActorCount = skill.GetPVPValue(10);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			PadRemoveBuff(pad, RelationType.Enemy, 0, 0, BuffId.Common_Slow);
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

			PadTargetBuff(pad, initiator, RelationType.Enemy, 0, 0, BuffId.Common_Slow, skill.Level, 0, 0, 1, 100, false);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator))
				return;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Common_Slow, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);
		}
	}
}
