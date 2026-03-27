using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.Mon_Heal, PadName.BubeMagePrist_Heal)]
	public class Mon_Heal : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(10f);
			pad.Trigger.MaxUseCount = 1;
			pad.SetUpdateInterval(750);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!initiator.IsAlly(creator))
				return;

			if (initiator.Hp >= initiator.MaxHp)
				return;

			var modifier = new SkillModifier();
			var skillHitResult = SCR_SkillHit(creator, initiator, skill, modifier);
			var healAmount = skillHitResult.Damage;

			initiator.StartBuff(BuffId.Mon_Heal_Buff, skill.Level, healAmount, TimeSpan.FromMilliseconds(1000), creator);

			pad.Trigger.IncreaseUseCount();
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

		}
	}
}
