using System;
using System.Collections.Generic;
using Melia.Shared.World;
using Melia.Zone.World.Maps;

namespace Melia.Zone.World.Patrols
{
	/// <summary>
	/// The network of room and corridor centers of one map, used to
	/// generate patrol routes.
	/// </summary>
	/// <remarks>
	/// The nodes are the points of the ground that sit furthest away
	/// from the walls around them, which puts one in the middle of
	/// every room and a chain of them down every corridor.
	/// </remarks>
	public class PatrolGraph
	{
		private const int MaxSamples = 400000;
		private const float RouteAgentRadius = 20f;

		private readonly List<PatrolNode> _nodes = new();

		/// <summary>
		/// Returns the amount of nodes in the graph.
		/// </summary>
		public int Count => _nodes.Count;

		/// <summary>
		/// Returns the graph's nodes.
		/// </summary>
		public IReadOnlyList<PatrolNode> Nodes => _nodes;

		/// <summary>
		/// Returns the amount of connections between the graph's nodes.
		/// </summary>
		public int EdgeCount
		{
			get
			{
				var count = 0;
				foreach (var node in _nodes)
					count += node.Neighbors.Count;

				return count / 2;
			}
		}

		/// <summary>
		/// Builds the patrol graph for the given map.
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		public static PatrolGraph Build(Map map)
		{
			var graph = new PatrolGraph();

			var ground = map.Ground;
			if (ground == null || !ground.HasData())
				return graph;

			var conf = ZoneServer.Instance.Conf.World;

			var step = Math.Max(5, conf.PatrolNodeSampleStep);
			var width = (ground.SizeX / step) + 1;
			var height = (ground.SizeZ / step) + 1;

			// Huge maps get sampled more coarsely instead of eating an
			// unbounded amount of memory and time.
			while ((long)width * height > MaxSamples)
			{
				step *= 2;
				width = (ground.SizeX / step) + 1;
				height = (ground.SizeZ / step) + 1;
			}

			var clearances = SampleClearances(ground, step, width, height, conf.PatrolNodeMinClearance);

			graph.ExtractNodes(ground, clearances, step, width, height, conf.PatrolNodeSpacing);
			graph.ConnectNodes(ground, conf.PatrolEdgeMaxLength);
			graph.KeepLargestComponent();

			return graph;
		}

		/// <summary>
		/// Returns a grid with the distance to the nearest wall for every
		/// sampled point, or -1 where there's no usable ground.
		/// </summary>
		/// <param name="ground"></param>
		/// <param name="step"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="minClearance"></param>
		/// <returns></returns>
		private static float[] SampleClearances(Ground ground, int step, int width, int height, float minClearance)
		{
			var clearances = new float[width * height];

			for (var y = 0; y < height; ++y)
			{
				for (var x = 0; x < width; ++x)
				{
					var index = y * width + x;
					clearances[index] = -1;

					var position = new Position(ground.Left + x * step, 0, ground.Bottom + y * step);

					if (!ground.TryGetClearance(position, out var clearance))
						continue;

					if (clearance < minClearance)
						continue;

					clearances[index] = clearance;
				}
			}

			return clearances;
		}

		/// <summary>
		/// Turns the sampled clearances into spaced out nodes, one for
		/// every room and a chain of them along every corridor.
		/// </summary>
		/// <param name="ground"></param>
		/// <param name="clearances"></param>
		/// <param name="step"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="spacing"></param>
		private void ExtractNodes(Ground ground, float[] clearances, int step, int width, int height, float spacing)
		{
			var candidates = new List<int>();

			for (var y = 1; y < height - 1; ++y)
			{
				for (var x = 1; x < width - 1; ++x)
				{
					var index = y * width + x;
					var clearance = clearances[index];

					if (clearance < 0)
						continue;

					// A point that no neighbor to either side beats is as
					// far from the walls on that axis as it can get, which
					// is the center of the room or corridor it's in.
					var ridgeOnX = clearance >= clearances[index - 1] && clearance >= clearances[index + 1];
					var ridgeOnZ = clearance >= clearances[index - width] && clearance >= clearances[index + width];

					if (ridgeOnX || ridgeOnZ)
						candidates.Add(index);
				}
			}

			candidates.Sort((a, b) => clearances[b].CompareTo(clearances[a]));

			foreach (var index in candidates)
			{
				var x = index % width;
				var y = index / width;
				var position = new Position(ground.Left + x * step, 0, ground.Bottom + y * step);

				if (this.IsTooCloseToNode(position, spacing))
					continue;

				if (!ground.TryGetHeightAt(position, out var groundHeight))
					continue;

				_nodes.Add(new PatrolNode(position.WithHeight(groundHeight), clearances[index]));
			}
		}

		/// <summary>
		/// Returns true if an accepted node already sits within the
		/// given spacing of the position.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="spacing"></param>
		/// <returns></returns>
		private bool IsTooCloseToNode(Position position, float spacing)
		{
			foreach (var node in _nodes)
			{
				if (node.Position.InRange2D(position, spacing))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Connects the nodes that can see each other, rebuilding the
		/// map's room and corridor layout.
		/// </summary>
		/// <param name="ground"></param>
		/// <param name="maxEdgeLength"></param>
		private void ConnectNodes(Ground ground, float maxEdgeLength)
		{
			for (var i = 0; i < _nodes.Count; ++i)
			{
				for (var j = i + 1; j < _nodes.Count; ++j)
				{
					var first = _nodes[i];
					var second = _nodes[j];

					if (!first.Position.InRange2D(second.Position, maxEdgeLength))
						continue;

					if (ground.AnyObstacles(first.Position, second.Position))
						continue;

					first.Neighbors.Add(j);
					second.Neighbors.Add(i);
				}
			}
		}

		/// <summary>
		/// Drops every node outside of the graph's largest connected
		/// group, since those sit in geometry nothing can walk to.
		/// </summary>
		private void KeepLargestComponent()
		{
			if (_nodes.Count == 0)
				return;

			var components = new int[_nodes.Count];
			for (var i = 0; i < components.Length; ++i)
				components[i] = -1;

			var largestComponent = -1;
			var largestSize = 0;
			var componentId = 0;
			var queue = new Queue<int>();

			for (var i = 0; i < _nodes.Count; ++i)
			{
				if (components[i] != -1)
					continue;

				var size = 0;
				queue.Clear();
				queue.Enqueue(i);
				components[i] = componentId;

				while (queue.Count > 0)
				{
					var current = queue.Dequeue();
					size++;

					foreach (var neighbor in _nodes[current].Neighbors)
					{
						if (components[neighbor] != -1)
							continue;

						components[neighbor] = componentId;
						queue.Enqueue(neighbor);
					}
				}

				if (size > largestSize)
				{
					largestSize = size;
					largestComponent = componentId;
				}

				componentId++;
			}

			var remappedIndices = new int[_nodes.Count];
			var keptNodes = new List<PatrolNode>();

			for (var i = 0; i < _nodes.Count; ++i)
			{
				if (components[i] != largestComponent)
				{
					remappedIndices[i] = -1;
					continue;
				}

				remappedIndices[i] = keptNodes.Count;
				keptNodes.Add(_nodes[i]);
			}

			foreach (var node in keptNodes)
			{
				var neighbors = node.Neighbors;
				for (var i = neighbors.Count - 1; i >= 0; --i)
				{
					var remapped = remappedIndices[neighbors[i]];
					if (remapped == -1)
						neighbors.RemoveAt(i);
					else
						neighbors[i] = remapped;
				}
			}

			_nodes.Clear();
			_nodes.AddRange(keptNodes);
		}

		/// <summary>
		/// Returns the index of the node closest to the given position
		/// via out. Returns false if the graph is empty.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool TryGetNearestNode(Position position, out int index)
		{
			index = -1;

			var nearestDistance = double.MaxValue;

			for (var i = 0; i < _nodes.Count; ++i)
			{
				var distance = _nodes[i].Position.Get2DDistance(position);
				if (distance < nearestDistance)
				{
					nearestDistance = distance;
					index = i;
				}
			}

			return index != -1;
		}

		/// <summary>
		/// Builds a route through the nodes around the given position
		/// via out. Returns false if no usable route was found.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="position"></param>
		/// <param name="route"></param>
		/// <returns></returns>
		public bool TryBuildRoute(Map map, Position position, out PatrolRoute route)
		{
			route = null;

			if (!this.TryGetNearestNode(position, out var startIndex))
				return false;

			var conf = ZoneServer.Instance.Conf.World;

			var collected = this.CollectNodes(startIndex, conf.PatrolRouteRadius, conf.PatrolRouteMaxNodes);
			if (collected.Count < 2)
				return false;

			var ordered = OrderIntoWalk(collected, _nodes[startIndex].Position);
			var positions = new List<Position>();

			foreach (var index in ordered)
			{
				var nodePosition = _nodes[index].Position;

				// A leg the pathfinder can't solve would leave the monster
				// stuck at the node before it.
				if (positions.Count > 0 && !map.Pathfinder.TryFindPath(positions[positions.Count - 1], nodePosition, RouteAgentRadius, out _))
					continue;

				positions.Add(nodePosition);
			}

			if (positions.Count < 2)
				return false;

			route = new PatrolRoute(positions.ToArray());
			return true;
		}

		/// <summary>
		/// Returns the nodes reachable from the given one, within the
		/// radius and up to the given amount.
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="radius"></param>
		/// <param name="maxNodes"></param>
		/// <returns></returns>
		private List<int> CollectNodes(int startIndex, float radius, int maxNodes)
		{
			var collected = new List<int> { startIndex };
			var visited = new HashSet<int> { startIndex };
			var queue = new Queue<int>();
			queue.Enqueue(startIndex);

			var origin = _nodes[startIndex].Position;

			while (queue.Count > 0 && collected.Count < maxNodes)
			{
				var current = queue.Dequeue();

				foreach (var neighbor in _nodes[current].Neighbors)
				{
					if (!visited.Add(neighbor))
						continue;

					if (!_nodes[neighbor].Position.InRange2D(origin, radius))
						continue;

					collected.Add(neighbor);
					queue.Enqueue(neighbor);

					if (collected.Count >= maxNodes)
						break;
				}
			}

			return collected;
		}

		/// <summary>
		/// Orders the given nodes into a walk that always continues with
		/// the closest node it hasn't visited yet.
		/// </summary>
		/// <param name="indices"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		private List<int> OrderIntoWalk(List<int> indices, Position start)
		{
			var remaining = new List<int>(indices);
			var ordered = new List<int>();
			var current = start;

			while (remaining.Count > 0)
			{
				var nearest = 0;
				var nearestDistance = double.MaxValue;

				for (var i = 0; i < remaining.Count; ++i)
				{
					var distance = _nodes[remaining[i]].Position.Get2DDistance(current);
					if (distance < nearestDistance)
					{
						nearestDistance = distance;
						nearest = i;
					}
				}

				current = _nodes[remaining[nearest]].Position;
				ordered.Add(remaining[nearest]);
				remaining.RemoveAt(nearest);
			}

			return ordered;
		}
	}
}
