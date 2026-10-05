using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Evasive Action buff.
	/// Adds evasion based on the character's current evasion when the buff is activated.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EvasiveAction_Buff)]
	public class SchwarzerReiter_EvasiveAction_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is Character character)
			{
				var evasionBonus = this.GetEvasionBonus(buff, character);

				// Store the exact applied bonus so OnEnd removes the same value.
				buff.NumArg2 = evasionBonus;

				character.Properties.Modify(PropertyName.DR_BM, evasionBonus);

				// Reapply Evasive Action: Enhance after the Evasive Action buff becomes active.
				this.RefreshEvasiveActionEnhance(character, true);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.DR,
					PropertyName.DR_BM);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is Character character)
			{
				// Remove Evasive Action: Enhance before removing the base Evasive Action bonus.
				this.RefreshEvasiveActionEnhance(character, false);

				// Remove the exact value applied during OnActivate.
				var evasionBonus = buff.NumArg2;
				character.Properties.Modify(PropertyName.DR_BM, -evasionBonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.DR,
					PropertyName.DR_BM);
			}
		}

		private float GetEvasionBonus(Buff buff, Character character)
		{
			var skillLevel = buff.NumArg1;

			// Use the character's current evasion at activation time.
			var currentEvasion = character.Properties.GetFloat(PropertyName.DR);

			// Adds 2% of current evasion per skill level.
			return currentEvasion * (0.02f * skillLevel);
		}

		/// <summary>
		/// Refreshes the Evasive Action: Enhance property handler when the Evasive Action buff state changes.
		/// </summary>
		private void RefreshEvasiveActionEnhance(Character character, bool shouldActivate)
		{
			if (!character.Abilities.TryGet(AbilityId.Schwarzereiter33, out var ability))
				return;

			ZoneServer.Instance.AbilityHandlers.DeactivatePropertyHandler(ability, character);

			if (shouldActivate)
				ZoneServer.Instance.AbilityHandlers.ActivatePropertyHandler(ability, character);
		}
	}
}
