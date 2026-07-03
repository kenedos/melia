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
	/// Ability handler for Marksmanship: Additional Damage.
	/// Adds extra damage to Schwarzer Reiter pistol skills.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter35)]
	public class SchwarzerReiter_Marksmanship_AdditionalDamageAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies additional damage after the base damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter35)]
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

			// This ability should only affect Schwarzer Reiter pistol skills.
			if (!this.IsSchwarzerReiterPistolSkill(skill))
				return;

			// Apply flat additional damage based on ability level.
			// Adjust this formula if the original data uses another value.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter35);
			var additionalDamage = abilityLevel * 100;

			skillHitResult.Damage += additionalDamage;
		}

		/// <summary>
		/// Returns whether the skill belongs to the Schwarzer Reiter pistol skill set.
		/// </summary>
		private bool IsSchwarzerReiterPistolSkill(Skill skill)
		{
			return
				skill.Id == SkillId.Pistol_Attack2 ||
				skill.Id == SkillId.DoubleBullet_Attack ||
				skill.Id == SkillId.Schwarzereiter_ConcentratedFire ||
				skill.Id == SkillId.Schwarzereiter_Caracole ||
				skill.Id == SkillId.Schwarzereiter_RetreatShot ||
				skill.Id == SkillId.Schwarzereiter_AssaultFire;
		}
	}
}
