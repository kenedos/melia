using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using g4;
using Melia.Shared.Data.Database;
using Melia.Shared.World;
using Melia.Zone.World.Maps.NavMesh;
using Yggdrasil.Geometry;
using Yggdrasil.Util;

namespace Melia.Zone.World.Maps
{
	/// <summary>
	/// Represents a map's ground, providing collision, height, and pathing information.
	/// </summary>
	public class Ground
	{
		private const float RayOriginHeight = 30000;

		private GroundData _data;
		private DMesh3 _mesh;
		private DMeshAABBTree3 _spatial;
		private Polygon2d[] _cells;
		private LineF[] _outlines;
		private Polygon2d[] _navGraphPolygons;
		private NavGraphNode[] _graphNodes;

		// Spatial indexing for performance
		private QuadTree<int> _cellQuadTree;
		private QuadTree<int> _outlineQuadTree;
		private Dictionary<Vector2d, HashSet<int>> _vertexToPolygonMap;

		/// <summary>Returns the width of the ground in world units.</summary>
		public int SizeX => _data?.Width ?? 0;

		/// <summary>Returns the depth of the ground in world units.</summary>
		public int SizeZ => _data?.Height ?? 0;

		/// <summary>Returns the left (minimum X) boundary of the ground.</summary>
		public int Left => _data?.Left ?? 0;

		/// <summary>Returns the right (maximum X) boundary of the ground.</summary>
		public int Right => _data?.Right ?? 0;

		/// <summary>Returns the bottom (minimum Z) boundary of the ground.</summary>
		public int Bottom => _data?.Bottom ?? 0;

		/// <summary>Returns the top (maximum Z) boundary of the ground.</summary>
		public int Top => _data?.Top ?? 0;

		/// <summary>Returns the navigation graph nodes built from the ground cells.</summary>
		public NavGraphNode[] GraphNodes => _graphNodes;

		/// <summary>
		/// Returns the raw array of walkable cell polygons.
		/// </summary>
		public Polygon2d[] GetCellPolygons() => _cells;

		/// <summary>
		/// Loads the ground data and builds internal spatial structures.
		/// </summary>
		/// <param name="data">The ground data to load.</param>
		public async Task LoadAsync(GroundData data)
		{
			_data = data;

			if (!this.HasData()) return;

			// Load these in parallel since they're independent
			var meshTask = Task.Run(() => this.LoadGroundMesh());
			var cellsTask = Task.Run(() => this.LoadCells());
			var outlinesTask = meshTask.ContinueWith(_ => this.LoadOutlines());

			await Task.WhenAll(meshTask, cellsTask);
			await outlinesTask;

			// Build spatial indices for performance
			this.BuildSpatialIndices();

			await this.GenerateNavGraphPolygonsAsync();
			await this.BuildNavGraphFromPolygonsAsync();
			if (false)
			{
				// Load or generate the navigation graph polygons from cache.
				if (!await this.TryLoadNavGraphPolygonsAsync())
				{
					await this.GenerateNavGraphPolygonsAsync();
					await this.SaveNavGraphPolygonsAsync();
				}

				// Build the runtime graph representation from the loaded/generated polygons.
				await this.BuildNavGraphFromPolygonsAsync();
			}
		}

		/// <summary>
		/// Synchronous version for backward compatibility
		/// </summary>
		public void Load(GroundData data)
		{
			LoadAsync(data).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Builds spatial indices for faster lookups
		/// </summary>
		private void BuildSpatialIndices()
		{
			if (_cells?.Length > 0)
			{
				// Build QuadTree for cells
				var bounds = this.CalculateBounds(_cells.Where(c => c != null).Select(c => c.Bounds));
				_cellQuadTree = new QuadTree<int>(bounds, maxDepth: 6, maxObjectsPerNode: 10);

				for (var i = 0; i < _cells.Length; i++)
				{
					if (_cells[i] != null)
						_cellQuadTree.Insert(i, _cells[i].Bounds);
				}
			}

			if (_outlines?.Length > 0)
			{
				// Build QuadTree for outlines
				var outlineBounds = _outlines.Select(o => new AxisAlignedBox2d(
					Math.Min(o.Point1.X, o.Point2.X), Math.Min(o.Point1.Y, o.Point2.Y),
					Math.Max(o.Point1.X, o.Point2.X), Math.Max(o.Point1.Y, o.Point2.Y)));
				var bounds = this.CalculateBounds(outlineBounds);
				_outlineQuadTree = new QuadTree<int>(bounds, maxDepth: 6, maxObjectsPerNode: 10);

				for (var i = 0; i < _outlines.Length; i++)
				{
					var lineBounds = new AxisAlignedBox2d(
						Math.Min(_outlines[i].Point1.X, _outlines[i].Point2.X),
						Math.Min(_outlines[i].Point1.Y, _outlines[i].Point2.Y),
						Math.Max(_outlines[i].Point1.X, _outlines[i].Point2.X),
						Math.Max(_outlines[i].Point1.Y, _outlines[i].Point2.Y));
					_outlineQuadTree.Insert(i, lineBounds);
				}
			}
		}

		/// <summary>
		/// Calculate overall bounds from a collection of individual bounds
		/// </summary>
		private AxisAlignedBox2d CalculateBounds(IEnumerable<AxisAlignedBox2d> bounds)
		{
			var first = bounds.FirstOrDefault();
			if (!bounds.Any()) return new AxisAlignedBox2d(0, 0, 1000, 1000); // Default bounds

			double minX = first.Min.x, minY = first.Min.y;
			double maxX = first.Max.x, maxY = first.Max.y;

			foreach (var bound in bounds.Skip(1))
			{
				minX = Math.Min(minX, bound.Min.x);
				minY = Math.Min(minY, bound.Min.y);
				maxX = Math.Max(maxX, bound.Max.x);
				maxY = Math.Max(maxY, bound.Max.y);
			}

			return new AxisAlignedBox2d(minX, minY, maxX, maxY);
		}

		/// <summary>
		/// Builds the in-memory navigation graph nodes and their adjacency from the polygon data.
		/// Uses parallel processing and optimized neighbor detection.
		/// </summary>
		private async Task BuildNavGraphFromPolygonsAsync()
		{
			var nodes = new NavGraphNode[_navGraphPolygons.Length];

			// Initialize nodes in parallel
			await Task.Run(() =>
				Parallel.For(0, _navGraphPolygons.Length, i =>
					nodes[i] = new NavGraphNode(i, _navGraphPolygons[i])
				)
			);

			// Build vertex-to-polygon mapping for faster neighbor detection
			await Task.Run(() => this.BuildVertexToPolygonMapping());

			// Find neighbors using the vertex mapping (much faster than brute force)
			await Task.Run(() =>
			{
				var lockObj = new object();
				Parallel.For(0, nodes.Length, i =>
				{
					var nodeA = nodes[i];
					var neighbors = this.FindNeighborsUsingVertexMap(i, nodeA.Polygon);

					lock (lockObj)
					{
						foreach (var neighborIdx in neighbors)
						{
							if (neighborIdx > i) // Avoid duplicate work
							{
								var nodeB = nodes[neighborIdx];
								nodeA.Neighbors.Add(neighborIdx);
								nodeB.Neighbors.Add(i);
							}
						}
					}
				});
			});

			_graphNodes = nodes;
		}

		/// <summary>
		/// Builds a mapping from vertices to polygon indices for faster neighbor detection
		/// </summary>
		private void BuildVertexToPolygonMapping()
		{
			_vertexToPolygonMap = new Dictionary<Vector2d, HashSet<int>>();

			for (int i = 0; i < _navGraphPolygons.Length; i++)
			{
				var polygon = _navGraphPolygons[i];
				foreach (var vertex in polygon.Vertices)
				{
					if (!_vertexToPolygonMap.ContainsKey(vertex))
						_vertexToPolygonMap[vertex] = new HashSet<int>();
					_vertexToPolygonMap[vertex].Add(i);
				}
			}
		}

		/// <summary>
		/// Finds neighbors using the vertex mapping (O(V) instead of O(N²))
		/// </summary>
		private HashSet<int> FindNeighborsUsingVertexMap(int polygonIndex, Polygon2d polygon)
		{
			var neighbors = new HashSet<int>();

			foreach (var vertex in polygon.Vertices)
			{
				if (_vertexToPolygonMap.TryGetValue(vertex, out var polygonsAtVertex))
				{
					foreach (var neighborIndex in polygonsAtVertex)
					{
						if (neighborIndex != polygonIndex)
							neighbors.Add(neighborIndex);
					}
				}
			}

			return neighbors;
		}

		/// <summary>
		/// Finds the index of the navigation graph polygon that contains the given 2D position.
		/// Uses spatial indexing for better performance.
		/// </summary>
		/// <param name="pos">The position to check.</param>
		/// <returns>The index of the containing polygon, or -1 if not found.</returns>
		public int FindContainingPolygon(Vector2d pos)
		{
			if (_cellQuadTree != null)
			{
				// Use spatial index to narrow down candidates
				var candidates = _cellQuadTree.Query(new AxisAlignedBox2d(pos.x - 1, pos.y - 1, pos.x + 1, pos.y + 1));
				foreach (var candidateIdx in candidates)
				{
					if (_graphNodes[candidateIdx].Polygon.Contains(pos))
						return candidateIdx;
				}
				return -1;
			}

			// Fallback to linear search
			for (var i = 0; i < _graphNodes.Length; i++)
			{
				if (_graphNodes[i].Polygon.Contains(pos))
					return i;
			}
			return -1;
		}

		private bool PolygonsAreNeighbors(Polygon2d a, Polygon2d b)
		{
			var vertsA = new HashSet<Vector2d>(a.Vertices);
			return b.Vertices.Any(vertsA.Contains);
		}

		/// <summary>
		/// Tries to load the navgraph polygon data from a cached file asynchronously.
		/// </summary>
		private async Task<bool> TryLoadNavGraphPolygonsAsync()
		{
			var filePath = this.GetNavGraphFilePath(_data.MapName);
			if (!File.Exists(filePath))
				return false;

			try
			{
				return await Task.Run(() =>
				{
					using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
					ZoneServer.Instance.Data.NavGraphDb.Load(fs);
					if (ZoneServer.Instance.Data.NavGraphDb.TryFind(_data.MapName, out var navData))
					{
						_navGraphPolygons = NavGraphBuilder.ConvertToPolygons(navData);
						return true;
					}
					return false;
				});
			}
			catch (Exception ex)
			{
				// Log or handle read error. Proceeding will cause it to be regenerated.
				return false;
			}
		}

		private string GetNavGraphFilePath(string mapName)
		{
			return Path.Combine("Cache", "NavGraphs", $"{mapName}.navgraph");
		}

		/// <summary>
		/// Generates the navgraph polygons from the ground's cell data asynchronously.
		/// </summary>
		private async Task GenerateNavGraphPolygonsAsync()
		{
			await Task.Run(() =>
			{
				_navGraphPolygons = _cells.Where(c => c != null).ToArray();
			});
		}

		/// <summary>
		/// Saves the current navgraph polygon data to a cache file asynchronously.
		/// </summary>
		private async Task SaveNavGraphPolygonsAsync()
		{
			await Task.Run(() =>
			{
				var navData = NavGraphBuilder.ConvertFromPolygons(_data.MapName, _navGraphPolygons);
				ZoneServer.Instance.Data.NavGraphDb.Entries[_data.MapName] = navData;

				var filePath = this.GetNavGraphFilePath(_data.MapName);
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));

				using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
				ZoneServer.Instance.Data.NavGraphDb.Save(fs);
			});
		}

		/// <summary>
		/// Generates the outline of the ground mesh, for simpler 2D collision checks.
		/// </summary>
		private void LoadOutlines()
		{
			var outlines = new List<LineF>();
			var boundaryFinder = new MeshBoundaryLoops(_mesh);

			foreach (var loop in boundaryFinder.Loops)
			{
				for (var i = 0; i < loop.VertexCount; i++)
				{
					var vert1 = _mesh.GetVertex(loop.Vertices[i]);
					var vert2 = _mesh.GetVertex(loop.Vertices[(i + 1) % loop.VertexCount]);

					var p1 = new Vector2F((float)vert1.x, (float)vert1.z);
					var p2 = new Vector2F((float)vert2.x, (float)vert2.z);

					outlines.Add(new LineF(p1, p2));
				}
			}

			_outlines = outlines.ToArray();
		}

		/// <summary>
		/// Generates a 3D mesh for the ground that can be used for raycasting.
		/// </summary>
		private void LoadGroundMesh()
		{
			var vertices = _data.Vertices.Select(a => new Vector3f(a.X, a.Z, a.Y));
			var triangles = _data.Triangles.Select(a => new Index3i(a.Indices[0], a.Indices[1], a.Indices[2]));

			_mesh = DMesh3Builder.Build<Vector3f, Index3i, Vector3f>(vertices, triangles, null, null);
			_spatial = new DMeshAABBTree3(_mesh, autoBuild: true);
		}

		/// <summary>
		/// Generates 2D polygon representations of walkable cells.
		/// </summary>
		private void LoadCells()
		{
			_cells = new Polygon2d[_data.Cells.Length];

			for (var i = 0; i < _data.Cells.Length; ++i)
			{
				var cellData = _data.Cells[i];
				if (cellData == null) continue;

				var vertices = cellData.Vertices.Select(a => new Vector2d(a.X, a.Y)).ToArray();
				_cells[i] = new Polygon2d(vertices);
			}
		}

		/// <summary>
		/// Returns whether the given 2D position is on a walkable cell.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool IsValidPosition(Position pos)
		{
			return this.TryGetCellIndex(pos, out _);
		}

		/// <summary>
		/// Returns the height of the ground at the given 2D position.
		/// If there's no ground at the position, float.NaN is returned.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public float GetHeightAt(Position pos)
		{
			return this.TryGetHeightAt(pos, out var height) ? height : float.NaN;
		}

		/// <summary>
		/// Returns the height of the ground at the given 2D position via out.
		/// Returns false if there is no ground at the position.
		/// </summary>
		/// <param name="pos">The position to check (X and Z are used).</param>
		/// <param name="height">When this method returns, contains the ground height, or NaN if no ground was found.</param>
		/// <returns>True if ground was found and height was set; otherwise, false.</returns>
		public bool TryGetHeightAt(Position pos, out float height)
		{
			height = float.NaN;
			if (_spatial == null) return false;

			var origin = new Vector3d(pos.X, RayOriginHeight, pos.Z);
			var ray = new Ray3d(origin, Vector3d.AxisY * -1);

			var hitId = _spatial.FindNearestHitTriangle(ray);
			if (hitId == DMesh3.InvalidID) return false;

			var intersection = MeshQueries.TriangleIntersection(_mesh, hitId, ray);
			height = (float)(origin.y - intersection.RayParameter);
			return true;
		}

		/// <summary>
		/// Returns a copy of position, where Y is replaced with the cell
		/// index. If no cell could be found, Y is -1.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public Position GetCellPosition(Position pos)
		{
			this.TryGetCellIndex(pos, out var cellIndex);
			pos.Y = cellIndex;
			return pos;
		}

		/// <summary>
		/// Returns the cell index for the given position via out. Returns
		/// false if no cell exists at the position.
		/// Uses spatial indexing for better performance.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="cellIndex"></param>
		/// <returns></returns>
		public bool TryGetCellIndex(Position pos, out int cellIndex)
		{
			cellIndex = -1;
			if (_cells == null) return false;

			var vecPos = new Vector2d(pos.X, pos.Z);

			if (_cellQuadTree != null)
			{
				// Use spatial index to narrow down candidates
				var candidates = _cellQuadTree.Query(new AxisAlignedBox2d(vecPos.x - 1, vecPos.y - 1, vecPos.x + 1, vecPos.y + 1));
				foreach (var candidateIdx in candidates)
				{
					var cell = _cells[candidateIdx];
					if (cell != null && cell.Contains(vecPos))
					{
						cellIndex = candidateIdx;
						return true;
					}
				}
				return false;
			}

			// Fallback to linear scan
			for (var i = 0; i < _cells.Length; ++i)
			{
				var cell = _cells[i];
				if (cell != null && cell.Contains(vecPos))
				{
					cellIndex = i;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns a random position on the walkable ground via out.
		/// Returns false if no valid position could be found.
		/// </summary>
		/// <returns></returns>
		public bool TryGetRandomPosition(out Position pos)
		{
			pos = Position.Zero;
			if (_cells == null) return false;

			var validCells = _cells.Where(c => c != null).ToArray();
			if (validCells.Length == 0) return false;

			var rnd = RandomProvider.Get();
			var rndCell = validCells[rnd.Next(validCells.Length)];

			var bounds = rndCell.Bounds;
			for (var i = 0; i < 50; ++i)
			{
				var x = rnd.Next((int)bounds.Min.x + 1, (int)bounds.Max.x);
				var z = rnd.Next((int)bounds.Min.y + 1, (int)bounds.Max.y);
				var candidatePos = new Position(x, 0, z);

				if (!rndCell.Contains(new Vector2d(x, z)))
					continue;

				if (!this.TryGetHeightAt(candidatePos, out var height))
					continue;

				pos = candidatePos.WithHeight(height);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the last valid position on the path between origin and
		/// destination. If there are no obstacles, the destination is returned.
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		public Position GetLastValidPosition(Position origin, Position destination)
		{
			var dir = origin.GetDirection(destination);
			var stepSize = 10;
			var currentPos = origin;
			var lastValidPos = currentPos;

			while (currentPos.Get2DDistance(destination) > stepSize)
			{
				currentPos = currentPos.GetRelative(dir, stepSize);

				if (!this.TryGetHeightAt(currentPos, out var height))
					return lastValidPos;

				lastValidPos = currentPos.WithHeight(height);
			}

			return destination;
		}

		/// <summary>
		/// Returns the last valid center position of a circle on the path between the origin and destination centers.
		/// </summary>
		/// <param name="originCenter"></param>
		/// <param name="radius"></param>
		/// <param name="destinationCenter"></param>
		/// <returns></returns>
		public Position GetLastValidCirclePosition(Position originCenter, float radius, Position destinationCenter)
		{
			var dir = originCenter.GetDirection(destinationCenter);
			var stepSize = Math.Max(2.5f, radius * 0.5f);
			var currentPos = originCenter;
			var lastValidPos = currentPos;

			while (currentPos.Get2DDistance(destinationCenter) > stepSize)
			{
				currentPos = currentPos.GetRelative(dir, stepSize);

				if (!this.IsValidCirclePosition(currentPos, radius))
					return lastValidPos;

				if (this.TryGetHeightAt(currentPos, out var height))
				{
					lastValidPos = currentPos.WithHeight(height);
				}
				else
				{
					// This should not happen if IsValidCirclePosition is true, but as a safeguard:
					return lastValidPos;
				}
			}

			// Final check at the destination itself.
			if (this.IsValidCirclePosition(destinationCenter, radius) && this.TryGetHeightAt(destinationCenter, out var destHeight))
			{
				return destinationCenter.WithHeight(destHeight);
			}

			return lastValidPos;
		}

		/// <summary>
		/// Checks if a circle is on valid ground (not on a hole or too-steep slope).
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public bool IsValidCirclePosition(Position center, float radius)
		{
			const float maxTerrainVarianceMultiplier = 1.5f;
			var maxTerrainVariance = radius * maxTerrainVarianceMultiplier;

			if (!this.TryGetHeightAt(center, out var centerHeight)) return false;

			var minHeight = centerHeight;
			var maxHeight = centerHeight;

			var perimeterOffsets = new Vector2f[]
			{
				new(radius, 0), new(-radius, 0), new(0, radius), new(0, -radius),
				new(radius * 0.707f, radius * 0.707f), new(-radius * 0.707f, radius * 0.707f),
				new(radius * 0.707f, -radius * 0.707f), new(-radius * 0.707f, -radius * 0.707f)
			};

			foreach (var offset in perimeterOffsets)
			{
				var perimeterPos = new Position(center.X + offset.x, 0, center.Z + offset.y);
				if (!this.TryGetHeightAt(perimeterPos, out var height)) return false;

				minHeight = Math.Min(minHeight, height);
				maxHeight = Math.Max(maxHeight, height);
			}

			return (maxHeight - minHeight) <= maxTerrainVariance;
		}

		/// <summary>
		/// Returns true if a line between two positions intersects with the ground's outer boundary.
		/// Uses spatial indexing for better performance.
		/// </summary>
		/// <param name="pos1"></param>
		/// <param name="pos2"></param>
		/// <returns></returns>
		public bool AnyObstacles(Position pos1, Position pos2)
		{
			if (_outlines == null) return !this.IsValidPosition(pos2);
			if (!this.IsValidPosition(pos1) || !this.IsValidPosition(pos2)) return true;

			var pathLine = new LineF(new Vector2F(pos1.X, pos1.Z), new Vector2F(pos2.X, pos2.Z));

			if (_outlineQuadTree != null)
			{
				// Use spatial index to narrow down candidates
				var lineBounds = new AxisAlignedBox2d(
					Math.Min(pos1.X, pos2.X), Math.Min(pos1.Z, pos2.Z),
					Math.Max(pos1.X, pos2.X), Math.Max(pos1.Z, pos2.Z));
				var candidates = _outlineQuadTree.Query(lineBounds);

				foreach (var candidateIdx in candidates)
				{
					if (pathLine.Intersects(_outlines[candidateIdx], out _))
						return true;
				}
				return false;
			}

			// Fallback to linear scan
			foreach (var outline in _outlines)
			{
				if (pathLine.Intersects(outline, out _))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns true if the ground has been loaded with data.
		/// </summary>
		public bool HasData()
		{
			return _data != null && _data.Vertices.Length > 0;
		}

		/// <summary>
		/// Attempts to find the nearest valid position to a given point.
		/// </summary>
		/// <param name="pos">Input position (X, Z used)</param>
		/// <param name="nearestPos">Nearest valid position with height.</param>
		/// <param name="maxDistance">Maximum search radius.</param>
		/// <returns>True if a valid position is found, false otherwise</returns>
		public bool TryGetNearestValidPosition(Position pos, out Position nearestPos, float maxDistance = 100f)
		{
			const float defaultRadius = 5f; // A small default radius suitable for NPCs.
			return this.TryGetNearestValidPosition(pos, defaultRadius, out nearestPos, maxDistance);
		}

		/// <summary>
		/// Attempts to find the nearest valid position that can accommodate an entity of a given radius.
		/// </summary>
		/// <param name="pos">Input position (X, Z used)</param>
		/// <param name="radius">The radius of the entity to check walkability for.</param>
		/// <param name="nearestPos">Nearest valid position with height.</param>
		/// <param name="maxDistance">Maximum search radius.</param>
		/// <returns>True if a valid position is found, false otherwise.</returns>
		public bool TryGetNearestValidPosition(Position pos, float radius, out Position nearestPos, float maxDistance = 100f)
		{
			nearestPos = pos;
			// Check original position first
			if (this.IsValidCirclePosition(pos, radius) && this.TryGetHeightAt(pos, out var height))
			{
				nearestPos = pos.WithHeight(height);
				return true;
			}

			// Spiral search for a nearby valid point
			const int pointsPerRing = 12;
			const float step = 10f;
			for (var r = step; r <= maxDistance; r += step)
			{
				for (var i = 0; i < pointsPerRing; i++)
				{
					var angle = 2 * Math.PI * i / pointsPerRing;
					var candidate = new Position(
						(float)(pos.X + r * Math.Cos(angle)), 0, (float)(pos.Z + r * Math.Sin(angle))
					);

					if (this.IsValidCirclePosition(candidate, radius) && this.TryGetHeightAt(candidate, out var candidateHeight))
					{
						nearestPos = candidate.WithHeight(candidateHeight);
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Returns if the Line of Sight is clear between two positions by performing a raycast against the ground mesh.
		/// </summary>
		/// <param name="pos1">The starting position.</param>
		/// <param name="pos2">The ending position.</param>
		/// <param name="eyeHeight">The vertical offset from the ground for the start and end points of the ray.</param>
		/// <returns>True if there is a clear line of sight, false otherwise.</returns>
		public bool LineOfSight(Position pos1, Position pos2, float eyeHeight = 10f)
		{
			if (_spatial == null)
			{
				// Fallback to 2D check if 3D spatial data isn't available.
				return !this.AnyObstacles(pos1, pos2);
			}

			if (!this.TryGetHeightAt(pos1, out var h1) || !this.TryGetHeightAt(pos2, out var h2))
			{
				// Cannot determine height at one of the positions, assume no line of sight.
				return false;
			}

			var startVec = new Vector3d(pos1.X, h1 + eyeHeight, pos1.Z);
			var endVec = new Vector3d(pos2.X, h2 + eyeHeight, pos2.Z);
			var direction = endVec - startVec;
			var totalDistance = direction.Length;

			if (totalDistance < 1.0f) return true; // Positions are too close to be obstructed.

			var ray = new Ray3d(startVec, direction.Normalized);

			var hitId = _spatial.FindNearestHitTriangle(ray);
			if (hitId == DMesh3.InvalidID)
			{
				// Ray didn't hit any part of the mesh, so sight is clear.
				return true;
			}

			var intersection = MeshQueries.TriangleIntersection(_mesh, hitId, ray);

			// If the first thing the ray hits is farther away than the target, the line of sight is clear.
			return intersection.RayParameter >= totalDistance;
		}
	}

	/// <summary>
	/// Simple QuadTree implementation for spatial indexing
	/// </summary>
	public class QuadTree<T>
	{
		private readonly AxisAlignedBox2d _bounds;
		private readonly int _maxDepth;
		private readonly int _maxObjectsPerNode;
		private readonly List<(T item, AxisAlignedBox2d bounds)> _objects;
		private readonly QuadTree<T>[] _children;
		private readonly int _depth;

		public QuadTree(AxisAlignedBox2d bounds, int maxDepth = 5, int maxObjectsPerNode = 10, int depth = 0)
		{
			_bounds = bounds;
			_maxDepth = maxDepth;
			_maxObjectsPerNode = maxObjectsPerNode;
			_objects = new List<(T, AxisAlignedBox2d)>();
			_children = new QuadTree<T>[4];
			_depth = depth;
		}

		/// <summary>
		/// Inserts an item with its bounding box into the quad tree.
		/// </summary>
		public void Insert(T item, AxisAlignedBox2d itemBounds)
		{
			if (!_bounds.Intersects(itemBounds))
				return;

			if (_objects.Count < _maxObjectsPerNode || _depth >= _maxDepth)
			{
				_objects.Add((item, itemBounds));
				return;
			}

			if (_children[0] == null)
				this.Subdivide();

			foreach (var child in _children)
				child.Insert(item, itemBounds);
		}

		/// <summary>
		/// Returns all items whose bounding boxes intersect the given query bounds.
		/// </summary>
		public List<T> Query(AxisAlignedBox2d queryBounds)
		{
			var result = new List<T>();
			Query(queryBounds, result);
			return result;
		}

		private void Query(AxisAlignedBox2d queryBounds, List<T> result)
		{
			if (!_bounds.Intersects(queryBounds))
				return;

			foreach (var (item, bounds) in _objects)
			{
				if (bounds.Intersects(queryBounds))
					result.Add(item);
			}

			if (_children[0] != null)
			{
				foreach (var child in _children)
					child.Query(queryBounds, result);
			}
		}

		private void Subdivide()
		{
			var halfWidth = (_bounds.Max.x - _bounds.Min.x) / 2;
			var halfHeight = (_bounds.Max.y - _bounds.Min.y) / 2;
			var centerX = _bounds.Min.x + halfWidth;
			var centerY = _bounds.Min.y + halfHeight;

			_children[0] = new QuadTree<T>(new AxisAlignedBox2d(_bounds.Min.x, _bounds.Min.y, centerX, centerY), _maxDepth, _maxObjectsPerNode, _depth + 1);
			_children[1] = new QuadTree<T>(new AxisAlignedBox2d(centerX, _bounds.Min.y, _bounds.Max.x, centerY), _maxDepth, _maxObjectsPerNode, _depth + 1);
			_children[2] = new QuadTree<T>(new AxisAlignedBox2d(_bounds.Min.x, centerY, centerX, _bounds.Max.y), _maxDepth, _maxObjectsPerNode, _depth + 1);
			_children[3] = new QuadTree<T>(new AxisAlignedBox2d(centerX, centerY, _bounds.Max.x, _bounds.Max.y), _maxDepth, _maxObjectsPerNode, _depth + 1);
		}
	}
}
