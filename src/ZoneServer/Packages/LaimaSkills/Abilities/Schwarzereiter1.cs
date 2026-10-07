using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Gun: Accuracy ability, which raises the accuracy of gun attacks by
	/// 5% per ability level.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Schwarzereiter1)]
	public class Schwarzereiter1Override : IAbilityHandler
	{
		private const float AccuracyPerLevel = 0.05f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Schwarzereiter1)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Data.AttackType != SkillAttackType.Gun)
				return;

			if (!attacker.TryGetActiveAbilityLevel(AbilityId.Schwarzereiter1, out var level))
				return;

			modifier.HitRateMultiplier += AccuracyPerLevel * level;
		}
	}
}
