using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Sniper Exposed buff, which weakens Snipe for every
	/// stack the caster has built up. One stack is lost every update.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Musketeer_Snipe_UseStack_Buff)]
	public class Musketeer_Snipe_UseStack_BuffOverride : BuffHandler
	{
		private const float PenaltyPerStack = 0.10f;
		private const float MinPenaltyRate = 0.1f;

		public override void WhileActive(Buff buff)
		{
			buff.OverbuffCounter--;

			if (buff.OverbuffCounter <= 0)
			{
				buff.Target.StopBuff(BuffId.Musketeer_Snipe_UseStack_Buff);
				return;
			}

			buff.NotifyUpdate();
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Musketeer_Snipe_UseStack_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id != SkillId.Musketeer_Snipe)
				return;

			if (!attacker.TryGetBuff(BuffId.Musketeer_Snipe_UseStack_Buff, out var buff))
				return;

			var rate = Math.Max(MinPenaltyRate, 1f - PenaltyPerStack * buff.OverbuffCounter);

			skillHitResult.Damage *= rate;
			modifier.HitRateMultiplier *= rate;
		}
	}
}
