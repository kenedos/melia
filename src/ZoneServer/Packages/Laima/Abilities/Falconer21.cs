using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Falconer: Hawk Eye ability.
	/// Raises the minimum critical chance of non-basic Falconer skills
	/// by 3% per ability level.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Falconer21)]
	public class Falconer21Override : IAbilityHandler
	{
		private const float MinCritPerLevel = 3f;

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, AbilityId.Falconer21)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill?.Data == null)
				return;

			if (skill.Id == SkillId.Normal_Attack)
				return;

			var className = skill.Data.ClassName;
			if (string.IsNullOrEmpty(className) || !className.StartsWith("Falconer_"))
				return;

			if (!attacker.TryGetActiveAbilityLevel(AbilityId.Falconer21, out var level))
				return;

			modifier.MinCritChance += MinCritPerLevel * level;
		}
	}
}
