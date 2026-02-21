using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// Tracks attachment state for entities that can be attached to other entities.
	/// Used for:
	/// - HangingShot: Character hangs from hawk, character controls movement
	/// - Riding: Character mounts companion, companion controls movement
	/// </summary>
	public class AttachmentComponent : CombatEntityComponent, IUpdateable
	{
		/// <summary>
		/// The entity this entity is attached to.
		/// </summary>
		public ICombatEntity AttachedTo { get; private set; }

		/// <summary>
		/// The bone/node name the entity is attached to.
		/// </summary>
		public string AttachmentNode { get; private set; }

		/// <summary>
		/// Vertical offset from the attachment node.
		/// </summary>
		public float AttachmentOffset { get; private set; }

		/// <summary>
		/// Returns whether the entity is currently attached to something.
		/// </summary>
		public bool IsAttached => this.AttachedTo != null;

		/// <summary>
		/// The type of attachment (determines movement behavior).
		/// </summary>
		public AttachmentType Type { get; private set; }

		/// <summary>
		/// Whether the attached entity controls movement (true) or follows (false).
		/// For HangingShot: Character is controller (true), hawk follows
		/// For Riding: Companion is controller (true), character follows
		/// </summary>
		public bool IsController { get; private set; }

		/// <summary>
		/// Whether to sync direction with the attached entity.
		/// </summary>
		public bool SyncDirection { get; private set; }

		/// <summary>
		/// Angle offset in radians for Formation attachment (relative to leader's facing).
		/// </summary>
		public float FormationAngle { get; set; }

		/// <summary>
		/// Distance offset for Formation attachment (world units from leader).
		/// </summary>
		public float FormationDistance { get; set; }

		/// <summary>
		/// Last known position of the controller, used for change detection.
		/// </summary>
		private Position _lastControllerPosition;

		/// <summary>
		/// Creates new attachment component.
		/// </summary>
		/// <param name="entity"></param>
		public AttachmentComponent(ICombatEntity entity) : base(entity)
		{
		}

		/// <summary>
		/// Attaches this entity to another entity.
		/// </summary>
		/// <param name="target">Entity to attach to.</param>
		/// <param name="nodeName">Bone/node name for attachment.</param>
		/// <param name="offset">Vertical offset from node.</param>
		/// <param name="type">Type of attachment.</param>
		/// <param name="isController">Whether this entity controls movement.</param>
		/// <param name="syncDirection">Whether to sync direction.</param>
		/// <param name="sendPackets">Whether to send attachment packets. Set to false if caller handles packets manually.</param>
		public void AttachTo(ICombatEntity target, string nodeName, float offset, AttachmentType type, bool isController = false, bool syncDirection = false, bool sendPackets = true)
		{
			if (target == null)
				throw new ArgumentNullException(nameof(target));

			this.AttachedTo = target;
			this.AttachmentNode = nodeName;
			this.AttachmentOffset = offset;
			this.Type = type;
			this.IsController = isController;
			this.SyncDirection = syncDirection;

			// Initialize position tracking
			if (isController)
				_lastControllerPosition = this.Entity.Position;
			else
				_lastControllerPosition = target.Position;

			// Send attachment packets if requested
			if (sendPackets)
				this.SendAttachmentUpdate();
		}

		/// <summary>
		/// Detaches this entity from its current attachment.
		/// </summary>
		/// <param name="sendPackets">Whether to send detachment packets. Set to false if caller handles packets manually.</param>
		public void Detach(bool sendPackets = true)
		{
			if (!this.IsAttached)
				return;

			var previousAttachment = this.AttachedTo;
			var previousType = this.Type;

			this.AttachedTo = null;
			this.AttachmentNode = null;
			this.AttachmentOffset = 0;
			this.Type = AttachmentType.None;
			this.IsController = false;
			this.SyncDirection = false;
			this.FormationAngle = 0;
			this.FormationDistance = 0;

			// Send detachment packets if requested
			if (sendPackets)
				this.SendDetachmentUpdate(previousAttachment, previousType);
		}

		/// <summary>
		/// Updates attachment state and position synchronization.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			if (!this.IsAttached)
				return;

			// Check if attached entity is still valid
			if (this.AttachedTo == null || this.AttachedTo.IsDead)
			{
				this.Detach();
				return;
			}

			// Sync position based on attachment type and controller status
			switch (this.Type)
			{
				case AttachmentType.FlyWith:
					this.UpdateFlyWithPosition();
					break;

				case AttachmentType.Ride:
					this.UpdateRidePosition();
					break;

				case AttachmentType.Control:
					// Controller updates the target, not itself
					break;

				case AttachmentType.Formation:
					// Position is managed by the pad handler's Updated
					// event to stay in sync with the pad's trigger area.
					break;
			}
		}

		/// <summary>
		/// Updates position for FlyWith attachment (HangingShot style).
		/// Controller (character) moves, follower (hawk) follows.
		/// </summary>
		private void UpdateFlyWithPosition()
		{
			if (this.AttachedTo == null)
				return;

			if (this.IsController)
			{
				// This entity controls movement
				// Check if we moved and update the attached entity
				if (this.Entity.Position != _lastControllerPosition)
				{
					var newPos = this.Entity.Position;

					// Update attached entity's position (with offset for vertical difference)
					this.AttachedTo.Position = new Position(
						newPos.X,
						newPos.Y - this.AttachmentOffset,
						newPos.Z
					);

					if (this.SyncDirection)
						this.AttachedTo.Direction = this.Entity.Direction;

					_lastControllerPosition = newPos;
				}
			}
			else
			{
				// This entity follows the target
				var targetPos = this.AttachedTo.Position;
				var newPos = new Position(targetPos.X, targetPos.Y + this.AttachmentOffset, targetPos.Z);

				if (this.Entity.Position != newPos)
				{
					this.Entity.Position = newPos;

					if (this.SyncDirection)
						this.Entity.Direction = this.AttachedTo.Direction;
				}
			}
		}

		/// <summary>
		/// Updates position for Ride attachment (mount style).
		/// Controller (companion) moves, follower (character) follows.
		/// </summary>
		private void UpdateRidePosition()
		{
			if (this.AttachedTo == null)
				return;

			if (this.IsController)
			{
				// This entity (companion) controls movement
				// Check if we moved and update the rider
				if (this.Entity.Position != _lastControllerPosition)
				{
					var newPos = this.Entity.Position;

					// Update rider's position
					this.AttachedTo.Position = newPos;

					if (this.SyncDirection)
						this.AttachedTo.Direction = this.Entity.Direction;

					_lastControllerPosition = newPos;
				}
			}
			else
			{
				// This entity (character) follows the mount
				var targetPos = this.AttachedTo.Position;

				if (this.Entity.Position != targetPos)
				{
					this.Entity.Position = targetPos;

					if (this.SyncDirection)
						this.Entity.Direction = this.AttachedTo.Direction;
				}
			}
		}

		/// <summary>
		/// Updates position for Formation attachment (Centurion style).
		/// Follower stays at a fixed angle/distance offset from the leader,
		/// rotating with the leader's facing direction.
		/// </summary>
		private void UpdateFormationPosition()
		{
			if (this.AttachedTo == null)
				return;

			var leader = this.AttachedTo;
			var leaderAngle = leader.Direction.RadianAngle;
			var worldAngle = leaderAngle + this.FormationAngle;

			var offsetX = (float)Math.Cos(worldAngle) * this.FormationDistance;
			var offsetZ = (float)Math.Sin(worldAngle) * this.FormationDistance;

			var newPos = new Position(
				leader.Position.X + offsetX,
				leader.Position.Y,
				leader.Position.Z + offsetZ
			);

			if (this.Entity.Position != newPos)
			{
				this.Entity.Position = newPos;

				if (this.SyncDirection)
					this.Entity.Direction = leader.Direction;
			}
		}

		/// <summary>
		/// Called when the attached entity (this one) moves.
		/// Updates the target entity's position accordingly.
		/// Only used when this entity is the controller.
		/// </summary>
		/// <param name="newPosition"></param>
		public void OnAttachedEntityMoved(Position newPosition)
		{
			if (!this.IsAttached || !this.IsController)
				return;

			_lastControllerPosition = newPosition;

			switch (this.Type)
			{
				case AttachmentType.FlyWith:
					// HangingShot: character moved, update hawk position
					this.AttachedTo.Position = new Position(
						newPosition.X,
						newPosition.Y - this.AttachmentOffset,
						newPosition.Z
					);
					break;

				case AttachmentType.Ride:
					// Riding: companion moved, update character position
					this.AttachedTo.Position = newPosition;
					break;
			}

			if (this.SyncDirection)
				this.AttachedTo.Direction = this.Entity.Direction;
		}

		/// <summary>
		/// Sends packets to update attachment state on clients.
		/// </summary>
		private void SendAttachmentUpdate()
		{
			switch (this.Type)
			{
				case AttachmentType.FlyWith:
					// HangingShot style - character attached to hawk
					if (this.Entity is Character character && this.AttachedTo is Companion companion)
					{
						Send.ZC_NORMAL.FlyWithObject(character, companion, this.AttachmentNode, (int)(-this.AttachmentOffset));
						Send.ZC_NORMAL.ControlObject(character, companion, ControlLookType.None, false, false, null, true);
					}
					break;

				case AttachmentType.Ride:
					// Riding is handled by RidePet packet in the buff handler
					break;
			}
		}

		/// <summary>
		/// Sends packets to update detachment state on clients.
		/// </summary>
		private void SendDetachmentUpdate(ICombatEntity previousAttachment, AttachmentType previousType)
		{
			switch (previousType)
			{
				case AttachmentType.FlyWith:
					if (this.Entity is Character character)
					{
						Send.ZC_NORMAL.FlyWithObject(character, null);
						Send.ZC_NORMAL.ControlObject(character, null, ControlLookType.SameDirection, true, true, "None", true);

						if (previousAttachment is Companion companion)
						{
							Send.ZC_MOVE_ANIM(companion, FixedAnimation.EMPTY, 0);
						}
					}
					break;

				case AttachmentType.Ride:
					// Riding detachment is handled by RidePet packet in the buff handler
					break;
			}
		}
	}

	/// <summary>
	/// Types of entity attachment.
	/// </summary>
	public enum AttachmentType
	{
		/// <summary>
		/// No attachment.
		/// </summary>
		None,

		/// <summary>
		/// Flying with another entity (HangingShot style).
		/// The attached entity controls movement, target follows.
		/// </summary>
		FlyWith,

		/// <summary>
		/// Riding another entity (mount style).
		/// The mount controls movement, rider follows.
		/// </summary>
		Ride,

		/// <summary>
		/// Controlling another entity's position directly.
		/// </summary>
		Control,

		/// <summary>
		/// Formation attachment (Centurion style).
		/// The participant follows at a fixed angle/distance offset
		/// from the controller (formation leader).
		/// </summary>
		Formation,
	}

	/// <summary>
	/// Look/direction synchronization types for ControlObject.
	/// </summary>
	public enum ControlLookType
	{
		None = 0,
		SameDirection = 1,
		LookAtTarget = 2
	}
}
