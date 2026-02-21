using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.Monster_Slow, PadName.Summon_Lethargy)]
	public class Monster_Slow : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(50f);
			pad.SetUpdateInterval(750);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(2000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			var buffTime = 2000;

			PadTargetBuffAfterBuffCheck(pad, initiator, RelationType.Enemy, 0, 0, BuffId.UC_slowdown, BuffId.UC_slowdown, 1, 0, (int)buffTime, 1, 100, false);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!PadDeactivate(pad, initiator, RelationType.Enemy)) return;
			PadTargetBuffRemoveMonster(pad, initiator, RelationType.Enemy, -5000, -1000, BuffId.UC_slowdown);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var buffTime = 2000;

			PadBuffEnemyMonster(pad, RelationType.Enemy, -500, -10, BuffId.UC_slowdown, 1, 0, (int)buffTime, 1, 100);
		}
	}
}
