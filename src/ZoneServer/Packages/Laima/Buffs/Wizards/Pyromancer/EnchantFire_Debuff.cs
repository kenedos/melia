using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Shared.Data.Database;

namespace Melia.Zone.Buffs.Handlers.Wizard
{
	/// <summary>
	/// Handler for the Enchant Fire debuff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EnchantFire_Debuff)]
	public class EnchantFire_DebuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
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
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Data.Attribute != AttributeType.Fire && modifier.AttackAttribute != AttributeType.Fire)
				return;

			var abilityLevel = buff.NumArg2;
			modifier.DamageMultiplier += abilityLevel * 0.1f;
		}
	}
}
