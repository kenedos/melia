using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Wizards.Sage
{
    /// <summary>
    /// Sage8 - Micro Dimension: Duplicate.
    /// Enables Micro Dimension to duplicate compatible installations.
    /// The duplication itself is handled by Sage_MicroDimension.
    /// </summary>
    [Package("laima")]
    [AbilityHandler(AbilityId.Sage8)]
    public class Sage_MicroDimensionDuplicateAbility : AbilityPropertyHandler
    {
        public override void OnActivate(Ability ability, Character character)
        {
        }

        public override void OnDeactivate(Ability ability, Character character)
        {
        }
    }
}
