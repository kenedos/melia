using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Scouts.BulletMarker
{
	/// <summary>
	/// Bulletmarker13 - R.I.P.: AoE.
	///
	/// Effect:
	/// - Increases R.I.P. AoE Attack Ratio by 5.
	/// - Base AoE Attack Ratio: 10.
	/// - With ability active: 15.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Bulletmarker13)]
	public class BulletMarker_RestInPeaceAoEAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
