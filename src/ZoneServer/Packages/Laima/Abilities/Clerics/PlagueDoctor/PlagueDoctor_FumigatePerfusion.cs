using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Clerics.PlagueDoctor
{
	/// <summary>
	/// Fumigate: Perfusion.
	/// Marker ability whose behavior is handled by PlagueDoctor_Fumigate.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.PlagueDoctor29)]
	public class PlagueDoctor_FumigatePerfusionAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
