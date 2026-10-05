using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Logging;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsmen.Swordsman
{
	/// <summary>
	/// Handler for the Liberate buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Liberate_Buff)]
	public class Liberate_BuffOverride : BuffHandler
	{
		/// <summary>
		/// Applies the buff's effect during the combat calculations.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Liberate_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Liberate_Buff, out var buff))
				return;

			if (buff.NumArg2 != (int)AbilityId.Swordman32)
				return;

			var multiplier = 0.5f;
			modifier.DamageMultiplier *= (1f - multiplier);
		}

		/// <summary>
		/// Applies the buff's effect during the combat calculations.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Liberate_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Liberate_Buff, out var buff))
				return;

			if (buff.NumArg2 != (int)AbilityId.Swordman31)
				return;

			var multiplier = 0.5f;
			modifier.DamageMultiplier += multiplier;
		}
	}
}
