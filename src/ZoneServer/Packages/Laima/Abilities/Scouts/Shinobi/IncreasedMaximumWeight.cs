using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Scouts.Shinobi
{
	/// <summary>
	/// Increased Maximum Weight.
	///
	/// Effect:
	/// - Increases maximum carrying weight by 20 per ability level.
	/// - Maximum level: 10.
	/// - Maximum bonus: +200.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.MaxWeightAbil)]
	public class IncreasedMaximumWeightAbility : AbilityPropertyHandler
	{
		private const float WeightBonusPerLevel = 20f;
		private const int MaxAbilityLevel = 10;

		public override void OnActivate(Ability ability, Character character)
		{
			if (!ability.Active)
				return;

			var level = Math.Clamp(ability.Level, 1, MaxAbilityLevel);
			var weightBonus = level * WeightBonusPerLevel;

			character.Properties.Modify(PropertyName.MaxWeight_BM, weightBonus);
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
			var level = Math.Clamp(ability.Level, 1, MaxAbilityLevel);
			var weightBonus = level * WeightBonusPerLevel;

			character.Properties.Modify(PropertyName.MaxWeight_BM, -weightBonus);
		}
	}
}
