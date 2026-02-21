using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Elementalist: Resistance ability.
	/// Increases Fire, Ice, and Lightning resistance by 10 per ability level.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Elementalist6)]
	public class Elementalist6Override : AbilityPropertyHandler
	{
		/// <summary>
		/// Applies elemental resistance bonuses when ability is activated.
		/// </summary>
		public override void OnActivate(Ability ability, Character character)
		{
			var bonus = 10 * ability.Level;

			AddPropertyModifier(ability, character, PropertyName.ResFire_BM, bonus);
			AddPropertyModifier(ability, character, PropertyName.ResIce_BM, bonus);
			AddPropertyModifier(ability, character, PropertyName.ResLightning_BM, bonus);
		}

		/// <summary>
		/// Removes elemental resistance bonuses when ability is deactivated.
		/// </summary>
		public override void OnDeactivate(Ability ability, Character character)
		{
			RemovePropertyModifier(ability, character, PropertyName.ResFire_BM);
			RemovePropertyModifier(ability, character, PropertyName.ResIce_BM);
			RemovePropertyModifier(ability, character, PropertyName.ResLightning_BM);
		}
	}
}
