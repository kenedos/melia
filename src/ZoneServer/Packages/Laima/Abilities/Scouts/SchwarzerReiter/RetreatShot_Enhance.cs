using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability handler for Retreat Shot: Enhance.
	/// Increases Retreat Shot damage while the ability is learned/active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter14)]
	public class SchwarzerReiter_RetreatShot_EnhanceAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies the Retreat Shot: Enhance damage bonus after the base damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter14)]
		public void OnAttackAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// This ability should only affect Retreat Shot.
			if (skill.Id != SkillId.Schwarzereiter_RetreatShot)
				return;

			// Only characters have abilities.
			if (attacker is not Character character)
				return;

			// The standard Enhance pattern is +0.5% damage per ability level.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter14);
			var damageMultiplier = 1f + (0.005f * abilityLevel);

			skillHitResult.Damage *= damageMultiplier;
		}
	}
}
