using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Hoplite
{
	/// <summary>
	/// Handler override for the Hoplite passive skill Sharp Spear.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hoplite_SharpSpear)]
	public class Hoplite_SharpSpearOverride : ISkillHandler, ISkillCombatAttackBeforeCalcHandler
	{
		/// <summary>
		/// Applies the skill's effect during combat calculations.
		/// Increases crit rate for all pierce (Aries) type damage skills.
		/// </summary>
		/// <param name="skill">The Sharp Spear passive skill</param>
		/// <param name="attacker">The entity with Sharp Spear active</param>
		/// <param name="target">The target being attacked</param>
		/// <param name="attackerSkill">The skill being used to attack</param>
		/// <param name="modifier">The skill modifier to apply crit rate bonus to</param>
		/// <param name="skillHitResult">The hit result</param>
		public void OnAttackBeforeCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Check if the attacking skill is pierce type (Aries)
			var isPierceType = attackerSkill.Data.AttackType == SkillAttackType.Aries;

			if (isPierceType)
			{
				// Base 20% + 2% per skill level
				var critRateBonus = 0.20f + (skill.Level * 0.02f);

				// Hoplite31: Sharp Spear: Enhance - +0.5% per ability level
				if (attacker.TryGetActiveAbilityLevel(AbilityId.Hoplite31, out var abilityLevel))
					critRateBonus *= 1f + abilityLevel * 0.005f;

				modifier.CritRateMultiplier += critRateBonus;
			}
		}
	}
}
