using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.shootpad_Denoptic)]
	public class shootpad_Denoptic : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(15f);
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

			if (PadTargetDamage(pad, initiator, skillHit: out SkillHitInfo skillHitResult))
			{
				SkillResultTargetBuff(creator, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 5, -1, skillHitResult);
				SkillResultKnockTarget(creator, null, skill, skillHitResult, KnockType.KnockDown, KnockDirection.Random, 150, 30, 0, 2, 5);
			}
		}
	}
}
