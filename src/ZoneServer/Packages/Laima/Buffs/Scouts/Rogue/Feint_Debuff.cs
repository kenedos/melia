using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Feint debuff.
	/// Increases critical chance and minimum critical rate of attackers
	/// against the debuffed target.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Feint_Debuff)]
	public class Feint_DebuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Feint_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Feint_Debuff, out var buff))
				return;

			var skillLevel = (int)buff.NumArg1;

			var byAbility = 1f;
			if (buff.Caster is ICombatEntity caster && caster.TryGetSkill(buff.SkillId, out var buffSkill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				byAbility += SCR_Get_AbilityReinforceRate(buffSkill);
			}

			modifier.BonusCritChance += (150 + (skillLevel * 15)) * byAbility;
			modifier.MinCritChance += (0.30f + (skillLevel * 0.03f)) * byAbility;
		}
	}
}
