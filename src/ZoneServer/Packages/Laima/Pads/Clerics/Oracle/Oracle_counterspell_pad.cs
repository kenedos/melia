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
	[PadHandler(PadName.counterspell_pad)]
	public class Oracle_counterspell_padOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(30f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = skill.Properties.CaptionTime;
			pad.BlocksMagicPads = true;

			PadKillEnemyMagicPads(pad);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.CounterSpell_Buff);
			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
			if (!creator.IsAlly(initiator)) return;

			PadTargetBuff(pad, initiator, RelationType.Friendly, 0, 0, BuffId.CounterSpell_Buff, skill.Level, 0, 0, 1, 100, false);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			if (!creator.IsAlly(initiator)) return;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.CounterSpell_Buff, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadKillEnemyMagicPads(pad);
		}
	}
}
