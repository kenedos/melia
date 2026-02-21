using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Psychokino
{
	[Package("laima")]
	[PadHandler(PadName.HeavyGravity_PAD)]
	public class HeavyGravity_PADOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(120f);
			pad.SetUpdateInterval(200);
			var duration = 5000;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(duration);
			PadSelectPadKill(pad, PadName.Psychokino_Raise, 120f);
			var value = 13;
			pad.Trigger.MaxActorCount = value;
			pad.Trigger.MaxConcurrentUseCount = value;
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

			if (!creator.IsEnemy(initiator))
				return;

			if (pad.Trigger.AtCapacity)
				return;

			pad.Trigger.ActivateCount++;
			PadTargetBuffMon(pad, initiator, RelationType.Enemy, 0, 0, BuffId.HeavyGravity_Debuff, skill.Level, 0, 0, 1, 100);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator))
				return;

			if (!initiator.IsBuffActive(BuffId.HeavyGravity_Debuff))
				return;

			pad.Trigger.ActivateCount--;
			PadTargetBuffRemoveMonster(pad, initiator, RelationType.Enemy, 0, 0, BuffId.HeavyGravity_Debuff);
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
