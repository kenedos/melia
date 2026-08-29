using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Patrols
{
	/// <summary>
	/// Builds the patrol graphs of dungeon maps and puts the monsters
	/// living on them on patrol routes.
	/// </summary>
	public class PatrolManager : IUpdateable
	{
		private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(2);

		private const string NodeEffectName = "F_light110_pink_ground_loop";

		private const string DungeonPrefix = "d_";
		private const string InstancedDungeonPrefix = "id_";
		private const float NodeEffectDuration = 10000f;

		private TimeSpan _checkDelay = CheckInterval;

		private readonly object _graphLock = new();
		private readonly Dictionary<string, PatrolGraph> _graphs = new();
		private readonly HashSet<string> _buildingGraphs = new();

		/// <summary>
		/// Builds the graphs of the dungeon maps players are on and
		/// assigns routes to the monsters that don't have one yet.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			_checkDelay -= elapsed;
			if (_checkDelay > TimeSpan.Zero)
				return;

			_checkDelay = CheckInterval;

			if (!ZoneServer.Instance.Conf.World.PatrolEnabled)
				return;

			var maps = ZoneServer.Instance.World.Maps.GetList(a => IsPatrolMap(a) && a.HasCharacters);
			foreach (var map in maps)
			{
				var graph = this.GetGraph(map);
				if (graph == null)
				{
					this.StartGraphBuild(map);
					continue;
				}

				if (graph.Count == 0)
					continue;

				this.AssignRoutes(map, graph);
			}
		}

		/// <summary>
		/// Returns the map's patrol graph, or null if it hasn't been
		/// built yet.
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		public PatrolGraph GetGraph(Map map)
		{
			lock (_graphLock)
				return _graphs.TryGetValue(map.ClassName, out var graph) ? graph : null;
		}

		/// <summary>
		/// Builds the map's patrol graph in the background, unless it's
		/// already built or being built.
		/// </summary>
		/// <param name="map"></param>
		private void StartGraphBuild(Map map)
		{
			lock (_graphLock)
			{
				if (_graphs.ContainsKey(map.ClassName) || !_buildingGraphs.Add(map.ClassName))
					return;
			}

			Task.Run(() =>
			{
				try
				{
					var stopwatch = Stopwatch.StartNew();
					var graph = PatrolGraph.Build(map);

					lock (_graphLock)
					{
						_graphs[map.ClassName] = graph;
						_buildingGraphs.Remove(map.ClassName);
					}

					Log.Info($"PatrolManager: Built {graph.Count} patrol nodes and {graph.EdgeCount} connections for '{map.ClassName}' in {stopwatch.ElapsedMilliseconds}ms.");
				}
				catch (Exception ex)
				{
					lock (_graphLock)
						_buildingGraphs.Remove(map.ClassName);

					Log.Error($"PatrolManager: Failed to build the patrol graph for '{map.ClassName}'. {ex}");
				}
			});
		}

		/// <summary>
		/// Puts the map's eligible monsters on patrol routes, with the
		/// ones spawned next to a patrol leader following it instead.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="graph"></param>
		private void AssignRoutes(Map map, PatrolGraph graph)
		{
			var conf = ZoneServer.Instance.Conf.World;
			var monsters = map.GetMonsters(a => a is Mob mob && IsPatrolMob(mob));

			foreach (var monster in monsters)
			{
				var mob = (Mob)monster;

				if (!mob.Components.TryGet<AiComponent>(out var ai) || ai.Script.PatrolConsidered)
					continue;

				ai.Script.AssignPatrol(null);

				if (mob.IsBuffActive(BuffId.EliteMonsterBuff))
					continue;

				if (GameRandom.Get().NextDouble() * 100 >= conf.PatrolChance)
					continue;

				if (!graph.TryBuildRoute(map, mob.Position, out var route))
					continue;

				ai.Script.AssignPatrol(route);

				// A lone monster wandering a dungeon reads as a stray, so
				// a patrol only exists if there's a group to walk it.
				if (this.AssignFollowers(monsters, mob, conf.PatrolGroupSize, conf.PatrolGroupRadius) == 0)
					ai.Script.AssignPatrol(null);
			}
		}

		/// <summary>
		/// Makes the monsters of the leader's own kind around it follow
		/// it on its patrol and returns how many joined it.
		/// </summary>
		/// <param name="monsters"></param>
		/// <param name="leader"></param>
		/// <param name="groupSize"></param>
		/// <param name="groupRadius"></param>
		/// <returns></returns>
		private int AssignFollowers(List<IMonster> monsters, Mob leader, int groupSize, float groupRadius)
		{
			var followerCount = 0;

			foreach (var monster in monsters)
			{
				if (followerCount >= groupSize)
					break;

				// A patrol is one kind of monster moving together, never
				// a mix of whatever happened to spawn nearby.
				if (monster is not Mob mob || mob.Handle == leader.Handle || mob.Id != leader.Id)
					continue;

				if (!mob.Position.InRange2D(leader.Position, groupRadius))
					continue;

				if (!mob.Components.TryGet<AiComponent>(out var ai))
					continue;

				// Monsters that were passed over for leading a patrol are
				// still free to walk in one.
				if (ai.Script.HasPatrolRoute || ai.Script.HasPatrolFormation || ai.Script.GetMaster() != null)
					continue;

				ai.Script.SetPatrolFormationSlot(followerCount + 1);
				ai.Script.SetMaster(leader);

				followerCount++;
			}

			return followerCount;
		}

		/// <summary>
		/// Plays an effect on every patrol node around the character and
		/// returns the graph's size via out. Returns false if the map has
		/// no graph.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="range"></param>
		/// <param name="shownCount"></param>
		/// <param name="nodeCount"></param>
		/// <param name="edgeCount"></param>
		/// <returns></returns>
		public bool TryShowNodes(Character character, float range, out int shownCount, out int nodeCount, out int edgeCount)
		{
			shownCount = 0;
			nodeCount = 0;
			edgeCount = 0;

			var graph = this.GetGraph(character.Map);
			if (graph == null)
			{
				this.StartGraphBuild(character.Map);
				return false;
			}

			nodeCount = graph.Count;
			edgeCount = graph.EdgeCount;

			foreach (var node in graph.Nodes)
			{
				if (!node.Position.InRange2D(character.Position, range))
					continue;

				var effectHandle = ZoneServer.Instance.World.CreateEffectHandle();
				Send.ZC_NORMAL.PlayEffectAtPosition(character, NodeEffectName, node.Position, 2f, effectHandle, NodeEffectDuration);

				shownCount++;
			}

			return true;
		}

		/// <summary>
		/// Returns a report about the patrol state of the character's
		/// map and the monsters around them.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public List<string> GetStatus(Character character, float range)
		{
			var lines = new List<string>();
			var map = character.Map;
			var conf = ZoneServer.Instance.Conf.World;

			lines.Add($"Map '{map.ClassName}': type {map.Data?.Type}, instance {map.IsInstance}, dynamic {map is DynamicMap}, dormant {map.IsDormant}, patrols {(IsPatrolMap(map) ? "allowed" : "BLOCKED")}.");
			lines.Add($"Conf: enabled {conf.PatrolEnabled}, chance {conf.PatrolChance}%, group {conf.PatrolGroupSize}.");

			var graph = this.GetGraph(map);
			if (graph == null)
			{
				lines.Add("Graph: not built yet, starting it now.");
				this.StartGraphBuild(map);
			}
			else
			{
				lines.Add($"Graph: {graph.Count} nodes, {graph.EdgeCount} connections.");
			}

			var monsters = map.GetMonsters(a => a is Mob mob && IsPatrolMob(mob));
			var considered = 0;
			var patrolling = 0;

			foreach (var monster in monsters)
			{
				if (!monster.Components.TryGet<AiComponent>(out var ai))
					continue;

				if (ai.Script.PatrolConsidered)
					considered++;

				if (ai.Script.HasPatrolRoute)
					patrolling++;
			}

			lines.Add($"Monsters: {monsters.Count} eligible, {considered} considered, {patrolling} on a route.");

			foreach (var monster in monsters)
			{
				if (monster is not Mob mob)
					continue;

				if (!mob.Position.InRange2D(character.Position, range))
					continue;

				if (!mob.Components.TryGet<AiComponent>(out var ai))
					continue;

				var master = ai.Script.GetMaster();
				var role = ai.Script.HasPatrolRoute ? "leader" : master != null ? "follower" : "none";

				lines.Add($"  {mob.Name} ({mob.Handle}): routine '{ai.Script.CurrentRoutine}', patrol {role}, considered {ai.Script.PatrolConsidered}.");

				if (lines.Count > 15)
					break;
			}

			return lines;
		}

		/// <summary>
		/// Returns true if monsters can patrol on the given map.
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		private static bool IsPatrolMap(Map map)
		{
			if (map == null || map == Map.Limbo || map is DynamicMap || map.IsDormant)
				return false;

			if (map.IsInstance)
				return false;

			// Instanced dungeons are named apart from the dungeons that
			// are part of the world.
			if (map.ClassName.StartsWith(InstancedDungeonPrefix))
				return false;

			// Most dungeons are typed as fields in the client's map data,
			// so their names are the more reliable signal.
			return map.Data?.Type == MapType.Dungeon || map.ClassName.StartsWith(DungeonPrefix);
		}

		/// <summary>
		/// Returns true if the monster can be put on a patrol route.
		/// </summary>
		/// <param name="mob"></param>
		/// <returns></returns>
		private static bool IsPatrolMob(Mob mob)
		{
			if (mob.IsDead || mob is Summon)
				return false;

			if (mob.Rank != MonsterRank.Normal || mob.Tendency != TendencyType.Aggressive)
				return false;

			// Statues, crystals, and anything else rooted to its spawn
			// point can't walk a route.
			if (mob.Data.MoveType == MoveType.None || mob.Data.MoveType == MoveType.Holding || (mob.Data.WalkSpeed <= 0 && mob.Data.RunSpeed <= 0))
				return false;

			return mob.Components.Has<MovementComponent>() && mob.Components.Has<AiComponent>();
		}
	}
}
