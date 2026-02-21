using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsmen.Hoplite
{
	/// <summary>
	/// Handler override for the Spear Lunge debuff.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.SpearLunge_Debuff)]
	public class SpearLunge_DebuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		/// <summary>
		/// Applies the debuff's effect during the combat calculations.
		/// Increases damage taken from pierce-damage type weapons by 30% + 2% * SkillLv
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Check if the incoming attack is of the 'Aries' type (pierce/thrust damage).
			if (skill.Data.AttackType == SkillAttackType.Aries)
			{
				modifier.DamageMultiplier += 0.30f + (buff.NumArg1 * 0.02f);
			}
		}
	}
}
