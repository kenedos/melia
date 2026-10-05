using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability handler for Retreat Shot: Increased Nullification.
	/// Gives the character a chance to nullify incoming damage while Retreat Shot is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter3)]
	public class SchwarzerReiter_RetreatShot_IncreasedNullificationAbility : IAbilityHandler
	{
		/// <summary>
		/// Applies the nullification chance after incoming damage is calculated.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter3)]
		public void OnDefenseAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// Only characters can have Schwarzer Reiter abilities.
			if (target is not Character character)
				return;

			// This ability only works while Retreat Shot is active.
			if (!character.TryGetBuff(BuffId.RetreatShot, out _))
				return;

			// Get the learned ability level.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter3);

			if (abilityLevel <= 0)
				return;

			// Nullification chance.
			// Adjust this formula if the official data uses another value.
			// Example: level 1 = 2%, level 5 = 10%.
			var nullificationChance = abilityLevel * 2;

			if (RandomProvider.Get().Next(100) >= nullificationChance)
				return;

			// Nullify the incoming damage.
			skillHitResult.Damage = 0;
		}
	}
}
