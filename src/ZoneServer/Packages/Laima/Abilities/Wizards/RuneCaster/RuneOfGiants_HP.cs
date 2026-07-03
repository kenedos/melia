using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster4 - Rune of Giants: HP.
	///
	/// Effect:
	/// - Increases the HP bonus from Rune of Protection: Giant.
	/// - The real effect is applied in RuneOfProtection_Giant_Buff.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster4)]
	public class RuneCaster_RuneOfGiantsHpAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
