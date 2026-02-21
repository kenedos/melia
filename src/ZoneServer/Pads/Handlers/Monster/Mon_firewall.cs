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
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.Mon_firewall, PadName.boss_firewall)]
	public class Mon_firewall : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float KnockbackDistance = 20f;
		private const float KnockbackVelocity = 40f;
		private const int DebuffDuration = 2;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(5000);

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetUpdateInterval(1000);
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

			var skillHitResult = SCR_SkillHit(creator, initiator, skill);
			var dotDamage = skillHitResult.Damage * 0.15f;

			initiator.StartBuff(BuffId.Mon_FireWall, skill.Level, dotDamage, TimeSpan.FromSeconds(DebuffDuration), creator);
			pad.Trigger.LifeTime -= TimeSpan.FromSeconds(2);

			if (pad.Trigger.LifeTime < TimeSpan.Zero)
			{
				pad.Destroy();
				return;
			}

			this.ApplyKnockback(pad, creator, initiator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);
		}

		private void ApplyKnockback(Pad pad, ICombatEntity creator, ICombatEntity initiator)
		{
			if (!initiator.IsKnockdownable())
				return;

			var skill = pad.Skill;
			if (skill == null)
				return;

			var skillHitResult = new SkillHitResult { Damage = 0, Result = HitResultType.Hit };
			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult);

			skillHit.KnockBackInfo = new KnockBackInfo(pad.Position, initiator, HitType.KnockBack, (int)KnockbackVelocity, 10);
			skillHit.HitInfo.Type = HitType.KnockBack;

			initiator.ApplyKnockback(creator, skill, skillHit);

			Send.ZC_SKILL_HIT_INFO(creator, skillHit);
		}
	}
}
