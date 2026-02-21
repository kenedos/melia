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
	/// Handler for the Bear buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Bear_Buff)]
	public class Bear_BuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		private const float DamageReductionPerLevel = 0.03f;
		private const float AbilityBonus = 0.005f;

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
			var skillLevel = buff.NumArg1;
			var multiplierReduction = skillLevel * DamageReductionPerLevel;

			var byAbility = 0f;
			if (buff.Target.TryGetActiveAbilityLevel(AbilityId.Swordman30, out var abilityLevel))
				byAbility = abilityLevel * AbilityBonus;

			multiplierReduction *= 1f + byAbility;

			multiplierReduction = Math.Min(0.80f, multiplierReduction);

			// We originally reduced the damage directly from inside the combat
			// calculations, on AfterBonuses, but setting the multiplier seems
			// much easier. Is this correct? Who knows.

			modifier.DamageMultiplier -= multiplierReduction;
		}
	}
}
