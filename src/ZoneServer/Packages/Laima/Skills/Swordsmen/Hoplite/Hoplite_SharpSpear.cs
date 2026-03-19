using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Hoplite
{
	/// <summary>
	/// Handler override for the Hoplite passive skill Sharp Spear.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hoplite_SharpSpear)]
	public class Hoplite_SharpSpearOverride : ISkillHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, SkillId.Hoplite_SharpSpear)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			// Check if the attacking skill is pierce type (Aries)
			var isPierceType = skill.Data.AttackType == SkillAttackType.Aries;

			if (isPierceType)
			{
				if (!attacker.TryGetSkill(SkillId.Hoplite_SharpSpear, out var sharpSpearSkill))
					return;

				// Base 20% + 2% per skill level
				var critRateBonus = 0.20f + (sharpSpearSkill.Level * 0.02f);

				// Hoplite31: Sharp Spear: Enhance - +0.5% per ability level
				if (attacker.TryGetActiveAbilityLevel(AbilityId.Hoplite31, out var abilityLevel))
					critRateBonus *= 1f + abilityLevel * 0.005f;

				modifier.CritRateMultiplier += critRateBonus;
			}
		}
	}
}
