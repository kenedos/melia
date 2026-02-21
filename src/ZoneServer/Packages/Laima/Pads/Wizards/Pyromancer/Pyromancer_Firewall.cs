using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Shared.World;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Skills.Handlers.Pyromancer
{
	[Package("laima")]
	[PadHandler(PadName.Pyromancer_FireWall)]
	public class Pyromancer_FireWallPadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float KnockbackDistance = 20f;
		private const float KnockbackVelocity = 80f;
		private const int DebuffDuration = 10;
		private const int AllyBuffDuration = 6000;
		private const int BuffRefreshThreshold = 3000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

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

			if (creator.IsEnemy(initiator))
			{
				initiator.StartBuff(BuffId.FireWall_Debuff, skill.Level, 0, TimeSpan.FromSeconds(DebuffDuration), creator);
				pad.Trigger.LifeTime -= TimeSpan.FromSeconds(2);

				if (pad.Trigger.LifeTime < TimeSpan.Zero)
				{
					pad.Destroy();
					return;
				}

				this.ApplyKnockback(pad, creator, initiator);
			}
			else if (creator.IsAlly(initiator))
			{
				this.TryApplyAllyBuff(pad, creator, initiator, skill);
			}
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);

			if (!creator.TryGetActiveAbilityLevel(AbilityId.Pyromancer2, out var abilityLevel))
				return;

			var targets = pad.Trigger.GetAlliedEntities(creator);
			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);

			foreach (var target in targets)
			{
				if (target.IsDead)
					continue;

				if (target.TryGetBuff(BuffId.FireWall_Buff, out var existingBuff) && existingBuff.RemainingDuration.TotalMilliseconds > BuffRefreshThreshold)
					continue;

				this.TryApplyAllyBuff(pad, creator, target, skill);
			}
		}

		private void TryApplyAllyBuff(Pad pad, ICombatEntity creator, ICombatEntity target, Skill skill)
		{
			if (!creator.TryGetActiveAbilityLevel(AbilityId.Pyromancer2, out var abilityLevel))
				return;

			var casterInt = creator.Properties.GetFloat(PropertyName.INT);
			target.StartBuff(BuffId.FireWall_Buff, abilityLevel, casterInt, TimeSpan.FromMilliseconds(AllyBuffDuration), creator);
		}

		private void ApplyKnockback(Pad pad, ICombatEntity creator, ICombatEntity initiator)
		{
			if (!initiator.IsKnockdownable())
				return;

			var skill = pad.Skill;
			if (skill == null)
				return;

			var knockbackDirection = pad.Position.GetDirection(initiator.Position);

			var skillHitResult = new SkillHitResult { Damage = 0, Result = HitResultType.Hit };
			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult);

			skillHit.KnockBackInfo = new KnockBackInfo(pad.Position, initiator, HitType.KnockBack, (int)KnockbackVelocity, 10);
			skillHit.HitInfo.Type = HitType.KnockBack;

			initiator.ApplyKnockback(creator, skill, skillHit);

			Send.ZC_SKILL_HIT_INFO(creator, skillHit);
		}
	}
}
