using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Inquisitor: Darkness Resistance ability, which raises Dark
	/// resistance by 10 per ability level.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Inquisitor9)]
	public class Inquisitor9Override : AbilityPropertyHandler
	{
		private const float ResistancePerLevel = 10;

		/// <summary>
		/// Applies the resistance bonus when the ability is activated.
		/// </summary>
		public override void OnActivate(Ability ability, Character character)
		{
			AddPropertyModifier(ability, character, PropertyName.ResDark_BM, ResistancePerLevel * ability.Level);
		}

		/// <summary>
		/// Removes the resistance bonus when the ability is deactivated.
		/// </summary>
		public override void OnDeactivate(Ability ability, Character character)
		{
			RemovePropertyModifier(ability, character, PropertyName.ResDark_BM);
		}
	}
}
