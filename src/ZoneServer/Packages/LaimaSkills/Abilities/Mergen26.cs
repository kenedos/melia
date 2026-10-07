using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Zenith: Fire For Effect ability, which trades Zenith's AoE Attack
	/// Ratio for 12% + 2% per Zenith level final damage on Mergen attacks.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Mergen26)]
	public class Mergen26Override : IAbilityHandler
	{
		private const float BaseRate = 12f;
		private const float RatePerZenithLevel = 2f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Mergen26)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!MergenSkillHelper.IsAttackSkill(skill) || !attacker.IsAbilityActive(AbilityId.Mergen26))
				return;

			if (!attacker.TryGetSkill(SkillId.Mergen_Zenith, out var zenith))
				return;

			modifier.FinalDamageMultiplier += (BaseRate + zenith.Level * RatePerZenithLevel) / 100f;
		}
	}
}
