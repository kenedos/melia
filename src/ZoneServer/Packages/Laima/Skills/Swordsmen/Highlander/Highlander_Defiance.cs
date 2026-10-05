using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsman.Highlander
{
	/// <summary>
	/// Handler for the passive Highlander skill Defiance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Highlander_Defiance)]
	public class Highlander_DefianceOverride : ISkillHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, SkillId.Highlander_Defiance)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetSkill(SkillId.Highlander_Defiance, out var defianceSkill))
				return;

			var hp = ((float)target.Hp / target.MaxHp);
			var hpLost = 1f - hp;

			var baseValue = 0.06f;

			var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
			var byAttribute = SCR_Get_AbilityReinforceRate(defianceSkill);

			var reductionPerSkilllevel = baseValue + (baseValue * byAttribute);

			// 90% Damage reduction hard cap
			var reduction = Math.Min(0.9f, reductionPerSkilllevel * defianceSkill.Level * hpLost);

			skillHitResult.Damage -= skillHitResult.Damage * reduction;
		}
	}
}
