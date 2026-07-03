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
	/// Ability handler for Revolver Mastery.
	/// Increases pistol attack damage while the ability is learned/active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter2)]
	public class SchwarzerReiter_RevolverMasteryAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies Revolver Mastery damage bonus after the base damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter2)]
		public void OnAttackAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// Only characters have abilities.
			if (attacker is not Character character)
				return;

			// Revolver Mastery should only affect pistol-based attacks.
			if (!this.IsPistolSkill(skill))
				return;

			// Standard mastery pattern.
			// Adjust this value if the original data uses a different ratio.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter2);
			var damageMultiplier = 1f + (0.005f * abilityLevel);

			skillHitResult.Damage *= damageMultiplier;
		}

		/// <summary>
		/// Returns whether the skill should be treated as a pistol attack.
		/// </summary>
		private bool IsPistolSkill(Skill skill)
		{
			return
				skill.Id == SkillId.Pistol_Attack ||
				skill.Id == SkillId.Pistol_Attack2 ||
				skill.Id == SkillId.DoubleBullet_Attack ||
				skill.Id == SkillId.Schwarzereiter_ConcentratedFire ||
				skill.Id == SkillId.Schwarzereiter_Caracole ||
				skill.Id == SkillId.Schwarzereiter_RetreatShot ||
				skill.Id == SkillId.Schwarzereiter_AssaultFire;
		}
	}
}
