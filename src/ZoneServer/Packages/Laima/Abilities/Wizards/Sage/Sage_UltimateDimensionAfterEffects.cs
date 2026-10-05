using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Wizards.Sage
{
    /// <summary>
    /// Sage12 - Ultimate Dimension: After Effects.
    /// The Ultimate Dimension skill handles the additional effect while this ability is active.
    /// </summary>
    [Package("laima")]
    [AbilityHandler(AbilityId.Sage12)]
    public class Sage_UltimateDimensionAfterEffectsAbility : AbilityPropertyHandler
    {
        public override void OnActivate(Ability ability, Character character)
        {
        }

        public override void OnDeactivate(Ability ability, Character character)
        {
        }
    }
}
