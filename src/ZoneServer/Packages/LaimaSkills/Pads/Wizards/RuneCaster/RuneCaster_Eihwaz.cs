using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for Rune of Repulsion's spheres, which strike the enemies they
	/// fly through, up to the skill's AoE Attack Ratio, and push away the
	/// ones close to the Rune Caster.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.RuneCaster_Eihwaz_Pad)]
	public class RuneCaster_EihwazOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		private const float RepulsionRange = 60f;
		private const int RepulsionVelocity = 150;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(5);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.Trigger.LifeTime = LifeTime;
			pad.Trigger.MaxUseCount = Math.Max(1, (int)pad.Skill.Properties.GetFloat(PropertyName.SkillSR));
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var target = args.Initiator;
			var skill = pad.Skill;

			if (caster.IsDead || target.IsDead || !caster.IsEnemy(target))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);

			if (skillHitResult.Damage > 0 && target.IsKnockdownable() && target.Position.InRange2D(caster.Position, RepulsionRange))
			{
				skillHit.KnockBackInfo = new KnockBackInfo(caster, target, KnockBackType.KnockBack, RepulsionVelocity, 10, KnockDirection.TowardsTarget);
				skillHit.HitInfo.KnockBackType = KnockBackType.KnockBack;
				target.ApplyKnockback(caster, skill, skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, skillHit);

			pad.Trigger.IncreaseUseCount();
		}
	}
}
