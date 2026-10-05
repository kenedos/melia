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
	/// Ability handler for Serial Bullet: Enhance.
	/// Increases Serial Bullet damage while the ability is learned/active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter28)]
	public class SchwarzerReiter_SerialBullet_EnhanceAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies the Serial Bullet: Enhance damage bonus after the base damage calculation.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter28)]
		public void OnAttackAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// Serial Bullet damage is handled through DoubleBullet_Attack.
			if (skill.Id != SkillId.DoubleBullet_Attack)
				return;

			// Only apply the bonus while Serial Bullet is active.
			if (!attacker.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out _))
				return;

			// Only characters have abilities.
			if (attacker is not Character character)
				return;

			// The standard Enhance pattern is +0.5% damage per ability level.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter28);
			var damageMultiplier = 1f + (0.005f * abilityLevel);

			skillHitResult.Damage *= damageMultiplier;
		}
	}
}
