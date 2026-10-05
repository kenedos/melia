using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability handler for Schwarzer Reiter: Special Steering.
	/// Increases movement speed while the character is mounted.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter26)]
	public class SchwarzerReiter_SpecialSteeringAbility : AbilityPropertyHandler
	{
		/// <summary>
		/// Applies the movement speed bonus when the ability is activated.
		/// </summary>
		public override void OnActivate(Ability ability, Character character)
		{
			// Special Steering should only apply while the character is mounted.
			if (!character.IsRiding)
				return;

			// Apply movement speed bonus based on ability level.
			// Adjust this value if the original data uses a different ratio.
			var moveSpeedBonus = ability.Level * 1f;

			AddPropertyModifier(
				ability,
				character,
				PropertyName.MSPD_BM,
				moveSpeedBonus);
		}

		/// <summary>
		/// Removes the movement speed bonus when the ability is deactivated.
		/// </summary>
		public override void OnDeactivate(Ability ability, Character character)
		{
			// Remove the exact value previously applied by this ability.
			RemovePropertyModifier(
				ability,
				character,
				PropertyName.MSPD_BM);
		}
	}
}
