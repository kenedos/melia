using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
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
	public class Lethargy_Abil_BuffOverride : BuffHandler
	{
		private const float ChancePerAbilityLevel = 1;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Lethargy_Abil_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Lethargy_Abil_Buff, out var buff))
				return;

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
