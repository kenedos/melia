using Melia.Shared.Game.Const;
using Melia.Shared.Packages;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Companion Mastery: Fang ability.
	/// Increases companion's ATK by 25% while reducing CON by 25%.
	/// Stat effects are applied in calc_companion.cs based on the owner's
	/// active ability; companion property refresh on toggle is handled
	/// centrally via the AbilityComponent.AbilityActivated/Deactivated events
	/// subscribed by CharacterProperties.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.CompMastery5)]
	public class CompMastery5Override : IAbilityHandler
	{
	}
}
