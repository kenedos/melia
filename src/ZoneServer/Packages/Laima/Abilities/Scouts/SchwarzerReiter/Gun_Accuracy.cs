using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability handler for Gun Accuracy.
	/// Increases the character's accuracy while the ability is active.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter1)]
	public class SchwarzerReiter_GunAccuracyAbility : AbilityPropertyHandler
	{
		/// <summary>
		/// Applies the accuracy bonus when the ability is activated.
		/// </summary>
		public override void OnActivate(Ability ability, Character character)
		{
			// Gun Accuracy increases accuracy based on ability level.
			// Adjust the multiplier if the original skill data uses a different value.
			var accuracyBonus = ability.Level * 10;

			AddPropertyModifier(
				ability,
				character,
				PropertyName.HR_BM,
				accuracyBonus);
		}

		/// <summary>
		/// Removes the accuracy bonus when the ability is deactivated.
		/// </summary>
		public override void OnDeactivate(Ability ability, Character character)
		{
			// Remove the exact value previously applied by this ability.
			RemovePropertyModifier(
				ability,
				character,
				PropertyName.HR_BM);
		}
	}
}
