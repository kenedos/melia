using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Wizards.Sage
{
    /// <summary>
    /// Sage11 - Micro Dimension: After Effects.
    /// Causes Micro Dimension to deal damage a second time
    /// to enemies at the original cast location.
    /// </summary>
    [Package("laima")]
    [AbilityHandler(AbilityId.Sage11)]
    public class Sage_MicroDimensionAfterEffectsAbility : AbilityPropertyHandler
    {
        public override void OnActivate(Ability ability, Character character)
        {
        }

        public override void OnDeactivate(Ability ability, Character character)
        {
        }
    }
}
