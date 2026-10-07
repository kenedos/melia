using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for Wiegenlied's lullaby, which puts the target to sleep.
	/// The first attack against it can't miss, can't be blocked, is
	/// critical and wakes it up.
	/// </summary>
	/// <remarks>
	/// Waking up, or the lullaby running out, leaves the target drowsy.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Lullaby_Debuff)]
	public class PiedPiper_Lullaby_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Sleep);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			target.RemoveState(StateType.Sleep);

			if (target.IsDead)
				return;

			var drowsyDuration = TimeSpan.FromSeconds(GetCaptionRatio(buff, 1));
			target.StartBuff(BuffId.Wiegenlied_Debuff, buff.NumArg1, 0, drowsyDuration, buff.Caster, buff.SkillId);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Lullaby_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.Lullaby_Debuff))
				return;

			modifier.ForcedHit = true;
			modifier.Unblockable = true;
			modifier.ForcedCritical = true;
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Lullaby_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			target.StopBuff(BuffId.Lullaby_Debuff);
		}
	}

	/// <summary>
	/// Handler for Wiegenlied's drowsiness, which lowers accuracy.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Wiegenlied_Debuff)]
	public class PiedPiper_Wiegenlied_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM, -GetCaptionRatio(buff, 2) / 100f);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM);
		}
	}
}
