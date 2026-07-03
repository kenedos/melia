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
	/// Ability handler for [Arts] Evasive Action: Cobra.
	/// Adds extra damage to Schwarzer Reiter pistol attacks while Evasive Action is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter20)]
	public class SchwarzerReiter_EvasiveAction_CobraAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies Cobra bonus damage after the base damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter20)]
		public void OnAttackAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// Only characters can own abilities.
			if (attacker is not Character character)
				return;

			// Cobra only works while Evasive Action is active.
			if (!character.TryGetBuff(BuffId.EvasiveAction_Buff, out _))
				return;

			// Cobra should only affect Schwarzer Reiter pistol attacks.
			if (!this.IsSchwarzerReiterPistolSkill(skill))
				return;

			// Calculate bonus damage based on the character's current evasion.
			// Adjust this formula if the original data uses another value.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter20);
			var currentEvasion = character.Properties.GetFloat(PropertyName.DR);
			var additionalDamage = currentEvasion * (0.01f * abilityLevel);

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
