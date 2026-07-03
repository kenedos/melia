using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster22 - Rune of Protection: Giant.
	///
	/// Effect:
	/// - Changes Rune of Protection behavior.
	/// - Instead of the normal protection effect, applies Giant effect.
	/// - Defense +20%, CON/HP +20%, Movement Speed +20.
	/// - Duration: skill level * 60 seconds.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster22)]
	public class RuneCaster_RuneOfProtectionGiantAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
