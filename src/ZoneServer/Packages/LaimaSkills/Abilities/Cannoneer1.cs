using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Cannon Mastery: Penetration ability, which gives Cannoneer skills a
	/// 3% chance per ability level to ignore the target's block.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Cannoneer1)]
	public class Cannoneer1Override : IAbilityHandler
	{
		private const int ChancePerLevel = 3;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Cannoneer1)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!skill.Data.ClassName.StartsWith("Cannoneer_", StringComparison.Ordinal))
				return;

			if (!attacker.TryGetActiveAbilityLevel(AbilityId.Cannoneer1, out var level))
				return;

			if (GameRandom.Get().Next(100) < level * ChancePerLevel)
				modifier.Unblockable = true;
		}
	}
}
