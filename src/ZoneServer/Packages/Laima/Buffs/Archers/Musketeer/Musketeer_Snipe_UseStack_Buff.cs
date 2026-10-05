using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handles Sniper Exposed on the player, reducing final damage and
	/// accuracy by 10% per stack. One stack expires every five seconds.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Musketeer_Snipe_UseStack_Buff)]
	public class Musketeer_Snipe_UseStack_BuffOverride : BuffHandler, IBuffCombatAttackBeforeCalcHandler
	{
		private const int MaximumStacks = 10;
		private const float ReductionPerStack = 0.10f;
		private const int StackDecayInterval = 5000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(StackDecayInterval);
			this.UpdateAccuracyPenalty(buff);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.OverbuffCounter <= 1)
			{
				buff.Target.StopBuff(BuffId.Musketeer_Snipe_UseStack_Buff);
				return;
			}

			buff.OverbuffCounter--;
			this.UpdateAccuracyPenalty(buff);
			buff.NotifyUpdate();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
		}

		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker != buff.Target)
				return;

			var stacks = Math.Clamp(buff.OverbuffCounter, 1, MaximumStacks);
			var reduction = Math.Min(1f, stacks * ReductionPerStack);
			modifier.FinalDamageMultiplier *= 1f - reduction;
		}

		private void UpdateAccuracyPenalty(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			var stacks = Math.Clamp(buff.OverbuffCounter, 1, MaximumStacks);
			var reduction = Math.Min(1f, stacks * ReductionPerStack);
			var currentAccuracy = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.HR));
			AddPropertyModifier(buff, buff.Target, PropertyName.HR_BM, -(currentAccuracy * reduction));
		}
	}
}
