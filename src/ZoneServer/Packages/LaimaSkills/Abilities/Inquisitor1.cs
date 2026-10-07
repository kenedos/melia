using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Inquisitor: Burn ability, which may ignite a flame where an enemy
	/// the Inquisitor killed fell.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Inquisitor1)]
	public class Inquisitor1Override : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Inquisitor1)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character || !character.IsAbilityActive(AbilityId.Inquisitor1))
				return;

			if (skillHitResult.Damage < target.Hp)
				return;

			InquisitorSkillHelper.TryBurn(character, target, skill);
		}
	}
}
