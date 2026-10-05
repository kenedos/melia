using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Wizards.Sage
{
    /// <summary>
    /// Sage14 - Blink: Looming.
    /// Allows Blink to teleport nearby party members together with the caster.
    /// The teleport behavior itself is handled by Sage_BlinkOverride.
    /// </summary>
    [Package("laima")]
    [AbilityHandler(AbilityId.Sage14)]
    public class Sage_BlinkLoomingAbility : AbilityPropertyHandler
    {
        public override void OnActivate(Ability ability, Character character)
        {
        }

        public override void OnDeactivate(Ability ability, Character character)
        {
        }
    }
}
