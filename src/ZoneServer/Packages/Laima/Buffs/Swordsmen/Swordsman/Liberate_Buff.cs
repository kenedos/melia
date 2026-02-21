using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
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
	public class Liberate_BuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler, IBuffCombatAttackBeforeCalcHandler
	{
		/// <summary>
		/// Applies the buff's effect during the combat calculations.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (buff.NumArg2 != (int)AbilityId.Swordman32)
				return;

			var multiplier = 0.5f;
			modifier.DamageMultiplier -= multiplier;
		}

		/// <summary>
		/// Applies the buff's effect during the combat calculations.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (buff.NumArg2 != (int)AbilityId.Swordman31)
				return;

			var multiplier = 0.5f;
			modifier.DamageMultiplier += multiplier;
		}
	}
}
