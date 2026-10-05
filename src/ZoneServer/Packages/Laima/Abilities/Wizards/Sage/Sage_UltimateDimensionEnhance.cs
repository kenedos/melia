using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Wizards.Sage
{
    /// <summary>
    /// Sage3 - Ultimate Dimension: Enhance.
    /// Increases Ultimate Dimension damage by 0.5% per level.
    /// At level 100, adds another 10%, totaling 60%.
    /// </summary>
    [Package("laima")]
    [AbilityHandler(AbilityId.Sage3)]
    public class Sage_UltimateDimensionEnhanceAbility : AbilityPropertyHandler
    {
        private const int MaximumLevel = 100;

        public override void OnActivate(Ability ability, Character character)
        {
        }

        public override void OnDeactivate(Ability ability, Character character)
        {
        }

        public static float GetDamageMultiplier(Character character)
        {
            var level = Math.Min(character.Abilities.GetLevel(AbilityId.Sage3), MaximumLevel);

            if (level <= 0)
                return 1f;

            var enhancePercent = level * 0.5f;

            if (level >= MaximumLevel)
                enhancePercent += 10f;

            return 1f + enhancePercent / 100f;
        }
    }
}
