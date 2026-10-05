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
	/// Ability handler for Schwarzer Reiter: Marksmanship.
	/// Increases damage dealt by Schwarzer Reiter pistol skills.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter4)]
	public class SchwarzerReiter_MarksmanshipAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies the Marksmanship damage bonus after the base damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter4)]
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

			// Apply a damage bonus based on ability level.
			// Adjust the ratio if the original data uses another value.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter4);
			var damageMultiplier = 1f + (0.005f * abilityLevel);

			skillHitResult.Damage *= damageMultiplier;
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
