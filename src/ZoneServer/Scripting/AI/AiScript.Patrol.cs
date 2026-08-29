using System;
using System.Collections;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Patrols;

namespace Melia.Zone.Scripting.AI
{
	public abstract partial class AiScript
	{
		private const float StalledLegDistance = 10;
		private const int StalledLegDelay = 1000;

		private const float PatrolAlertRange = 500;
		private const float PatrolAlertHate = 200;

		private static readonly TimeSpan PatrolAlertCooldown = TimeSpan.FromSeconds(1);

		private const float FormationSpacing = 25;
		private const float FormationArriveDistance = 30;
		private const float FormationCatchUpDistance = 60;

		private PatrolRoute _patrolRoute;
		private int[] _patrolVisits;
		private int _patrolNodeIndex;
		private int _patrolPreviousIndex = -1;
		private int _patrolFormationSlot;
		private DateTime _lastPatrolAlertTime = DateTime.MinValue;

		/// <summary>
		/// Returns true if the entity was already considered for a
		/// patrol route, whether it got one or not.
		/// </summary>
		public bool PatrolConsidered { get; private set; }

		/// <summary>
		/// Returns true if the entity walks a patrol route.
		/// </summary>
		public bool HasPatrolRoute => _patrolRoute != null;

		/// <summary>
		/// Returns true if the entity walks in a patrol group's
		/// formation.
		/// </summary>
		public bool HasPatrolFormation => _patrolFormationSlot > 0;

		/// <summary>
		/// Returns the entity's slot in its leader's formation, counting
		/// from one, or zero if it has none.
		/// </summary>
		public int PatrolFormationSlot => _patrolFormationSlot;

		/// <summary>
		/// Puts the entity in the given slot of its leader's formation,
		/// counting from one.
		/// </summary>
		/// <param name="slot"></param>
		public void SetPatrolFormationSlot(int slot)
		{
			this.PatrolConsidered = true;

			_patrolFormationSlot = slot;
		}

		/// <summary>
		/// Puts the entity on the given patrol route, or marks it as
		/// considered without one if the route is null.
		/// </summary>
		/// <param name="route"></param>
		public void AssignPatrol(PatrolRoute route)
		{
			this.PatrolConsidered = true;

			_patrolRoute = route;
			_patrolVisits = route != null ? new int[route.Count] : null;
			_patrolNodeIndex = route?.GetNearestNodeIndex(this.Entity.Position) ?? 0;
			_patrolPreviousIndex = -1;
		}

		/// <summary>
		/// Makes the rest of the entity's patrol group hate its
		/// attacker, so a pack answers an attack on any of its members.
		/// </summary>
		/// <param name="attacker"></param>
		protected virtual void AlertPatrolGroup(ICombatEntity attacker)
		{
			if (attacker == null || this.Entity == null)
				return;

			// The leader carries the group's handle, everyone else holds
			// it as their master.
			var leaderHandle = this.HasPatrolRoute ? this.Entity.Handle : _masterHandle;
			if (leaderHandle == 0 || (!this.HasPatrolRoute && !this.HasPatrolFormation))
				return;

			if ((GameClock.Now - _lastPatrolAlertTime) < PatrolAlertCooldown)
				return;

			_lastPatrolAlertTime = GameClock.Now;

			var candidates = this.Entity.Map.GetAttackableEnemiesInPosition(attacker, this.Entity.Position, PatrolAlertRange);
			foreach (var candidate in candidates)
			{
				if (candidate.Handle == this.Entity.Handle || candidate is not Mob mob || mob.IsDead)
					continue;

				if (!mob.Components.TryGet<AiComponent>(out var ai))
					continue;

				if (mob.Handle != leaderHandle && ai.Script.MasterHandle != leaderHandle)
					continue;

				ai.Script.QueueEventAlert(new HateIncreaseAlert(attacker, PatrolAlertHate));
			}
		}

		/// <summary>
		/// Returns the position of the route node the entity counts as
		/// being at home at via out. Returns false if it has no route.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		protected bool TryGetPatrolAnchor(out Position position)
		{
			position = Position.Zero;

			if (this.Entity == null)
				return false;

			if (_patrolRoute != null)
			{
				position = _patrolRoute.GetNearestNode(this.Entity.Position);
				return true;
			}

			// Followers belong wherever their leader took the group.
			if (this.HasPatrolFormation)
			{
				var master = this.GetMaster();
				if (master == null)
					return false;

				position = master.Position;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Makes the entity walk in its slot of the leader's formation,
		/// so a group doesn't pile up on a single spot.
		/// </summary>
		/// <param name="leader"></param>
		/// <returns></returns>
		protected virtual IEnumerable PatrolFollow(ICombatEntity leader)
		{
			var conf = ZoneServer.Instance.Conf.World;
			var running = false;

			this.SetRunning(false);

			while (leader != null && !leader.IsDead && !this.Entity.IsDead && this.Entity.Map == leader.Map)
			{
				if (this.Entity.IsLocked(LockType.Movement))
				{
					yield return this.Wait(100);
					continue;
				}

				var slotPosition = this.GetFormationPosition(leader);
				var distance = this.Entity.Position.Get2DDistance(slotPosition);

				// Running is for catching up only, or the group outpaces
				// the leader and bunches up in front of it.
				var shouldRun = distance > FormationCatchUpDistance;
				if (shouldRun != running)
				{
					running = shouldRun;
					this.SetRunning(shouldRun || !conf.PatrolWalk);
				}

				if (distance > FormationArriveDistance)
					yield return this.MoveTo(slotPosition, wait: false);

				yield return this.Wait(300);
			}

			this.ResetMoveSpeed();
			this.StartRoutine("Idle", this.Idle());
		}

		/// <summary>
		/// Returns the position the entity's formation slot sits at,
		/// which is staggered behind the leader.
		/// </summary>
		/// <param name="leader"></param>
		/// <returns></returns>
		private Position GetFormationPosition(ICombatEntity leader)
		{
			var row = (_patrolFormationSlot + 1) / 2;
			var side = (_patrolFormationSlot % 2 == 0) ? 1 : -1;

			var direction = leader.Direction;
			var forwardX = (float)direction.Cos;
			var forwardZ = (float)direction.Sin;

			var behind = row * FormationSpacing;
			var beside = side * FormationSpacing * 0.75f;

			var slotPosition = new Position(
				leader.Position.X - forwardX * behind - forwardZ * beside,
				leader.Position.Y,
				leader.Position.Z - forwardZ * behind + forwardX * beside);

			if (this.Entity.Map.Ground.TryGetNearestValidPosition(slotPosition, this.Entity.AgentRadius, out var validPosition))
				return validPosition;

			return leader.Position;
		}

		/// <summary>
		/// Walks the entity from node to node along its patrol route,
		/// without stopping in between.
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerable Patrol()
		{
			var conf = ZoneServer.Instance.Conf.World;

			this.SetRunning(!conf.PatrolWalk);

			while (_patrolRoute != null && !this.Entity.IsDead)
			{
				var startPosition = this.Entity.Position;

				yield return this.MoveTo(_patrolRoute.GetNode(_patrolNodeIndex));

				var visits = _patrolVisits;
				if (visits != null && _patrolNodeIndex < visits.Length)
					visits[_patrolNodeIndex]++;

				this.SelectNextPatrolNode();

				// A leg that went nowhere would spin the routine, so it's
				// the only case the entity holds still for.
				if (this.Entity.Position.InRange2D(startPosition, StalledLegDistance))
					yield return this.Wait(StalledLegDelay);
			}

			this.ResetMoveSpeed();
			this.StartRoutine("Idle", this.Idle());
		}

		/// <summary>
		/// Picks the node to head to next, preferring the closest of the
		/// ones the entity has visited the least.
		/// </summary>
		private void SelectNextPatrolNode()
		{
			var route = _patrolRoute;
			var visits = _patrolVisits;

			if (route == null || visits == null || route.Count < 2 || visits.Length != route.Count)
				return;

			var position = this.Entity.Position;
			var currentIndex = _patrolNodeIndex;

			var bestIndex = -1;
			var bestVisits = int.MaxValue;
			var bestDistance = double.MaxValue;

			for (var i = 0; i < route.Count; ++i)
			{
				if (i == currentIndex)
					continue;

				// Turning straight back around reads as pacing, so the
				// node we came from is a last resort.
				if (i == _patrolPreviousIndex && route.Count > 2)
					continue;

				var nodeVisits = visits[i];
				if (nodeVisits > bestVisits)
					continue;

				var distance = route.GetNode(i).Get2DDistance(position);
				if (nodeVisits == bestVisits && distance >= bestDistance)
					continue;

				bestIndex = i;
				bestVisits = nodeVisits;
				bestDistance = distance;
			}

			if (bestIndex == -1)
				return;

			_patrolPreviousIndex = currentIndex;
			_patrolNodeIndex = bestIndex;
		}
	}
}
