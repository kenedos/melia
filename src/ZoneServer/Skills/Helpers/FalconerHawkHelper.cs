using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Helper class for Falconer hawk interactions.
	/// Provides methods to retrieve and command the hawk companion.
	/// </summary>
	/// <remarks>
	/// This helper manages hawk state through Companion.Vars:
	/// - Hawk.UsingSkill: bool - Whether hawk is currently executing a skill
	/// - Hawk.SkillFunction: string - Name of current skill being executed
	/// - Hawk.FlyingAway: bool - Whether hawk has flown away and is hidden
	/// - Hawk.StopFlyAway: bool - Flag to interrupt fly-away sequence
	/// - Companion.IsLandedOnShoulder: bool - Whether hawk is landed on owner's shoulder
	/// - Hawk.IsHidden: bool - Whether hawk is hidden (not visible)
	/// - Hawk.Circling.Active: bool - Whether Circling is active
	/// - Hawk.Circling.PadId: int - Handle of current Circling pad
	/// - Hawk.Aiming.Active: bool - Whether Aiming is active
	/// - Hawk.Aiming.PadId: int - Handle of current Aiming pad
	/// - Hawk.Hovering.Active: bool - Whether Hovering is active
	/// - Hawk.FirstStrike.Active: bool - Whether First Strike auto-attack is active
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
		/// Distance hawk flies away from owner after skills.
		/// </summary>
		public const float FlyAwayDistance = 300f;

		/// <summary>
		/// Per-hawk skill queues, keyed by hawk handle.
		/// </summary>
		private static readonly Dictionary<int, Queue<Action>> _skillQueues = new();

		/// <summary>
		/// Queues a skill action for the hawk if it is currently busy.
		/// Returns true if the action was queued, false if the hawk is
		/// free and the caller should execute immediately.
		/// </summary>
		public static bool TryQueueSkill(Companion hawk, Action executeSkill)
		{
			if (!IsHawkBusy(hawk))
				return false;

			if (!_skillQueues.TryGetValue(hawk.Handle, out var queue))
			{
				queue = new Queue<Action>();
				_skillQueues[hawk.Handle] = queue;
			}
			queue.Enqueue(executeSkill);
			return true;
		}

		/// <summary>
		/// Returns true if the hawk has queued skills waiting.
		/// </summary>
		public static bool HasQueuedSkills(Companion hawk)
		{
			return _skillQueues.TryGetValue(hawk.Handle, out var queue) && queue.Count > 0;
		}

		/// <summary>
		/// Clears all queued skills for the hawk.
		/// </summary>
		public static void ClearQueue(Companion hawk)
		{
			if (_skillQueues.ContainsKey(hawk.Handle))
				_skillQueues[hawk.Handle].Clear();
		}

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

			// Try to get hawk companion specifically
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
			return hawk.Vars.Get<bool>("Hawk.UsingSkill", false);
		}

		/// <summary>
		/// Locks the hawk for a skill: sets the busy flag and locks
		/// movement so the AI cannot move the hawk mid-attack.
		/// </summary>
		public static void LockHawk(Companion hawk)
		{
			hawk.Vars.Set("Hawk.UsingSkill", true);
			hawk.Vars.Set("Hawk.LastSkillStartTime", DateTime.UtcNow);
			hawk.Lock(LockType.Movement);
		}

		/// <summary>
		/// Returns true if the hawk has used a skill too recently
		/// (within 2.5 seconds).
		/// </summary>
		public static bool IsOnGlobalCooldown(Companion hawk)
		{
			if (hawk.Vars.TryGet<DateTime>("Hawk.LastSkillStartTime", out var lastStart))
				return (DateTime.UtcNow - lastStart).TotalMilliseconds < 2500;
			return false;
		}

		/// <summary>
		/// Returns the number of milliseconds remaining on the hawk's
		/// 2.5s global cooldown, or 0 if it has expired.
		/// </summary>
		public static double GetGlobalCooldownRemaining(Companion hawk)
		{
			if (hawk.Vars.TryGet<DateTime>("Hawk.LastSkillStartTime", out var lastStart))
			{
				var remaining = 2500.0 - (DateTime.UtcNow - lastStart).TotalMilliseconds;
				return remaining > 0 ? remaining : 0;
			}
			return 0;
		}

		/// <summary>
		/// Unlocks the hawk after a skill: clears the busy flag and
		/// restores AI movement.
		/// </summary>
		public static void UnlockHawk(Companion hawk)
		{
			hawk.Vars.Set("Hawk.UsingSkill", false);
			hawk.Unlock(LockType.Movement);
		}

		/// <summary>
		/// Checks if the hawk is currently flying away or hidden.
		/// </summary>
		/// <param name="hawk">The hawk companion</param>
		/// <returns>True if hawk is flying away</returns>
		public static bool IsHawkFlyingAway(Companion hawk)
		{
			var flyingAway = hawk.Vars.Get<bool>("Hawk.FlyingAway", false);
			if (flyingAway)
				return true;

			return hawk.Vars.Get<bool>("Hawk.IsHidden", false);
		}

		/// <summary>
		/// Makes the hawk unhide and return to the target.
		/// Called by Calling skill.
		/// </summary>
		/// <param name="skill">The skill</param>
		/// <param name="caster">The Falconer</param>
		/// <param name="hawk">The hawk</param>
		public static async Task HawkUnhide(Skill skill, ICombatEntity caster, Companion hawk)
		{
			if (hawk == null)
				return;

			// Check if currently flying away
			var isFlyingAway = hawk.Vars.Get<bool>("Hawk.FlyingAway", false);
			if (isFlyingAway)
			{
				// Set stop flag and clear flying away state
				hawk.Vars.Set("Hawk.StopFlyAway", true);
				hawk.Vars.Set("Hawk.FlyingAway", false);
			}
			else
			{
				// Completely hidden - teleport near owner
				var returnPos = caster.Position.GetRandomInRange2D(30, 50);
				returnPos = new Position(returnPos.X, returnPos.Y + DefaultHawkHeight, returnPos.Z);
				hawk.SetPosition(returnPos);
				hawk.SetHide(false);
				hawk.Vars.Set("Hawk.IsHidden", false);
			}

			// Clear skill lock
			UnlockHawk(hawk);

			// Move towards owner
			hawk.PlayEffect("F_buff_basic008_blue", scale: 1f);

			await skill.Wait(TimeSpan.FromMilliseconds(500));
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
		/// Commands the hawk to execute Blistering Thrash / Sonic Strike attack.
		/// </summary>
		public static async Task ExecuteBlisteringThrash(ICombatEntity caster, Companion hawk, Skill skill, Position targetPos, ICombatEntity target = null)
		{
			if (hawk == null)
				return;

			// Interrupt current action if busy
			if (IsHawkBusy(hawk))
			{
				await InterruptHawkAction(skill, hawk);
			}

			LockHawk(hawk);

			UnrestHawkIfNeeded(hawk);
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			var attackPos = target?.Position ?? targetPos;

			// Screen shake
			Send.ZC_CHANGE_CAMERA_ZOOM(hawk, 2, 99999f, 7f, 0.5f, 50f, 0f, 0f);

			// Hawk dive to target position using PenetratePosition
			var divePos = new Position(attackPos.X, attackPos.Y + DefaultHawkHeight, attackPos.Z);
			var syncKey = hawk.GenerateSyncKey();
			Send.ZC_NORMAL.PenetratePosition(hawk, divePos, 10f, syncKey, "HOVERING_SHOT", 0.7f, 7f, 0.5f, 0.7f, 30f);

			// Shockwave effect
			hawk.BroadcastShockWave(2, 7, 0.5f, 50f, 0);

			await skill.Wait(TimeSpan.FromMilliseconds(1200));

			// Fly away after attack
			await HawkFlyAway(skill, caster, hawk);
		}

		/// <summary>
		/// Commands the hawk to execute Hovering at the specified position.
		/// Note: Most hovering logic is in the skill handler; this just sets state.
		/// </summary>
		public static async Task ExecuteHovering(ICombatEntity caster, Companion hawk, Skill skill, Position targetPos)
		{
			if (hawk == null)
				return;

			// Interrupt if busy
			if (IsHawkBusy(hawk))
			{
				await InterruptHawkAction(skill, hawk);
			}

			LockHawk(hawk);

			UnrestHawkIfNeeded(hawk);
		}

		/// <summary>
		/// Commands the hawk to execute Combination attack.
		/// Note: Most logic is in skill handler.
		/// </summary>
		public static async Task ExecuteCombination(ICombatEntity caster, Companion hawk, Skill skill, ICombatEntity target)
		{
			if (hawk == null || target == null)
				return;

			LockHawk(hawk);

			UnrestHawkIfNeeded(hawk);

			// Combination doesn't cause fly-away, handled in skill
			await Task.CompletedTask;
		}

		/// <summary>
		/// Commands the hawk to execute Hanging Shot (player hangs from hawk).
		/// </summary>
		public static async Task ExecuteHangingShot(ICombatEntity caster, Companion hawk, Skill skill)
		{
			if (hawk == null)
				return;

			LockHawk(hawk);

			UnrestHawkIfNeeded(hawk);

			await Task.CompletedTask;
		}

		/// <summary>
		/// Commands the hawk to interact with Pheasant decoy.
		/// </summary>
		public static async Task ExecutePheasantAttack(ICombatEntity caster, Companion hawk, Skill skill, ICombatEntity pheasant, Position targetPos)
		{
			if (hawk == null || pheasant == null)
				return;

			LockHawk(hawk);

			UnrestHawkIfNeeded(hawk);

			var syncKey = hawk.GenerateSyncKey();

			// Dive to pheasant
			Send.ZC_NORMAL.CollisionAndBack(hawk, pheasant, syncKey, "HOVERING_SHOT", 1f, 7f, 1f, 0.7f, 20f, true);

			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			await HawkFlyAway(skill, caster, hawk);
		}

		/// <summary>
		/// Makes the hawk fly away after using a skill.
		/// </summary>
		public static async Task HawkFlyAway(Skill skill, ICombatEntity caster, Companion hawk)
		{
			if (hawk == null)
				return;

			if (caster is not Character character)
				return;

			// If there's a queued skill, wait until the 2.5s global
			// cooldown has passed, then fire the next skill
			if (HasQueuedSkills(hawk))
			{
				var remaining = 2500.0;
				if (hawk.Vars.TryGet<DateTime>("Hawk.LastSkillStartTime", out var lastStart))
					remaining = 2500.0 - (DateTime.UtcNow - lastStart).TotalMilliseconds;
				if (remaining > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(remaining));

				UnlockHawk(hawk);

				if (_skillQueues.TryGetValue(hawk.Handle, out var queue) && queue.Count > 0)
				{
					var next = queue.Dequeue();
					next();
				}
				return;
			}

			UnlockHawk(hawk);

			// Check for roost - don't fly away if owner near roost
			var roostHandle = (int)caster.GetTempVar("Falconer.Roost.Handle");
			if (roostHandle != 0 && caster.Map.TryGetCombatEntity(roostHandle, out var roost))
			{
				if (roost != null && !roost.IsDead)
				{
					hawk.Vars.Set("Hawk.LastSkillEndTime", DateTime.UtcNow);
					return;
				}
			}

			// Check if First Strike is active - hawk doesn't fly away
			if (caster.IsBuffActive(BuffId.FirstStrike_Buff))
				return;

			// Falconer20: Hawk Hunt - hawk doesn't fly away
			if (caster.IsAbilityActive(AbilityId.Falconer20))
				return;

			// Check if already flying away
			if (hawk.Vars.Get<bool>("Hawk.FlyingAway", false))
				return;

			hawk.Vars.Set("Hawk.FlyingAway", true);

			// Get random fly away position
			var randomAngle = RandomProvider.Get().Next(360);
			var flyPos = caster.Position.GetRelative(new Direction(randomAngle), FlyAwayDistance);
			flyPos = new Position(flyPos.X, flyPos.Y + DefaultHawkHeight + 200f, flyPos.Z);

			// Start flying away
			await skill.Wait(TimeSpan.FromMilliseconds(5000));

			// Check if fly-away was interrupted
			var stopFlyAway = hawk.Vars.Get<bool>("Hawk.StopFlyAway", false);
			if (stopFlyAway)
			{
				hawk.Vars.Set("Hawk.StopFlyAway", false);
				hawk.Vars.Set("Hawk.FlyingAway", false);
				return;
			}

			// Still flying away - hide hawk
			if (hawk.Vars.Get<bool>("Hawk.FlyingAway", false))
			{
				hawk.SetHide(true);
				hawk.Vars.Set("Hawk.IsHidden", true);
				hawk.Vars.Set("Hawk.FlyingAway", false);
			}
		}

		/// <summary>
		/// Interrupts the current hawk action.
		/// </summary>
		private static async Task InterruptHawkAction(Skill skill, Companion hawk)
		{
			var currentFunction = hawk.Vars.Get<string>("Hawk.SkillFunction", "None");

			UnlockHawk(hawk);

			if (currentFunction == "Hovering" || currentFunction == "Circling")
			{
				hawk.Vars.Set("Hawk.Hovering.Active", false);
				hawk.Vars.Set("Hawk.Circling.Active", false);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(100));
		}

		/// <summary>
		/// Unrests the hawk if it's currently resting on shoulder or roost.
		/// </summary>
		private static void UnrestHawkIfNeeded(Companion hawk)
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
