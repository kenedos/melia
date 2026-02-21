using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Sadhu
{
	[Package("laima")]
	[PadHandler(PadName.Sadhu_Prakriti)]
	public class Sadhu_PrakritiOverride : ICreatePadHandler, IDestroyPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(50f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
			PadRemoveBuff(pad, RelationType.Party, 0, 0, BuffId.Prakriti_Buff);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			PadTargetBuffRemove(pad, initiator, RelationType.Party, 0, 0, BuffId.Prakriti_Buff, true);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadBuff(pad, RelationType.Party, 0, 0, BuffId.Prakriti_Buff, skill.Level, 0, 0, 1, 100);
		}
	}
}
