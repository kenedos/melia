using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Lethargy ability buff that applies lethargy to monsters
	/// with a given chance when attacking.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Lethargy_Abil_Buff)]
	public class Lethargy_Abil_BuffOverride : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		private const float ChancePerAbilityLevel = 1;

		/// <summary>
		/// Applies the buff's effects during the combat calculations.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage > 0)
			{
				var applyDebuffChance = ChancePerAbilityLevel * buff.NumArg2;
				var rng = RandomProvider.Get().Next(100);

				var enhanceAbilityLevel = 0;
				if (attacker.TryGetAbility(AbilityId.Wizard27, out var enhanceAbility))
					enhanceAbilityLevel = enhanceAbility.Level;

				if (rng < applyDebuffChance)
					target.StartBuff(BuffId.Lethargy_Debuff, skill.Level, enhanceAbilityLevel, TimeSpan.FromSeconds(8), attacker);
			}
		}
	}
}
