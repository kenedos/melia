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
	[PadHandler(PadName.Colimencia_circlewave)]
	public class Colimencia_circlewave : ICreatePadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetRange(90f);
			pad.SetUpdateInterval(300);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(1500);
			pad.Trigger.MaxUseCount = 1;
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (PadTargetDamage(pad, initiator, skillHit: out SkillHitInfo skillHitResult))
				SkillResultTargetBuff(creator, skill, skillHitResult, BuffId.UC_freeze, 1, 0, 3000, 1, 100);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;
		}
	}
}
