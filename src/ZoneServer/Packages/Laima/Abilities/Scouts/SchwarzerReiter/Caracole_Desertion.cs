using System;
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
	/// Ability handler for Caracole: Desertion.
	/// Reduces the target's defense after being hit by Caracole.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter16)]
	public class SchwarzerReiter_Caracole_DesertionAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies the Desertion effect after Caracole damage is calculated.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter16)]
		public void OnAttackAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// This ability only affects Caracole.
			if (skill.Id != SkillId.Schwarzereiter_Caracole)
				return;

			// Only characters can own abilities.
			if (attacker is not Character character)
				return;

			// Verify that the ability is learned.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter16);

			if (abilityLevel <= 0)
				return;

			// Ignore invalid targets.
			if (target == null || target.IsDead)
				return;

			// Apply the Caracole Desertion debuff.
			// The buff handler contains the defense reduction logic.
			target.StartBuff(
				BuffId.Caracole_Silence_Debuff,
				abilityLevel,
				0,
				TimeSpan.FromSeconds(10),
				character,
				skill.Id);
		}
	}
}
