using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Hanging Shot buff - character hanging from hawk companion.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.HangingShot)]
	public class Falconer_HangingShotOverride : BuffHandler
	{
		/// <summary>
		/// Called when the HangingShot buff is activated.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			// Change normal attack to hanging shot variant
			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Bow_Hanging_Attack);

			// Disable sitting while hanging from hawk
			character.SetSittingDisabled(true);

			// Force stand up if currently sitting
			if (character.IsSitting)
				character.SetSitting(false);
		}

		/// <summary>
		/// Called when the HangingShot buff ends (natural expiry or forced removal).
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			// Re-enable sitting
			character.SetSittingDisabled(false);

			// Cleanup attachment state if still attached
			// This handles cases where buff is removed by external means
			// (dispel, death, etc.) without going through skill handler cleanup
			this.CleanupAttachment(character);

			// Restore normal attack
			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
		}

		/// <summary>
		/// Cleans up attachment state and sends detachment packets.
		/// </summary>
		private void CleanupAttachment(Character character)
		{
			// Detach from attachment system
			if (character.Components.TryGet<AttachmentComponent>(out var attachment) && attachment.IsAttached)
			{
				var hawk = attachment.AttachedTo as Companion;

				attachment.Detach(sendPackets: false);

				// Reset movement mode
				if (character.Components.TryGet<MovementComponent>(out var movement))
					movement.SetAttachmentMovementMode(false);

				// Send detachment packets
				Send.ZC_NORMAL.ControlObject(character, null, ControlLookType.SameDirection, true, true, "None", false);
				Send.ZC_NORMAL.FlyWithObject(character, null);

				// Reset hawk state if available
				if (hawk != null)
				{
					Send.ZC_MOVE_ANIM(hawk, FixedAnimation.EMPTY, 0);

					hawk.Vars.Set("Hawk.UsingSkill", false);
					hawk.Vars.Set("Hawk.SkillFunction", "None");
				}
			}
		}
	}
}
