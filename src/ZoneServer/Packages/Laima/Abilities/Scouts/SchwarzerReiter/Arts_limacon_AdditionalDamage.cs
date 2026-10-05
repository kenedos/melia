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
	/// Ability handler for [Arts] Limacon: Additional Damage.
	/// Adds extra damage to Limacon's main attack while Limacon is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter32)]
	public class SchwarzerReiter_Limacon_AdditionalDamageAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies additional damage after Limacon damage is calculated.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter32)]
		public void OnAttackAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// This arts only affects Limacon's main attack.
			if (skill.Id != SkillId.Pistol_Attack2)
				return;

			// The arts only works while Limacon is active.
			if (!attacker.TryGetBuff(BuffId.Limacon_Buff, out _))
				return;

			// Only characters can own abilities.
			if (attacker is not Character character)
				return;

			// Get the learned ability level.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter32);

			if (abilityLevel <= 0)
				return;

			// Apply flat additional damage.
			// Adjust this formula if the original data uses another value.
			var additionalDamage = abilityLevel * 100;

			skillHitResult.Damage += additionalDamage;
		}
	}
}
