using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability handler for Evasive Action: Enhance.
	/// Increases the evasion bonus granted by Evasive Action.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter33)]
	public class SchwarzerReiter_EvasiveAction_EnhanceAbility : AbilityPropertyHandler
	{
		/// <summary>
		/// Applies additional evasion while Evasive Action is active.
		/// </summary>
		public override void OnActivate(Ability ability, Character character)
		{
			// This ability should only apply while Evasive Action is active.
			if (!character.TryGetBuff(BuffId.EvasiveAction_Buff, out _))
				return;

			// Evasive Action: Enhance increases evasion by 0.5% per ability level.
			var currentEvasion = character.Properties.GetFloat(PropertyName.DR);
			var evasionBonus = currentEvasion * (0.005f * ability.Level);

			AddPropertyModifier(
				ability,
				character,
				PropertyName.DR_BM,
				evasionBonus);
		}

		/// <summary>
		/// Removes the additional evasion granted by this ability.
		/// </summary>
		public override void OnDeactivate(Ability ability, Character character)
		{
			// Remove the exact value previously applied by this ability.
			RemovePropertyModifier(
				ability,
				character,
				PropertyName.DR_BM);
		}
	}
}
