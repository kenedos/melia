// ===================================================================
// CharacterActions.cs - General player actions and states
// ===================================================================
using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		/// <summary>
		/// When true, the character cannot sit down (e.g., during HangingShot).
		/// </summary>
		private bool _sittingDisabled = false;

		/// <summary>
		/// Returns whether sitting is currently disabled for this character.
		/// </summary>
		public bool IsSittingDisabled => _sittingDisabled;

		/// <summary>
		/// Toggles whether the character is sitting or not.
		/// </summary>
		public void ToggleSitting()
		{
			// Can't toggle to sitting if it's disabled
			if (!this.IsSitting && !this.CanSit())
				return;

			this.IsSitting = !this.IsSitting;
			this.SitStatusChanged?.Invoke(this);
			Send.ZC_REST_SIT(this);
		}

		/// <summary>
		/// Sets the character's sitting state.
		/// </summary>
		/// <param name="sittingState">Whether the character should be sitting.</param>
		public void SetSitting(bool sittingState)
		{
			// Can't sit if sitting is not allowed
			if (sittingState && !this.CanSit())
				return;

			this.IsSitting = sittingState;
			this.SitStatusChanged?.Invoke(this);
			Send.ZC_REST_SIT(this);
		}

		/// <summary>
		/// Enables or disables the character's ability to sit.
		/// Used by skills like HangingShot where sitting is not allowed.
		/// </summary>
		/// <param name="disabled">True to disable sitting, false to enable.</param>
		public void SetSittingDisabled(bool disabled)
		{
			_sittingDisabled = disabled;

			// If disabling sitting and currently sitting, force stand up
			if (disabled && this.IsSitting)
			{
				this.IsSitting = false;
				this.SitStatusChanged?.Invoke(this);
				Send.ZC_REST_SIT(this);
			}
		}

		/// <summary>
		/// Returns whether the character can currently sit.
		/// </summary>
		/// <returns>True if sitting is allowed.</returns>
		public bool CanSit()
		{
			// Explicitly disabled (HangingShot, etc.)
			if (_sittingDisabled)
				return false;

			// Can't sit while dead
			if (this.IsDead)
				return false;

			// Can't sit while warping
			if (this.IsWarping)
				return false;

			// Can't sit while riding companion
			if (this.IsRiding)
				return false;

			// Can't sit while casting non-moveable skills
			if (this.IsCasting() && !this.IsMoveableCasting())
				return false;

			// Can't sit while movement locked (knockback, stun, etc.)
			if (this.IsLocked(LockType.Movement))
				return false;

			// Can't sit while attached via FlyWith (HangingShot)
			// Note: Riding uses IsRiding flag, not AttachmentComponent
			if (this.Components.TryGet<AttachmentComponent>(out var attachment)
				&& attachment.IsAttached
				&& attachment.Type == AttachmentType.FlyWith)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Returns whether the character can currently mount a companion.
		/// </summary>
		/// <returns>True if mounting is allowed.</returns>
		public bool CanMount()
		{
			// Can't mount while dead
			if (this.IsDead)
				return false;

			// Can't mount while warping
			if (this.IsWarping)
				return false;

			// Can't mount while already riding
			if (this.IsRiding)
				return false;

			// Can't mount while casting
			if (this.IsCasting())
				return false;

			// Can't mount while movement locked
			if (this.IsLocked(LockType.Movement))
				return false;

			// Can't mount while attached via FlyWith (HangingShot)
			if (this.Components.TryGet<AttachmentComponent>(out var attachment)
				&& attachment.IsAttached
				&& attachment.Type == AttachmentType.FlyWith)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Returns whether the character is currently in a FlyWith attachment (HangingShot).
		/// </summary>
		public bool IsInFlyWithAttachment()
		{
			return this.Components.TryGet<AttachmentComponent>(out var attachment)
				&& attachment.IsAttached
				&& attachment.Type == AttachmentType.FlyWith;
		}

		/// <summary>
		/// Plays an effect for the character, visible only to them.
		/// </summary>
		public void PlayEffectLocal(string packetString, float scale = 1, EffectLocation heightOffset = EffectLocation.Bottom, TimeSpan delay = default)
		{
			if (delay == default)
				delay = TimeSpan.Zero;
			Task.Delay(delay).ContinueWith(_ => this.PlayEffectLocal(this.Connection, packetString, scale, heightOffset));
		}

		/// <summary>
		/// Enable or disable character control via a client script.
		/// </summary>
		public void EnableControl(string controlScript, bool enabled)
		{
			Send.ZC_ENABLE_CONTROL(this.Connection, controlScript, enabled);
			Send.ZC_LOCK_KEY(this, controlScript, !enabled);
		}
	}
}
