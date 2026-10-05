using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Reduces the target's Accuracy while Caracole HR debuff is active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Caracole_HR_Debuff)]
	public class SchwarzerReiter_CaracoleAccuracyDebuffOverride : BuffHandler, IBuffCombatAttackBeforeCalcHandler
	{
		private const float DefaultAccuracyReductionRate = 0.25f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker != buff.Target)
				return;

			var reductionRate = buff.NumArg2 > 0f ? buff.NumArg2 : DefaultAccuracyReductionRate;
			reductionRate = Math.Clamp(reductionRate, 0f, 1f);

			modifier.HitRateMultiplier *= 1f - reductionRate;
		}
	}
}
