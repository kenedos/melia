using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.World;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.World.Maps.Pathfinding
{
	/// <summary>
	/// A pathfinder that uses a dynamic grid with the A* algorithm.
	/// </summary>
	public class DynamicGridPathfinder : IPathfinder
	{
		private readonly static int MaxNodeExpand = 20000;

		private readonly Ground _ground;
		private readonly Map _map;

		/// <summary>
		/// Creates a new instance for the given map.
		/// </summary>
		/// <param name="map"></param>
		public DynamicGridPathfinder(Map map)
		{
			_ground = map.Ground;
			_map = map;
		}

		/// <summary>
		/// Finds a path using a dynamic-resolution A* algorithm and smoothes the result.
		/// </summary>
		/// <param name="start">The starting position for the path.</param>
		/// <param name="goal">The desired destination for the path.</param>
		/// <param name="actorRadius">The radius of the actor, used for clearance checks.</param>
		/// <param name="path">When this method returns, contains the sequence of waypoints for the path, or an empty list if no path was found.</param>
		/// <returns>True if a path was found; otherwise, false.</returns>
		public bool TryFindPath(Position start, Position goal, float actorRadius, out List<Position> path)
		{
			path = new List<Position>();

			if (_ground == null || !_ground.HasData())
				return false;

			// Validate start position. If it's invalid, find the nearest walkable one.
			if (!_map.IsWalkablePosition(start, actorRadius))
			{
				if (!_ground.TryGetNearestValidPosition(start, actorRadius, out start, 50f))
				{
					Log.Warning($"Pathfinder: Could not find a walkable start position near {start} for actor with radius {actorRadius}.");
					return false; // Pathfinding is impossible if we can't find a valid place to start.
				}
			}

			// Validate goal position. If it's invalid, find the nearest walkable one.
			// If we can't find a valid spot near the goal, we don't fail; A* will find a path to the closest reachable node.
			_ground.TryGetNearestValidPosition(goal, actorRadius, out goal, 50f);

			var distance = start.Get2DDistance(goal);
			var gridScale = this.GetGridScale(distance, actorRadius);

			var roughPath = this.FindPathScale(start, goal, gridScale, actorRadius);

			// Merge nodes to create a smoother, more direct path
			if (roughPath.Count > 0)
			{
				var distinctPath = roughPath.Distinct().ToList();
				path.AddRange(this.MergePathNodes(distinctPath, actorRadius));
			}

#if DEBUG
			Debug.ShowPositions(_map, path, TimeSpan.FromMilliseconds(200));
#endif

			return path.Count != 0;
		}

		/// <summary>
		/// Finds a path from start to goal using a recursive A* with a variable grid scale.
		/// </summary>
		private List<Position> FindPathScale(Position start, Position goal, int gridScale, float actorRadius)
		{
			var path = new List<Position>();
			var openSet = new PriorityQueue<Position, float>();
			var cameFrom = new Dictionary<Position, Position>();
			var gScore = new Dictionary<Position, float> { [start] = 0 };
			var fScore = new Dictionary<Position, float> { [start] = this.Heuristic(start, goal) };

			// Base case for recursion: if we are very close, return a direct path.
			if (gridScale <= 10 || start.Get2DDistance(goal) < actorRadius)
			{
				if (_ground.IsValidCirclePosition(start, actorRadius))
					path.Add(start);

				return path;
			}

			openSet.Enqueue(start, fScore[start]);
			while (openSet.Count > 0 && openSet.Count < MaxNodeExpand)
			{
				var current = openSet.Dequeue();
				var distance = current.Get2DDistance(goal);

				// As we approach the target, recursively call with a smaller, more refined grid scale.
				if (distance < gridScale * 2)
				{
					path.AddRange(this.ReconstructPath(cameFrom, current));
					var newGridScale = this.GetGridScale(distance, actorRadius);
					path.AddRange(this.FindPathScale(current, goal, newGridScale, actorRadius));
					return path;
				}

				var neighbors = this.GetNeighbors(current, actorRadius, gridScale);

				foreach (var neighbor in neighbors)
				{
					var tentativeGScore = gScore[current] + (float)current.Get3DDistance(neighbor);
					if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
					{
						cameFrom[neighbor] = current;
						gScore[neighbor] = tentativeGScore;
						fScore[neighbor] = tentativeGScore + this.Heuristic(neighbor, goal);
						openSet.Enqueue(neighbor, fScore[neighbor]);
					}
				}
			}

			// If goal was not reached, return a path to the closest node we found.
			if (cameFrom.Count > 0)
			{
				var closestNode = fScore.MinBy(kv => kv.Value).Key;
				path.AddRange(this.ReconstructPath(cameFrom, closestNode));
			}

			return path;
		}

		private int GetGridScale(double distance, float actorRadius)
		{
			// This heuristic determines the distance between pathfinding nodes.
			// A smaller value is more accurate but computationally expensive.
			// actorRadius * 2 is a safe value to prevent clipping through thin obstacles.
			const int minStep = 5;
			return (int)Math.Min(distance / 2, actorRadius * 2 + minStep);
		}

		private List<Position> ReconstructPath(Dictionary<Position, Position> cameFrom, Position current)
		{
			var totalPath = new List<Position> { current };
			while (cameFrom.ContainsKey(current))
			{
				current = cameFrom[current];
				totalPath.Add(current);
			}
			totalPath.Reverse();
			return totalPath;
		}

		private float Heuristic(Position a, Position b)
		{
			// Euclidean distance with a penalty for height differences to encourage
			// finding paths on similar elevations first.
			const float heightPenalty = 3f;
			var dx = Math.Abs(a.X - b.X);
			var dy = Math.Abs(a.Y - b.Y) * heightPenalty;
			var dz = Math.Abs(a.Z - b.Z);

			return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
		}

		private List<Position> GetNeighbors(Position pos, float actorRadius, int gridScale, int angleSubdivisions = 1)
		{
			if (gridScale <= 0) return new List<Position>();

			var neighbors = new List<Position>();
			var directions = this.GenerateDirections(gridScale, angleSubdivisions);

			foreach (var dir in directions)
			{
				var neighbor = new Position(pos.X + dir.X, 0, pos.Z + dir.Z);
				if (_map.IsWalkablePosition(neighbor, actorRadius) && _ground.TryGetHeightAt(neighbor, out var height))
				{
					neighbors.Add(neighbor.WithHeight(height));
				}
			}

			// If we are in a tight spot with few options, increase search resolution.
			if (neighbors.Count <= 3 && angleSubdivisions < 8)
			{
				return this.GetNeighbors(pos, actorRadius, gridScale, angleSubdivisions * 2);
			}

			// If still no neighbors, try again with a smaller step distance.
			if (neighbors.Count == 0 && gridScale > 1)
			{
				return this.GetNeighbors(pos, actorRadius, gridScale / 2);
			}

			return neighbors;
		}

		private List<(int X, int Z)> GenerateDirections(int distance, int subdivisions)
		{
			var directions = new List<(int X, int Z)>();
			var totalDirections = 8 * subdivisions;
			var angleStep = 2 * Math.PI / totalDirections;

			for (var i = 0; i < totalDirections; i++)
			{
				var angle = i * angleStep;
				var x = (int)Math.Round(distance * Math.Cos(angle));
				var z = (int)Math.Round(distance * Math.Sin(angle));
				directions.Add((x, z));
			}

			return directions.Distinct().ToList();
		}

		/// <summary>
		/// Simplifies a path by removing intermediate nodes that have a clear line of sight. (Path smoothing)
		/// </summary>
		private List<Position> MergePathNodes(List<Position> path, float actorRadius)
		{
			if (path.Count < 3) return path;

			var mergedPath = new List<Position> { path.First() };
			var currentAnchorIndex = 0;

			while (currentAnchorIndex < path.Count - 1)
			{
				var farthestVisibleIndex = currentAnchorIndex + 1;
				for (var i = currentAnchorIndex + 2; i < path.Count; i++)
				{
					if (_map.IsLineOfSightWalkable(path[currentAnchorIndex], path[i], actorRadius))
					{
						farthestVisibleIndex = i;
					}
					else
					{
						break; // Obstacle found, the previous node was the last visible one.
					}
				}

				mergedPath.Add(path[farthestVisibleIndex]);
				currentAnchorIndex = farthestVisibleIndex;
			}

			return mergedPath;
		}
	}
}
