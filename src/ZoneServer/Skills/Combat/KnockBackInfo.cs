using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Combat
{
	public enum KnockType
	{
		Motion = 3,
		KnockDown = 4,
	}

	/// <summary>
	/// Horizontal direction in which knockdown will occur in reference
	/// to caster.
	/// </summary>
	public enum KnockDirection
	{
		TowardsTarget = 0,
		TowardsCaster = 1,
		CasterForward = 2,
		Random = 3,
	}

	/// <summary>
	/// Type of knockback distance formula used by the client.
	/// Different skills use different formulas for calculating knockback distance.
	/// </summary>
	public enum KnockBackFormulaType
	{
		/// <summary>
		/// Short formula with reduced knockback distances.
		/// Used by most other knockback skills.
		/// </summary>
		Standard = 0,

		/// <summary>
		/// Standard formula with longer knockback distances.
		/// Used by skills like RimBlow.
		/// </summary>
		Extended = 1,
	}

	/// <summary>
	/// Information about a knockback.
	/// </summary>
	public class KnockBackInfo
	{
		/// <summary>
		/// Returns the position of the attacker.
		/// </summary>
		public Position AttackerPosition { get; }

		/// <summary>
		/// Returns the position of the target before the knock back.
		/// </summary>
		public Position FromPosition { get; }

		/// <summary>
		/// Returns the position of the target after the knock back.
		/// </summary>
		public Position ToPosition { get; }

		/// <summary>
		/// Returns the direction in which the target was knocked back.
		/// </summary>
		public Direction Direction { get; }

		/// <summary>
		/// Returns the hit type of the knock back.
		/// </summary>
		public HitType HitType { get; }

		/// <summary>
		/// Returns the velocity by which the target moves back.
		/// </summary>
		public int Velocity { get; }

		/// <summary>
		/// Returns the angle at which the target was knocked back based
		/// on the direction.
		/// </summary>
		public int HAngle { get; }

		/// <summary>
		/// Returns the verticle angle used in the packets based on the skill
		/// data.
		/// </summary>
		public int VAngle { get; }

		/// <summary>
		/// Could be bounce/bound?
		/// </summary>
		public int BounceCount { get; set; }

		public float UnkFloat1 { get; set; }
		public float UnkFloat2 { get; set; }

		/// <summary>
		/// Duration of the knock back.
		/// </summary>
		/// <remarks>
		/// Affects knock downs in particular, where the target bounces
		/// off the ground a few times. If the time is not long enough,
		/// the target stops in mid-air.
		/// </remarks>
		public TimeSpan Time { get; }
		public float Speed { get; set; } = 1f;
		public float VPow { get; set; } = 1f;

		/// <summary>
		/// Creates new knock back info.
		/// </summary>
		/// <param name="attackerPosition"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		public KnockBackInfo(Position attackerPosition, ICombatEntity target, Skill skill)
			: this(attackerPosition, target, skill.Data.KnockDownHitType, skill.Data.KnockDownVelocity, skill.Data.KnockDownVAngle)
		{
		}

		/// <summary>
		/// Creates new knock back info with a specified knock direction.
		/// </summary>
		/// <param name="caster">The caster/attacker entity</param>
		/// <param name="target">The target being knocked back</param>
		/// <param name="knockDownHitType">The hit type for the knockback</param>
		/// <param name="velocity">Knockback velocity</param>
		/// <param name="vAngle">Vertical angle</param>
		/// <param name="knockDirection">Direction type for the knockback</param>
		/// <param name="formulaType">Type of knockback formula to use</param>
		public KnockBackInfo(ICombatEntity caster, ICombatEntity target, HitType knockDownHitType, int velocity, int vAngle, KnockDirection knockDirection, KnockBackFormulaType formulaType = KnockBackFormulaType.Standard)
			: this(GetKnockbackOrigin(caster, target, knockDirection), target, knockDownHitType, velocity, vAngle, formulaType)
		{
		}

		/// <summary>
		/// Calculates the knockback origin position based on the knock direction type.
		/// </summary>
		private static Position GetKnockbackOrigin(ICombatEntity caster, ICombatEntity target, KnockDirection knockDirection)
		{
			return knockDirection switch
			{
				KnockDirection.TowardsTarget => caster.Position,
				KnockDirection.TowardsCaster => target.Position.GetRelative(caster.Position.GetDirection(target.Position), 200f),
				KnockDirection.CasterForward => target.Position.GetRelative(caster.Direction.Backwards, 100f),
				KnockDirection.Random => target.Position.GetRelative(new Direction(RandomProvider.Get().NextDouble() * Math.PI * 2), 100f),
				_ => caster.Position,
			};
		}

		/// <summary>
		/// Creates new knock back info with a forced direction.
		/// All targets will be knocked in exactly this direction regardless of their position.
		/// </summary>
		/// <param name="target">The target being knocked back</param>
		/// <param name="knockDownHitType">The hit type for the knockback</param>
		/// <param name="velocity">Knockback velocity</param>
		/// <param name="vAngle">Vertical angle</param>
		/// <param name="forcedDirection">The exact direction to knock the target</param>
		/// <param name="formulaType">Type of knockback formula to use</param>
		public KnockBackInfo(ICombatEntity target, HitType knockDownHitType, int velocity, int vAngle, Direction forcedDirection, KnockBackFormulaType formulaType = KnockBackFormulaType.Standard)
			: this(target.Position.GetRelative(forcedDirection.Backwards, 100f), target, knockDownHitType, velocity, vAngle, formulaType)
		{
		}

		/// <summary>
		/// Creates new knock back info using the enhanced knockback formula.
		/// Formula derived from extensive client testing with velocities 50-400 at various VAngles.
		/// Uses velocity-dependent piecewise formulas to accurately match client physics across all ranges.
		/// Includes ground bounce energy loss and wall bounce mechanics for KnockDown hit types.
		/// </summary>
		/// <param name="attackerPosition"></param>
		/// <param name="target"></param>
		/// <param name="knockDownHitType"></param>
		/// <param name="velocity"></param>
		/// <param name="vAngle"></param>
		/// <param name="formulaType">Type of knockback formula to use (Standard for RimBlow-type, Short for most other skills)</param>
		public KnockBackInfo(Position attackerPosition, ICombatEntity target, HitType knockDownHitType, int velocity, int vAngle, KnockBackFormulaType formulaType = KnockBackFormulaType.Standard)
		{
			var targetPosition = target.Position;
			this.Direction = attackerPosition.GetDirection(targetPosition);
			this.HitType = knockDownHitType;
			this.Velocity = velocity;
			this.HAngle = (int)this.Direction.NormalDegreeAngle;
			this.VAngle = vAngle;

			this.AttackerPosition = attackerPosition;
			this.FromPosition = targetPosition;

			// Knockback distance formula based on testing
			// Different formulas are used based on HitType:
			// g ≈ 240.0 is derived from client testing data
			float distance;
			var g = 240.0f;
			var vAngleRad = MathF.PI * this.VAngle / 180f;
			if (knockDownHitType == HitType.KnockDown)
			{
				// KnockDown uses projectile motion physics: Range = v² * sin(2θ) / g
				// This gives the theoretical maximum range assuming no energy loss
				var baseDistance = (velocity * velocity * MathF.Sin(2f * vAngleRad)) / g;

				// Formula uses angle-dependent scaling:
				// - Higher angles → better bounce efficiency → higher retention
				// - Lower angles → worse bounce efficiency → steeper energy loss curve
				var sinVAngle = MathF.Sin(vAngleRad);

				// Base retention for angle (assumes good bouncing conditions)
				var bounceRetention = 0.70f + 0.25f * sinVAngle;

				// Apply velocity-dependent scaling to account for low-energy cases
				// Lower velocities at low angles don't bounce well → apply penalty
				var energyFactor = (velocity * sinVAngle) / 100f;
				var retentionPenalty = Math.Min(1.0f, energyFactor * 0.9f); // Caps at 1.0 for high energy

				var groundEnergyRetention = bounceRetention * retentionPenalty;

				// Apply energy retention to get actual knockback distance
				distance = baseDistance * groundEnergyRetention;
			}
			else
			{
				// Normal/KnockBack: The client uses different formulas depending on the skill.
				// VAngle only affects visual arc, not horizontal distance for these hit types.
				// Two main formula types exist: Standard (longer knockback) and Short (shorter knockback).

				if (formulaType == KnockBackFormulaType.Extended)
				{
					// Standard formula (RimBlow-type skills): Longer knockback distances
					// Coefficients derived from RimBlow testing data.
					if (velocity <= 100)
					{
						// Low velocity range: Near-linear scaling with slight quadratic component
						distance = velocity * (0.115f + 0.00115f * velocity);
					}
					else if (velocity <= 200)
					{
						// Mid velocity range: Standard quadratic scaling
						distance = velocity * (0.0827f + 0.00265f * velocity);
					}
					else
					{
						// High velocity range: Quadratic with damping factor
						var baseDistance = velocity * (0.0827f + 0.00265f * velocity);
						var dampingFactor = 1.0f / (1.0f + 0.002f * (velocity - 200));
						distance = baseDistance * dampingFactor;
					}
				}
				else
				{
					// Short formula (most other knockback skills): Shorter knockback distances
					// Single quadratic formula: distance = v × (0.00613 + 0.000759 × v)
					// Derived from empirical testing at velocities 60, 115, 175, 235, 305, 400.
					distance = velocity * (0.00613f + 0.000759f * velocity);
				}
			}

			// Calculate knockback duration based on projectile motion physics
			var baseTimeSeconds = 2 * velocity * MathF.Sin(vAngleRad) / g;

			// Set ToPosition with wall bounce calculation for KnockDown
			if (knockDownHitType == HitType.KnockDown)
			{
				var (finalPos, actualDistance) = this.CalculatePositionWithWallBounce(target, this.FromPosition, this.Direction, distance);
				this.ToPosition = finalPos;

				var timeMultiplier = 1.0f + MathF.Sin(vAngleRad);
				this.Time = TimeSpan.FromSeconds(Math.Max(0.6f, baseTimeSeconds * timeMultiplier));
			}
			else
			{
				this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
				this.ToPosition = target.Map.GetLastWalkablePosition(this.FromPosition, target.AgentRadius, this.ToPosition);

				var timeMultiplier = 1.0f + MathF.Sin(vAngleRad);
				this.Time = TimeSpan.FromSeconds(Math.Max(0.6f, baseTimeSeconds * timeMultiplier));
			}
		}

		/// <summary>
		/// Calculates the final knockback position with wall bouncing physics.
		/// Simulates realistic bouncing behavior when a knocked-down entity hits walls.
		/// </summary>
		/// <param name="target">The entity being knocked back</param>
		/// <param name="startPos">Starting position</param>
		/// <param name="initialDir">Initial knockback direction</param>
		/// <param name="totalDistance">Total knockback distance</param>
		/// <returns>Final position after accounting for wall bounces</returns>
		private (Position finalPos, float actualDistance) CalculatePositionWithWallBounce(ICombatEntity target, Position startPos, Direction initialDir, float totalDistance)
		{
			const int maxBounces = 3;
			const float bounceEnergyRetention = 0.25f;
			const float minBounceDistance = 30f;

			var currentPos = startPos;
			var currentDir = initialDir;
			var remainingDistance = totalDistance;
			var totalDistanceTraveled = 0f;

			for (int bounceCount = 0; bounceCount < maxBounces && remainingDistance > minBounceDistance; bounceCount++)
			{
				// Calculate target position for this segment
				var targetPos = currentPos.GetRelative(currentDir, remainingDistance);

				// Find the last walkable position (stops at walls)
				var reachablePos = target.Map.GetLastWalkablePosition(currentPos, target.AgentRadius, targetPos);

				// Calculate how far we actually traveled
				var distanceTraveled = (float)currentPos.Get2DDistance(reachablePos);
				totalDistanceTraveled += distanceTraveled;

				// If we reached our target (no wall hit), we're done
				if (reachablePos.Get2DDistance(targetPos) < 1f || distanceTraveled >= remainingDistance - 0.1f)
				{
					return (reachablePos, totalDistanceTraveled);
				}

				// We hit a wall - calculate remaining distance and bounce
				remainingDistance -= distanceTraveled;

				// Apply energy loss from the bounce
				remainingDistance *= bounceEnergyRetention;

				// If remaining distance is too small, stop here
				if (remainingDistance < minBounceDistance)
				{
					return (reachablePos, totalDistanceTraveled);
				}

				// Calculate wall normal by sampling the area around collision point
				var wallNormal = this.EstimateWallNormal(target, reachablePos, currentDir);

				// Reflect the direction off the wall
				currentDir = this.ReflectDirection(currentDir, wallNormal);

				// Update current position for next bounce
				currentPos = reachablePos;

				// Increment bounce counter for tracking
				this.BounceCount = bounceCount + 1;
			}

			return (currentPos, totalDistanceTraveled);
		}

		/// <summary>
		/// Estimates the wall normal at a collision point by sampling walkability in multiple directions.
		/// The normal points away from the wall into the walkable area.
		/// </summary>
		/// <param name="target">The entity being knocked back</param>
		/// <param name="collisionPoint">Point where wall collision occurred</param>
		/// <param name="approachDir">Direction of approach to the wall</param>
		/// <returns>Estimated wall normal direction</returns>
		private Direction EstimateWallNormal(ICombatEntity target, Position collisionPoint, Direction approachDir)
		{
			const float sampleDistance = 15f;
			const int numSamples = 8;

			// Sample points in a circle around the collision point
			var validDirX = 0f;
			var validDirZ = 0f;
			var validCount = 0;

			for (int i = 0; i < numSamples; i++)
			{
				var angle = (2 * MathF.PI * i) / numSamples;
				var sampleDir = new Direction(MathF.Cos(angle), MathF.Sin(angle));
				var samplePos = collisionPoint.GetRelative(sampleDir, sampleDistance);

				// Check if this direction leads to walkable ground
				if (target.Map.Ground.IsValidPosition(samplePos))
				{
					validDirX += sampleDir.Cos;
					validDirZ += sampleDir.Sin;
					validCount++;
				}
			}

			// If we found valid directions, normalize and return
			if (validCount > 0)
			{
				var length = MathF.Sqrt(validDirX * validDirX + validDirZ * validDirZ);
				if (length > 0.001f)
				{
					return new Direction(validDirX / length, validDirZ / length);
				}
			}

			// Fallback: reflect straight back (reverse approach direction)
			return new Direction(-approachDir.Cos, -approachDir.Sin);
		}

		/// <summary>
		/// Reflects a direction vector off a surface normal.
		/// Uses the standard reflection formula: R = I - 2(I·N)N
		/// </summary>
		/// <param name="incoming">Incoming direction</param>
		/// <param name="normal">Surface normal</param>
		/// <returns>Reflected direction</returns>
		private Direction ReflectDirection(Direction incoming, Direction normal)
		{
			var ix = incoming.Cos;
			var iz = incoming.Sin;
			var nx = normal.Cos;
			var nz = normal.Sin;

			// Calculate dot product: I·N
			var dot = ix * nx + iz * nz;

			// Reflection formula: R = I - 2(I·N)N
			var rx = ix - 2 * dot * nx;
			var rz = iz - 2 * dot * nz;

			// Normalize the result
			var length = MathF.Sqrt(rx * rx + rz * rz);
			if (length > 0.001f)
			{
				return new Direction(rx / length, rz / length);
			}

			// Fallback: return opposite direction
			return new Direction(-incoming.Cos, -incoming.Sin);
		}


		/// UNUSED - Melia's Implementation
		/// <summary>
		/// Creates new knock back info.
		/// </summary>
		/// <param name="attackerPosition"></param>
		/// <param name="target"></param>
		/// <param name="knockDownHitType"></param>
		/// <param name="velocity"></param>
		/// <param name="vAngle"></param>
		//public KnockBackInfo(Position attackerPosition, ICombatEntity target, HitType knockDownHitType, int velocity, int vAngle)
		//{
		//	var targetPosition = target.Position;
		//	this.Direction = attackerPosition.GetDirection(targetPosition);
		//	this.HitType = knockDownHitType;
		//	this.Velocity = velocity;
		//	this.HAngle = (int)this.Direction.NormalDegreeAngle;
		//	this.VAngle = vAngle;
		//
		//	this.AttackerPosition = attackerPosition;
		//	this.FromPosition = targetPosition;
		//	//this.ToPosition = this.GetNewPosition();
		//	//this.Time = this.GetTime();
		//
		//	if (target.MoveType == MoveType.Holding)
		//	{
		//		this.ToPosition = target.Position;
		//	}
		//	// Hacks until we have a working formula
		//	else if (this.Velocity == 400 && this.VAngle == 86)
		//	{
		//		var distance = 93.570992087511f;
		//
		//		this.Time = TimeSpan.FromMilliseconds(6747);
		//		this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
		//		this.ToPosition = target.Map.GetLastWalkablePosition(this.FromPosition, target.AgentRadius, this.ToPosition);
		//	}
		//	else if (this.Velocity == 250 && this.VAngle == 85) // Wagon Wheel
		//	{
		//		var distance = 30.570992087511f;
		//
		//		this.Time = TimeSpan.FromMilliseconds(4200);
		//		this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
		//		this.ToPosition = target.Map.GetLastWalkablePosition(this.FromPosition, target.AgentRadius, this.ToPosition);
		//	}
		//	else if (this.Velocity == 115 && this.VAngle == 10) // Rim Blow
		//	{
		//		// TODO: Double check this distance. In the PR it was a for loop
		//		//   over the hit count (4)?
		//		var distance = 65f;
		//
		//		this.Time = TimeSpan.FromMilliseconds(180);
		//		this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
		//		this.ToPosition = target.Map.GetLastWalkablePosition(this.FromPosition, target.AgentRadius, this.ToPosition);
		//	}
		//	else if (this.Velocity == 150 && this.VAngle == 10) // Taglio
		//	{
		//		var distance = 10;
		//
		//		this.Time = TimeSpan.FromMilliseconds(180);
		//		this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
		//		this.ToPosition = target.Map.GetLastWalkablePosition(this.FromPosition, target.AgentRadius, this.ToPosition);
		//	}
		//	else if (this.Velocity == 100 && this.VAngle == 85) // Granata
		//	{
		//		var distance = 1;
		//
		//		this.Time = TimeSpan.FromMilliseconds(180);
		//		this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
		//		this.ToPosition = target.Map.GetLastWalkablePosition(this.FromPosition, target.AgentRadius, this.ToPosition);
		//	}
		//	else if (this.Velocity == 150 && this.VAngle == 60) // Timebomb Arrow, other targets
		//	{
		//		var distance = 70;
		//
		//		this.Time = TimeSpan.FromMilliseconds(2000);
		//		this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
		//	}
		//	else if (this.Velocity == 20 && this.VAngle == 10) // Firewall
		//	{
		//		var distance = 10;
		//
		//		this.Time = TimeSpan.FromMilliseconds(180);
		//		this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
		//		this.ToPosition = target.Map.GetLastWalkablePosition(this.FromPosition, target.AgentRadius, this.ToPosition);
		//	}
		//	else
		//	{
		//		var distance = 17.700299992446f;
		//
		//		this.Velocity = 150;
		//		this.VAngle = 10;
		//		this.Time = TimeSpan.FromMilliseconds(180);
		//		this.ToPosition = this.FromPosition.GetRelative(this.Direction, distance);
		//		this.ToPosition = target.Map.GetLastWalkablePosition(this.FromPosition, target.AgentRadius, this.ToPosition);
		//	}
		//}

		/// UNUSED - Melia's Implementation
		/// <summary>
		/// Returns the position a target would be knocked back to,
		/// given the direction and velocity.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		/// <param name="velocity"></param>
		/// <returns></returns>
		/// <remarks>
		/// DEPRECATED: This method uses an outdated linear formula.
		/// Use the constructor overload with enhanced quadratic formula instead.
		/// The enhanced formula: distance = 0.0825*v + 0.0026*v²
		/// This accurately matches client behavior for VAngle 10° with error &lt; 2%
		/// </remarks>
		// public Position GetNewPosition()
		// {
		// 	// DEPRECATED: This method uses an outdated linear formula that doesn't
		// 	// accurately match client behavior. The client uses a quadratic relationship
		// 	// between velocity and distance (see enhanced constructor).
		// 	//
		// 	// Historical test data (VAngle 10, old formula):
		// 	// Velocity: 100, Expected: 34.20, Old Formula: 8.00  (76.6% error)
		// 	// Velocity: 150, Expected: 70.75, Old Formula: 18.0  (74.6% error)
		// 	// Velocity: 200, Expected: 118.35, Old Formula: 32.0 (73.0% error)
		// 	//
		// 	// The VAngle parameter significantly affects knockback distance by determining
		// 	// the vertical component of the trajectory. Higher VAngle = more vertical,
		// 	// less horizontal distance traveled.
		// 
		// 	var pos = this.FromPosition;
		// 	var dir = this.Direction;
		// 	var velocity = this.Velocity;
		// 	var unitsPerSecond = velocity;
		// 
		// 	// Old linear formula - produces significant errors
		// 	var seconds = velocity / 1000f * 0.8f;
		// 	var distance = unitsPerSecond * seconds;
		// 
		// 	Log.Debug("Velocity: {0}, Seconds: {1}, Distance: {2}", velocity, seconds, distance);
		// 
		// 	return pos.GetRelative(dir, distance);
		// }
	}
}
