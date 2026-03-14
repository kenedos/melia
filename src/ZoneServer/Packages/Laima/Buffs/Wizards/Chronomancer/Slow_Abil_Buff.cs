using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Slow_Abil_Buff)]
	public class Slow_Abil_BuffOverride : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		private const float ChancePerAbilityLevel = 1;

		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage > 0)
			{
				var applyDebuffChance = ChancePerAbilityLevel * buff.NumArg2;
				var rng = RandomProvider.Get().Next(100);

				if (rng < applyDebuffChance)
					target.StartBuff(BuffId.Slow_Debuff, buff.NumArg1, 0, TimeSpan.FromSeconds(10 + buff.NumArg1), attacker);
			}
		}
	}
}
