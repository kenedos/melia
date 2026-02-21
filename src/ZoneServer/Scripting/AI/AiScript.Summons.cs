using System;
using System.Collections;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.Scripting.AI
{
	/// <summary>
	/// Summon-specific AI logic for AiScript.
	/// Contains methods for summon behavior including zombie-specific mechanics.
	/// </summary>
	public abstract partial class AiScript
	{
		protected const int DefaultMaxMasterDistance = 300;
		protected const int TeleportDistance = 200;
		protected const int DefendDistance = 100;
		protected const int IdleRadius = 35;

		/// <summary>
		/// Checks if this summon is a Bokor zombie.
		/// </summary>
		/// <returns></returns>
		protected bool IsZombie()
		{
			if (this.Entity is Mob mob)
			{
				return mob.ClassName == "summons_zombie";
			}
			return false;
		}

		/// <summary>
		/// Gets the most hated enemy, prioritizing targets according to bokor's curse
		/// </summary>
		/// <returns></returns>
		protected ICombatEntity GetMostHatedWithCurse()
		{
			var mostHated = this.GetMostHated();
			if (mostHated == null)
				return null;

			if (!this.IsZombie())
				return mostHated;

			if (mostHated.IsBuffActive(BuffId.CurseOfWeakness_Damage_Debuff))
				return mostHated;

			// Curse buff of zombies
			var cursedTargets = this.Entity.Map.GetActorsInRange<ICombatEntity>(this.Entity.Position, _viewRange, target =>
			{
				if (target == null || target.IsDead)
					return false;

				// Check if target can be attacked (not cloaked, etc.)
				if (!this.CanBeHated(target))
					return false;

				return target.IsBuffActive(BuffId.CurseOfWeakness_Damage_Debuff) && this.IsHating(target);
			});

			if (cursedTargets.Count > 0)
			{
				var closestCursed = cursedTargets.OrderBy(t => t.Position.Get2DDistance(this.Entity.Position)).FirstOrDefault();
				if (closestCursed != null)
					return closestCursed;
			}

			return mostHated;
		}

		/// <summary>
		/// Summon-specific enemy checking that prioritizes cursed targets for zombies.
		/// Called during Idle routine.
		/// </summary>
		protected void CheckEnemiesSummon()
		{
			if (this.Entity.IsLocked(LockType.Attack))
				return;

			var mostHated = this.GetMostHatedWithCurse();
			if (mostHated != null)
			{
				this._target = mostHated;
				this.StartRoutine("StopAndAttack", this.StopAndAttack());
			}
		}

		/// <summary>
		/// Summon-specific target checking.
		/// Ensures target is still valid and in range.
		/// Called during Attack routine.
		/// </summary>
		protected void CheckTargetSummon()
		{
			// Transition to idle if the target has vanished or is out of range
			if (this.EntityGone(_target) || !this.InRangeOf(_target, MaxChaseDistance) || !this.IsHating(_target))
			{
				this._target = null;
				this.StartRoutine("StopAndIdle", this.StopAndIdle());
			}
		}

		/// <summary>
		/// Summon-specific master checking.
		/// Returns to master if they go beyond the max follow distance.
		/// Called during Attack routine.
		/// </summary>
		protected void CheckMasterSummon()
		{
			if (_target == null)
				return;

			if (!this.TryGetMaster(out var master))
				return;

			// Reset aggro and return to master if they went too far
			if (this.EntityGone(master) || !this.InRangeOf(master, DefaultMaxMasterDistance))
			{
				this._target = null;
				this.RemoveAllHate();
				this.StartRoutine("StopAndIdle", this.StopAndIdle());
			}
		}

		/// <summary>
		/// Teleports the summon to the master if they're too far away (200+ range) or on a different map.
		/// Resets hate when teleporting to prevent chasing distant targets.
		/// Should be called during Follow or Idle routines.
		/// </summary>
		protected void TeleportToMasterIfTooFar()
		{
			if (!this.TryGetMaster(out var master))
				return;

			var onDifferentMap = this.Entity.Map != master.Map;
			var distance = this.Entity.Position.Get2DDistance(master.Position);

			if (onDifferentMap || distance > TeleportDistance)
			{
				if (onDifferentMap)
				{
					var currentMap = this.Entity.Map;
					if (currentMap != null && this.Entity is IMonster monster)
					{
						currentMap.RemoveMonster(monster);
						master.Map.AddMonster(monster);
					}
				}

				// Try to find a valid position near master using spiral search
				var targetPos = master.Position.GetRandomInRange2D(30, 50);
				if (!master.Map.Ground.TryGetNearestValidPosition(targetPos, out var validPos, maxDistance: 100f))
				{
					// Fallback to master's position if no valid position found
					validPos = master.Position;
				}

				this.Entity.PlayGroundEffect("F_buff_basic008_blue", 1);
				this.Entity.SetPosition(validPos);
				this.RemoveAllHate();
				this._target = null;
			}
		}

		/// <summary>
		/// Gets potential enemies based on their attack state, not just hate.
		/// Useful for aggressive summons that should engage any enemy in combat.
		/// </summary>
		/// <returns>The closest enemy in attack state within view range, or null</returns>
		protected ICombatEntity GetEnemyInAttackState()
		{
			var enemiesInAttackState = this.Entity.Map.GetActorsInRange<ICombatEntity>(this.Entity.Position, _viewRange, target =>
			{
				if (target == null || target.IsDead)
					return false;

				// Check if it's an enemy
				if (!this.Entity.CheckRelation(target, RelationType.Enemy))
					return false;

				// Check if target can be attacked (not cloaked, etc.)
				if (!this.CanBeHated(target))
					return false;

				// Check if target is in attack state
				if (!target.Components.TryGet<CombatComponent>(out var combatComponent))
					return false;

				return combatComponent.AttackState;
			});

			if (enemiesInAttackState.Count == 0)
				return null;

			// Return closest enemy
			return enemiesInAttackState.OrderBy(t => t.Position.Get2DDistance(this.Entity.Position)).FirstOrDefault();
		}

		/// <summary>
		/// Idle behavior for summons - disperses around master in a circular radius.
		/// Summons pick a random angle and maintain ~75 units distance from master.
		/// Loops continuously while master is valid.
		/// </summary>
		protected IEnumerable DispersedIdle()
		{
			this.ResetMoveSpeed();

			while (true)
			{
				var master = this.GetMaster();
				if (master == null || this.EntityGone(master))
				{
					yield return this.Wait(400, 800);
					yield break;
				}

				this.TeleportToMasterIfTooFar();

				var distance = this.Entity.Position.Get2DDistance(master.Position);

				if (distance < IdleRadius - 20 || distance > IdleRadius + 20)
				{
					var randomAngle = RandomProvider.Get().Next(360);
					var targetPos = master.Position.GetRelative(new Direction(randomAngle), IdleRadius);

					if (this.Entity.Map.Ground.IsValidPosition(targetPos))
					{
						yield return this.MoveTo(targetPos);
					}
				}

				yield return this.Animation("IDLE");
				yield return this.Wait(400, 800);
			}
		}

		/// <summary>
		/// Checks if the master is under attack and within defend distance (50 units).
		/// If so, prioritizes attacking the master's attacker.
		/// Should be called during Idle routine.
		/// </summary>
		protected void DefendMasterIfNearby()
		{
			if (this.Entity.IsLocked(LockType.Attack))
				return;

			if (!this.TryGetMaster(out var master))
				return;

			// Only defend if within 50 units
			if (!this.InRangeOf(master, DefendDistance))
				return;

			// Check if master has combat component and is being attacked
			if (!master.Components.TryGet<CombatComponent>(out var masterCombat))
				return;

			// Get master's last attacker if they're in attack state
			if (!masterCombat.AttackState)
				return;

			// Find the attacker that recently hit the master
			var attacker = masterCombat.GetTopAttackerByDamage();
			if (attacker == null || this.EntityGone(attacker))
				return;

			// Check if attacker can be targeted (not cloaked, etc.)
			if (!this.CanBeHated(attacker))
				return;

			// Check if it's a valid enemy
			if (!this.Entity.CheckRelation(attacker, RelationType.Enemy))
				return;

			// Prioritize this attacker
			this._target = attacker;
			this.IncreaseHate(attacker, 300f);
			this.StartRoutine("StopAndAttack", this.StopAndAttack());
		}

		/// <summary>
		/// Resets hate during Idle if targets are too far from master.
		/// Prevents summons from chasing targets that are beyond master's range.
		/// Called during Idle routine.
		/// </summary>
		protected void ResetDistantHateDuringIdle()
		{
			if (!this.TryGetMaster(out var master))
				return;

			var mostHated = this.GetMostHated();
			if (mostHated == null)
				return;

			// If the most hated target is too far from master, reset hate
			var distanceToMaster = mostHated.Position.Get2DDistance(master.Position);
			if (distanceToMaster > DefaultMaxMasterDistance)
			{
				this.RemoveAllHate();
				this._target = null;
			}
		}

		/// <summary>
		/// Enhanced enemy checking for summons.
		/// Combines curse priority, attack state checks, and master defense.
		/// Called during Idle routine.
		/// </summary>
		protected void CheckEnemiesEnhanced()
		{
			if (this.Entity.IsLocked(LockType.Attack))
				return;

			// Changes target to attack who's attacking the master
			// if attacker is nearby.
			if (this.TryGetMaster(out var master) && this.InRangeOf(master, DefendDistance))
			{
				if (master.Components.TryGet<CombatComponent>(out var masterCombat) && masterCombat.AttackState)
				{
					var attacker = masterCombat.GetTopAttackerByDamage();
					if (attacker != null && !this.EntityGone(attacker) && this.CanBeHated(attacker) && this.Entity.CheckRelation(attacker, RelationType.Enemy))
					{
						this._target = attacker;
						this.IncreaseHate(attacker, 100f);
						this.StartRoutine("StopAndAttack", this.StopAndAttack());
						return;
					}
				}
			}

			var mostHated = this.GetMostHatedWithCurse();
			if (mostHated != null)
			{
				this._target = mostHated;
				this.StartRoutine("StopAndAttack", this.StopAndAttack());
				return;
			}

			var enemyInCombat = this.GetEnemyInAttackState();
			if (enemyInCombat != null)
			{
				this._target = enemyInCombat;
				this.IncreaseHate(enemyInCombat, 150f);
				this.StartRoutine("StopAndAttack", this.StopAndAttack());
			}
		}
	}
}
