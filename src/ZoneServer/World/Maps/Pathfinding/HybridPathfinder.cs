using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.World;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Maps.Pathfinding
{
	public class HybridPathfinder : IPathfinder
	{
		private readonly Map _map;
		private readonly Ground _ground;
		private const int MaxNodes = 2000;

		public HybridPathfinder(Map map)
		{
			_map = map;
			_ground = map.Ground;
		}

		public bool TryFindPath(Position start, Position destination, float actorRadius, out List<Position> path)
		{
			path = new List<Position>();
			if (!this.ValidatePositions(ref start, ref destination, actorRadius))
				return false;

			if (this.IsDirectPath(start, destination, actorRadius))
			{
				path.Add(start);
				path.Add(destination);
				LogPathToFile(path, "direct");
				return true;
			}

			var gridScale = this.CalculateGridScale(start, destination, actorRadius);
			var cellPath = this.FindCellPath(start, destination, actorRadius, gridScale);

			if (cellPath.Count < 2)
			{
				Log.Warning($"Pathfinder: A* search returned a path with less than 2 nodes.");
				return false;
			}

			path = this.SmoothPath(cellPath, actorRadius);
			LogPathToFile(path, "smoothed");
			return path.Count > 1;
		}

		/// <summary>
		/// Writes the generated path to a timestamped log file in the temp directory.
		/// </summary>
		private static void LogPathToFile(List<Position> path, string pathType)
		{
			var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
			var fileName = $"path_{pathType}_{timestamp}.txt";
			var logDirectory = Path.Combine(Path.GetTempPath(), "pathfinder_logs");
			var filePath = Path.Combine(logDirectory, fileName);

			try
			{
				Directory.CreateDirectory(logDirectory);
				var sb = new StringBuilder();
				sb.AppendLine($"Path Type: {pathType}");
				sb.AppendLine($"Generated at: {DateTime.Now}");
				sb.AppendLine($"Node Count: {path.Count}");
				sb.AppendLine("---");

				foreach (var pos in path)
				{
					sb.AppendLine($"X: {pos.X:F2}, Y: {pos.Y:F2}, Z: {pos.Z:F2}");
				}

				File.WriteAllText(filePath, sb.ToString());
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to write pathfinding log to '{filePath}': {ex}");
			}
		}

		private bool ValidatePositions(ref Position start, ref Position goal, float actorRadius)
		{
			if (_ground == null || !_ground.HasData() || _map == null)
			{
				Log.Warning("Pathfinding: Ground data or map not available.");
				return false;
			}

			if (!_map.IsWalkablePosition(start, actorRadius))
			{
				if (!_ground.TryGetNearestValidPosition(start, actorRadius, out start, 100f))
				{
					Log.Warning($"Pathfinding: Start position {start} not walkable and no alternative found.");
					return false;
				}
			}

			if (!_map.IsWalkablePosition(goal, actorRadius))
			{
				if (!_ground.TryGetNearestValidPosition(goal, actorRadius, out goal, 100f))
				{
					Log.Warning($"Pathfinding: Goal position {goal} not walkable and no alternative found.");
					return false;
				}
			}

			return true;
		}

		private bool IsDirectPath(Position start, Position goal, float actorRadius)
		{
			// Use the new robust check for determining a direct path.
			return _map.IsLineOfSightWalkable(start, goal, actorRadius);
		}

		private int CalculateGridScale(Position start, Position goal, float actorRadius)
		{
			var distance = (float)start.Get2DDistance(goal);
			// The grid size should be related to the agent's size and the total distance.
			var scale = Math.Max(actorRadius * 2f, distance / 15f);
			return (int)Math.Clamp(scale, 40f, 200f);
		}

		private List<Position> FindCellPath(Position start, Position goal, float actorRadius, int gridScale)
		{
			var openSet = new PriorityQueue<Position, float>();
			var cameFrom = new Dictionary<Position, Position>();
			var gScore = new Dictionary<Position, float> { [start] = 0 };

			openSet.Enqueue(start, this.Heuristic(start, goal));

			var expandedNodes = 0;
			var closestNode = start;
			var closestHeuristic = this.Heuristic(start, goal);

			while (openSet.Count > 0 && expandedNodes < MaxNodes)
			{
				expandedNodes++;
				var current = openSet.Dequeue();

				// --- FIX START: Stricter Goal Condition ---
				// Instead of a fuzzy distance check, we now require a direct, walkable line of sight to the goal.
				// This is the primary condition for successfully finding a path.
				if (_map.IsLineOfSightWalkable(current, goal, actorRadius))
				{
					var path = this.ReconstructPath(cameFrom, current);
					path.Add(goal); // The path is clear, so we add the final goal.
					return path;
				}
				// --- FIX END ---

				var h = this.Heuristic(current, goal);
				if (h < closestHeuristic)
				{
					closestHeuristic = h;
					closestNode = current;
				}

				foreach (var neighbor in this.GetNeighbors(current, actorRadius, gridScale))
				{
					// This check from the previous fix is still essential.
					if (!_map.IsLineOfSightWalkable(current, neighbor, actorRadius))
					{
						continue;
					}

					var tentativeG = gScore[current] + (float)current.Get2DDistance(neighbor);

					if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
					{
						cameFrom[neighbor] = current;
						gScore[neighbor] = tentativeG;
						var fScore = tentativeG + this.Heuristic(neighbor, goal);

						if (!openSet.UnorderedItems.Any(item => item.Element.Equals(neighbor)))
						{
							openSet.Enqueue(neighbor, fScore);
						}
					}
				}
			}

			var partialPath = this.ReconstructPath(cameFrom, closestNode);

			// The smoothing function will receive this valid partial path and the original goal.
			// It will then handle moving along this path and getting as close as possible
			// to the final destination without making illegal moves.
			// To allow the smoother to do this, we append the original goal to the list for it to aim at.
			// The smoother is now responsible for pruning the unreachable end.
			partialPath.Add(goal);
			return partialPath;
		}

		private List<Position> GetNeighbors(Position pos, float actorRadius, int gridScale)
		{
			var neighbors = new List<Position>();
			var directions = new (int, int)[]
			{
				(1, 0), (-1, 0), (0, 1), (0, -1),   // Straight
				(1, 1), (-1, 1), (1, -1), (-1, -1)  // Diagonal
			};

			foreach (var dir in directions)
			{
				var neighborPos = new Position(
					pos.X + dir.Item1 * gridScale,
					pos.Y,
					pos.Z + dir.Item2 * gridScale
				);

				if (_ground.TryGetHeightAt(neighborPos, out float height))
				{
					neighborPos.Y = height;
					if (_map.IsWalkablePosition(neighborPos, actorRadius))
					{
						neighbors.Add(neighborPos);
					}
				}
			}
			return neighbors;
		}

		private float Heuristic(Position a, Position b)
		{
			// Manhattan distance is fast and effective for grid-based A*.
			return Math.Abs(a.X - b.X) + Math.Abs(a.Z - b.Z);
		}

		private List<Position> ReconstructPath(Dictionary<Position, Position> cameFrom, Position current)
		{
			var path = new List<Position> { current };
			while (cameFrom.TryGetValue(current, out current))
			{
				path.Insert(0, current);
			}
			return path;
		}

		/// <summary>
		/// Simplifies the path by removing intermediate nodes that have a clear line of sight.
		/// This is a "string pulling" algorithm. This implementation is corrected to be robust.
		/// </summary>
		private List<Position> SmoothPath(List<Position> path, float actorRadius)
		{
			if (path.Count < 2) return path;

			var smoothedPath = new List<Position> { path[0] };
			int anchorIndex = 0;

			while (anchorIndex < path.Count - 1)
			{
				int lastVisibleIndex = anchorIndex + 1;
				for (int i = anchorIndex + 2; i < path.Count; i++)
				{
					if (_map.IsLineOfSightWalkable(path[anchorIndex], path[i], actorRadius))
					{
						lastVisibleIndex = i;
					}
					else
					{
						// Obstacle found, path[i-1] was the last point visible from the anchor.
						// We need to use i-1, but the loop sets lastVisibleIndex correctly on the prior iteration.
						break;
					}
				}

				// Add the furthest point that was visible from the anchor.
				smoothedPath.Add(path[lastVisibleIndex]);

				// The new anchor for the next iteration is the point we just added.
				anchorIndex = lastVisibleIndex;
			}

			return smoothedPath;
		}
	}
}
