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
	[PadHandler(PadName.Monster_FireBall, PadName.Mon_Fireball_santan)]
	public class Mon_Fireball : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.Trigger.MaxUseCount = 1;
			pad.SetRange(15f);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(30000);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

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

			if (initiator.IsDead)
				return;

			var modifier = new SkillModifier();
			var skillHitResult = SCR_SkillHit(creator, initiator, skill, modifier);

			initiator.TakeDamage(skillHitResult.Damage, creator);

			var hitInfo = new HitInfo(creator, initiator, skill, skillHitResult.Damage, skillHitResult.Result);
			hitInfo.ForceId = ForceId.GetNew();
			Send.ZC_HIT_INFO(creator, initiator, hitInfo);

			pad.Trigger.IncreaseUseCount();
		}
	}
}
