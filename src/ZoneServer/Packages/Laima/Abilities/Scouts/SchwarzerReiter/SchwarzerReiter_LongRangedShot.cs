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
	/// Ability handler for Schwarzer Reiter: Long-ranged Shot.
	/// Increases damage dealt by Schwarzer Reiter pistol skills when attacking from long range.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter31)]
	public class SchwarzerReiter_LongRangedShotAbility : IAbilityHandler
	{
		private const float MinimumDistance = 120f;

		/// <summary>
		/// Applies the Long-ranged Shot damage bonus after the base damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter31)]
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

			// Ignore invalid targets.
			if (target == null)
				return;

			// This ability should only affect Schwarzer Reiter pistol skills.
			if (!this.IsSchwarzerReiterPistolSkill(skill))
				return;

			// Only apply the bonus when the attacker is far enough from the target.
			var distance = attacker.Position.Get2DDistance(target.Position);
			if (distance < MinimumDistance)
				return;

			// Apply damage bonus based on ability level.
			// Adjust the ratio if the original data uses another value.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter31);
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
