using System.Collections;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone;
using System;
using Melia.Zone.Skills.Handlers.Hunter;

/// <summary>
/// Basic AI for most monsters.
/// </summary>
[Ai("PC_Pet")]
public class PcPetAiScript : AiScript
{
	protected override void Setup()
	{
		this.MaxChaseDistance = 350;
		this.MaxRoamDistance = 1000;
		this.EnableReturnHome = false;

		During("Idle", CheckEnemies);
		During("Idle", CheckAggressiveMode);
		During("Idle", CheckFear);
		During("Attack", CheckTarget);
		During("Attack", CheckMaster);
		During("Attack", CheckFear);
	}

	protected override void Root()
	{
		StartRoutine("Idle", Idle());
	}

	/// <summary>
	/// Checks for nearby enemies when in aggressive mode and engages them.
	/// Prioritizes enemies closer to the master.
	/// </summary>
	protected virtual void CheckAggressiveMode()
	{
		// Only check if we're a companion and in aggressive mode
		if (this.Entity is not Companion companion || !companion.IsAggressiveMode)
			return;

		// Don't attack if we already have a target
		if (_target != null)
			return;

		// Don't attack if we can't attack
		if (this.Entity.IsLocked(LockType.Attack))
			return;

		// Don't attack if we don't have a master
		if (!this.TryGetMaster(out var master))
			return;

		// Don't aggro if we're too far from master - need to return first
		if (!this.InRangeOf(master, MaxMasterDistance))
			return;

		// Find nearby enemies within view range, sorted by distance to master
		var nearbyEnemies = this.Entity.Map.GetAttackableEnemiesInPosition(this.Entity, this.Entity.Position, _viewRange)
				.Where(e => !e.IsDead && this.IsHostileTowards(e))
				.OrderBy(e => e.Position.Get2DDistance(master.Position))
				.ToList();

		if (nearbyEnemies.Any())
		{
			var closestToMaster = nearbyEnemies.First();
			// Increase hate enough to trigger aggro
			this.IncreaseHate(closestToMaster, 150);

			// Check enemies will pick up the most hated target
			this.CheckEnemies();
		}
	}

	/// <summary>
	/// Enhanced attack that predicts enemies position if they're moving.
	/// This makes pets "smarter" as they move further in front of enemies
	/// giving enough time for their attack animation to complete before 
	/// attacking the predicted position.
	/// </summary>
	/// <returns></returns>
	protected override IEnumerable Attack()
	{
		this.SetRunning(true);

		while (!this.EntityGone(_target) && this.IsHating(_target))
		{
			// Check if too far from master - return to master immediately
			if (this.TryGetMaster(out var master))
			{
				if (this.EntityGone(master) || !this.InRangeOf(master, MaxMasterDistance))
				{
					_target = null;
					this.RemoveAllHate();
					this.StartRoutine("Idle", this.Idle());
					yield break;
				}
			}

			// Check if we can attack
			var cannotAttack = this.Entity.IsLocked(LockType.Attack);
			if (cannotAttack)
			{
				// When silenced, keep following the target but don't try skills
				var followRange = 80f;

				if (!this.InRangeOf(_target, followRange))
				{
					// Keep moving towards target
					yield return this.MoveToAttack(_target, followRange);
				}
				else
				{
					// We're close enough, just face them and wait
					yield return this.TurnTowards(_target);
				}

				// Important: yield to prevent blocking the thread
				yield return this.Wait(100, 200);
				continue;
			}

			// Try to get a skill to use
			if (!this.TryGetRandomSkill(out var skill))
			{
				// No skills available, wait a bit and retry
				yield return this.Wait(250);
				continue;
			}

			// Check master distance after waiting
			if (this.TryGetMaster(out var masterAfterWait))
			{
				if (this.EntityGone(masterAfterWait) || !this.InRangeOf(masterAfterWait, MaxMasterDistance))
				{
					_target = null;
					this.RemoveAllHate();
					this.StartRoutine("Idle", this.Idle());
					yield break;
				}
			}

			// Get attack range for the skill
			var attackRange = this.GetAttackRange(skill);

			// Try to use master's skills during chase
			this.UseSkillDuringChase(_target);

			// Check if target is moving and predict position
			var targetPos = _target.Position;
			var isTargetMoving = _target.Components.Get<MovementComponent>()?.IsMoving ?? false;

			// Check if target has fear debuff - don't predict position if they're fleeing
			var isTargetFeared = _target.IsBuffActive(BuffId.Pollution_Debuff)
				|| _target.IsBuffActive(BuffId.Fear)
				|| _target.IsBuffActive(BuffId.Growling_fear_Debuff);

			if (isTargetMoving && !isTargetFeared)
			{
				// Predict where the target will be
				var movement = _target.Components.Get<MovementComponent>();
				if (movement != null)
				{
					// Calculate prediction time based on distance
					// 1.2-1.8 seconds
					var distance = this.Entity.Position.Get2DDistance(_target.Position);
					var predictionTime = 1.2f + (distance / 150f) * 0.6f;

					// Get target's movement speed
					var targetSpeed = _target.Properties.GetFloat(PropertyName.MSPD);
					if (_target is Character)
						targetSpeed *= 2.4f; // Character speed multiplier

					// Try to get the direction based on what the enemy is chasing
					// Default to facing direction
					var predictionDirection = _target.Direction;

					// If target is a mob with AI, get their target
					if (_target is Mob mob && mob.Components.TryGet<AiComponent>(out var aiComponent))
					{
						var enemyTarget = aiComponent.Script.Target;
						if (enemyTarget != null && !enemyTarget.IsDead && this.Entity.Map == enemyTarget.Map && enemyTarget != this.Entity)
						{
							// Calculate direction from enemy to their target
							predictionDirection = _target.Position.GetDirection(enemyTarget.Position);
						}
					}

					// Calculate predicted position using the direction toward their target
					var predictionDistance = targetSpeed * predictionTime;
					var predictedPos = _target.Position.GetRelative(predictionDirection, (float)predictionDistance);

					// Validate predicted position is on walkable ground
					if (this.Entity.Map.Ground.IsValidPosition(predictedPos))
					{
						targetPos = predictedPos;
					}
				}
			}

			// Move into range if needed
			if (!this.Entity.Position.InRange2D(targetPos, attackRange))
			{
				// Check master distance BEFORE attempting movement
				if (this.TryGetMaster(out var masterBeforeMove))
				{
					if (this.EntityGone(masterBeforeMove) || !this.InRangeOf(masterBeforeMove, MaxMasterDistance))
					{
						_target = null;
						this.RemoveAllHate();
						this.StartRoutine("Idle", this.Idle());
						yield break;
					}
				}

				if (RangeType == AttackerRangeType.Melee)
				{
					// Move to predicted position for moving targets (unless feared), or current position for stationary
					if (isTargetMoving && !isTargetFeared && this.Entity.Map.Ground.IsValidPosition(targetPos))
					{
						yield return this.MoveTo(targetPos, wait: false);
					}
					else
					{
						yield return this.MoveToAttack(_target, attackRange);
					}
				}
				else if (RangeType == AttackerRangeType.Ranged)
				{
					yield return this.MoveToRangedAttack(_target, attackRange);
				}

				// After moving, re-check if target is still valid
				if (this.EntityGone(_target) || !this.IsHating(_target))
				{
					_target = null;
					if (EnableReturnHome)
						this.StartRoutine("ReturnHome", this.ReturnHome());
					else
						this.StartRoutine("Idle", this.Idle());
					yield break;
				}
			}
			// Attack only when we're at the predicted position (or close to it)
			else if (this.Entity.Position.InRange2D(targetPos, attackRange) && this.CanUseSkill(skill, _target))
			{
				yield return this.UseSkill(skill, _target);

				// After skill completes, check if we're feared
				if (this.IsFeared() && !this.IsUsingSkill())
				{
					_target = null;
					this.StartRoutine("Idle", this.Idle());
					yield break;
				}
			}

			// Small wait to avoid tight loop
			yield return true;
		}

		_target = null;
		this.CheckEnemies();

		if (_target != null)
			yield break;

		if (EnableReturnHome)
			this.StartRoutine("ReturnHome", this.ReturnHome());
		else
			this.StartRoutine("Idle", this.Idle());
		yield break;
	}

	/// <summary>
	/// Generic method to use master's skills during chase based on conditions.
	/// Can be extended to add more skills in the future.
	/// </summary>
	/// <param name="target">The target being chased</param>
	protected virtual void UseSkillDuringChase(ICombatEntity target)
	{
		if (this.Entity is not Companion companion)
			return;

		if (!this.TryGetMaster(out var master))
			return;

		var distanceToTarget = this.Entity.Position.Get2DDistance(target.Position);

		// Hunter_PetAttack when target is >50 range away
		if (distanceToTarget > 50f)
		{
			Hunter_PetAttackOverride.TryActivate(master, companion, target);
		}

		// Future skills can be added here with different conditions
		// Example: if (distanceToTarget < 50f) { SomeOtherSkill.TryActivate(...); }
	}
}
