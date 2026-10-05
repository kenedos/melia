using System;
using System.Collections;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;
using Yggdrasil.Util; // For RandomProvider

namespace Melia.Zone.Scripting.AI
{
	public abstract partial class AiScript
	{
		/// <summary>
		/// Service-like method within AiScript to move the entity to a specific target position.
		/// This routine will complete when the entity reaches the destination or movement is interrupted.
		/// </summary>
		public IEnumerable MoveToLocationAction(Position targetPosition, string debugName = "TargetLocation")
		{
			if (this.Entity == null) yield break;
			if (targetPosition == Position.Zero || targetPosition == Position.Invalid)
			{
				Log.Warning($"AiScript '{this.Entity.Name}': Invalid targetPosition for MoveToLocationAction ({debugName}).");
				yield break;
			}

			if (this.Entity.Position.Get2DDistance(targetPosition) < 10f)
			{
				if (this.ShowDebug)
					Log.Debug($"AiScript '{this.Entity.Name}' already at/near {debugName} target {targetPosition}.");
				yield break; // Already there
			}

			if (this.ShowDebug)
				Log.Debug($"AiScript '{this.Entity.Name}' MoveToLocationAction: Moving to {debugName} at {targetPosition}.");
			// Directly yield the base MoveTo primitive. EnumerableAi.Heartbeat will handle nesting.
			yield return this.MoveTo(targetPosition); // Assuming this.MoveTo is the base primitive IEnumerable

			// After this.MoveTo completes, control returns to the caller of MoveToLocationAction.
			// We can add a final log here, but the "waiting" is handled by the yield above.
			if (this.ShowDebug)
			{
				if (this.Entity.Components.TryGet(out MovementComponent movement))
				{
					Log.Debug($"AiScript '{this.Entity.Name}' MoveToLocationAction for {debugName} to {targetPosition} finished. At: {this.Entity.Position}, IsMoving: {movement.IsMoving}.");
				}
				else
				{
					Log.Debug($"AiScript '{this.Entity.Name}' MoveToLocationAction for {debugName} to {targetPosition} finished.");
				}
			}
		}

		public IEnumerable MoveToWarpAction(Npc warpNpc)
		{
			if (warpNpc == null)
			{
				Log.Warning($"AiScript '{this.Entity.Name}': MoveToWarpAction called with null warpNpc.");
				yield break;
			}
			if (this.ShowDebug)
				Log.Debug($"AiScript '{this.Entity.Name}' moving to warp '{warpNpc.Name}' at {warpNpc.Position}.");
			yield return this.MoveToLocationAction(warpNpc.Position, $"Warp_{warpNpc.Name}");
		}

		public IEnumerable RoamAction(Position patrolCenter, float maxRoamDistance, float minRoam = 50f, float maxRoam = 150f)
		{
			if (this.Entity.Components.TryGet(out MovementComponent movement) && movement.IsMoving)
			{
				yield return true; yield break;
			}

			Position randomDest;
			if (patrolCenter != Position.Zero && this.Entity.Position.Get2DDistance(patrolCenter) > maxRoamDistance)
			{
				randomDest = patrolCenter.GetRandomInRange2D((int)(maxRoamDistance * 0.5f), (int)(maxRoamDistance * 0.8f));
			}
			else
			{
				randomDest = this.Entity.Position.GetRandomInRange2D((int)minRoam, (int)maxRoam);
			}

			// Try to find a walkable position at or near the random destination.
			if (this.Entity.Map.Ground.TryGetNearestValidPosition(randomDest, this.Entity.AgentRadius, out var validDest, 50f))
			{
				yield return this.MoveToLocationAction(validDest, "Roam");
			}
			else
			{
				if (this.ShowDebug)
					Log.Debug($"AiScript '{this.Entity.Name}' failed to find a walkable roam destination near {randomDest}.");
				yield return this.Wait(200, 500); // Use base AiScript's Wait
			}
		}

		public IEnumerable FollowTargetAction_Step(ICombatEntity targetToFollow, float idealFollowDistance, float maxStrayDistance)
		{
			if (targetToFollow == null || this.EntityGone(targetToFollow) || this.Entity.IsDead)
			{
				if (this.Entity.Components.TryGet(out MovementComponent selfMove) && selfMove.IsMoving)
				{
					yield return this.StopMove();
				}
				yield break;
			}

			var distanceToTarget = (float)this.Entity.GetDistance(targetToFollow);
			var targetIsMoving = targetToFollow.Components.Get<MovementComponent>()?.IsMoving ?? false;
			var amIMovingToTarget = this.Entity.Components.TryGet(out MovementComponent myMovement) && myMovement.IsMoving &&
									 myMovement.Destination.Get2DDistance(targetToFollow.Position) < idealFollowDistance * 1.5f; // Crude check

			if (amIMovingToTarget && distanceToTarget < idealFollowDistance * 0.8f && !targetIsMoving)
			{
				if (this.ShowDebug)
					Log.Debug($"AiScript '{this.Entity.Name}' FollowStep: Close to stationary target, stopping.");
				yield return this.StopMove();
				yield break;
			}

			var needsToMove = false;
			var moveDestination = targetToFollow.Position;

			if (distanceToTarget > maxStrayDistance)
			{
				needsToMove = true;
				moveDestination = targetToFollow.Position.GetRandomInRange2D((int)(idealFollowDistance * 0.3f), (int)(idealFollowDistance * 0.7f));
			}
			else if (distanceToTarget > idealFollowDistance)
			{
				needsToMove = true;
				moveDestination = targetToFollow.Position.GetRandomInRange2D((int)(idealFollowDistance * 0.5f), (int)(idealFollowDistance * 0.8f));
			}

			if (needsToMove)
			{
				// If already moving, but destination is too different from new ideal, or not moving at all
				if (!amIMovingToTarget || (myMovement != null && myMovement.Destination.Get2DDistance(moveDestination) > idealFollowDistance * 0.3f))
				{
					if (this.ShowDebug)
						Log.Debug($"AiScript '{this.Entity.Name}' FollowStep: Moving/repathing to {targetToFollow.Name} at {moveDestination}.");
					yield return this.MoveTo(moveDestination); // Yield the primitive
				}
				else
				{
					yield return true; // Already moving correctly
				}
			}
			else if (myMovement != null && myMovement.IsMoving && !targetIsMoving)
			{
				if (this.ShowDebug)
					// Close enough, target stopped, bot still moving
					Log.Debug($"AiScript '{this.Entity.Name}' FollowStep: Close to stationary target, stopping residual move.");
				yield return this.StopMove();
			}
			else
			{
				yield return true; // No movement needed this tick
			}
		}

		public IEnumerable FollowTargetAction(ICombatEntity targetToFollow, float idealFollowDistance, float maxStrayDistance)
		{
			if (targetToFollow == null || this.EntityGone(targetToFollow) || this.Entity.IsDead)
			{
				if (this.Entity.Components.TryGet(out MovementComponent selfMove) && selfMove.IsMoving) yield return this.StopMove();
				yield break;
			}

			var distanceToTarget = (float)this.Entity.GetDistance(targetToFollow);
			var targetIsMoving = targetToFollow.Components.Get<MovementComponent>()?.IsMoving ?? false;
			var moveToTargetRoutineName = $"FollowMove_{targetToFollow.Handle}_{targetToFollow.Position.GetHashCode()}"; // More unique name

			if (this.Entity.Components.TryGet(out MovementComponent botMovement) && botMovement.IsMoving && this.CurrentRoutine == moveToTargetRoutineName)
			{
				if (distanceToTarget < idealFollowDistance * 0.8f && !targetIsMoving)
				{
					yield return this.StopMove();
					yield break;
				}
				// If already correctly moving, let it continue.
				// This yield is important so that the caller (FollowingMasterRoutine) yields too.
				yield return true;
				yield break;
			}

			bool needsToMove = false;
			Position moveDestination = targetToFollow.Position;

			if (distanceToTarget > maxStrayDistance)
			{
				needsToMove = true;
				moveDestination = targetToFollow.Position.GetRandomInRange2D((int)(idealFollowDistance * 0.3f), (int)(idealFollowDistance * 0.7f));
			}
			else if (distanceToTarget > idealFollowDistance)
			{
				needsToMove = true;
				moveDestination = targetToFollow.Position.GetRandomInRange2D((int)(idealFollowDistance * 0.5f), (int)(idealFollowDistance * 0.8f));
			}

			if (needsToMove)
			{
				// No need to check CurrentRoutine here again if MoveToLocationAction handles it
				yield return this.MoveToLocationAction(moveDestination, $"FollowMove_{targetToFollow.Handle}");
			}
			else if (botMovement != null && botMovement.IsMoving && !targetIsMoving)
			{
				yield return this.StopMove();
			}
			else
			{
				yield return true; // No movement needed this tick, but yield to allow other logic.
			}
		}
	}
}
