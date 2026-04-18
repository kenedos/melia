using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.Scripting.AI
{
	public abstract partial class AiScript
	{
		protected Position GetRetreatPosition(ICombatEntity target, float idealRange)
		{
			// Get direction vector from target to self
			var retreatVec = this.Entity.Position - target.Position;
			retreatVec = retreatVec.Normalize2D();

			// Calculate retreat position at 80% of ideal range away from current position
			var retreatDistance = idealRange * 0.8f;
			var destination = this.Entity.Position + (retreatVec * retreatDistance);

			// Add a little randomness to avoid getting stuck in a back-and-forth pattern
			destination = destination.GetRandomInRange2D(10, 20);

			if (this.Entity.Map.Ground.IsValidPosition(destination))
				return destination;

			// Fallback to simpler opposite direction if first attempt is invalid
			var currentDir = this.Entity.Position.GetDirection(target.Position);
			var retreatDir = currentDir.Backwards; // Move opposite of target
			return this.Entity.Position.GetRelative(retreatDir, retreatDistance);
		}


		/// <summary>
		/// Moves entity to a random location within the given distance.
		/// </summary>
		/// <param name="min">Minimum distance to move.</param>
		/// <param name="max">Maximum distance to move.</param>
		/// <param name="wait">If true, the routine doesn't return until the destination was reached.</param>
		/// <returns></returns>
		protected IEnumerable MoveRandom(int min = 50, int max = 100, bool wait = true)
		{
			if (this.Entity.MoveType == MoveType.Holding)
				yield break;

			min = Math.Max(1, min);
			max = Math.Max(min, max);

			var radius = this.Random(min, max + 1);
			var destination = this.Entity.Position;
			var creationPos = this.CreationPosition;
			var foundValidDest = false;

			var wanderRange = Math.Max(max, _wanderRange);
			var extraRangeRate = _extraWanderRangeRate;
			var freeRoam = creationPos == null || ZoneServer.Instance.Conf.World.FreeRoamMonsters;

			for (var i = 0; i < 10; ++i)
			{
				destination = this.Entity.Position.GetRandomInRange2D(radius, RandomProvider.Get());

				// Give entities a random chance to move past their wander
				// limit, that decreases with distance, to add some
				// unpredictability. The extra range rate respresents how
				// much further past the wander limit movement is allowed.
				// For example, 0.25 means movement is allowed up to 25%
				// past the wander limit.
				if (!freeRoam)
				{
					var distance = destination.Get2DDistance(creationPos.Value);

					if (distance > wanderRange)
					{
						var chance = Math.Clamp(1 - (distance - wanderRange) / (wanderRange * extraRangeRate), 0, 1);

						if (RandomProvider.Get().NextDouble() > chance)
							continue;
					}
				}

				if (!this.Entity.Map.Ground.AnyObstacles(this.Entity.Position, destination))
				{
					foundValidDest = true;
					break;
				}
			}

			if (foundValidDest)
				yield return this.MoveStraight(destination, wait);
			else if (wait)
				yield return this.Wait(2000);

			yield break;
		}

		/// <summary>
		/// Makes the entity run away from its current target.
		/// Equivalent to Lua's BT_ACT_SELF_RUNAWAY.
		/// </summary>
		protected IEnumerable RunAwayFromTarget(float distance)
		{
			var target = this.Target;
			if (target == null || EntityGone(target))
			{
				yield break;
			}

			this.SetRunning(true);

			// Calculate a position directly opposite from the target
			var awayVector = (this.Entity.Position - target.Position).Normalize2D();
			var destination = this.Entity.Position + (awayVector * distance);

			// Add some randomness to avoid getting stuck
			destination = destination.GetRandomInRange2D(10, 20);

			// Optional: check if destination is valid
			if (!this.Entity.Map.IsWalkablePosition(destination, this.Entity.AgentRadius))
			{
				// Fallback to just moving away
				destination = this.Entity.Position + (awayVector * distance);
			}

			yield return this.MoveTo(destination);
		}

		/// <summary>
		/// Moves entity to the given destination on a path.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="wait">If true, the routine doesn't return until the destination was reached.</param>
		/// <returns></returns>
		protected IEnumerable MoveTo(Position destination, bool wait = true)
		{
			var moveTime = _movement?.MoveTo(destination) ?? TimeSpan.Zero;

			if (wait)
				yield return this.Wait(moveTime);
			else
				yield break;
		}

		/// <summary>
		/// Moves to be in attack range of an enemy. Aims one move-speed
		/// step in front of the target so the mob commits to where the
		/// player is heading instead of chasing their current position.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="attackRange"></param>
		/// <returns></returns>
		protected IEnumerable MoveToAttack(ICombatEntity target, float attackRange, Skill skill = null)
		{
			if (target == null) yield break;
			if (!this.Entity.CanMove()) yield break;
			if (this.Entity.IsLocked(LockType.Movement)) yield break;

			var rangeWithBuffer = this.Entity is Mob mob && mob.Rank == MonsterRank.Boss
				? attackRange * 0.7f
				: attackRange - 10;

			if (this.Entity.Position.InRange2D(target.Position, rangeWithBuffer))
			{
				yield return this.StopMove();
				yield break;
			}

			var shootSec = skill != null ? (float)skill.Properties.ShootTime.TotalSeconds : 0f;
			var commitRange = attackRange * 0.5f;

			// Keeps re-aiming the mob at the target as it moves, re-issuing
			// MoveStraight before the current path naturally completes so
			// the client never sees an idle gap between packets.
			Position lastDestination = default;
			var nextRepath = DateTime.MinValue;
			var arrivedInRange = false;

			while (true)
			{
				if (_movement == null) break;
				if (this.Entity.IsDead || target.IsDead || this.Entity.Map != target.Map) break;

				if (this.Entity.Position.InRange2D(target.Position, rangeWithBuffer))
				{
					arrivedInRange = true;
					break;
				}

				// Skill commit shortcut: stop the walk early once the
				// shoot-time lead is within MaxR/2, so the caller can fire.
				if (skill != null && this.IsTargetApproaching())
				{
					var leadPos = this.GetLeadPosition(target, shootSec);
					if (this.Entity.Position.InRange2D(leadPos, commitRange))
					{
						arrivedInRange = true;
						break;
					}
				}

				var now = DateTime.UtcNow;

				// Recompute the lead every ~250ms or whenever the target
				// has drifted far enough from the last destination that
				// the current path is no longer useful.
				var mobSpeed = this.Entity.Properties.GetFloat(PropertyName.MSPD);
				var distToTarget = (float)this.Entity.Position.Get2DDistance(target.Position);
				var travelSec = mobSpeed > 0f ? distToTarget / (mobSpeed * UnitsPerMspdSecond) : 0f;

				var destination = this.GetLeadPosition(target, travelSec);
				destination = this.ApplyAllySeparation(destination);

				var needsRepath = now >= nextRepath
					|| !_movement.IsMoving
					|| (float)destination.Get2DDistance(lastDestination) > 25f;

				if (needsRepath)
				{
					if (this.Entity.Map.Ground.TryGetNearestValidPosition(destination, this.Entity.AgentRadius, out var validDest, 50f))
					{
						if (_movement.MoveTo(validDest) > TimeSpan.Zero)
						{
							lastDestination = validDest;
							nextRepath = now + TimeSpan.FromMilliseconds(100);
						}
					}
				}

				yield return true;
			}

			// Only stop if we actually arrived in attack range.
			if (arrivedInRange)
				yield return this.StopMove();
		}

		/// <summary>
		/// World units covered per second, per point of MSPD. Empirically
		/// a 30 MSPD entity travels ~75 units/s, so 1 MSPD ≈ 2.5 units/s.
		/// </summary>
		private const float UnitsPerMspdSecond = 2.5f;

		/// <summary>
		/// Returns the position the target will reach in
		/// <paramref name="leadSec"/> seconds if it keeps moving in its
		/// current facing direction at its current move speed.
		/// </summary>
		/// <summary>
		/// Hard cap on how far ahead of the target the lead can project.
		/// Stops very long travel times (e.g. slow mob chasing a distant
		/// target) from extrapolating the aim point into the next zip code.
		/// </summary>
		private const float MaxLeadDistance = 100f;

		/// <summary>
		/// Hard cap on lead horizon. If the mob can't reach the predicted
		/// spot within this many seconds, the prediction is abandoned.
		/// </summary>
		private const float MaxLeadSeconds = 0.75f;

		private Position GetLeadPosition(ICombatEntity target, float leadSec)
		{
			if (leadSec <= 0f) return target.Position;

			if (target.Components.TryGet<MovementComponent>(out var movement) && !movement.IsMoving)
				return target.Position;

			var targetSpeed = target.Properties.GetFloat(PropertyName.MSPD);
			var distance = targetSpeed * UnitsPerMspdSecond * leadSec;
			if (distance > MaxLeadDistance) distance = MaxLeadDistance;

			var leadPos = target.Position.GetRelative(target.Direction, distance);

			// Give up on the prediction if the mob itself can't reach the
			// lead point within MaxLeadSeconds
			var mobSpeed = this.Entity.Properties.GetFloat(PropertyName.MSPD);
			if (mobSpeed > 0f)
			{
				var mobDistToLead = (float)this.Entity.Position.Get2DDistance(leadPos);
				var mobTravelSec = mobDistToLead / (mobSpeed * UnitsPerMspdSecond);
				if (mobTravelSec > MaxLeadSeconds) return target.Position;
			}

			return leadPos;
		}

		/// <summary>
		/// Minimum spacing mobs try to keep from their allies while
		/// approaching a target. Purely cosmetic — stops packs from
		/// clumping onto the exact same destination.
		/// </summary>
		private const float MinAllySeparation = 15f;

		/// <summary>
		/// Nudges <paramref name="destination"/> away from any allied mob
		/// within <see cref="MinAllySeparation"/> so mobs don't stack on
		/// top of each other. Contribution from each neighbor falls off
		/// linearly with distance.
		/// </summary>
		private Position ApplyAllySeparation(Position destination)
		{
			if (this.Entity is not Mob selfMob) return destination;

			var selfHandle = selfMob.Handle;
			var self = this.Entity;

			// Repel from allies near the destination itself (not just near
			// self) so predicted attack spots don't stack on top of each
			// other — match against each ally's final destination if they
			// have one, otherwise their current position.
			var allies = this.Entity.Map.GetActorsInRange<Mob>(destination, MinAllySeparation,
				m => m.Handle != selfHandle && !m.IsDead && self.IsAlly(m));

			if (allies.Count == 0) return destination;

			float px = 0, pz = 0;
			foreach (var a in allies)
			{
				var allyPos = a.Components.TryGet<MovementComponent>(out var am) && am.IsMoving
					? am.FinalDestination
					: a.Position;

				var dist = (float)destination.Get2DDistance(allyPos);
				if (dist <= 0.01f) continue;
				if (dist >= MinAllySeparation) continue;

				var away = (destination - allyPos).Normalize2D();
				var weight = 1f - (dist / MinAllySeparation);
				px += away.X * weight;
				pz += away.Z * weight;
			}

			return destination + new Position(px * MinAllySeparation, 0, pz * MinAllySeparation);
		}

		/// <summary>
		/// Walks to a position one move-speed step in front of the target
		/// so the cast resolves on where the target is heading rather than
		/// where they currently stand.
		/// </summary>
		protected IEnumerable PreCastLunge(Skill skill, float maxAttackRange)
		{
			if (_target == null || skill == null) yield break;
			if (this.RangeType != AttackerRangeType.Melee) yield break;
			if (!this.Entity.CanMove() || this.Entity.IsLocked(LockType.Movement)) yield break;

			// Already in range — don't walk, just let the caller cast.
			if (this.Entity.Position.InRange2D(_target.Position, maxAttackRange))
				yield break;

			// Predicted hit position (shoot-time lead) is already within
			// MaxR/2 of where we stand, so the cast will land. Skip the
			// walk entirely and let the caller fire.
			var shootSec = (float)skill.Properties.ShootTime.TotalSeconds;
			var shootLeadPos = this.GetLeadPosition(_target, shootSec);
			if (this.Entity.Position.InRange2D(shootLeadPos, maxAttackRange * 0.5f))
				yield break;

			// Otherwise lead by mob travel time + skill shoot time so the
			// hit lands where the target will be when the cast resolves.
			var mobSpeed = this.Entity.Properties.GetFloat(PropertyName.MSPD);
			var distToTarget = (float)this.Entity.Position.Get2DDistance(_target.Position);
			var travelSec = mobSpeed > 0f ? distToTarget / (mobSpeed * UnitsPerMspdSecond) : 0f;
			var leadSec = travelSec + shootSec;

			var destination = this.GetLeadPosition(_target, leadSec);

			if (!this.Entity.Map.Ground.TryGetNearestValidPosition(destination, this.Entity.AgentRadius, out var validDest, 50f))
				yield break;

			var estimatedTime = _movement?.MoveStraight(validDest) ?? TimeSpan.Zero;
			if (estimatedTime <= TimeSpan.Zero) yield break;

			var deadline = DateTime.UtcNow + estimatedTime + TimeSpan.FromMilliseconds(200);
			while (_movement != null && _movement.IsMoving && DateTime.UtcNow < deadline)
			{
				// Target came into range during the walk — stop and cast now
				// so we don't waste time arriving at a spot we no longer need.
				if (this.Entity.Position.InRange2D(_target.Position, maxAttackRange))
				{
					yield return this.StopMove();
					yield break;
				}

				yield return true;
			}
		}

		/// <summary>
		/// Uses MoveToAttack while trying to keep further away range
		/// </summary>
		/// <param name="target"></param>
		/// <param name="idealRange"></param>
		/// <returns></returns>
		protected IEnumerable MoveToRangedAttack(ICombatEntity target, float idealRange)
		{
			var currentDist = this.Entity.Position.Get2DDistance(target.Position);

			if (currentDist > idealRange * 1.2f)
			{
				// Approach if too far
				yield return this.MoveToAttack(target, idealRange);
			}
			else if (currentDist < idealRange * 0.8f)
			{
				// Retreat if too close
				var retreatPos = this.GetRetreatPosition(target, idealRange);
				yield return this.MoveTo(retreatPos);
			}
			else
			{
				// Strafe sideways while maintaining range
				var currentDir = this.Entity.Position.GetDirection(target.Position);
				var strafeDir = RandomProvider.Get().Next(2) == 0
					? currentDir.Left
					: currentDir.Right;
				var strafePos = this.Entity.Position.GetRelative(strafeDir, idealRange / 2);
				yield return this.MoveTo(strafePos);
			}
		}

		/// <summary>
		/// Moves entity to the given destination in a straight line.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="wait"></param>
		/// <returns></returns>
		protected IEnumerable MoveStraight(Position destination, bool wait = true)
		{
			var moveTime = _movement?.MoveStraight(destination) ?? TimeSpan.Zero;

			if (wait)
				yield return this.Wait(moveTime);
			else
				yield break;
		}

		/// <summary>
		/// Stops entity movement.
		/// </summary>
		/// <returns></returns>
		protected IEnumerable StopMove()
		{
			_movement?.Stop();
			yield break;
		}

		/// <summary>
		/// Makes entity turn towards the given actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <returns></returns>
		protected IEnumerable TurnTowards(IActor actor)
		{
			if (this.Entity is Mob mob && !mob.Data.CanRotate)
				yield break;

			this.Entity.TurnTowards(actor);
			yield break;
		}

		/// <summary>
		/// Makes entity turn towards the given position.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		protected IEnumerable TurnTowards(Position pos)
		{
			if (this.Entity is Mob mob && !mob.Data.CanRotate)
				yield break;

			this.Entity.TurnTowards(pos);
			yield break;
		}

		/// <summary>
		/// Makes entity say the given message.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		protected IEnumerable Say(string message)
		{
			if (this.Entity.IsLocked(LockType.Speak))
				yield break;

			Send.ZC_CHAT(this.Entity, message);
		}

		/// <summary>
		/// Makes entity use the given emoticon.
		/// </summary>
		/// <param name="packetString"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		protected IEnumerable Emoticon(string packetString)
		{
			Send.ZC_SHOW_EMOTICON(this.Entity, packetString, TimeSpan.FromSeconds(2));
			yield break;
		}

		/// <summary>
		/// Returns a random skill the entity can use.
		/// </summary>
		/// <remarks>
		/// The chance for each skill to be returned is determined by its
		/// probability. These are equal by default, except for the
		/// default skill, which has a higher chance. The probabilities
		/// can be modified using <see cref="SetRandomSkill"/>.
		/// </remarks>
		/// <param name="skill"></param>
		/// <returns></returns>
		protected virtual bool TryGetRandomSkill(out Skill skill)
		{
			skill = null;

			if (this.Entity is not Mob mob)
			{
				Log.Error($"TryGetRandomSkill: Failed for {this.Entity.Name} which is not a mob.");
				return false;
			}

			if (!this.Entity.Components.TryGet<BaseSkillComponent>(out var skills))
				return false;

			if (mob.Data.Skills.Count == 0 && skills.Count == 0)
			{
				return false;
			}

			var possibleSkills = new List<SkillId>();
			foreach (var a in mob.Data.Skills)
				if (!this.Entity.IsOnCooldown(a.SkillId))
					possibleSkills.Add(a.SkillId);
			if (possibleSkills.Count == 0)
				return false;
			var rndSkillId = possibleSkills.Random();

			if (!skills.Has(rndSkillId))
			{
				var skillId = rndSkillId;
				if (Versions.Protocol < 500)
					skillId -= 100000;
				skill = new Skill(this.Entity, skillId, 1);
				skills.AddSilent(skill);
			}
			else
				skills.TryGet(rndSkillId, out skill);
			return true;
		}

		/// <summary>
		/// Makes entity use the given skill on the target. This routine is robust and handles state management.
		/// </summary>
		protected virtual IEnumerable UseSkill(Skill skill, ICombatEntity target, TimeSpan delay = default)
		{
			// Track when we start using a skill for fear behavior timing
			_lastSkillUseTime = DateTime.UtcNow;
			// Track the skill's duration to prevent interruption during animation
			_lastSkillDuration = (delay == default) ? skill.Properties.ShootTime : delay;

			yield return this.StopMove();

			if (!this.CanUseSkill(skill, target))
			{
				Send.ZC_SKILL_DISABLE(this.Entity);
				yield break;
			}

			if (!(this.Entity is Mob mob && !mob.Data.CanRotate))
				this.Entity.TurnTowards(target);

			var skillId = skill.Id;
			if (Versions.Client == KnownVersions.ClosedBeta1)
				skillId += 100000;

			var skillUsedSuccessfully = false;

			// Standard monster skill handling
			if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<ITargetSkillHandler>(skillId, out var handler))
			{
				Log.Warning($"AiScript: No handler found for skill '{skillId}'.");
			}
			else
			{
				if (this.Entity.Components.TryGet<BaseSkillComponent>(out var skillComponent))
					skillComponent.UseSkill(skill.Id);

				handler.Handle(skill, this.Entity, target);
			}
			skillUsedSuccessfully = true;

			if (skillUsedSuccessfully)
			{
				// Record skill history for condition checks
				_lastUsedSkill = skill.Id;
				_usedSkillHistory.Insert(0, skill.Id);
				if (_usedSkillHistory.Count > 10) // Limit history size
				{
					_usedSkillHistory.RemoveAt(_usedSkillHistory.Count - 1);
				}
			}

			// --- Perform the intentional wait ---
			// Wait while casting, but break early if interrupted
			var useTime = (delay == default) ? skill.Properties.ShootTime : delay;
			if (useTime > TimeSpan.Zero)
			{
				var waitEnd = DateTime.Now + useTime;
				while (DateTime.Now < waitEnd)
				{
					// If the cast was interrupted, stop waiting immediately
					if (skill.Vars.GetBool("Melia.MonsterCastInterrupted"))
						break;

					yield return this.Wait(TimeSpan.FromMilliseconds(100));
				}
			}
		}

		/// <summary>
		/// Halts execution of the current routine until the entity is no longer using a skill.
		/// </summary>
		protected IEnumerable WaitSkillEnd()
		{
			// Wait for 2 frames to ensure casting state has been set
			yield return true;
			yield return true;

			while (this.IsUsingSkill())
			{
				yield return true;
			}
		}

		/// <summary>
		/// A routine that waits until the entity has stopped moving.
		/// </summary>
		protected IEnumerable WaitMoveEnd()
		{
			// Give movement a moment to start
			yield return this.Wait(100);

			var movement = _movement;
			if (movement == null)
			{
				yield break;
			}

			while (movement.IsMoving)
			{
				yield return true; // Wait for the next tick
			}
		}

		/// <summary>
		/// Makes the entity play a cosmetic effect at its location.
		/// Equivalent to BT_PLAY_EFT.
		/// </summary>
		protected IEnumerable PlayEffect(string effectName, float scale = 1.0f, EffectLocation attachNode = EffectLocation.Top)
		{
			Log.Debug($"AiScript '{this.Entity.Name}' playing effect '{effectName}'.");
			this.Entity.PlayEffect(effectName, scale, heightOffset: attachNode);
			yield break;
		}

		/// <summary>
		/// Heals the entity for a specific amount.
		/// </summary>
		protected IEnumerable HealSelf(int amount)
		{
			this.Entity.Heal(amount, 0);
			yield break;
		}

		/// <summary>
		/// Gets the attack range of a skill considering the entity's
		/// agent radius
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		protected float GetAttackRange(Skill skill)
		{
			return this.Entity.AgentRadius + skill.GetAttackRange();
		}

		/// <summary>
		/// Makes entity play the given animation.
		/// </summary>
		/// <param name="packetString"></param>
		/// <returns></returns>
		protected IEnumerable Animation(string packetString)
		{
			Send.ZC_PLAY_ANI(this.Entity, packetString);
			yield break;
		}

		/// <summary>
		/// Makes entity keep following the given target.
		/// This routine will run until the target is dead, the entity is dead,
		/// or the target is no longer on the same map.
		/// </summary>
		/// <param name="followTarget">The target to follow.</param>
		/// <param name="minDistance">The minimum distance to the target the AI attempts to stay in.</param>
		/// <param name="maxFollowDistance">The maximum distance before the AI gives up or teleports.</param>
		/// <param name="matchSpeed">If true, the entity's speed will be changed to match the target's.</param>
		/// <returns></returns>
		protected IEnumerable Follow(ICombatEntity followTarget, float minDistance = 50, float maxFollowDistance = 1000, bool matchSpeed = false)
		{
			if (followTarget == null)
			{
				Log.Warning($"AI for '{this.Entity.Name}' was told to follow a null target.");
				yield break; // Exit immediately if target is null
			}

			var movement = _movement;
			var targetWasInRange = false;

			if (matchSpeed)
			{
				var targetMspd = followTarget.Properties.GetFloat(PropertyName.MSPD);
				if (followTarget is Character)
					targetMspd *= 2.4f;
				this.SetFixedMoveSpeed(targetMspd);
			}
			else
			{
				this.SetRunning(true);
			}

			// This is now a safe loop with multiple exit conditions.
			while (!this.Entity.IsDead && !followTarget.IsDead && this.Entity.Map == followTarget.Map)
			{
				// Check for movement locks (e.g., stunned, casting)
				if (this.Entity.IsLocked(LockType.Movement))
				{
					// If locked, just wait and re-evaluate next tick.
					yield return this.Wait(100);
					continue;
				}

				var distance = this.Entity.Position.Get2DDistance(followTarget.Position);

				// Condition 1: Target is too far away (teleport or give up)
				if (distance > maxFollowDistance)
				{
					// Option A: Teleport to target
					movement?.Stop();
					this.Entity.Position = followTarget.Position.GetRandomInRange2D((int)minDistance / 2, RandomProvider.Get()); // Teleport nearby, not directly on top
					Send.ZC_SET_POS(this.Entity);
					yield return this.Wait(250); // Small delay after teleport to re-orient.
					continue; // Continue the loop from the new position
				}

				// Condition 2: Target is within comfortable follow distance, but outside minDistance.
				var targetIsMoving = followTarget.Components.Get<MovementComponent>()?.IsMoving ?? false;
				var shouldChase = distance > minDistance || (targetIsMoving && distance > minDistance * 0.5f);

				if (shouldChase)
				{
					// If we are chasing, move towards the target.
					// The MoveTo routine is itself a coroutine that handles pathing.
					// We call it without waiting (wait: false) so we can re-evaluate every tick.
					yield return this.MoveTo(followTarget.Position, wait: false);
				}
				else
				{
					// Target is close enough and not moving away, so we stop.
					if (movement?.IsMoving ?? false)
					{
						yield return this.StopMove();
					}
				}

				// Yield control back to the AI scheduler for one frame.
				yield return true;
			}

			// If the loop exits, it means one of the conditions failed (target died, etc.)
			Log.Debug($"'{this.Entity.Name}' is stopping its Follow routine for target '{followTarget.Name}'.");
			this.SetRunning(false); // Revert speed
			yield return this.StopMove();
		}

		/// <summary>
		/// A complex routine demonstrating how to combine state changes, movement, and skill usage.
		/// </summary>
		protected IEnumerable FlyAndAttack(Skill skill, ICombatEntity target, float flyHeight)
		{
			if (!this.CanUseSkill(skill, target))
			{
				yield break;
			}

			var movement = _movement;
			if (movement == null)
			{
				yield break;
			}

			var originalSpeed = this.Entity.Properties.GetFloat(PropertyName.MSPD);
			var originalFlyState = movement.IsFlying;

			try
			{
				// --- Setup Phase ---
				Log.Debug($"AiScript '{this.Entity.Name}' starting FlyAndAttack sequence.");
				// movement.SetFlying(true, flyHeight); // Assuming this method exists on MovementComponent
				this.SetFixedMoveSpeed(originalSpeed * 1.5f); // Increase speed while flying
				yield return this.Wait(100); // Wait for state to apply

				// --- Movement Phase ---
				yield return this.MoveTo(target.Position);
				yield return this.WaitMoveEnd(); // Wait until we've arrived at the target

				// --- Attack Phase ---
				// Re-check target after moving
				if (this.EntityGone(target) || !this.CanUseSkill(skill, target))
				{
					Log.Debug($"AiScript '{this.Entity.Name}' target invalid after flying. Aborting attack.");
					yield break;
				}

				yield return this.UseSkill(skill, target);

				// --- Landing & Cooldown Phase ---
				Log.Debug($"AiScript '{this.Entity.Name}' landing after attack.");
				movement.NotifyFlying(false);
				yield return this.Wait(500);

				// Example of applying a self-debuff after a powerful move
				this.Entity.StartBuff(BuffId.Stun, duration: TimeSpan.FromSeconds(2));
			}
			finally
			{
				// --- Cleanup Phase (always runs) ---
				Log.Debug($"AiScript '{this.Entity.Name}' cleaning up from FlyAndAttack.");
				movement.NotifyFlying(originalFlyState);
				this.SetFixedMoveSpeed(originalSpeed);
				this.ResetMoveSpeed(); // Resets fixed speed
			}
		}
	}
}
