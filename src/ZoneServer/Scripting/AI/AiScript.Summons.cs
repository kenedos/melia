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
	public abstract partial class AiScript
	{
		protected const int DefaultMaxMasterDistance = 300;
		protected const int TeleportDistance = 200;
		protected const int DefendDistance = 100;
		protected const int IdleRadius = 35;

		protected bool IsZombie()
		{
			if (this.Entity is Mob mob)
				return mob.ClassName == "summons_zombie";

			return false;
		}

		protected bool IsValidSummonCombatTarget(ICombatEntity target)
		{
			if (target == null)
				return false;

			if (this.Entity == null || this.Entity.Map == null)
				return false;

			if (this.EntityGone(target))
				return false;

			if (target.IsDead)
				return false;

			if (target.Map != this.Entity.Map)
				return false;

			var registeredTarget = this.Entity.Map.GetCombatEntity(target.Handle);

			if (registeredTarget == null || !ReferenceEquals(registeredTarget, target))
				return false;

			if (!this.CanBeHated(target))
				return false;

			if (target is Mob mob)
			{
				switch (mob.Rank)
				{
					case MonsterRank.MISC:
					case MonsterRank.Material:
					case MonsterRank.NPC:
					case MonsterRank.Neutral:
					case MonsterRank.Pet:
						return false;
				}
			}

			return true;
		}

		protected ICombatEntity GetMostHatedWithCurse()
		{
			var mostHated = this.GetMostHated();

			while (mostHated != null && !this.IsValidSummonCombatTarget(mostHated))
			{
				this.RemoveHate(mostHated);
				mostHated = this.GetMostHated();
			}

			if (mostHated == null)
				return null;

			if (!this.IsZombie())
				return mostHated;

			if (mostHated.IsBuffActive(BuffId.CurseOfWeakness_Damage_Debuff))
				return mostHated;

			var cursedTargets = this.Entity.Map.GetActorsInRange<ICombatEntity>(
				this.Entity.Position,
				_viewRange,
				target =>
				{
					if (!this.IsValidSummonCombatTarget(target))
						return false;

					return target.IsBuffActive(BuffId.CurseOfWeakness_Damage_Debuff)
						&& this.IsHating(target);
				}
			);

			if (cursedTargets.Count > 0)
			{
				ICombatEntity closestCursed = null;
				var closestCursedDistance = double.MaxValue;

				foreach (var target in cursedTargets)
				{
					var distance = target.Position.Get2DDistance(this.Entity.Position);

					if (distance >= closestCursedDistance)
						continue;

					closestCursedDistance = distance;
					closestCursed = target;
				}

				if (closestCursed != null)
					return closestCursed;
			}

			return mostHated;
		}

		protected void CheckEnemiesSummon()
		{
			if (this.Entity.IsLocked(LockType.Attack))
				return;

			var mostHated = this.GetMostHatedWithCurse();

			if (!this.IsValidSummonCombatTarget(mostHated))
				return;

			this._target = mostHated;
			this.StartRoutine("StopAndAttack", this.StopAndAttack());
		}

		protected void CheckTargetSummon()
		{
			if (this.IsValidSummonCombatTarget(_target)
				&& this.InRangeOf(_target, MaxChaseDistance)
				&& this.IsHating(_target))
			{
				return;
			}

			if (_target != null)
				this.RemoveHate(_target);

			this._target = null;
			this.StartRoutine("StopAndIdle", this.StopAndIdle());
		}

		protected void CheckMasterSummon()
		{
			if (_target == null)
				return;

			if (!this.TryGetMaster(out var master))
				return;

			if (this.EntityGone(master) || !this.InRangeOf(master, DefaultMaxMasterDistance))
			{
				this._target = null;
				this.RemoveAllHate();
				this.StartRoutine("StopAndIdle", this.StopAndIdle());
			}
		}

		protected void TeleportToMasterIfTooFar()
		{
			if (!this.TryGetMaster(out var master))
				return;

			var onDifferentMap = this.Entity.Map != master.Map;
			var distance = this.Entity.Position.Get2DDistance(master.Position);

			if (!onDifferentMap && distance <= TeleportDistance)
				return;

			if (onDifferentMap)
			{
				var currentMap = this.Entity.Map;

				if (currentMap != null && this.Entity is IMonster monster)
				{
					currentMap.RemoveMonster(monster);
					master.Map.AddMonster(monster);
				}
			}

			var targetPosition = master.Position.GetRandomInRange2D(30, 50);

			if (!master.Map.Ground.TryGetNearestValidPosition(
				targetPosition,
				out var validPosition,
				maxDistance: 100f))
			{
				validPosition = master.Position;
			}

			this.Entity.PlayGroundEffect("F_buff_basic008_blue", 1);
			this.Entity.SetPosition(validPosition);
			this.RemoveAllHate();
			this._target = null;
		}

		protected ICombatEntity GetEnemyInAttackState()
		{
			this.TryGetMaster(out var masterForRange);

			var searchRange = Math.Min(_viewRange, MaxChaseDistance);

			var enemiesInAttackState = this.Entity.Map.GetActorsInRange<ICombatEntity>(
				this.Entity.Position,
				searchRange,
				target =>
				{
					if (!this.IsValidSummonCombatTarget(target))
						return false;

					if (!this.Entity.CheckRelation(target, RelationType.Enemy))
						return false;

					if (!this.InRangeOf(target, MaxChaseDistance))
						return false;

					if (masterForRange != null
						&& !target.Position.InRange2D(
							masterForRange.Position,
							DefaultMaxMasterDistance))
					{
						return false;
					}

					if (!target.Components.TryGet<CombatComponent>(out var combatComponent))
						return false;

					return combatComponent.AttackState;
				}
			);

			if (enemiesInAttackState.Count == 0)
				return null;

			ICombatEntity closestEnemy = null;
			var closestEnemyDistance = double.MaxValue;

			foreach (var target in enemiesInAttackState)
			{
				var distance = target.Position.Get2DDistance(this.Entity.Position);

				if (distance >= closestEnemyDistance)
					continue;

				closestEnemyDistance = distance;
				closestEnemy = target;
			}

			return closestEnemy;
		}

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

					var targetPosition = master.Position.GetRelative(
						new Direction(randomAngle),
						IdleRadius
					);

					if (this.Entity.Map.Ground.IsValidPosition(targetPosition))
						yield return this.MoveTo(targetPosition);
				}

				yield return this.Animation("IDLE");
				yield return this.Wait(400, 800);
			}
		}

		protected void DefendMasterIfNearby()
		{
			if (this.Entity.IsLocked(LockType.Attack))
				return;

			if (!this.TryGetMaster(out var master))
				return;

			if (!this.InRangeOf(master, DefendDistance))
				return;

			if (!master.Components.TryGet<CombatComponent>(out var masterCombat))
				return;

			if (!masterCombat.AttackState)
				return;

			var attacker = masterCombat.GetTopAttackerByDamage();

			if (!this.IsValidSummonCombatTarget(attacker))
				return;

			if (!this.Entity.CheckRelation(attacker, RelationType.Enemy))
				return;

			if (!this.InRangeOf(attacker, MaxChaseDistance))
				return;

			if (!attacker.Position.InRange2D(
				master.Position,
				DefaultMaxMasterDistance))
			{
				return;
			}

			this._target = attacker;
			this.IncreaseHate(attacker, 300f);
			this.StartRoutine("StopAndAttack", this.StopAndAttack());
		}

		protected void ResetDistantHateDuringIdle()
		{
			if (!this.TryGetMaster(out var master))
				return;

			var mostHated = this.GetMostHated();

			while (mostHated != null && !this.IsValidSummonCombatTarget(mostHated))
			{
				this.RemoveHate(mostHated);

				if (this._target == mostHated)
					this._target = null;

				mostHated = this.GetMostHated();
			}

			if (mostHated == null)
				return;

			var distanceToMaster = mostHated.Position.Get2DDistance(master.Position);
			var distanceToSelf = mostHated.Position.Get2DDistance(this.Entity.Position);

			if (distanceToMaster > DefaultMaxMasterDistance || distanceToSelf > MaxChaseDistance)
			{
				this.RemoveHate(mostHated);

				if (this._target == mostHated)
					this._target = null;
			}
		}

		protected void CheckEnemiesEnhanced()
		{
			if (this.Entity.IsLocked(LockType.Attack))
				return;

			if (this.TryGetMaster(out var master)
				&& this.InRangeOf(master, DefendDistance))
			{
				if (master.Components.TryGet<CombatComponent>(out var masterCombat)
					&& masterCombat.AttackState)
				{
					var attacker = masterCombat.GetTopAttackerByDamage();

					if (this.IsValidSummonCombatTarget(attacker)
						&& this.Entity.CheckRelation(attacker, RelationType.Enemy)
						&& this.InRangeOf(attacker, MaxChaseDistance)
						&& attacker.Position.InRange2D(
							master.Position,
							DefaultMaxMasterDistance))
					{
						this._target = attacker;
						this.IncreaseHate(attacker, 100f);
						this.StartRoutine("StopAndAttack", this.StopAndAttack());
						return;
					}
				}
			}

			var mostHated = this.GetMostHatedWithCurse();

			if (this.IsValidSummonCombatTarget(mostHated))
			{
				this._target = mostHated;
				this.StartRoutine("StopAndAttack", this.StopAndAttack());
				return;
			}

			var enemyInCombat = this.GetEnemyInAttackState();

			if (!this.IsValidSummonCombatTarget(enemyInCombat))
				return;

			this._target = enemyInCombat;
			this.IncreaseHate(enemyInCombat, 150f);
			this.StartRoutine("StopAndAttack", this.StopAndAttack());
		}
	}
}
