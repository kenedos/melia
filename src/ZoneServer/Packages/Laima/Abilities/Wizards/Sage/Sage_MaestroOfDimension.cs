using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Wizards.Sage
{
	[Package("laima")]
	[AbilityHandler(AbilityId.Sage19)]
	public class Sage_MaestroOfDimensionAbility : AbilityPropertyHandler
	{
		private const float DamageMultiplier = 1.20f;

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, AbilityId.Sage19)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			var attackAttribute = modifier.AttackAttribute == AttributeType.None
				? skill.Data.Attribute
				: modifier.AttackAttribute;

			if (attackAttribute != AttributeType.None)
				return;

			modifier.DamageMultiplier *= DamageMultiplier;
		}
	}
}
