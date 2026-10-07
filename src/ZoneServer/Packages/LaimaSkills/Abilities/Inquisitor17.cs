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
	/// Inquisitor: Torture Expert ability, which shortens the torture
	/// skills' cooldowns when one of them kills an enemy.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Inquisitor17)]
	public class Inquisitor17Override : IAbilityHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Inquisitor17)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character || !character.IsAbilityActive(AbilityId.Inquisitor17))
				return;

			if (skillHitResult.Damage < target.Hp)
				return;

			InquisitorSkillHelper.TryTortureExpert(character, skill);
		}
	}
}
