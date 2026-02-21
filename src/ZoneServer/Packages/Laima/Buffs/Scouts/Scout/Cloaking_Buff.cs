using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scout
{
	/// <summary>
	/// Handler for the Cloaking buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Cloaking_Buff)]
	public class Cloaking_BuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler, IBuffCombatDefenseAfterCalcHandler
	{
		/// <summary>
		/// Applies the buff's effects during the combat calculations.
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
			var damageReduction = 0.30f + 0.06f * skill.Level;
			var byAbility = 1f;
			if (buff.Target.TryGetActiveAbility(AbilityId.Scout18, out var ability))
				byAbility += ability.Level * 0.005f;
			damageReduction *= byAbility;

			damageReduction = (float)Math.Min(0.95, damageReduction);

			modifier.DamageMultiplier -= damageReduction;
		}

		/// <summary>
		/// Removes buff if cloaking user takes damage
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage > 0)
			{
				target.StopBuffByTag(BuffTag.Cloaking);
			}
		}
	}
}
