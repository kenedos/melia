using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Hunter
{
	/// <summary>
	/// Handler for the Praise Attack Buff, which dramatically increases
	/// the companion's attack power and applies bleeding on attacks.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: None
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Praise_Atk_Buff)]
	public class Praise_Atk_BuffOverride : BuffHandler
	{
		private const int BleedingDurationSeconds = 8;
		private const int BleedingTickCount = 8;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			AddPropertyModifier(buff, target, PropertyName.ATK_BM, GetCaptionRatio(buff, 1));

			var atkRate = GetCaptionRatio(buff, 2) / 100f;
			var currentAtk = target.Properties.GetFloat(PropertyName.ATK);
			var atkRateBonus = currentAtk * atkRate;
			AddPropertyModifier(buff, target, PropertyName.ATK_BM, atkRateBonus);

			var srBonus = GetCaptionRatio(buff, 3);
			AddPropertyModifier(buff, target, PropertyName.SR_BM, srBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.ATK_BM);
			RemovePropertyModifier(buff, target, PropertyName.SR_BM);
		}

		/// <summary>
		/// Applies bleeding to targets when the companion attacks.
		/// Bleeding deals 100% of attack damage over 8 seconds.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Praise_Atk_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Praise_Atk_Buff, out var buff))
				return;

			if (skillHitResult.Damage <= 0)
				return;


			// Apply bleeding: 100% of attack damage spread over 8 ticks
			var bleedingDamagePerTick = skillHitResult.Damage / BleedingTickCount;
			target.StartBuff(BuffId.HeavyBleeding, buff.NumArg1, bleedingDamagePerTick, TimeSpan.FromSeconds(BleedingDurationSeconds), attacker);
		}
	}
}
