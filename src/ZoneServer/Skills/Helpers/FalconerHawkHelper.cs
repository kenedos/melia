using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Helper class for Falconer hawk interactions.
	/// Provides methods to retrieve and command the hawk companion.
	/// </summary>
	/// <remarks>
	/// This helper manages hawk state through Companion.Vars:
	/// - Hawk.UsingSkill: bool - Whether hawk is currently executing a skill
	/// - Companion.IsLandedOnShoulder: bool - Whether hawk is landed on owner's shoulder
	/// - Hawk.Circling.Active: bool - Whether Circling is active
	/// - Hawk.Circling.PadId: int - Handle of current Circling pad
	/// - Hawk.Aiming.Active: bool - Whether Aiming is active
	/// - Hawk.Aiming.PadId: int - Handle of current Aiming pad
	/// - Hawk.Hovering.Active: bool - Whether Hovering is active
	/// - Hawk.FirstStrike.Active: bool - Whether First Strike auto-attack is active
	///
	/// Skill serialisation (one hawk skill at a time) is handled by
	/// FalconerHawkQueue; this helper only exposes the lock primitives
	/// and stateless hawk utilities.
	/// </remarks>
	public static class FalconerHawkHelper
	{
		/// <summary>
		/// Default flying height for the hawk.
		/// </summary>
		public const float DefaultHawkHeight = 80f;

		/// <summary>
		/// Distance at which hawk teleports to owner instead of flying.
		/// </summary>
		public const float TeleportDistance = 250f;

		/// <summary>
		/// Attempts to get the Falconer's active hawk companion.
		/// </summary>
		/// <param name="caster">The caster (Falconer)</param>
		/// <param name="hawk">The hawk companion if found</param>
		/// <returns>True if hawk was found and is active</returns>
		public static bool TryGetHawk(ICombatEntity caster, out Companion hawk)
		{
			hawk = null;

			if (caster is not Character character)
				return false;

			if (!character.TryGetActiveBirdCompanion(out var companion))
				return false;

			hawk = companion;
			return true;
		}

		/// <summary>
		/// Checks if the hawk's actions are currently locked (using a skill).
		/// </summary>
		/// <param name="hawk">The hawk companion</param>
		/// <returns>True if hawk is busy with a skill</returns>
		public static bool IsHawkBusy(Companion hawk)
		{
			if (hawk == null)
				return false;

			return hawk.Vars.Get<bool>("Hawk.UsingSkill", false);
		}

		/// <summary>
		/// Locks the hawk for a skill: sets busy flag and optionally movement.
		/// </summary>
		public static void LockHawk(Companion hawk, bool lockMovement = true)
		{
			if (hawk == null)
				return;

			hawk.Vars.Set("Hawk.UsingSkill", true);

			if (lockMovement)
				hawk.Lock(LockType.Movement);
		}

		/// <summary>
		/// Unlocks the hawk after a skill: clears the busy flag and
		/// restores AI movement.
		/// </summary>
		public static void UnlockHawk(Companion hawk)
		{
			if (hawk == null)
				return;

			hawk.Vars.Set("Hawk.UsingSkill", false);
			hawk.Unlock(LockType.Movement);
		}

		/// <summary>
		/// Fully resets hawk skill state. Called when the hawk dies so
		/// that stale skill/hidden flags don't persist through
		/// revival and block all future skill use.
		/// </summary>
		public static void ResetHawkState(Companion hawk)
		{
			if (hawk == null)
				return;

			hawk.Vars.Set("Hawk.UsingSkill", false);
			hawk.Vars.Set("Hawk.Circling.Active", false);
			hawk.Vars.Set("Hawk.Aiming.Active", false);
			hawk.Vars.Set("Hawk.Hovering.Active", false);

			hawk.Unlock(LockType.Movement);

			FalconerHawkQueue.Clear(hawk);
		}

		/// <summary>
		/// Commands the hawk to execute Circling at the specified position.
		/// </summary>
		public static void ExecuteCircling(ICombatEntity caster, Companion hawk, Skill skill, Position targetPos)
		{
			if (hawk == null || IsHawkBusy(hawk))
				return;

			hawk.Vars.Set("Hawk.Circling.Active", true);
			hawk.Vars.Set("Hawk.Circling.Position", targetPos);

			UnrestHawkIfNeeded(hawk);
		}

		/// <summary>
		/// Stops the Circling skill.
		/// </summary>
		public static void StopCircling(ICombatEntity caster, Companion hawk)
		{
			if (hawk == null)
				return;

			hawk.Vars.Set("Hawk.Circling.Active", false);
		}

		/// <summary>
		/// Commands the hawk to execute Aiming at the specified position.
		/// </summary>
		public static void ExecuteAiming(ICombatEntity caster, Companion hawk, Skill skill, Position targetPos)
		{
			if (hawk == null)
				return;

			UnrestHawkIfNeeded(hawk);

			hawk.Vars.Set("Hawk.Aiming.Active", true);
			hawk.Vars.Set("Hawk.Aiming.Position", targetPos);
		}

		/// <summary>
		/// Stops the Aiming skill.
		/// </summary>
		public static void StopAiming(ICombatEntity caster, Companion hawk)
		{
			if (hawk == null)
				return;

			hawk.Vars.Set("Hawk.Aiming.Active", false);
		}

		/// <summary>
		/// Unrests the hawk if it's currently resting on shoulder or roost.
		/// </summary>
		public static void UnrestHawkIfNeeded(Companion hawk)
		{
			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();
			else if (hawk.IsOnRoost)
				hawk.LeaveRoost();
		}

		/// <summary>
		/// Registers a roost for the hawk.
		/// </summary>
		public static void RegisterRoost(ICombatEntity caster, ICombatEntity roost)
		{
			// Remove old roost if exists
			var oldHandle = (int)caster.GetTempVar("Falconer.Roost.Handle");

			if (oldHandle != 0
				&& caster.Map.TryGetCombatEntity(oldHandle, out var oldRoost)
				&& oldRoost is Mob mob)
			{
				mob.Kill(null);
			}

			caster.SetTempVar("Falconer.Roost.Handle", roost.Handle);
		}

		/// <summary>
		/// Unregisters the roost.
		/// </summary>
		public static void UnregisterRoost(ICombatEntity caster)
		{
			caster.RemoveTempVar("Falconer.Roost.Handle");
		}

		/// <summary>
		/// Attaches a character to their hawk using the attachment system.
		/// </summary>
		/// <param name="character">The character to attach.</param>
		/// <param name="hawk">The hawk to attach to.</param>
		/// <param name="nodeName">Attachment bone name.</param>
		/// <param name="offset">Vertical offset.</param>
		public static void AttachToHawk(Character character, Companion hawk, string nodeName, float offset)
		{
			if (!character.Components.TryGet<AttachmentComponent>(out var attachment))
			{
				attachment = new AttachmentComponent(character);
				character.Components.Add(attachment);
			}

			attachment.AttachTo(
				hawk,
				nodeName,
				Math.Abs(offset),
				AttachmentType.FlyWith,
				isController: true,
				syncDirection: false
			);

			// Enable position-only movement mode
			if (character.Components.TryGet<MovementComponent>(out var movement))
			{
				movement.SetAttachmentMovementMode(true);
			}
		}

		/// <summary>
		/// Detaches a character from their hawk.
		/// </summary>
		/// <param name="character">The character to detach.</param>
		public static void DetachFromHawk(Character character)
		{
			if (character.Components.TryGet<AttachmentComponent>(out var attachment))
			{
				attachment.Detach();
			}

			// Disable position-only movement mode
			if (character.Components.TryGet<MovementComponent>(out var movement))
			{
				movement.SetAttachmentMovementMode(false);
			}
		}
	}
}
