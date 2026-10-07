using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the High Scale Magnifying Glass: Focus Trim buff, which
	/// raises final damage by 3% + 1% per skill level.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.HighMagnifyingGlass_Abil_Buff)]
	public class Appraiser_HighMagnifyingGlass_Abil_BuffOverride : BuffHandler
	{
		private const float BaseRate = 3f;
		private const float RatePerLevel = 1f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.HighMagnifyingGlass_Abil_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.HighMagnifyingGlass_Abil_Buff, out var buff))
				return;

			var rate = BaseRate + buff.NumArg1 * RatePerLevel;

			if (buff.Caster is ICombatEntity caster && caster.TryGetSkill(buff.SkillId, out var buffSkill))
				rate *= 1f + ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate")(buffSkill);

			modifier.FinalDamageMultiplier += rate / 100f;
		}
	}
}
