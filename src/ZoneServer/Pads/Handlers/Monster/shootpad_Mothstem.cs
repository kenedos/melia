using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.shootpad_Mothstem)]
	public class shootpad_Mothstem : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(23f);
			pad.SetUpdateInterval(100);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
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

			if (PadTargetDamage(pad, initiator, out var skillHit))
			{
				SkillResultTargetBuff(creator, skill, skillHit, BuffId.ElectricShock, 1, 0, 6000, 1, 20);
			}
		}
	}
}
