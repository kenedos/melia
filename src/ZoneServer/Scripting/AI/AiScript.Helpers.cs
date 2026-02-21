using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Scripting.AI
{
	public abstract partial class AiScript
	{
		private readonly Dictionary<string, DateTime> _actionTimers = new();

		/// <summary>
		/// Checks if a named action is ready to be performed, based on a cooldown.
		/// If it is ready, the timer is reset.
		/// Useful for implementing cooldown-based action gating.
		/// </summary>
		/// <param name="actionName">A unique name for the action's timer.</param>
		/// <param name="cooldown">The duration to wait before this action can be performed again.</param>
		/// <returns>True if the action can be performed, false otherwise.</returns>
		protected bool IsActionReady(string actionName, TimeSpan cooldown)
		{
			var now = DateTime.UtcNow;
			if (_actionTimers.TryGetValue(actionName, out var readyTime))
			{
				if (now >= readyTime)
				{
					_actionTimers[actionName] = now + cooldown;
					return true;
				}
				return false;
			}
			else
			{
				// First time, it's always ready.
				_actionTimers[actionName] = now + cooldown;
				return true;
			}
		}

		/// <summary>
		/// Gets the nearest hostile entity within a given range.
		/// </summary>
		/// <param name="range">The search range.</param>
		/// <returns>The nearest ICombatEntity, or null if none are found.</returns>
		protected ICombatEntity GetNearestEnemy(float range)
		{
			return this.Entity.Map
				.GetAttackableEnemiesInPosition(this.Entity, this.Entity.Position, range)
				.Where(e => this.IsHostileTowards(e) && !this.EntityGone(e))
				.OrderBy(e => this.Entity.Position.Get2DDistance(e.Position))
				.FirstOrDefault();
		}

		/// <summary>
		/// Gets the farthest hostile entity within a given range.
		/// </summary>
		/// <param name="range">The search range.</param>
		/// <returns>The farthest ICombatEntity, or null if none are found.</returns>
		protected ICombatEntity GetFarthestEnemy(float range)
		{
			return this.Entity.Map
				.GetAttackableEnemiesInPosition(this.Entity, this.Entity.Position, range)
				.Where(e => this.IsHostileTowards(e) && !this.EntityGone(e))
				.OrderByDescending(e => this.Entity.Position.Get2DDistance(e.Position))
				.FirstOrDefault();
		}

		/// <summary>
		/// Checks if the entity's HP is below a certain percentage.
		/// </summary>
		/// <param name="percent">The percentage threshold (e.g., 0.5 for 50%).</param>
		/// <returns>True if HP is at or below the threshold.</returns>
		protected bool IsHpBelow(float percent)
		{
			if (this.Entity.MaxHp == 0) return false;
			return (this.Entity.Hp / (float)this.Entity.MaxHp) <= percent;
		}

		/// <summary>
		/// Checks if the entity is near its original spawn position.
		/// </summary>
		/// <param name="range">The distance to check against.</param>
		/// <returns>True if the entity is within the specified range of its spawn point.</returns>
		protected bool IsNearSpawnPosition(float range)
		{
			if (this.Entity is not IMonster monster || monster.SpawnPosition == Position.Zero)
			{
				return true; // Not a monster or no spawn position, so it's not "far"
			}
			return monster.Position.InRange2D(monster.SpawnPosition, range);
		}
	}
}
