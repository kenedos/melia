using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// A component that controls an entity's movement.
	/// </summary>
	public class MovementComponent : CombatEntityComponent, IUpdateable
	{
		private Queue<Position> _path;

		private readonly object _positionSyncLock = new();
		private double _moveX, _moveZ;
		private TimeSpan _moveTime;

		private ITriggerableArea[] _triggerAreas = [];

		/// <summary>
		/// Returns the entity's current destination, if it's moving to
		/// a specific location.
		/// </summary>
		public Position Destination { get; private set; }

		/// <summary>
		/// Returns the entity's final destination, if it's moving to a
		/// specific location on a path.
		/// </summary>
		public Position FinalDestination { get; private set; }

		/// <summary>
		/// Returns whether the entity is currently moving.
		/// </summary>
		public bool IsMoving { get; private set; }

		/// <summary>
		/// Gets or sets where the entity is moving to.
		/// </summary>
		public MoveTargetType MoveTarget { get; private set; }

		/// <summary>
		/// Returns whether the entity is currently on the ground or
		/// in the air.
		/// </summary>
		public bool IsGrounded { get; private set; } = true;

		/// <summary>
		/// Returns whether the entity is currently on the flying or
		/// not
		/// </summary>
		public bool IsFlying { get; private set; } = false;

		/// <summary>
		/// Enable or disable a minimap marker being shown when the entity moves.
		/// </summary>
		public bool ShowMinimapMarker { get; set; } = false;

		/// <summary>
		/// Returns the entity's current movement speed type.
		/// </summary>
		public MoveSpeedType MoveSpeedType { get; private set; }

		/// <summary>
		/// When true, use ZC_MOVE_POS instead of ZC_PC_MOVE_STOP for position updates.
		/// Used when entity is attached and movement should be position-based only.
		/// </summary>
		public bool UsePositionOnlyMovement { get; set; } = false;


		/// <summary>
		/// Enables or disables movement debug packets being sent.
		/// </summary>
		public bool ShowDebug { get; set; }

		/// <summary>
		/// Creates new movement component.
		/// </summary>
		/// <param name="entity"></param>
		public MovementComponent(ICombatEntity entity) : base(entity)
		{
		}

		/// <summary>
		/// Moves the entity from the starting position to the ending position after a delay and within a specified timespan.
		/// </summary>
		/// <param name="startPos">The starting position.</param>
		/// <param name="endPos">The ending position.</param>
		/// <param name="delay">The delay before starting the movement.</param>
		/// <param name="timespan">The duration of the movement.</param>
		public async Task MoveToWithDelay(Position startPos, Position endPos, TimeSpan delay, TimeSpan timespan)
		{
			if (!this.Entity.CanMove())
				return;

			await Task.Delay(delay);

			lock (_positionSyncLock)
			{
				// Don't move if the entity is already at the destination
				if (endPos == this.Entity.Position)
					return;

				// Set initial position
				this.Entity.Position = startPos;

				// Calculate movement parameters
				var diffX = endPos.X - startPos.X;
				var diffZ = endPos.Z - startPos.Z;
				var totalDistance = Math.Sqrt(diffX * diffX + diffZ * diffZ);
				var speed = totalDistance / timespan.TotalSeconds;

				_moveTime = timespan;
				_moveX = diffX / _moveTime.TotalSeconds;
				_moveZ = diffZ / _moveTime.TotalSeconds;

				this.IsMoving = true;
				this.Destination = endPos;
				this.MoveTarget = MoveTargetType.Position;

				// Set direction relative to current position
				this.Entity.Direction = startPos.GetDirection(endPos);

				var fromCellPos = this.Entity.Map.Ground.GetCellPosition(startPos);
				var toCellPos = this.Entity.Map.Ground.GetCellPosition(endPos);

				// Update clients
				Send.ZC_MOVE_PATH(this.Entity, fromCellPos, toCellPos, (float)speed);
			}
		}

		/// <summary>
		/// Makes entity move to the given destination via path.
		/// Returns the amount of time the move will take.
		/// </summary>
		/// <remarks>
		/// The position doesn't need a correct Y coordinate, as the
		/// method sets it as needed.
		/// </remarks>
		/// <param name="destination"></param>
		/// <returns></returns>
		public TimeSpan MoveTo(Position destination)
		{
			if (!this.IsValidDestination(destination))
				return TimeSpan.Zero;

			var pathfinder = this.Entity.Map.Pathfinder;
			var start = this.Entity.Position;
			var end = destination;
			var radius = this.Entity.AgentRadius;

			if (!pathfinder.TryFindPath(start, end, radius, out var path))
				return TimeSpan.Zero;

			return this.MovePath(path, true);
		}

		/// <summary>
		/// Makes entity move to the given destination in a straight line.
		/// Returns the amount of time the move will take.
		/// </summary>
		/// <remarks>
		/// The position doesn't need a correct Y coordinate, as the
		/// method sets it as needed.
		/// </remarks>
		/// <param name="destination"></param>
		public TimeSpan MoveStraight(Position destination)
		{
			if (!this.IsValidDestination(destination))
				return TimeSpan.Zero;

			return this.MovePath([destination], true);
		}

		/// <summary>
		/// Calculates and returns the time it would take the entity
		/// to move to the destination from its current position.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="walk"></param>
		/// <returns></returns>
		public TimeSpan CalcMoveToTime(Position destination)
		{
			return this.MoveToConditional(destination, false);
		}

		/// <summary>
		/// Prepares and possibly starts movement through the given
		/// path list, returning the amount of time it will/would 
		/// take the entity to get there.
		/// </summary>
		/// <remarks>
		/// The destinations doesn't need correct Y coordinates, as the
		/// method sets them as needed.
		/// </remarks>
		/// <remarks>
		/// The path list is expected to include the start
		/// and destination positons.
		/// </remarks>
		/// <param name="destinations"></param>
		/// <param name="executeMove"></param>
		/// <returns></returns>
		public TimeSpan MovePath(IEnumerable<Position> destinations, bool executeMove)
		{
			var destQueue = new Queue<Position>(destinations);

			lock (_positionSyncLock)
			{
				var curPos = this.Entity.Position;

				// We're treating the path as a list of destinations, so we'll
				// dequeue the first destination if it's the current position,
				// just in case someone decided to start the path with the
				// current position, as path finding algorithms tend to do.
				if (destQueue.Peek() == curPos)
					destQueue.Dequeue();

				// Check the destinations we have left after potentially removing
				// the first one
				if (destQueue.Count == 0)
					return TimeSpan.Zero;

				var totalDistance = 0.0;
				var prevPos = curPos;

				foreach (var pos in destQueue)
				{
					totalDistance += prevPos.Get2DDistance(pos);
					prevPos = pos;
				}

				var speed = this.Entity.Properties.GetFloat(PropertyName.MSPD);

				// Prevent division by zero
				if (speed < 1f)
					return TimeSpan.MaxValue;

				var totalMoveTime = TimeSpan.FromSeconds(totalDistance / speed);

				if (executeMove)
				{
					_path = destQueue;

					this.IsMoving = true;
					this.FinalDestination = _path.Last();
					this.MoveTarget = MoveTargetType.Position;

					this.ExecuteNextMove();
				}

				return totalMoveTime;
			}
		}

		/// <summary>
		/// Returns whether the entity can move to the destination,
		/// based on its validity and the entity's current state.
		/// </summary>
		/// <param name="destination"></param>
		/// <returns></returns>
		private bool IsValidDestination(Position destination)
		{
			lock (_positionSyncLock)
			{
				var distance = this.Entity.Position.Get3DDistance(destination);
				var speed = this.Entity.Properties.GetFloat(PropertyName.MSPD);

				// Don't move if too close to destination
				if (distance <= 10)
					return false;

				// With 0 speed, we can't move anywhere
				if (speed == 0)
					return false;

				// Can't move if entity is casting
				if (this.Entity.IsCasting())
					return false;

				return true;
			}
		}

		/// <summary>
		/// Sets the destination to the next position in the path list.
		/// </summary>
		private void ExecuteNextMove()
		{
			lock (_positionSyncLock)
			{
				if (_path.Count == 0)
					return;

				var nextDestination = _path.Dequeue();

				var position = this.Entity.Position;
				var diffX = nextDestination.X - position.X;
				var diffZ = nextDestination.Z - position.Z;
				var distance = Math.Sqrt(diffX * diffX + diffZ * diffZ);

				var speed = this.Entity.Properties.GetFloat(PropertyName.MSPD);

				if (speed <= 0)
				{
					_moveTime = TimeSpan.Zero;
					_moveX = 0;
					_moveZ = 0;
					return;
				}

				_moveTime = TimeSpan.FromSeconds(distance / speed);
				_moveX = (diffX / _moveTime.TotalSeconds);
				_moveZ = (diffZ / _moveTime.TotalSeconds);

				this.Destination = nextDestination;
				this.Entity.Direction = position.GetDirection(nextDestination);

				var fromCellPos = this.Entity.Map.Ground.GetCellPosition(position);
				var toCellPos = this.Entity.Map.Ground.GetCellPosition(nextDestination);

				Send.ZC_MOVE_PATH(this.Entity, fromCellPos, toCellPos, speed);
			}
		}

		/// <summary>
		/// Starts movement to destination if execution is requested,
		/// returns the amount of time it will/would take the entity
		/// to get there.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="executeMove"></param>
		private TimeSpan MoveToConditional(Position destination, bool executeMove)
		{
			if (!this.Entity.CanMove())
				return TimeSpan.Zero;

			lock (_positionSyncLock)
			{
				// Don't move if the entity is already at the destination
				if (destination == this.Entity.Position)
					return TimeSpan.Zero;

				// Get distance to destination
				var position = this.Entity.Position;
				var diffX = destination.X - position.X;
				var diffZ = destination.Z - position.Z;
				var distance = Math.Sqrt(diffX * diffX + diffZ * diffZ);

				// Get speed
				var speed = this.Entity.Properties.GetFloat(PropertyName.MSPD);

				// With 0 speed, we can't move anywhere
				if (speed == 0)
					return TimeSpan.Zero;

				// Don't move if too close to destination
				if (distance <= 10)
					return TimeSpan.Zero;

				// Calculate movement and move time
				_moveTime = TimeSpan.FromSeconds(distance / speed);
				_moveX = (diffX / _moveTime.TotalSeconds);
				_moveZ = (diffZ / _moveTime.TotalSeconds);

				if (executeMove)
				{
					this.IsMoving = true;
					this.Destination = destination;
					this.MoveTarget = MoveTargetType.Position;

					// Set direction relative to current position
					this.Entity.Direction = position.GetDirection(destination);

					var fromCellPos = this.Entity.Map.Ground.GetCellPosition(this.Entity.Position);
					var toCellPos = this.Entity.Map.Ground.GetCellPosition(this.Destination);

					// Update clients
					Send.ZC_MOVE_PATH(this.Entity, fromCellPos, toCellPos, speed);
				}

				return _moveTime;
			}
		}

		public void AddMarker()
		{
			this.ShowMinimapMarker = true;
			if (this.Entity is IMonster monster)
				Send.ZC_NORMAL.MinimapMarker(monster, 1, 1, 0);
			else if (this.Entity is Character character)
				Send.ZC_NORMAL.MinimapMarker(character, 2, 0);
		}

		public void RemoveMarker()
		{
			if (this.ShowMinimapMarker)
				Send.ZC_NORMAL.RemoveMapMarker(this.Entity);
			this.ShowMinimapMarker = false;
		}

		/// <summary>
		/// Stops movement and returns the current position the entity
		/// stopped at.
		/// </summary>
		/// <returns></returns>
		public Position Stop()
		{
			var pos = this.Entity.Position;

			if (this.IsMoving)
			{
				this.IsMoving = false;
				this.Destination = pos;

				Send.ZC_MOVE_STOP(this.Entity, pos);
			}

			// Clear any remaining path waypoints to prevent resuming movement
			// This is critical for knockback to work properly
			lock (_positionSyncLock)
			{
				_path?.Clear();
				_moveTime = TimeSpan.Zero;
				_moveX = 0;
				_moveZ = 0;
				this.FinalDestination = pos;
			}

			return pos;
		}

		/// <summary>
		/// Makes entity jump.
		/// </summary>
		/// <remarks>
		/// This method is primarily used by the server to forward
		/// character movement information. It should only be used
		/// by the packet handlers for the moment.
		/// </remarks>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		/// <param name="unkFloat"></param>
		/// <param name="unkByte"></param>
		internal void NotifyJump(Position pos, Direction dir, float unkFloat, byte unkByte)
		{
			this.Entity.Position = pos;
			this.Entity.Direction = dir;

			this.IsMoving = true;
			this.MoveTarget = MoveTargetType.Direction;

			if (this.Entity is Character character)
			{
				var staminaUsage = (int)character.Properties.GetFloat(PropertyName.Sta_Jump);
				character.ModifyStamina(-staminaUsage);

				Send.ZC_JUMP(character, pos, dir, unkFloat, unkByte);
			}
		}

		/// <summary>
		/// Sets whether the entity is currently on the ground.
		/// </summary>
		/// <remarks>
		/// This method is primarily used by the server to forward
		/// character movement information. It should only be used
		/// by the packet handlers for the moment.
		/// </remarks>
		/// <param name="grounded"></param>
		internal void NotifyGrounded(bool grounded)
		{
			this.IsGrounded = grounded;
		}

		public void NotifyFlying(bool flying, float height = 0, float raiseTime = 1, float easing = 0)
		{
			this.IsFlying = flying;
			if (this.Entity is Character character)
			{
				Send.ZC_FLY(character, height, 5);
			}
			else
			{
				Send.ZC_FLY_MATH(this.Entity, height, raiseTime, easing);
			}
		}

		/// <summary>
		/// Updates current position and direction of the entity.
		/// </summary>
		/// <remarks>
		/// This method is primarily used by the server to forward
		/// character movement information. It should only be used
		/// by the packet handlers for the moment.
		/// </remarks>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		/// <param name="unkFloat"></param>
		internal void NotifyMove(Position pos, Direction dir, float unkFloat)
		{
			var fromPos = this.Entity.Position;
			this.Entity.Position = pos;
			this.Entity.Direction = dir;

			this.IsMoving = true;
			this.MoveTarget = MoveTargetType.Direction;

			if (!UsePositionOnlyMovement)
				Send.ZC_MOVE_DIR(this.Entity, pos, dir, unkFloat);
			else
				Send.ZC_MOVE_POS(this.Entity, fromPos, pos, 60, 0, 1);
			if (this.Entity is Character character)
			{
				// Check if character is attached to something
				if (character.Components.TryGet<AttachmentComponent>(out var attachment) && attachment.IsAttached)
				{
					// Notify attachment component that we moved - it will sync hawk position
					attachment.OnAttachedEntityMoved(pos);
				}

				character.Connection.Party?.UpdateMemberInfo(character);
				// character.Connection.Guild?.UpdateMemberInfo(character); // Removed: Guild type deleted

				if (character.IsRiding && character.ActiveCompanion != null)
					character.ActiveCompanion.Position = pos;

				if (this.ShowMinimapMarker)
					Send.ZC_NORMAL.MinimapMarker(character, 2, 0);
			}
		}

		/// <summary>
		/// Stops movement and returns the new position.
		/// </summary>
		/// <remarks>
		/// This method is primarily used by the server to forward
		/// character movement information. It should only be used
		/// by the packet handlers for the moment.
		/// </remarks>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		internal Position NotifyStopMove(Position pos, Direction dir)
		{
			this.Entity.Position = pos;
			this.Entity.Direction = dir;

			this.IsMoving = false;
			this.Destination = pos;
			this.MoveTarget = MoveTargetType.Direction;

			if (this.Entity is Character character)
			{
				// Check if character is attached to something (e.g., hanging from hawk)
				if (character.Components.TryGet<AttachmentComponent>(out var attachment) && attachment.IsAttached)
				{
					// Notify attachment component that we moved - it will sync hawk position
					attachment.OnAttachedEntityMoved(pos);
				}

				// Sending ZC_MOVE_STOP works as well, but it doesn't have
				// a direction, so the character stops and looks north
				// on others' screens.
				Send.ZC_PC_MOVE_STOP(character, character.Position, character.Direction);
				if (this.ShowMinimapMarker)
					Send.ZC_NORMAL.MinimapMarker(character, 2, 0);
			}
			else
			{
				Send.ZC_MOVE_STOP(this.Entity, pos);
			}

			this.Entity.Components.Get<BuffComponent>()?.Remove(BuffId.DashRun);
			this.CheckWarp();

			return pos;
		}

		/// <summary>
		/// Checks for a warp at the entity's current position and executes
		/// it if they don't move for a moment.
		/// </summary>
		private void CheckWarp()
		{
			if (this.Entity is not Character character)
				return;

			if (character.IsOutOfBody())
				return;

			var prevPos = character.Position;

			// In the packets I don't see any indication for a client-side
			// trigger, so I guess the server has to check for warps and
			// initiate it all on its own. Seems a little weird... but
			// oh well. If this is a thing, we probably should have some
			// kind of "trigger" system. -- exec
			// 
			// Update: By now we know that this is in fact how it works,
			// but we also know that warps aren't triggered on a delay
			// as we initially assumed (see below). The official behavior
			// is to either warp on contact (classic) or after confirming
			// the warp in a dialog (newer versions). But since I'm not
			// a fan of either option we'll keep our own implementation.
			// Eventually we'll make it configurable. -- exec

			var warpNpc = this.Entity.Map.GetNearbyWarp(prevPos);
			if (warpNpc == null)
				return;

			// Wait 1s to see if the character actually wants to warp
			// (indicated by him not moving). Official behavior unknown,
			// as I have never played the game =<
			Task.Delay(1000).ContinueWith(_ =>
			{
				// Cancel if character moved in that time
				if (character.Position != prevPos)
					return;

				character.Warp(warpNpc.WarpLocation);
				warpNpc.IncreaseUseCount();
			});
		}

		/// <summary>
		/// Sets whether the entity is walking or running, scaling their
		/// movement speed up and down accordingly. Not supported by
		/// all entity types.
		/// </summary>
		/// <param name="type"></param>
		public void SetMoveSpeedType(MoveSpeedType type)
		{
			if (this.MoveSpeedType != type)
			{
				this.MoveSpeedType = type;
				this.Entity.Properties.Invalidate(PropertyName.MSPD);

				Send.ZC_MSPD(this.Entity);
			}
		}

		/// <summary>
		/// Fixes the entity's movement speed at the given value.
		/// </summary>
		/// <param name="mspd"></param>
		public void SetFixedMoveSpeed(float mspd)
		{
			this.Entity.Properties.SetFloat(PropertyName.FIXMSPD_BM, mspd);
			Send.ZC_MSPD(this.Entity);
		}

		/// <summary>
		/// Resets the entity's movement speed to its default value.
		/// </summary>
		public void ResetFixedMoveSpeed()
		{
			this.Entity.Properties.SetFloat(PropertyName.FIXMSPD_BM, 0);
			Send.ZC_MSPD(this.Entity);
		}

		/// <summary>
		/// Updates the entity's position while it's moving.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.UpdateMove(elapsed);
			this.UpdateTriggerAreas(elapsed);
		}

		/// <summary>
		/// Updates movement, setting positions and queuing moves.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateMove(TimeSpan elapsed)
		{
			lock (_positionSyncLock)
			{
				// No need to update the position if the character isn't moving.
				if (!this.IsMoving)
					return;

				// Don't update movement if the entity is movement locked (e.g., knocked back)
				// This prevents resuming movement during knockback states
				if (this.Entity.IsLocked(LockType.Movement))
				{
					// Stop and clear any remaining movement to prevent resumption
					this.IsMoving = false;
					_path?.Clear();
					_moveTime = TimeSpan.Zero;
					return;
				}

				// Don't update the position this way for directional
				// movement for now. That will require a bit more
				// research to get right.
				if (this.MoveTarget != MoveTargetType.Position)
					return;

				var arrived = (_moveTime -= elapsed) <= TimeSpan.Zero;

				if (!arrived)
				{
					this.UpdateEntityPosition(elapsed);
				}
				else
					this.QueueNextMove();
				if (this.ShowMinimapMarker)
				{
					if (this.Entity is IMonster monster)
						Send.ZC_NORMAL.MinimapMarker(monster, 1, 1, 0);
					else if (this.Entity is Character character)
						Send.ZC_NORMAL.MinimapMarker(character, 2, 0);
				}
				if (this.ShowDebug)
				{
					Send.ZC_TEST_DBG(this.Entity);
				}
			}
		}

		/// <summary>
		/// Stops movement if the end of the path was reached or queues the
		/// move to the next destination on it.
		/// </summary>
		private void QueueNextMove()
		{
			this.Entity.Position = this.Destination;

			if (_path.Count == 0)
			{
				_moveTime = TimeSpan.Zero;
				this.IsMoving = false;
				return;
			}

			this.ExecuteNextMove();
		}

		/// <summary>
		/// Updates the entity's position based on its current movement parameters.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateEntityPosition(TimeSpan elapsed)
		{
			var position = this.Entity.Position;
			position.X += (float)(_moveX * elapsed.TotalSeconds);
			position.Z += (float)(_moveZ * elapsed.TotalSeconds);

			if (!this.Entity.Map.Ground.TryGetHeightAt(position, out var height))
				height = position.Y;

			position.Y = height;

			this.Entity.Position = position;
		}

		/// <summary>
		/// Updates trigger areas and triggers relevant ones.
		/// </summary>
		private void UpdateTriggerAreas(TimeSpan elapsed)
		{
			// TODO: It's technically possible for an entity to move so
			//   quickly that they zap past a trigger area before we see
			//   it. To solve this issue we'd need to get the movement
			//   path since the last update and check if it intersects
			//   with any trigger areas. Not overly complicated, but
			//   support needs to be added to the shape classes first.

			var prevTriggerAreas = _triggerAreas;
			var triggerAreas = this.Entity.Map.GetTriggerableAreasAt(this.Entity.Position);

			if (prevTriggerAreas.Length == 0 && triggerAreas.Length == 0)
				return;

			var enteredTriggerAreas = triggerAreas.Except(prevTriggerAreas);
			var leftTriggerAreas = prevTriggerAreas.Except(triggerAreas);

			foreach (var triggerArea in enteredTriggerAreas)
				triggerArea.EnterFunc?.Invoke(new TriggerActorArgs(TriggerType.Enter, triggerArea, this.Entity));

			foreach (var triggerArea in leftTriggerAreas)
				triggerArea.LeaveFunc?.Invoke(new TriggerActorArgs(TriggerType.Leave, triggerArea, this.Entity));

			foreach (var triggerArea in triggerAreas)
				triggerArea.WhileInsideFunc?.Invoke(new TriggerActorArgs(TriggerType.Update, triggerArea, this.Entity));

			_triggerAreas = triggerAreas;
		}

		/// <summary>
		/// Sets the movement mode for attached state.
		/// </summary>
		/// <param name="usePositionOnly">If true, uses ZC_MOVE_POS instead of directional movement.</param>
		public void SetAttachmentMovementMode(bool usePositionOnly)
		{
			this.UsePositionOnlyMovement = usePositionOnly;
		}
	}

	public enum MoveTargetType
	{
		Position,
		Direction,
	}

	public enum MoveSpeedType
	{
		Walk,
		Run,
	}
}
