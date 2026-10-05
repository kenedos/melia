using Melia.Shared.Game.Const;
using Melia.Shared.Packages;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Companion Mastery: Fortitude ability.
	/// Increases companion's DEF and CON by 25% while reducing ATK by 25%.
	/// Stat effects are applied in calc_companion.cs based on the owner's
	/// active ability; companion property refresh on toggle is handled
	/// centrally via the AbilityComponent.AbilityActivated/Deactivated events
	/// subscribed by CharacterProperties.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.CompMastery4)]
	public class CompMastery4Override : IAbilityHandler
	{
	}
}
