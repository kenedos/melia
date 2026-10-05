using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Onmyoji20)]
	public class Onmyoji_MediatorAbility : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Onmyoji20)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character || skill == null)
				return;
			if (!character.IsAbilityActive(AbilityId.Onmyoji20))
				return;
			if (!skill.Id.ToString().StartsWith("Onmyoji_", StringComparison.Ordinal))
				return;
			skillHitResult.Damage *= 1.10f;
		}
	}
}
