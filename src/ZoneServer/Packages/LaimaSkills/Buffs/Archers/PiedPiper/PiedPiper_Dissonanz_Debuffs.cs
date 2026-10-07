using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for Dissonanz's stun, which leaves the target unable to act.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Dissonanz_Stun_Debuff)]
	public class PiedPiper_Dissonanz_Stun_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Stunned);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Stunned);
		}
	}

	/// <summary>
	/// Handler for Dissonanz's sound waves, which wound the target every
	/// second.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Dissonanz_Debuff)]
	public class PiedPiper_Dissonanz_DebuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead || buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.PiedPiper_Dissonanz, out var skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
		}
	}
}
