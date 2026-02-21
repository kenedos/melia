using System;
using Melia.Shared.Packages;
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
	[Package("laima")]
	[PadHandler(PadName.Pyromancer_HellBreath)]
	public class Pyromancer_HellBreathOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		private const int KnockbackDistance = 30;
		private const int KnockbackVelocityPerAbilityLevel = 5;

		/// <summary>
		/// Initializes the Hell Breath pad
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(30);
			pad.Trigger.MaxConcurrentUseCount = 2;
		}

		/// <summary>
		/// Cleans up the Hell Breath pad
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		/// <summary>
		/// Applies damage when the pad enters an enemy
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var target = args.Initiator;

			if (!creator.IsEnemy(target))
				return;

			if (pad.Trigger.AtCapacity)
				return;

			var skill = pad.Skill;
			var skillHitResult = SCR_SkillHit(creator, target, skill);
			target.TakeDamage(skillHitResult.Damage, creator);

			var hitInfo = new HitInfo(creator, target, skill, skillHitResult.Damage, HitResultType.Hit);
			Send.ZC_HIT_INFO(creator, target, hitInfo);

			// Apply fire effect
			target.PlayEffect("F_hit_fire", 1f);

			if (creator.TryGetActiveAbility(AbilityId.Pyromancer4, out var ability))
				this.ApplyKnockback(pad, creator, target, ability.Level);
		}

		private void ApplyKnockback(Pad pad, ICombatEntity creator, ICombatEntity initiator, int abilityLevel)
		{
			if (!initiator.IsKnockdownable())
				return;

			var skill = pad.Skill;
			if (skill == null)
				return;

			var knockbackDirection = pad.Position.GetDirection(initiator.Position);
			var knockbackVelocity = KnockbackVelocityPerAbilityLevel * abilityLevel;

			var skillHitResult = new SkillHitResult { Damage = 0, Result = HitResultType.Hit };
			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult);

			skillHit.KnockBackInfo = new KnockBackInfo(pad.Position, initiator, HitType.KnockBack, knockbackVelocity, 10);
			skillHit.HitInfo.Type = HitType.KnockBack;

			initiator.ApplyKnockback(creator, skill, skillHit);

			Send.ZC_SKILL_HIT_INFO(creator, skillHit);
		}
	}
}
