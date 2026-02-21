using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsman.Barbarian
{
	/// <summary>
	/// Handle for the Cleave_Debuff debuff, which increases damage taken
	/// from Slash attacks.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Cleave_Debuff)]
	public class Cleave_DebuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		/// <summary>
		/// Applies the debuff's effect during the combat calculations.
		/// </summary>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Check if the incoming attack is of the 'Slash' type.
			if (skill.Data.AttackType == SkillAttackType.Slash)
			{
				// Increase damage taken from slash type damage by 30% + 2% * SkillLv.
				modifier.DamageMultiplier += 0.30f + (buff.NumArg1 * 0.02f);
			}
		}
	}
}
