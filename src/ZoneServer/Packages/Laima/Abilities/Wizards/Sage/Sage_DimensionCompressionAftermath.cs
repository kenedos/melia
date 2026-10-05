using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Abilities.Wizards.Sage
{
	/// <summary>
	/// Sage18 - Dimension Compression: Aftermath.
	/// Applies Confusion for 5 seconds to enemies damaged by Dimension Compression.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Sage18)]
	public class Sage_DimensionCompressionAftermathAbility : AbilityPropertyHandler
	{
		private static readonly TimeSpan ConfusionDuration = TimeSpan.FromSeconds(5);

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool IsActive(Character character)
		{
			return character != null && character.IsAbilityActive(AbilityId.Sage18);
		}

		public static TimeSpan GetConfusionDuration(Character character)
		{
			return IsActive(character) ? ConfusionDuration : TimeSpan.Zero;
		}
	}
}
