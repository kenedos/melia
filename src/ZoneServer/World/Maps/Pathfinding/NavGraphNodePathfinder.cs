using System.Collections.Generic;
using System.Linq;
using g4;
using Melia.Shared.World;
using Melia.Zone.World.Maps.NavMesh;

namespace Melia.Zone.World.Maps.Pathfinding
{
	/// <summary>
	/// A pathfinder that uses a pre-calculated navigation graph of polygons (a navmesh).
	/// It uses A* to find a channel of polygons and the Funnel Algorithm to produce a smooth, optimal path.
	/// </summary>
	public class NavGraphNodePathfinder : IPathfinder
	{
		private readonly Ground _ground;
		private readonly NavGraphNode[] _graphNodes;

		/// <summary>
		/// Initializes a new instance of the <see cref="NavGraphNodePathfinder"/> class with the specified map.
		/// </summary>
		/// <remarks>The provided <paramref name="map"/> must contain valid ground and graph node data to enable
		/// pathfinding operations.</remarks>
		/// <param name="map">The map containing the ground and graph nodes used for pathfinding.</param>
		public NavGraphNodePathfinder(Map map)
		{
			_ground = map.Ground;
			_graphNodes = map.Ground.GraphNodes;
		}

		/// <summary>
		/// Attempts to find a smoothed, navigable path between the specified start and destination positions using A* on the navgraph.
		/// </summary>
		/// <param name="start">The starting position of the path.</param>
		/// <param name="destination">The destination position of the path.</param>
		/// <param name="actorRadius">This parameter is currently ignored by the navgraph pathfinder.</param>
		/// <param name="path">When this method returns, contains the list of waypoints for the calculated path, if found; otherwise, an empty list.</param>
		/// <returns><see langword="true"/> if a path is successfully found; otherwise, <see langword="false"/>.</returns>
		public bool TryFindPath(Position start, Position destination, float actorRadius, out List<Position> path)
		{
			path = new List<Position>();

			if (_graphNodes == null || _graphNodes.Length == 0)
				return false;

			var startIndex = _ground.FindContainingPolygon(new Vector2d(start.X, start.Z));
			var destIndex = _ground.FindContainingPolygon(new Vector2d(destination.X, destination.Z));

			if (startIndex == -1 || destIndex == -1)
				return false;

			// If start and destination are in the same polygon, the path is a straight line.
			if (startIndex == destIndex)
			{
				path.Add(start);
				path.Add(destination);
				return true;
			}

			// Step 1: A* algorithm to find the sequence of polygons.
			var cameFrom = new Dictionary<int, int>();
			if (!FindPolygonPath(startIndex, destIndex, cameFrom))
				return false;

			var polygonPath = ReconstructPolygonPath(startIndex, destIndex, cameFrom);

			// Step 2: Extract the "portals" (shared edges) between the polygons in the path.
			var portals = new List<(Vector2d Left, Vector2d Right)>();
			for (int i = 0; i < polygonPath.Count - 1; i++)
			{
				if (TryGetPortal(polygonPath[i], polygonPath[i + 1], out var portal))
				{
					portals.Add(portal);
				}
				else
				{
					// If we can't form a valid channel, fall back to a simple centroid-based path.
					return BuildCentroidPath(start, destination, polygonPath, out path);
				}
			}

			// Step 3: Use the Funnel Algorithm (String Pulling) to find the shortest path through the portals.
			var smoothedPoints2d = StringPull(new Vector2d(start.X, start.Z), new Vector2d(destination.X, destination.Z), portals);

			// Step 4: Convert 2D points to 3D positions with correct ground height.
			foreach (var p2d in smoothedPoints2d)
			{
				var waypoint = new Position((float)p2d.x, 0, (float)p2d.y);
				if (_ground.TryGetHeightAt(waypoint, out float height))
				{
					path.Add(waypoint.WithHeight(height));
				}
				else
				{
					path.Add(waypoint); // Fallback if height check fails.
				}
			}

			return true;
		}

		private bool FindPolygonPath(int startIndex, int destIndex, Dictionary<int, int> cameFrom)
		{
			var frontier = new PriorityQueue<int, double>();
			frontier.Enqueue(startIndex, 0);

			cameFrom[startIndex] = startIndex;
			var costSoFar = new Dictionary<int, double> { [startIndex] = 0 };

			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();

				if (current == destIndex)
					return true; // Path found

				foreach (var neighbor in _graphNodes[current].Neighbors)
				{
					var newCost = costSoFar[current] + Distance(_graphNodes[current], _graphNodes[neighbor]);
					if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
					{
						costSoFar[neighbor] = newCost;
						var priority = newCost + Heuristic(_graphNodes[neighbor], _graphNodes[destIndex]);
						frontier.Enqueue(neighbor, priority);
						cameFrom[neighbor] = current;
					}
				}
			}

			return false; // No path found
		}

		private List<int> ReconstructPolygonPath(int startIndex, int destIndex, Dictionary<int, int> cameFrom)
		{
			var polygonPath = new List<int>();
			var currentIndex = destIndex;
			while (currentIndex != startIndex)
			{
				polygonPath.Add(currentIndex);
				currentIndex = cameFrom[currentIndex];
			}
			polygonPath.Add(startIndex);
			polygonPath.Reverse();
			return polygonPath;
		}

		/// <summary>
		/// Finds the shared edge between two polygons and orders its vertices as Left/Right relative to the direction of travel.
		/// </summary>
		private bool TryGetPortal(int polyA_idx, int polyB_idx, out (Vector2d Left, Vector2d Right) portal)
		{
			portal = default;

			var polyA = _graphNodes[polyA_idx].Polygon;
			var polyB = _graphNodes[polyB_idx].Polygon;

			var sharedVerts = polyA.Vertices.Intersect(polyB.Vertices).ToList();
			if (sharedVerts.Count != 2) return false;

			var v1 = sharedVerts[0];
			var v2 = sharedVerts[1];

			// Use the vector between centroids as the direction of travel.
			var travelDirection = _graphNodes[polyB_idx].Centroid - _graphNodes[polyA_idx].Centroid;
			var startPoint = _graphNodes[polyA_idx].Centroid;

			// Use the cross product to determine which vertex is on the "left".
			if (TriArea(startPoint, startPoint + travelDirection, v1) > 0)
			{
				portal = (Left: v1, Right: v2);
			}
			else
			{
				portal = (Left: v2, Right: v1);
			}
			return true;
		}

		/// <summary>
		/// Implements the Funnel Algorithm (or String Pulling) to find the shortest path within a channel of portals.
		/// </summary>
		private List<Vector2d> StringPull(Vector2d start, Vector2d end, List<(Vector2d Left, Vector2d Right)> portals)
		{
			var path = new List<Vector2d> { start };

			var apex = start;
			var portalLeft = start;
			var portalRight = start;
			var apexIndex = 0;
			var leftIndex = 0;
			var rightIndex = 0;

			for (int i = 0; i < portals.Count; ++i)
			{
				var (left, right) = portals[i];

				// Update right vertex.
				if (TriArea(apex, portalRight, right) >= 0.0)
				{
					if (apex == portalRight || TriArea(apex, portalLeft, right) < 0.0)
					{
						portalRight = right;
						rightIndex = i;
					}
					else
					{
						path.Add(portalLeft);
						apex = portalLeft;
						apexIndex = leftIndex;
						portalLeft = apex;
						portalRight = apex;
						leftIndex = apexIndex;
						rightIndex = apexIndex;
						i = apexIndex;
						continue;
					}
				}

				// Update left vertex.
				if (TriArea(apex, portalLeft, left) <= 0.0)
				{
					if (apex == portalLeft || TriArea(apex, portalRight, left) > 0.0)
					{
						portalLeft = left;
						leftIndex = i;
					}
					else
					{
						path.Add(portalRight);
						apex = portalRight;
						apexIndex = rightIndex;
						portalLeft = apex;
						portalRight = apex;
						leftIndex = apexIndex;
						rightIndex = apexIndex;
						i = apexIndex;
						continue;
					}
				}
			}

			path.Add(end);
			return path;
		}

		/// <summary>
		/// Calculates the signed area of a triangle. The sign indicates orientation.
		/// > 0: CCW (c is to the left of a->b), < 0: CW (c is to the right of a->b), = 0: Collinear.
		/// </summary>
		private double TriArea(Vector2d a, Vector2d b, Vector2d c)
		{
			return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
		}

		/// <summary>
		/// A fallback method to create a simple path through polygon centroids if smoothing fails.
		/// </summary>
		private bool BuildCentroidPath(Position start, Position destination, List<int> polygonPath, out List<Position> path)
		{
			path = new List<Position> { start };
			for (var i = 1; i < polygonPath.Count; i++)
			{
				var polyIndex = polygonPath[i];
				var centroid = _graphNodes[polyIndex].Centroid;
				var waypoint = new Position((float)centroid.x, 0, (float)centroid.y);

				if (_ground.TryGetHeightAt(waypoint, out float height))
				{
					path.Add(waypoint.WithHeight(height));
				}
				else
				{
					path.Add(waypoint);
				}
			}
			path.Add(destination);
			return true;
		}

		private double Distance(NavGraphNode a, NavGraphNode b) => (a.Centroid - b.Centroid).Length;

		private double Heuristic(NavGraphNode a, NavGraphNode b) => this.Distance(a, b);
	}
}
