using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.Mon_FirePilla, PadName.BubeMageFire_FirePillar, PadName.Mon_FirePilla_5, PadName.Grinender_FirePillar)]
	public class Mon_FirePilla : ICreatePadHandler, IDestroyPadHandler, ILeavePadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			if (pad.Name == PadName.Mon_FirePilla_5)
				pad.SetRange(15f);
			else
				pad.SetRange(30f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(skill.Level * 40 + 6000);
			pad.SetUpdateInterval(3000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadRemoveBuff(pad, RelationType.Enemy, 0, 0, BuffId.Mon_FirePilla);

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			PadTargetBuff(pad, initiator, RelationType.Enemy, 0, 0, BuffId.Mon_FirePilla, skill.Level, 0, 3000, 1, 100);
		}
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadBuff(pad, RelationType.Enemy, 0, 0, BuffId.Mon_FirePilla, skill.Level, 0, 3000, 1, 100);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			PadTargetBuffRemove(pad, initiator, RelationType.Enemy, 0, 0, BuffId.Mon_FirePilla, false);
		}

	}
}
