using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsman.Highlander
{
	/// <summary>
	/// Handler for the passive Highlander skill Defiance.
	/// </summary>
	/// 
	[Package("laima")]
	[SkillHandler(SkillId.Highlander_Defiance)]
	public class Highlander_DefianceOverride : ISkillHandler, ISkillCombatDefenseAfterCalcHandler
	{
		/// <summary>
		/// Applies the skill's effect after combat calculations.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="attackerSkill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnDefenseAfterCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var hp = ((float)target.Hp / target.MaxHp);
			var hpLost = 1f - hp;

			var baseValue = 0.06f;

			var byAttribute = 0f;
			if (target.TryGetActiveAbilityLevel(AbilityId.Highlander44, out var abilityLevel))
				byAttribute = abilityLevel * 0.005f;

			var reductionPerSkilllevel = baseValue + (baseValue * byAttribute);

			// 90% Damage reduction hard cap
			var reduction = Math.Min(0.9f, reductionPerSkilllevel * skill.Level * hpLost);

			skillHitResult.Damage -= skillHitResult.Damage * reduction;
		}
	}
}
