using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Elementalist: Elemental Exploitation ability.
	/// Increases final damage by 0.5% per ability level for each
	/// different elemental condition (Fire, Freeze, Shock) on the target.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Elementalist39)]
	public class Elementalist39Override : IAbilityHandler, IAbilityCombatAttackAfterCalcHandler
	{
		private const float DamagePerLevelPerCondition = 0.005f;

		public void OnAttackAfterCalc(Ability ability, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var conditionCount = 0;

			if (target.IsBuffActiveByKeyword(BuffTag.Fire))
				conditionCount++;

			if (target.IsBuffActiveByKeyword(BuffTag.Freeze))
				conditionCount++;

			if (target.IsBuffActiveByKeyword(BuffTag.Shock))
				conditionCount++;

			if (conditionCount == 0)
				return;

			var bonus = 1f + DamagePerLevelPerCondition * ability.Level * conditionCount;
			skillHitResult.Damage *= bonus;
		}
	}
}
