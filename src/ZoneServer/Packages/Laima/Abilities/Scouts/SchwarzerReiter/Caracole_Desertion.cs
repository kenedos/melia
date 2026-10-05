using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Marker handler for Caracole: Desertion.
	/// The base Caracole handler now applies its debuffs.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter16)]
	public class SchwarzerReiter_Caracole_DesertionAbility : IAbilityHandler
	{
	}
}
